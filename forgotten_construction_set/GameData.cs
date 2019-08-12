using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class GameData
	{
		public string activeFileName;

		public Dictionary<string, GameData.Item> items = new Dictionary<string, GameData.Item>(36864);

		public GameData.Header header = new GameData.Header();

		public static SortedList<itemType, SortedList<string, GameData.Desc>> desc;

		public static GameData.Desc nullDesc;

		private int lastID;

		private static byte[] StrByteBuffer;

		public bool SingleFileMode
		{
			get
			{
				bool fileMode;
				IEnumerator enumerator = Application.OpenForms.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Form current = (Form)enumerator.Current;
						if (!(current is baseForm))
						{
							continue;
						}
						fileMode = (current as baseForm).nav.FileMode == navigation.ModFileMode.SINGLE;
						return fileMode;
					}
					return false;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				//return fileMode;
			}
		}

		static GameData()
		{
			GameData.StrByteBuffer = new byte[4096];
		}

		public GameData()
		{
		}

		public static bool addCondition(itemType type, string name, string key, object values, bool match = true)
		{
			GameData.DescCondition descCondition = new GameData.DescCondition()
			{
				key = key,
				match = match,
				values = values
			};
			GameData.Desc desc = GameData.getDesc(type, name);
			if (desc == GameData.nullDesc)
			{
				return false;
			}
			desc.condition = descCondition;
			return true;
		}

		public void changeID(GameData.Item item, string id)
		{
			if (id == item.stringID)
			{
				return;
			}
			this.getReferences(item, new ArrayList());
			this.items.Remove(item.stringID);
			item.stringID = id;
			this.items.Add(id, item);
		}

		public void clear()
		{
			this.items.Clear();
		}

		public int clearReferencesTo(GameData.Item item)
		{
			int count = 0;
			List<string> strs = new List<string>();
			foreach (GameData.Item value in this.items.Values)
			{
				if (value.getState() == GameData.State.REMOVED)
				{
					continue;
				}
				foreach (string str in value.referenceLists())
				{
					foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in value.referenceData(str, false))
					{
						if (keyValuePair.Key != item.stringID)
						{
							continue;
						}
						strs.Add(str);
					}
				}
				foreach (string str1 in strs)
				{
					value.removeReference(str1, item.stringID);
				}
				count += strs.Count;
				strs.Clear();
				foreach (KeyValuePair<string, GameData.Instance> keyValuePair1 in value.instanceData())
				{
					if (keyValuePair1.Value.sdata["ref"] != item.stringID)
					{
						keyValuePair1.Value.removeReference("states", item);
					}
					else
					{
						strs.Add(keyValuePair1.Key);
					}
				}
				foreach (string str2 in strs)
				{
					value.removeInstance(str2);
				}
				count += strs.Count;
				strs.Clear();
			}
			return count;
		}

		public GameData.Item cloneItem(GameData.Item other)
		{
			GameData.Item value = this.createItem(other.type);
			value.Name = string.Concat(other.Name, " copy");
			foreach (KeyValuePair<string, object> keyValuePair in other)
			{
				value[keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (string str in other.referenceLists())
			{
				foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair1 in other.referenceData(str, false))
				{
					int? nullable = null;
					int? nullable1 = nullable;
					nullable = null;
					int? nullable2 = nullable;
					nullable = null;
					value.addReference(str, keyValuePair1.Key, nullable1, nullable2, nullable);
					value.setReferenceValue(str, keyValuePair1.Key, keyValuePair1.Value);
				}
			}
			value.resolveReferences(this);
			foreach (KeyValuePair<string, GameData.Instance> keyValuePair2 in other.instanceData())
			{
				value.addInstance(keyValuePair2.Key, keyValuePair2.Value);
			}
			return value;
		}

		private string createID()
		{
			string str;
			do
			{
				int num = this.lastID + 1;
				this.lastID = num;
				str = string.Concat(num, "-", this.activeFileName);
			}
			while (this.items.ContainsKey(str));
			return str;
		}

		public GameData.Item createItem(itemType type)
		{
			GameData.Item item = new GameData.Item(type, this.createID());
			item.setMissingValues();
			this.items.Add(item.stringID, item);
			item.Name = type.ToString();
			return item;
		}

		public void deleteItem(GameData.Item item)
		{
			this.clearReferencesTo(item);
			if (item.getState() == GameData.State.OWNED)
			{
				this.items.Remove(item.stringID);
				return;
			}
			if (item.getState() != GameData.State.REMOVED || !this.SingleFileMode)
			{
				item.flagDeleted();
				return;
			}
			this.items.Remove(item.stringID);
		}

		public static GameData.Desc getDesc(itemType type, string name)
		{
			if (!GameData.desc.ContainsKey(type) || !GameData.desc[type].ContainsKey(name))
			{
				return GameData.nullDesc;
			}
			return GameData.desc[type][name];
		}

		public GameData.Item getItem(string id)
		{
			GameData.Item item = null;
			if (this.items.TryGetValue(id, out item))
			{
				return item;
			}
			return null;
		}

		public GameData.Item getOrCreateItem(GameData.Item i)
		{
			GameData.Item item = null;
			if (this.items.TryGetValue(i.stringID, out item))
			{
				return item;
			}
			item = new GameData.Item(i.type, i.stringID)
			{
				Name = i.Name
			};
			this.items.Add(item.stringID, item);
			return item;
		}

		public List<string> getReferencedMods()
		{
			List<string> strs = new List<string>();
			foreach (GameData.Item value in this.items.Values)
			{
				if (value.getState() != GameData.State.MODIFIED && value.getState() != GameData.State.OWNED)
				{
					continue;
				}
				foreach (string str in value.referenceLists())
				{
					foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in value.referenceData(str, false))
					{
						GameData.State state = value.getState(str, keyValuePair.Key);
						if (state != GameData.State.MODIFIED && state != GameData.State.OWNED)
						{
							continue;
						}
						GameData.Item item = this.getItem(keyValuePair.Key);
						if (item == null || item.Mod == null || strs.Contains(item.Mod))
						{
							continue;
						}
						strs.Add(item.Mod);
					}
				}
				foreach (KeyValuePair<string, GameData.Instance> keyValuePair1 in value.instanceData())
				{
					if (keyValuePair1.Value.getState() != GameData.State.MODIFIED && keyValuePair1.Value.getState() != GameData.State.OWNED)
					{
						continue;
					}
					GameData.Item item1 = this.getItem(keyValuePair1.Value.sdata["ref"]);
					if (item1 == null || item1.Mod == null || strs.Contains(item1.Mod))
					{
						continue;
					}
					strs.Add(item1.Mod);
				}
			}
			strs.Remove(this.activeFileName);
			strs.Remove("");
			return strs;
		}

		public int getReferences(GameData.Item item, ArrayList list = null)
		{
			int num = 0;
			foreach (GameData.Item value in this.items.Values)
			{
				bool key = false;
				foreach (string str in value.referenceLists())
				{
					foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in value.referenceData(str, false))
					{
						if (keyValuePair.Key != item.stringID)
						{
							continue;
						}
						key = true;
					}
				}
				foreach (KeyValuePair<string, GameData.Instance> keyValuePair1 in value.instanceData())
				{
					if (keyValuePair1.Value.getState() == GameData.State.REMOVED || keyValuePair1.Value.getState() == GameData.State.LOCKED_REMOVED)
					{
						continue;
					}
					if (keyValuePair1.Value.sdata["ref"] != item.stringID)
					{
						foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair2 in keyValuePair1.Value.referenceData("states", false))
						{
							key = key | (keyValuePair2.Key == item.stringID);
						}
					}
					else
					{
						key = true;
					}
				}
				if (!key)
				{
					continue;
				}
				if (list != null)
				{
					list.Add(value);
				}
				num++;
			}
			return num;
		}

		public List<string> getRequiredMods()
		{
			List<string> strs = new List<string>();
			foreach (GameData.Item value in this.items.Values)
			{
				if (value.getState() != GameData.State.MODIFIED || strs.Contains(value.Mod))
				{
					continue;
				}
				strs.Add(value.Mod);
			}
			return strs;
		}

		public static void initialise()
		{
			GameData.nullDesc = new GameData.Desc();
			GameData.desc = new SortedList<itemType, SortedList<string, GameData.Desc>>();
			Enum.GetValues(typeof(itemType));
		}

		public static bool isListType(GameData.Desc d)
		{
			if (d.defaultValue is GameData.TripleInt)
			{
				return true;
			}
			return d.defaultValue is GameData.Instance;
		}

		public bool load(string filename, GameData.ModMode mode, bool skipMissing = false)
		{
			bool flag;
			if (!System.IO.File.Exists(filename))
			{
				return false;
			}
			string fileName = Path.GetFileName(filename);
			if (mode == GameData.ModMode.ACTIVE)
			{
				this.activeFileName = fileName;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					using (Stream stream = System.IO.File.OpenRead(filename))
					{
						stream.CopyTo(memoryStream);
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					MessageBox.Show(string.Concat("Corrupt mod file: ", fileName), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					baseForm.logger.Error(exception, "Error reading mod file '{0}'.", new object[] { fileName });
				}
				memoryStream.Position = (long)0;
				using (BinaryReader binaryReader = new BinaryReader(memoryStream))
				{
					try
					{
						int num = binaryReader.ReadInt32();
						if (num < 1 || num > 16)
						{
							throw new EndOfStreamException("Invalid mod Version");
						}
						if (num > 15)
						{
							GameData.Header header = GameData.readHeader(binaryReader);
							if (mode == GameData.ModMode.ACTIVE)
							{
								this.header = header;
							}
						}
						bool singleFileMode = this.SingleFileMode;
						int num1 = binaryReader.ReadInt32();
						this.lastID = Math.Max(this.lastID, num1);
						int num2 = binaryReader.ReadInt32();
						for (int i = 0; i < num2; i++)
						{
							binaryReader.ReadInt32();
							itemType _itemType = (itemType)binaryReader.ReadInt32();
							int num3 = binaryReader.ReadInt32();
							string str = GameData.readString(binaryReader);
							string str1 = (num >= 7 ? GameData.readString(binaryReader) : string.Concat(num3.ToString(), "-", fileName));
							GameData.Item item = this.getItem(str1);
							bool flag1 = item == null;
							if (item == null)
							{
								item = new GameData.Item(_itemType, str1);
								this.items.Add(str1, item);
							}
							if (item.type != _itemType)
							{
								itemType _itemType1 = item.type;
								Errors.addError(Error.WARNING, item, filename, string.Concat("Item has changed type: ", _itemType1.ToString(), " -> ", _itemType.ToString()));
							}
							bool flag2 = item.load(binaryReader, str, mode, num, fileName, flag1);
							if (item.getState() == GameData.State.REMOVED)
							{
								item.refreshState();
								if (mode == GameData.ModMode.BASE || item.getState() == GameData.State.OWNED && !singleFileMode)
								{
									this.items.Remove(item.stringID);
								}
								else
								{
									item.flagDeleted();
								}
							}
							if (!flag2 & skipMissing)
							{
								this.items.Remove(item.stringID);
							}
						}
						return true;
					}
					catch (Exception exception3)
					{
						Exception exception2 = exception3;
						MessageBox.Show(string.Concat("Corrupt mod file: ", fileName), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						baseForm.logger.Error(exception2, "Error processing mod file '{0}'.", new object[] { fileName });
						flag = false;
					}
				}
			}
			return flag;
		}

		public static GameData.Header loadHeader(string filename)
		{
			GameData.Header header;
			GameData.Header header1 = null;
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				try
				{
					if (binaryReader.ReadInt32() > 15)
					{
						header1 = GameData.readHeader(binaryReader);
					}
				}
				catch (EndOfStreamException endOfStreamException)
				{
				}
				binaryReader.Close();
				fileStream.Close();
				return header1;
			}
			catch (FileNotFoundException fileNotFoundException)
			{
				header = null;
			}
			return header;
		}

		public static GameData.Header readHeader(BinaryReader file)
		{
			GameData.Header header = new GameData.Header()
			{
				Version = file.ReadInt32(),
				Author = GameData.readString(file),
				Description = GameData.readString(file),
				Dependencies = new List<string>(GameData.readString(file).Split(new char[] { ',' })),
				Referenced = new List<string>(GameData.readString(file).Split(new char[] { ',' }))
			};
			if (header.Dependencies.Count == 1 && header.Dependencies[0] == "")
			{
				header.Dependencies.Clear();
			}
			if (header.Referenced.Count == 1 && header.Referenced[0] == "")
			{
				header.Referenced.Clear();
			}
			return header;
		}

		private static string readString(BinaryReader file)
		{
			int num = file.ReadInt32();
			if (num <= 0)
			{
				return string.Empty;
			}
			if (num > (int)GameData.StrByteBuffer.Length)
			{
				Array.Resize<byte>(ref GameData.StrByteBuffer, (int)GameData.StrByteBuffer.Length * 2);
			}
			file.Read(GameData.StrByteBuffer, 0, num);
			return Encoding.UTF8.GetString(GameData.StrByteBuffer, 0, num);
		}

		public void resolveAllReferences()
		{
			ParallelOptions parallelOption = new ParallelOptions()
			{
				MaxDegreeOfParallelism = Environment.ProcessorCount
			};
			Parallel.ForEach<KeyValuePair<string, GameData.Item>>(this.items, parallelOption, (KeyValuePair<string, GameData.Item> item) => item.Value.resolveReferences(this));
		}

		public bool save(string filename)
		{
			int num = 16;
			Directory.CreateDirectory(filename.Remove(filename.LastIndexOf('\\')));
			FileStream fileStream = System.IO.File.Open(filename, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(num);
			GameData.writeHeader(this.header, binaryWriter);
			binaryWriter.Write(this.lastID);
			List<GameData.Item> items = new List<GameData.Item>();
			foreach (GameData.Item value in this.items.Values)
			{
				GameData.State state = value.getState();
				if (state == GameData.State.MODIFIED || state == GameData.State.OWNED || state == GameData.State.REMOVED)
				{
					items.Add(value);
				}
				else
				{
					if (state != GameData.State.LOCKED_REMOVED || !value.hasLocalChanges())
					{
						continue;
					}
					items.Add(value);
				}
			}
			binaryWriter.Write(items.Count);
			foreach (GameData.Item item in items)
			{
				binaryWriter.Write(0);
				binaryWriter.Write((int)item.type);
				binaryWriter.Write(item.id);
				GameData.writeString(item.ModName, binaryWriter);
				GameData.writeString(item.stringID, binaryWriter);
				item.save(binaryWriter);
			}
			binaryWriter.Close();
			fileStream.Close();
			return true;
		}

		public static GameData.Desc setDesc(itemType type, string category, string name, object value, string description)
		{
			GameData.Desc desc = new GameData.Desc()
			{
				description = description,
				defaultValue = value,
				category = category
			};
			GameData.setDesc(type, name, desc);
			return desc;
		}

		public static GameData.Desc setDesc(itemType type, string category, string name, string fileType, GameData.File value, string description)
		{
			GameData.Desc desc = GameData.setDesc(type, category, name, value, description);
			desc.mask = fileType;
			return desc;
		}

		public static GameData.Desc setDesc(itemType type, string name, itemType list, GameData.TripleInt values, int valueCount, string description)
		{
			GameData.Desc desc = GameData.setDesc(type, "", name, values, description);
			desc.list = list;
			desc.flags = valueCount;
			return desc;
		}

		public static GameData.Desc setDesc(itemType type, string name, itemType list, GameData.vec pos, GameData.quat rot, string description)
		{
			GameData.Instance instance = new GameData.Instance();
			GameData.Desc desc = GameData.setDesc(type, "", name, instance, description);
			desc.list = list;
			instance["x"] = pos.x;
			instance["y"] = pos.y;
			instance["z"] = pos.z;
			instance["qx"] = rot.x;
			instance["qy"] = rot.y;
			instance["qz"] = rot.z;
			instance["qw"] = rot.w;
			return desc;
		}

		public static GameData.Desc setDesc(itemType type, string name, GameData.Desc d)
		{
			if (!GameData.desc.ContainsKey(type))
			{
				GameData.desc[type] = new SortedList<string, GameData.Desc>();
			}
			if (GameData.desc[type].ContainsKey(name) && GameData.isListType(GameData.desc[type][name]) != GameData.isListType(d))
			{
				MessageBox.Show(string.Concat("Error: Paramerter conflict - ", type.ToString(), ":", name));
			}
			GameData.desc[type][name] = d;
			return d;
		}

		public static void writeHeader(GameData.Header h, BinaryWriter file)
		{
			file.Write(h.Version);
			GameData.writeString(h.Author, file);
			GameData.writeString(h.Description, file);
			GameData.writeString((h.Dependencies == null ? "" : string.Join(",", h.Dependencies)), file);
			List<string> strs = new List<string>();
			if (h.Referenced != null)
			{
				foreach (string referenced in h.Referenced)
				{
					if (h.Dependencies.Contains(referenced))
					{
						continue;
					}
					strs.Add(referenced);
				}
			}
			GameData.writeString(string.Join(",", strs), file);
		}

		private static void writeString(string s, BinaryWriter file)
		{
			file.Write(Encoding.UTF8.GetByteCount(s));
			file.Write(s.ToCharArray());
		}

		public class Colour
		{
			public int @value;

			public Colour(int c)
			{
				this.@value = c;
			}

			public override string ToString()
			{
				return string.Format("{0}, {1}, {2}", this.@value >> 16 & 255, this.@value >> 8 & 255, this.@value & 255);
			}
		}

		public class Desc
		{
            public itemType list = itemType.NULL_ITEM;

            public object defaultValue;

            public string description = "";

            public string category = "misc";

            public string mask = "";

            public int flags;

            public int limit;

            public GameData.DescCondition condition;

            public Desc()
			{
			}
		}

		public class DescCondition
		{
			public string key;

			public object values;

			public bool match;

			public DescCondition()
			{
			}
		}

		public class File : IComparable
		{
			public string filename;

			public File(string f)
			{
				this.filename = f;
			}

			public int CompareTo(object obj)
			{
				return this.filename.CompareTo(obj.ToString());
			}

			public override bool Equals(object obj)
			{
				return this.filename.Equals(obj.ToString());
			}

			public override int GetHashCode()
			{
				return this.filename.GetHashCode();
			}

			public override string ToString()
			{
				return this.filename;
			}
		}

		public class Header
		{
            public string Author = "";

            public string Description = "";

            public List<string> Dependencies;

            public List<string> Referenced;

            public int Version = 1;

            public Header()
			{
			}
		}

		public class Instance : GameData.Item
		{
			public GameData.Item resolvedRef;

			public ArrayList resolvedStates;

			public Instance() : base(itemType.NULL_ITEM, "")
			{
				base.Name = "";
			}
		}

		[DebuggerDisplay("StringID = {stringID}")]
		public class Item
		{
			public int id;

			private GameData.State cachedState;

			private string baseName;

			private string modName;

			private string lockedName;

			private string mod;

            private SortedList<string, object> data = new SortedList<string, object>();

            private SortedList<string, object> modData = new SortedList<string, object>();

            private SortedList<string, object> lockedData = new SortedList<string, object>();

            private SortedList<string, ArrayList> references = new SortedList<string, ArrayList>();

            private SortedList<string, ArrayList> removed = new SortedList<string, ArrayList>();

            private SortedList<string, GameData.Instance> instances = new SortedList<string, GameData.Instance>();


            public GameData.Item.Accessor<int> idata;

			public GameData.Item.Accessor<bool> bdata;

			public GameData.Item.Accessor<float> fdata;

			public GameData.Item.Accessor<string> sdata;

			public GameData.Item.Accessor<GameData.File> filesdata;

			public object this[string s]
			{
				get
				{
					if (this.lockedData.ContainsKey(s))
					{
						return this.lockedData[s];
					}
					if (!this.modData.ContainsKey(s))
					{
						return this.data[s];
					}
					return this.modData[s];
				}
				set
				{
					if (!this.data.ContainsKey(s) || !value.Equals(this.data[s]))
					{
						this.modData[s] = value;
					}
					else if (this.modData.ContainsKey(s))
					{
						this.modData.Remove(s);
					}
					this.refreshState();
				}
			}

			public bool HasInstances
			{
				get
				{
					return this.instances.Count > 0;
				}
			}

			public string Mod
			{
				get
				{
					return this.mod;
				}
			}

			public string ModName
			{
				get
				{
					return this.modName ?? this.OriginalName;
				}
			}

			public string Name
			{
				get
				{
					if (this.lockedName != null)
					{
						return this.lockedName;
					}
					if (this.modName == null)
					{
						return this.baseName;
					}
					return this.modName;
				}
				set
				{
					string str;
					if (this.baseName == value)
					{
						str = null;
					}
					else
					{
						str = value;
					}
					this.modName = str;
					this.refreshState();
				}
			}

			private navigation nav
			{
				get
				{
					navigation _navigation;
					IEnumerator enumerator = Application.OpenForms.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Form current = (Form)enumerator.Current;
							if (!(current is baseForm))
							{
								continue;
							}
							_navigation = (current as baseForm).nav;
							return _navigation;
						}
						return null;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					return _navigation;
				}
			}

			public string OriginalName
			{
				get
				{
					return this.baseName;
				}
			}

			public int refCount
			{
				get;
				private set;
			}

			public string stringID
			{
				get;
				set;
			}

			public itemType type
			{
				get;
				private set;
			}

			public Item(itemType type, string id)
			{
				this.type = type;
				this.stringID = id;
				this.setupAccessors();
				this.refCount = 0;
			}

			public GameData.Instance addInstance(string id, string item, float x = 0f, float y = 0f, float z = 0f, float qx = 0f, float qy = 0f, float qz = 0f, float qw = 1f)
			{
				GameData.Instance instance;
				if (!this.instances.TryGetValue(id, out instance))
				{
					instance = new GameData.Instance();
					this.instances.Add(id, instance);
				}
				instance.revert();
				instance["x"] = x;
				instance["y"] = y;
				instance["z"] = z;
				instance["qx"] = qx;
				instance["qy"] = qy;
				instance["qz"] = qz;
				instance["qw"] = qw;
				instance["ref"] = item;
				this.refreshState();
				return instance;
			}

			public GameData.Instance addInstance(string id, GameData.Item item, float x = 0f, float y = 0f, float z = 0f, float qx = 0f, float qy = 0f, float qz = 0f, float qw = 1f)
			{
				GameData.Instance instance = this.addInstance(id, item.stringID, x, y, z, qx, qy, qz, qw);
				if (instance.resolvedRef != null)
				{
					instance.resolvedRef.removeRef(this);
				}
				item.addRef(this);
				instance.resolvedRef = item;
				return instance;
			}

			public GameData.Instance addInstance(GameData.Item item, float x = 0f, float y = 0f, float z = 0f, float qx = 0f, float qy = 0f, float qz = 0f, float qw = 1f)
			{
				string str = this.instances.Count.ToString();
				return this.addInstance(str, item, x, y, z, qx, qy, qz, qw);
			}

			public GameData.Instance addInstance(GameData.Item item, GameData.Instance value)
			{
				return this.addInstance(item, value.fdata["x"], value.fdata["y"], value.fdata["z"], value.fdata["qx"], value.fdata["qy"], value.fdata["qz"], value.fdata["qw"]);
			}

			public GameData.Instance addInstance(string id, GameData.Instance value)
			{
				return this.addInstance(id, value.sdata["ref"], value.fdata["x"], value.fdata["y"], value.fdata["z"], value.fdata["qx"], value.fdata["qy"], value.fdata["qz"], value.fdata["qw"]);
			}

			private void addRef(GameData.Item from)
			{
				foreach (string str in from.referenceLists())
				{
					foreach (object item in from.references[str])
					{
						if (((GameData.Reference)item).item != this)
						{
							continue;
						}
						return;
					}
				}
				foreach (GameData.Instance value in from.instances.Values)
				{
					if (value.resolvedRef != this)
					{
						if (value.resolvedStates == null)
						{
							continue;
						}
						foreach (object resolvedState in value.resolvedStates)
						{
							if ((GameData.Item)resolvedState != this)
							{
								continue;
							}
							return;
						}
					}
					else
					{
						return;
					}
				}
				this.refCount = this.refCount + 1;
			}

			public GameData.Reference addReference(string section, string id, int? v0 = null, int? v1 = null, int? v2 = null)
			{
				GameData.Reference reference = this.getReference(section, id);
				if (reference == null)
				{
					reference = this.getRemovedReference(section, id);
					if (reference == null)
					{
						reference = new GameData.Reference(id, null);
					}
					else
					{
						this.removed[section].Remove(reference);
					}
					this.references[section].Add(reference);
					GameData.Desc desc = GameData.getDesc(this.type, section);
					if (!(desc.defaultValue is GameData.TripleInt))
					{
						reference.mod = new GameData.TripleInt(0, 0, 0);
					}
					else
					{
						reference.mod = new GameData.TripleInt((GameData.TripleInt)desc.defaultValue);
					}
					if (v0.HasValue)
					{
						reference.mod.v0 = v0.Value;
					}
					if (v1.HasValue)
					{
						reference.mod.v1 = v1.Value;
					}
					if (v2.HasValue)
					{
						reference.mod.v2 = v2.Value;
					}
					if (reference.original != null && reference.original.Equals(reference.mod))
					{
						reference.mod = null;
					}
					this.refreshState();
				}
				return reference;
			}

			public void addReference(string section, GameData.Item item, int? v0 = null, int? v1 = null, int? v2 = null)
			{
				GameData.Reference reference = this.addReference(section, item.stringID, v0, v1, v2);
				if (reference.item == null)
				{
					item.addRef(this);
					reference.item = item;
				}
			}

			public void changeOwner(string mod)
			{
				this.mod = mod;
			}

			public bool checkReferences()
			{
				bool flag = true;
				if (this.getState() == GameData.State.REMOVED)
				{
					return true;
				}
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					foreach (GameData.Reference value in reference.Value)
					{
						if (value.item != null)
						{
							if (value.item.getState() != GameData.State.REMOVED)
							{
								continue;
							}
							Errors.addError(Error.WARNING, this, null, string.Concat("Reference to deleted item ", value.itemID, " in ", reference.Key));
							flag = false;
						}
						else
						{
							Errors.addError(Error.WARNING, this, null, string.Concat("Undefined reference to ", value.itemID, " in ", reference.Key));
							flag = false;
						}
					}
				}
				return flag;
			}

			public int clean()
			{
				List<string> strs = new List<string>();
				SortedList<string, GameData.Desc> item = GameData.desc[this.type];
				foreach (KeyValuePair<string, object> modDatum in this.modData)
				{
					if (this.isLooper(modDatum.Key) || item.ContainsKey(modDatum.Key))
					{
						continue;
					}
					strs.Add(modDatum.Key);
				}
				foreach (string str in strs)
				{
					this.modData.Remove(str);
				}
				return strs.Count;
			}

			public void clearInstances()
			{
				ArrayList arrayLists = new ArrayList(this.instances.Count);
				foreach (KeyValuePair<string, GameData.Instance> instance in this.instances)
				{
					arrayLists.Add(instance.Key);
				}
				foreach (string arrayList in arrayLists)
				{
					this.removeInstance(arrayList);
				}
				this.refreshState();
			}

			public bool ContainsKey(string key)
			{
				if (this.modData.ContainsKey(key) || this.data.ContainsKey(key))
				{
					return true;
				}
				return this.lockedData.ContainsKey(key);
			}

			private int countChangedInstances()
			{
				int num = 0;
				foreach (GameData.Instance value in this.instances.Values)
				{
					if (value.getState() != GameData.State.MODIFIED && value.getState() != GameData.State.OWNED && value.getState() != GameData.State.REMOVED)
					{
						continue;
					}
					num++;
				}
				return num;
			}

			private int countChangedReferences()
			{
				int num = 0;
				foreach (ArrayList value in this.removed.Values)
				{
					foreach (object obj in value)
					{
						if (((GameData.Reference)obj).locked != null)
						{
							continue;
						}
						num++;
					}
				}
				foreach (ArrayList arrayLists in this.references.Values)
				{
					foreach (object obj1 in arrayLists)
					{
						if (((GameData.Reference)obj1).mod == null)
						{
							continue;
						}
						num++;
					}
				}
				return num;
			}

			private int countModifiedReferences(string section)
			{
				int num = 0;
				foreach (object item in this.references[section])
				{
					if (((GameData.Reference)item).mod == null)
					{
						continue;
					}
					num++;
				}
				if (this.removed.ContainsKey(section))
				{
					foreach (object obj in this.removed[section])
					{
						if (((GameData.Reference)obj).mod == null)
						{
							continue;
						}
						num++;
					}
				}
				return num;
			}

			private static int countType<T>(SortedList<string, object> list)
			{
				int num = 0;
				foreach (KeyValuePair<string, object> keyValuePair in list)
				{
					if (!(keyValuePair.Value is T))
					{
						continue;
					}
					num++;
				}
				return num;
			}

			public GameData.Item createFlatCopy()
			{
				GameData.Item item = new GameData.Item(this.type, this.stringID)
				{
					baseName = this.Name
				};
				foreach (KeyValuePair<string, object> datum in this.data)
				{
					item.data[datum.Key] = datum.Value;
				}
				foreach (KeyValuePair<string, object> modDatum in this.modData)
				{
					item.data[modDatum.Key] = modDatum.Value;
				}
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					item.references.Add(reference.Key, new ArrayList());
					foreach (GameData.Reference value in reference.Value)
					{
						if (value.mod == null && value.original == null)
						{
							continue;
						}
						item.references[reference.Key].Add(new GameData.Reference(value.itemID, new GameData.TripleInt(value.mod ?? value.original)));
					}
				}
				foreach (KeyValuePair<string, GameData.Instance> instance in this.instances)
				{
					if (instance.Value.cachedState == GameData.State.REMOVED || instance.Value.cachedState == GameData.State.LOCKED)
					{
						continue;
					}
					GameData.Instance instance1 = item.addInstance(instance.Key, instance.Value);
					foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in instance.Value.referenceData("states", false))
					{
						int? nullable = null;
						int? nullable1 = nullable;
						nullable = null;
						int? nullable2 = nullable;
						nullable = null;
						instance1.addReference("states", keyValuePair.Key, nullable1, nullable2, nullable);
					}
					instance1.flatten();
				}
				return item;
			}

			public void flagDeleted()
			{
				this.revert();
				this.cachedState = GameData.State.REMOVED;
			}

			public void flatten()
			{
				this.baseName = this.Name;
				this.modName = null;
				foreach (KeyValuePair<string, object> modDatum in this.modData)
				{
					this.data[modDatum.Key] = modDatum.Value;
				}
				this.modData.Clear();
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					foreach (GameData.Reference value in reference.Value)
					{
						if (value.mod == null)
						{
							continue;
						}
						value.original = new GameData.TripleInt(value.mod);
						value.mod = null;
					}
				}
				this.removed.Clear();
				List<string> strs = new List<string>();
				foreach (KeyValuePair<string, GameData.Instance> instance in this.instances)
				{
					if (instance.Value.cachedState != GameData.State.REMOVED)
					{
						instance.Value.flatten();
					}
					else
					{
						strs.Add(instance.Key);
					}
				}
				foreach (string str in strs)
				{
					this.instances.Remove(str);
				}
			}

			public List<GameData.Item> GetAllReferences(string section)
			{
				List<GameData.Item> items = new List<GameData.Item>();
				ArrayList arrayLists = null;
				if (this.references.TryGetValue(section, out arrayLists))
				{
					items.Capacity = arrayLists.Count;
					foreach (object obj in arrayLists)
					{
						if (((GameData.Reference)obj).item == null)
						{
							continue;
						}
						items.Add(((GameData.Reference)obj).item);
					}
				}
				return items;
			}

			public List<GameData.Item> GetAllReferencesOfType(itemType referenceType)
			{
				List<GameData.Item> items = new List<GameData.Item>();
				foreach (KeyValuePair<string, GameData.Desc> item in GameData.desc[this.type])
				{
					if (item.Value.list != referenceType || !this.references.ContainsKey(item.Key))
					{
						continue;
					}
					ArrayList arrayLists = this.references[item.Key];
					items.Capacity = Math.Max(items.Capacity, items.Count + arrayLists.Count);
					foreach (object obj in arrayLists)
					{
						GameData.Reference reference = obj as GameData.Reference;
						if (reference == null || reference.item == null || reference.item.getState() == GameData.State.REMOVED || reference.item.getState() == GameData.State.LOCKED_REMOVED || reference.item.type != referenceType)
						{
							continue;
						}
						items.Add(reference.item);
					}
				}
				return items;
			}

			public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
			{
				foreach (KeyValuePair<string, object> lockedDatum in this.lockedData)
				{
					yield return lockedDatum;
				}
				foreach (KeyValuePair<string, object> modDatum in this.modData)
				{
					if (this.lockedData.ContainsKey(modDatum.Key))
					{
						continue;
					}
					yield return modDatum;
				}
				foreach (KeyValuePair<string, object> datum in this.data)
				{
					if (this.modData.ContainsKey(datum.Key) || this.lockedData.ContainsKey(datum.Key))
					{
						continue;
					}
					yield return datum;
				}
			}

			public GameData.Instance getInstance(string id)
			{
				if (!this.instances.ContainsKey(id))
				{
					return null;
				}
				return this.instances[id];
			}

			public GameData.State getNameState()
			{
				if (this.lockedName != null || this.cachedState == GameData.State.LOCKED)
				{
					return GameData.State.LOCKED;
				}
				if (this.modName != null)
				{
					return GameData.State.MODIFIED;
				}
				return GameData.State.ORIGINAL;
			}

			private GameData.Reference getReference(string section, string id)
			{
				GameData.Reference reference;
				if (!this.references.ContainsKey(section))
				{
					this.references.Add(section, new ArrayList());
				}
				IEnumerator enumerator = this.references[section].GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						GameData.Reference current = (GameData.Reference)enumerator.Current;
						if (current.itemID != id)
						{
							continue;
						}
						reference = current;
						return reference;
					}
					return null;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				return reference;
			}

			private GameData.Reference getReference(string section, int index)
			{
				return (GameData.Reference)this.references[section][index];
			}

			public int getReferenceCount(string section)
			{
				if (!this.references.ContainsKey(section))
				{
					return 0;
				}
				return this.references[section].Count;
			}

			public GameData.Item GetReferenceItem(string section, int index)
			{
				ArrayList arrayLists;
				if (!this.references.TryGetValue(section, out arrayLists) || index >= arrayLists.Count)
				{
					return null;
				}
				return ((GameData.Reference)arrayLists[index]).item;
			}

			private GameData.TripleInt getReferenceValue(GameData.Reference r)
			{
				if (r == null)
				{
					return null;
				}
				return r.Values;
			}

			public GameData.TripleInt getReferenceValue(string section, string id)
			{
				GameData.TripleInt referenceValue = this.getReferenceValue(this.getReference(section, id));
				if (referenceValue == null)
				{
					return null;
				}
				return new GameData.TripleInt(referenceValue);
			}

			public GameData.TripleInt getReferenceValue(string section, int index)
			{
				GameData.TripleInt referenceValue = this.getReferenceValue(this.getReference(section, index));
				if (referenceValue == null)
				{
					return null;
				}
				return new GameData.TripleInt(referenceValue);
			}

			private GameData.Reference getRemovedReference(string section, string id)
			{
				GameData.Reference reference;
				if (this.removed.ContainsKey(section))
				{
					IEnumerator enumerator = this.removed[section].GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							GameData.Reference current = (GameData.Reference)enumerator.Current;
							if (current.itemID != id)
							{
								continue;
							}
							reference = current;
							return reference;
						}
						return null;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					return reference;
				}
				return null;
			}

			public GameData.State getState(string s)
			{
				if (this.lockedData.ContainsKey(s))
				{
					return GameData.State.LOCKED;
				}
				if (this.modData.ContainsKey(s))
				{
					return GameData.State.MODIFIED;
				}
				if (this.data.ContainsKey(s))
				{
					return GameData.State.ORIGINAL;
				}
				return GameData.State.INVALID;
			}

			public GameData.State getState(string section, string id)
			{
				GameData.Reference reference = this.getReference(section, id);
				if (reference == null)
				{
					reference = this.getRemovedReference(section, id);
					if (reference == null)
					{
						return GameData.State.INVALID;
					}
					if (reference.locked != null)
					{
						return GameData.State.LOCKED_REMOVED;
					}
					return GameData.State.REMOVED;
				}
				if (reference.item == null || reference.item.getState() == GameData.State.REMOVED)
				{
					return GameData.State.INVALID;
				}
				if (reference.locked != null)
				{
					return GameData.State.LOCKED;
				}
				if (reference.mod != null && reference.original != null)
				{
					return GameData.State.MODIFIED;
				}
				if (reference.original == null)
				{
					return GameData.State.OWNED;
				}
				return GameData.State.ORIGINAL;
			}

			public GameData.State getState()
			{
				if (this.cachedState == GameData.State.UNKNOWN)
				{
					if (this.baseName == null)
					{
						this.cachedState = GameData.State.OWNED;
					}
					else if (!this.hasLocalChanges())
					{
						this.cachedState = GameData.State.ORIGINAL;
					}
					else
					{
						this.cachedState = GameData.State.MODIFIED;
					}
				}
				return this.cachedState;
			}

			public bool hasLocalChanges()
			{
				if (this.modName != null || this.modData.Count > 0 || this.countChangedReferences() > 0)
				{
					return true;
				}
				return this.countChangedInstances() > 0;
			}

			public bool hasReference(string section)
			{
				if (this.references.ContainsKey(section) && this.references[section].Count > 0)
				{
					return true;
				}
				return false;
			}

			public bool hasReferenceTo(GameData.Item item)
			{
				bool flag;
				using (IEnumerator<KeyValuePair<string, ArrayList>> enumerator = this.references.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IEnumerator enumerator1 = enumerator.Current.Value.GetEnumerator();
						try
						{
							while (enumerator1.MoveNext())
							{
								if (((GameData.Reference)enumerator1.Current).item != item)
								{
									continue;
								}
								flag = true;
								return flag;
							}
						}
						finally
						{
							IDisposable disposable = enumerator1 as IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					return false;
				}
				return flag;
			}

			public bool hasReferenceTo(string section, string id)
			{
				bool flag;
				using (IEnumerator<KeyValuePair<string, GameData.TripleInt>> enumerator = this.referenceData(section, false).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Key != id)
						{
							continue;
						}
						flag = true;
						return flag;
					}
					return false;
				}
				return flag;
			}

			public IEnumerable<KeyValuePair<string, GameData.Instance>> instanceData()
			{
				foreach (KeyValuePair<string, GameData.Instance> instance in this.instances)
				{
					yield return instance;
				}
			}

			public bool isLooper(string key)
			{
				GameData.Desc desc;
				return this.isLooper(key, out desc);
			}

			public bool isLooper(string key, out GameData.Desc desc)
			{
				int length = key.Length - 1;
				while (length > 0 && char.IsDigit(key[length]))
				{
					length--;
				}
				if (length >= key.Length - 1)
				{
					desc = GameData.nullDesc;
				}
				else
				{
					key = string.Concat(key.Remove(length + 1), "0");
					desc = GameData.getDesc(this.type, key);
				}
				return desc.limit > 0;
			}

			public bool load(BinaryReader file, string name, GameData.ModMode mode, int fileVersion, string filename, bool newItem)
			{
				int num;
				int num1;
				int? nullable;
				string str;
				SortedList<string, object> strs = null;
				if (mode == GameData.ModMode.BASE)
				{
					strs = this.data;
				}
				else if (mode == GameData.ModMode.ACTIVE)
				{
					strs = this.modData;
				}
				else if (mode == GameData.ModMode.LOCKED)
				{
					strs = this.lockedData;
				}
				bool flag = false;
				if (fileVersion < 14 && this.type == itemType.CONSTANTS)
				{
					MessageBox.Show("Old mod type detected.  Modifications to GLOBAL CONSTANTS have been lost, please re-do your changes and re-save the mod to update it.");
					strs = new SortedList<string, object>();
				}
				Dictionary<string, bool> strs1 = null;
				if (fileVersion >= 15)
				{
					GameData.Item.ItemLoadFlags itemLoadFlag = (GameData.Item.ItemLoadFlags)(file.ReadInt32() & 2147483647);
					if (itemLoadFlag.HasFlag(GameData.Item.ItemLoadFlags.MODIFIED) & newItem)
					{
						if (this.nav.FileMode == navigation.ModFileMode.SINGLE)
						{
							this.baseName = (itemLoadFlag.HasFlag(GameData.Item.ItemLoadFlags.RENAMED) ? "?" : name);
						}
						else
						{
							flag = true;
							Errors.addError(Error.CRITICAL, this, filename, "Modified item undefined. Perhaps the load order is incorrect or a mod is missing.");
						}
					}
					else if (!itemLoadFlag.HasFlag(GameData.Item.ItemLoadFlags.MODIFIED) && !newItem)
					{
						Errors.addError(Error.CRITICAL, this, filename, string.Concat(new string[] { "Item with id ", this.stringID, " has already been defined by ", this.mod, "." }));
					}
					if (!itemLoadFlag.HasFlag(GameData.Item.ItemLoadFlags.RENAMED) && this.Name != null)
					{
						name = this.Name;
					}
				}
				else if (fileVersion >= 11)
				{
					num = file.ReadInt32();
					if (num > 0 && filename != "gamedata.base")
					{
						strs1 = new Dictionary<string, bool>();
						for (int i = 0; i < num; i++)
						{
							strs1[GameData.readString(file)] = file.ReadBoolean();
						}
					}
				}
				if (mode == GameData.ModMode.BASE)
				{
					this.baseName = name;
				}
				else if (name != this.Name)
				{
					if (mode == GameData.ModMode.ACTIVE)
					{
						this.modName = name;
					}
					else if (mode == GameData.ModMode.LOCKED)
					{
						this.lockedName = name;
					}
				}
				if (newItem)
				{
					this.mod = filename;
				}
				num = file.ReadInt32();
				for (int j = 0; j < num; j++)
				{
					string str1 = GameData.readString(file);
					bool flag1 = file.ReadBoolean();
					if (this.tagged(strs1, str1))
					{
						strs[str1] = flag1;
					}
				}
				num = file.ReadInt32();
				for (int k = 0; k < num; k++)
				{
					string str2 = GameData.readString(file);
					float single = file.ReadSingle();
					if (this.tagged(strs1, str2))
					{
						strs[str2] = single;
					}
				}
				num = file.ReadInt32();
				for (int l = 0; l < num; l++)
				{
					string str3 = GameData.readString(file);
					int num2 = file.ReadInt32();
					if (this.tagged(strs1, str3))
					{
						strs[str3] = num2;
					}
				}
				if (fileVersion > 8)
				{
					num = file.ReadInt32();
					for (int m = 0; m < num; m++)
					{
						string str4 = GameData.readString(file);
						GameData.vec _vec = new GameData.vec()
						{
							x = file.ReadSingle(),
							y = file.ReadSingle(),
							z = file.ReadSingle()
						};
						if (this.tagged(strs1, str4))
						{
							strs[str4] = _vec;
						}
					}
					num = file.ReadInt32();
					for (int n = 0; n < num; n++)
					{
						string str5 = GameData.readString(file);
						GameData.quat _quat = new GameData.quat()
						{
							x = file.ReadSingle(),
							y = file.ReadSingle(),
							z = file.ReadSingle(),
							w = file.ReadSingle()
						};
						if (this.tagged(strs1, str5))
						{
							strs[str5] = _quat;
						}
					}
				}
				num = file.ReadInt32();
				for (int o = 0; o < num; o++)
				{
					string str6 = GameData.readString(file);
					string str7 = GameData.readString(file);
					if ((!strs.ContainsKey(str6) || strs[str6] is string) && this.tagged(strs1, str6))
					{
						strs[str6] = str7;
					}
				}
				num = file.ReadInt32();
				for (int p = 0; p < num; p++)
				{
					string str8 = GameData.readString(file);
					string str9 = GameData.readString(file);
					if (this.tagged(strs1, str8))
					{
						strs[str8] = new GameData.File(str9);
					}
				}
				num = file.ReadInt32();
				for (int q = 0; q < num; q++)
				{
					string str10 = GameData.readString(file);
					int num3 = file.ReadInt32();
					for (int r = 0; r < num3; r++)
					{
						if (fileVersion >= 8)
						{
							string str11 = GameData.readString(file);
							GameData.TripleInt tripleInt = new GameData.TripleInt(0, 0, 0)
							{
								v0 = file.ReadInt32()
							};
							if (fileVersion >= 10)
							{
								tripleInt.v1 = file.ReadInt32();
								tripleInt.v2 = file.ReadInt32();
							}
							if (strs1 == null || strs1.ContainsKey(string.Concat("-ref-", str11)))
							{
								bool flag2 = (strs1 == null || strs1[string.Concat("-ref-", str11)] ? tripleInt.v2 == 2147483647 : true);
								GameData.Reference reference = this.getReference(str10, str11);
								if (!flag2 || reference != null)
								{
									if (reference == null)
									{
										reference = new GameData.Reference(str11, null);
										this.references[str10].Add(reference);
									}
									else if (flag2)
									{
										if (mode != GameData.ModMode.BASE)
										{
											this.removeReference(str10, str11);
										}
										else
										{
											if (reference.item != null)
											{
												reference.item.removeRef(this);
											}
											this.references[str10].Remove(reference);
										}
										tripleInt = GameData.Reference.Removed;
									}
									if (mode == GameData.ModMode.ACTIVE && this.type == itemType.DIALOGUE_PACKAGE && tripleInt.v1 == 100)
									{
										tripleInt.v1 = 0;
									}
									if (mode == GameData.ModMode.BASE)
									{
										reference.original = tripleInt;
									}
									else if (mode == GameData.ModMode.ACTIVE)
									{
										reference.mod = tripleInt;
									}
									else if (mode == GameData.ModMode.LOCKED)
									{
										reference.locked = tripleInt;
									}
								}
							}
						}
						else
						{
							file.ReadInt64();
						}
					}
				}
				num = file.ReadInt32();
				for (int s = 0; s < num; s++)
				{
					if (fileVersion < 15)
					{
						num1 = file.ReadInt32();
						str = string.Concat(num1.ToString(), "-", filename);
					}
					else
					{
						str = GameData.readString(file);
					}
					string str12 = str;
					GameData.Instance instance = this.getInstance(str12) ?? new GameData.Instance();
					instance["ref"] = (fileVersion < 8 ? "" : GameData.readString(file));
					instance["x"] = file.ReadSingle();
					instance["y"] = file.ReadSingle();
					instance["z"] = file.ReadSingle();
					instance["qw"] = file.ReadSingle();
					instance["qx"] = file.ReadSingle();
					instance["qy"] = file.ReadSingle();
					instance["qz"] = file.ReadSingle();
					if (fileVersion > 6)
					{
						int num4 = file.ReadInt32();
						if (fileVersion >= 15)
						{
							for (int t = 0; t < num4; t++)
							{
								nullable = null;
								int? nullable1 = nullable;
								nullable = null;
								int? nullable2 = nullable;
								nullable = null;
								instance.addReference("states", GameData.readString(file), nullable1, nullable2, nullable);
							}
						}
						else
						{
							for (int u = 0; u < num4; u++)
							{
								num1 = file.ReadInt32();
								nullable = null;
								int? nullable3 = nullable;
								nullable = null;
								int? nullable4 = nullable;
								nullable = null;
								instance.addReference("states", string.Concat(num1.ToString(), "-", filename, "-INGAME"), nullable3, nullable4, nullable);
							}
						}
					}
					if (mode != GameData.ModMode.ACTIVE)
					{
						instance.flatten();
					}
					if (!this.instances.ContainsKey(str12))
					{
						this.instances.Add(str12, instance);
					}
					if (string.IsNullOrEmpty(instance.sdata["ref"]))
					{
						if (this.nav.FileMode != navigation.ModFileMode.SINGLE)
						{
							this.removeInstance(str12);
						}
						else
						{
							this.instances[str12].flatten();
							this.instances[str12].flagDeleted();
						}
					}
					if (mode == GameData.ModMode.LOCKED)
					{
						instance.setLocked();
					}
				}
				if (this.ContainsKey("REMOVED"))
				{
					if (this.bdata["REMOVED"])
					{
						this.cachedState = GameData.State.REMOVED;
					}
					if (mode == GameData.ModMode.LOCKED)
					{
						this.setLocked();
					}
					strs.Remove("REMOVED");
					this.modData.Remove("REMOVED");
					this.lockedData.Remove("REMOVED");
					this.removeRefTargets();
				}
				else if (mode == GameData.ModMode.BASE)
				{
					this.cachedState = GameData.State.ORIGINAL;
				}
				else if (!newItem || mode != GameData.ModMode.LOCKED)
				{
					this.refreshState();
				}
				else
				{
					this.setLocked();
				}
				if (!flag & newItem && mode == GameData.ModMode.ACTIVE && this.nav.FileMode != navigation.ModFileMode.SINGLE && this.setMissingValues() > 0)
				{
					Errors.addError(Error.INFO, this, filename, "New values have been added");
				}
				if (mode == GameData.ModMode.ACTIVE && this.baseName != null)
				{
					foreach (KeyValuePair<string, object> datum in this.data)
					{
						if (!this.modData.ContainsKey(datum.Key) || !this.modData[datum.Key].Equals(datum.Value))
						{
							continue;
						}
						this.modData.Remove(datum.Key);
					}
					foreach (KeyValuePair<string, ArrayList> keyValuePair in this.references)
					{
						foreach (GameData.Reference value in keyValuePair.Value)
						{
							if (value.original == null || !value.original.Equals(value.mod))
							{
								continue;
							}
							value.mod = null;
						}
					}
				}
				if (!flag & newItem && mode == GameData.ModMode.ACTIVE)
				{
					head.AutomaticChanges(this);
				}
				return !flag;
			}

			public void mergeWithBase()
			{
				this.modName = this.Name;
				this.baseName = null;
				foreach (KeyValuePair<string, object> datum in this.data)
				{
					if (this.modData.ContainsKey(datum.Key))
					{
						continue;
					}
					this.modData[datum.Key] = datum.Value;
				}
				this.data.Clear();
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					foreach (GameData.Reference value in reference.Value)
					{
						if (value.mod == null)
						{
							value.mod = new GameData.TripleInt(value.original);
						}
						value.original = null;
					}
				}
				this.removed.Clear();
				foreach (KeyValuePair<string, GameData.Instance> instance in this.instances)
				{
					instance.Value.mergeWithBase();
				}
			}

			public void moveReferenceDown(string section, string id)
			{
				GameData.Reference reference = this.getReference(section, id);
				ArrayList item = this.references[section];
				int num = item.IndexOf(reference);
				if (num < 0 || num == item.Count - 1)
				{
					return;
				}
				item[num] = item[num + 1];
				item[num + 1] = reference;
			}

			public void moveReferenceUp(string section, string id)
			{
				GameData.Reference reference = this.getReference(section, id);
				ArrayList item = this.references[section];
				int num = item.IndexOf(reference);
				if (num <= 0)
				{
					return;
				}
				item[num] = item[num - 1];
				item[num - 1] = reference;
			}

			public object OriginalValue(string key)
			{
				if (!this.data.ContainsKey(key))
				{
					return null;
				}
				return this.data[key];
			}

			public GameData.TripleInt OriginalValue(string list, string refID)
			{
				GameData.Reference reference = this.getReference(list, refID);
				if (reference != null)
				{
					return reference.original;
				}
				reference = this.getRemovedReference(list, refID);
				if (reference == null)
				{
					return null;
				}
				return reference.original;
			}

			public IEnumerable<KeyValuePair<string, GameData.TripleInt>> referenceData(string name, bool includeDeleted = false)
			{
				if (this.references.ContainsKey(name))
				{
					foreach (GameData.Reference item in this.references[name])
					{
						yield return new KeyValuePair<string, GameData.TripleInt>(item.itemID, new GameData.TripleInt(item.Values));
					}
				}
				if (includeDeleted && this.removed.ContainsKey(name))
				{
					foreach (GameData.Reference reference in this.removed[name])
					{
						yield return new KeyValuePair<string, GameData.TripleInt>(reference.itemID, new GameData.TripleInt(reference.Values));
					}
				}
			}

			public IEnumerable<Tuple<GameData.Item, GameData.TripleInt>> referenceItems(string name, bool includeDeleted = false)
			{
				if (this.references.ContainsKey(name))
				{
					foreach (GameData.Reference item in this.references[name])
					{
						yield return new Tuple<GameData.Item, GameData.TripleInt>(item.item, item.Values);
					}
				}
				if (includeDeleted && this.removed.ContainsKey(name))
				{
					foreach (GameData.Reference reference in this.removed[name])
					{
						yield return new Tuple<GameData.Item, GameData.TripleInt>(reference.item, reference.Values);
					}
				}
			}

			public IEnumerable<string> referenceLists()
			{
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					yield return reference.Key;
				}
			}

			public void refreshState()
			{
				if (this.cachedState != GameData.State.LOCKED)
				{
					this.cachedState = GameData.State.UNKNOWN;
				}
			}

			public void Remove(string key)
			{
				this.modData.Remove(key);
				if (this.baseName == null)
				{
					this.data.Remove(key);
				}
			}

			public void removeInstance(string id)
			{
				GameData.Instance instance = this.getInstance(id);
				bool fileMode = this.nav.FileMode == navigation.ModFileMode.SINGLE;
				if (instance == null & fileMode)
				{
					this.addInstance(id, "", 0f, 0f, 0f, 0f, 0f, 0f, 1f).flagDeleted();
					this.refreshState();
					return;
				}
				if (instance.getState() == GameData.State.OWNED || fileMode && instance.getState() == GameData.State.REMOVED)
				{
					this.instances.Remove(id);
				}
				else
				{
					instance.flagDeleted();
				}
				if (instance.resolvedRef != null)
				{
					instance.resolvedRef.removeRef(this);
				}
				if (instance.resolvedStates != null)
				{
					foreach (GameData.Item resolvedState in instance.resolvedStates)
					{
						if (resolvedState == null)
						{
							continue;
						}
						resolvedState.removeRef(this);
					}
				}
				instance.resolvedStates = null;
				instance.resolvedRef = null;
				this.refreshState();
			}

			public int removeInvalidReferences()
			{
				int count = 0;
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					List<GameData.Reference> references = new List<GameData.Reference>();
					foreach (GameData.Reference value in reference.Value)
					{
						if (value.item != null && value.item.getState() != GameData.State.REMOVED)
						{
							continue;
						}
						references.Add(value);
					}
					foreach (GameData.Reference reference1 in references)
					{
						this.removeReference(reference.Key, reference1);
					}
					count += references.Count;
				}
				return count;
			}

			private void removeRef(GameData.Item from)
			{
				bool flag = false;
				foreach (string str in from.referenceLists())
				{
					foreach (object item in from.references[str])
					{
						if (((GameData.Reference)item).item != this)
						{
							continue;
						}
						if (!flag)
						{
							flag = true;
						}
						else
						{
							return;
						}
					}
				}
				foreach (GameData.Instance value in from.instances.Values)
				{
					if (value.resolvedRef == this)
					{
						if (!flag)
						{
							flag = true;
						}
						else
						{
							return;
						}
					}
					if (value.resolvedStates == null)
					{
						continue;
					}
					foreach (object resolvedState in value.resolvedStates)
					{
						if ((GameData.Item)resolvedState != this)
						{
							continue;
						}
						if (!flag)
						{
							flag = true;
						}
						else
						{
							return;
						}
					}
				}
				this.refCount = this.refCount - 1;
			}

			public void removeReference(string section, int index)
			{
				this.removeReference(section, this.getReference(section, index));
			}

			public void removeReference(string section, string id)
			{
				this.removeReference(section, this.getReference(section, id));
			}

			public void removeReference(string section, GameData.Item item)
			{
				this.removeReference(section, this.getReference(section, item.stringID));
			}

			private void removeReference(string section, GameData.Reference r)
			{
				if (r == null)
				{
					return;
				}
				if (r.item != null)
				{
					r.item.removeRef(this);
				}
				if (r.original != null)
				{
					r.item = null;
					r.mod = GameData.Reference.Removed;
					if (!this.removed.ContainsKey(section))
					{
						this.removed.Add(section, new ArrayList());
					}
					this.removed[section].Add(r);
				}
				this.references[section].Remove(r);
				this.refreshState();
			}

			private void removeRefTargets()
			{
				foreach (string str in this.referenceLists())
				{
					foreach (GameData.Reference item in this.references[str])
					{
						if (item.item != null)
						{
							item.item.removeRef(this);
						}
						item.item = null;
					}
				}
				foreach (GameData.Instance value in this.instances.Values)
				{
					if (value.resolvedRef != null)
					{
						value.resolvedRef.removeRef(this);
					}
					if (value.resolvedStates == null)
					{
						continue;
					}
					foreach (GameData.Item resolvedState in value.resolvedStates)
					{
						if (resolvedState == null)
						{
							continue;
						}
						resolvedState.removeRef(this);
					}
				}
			}

			public bool renameInstance(string oldID, string newID)
			{
				GameData.Instance instance = this.getInstance(oldID);
				if (instance == null || this.getInstance(newID) != null || newID == "")
				{
					return false;
				}
				this.instances.Remove(oldID);
				this.addInstance(newID, instance);
				return true;
			}

			private void resolveReference(GameData source, string id, ref GameData.Item target)
			{
				GameData.Item item = source.getItem(id);
				if (item != null && target != item)
				{
					item.addRef(this);
				}
				target = item;
			}

			public void resolveReferences(GameData source)
			{
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					foreach (GameData.Reference value in reference.Value)
					{
						this.resolveReference(source, value.itemID, ref value.item);
					}
				}
				foreach (GameData.Instance arrayLists in this.instances.Values)
				{
					if (arrayLists.getState() == GameData.State.REMOVED || arrayLists.getState() == GameData.State.LOCKED_REMOVED)
					{
						continue;
					}
					this.resolveReference(source, arrayLists.sdata["ref"], ref arrayLists.resolvedRef);
					int referenceCount = arrayLists.getReferenceCount("states");
					if (referenceCount <= 0)
					{
						continue;
					}
					if (arrayLists.resolvedStates == null)
					{
						arrayLists.resolvedStates = new ArrayList();
					}
					while (arrayLists.resolvedStates.Count < referenceCount)
					{
						arrayLists.resolvedStates.Add(null);
					}
					for (int i = 0; i < referenceCount; i++)
					{
						GameData.Item item = arrayLists.resolvedStates[i] as GameData.Item;
						this.resolveReference(source, (arrayLists.references["states"][i] as GameData.Reference).itemID, ref item);
						arrayLists.resolvedStates[i] = item;
					}
				}
			}

			public void revert()
			{
				this.modData.Clear();
				if (this.baseName == null)
				{
					this.setMissingValues();
				}
				else
				{
					this.modName = null;
				}
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					List<GameData.Reference> references = new List<GameData.Reference>();
					foreach (GameData.Reference value in reference.Value)
					{
						if (value.original != null)
						{
							value.mod = null;
						}
						else
						{
							references.Add(value);
						}
					}
					foreach (GameData.Reference reference1 in references)
					{
						this.removeReference(reference.Key, reference1);
					}
				}
				foreach (KeyValuePair<string, ArrayList> keyValuePair in this.removed)
				{
					foreach (GameData.Reference value1 in keyValuePair.Value)
					{
						value1.mod = null;
						this.references[keyValuePair.Key].Add(value1);
					}
				}
				this.removed.Clear();
				List<string> strs = new List<string>();
				foreach (KeyValuePair<string, GameData.Instance> instance in this.instances)
				{
					if (instance.Value.getState() != GameData.State.OWNED)
					{
						instance.Value.revert();
					}
					else
					{
						strs.Add(instance.Key);
					}
				}
				foreach (string str in strs)
				{
					this.instances.Remove(str);
				}
				this.refreshState();
			}

			public void save(BinaryWriter file)
			{
				GameData.Item.ItemLoadFlags itemLoadFlag = (GameData.Item.ItemLoadFlags)0;
				if (this.getState() == GameData.State.MODIFIED || this.getState() == GameData.State.REMOVED)
				{
					itemLoadFlag |= GameData.Item.ItemLoadFlags.MODIFIED;
				}
				if (this.getState() == GameData.State.LOCKED_REMOVED && this.baseName != null && this.hasLocalChanges())
				{
					itemLoadFlag |= GameData.Item.ItemLoadFlags.MODIFIED;
				}
				if (this.modName != null)
				{
					itemLoadFlag |= GameData.Item.ItemLoadFlags.RENAMED;
				}
				file.Write((uint)((int)itemLoadFlag | -2147483648));
				if (this.getState() == GameData.State.REMOVED)
				{
					this.modData["REMOVED"] = true;
				}
				List<KeyValuePair<string, int>> keyValuePairs = new List<KeyValuePair<string, int>>();
				foreach (KeyValuePair<string, object> modDatum in this.modData)
				{
					if (modDatum.Value.GetType().IsEnum || modDatum.Value is int)
					{
						keyValuePairs.Add(new KeyValuePair<string, int>(modDatum.Key, (int)modDatum.Value));
					}
					else if (!(modDatum.Value is Color))
					{
						if (!(modDatum.Value is EnumValue))
						{
							continue;
						}
						keyValuePairs.Add(new KeyValuePair<string, int>(modDatum.Key, (modDatum.Value as EnumValue).Value));
					}
					else
					{
						string key = modDatum.Key;
						Color value = (Color)modDatum.Value;
						keyValuePairs.Add(new KeyValuePair<string, int>(key, value.ToArgb() & 16777215));
					}
				}
				file.Write(GameData.Item.countType<bool>(this.modData));
				foreach (KeyValuePair<string, object> keyValuePair in this.modData)
				{
					if (!(keyValuePair.Value is bool))
					{
						continue;
					}
					GameData.writeString(keyValuePair.Key, file);
					file.Write((bool)keyValuePair.Value);
				}
				file.Write(GameData.Item.countType<float>(this.modData));
				foreach (KeyValuePair<string, object> modDatum1 in this.modData)
				{
					if (!(modDatum1.Value is float))
					{
						continue;
					}
					GameData.writeString(modDatum1.Key, file);
					file.Write((float)modDatum1.Value);
				}
				file.Write(keyValuePairs.Count);
				foreach (KeyValuePair<string, int> keyValuePair1 in keyValuePairs)
				{
					GameData.writeString(keyValuePair1.Key, file);
					file.Write(keyValuePair1.Value);
				}
				file.Write(GameData.Item.countType<GameData.vec>(this.modData));
				foreach (KeyValuePair<string, object> modDatum2 in this.modData)
				{
					if (!(modDatum2.Value is GameData.vec))
					{
						continue;
					}
					GameData.vec _vec = (GameData.vec)modDatum2.Value;
					GameData.writeString(modDatum2.Key, file);
					file.Write(_vec.x);
					file.Write(_vec.y);
					file.Write(_vec.z);
				}
				file.Write(GameData.Item.countType<GameData.quat>(this.modData));
				foreach (KeyValuePair<string, object> keyValuePair2 in this.modData)
				{
					if (!(keyValuePair2.Value is GameData.quat))
					{
						continue;
					}
					GameData.quat _quat = (GameData.quat)keyValuePair2.Value;
					GameData.writeString(keyValuePair2.Key, file);
					file.Write(_quat.x);
					file.Write(_quat.y);
					file.Write(_quat.z);
					file.Write(_quat.w);
				}
				file.Write(GameData.Item.countType<string>(this.modData));
				foreach (KeyValuePair<string, object> modDatum3 in this.modData)
				{
					if (!(modDatum3.Value is string))
					{
						continue;
					}
					GameData.writeString(modDatum3.Key, file);
					GameData.writeString(modDatum3.Value.ToString(), file);
				}
				file.Write(GameData.Item.countType<GameData.File>(this.modData));
				foreach (KeyValuePair<string, object> keyValuePair3 in this.modData)
				{
					if (!(keyValuePair3.Value is GameData.File))
					{
						continue;
					}
					GameData.writeString(keyValuePair3.Key, file);
					GameData.writeString(keyValuePair3.Value.ToString(), file);
				}
				SortedList<string, int> strs = new SortedList<string, int>();
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					int num = this.countModifiedReferences(reference.Key);
					if (num <= 0)
					{
						continue;
					}
					strs.Add(reference.Key, num);
				}
				file.Write(strs.Count);
				foreach (KeyValuePair<string, int> str in strs)
				{
					GameData.writeString(str.Key, file);
					file.Write(str.Value);
					int num1 = 0;
					foreach (GameData.Reference item in this.references[str.Key])
					{
						if (item.mod == null)
						{
							continue;
						}
						GameData.writeString(item.itemID, file);
						file.Write(item.mod.v0);
						file.Write(item.mod.v1);
						file.Write(item.mod.v2);
						num1++;
					}
					if (!this.removed.ContainsKey(str.Key))
					{
						continue;
					}
					foreach (GameData.Reference item1 in this.removed[str.Key])
					{
						if (item1.mod == null)
						{
							continue;
						}
						GameData.writeString(item1.itemID, file);
						file.Write(2147483647);
						file.Write(2147483647);
						file.Write(2147483647);
						num1++;
					}
				}
				List<GameData.Instance> instances = new List<GameData.Instance>();
				foreach (KeyValuePair<string, GameData.Instance> instance in this.instances)
				{
					if (instance.Value.getState() != GameData.State.MODIFIED && instance.Value.getState() != GameData.State.OWNED && instance.Value.getState() != GameData.State.REMOVED)
					{
						continue;
					}
					instance.Value.stringID = instance.Key;
					instances.Add(instance.Value);
				}
				file.Write(instances.Count);
				foreach (GameData.Instance instance1 in instances)
				{
					GameData.writeString(instance1.stringID, file);
					GameData.writeString((instance1.getState() == GameData.State.REMOVED ? "" : instance1.sdata["ref"]), file);
					file.Write(instance1.fdata["x"]);
					file.Write(instance1.fdata["y"]);
					file.Write(instance1.fdata["z"]);
					file.Write(instance1.fdata["qw"]);
					file.Write(instance1.fdata["qx"]);
					file.Write(instance1.fdata["qy"]);
					file.Write(instance1.fdata["qz"]);
					file.Write(instance1.getReferenceCount("states"));
					foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair4 in instance1.referenceData("states", false))
					{
						GameData.writeString(keyValuePair4.Key, file);
					}
				}
			}

			public void setLocked()
			{
				if (this.cachedState == GameData.State.REMOVED)
				{
					this.cachedState = GameData.State.LOCKED_REMOVED;
					return;
				}
				this.cachedState = GameData.State.LOCKED;
			}

			public int setMissingValues()
			{
				int num = 0;
				if (GameData.desc.ContainsKey(this.type))
				{
					foreach (KeyValuePair<string, GameData.Desc> item in GameData.desc[this.type])
					{
						if (item.Value.defaultValue is GameData.TripleInt || item.Value.defaultValue is GameData.Instance || item.Value.defaultValue == null || this.ContainsKey(item.Key))
						{
							continue;
						}
						this[item.Key] = item.Value.defaultValue;
						num++;
					}
				}
				if (GameData.desc.ContainsKey(this.type))
				{
					foreach (KeyValuePair<string, GameData.Desc> str in GameData.desc[this.type])
					{
						if (str.Value.defaultValue is GameData.TripleInt || str.Value.defaultValue is GameData.Instance || str.Value.defaultValue == null || !(this[str.Key].GetType() != str.Value.defaultValue.GetType()) || this[str.Key] is int && (str.Value.defaultValue.GetType().IsEnum || str.Value.defaultValue is Color))
						{
							continue;
						}
						object value = str.Value.defaultValue;
						object bah = this[str.Key];
						if (bah is bool)
						{
							bah = ((bool)bah ? 1 : 0);
						}
						if (value is string)
						{
							this[str.Key] = bah.ToString();
						}
						else if (value is float && bah.GetType().IsValueType)
						{
							this[str.Key] = (float)((int)bah);
						}
						else if (value is int || value is Color || value.GetType().IsEnum && bah is float)
						{
							this[str.Key] = (int)((float)bah);
						}
						else
						{
							if (value is EnumValue && bah is int)
							{
								continue;
							}
							this[str.Key] = value;
						}
						if (this[str.Key] != value)
						{
							Errors.addError(Error.WARNING, this, null, string.Concat("Value ", str.Key, " was the wrong type"));
						}
						else
						{
							Errors.addError(Error.CRITICAL, this, null, string.Concat(new object[] { "Value ", str.Key, " (", bah, ") is the wrong type and could not be converted" }));
						}
					}
				}
				if (num > 0)
				{
					this.nav.HasChanges = true;
				}
				return num;
			}

			public void setReferenceValue(string section, string id, int? v0, int? v1 = null, int? v2 = null)
			{
				GameData.Reference reference = this.getReference(section, id);
				if (reference.mod == null)
				{
					reference.mod = (reference.original == null ? new GameData.TripleInt(0, 0, 0) : new GameData.TripleInt(reference.original));
				}
				GameData.TripleInt tripleInt = reference.mod;
				int? nullable = v0;
				tripleInt.v0 = (nullable.HasValue ? nullable.GetValueOrDefault() : 0);
				GameData.TripleInt tripleInt1 = reference.mod;
				nullable = v1;
				tripleInt1.v1 = (nullable.HasValue ? nullable.GetValueOrDefault() : 0);
				GameData.TripleInt tripleInt2 = reference.mod;
				nullable = v2;
				tripleInt2.v2 = (nullable.HasValue ? nullable.GetValueOrDefault() : 0);
				if (reference.original != null && reference.original.Equals(reference.mod))
				{
					reference.mod = null;
				}
				this.refreshState();
			}

			public void setReferenceValue(string section, string id, GameData.TripleInt value)
			{
				this.setReferenceValue(section, id, new int?(value.v0), new int?(value.v1), new int?(value.v2));
			}

			private void setupAccessors()
			{
				this.idata = new GameData.Item.Accessor<int>(this);
				this.fdata = new GameData.Item.Accessor<float>(this);
				this.bdata = new GameData.Item.Accessor<bool>(this);
				this.sdata = new GameData.Item.Accessor<string>(this);
				this.filesdata = new GameData.Item.Accessor<GameData.File>(this);
			}

			private bool tagged(Dictionary<string, bool> tags, string key)
			{
				if (tags == null)
				{
					return true;
				}
				if (!tags.ContainsKey(key))
				{
					return false;
				}
				return tags[key];
			}

			public override string ToString()
			{
				itemType _itemType = this.type;
				return string.Concat(_itemType.ToString(), ": ", this.Name);
			}

			public bool validateReferenceTypes()
			{
				if (!GameData.desc.ContainsKey(this.type))
				{
					return true;
				}
				bool flag = true;
				SortedList<string, GameData.Desc> item = GameData.desc[this.type];
				foreach (KeyValuePair<string, ArrayList> reference in this.references)
				{
					itemType _itemType = item[reference.Key].list;
					foreach (object value in reference.Value)
					{
						if (((GameData.Reference)value).item.type == _itemType)
						{
							continue;
						}
						Errors.addError(Error.WARNING, this, null, string.Concat("Reference to incorrect item type in ", reference.Key));
						flag = false;
					}
				}
				return flag;
			}

			public class Accessor<T>
			{
				private GameData.Item item;

				public T this[string s]
				{
					get
					{
						return (T)this.item[s];
					}
					set
					{
						this.item[s] = value;
					}
				}

				public Accessor(GameData.Item me)
				{
					this.item = me;
				}

				public bool ContainsKey(string key)
				{
					if (!this.item.ContainsKey(key))
					{
						return false;
					}
					return this.item[key] is T;
				}

				public void Remove(string key)
				{
					if (this.ContainsKey(key))
					{
						this.item.Remove(key);
					}
				}
			}

			private enum ItemLoadFlags
			{
				MODIFIED = 1,
				RENAMED = 2
			}
		}

		public enum ModMode
		{
			BASE,
			ACTIVE,
			LOCKED
		}

		public class quat
		{
            public float w = 1f;

            public float x;

			public float y;

			public float z;

			public quat()
			{
			}

			public void @set(float qw, float qx, float qy, float qz)
			{
				this.w = qw;
				this.x = qx;
				this.y = qy;
				this.z = qz;
			}

			public void @set(GameData.quat v)
			{
				this.w = v.w;
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			public override string ToString()
			{
				return string.Format("{0} {1} {2} {3}", new object[] { this.w, this.x, this.y, this.z });
			}
		}

		public class Reference
		{
            private string sID = "";

            public GameData.Item item;

			public GameData.TripleInt original;

			public GameData.TripleInt mod;

			public GameData.TripleInt locked;

			public static GameData.TripleInt Removed;

			public string itemID
			{
				get
				{
					if (this.item == null)
					{
						return this.sID;
					}
					return this.item.stringID;
				}
			}

			public GameData.TripleInt Values
			{
				get
				{
					if (this.locked != null)
					{
						return this.locked;
					}
					if (this.mod == null)
					{
						return this.original;
					}
					return this.mod;
				}
			}

			static Reference()
			{
				GameData.Reference.Removed = new GameData.TripleInt(2147483647, 2147483647, 2147483647);
			}

			public Reference(string id, GameData.TripleInt value = null)
			{
				this.sID = id;
				this.original = value;
			}

			public Reference(GameData.Item item, GameData.TripleInt value = null)
			{
				this.item = item;
				this.original = value;
			}
		}

		public enum State
		{
			UNKNOWN,
			INVALID,
			ORIGINAL,
			OWNED,
			MODIFIED,
			LOCKED,
			REMOVED,
			LOCKED_REMOVED
		}

		public class TripleInt
		{
			public int v0;

			public int v1;

			public int v2;

			public TripleInt(GameData.TripleInt v)
			{
				this.v0 = v.v0;
				this.v1 = v.v1;
				this.v2 = v.v2;
			}

			public TripleInt(int i0 = 0, int i1 = 0, int i2 = 0)
			{
				this.v0 = i0;
				this.v1 = i1;
				this.v2 = i2;
			}

			public bool Equals(GameData.TripleInt b)
			{
				if (b == null || this.v0 != b.v0 || this.v1 != b.v1)
				{
					return false;
				}
				return this.v2 == b.v2;
			}
		}

		public class vec
		{
			public float x;

			public float y;

			public float z;

			public vec()
			{
			}

			public void @set(float a, float b, float c)
			{
				this.x = a;
				this.y = b;
				this.z = c;
			}

			public void @set(GameData.vec v)
			{
				this.x = v.x;
				this.y = v.y;
				this.z = v.z;
			}

			public override string ToString()
			{
				return string.Format("{0} {1} {2}", this.x, this.y, this.z);
			}
		}
	}
}