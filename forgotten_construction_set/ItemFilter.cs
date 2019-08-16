using System;
using System.Collections.Generic;

namespace forgotten_construction_set
{
    public class ItemFilter
    {
        private itemType type = itemType.NULL_ITEM;

        private string name = "";

        private List<ItemFilter.PropertyFilter> property = new List<ItemFilter.PropertyFilter>();

        public ItemFilter(itemType type, string filter)
        {
            this.type = type;
            if (filter != "")
            {
                this.parseFilterString(filter);
            }
        }

        public ItemFilter(string filter)
        {
            if (filter != "")
            {
                this.parseFilterString(filter);
            }
        }

        private bool compareValue(object lvalue, object rvalue, ItemFilter.PropertyFilter.Mode comparison)
        {
            float single;
            switch (comparison)
            {
                case ItemFilter.PropertyFilter.Mode.EQUAL:
                    {
                        if (lvalue.ToString().ToLower().Equals(rvalue.ToString().ToLower()))
                        {
                            break;
                        }
                        return false;
                    }
                case ItemFilter.PropertyFilter.Mode.CONTAINS:
                    {
                        if (lvalue.ToString().ToLower().Contains(rvalue.ToString().ToLower()))
                        {
                            break;
                        }
                        return false;
                    }
                case ItemFilter.PropertyFilter.Mode.NOT:
                    {
                        if (!lvalue.ToString().ToLower().Equals(rvalue.ToString().ToLower()))
                        {
                            break;
                        }
                        return false;
                    }
                case ItemFilter.PropertyFilter.Mode.GREATER:
                    {
                        if (!this.numberValue(lvalue, out single) || single > (float)rvalue)
                        {
                            break;
                        }
                        return false;
                    }
                case ItemFilter.PropertyFilter.Mode.LESS:
                    {
                        if (!this.numberValue(lvalue, out single) || single < (float)rvalue)
                        {
                            break;
                        }
                        return false;
                    }
                case ItemFilter.PropertyFilter.Mode.GEQUAL:
                    {
                        if (!this.numberValue(lvalue, out single) || single >= (float)rvalue)
                        {
                            break;
                        }
                        return false;
                    }
                case ItemFilter.PropertyFilter.Mode.LEQUAL:
                    {
                        if (!this.numberValue(lvalue, out single) || single <= (float)rvalue)
                        {
                            break;
                        }
                        return false;
                    }
            }
            return true;
        }

        private bool numberValue(object o, out float v)
        {
            if (!(o is int))
            {
                if (!(o is float))
                {
                    v = 0f;
                    return false;
                }
                v = (float)o;
            }
            else
            {
                v = (float)((int)o);
            }
            return true;
        }

        private void parseFilterString(string filter)
        {
            float single;
            string[] strArrays = new string[] { "==", ">=", "<=", "!=", "!", "=", ":", ">", "<" };
            int[] numArray = new int[] { 0, 5, 6, 2, 2, 0, 1, 3, 4 };
            string[] strArrays1 = filter.Split(new char[] { ';' });
            for (int i = 0; i < (int)strArrays1.Length; i++)
            {
                string str = strArrays1[i];
                if (str.Trim() != "")
                {
                    string[] strArrays2 = str.Split(strArrays, 2, StringSplitOptions.RemoveEmptyEntries);
                    int num = -1;
                    if ((int)strArrays2.Length > 1)
                    {
                        string str1 = str.Substring(strArrays2[0].Length, 2);
                        int num1 = 0;
                        while (num1 < (int)strArrays.Length)
                        {
                            if (!str1.StartsWith(strArrays[num1]))
                            {
                                num1++;
                            }
                            else
                            {
                                num = numArray[num1];
                                break;
                            }
                        }
                    }
                    if (num < 0)
                    {
                        this.name = str.Trim().ToLower();
                    }
                    else if ((int)strArrays2.Length >= 2)
                    {
                        ItemFilter.PropertyFilter propertyFilter = new ItemFilter.PropertyFilter()
                        {
                            mode = (ItemFilter.PropertyFilter.Mode)num,
                            property = strArrays2[0].Trim()
                        };
                        string lower = strArrays2[1].Trim().ToLower();
                        if (num >= 3)
                        {
                            if (!float.TryParse(lower, out single))
                            {
                                //  goto Label0;
                                if (single != 0)
                                {

                                }
                            }
                            propertyFilter.@value = single;
                        }
                        else
                        {
                            propertyFilter.@value = lower;
                        }
                        this.property.Add(propertyFilter);
                    }
                }
           // Label0:
            }
        }

