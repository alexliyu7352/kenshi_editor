using forgotten_construction_set.dialog;
using NLog;
using PropertyGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
    partial class ObjectPropertyBox : UserControl
    {
        private navigation nav;

        private Conversation callbackC;

        private Dictionary<string, List<KeyValuePair<string, GameData.Desc>>> conditionKeys;

        private IContainer components;

        private SplitContainer split;

        private Label selection;

        private Label description;

        public PropertyGrid.PropertyGrid grid;

        private ContextMenuStrip contextMenu;

        private ToolStripMenuItem openFile;

        private ToolStripMenuItem openFolder;

        private ToolStripMenuItem swapTextures;

        private ToolStripMenuItem copyValueToolStripMenuItem;

        private ToolStripMenuItem copyKeyToolStripMenuItem;

        private ToolStripMenuItem resetValue;



        public bool ShowDescription
        {
            get
            {
                return !this.split.Panel2Collapsed;
            }
            set
            {
                this.split.Panel2Collapsed = !value;
            }
        }



        public void callMeBack(Conversation c)
        {
            this.callbackC = c;
        }

        private object castItemValue(string key)
        {
            object bah = this.Item[key];
            if (!GameData.desc.ContainsKey(this.Item.type))
            {
                return bah;
            }
            GameData.Desc item = GameData.desc[this.Item.type][key];
            if (item.defaultValue != null && item.defaultValue.GetType().IsEnum)
            {
                bah = (bah is int || bah.GetType().IsEnum ? Enum.ToObject(item.defaultValue.GetType(), bah) : item.defaultValue);
            }
            if (item.defaultValue is Color && bah is int)
            {
                bah = Color.FromArgb(255, Color.FromArgb((int)bah));
            }
            return bah;
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (this.grid.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }
            if (!(this.grid.SelectedItem.Value is GameData.File))
            {
                this.openFile.Visible = false;
                this.openFolder.Visible = false;
                this.swapTextures.Visible = false;
            }
            else
            {
                this.openFile.Visible = true;
                this.openFolder.Visible = true;
                this.swapTextures.Visible = false;
                if (this.grid.SelectedItem.Editable)
                {
                    if (this.grid.SelectedItem.Name.StartsWith("texture map") && this.Item.ContainsKey("texture map 2"))
                    {
                        this.swapTextures.Visible = true;
                    }
                    else if (this.grid.SelectedItem.Name.StartsWith("normal map") && this.Item.ContainsKey("normal map 2"))
                    {
                        this.swapTextures.Visible = true;
                    }
                    else if (this.grid.SelectedItem.Name.StartsWith("metalness map") && this.Item.ContainsKey("metalness map 2"))
                    {
                        this.swapTextures.Visible = true;
                    }
                }
            }
            string name = this.grid.SelectedItem.Name;
            if (name == "Object Type" || name == "String ID")
            {
                this.resetValue.Visible = false;
                return;
            }
            if (name == "Name")
            {
                this.resetValue.Visible = this.Item.Name != this.Item.OriginalName;
                this.resetValue.Text = "Revert Value";
                return;
            }
            if (this.nav.FileMode == navigation.ModFileMode.SINGLE || this.Item.OriginalValue(this.grid.SelectedItem.Name) == null)
            {
                this.resetValue.Visible = true;
                this.resetValue.Text = "Delete Value";
                return;
            }
            this.resetValue.Visible = this.Item.getState(this.grid.SelectedItem.Name) == GameData.State.MODIFIED;
            this.resetValue.Text = "Revert Value";
        }

        private void copyKey_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.grid.SelectedItem.Name);
        }

        private void copyValue_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.grid.SelectedItem.Value.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void grid_OnPropertyChanged(object sender, PropertyChangedArgs e)
        {
            GameData.Desc desc;
            if (e.Item.Value == null)
            {
                e.Item.Value = e.OldValue;
                return;
            }
            if (e.Item.Name == "Name")
            {
                this.Item.Name = (string)e.Item.Value;
                this.grid.SelectedItem.TextColour = StateColours.GetStateColor(this.Item.getNameState());
            }
            else if (e.Item.Name != "String ID")
            {
                if (e.Item.Value.GetType().IsEnum)
                {
                    this.Item[e.Item.Name] = (int)e.Item.Value;
                }
                else if (e.Item.Value is Color)
                {
                    GameData.Item item = this.Item;
                    string name = e.Item.Name;
                    Color value = (Color)e.Item.Value;
                    item[name] = value.ToArgb() & 16777215;
                }
                else if (!(e.Item.Property is FileProperty))
                {
                    this.Item[e.Item.Name] = e.Item.Value;
                }
                else
                {
                    string str = this.validateFile(e.Item.Value.ToString());
                    if (str == null)
                    {
                        MessageBox.Show("Files must be somewhere in the game data directory or in your mod directory", "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        e.Item.Value = e.OldValue;
                    }
                    else
                    {
                        e.Item.Value = new GameData.File(str);
                        this.Item[e.Item.Name] = e.Item.Value;
                    }
                }
                GameData.State state = this.Item.getState(e.Item.Name);
                e.Item.TextColour = StateColours.GetStateColor(state);
            }
            else
            {
                this.nav.ou.gameData.changeID(this.Item, (string)e.Item.Value);
            }
            this.nav.refreshState(this.Item);
            this.nav.HasChanges = true;
            if (this.Item.isLooper(e.Item.Name, out desc))
            {
                this.ProcessLooper(e.Item.Name, desc);
            }
            this.updateConditions(e.Item.Name);
        }

        private void grid_OnPropertySelected(object sender, PropertySelectedArgs e)
        {
            this.SelectionControl.Text = e.Item.Name;
            this.DescriptionControl.Text = e.Item.Description;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.split = new SplitContainer();
            this.grid = new PropertyGrid.PropertyGrid();
            this.contextMenu = new ContextMenuStrip(this.components);
            this.openFile = new ToolStripMenuItem();
            this.openFolder = new ToolStripMenuItem();
            this.swapTextures = new ToolStripMenuItem();
            this.copyKeyToolStripMenuItem = new ToolStripMenuItem();
            this.copyValueToolStripMenuItem = new ToolStripMenuItem();
            this.resetValue = new ToolStripMenuItem();
            this.description = new Label();
            this.selection = new Label();
            ((ISupportInitialize)this.split).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.contextMenu.SuspendLayout();
            base.SuspendLayout();
            this.split.Dock = DockStyle.Fill;
            this.split.Location = new Point(0, 0);
            this.split.Name = "split";
            this.split.Orientation = Orientation.Horizontal;
            this.split.Panel1.Controls.Add(this.grid);
            this.split.Panel2.Controls.Add(this.description);
            this.split.Panel2.Controls.Add(this.selection);
            this.split.Size = new Size(239, 281);
            this.split.SplitterDistance = 222;
            this.split.TabIndex = 1;
            this.split.TabStop = false;
            //this.split.IsSplitterFixed = true;
            

            this.grid.AutoScroll = true;
            this.grid.AutoScrollMinSize = new Size(0, 100);
            this.grid.ContextMenuStrip = this.contextMenu;
            this.grid.Dock = DockStyle.Fill;
            this.grid.Location = new Point(0, 0);
            this.grid.Name = "grid";
            this.grid.Size = new Size(239, 222);
            this.grid.SortSections = true;
            this.grid.TabIndex = 0;
            this.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
            this.grid.OnPropertySelected += new PropertyGrid.PropertyGrid.PropertySelectedHandler(this.grid_OnPropertySelected);
            this.contextMenu.Items.AddRange(new ToolStripItem[] { this.openFile, this.openFolder, this.swapTextures, this.copyKeyToolStripMenuItem, this.copyValueToolStripMenuItem, this.resetValue });
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new Size(153, 158);
            this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
            this.openFile.Name = "openFile";
            this.openFile.Size = new Size(152, 22);
            this.openFile.Text = "Open File";
            this.openFile.Click += new EventHandler(this.openFile_Click);
            this.openFolder.Name = "openFolder";
            this.openFolder.Size = new Size(152, 22);
            this.openFolder.Text = "Open Folder";
            this.openFolder.Click += new EventHandler(this.openFolder_Click);
            this.swapTextures.Name = "swapTextures";
            this.swapTextures.Size = new Size(152, 22);
            this.swapTextures.Text = "Swap Textures";
            this.swapTextures.Click += new EventHandler(this.swapTextures_Click);
            this.copyKeyToolStripMenuItem.Name = "copyKeyToolStripMenuItem";
            this.copyKeyToolStripMenuItem.Size = new Size(152, 22);
            this.copyKeyToolStripMenuItem.Text = "Copy Key";
            this.copyKeyToolStripMenuItem.Click += new EventHandler(this.copyKey_Click);
            this.copyValueToolStripMenuItem.Name = "copyValueToolStripMenuItem";
            this.copyValueToolStripMenuItem.Size = new Size(152, 22);
            this.copyValueToolStripMenuItem.Text = "Copy Value";
            this.copyValueToolStripMenuItem.Click += new EventHandler(this.copyValue_Click);
            this.resetValue.Name = "resetValue";
            this.resetValue.Size = new Size(152, 22);
            this.resetValue.Text = "Reset Value";
            this.resetValue.Click += new EventHandler(this.resetValue_Click);
            this.description.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.description.Location = new Point(12, 13);
            this.description.Name = "description";
            this.description.Size = new Size(224, 42);
            this.description.TabIndex = 1;
            this.description.Text = "Description";
            this.selection.AutoSize = true;
            this.selection.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.selection.Location = new Point(3, 0);
            this.selection.Name = "selection";
            this.selection.Size = new Size(85, 13);
            this.selection.TabIndex = 0;
            this.selection.Text = "Selected Item";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.split);
            base.Name = "ObjectPropertyBox";
            base.Size = new Size(239, 281);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            this.split.Panel2.PerformLayout();
            ((ISupportInitialize)this.split).EndInit();
            this.split.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            AssetList.OpenFile(this.grid.SelectedItem.Value.ToString());
        }

        private void openFolder_Click(object sender, EventArgs e)
        {
            AssetList.OpenFolder(this.grid.SelectedItem.Value.ToString());
        }

        private void ProcessLooper(string name, GameData.Desc desc)
        {
            if (this.Item.getState() == GameData.State.LOCKED)
            {
                return;
            }
            while (char.IsDigit(name[name.Length - 1]))
            {
                name = name.Remove(name.Length - 1);
            }
            if (desc.limit == 0)
            {
                return;
            }
            int num = 0;
            while (true)
            {
                PropertyGrid.PropertyGrid.Item item = this.grid.getItem(desc.category, string.Concat(name, num));
                if (item == null)
                {
                    Color? nullable = null;
                    this.grid.addItem(desc.category, string.Concat(name, num), desc.defaultValue, desc.description, nullable, true);
                    return;
                }
                if (item.Value.Equals(desc.defaultValue))
                {
                    break;
                }
                num++;
            }
        }

        public void refresh(GameData.Item item)
        {
            GameData.Desc desc;
            Color? nullable;
            SortedList<string, GameData.Desc> strs;
            this.Item = item;
            this.grid.clear();
            if (item == null)
            {
                return;
            }
            this.grid.addItem("Base", "Name", item.Name, "Item name", new Color?(StateColours.GetStateColor(item.getNameState())), item.getNameState() != GameData.State.LOCKED);
            this.grid.addItem("Base", "Object Type", item.type, "GameData Type", new Color?(SystemColors.GrayText), false);
            if (item.getState() != GameData.State.OWNED)
            {
                this.grid.addItem("Base", "String ID", item.stringID, "The string ID is a unique identifier for each item, it must never be the same for 2 objects.", new Color?(SystemColors.GrayText), false);
            }
            else
            {
                nullable = null;
                this.grid.addItem("Base", "String ID", item.stringID, "The string ID is a unique identifier for each item, it must never be the same for 2 objects. DO NOT CHANGE IT if already placed in the game world or if referenced by another mod (otherwise those references will vanish). Generally you can only change this for newly created items", nullable, true);
            }
            if (GameData.desc.ContainsKey(item.type))
            {
                strs = GameData.desc[item.type];
            }
            else
            {
                strs = null;
            }
            SortedList<string, GameData.Desc> strs1 = strs;
            foreach (KeyValuePair<string, object> keyValuePair in item)
            {
                object value = keyValuePair.Value;
                if (!item.isLooper(keyValuePair.Key, out desc))
                {
                    desc = (strs1 == null || !strs1.ContainsKey(keyValuePair.Key) ? GameData.nullDesc : strs1[keyValuePair.Key]);
                }
                if (desc.defaultValue != null && desc.defaultValue.GetType().IsEnum)
                {
                    value = (value is int || value.GetType().IsEnum ? Enum.ToObject(desc.defaultValue.GetType(), value) : desc.defaultValue);
                }
                if (desc.defaultValue is Color && keyValuePair.Value is int)
                {
                    value = Color.FromArgb(255, Color.FromArgb((int)keyValuePair.Value));
                }
                nullable = null;
                PropertyGrid.PropertyGrid.Item extendedText = this.grid.addItem(desc.category, keyValuePair.Key, value, desc.description, nullable, true);
                if (value is string && desc != null && desc.flags == 16)
                {
                    extendedText.Property = new ExtendedText();
                }
                if (item.getState(keyValuePair.Key) == GameData.State.LOCKED)
                {
                    extendedText.Editable = false;
                }
                else if (keyValuePair.Value is GameData.File)
                {
                    extendedText.Property = new FileProperty(keyValuePair.Key, desc.mask);
                }
                if (desc.defaultValue is EnumValue)
                {
                    extendedText.Property = new FCSEnumProperty((desc.defaultValue as EnumValue).Enum);
                }
                if (desc.defaultValue != null && (desc.defaultValue.GetType().IsEnum || desc.defaultValue is EnumValue) && desc.flags == 256)
                {
                    extendedText.Property = new BitSetProperty(desc.defaultValue, false);
                }
                GameData.State state = item.getState(keyValuePair.Key);
                extendedText.TextColour = StateColours.GetStateColor(state);
            }
            this.grid.AutosizeDivider();
            this.grid.SortItems = true;
            if (strs1 != null)
            {
                foreach (KeyValuePair<string, GameData.Desc> keyValuePair1 in strs1)
                {
                    if (keyValuePair1.Value.limit <= 0)
                    {
                        continue;
                    }
                    this.ProcessLooper(keyValuePair1.Key, keyValuePair1.Value);
                }
            }
            PropertyGrid.PropertyGrid.Section section = this.grid.getSection("Base");
            if (this.grid.Sections[0] != section)
            {
                this.grid.Sections.Remove(section);
                this.grid.Sections.Insert(0, section);
            }
        }

        private void resetValue_Click(object sender, EventArgs e)
        {
            string name = this.grid.SelectedItem.Name;
            if (name != "Name")
            {
                if (!this.Item.ContainsKey(name))
                {
                    return;
                }
                if (this.Item.getState() != GameData.State.OWNED || this.nav.FileMode != navigation.ModFileMode.SINGLE)
                {
                    object obj = this.Item.OriginalValue(name);
                    if (obj == null)
                    {
                        this.grid.removeItem(this.grid.SelectedSection.Name, name);
                        if (this.grid.SelectedSection.Items.Count == 0)
                        {
                            this.grid.removeSection(this.grid.SelectedSection.Name);
                        }
                    }
                    else
                    {
                        this.Item[name] = obj;
                        object value = this.grid.SelectedItem.Value;
                        if (value is Color)
                        {
                            this.grid.SelectedItem.Value = Color.FromArgb(255, Color.FromArgb((int)obj));
                        }
                        else if (!value.GetType().IsEnum)
                        {
                            this.grid.SelectedItem.Value = this.Item.OriginalValue(name);
                        }
                        else
                        {
                            this.grid.SelectedItem.Value = Enum.ToObject(value.GetType(), obj);
                        }
                        this.grid.SelectedItem.TextColour = StateColours.GetStateColor(this.Item.getState(name));
                    }
                }
                else
                {
                    this.Item.Remove(name);
                    this.grid.removeItem(this.grid.SelectedSection.Name, name);
                    if (this.grid.SelectedSection.Items.Count == 0)
                    {
                        this.grid.removeSection(this.grid.SelectedSection.Name);
                    }
                }
            }
            else
            {
                this.Item.Name = this.Item.OriginalName;
                this.grid.SelectedItem.Value = this.Item.Name;
                this.grid.SelectedItem.TextColour = StateColours.GetStateColor(this.Item.getNameState());
            }
            this.grid.Invalidate();
            this.nav.refreshState(this.Item);
            this.nav.HasChanges = true;
        }

        public void setCustomDescriptionControls(Control title, Control desc)
        {
            this.ShowDescription = false;
            this.SelectionControl = title;
            this.DescriptionControl = desc;
        }

        public void setup(GameData.Item item, navigation nv)
        {
            this.Item = item;
            this.nav = nv;
            this.refresh(item);
            this.setupConditions();
        }

        private void setupConditions()
        {
            if (this.Item != null && GameData.desc.ContainsKey(this.Item.type))
            {
                this.conditionKeys = new Dictionary<string, List<KeyValuePair<string, GameData.Desc>>>();
                foreach (KeyValuePair<string, GameData.Desc> item in GameData.desc[this.Item.type])
                {
                    if (item.Value.condition == null)
                    {
                        continue;
                    }
                    if (!this.conditionKeys.ContainsKey(item.Value.condition.key))
                    {
                        this.conditionKeys[item.Value.condition.key] = new List<KeyValuePair<string, GameData.Desc>>();
                    }
                    this.conditionKeys[item.Value.condition.key].Add(item);
                }
                foreach (string key in this.conditionKeys.Keys)
                {
                    this.updateConditions(key);
                }
            }
        }

        private void swapTextures_Click(object sender, EventArgs e)
        {
            object bah = this.Item["texture map"];
            this.Item["texture map"] = this.Item["texture map 2"];
            this.Item["texture map 2"] = bah;
            bah = this.Item["normal map"];
            this.Item["normal map"] = this.Item["normal map 2"];
            this.Item["normal map 2"] = bah;
            bool flag = this.Item.ContainsKey("metalness map 2");
            if (flag)
            {
                bah = this.Item["metalness map"];
                this.Item["metalness map"] = this.Item["metalness map 2"];
                this.Item["metalness map 2"] = bah;
            }
            this.grid.getItem("texture map").Value = this.Item["texture map"];
            this.grid.getItem("texture map 2").Value = this.Item["texture map 2"];
            this.grid.getItem("normal map").Value = this.Item["normal map"];
            this.grid.getItem("normal map 2").Value = this.Item["normal map 2"];
            this.grid.getItem("texture map").TextColour = StateColours.GetStateColor(this.Item.getState("texture map"));
            this.grid.getItem("normal map").TextColour = StateColours.GetStateColor(this.Item.getState("normal map"));
            this.grid.getItem("texture map 2").TextColour = StateColours.GetStateColor(this.Item.getState("texture map 2"));
            this.grid.getItem("normal map 2").TextColour = StateColours.GetStateColor(this.Item.getState("normal map 2"));
            if (flag)
            {
                this.grid.getItem("metalness map").Value = this.Item["metalness map"];
                this.grid.getItem("metalness map 2").Value = this.Item["metalness map 2"];
                this.grid.getItem("metalness map").TextColour = StateColours.GetStateColor(this.Item.getState("metalness map"));
                this.grid.getItem("metalness map 2").TextColour = StateColours.GetStateColor(this.Item.getState("metalness map 2"));
            }
            this.grid.Invalidate();
            this.nav.HasChanges = true;
        }

        private bool testCondition(GameData.DescCondition c)
        {
            bool flag;
            if (c == null)
            {
                return true;
            }
            object obj = this.castItemValue(c.key);
            if (!c.values.GetType().IsArray)
            {
                return c.values.Equals(obj) == c.match;
            }
            IEnumerator enumerator = ((IEnumerable)c.values).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    if (!enumerator.Current.Equals(obj))
                    {
                        continue;
                    }
                    flag = c.match;
                    return flag;
                }
                return !c.match;
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return flag;
        }

        private void updateConditions(string changedKey)
        {
            if (this.conditionKeys == null || !this.conditionKeys.ContainsKey(changedKey))
            {
                return;
            }
            foreach (KeyValuePair<string, GameData.Desc> item in this.conditionKeys[changedKey])
            {
                bool flag = this.testCondition(item.Value.condition);
                if (flag != (this.grid.getItem(item.Key) == null) || !this.Item.ContainsKey(item.Key))
                {
                    continue;
                }
                if (!flag)
                {
                    this.grid.removeItem(item.Value.category, item.Key);
                }
                else
                {
                    Color? nullable = null;
                    this.grid.addItem(item.Value.category, item.Key, this.Item[item.Key], item.Value.description, nullable, true);
                }
            }
        }

        private string validateFile(string file)
        {
            if (file == "")
            {
                return "";
            }
            if (this.nav.FileMode == navigation.ModFileMode.SINGLE)
            {
                return file;
            }
            if (file.StartsWith(this.nav.BasePath) || file.StartsWith(this.nav.ModPath))
            {
                return string.Concat(".", file.Substring(this.nav.RootPath.Length));
            }
            if (!file.StartsWith("."))
            {
                baseForm.logger.Info(string.Concat(new string[] { "File '", file, " not in data or mod directoy [Root:", this.nav.RootPath, "]" }));
                return null;
            }
            string str = string.Concat(".", this.nav.BasePath.Substring(this.nav.RootPath.Length));
            string str1 = string.Concat(".", this.nav.ModPath.Substring(this.nav.RootPath.Length));
            if (file.StartsWith(str) || file.StartsWith(str1))
            {
                return file;
            }
            if (file.StartsWith(str.Replace('\\', '/')))
            {
                return file;
            }
            if (file.StartsWith(str1.Replace('\\', '/')))
            {
                return file;
            }
            baseForm.logger.Info(string.Concat("File '", file, " not in data or mod directoy"));
            return null;
        }
    }
}
