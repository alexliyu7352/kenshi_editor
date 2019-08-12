using NLog;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace forgotten_construction_set
{
    public static class TranslationManager
    {
        private readonly static ParallelOptions ParallelOpts;

        private static List<string> StringFields;

        private readonly static string WordSwapRegex;

        private readonly static string ImportFileExtension;

        private readonly static string ExportFileExtension;

        private readonly static string BaseGameDataFile;

        private readonly static string WordSwapsFile;

        private readonly static string TranslationsFolderName;

        private static string RootDir;

        public static List<TranslationManager.TranslationDialogue> EnabledDialogues;

        public static List<TranslationManager.TranslationDialogue> EnabledWordSwaps;

        public static ConcurrentDictionary<GameData.Item, TranslationManager.TranslationDialogueLine> DialogueLines;

        public static List<List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>>> LinkedLines;

        public static List<GameData.Item> Missing;

        public static TranslationManager.TranslationCulture ActiveTranslation
        {
            get;
            private set;
        }

        public static string ActiveTranslationMod
        {
            get;
            private set;
        }

        public static List<TranslationManager.TranslationCulture> AvaliableTranslations
        {
            get;
            private set;
        }

        public static bool TranslationMode
        {
            get
            {
                return TranslationManager.ActiveTranslation != null;
            }
        }

        static TranslationManager()
        {
            TranslationManager.ParallelOpts = new ParallelOptions();
            TranslationManager.StringFields = new List<string>();
            TranslationManager.WordSwapRegex = "(/.+?/)";
            TranslationManager.ImportFileExtension = ".po";
            TranslationManager.ExportFileExtension = ".pot";
            TranslationManager.BaseGameDataFile = "gamedata";
            TranslationManager.WordSwapsFile = "_word_swaps";
            TranslationManager.TranslationsFolderName = "__translations";
            TranslationManager.EnabledDialogues = new List<TranslationManager.TranslationDialogue>();
            TranslationManager.EnabledWordSwaps = new List<TranslationManager.TranslationDialogue>();
            TranslationManager.DialogueLines = new ConcurrentDictionary<GameData.Item, TranslationManager.TranslationDialogueLine>();
            TranslationManager.LinkedLines = new List<List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>>>();
            TranslationManager.Missing = new List<GameData.Item>();
            TranslationManager.ParallelOpts.MaxDegreeOfParallelism = 1;
            TranslationManager.RootDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), TranslationManager.TranslationsFolderName);
            Directory.CreateDirectory(TranslationManager.RootDir);
            TranslationManager.StringFields.Add("Description");
            TranslationManager.StringFields.Add("description");
            TranslationManager.StringFields.Add("building category");
            TranslationManager.StringFields.Add("building group");
            TranslationManager.StringFields.Add("category");
            TranslationManager.StringFields.Add("inventory content name");
            TranslationManager.StringFields.Add("building name");
            TranslationManager.StringFields.Add("string");
            TranslationManager.StringFields.Add("company description");
            TranslationManager.StringFields.Add("unexplored name");
            TranslationManager.StringFields.Add("display name");
            TranslationManager.StringFields.Add("difficulty");
            TranslationManager.StringFields.Add("style");
            TranslationManager.ActiveTranslation = null;
            TranslationManager.AvaliableTranslations = new List<TranslationManager.TranslationCulture>();
        }

        private static void AddOrCreateCoreMessage(Dictionary<string, TranslationManager.TranslationMessage> messages, string id, GameData.Item item, string key = "")
        {
            TranslationManager.TranslationMessage translationMessage = null;
            if (!messages.TryGetValue(id, out translationMessage))
            {
                translationMessage = new TranslationManager.TranslationMessage(item, id);
                messages.Add(id, translationMessage);
            }
            if (!string.IsNullOrWhiteSpace(key))
            {
                translationMessage.references.Add(string.Concat(item.stringID, ":", TranslationManager.StringFields.IndexOf(key)));
                if (string.IsNullOrWhiteSpace(translationMessage.translated) && item.sdata[key] != item.OriginalValue(key) as string)
                {
                    translationMessage.translated = item.sdata[key];
                }
            }
            else
            {
                translationMessage.references.Add(item.stringID);
                if (string.IsNullOrWhiteSpace(translationMessage.translated) && item.Name != item.OriginalName)
                {
                    translationMessage.translated = item.Name;
                    return;
                }
            }
        }

        private static string BuildCoreExport(Dictionary<string, GameData.Item> gameDataItems)
        {
            List<TranslationManager.TranslationMessage> translationMessages = TranslationManager.BuildCoreMessages(gameDataItems);
            translationMessages.Sort((TranslationManager.TranslationMessage x, TranslationManager.TranslationMessage y) => {
                int num = x.data.type.CompareTo(y.data.type);
                if (num != 0)
                {
                    return num;
                }
                return x.data.OriginalName.CompareTo(y.data.OriginalName);
            });
            string str = Path.Combine(TranslationManager.ActiveTranslation.path, string.Concat(TranslationManager.BaseGameDataFile, TranslationManager.ExportFileExtension));
            using (StreamWriter streamWriter = File.CreateText(str))
            {
                TranslationManager.WriteHeader(streamWriter, "en");
                foreach (TranslationManager.TranslationMessage translationMessage in translationMessages)
                {
                    TranslationManager.WriteCoreMessage(streamWriter, translationMessage, false);
                }
                streamWriter.Close();
            }
            foreach (TranslationManager.TranslationMessage translationMessage1 in translationMessages)
            {
                if (translationMessage1.data.getState() != GameData.State.MODIFIED)
                {
                    continue;
                }
                string str1 = Path.Combine(TranslationManager.ActiveTranslation.path, string.Concat(TranslationManager.BaseGameDataFile, TranslationManager.ImportFileExtension));
                using (StreamWriter streamWriter1 = File.CreateText(str1))
                {
                    TranslationManager.WriteHeader(streamWriter1, "en");
                    foreach (TranslationManager.TranslationMessage translationMessage2 in translationMessages)
                    {
                        TranslationManager.WriteCoreMessage(streamWriter1, translationMessage2, true);
                    }
                    streamWriter1.Close();
                }
                str = string.Concat(str, "\nExported existing translations as ", str1);
                return str;
            }
            return str;
        }

        private static List<TranslationManager.TranslationMessage> BuildCoreMessages(Dictionary<string, GameData.Item> items)
        {
            Dictionary<string, TranslationManager.TranslationMessage> strs = new Dictionary<string, TranslationManager.TranslationMessage>();
            foreach (GameData.Item value in items.Values)
            {
                if (value.getState() == GameData.State.OWNED)
                {
                    continue;
                }
                TranslationManager.ExportFilter exportFilter = TranslationManager.UseData(value);
                if ((exportFilter & TranslationManager.ExportFilter.Name) > TranslationManager.ExportFilter.Nope)
                {
                    TranslationManager.AddOrCreateCoreMessage(strs, value.OriginalName ?? value.Name, value, "");
                }
                if ((exportFilter & TranslationManager.ExportFilter.Fields) <= TranslationManager.ExportFilter.Nope)
                {
                    continue;
                }
                if (value.sdata.ContainsKey("building name") && string.IsNullOrWhiteSpace(value.OriginalValue("building name") as string) && value.ContainsKey("layout interior") && !string.IsNullOrWhiteSpace(value.sdata["layout interior"]))
                {
                    TranslationManager.AddOrCreateCoreMessage(strs, value.sdata["layout interior"], value, "building name");
                }
                foreach (string stringField in TranslationManager.StringFields)
                {
                    if (!value.sdata.ContainsKey(stringField) || !(value.OriginalValue(stringField) is string) || string.IsNullOrWhiteSpace(value.OriginalValue(stringField) as string))
                    {
                        continue;
                    }
                    TranslationManager.AddOrCreateCoreMessage(strs, value.OriginalValue(stringField) as string ?? string.Empty, value, stringField);
                }
            }
            return new List<TranslationManager.TranslationMessage>(strs.Values);
        }

        private static void BuildDialogueExport(GameData gameData, bool changesOnly)
        {
            List<TranslationManager.DialogueLine> dialogueLines;
            int state;
            Action<TranslationManager.DialogueMessage> action2 = null;
            string str = Path.Combine(TranslationManager.ActiveTranslation.path, "dialogue");
            Directory.CreateDirectory(str);
            ConcurrentDictionary<GameData.Item, TranslationManager.DialogueGroup> items = new ConcurrentDictionary<GameData.Item, TranslationManager.DialogueGroup>(TranslationManager.ParallelOpts.MaxDegreeOfParallelism, 1024);
            Parallel.ForEach<TranslationManager.TranslationDialogue>(TranslationManager.EnabledDialogues, TranslationManager.ParallelOpts, (TranslationManager.TranslationDialogue item) => {
                TranslationManager.DialogueGroup dialogueGroup = new TranslationManager.DialogueGroup(item.Item);
                items.TryAdd(item.Item, dialogueGroup);
            });
            Dictionary<string, TranslationManager.DialogueMessage> strs = new Dictionary<string, TranslationManager.DialogueMessage>();
            Action<TranslationManager.DialogueGroup, TranslationManager.DialogueMessage, GameData.Item, uint> action3 = null;
            action3 = (TranslationManager.DialogueGroup group, TranslationManager.DialogueMessage parent, GameData.Item data, uint levelIdx) => {
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in data.referenceData("lines", false))
                {
                    GameData.Item gameDataValid = TranslationManager.GetGameDataValid(gameData, keyValuePair.Key);
                    if (gameDataValid == null || keyValuePair.Value.v0 == 50 || strs.ContainsKey(gameDataValid.stringID))
                    {
                        continue;
                    }
                    TranslationManager.DialogueMessage dialogueMessage = new TranslationManager.DialogueMessage(group, gameDataValid, levelIdx, parent);
                    strs.Add(gameDataValid.stringID, dialogueMessage);
                    group.messages.Add(dialogueMessage);
                    action3(group, dialogueMessage, gameDataValid, levelIdx + 1);
                }
            };
            foreach (KeyValuePair<GameData.Item, TranslationManager.DialogueGroup> keyValuePair1 in items)
            {
                TranslationManager.DialogueGroup dialogueGroup1 = keyValuePair1.Value;
                action3(dialogueGroup1, null, dialogueGroup1.data, 0);
            }
            Parallel.ForEach<KeyValuePair<GameData.Item, TranslationManager.DialogueGroup>>(items, TranslationManager.ParallelOpts, (KeyValuePair<GameData.Item, TranslationManager.DialogueGroup> rl) => {
                List<TranslationManager.DialogueMessage> value = rl.Value.messages;
                ParallelOptions parallelOpts = TranslationManager.ParallelOpts;
                Action<TranslationManager.DialogueMessage> u003cu003e9_3 = action2;
                if (u003cu003e9_3 == null)
                {
                    Action<TranslationManager.DialogueMessage> action = (TranslationManager.DialogueMessage message) => message.Setup(gameData);
                    Action<TranslationManager.DialogueMessage> action1 = action;
                    action2 = action;
                    u003cu003e9_3 = action1;
                }
                Parallel.ForEach<TranslationManager.DialogueMessage>(value, parallelOpts, u003cu003e9_3);
            });
            Dictionary<string, List<TranslationManager.DialogueLine>> strs1 = new Dictionary<string, List<TranslationManager.DialogueLine>>();
            foreach (KeyValuePair<GameData.Item, TranslationManager.DialogueGroup> keyValuePair2 in items)
            {
                foreach (TranslationManager.DialogueMessage dialogueMessage1 in keyValuePair2.Value.messages)
                {
                    for (int i = 0; i < dialogueMessage1.lines.Count; i++)
                    {
                        string str1 = dialogueMessage1.lines[i];
                        if (!string.IsNullOrWhiteSpace(Regex.Replace(str1, TranslationManager.WordSwapRegex, string.Empty)))
                        {
                            if (!strs1.ContainsKey(str1))
                            {
                                List<TranslationManager.DialogueLine> dialogueLines1 = new List<TranslationManager.DialogueLine>()
                                {
                                    new TranslationManager.DialogueLine(dialogueMessage1, (uint)i)
                                };
                                strs1.Add(str1, dialogueLines1);
                            }
                            else
                            {
                                strs1[str1].Add(new TranslationManager.DialogueLine(dialogueMessage1, (uint)i));
                            }
                        }
                    }
                }
            }
            foreach (KeyValuePair<GameData.Item, TranslationManager.DialogueGroup> keyValuePair3 in items)
            {
                if (!changesOnly)
                {
                    TranslationManager.WriteDialogue(str, keyValuePair3.Value, strs1, false);
                }
                else
                {
                    TranslationManager.TranslationDialogue translationDialogue = TranslationManager.EnabledDialogues.Find((TranslationManager.TranslationDialogue o) => o.Item == keyValuePair3.Key);
                    if (translationDialogue == null)
                    {
                        state = 1;
                    }
                    else
                    {
                        state = (int)translationDialogue.State;
                    }
                    if (state == 3)
                    {
                        continue;
                    }
                    if (!TranslationManager.isModifiedRecursive(keyValuePair3.Value.data))
                    {
                        TranslationManager.WriteDialogue(str, keyValuePair3.Value, strs1, false);
                    }
                    else
                    {
                        TranslationManager.WriteDialogue(str, keyValuePair3.Value, strs1, true);
                    }
                }
            }
            List<TranslationManager.WordSpawGroup> wordSpawGroups = new List<TranslationManager.WordSpawGroup>(512);
            foreach (TranslationManager.TranslationDialogue enabledWordSwap in TranslationManager.EnabledWordSwaps)
            {
                wordSpawGroups.Add(new TranslationManager.WordSpawGroup(enabledWordSwap.Item));
            }
            Parallel.ForEach<TranslationManager.WordSpawGroup>(wordSpawGroups, TranslationManager.ParallelOpts, (TranslationManager.WordSpawGroup group) => {
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in group.data.referenceData("lines", false))
                {
                    GameData.Item gameDataValid = TranslationManager.GetGameDataValid(gameData, keyValuePair.Key);
                    if (gameDataValid == null)
                    {
                        continue;
                    }
                    TranslationManager.WordSpawMessage wordSpawMessage = new TranslationManager.WordSpawMessage(group, gameDataValid);
                    wordSpawMessage.Setup(gameData);
                    group.messages.Add(wordSpawMessage);
                    Parallel.For(0, wordSpawMessage.lines.Count, TranslationManager.ParallelOpts, (int l) => {
                        if (string.IsNullOrWhiteSpace(Regex.Replace(wordSpawMessage.lines[l], TranslationManager.WordSwapRegex, string.Empty)))
                        {
                            string empty = string.Empty;
                        }
                    });
                }
            });
            Dictionary<string, List<TranslationManager.DialogueLine>> strs2 = new Dictionary<string, List<TranslationManager.DialogueLine>>(wordSpawGroups.Count * 5);
            foreach (TranslationManager.WordSpawGroup wordSpawGroup in wordSpawGroups)
            {
                foreach (TranslationManager.DialogueMessage dialogueMessage2 in wordSpawGroup.messages)
                {
                    for (int j = 0; j < dialogueMessage2.lines.Count; j++)
                    {
                        string str2 = dialogueMessage2.lines[j];
                        if (!string.IsNullOrEmpty(str2))
                        {
                            if (!strs2.TryGetValue(str2, out dialogueLines))
                            {
                                dialogueLines = new List<TranslationManager.DialogueLine>();
                                strs2.Add(str2, dialogueLines);
                            }
                            dialogueLines.Add(new TranslationManager.DialogueLine(dialogueMessage2, (uint)j));
                        }
                    }
                }
            }
            using (StreamWriter streamWriter = File.CreateText(Path.Combine(str, string.Concat(TranslationManager.WordSwapsFile, TranslationManager.ExportFileExtension))))
            {
                TranslationManager.WriteHeader(streamWriter, "en");
                foreach (TranslationManager.WordSpawGroup wordSpawGroup1 in wordSpawGroups)
                {
                    if (wordSpawGroup1.messages.Count == 0)
                    {
                        continue;
                    }
                    foreach (TranslationManager.DialogueMessage dialogueMessage3 in wordSpawGroup1.messages)
                    {
                        for (int k = 0; k < dialogueMessage3.lines.Count; k++)
                        {
                            string str3 = dialogueMessage3.lines[k];
                            if (strs2.ContainsKey(str3) && !string.IsNullOrEmpty(str3))
                            {
                                List<TranslationManager.DialogueLine> dialogueLines2 = strs2[str3];
                                if (dialogueLines2[0].message == dialogueMessage3 && dialogueLines2[0].lineIndex == k)
                                {
                                    List<string> strs3 = new List<string>(dialogueLines2.Count);
                                    foreach (TranslationManager.DialogueLine dialogueLine in dialogueLines2)
                                    {
                                        streamWriter.WriteLine(string.Concat("#. Id: ", dialogueLine.message.data.stringID));
                                        streamWriter.WriteLine(string.Concat("#. -- Group: ", dialogueLine.message.@group.data.Name));
                                        foreach (string condition in dialogueLine.message.conditions)
                                        {
                                            streamWriter.WriteLine(string.Concat("#. -- Condition: ", condition));
                                        }
                                        strs3.Add(string.Concat(dialogueLine.message.data.stringID.Replace(' ', '?'), ":", dialogueLine.lineIndex));
                                    }
                                    streamWriter.WriteLine(string.Concat("#: ", string.Join(" ", strs3)));
                                    streamWriter.WriteLine("msgid \"{0}\"", str3);
                                    streamWriter.WriteLine("msgstr \"\"");
                                    streamWriter.WriteLine();
                                }
                            }
                        }
                    }
                }
                streamWriter.Close();
            }
        }

        public static void ClearActiveTranslation()
        {
            TranslationManager.ActiveTranslation = null;
            TranslationManager.EnabledDialogues.Clear();
            TranslationManager.EnabledWordSwaps.Clear();
            TranslationManager.DialogueLines.Clear();
            TranslationManager.Missing.Clear();
        }

        public static int CopyTranslationsFromExistingLines()
        {
            int num = 0;
            Dictionary<string, TranslationManager.TranslationDialogueLine.Line> strs = new Dictionary<string, TranslationManager.TranslationDialogueLine.Line>();
            foreach (KeyValuePair<GameData.Item, TranslationManager.TranslationDialogueLine> dialogueLine in TranslationManager.DialogueLines)
            {
                foreach (TranslationManager.TranslationDialogueLine.Line line in dialogueLine.Value.Lines)
                {
                    if (line.State != TranslationManager.DialogueTranslationState.OK)
                    {
                        continue;
                    }
                    strs[line.Original] = line;
                }
            }
            int num1 = -1;
            while (num1 != num)
            {
                num1 = num;
            Label0:
                foreach (KeyValuePair<GameData.Item, TranslationManager.TranslationDialogueLine> keyValuePair in TranslationManager.DialogueLines)
                {
                    foreach (TranslationManager.TranslationDialogueLine.Line translation in keyValuePair.Value.Lines)
                    {
                        if (translation.State != TranslationManager.DialogueTranslationState.NEW || !strs.ContainsKey(translation.Original))
                        {
                            continue;
                        }
                        TranslationManager.TranslationDialogueLine.Line item = strs[translation.Original];
                        baseForm.logger.Info(string.Concat(new string[] { "Translate ", keyValuePair.Value.RefDialogues[0].Item.Name, ": '", translation.Original, "' => '", item.Translation, "'" }));
                        num++;
                        translation.Translation = item.Translation;
                        translation.IsUser = item.IsUser;
                        keyValuePair.Value.UpdateLineState(translation);
                        if (translation.IsLinked)
                        {
                            TranslationManager.UnlinkLine(translation);
                        }
                        if (!item.IsLinked)
                        {
                            break;
                        }
                        translation.LinkedGroup = item.LinkedGroup;
                        TranslationManager.LinkedLines[translation.LinkedGroup].Add(new KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>(keyValuePair.Value, translation));
                        goto Label0;
                    }
                }
            }
            return num;
        }

        public static bool CreateTranslationFolder(TranslationManager.TranslationCulture transCulture)
        {
            if (transCulture == null)
            {
                return false;
            }
            string str = transCulture.culture.Name.Replace('-', '\u005F');
            if (Directory.Exists(Path.Combine(TranslationManager.RootDir, str)))
            {
                return false;
            }
            DirectoryInfo directoryInfo = (new DirectoryInfo(TranslationManager.RootDir)).CreateSubdirectory(str);
            if (!directoryInfo.Exists)
            {
                return false;
            }
            TranslationManager.AvaliableTranslations.Add(new TranslationManager.TranslationCulture(transCulture.culture, directoryInfo.Name, directoryInfo.FullName));
            return true;
        }

        public static string ExportTranslationFiles(navigation nav, bool exportDialogue, bool changesOnly)
        {
            string activeTranslation = TranslationManager.BuildCoreExport(nav.ou.gameData.items);
            if (exportDialogue)
            {
                TranslationManager.BuildDialogueExport(nav.ou.gameData, changesOnly);
                activeTranslation = TranslationManager.ActiveTranslation.path;
            }
            return activeTranslation;
        }

        private static GameData.Item GetGameDataValid(GameData baseData, string sId)
        {
            GameData.Item item = baseData.getItem(sId);
            if (item != null && item.getState() != GameData.State.REMOVED && item.getState() != GameData.State.LOCKED_REMOVED)
            {
                return item;
            }
            return null;
        }

        public static void GetStats(out float percentage, out int translatedWords, out int totalWords, out int extraWords)
        {
            Action<TranslationManager.TranslationDialogueLine.Line> action1 = null;
            char[] chrArray = new char[] { ' ', '\n' };
            percentage = 0f;
            translatedWords = 0;
            totalWords = 0;
            extraWords = 0;
            int num = 0;
            int num1 = 0;
            ConcurrentDictionary<string, byte> strs = new ConcurrentDictionary<string, byte>(TranslationManager.ParallelOpts.MaxDegreeOfParallelism, TranslationManager.DialogueLines.Count * 3);
            Parallel.ForEach<KeyValuePair<GameData.Item, TranslationManager.TranslationDialogueLine>>(TranslationManager.DialogueLines, TranslationManager.ParallelOpts, (KeyValuePair<GameData.Item, TranslationManager.TranslationDialogueLine> item) => {
                List<TranslationManager.TranslationDialogueLine.Line> lines = item.Value.Lines;
                ParallelOptions parallelOpts = TranslationManager.ParallelOpts;
                Action<TranslationManager.TranslationDialogueLine.Line> u003cu003e9_1 = action1;
                if (u003cu003e9_1 == null)
                {
                    Action<TranslationManager.TranslationDialogueLine.Line> isUser = (TranslationManager.TranslationDialogueLine.Line line) => {
                        if (!line.IsUser)
                        {
                            bool flag = strs.ContainsKey(line.Original);
                            if (flag && !line.IsLinked && line.Translation != line.Original && !strs.ContainsKey(line.Translation))
                            {
                                strs.TryAdd(line.Translation, 0);
                                flag = false;
                            }
                            if (!flag)
                            {
                                int length = (int)line.Original.Split(chrArray, StringSplitOptions.RemoveEmptyEntries).Length;
                                Interlocked.Add(ref num, length);
                                if (line.State == TranslationManager.DialogueTranslationState.OK)
                                {
                                    Interlocked.Add(ref num1, length);
                                }
                                strs.TryAdd(line.Original, 0);
                            }
                        }
                    };
                    Action<TranslationManager.TranslationDialogueLine.Line> action = isUser;
                    action1 = isUser;
                    u003cu003e9_1 = action;
                }
                Parallel.ForEach<TranslationManager.TranslationDialogueLine.Line>(lines, parallelOpts, u003cu003e9_1);
            });
            int num2 = 0;
            Parallel.ForEach<GameData.Item>(TranslationManager.Missing, TranslationManager.ParallelOpts, (GameData.Item item) => {
                foreach (KeyValuePair<string, object> keyValuePair in item)
                {
                    if (item.getState(keyValuePair.Key) == GameData.State.ORIGINAL || !keyValuePair.Key.StartsWith("text"))
                    {
                        continue;
                    }
                    int length = (int)(keyValuePair.Value as string).Split(chrArray, StringSplitOptions.RemoveEmptyEntries).Length;
                    Interlocked.Add(ref num2, length);
                }
            });
            extraWords = num2;
            if (num > 0)
            {
                totalWords = num;
                translatedWords = num1;
                percentage = 100f * ((float)translatedWords / (float)totalWords);
            }
        }

        public static bool ImportTranslationFiles(navigation nav, out int statsImportedLines)
        {
            statsImportedLines = 0;
            if (!TranslationManager.TranslationMode)
            {
                return false;
            }
            string[] strArrays = new string[] { "\n\n" };
            char[] chrArray = new char[] { '\n' };
            GameData gameDatum = nav.ou.gameData;
            DirectoryInfo directoryInfo = new DirectoryInfo(TranslationManager.ActiveTranslation.path);
            using (IEnumerator<FileInfo> enumerator = directoryInfo.EnumerateFiles(string.Concat(TranslationManager.BaseGameDataFile, TranslationManager.ImportFileExtension), SearchOption.TopDirectoryOnly).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    FileInfo current = enumerator.Current;
                    string empty = string.Empty;
                    using (StreamReader streamReader = File.OpenText(current.FullName))
                    {
                        empty = streamReader.ReadToEnd().Replace("\r", string.Empty);
                    }
                    string[] strArrays1 = empty.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
                    ConcurrentBag<TranslationManager.ParsedMessage> parsedMessages = new ConcurrentBag<TranslationManager.ParsedMessage>();
                    Parallel.For(1, (int)strArrays1.Length, TranslationManager.ParallelOpts, (int l) => {
                        string str = strArrays1[l];
                        if (str.IndexOf(TranslationManager.ParsedMessage.MessageIDStr) < 0 || str.Contains("#~ msgid \"") || str.Contains("#~ msgstr \""))
                        {
                            return;
                        }
                        TranslationManager.ParsedMessage parsedMessage = new TranslationManager.ParsedMessage(str.Split(chrArray));
                        if (parsedMessage.isValid)
                        {
                            parsedMessages.Add(parsedMessage);
                        }
                    });
                    foreach (TranslationManager.ParsedMessage parsedMessage1 in parsedMessages)
                    {
                        if (string.IsNullOrEmpty(parsedMessage1.message))
                        {
                            continue;
                        }
                        foreach (TranslationManager.ParsedMessage.MessagePair reference in parsedMessage1.references)
                        {
                            GameData.Item gameDataValid = TranslationManager.GetGameDataValid(gameDatum, reference.stringId);
                            if (gameDataValid != null)
                            {
                                if (reference.key >= 0)
                                {
                                    string item = TranslationManager.StringFields[reference.key];
                                    gameDataValid[item] = parsedMessage1.message;
                                }
                                else
                                {
                                    gameDataValid.Name = parsedMessage1.message;
                                }
                                statsImportedLines++;
                            }
                            else
                            {
                                baseForm.logger.Info("Translation: Item {0} does not exist", reference.stringId);
                            }
                        }
                    }
                }
            }
            DirectoryInfo[] directories = directoryInfo.GetDirectories("dialogue");
            if (directories.Length != 0)
            {
                foreach (FileInfo fileInfo in directories[0].EnumerateFiles("*.po", SearchOption.AllDirectories))
                {
                    string empty1 = string.Empty;
                    using (StreamReader streamReader1 = File.OpenText(fileInfo.FullName))
                    {
                        empty1 = streamReader1.ReadToEnd().Replace("\r", string.Empty);
                    }
                    string[] strArrays2 = empty1.Split(strArrays, StringSplitOptions.RemoveEmptyEntries);
                    if ((int)strArrays2.Length <= 1)
                    {
                        continue;
                    }
                    ConcurrentBag<TranslationManager.ParsedMessage> parsedMessages1 = new ConcurrentBag<TranslationManager.ParsedMessage>();
                    Parallel.For(1, (int)strArrays2.Length, TranslationManager.ParallelOpts, (int l) => {
                        string str = strArrays2[l];
                        if (str.IndexOf(TranslationManager.ParsedMessage.MessageIDStr) < 0)
                        {
                            return;
                        }
                        parsedMessages1.Add(new TranslationManager.ParsedMessage(str.Split(chrArray)));
                    });
                    foreach (TranslationManager.ParsedMessage parsedMessage2 in parsedMessages1)
                    {
                        if (string.IsNullOrEmpty(parsedMessage2.message))
                        {
                            continue;
                        }
                        foreach (TranslationManager.ParsedMessage.MessagePair messagePair in parsedMessage2.references)
                        {
                            GameData.Item gameDataValid1 = TranslationManager.GetGameDataValid(gameDatum, messagePair.stringId);
                            if (gameDataValid1 == null)
                            {
                                continue;
                            }
                            string bah = string.Concat("text", messagePair.key);
                            if (!gameDataValid1.ContainsKey(bah))
                            {
                                continue;
                            }
                            if (!gameDataValid1.ContainsKey(string.Concat("_original_", bah)))
                            {
                                gameDataValid1[string.Concat("_original_", bah)] = gameDataValid1[bah];
                            }
                            gameDataValid1[bah] = parsedMessage2.message;
                            statsImportedLines++;
                        }
                    }
                }
            }
            TranslationManager.RefreshStates();
            return true;
        }

        private static bool isModifiedRecursive(GameData.Item item)
        {
            bool flag;
            if (item.getState() == GameData.State.MODIFIED)
            {
                return true;
            }
            using (IEnumerator<Tuple<GameData.Item, GameData.TripleInt>> enumerator = item.referenceItems("lines", false).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Tuple<GameData.Item, GameData.TripleInt> current = enumerator.Current;
                    if (current.Item1 == null || current.Item2.v0 == 50 || !TranslationManager.isModifiedRecursive(current.Item1))
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

        public static void RefreshAvailableTranslations()
        {
            TranslationManager.AvaliableTranslations.Clear();
            foreach (DirectoryInfo directoryInfo in (new DirectoryInfo(TranslationManager.RootDir)).EnumerateDirectories())
            {
                if (directoryInfo.Name == "base")
                {
                    continue;
                }
                CultureInfo cultureInfo = null;
                try
                {
                    cultureInfo = CultureInfo.GetCultureInfo(directoryInfo.Name);
                }
                catch (Exception exception)
                {
                    cultureInfo = null;
                }
                if (cultureInfo == null)
                {
                    try
                    {
                        cultureInfo = CultureInfo.GetCultureInfo(directoryInfo.Name.Replace('\u005F', '-'));
                    }
                    catch (Exception exception1)
                    {
                        cultureInfo = null;
                    }
                }
                TranslationManager.AvaliableTranslations.Add(new TranslationManager.TranslationCulture(cultureInfo, directoryInfo.Name, directoryInfo.FullName));
            }
        }

        public static void RefreshStates()
        {
            Parallel.ForEach<KeyValuePair<GameData.Item, TranslationManager.TranslationDialogueLine>>(TranslationManager.DialogueLines, TranslationManager.ParallelOpts, (KeyValuePair<GameData.Item, TranslationManager.TranslationDialogueLine> l) => l.Value.RefreshState());
            Parallel.ForEach<TranslationManager.TranslationDialogue>(TranslationManager.EnabledDialogues, TranslationManager.ParallelOpts, (TranslationManager.TranslationDialogue d) => d.UpdateState());
            Parallel.ForEach<TranslationManager.TranslationDialogue>(TranslationManager.EnabledWordSwaps, TranslationManager.ParallelOpts, (TranslationManager.TranslationDialogue d) => d.UpdateState());
        }

        public static void SetActiveTranslation(TranslationManager.TranslationCulture translation, GameData gameData)
        {
            TranslationManager.ActiveTranslation = translation;
            TranslationManager.ActiveTranslationMod = gameData.activeFileName;
            TranslationManager.UpdateActiveTranslationData(gameData);
        }

        public static void UnlinkLine(TranslationManager.TranslationDialogueLine.Line line)
        {
            List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>> item = TranslationManager.LinkedLines[line.LinkedGroup];
            foreach (KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line> keyValuePair in item)
            {
                if (keyValuePair.Value != line)
                {
                    continue;
                }
                item.Remove(keyValuePair);
                line.IsLinked = false;
                return;
            }
            line.IsLinked = false;
        }

        private static void UpdateActiveTranslationData(GameData gameData)
        {
            TranslationManager.DialogueLines.Clear();
            TranslationManager.LinkedLines.Clear();
            TranslationManager.EnabledDialogues.Clear();
            TranslationManager.EnabledWordSwaps.Clear();
            TranslationManager.Missing.Clear();
            Dictionary<forgotten_construction_set.itemType, HashSet<GameData.Item>> itemTypes = new Dictionary<forgotten_construction_set.itemType, HashSet<GameData.Item>>(4);
            itemTypes[forgotten_construction_set.itemType.SQUAD_TEMPLATE] = new HashSet<GameData.Item>();
            itemTypes[forgotten_construction_set.itemType.DIALOGUE_PACKAGE] = new HashSet<GameData.Item>();
            itemTypes[forgotten_construction_set.itemType.DIALOGUE] = new HashSet<GameData.Item>();
            itemTypes[forgotten_construction_set.itemType.CHARACTER] = new HashSet<GameData.Item>();
            itemTypes[forgotten_construction_set.itemType.FACTION_CAMPAIGN] = new HashSet<GameData.Item>();
            itemTypes[forgotten_construction_set.itemType.AI_PACKAGE] = new HashSet<GameData.Item>();
            HashSet<GameData.Item> items = new HashSet<GameData.Item>();
            BitArray bitArrays = new BitArray(116, false);
            bitArrays.Set(64, true);
            bitArrays.Set(95, true);
            bitArrays.Set(13, true);
            bitArrays.Set(10, true);
            bitArrays.Set(93, true);
            bitArrays.Set(72, true);
            foreach (KeyValuePair<string, GameData.Item> item in gameData.items)
            {
                GameData.Item value = item.Value;
                if (value.type != forgotten_construction_set.itemType.WORD_SWAPS)
                {
                    if (!bitArrays[(int)value.type])
                    {
                        continue;
                    }
                    foreach (KeyValuePair<forgotten_construction_set.itemType, HashSet<GameData.Item>> itemType in itemTypes)
                    {
                        itemType.Value.UnionWith(value.GetAllReferencesOfType(itemType.Key));
                    }
                }
                else
                {
                    TranslationManager.TranslationDialogue translationDialogue = new TranslationManager.TranslationDialogue(value);
                    TranslationManager.EnabledWordSwaps.Add(translationDialogue);
                    foreach (GameData.Item allReference in value.GetAllReferences("lines"))
                    {
                        TranslationManager.TranslationDialogueLine translationDialogueLine = new TranslationManager.TranslationDialogueLine(allReference);
                        translationDialogue.AddLine(translationDialogueLine);
                        TranslationManager.DialogueLines.TryAdd(allReference, translationDialogueLine);
                    }
                }
            }
            HashSet<GameData.Item> items1 = new HashSet<GameData.Item>(itemTypes[forgotten_construction_set.itemType.SQUAD_TEMPLATE]);
            Queue<GameData.Item> items2 = new Queue<GameData.Item>(itemTypes[forgotten_construction_set.itemType.SQUAD_TEMPLATE]);
            while (items2.Count > 0)
            {
                GameData.Item item1 = items2.Dequeue();
                items1.Add(item1);
                itemTypes[forgotten_construction_set.itemType.DIALOGUE_PACKAGE].UnionWith(item1.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE_PACKAGE));
                itemTypes[forgotten_construction_set.itemType.DIALOGUE].UnionWith(item1.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE));
                itemTypes[forgotten_construction_set.itemType.CHARACTER].UnionWith(item1.GetAllReferencesOfType(forgotten_construction_set.itemType.CHARACTER));
                foreach (GameData.Item allReferencesOfType in item1.GetAllReferencesOfType(forgotten_construction_set.itemType.SQUAD_TEMPLATE))
                {
                    if (items1.Contains(allReferencesOfType))
                    {
                        continue;
                    }
                    items2.Enqueue(allReferencesOfType);
                }
            }
            foreach (GameData.Item item2 in itemTypes[forgotten_construction_set.itemType.AI_PACKAGE])
            {
                itemTypes[forgotten_construction_set.itemType.DIALOGUE].UnionWith(item2.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE));
            }
            HashSet<GameData.Item> items3 = new HashSet<GameData.Item>(itemTypes[forgotten_construction_set.itemType.FACTION_CAMPAIGN]);
            Queue<GameData.Item> items4 = new Queue<GameData.Item>(itemTypes[forgotten_construction_set.itemType.FACTION_CAMPAIGN]);
            while (items4.Count > 0)
            {
                GameData.Item item3 = items4.Dequeue();
                items3.Add(item3);
                itemTypes[forgotten_construction_set.itemType.DIALOGUE_PACKAGE].UnionWith(item3.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE_PACKAGE));
                itemTypes[forgotten_construction_set.itemType.DIALOGUE].UnionWith(item3.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE));
                itemTypes[forgotten_construction_set.itemType.CHARACTER].UnionWith(item3.GetAllReferencesOfType(forgotten_construction_set.itemType.CHARACTER));
                foreach (GameData.Item allReferencesOfType1 in item3.GetAllReferencesOfType(forgotten_construction_set.itemType.FACTION_CAMPAIGN))
                {
                    if (items3.Contains(allReferencesOfType1))
                    {
                        continue;
                    }
                    items4.Enqueue(allReferencesOfType1);
                }
            }
            foreach (GameData.Item item4 in itemTypes[forgotten_construction_set.itemType.CHARACTER])
            {
                itemTypes[forgotten_construction_set.itemType.DIALOGUE_PACKAGE].UnionWith(item4.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE_PACKAGE));
                itemTypes[forgotten_construction_set.itemType.DIALOGUE].UnionWith(item4.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE));
            }
            HashSet<GameData.Item> items5 = new HashSet<GameData.Item>(itemTypes[forgotten_construction_set.itemType.DIALOGUE_PACKAGE]);
            Queue<GameData.Item> items6 = new Queue<GameData.Item>(itemTypes[forgotten_construction_set.itemType.DIALOGUE_PACKAGE]);
            while (items6.Count > 0)
            {
                GameData.Item item5 = items6.Dequeue();
                items5.Add(item5);
                if (CultureInfo.InvariantCulture.CompareInfo.IndexOf(item5.Name, "--unfinished--", CompareOptions.IgnoreCase) >= 0)
                {
                    continue;
                }
                HashSet<GameData.Item> items7 = new HashSet<GameData.Item>(item5.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE));
                items7.RemoveWhere((GameData.Item a) => {
                    if (a.fdata["chance temporary"] > 0f)
                    {
                        return false;
                    }
                    return a.fdata["chance permanent"] <= 0f;
                });
                itemTypes[forgotten_construction_set.itemType.DIALOGUE].UnionWith(items7);
                foreach (GameData.Item allReferencesOfType2 in item5.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE_PACKAGE))
                {
                    if (items5.Contains(allReferencesOfType2))
                    {
                        continue;
                    }
                    items6.Enqueue(allReferencesOfType2);
                }
            }
            HashSet<GameData.Item> items8 = new HashSet<GameData.Item>();
            foreach (TranslationManager.TranslationDialogue enabledWordSwap in TranslationManager.EnabledWordSwaps)
            {
                items8.UnionWith(enabledWordSwap.Item.GetAllReferences("crowd trigger"));
            }
            foreach (GameData.Item item6 in itemTypes[forgotten_construction_set.itemType.DIALOGUE])
            {
                items8.UnionWith(item6.GetAllReferences("crowd trigger"));
            }
            itemTypes[forgotten_construction_set.itemType.DIALOGUE].UnionWith(items8);
            for (HashSet<GameData.Item> i = itemTypes[forgotten_construction_set.itemType.DIALOGUE]; i.Count > 0; i = new HashSet<GameData.Item>(items8))
            {
                items8.Clear();
                foreach (GameData.Item item7 in i)
                {
                    Queue<GameData.Item> items9 = new Queue<GameData.Item>(item7.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE_LINE));
                    while (items9.Count > 0)
                    {
                        GameData.Item item8 = items9.Dequeue();
                        items8.UnionWith(item8.GetAllReferences("crowd trigger"));
                        items8.UnionWith(item8.GetAllReferences("interrupt"));
                        foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in item8.referenceData("lines", false))
                        {
                            if (keyValuePair.Value.v0 == 50)
                            {
                                continue;
                            }
                            items9.Enqueue(gameData.getItem(keyValuePair.Key));
                        }
                    }
                }
                items8.ExceptWith(itemTypes[forgotten_construction_set.itemType.DIALOGUE]);
                itemTypes[forgotten_construction_set.itemType.DIALOGUE].UnionWith(items8);
            }
            foreach (GameData.Item item9 in itemTypes[forgotten_construction_set.itemType.DIALOGUE])
            {
                List<GameData.Item> items10 = new List<GameData.Item>();
                Queue<GameData.Item> items11 = new Queue<GameData.Item>(item9.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE_LINE));
                while (items11.Count > 0)
                {
                    GameData.Item item10 = items11.Dequeue();
                    items10.Add(item10);
                    TranslationManager.DialogueLines.TryAdd(item10, new TranslationManager.TranslationDialogueLine(item10));
                    foreach (GameData.Item allReferencesOfType3 in item10.GetAllReferencesOfType(forgotten_construction_set.itemType.DIALOGUE_LINE))
                    {
                        if (items10.Contains(allReferencesOfType3))
                        {
                            continue;
                        }
                        items11.Enqueue(allReferencesOfType3);
                    }
                }
                if (items10.Count <= 0)
                {
                    continue;
                }
                bool flag = false;
                foreach (GameData.Item item11 in items10)
                {
                    if (TranslationManager.DialogueLines[item11].Lines.Count <= 0)
                    {
                        continue;
                    }
                    flag = true;
                    goto Label0;
                }
            Label0:
                if (!flag)
                {
                    continue;
                }
                TranslationManager.TranslationDialogue translationDialogue1 = new TranslationManager.TranslationDialogue(item9);
                foreach (GameData.Item item12 in items10)
                {
                    translationDialogue1.AddLine(TranslationManager.DialogueLines[item12]);
                }
                TranslationManager.EnabledDialogues.Add(translationDialogue1);
            }
            Dictionary<string, List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>>> strs = new Dictionary<string, List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>>>(TranslationManager.DialogueLines.Count * 3);
            foreach (KeyValuePair<GameData.Item, TranslationManager.TranslationDialogueLine> dialogueLine in TranslationManager.DialogueLines)
            {
                foreach (TranslationManager.TranslationDialogueLine.Line line in dialogueLine.Value.Lines)
                {
                    if (line.IsUser)
                    {
                        continue;
                    }
                    List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>> keyValuePairs = null;
                    string str = string.Concat(line.Original, "|", line.Translation);
                    if (strs.ContainsKey(str))
                    {
                        keyValuePairs = strs[str];
                    }
                    else
                    {
                        keyValuePairs = new List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>>();
                        strs[str] = keyValuePairs;
                    }
                    keyValuePairs.Add(new KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>(dialogueLine.Value, line));
                }
            }
            foreach (KeyValuePair<string, List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>>> str1 in strs)
            {
                if (str1.Value.Count <= 1)
                {
                    continue;
                }
                int count = TranslationManager.LinkedLines.Count;
                TranslationManager.LinkedLines.Add(str1.Value);
                foreach (KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line> value1 in str1.Value)
                {
                    value1.Value.IsLinked = true;
                    value1.Value.LinkedGroup = count;
                }
            }
            foreach (GameData.Item value2 in gameData.items.Values)
            {
                if (value2.type != forgotten_construction_set.itemType.DIALOGUE_LINE || value2.getState() != GameData.State.MODIFIED || value2.refCount != 0 && TranslationManager.DialogueLines.ContainsKey(value2))
                {
                    continue;
                }
                TranslationManager.Missing.Add(value2);
            }
            TranslationManager.RefreshStates();
        }

        public static void UpdateLinkedLines(TranslationManager.TranslationDialogueLine.Line line, bool prompt)
        {
            List<KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line>> item = TranslationManager.LinkedLines[line.LinkedGroup];
            if (prompt && MessageBox.Show(string.Concat("Line is duplicated in ", item.Count - 1, " other places. Do you want to change them all?"), "Translation", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                TranslationManager.UnlinkLine(line);
                return;
            }
            foreach (KeyValuePair<TranslationManager.TranslationDialogueLine, TranslationManager.TranslationDialogueLine.Line> translation in item)
            {
                translation.Value.Translation = line.Translation;
                translation.Key.UpdateLineState(translation.Value);
            }
        }

        private static TranslationManager.ExportFilter UseData(GameData.Item item)
        {
            if (item == null)
            {
                return TranslationManager.ExportFilter.Nope;
            }
            itemType _itemType = item.type;
            if (_itemType <= itemType.ANIMAL_CHARACTER)
            {
                if (_itemType <= itemType.SQUAD_TEMPLATE)
                {
                    switch (_itemType)
                    {
                        case itemType.BUILDING:
                            {
                                if (item.ContainsKey("is node") && item.bdata["is node"])
                                {
                                    return TranslationManager.ExportFilter.Nope;
                                }
                                if (item.ContainsKey("building category") && item.sdata["building category"] == "DOORS")
                                {
                                    return TranslationManager.ExportFilter.Nope;
                                }
                                if (item.ContainsKey("is sign") && item.bdata["is sign"])
                                {
                                    return TranslationManager.ExportFilter.Nope;
                                }
                                return TranslationManager.ExportFilter.All;
                            }
                        case itemType.CHARACTER:
                            {
                                if (item.ContainsKey("named") && !item.bdata["named"])
                                {
                                    return TranslationManager.ExportFilter.Name;
                                }
                                return TranslationManager.ExportFilter.Nope;
                            }
                        case itemType.WEAPON:
                        case itemType.ITEM:
                        case itemType.RACE:
                        case itemType.FACTION:
                        case itemType.TOWN:
                        case itemType.LOCATIONAL_DAMAGE:
                            {
                                return TranslationManager.ExportFilter.All;
                            }
                        case itemType.ARMOUR:
                            {
                                return TranslationManager.ExportFilter.All;
                            }
                        case itemType.ANIMAL_ANIMATION:
                        case itemType.ATTACHMENT:
                        case itemType.LOCATION:
                        case itemType.WAR_SAVESTATE:
                        case itemType.NULL_ITEM:
                        case itemType.ZONE_MAP:
                        case itemType.WORLDMAP_CHARACTER:
                        case itemType.CHARACTER_APPEARANCE_OLD:
                            {
                                break;
                            }
                        default:
                            {
                                if (_itemType == itemType.RESEARCH)
                                {
                                    return TranslationManager.ExportFilter.All;
                                }
                                switch (_itemType)
                                {
                                    case itemType.CONTAINER:
                                    case itemType.MATERIAL_SPECS_WEAPON:
                                    case itemType.WEAPON_MANUFACTURER:
                                        {
                                            return TranslationManager.ExportFilter.All;
                                        }
                                    case itemType.SQUAD_TEMPLATE:
                                        {
                                            return TranslationManager.ExportFilter.Fields;
                                        }
                                }
                                break;
                            }
                    }
                }
                else if (_itemType > itemType.NEW_GAME_STARTOFF)
                {
                    if (_itemType == itemType.SINGLE_DIPLOMATIC_ASSAULT)
                    {
                        return TranslationManager.ExportFilter.All;
                    }
                    if (_itemType == itemType.ANIMAL_CHARACTER)
                    {
                        return TranslationManager.ExportFilter.Name;
                    }
                }
                else
                {
                    if (_itemType == itemType.BUILDING_FUNCTIONALITY)
                    {
                        return TranslationManager.ExportFilter.Fields;
                    }
                    if (_itemType == itemType.NEW_GAME_STARTOFF)
                    {
                        return TranslationManager.ExportFilter.All;
                    }
                }
            }
            else if (_itemType <= itemType.CHARACTER_PHYSICS_ATTACHMENT)
            {
                if (_itemType == itemType.UNIQUE_SQUAD_TEMPLATE)
                {
                    return TranslationManager.ExportFilter.Fields;
                }
                if (_itemType == itemType.NEST_ITEM || _itemType == itemType.CHARACTER_PHYSICS_ATTACHMENT)
                {
                    return TranslationManager.ExportFilter.All;
                }
            }
            else if (_itemType > itemType.BIOME_GROUP)
            {
                switch (_itemType)
                {
                    case itemType.RACE_GROUP:
                    case itemType.MAP_ITEM:
                    case itemType.ITEMS_CULTURE:
                    case itemType.CROSSBOW:
                        {
                            return TranslationManager.ExportFilter.All;
                        }
                    case itemType.ARTIFACTS:
                        {
                            return TranslationManager.ExportFilter.All;
                        }
                    case itemType.BUILDINGS_SWAP:
                    case itemType.ANIMATION_EVENT:
                    case itemType.TUTORIAL:
                        {
                            break;
                        }
                    default:
                        {
                            if (_itemType == itemType.LIMB_REPLACEMENT)
                            {
                                return TranslationManager.ExportFilter.All;
                            }
                            break;
                        }
                }
            }
            else
            {
                if (_itemType == itemType.FACTION_CAMPAIGN)
                {
                    return TranslationManager.ExportFilter.Fields;
                }
                if (_itemType == itemType.BIOME_GROUP)
                {
                    return TranslationManager.ExportFilter.Name;
                }
            }
            return TranslationManager.ExportFilter.Nope;
        }

        private static void WriteCoreMessage(StreamWriter writer, TranslationManager.TranslationMessage message, bool translated = false)
        {
            writer.WriteLine("#. Type: {0}", message.data.type.ToString());
            writer.WriteLine("#. Name: {0}", message.data.OriginalName);
            foreach (string reference in message.references)
            {
                writer.WriteLine("#: {0}", reference.Replace(' ', '?'));
            }
            string str = message.id;
            if (str.IndexOf("\n") == -1)
            {
                writer.WriteLine("msgid \"{0}\"", str);
            }
            else
            {
                writer.WriteLine("msgid \"\"");
                str = str.Replace("\r", "");
                string[] strArrays = str.Split(new char[] { '\n' });
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    if (i >= (int)strArrays.Length - 1)
                    {
                        writer.WriteLine("\"{0}\"", strArrays[i]);
                    }
                    else
                    {
                        writer.WriteLine("\"{0}\\n\"", strArrays[i]);
                    }
                }
            }
            if (!translated || string.IsNullOrEmpty(message.translated))
            {
                writer.WriteLine("msgstr \"\"");
            }
            else if (message.translated.IndexOf("\n") == -1)
            {
                writer.WriteLine("msgstr \"{0}\"", message.translated);
            }
            else
            {
                writer.WriteLine("msgstr \"\"");
                string[] strArrays1 = message.translated.Split(new char[] { '\n' });
                for (int j = 0; j < (int)strArrays1.Length; j++)
                {
                    writer.WriteLine("\"{0}\\n\"", strArrays1[j]);
                }
            }
            writer.WriteLine();
        }

        private static void WriteDialogue(string dialogueDir, TranslationManager.DialogueGroup group, Dictionary<string, List<TranslationManager.DialogueLine>> lines, bool translatedOnly)
        {
            if (group.messages.Any<TranslationManager.DialogueMessage>((TranslationManager.DialogueMessage m) => m.lines.Count > 0))
            {
                string name = group.data.stringID;
                if (!string.IsNullOrWhiteSpace(group.data.Name))
                {
                    name = group.data.Name;
                }
                name = Regex.Replace(name, string.Concat("[", Regex.Escape(new string(Path.GetInvalidFileNameChars())), "]"), string.Empty);
                if (!translatedOnly)
                {
                    string str = Path.Combine(dialogueDir, string.Concat(name, TranslationManager.ExportFileExtension));
                    bool flag = false;
                    using (StreamWriter streamWriter = File.CreateText(str))
                    {
                        TranslationManager.WriteHeader(streamWriter, "en");
                        for (int i = 0; i < group.messages.Count; i++)
                        {
                            flag |= TranslationManager.WriteDialogueMessage(streamWriter, group, group.messages[i], lines, false);
                        }
                        streamWriter.Close();
                    }
                    if (!flag)
                    {
                        try
                        {
                            File.Delete(str);
                        }
                        catch (Exception exception)
                        {
                        }
                    }
                }
                string str1 = Path.Combine(dialogueDir, "translated");
                if (TranslationManager.isModifiedRecursive(group.data))
                {
                    Directory.CreateDirectory(str1);
                    StreamWriter streamWriter1 = File.CreateText(Path.Combine(str1, string.Concat(name, ".po")));
                    TranslationManager.WriteHeader(streamWriter1, TranslationManager.ActiveTranslation.id);
                    foreach (TranslationManager.DialogueMessage message in group.messages)
                    {
                        TranslationManager.WriteDialogueMessage(streamWriter1, group, message, lines, true);
                    }
                    streamWriter1.Close();
                }
            }
        }

        private static bool WriteDialogueMessage(StreamWriter writer, TranslationManager.DialogueGroup group, TranslationManager.DialogueMessage message, Dictionary<string, List<TranslationManager.DialogueLine>> lines, bool translated = false)
        {
            if (message.lines.Count == 0)
            {
                return false;
            }
            bool flag = false;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < message.lines.Count; i++)
            {
                string item = message.lines[i];
                if (lines.ContainsKey(item))
                {
                    List<TranslationManager.DialogueLine> dialogueLines = lines[item];
                    if (dialogueLines[0].message == message && dialogueLines[0].lineIndex == i)
                    {
                        stringBuilder.Clear();
                        List<string> strs = new List<string>(dialogueLines.Count);
                        foreach (TranslationManager.DialogueLine dialogueLine in dialogueLines)
                        {
                            TranslationManager.DialogueMessage dialogueMessage = dialogueLine.message;
                            stringBuilder.Append("#. Id: ").AppendLine(dialogueLine.message.data.stringID);
                            stringBuilder.Append("#. -- Index: ").Append(dialogueLine.message.index).Append(".").Append(dialogueLine.lineIndex).AppendLine();
                            if (dialogueLine.message.parent == null)
                            {
                                stringBuilder.AppendLine("#. -- Parent line: -");
                            }
                            else
                            {
                                stringBuilder.Append("#. -- Parent line: ").Append(dialogueLine.message.parent.index).Append(" (").Append(dialogueLine.message.parent.data.stringID).AppendLine(")");
                            }
                            stringBuilder.Append("#. -- Speaker: ").Append(dialogueLine.message.speaker).Append(" (").Append(dialogueLine.message.speakerIdx).AppendLine(")");
                            stringBuilder.Append("#. -- Group title: ").AppendLine(dialogueLine.message.@group.data.Name);
                            foreach (string condition in dialogueLine.message.conditions)
                            {
                                stringBuilder.Append("#. -- Condition: ").AppendLine(condition);
                            }
                            strs.Add(string.Format("{0}:{1}", dialogueLine.message.data.stringID.Replace(' ', '?'), dialogueLine.lineIndex));
                        }
                        stringBuilder.Append("#: ").AppendLine(string.Join(" ", strs));
                        stringBuilder.Append("msgid \"").Append(item).AppendLine("\"");
                        if (!translated || message.data.getState(string.Concat("text", i.ToString())) != GameData.State.MODIFIED)
                        {
                            stringBuilder.AppendLine("msgstr \"\"");
                        }
                        else
                        {
                            string str = message.data.sdata[string.Concat("text", i.ToString())];
                            stringBuilder.Append("msgstr \"").Append(str).AppendLine("\"");
                        }
                        writer.WriteLine(stringBuilder);
                        flag = true;
                    }
                }
            }
            return flag;
        }

        private static void WriteHeader(StreamWriter writer, string language = "en")
        {
            string str = DateTime.Now.ToString("yyyy-MM-dd HH':'mmzzz");
            str = str.Remove(str.LastIndexOf(':'), 1);
            writer.WriteLine("msgid \"\"");
            writer.WriteLine("msgstr \"\"");
            writer.WriteLine("\"Project-Id-Version: Kenshi\\n\"");
            writer.WriteLine("\"POT-Creation-Date: {0}\\n\"", str);
            writer.WriteLine("\"PO-Revision-Date: \\n\"");
            writer.WriteLine("\"Last-Translator: \\n\"");
            writer.WriteLine("\"Language-Team: Lo-Fi Games\\n\"");
            writer.WriteLine(string.Concat("\"Language: ", language, "\\n\""));
            writer.WriteLine("\"MIME-Version: 1.0\\n\"");
            writer.WriteLine("\"Content-Type: text/plain; charset=UTF-8\\n\"");
            writer.WriteLine("\"Content-Transfer-Encoding: 8bit\\n\"");
            writer.WriteLine("\"Plural-Forms: nplurals=2; plural=(n != 1);\\n\"");
            writer.WriteLine();
        }

        private class DialogueGroup
        {
            public GameData.Item data;

            public List<TranslationManager.DialogueMessage> messages = new List<TranslationManager.DialogueMessage>();

            public DialogueGroup(GameData.Item d)
            {
                this.data = d;
            }
        }

        private class DialogueLine
        {
            public TranslationManager.DialogueMessage message;

            public uint lineIndex;

            public DialogueLine(TranslationManager.DialogueMessage msg, uint line)
            {
                this.message = msg;
                this.lineIndex = line;
            }
        }

        private class DialogueMessage
        {
            public TranslationManager.DialogueGroup @group;

            public GameData.Item data;

            public TranslationManager.DialogueMessage parent;

            public uint index;

            public byte speakerIdx;

            public string speaker = string.Empty;

            public List<string> lines = new List<string>();

            public List<string> conditions = new List<string>();

            private readonly static string[] ComparerStr;

            private readonly static byte[] SpeakerIndices;

            private readonly static string[] Speakers;

            static DialogueMessage()
            {
                TranslationManager.DialogueMessage.ComparerStr = new string[] { "=", "<", ">" };
                TranslationManager.DialogueMessage.SpeakerIndices = new byte[] { 0, 1, 1, 2, 3, 4, 5, 6 };
                TranslationManager.DialogueMessage.Speakers = new string[] { "Me", "Target", "Extra 1", "Extra 2", "Extra 3", "Whole squad", "Target with race" };
            }

            protected DialogueMessage()
            {
            }

            public DialogueMessage(TranslationManager.DialogueGroup group, GameData.Item d, uint lineIdx, TranslationManager.DialogueMessage parent = null)
            {
                this.@group = group;
                this.data = d;
                this.index = lineIdx;
                this.parent = parent;
                int item = this.data.idata["speaker"];
                if (item < 0 || item >= (int)TranslationManager.DialogueMessage.SpeakerIndices.Length)
                {
                    this.speakerIdx = 0;
                    this.speaker = string.Empty;
                    return;
                }
                this.speakerIdx = TranslationManager.DialogueMessage.SpeakerIndices[item];
                this.speaker = TranslationManager.DialogueMessage.Speakers[this.speakerIdx];
            }

            public void Setup(GameData gameData)
            {
                for (int i = 0; this.data.sdata.ContainsKey(string.Concat("text", i)); i++)
                {
                    object obj = this.data.OriginalValue(string.Concat("text", i));
                    if (obj != null)
                    {
                        string str = obj.ToString();
                        if (string.IsNullOrWhiteSpace(str))
                        {
                            this.lines.Add(string.Empty);
                        }
                        else
                        {
                            this.lines.Add(str);
                        }
                    }
                }
                StringBuilder stringBuilder = new StringBuilder();
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in this.data.referenceData("conditions", false))
                {
                    GameData.Item item = gameData.getItem(keyValuePair.Key);
                    DialogConditionEnum dialogConditionEnum = (DialogConditionEnum)item.idata["condition name"];
                    stringBuilder.Clear();
                    StringBuilder stringBuilder1 = stringBuilder.Append("Who: ");
                    TalkerEnum talkerEnum = (TalkerEnum)item.idata["who"];
                    stringBuilder1.Append(talkerEnum.ToString().Substring(2));
                    stringBuilder.Append(", Condition: ").Append(dialogConditionEnum.ToString().Substring(3));
                    stringBuilder.Append(TranslationManager.DialogueMessage.ComparerStr[item.idata["compare by"]]);
                    stringBuilder.Append(keyValuePair.Value.v0);
                    int num = item.idata["tag"];
                    if (num < 0)
                    {
                        stringBuilder.Append(", Tag: ").Append(num);
                    }
                    else if (dialogConditionEnum == DialogConditionEnum.DC_HAS_TAG)
                    {
                        CharacterPerceptionTags_LongTerm characterPerceptionTagsLongTerm = (CharacterPerceptionTags_LongTerm)num;
                        stringBuilder.Append(", Tag: ").Append(characterPerceptionTagsLongTerm.ToString().Substring(3));
                    }
                    else if (dialogConditionEnum == DialogConditionEnum.DC_PERSONALITY_TAG)
                    {
                        PersonalityTags personalityTag = (PersonalityTags)num;
                        stringBuilder.Append(", Tag: ").Append(personalityTag.ToString().Substring(3));
                    }
                    this.conditions.Add(stringBuilder.ToString());
                }
                stringBuilder.Clear();
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair1 in this.data.referenceData("my race", false))
                {
                    GameData.Item item1 = gameData.getItem(keyValuePair1.Key);
                    if (item1 == null)
                    {
                        continue;
                    }
                    if (stringBuilder.Length != 0)
                    {
                        stringBuilder.Append(" or ").Append(item1.Name);
                    }
                    else
                    {
                        stringBuilder.Append("My Race(s): ").Append(item1.Name);
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    this.conditions.Add(stringBuilder.ToString());
                }
                stringBuilder.Clear();
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair2 in this.data.referenceData("my subrace", false))
                {
                    GameData.Item item2 = gameData.getItem(keyValuePair2.Key);
                    if (item2 == null)
                    {
                        continue;
                    }
                    if (stringBuilder.Length != 0)
                    {
                        stringBuilder.Append(" or ").Append(item2.Name);
                    }
                    else
                    {
                        stringBuilder.Append("My Sub-Race(s): ").Append(item2.Name);
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    this.conditions.Add(stringBuilder.ToString());
                }
                stringBuilder.Clear();
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair3 in this.data.referenceData("target race", false))
                {
                    GameData.Item item3 = gameData.getItem(keyValuePair3.Key);
                    if (item3 == null)
                    {
                        continue;
                    }
                    if (stringBuilder.Length != 0)
                    {
                        stringBuilder.Append(" or ").Append(item3.Name);
                    }
                    else
                    {
                        stringBuilder.Append("Target Race(s): ").Append(item3.Name);
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    this.conditions.Add(stringBuilder.ToString());
                }
                stringBuilder.Clear();
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair4 in this.data.referenceData("in town of", false))
                {
                    GameData.Item item4 = gameData.getItem(keyValuePair4.Key);
                    if (item4 == null)
                    {
                        continue;
                    }
                    if (stringBuilder.Length != 0)
                    {
                        stringBuilder.Append(" or ").Append(item4.Name);
                    }
                    else
                    {
                        stringBuilder.Append("In town(s) of faction(s): ").Append(item4.Name);
                    }
                }
                if (stringBuilder.Length > 0)
                {
                    this.conditions.Add(stringBuilder.ToString());
                }
            }
        }

        public enum DialogueTranslationState : byte
        {
            NULL,
            NEW,
            ORIGINAL_MODIFIED,
            OK
        }

        private enum ExportFilter
        {
            Nope,
            Name,
            Fields,
            All
        }

        private class ParsedMessage
        {
            public readonly static string MessageIDStr;

            private readonly static string MessageIDEmptyStr;

            private readonly static int MessageIDBeginLength;

            public string message;

            public List<TranslationManager.ParsedMessage.MessagePair> references = new List<TranslationManager.ParsedMessage.MessagePair>();

            public bool isValid;

            static ParsedMessage()
            {
                TranslationManager.ParsedMessage.MessageIDStr = "msgstr";
                TranslationManager.ParsedMessage.MessageIDEmptyStr = string.Concat(TranslationManager.ParsedMessage.MessageIDStr, " \"\"");
                TranslationManager.ParsedMessage.MessageIDBeginLength = string.Concat(TranslationManager.ParsedMessage.MessageIDStr, " \"").Length;
            }

            public ParsedMessage(string[] lines)
            {
                int num = -1;
                List<string> strs = new List<string>();
                for (int i = 0; i < (int)lines.Length; i++)
                {
                    string str = lines[i];
                    if (str.StartsWith("#: "))
                    {
                        string str1 = str.Substring(3);
                        int num1 = 0;
                        while (str1.Length > 0)
                        {
                            int num2 = str1.IndexOf(' ', num1);
                            if (num2 >= 0)
                            {
                                string str2 = str1.Substring(0, num2);
                                if (str2.IndexOf(':') > 0 || str2.EndsWith(".mod") || str2.EndsWith(".base") || str2.EndsWith(".quack"))
                                {
                                    strs.Add(str2);
                                    str1 = str1.Substring(num2 + 1);
                                    num1 = 0;
                                }
                                else
                                {
                                    num1 = num2 + 1;
                                }
                            }
                            else
                            {
                                strs.Add(str1);
                                //goto Label0;
                            }
                        }
                    }
                    else if (str.StartsWith(TranslationManager.ParsedMessage.MessageIDStr))
                    {
                        num = i;
                    }
              //  Label0:
                }
                Parallel.For(0, strs.Count, (int r) => strs[r] = strs[r].Replace("\r", string.Empty));
                foreach (string str3 in strs)
                {
                    string[] strArrays = str3.Split(new char[] { ':' });
                    if (strArrays.Length == 0)
                    {
                        continue;
                    }
                    TranslationManager.ParsedMessage.MessagePair messagePair = new TranslationManager.ParsedMessage.MessagePair()
                    {
                        stringId = strArrays[0].Replace('?', ' ')
                    };
                    if ((int)strArrays.Length > 1)
                    {
                        messagePair.key = int.Parse(strArrays[1]);
                    }
                    this.references.Add(messagePair);
                }
                if (num < 0)
                {
                    return;
                }
                string str4 = lines[num];
                if (str4 != TranslationManager.ParsedMessage.MessageIDEmptyStr)
                {
                    this.message = str4.Substring(TranslationManager.ParsedMessage.MessageIDBeginLength, str4.Length - TranslationManager.ParsedMessage.MessageIDBeginLength);
                    if (!this.message.EndsWith("\"") || this.message.EndsWith("\\\""))
                    {
                        StringBuilder stringBuilder = new StringBuilder(this.message);
                        int num3 = num + 1;
                        while (num3 < (int)lines.Length)
                        {
                            stringBuilder.Append(lines[num3]);
                            if (stringBuilder.Length <= 1 || stringBuilder[stringBuilder.Length - 1] != '\"' || stringBuilder[stringBuilder.Length - 2] == '\\')
                            {
                                num3++;
                            }
                            else
                            {
                                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                                break;
                            }
                        }
                        stringBuilder.Replace("\\n", "\n");
                        stringBuilder.Replace("\\\"", "\"");
                        this.message = stringBuilder.ToString();
                    }
                    else
                    {
                        this.message = this.message.Remove(this.message.Length - 1).Replace("\\n", "\n");
                    }
                }
                else
                {
                    this.message = string.Empty;
                    if (num + 1 < (int)lines.Length)
                    {
                        StringBuilder stringBuilder1 = new StringBuilder();
                        for (int j = num + 1; j < (int)lines.Length; j++)
                        {
                            stringBuilder1.Append(lines[j].Trim(new char[] { '\"' }));
                        }
                        stringBuilder1.Replace("\\n", "\n");
                        stringBuilder1.Replace("\\\"", "\"");
                        this.message = stringBuilder1.ToString();
                    }
                }
                this.isValid = true;
            }

            public class MessagePair
            {
                public int key = -1;

                public string stringId = string.Empty;

                public MessagePair()
                {
                }
            }
        }

        public class TranslationCulture
        {
            public CultureInfo culture;

            public string id;

            public string path;

            public TranslationCulture(CultureInfo c, string id, string path)
            {
                this.culture = c;
                this.id = id;
                char directorySeparatorChar = Path.DirectorySeparatorChar;
                this.path = string.Concat(path, directorySeparatorChar.ToString());
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                TranslationManager.TranslationCulture translationCulture = obj as TranslationManager.TranslationCulture;
                if (translationCulture == null)
                {
                    return false;
                }
                if (translationCulture.culture == null && this.culture == null)
                {
                    return translationCulture.id == this.id;
                }
                if (this.culture == null || translationCulture.culture == null)
                {
                    return false;
                }
                return translationCulture.culture.Equals(this.culture);
            }

            public string GetFileName()
            {
                return Path.Combine(this.path, string.Concat(this.id, ".translation"));
            }

            public override int GetHashCode()
            {
                return this.culture.GetHashCode();
            }

            public override string ToString()
            {
                if (this.culture == null)
                {
                    return this.id;
                }
                return this.culture.DisplayName;
            }
        }

        public class TranslationDialogue
        {
            public List<TranslationManager.TranslationDialogueLine> Lines = new List<TranslationManager.TranslationDialogueLine>(2);

            public GameData.Item Item
            {
                get;
                private set;
            }

            public TranslationManager.DialogueTranslationState State
            {
                get;
                private set;
            }

            public TranslationDialogue(GameData.Item item)
            {
                this.Item = item;
                this.State = TranslationManager.DialogueTranslationState.OK;
            }

            public void AddLine(TranslationManager.TranslationDialogueLine line)
            {
                line.RefDialogues.Add(this);
                this.Lines.Add(line);
            }

            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is TranslationManager.TranslationDialogue))
                {
                    return false;
                }
                return ((TranslationManager.TranslationDialogue)obj).Item == this.Item;
            }

            public override int GetHashCode()
            {
                return this.Item.stringID.GetHashCode();
            }

            public override string ToString()
            {
                return this.Item.Name;
            }

            public void UpdateState()
            {
                this.State = TranslationManager.DialogueTranslationState.OK;
                foreach (TranslationManager.TranslationDialogueLine line in this.Lines)
                {
                    TranslationManager.DialogueTranslationState state = line.State;
                    if (state == TranslationManager.DialogueTranslationState.NEW)
                    {
                        this.State = TranslationManager.DialogueTranslationState.NEW;
                        return;
                    }
                    else
                    {
                        if (state != TranslationManager.DialogueTranslationState.ORIGINAL_MODIFIED)
                        {
                            continue;
                        }
                        this.State = line.State;
                    }
                }
            }
        }

        [DebuggerDisplay("Item = {Item.stringID}, Count = {Lines.Count}")]
        public class TranslationDialogueLine
        {
            public GameData.Item Item;

            public TranslationManager.DialogueTranslationState State;

            public List<TranslationManager.TranslationDialogueLine.Line> Lines = new List<TranslationManager.TranslationDialogueLine.Line>();

            public List<TranslationManager.TranslationDialogue> RefDialogues = new List<TranslationManager.TranslationDialogue>();

            private int lastLineIndex;

            public TranslationDialogueLine(GameData.Item item)
            {
                this.Item = item;
                while (true)
                {
                    string str = string.Concat("text", this.lastLineIndex.ToString());
                    if (!this.Item.sdata.ContainsKey(str))
                    {
                        break;
                    }
                    this.CreateLine(str);
                    this.lastLineIndex++;
                }
            }

            private void CreateLine(string key)
            {
                if (string.IsNullOrEmpty(this.Item.sdata[key]) && (this.Item.OriginalValue(key) == null || this.Item.OriginalValue(key).ToString() == ""))
                {
                    return;
                }
                TranslationManager.TranslationDialogueLine.Line line = new TranslationManager.TranslationDialogueLine.Line()
                {
                    Key = key,
                    OriginalKey = string.Concat("_original_", key),
                    Translation = this.Item.sdata[key],
                    State = TranslationManager.DialogueTranslationState.OK
                };
                string.Concat("_user_", line.Key);
                if (!this.Item.sdata.ContainsKey(line.OriginalKey))
                {
                    if (this.Item.OriginalValue(line.Key) != null)
                    {
                        line.State = TranslationManager.DialogueTranslationState.NEW;
                    }
                    else
                    {
                        line.State = TranslationManager.DialogueTranslationState.OK;
                        line.IsUser = true;
                    }
                    line.Original = line.Translation;
                }
                else
                {
                    string item = this.Item.sdata[line.OriginalKey];
                    object obj = this.Item.OriginalValue(line.Key);
                    line.Original = (obj == null ? "" : obj.ToString());
                    if (line.Original != item)
                    {
                        line.State = TranslationManager.DialogueTranslationState.ORIGINAL_MODIFIED;
                    }
                }
                if (line.State == TranslationManager.DialogueTranslationState.NEW && line.Original == line.Translation && Regex.IsMatch(line.Original, "^/[^/]*/$"))
                {
                    line.State = TranslationManager.DialogueTranslationState.OK;
                }
                this.Lines.Add(line);
            }

            public TranslationManager.TranslationDialogueLine.Line CreateUserLine()
            {
                TranslationManager.TranslationDialogueLine.Line line = new TranslationManager.TranslationDialogueLine.Line()
                {
                    Key = string.Concat("text", this.lastLineIndex.ToString()),
                    OriginalKey = string.Concat("_original_", string.Concat("text", this.lastLineIndex.ToString())),
                    Translation = string.Empty,
                    Original = string.Empty,
                    State = TranslationManager.DialogueTranslationState.OK,
                    IsUser = true
                };
                this.Item.sdata[line.Key] = line.Translation;
                this.Item.bdata[string.Concat("_user_", line.Key)] = true;
                this.Lines.Add(line);
                this.lastLineIndex++;
                return line;
            }

            public void RefreshState()
            {
                this.State = TranslationManager.DialogueTranslationState.OK;
                foreach (TranslationManager.TranslationDialogueLine.Line line in this.Lines)
                {
                    if (!(this.Item.sdata[line.Key] != line.Translation) || line.State == TranslationManager.DialogueTranslationState.ORIGINAL_MODIFIED)
                    {
                        continue;
                    }
                    line.Translation = this.Item.sdata[line.Key];
                    line.State = TranslationManager.DialogueTranslationState.OK;
                }
                foreach (TranslationManager.TranslationDialogueLine.Line line1 in this.Lines)
                {
                    if (line1.State != TranslationManager.DialogueTranslationState.NEW)
                    {
                        if (line1.State != TranslationManager.DialogueTranslationState.ORIGINAL_MODIFIED)
                        {
                            continue;
                        }
                        this.State = line1.State;
                    }
                    else
                    {
                        this.State = line1.State;
                        return;
                    }
                }
            }

            public void RemoveUserLine(TranslationManager.TranslationDialogueLine.Line line)
            {
                int num = this.Lines.IndexOf(line);
                if (num < 0)
                {
                    return;
                }
                if (this.lastLineIndex == num + 1)
                {
                    this.lastLineIndex--;
                }
                this.Lines.RemoveAt(num);
                this.Item.Remove(line.Key);
                this.Item.Remove(string.Concat("_user_", line.Key));
            }

            public void UpdateLineState(TranslationManager.TranslationDialogueLine.Line line)
            {
                this.Item.sdata[line.Key] = line.Translation;
                if (!line.IsUser)
                {
                    this.Item.sdata[line.OriginalKey] = line.Original;
                }
                line.State = TranslationManager.DialogueTranslationState.OK;
                this.UpdateState();
            }

            public void UpdateState()
            {
                TranslationManager.DialogueTranslationState state = this.State;
                this.State = TranslationManager.DialogueTranslationState.OK;
                foreach (TranslationManager.TranslationDialogueLine.Line line in this.Lines)
                {
                    if (line.State != TranslationManager.DialogueTranslationState.NEW)
                    {
                        if (line.State != TranslationManager.DialogueTranslationState.ORIGINAL_MODIFIED)
                        {
                            continue;
                        }
                        this.State = line.State;
                    }
                    else
                    {
                        this.State = line.State;
                        if (state != this.State)
                        {
                            foreach (TranslationManager.TranslationDialogue refDialogue in this.RefDialogues)
                            {
                                refDialogue.UpdateState();
                            }
                        }
                        return;
                    }
                }
                if (state != this.State)
                {
                    foreach (TranslationManager.TranslationDialogue translationDialogue in this.RefDialogues)
                    {
                        translationDialogue.UpdateState();
                    }
                }
            }

            [DebuggerDisplay("Text = {Original}")]
            public class Line
            {
                public string Key = string.Empty;

                public string Translation = string.Empty;

                public string Original = string.Empty;

                public string OriginalKey = string.Empty;

                public TranslationManager.DialogueTranslationState State;

                public bool IsUser;

                public bool IsLinked;

                public int LinkedGroup = -1;

                public Line()
                {
                }
            }
        }

        private class TranslationMessage
        {
            public GameData.Item data;

            public string id;

            public string translated = "";

            public List<string> references = new List<string>();

            public TranslationMessage(GameData.Item d, string mId)
            {
                this.data = d;
                this.id = mId;
            }
        }

        private class WordSpawGroup : TranslationManager.DialogueGroup
        {
            public WordSpawGroup(GameData.Item d) : base(d)
            {
            }
        }

        private class WordSpawMessage : TranslationManager.DialogueMessage
        {
            public WordSpawMessage(TranslationManager.WordSpawGroup grp, GameData.Item d)
            {
                this.@group = grp;
                this.data = d;
            }
        }
    }
}