using forgotten_construction_set.Components;
using PropertyGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class ReferenceList : UserControl
	{
		private navigation nav;

		private bool readOnly;

		private IContainer components;

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

		public Control DescriptionControl
		{
			get;
			set;
		}

		public ArrayList Exclusions
		{
			get;
			set;
		}

		public GameData.Item Item
		{
			get;
			private set;
		}

		public bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				this.readOnly = value;
				this.splitContainer1.Panel1Collapsed = value;
				this.grid.Enabled = !value;
			}
		}

		public Control SelectionControl
		{
			get;
			set;
		}

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

		public ReferenceList()
		{
			this.readOnly = false;
			this.InitializeComponent();
			this.SelectionControl = this.selection;
			this.DescriptionControl = this.description;
			this.Exclusions = new ArrayList();
			this.grid.DoubleClick += new EventHandler(this.grid_DoubleClick);
			this.grid.OnPropertySelected += new PropertyGrid.PropertyGrid.PropertySelectedHandler(this.grid_OnPropertySelected);
			this.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			string str = this.addList.SelectedItem.ToString();
			GameData.Desc desc = GameData.getDesc(this.Item.type, str);
			ItemDialog itemDialog = new ItemDialog(string.Concat("选择 ", str, " 的关联"), this.nav.ou.gameData, desc.list, true, "", itemType.NULL_ITEM);
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
			this.openItem = new ToolStripMenuItem();
			this.revertItem = new ToolStripMenuItem();
			this.removeItem = new ToolStripMenuItem();
			this.split = new SplitContainer();
			this.splitContainer1 = new SplitContainer();
			this.addList = new ComboBox();
			this.addButton = new Button();
			this.grid = new PropertyGrid.PropertyGrid();
			this.description = new Label();
			this.selection = new Label();
			this.contextMenu.SuspendLayout();
			((ISupportInitialize)this.split).BeginInit();
			this.split.Panel1.SuspendLayout();
			this.split.Panel2.SuspendLayout();
			this.split.SuspendLayout();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			base.SuspendLayout();
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.openItem, this.revertItem, this.removeItem });
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(118, 70);
			this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
			this.openItem.Name = "openItem";
			this.openItem.Size = new System.Drawing.Size(117, 22);
			this.openItem.Text = "打开";
			this.openItem.Click += new EventHandler(this.openItem_Click);
			this.revertItem.Name = "revertItem";
			this.revertItem.Size = new System.Drawing.Size(117, 22);
			this.revertItem.Text = "还原";
			this.revertItem.Click += new EventHandler(this.revertItem_Click);
			this.removeItem.Name = "removeItem";
			this.removeItem.Size = new System.Drawing.Size(117, 22);
			this.removeItem.Text = "移除";
			this.removeItem.Click += new EventHandler(this.removeItem_Click);
			this.split.Dock = DockStyle.Fill;
			this.split.Location = new Point(0, 0);
			this.split.Name = "split";
			this.split.Orientation = Orientation.Horizontal;
			this.split.Panel1.Controls.Add(this.splitContainer1);
			this.split.Panel2.Controls.Add(this.description);
			this.split.Panel2.Controls.Add(this.selection);
			this.split.Size = new System.Drawing.Size(221, 210);
			this.split.SplitterDistance = 165;
			this.split.TabIndex = 36;
			this.split.TabStop = false;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.addList);
			this.splitContainer1.Panel1.Controls.Add(this.addButton);
			this.splitContainer1.Panel2.Controls.Add(this.grid);
			this.splitContainer1.Size = new System.Drawing.Size(221, 166);
			this.splitContainer1.SplitterDistance = 39;
			this.splitContainer1.TabIndex = 38;
			this.addList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.addList.FormattingEnabled = true;
			this.addList.Location = new Point(6, 9);
			this.addList.Name = "addList";
			this.addList.Size = new System.Drawing.Size(150, 21);
			this.addList.TabIndex = 37;
			this.addList.SelectedIndexChanged += new EventHandler(this.addList_SelectedIndexChanged);
			this.addList.Enter += new EventHandler(this.addList_Enter);
			this.addButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.addButton.Location = new Point(162, 9);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(56, 21);
			this.addButton.TabIndex = 34;
			this.addButton.Text = "添加";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new EventHandler(this.addButton_Click);
			this.grid.AutoScroll = true;
			this.grid.AutoScrollMinSize = new System.Drawing.Size(0, 100);
			this.grid.ContextMenuStrip = this.contextMenu;
			this.grid.Dock = DockStyle.Fill;
			this.grid.Location = new Point(0, 0);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(221, 123);
			this.grid.TabIndex = 0;
			this.description.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.description.Location = new Point(12, 13);
			this.description.Name = "description";
			this.description.Size = new System.Drawing.Size(206, 28);
			this.description.TabIndex = 1;
			this.description.Text = "描述";
			this.selection.AutoSize = true;
			this.selection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.selection.Location = new Point(3, 0);
			this.selection.Name = "selection";
			this.selection.Size = new System.Drawing.Size(85, 13);
			this.selection.TabIndex = 0;
			this.selection.Text = "已选择的项目";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.split);
			base.Name = "ReferenceList";
			base.Size = new System.Drawing.Size(221, 210);
			this.contextMenu.ResumeLayout(false);
			this.split.Panel1.ResumeLayout(false);
			this.split.Panel2.ResumeLayout(false);
			this.split.Panel2.PerformLayout();
			((ISupportInitialize)this.split).EndInit();
			this.split.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			base.ResumeLayout(false);
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