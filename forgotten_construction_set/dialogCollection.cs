using forgotten_construction_set.dialog;
using forgotten_construction_set.Components;
using forgotten_construction_set.PropertyGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class dialogCollection : Form
	{
		public navigation nav;

		private IContainer components;

		private TreeView treeView1;

		private TextBox packageName;

		private Label label1;

		private ConditionControl conditionControl1;

		private ListViewTrans eventsList;

		private ColumnHeader EventsColumn;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ToolStripMenuItem editItem;

		private ToolStripMenuItem addItem;

		private ToolStripMenuItem removeItem;

		private ObjectPropertyBox objectPropertyBox1;

		private GroupBox groupBox1;

		private SplitContainer splitContainer1;

		private SplitContainer splitContainer2;

		private Button buttonAddNew;

		private Button buttonAddItem;

		private SplitContainer splitContainer3;

		private SplitContainer splitContainer4;

		private ReferenceList referenceList1;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem expandDialogue;

		private ToolStripMenuItem collapseDialogue;

		private ToolStripMenuItem addNew;

		public GameData.Item Item
		{
			get;
			set;
		}

		private EventTriggerEnum SelectedEvent
		{
			get;
			set;
		}

		public GameData.Item SelectedItem
		{
			get;
			set;
		}

		public dialogCollection(GameData.Item item, navigation n)
		{
			this.InitializeComponent();
			this.Item = item;
			this.nav = n;
			this.referenceList1.Exclusions.Add("dialogs");
			this.referenceList1.setup(this.Item, this.nav);
			this.objectPropertyBox1.setup(null, this.nav);
			this.objectPropertyBox1.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
			this.packageName.Text = this.Item.Name;
			bool state = this.Item.getState() != GameData.State.LOCKED;
			this.packageName.Enabled = state;
			this.addNew.Enabled = state;
			this.addItem.Enabled = state;
			this.removeItem.Enabled = state;
			this.buttonAddItem.Enabled = state;
			this.buttonAddNew.Enabled = state;
			this.SelectedEvent = EventTriggerEnum.EV_I_SEE_NEUTRAL_SQUAD;
			this.eventsList.Items.Clear();
			foreach (EventTriggerEnum value in Enum.GetValues(typeof(EventTriggerEnum)))
			{
				if (value == EventTriggerEnum.EV_NONE || value == EventTriggerEnum.EV_MAX)
				{
					continue;
				}
				this.eventsList.Items.Add(value.ToString());
			}
			this.conditionControl1.setup(null, this.nav);
			this.refresh();
		}

		private void addDialogRef(GameData.Item what)
		{
			if (!this.Item.hasReferenceTo("dialogs", what.stringID))
			{
				this.Item.addReference("dialogs", what, new int?((int)this.SelectedEvent), new int?(0), new int?(0));
				return;
			}
			GameData.TripleInt referenceValue = this.Item.getReferenceValue("dialogs", what.stringID);
			if (referenceValue.v1 == 100)
			{
				referenceValue.v1 = 0;
			}
			if (referenceValue.v0 == 0)
			{
				referenceValue.v0 = (int)this.SelectedEvent;
			}
			else if (referenceValue.v1 == 0)
			{
				referenceValue.v1 = (int)this.SelectedEvent;
			}
			else if (referenceValue.v2 != 0)
			{
				MessageBox.Show("对该对话对象引用过多，对话包最多只能引用一个对象3次", "错误", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				referenceValue.v2 = (int)this.SelectedEvent;
			}
			this.Item.setReferenceValue("dialogs", what.stringID, new int?(referenceValue.v0), new int?(referenceValue.v1), new int?(referenceValue.v2));
		}

		private void addItem_Click(object sender, EventArgs e)
		{
			ItemDialog itemDialog = new ItemDialog("Add Dialogue", this.nav.ou.gameData, itemType.DIALOGUE, true, "", itemType.NULL_ITEM);
			if (itemDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				foreach (GameData.Item item in itemDialog.Items)
				{
					this.addDialogRef(item);
				}
				if (itemDialog.Items.Count > 0)
				{
					this.refresh();
				}
				this.nav.refreshState(this.Item);
				this.nav.HasChanges = true;
			}
		}

		private void addNew_Click(object sender, EventArgs e)
		{
			GameData.Item item = this.nav.ou.gameData.createItem(itemType.DIALOGUE);
			int? nullable = null;
			this.Item.addReference("dialogs", item, new int?((int)this.SelectedEvent), new int?(0), nullable);
			this.refresh();
			this.nav.showItemProperties(item);
			this.nav.refreshState(this.Item);
			this.nav.HasChanges = true;
		}

		private void collapseDialogue_Click(object sender, EventArgs e)
		{
			this.getRootNodeOf(this.treeView1.SelectedNode).Collapse();
		}

		private void dialogCollection_Activated(object sender, EventArgs e)
		{
			this.refresh();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void editItem_Click(object sender, EventArgs e)
		{
			GameData.Item tag = (GameData.Item)this.getRootNodeOf(this.treeView1.SelectedNode).Tag;
			this.nav.showItemProperties(tag);
		}

		private void eventsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.eventsList.SelectedIndices.Count > 0)
			{
				this.SelectedEvent = (EventTriggerEnum)(this.eventsList.SelectedIndices[0] + 1);
				this.SelectedItem = null;
				this.conditionControl1.refresh(null);
				this.refresh();
			}
		}

		private void expandDialogue_Click(object sender, EventArgs e)
		{
			this.getRootNodeOf(this.treeView1.SelectedNode).ExpandAll();
		}

		private TreeNode getRootNodeOf(TreeNode n)
		{
			while (n.Parent != null)
			{
				n = n.Parent;
			}
			return n;
		}

		private void grid_OnPropertyChanged(object sender, PropertyChangedArgs e)
		{
			string item = this.objectPropertyBox1.Item.stringID;
			TreeNode[] treeNodeArray = this.treeView1.Nodes.Find(item, true);
			for (int i = 0; i < (int)treeNodeArray.Length; i++)
			{
				Conversation.refreshNode(treeNodeArray[i]);
			}
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dialogCollection));
			this.treeView1 = new TreeView();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editItem = new ToolStripMenuItem();
			this.addNew = new ToolStripMenuItem();
			this.addItem = new ToolStripMenuItem();
			this.removeItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.expandDialogue = new ToolStripMenuItem();
			this.collapseDialogue = new ToolStripMenuItem();
			this.packageName = new TextBox();
			this.label1 = new Label();
			this.eventsList = new ListViewTrans();
			this.EventsColumn = new ColumnHeader();
			this.groupBox1 = new GroupBox();
			this.splitContainer1 = new SplitContainer();
			this.splitContainer4 = new SplitContainer();
			this.splitContainer2 = new SplitContainer();
			this.buttonAddNew = new Button();
			this.buttonAddItem = new Button();
			this.splitContainer3 = new SplitContainer();
			this.referenceList1 = new ReferenceList();
			this.conditionControl1 = new ConditionControl();
			this.objectPropertyBox1 = new ObjectPropertyBox();
			this.contextMenu.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((ISupportInitialize)this.splitContainer4).BeginInit();
			this.splitContainer4.Panel1.SuspendLayout();
			this.splitContainer4.Panel2.SuspendLayout();
			this.splitContainer4.SuspendLayout();
			((ISupportInitialize)this.splitContainer2).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((ISupportInitialize)this.splitContainer3).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			base.SuspendLayout();
			this.treeView1.ContextMenuStrip = this.contextMenu;
			this.treeView1.Dock = DockStyle.Fill;
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(436, 532);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.editItem, this.addNew, this.addItem, this.removeItem, this.toolStripSeparator1, this.expandDialogue, this.collapseDialogue });
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.Size = new System.Drawing.Size(140, 142);
			this.editItem.Name = "editItem";
			this.editItem.Size = new System.Drawing.Size(139, 22);
			this.editItem.Text = "编辑";
			this.editItem.Click += new EventHandler(this.editItem_Click);
			this.addNew.Name = "addNew";
			this.addNew.Size = new System.Drawing.Size(139, 22);
			this.addNew.Text = "新增";
			this.addNew.Click += new EventHandler(this.addNew_Click);
			this.addItem.Name = "addItem";
			this.addItem.Size = new System.Drawing.Size(139, 22);
			this.addItem.Text = "添加已存在的";
			this.addItem.Click += new EventHandler(this.addItem_Click);
			this.removeItem.Name = "removeItem";
			this.removeItem.Size = new System.Drawing.Size(139, 22);
			this.removeItem.Text = "删除";
			this.removeItem.Click += new EventHandler(this.removeItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
			this.expandDialogue.Name = "expandDialogue";
			this.expandDialogue.Size = new System.Drawing.Size(139, 22);
			this.expandDialogue.Text = "展开";
			this.expandDialogue.Click += new EventHandler(this.expandDialogue_Click);
			this.collapseDialogue.Name = "collapseDialogue";
			this.collapseDialogue.Size = new System.Drawing.Size(139, 22);
			this.collapseDialogue.Text = "折叠";
			this.collapseDialogue.Click += new EventHandler(this.collapseDialogue_Click);
			this.packageName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.packageName.Location = new Point(60, 18);
			this.packageName.Margin = new System.Windows.Forms.Padding(2);
			this.packageName.Name = "packageName";
			this.packageName.Size = new System.Drawing.Size(250, 20);
			this.packageName.TabIndex = 0;
			this.packageName.TextChanged += new EventHandler(this.packageName_TextChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(14, 18);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "名称";
			this.eventsList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.eventsList.Columns.AddRange(new ColumnHeader[] { this.EventsColumn });
			this.eventsList.FullRowSelect = true;
			this.eventsList.GridLines = true;
			this.eventsList.HideSelection = false;
			this.eventsList.Location = new Point(0, 0);
			this.eventsList.Name = "eventsList";
			this.eventsList.Size = new System.Drawing.Size(270, 532);
			this.eventsList.TabIndex = 71;
			this.eventsList.UseCompatibleStateImageBehavior = false;
			this.eventsList.View = View.Details;
			this.eventsList.SelectedIndexChanged += new EventHandler(this.eventsList_SelectedIndexChanged);
			this.EventsColumn.Text = "事件";
			this.EventsColumn.Width = 263;
			this.groupBox1.Controls.Add(this.referenceList1);
			this.groupBox1.Controls.Add(this.packageName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Dock = DockStyle.Fill;
			this.groupBox1.Location = new Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(343, 250);
			this.groupBox1.TabIndex = 75;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "包";
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer4);
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(1031, 786);
			this.splitContainer1.SplitterDistance = 250;
			this.splitContainer1.TabIndex = 76;
			this.splitContainer4.Dock = DockStyle.Fill;
			this.splitContainer4.Location = new Point(0, 0);
			this.splitContainer4.Name = "splitContainer4";
			this.splitContainer4.Panel1.Controls.Add(this.groupBox1);
			this.splitContainer4.Panel2.Controls.Add(this.conditionControl1);
			this.splitContainer4.Size = new System.Drawing.Size(1031, 250);
			this.splitContainer4.SplitterDistance = 343;
			this.splitContainer4.TabIndex = 76;
			this.splitContainer2.Dock = DockStyle.Fill;
			this.splitContainer2.Location = new Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Panel1.Controls.Add(this.buttonAddNew);
			this.splitContainer2.Panel1.Controls.Add(this.buttonAddItem);
			this.splitContainer2.Panel1.Controls.Add(this.eventsList);
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer2.Size = new System.Drawing.Size(1031, 532);
			this.splitContainer2.SplitterDistance = 343;
			this.splitContainer2.TabIndex = 0;
			this.buttonAddNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.buttonAddNew.Location = new Point(276, 3);
			this.buttonAddNew.Name = "buttonAddNew";
			this.buttonAddNew.Size = new System.Drawing.Size(60, 48);
			this.buttonAddNew.TabIndex = 77;
			this.buttonAddNew.Text = "新增";
			this.buttonAddNew.UseVisualStyleBackColor = true;
			this.buttonAddNew.Click += new EventHandler(this.addNew_Click);
			this.buttonAddItem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.buttonAddItem.Location = new Point(276, 57);
			this.buttonAddItem.Name = "buttonAddItem";
			this.buttonAddItem.Size = new System.Drawing.Size(60, 61);
			this.buttonAddItem.TabIndex = 74;
			this.buttonAddItem.Text = "添加对话";
			this.buttonAddItem.UseVisualStyleBackColor = true;
			this.buttonAddItem.Click += new EventHandler(this.addItem_Click);
			this.splitContainer3.Dock = DockStyle.Fill;
			this.splitContainer3.Location = new Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Panel1.Controls.Add(this.treeView1);
			this.splitContainer3.Panel2.Controls.Add(this.objectPropertyBox1);
			this.splitContainer3.Size = new System.Drawing.Size(684, 532);
			this.splitContainer3.SplitterDistance = 436;
			this.splitContainer3.TabIndex = 0;
			this.referenceList1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.referenceList1.Exclusions = (ArrayList)resources.GetObject("referenceList1.Exclusions");
			this.referenceList1.Location = new Point(6, 43);
			this.referenceList1.Name = "referenceList1";
			this.referenceList1.ReadOnly = false;
			this.referenceList1.ShowDescription = true;
			this.referenceList1.Size = new System.Drawing.Size(330, 201);
			this.referenceList1.TabIndex = 11;
			this.conditionControl1.CurrentLine = null;
			this.conditionControl1.Dock = DockStyle.Fill;
			this.conditionControl1.Item = null;
			this.conditionControl1.Location = new Point(0, 0);
			this.conditionControl1.Name = "conditionControl1";
			this.conditionControl1.Size = new System.Drawing.Size(684, 250);
			this.conditionControl1.TabIndex = 67;
			this.objectPropertyBox1.Dock = DockStyle.Fill;
			this.objectPropertyBox1.Item = null;
			this.objectPropertyBox1.Location = new Point(0, 0);
			this.objectPropertyBox1.Name = "objectPropertyBox1";
			this.objectPropertyBox1.ShowDescription = true;
			this.objectPropertyBox1.Size = new System.Drawing.Size(244, 532);
			this.objectPropertyBox1.TabIndex = 74;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1031, 786);
			base.Controls.Add(this.splitContainer1);
			base.Name = "dialogCollection";
			base.ShowIcon = false;
			this.Text = "对话包";
			base.Activated += new EventHandler(this.dialogCollection_Activated);
			this.contextMenu.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer4.Panel1.ResumeLayout(false);
			this.splitContainer4.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer4).EndInit();
			this.splitContainer4.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer2).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer3).EndInit();
			this.splitContainer3.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void packageName_TextChanged(object sender, EventArgs e)
		{
			string text = ((TextBox)sender).Text;
			if (text != this.Item.Name)
			{
				this.Item.Name = text;
				this.nav.refreshState(this.Item);
			}
		}

		private void refresh()
		{
			foreach (object item in this.eventsList.Items)
			{
				((ListViewItem)item).ForeColor = Color.Gray;
			}
			foreach (KeyValuePair<string, GameData.TripleInt> black in this.Item.referenceData("dialogs", false))
			{
				if (black.Value.v0 > 0 && black.Value.v0 <= this.eventsList.Items.Count)
				{
					this.eventsList.Items[black.Value.v0 - 1].ForeColor = Color.Black;
				}
				if (black.Value.v1 > 0 && black.Value.v1 <= this.eventsList.Items.Count)
				{
					this.eventsList.Items[black.Value.v1 - 1].ForeColor = Color.Black;
				}
				if (black.Value.v2 <= 0 || black.Value.v2 > this.eventsList.Items.Count)
				{
					continue;
				}
				this.eventsList.Items[black.Value.v2 - 1].ForeColor = Color.Black;
			}
			this.treeView1.Nodes.Clear();
			foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in this.Item.referenceData("dialogs", false))
			{
				if (keyValuePair.Value.v0 != (int)this.SelectedEvent && keyValuePair.Value.v1 != (int)this.SelectedEvent && keyValuePair.Value.v2 != (int)this.SelectedEvent)
				{
					continue;
				}
				GameData.Item item1 = this.nav.ou.gameData.getItem(keyValuePair.Key);
				if (item1 == null)
				{
					continue;
				}
				TreeNode stateColor = this.treeView1.Nodes.Add(item1.Name);
				stateColor.ForeColor = StateColours.GetStateColor(this.Item.getState("dialogs", keyValuePair.Key));
				stateColor.Tag = item1;
                Conversation.createConversationTree(this.nav.ou.gameData, item1, stateColor.Nodes);
			}
			this.conditionControl1.refresh(this.SelectedItem);
			this.objectPropertyBox1.refresh(this.SelectedItem);
			this.objectPropertyBox1.grid.removeSection("Base");
		}

		private void removeItem_Click(object sender, EventArgs e)
		{
			if (this.treeView1.SelectedNode == null)
			{
				return;
			}
			TreeNode rootNodeOf = this.getRootNodeOf(this.treeView1.SelectedNode);
			GameData.Item tag = (GameData.Item)rootNodeOf.Tag;
			GameData.TripleInt referenceValue = this.Item.getReferenceValue("dialogs", tag.stringID);
			if (referenceValue.v0 == (int)this.SelectedEvent)
			{
				referenceValue.v0 = 0;
			}
			else if (referenceValue.v1 == (int)this.SelectedEvent)
			{
				referenceValue.v1 = 0;
			}
			else if (referenceValue.v2 == (int)this.SelectedEvent)
			{
				referenceValue.v2 = 0;
			}
			if (referenceValue.v0 + referenceValue.v1 + referenceValue.v2 != 0)
			{
				this.Item.setReferenceValue("dialogs", tag.stringID, new int?(referenceValue.v0), new int?(referenceValue.v1), new int?(referenceValue.v2));
			}
			else
			{
				this.Item.removeReference("dialogs", tag);
			}
			this.treeView1.Nodes.Remove(rootNodeOf);
			if (this.treeView1.Nodes.Count == 0)
			{
				this.updateEventState(this.SelectedEvent, false);
			}
			this.nav.refreshState(this.Item);
			this.nav.HasChanges = true;
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.SelectedItem = (GameData.Item)e.Node.Tag;
			this.conditionControl1.refresh(this.SelectedItem);
			this.objectPropertyBox1.refresh(this.SelectedItem);
			this.objectPropertyBox1.grid.removeSection("Base");
		}

		private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			this.treeView1.SelectedNode = e.Node;
		}

		private void updateEventState(EventTriggerEnum e, bool used)
		{
			int num = (int)e - (int)EventTriggerEnum.EV_PLAYER_TALK_TO_ME;
			this.eventsList.Items[num].ForeColor = (used ? Color.Black : Color.Gray);
		}
	}
}