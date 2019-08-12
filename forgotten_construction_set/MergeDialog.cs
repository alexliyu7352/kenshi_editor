using forgotten_construction_set.Components;
using forgotten_construction_set.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class MergeDialog : Form
	{
		private GameData gameData;

		private GameData tempData;

		private string currentMod;

		private IContainer components;

		private ComboBox modBox;

		private Label label1;

		private Button mergeButton;

		private Button cancelButton;

		private ChangeListTree tree;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ToolStripMenuItem selectAll;

		private ToolStripMenuItem selectNone;

		private ToolStripMenuItem selectRecursive;

		private ToolStripMenuItem selectNoneRecursive;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem fixReferences;

		private Button anyFile;

		private ToolTip toolTip;

		private CheckBox leveldata;

		private static navigation nav
		{
			get
			{
				navigation _navigation;
				IEnumerator enumerator = Application.OpenForms.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Form current = (Form)enumerator.Current;
						if (!(current is baseForm))
						{
							continue;
						}
						_navigation = (current as baseForm).nav;
						return _navigation;
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
				return _navigation;
			}
		}

		public MergeDialog(GameData gameData, navigation nav)
		{
			int i;
			this.gameData = gameData;
			this.InitializeComponent();
			base.ShowIcon = true;
			base.Icon = System.Drawing.Icon.FromHandle(Resources.Merge_24x.GetHicon());
			this.tree.ItemChecked += new ChangeListTree.ItemCheckedEventHandler(this.Tree_ItemChecked);
			if (nav.FileMode != navigation.ModFileMode.SINGLE && !TranslationManager.TranslationMode)
			{
				if (nav.BasePath != null)
				{
					FileInfo[] files = (new DirectoryInfo(nav.BasePath)).GetFiles("*.mod");
					for (i = 0; i < (int)files.Length; i++)
					{
						FileInfo fileInfo = files[i];
						this.modBox.Items.Add(new MergeDialog.ModListItem(fileInfo.Name, fileInfo.FullName));
					}
				}
				if (nav.RootPath != null)
				{
					DirectoryInfo[] directories = (new DirectoryInfo(string.Concat(nav.RootPath, "\\mods"))).GetDirectories();
					for (i = 0; i < (int)directories.Length; i++)
					{
						DirectoryInfo directoryInfo = directories[i];
						FileInfo fileInfo1 = new FileInfo(string.Concat(directoryInfo.FullName, "\\", directoryInfo.Name, ".mod"));
						if (fileInfo1.Exists)
						{
							this.modBox.Items.Add(new MergeDialog.ModListItem(fileInfo1.Name, fileInfo1.FullName));
						}
					}
				}
			}
			this.tempData = new GameData();
			this.Cursor = Cursors.WaitCursor;
			foreach (GameData.Item value in gameData.items.Values)
			{
				if (value.getState() == GameData.State.LOCKED)
				{
					continue;
				}
				this.tempData.items.Add(value.stringID, value.createFlatCopy());
			}
			foreach (GameData.Item item in this.tempData.items.Values)
			{
				item.resolveReferences(this.tempData);
			}
			this.Cursor = Cursors.Default;
			this.tree.GameData = this.tempData;
			this.tree.CheckBoxes = true;
			this.mergeButton.Enabled = false;
			this.leveldata.Visible = nav.FileMode != navigation.ModFileMode.SINGLE;
			this.leveldata.Enabled = false;
		}

		private void anyFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = "Load changes from another file",
				InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
				FileName = ""
			};
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.modBox.SelectedIndex = -1;
				this.loadChanges(openFileDialog.FileName);
				this.leveldata.Enabled = false;
				this.leveldata.Checked = false;
			}
		}

		private void checkItem(ChangeListTree.TreeNode n, bool check)
		{
			n.Checked = check;
			if (n.Children == null)
			{
				return;
			}
			foreach (ChangeListTree.TreeNode child in n.Children)
			{
				child.Checked = check;
				if (child.Children == null)
				{
					continue;
				}
				foreach (ChangeListTree.TreeNode treeNode in child.Children)
				{
					treeNode.Checked = check;
				}
			}
		}

		private bool checkItem(GameData.Item item, bool check)
		{
			bool flag;
			List<ChangeListTree.RootNode>.Enumerator enumerator = this.tree.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ChangeListTree.TreeNode current = enumerator.Current;
					if (this.tree.GetItem(current) != item)
					{
						continue;
					}
					this.checkItem(current, check);
					flag = true;
					return flag;
				}
				return false;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return flag;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private int findMissingReferences(GameData.Item item, HashSet<string> added)
		{
			int num = 0;
			foreach (string str in item.referenceLists())
			{
				foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in item.referenceData(str, true))
				{
					if (this.gameData.getItem(keyValuePair.Key) != null || this.tempData.getItem(keyValuePair.Key) == null)
					{
						continue;
					}
					added.Add(keyValuePair.Key);
					num++;
				}
			}
			foreach (KeyValuePair<string, GameData.Instance> keyValuePair1 in item.instanceData())
			{
				string str1 = keyValuePair1.Value.sdata["ref"];
				if (this.gameData.getItem(str1) != null || this.tempData.getItem(str1) == null)
				{
					continue;
				}
				added.Add(str1);
				num++;
			}
			return num;
		}

		private void fixReferences_Click(object sender, EventArgs e)
		{
			int num = 0;
			HashSet<string> strs = new HashSet<string>();
			foreach (GameData.Item value in this.gameData.items.Values)
			{
				num += this.findMissingReferences(value, strs);
			}
			int count = 0;
			while (strs.Count != count)
			{
				HashSet<string> strs1 = new HashSet<string>();
				foreach (string str in strs)
				{
					GameData.Item item = this.tempData.getItem(str);
					this.findMissingReferences(item, strs1);
				}
				count = strs.Count;
				foreach (string str1 in strs1)
				{
					strs.Add(str1);
				}
			}
			foreach (string str2 in strs)
			{
				GameData.Item item1 = this.tempData.getItem(str2);
				this.checkItem(item1, true);
			}
			MessageBox.Show(string.Concat("Selected ", strs.Count, " missing references"), "Merge");
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.modBox = new ComboBox();
			this.label1 = new Label();
			this.mergeButton = new Button();
			this.cancelButton = new Button();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.selectAll = new ToolStripMenuItem();
			this.selectNone = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.selectRecursive = new ToolStripMenuItem();
			this.selectNoneRecursive = new ToolStripMenuItem();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.fixReferences = new ToolStripMenuItem();
			this.tree = new ChangeListTree(this.components);
			this.anyFile = new Button();
			this.toolTip = new ToolTip(this.components);
			this.leveldata = new CheckBox();
			this.contextMenu.SuspendLayout();
			base.SuspendLayout();
			this.modBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.modBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.modBox.FormattingEnabled = true;
			this.modBox.Location = new Point(122, 2);
			this.modBox.Name = "modBox";
			this.modBox.Size = new System.Drawing.Size(261, 21);
			this.modBox.TabIndex = 1;
			this.toolTip.SetToolTip(this.modBox, "Select mod to load changes from");
			this.modBox.SelectedIndexChanged += new EventHandler(this.modBox_SelectedIndexChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(0, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Add changes from mod";
			this.mergeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.mergeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.mergeButton.Location = new Point(323, 236);
			this.mergeButton.Name = "mergeButton";
			this.mergeButton.Size = new System.Drawing.Size(75, 23);
			this.mergeButton.TabIndex = 4;
			this.mergeButton.Text = "Merge";
			this.mergeButton.UseVisualStyleBackColor = true;
			this.mergeButton.Click += new EventHandler(this.mergeButton_Click);
			this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new Point(242, 236);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.selectAll, this.selectNone, this.toolStripSeparator1, this.selectRecursive, this.selectNoneRecursive, this.toolStripSeparator2, this.fixReferences });
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(172, 126);
			this.selectAll.Name = "selectAll";
			this.selectAll.Size = new System.Drawing.Size(171, 22);
			this.selectAll.Text = "Select &All";
			this.selectAll.Click += new EventHandler(this.selectAll_Click);
			this.selectNone.Name = "selectNone";
			this.selectNone.Size = new System.Drawing.Size(171, 22);
			this.selectNone.Text = "&Deselect All";
			this.selectNone.Click += new EventHandler(this.selectNone_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(168, 6);
			this.selectRecursive.Name = "selectRecursive";
			this.selectRecursive.Size = new System.Drawing.Size(171, 22);
			this.selectRecursive.Text = "Select &Recursive";
			this.selectRecursive.Click += new EventHandler(this.selectRecursive_Click);
			this.selectNoneRecursive.Name = "selectNoneRecursive";
			this.selectNoneRecursive.Size = new System.Drawing.Size(171, 22);
			this.selectNoneRecursive.Text = "Deselect R&ecursive";
			this.selectNoneRecursive.Click += new EventHandler(this.selectNoneRecursive_Click);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(168, 6);
			this.fixReferences.Name = "fixReferences";
			this.fixReferences.Size = new System.Drawing.Size(171, 22);
			this.fixReferences.Text = "F&ix References";
			this.fixReferences.Click += new EventHandler(this.fixReferences_Click);
			this.tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.tree.ContextMenuStrip = this.contextMenu;
			this.tree.Filter = null;
			this.tree.FullRowSelect = true;
			this.tree.GameData = null;
			this.tree.Location = new Point(3, 29);
			this.tree.Name = "tree";
			this.tree.Nodes = null;
			this.tree.OwnerDraw = true;
			this.tree.ShowChanged = true;
			this.tree.ShowDeleted = true;
			this.tree.ShowNew = true;
			this.tree.Size = new System.Drawing.Size(406, 201);
			this.tree.SortKey = null;
			this.tree.TabIndex = 0;
			this.tree.UseCompatibleStateImageBehavior = false;
			this.tree.View = View.Details;
			this.tree.VirtualMode = true;
			this.anyFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.anyFile.Location = new Point(383, 1);
			this.anyFile.Name = "anyFile";
			this.anyFile.Size = new System.Drawing.Size(26, 23);
			this.anyFile.TabIndex = 6;
			this.anyFile.Text = "*";
			this.toolTip.SetToolTip(this.anyFile, "Select a file to load changes from");
			this.anyFile.UseVisualStyleBackColor = true;
			this.anyFile.Click += new EventHandler(this.anyFile_Click);
			this.leveldata.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.leveldata.AutoSize = true;
			this.leveldata.Location = new Point(12, 236);
			this.leveldata.Name = "leveldata";
			this.leveldata.Size = new System.Drawing.Size(111, 17);
			this.leveldata.TabIndex = 7;
			this.leveldata.Text = "Merge Level Data";
			this.leveldata.UseVisualStyleBackColor = true;
			this.leveldata.CheckedChanged += new EventHandler(this.leveldata_CheckedChanged);
			base.AcceptButton = this.mergeButton;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new System.Drawing.Size(410, 266);
			base.Controls.Add(this.leveldata);
			base.Controls.Add(this.anyFile);
			base.Controls.Add(this.cancelButton);
			base.Controls.Add(this.mergeButton);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.modBox);
			base.Controls.Add(this.tree);
			base.Name = "MergeDialog";
			base.ShowIcon = false;
			this.Text = "Merge Mod";
			this.contextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void leveldata_CheckedChanged(object sender, EventArgs e)
		{
			if (this.leveldata.Checked)
			{
				this.mergeButton.Enabled = true;
				return;
			}
			this.mergeButton.Enabled = this.tree.Items.Count > 0;
		}

		private void loadChanges(string file)
		{
			this.Cursor = Cursors.WaitCursor;
			List<GameData.Item> items = new List<GameData.Item>();
			foreach (GameData.Item value in this.tempData.items.Values)
			{
				if (value.getState() != GameData.State.OWNED)
				{
					value.revert();
				}
				else
				{
					items.Add(value);
				}
			}
			foreach (GameData.Item item in items)
			{
				this.tempData.deleteItem(item);
			}
			this.tempData.load(file, GameData.ModMode.ACTIVE, true);
			this.tempData.resolveAllReferences();
			this.tree.RefreshList();
			foreach (ChangeListTree.TreeNode node in this.tree.Nodes)
			{
				this.checkItem(node, true);
				if (node.Children == null)
				{
					continue;
				}
				foreach (ChangeListTree.TreeNode child in node.Children)
				{
					this.checkItem(child, true);
				}
			}
			this.Cursor = Cursors.Default;
			this.mergeButton.Enabled = this.tree.Nodes.Count > 0;
			this.currentMod = file;
			this.tree.Select();
		}

		private void mergeButton_Click(object sender, EventArgs e)
		{
			string str;
			GameData.TripleInt referenceValue;
			int num = 0;
			this.Cursor = Cursors.WaitCursor;
			foreach (ChangeListTree.TreeNode node in this.tree.Nodes)
			{
				if (!node.Checked)
				{
					continue;
				}
				GameData.Item item = this.tree.GetItem(node);
				GameData.Item newValue = this.gameData.getItem(item.stringID);
				if (item.getState() == GameData.State.OWNED || newValue == null)
				{
					this.gameData.items.Add(item.stringID, item);
					item.resolveReferences(this.gameData);
					item.changeOwner(this.gameData.activeFileName);
					num++;
				}
				else if (item.getState() != GameData.State.REMOVED)
				{
					foreach (ChangeListTree.TreeNode child in node.Children)
					{
						if (!child.Checked)
						{
							continue;
						}
						ChangeListTree.ChangeData changeDatum = child as ChangeListTree.ChangeData;
						switch (changeDatum.Type)
						{
							case ChangeListTree.ChangeType.NAME:
							{
								newValue.Name = changeDatum.NewValue.ToString();
								break;
							}
							case ChangeListTree.ChangeType.VALUE:
							{
								newValue[changeDatum.Key] = changeDatum.NewValue;
								break;
							}
							case ChangeListTree.ChangeType.NEWREF:
							case ChangeListTree.ChangeType.INVALIDREF:
							{
								referenceValue = item.getReferenceValue(changeDatum.Section, changeDatum.Key);
								newValue.addReference(changeDatum.Section, changeDatum.Key, new int?(referenceValue.v0), new int?(referenceValue.v1), new int?(referenceValue.v2));
								break;
							}
							case ChangeListTree.ChangeType.MODREF:
							{
								referenceValue = item.getReferenceValue(changeDatum.Section, changeDatum.Key);
								newValue.setReferenceValue(changeDatum.Section, changeDatum.Key, referenceValue);
								break;
							}
							case ChangeListTree.ChangeType.DELREF:
							{
								newValue.removeReference(changeDatum.Section, changeDatum.Key);
								break;
							}
							case ChangeListTree.ChangeType.NEWINST:
							{
								GameData.Instance instance = item.getInstance(changeDatum.Key);
								GameData.Instance instance1 = newValue.addInstance(changeDatum.Key, instance);
								using (IEnumerator<KeyValuePair<string, GameData.TripleInt>> enumerator = instance.referenceData("states", false).GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										KeyValuePair<string, GameData.TripleInt> current = enumerator.Current;
										int? nullable = null;
										int? nullable1 = nullable;
										nullable = null;
										int? nullable2 = nullable;
										nullable = null;
										instance1.addReference("states", current.Key, nullable1, nullable2, nullable);
									}
									break;
								}
								break;
							}
							case ChangeListTree.ChangeType.MODINST:
							{
								if (item.getInstance(changeDatum.Key).getState() != GameData.State.REMOVED)
								{
									foreach (ChangeListTree.TreeNode treeNode in child.Children)
									{
										if (!treeNode.Checked)
										{
											continue;
										}
										ChangeListTree.ChangeData changeDatum1 = treeNode as ChangeListTree.ChangeData;
										if (changeDatum1.Type != ChangeListTree.ChangeType.INSTVALUE)
										{
											continue;
										}
										newValue.getInstance(changeDatum1.Section)[changeDatum1.Key] = changeDatum1.NewValue;
										num++;
									}
									num--;
									break;
								}
								else
								{
									newValue.removeInstance(changeDatum.Key);
									break;
								}
							}
							case ChangeListTree.ChangeType.INSTVALUE:
							{
								newValue.getInstance(changeDatum.Section)[changeDatum.Key] = changeDatum.NewValue;
								break;
							}
						}
						num++;
					}
				}
				else
				{
					if (newValue == null)
					{
						continue;
					}
					this.gameData.deleteItem(newValue);
					num++;
				}
			}
			foreach (ChangeListTree.TreeNode node1 in this.tree.Nodes)
			{
				if (!node1.Checked)
				{
					continue;
				}
				GameData.Item item1 = this.gameData.getItem(this.tree.GetItem(node1).stringID);
				if (item1 == null)
				{
					continue;
				}
				item1.resolveReferences(this.gameData);
			}
			if (num != 0)
			{
				str = (num != 1 ? string.Concat(num, " changes merged") : "1 change merged");
			}
			else
			{
				str = "No changes detected";
			}
			if (this.leveldata.Checked)
			{
				string str1 = string.Concat(Directory.GetParent(this.currentMod), "/leveldata");
				string str2 = string.Concat(MergeDialog.nav.ModPath, "leveldata");
				if (MergeDialog.nav.ModPath == MergeDialog.nav.BasePath)
				{
					str2 = string.Concat(MergeDialog.nav.ModPath, "/newland/leveldata/");
					if (!MergeDialog.nav.ActiveFile.EndsWith("gamedata.base"))
					{
						FileInfo fileInfo = new FileInfo(MergeDialog.nav.ActiveFile);
						str2 = string.Concat(str2, fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length));
					}
				}
				string str3 = MergeDialog.mergeLevelData(str1, str2);
				if (str3.Length > 0)
				{
					str = string.Concat(str, "\n\n", str3);
				}
			}
			this.Cursor = Cursors.Default;
			MessageBox.Show(str, "Merge", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}

		public static string mergeLevelData(string srcPath, string destPath)
		{
			string str = "";
			navigation.ModFileMode fileMode = MergeDialog.nav.FileMode;
			MergeDialog.nav.FileMode = navigation.ModFileMode.SINGLE;
			Directory.CreateDirectory(destPath);
			string str1 = string.Concat(srcPath, "/leveldata.level");
			string str2 = string.Concat(destPath, "/leveldata.level");
			if (!File.Exists(str2))
			{
				File.Copy(str1, str2, false);
				str = string.Concat(str, "Copied leveldata\n");
			}
			else
			{
				GameData gameDatum = new GameData();
				gameDatum.load(str2, GameData.ModMode.ACTIVE, false);
				gameDatum.load(str1, GameData.ModMode.ACTIVE, false);
				gameDatum.save(str2);
				str = string.Concat(str, "Merged leveldata\n");
			}
			int num = 0;
			int num1 = 0;
			FileInfo[] files = (new DirectoryInfo(srcPath)).GetFiles("*.zone");
			for (int i = 0; i < (int)files.Length; i++)
			{
				FileInfo fileInfo = files[i];
				string str3 = string.Concat(destPath, "/", fileInfo.Name);
				if (!File.Exists(str3))
				{
					fileInfo.CopyTo(str3);
					num1++;
				}
				else
				{
					GameData gameDatum1 = new GameData();
					gameDatum1.load(str3, GameData.ModMode.ACTIVE, false);
					gameDatum1.load(fileInfo.FullName, GameData.ModMode.ACTIVE, false);
					gameDatum1.save(str3);
					num++;
				}
			}
			str = string.Concat(new object[] { str, "Merged ", num, " Zone files\n" });
			str = string.Concat(new object[] { str, "Copied ", num1, " Zone files\n" });
			string str4 = string.Concat(srcPath, "/interiors.level");
			if (File.Exists(str4))
			{
				string str5 = string.Concat(destPath, "/interiors.level");
				if (!File.Exists(str5))
				{
					File.Copy(str4, str5, false);
					str = string.Concat(str, "Copied interiors\n");
				}
				else
				{
					GameData gameDatum2 = new GameData();
					gameDatum2.load(str5, GameData.ModMode.ACTIVE, false);
					gameDatum2.load(str4, GameData.ModMode.ACTIVE, false);
					gameDatum2.save(str5);
					str = string.Concat(str, "Merged interiors\n");
				}
			}
			MergeDialog.nav.FileMode = fileMode;
			return str;
		}

		private void modBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			MergeDialog.ModListItem selectedItem = this.modBox.SelectedItem as MergeDialog.ModListItem;
			if (selectedItem != null)
			{
				this.loadChanges(selectedItem.path);
				this.leveldata.Enabled = File.Exists(string.Concat(Directory.GetParent(selectedItem.path).FullName, "/leveldata/leveldata.level"));
				this.leveldata.Checked = false;
			}
		}

		private void selectAll_Click(object sender, EventArgs e)
		{
			if (this.tree.SelectedIndices.Count <= 1)
			{
				foreach (ChangeListTree.TreeNode node in this.tree.Nodes)
				{
					this.checkItem(node, true);
				}
			}
			else
			{
				foreach (ChangeListTree.TreeNode selectedItem in this.tree.SelectedItems)
				{
					this.checkItem(selectedItem, true);
				}
			}
			this.tree.Invalidate();
		}

		private void selectNone_Click(object sender, EventArgs e)
		{
			if (this.tree.SelectedIndices.Count <= 1)
			{
				foreach (ChangeListTree.TreeNode node in this.tree.Nodes)
				{
					this.checkItem(node, false);
				}
			}
			else
			{
				foreach (ChangeListTree.TreeNode selectedItem in this.tree.SelectedItems)
				{
					this.checkItem(selectedItem, false);
				}
			}
			this.tree.Invalidate();
		}

		private void selectNoneRecursive_Click(object sender, EventArgs e)
		{
			foreach (ChangeListTree.TreeNode selectedItem in this.tree.SelectedItems)
			{
				HashSet<GameData.Item> items = new HashSet<GameData.Item>();
				this.selectRecursiveData(this.tree.GetItem(selectedItem), false, items);
				this.checkItem(selectedItem, false);
			}
			this.tree.Invalidate();
		}

		private void selectRecursive_Click(object sender, EventArgs e)
		{
			foreach (ChangeListTree.TreeNode selectedItem in this.tree.SelectedItems)
			{
				HashSet<GameData.Item> items = new HashSet<GameData.Item>();
				this.selectRecursiveData(this.tree.GetItem(selectedItem), true, items);
				this.checkItem(selectedItem, true);
			}
			this.tree.Invalidate();
		}

		private void selectRecursiveData(GameData.Item item, bool check, HashSet<GameData.Item> traversed)
		{
			if (traversed.Contains(item))
			{
				return;
			}
			traversed.Add(item);
			foreach (string str in item.referenceLists())
			{
				foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in item.referenceData(str, true))
				{
					GameData.Item item1 = this.tempData.getItem(keyValuePair.Key);
					if (item1 == null || item1.getState() == GameData.State.ORIGINAL)
					{
						continue;
					}
					this.checkItem(item1, check);
					this.selectRecursiveData(item1, check, traversed);
				}
			}
			foreach (KeyValuePair<string, GameData.Instance> keyValuePair1 in item.instanceData())
			{
				string str1 = keyValuePair1.Value.sdata["ref"];
				GameData.Item item2 = this.tempData.getItem(str1);
				if (item2 == null || item2.getState() == GameData.State.ORIGINAL)
				{
					continue;
				}
				this.checkItem(item2, check);
				this.selectRecursiveData(item2, check, traversed);
			}
		}

		private void Tree_ItemChecked(object sender, ChangeListTree.ItemCheckedEventArgs e)
		{
			if (e.Node.Children != null)
			{
				foreach (ChangeListTree.TreeNode child in e.Node.Children)
				{
					child.Checked = e.Node.Checked;
					if (child.Children == null)
					{
						continue;
					}
					foreach (ChangeListTree.TreeNode @checked in child.Children)
					{
						@checked.Checked = e.Node.Checked;
					}
				}
			}
		}

		private class ModListItem
		{
			public string name;

			public string path;

			public ModListItem(string name, string path)
			{
				this.name = name;
				this.path = path;
			}

			public override string ToString()
			{
				return this.name;
			}
		}
	}
}