        public bool Test(GameData.Item item)
        {
            object bah;
            bool flag;
            if (this.type != itemType.NULL_ITEM && item.type != this.type)
            {
                return false;
            }
            if (this.name != "" && !item.Name.ToLower().Contains(this.name) && !item.stringID.ToLower().Contains(this.name))
            {
                return false;
            }
            List<ItemFilter.PropertyFilter>.Enumerator enumerator = this.property.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ItemFilter.PropertyFilter current = enumerator.Current;
                    if (current.property == "Type")
                    {
                        bah = item.type;
                    }
                    else if (current.property == "StringID")
                    {
                        bah = item.stringID;
                    }
                    else if (item.ContainsKey(string.Concat(current.property, "0")))
                    {
                        bool flag1 = false;
                        int num = 0;
                        while (!flag1)
                        {
                            string str = string.Concat(current.property, num);
                            if (!item.ContainsKey(str))
                            {
                                break;
                            }
                            flag1 |= this.compareValue(item[str], current.@value, current.mode);
                            num++;
                        }
                        if (flag1)
                        {
                            continue;
                        }
                        flag = false;
                        return flag;
                    }
                    else if (item.ContainsKey(current.property))
                    {
                        bah = item[current.property];
                        if (bah is int && current.mode <= ItemFilter.PropertyFilter.Mode.NOT)
                        {
                            GameData.Desc desc = GameData.getDesc(item.type, current.property);
                            if (desc != GameData.nullDesc)
                            {
                                Type type = desc.defaultValue.GetType();
                                if (type.IsEnum)
                                {
                                    bah = Enum.GetName(type, bah);
                                }
                                if (bah == null)
                                {
                                    flag = false;
                                    return flag;
                                }
                            }
                        }
                    }
                    else if (!item.hasReference(current.property))
                    {
                        GameData.Desc desc1 = GameData.getDesc(item.type, current.property);
                        if (desc1 == null || desc1.list == itemType.NULL_ITEM)
                        {
                            flag = false;
                            return flag;
                        }
                        else
                        {
                            bah = 0;
                        }
                    }
                    else
                    {
                        bah = item.getReferenceCount(current.property);
                    }
                    if (this.compareValue(bah, current.@value, current.mode))
                    {
                        continue;
                    }
                    flag = false;
                    return flag;
                }
                return true;
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();

            }

            return flag;

        }

        public override string ToString()
        {
            string str = this.name;
            foreach (ItemFilter.PropertyFilter propertyFilter in this.property)
            {
                if (str != "")
                {
                    str = string.Concat(str, "; ");
                }
                str = string.Concat(str, propertyFilter.property);
                switch (propertyFilter.mode)
                {
                    case ItemFilter.PropertyFilter.Mode.EQUAL:
                        {
                            str = string.Concat(str, " = ");
                            break;
                        }
                    case ItemFilter.PropertyFilter.Mode.CONTAINS:
                        {
                            str = string.Concat(str, " : ");
                            break;
                        }
                    case ItemFilter.PropertyFilter.Mode.NOT:
                        {
                            str = string.Concat(str, " != ");
                            break;
                        }
                    case ItemFilter.PropertyFilter.Mode.GREATER:
                        {
                            str = string.Concat(str, " > ");
                            break;
                        }
                    case ItemFilter.PropertyFilter.Mode.LESS:
                        {
                            str = string.Concat(str, " < ");
                            break;
                        }
                    case ItemFilter.PropertyFilter.Mode.GEQUAL:
                        {
                            str = string.Concat(str, " >= ");
                            break;
                        }
                    case ItemFilter.PropertyFilter.Mode.LEQUAL:
                        {
                            str = string.Concat(str, " <= ");
                            break;
                        }
                }
                str = string.Concat(str, propertyFilter.@value.ToString());
            }
            return str;
        }

        private struct PropertyFilter
        {
            public ItemFilter.PropertyFilter.Mode mode;

            public string property;

            public object @value;

            public enum Mode
            {
                EQUAL,
                CONTAINS,
                NOT,
                GREATER,
                LESS,
                GEQUAL,
                LEQUAL
            }
        }
    }
}