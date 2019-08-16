using forgotten_construction_set.PropertyGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
namespace forgotten_construction_set.Components
{
    partial class ReferenceList : UserControl
    {
        private navigation nav;

        private bool readOnly;

        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ContextMenuStrip contextMenu;

        private ToolStripMenuItem openItem;

        private ToolStripMenuItem removeItem;

        private SplitContainer split;

        private ComboBox addList;

        private Button addButton;

        public PropertyGrid.PropertyGrid grid;

        private Label description;

        private Label selection;

        private ToolStripMenuItem revertItem;

        private SplitContainer splitContainer1;

       

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

       
        private void addButton_Click(object sender, EventArgs e)
        {
            string str = this.addList.SelectedItem.ToString();
            GameData.Desc desc = GameData.getDesc(this.Item.type, str);
            ItemDialog itemDialog = new ItemDialog(string.Concat("选择与 ", str, " 进行关联"), this.nav.ou.gameData, desc.list, true, "", itemType.NULL_ITEM);
            if (itemDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (GameData.Item item in itemDialog.Items)
                {
                    int? nullable = null;
                    int? nullable1 = nullable;
                    nullable = null;
                    int? nullable2 = nullable;
                    nullable = null;
                    this.Item.addReference(str, item, nullable1, nullable2, nullable);
                }
                if (itemDialog.Items.Count > 0)
                {
                    this.refresh(this.Item);
                    this.nav.refreshState(this.Item);
                    this.nav.HasChanges = true;
                    if (this.ChangeEvent != null)
                    {
                        this.ChangeEvent(this);
                    }
                }
            }
        }

        private void addList_Enter(object sender, EventArgs e)
        {
            this.addList_SelectedIndexChanged(sender, e);
        }

        private void addList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameData.Desc desc = GameData.getDesc(this.Item.type, this.addList.Text);
            this.SelectionControl.Text = this.addList.Text;
            this.DescriptionControl.Text = desc.description;
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (this.grid.SelectedItem == null)
            {
                e.Cancel = true;
                return;
            }
            bool textColour = this.grid.SelectedItem.TextColour != StateColours.GetStateColor(GameData.State.LOCKED);
            this.removeItem.Enabled = textColour;
            this.revertItem.Enabled = textColour;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void grid_DoubleClick(object sender, EventArgs e)
        {
            this.openItem_Click(sender, e);
        }

        private void grid_OnPropertyChanged(object sender, PropertyChangedArgs e)
        {
            string str = e.Item.Data.ToString();
            this.Item.setReferenceValue(e.Section.Name, str, (GameData.TripleInt)e.Item.Value);
            e.Item.TextColour = StateColours.GetStateColor(this.Item.getState(e.Section.Name, str));
            this.nav.refreshState(this.Item);
            this.nav.HasChanges = true;
        }

