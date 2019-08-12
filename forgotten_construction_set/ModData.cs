using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace forgotten_construction_set
{
	public class ModData
	{
		public ulong id;

		public string mod;

		public string title;

		public List<string> tags;

		public int visibility;

		public DateTime? lastUpdate;

		[XmlIgnore]
		public GameData.Header header;

		[XmlIgnore]
		public string changeNotes = string.Empty;

		private string dir;

		public ModData()
		{
		}

		public ModData(GameData.Header header, string rootDir, string modName)
		{
			this.id = (ulong)0;
			this.visibility = 0;
			this.tags = new List<string>();
			this.mod = modName;
			this.dir = rootDir;
			this.header = header;
		}

		public string GetAbsoluteFolder()
		{
			return Path.GetFullPath(this.dir);
		}

		public string GetAbsoluteImagePath()
		{
			return Path.GetFullPath(this.GetImagePath());
		}

		private string GetFileName()
		{
			return string.Concat(this.dir, "_", this.mod, ".info");
		}

		public string GetFolder()
		{
			return this.dir;
		}

		public ulong GetFolderSize()
		{
			ulong length = (ulong)0;
			FileInfo[] files = (new DirectoryInfo(this.GetAbsoluteFolder())).GetFiles("*.*", SearchOption.AllDirectories);
			for (int i = 0; i < (int)files.Length; i++)
			{
				length += (ulong) files[i].Length;
			}
			return length;
		}

		public string GetImagePath()
		{
			return string.Concat(this.dir, "_", this.mod, ".img");
		}

		public void Load()
		{
			string fileName = this.GetFileName();
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return;
			}
			if ((new FileInfo(fileName)).Exists)
			{
				ModData modDatum = null;
				try
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModData));
					using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
					{
						object obj = xmlSerializer.Deserialize(fileStream);
						if (obj != null)
						{
							modDatum = obj as ModData;
						}
					}
				}
				catch (Exception exception)
				{
					modDatum = null;
				}
				if (modDatum != null)
				{
					this.title = modDatum.title;
					this.visibility = modDatum.visibility;
					this.tags = modDatum.tags;
					this.lastUpdate = modDatum.lastUpdate;
					this.id = modDatum.id;
				}
			}
		}

		public void Save()
		{
			string fileName = this.GetFileName();
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return;
			}
			FileInfo fileInfo = new FileInfo(fileName);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ModData));
			using (FileStream fileStream = new FileStream(fileName, (fileInfo.Exists ? FileMode.Truncate : FileMode.Create)))
			{
				xmlSerializer.Serialize(fileStream, this);
			}
		}
	}
}