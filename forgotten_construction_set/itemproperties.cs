using forgotten_construction_set.Components;
using PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class itemproperties : Form
	{
		public navigation nav;

		private IContainer components;

		private SplitContainer split2;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ToolStripMenuItem bOpenReference;

		private Button bAddReference;

		private ComboBox referenceTypes;

		private ToolStripMenuItem bRemoveReference;

		private ObjectPropertyBox propertyList;

		private Button additionalCommands;

		private ToolTip tooltips;

		private SplitContainer split1;

		private PropertyGrid.PropertyGrid referenceList;

		private Label description;

		private Label selected;

		private ToolStripMenuItem bReplaceWithCopy;

		private System.Windows.Forms.ContextMenuStrip commandMenu;

		private ToolStripMenuItem showReferences;

		private ToolStripMenuItem copyData;

		private ToolStripMenuItem addTodo;

		private ToolStripMenuItem importScene;

		private ToolStripMenuItem duplicateItem;

		private ToolStripMenuItem addMissingFields;

		private ToolStripMenuItem cleanItem;

		private ToolStripMenuItem bRemoveSection;

		public GameData.Item Item
		{
			get;
			set;
		}

		public itemproperties()
		{
			this.InitializeComponent();
		}

		public itemproperties(GameData.Item itm, navigation n)
		{
			this.InitializeComponent();
			base.MdiParent = n.MdiParent;
			this.Item = itm;
			this.nav = n;
			this.Text = this.Item.Name;
			this.propertyList.setCustomDescriptionControls(this.selected, this.description);
			this.init();
		}

		private void additionalCommands_MouseDown(object sender, MouseEventArgs e)
		{
			Button button = sender as Button;
			this.commandMenu.Show(button, new Point(button.Width, 0));
			this.commandMenu.Focus();
		}

		private void addMissingFields_Click(object sender, EventArgs e)
		{
			if (this.Item.setMissingValues() > 0)
			{
				this.propertyList.refresh(this.Item);
				this.nav.refreshListView();
				this.nav.HasChanges = true;
			}
		}

		private void addTodo_Click(object sender, EventArgs e)
		{
			(this.nav.MdiParent as baseForm).todoList.AddItem(this.Item, "");
		}

		private void bAddReference_Click(object sender, EventArgs e)
		{
			if (this.referenceTypes.SelectedItem == null)
			{
				return;
			}
			string str = this.referenceTypes.SelectedItem.ToString();
			GameData.Desc desc = GameData.getDesc(this.Item.type, str);
			itemType _itemType = desc.list;
			string str1 = "";
			if (_itemType == itemType.NULL_ITEM)
			{
				_itemType = itemType.BUILDING;
				str1 = "is node=true";
			}
			ItemDialog itemDialog = new ItemDialog(string.Concat("Select ", str, " reference"), this.nav.ou.gameData, _itemType, true, str1, _itemType);
			if (itemDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				if (!(desc.defaultValue is GameData.Instance))
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
				}
				else
				{
					foreach (GameData.Item item1 in itemDialog.Items)
					{
						this.Item.addInstance(item1, (GameData.Instance)desc.defaultValue);
					}
				}
				this.refreshReferenceList();
				this.nav.refreshState(this.Item);
				this.nav.HasChanges = true;
			}
		}

		private void bRemoveReference_Click(object sender, EventArgs e)
		{
			string data;
			if (this.referenceList.SelectedItem == null)
			{
				return;
			}
			if (this.referenceList.SelectedItem.TextColour == StateColours.GetStateColor(GameData.State.LOCKED))
			{
				return;
			}
			string name = this.referenceList.SelectedSection.Name;
			string str = this.referenceList.SelectedItem.Name;
			if (this.referenceList.SelectedItem.Data is string)
			{
				data = (string)this.referenceList.SelectedItem.Data;
			}
			else
			{
				data = null;
			}
			string str1 = data;
			this.referenceList.removeItem(name, this.referenceList.SelectedItem);
			if (this.referenceList.getSection(name).Items.Count == 0)
			{
				this.referenceList.removeSection(name);
			}
			if (str1 != null)
			{
				this.Item.removeReference(name, str1);
			}
			else
			{
				this.Item.removeInstance(str);
			}
			this.nav.refreshState(this.Item);
			this.nav.HasChanges = true;
		}

		private void bRemoveSection_Click(object sender, EventArgs e)
		{
			if (this.referenceList.SelectedSection == null)
			{
				return;
			}
			string name = this.referenceList.SelectedSection.Name;
			List<string> strs = new List<string>();
			foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in this.Item.referenceData(name, false))
			{
				if (this.Item.getState(name, keyValuePair.Key) == GameData.State.LOCKED)
				{
					continue;
				}
				strs.Add(keyValuePair.Key);
			}
			if (strs.Count == 0)
			{
				return;
			}
			foreach (string str in strs)
			{
				this.Item.removeReference(name, str);
				PropertyGrid.PropertyGrid.Item item = this.referenceList.getSection(name).Items.Find((PropertyGrid.PropertyGrid.Item x) => x.Data.ToString() == str);
				this.referenceList.removeItem(name, item);
			}
			if (this.referenceList.getSection(name).Items.Count == 0)
			{
				this.referenceList.removeSection(name);
			}
			this.nav.refreshState(this.Item);
			this.nav.HasChanges = true;
		}

		private void cleanItem_Click(object sender, EventArgs e)
		{
			if (this.Item.clean() > 0)
			{
				this.propertyList.refresh(this.Item);
				this.nav.refreshListView();
				this.nav.HasChanges = true;
			}
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			GameData.Item item;
			if (this.referenceList.SelectedSection == null)
			{
				e.Cancel = true;
				return;
			}
			if (this.referenceList.SelectedItem == null)
			{
				this.bRemoveSection.Visible = true;
				this.bRemoveReference.Visible = false;
				this.bOpenReference.Visible = false;
				this.bReplaceWithCopy.Visible = false;
				return;
			}
			string data = (string)this.referenceList.SelectedItem.Data;
			if (data == null)
			{
				item = null;
			}
			else
			{
				item = this.nav.ou.gameData.getItem(data);
			}
			GameData.Item item1 = item;
			this.bRemoveSection.Visible = false;
			this.bRemoveReference.Visible = true;
			this.bOpenReference.Visible = true;
			this.bReplaceWithCopy.Visible = (item1 == null || item1.type == itemType.DIALOGUE ? false : item1.type != itemType.DIALOGUE_LINE);
			this.bRemoveReference.Enabled = this.referenceList.SelectedItem.TextColour != StateColours.GetStateColor(GameData.State.LOCKED);
		}

		private void copyData_Click(object sender, EventArgs e)
		{
			string str = "";
			ItemDialog itemDialog = new ItemDialog("Select Object To Copy From", this.nav.ou.gameData, this.Item.type, false, str, itemType.NULL_ITEM);
			if (itemDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.Item.clearInstances();
				foreach (GameData.Item item in itemDialog.Items)
				{
					foreach (string str1 in item.referenceLists())
					{
						foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in item.referenceData(str1, false))
						{
							int? nullable = null;
							int? nullable1 = nullable;
							nullable = null;
							int? nullable2 = nullable;
							nullable = null;
							this.Item.addReference(str1, keyValuePair.Key, nullable1, nullable2, nullable);
							this.Item.setReferenceValue(str1, keyValuePair.Key, keyValuePair.Value);
						}
					}
					this.Item.resolveReferences(this.nav.ou.gameData);
					foreach (KeyValuePair<string, object> value in item)
					{
						this.Item[value.Key] = value.Value;
					}
					this.nav.HasChanges = true;
				}
				this.nav.refreshState(this.Item);
				this.nav.HasChanges = true;
				this.refreshData();
				this.refreshReferenceList();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void duplicateItem_Click(object sender, EventArgs e)
		{
			GameData.Item item = this.nav.ou.gameData.cloneItem(this.Item);
			this.nav.showItemProperties(item);
			this.nav.refreshListView();
			this.nav.HasChanges = true;
		}

		private void Grid_OnPropertyChanged(object sender, PropertyChangedArgs e)
		{
			if (e.Item.Name == "Name")
			{
				this.Text = (string)e.Item.Value;
				Form[] mdiChildren = base.MdiParent.MdiChildren;
				for (int i = 0; i < (int)mdiChildren.Length; i++)
				{
					Form form = mdiChildren[i];
					if (form is itemproperties && form != this && (form as itemproperties).Item.hasReferenceTo(this.Item))
					{
						(form as itemproperties).refreshReferenceList();
					}
				}
			}
		}

		private void importScene_Click(object sender, EventArgs e)
		{
			(new OgreSceneImporter(this.Item, this.nav)).ShowDialog();
			this.refreshReferenceList();
		}

		private void init()
		{
			this.referenceList.DoubleClick += new EventHandler(this.openReference_Click);
			this.referenceList.OnPropertySelected += new PropertyGrid.PropertyGrid.PropertySelectedHandler(this.referenceList_OnPropertySelected);
			this.referenceList.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.referenceList_OnPropertyChanged);
			this.referenceList.AddPropertyType(typeof(GameData.Instance), new InstanceProperty(this.Item, this.nav));
			this.refreshData();
			this.refreshReferenceList();
			this.addTodo.Visible = (this.nav.MdiParent as baseForm).todoList != null;
			if (this.Item.type == itemType.BUILDING || this.Item.type == itemType.BUILDING_PART)
			{
				this.importScene.Visible = true;
				this.importScene.Enabled = this.Item.getState() != GameData.State.LOCKED;
			}
			else
			{
				this.importScene.Visible = false;
			}
			this.referenceTypes.Items.Clear();
			if (GameData.desc.ContainsKey(this.Item.type) && this.Item.getState() != GameData.State.LOCKED)
			{
				foreach (KeyValuePair<string, GameData.Desc> item in GameData.desc[this.Item.type])
				{
					if (!(item.Value.defaultValue is GameData.TripleInt) && !(item.Value.defaultValue is GameData.Instance))
					{
						continue;
					}
					this.referenceTypes.Items.Add(item.Key);
				}
			}
			if (this.referenceTypes.Items.Count > 0)
			{
				this.referenceTypes.SelectedIndex = 0;
				return;
			}
			this.referenceTypes.Enabled = false;
			this.bAddReference.Enabled = false;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.split2 = new SplitContainer();
			this.propertyList = new ObjectPropertyBox();
			this.referenceList = new PropertyGrid.PropertyGrid();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.bOpenReference = new ToolStripMenuItem();
			this.bRemoveReference = new ToolStripMenuItem();
			this.bReplaceWithCopy = new ToolStripMenuItem();
			this.bRemoveSection = new ToolStripMenuItem();
			this.additionalCommands = new Button();
			this.bAddReference = new Button();
			this.referenceTypes = new ComboBox();
			this.tooltips = new ToolTip(this.components);
			this.split1 = new SplitContainer();
			this.description = new Label();
			this.selected = new Label();
			this.commandMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showReferences = new ToolStripMenuItem();
			this.copyData = new ToolStripMenuItem();
			this.addTodo = new ToolStripMenuItem();
			this.importScene = new ToolStripMenuItem();
			this.duplicateItem = new ToolStripMenuItem();
			this.addMissingFields = new ToolStripMenuItem();
			this.cleanItem = new ToolStripMenuItem();
			((ISupportInitialize)this.split2).BeginInit();
			this.split2.Panel1.SuspendLayout();
			this.split2.Panel2.SuspendLayout();
			this.split2.SuspendLayout();
			this.contextMenu.SuspendLayout();
			((ISupportInitialize)this.split1).BeginInit();
			this.split1.Panel1.SuspendLayout();
			this.split1.Panel2.SuspendLayout();
			this.split1.SuspendLayout();
			this.commandMenu.SuspendLayout();
			base.SuspendLayout();
			this.split2.Dock = DockStyle.Fill;
			this.split2.Location = new Point(0, 0);
			this.split2.Name = "split2";
			this.split2.Panel1.Controls.Add(this.propertyList);
			this.split2.Panel2.Controls.Add(this.referenceList);
			this.split2.Panel2.Controls.Add(this.additionalCommands);
			this.split2.Panel2.Controls.Add(this.bAddReference);
			this.split2.Panel2.Controls.Add(this.referenceTypes);
			this.split2.Size = new System.Drawing.Size(529, 498);
			this.split2.SplitterDistance = 297;
			this.split2.TabIndex = 1;
			this.split2.TabStop = false;
			this.propertyList.BackColor = SystemColors.ControlDark;
			this.propertyList.Dock = DockStyle.Fill;
			this.propertyList.Item = null;
			this.propertyList.Location = new Point(0, 0);
			this.propertyList.Margin = new System.Windows.Forms.Padding(4);
			this.propertyList.Name = "propertyList";
			this.propertyList.ShowDescription = false;
			this.propertyList.Size = new System.Drawing.Size(297, 498);
			this.propertyList.TabIndex = 0;
			this.propertyList.Load += new EventHandler(this.objectPropertyBox1_Load);
			this.referenceList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.referenceList.AutoScroll = true;
			this.referenceList.AutoScrollMinSize = new System.Drawing.Size(0, 100);
			this.referenceList.BackColor = SystemColors.ControlDark;
			this.referenceList.ContextMenuStrip = this.contextMenu;
			this.referenceList.Location = new Point(3, 50);
			this.referenceList.Name = "referenceList";
			this.referenceList.Size = new System.Drawing.Size(224, 447);
			this.referenceList.TabIndex = 1;
			this.referenceList.Text = "propertyGrid1";
			this.referenceList.PreviewKeyDown += new PreviewKeyDownEventHandler(this.referenceList_PreviewKeyDown);
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.bOpenReference, this.bRemoveReference, this.bReplaceWithCopy, this.bRemoveSection });
			this.contextMenu.Name = "contextMenuStrip1";
			this.contextMenu.Size = new System.Drawing.Size(180, 92);
			this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
			this.bOpenReference.Name = "bOpenReference";
			this.bOpenReference.Size = new System.Drawing.Size(179, 22);
			this.bOpenReference.Text = "Open";
			this.bOpenReference.Click += new EventHandler(this.openReference_Click);
			this.bRemoveReference.Name = "bRemoveReference";
			this.bRemoveReference.Size = new System.Drawing.Size(179, 22);
			this.bRemoveReference.Text = "Remove from list";
			this.bRemoveReference.Click += new EventHandler(this.bRemoveReference_Click);
			this.bReplaceWithCopy.Name = "bReplaceWithCopy";
			this.bReplaceWithCopy.Size = new System.Drawing.Size(179, 22);
			this.bReplaceWithCopy.Text = "Replace with copy";
			this.bReplaceWithCopy.Click += new EventHandler(this.replaceWithCopyToolStripMenuItem_Click);
			this.bRemoveSection.Name = "bRemoveSection";
			this.bRemoveSection.Size = new System.Drawing.Size(179, 22);
			this.bRemoveSection.Text = "Remove all from list";
			this.bRemoveSection.Visible = false;
			this.bRemoveSection.Click += new EventHandler(this.bRemoveSection_Click);
			this.additionalCommands.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.additionalCommands.Location = new Point(200, 12);
			this.additionalCommands.Name = "additionalCommands";
			this.additionalCommands.Size = new System.Drawing.Size(25, 23);
			this.additionalCommands.TabIndex = 4;
			this.additionalCommands.Text = "...";
			this.tooltips.SetToolTip(this.additionalCommands, "List all game objects that reference this item");
			this.additionalCommands.UseVisualStyleBackColor = true;
			this.additionalCommands.MouseDown += new MouseEventHandler(this.additionalCommands_MouseDown);
			this.bAddReference.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.bAddReference.Location = new Point(135, 11);
			this.bAddReference.Name = "bAddReference";
			this.bAddReference.Size = new System.Drawing.Size(58, 23);
			this.bAddReference.TabIndex = 3;
			this.bAddReference.Text = "Add";
			this.tooltips.SetToolTip(this.bAddReference, "Add a reference to the listed section");
			this.bAddReference.UseVisualStyleBackColor = true;
			this.bAddReference.Click += new EventHandler(this.bAddReference_Click);
			this.referenceTypes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.referenceTypes.DropDownStyle = ComboBoxStyle.DropDownList;
			this.referenceTypes.FormattingEnabled = true;
			this.referenceTypes.Location = new Point(13, 12);
			this.referenceTypes.Name = "referenceTypes";
			this.referenceTypes.Size = new System.Drawing.Size(122, 21);
			this.referenceTypes.TabIndex = 2;
			this.referenceTypes.SelectedIndexChanged += new EventHandler(this.referenceTypes_SelectedIndexChanged);
			this.referenceTypes.Enter += new EventHandler(this.referenceTypes_Enter);
			this.split1.Dock = DockStyle.Fill;
			this.split1.Location = new Point(0, 0);
			this.split1.Name = "split1";
			this.split1.Orientation = Orientation.Horizontal;
			this.split1.Panel1.Controls.Add(this.split2);
			this.split1.Panel2.Controls.Add(this.description);
			this.split1.Panel2.Controls.Add(this.selected);
			this.split1.Size = new System.Drawing.Size(529, 559);
			this.split1.SplitterDistance = 498;
			this.split1.TabIndex = 2;
			this.split1.TabStop = false;
			this.description.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.description.Location = new Point(12, 13);
			this.description.Name = "description";
			this.description.Size = new System.Drawing.Size(517, 44);
			this.description.TabIndex = 1;
			this.description.Text = "Description";
			this.selected.AutoSize = true;
			this.selected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.selected.Location = new Point(3, 0);
			this.selected.Name = "selected";
			this.selected.Size = new System.Drawing.Size(85, 13);
			this.selected.TabIndex = 0;
			this.selected.Text = "Selected Item";
			this.commandMenu.Items.AddRange(new ToolStripItem[] { this.showReferences, this.copyData, this.addTodo, this.importScene, this.duplicateItem, this.addMissingFields, this.cleanItem });
			this.commandMenu.Name = "commandMenu";
			this.commandMenu.Size = new System.Drawing.Size(201, 180);
			this.showReferences.Name = "showReferences";
			this.showReferences.Size = new System.Drawing.Size(200, 22);
			this.showReferences.Text = "Show References";
			this.showReferences.Click += new EventHandler(this.showReferences_Click);
			this.copyData.Name = "copyData";
			this.copyData.Size = new System.Drawing.Size(200, 22);
			this.copyData.Text = "Copy data from Item";
			this.copyData.Click += new EventHandler(this.copyData_Click);
			this.addTodo.Name = "addTodo";
			this.addTodo.Size = new System.Drawing.Size(200, 22);
			this.addTodo.Text = "Add Todo List Item";
			this.addTodo.Click += new EventHandler(this.addTodo_Click);
			this.importScene.Name = "importScene";
			this.importScene.Size = new System.Drawing.Size(200, 22);
			this.importScene.Text = "Import Scene";
			this.importScene.Click += new EventHandler(this.importScene_Click);
			this.duplicateItem.Name = "duplicateItem";
			this.duplicateItem.Size = new System.Drawing.Size(200, 22);
			this.duplicateItem.Text = "Duplicate Item";
			this.duplicateItem.Click += new EventHandler(this.duplicateItem_Click);
			this.addMissingFields.Name = "addMissingFields";
			this.addMissingFields.Size = new System.Drawing.Size(200, 22);
			this.addMissingFields.Text = "Add Missing Fields";
			this.addMissingFields.Click += new EventHandler(this.addMissingFields_Click);
			this.cleanItem.Name = "cleanItem";
			this.cleanItem.Size = new System.Drawing.Size(200, 22);
			this.cleanItem.Text = "Remove Obsolete Fields";
			this.cleanItem.Click += new EventHandler(this.cleanItem_Click);
			this.AllowDrop = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(529, 559);
			base.Controls.Add(this.split1);
			base.Name = "itemproperties";
			this.Text = "itemproperties";
			base.Activated += new EventHandler(this.itemproperties_Activated);
			this.split2.Panel1.ResumeLayout(false);
			this.split2.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.split2).EndInit();
			this.split2.ResumeLayout(false);
			this.contextMenu.ResumeLayout(false);
			this.split1.Panel1.ResumeLayout(false);
			this.split1.Panel2.ResumeLayout(false);
			this.split1.Panel2.PerformLayout();
			((ISupportInitialize)this.split1).EndInit();
			this.split1.ResumeLayout(false);
			this.commandMenu.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void itemproperties_Activated(object sender, EventArgs e)
		{
		}

		private void objectPropertyBox1_Load(object sender, EventArgs e)
		{
			this.propertyList.setup(this.Item, this.nav);
		}

		private void openReference_Click(object sender, EventArgs e)
		{
			if (this.referenceList.SelectedItem == null)
			{
				return;
			}
			if (!(this.referenceList.SelectedItem.Value is GameData.TripleInt))
			{
				if (this.referenceList.SelectedItem.Value is GameData.Instance)
				{
					this.nav.showItemInstance(this.Item, this.referenceList.SelectedItem.Name, itemType.NULL_ITEM);
				}
				return;
			}
			string data = (string)this.referenceList.SelectedItem.Data;
			GameData.Item item = this.nav.ou.gameData.getItem(data);
			this.nav.showItemProperties(item);
		}

		private void referenceList_OnPropertyChanged(object sender, PropertyChangedArgs e)
		{
			if (e.Section.Name == "Instances")
			{
				return;
			}
			string str = e.Item.Data.ToString();
			if (this.Item.getState(e.Section.Name, str) == GameData.State.LOCKED)
			{
				return;
			}
			this.Item.setReferenceValue(e.Section.Name, str, (GameData.TripleInt)e.Item.Value);
			e.Item.TextColour = StateColours.GetStateColor(this.Item.getState(e.Section.Name, str));
			this.nav.refreshState(this.Item);
			this.nav.HasChanges = true;
		}

		private void referenceList_OnPropertySelected(object sender, PropertySelectedArgs e)
		{
			this.selected.Text = e.Section.Name;
			this.description.Text = GameData.getDesc(this.Item.type, e.Section.Name).description;
		}

		private void referenceList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				this.bRemoveReference_Click(sender, new EventArgs());
			}
		}

		private void referenceTypes_Enter(object sender, EventArgs e)
		{
			this.referenceTypes_SelectedIndexChanged(sender, e);
		}

		private void referenceTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			GameData.Desc desc = GameData.getDesc(this.Item.type, this.referenceTypes.Text);
			this.selected.Text = this.referenceTypes.Text;
			this.description.Text = desc.description;
		}

		public void refreshData()
		{
			this.propertyList.refresh(this.Item);
			this.propertyList.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.Grid_OnPropertyChanged);
		}

		public void refreshReferenceList()
		{
			KeyValuePair<string, GameData.Desc> keyValuePair;
			this.referenceList.clear();
			foreach (string str in this.Item.referenceLists())
			{
				GameData.Desc desc = GameData.getDesc(this.Item.type, str);
				int num = (desc == GameData.nullDesc ? 3 : desc.flags);
				TripleIntProperty tripleIntProperty = new TripleIntProperty(num);
				foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair1 in this.Item.referenceData(str, false))
				{
					GameData.Item item = this.nav.ou.gameData.getItem(keyValuePair1.Key);
					GameData.State state = this.Item.getState(str, keyValuePair1.Key);
					string str1 = (item != null ? item.Name : keyValuePair1.Key);
					Color stateColor = StateColours.GetStateColor(state);
					PropertyGrid.PropertyGrid.Item key = this.referenceList.addItem(str, str1, keyValuePair1.Value, desc.description, new Color?(stateColor), num > 0);
					key.Property = tripleIntProperty;
					key.Data = keyValuePair1.Key;
					key.Editable = state != GameData.State.LOCKED;
				}
			}
			if (this.Item.HasInstances)
			{
				Dictionary<itemType, KeyValuePair<string, GameData.Desc>> itemTypes = new Dictionary<itemType, KeyValuePair<string, GameData.Desc>>();
				if (GameData.desc.ContainsKey(this.Item.type))
				{
					foreach (KeyValuePair<string, GameData.Desc> item1 in GameData.desc[this.Item.type])
					{
						if (!(item1.Value.defaultValue is GameData.Instance))
						{
							continue;
						}
						itemTypes.Add(item1.Value.list, item1);
					}
				}
				if (itemTypes.Count != 0)
				{
					foreach (KeyValuePair<string, GameData.Instance> keyValuePair2 in this.Item.instanceData())
					{
						if (keyValuePair2.Value.getState() == GameData.State.REMOVED)
						{
							continue;
						}
						GameData.Item item2 = this.nav.ou.gameData.getItem(keyValuePair2.Value.sdata["ref"]);
						Color color = StateColours.GetStateColor(keyValuePair2.Value.getState());
						bool flag = keyValuePair2.Value.getState() != GameData.State.LOCKED;
						if (item2 == null || !itemTypes.TryGetValue(item2.type, out keyValuePair))
						{
							this.referenceList.addItem("Instances", keyValuePair2.Key, keyValuePair2.Value, "Object instances", new Color?(color), flag);
						}
						else
						{
							this.referenceList.addItem(keyValuePair.Key, keyValuePair2.Key, keyValuePair2.Value, keyValuePair.Value.description, new Color?(color), flag).Data = item2.type;
						}
					}
				}
				else
				{
					foreach (KeyValuePair<string, GameData.Instance> keyValuePair3 in this.Item.instanceData())
					{
						if (keyValuePair3.Value.getState() == GameData.State.REMOVED)
						{
							continue;
						}
						bool state1 = keyValuePair3.Value.getState() != GameData.State.LOCKED;
						Color stateColor1 = StateColours.GetStateColor(keyValuePair3.Value.getState());
						this.referenceList.addItem("Instances", keyValuePair3.Key, keyValuePair3.Value, "Object instances", new Color?(stateColor1), state1);
					}
				}
				if (this.nav.FileMode == navigation.ModFileMode.SINGLE)
				{
					foreach (KeyValuePair<string, GameData.Instance> keyValuePair4 in this.Item.instanceData())
					{
						if (keyValuePair4.Value.getState() != GameData.State.REMOVED)
						{
							continue;
						}
						Color color1 = StateColours.GetStateColor(keyValuePair4.Value.getState());
						this.referenceList.addItem("Instances", keyValuePair4.Key, keyValuePair4.Value, "Object instances", new Color?(color1), false);
					}
				}
			}
			this.referenceList.AutosizeDivider();
		}

		private void replaceWithCopyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.referenceList.SelectedItem != null && this.referenceList.SelectedItem.Value is GameData.TripleInt)
			{
				string data = (string)this.referenceList.SelectedItem.Data;
				GameData.Item item = this.nav.ou.gameData.getItem(data);
				if (this.referenceList.SelectedItem.TextColour == StateColours.GetStateColor(GameData.State.LOCKED))
				{
					return;
				}
				string name = this.referenceList.SelectedSection.Name;
				string str = this.referenceList.SelectedItem.Name;
				this.referenceList.removeItem(name, this.referenceList.SelectedItem);
				if (data != null)
				{
					this.Item.removeReference(name, data);
				}
				else
				{
					this.Item.removeInstance(str);
				}
				GameData.Item item1 = this.nav.ou.gameData.cloneItem(item);
				int? nullable = null;
				int? nullable1 = nullable;
				nullable = null;
				int? nullable2 = nullable;
				nullable = null;
				this.Item.addReference(name, item1, nullable1, nullable2, nullable);
				this.nav.refreshState(this.Item);
				this.nav.HasChanges = true;
				this.refreshData();
				this.refreshReferenceList();
			}
		}

		private void showReferences_Click(object sender, EventArgs e)
		{
			(new ReferencingItems(this.nav.ou.gameData, this.Item)).ShowDialog();
		}
	}
}