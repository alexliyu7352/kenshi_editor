using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace forgotten_construction_set
{
	public class navigation : Form
	{
		public head ou;

		private bool modHasChanged;

		private static Dictionary<GameData.Item, Form> forms;

		private static Dictionary<GameData.Instance, Form> instForms;

		private IContainer components;

		private SplitContainer splitContainer1;

		private TreeView treeView;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		private ToolStripMenuItem newItem;

		private ToolStripMenuItem duplicateItem;

		private GameDataList listView1;

		private ColumnHeader columnHeader2;

		private ToolStripMenuItem deleteItem;

		private ToolStripMenuItem clearChanges;

		private ToolStripMenuItem cleanItem;

		private TextBox filter;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem columnsMenu;

		private ToolStripMenuItem showStringID;

		private ToolStripMenuItem showRef;

		private ToolStripMenuItem showType;

		private ToolStripMenuItem openItem;

		private ToolStripMenuItem listReferences;

		private ToolStripSeparator toolStripSeparator3;

		private ToolStripMenuItem showData;

		private ToolStripSeparator toolStripSeparator4;

		private ToolStripMenuItem addColumn;

		private ToolStripMenuItem addRefColumn;

		private ToolStripMenuItem setField;

		public string ActiveFile
		{
			get;
			set;
		}

		public string BasePath
		{
			get;
			set;
		}

		public navigation.ModFileMode FileMode
		{
			get;
			set;
		}

		public bool HasChanges
		{
			get
			{
				return this.modHasChanged;
			}
			set
			{
				bool flag = this.modHasChanged != value;
				this.modHasChanged = value;
				if (flag)
				{
					((baseForm)base.MdiParent).updateTitle();
				}
			}
		}

		public string ModPath
		{
			get;
			set;
		}

		public string RootPath
		{
			get;
			set;
		}

		public bool SecretDeveloperMode
		{
			get;
			private set;
		}

		static navigation()
		{
			navigation.forms = new Dictionary<GameData.Item, Form>();
			navigation.instForms = new Dictionary<GameData.Instance, Form>();
		}

		public navigation()
		{
            // TODO 测试使用
            // this.SecretDeveloperMode = File.Exists("__Artist_Build_x64.exe");
            this.SecretDeveloperMode = true;
            this.ou = new head(this);
			this.InitializeComponent();
			this.treeView.SelectedNode = this.treeView.GetNodeAt(0, 0);
			this.createCategories();
		}

		private GameData.Item _getParentDialogueFromLineRecurse(GameData.Item line, ArrayList done)
		{
			GameData.Item item;
			if (line.type == itemType.DIALOGUE || line.type == itemType.WORD_SWAPS)
			{
				return line;
			}
			ArrayList arrayLists = new ArrayList();
			this.ou.gameData.getReferences(line, arrayLists);
			IEnumerator enumerator = arrayLists.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameData.Item current = (GameData.Item)enumerator.Current;
					if (done.Contains(current))
					{
						continue;
					}
					if (current.type != itemType.DIALOGUE)
					{
						done.Add(current);
						item = this._getParentDialogueFromLineRecurse(current, done);
						return item;
					}
					else
					{
						item = current;
						return item;
					}
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
			return item;
		}

		public void AddCategory(string name, itemType type)
		{
			this.AddCategory(name, new navigation.Filter(type, ""));
		}

		public void AddCategory(string name, navigation.Filter filter)
		{
			this.treeView.Nodes.Add(name, name).Tag = filter;
		}

		public void AddCategory(string parent, string name, itemType type)
		{
			this.AddCategory(parent, name, new navigation.Filter(type, ""));
		}

		public void AddCategory(string parent, string name, navigation.Filter filter)
		{
			TreeNode[] treeNodeArray = this.treeView.Nodes.Find(parent.ToString(), true);
			if (treeNodeArray.Length == 0)
			{
				this.AddCategory(name, filter);
				return;
			}
			treeNodeArray[0].Nodes.Add(name, name).Tag = filter;
		}

		private void addColumn_Click(object sender, EventArgs e)
		{
			System.Drawing.Size size = new System.Drawing.Size(200, 70);
			Form form = new Form()
			{
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog,
				MinimizeBox = false,
				MaximizeBox = false,
				ClientSize = size,
				Text = "新增列"
			};
			ComboBox comboBox = new ComboBox()
			{
				Size = new System.Drawing.Size(size.Width - 10, 23),
				Location = new Point(5, 5),
				Text = "",
				AutoCompleteMode = AutoCompleteMode.SuggestAppend,
				AutoCompleteSource = AutoCompleteSource.ListItems
			};
			foreach (KeyValuePair<string, GameData.Desc> currentFilterDescList in this.getCurrentFilterDescList())
			{
				if (GameData.isListType(currentFilterDescList.Value))
				{
					continue;
				}
				comboBox.Items.Add(currentFilterDescList.Key);
			}
			form.Controls.Add(comboBox);
			Button button = new Button()
			{
				DialogResult = System.Windows.Forms.DialogResult.OK,
				Name = "okButton",
				Size = new System.Drawing.Size(75, 23),
				Text = "&Add",
				Location = new Point(size.Width - 80 - 80, 39)
			};
			form.Controls.Add(button);
			Button button1 = new Button()
			{
				DialogResult = System.Windows.Forms.DialogResult.Cancel,
				Name = "cancelButton",
				Size = new System.Drawing.Size(75, 23),
				Text = "&Cancel",
				Location = new Point(size.Width - 80, 39)
			};
			form.Controls.Add(button1);
			form.AcceptButton = button;
			form.CancelButton = button1;
			if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK && comboBox.Text.Trim() != "")
			{
				string str = comboBox.Text.Trim();
				ToolStripMenuItem toolStripMenuItem = null;
				foreach (ToolStripItem dropDownItem in this.columnsMenu.DropDownItems)
				{
					if (dropDownItem.Tag == null || !(dropDownItem.Tag.ToString() == str))
					{
						continue;
					}
					toolStripMenuItem = dropDownItem as ToolStripMenuItem;
					goto Label0;
				}
			Label0:
				if (toolStripMenuItem == null)
				{
					this.listView1.AddColumn(str, str);
					toolStripMenuItem = new ToolStripMenuItem(str, null, new EventHandler(this.showColumn_Click))
					{
						CheckOnClick = true,
						CheckState = CheckState.Checked
					};
					this.columnsMenu.DropDownItems.Insert(this.columnsMenu.DropDownItems.Count - 3, toolStripMenuItem);
					return;
				}
				if (toolStripMenuItem.CheckState != CheckState.Checked)
				{
					toolStripMenuItem.CheckState = CheckState.Checked;
					this.showColumn_Click(toolStripMenuItem, new EventArgs());
				}
			}
		}

		private void addRefColumn_Click(object sender, EventArgs e)
		{
			System.Drawing.Size size = new System.Drawing.Size(200, 84);
			Form form = new Form()
			{
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog,
				MinimizeBox = false,
				MaximizeBox = false,
				ClientSize = size,
				Text = "Add List Column"
			};
			ComboBox comboBox = new ComboBox()
			{
				Size = new System.Drawing.Size(size.Width - 10, 23),
				Location = new Point(5, 5),
				Text = "",
				AutoCompleteMode = AutoCompleteMode.SuggestAppend,
				AutoCompleteSource = AutoCompleteSource.ListItems
			};
			foreach (KeyValuePair<string, GameData.Desc> currentFilterDescList in this.getCurrentFilterDescList())
			{
				if (!(currentFilterDescList.Value.defaultValue is GameData.TripleInt))
				{
					continue;
				}
				comboBox.Items.Add(currentFilterDescList.Key);
			}
			form.Controls.Add(comboBox);
			CheckBox checkBox = new CheckBox()
			{
				Text = "Items",
				Checked = false,
				Size = new System.Drawing.Size((size.Width - 10) / 2, 23),
				Location = new Point(5, 30)
			};
			form.Controls.Add(checkBox);
			CheckBox checkBox1 = new CheckBox()
			{
				Text = "Values",
				Checked = false,
				Size = new System.Drawing.Size((size.Width - 10) / 2, 23),
				Location = new Point(size.Width / 2, 30)
			};
			form.Controls.Add(checkBox1);
			Button button = new Button()
			{
				DialogResult = System.Windows.Forms.DialogResult.OK,
				Name = "okButton",
				Size = new System.Drawing.Size(75, 23),
				Text = "&Add",
				Location = new Point(size.Width - 80 - 80, 56)
			};
			form.Controls.Add(button);
			Button button1 = new Button()
			{
				DialogResult = System.Windows.Forms.DialogResult.Cancel,
				Name = "cancelButton",
				Size = new System.Drawing.Size(75, 23),
				Text = "&Cancel",
				Location = new Point(size.Width - 80, 56)
			};
			form.Controls.Add(button1);
			form.AcceptButton = button;
			form.CancelButton = button1;
			if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK && comboBox.Text.Trim() != "")
			{
				string str = comboBox.Text.Trim();
				ToolStripMenuItem toolStripMenuItem = null;
				foreach (ToolStripItem dropDownItem in this.columnsMenu.DropDownItems)
				{
					if (dropDownItem.Tag == null || !(dropDownItem.Tag.ToString() == str))
					{
						continue;
					}
					toolStripMenuItem = dropDownItem as ToolStripMenuItem;
					goto Label0;
				}
			Label0:
				if (toolStripMenuItem == null)
				{
					GameDataList.ColumnInfo columnInfo = this.listView1.AddReferenceColumn(str, str, checkBox.Checked, checkBox1.Checked);
					toolStripMenuItem = new ToolStripMenuItem(str, null, new EventHandler(this.showColumn_Click))
					{
						Tag = columnInfo,
						CheckOnClick = true,
						CheckState = CheckState.Checked
					};
					this.columnsMenu.DropDownItems.Insert(this.columnsMenu.DropDownItems.Count - 3, toolStripMenuItem);
					return;
				}
				if (toolStripMenuItem.CheckState == CheckState.Checked)
				{
					this.listView1.RemoveColumn(str);
				}
				toolStripMenuItem.Tag = this.listView1.AddReferenceColumn(str, str, checkBox.Checked, checkBox1.Checked);
				toolStripMenuItem.CheckState = CheckState.Checked;
			}
		}

		public void addTodoMenuItem()
		{
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Add Todo Item");
			toolStripMenuItem.Click += new EventHandler(this.Todo_Click);
			this.contextMenuStrip1.Items.Insert(2, toolStripMenuItem);
		}

		private void cleanItem_Click(object sender, EventArgs e)
		{
			foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
			{
				selectedItem.clean();
				this.refreshState(selectedItem);
				if (navigation.forms.ContainsKey(selectedItem))
				{
					navigation.forms[selectedItem].Close();
				}
				this.HasChanges = true;
			}
		}

		public void clearCategories()
		{
			this.treeView.Nodes.Clear();
		}

		private void clearChanges_Click(object sender, EventArgs e)
		{
			foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
			{
				selectedItem.revert();
				this.refreshState(selectedItem);
				if (navigation.forms.ContainsKey(selectedItem))
				{
					navigation.forms[selectedItem].Close();
				}
				this.HasChanges = true;
			}
		}

		public void clearFilter()
		{
			this.filter.Text = "";
			this.filter.BackColor = SystemColors.Window;
		}

		public void closeItemProperties(GameData.Item item)
		{
			Form form;
			if (navigation.forms.TryGetValue(item, out form))
			{
				form.Close();
			}
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			bool count = this.listView1.SelectedIndices.Count > 0;
			this.duplicateItem.Enabled = count;
			this.deleteItem.Enabled = count;
			this.openItem.Enabled = count;
			this.listReferences.Enabled = count;
			this.cleanItem.Enabled = count;
			this.clearChanges.Enabled = false;
			this.setField.Enabled = count;
			foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
			{
				if (selectedItem.getState() == GameData.State.MODIFIED || selectedItem.getState() == GameData.State.REMOVED)
				{
					this.clearChanges.Enabled = true;
				}
				if (selectedItem.type != itemType.DIALOGUE && selectedItem.type != itemType.DIALOGUE_LINE && selectedItem.type != itemType.WORD_SWAPS && selectedItem.type != itemType.DIALOGUE_PACKAGE)
				{
					continue;
				}
				this.duplicateItem.Enabled = false;
			}
		}

		private void createCategories()
		{
			this.treeView.Nodes.Clear();
			this.AddCategory("Building", new navigation.Filter(itemType.BUILDING, "building category!=DOORS;building category!=WALLS;is interior furniture=false;is exterior furniture=false;is node=false"));
			this.AddCategory("Building", "Walls", new navigation.Filter(itemType.BUILDING, "building category==WALLS"));
			this.AddCategory("Building", "Furniture", new navigation.Filter(itemType.BUILDING, "is interior furniture=true"));
			this.AddCategory("Building", "Exterior Furniture", new navigation.Filter(itemType.BUILDING, "is exterior furniture=true"));
			this.AddCategory("Building", "Nodes", new navigation.Filter(itemType.BUILDING, "is node=true"));
			this.AddCategory("Building", "Doors", new navigation.Filter(itemType.BUILDING, "building category==DOORS"));
			this.AddCategory("Building", "Parts", itemType.BUILDING_PART);
			this.AddCategory("Building", "Materials", itemType.MATERIAL_SPEC);
			this.AddCategory("Building", "Functionality", itemType.BUILDING_FUNCTIONALITY);
			this.AddCategory("Building", "Farm Data", itemType.FARM_DATA);
			this.AddCategory("Building", "Lights", itemType.LIGHT);
			this.AddCategory("Farm Data", "Plants", itemType.FARM_PART);
			this.AddCategory("Items", new navigation.Filter(itemType.ITEM, "item function!=ITEM_BOOK"));
			this.AddCategory("Items", "Weapons", itemType.WEAPON);
			this.AddCategory("Weapons", "Models", itemType.MATERIAL_SPECS_WEAPON);
			this.AddCategory("Weapons", "Manufacturer", itemType.WEAPON_MANUFACTURER);
			this.AddCategory("Items", "Ranged Weapons", itemType.GUN_DATA);
			this.AddCategory("Items", "Crossbows", itemType.CROSSBOW);
			this.AddCategory("Items", "Armour", itemType.ARMOUR);
			this.AddCategory("Items", "Bags", itemType.CONTAINER);
			this.AddCategory("Items", "Materials", itemType.MATERIAL_SPECS_CLOTHING);
			this.AddCategory("Items", "Placeable Groups", itemType.ITEM_PLACEMENT_GROUP);
			this.AddCategory("Items", "Nest Items", itemType.NEST_ITEM);
			this.AddCategory("Items", "Physics Attachment", itemType.CHARACTER_PHYSICS_ATTACHMENT);
			this.AddCategory("Items", "Maps", itemType.MAP_ITEM);
			this.AddCategory("Items", "Books", new navigation.Filter(itemType.ITEM, "item function=ITEM_BOOK"));
			this.AddCategory("Items", "Limb replacement", itemType.LIMB_REPLACEMENT);
			this.AddCategory("Dialogue Package", itemType.DIALOGUE_PACKAGE);
			this.AddCategory("Dialogue Package", "Dialogue", itemType.DIALOGUE);
			this.AddCategory("Dialogue Package", "Diplomatic Assaults", itemType.DIPLOMATIC_ASSAULTS);
			this.AddCategory("Dialogue Package", "Word Swaps", itemType.WORD_SWAPS);
			this.AddCategory("Dialogue Package", "World States", itemType.WORLD_EVENT_STATE);
			this.AddCategory("Dialogue Package", "_lines", itemType.DIALOGUE_LINE);
			this.AddCategory("Races", itemType.RACE);
			this.AddCategory("Races", "Groups", itemType.RACE_GROUP);
			this.AddCategory("Characters", itemType.CHARACTER);
			this.AddCategory("Characters", "Stats", itemType.STATS);
			this.AddCategory("Characters", "Personality", itemType.PERSONALITY);
			this.AddCategory("Characters", "Hair", itemType.ATTACHMENT);
			this.AddCategory("Characters", "Head", itemType.HEAD);
			this.AddCategory("Characters", "Animals", itemType.ANIMAL_CHARACTER);
			this.AddCategory("Animation", itemType.ANIMATION);
			this.AddCategory("Animation", "Animation Events", itemType.ANIMATION_EVENT);
			this.AddCategory("Animation", "Animal Animations", itemType.ANIMAL_ANIMATION);
			this.AddCategory("Animation", "Combat Techniques", itemType.COMBAT_TECHNIQUE);
			this.AddCategory("Animation", "Animation Files", itemType.ANIMATION_FILE);
			this.AddCategory("Faction", itemType.FACTION);
			this.AddCategory("Faction", "Squads", itemType.SQUAD_TEMPLATE);
			this.AddCategory("Faction", "Special Squads", itemType.UNIQUE_SQUAD_TEMPLATE);
			this.AddCategory("Faction", "Faction Templates", itemType.FACTION_TEMPLATE);
			this.AddCategory("Faction", "Buildings swaps", itemType.BUILDINGS_SWAP);
			this.AddCategory("Faction", "Trade Culture", itemType.ITEMS_CULTURE);
			this.AddCategory("Towns", itemType.TOWN);
			this.AddCategory("Locational Damage", itemType.LOCATIONAL_DAMAGE);
			this.AddCategory("Biomes", itemType.BIOMES);
			this.AddCategory("Biomes", "Foliage Layers", itemType.FOLIAGE_LAYER);
			this.AddCategory("Foliage Layers", "Meshes", itemType.FOLIAGE_MESH);
			this.AddCategory("Foliage Layers", "Grass", itemType.GRASS);
			this.AddCategory("Biomes", "Map Features", itemType.MAP_FEATURES);
			this.AddCategory("Biomes", "Birds", itemType.WILDLIFE_BIRDS);
			this.AddCategory("Biomes", "Weather", itemType.WEATHER);
			this.AddCategory("Biomes", "Spawn Area", itemType.BIOME_GROUP);
			this.AddCategory("Biomes", "Resources", itemType.ENVIRONMENT_RESOURCES);
			this.AddCategory("Weather", "Seasons", itemType.SEASON);
			this.AddCategory("AI Packages", itemType.AI_PACKAGE);
			this.AddCategory("AI Packages", "Goals", itemType.AI_TASK);
			this.AddCategory("AI Packages", "War Campaigns", itemType.FACTION_CAMPAIGN);
			this.AddCategory("Vendor Lists", itemType.VENDOR_LIST);
			this.AddCategory("Colour Scheme", itemType.COLOR_DATA);
			this.AddCategory("Research", itemType.RESEARCH);
			this.AddCategory("Game Starts", itemType.NEW_GAME_STARTOFF);
			this.AddCategory("Effects", itemType.EFFECT);
			this.AddCategory("Effects", "Effect volumes", itemType.EFFECT_FOG_VOLUME);
			this.AddCategory("Ambient Sounds", itemType.AMBIENT_SOUND);
		}

		private void deleteItem_Click(object sender, EventArgs e)
		{
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
			{
				this.ou.gameData.deleteItem(selectedItem);
				if (!navigation.forms.ContainsKey(selectedItem))
				{
					continue;
				}
				navigation.forms[selectedItem].Close();
			}
			this.refreshListView();
			System.Windows.Forms.Cursor.Current = Cursors.Default;
			this.HasChanges = true;
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
			foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
			{
				this.showItemProperties(this.ou.gameData.cloneItem(selectedItem));
			}
			this.refreshListView();
			this.HasChanges = true;
		}

		public void ensureCategoryIsSelected()
		{
			if (this.treeView.SelectedNode == null)
			{
				this.treeView.SelectedNode = this.treeView.Nodes[0];
			}
		}

		private void filter_TextChanged(object sender, EventArgs e)
		{
			this.refreshListView();
			this.filter.BackColor = (this.filter.Text == "" ? SystemColors.Window : Color.Yellow);
		}

		private void form_FormClosed(object sender, FormClosedEventArgs e)
		{
			Form form = (Form)sender;
			foreach (KeyValuePair<GameData.Item, Form> keyValuePair in navigation.forms)
			{
				if (keyValuePair.Value != form)
				{
					continue;
				}
				navigation.forms.Remove(keyValuePair.Key);
				return;
			}
		}

		private SortedList<string, GameData.Desc> getCurrentFilterDescList()
		{
			if (this.treeView.SelectedNode != null)
			{
				itemType type = (this.treeView.SelectedNode.Tag as navigation.Filter).Type;
				if (GameData.desc.ContainsKey(type))
				{
					return GameData.desc[type];
				}
			}
			return new SortedList<string, GameData.Desc>();
		}

		private GameData.Item getParentDialogueFromLine(GameData.Item line)
		{
			return this._getParentDialogueFromLineRecurse(line, new ArrayList());
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(navigation));
			this.splitContainer1 = new SplitContainer();
			this.treeView = new TreeView();
			this.filter = new TextBox();
			this.listView1 = new GameDataList();
			this.columnHeader2 = new ColumnHeader();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openItem = new ToolStripMenuItem();
			this.listReferences = new ToolStripMenuItem();
			this.toolStripSeparator3 = new ToolStripSeparator();
			this.newItem = new ToolStripMenuItem();
			this.duplicateItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.cleanItem = new ToolStripMenuItem();
			this.setField = new ToolStripMenuItem();
			this.clearChanges = new ToolStripMenuItem();
			this.deleteItem = new ToolStripMenuItem();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.columnsMenu = new ToolStripMenuItem();
			this.showStringID = new ToolStripMenuItem();
			this.showRef = new ToolStripMenuItem();
			this.showType = new ToolStripMenuItem();
			this.showData = new ToolStripMenuItem();
			this.toolStripSeparator4 = new ToolStripSeparator();
			this.addColumn = new ToolStripMenuItem();
			this.addRefColumn = new ToolStripMenuItem();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.FixedPanel = FixedPanel.Panel1;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.treeView);
			this.splitContainer1.Panel1MinSize = 100;
			this.splitContainer1.Panel2.Controls.Add(this.filter);
			this.splitContainer1.Panel2.Controls.Add(this.listView1);
			this.splitContainer1.Size = new System.Drawing.Size(800, 564);
			this.splitContainer1.SplitterDistance = 186;
			this.splitContainer1.TabIndex = 0;
			this.splitContainer1.TabStop = false;
			this.treeView.Dock = DockStyle.Fill;
			this.treeView.HideSelection = false;
			this.treeView.Location = new Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(186, 564);
			this.treeView.TabIndex = 0;
			this.treeView.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
			this.filter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.filter.BackColor = SystemColors.Window;
			this.filter.Location = new Point(0, 544);
			this.filter.Margin = new System.Windows.Forms.Padding(2);
			this.filter.Name = "filter";
			this.filter.Size = new System.Drawing.Size(610, 20);
			this.filter.TabIndex = 2;
			this.filter.TextChanged += new EventHandler(this.filter_TextChanged);
			this.listView1.AllowColumnReorder = true;
			this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.listView1.Columns.AddRange(new ColumnHeader[] { this.columnHeader2 });
			this.listView1.ContextMenuStrip = this.contextMenuStrip1;
			this.listView1.Filter = null;
			this.listView1.FullRowSelect = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(610, 543);
			this.listView1.Sorting = SortOrder.Ascending;
			this.listView1.SortKey = "Name";
			this.listView1.Source = null;
			this.listView1.TabIndex = 1;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = View.Details;
			this.listView1.VirtualMode = true;
			this.listView1.ColumnClick += new ColumnClickEventHandler(this.listView1_ColumnClick);
			this.listView1.DoubleClick += new EventHandler(this.listView1_DoubleClick);
			this.listView1.KeyDown += new KeyEventHandler(this.listView1_KeyDown);
			this.columnHeader2.Tag = "Name";
			this.columnHeader2.Text = "Name";
			this.columnHeader2.Width = 308;
			this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.openItem, this.listReferences, this.toolStripSeparator3, this.newItem, this.duplicateItem, this.toolStripSeparator1, this.cleanItem, this.setField, this.clearChanges, this.deleteItem, this.toolStripSeparator2, this.columnsMenu });
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(157, 220);
			this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
			this.openItem.Name = "openItem";
			this.openItem.Size = new System.Drawing.Size(156, 22);
			this.openItem.Text = "Open Item";
			this.openItem.Click += new EventHandler(this.openItem_Click);
			this.listReferences.Name = "listReferences";
			this.listReferences.Size = new System.Drawing.Size(156, 22);
			this.listReferences.Text = "List References";
			this.listReferences.Click += new EventHandler(this.listReferences_Click);
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(153, 6);
			this.newItem.Name = "newItem";
			this.newItem.Size = new System.Drawing.Size(156, 22);
			this.newItem.Text = "New Item";
			this.newItem.Click += new EventHandler(this.newItem_Click);
			this.duplicateItem.Name = "duplicateItem";
			this.duplicateItem.Size = new System.Drawing.Size(156, 22);
			this.duplicateItem.Text = "Duplicate Item";
			this.duplicateItem.Click += new EventHandler(this.duplicateItem_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
			this.cleanItem.Name = "cleanItem";
			this.cleanItem.Size = new System.Drawing.Size(156, 22);
			this.cleanItem.Text = "Clean Item";
			this.cleanItem.Click += new EventHandler(this.cleanItem_Click);
			this.setField.Name = "setField";
			this.setField.Size = new System.Drawing.Size(156, 22);
			this.setField.Text = "Set Field";
			this.setField.Click += new EventHandler(this.setField_Click);
			this.clearChanges.Name = "clearChanges";
			this.clearChanges.Size = new System.Drawing.Size(156, 22);
			this.clearChanges.Text = "Revert Changes";
			this.clearChanges.Click += new EventHandler(this.clearChanges_Click);
			this.deleteItem.Name = "deleteItem";
			this.deleteItem.Size = new System.Drawing.Size(156, 22);
			this.deleteItem.Text = "Delete Item";
			this.deleteItem.Click += new EventHandler(this.deleteItem_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(153, 6);
			this.columnsMenu.DropDownItems.AddRange(new ToolStripItem[] { this.showStringID, this.showRef, this.showType, this.showData, this.toolStripSeparator4, this.addColumn, this.addRefColumn });
			this.columnsMenu.Name = "columnsMenu";
			this.columnsMenu.Size = new System.Drawing.Size(156, 22);
			this.columnsMenu.Text = "Columns";
			this.showStringID.CheckOnClick = true;
			this.showStringID.Name = "showStringID";
			this.showStringID.Size = new System.Drawing.Size(117, 22);
			this.showStringID.Text = "StringID";
			this.showStringID.Click += new EventHandler(this.showColumn_Click);
			this.showRef.CheckOnClick = true;
			this.showRef.Name = "showRef";
			this.showRef.Size = new System.Drawing.Size(117, 22);
			this.showRef.Text = "Ref";
			this.showRef.Click += new EventHandler(this.showColumn_Click);
			this.showType.CheckOnClick = true;
			this.showType.Name = "showType";
			this.showType.Size = new System.Drawing.Size(117, 22);
			this.showType.Text = "Type";
			this.showType.Click += new EventHandler(this.showColumn_Click);
			this.showData.CheckOnClick = true;
			this.showData.Name = "showData";
			this.showData.Size = new System.Drawing.Size(117, 22);
			this.showData.Text = "Data";
			this.showData.Click += new EventHandler(this.showColumn_Click);
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(114, 6);
			this.addColumn.Name = "addColumn";
			this.addColumn.Size = new System.Drawing.Size(117, 22);
			this.addColumn.Text = "Add";
			this.addColumn.Click += new EventHandler(this.addColumn_Click);
			this.addRefColumn.Name = "addRefColumn";
			this.addRefColumn.Size = new System.Drawing.Size(117, 22);
			this.addRefColumn.Text = "Add List";
			this.addRefColumn.Click += new EventHandler(this.addRefColumn_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(800, 564);
			base.ControlBox = false;
			base.Controls.Add(this.splitContainer1);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "navigation";
			this.Text = "Game World";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void instForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			InstanceDialog instanceDialog = (InstanceDialog)sender;
			navigation.instForms.Remove(instanceDialog.Instance);
		}

		private void listReferences_Click(object sender, EventArgs e)
		{
			GameData.Item item = this.listView1.SelectedItems[0];
			(new ReferencingItems(this.ou.gameData, item)).ShowDialog();
		}

		private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			string str = this.listView1.Columns[e.Column].Tag.ToString();
			if (str == "Data" || this.listView1.Columns[e.Column].Tag is GameDataList.ColumnInfo)
			{
				return;
			}
			if (str != this.listView1.SortKey || this.listView1.Sorting == SortOrder.Descending)
			{
				this.listView1.Sorting = SortOrder.Ascending;
			}
			else
			{
				this.listView1.Sorting = SortOrder.Descending;
			}
			this.listView1.SortKey = str;
			this.refreshListView();
		}

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			if (Control.ModifierKeys == Keys.Control)
			{
				return;
			}
			GameData.Item item = this.listView1.SelectedItems[0];
			if (item.getState() == GameData.State.REMOVED)
			{
				if (MessageBox.Show(string.Concat("Item ", item.Name, " has been removed by the active mod.\nDo you want to restore it?"), "Removed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
				{
					return;
				}
				item.revert();
				this.refreshState(item);
			}
			else if (item.getState() == GameData.State.LOCKED_REMOVED)
			{
				MessageBox.Show(string.Concat("Item ", item.Name, " has been removed by a later mod."), "Removed");
				return;
			}
			this.showItemProperties(item);
		}

		private void listView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
				{
					this.showItemProperties(selectedItem);
				}
			}
		}

		private void newItem_Click(object sender, EventArgs e)
		{
			navigation.Filter tag = (navigation.Filter)this.treeView.SelectedNode.Tag;
			GameData.Item item = this.ou.gameData.createItem(tag.Type);
			if (!string.IsNullOrEmpty(tag.Custom))
			{
				this.setValuesFromFilter(item, tag.Custom);
			}
			this.showItemProperties(item);
			this.refreshListView();
			this.HasChanges = true;
		}

		private void openItem_Click(object sender, EventArgs e)
		{
			foreach (GameData.Item selectedItem in this.listView1.SelectedItems)
			{
				this.showItemProperties(selectedItem);
			}
		}

		public void refreshItemWindow(GameData.Item item)
		{
			Form form;
			if (navigation.forms.TryGetValue(item, out form) && form is itemproperties)
			{
				(form as itemproperties).refreshReferenceList();
			}
		}

		public void refreshListView()
		{
			if (TranslationManager.TranslationMode)
			{
				return;
			}
			if (this.treeView.SelectedNode == null)
			{
				itemType? nullable = null;
				this.listView1.UpdateItems(this.ou.gameData, nullable, this.filter.Text);
				return;
			}
			navigation.Filter tag = (navigation.Filter)this.treeView.SelectedNode.Tag;
			if (tag == null)
			{
				return;
			}
			this.listView1.UpdateItems(this.ou.gameData, new itemType?(tag.Type), string.Concat(this.filter.Text, ";", tag.Custom));
		}

		public void refreshListViewAll()
		{
			this.treeView.SelectedNode = null;
			this.refreshListView();
		}

		public void refreshState(GameData.Item item)
		{
			this.listView1.RefreshItem(item);
		}

		public void setActiveFilename(string file, navigation.ModFileMode mode)
		{
			this.FileMode = mode;
			this.ActiveFile = file;
			this.ou.setActiveFilename(file);
			this.splitContainer1.Panel1Collapsed = this.FileMode == navigation.ModFileMode.SINGLE;
		}

		private void setField_Click(object sender, EventArgs e)
		{
			if (this.listView1.SelectedItems.Count > 0)
			{
				(new SetFieldValue(this, this.listView1.SelectedItems)).ShowDialog();
			}
		}

		private void setValuesFromFilter(GameData.Item item, string filter)
		{
			string[] strArrays = filter.Split(new char[] { ';' });
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string[] strArrays1 = strArrays[i].Split(new char[] { '=' });
				if ((int)strArrays1.Length == 2)
				{
					item[strArrays1[0].Trim()] = strArrays1[1].Trim() == "true";
				}
			}
		}

		private void showColumn_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			if (!toolStripMenuItem.Checked)
			{
				this.listView1.RemoveColumn(toolStripMenuItem.Text);
				return;
			}
			if (toolStripMenuItem.Tag == null)
			{
				this.listView1.AddColumn(toolStripMenuItem.Text, toolStripMenuItem.Text);
				return;
			}
			this.listView1.AddColumn(toolStripMenuItem.Text, toolStripMenuItem.Tag as GameDataList.ColumnInfo);
		}

		public void showItemInstance(GameData.Item parent, string id, itemType mask = itemType.ITEM)
		{
			GameData.Instance instance = parent.getInstance(id);
			if (navigation.instForms.ContainsKey(instance))
			{
				navigation.instForms[instance].BringToFront();
				return;
			}
			InstanceDialog instanceDialog = new InstanceDialog(parent, id, mask, this);
			instanceDialog.FormClosed += new FormClosedEventHandler(this.instForm_FormClosed);
			navigation.instForms.Add(instance, instanceDialog);
			instanceDialog.MdiParent = base.MdiParent;
			instanceDialog.Show();
		}

		public void showItemProperties(GameData.Item item)
		{
			Form itemproperty;
			if (item == null)
			{
				return;
			}
			if (navigation.forms.ContainsKey(item))
			{
				navigation.forms[item].BringToFront();
				return;
			}
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			if (Control.ModifierKeys == Keys.Alt)
			{
				itemproperty = new itemproperties(item, this);
			}
			else if (item.type == itemType.WORD_SWAPS)
			{
				itemproperty = new wordswaps(item, this);
			}
			else if (item.type == itemType.DIALOGUE)
			{
				itemproperty = new conversation(item, this);
			}
			else if (item.type == itemType.DIALOGUE_PACKAGE)
			{
				itemproperty = new dialogCollection(item, this);
			}
			else if (item.type == itemType.DIPLOMATIC_ASSAULTS)
			{
				itemproperty = new diplomat(item, this);
			}
			else if (item.type != itemType.DIALOGUE_LINE)
			{
				itemproperty = new itemproperties(item, this);
			}
			else
			{
				GameData.Item parentDialogueFromLine = this.getParentDialogueFromLine(item);
				if (parentDialogueFromLine != null)
				{
					if (navigation.forms.ContainsKey(parentDialogueFromLine))
					{
						navigation.forms[parentDialogueFromLine].BringToFront();
						if (navigation.forms[parentDialogueFromLine] is conversation)
						{
							(navigation.forms[parentDialogueFromLine] as conversation).SelectLine(item);
						}
						return;
					}
					if (item.type != itemType.WORD_SWAPS)
					{
						itemproperty = new conversation(parentDialogueFromLine, this);
					}
					else
					{
						itemproperty = new wordswaps(parentDialogueFromLine, this);
					}
					(itemproperty as conversation).SelectLine(item);
					item = parentDialogueFromLine;
				}
				else
				{
					MessageBox.Show("Dialogue line is not associated with any dialogue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					itemproperty = new itemproperties(item, this);
				}
			}
			navigation.forms.Add(item, itemproperty);
			itemproperty.FormClosed += new FormClosedEventHandler(this.form_FormClosed);
			itemproperty.MdiParent = base.MdiParent;
			itemproperty.Show();
			System.Windows.Forms.Cursor.Current = Cursors.Default;
		}

		private void Todo_Click(object sender, EventArgs e)
		{
			List<GameData.Item>.Enumerator enumerator = this.listView1.SelectedItems.GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					GameData.Item current = enumerator.Current;
					(base.MdiParent as baseForm).todoList.AddItem(current, "");
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.FileMode == navigation.ModFileMode.SINGLE)
			{
				this.treeView.SelectedNode = null;
			}
			this.refreshListView();
		}

		public class Filter
		{
			public string Custom
			{
				get;
				set;
			}

			public itemType Type
			{
				get;
				set;
			}

			public Filter(itemType t, string f = "")
			{
				this.Type = t;
				this.Custom = f;
			}
		}

		public enum ModFileMode
		{
			SINGLE,
			BASE,
			USER
		}
	}
}