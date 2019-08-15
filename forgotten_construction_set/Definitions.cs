using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	internal class Definitions
	{
		private static int lineNumber;

		private static string errors;

		public Definitions()
		{
		}

		private static void error(string e)
		{
			string str = string.Concat(new object[] { "line ", Definitions.lineNumber, ": ", e });
			baseForm.logger.Error(str);
			Definitions.errors = string.Concat(Definitions.errors, str, "\n");
		}

		public static void export(string file)
		{
			SortedList<string, SortedList<string, List<string>>> strs = new SortedList<string, SortedList<string, List<string>>>();
			foreach (KeyValuePair<itemType, SortedList<string, GameData.Desc>> keyValuePair in GameData.desc)
			{
				SortedList<string, List<string>> strs1 = new SortedList<string, List<string>>();
				strs.Add(keyValuePair.Key.ToString(), strs1);
				foreach (KeyValuePair<string, GameData.Desc> value in keyValuePair.Value)
				{
					string str = value.Value.category;
					string str1 = string.Concat(value.Key, ": ");
					while (str1.Length < 22)
					{
						str1 = string.Concat(str1, " ");
					}
					if (value.Value.defaultValue is string)
					{
						str1 = string.Concat(str1, "\"", value.Value.defaultValue.ToString(), "\" ");
						if ((value.Value.flags & 16) > 0)
						{
							str1 = string.Concat(str1, "multiline ");
						}
					}
					else if (value.Value.defaultValue is GameData.TripleInt)
					{
						GameData.TripleInt tripleInt = value.Value.defaultValue as GameData.TripleInt;
						str1 = string.Concat(str1, value.Value.list.ToString(), " ");
						if (value.Value.flags == 1)
						{
							str1 = string.Concat(new object[] { str1, "(", tripleInt.v0, ") " });
						}
						else if (value.Value.flags == 2)
						{
							str1 = string.Concat(new object[] { str1, "(", tripleInt.v0, ", ", tripleInt.v1, ") " });
						}
						else if (value.Value.flags == 3)
						{
							str1 = string.Concat(new object[] { str1, "(", tripleInt.v0, ", ", tripleInt.v1, ",", tripleInt.v2, ") " });
						}
						str = "";
					}
					else if (value.Value.defaultValue is GameData.Instance)
					{
						str1 = string.Concat(str1, value.Value.list.ToString(), " (0,0,0) (1,0,0,0) ");
						str = "";
					}
					else if (value.Value.defaultValue is Color)
					{
						Color color = (Color)value.Value.defaultValue;
						int argb = color.ToArgb() & 16777215;
						str1 = string.Concat(str1, "#", argb.ToString("X").PadLeft(6, '0'), " ");
					}
					else if (value.Value.defaultValue is GameData.File)
					{
						if (!value.Value.mask.EndsWith("*.dds"))
						{
							str1 = (value.Value.mask != "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp" ? string.Concat(str1, "\"", value.Value.mask, "\" ") : string.Concat(str1, "TEXTURE_ANY "));
						}
						else
						{
							str1 = string.Concat(str1, "TEXTURE_DDS ");
						}
					}
					else if (value.Value.defaultValue == null)
					{
						str1 = string.Concat(str1, "null ");
					}
					else if (value.Value.defaultValue.GetType().IsEnum)
					{
						str1 = string.Concat(new string[] { str1, value.Value.defaultValue.GetType().Name, ".", value.Value.defaultValue.ToString(), " " });
						if ((value.Value.flags & 256) > 0)
						{
							str1 = string.Concat(str1, "bitset ");
						}
					}
					else if (value.Value.defaultValue is float || value.Value.defaultValue is double)
					{
						string str2 = value.Value.defaultValue.ToString();
						if (!str2.Contains("."))
						{
							str2 = string.Concat(str2, ".0");
						}
						str1 = string.Concat(str1, str2, " ");
					}
					else
					{
						str1 = string.Concat(str1, value.Value.defaultValue.ToString(), " ");
					}
					if (str.Length > 0 && value.Value.limit == 9999)
					{
						str1 = string.Concat(str1, "looped ");
					}
					if (value.Value.description.Length > 0)
					{
						str1 = string.Concat(str1, "\"", value.Value.description.Replace("\n", "\\n"), "\"");
					}
					if (!strs1.ContainsKey(str))
					{
						strs1.Add(str, new List<string>());
					}
					strs1[str].Add(str1);
					if (value.Value.condition == null)
					{
						continue;
					}
					GameData.DescCondition descCondition = value.Value.condition;
					string str3 = "";
					if (!descCondition.values.GetType().IsArray)
					{
						if (descCondition.values.GetType().IsEnum)
						{
							str3 = string.Concat(descCondition.values.GetType().Name, ".");
						}
						str3 = string.Concat(str3, descCondition.values.ToString());
					}
					else
					{
						List<string> strs2 = new List<string>();
						foreach (object obj in (IEnumerable)descCondition.values)
						{
							strs2.Add(string.Concat((obj.GetType().IsEnum ? string.Concat(obj.GetType().Name, ".") : ""), obj.ToString()));
						}
						str3 = string.Join(" or ", strs2);
					}
					if (descCondition.values.GetType().IsEnum)
					{
						string.Concat(descCondition.values.GetType().Name, ".");
					}
					if (!strs1.ContainsKey("CONDITIONS"))
					{
						strs1.Add("CONDITIONS", new List<string>());
					}
					List<string> item = strs1["CONDITIONS"];
					string[] key = new string[] { "condition \"", value.Key, "\" if \"", descCondition.key, "\"", null, null };
					key[5] = (descCondition.match ? " is " : " not ");
					key[6] = str3;
					item.Add(string.Concat(key));
				}
			}
			bool flag = true;
		Label0:
			while (flag)
			{
				flag = false;
				foreach (KeyValuePair<string, SortedList<string, List<string>>> keyValuePair1 in strs)
				{
					foreach (KeyValuePair<string, List<string>> value1 in keyValuePair1.Value)
					{
						foreach (string value2 in value1.Value)
						{
							foreach (KeyValuePair<string, SortedList<string, List<string>>> keyValuePair2 in strs)
							{
								if (!(keyValuePair1.Key != keyValuePair2.Key) || !keyValuePair2.Value.ContainsKey(value1.Key) || !keyValuePair2.Value[value1.Key].Contains(value2))
								{
									continue;
								}
								string str4 = string.Concat(keyValuePair1.Key, ",", keyValuePair2.Key);
								if (!strs.ContainsKey(str4))
								{
									strs.Add(str4, new SortedList<string, List<string>>());
								}
								if (!strs[str4].ContainsKey(value1.Key))
								{
									strs[str4].Add(value1.Key, new List<string>());
								}
								strs[str4][value1.Key].Add(value2);
								keyValuePair2.Value[value1.Key].Remove(value2);
								value1.Value.Remove(value2);
								if (keyValuePair2.Value[value1.Key].Count == 0)
								{
									keyValuePair2.Value.Remove(value1.Key);
								}
								if (value1.Value.Count == 0)
								{
									keyValuePair1.Value.Remove(value1.Key);
								}
								flag = true;
								goto Label3;
							}
						Label3:
							if (!flag)
							{
								continue;
							}
							goto Label2;
						}
					Label2:
						if (!flag)
						{
							continue;
						}
						goto Label1;
					}
				Label1:
					if (!flag)
					{
						continue;
					}
					goto Label0;
				}
			}
			string str5 = "";
			foreach (KeyValuePair<string, SortedList<string, List<string>>> keyValuePair3 in strs)
			{
				if (keyValuePair3.Value.Count == 0)
				{
					continue;
				}
				str5 = string.Concat(str5, "[", keyValuePair3.Key, "]\n");
				foreach (KeyValuePair<string, List<string>> value3 in keyValuePair3.Value)
				{
					if (value3.Key == "CONDITIONS")
					{
						continue;
					}
					if (value3.Key.Length > 0 && value3.Value.Count > 0)
					{
						str5 = string.Concat(str5, value3.Key, ":\n");
					}
					foreach (string value4 in value3.Value)
					{
						str5 = string.Concat(str5, value4, "\n");
					}
				}
				if (!keyValuePair3.Value.ContainsKey("CONDITIONS") || keyValuePair3.Value["CONDITIONS"].Count <= 0)
				{
					continue;
				}
				str5 = string.Concat(str5, "CONDITIONS:\n");
				foreach (string item1 in keyValuePair3.Value["CONDITIONS"])
				{
					str5 = string.Concat(str5, item1, "\n");
				}
			}
			FileStream fileStream = File.Open(file, FileMode.Create);
			byte[] bytes = Encoding.ASCII.GetBytes(str5);
			fileStream.Write(bytes, 0, (int)bytes.Length);
			fileStream.Close();
		}

		public static bool load(string filename, navigation nav)
		{
			itemType _itemType;
			Definitions.lineNumber = 0;
			Definitions.errors = "";
			if (!File.Exists(filename))
			{
				return false;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				File.OpenRead(filename).CopyTo(memoryStream);
				memoryStream.Position = (long)0;
				using (StreamReader streamReader = new StreamReader(memoryStream))
				{
					while (!streamReader.EndOfStream)
					{
						string str = Definitions.readLine(streamReader);
						if (!str.StartsWith("["))
						{
							if (!str.StartsWith("enum"))
							{
								continue;
							}
							Definitions.parseEnum(streamReader, str);
						}
						else
						{
							string str1 = str.Substring(1, str.IndexOf(']') - 1);
							if (str1 != "FCS_LAYOUT")
							{
								List<itemType> itemTypes = new List<itemType>();
								string[] strArrays = str1.Split(new char[] { ',' });
								for (int i = 0; i < (int)strArrays.Length; i++)
								{
									string str2 = strArrays[i];
									if (!Enum.TryParse<itemType>(str2, out _itemType))
									{
										Definitions.error(string.Concat("Invalid item type ", str2));
									}
									else
									{
										itemTypes.Add(_itemType);
									}
								}
								if (itemTypes.Count <= 0)
								{
									continue;
								}
								Definitions.parseItem(streamReader, itemTypes);
							}
							else
							{
                                //TODO 不需要外部翻译
								//Definitions.parseLayout(streamReader, nav);
							}
						}
					}
				}
			}
			if (Definitions.errors.Length > 0)
			{
				MessageBox.Show(Definitions.errors, string.Concat(filename, " 错误"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return true;
		}

		private static bool parseCondition(string line, List<itemType> types)
		{
			if (!line.StartsWith("condition"))
			{
				return false;
			}
			List<string> strs = Definitions.tokenise(line, 999);
			if (strs.Count < 6 || strs[2].ToLower() != "if" || strs[4].ToLower() != "is" && strs[4].ToLower() != "not")
			{
				return false;
			}
			string str = Definitions.stripQuotes(strs[1]);
			string str1 = Definitions.stripQuotes(strs[3]);
			bool lower = strs[4].ToLower() == "is";
			object array = null;
			List<object> objs = new List<object>();
			int num = 5;
			while (num < strs.Count)
			{
				object obj = Definitions.parseValue(strs[num]);
				if (obj != null)
				{
					objs.Add(obj);
				}
				else
				{
					Definitions.error(string.Concat("Condition has an invalid value: ", strs[num]));
				}
				if (num >= strs.Count - 1 || !(strs[num + 1] != "or"))
				{
					num += 2;
				}
				else
				{
					Definitions.error("Expected or");
					break;
				}
			}
			if (objs.Count == 0)
			{
				return true;
			}
			if (objs.Count != 1)
			{
				array = objs.ToArray();
			}
			else
			{
				array = objs[0];
			}
			foreach (itemType type in types)
			{
				if (GameData.addCondition(type, str, str1, array, lower))
				{
					continue;
				}
				Definitions.error(string.Concat("Item ", str, " not yet defined"));
			}
			return true;
		}

		private static bool parseEnum(StreamReader r, string firstLine)
		{
			string i;
			string str = null;
			if (!firstLine.StartsWith("enum"))
			{
				return false;
			}
			List<string> strs = Definitions.tokenise(firstLine, 3);
			if (strs[0] != "enum" || strs.Count > 2 && strs[2] != "{")
			{
				return false;
			}
			for (i = firstLine; !r.EndOfStream && !i.Contains<char>('}'); i = string.Concat(i, " ", str.Trim()))
			{
				str = Definitions.readLine(r);
				int num = str.IndexOf("//");
				if (num >= 0)
				{
					str = str.Remove(num);
				}
			}
			List<string> strs1 = Definitions.tokenise(i, 999);
			if (strs1[0] != "enum")
			{
				return false;
			}
			string item = strs1[1];
			if (strs1[2] != "{")
			{
				return false;
			}
			FCSEnum fCSEnums = new FCSEnum();
			int num1 = 3;
			while (num1 < strs1.Count)
			{
                string tansedStr = "";
                string item1 = strs1[num1];
				if (item1 != ",")
				{
					if (item1 == "}")
					{
						break;
					}
					int max = fCSEnums.Max;
					if (strs1[num1 + 1] != "=")
					{
						num1++;
					}
					else
					{
						max = int.Parse(strs1[num1 + 2]);
						num1 += 3;
                    }
                    if (!(bool)NativeTranslte.enumDict.TryGetValue(item1, out tansedStr))
                    {
                        tansedStr = item1;
                    }
                    // Console.Write(tansedStr, max);
                    try
                    {
                        fCSEnums.addValue(tansedStr, max);
                    }
                    catch (Exception exception)
                    {
                        fCSEnums.addValue(item1, max);
                    }

                    //  fCSEnums.addValue(item1, max);
                }
                else
				{
					num1++;
				}
			}
			FCSEnums.types.Add(item, fCSEnums);
			return true;
		}

		private static void parseItem(StreamReader r, List<itemType> types)
		{
			itemType _itemType;
			float[] singleArray;
			float[] singleArray1;
			string str = "General";
			while (!r.EndOfStream && r.Peek() != 91)
			{
				string str1 = Definitions.readLine(r);
				int num = str1.IndexOf("//");
				if (num >= 0)
				{
					str1 = str1.Remove(num);
				}
				str1 = str1.Trim();
				if (str1.Length == 0 || Definitions.parseEnum(r, str1) || Definitions.parseCondition(str1, types))
				{
					continue;
				}
				string[] strArrays = str1.Split(new char[] { ':' }, 2);
				if ((int)strArrays.Length != 2)
				{
					Definitions.error(string.Concat("Invalid line ", str1));
				}
				else if (strArrays[1].Length != 0)
				{
					GameData.Desc desc = new GameData.Desc();
					List<string> strs = Definitions.tokenise(strArrays[1], 999);
					string str2 = strArrays[0].Trim();
					if (strs.Count > 1 && strs[strs.Count - 1][0] == '\"')
					{
						desc.description = Definitions.stripQuotes(strs[strs.Count - 1]).Replace("\\n", "\n");
					}
					desc.category = str;
					if (strs[0] == "null")
					{
						desc.defaultValue = null;
					}
					else if (strs[0] == "TEXTURE_DDS")
					{
						desc.defaultValue = new GameData.File("");
						desc.mask = "Nvidia dds| *.dds";
					}
					else if (strs[0] == "TEXTURE_ANY")
					{
						desc.defaultValue = new GameData.File("");
						desc.mask = "Nvidia dds|*.dds|tga|*.tga|jpg|*.jpg|png|*.png|bmp|*.bmp";
					}
					else if (strs[0].Contains("*.") || strs[0].Contains("|"))
					{
						desc.defaultValue = new GameData.File("");
						desc.mask = Definitions.stripQuotes(strs[0]);
					}
					else if (strs[0] != "multiline")
					{
						object obj = Definitions.parseValue(strs[0]);
						object obj1 = obj;
						desc.defaultValue = obj;
						if (obj1 == null)
						{
							if (!Enum.TryParse<itemType>(strs[0], out _itemType))
							{
								Definitions.error(string.Concat("Invalid type ", strs[0]));
								continue;
							}
							else
							{
								if (strs.Count > 1)
								{
									singleArray = Definitions.parseVector(strs[1]);
								}
								else
								{
									singleArray = null;
								}
								float[] singleArray2 = singleArray;
								if (strs.Count > 2)
								{
									singleArray1 = Definitions.parseVector(strs[2]);
								}
								else
								{
									singleArray1 = null;
								}
								float[] singleArray3 = singleArray1;
								if (singleArray2 == null || singleArray3 == null)
								{
									GameData.TripleInt tripleInt = new GameData.TripleInt(0, 0, 0);
									if (singleArray2 != null)
									{
										if (singleArray2.Length != 0)
										{
											tripleInt.v0 = (int)singleArray2[0];
										}
										if ((int)singleArray2.Length > 1)
										{
											tripleInt.v1 = (int)singleArray2[1];
										}
										if ((int)singleArray2.Length > 2)
										{
											tripleInt.v2 = (int)singleArray2[2];
										}
										desc.flags = (int)singleArray2.Length;
									}
									desc.list = _itemType;
									desc.defaultValue = tripleInt;
								}
								else if ((int)singleArray2.Length != 3 || (int)singleArray3.Length != 4)
								{
									Definitions.error("Invalid instance values");
									continue;
								}
								else
								{
									GameData.Instance instance = new GameData.Instance();
									instance["x"] = singleArray2[0];
									instance["y"] = singleArray2[1];
									instance["z"] = singleArray2[2];
									instance["qx"] = singleArray3[0];
									instance["qy"] = singleArray3[1];
									instance["qz"] = singleArray3[2];
									instance["qw"] = singleArray3[3];
									desc.list = _itemType;
									desc.defaultValue = instance;
								}
							}
						}
					}
					else
					{
						desc.defaultValue = "";
					}
					if (desc.defaultValue is string && strs.Contains("multiline"))
					{
						desc.flags |= 16;
					}
					if ((desc.defaultValue is EnumValue || desc.GetType().IsEnum) && strs.Contains("bitset"))
					{
						desc.flags |= 256;
					}
					if (strs.Contains("looped"))
					{
						desc.limit = 9999;
					}
					foreach (itemType type in types)
					{
						GameData.Desc desc1 = new GameData.Desc()
						{
							category = desc.category,
							condition = GameData.getDesc(type, str2).condition,
							defaultValue = desc.defaultValue,
							description = desc.description,
							flags = desc.flags,
							limit = desc.limit,
							list = desc.list,
							mask = desc.mask
						};
						GameData.setDesc(type, str2, desc1);
					}
				}
				else
				{
					str = strArrays[0].Trim();
				}
			}
		}

		private static void parseLayout(StreamReader r, navigation nav)
		{
			nav.clearCategories();
			Stack<Tuple<string, int>> tuples = new Stack<Tuple<string, int>>();
			while (!r.EndOfStream && r.Peek() != 91)
			{
				string str = Definitions.readLine(r);
				int num = str.IndexOf("//");
				if (num >= 0)
				{
					str.Remove(num);
				}
				int length = str.TrimStart(new char[0]).Length - str.Length;
				if (string.IsNullOrWhiteSpace(str))
				{
					continue;
				}
				string[] strArrays = str.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
				string str1 = strArrays[0].Trim();
				itemType _itemType = itemType.NULL_ITEM;
				string str2 = strArrays[1].TrimStart(new char[0]);
				int num1 = str2.IndexOfAny(new char[] { ' ', '\t', '\"', ',', ';', '/', '#' });
				if (num1 > 0)
				{
					str2 = str2.Remove(num1);
				}
				Enum.TryParse<itemType>(str2, out _itemType);
				string str3 = null;
				int num2 = strArrays[1].IndexOf('\"') + 1;
				if (num2 > 0)
				{
					int num3 = strArrays[1].IndexOf('\"', num2);
					str3 = strArrays[1].Substring(num2, num3 - num2);
				}
				Tuple<string, int> tuple = new Tuple<string, int>(str1, length);
				while (tuples.Count > 0 && tuples.Peek().Item2 <= length)
				{
					tuples.Pop();
				}
				if (tuples.Count != 0)
				{
					nav.AddCategory(tuples.Peek().Item1, str1, new navigation.Filter(_itemType, str3));
				}
				else
				{
					nav.AddCategory(str1, new navigation.Filter(_itemType, str3));
				}
				tuples.Push(new Tuple<string, int>(str1, length));
			}
		}

		private static object parseValue(string s)
		{
			int num;
			float single;
			FCSEnum fCSEnums;
			object obj;
			if (s.ToLower() == "true")
			{
				return true;
			}
			if (s.ToLower() == "false")
			{
				return false;
			}
			if (s.StartsWith("\""))
			{
				return Definitions.stripQuotes(s);
			}
			if (s.IndexOf(".") > 0 && float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out single))
			{
				return single;
			}
			if (int.TryParse(s, out num))
			{
				return num;
			}
			if (s.StartsWith("#"))
			{
				try
				{
					num = Convert.ToInt32(s.Substring(1), 16);
				}
				catch (Exception exception)
				{
					num = 0;
					Definitions.error(string.Concat(s, " is not a valid colour"));
				}
				return Color.FromArgb(255, Color.FromArgb(num));
			}
			if (s.Contains(".") && (s[0] >= 'A' && s[0] <= 'Z' || s[0] >= 'a' && s[0] <= 'z' || s[0] == '\u005F'))
			{
				string[] strArrays = s.Split(new char[] { '.' });
				if (FCSEnums.types.TryGetValue(strArrays[0], out fCSEnums))
				{
					return new EnumValue(fCSEnums, strArrays[1]);
				}
				Type type = Type.GetType(string.Concat("forgotten_construction_set.", strArrays[0]), false);
				if (!(type != null) || !type.IsEnum)
				{
					Definitions.error(string.Concat("Enum ", strArrays[0], " not found"));
				}
				else
				{
					try
					{
						obj = Enum.Parse(type, strArrays[1]);
					}
					catch (Exception exception1)
					{
						Definitions.error(string.Concat("Enum ", strArrays[0], " does not contain ", strArrays[1]));
						return null;
					}
					return obj;
				}
			}
			return null;
		}

		private static float[] parseVector(string s)
		{
			if (s.Length == 0 || s[0] != '(' || s[s.Length - 1] != ')')
			{
				return null;
			}
			string[] strArrays = s.Substring(1, s.Length - 2).Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			float[] singleArray = new float[(int)strArrays.Length];
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				float.TryParse(strArrays[i], NumberStyles.Any, CultureInfo.InvariantCulture, out singleArray[i]);
			}
			return singleArray;
		}

		private static string readLine(StreamReader r)
		{
			Definitions.lineNumber++;
			return r.ReadLine();
		}

		private static string readToken(string s, int start, char end)
		{
			int length = s.IndexOf(end, start);
			if (length < 0)
			{
				length = s.Length;
			}
			return s.Substring(start, length - start);
		}

		private static string stripQuotes(string s)
		{
			if (s[0] != '\"' || s[s.Length - 1] != '\"')
			{
				return s;
			}
			return s.Substring(1, s.Length - 2);
		}

		private static List<string> tokenise(string s, int limit = 999)
		{
			List<string> strs = new List<string>();
			int length = 0;
			int num = 0;
			while (length + num < s.Length)
			{
				if (strs.Count >= limit)
				{
					return strs;
				}
				char chr = s[length + num];
				if ((chr < '0' || chr > '9') && chr != '.' && chr != '-' && (chr < 'A' || chr > 'Z') && (chr < 'a' || chr > 'z') && chr != '\u005F' && chr != '#')
				{
					if (num > 0)
					{
						strs.Add(s.Substring(length, num));
					}
					length += num;
					num = 0;
					if (chr == ' ' || chr == '\t')
					{
						length++;
					}
					else if (chr == '\"' || chr == '\'')
					{
						string str = Definitions.readToken(s, length + 1, chr);
						length = length + str.Length + 2;
						strs.Add(string.Concat(chr.ToString(), str, chr.ToString()));
					}
					else if (chr != '(')
					{
						strs.Add(s.Substring(length, 1));
						length++;
					}
					else
					{
						string str1 = Definitions.readToken(s, length + 1, ')');
						length = length + str1.Length + 2;
						strs.Add(string.Concat("(", str1, ")"));
					}
				}
				else
				{
					num++;
				}
			}
			if (num > 0)
			{
				strs.Add(s.Substring(length, num));
			}
			return strs;
		}
	}
}