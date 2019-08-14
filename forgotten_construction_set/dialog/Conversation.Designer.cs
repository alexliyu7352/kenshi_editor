using forgotten_construction_set.Components;
using PropertyGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
namespace forgotten_construction_set.dialog
{
    partial class Conversation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public navigation nav;
        private GameData gameData;
        private System.Windows.Forms.ContextMenuStrip contextMenu;

        private ToolStripMenuItem deleteItem;

        private ListView listView2;

        private ColumnHeader columnHeader2;

        private TextBox conversationName;

        private Label label1;

        private SplitContainer splitContainer1;

        private Button buttonAddEffect;

        private ComboBox PossibleEffects;

        private Button buttonRemoveEffect;

        private ColumnHeader columnHeader3;

        private NumericUpDown effectValue;

        private CheckBox checkBox1;

        private CheckBox checkBox2;

        protected ConditionControl conditionControl1;

        private ObjectPropertyBox lineProperties;

        protected Button btnAddChild;

        protected Button btnAddInterjection;

        private ReferenceList referenceList1;

        private SplitContainer splitContainer2;

        protected GroupBox effectsPanel;

        protected ToolStripMenuItem addChild;

        protected ToolStripMenuItem addInterjection;

        private ToolStripSeparator toolStripSeparator1;

        private ToolStripMenuItem deleteBranch;

        private ToolStripMenuItem expandAll;

        private ToolStripSeparator toolStripSeparator2;

