using forgotten_construction_set.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace forgotten_construction_set
{
	public class InheritFiles : Form
	{
		public baseForm parentForm;

		private TreeNode activeNode;

		private string baseDir;

		private string modDir;

		private static string[] FixedModsList;

		public static HashSet<string> FixedMods;

		private HashSet<string> defaultChecked = new HashSet<string>();

		private IContainer components;

		private Button moveUp;

		private Button moveDown;

		private Button newMod;

		private Button cancelButton;

		private TreeView modList;

		private TreeView baseList;

		private SplitContainer splitContainer1;

		private Button loadButton;

		private Button btnNewTranslation;

		private ComboBox cbActiveTranslation;

		private CheckBox cbTranslationMode;

		private ToolTip toolTipInfo;

		private GroupBox groupBox1;

		private Button selectAll;

		static InheritFiles()
		{
			InheritFiles.FixedModsList = new string[] { "gamedata.base", "Newwworld.mod", "Dialogue.mod", "Vitali.mod", "Nizu.mod", "rebirth.mod" };
			InheritFiles.FixedMods = new HashSet<string>(InheritFiles.FixedModsList);
		}

		public InheritFiles(baseForm _nav, string baseDir, string modDir)
		{
			int i;
			this.parentForm = _nav;
			this.InitializeComponent();
			this.baseDir = baseDir;
			this.modDir = modDir;
			string[] strArrays = new string[0];
			try
			{
				strArrays = File.ReadAllLines(string.Concat(baseDir, "mods.cfg"));
			}
			catch (Exception exception)
			{
			}
			string[] strArrays1 = new string[(int)InheritFiles.FixedModsList.Length + (int)strArrays.Length];
			InheritFiles.FixedMods.CopyTo(strArrays1, 0);
			strArrays.CopyTo(strArrays1, (int)InheritFiles.FixedModsList.Length);
			List<string> strs = new List<string>();
			DirectoryInfo directoryInfo = new DirectoryInfo(baseDir);
			FileInfo[] files = directoryInfo.GetFiles("*.base");
			for (i = 0; i < (int)files.Length; i++)
			{
				strs.Add(files[i].Name);
			}
			files = directoryInfo.GetFiles("*.mod");
			for (i = 0; i < (int)files.Length; i++)
			{
				strs.Add(files[i].Name);
			}
			List<string> strs1 = new List<string>();
			directoryInfo = new DirectoryInfo(modDir);
			if (directoryInfo.Exists)
			{
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				List<string> strs2 = new List<string>((int)directories.Length);
				DirectoryInfo[] directoryInfoArray = directories;
				for (i = 0; i < (int)directoryInfoArray.Length; i++)
				{
					DirectoryInfo directoryInfo1 = directoryInfoArray[i];
					FileInfo[] fileInfoArray = directoryInfo1.GetFiles(string.Concat(directoryInfo1.Name, ".mod"));
					if (fileInfoArray.Length != 0)
					{
						strs1.Add(fileInfoArray[0].Name);
					}
				}
			}
			string[] strArrays2 = strArrays1;
			for (i = 0; i < (int)strArrays2.Length; i++)
			{
				string str = strArrays2[i];
				if (strs.Contains(str))
				{
					this.baseList.Nodes.Add(str).Checked = true;
					strs.Remove(str);
				}
				else if (strs1.Contains(str))
				{
					this.modList.Nodes.Add(str).Checked = true;
					strs1.Remove(str);
					this.defaultChecked.Add(str);
				}
			}
			foreach (string str1 in strs)
			{
				this.baseList.Nodes.Add(str1);
			}
			foreach (string str2 in strs1)
			{
				this.modList.Nodes.Add(str2);
			}
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			foreach (TreeNode node in this.baseList.Nodes)
			{
				GameData.Header header = GameData.loadHeader(string.Concat(baseDir, node.Text));
				node.ToolTipText = this.createToolTip(node.Text, header);
				node.Tag = header;
			}
			foreach (TreeNode treeNode in this.modList.Nodes)
			{
				string str3 = treeNode.Text.Substring(0, treeNode.Text.Length - 4);
				GameData.Header header1 = GameData.loadHeader(string.Concat(modDir, str3, "/", treeNode.Text));
				treeNode.ToolTipText = this.createToolTip(treeNode.Text, header1);
				treeNode.Tag = header1;
			}
			System.Windows.Forms.Cursor.Current = Cursors.Default;
			this.updateActiveMod();
			this.baseList.DrawMode = TreeViewDrawMode.OwnerDrawText;
			this.baseList.DrawNode += new DrawTreeNodeEventHandler(this.modList_DrawNode);
			this.baseList.MouseDown += new MouseEventHandler(this.modList_MouseDown);
			this.baseList.MouseClick += new MouseEventHandler(this.modList_MouseClick);
			this.baseList.MouseDoubleClick += new MouseEventHandler(this.modList_MouseClick);
			this.baseList.ItemDrag += new ItemDragEventHandler(this.modList_ItemDrag);
			this.baseList.DragDrop += new DragEventHandler(this.modList_DragDrop);
			this.baseList.DragOver += new DragEventHandler(this.modList_DragOver);
			this.modList.DrawMode = TreeViewDrawMode.OwnerDrawText;
			this.modList.DrawNode += new DrawTreeNodeEventHandler(this.modList_DrawNode);
			this.modList.MouseDown += new MouseEventHandler(this.modList_MouseDown);
			this.modList.MouseClick += new MouseEventHandler(this.modList_MouseClick);
			this.modList.MouseDoubleClick += new MouseEventHandler(this.modList_MouseClick);
			this.modList.ItemDrag += new ItemDragEventHandler(this.modList_ItemDrag);
			this.modList.DragDrop += new DragEventHandler(this.modList_DragDrop);
			this.modList.DragOver += new DragEventHandler(this.modList_DragOver);
			this.btnNewTranslation.Enabled = false;
			this.cbActiveTranslation.Enabled = false;
		}

		private void btnNewTranslation_Click(object sender, EventArgs e)
		{
			if ((new NewTranslationModDialog()).ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.updateTranslationsList(false);
			}
		}

		private void cbActiveTranslation_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.updateActiveMod();
			Button enabled = this.loadButton;
			enabled.Enabled = enabled.Enabled & this.cbActiveTranslation.SelectedItem != null;
		}

		private void cbTranslationMode_CheckedChanged(object sender, EventArgs e)
		{
			this.updateActiveMod();
			if (this.cbTranslationMode.Checked)
			{
				this.updateTranslationsList(true);
				Button enabled = this.loadButton;
				enabled.Enabled = enabled.Enabled & this.cbActiveTranslation.SelectedItem != null;
			}
			this.btnNewTranslation.Enabled = this.cbTranslationMode.Checked;
			this.cbActiveTranslation.Enabled = this.cbTranslationMode.Checked;
		}

		private InheritFiles.DependencyState checkDependencies(TreeNode n)
		{
			InheritFiles.DependencyState dependencyState;
			if (n.Tag != null)
			{
				GameData.Header tag = n.Tag as GameData.Header;
				foreach (string dependency in tag.Dependencies)
				{
					TreeNode treeNode = this.findNode(dependency);
					if (treeNode != null && treeNode.Checked)
					{
						continue;
					}
					dependencyState = InheritFiles.DependencyState.MISSING_DEPENDENCIES;
					return dependencyState;
				}
				List<string>.Enumerator enumerator = tag.Referenced.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						TreeNode treeNode1 = this.findNode(enumerator.Current);
						if (treeNode1 != null && treeNode1.Checked)
						{
							continue;
						}
						dependencyState = InheritFiles.DependencyState.MISSING_REFERENCES;
						return dependencyState;
					}
					return InheritFiles.DependencyState.OK;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				return dependencyState;
			}
			return InheritFiles.DependencyState.OK;
		}

		private string createToolTip(string mod, GameData.Header h)
		{
			if (h == null)
			{
				return mod;
			}
			string str = string.Concat(mod, "\n版本: ", h.Version.ToString());
			if (h.Author != "")
			{
				str = string.Concat(str, "\n作者: ", h.Author);
			}
			if (h.Description != "")
			{
				str = string.Concat(str, "\n", h.Description);
			}
			if (h.Dependencies.Count > 0)
			{
				str = string.Concat(str, "\n依赖:");
				foreach (string dependency in h.Dependencies)
				{
					str = string.Concat(str, "\n\t", dependency);
				}
			}
			if (h.Referenced.Count > 0)
			{
				str = string.Concat(str, "\n引用:");
				foreach (string referenced in h.Referenced)
				{
					str = string.Concat(str, "\n\t", referenced);
				}
			}
			return str;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void enforceDependencies(TreeNode n, bool check, bool reorder, List<TreeNode> cycle = null)
		{
			if (n.Tag != null)
			{
				if (cycle == null)
				{
					cycle = new List<TreeNode>();
				}
				if (cycle.Contains(n))
				{
					return;
				}
				cycle.Add(n);
				GameData.Header tag = n.Tag as GameData.Header;
				foreach (string dependency in tag.Dependencies)
				{
					TreeNode treeNode = this.findNode(dependency);
					if (check && treeNode != null && !treeNode.Checked)
					{
						treeNode.Checked = true;
						this.enforceDependencies(treeNode, check, reorder, cycle);
					}
					if (!reorder || treeNode == null || !this.moveAbove(treeNode, n))
					{
						continue;
					}
					this.enforceDependencies(treeNode, check, reorder, cycle);
				}
				foreach (string referenced in tag.Referenced)
				{
					TreeNode treeNode1 = this.findNode(referenced);
					if (!check || treeNode1 == null || treeNode1.Checked)
					{
						continue;
					}
					treeNode1.Checked = true;
					this.enforceDependencies(treeNode1, check, reorder, cycle);
				}
			}
			if (reorder)
			{
				foreach (TreeNode node in this.modList.Nodes)
				{
					if (node.TreeView != n.TreeView || node.Index <= n.Index)
					{
						if (node.Tag == null || !(node.Tag as GameData.Header).Dependencies.Contains(n.Text) || !this.moveBelow(node, n))
						{
							continue;
						}
						this.enforceDependencies(node, check, reorder, cycle);
					}
					else
					{
						return;
					}
				}
			}
		}

		private TreeNode findNode(string mod)
		{
			TreeNode treeNode;
			foreach (TreeNode node in this.baseList.Nodes)
			{
				if (node.Text != mod)
				{
					continue;
				}
				treeNode = node;
				return treeNode;
			}
			IEnumerator enumerator = this.modList.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					TreeNode current = (TreeNode)enumerator.Current;
					if (current.Text != mod)
					{
						continue;
					}
					treeNode = current;
					return treeNode;
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
			return treeNode;
		}

		private void InheritFiles_Shown(object sender, EventArgs e)
		{
			this.modList.Focus();
			this.modList.SelectedNode = null;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.cancelButton = new Button();
			this.modList = new TreeView();
			this.baseList = new TreeView();
			this.splitContainer1 = new SplitContainer();
			this.selectAll = new Button();
			this.moveUp = new Button();
			this.moveDown = new Button();
			this.newMod = new Button();
			this.loadButton = new Button();
			this.cbActiveTranslation = new ComboBox();
			this.cbTranslationMode = new CheckBox();
			this.btnNewTranslation = new Button();
			this.toolTipInfo = new ToolTip(this.components);
			this.groupBox1 = new GroupBox();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new Point(237, 224);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(69, 25);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.TabStop = false;
			this.cancelButton.Text = "取消";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.modList.AllowDrop = true;
			this.modList.Dock = DockStyle.Left;
			this.modList.Location = new Point(0, 0);
			this.modList.Name = "modList";
			this.modList.ShowNodeToolTips = true;
			this.modList.ShowRootLines = false;
			this.modList.Size = new System.Drawing.Size(464, 294);
			this.modList.TabIndex = 0;
			this.modList.PreviewKeyDown += new PreviewKeyDownEventHandler(this.modList_PreviewKeyDown);
			this.baseList.AllowDrop = true;
			this.baseList.Dock = DockStyle.Fill;
			this.baseList.Location = new Point(0, 0);
			this.baseList.Name = "baseList";
			this.baseList.ShowNodeToolTips = true;
			this.baseList.ShowRootLines = false;
			this.baseList.Size = new System.Drawing.Size(493, 109);
			this.baseList.TabIndex = 5;
			this.baseList.PreviewKeyDown += new PreviewKeyDownEventHandler(this.modList_PreviewKeyDown);
			this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.splitContainer1.Location = new Point(12, 12);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.baseList);
			this.splitContainer1.Panel2.Controls.Add(this.selectAll);
			this.splitContainer1.Panel2.Controls.Add(this.modList);
			this.splitContainer1.Panel2.Controls.Add(this.moveUp);
			this.splitContainer1.Panel2.Controls.Add(this.moveDown);
			this.splitContainer1.Panel2.Controls.Add(this.newMod);
			this.splitContainer1.Size = new System.Drawing.Size(493, 407);
			this.splitContainer1.SplitterDistance = 109;
			this.splitContainer1.TabIndex = 7;
			this.splitContainer1.TabStop = false;
			this.selectAll.Image = Resources.ModSets_16x;
			this.selectAll.Location = new Point(470, 87);
			this.selectAll.Name = "selectAll";
			this.selectAll.Size = new System.Drawing.Size(23, 23);
			this.selectAll.TabIndex = 5;
			this.toolTipInfo.SetToolTip(this.selectAll, "选择/反选所有的MOD");
			this.selectAll.UseVisualStyleBackColor = true;
			this.selectAll.Click += new EventHandler(this.selectAll_Click);
			this.moveUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.moveUp.Image = Resources.Upload_16x;
			this.moveUp.Location = new Point(470, 29);
			this.moveUp.Name = "moveUp";
			this.moveUp.Size = new System.Drawing.Size(23, 23);
			this.moveUp.TabIndex = 3;
			this.toolTipInfo.SetToolTip(this.moveUp, "移动MOD到列表上方");
			this.moveUp.UseVisualStyleBackColor = true;
			this.moveUp.Click += new EventHandler(this.moveUp_Click);
			this.moveDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.moveDown.Image = Resources.Download_16x;
			this.moveDown.Location = new Point(470, 58);
			this.moveDown.Name = "moveDown";
			this.moveDown.Size = new System.Drawing.Size(23, 23);
			this.moveDown.TabIndex = 4;
			this.toolTipInfo.SetToolTip(this.moveDown, "移动MOD到列表下方");
			this.moveDown.UseVisualStyleBackColor = true;
			this.moveDown.Click += new EventHandler(this.moveDown_Click);
			this.newMod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.newMod.Image = Resources.NewItem_16x;
			this.newMod.Location = new Point(470, 0);
			this.newMod.Name = "newMod";
			this.newMod.Size = new System.Drawing.Size(23, 23);
			this.newMod.TabIndex = 1;
			this.toolTipInfo.SetToolTip(this.newMod, "创建新MOD文件");
			this.newMod.UseVisualStyleBackColor = true;
			this.newMod.Click += new EventHandler(this.newMod_Click);
			this.loadButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.loadButton.Location = new Point(390, 442);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(115, 52);
			this.loadButton.TabIndex = 0;
			this.loadButton.Text = "完成";
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new EventHandler(this.loadButton_Click);
			this.cbActiveTranslation.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.cbActiveTranslation.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbActiveTranslation.FormattingEnabled = true;
			this.cbActiveTranslation.Location = new Point(138, 42);
			this.cbActiveTranslation.Name = "cbActiveTranslation";
			this.cbActiveTranslation.Size = new System.Drawing.Size(206, 21);
			this.cbActiveTranslation.Sorted = true;
			this.cbActiveTranslation.TabIndex = 8;
			this.cbActiveTranslation.SelectedIndexChanged += new EventHandler(this.cbActiveTranslation_SelectedIndexChanged);
			this.cbTranslationMode.AutoSize = true;
			this.cbTranslationMode.Location = new Point(6, 19);
			this.cbTranslationMode.Name = "cbTranslationMode";
			this.cbTranslationMode.Size = new System.Drawing.Size(59, 17);
			this.cbTranslationMode.TabIndex = 9;
			this.cbTranslationMode.Text = "允许";
			this.toolTipInfo.SetToolTip(this.cbTranslationMode, "激活翻译模式。\r\n只使用此模式导出/导入翻译和翻译对话。\r\n大多数mods编辑特性将被禁用\r\n");
			this.cbTranslationMode.UseVisualStyleBackColor = true;
			this.cbTranslationMode.CheckedChanged += new EventHandler(this.cbTranslationMode_CheckedChanged);
			this.btnNewTranslation.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.btnNewTranslation.Image = Resources.NewTranslateDocument_16x;
			this.btnNewTranslation.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnNewTranslation.Location = new Point(6, 42);
			this.btnNewTranslation.Name = "btnNewTranslation";
			this.btnNewTranslation.Size = new System.Drawing.Size(109, 21);
			this.btnNewTranslation.TabIndex = 1;
			this.btnNewTranslation.Text = "新翻译";
			this.btnNewTranslation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTipInfo.SetToolTip(this.btnNewTranslation, "创建一个新的翻译目录");
			this.btnNewTranslation.UseVisualStyleBackColor = true;
			this.btnNewTranslation.Click += new EventHandler(this.btnNewTranslation_Click);
			this.groupBox1.Controls.Add(this.cbTranslationMode);
			this.groupBox1.Controls.Add(this.cbActiveTranslation);
			this.groupBox1.Controls.Add(this.btnNewTranslation);
			this.groupBox1.Location = new Point(12, 425);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(350, 69);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "翻译模式";
			base.AcceptButton = this.loadButton;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.cancelButton;
			base.ClientSize = new System.Drawing.Size(517, 506);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.loadButton);
			base.Controls.Add(this.splitContainer1);
			base.Controls.Add(this.cancelButton);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "InheritFiles";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "要继承的数据文件，列表中较低的文件将覆盖其他文件";
			base.Shown += new EventHandler(this.InheritFiles_Shown);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		private void loadButton_Click(object sender, EventArgs e)
		{
			string str;
			List<TreeNode> treeNodes = new List<TreeNode>();
			foreach (TreeNode node in this.baseList.Nodes)
			{
				if (!node.Checked)
				{
					continue;
				}
				treeNodes.Add(node);
			}
			foreach (TreeNode treeNode in this.modList.Nodes)
			{
				if (!treeNode.Checked)
				{
					continue;
				}
				treeNodes.Add(treeNode);
			}
			if (treeNodes[0].TreeView != this.baseList)
			{
				MessageBox.Show("您将要加载没有激活基本文件的mod。\n这可能会导致无效的引用。 被覆盖的项目将显示不正确，您可能会因加载产生大量的错误消息。", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			InheritFiles.DependencyState dependencyState = InheritFiles.DependencyState.OK;
			List<string> strs = new List<string>();
			foreach (TreeNode treeNode1 in treeNodes)
			{
				InheritFiles.DependencyState dependencyState1 = this.checkDependencies(treeNode1);
				if (dependencyState1 == InheritFiles.DependencyState.OK)
				{
					continue;
				}
				strs.Add(treeNode1.Text);
				if (dependencyState == InheritFiles.DependencyState.MISSING_DEPENDENCIES)
				{
					continue;
				}
				dependencyState = dependencyState1;
			}
			if (dependencyState == InheritFiles.DependencyState.MISSING_DEPENDENCIES)
			{
				if (MessageBox.Show(string.Concat("您正在尝试加载缺少依赖文件的 ", string.Join(", ", strs), ". 这将导致任何已修改的项目显示为已损坏或缺少某些数据.\n你确定要继续么?"), "缺少依赖", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                {
					return;
				}
			}
			else if (dependencyState == InheritFiles.DependencyState.MISSING_REFERENCES && MessageBox.Show(string.Concat("您正在尝试加载缺少外部引用Mod的MOD ", string.Join(", ", strs), " 这些MOD引用了其他MOD的项目. 这将会导致部分的项目出现非法引用.\n你确定要继续么?"), "缺少依赖", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
			{
				return;
			}
			if (this.activeNode != null && this.activeNode.TreeView == this.baseList && !this.parentForm.nav.SecretDeveloperMode && MessageBox.Show("您即将编辑游戏的主要数据文件之一，我想也许这不应该时你的本意\n\n因为如果你改变这个文件,你的更改将在你下次更新游戏时丢失.您也将无法与其他人共享您的mod.\n您应该创建一个新的mod文件以更改游戏数据,这将使您的更改保持独立.\n\n你确定要继续么?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != System.Windows.Forms.DialogResult.Yes)
			{
				return;
			}
			this.parentForm.clearScene();
			head.isAMod = false;
			this.parentForm.nav.ou.gameData.clear();
			this.parentForm.nav.BasePath = this.baseDir;
			this.parentForm.nav.FileMode = navigation.ModFileMode.BASE;
			GameData.ModMode modMode = GameData.ModMode.BASE;
			foreach (TreeNode treeNode2 in treeNodes)
			{
				if (treeNode2 == this.activeNode)
				{
					modMode = GameData.ModMode.ACTIVE;
				}
				else if (modMode == GameData.ModMode.ACTIVE)
				{
					modMode = GameData.ModMode.LOCKED;
				}
				if (modMode == GameData.ModMode.ACTIVE && this.cbTranslationMode.Checked)
				{
					modMode = GameData.ModMode.BASE;
				}
				if (treeNode2.TreeView == this.baseList)
				{
					str = this.baseDir;
				}
				else
				{
					string str1 = Path.Combine(this.modDir, Path.GetFileNameWithoutExtension(treeNode2.Text));
					char directorySeparatorChar = Path.DirectorySeparatorChar;
					str = string.Concat(str1, directorySeparatorChar.ToString());
				}
				string str2 = str;
				if (!this.cbTranslationMode.Checked && modMode == GameData.ModMode.ACTIVE)
				{
					this.parentForm.nav.ModPath = str2;
				}
				this.parentForm.addLoadedFile(str2, treeNode2.Text, modMode);
			}
			if (!this.cbTranslationMode.Checked)
			{
				TranslationManager.ClearActiveTranslation();
			}
			else
			{
				TranslationManager.TranslationCulture selectedItem = this.cbActiveTranslation.SelectedItem as TranslationManager.TranslationCulture;
				if (selectedItem == null)
				{
					MessageBox.Show("加载翻译失败: 无效的语言", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				FileInfo fileInfo = new FileInfo(selectedItem.GetFileName());
				if (!fileInfo.Exists)
				{
					(new GameData()).save(fileInfo.FullName);
				}
				this.parentForm.addLoadedFile(selectedItem.path, fileInfo.Name, GameData.ModMode.ACTIVE);
				this.parentForm.nav.ModPath = selectedItem.path;
				this.parentForm.nav.ou.gameData.resolveAllReferences();
				if (Control.ModifierKeys != Keys.Alt)
				{
					TranslationManager.SetActiveTranslation(selectedItem, this.parentForm.nav.ou.gameData);
				}
				else
				{
					TranslationManager.ClearActiveTranslation();
				}
			}
			this.parentForm.nav.ou.gameData.resolveAllReferences();
			this.parentForm.finishedLoading();
			base.Close();
		}

		private void modList_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
			{
				TreeView treeView = (TreeView)sender;
				Point client = treeView.PointToClient(new Point(e.X, e.Y));
				TreeNode data = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
				TreeNode nodeAt = treeView.GetNodeAt(client);
				TreeNode treeNode = (TreeNode)data.Clone();
				if (nodeAt != null)
				{
					int num = (client.Y < (nodeAt.Bounds.Top + nodeAt.Bounds.Bottom) / 2 ? 0 : 1);
					int num1 = nodeAt.TreeView.Nodes.IndexOf(nodeAt) + num;
					treeView.Nodes.Insert(num1, treeNode);
				}
				else
				{
					treeView.Nodes.Add(treeNode);
				}
				data.Remove();
				treeView.SelectedNode = treeNode;
				if (treeNode.Checked)
				{
					this.enforceDependencies(treeNode, true, true, null);
				}
				this.updateActiveMod();
			}
		}

		private void modList_DragOver(object sender, DragEventArgs e)
		{
			if (((TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode")).TreeView == sender)
			{
				e.Effect = DragDropEffects.Move;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void modList_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			Rectangle bounds = e.Bounds;
			bounds.Width = base.Width - bounds.X;
			System.Drawing.Size glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, CheckBoxState.CheckedNormal);
			Point point = new Point(bounds.X, bounds.Y + bounds.Height / 2 - glyphSize.Height / 2);
			bounds.X = bounds.X + glyphSize.Width + 2;
			if (e.Node.IsSelected)
			{
				e.Graphics.FillRectangle(Brushes.White, 0, bounds.Y, bounds.X, bounds.Height);
				e.Graphics.FillRectangle(SystemBrushes.Highlight, bounds);
			}
			else if (e.Node == this.activeNode)
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 255, 255)), bounds);
			}
			CheckBoxRenderer.DrawCheckBox(e.Graphics, point, (e.Node.Checked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal));
			TextRenderer.DrawText(e.Graphics, e.Node.Text, this.modList.Font, bounds, e.Node.ForeColor, TextFormatFlags.Default);
			if (e.Node == this.activeNode)
			{
				TextRenderer.DrawText(e.Graphics, "*ACTIVE*", this.modList.Font, bounds, e.Node.ForeColor);
			}
		}

		private void modList_ItemDrag(object sender, ItemDragEventArgs e)
		{
			base.DoDragDrop(e.Item, DragDropEffects.Move);
		}

		private void modList_MouseClick(object sender, MouseEventArgs e)
		{
			TreeNode nodeAt = ((TreeView)sender).GetNodeAt(e.Location);
			if (nodeAt != null)
			{
				if (sender != this.baseList)
				{
					this.baseList.SelectedNode = null;
				}
				else
				{
					this.modList.SelectedNode = null;
				}
				if (e.X < 24 || !nodeAt.Checked || nodeAt == this.activeNode || this.cbTranslationMode.Checked)
				{
					this.toggleFile(nodeAt);
					return;
				}
				this.updateActiveMod();
			}
		}

		private void modList_MouseDown(object sender, MouseEventArgs e)
		{
			TreeView nodeAt = (TreeView)sender;
			nodeAt.SelectedNode = nodeAt.GetNodeAt(e.Location);
		}

		private void modList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			TreeView treeView = (TreeView)sender;
			if (treeView.SelectedNode != null && e.KeyCode == Keys.Space)
			{
				this.toggleFile(treeView.SelectedNode);
			}
		}

		private bool moveAbove(TreeNode node, TreeNode above)
		{
			if (node.TreeView != above.TreeView)
			{
				return false;
			}
			if (node.Index < above.Index)
			{
				return false;
			}
			TreeView treeView = node.TreeView;
			node.Remove();
			treeView.Nodes.Insert(above.Index, node);
			return true;
		}

		private bool moveBelow(TreeNode node, TreeNode below)
		{
			if (node.TreeView != below.TreeView)
			{
				return false;
			}
			if (node.Index > below.Index)
			{
				return false;
			}
			TreeView treeView = node.TreeView;
			node.Remove();
			treeView.Nodes.Insert(below.Index, node);
			return true;
		}

		private void moveDown_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.modList.SelectedNode ?? this.baseList.SelectedNode;
			int num = selectedNode.TreeView.Nodes.IndexOf(selectedNode);
			if (num < selectedNode.TreeView.Nodes.Count - 1)
			{
				TreeView treeView = selectedNode.TreeView;
				selectedNode.Remove();
				treeView.Nodes.Insert(num + 1, selectedNode);
				if (selectedNode.Checked)
				{
					this.enforceDependencies(selectedNode, true, true, null);
				}
				treeView.SelectedNode = selectedNode;
				this.updateActiveMod();
			}
		}

		private void moveUp_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = this.modList.SelectedNode ?? this.baseList.SelectedNode;
			int num = selectedNode.TreeView.Nodes.IndexOf(selectedNode);
			if (num > 0)
			{
				TreeView treeView = selectedNode.TreeView;
				selectedNode.Remove();
				treeView.Nodes.Insert(num - 1, selectedNode);
				if (selectedNode.Checked)
				{
					this.enforceDependencies(selectedNode, true, true, null);
				}
				treeView.SelectedNode = selectedNode;
				this.updateActiveMod();
			}
		}

		private void newMod_Click(object sender, EventArgs e)
		{
			NewMod newMod = new NewMod(this.modDir);
			newMod.ShowDialog();
			if (!string.IsNullOrEmpty(newMod.name))
			{
				this.activeNode = this.modList.Nodes.Add(newMod.name);
				this.activeNode.Checked = true;
				this.activeNode.EnsureVisible();
				this.baseList.Invalidate();
				this.loadButton.Select();
			}
		}

		private void selectAll_Click(object sender, EventArgs e)
		{
			int num = 0;
			foreach (object node in this.modList.Nodes)
			{
				if (!((TreeNode)node).Checked)
				{
					continue;
				}
				num++;
			}
			if (num == 0)
			{
				foreach (object obj in this.modList.Nodes)
				{
					((TreeNode)obj).Checked = true;
				}
			}
			else if (num != this.modList.Nodes.Count)
			{
				foreach (object node1 in this.modList.Nodes)
				{
					((TreeNode)node1).Checked = false;
				}
			}
			else
			{
				foreach (TreeNode treeNode in this.modList.Nodes)
				{
					treeNode.Checked = this.defaultChecked.Contains(treeNode.Text);
				}
			}
			this.updateActiveMod();
		}

		private void toggleFile(TreeNode item)
		{
			item.Checked = !item.Checked;
			if (item.TreeView != this.modList)
			{
				this.modList.SelectedNode = null;
			}
			else
			{
				this.baseList.SelectedNode = null;
			}
			if (item.Checked)
			{
				this.enforceDependencies(item, true, true, null);
			}
			this.updateActiveMod();
		}

		private void updateActiveMod()
		{
			TreeNode selectedNode = null;
			foreach (TreeNode node in this.baseList.Nodes)
			{
				if (!node.Checked)
				{
					continue;
				}
				selectedNode = node;
			}
			foreach (TreeNode treeNode in this.modList.Nodes)
			{
				if (!treeNode.Checked)
				{
					continue;
				}
				selectedNode = treeNode;
			}
			if (this.modList.SelectedNode != null && this.modList.SelectedNode.Checked)
			{
				selectedNode = this.modList.SelectedNode;
			}
			if (this.baseList.SelectedNode != null && this.baseList.SelectedNode.Checked)
			{
				selectedNode = this.baseList.SelectedNode;
			}
			if (selectedNode != null && selectedNode != this.activeNode)
			{
				this.modList.Invalidate();
				this.baseList.Invalidate();
			}
			this.activeNode = selectedNode;
			this.loadButton.Enabled = selectedNode != null;
			if (this.cbTranslationMode.Checked)
			{
				this.activeNode = null;
			}
		}

		private void updateTranslationsList(bool refresh)
		{
			object selectedItem = this.cbActiveTranslation.SelectedItem;
			this.cbActiveTranslation.Items.Clear();
			if (refresh)
			{
				TranslationManager.RefreshAvailableTranslations();
			}
			List<TranslationManager.TranslationCulture> avaliableTranslations = TranslationManager.AvaliableTranslations;
			if (avaliableTranslations.Count > 0)
			{
				this.cbActiveTranslation.Items.AddRange(avaliableTranslations.ToArray());
				if (!refresh)
				{
					selectedItem = avaliableTranslations[avaliableTranslations.Count - 1];
				}
				if (selectedItem != null)
				{
					this.cbActiveTranslation.SelectedItem = selectedItem;
					return;
				}
				this.cbActiveTranslation.SelectedIndex = 0;
			}
		}

		private enum DependencyState
		{
			OK,
			MISSING_DEPENDENCIES,
			MISSING_REFERENCES
		}
	}
}