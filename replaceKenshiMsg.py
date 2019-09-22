
import sys, os
import struct
import binascii
import shutil
import re

BASE_PATH = 'F:/SteamLibrary/steamapps/common/Kenshi/mods/'
BASE_PO_PATH = 'F:/SteamLibrary/steamapps/common/Kenshi/locale/zh_CN/gamedata.po'

class ReplaceKenshi(object):

    def __init__(self, mod_name):
        self.current_mod = mod_name
        self.content = b""
        self.current_ids_list = []
        self.replace_dict = self.get_msg_string()
        self.start_offset = 0

    def read_file(self):
        content = None
        current_file_path = os.path.join(BASE_PATH, "%s/%s.mod" % (self.current_mod, self.current_mod))
        bak_file_path = os.path.join(BASE_PATH, "%s/%s.mod.bak" % (self.current_mod, self.current_mod))
        with open(file=current_file_path,mode='rb') as f:
            content = f.read()
            f.close()
        shutil.copyfile(current_file_path, bak_file_path)
        return bytearray(content)
    
    def get_msg_string(self):
        """
        得到需要替换的字符串
        #: 2933-Walls2.mod
        msgid "Defensive Gate level 2"
        msgstr "防御大门 2级水平"
        """
        msg_dict = {}
        msg_id_list = []
        with open(file=BASE_PO_PATH,mode='rb') as f:
            for line in  f.readlines():
                line = line.decode("utf-8")
                if line.startswith("#:"):
                    msg_id = line.split("#:")[1].strip()
                    if not msg_id or len(msg_id) <1:
                        msg_id_list = []
                    else:
                        for m in msg_id.split(" "):
                            m = m.replace("?", " ")
                            if m[-2] != ":" and m[-3] != ":":
                                msg_id_list.append(m)
                elif line.startswith("msgstr"):
                    if msg_id_list and len(msg_id_list) > 0:
                        for m in msg_id_list:
                            msg_dict[m] = line.split("msgstr")[1].strip().replace('"', "")
                        msg_id_list = []
            f.close()
        return msg_dict

    def add_content(self, string_id, trans_string):
        print(string_id, trans_string)
        start_content = bytearray(b'\x00' * 12)
        end_content = bytearray(b'\x03\x00\x00\x80' + b'\x00' * 36)
        string_id_count = self.int_to_byte(len(string_id))
        trans_string_count = self.int_to_byte(len(trans_string))
        self.content += start_content
        self.content += trans_string_count
        self.content += bytearray(trans_string)
        self.content += string_id_count
        self.content += bytearray(string_id)
        self.content += end_content
        self.content[self.start_offset+21:self.start_offset+25] = self.int_to_byte(int.from_bytes(self.content[self.start_offset+21:self.start_offset+25], byteorder='little', signed=True)+1)

    def int_to_byte(self, num):
        return (num).to_bytes(length=4, byteorder='little')

    def write_file(self):
        current_file_path = os.path.join(BASE_PATH, "%s/%s.mod" % (self.current_mod, self.current_mod))
        with open(file=current_file_path,mode='wb') as f:
            #for content in content_list:
            #    f.writelines([content, b"\r"])
            f.write(self.content)
            f.close()

    def do_replace(self):
        total_times = 0
        self.content = self.read_file()
        self.start_offset = [i.start() for i in re.finditer(b'gamedata.base', self.content)][0]
        for k, v in self.replace_dict.items():
            self.add_content(k.encode("utf-8"), v.encode("utf-8"))
        self.write_file()



def main():
    print('参数个数为:', len(sys.argv), '个参数。')
    print('参数列表:', str(sys.argv))
    print('脚本名为：', sys.argv[0])
    for i in range(1, len(sys.argv)):
        print('参数 %s 为：%s' % (i, sys.argv[i]))

    mod_name = "chs_fcs"
    replaceInstance = ReplaceKenshi(mod_name)
    replaceInstance.do_replace()


if __name__ == "__main__":
    main()