        protected TreeView tree;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        protected void addChild_Click(object sender, EventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }
            GameData.Item item = this.gameData.createItem(itemType.DIALOGUE_LINE);
            int? nullable = null;
            int? nullable1 = nullable;
            nullable = null;
            int? nullable2 = nullable;
            nullable = null;
            this.SelectedItem.addReference("lines", item, nullable1, nullable2, nullable);
            TreeNode treeNode = this.tree.SelectedNode.Nodes.Add(item.stringID, "");
            treeNode.Tag = item;
            Conversation.refreshNode(treeNode);
            this.tree.SelectedNode = treeNode;
            this.countLines();
        }

        private void addInterjection_Click(object sender, EventArgs e)
        {
            if (this.SelectedItem == null)
            {
                return;
            }
            GameData.Item item = this.gameData.createItem(itemType.DIALOGUE_LINE);
            int? nullable = null;
            int? nullable1 = nullable;
            nullable = null;
            int? nullable2 = nullable;
            nullable = null;
            this.SelectedItem.addReference("lines", item, nullable1, nullable2, nullable);
            item["interjection"] = true;
            item["speaker"] = 3;
            item.Remove("text0");
            TreeNode treeNode = this.tree.SelectedNode.Nodes.Add(item.stringID, "");
            treeNode.Tag = item;
            Conversation.refreshNode(treeNode);
            this.tree.SelectedNode = treeNode;
            this.countLines();
        }

        private void buttonAddEffect_Click(object sender, EventArgs e)
        {
            DialogActionEnum selectedIndex = (DialogActionEnum)this.PossibleEffects.SelectedIndex;
            if (selectedIndex <= DialogActionEnum.DA_NONE)
            {
                return;
            }
            if (this.tree.SelectedNode == null)
            {
                return;
            }
            GameData.Item item = this.gameData.createItem(itemType.DIALOG_ACTION);
            item["action name"] = (int)selectedIndex;
            int? nullable = null;
            int? nullable1 = nullable;
            nullable = null;
            this.gameData.getItem(this.tree.SelectedNode.Name).addReference("effects", item, new int?((int)this.effectValue.Value), nullable1, nullable);
            this.refreshEffectsList();
            Conversation.refreshNode(this.tree.SelectedNode);
            this.nav.HasChanges = true;
        }

        private void buttonRemoveEffect_Click(object sender, EventArgs e)
        {
            if (this.listView2.SelectedItems.Count == 0)
            {
                return;
            }
            int index = this.listView2.SelectedItems[0].Index;
            GameData.Item tag = (GameData.Item)this.tree.SelectedNode.Tag;
            if (tag == null)
            {
                return;
            }
            tag.removeReference("effects", index);
            this.refreshEffectsList();
            Conversation.refreshNode(this.tree.SelectedNode);
            this.nav.HasChanges = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.Item["once only"] = ((CheckBox)sender).Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.Item["for enemies"] = ((CheckBox)sender).Checked;
        }

        private void conditionControl1_ChangeEvent(object sender)
        {
            Conversation.refreshNode(this.tree.SelectedNode);
            this.nav.HasChanges = true;
        }

        private void conversationName_TextChanged(object sender, EventArgs e)
        {
            this.Item.Name = ((TextBox)sender).Text;
            this.nav.refreshListView();
        }

        public int countLines()
        {
            List<GameData.Item> items = new List<GameData.Item>()
            {
                this.Item
            };
            int num = -1;
            int num1 = 0;
            while (items.Count > 0)
            {
                num++;
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in items[0].referenceData("lines", false))
                {
                    GameData.Item item = this.nav.ou.gameData.getItem(keyValuePair.Key);
                    if (keyValuePair.Value.v0 == 50 || item == null)
                    {
                        num++;
                    }
                    else if (item != null)
                    {
                        items.Add(item);
                    }
                    if (item != null)
                    {
                        continue;
                    }
                    num1++;
                }
                items.RemoveAt(0);
            }
            int nodeCount = this.tree.GetNodeCount(true);
            object[] objArray = new object[] { "Dialogue (", nodeCount, " lines", null, null };
            objArray[3] = (num1 > 0 ? string.Concat(", ", num1, " errors") : "");
            objArray[4] = ")";
            this.Text = string.Concat(objArray);
            return nodeCount;
        }

        public static void createConversationTree(GameData source, GameData.Item item, TreeNodeCollection n)
        {
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in item.referenceData("lines", false))
            {
                GameData.Item item1 = source.getItem(keyValuePair.Key);
                if (item1 != null)
                {
                    TreeNode red = n.Add(keyValuePair.Key, "");
                    red.Tag = item1;
                    Conversation.refreshNode(red);
                    if (item1.getState() == GameData.State.REMOVED)
                    {
                        red.BackColor = Color.Red;
                        red.ForeColor = Color.White;
                        TreeNode treeNode = red;
                        treeNode.Text = string.Concat(treeNode.Text, " (Error: line removed)");
                    }
                    if (keyValuePair.Value.v0 != 50)
                    {
                        Conversation.createConversationTree(source, item1, red.Nodes);
                    }
                    else
                    {
                        red.ForeColor = Color.Gray;
                    }
                }
                else
                {
                    TreeNode white = n.Add(keyValuePair.Key, string.Concat("ERROR: Dialog line missing: ", keyValuePair.Key));
                    white.BackColor = Color.Red;
                    white.ForeColor = Color.White;
                }
            }
        }

        private void deleteBranch_Click(object sender, EventArgs e)
        {
            if (this.tree.SelectedNode.Nodes.Count > 0 && MessageBox.Show("Delete this entire conversation branch?", "Conversation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            List<string> strs = new List<string>();
            this.deleteRecursive(this.SelectedItem, strs);
            foreach (string str in strs)
            {
                GameData.Item item = this.gameData.getItem(str);
                this.gameData.deleteItem(item);
                this.deleteLinksTo(item);
            }
            this.tree.Nodes.Remove(this.tree.SelectedNode);
            this.tree.SelectedNode = null;
            this.countLines();
        }

        private void deleteItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.tree.SelectedNode;
            GameData.Item item = (selectedNode.Parent == null ? this.Item : (GameData.Item)selectedNode.Parent.Tag);
            GameData.Item selectedItem = this.SelectedItem;
            if (selectedItem != null || item == null)
            {
                if (item.getReferenceValue("lines", selectedItem.stringID).v0 == 50)
                {
                    item.removeReference("lines", selectedItem);
                    selectedNode.Remove();
                    this.countLines();
                    return;
                }
                int references = this.nav.ou.gameData.getReferences(selectedItem, null) - 1;
                if (references > 0)
                {
                    object[] objArray = new object[] { "This line is linked from ", references, " other place", null, null };
                    objArray[3] = (references > 1 ? "s" : "");
                    objArray[4] = ". Delete it and all links?";
                    if (MessageBox.Show(string.Concat(objArray), "Delete line", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                }
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in selectedItem.referenceData("lines", false))
                {
                    int? nullable = null;
                    int? nullable1 = nullable;
                    nullable = null;
                    int? nullable2 = nullable;
                    nullable = null;
                    item.addReference("lines", keyValuePair.Key, nullable1, nullable2, nullable);
                }
                List<string> strs = new List<string>();
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair1 in this.SelectedItem.referenceData("conditions", false))
                {
                    strs.Add(keyValuePair1.Key);
                }
                foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair2 in this.SelectedItem.referenceData("effects", false))
                {
                    strs.Add(keyValuePair2.Key);
                }
                foreach (string str in strs)
                {
                    this.gameData.deleteItem(this.gameData.getItem(str));
                }
                this.gameData.deleteItem(this.SelectedItem);
                TreeNodeCollection treeNodeCollections = (selectedNode.Parent == null ? this.tree.Nodes : selectedNode.Parent.Nodes);
                int num = treeNodeCollections.IndexOf(selectedNode);
                treeNodeCollections.Remove(selectedNode);
                this.tree.SelectedNode = null;
                foreach (TreeNode node in selectedNode.Nodes)
                {
                    int num1 = num;
                    num = num1 + 1;
                    treeNodeCollections.Insert(num1, node);
                }
                if (references > 0)
                {
                    this.deleteLinksTo(selectedItem);
                }
            }
            else
            {
                item.removeReference("lines", this.tree.SelectedNode.Index);
                selectedNode.Remove();
            }
            this.countLines();
        }

        private void deleteLinksTo(GameData.Item item)
        {
            List<TreeNode> treeNodes = new List<TreeNode>();
            foreach (TreeNode node in this.tree.Nodes)
            {
                treeNodes.Add(node);
            }
            while (treeNodes.Count > 0)
            {
                if (treeNodes[0].Tag != item)
                {
                    foreach (TreeNode treeNode in treeNodes[0].Nodes)
                    {
                        treeNodes.Add(treeNode);
                    }
                }
                else
                {
                    treeNodes[0].Remove();
                }
                treeNodes.RemoveAt(0);
            }
        }

        private void deleteRecursive(GameData.Item line, List<string> del)
        {
            if (line == null)
            {
                return;
            }
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in line.referenceData("lines", false))
            {
                if (keyValuePair.Value.v0 == 50)
                {
                    continue;
                }
                this.deleteRecursive(this.gameData.getItem(keyValuePair.Key), del);
            }
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair1 in line.referenceData("conditions", false))
            {
                del.Add(keyValuePair1.Key);
            }
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair2 in line.referenceData("effects", false))
            {
                del.Add(keyValuePair2.Key);
            }
            del.Add(line.stringID);
        }

        private void expandAll_Click(object sender, EventArgs e)
        {
            this.tree.SelectedNode.ExpandAll();
        }

        private static string getLineText(GameData.Item item)
        {
            if (item == null)
            {
                return "";
            }
            for (int i = 0; i < 99; i++)
            {
                string str = string.Concat("text", i);
                if (!item.ContainsKey(str))
                {
                    break;
                }
                if (item[str].ToString() != "")
                {
                    return item.sdata[str];
                }
            }
            return "<Empty>";
        }

        private void grid_OnPropertyChanged(object sender, PropertyChangedArgs e)
        {
            string item = this.lineProperties.Item.stringID;
            TreeNode[] treeNodeArray = this.tree.Nodes.Find(item, true);
            for (int i = 0; i < (int)treeNodeArray.Length; i++)
            {
                Conversation.refreshNode(treeNodeArray[i]);
            }
        }

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Conversation));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addChild = new System.Windows.Forms.ToolStripMenuItem();
            this.addInterjection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteBranch = new System.Windows.Forms.ToolStripMenuItem();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.conversationName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddChild = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.effectsPanel = new System.Windows.Forms.GroupBox();
            this.PossibleEffects = new System.Windows.Forms.ComboBox();
            this.buttonAddEffect = new System.Windows.Forms.Button();
            this.effectValue = new System.Windows.Forms.NumericUpDown();
            this.buttonRemoveEffect = new System.Windows.Forms.Button();
            this.referenceList1 = new forgotten_construction_set.Components.ReferenceList();
            this.btnAddInterjection = new System.Windows.Forms.Button();
            this.conditionControl1 = new forgotten_construction_set.dialog.ConditionControl();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tree = new System.Windows.Forms.TreeView();
            this.lineProperties = new forgotten_construction_set.Components.ObjectPropertyBox();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.effectsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.effectValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addChild,
            this.addInterjection,
            this.toolStripSeparator1,
            this.expandAll,
            this.toolStripSeparator2,
            this.deleteItem,
            this.deleteBranch});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(170, 126);
            // 
            // addChild
            // 
            this.addChild.Name = "addChild";
            this.addChild.Size = new System.Drawing.Size(169, 22);
            this.addChild.Text = "Add Line";
            this.addChild.Click += new System.EventHandler(this.addChild_Click);
            // 
            // addInterjection
            // 
            this.addInterjection.Name = "addInterjection";
            this.addInterjection.Size = new System.Drawing.Size(169, 22);
            this.addInterjection.Text = "Add Interjection";
            this.addInterjection.Click += new System.EventHandler(this.addInterjection_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // expandAll
            // 
            this.expandAll.Name = "expandAll";
            this.expandAll.Size = new System.Drawing.Size(169, 22);
            this.expandAll.Text = "Expand All";
            this.expandAll.Click += new System.EventHandler(this.expandAll_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
            // 
            // deleteItem
            // 
            this.deleteItem.Name = "deleteItem";
            this.deleteItem.Size = new System.Drawing.Size(169, 22);
            this.deleteItem.Text = "Delete";
            this.deleteItem.ToolTipText = "Delete line and reconnect children";
            this.deleteItem.Click += new System.EventHandler(this.deleteItem_Click);
            // 
            // deleteBranch
            // 
            this.deleteBranch.Name = "deleteBranch";
            this.deleteBranch.Size = new System.Drawing.Size(169, 22);
            this.deleteBranch.Text = "Delete Branch";
            this.deleteBranch.ToolTipText = "Delete this entire conversation branch";
            this.deleteBranch.Click += new System.EventHandler(this.deleteBranch_Click);
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView2.AutoArrange = false;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3});
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HideSelection = false;
            this.listView2.LabelWrap = false;
            this.listView2.Location = new System.Drawing.Point(5, 41);
            this.listView2.Margin = new System.Windows.Forms.Padding(2);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(225, 139);
            this.listView2.TabIndex = 6;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Effects";
            this.columnHeader2.Width = 158;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Value";
            this.columnHeader3.Width = 55;
            // 
            // conversationName
            // 
            this.conversationName.Location = new System.Drawing.Point(55, 6);
            this.conversationName.Margin = new System.Windows.Forms.Padding(2);
            this.conversationName.Name = "conversationName";
            this.conversationName.Size = new System.Drawing.Size(250, 21);
            this.conversationName.TabIndex = 7;
            this.conversationName.TextChanged += new System.EventHandler(this.conversationName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name";
            // 
            // btnAddChild
            // 
            this.btnAddChild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddChild.Location = new System.Drawing.Point(161, 160);
            this.btnAddChild.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddChild.Name = "btnAddChild";
            this.btnAddChild.Size = new System.Drawing.Size(120, 19);
            this.btnAddChild.TabIndex = 11;
            this.btnAddChild.Text = "Add Line";
            this.btnAddChild.UseVisualStyleBackColor = true;
            this.btnAddChild.Click += new System.EventHandler(this.addChild_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.effectsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.referenceList1);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddInterjection);
            this.splitContainer1.Panel1.Controls.Add(this.conditionControl1);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox2);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel1.Controls.Add(this.conversationName);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddChild);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1390, 576);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 12;
            // 
            // effectsPanel
            // 
            this.effectsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.effectsPanel.Controls.Add(this.PossibleEffects);
            this.effectsPanel.Controls.Add(this.listView2);
            this.effectsPanel.Controls.Add(this.buttonAddEffect);
            this.effectsPanel.Controls.Add(this.effectValue);
            this.effectsPanel.Controls.Add(this.buttonRemoveEffect);
            this.effectsPanel.Location = new System.Drawing.Point(708, 2);
            this.effectsPanel.Name = "effectsPanel";
            this.effectsPanel.Size = new System.Drawing.Size(231, 212);
            this.effectsPanel.TabIndex = 41;
            this.effectsPanel.TabStop = false;
            this.effectsPanel.Text = "Effects";
            // 
            // PossibleEffects
            // 
            this.PossibleEffects.FormattingEnabled = true;
            this.PossibleEffects.Location = new System.Drawing.Point(5, 16);
            this.PossibleEffects.Margin = new System.Windows.Forms.Padding(2);
            this.PossibleEffects.Name = "PossibleEffects";
            this.PossibleEffects.Size = new System.Drawing.Size(140, 20);
            this.PossibleEffects.TabIndex = 12;
            // 
            // buttonAddEffect
            // 
            this.buttonAddEffect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddEffect.Location = new System.Drawing.Point(108, 184);
            this.buttonAddEffect.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAddEffect.Name = "buttonAddEffect";
            this.buttonAddEffect.Size = new System.Drawing.Size(56, 19);
            this.buttonAddEffect.TabIndex = 13;
            this.buttonAddEffect.Text = "Add";
            this.buttonAddEffect.UseVisualStyleBackColor = true;
            this.buttonAddEffect.Click += new System.EventHandler(this.buttonAddEffect_Click);
            // 
            // effectValue
            // 
            this.effectValue.Location = new System.Drawing.Point(150, 17);
            this.effectValue.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.effectValue.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.effectValue.Name = "effectValue";
            this.effectValue.Size = new System.Drawing.Size(66, 21);
            this.effectValue.TabIndex = 15;
            this.effectValue.ThousandsSeparator = true;
            // 
            // buttonRemoveEffect
            // 
            this.buttonRemoveEffect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemoveEffect.Location = new System.Drawing.Point(169, 185);
            this.buttonRemoveEffect.Name = "buttonRemoveEffect";
            this.buttonRemoveEffect.Size = new System.Drawing.Size(56, 18);
            this.buttonRemoveEffect.TabIndex = 14;
            this.buttonRemoveEffect.Text = "Remove";
            this.buttonRemoveEffect.UseVisualStyleBackColor = true;
            this.buttonRemoveEffect.Click += new System.EventHandler(this.buttonRemoveEffect_Click);
            // 
            // referenceList1
            // 
            this.referenceList1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.referenceList1.AutoScroll = true;
            this.referenceList1.Exclusions = ((System.Collections.ArrayList)(resources.GetObject("referenceList1.Exclusions")));
            this.referenceList1.Location = new System.Drawing.Point(945, 2);
            this.referenceList1.Name = "referenceList1";
            this.referenceList1.ReadOnly = false;
            this.referenceList1.ShowDescription = true;
            this.referenceList1.Size = new System.Drawing.Size(306, 211);
            this.referenceList1.TabIndex = 40;
            // 
            // btnAddInterjection
            // 
            this.btnAddInterjection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddInterjection.Location = new System.Drawing.Point(161, 184);
            this.btnAddInterjection.Name = "btnAddInterjection";
            this.btnAddInterjection.Size = new System.Drawing.Size(120, 21);
            this.btnAddInterjection.TabIndex = 39;
            this.btnAddInterjection.Text = "Add Interjection";
            this.btnAddInterjection.UseVisualStyleBackColor = true;
            this.btnAddInterjection.Click += new System.EventHandler(this.addInterjection_Click);
            // 
            // conditionControl1
            // 
            this.conditionControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.conditionControl1.CurrentLine = null;
            this.conditionControl1.Item = null;
            this.conditionControl1.Location = new System.Drawing.Point(308, 5);
            this.conditionControl1.Name = "conditionControl1";
            this.conditionControl1.Size = new System.Drawing.Size(395, 210);
            this.conditionControl1.TabIndex = 34;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(21, 32);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(126, 16);
            this.checkBox2.TabIndex = 30;
            this.checkBox2.Text = "Can talk to enemy";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(141, 32);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(102, 16);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Run once only";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lineProperties);
            this.splitContainer2.Size = new System.Drawing.Size(1390, 357);
            this.splitContainer2.SplitterDistance = 939;
            this.splitContainer2.TabIndex = 36;
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.ContextMenuStrip = this.contextMenu;
            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tree.ForeColor = System.Drawing.Color.Black;
            this.tree.FullRowSelect = true;
            this.tree.HideSelection = false;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Margin = new System.Windows.Forms.Padding(2);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(939, 357);
            this.tree.TabIndex = 4;
            this.tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_ItemDrag);
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.tree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
            this.tree.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_DragDrop);
            this.tree.DragOver += new System.Windows.Forms.DragEventHandler(this.tree_DragOver);
            this.tree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tree_KeyDown);
            this.tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
            // 
            // lineProperties
            // 
            this.lineProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lineProperties.Item = null;
            this.lineProperties.Location = new System.Drawing.Point(0, 0);
            this.lineProperties.Name = "lineProperties";
            this.lineProperties.ShowDescription = true;
            this.lineProperties.Size = new System.Drawing.Size(447, 357);
            this.lineProperties.TabIndex = 35;
            // 
            // Conversation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1390, 576);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Conversation";
            this.Text = "Conversation Editor";
            this.contextMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.effectsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.effectValue)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        private static bool isLink(TreeNode node)
        {
            return node.ForeColor == Color.Gray;
        }

        private void referenceList1_ChangeEvent(object sender)
        {
            this.conditionControl1_ChangeEvent(sender);
        }

        public void refreshEffectsList()
        {
            this.listView2.Items.Clear();
            this.effectsPanel.Enabled = this.tree.SelectedNode != null;
            if (this.tree.SelectedNode == null)
            {
                return;
            }
            GameData.Item tag = (GameData.Item)this.tree.SelectedNode.Tag;
            if (tag == null)
            {
                return;
            }
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in tag.referenceData("effects", false))
            {
                GameData.Item item = this.gameData.getItem(keyValuePair.Key);
                if (item == null || item.getState() == GameData.State.REMOVED)
                {
                    this.listView2.Items.Add("错误-无效引用").ForeColor = Color.Red;
                }
                else if (!item.sdata.ContainsKey("action name"))
                {
                    DialogActionEnum dialogActionEnum = (DialogActionEnum)item.idata["action name"];
                    this.listView2.Items.Add(dialogActionEnum.ToString()).SubItems.Add(keyValuePair.Value.v0.ToString());
                }
                else
                {
                    this.listView2.Items.Add(item.sdata["action name"]).SubItems.Add(keyValuePair.Value.v0.ToString());
                }
            }
        }

        public static void refreshNode(TreeNode node)
        {
            if (node == null || node.Tag == null)
            {
                return;
            }
            GameData.Item tag = (GameData.Item)node.Tag;
            if (node.Parent == null)
            {
                node.Text = string.Concat((tag.getReferenceCount("conditions") > 0 ? "[C] " : ""), "root");
                return;
            }
            bool flag = (!tag.ContainsKey("interjection") ? false : tag.bdata["interjection"]);
            node.Text = (flag ? "*Interjection Node*" : Conversation.getLineText(tag));
            string str = "";
            int num = 0;
            if (tag.getState() != GameData.State.ORIGINAL)
            {
                str = string.Concat(str, "*");
            }
            for (int i = 0; tag.ContainsKey(string.Concat("text", i)); i++)
            {
                if (tag.sdata[string.Concat("text", i)] != "")
                {
                    num++;
                }
            }
            bool referenceCount = tag.getReferenceCount("conditions") > 0;
            bool referenceCount1 = tag.getReferenceCount("effects") > 0;
            foreach (string str1 in tag.referenceLists())
            {
                if (tag.getReferenceCount(str1) <= 0)
                {
                    continue;
                }
                if (!GameData.getDesc(tag.type, str1).description.Contains("condition"))
                {
                    if (!(str1 != "conditions") || !(str1 != "lines"))
                    {
                        continue;
                    }
                    referenceCount1 = true;
                }
                else
                {
                    referenceCount = true;
                }
            }
            if (referenceCount)
            {
                str = string.Concat(str, "C");
            }
            if (referenceCount1)
            {
                str = string.Concat(str, "E");
            }
            if (num > 1)
            {
                str = string.Concat(str, (str != "" ? " " : ""), num.ToString());
            }
            if (str != "")
            {
                node.Text = string.Concat("[", str, "] ", node.Text);
            }
            if (!Conversation.isLink(node))
            {
                TalkerEnum bah = (TalkerEnum)tag["speaker"];
                if (flag)
                {
                    node.BackColor = StateColours.getIntejectionColour(bah);
                    return;
                }
                node.BackColor = node.TreeView.BackColor;
                node.ForeColor = StateColours.getTalkerColour(bah);
            }
        }

        public void SelectLine(GameData.Item line)
        {
            TreeNode[] treeNodeArray = this.tree.Nodes.Find(line.stringID, true);
            if (treeNodeArray.Length != 0)
            {
                for (TreeNode i = treeNodeArray[0].Parent; i != null; i = i.Parent)
                {
                    i.Expand();
                }
                this.tree.SelectedNode = treeNodeArray[0];
                this.tree_AfterSelect(this.tree, null);
                this.tree.Select();
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.refreshEffectsList();
            this.conditionControl1.refresh(this.SelectedItem);
            this.referenceList1.refresh(this.SelectedItem);
            this.lineProperties.refresh(this.SelectedItem);
            this.lineProperties.grid.removeSection("Base");
            if ((this.SelectedItem == null || !this.SelectedItem.ContainsKey("interjection") ? false : this.SelectedItem.bdata["interjection"]))
            {
                this.lineProperties.grid.removeSection("text");
            }
            bool flag = (this.SelectedItem == null || Conversation.isLink(this.tree.SelectedNode) ? false : this.SelectedItem.getState() != GameData.State.LOCKED);
            this.btnAddChild.Enabled = flag;
            this.btnAddInterjection.Enabled = flag;
            this.addChild.Enabled = flag;
            this.addInterjection.Enabled = flag;
            bool parent = this.tree.SelectedNode.Parent == null;
            bool flag1 = (this.SelectedItem == null ? false : this.SelectedItem.getState() != GameData.State.LOCKED);
            this.buttonAddEffect.Enabled = (!flag1 ? false : !parent);
            this.buttonRemoveEffect.Enabled = flag1;
            this.deleteBranch.Enabled = (!flag1 ? false : !parent);
            this.deleteItem.Enabled = (!flag1 ? false : !parent);
            this.effectsPanel.Enabled = !parent;
        }

        private void tree_DragDrop(object sender, DragEventArgs e)
        {
            int? nullable;
            GameData.Item item;
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point client = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode data = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                TreeNode nodeAt = ((TreeView)sender).GetNodeAt(client);
                TreeNodeCollection treeNodeCollections = (nodeAt != null ? nodeAt.Nodes : this.tree.Nodes);
                GameData.Item tag = (GameData.Item)nodeAt.Tag;
                if (e.Effect == DragDropEffects.Move)
                {
                    GameData.Item tag1 = (GameData.Item)data.Tag;
                    if (tag.getReferenceValue("lines", tag1.stringID) != null)
                    {
                        return;
                    }
                    item = (data.Parent == null ? this.Item : (GameData.Item)data.Parent.Tag);
                    int referenceValue = item.getReferenceValue("lines", tag1.stringID).v0;
                    item.removeReference("lines", tag1);
                    nullable = null;
                    int? nullable1 = nullable;
                    nullable = null;
                    tag.addReference("lines", tag1, new int?(referenceValue), nullable1, nullable);
                    TreeNode treeNode = (TreeNode)data.Clone();
                    treeNodeCollections.Add(treeNode);
                    data.Remove();
                    this.tree.SelectedNode = treeNode;
                    this.nav.HasChanges = true;
                }
                else if (e.Effect == DragDropEffects.Link)
                {
                    GameData.Item item1 = (GameData.Item)data.Tag;
                    if (tag.getReferenceValue("lines", item1.stringID) != null)
                    {
                        return;
                    }
                    nullable = null;
                    int? nullable2 = nullable;
                    nullable = null;
                    tag.addReference("lines", item1, new int?(50), nullable2, nullable);
                    if (tag.getReferenceValue("lines", item1.stringID).v0 != 50)
                    {
                        MessageBox.Show("这个链接有一些问题,已经帮你修复.\n请反馈这个错误.");
                        nullable = null;
                        int? nullable3 = nullable;
                        nullable = null;
                        tag.setReferenceValue("lines", item1.stringID, new int?(50), nullable3, nullable);
                    }
                    TreeNode gray = treeNodeCollections.Add(item1.stringID, data.Text);
                    gray.Tag = item1;
                    gray.ForeColor = Color.Gray;
                    this.tree.SelectedNode = gray;
                    this.countLines();
                    this.nav.HasChanges = true;
                }
                if (nodeAt != null)
                {
                    nodeAt.Expand();
                }
            }
        }

        private void tree_DragOver(object sender, DragEventArgs e)
        {
            Point client = this.tree.PointToClient(new Point(e.X, e.Y));
            TreeNode nodeAt = this.tree.GetNodeAt(client);
            this.tree.SelectedNode = nodeAt;
            if (nodeAt == null || Conversation.isLink(nodeAt))
            {
                e.Effect = DragDropEffects.None;
            }
            else if ((e.KeyState & 12) <= 0)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.Link;
            }
            TreeNode data = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            if (data.TreeView != sender)
            {
                e.Effect = DragDropEffects.None;
            }
            if (e.Effect == DragDropEffects.Move)
            {
                for (TreeNode i = nodeAt; i != null; i = i.Parent)
                {
                    if (i == data)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
        }

        private void tree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode item = (TreeNode)e.Item;
            if (item.Parent == null)
            {
                return;
            }
            Color backColor = item.BackColor;
            item.BackColor = Color.Yellow;
            if (base.DoDragDrop(item, DragDropEffects.Move | DragDropEffects.Link) != DragDropEffects.None)
            {
                this.tree.SelectedNode.BackColor = backColor;
            }
            item.BackColor = backColor;
        }

        private void tree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && this.SelectedItem != null && this.SelectedItem.type == itemType.DIALOGUE_LINE)
            {
                this.deleteItem_Click(sender, e);
            }
        }

        private void tree_MouseDown(object sender, MouseEventArgs e)
        {
            this.tree.SelectedNode = this.tree.GetNodeAt(e.Location);
        }

        private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Conversation.isLink(e.Node))
            {
                GameData.Item tag = (GameData.Item)e.Node.Tag;
                TreeNode[] treeNodeArray = this.tree.Nodes.Find(tag.stringID, true);
                for (int i = 0; i < (int)treeNodeArray.Length; i++)
                {
                    TreeNode treeNode = treeNodeArray[i];
                    if (!Conversation.isLink(treeNode))
                    {
                        this.tree.SelectedNode = treeNode;
                        return;
                    }
                }
            }
        }

        private void updateTreeNodesText(TreeNodeCollection list, string ID, string text)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == ID)
                {
                    list[i].Text = text;
                }
                this.updateTreeNodesText(list[i].Nodes, ID, text);
            }
        }

        #endregion
    }
}
