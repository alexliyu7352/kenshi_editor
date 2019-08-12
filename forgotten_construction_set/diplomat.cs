using forgotten_construction_set.dialog;
using PropertyGrid;
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
	public class diplomat : Form
	{
		public navigation nav;

		public head ou;

		private GameData.Item selectedAssault;

		private IContainer components;

		private GameDataList assaultList;

		private ColumnHeader columnHeader1;

		private TextBox nameBox;

		private Label nameLabel;

		private Button addAssault;

		private Button removeAssault;

		private ConditionControl conditionControl1;

		private ObjectPropertyBox propertyList;

		private SplitContainer splitContainer1;

		private SplitContainer splitContainer2;

		private SplitContainer splitContainer3;

		private SplitContainer splitContainer4;

		private ReferenceList referenceList;

		private Label description;

		private Label selection;

		public GameData.Item Item
		{
			get;
			set;
		}

		public diplomat(GameData.Item item, navigation n)
		{
			this.InitializeComponent();
			this.Item = item;
			this.ou = n.ou;
			this.nav = n;
			this.conditionControl1.setup(item, this.nav);
			this.propertyList.setup(null, this.nav);
			this.referenceList.setup(null, this.nav);
			this.referenceList.Exclusions.Add("conditions");
			this.propertyList.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
			this.nameBox.Text = this.Item.Name;
			bool state = item.getState() != GameData.State.LOCKED;
			this.nameBox.Enabled = state;
			this.addAssault.Enabled = state;
			this.removeAssault.Enabled = state;
			this.assaultList.Items.Clear();
			foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in this.Item.referenceData("assaults", false))
			{
				GameData.Item item1 = this.ou.gameData.getItem(keyValuePair.Key);
				this.assaultList.Items.Add(item1);
			}
			this.assaultList.Refresh();
		}

		private void addAssault_Click(object sender, EventArgs e)
		{
			GameData.Item item = this.ou.gameData.createItem(itemType.SINGLE_DIPLOMATIC_ASSAULT);
			int? nullable = null;
			int? nullable1 = nullable;
			nullable = null;
			int? nullable2 = nullable;
			nullable = null;
			this.Item.addReference("assaults", item, nullable1, nullable2, nullable);
			this.assaultList.AddItem(item);
			this.nav.refreshState(this.Item);
			this.assaultList.Refresh();
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
			this.assaultList.RefreshItem(this.selectedAssault);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(diplomat));
			this.nameBox = new TextBox();
			this.nameLabel = new Label();
			this.addAssault = new Button();
			this.removeAssault = new Button();
			this.description = new Label();
			this.selection = new Label();
			this.splitContainer1 = new SplitContainer();
			this.assaultList = new GameDataList();
			this.columnHeader1 = new ColumnHeader();
			this.splitContainer2 = new SplitContainer();
			this.conditionControl1 = new ConditionControl();
			this.splitContainer3 = new SplitContainer();
			this.splitContainer4 = new SplitContainer();
			this.propertyList = new ObjectPropertyBox();
			this.referenceList = new ReferenceList();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((ISupportInitialize)this.splitContainer2).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((ISupportInitialize)this.splitContainer3).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((ISupportInitialize)this.splitContainer4).BeginInit();
			this.splitContainer4.Panel1.SuspendLayout();
			this.splitContainer4.Panel2.SuspendLayout();
			this.splitContainer4.SuspendLayout();
			base.SuspendLayout();
			this.nameBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.nameBox.Location = new Point(46, 6);
			this.nameBox.Name = "nameBox";
			this.nameBox.Size = new System.Drawing.Size(208, 20);
			this.nameBox.TabIndex = 7;
			this.nameBox.TextChanged += new EventHandler(this.textBox1_TextChanged);
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new Point(3, 9);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(38, 13);
			this.nameLabel.TabIndex = 8;
			this.nameLabel.Text = "名称:";
			this.addAssault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.addAssault.Location = new Point(110, 32);
			this.addAssault.Name = "addAssault";
			this.addAssault.Size = new System.Drawing.Size(64, 19);
			this.addAssault.TabIndex = 10;
			this.addAssault.Text = "新增";
			this.addAssault.UseVisualStyleBackColor = true;
			this.addAssault.Click += new EventHandler(this.addAssault_Click);
			this.removeAssault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.removeAssault.Location = new Point(180, 32);
			this.removeAssault.Name = "removeAssault";
			this.removeAssault.Size = new System.Drawing.Size(67, 19);
			this.removeAssault.TabIndex = 9;
			this.removeAssault.Text = "删除";
			this.removeAssault.UseVisualStyleBackColor = true;
			this.removeAssault.Click += new EventHandler(this.removeAssault_Click);
			this.description.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.description.Location = new Point(14, 13);
			this.description.Name = "description";
			this.description.Size = new System.Drawing.Size(496, 51);
			this.description.TabIndex = 1;
			this.description.Text = "描述";
			this.selection.AutoSize = true;
			this.selection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.selection.Location = new Point(3, 0);
			this.selection.Name = "selection";
			this.selection.Size = new System.Drawing.Size(82, 13);
			this.selection.TabIndex = 0;
			this.selection.Text = "以选择的项目";
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.assaultList);
			this.splitContainer1.Panel1.Controls.Add(this.addAssault);
			this.splitContainer1.Panel1.Controls.Add(this.nameBox);
			this.splitContainer1.Panel1.Controls.Add(this.removeAssault);
			this.splitContainer1.Panel1.Controls.Add(this.nameLabel);
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(772, 594);
			this.splitContainer1.SplitterDistance = 257;
			this.splitContainer1.TabIndex = 37;
			this.assaultList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.assaultList.Columns.AddRange(new ColumnHeader[] { this.columnHeader1 });
			this.assaultList.Filter = null;
			this.assaultList.FullRowSelect = true;
			this.assaultList.GridLines = true;
			this.assaultList.HideSelection = false;
			this.assaultList.LabelEdit = true;
			this.assaultList.Location = new Point(3, 57);
			this.assaultList.MultiSelect = false;
			this.assaultList.Name = "assaultList";
			this.assaultList.Size = new System.Drawing.Size(251, 534);
			this.assaultList.Source = null;
			this.assaultList.TabIndex = 0;
			this.assaultList.UseCompatibleStateImageBehavior = false;
			this.assaultList.View = View.Details;
			this.assaultList.VirtualMode = true;
			this.assaultList.AfterLabelEdit += new LabelEditEventHandler(this.listView1_AfterLabelEdit);
			this.assaultList.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
			this.columnHeader1.Text = "攻击列表";
			this.columnHeader1.Width = 238;
			this.splitContainer2.Dock = DockStyle.Fill;
			this.splitContainer2.Location = new Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = Orientation.Horizontal;
			this.splitContainer2.Panel1.Controls.Add(this.conditionControl1);
			this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer2.Size = new System.Drawing.Size(511, 594);
			this.splitContainer2.SplitterDistance = 190;
			this.splitContainer2.TabIndex = 0;
			this.conditionControl1.CurrentLine = null;
			this.conditionControl1.Dock = DockStyle.Fill;
			this.conditionControl1.Item = null;
			this.conditionControl1.Location = new Point(0, 0);
			this.conditionControl1.Name = "conditionControl1";
			this.conditionControl1.Size = new System.Drawing.Size(511, 190);
			this.conditionControl1.TabIndex = 35;
			this.splitContainer3.Dock = DockStyle.Fill;
			this.splitContainer3.Location = new Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = Orientation.Horizontal;
			this.splitContainer3.Panel1.Controls.Add(this.splitContainer4);
			this.splitContainer3.Panel2.Controls.Add(this.description);
			this.splitContainer3.Panel2.Controls.Add(this.selection);
			this.splitContainer3.Size = new System.Drawing.Size(511, 400);
			this.splitContainer3.SplitterDistance = 331;
			this.splitContainer3.TabIndex = 0;
			this.splitContainer4.Dock = DockStyle.Fill;
			this.splitContainer4.Location = new Point(0, 0);
			this.splitContainer4.Name = "splitContainer4";
			this.splitContainer4.Panel1.Controls.Add(this.propertyList);
			this.splitContainer4.Panel2.Controls.Add(this.referenceList);
			this.splitContainer4.Size = new System.Drawing.Size(511, 331);
			this.splitContainer4.SplitterDistance = 259;
			this.splitContainer4.TabIndex = 0;
			this.propertyList.DescriptionControl = this.description;
			this.propertyList.Dock = DockStyle.Fill;
			this.propertyList.Item = null;
			this.propertyList.Location = new Point(0, 0);
			this.propertyList.Name = "propertyList";
			this.propertyList.SelectionControl = this.selection;
			this.propertyList.ShowDescription = false;
			this.propertyList.Size = new System.Drawing.Size(259, 331);
			this.propertyList.TabIndex = 36;
			this.referenceList.DescriptionControl = this.description;
			this.referenceList.Dock = DockStyle.Fill;
			this.referenceList.Exclusions = (ArrayList)componentResourceManager.GetObject("referenceList.Exclusions");
			this.referenceList.Location = new Point(0, 0);
			this.referenceList.Name = "referenceList";
			this.referenceList.SelectionControl = this.selection;
			this.referenceList.ShowDescription = false;
			this.referenceList.Size = new System.Drawing.Size(248, 331);
			this.referenceList.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(772, 594);
			base.Controls.Add(this.splitContainer1);
			base.Name = "diplomat";
			base.ShowIcon = false;
			this.Text = "外交官和突击小队";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer2).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer3).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.splitContainer4.Panel1.ResumeLayout(false);
			this.splitContainer4.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer4).EndInit();
			this.splitContainer4.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			GameData.Item item = this.assaultList.Items[e.Item];
			if (e.Label == null)
			{
				return;
			}
			item.Name = e.Label;
			this.assaultList.RefreshItem(item);
			this.propertyList.refresh(this.selectedAssault);
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.assaultList.SelectedItems.Count == 0)
			{
				return;
			}
			this.selectedAssault = this.assaultList.SelectedItems[0];
			this.conditionControl1.refresh(this.selectedAssault);
			this.propertyList.refresh(this.selectedAssault);
			this.referenceList.refresh(this.selectedAssault);
		}

		private void removeAssault_Click(object sender, EventArgs e)
		{
			if (this.selectedAssault == null)
			{
				return;
			}
			this.Item.removeReference("assaults", this.selectedAssault);
			this.assaultList.RemoveItem(this.selectedAssault);
			this.assaultList.SelectedIndices.Clear();
			this.selectedAssault = null;
			this.conditionControl1.refresh(this.selectedAssault);
			this.propertyList.refresh(this.selectedAssault);
			this.referenceList.refresh(this.selectedAssault);
			this.nav.refreshState(this.Item);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			string text = ((TextBox)sender).Text;
			if (this.Item.Name != text)
			{
				this.Item.Name = text;
				this.nav.refreshState(this.Item);
			}
		}
	}
}