        private void grid_OnPropertySelected(object sender, PropertySelectedArgs e)
        {
            this.SelectionControl.Text = e.Section.Name;
            this.DescriptionControl.Text = e.Item.Description;
            this.revertItem.Visible = this.Item.getState(e.Section.Name, e.Item.Data.ToString()) == GameData.State.MODIFIED;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openItem = new System.Windows.Forms.ToolStripMenuItem();
            this.revertItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.split = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.addList = new System.Windows.Forms.ComboBox();
            this.addButton = new System.Windows.Forms.Button();
            this.grid = new PropertyGrid.PropertyGrid();
            this.description = new System.Windows.Forms.Label();
            this.selection = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openItem,
            this.revertItem,
            this.removeItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(101, 70);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // openItem
            // 
            this.openItem.Name = "openItem";
            this.openItem.Size = new System.Drawing.Size(100, 22);
            this.openItem.Text = "打开";
            this.openItem.Click += new System.EventHandler(this.openItem_Click);
            // 
            // revertItem
            // 
            this.revertItem.Name = "revertItem";
            this.revertItem.Size = new System.Drawing.Size(100, 22);
            this.revertItem.Text = "还原";
            this.revertItem.Click += new System.EventHandler(this.revertItem_Click);
            // 
            // removeItem
            // 
            this.removeItem.Name = "removeItem";
            this.removeItem.Size = new System.Drawing.Size(100, 22);
            this.removeItem.Text = "移除";
            this.removeItem.Click += new System.EventHandler(this.removeItem_Click);
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.split.IsSplitterFixed = true;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Name = "split";
            this.split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.splitContainer1);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.description);
            this.split.Panel2.Controls.Add(this.selection);
            this.split.Panel2.Margin = new System.Windows.Forms.Padding(3);
            this.split.Size = new System.Drawing.Size(221, 194);
            this.split.SplitterDistance = 152;
            this.split.TabIndex = 36;
            this.split.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.addList);
            this.splitContainer1.Panel1.Controls.Add(this.addButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grid);
            this.splitContainer1.Size = new System.Drawing.Size(221, 153);
            this.splitContainer1.SplitterDistance = 35;
            this.splitContainer1.TabIndex = 38;
            // 
            // addList
            // 
            this.addList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addList.FormattingEnabled = true;
            this.addList.Location = new System.Drawing.Point(6, 8);
            this.addList.Name = "addList";
            this.addList.Size = new System.Drawing.Size(148, 20);
            this.addList.TabIndex = 37;
            this.addList.SelectedIndexChanged += new System.EventHandler(this.addList_SelectedIndexChanged);
            this.addList.Enter += new System.EventHandler(this.addList_Enter);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addButton.Location = new System.Drawing.Point(160, 8);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(56, 19);
            this.addButton.TabIndex = 34;
            this.addButton.Text = "添加";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // grid
            // 
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.AutoScroll = true;
            this.grid.ContextMenuStrip = this.contextMenu;
            this.grid.LineHeight = 17;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(221, 114);
            this.grid.TabIndex = 0;
           
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Location = new System.Drawing.Point(12, 12);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(206, 26);
            this.description.TabIndex = 1;
            this.description.Text = "描述";
            // 
            // selection
            // 
            this.selection.AutoSize = true;
            this.selection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selection.Location = new System.Drawing.Point(3, 0);
            this.selection.Name = "selection";
            this.selection.Size = new System.Drawing.Size(85, 13);
            this.selection.TabIndex = 0;
            this.selection.Text = "已选择的项目";
            // 
            // ReferenceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.split);
            this.Name = "ReferenceList";
            this.Size = new System.Drawing.Size(221, 194);
            this.contextMenu.ResumeLayout(false);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            this.split.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void openItem_Click(object sender, EventArgs e)
        {
            if (this.grid.SelectedItem != null && this.grid.SelectedSection.Name != "Instances")
            {
                GameData.Item item = this.nav.ou.gameData.getItem(this.grid.SelectedItem.Data.ToString());
                this.nav.showItemProperties(item);
            }
        }

        public void refresh(GameData.Item item)
        {
            this.Item = item;
            this.grid.AddPropertyType(typeof(GameData.Instance), new InstanceProperty(item, this.nav));
            this.refreshAddList();
            this.refreshGrid();
        }

        public void refreshAddList()
        {
            this.addList.Items.Clear();
            if (!this.readOnly)
            {
                if (this.Item != null && this.Item.getState() != GameData.State.LOCKED && GameData.desc.ContainsKey(this.Item.type))
                {
                    foreach (KeyValuePair<string, GameData.Desc> item in GameData.desc[this.Item.type])
                    {
                        if (!(item.Value.defaultValue is GameData.TripleInt) || this.Exclusions.Contains(item.Key))
                        {
                            continue;
                        }
                        this.addList.Items.Add(item.Key);
                    }
                }
                if (this.addList.Items.Count > 0)
                {
                    this.addList.SelectedIndex = 0;
                }
            }
            this.addList.Enabled = this.addList.Items.Count > 0;
            this.addButton.Enabled = this.addList.Items.Count > 0;
        }

        public void refreshGrid()
        {
            this.grid.clear();
            if (this.Item == null)
            {
                return;
            }
            foreach (string str in this.Item.referenceLists())
            {
                GameData.Desc desc = GameData.getDesc(this.Item.type, str);
                TripleIntProperty tripleIntProperty = new TripleIntProperty(desc.flags);
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in this.Item.referenceData(str, false))
                {
                    if (this.Exclusions.Contains(str))
                    {
                        continue;
                    }
                    GameData.Item item = this.nav.ou.gameData.getItem(keyValuePair.Key);
                    GameData.State state = this.Item.getState(str, keyValuePair.Key);
                    string str1 = (item != null ? item.Name : keyValuePair.Key);
                    Color stateColor = StateColours.GetStateColor(state);
                    PropertyGrid.PropertyGrid.Item key = this.grid.addItem(str, str1, keyValuePair.Value, desc.description, new Color?(stateColor), desc.flags > 0);
                    key.Property = tripleIntProperty;
                    key.Data = keyValuePair.Key;
                    key.Editable = state != GameData.State.LOCKED;
                }
            }
            foreach (KeyValuePair<string, GameData.Instance> keyValuePair1 in this.Item.instanceData())
            {
                bool flag = (keyValuePair1.Value.getState() == GameData.State.LOCKED ? false : !this.readOnly);
                Color color = StateColours.GetStateColor(keyValuePair1.Value.getState());
                this.grid.addItem("Instances", keyValuePair1.Key, keyValuePair1.Value, "Object instances", new Color?(color), flag);
            }
            this.grid.AutosizeDivider();
        }

        private void removeItem_Click(object sender, EventArgs e)
        {
            if (this.grid.SelectedItem != null)
            {
                string str = this.grid.SelectedItem.Data.ToString();
                string name = this.grid.SelectedSection.Name;
                this.Item.removeReference(name, str);
                this.grid.removeItem(name, this.grid.SelectedItem);
                if (this.grid.getSection(name).Items.Count == 0)
                {
                    this.grid.removeSection(name);
                }
                this.nav.refreshState(this.Item);
                this.nav.HasChanges = true;
                if (this.ChangeEvent != null)
                {
                    this.ChangeEvent(this);
                }
            }
        }

        private void revertItem_Click(object sender, EventArgs e)
        {
        }

        public void setCustomDescriptionControls(Control title, Control desc)
        {
            this.ShowDescription = false;
            this.SelectionControl = title;
            this.DescriptionControl = desc;
        }

        public void setup(GameData.Item item, navigation nv)
        {
            this.nav = nv;
            this.refresh(item);
        }

        public event ReferenceList.ChangeNotifier ChangeEvent;

        public delegate void ChangeNotifier(object sender);
    }
}

