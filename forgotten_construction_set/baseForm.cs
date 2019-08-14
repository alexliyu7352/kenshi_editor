using forgotten_construction_set.Properties;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class baseForm : Form
	{
		public bool mergeModsMode;

		public navigation nav = new navigation();

		public Errors errorForm = new Errors();

		public ToDo todoList;

		public ModInfo modInfo;

		public NavigationTranslation navTranslation = new NavigationTranslation();

		public List<string> activeMods = new List<string>();

		public static Logger logger;

		private IContainer components;

		private ToolStrip toolStrip1;

		private ToolStripButton openMod;

		private ToolStripButton saveMod;

		private SaveFileDialog saveFileDialog;

		private OpenFileDialog openFileDialog;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripButton artifactsSettings;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripSeparator toolStripSeparator4;

		private ToolStripSeparator toolStripSeparator10;

		private ToolStripSeparator toolStripSeparator11;

		private ToolStripSeparator toolStripSeparator6;

		private ToolStripButton openAny;

		private ToolStripButton mergeMod;

		private ToolStripButton cleanupAll;

		private ToolStripButton changeList;

		private ToolStripButton assetList;

		private ToolStripSeparator toolStripSeparator5;

		private ToolStripButton exportModsFile;

		private ToolStripButton globalSettings;

		private ToolStripButton Info;

		private ToolStripButton btnTranslations;

		private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem updateStripMenuItem;
        private ToolStripMenuItem aboutMenuItem;
        private ToolStripButton translationFixMode;

		static baseForm()
		{
			baseForm.logger = LogManager.GetLogger("FCS");
		}

		public baseForm()
		{
			this.InitializeComponent();
			this.nav.MdiParent = this;
			this.navTranslation.MdiParent = this;
			this.errorForm.MdiParent = this;
			this.modInfo = new ModInfo(this)
			{
				MdiParent = this
			};
			GameData.initialise();
			Definitions.load("fcs_layout.def", this.nav);
			Definitions.load("fcs_enums.def", this.nav);
			Definitions.load("fcs.def", this.nav);
			base.Shown += new EventHandler(this.baseForm_Shown);
			if (this.nav.SecretDeveloperMode)
			{
				this.toolStrip1.Items.Insert(11, new ToolStripButton("待办列表", null, new EventHandler(this.toDoList_Click)));
				this.todoList = new ToDo(this, this.nav);
				this.nav.addTodoMenuItem();
			}
			SteamManager.Instance.Init();
			this.Info.Text = "Steam工坊";
			this.Info.ToolTipText = "Steam工坊";
            //this.nav.RootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            // TODO 这用来测试
            this.nav.RootPath = Path.GetDirectoryName("F:\\SteamLibrary\\steamapps\\common\\Kenshi\\");
            base.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
		}

		public void addLoadedFile(string path, string file, GameData.ModMode mode)
		{
			string extension = Path.GetExtension(file);
			if (extension == ".mod" || extension == ".translation")
			{
				head.isAMod = true;
				this.activeMods.Add(file);
			}
			if (mode == GameData.ModMode.ACTIVE)
			{
				this.nav.HasChanges = false;
				this.nav.setActiveFilename(string.Concat(path, file), navigation.ModFileMode.USER);
				this.updateTitle();
				GameData.nullDesc.description = "这个值不再被游戏使用";
			}
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			if (!this.nav.ou.gameData.load(string.Concat(path, "\\", file), mode, false))
			{
				baseForm.logger.Error("加载MOD错误 '{0}'.", file);
				return;
			}
			if (mode == GameData.ModMode.ACTIVE)
			{
				baseForm.logger.Info("已加载MOD '{0}'. (激活MOD)", file);
				return;
			}
			baseForm.logger.Info("已加载 '{0}'.", file);
		}

		private void artifactsSettings_Click(object sender, EventArgs e)
		{
			GameData.Item item = null;
			foreach (GameData.Item value in this.nav.ou.gameData.items.Values)
			{
				if (value.Name != "ARTIFACTS")
				{
					continue;
				}
				item = value;
			}
			if (item == null)
			{
				item = this.nav.ou.gameData.createItem(itemType.ARTIFACTS);
				item.Name = "ARTIFACTS";
			}
			this.nav.showItemProperties(item);
		}

		private void assetList_Click(object sender, EventArgs e)
		{
			Form[] mdiChildren = base.MdiChildren;
			for (int i = 0; i < (int)mdiChildren.Length; i++)
			{
				Form form = mdiChildren[i];
				if (form is AssetList)
				{
					form.BringToFront();
					return;
				}
			}
			(new AssetList(this.nav)
			{
				MdiParent = this
			}).Show();
		}

		private void baseForm_DragDrop(object sender, DragEventArgs e)
		{
			string[] data = e.Data.GetData(DataFormats.FileDrop, false) as string[];
			if (data.Length != 0)
			{
				this.openAnyFile(data[0]);
			}
		}

		private void baseForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] data = e.Data.GetData(DataFormats.FileDrop, false) as string[];
				if (data.Length != 0 && !File.GetAttributes(data[0]).HasFlag(FileAttributes.Directory))
				{
					e.Effect = DragDropEffects.All;
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}

		private void baseForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.promptSaveChanges())
			{
				SteamManager.Instance.Shutdown();
				return;
			}
			e.Cancel = true;
		}

		private void baseForm_Shown(object sender, EventArgs e)
		{
			if (Program.args.Length == 0 || !this.openAnyFile(Program.args[0]))
			{
				this.loadFile(false);
			}
		}

		private void btnTranslations_Click(object sender, EventArgs e)
		{
			(new TranslationDialog(this.nav, this.navTranslation)).ShowDialog();
		}

		private void changeList_Click(object sender, EventArgs e)
		{
			Form[] mdiChildren = base.MdiChildren;
			for (int i = 0; i < (int)mdiChildren.Length; i++)
			{
				Form form = mdiChildren[i];
				if (form is ChangeList)
				{
					form.BringToFront();
					return;
				}
			}
			(new ChangeList(this.nav)
			{
				MdiParent = this
			}).Show();
		}

		private void cleanupAll_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num1 = 0;
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			foreach (GameData.Item value in this.nav.ou.gameData.items.Values)
			{
				if (!GameData.desc.ContainsKey(value.type))
				{
					continue;
				}
				int num2 = value.clean();
				if (num2 > 0)
				{
					num++;
				}
				num1 += num2;
			}
			System.Windows.Forms.Cursor.Current = Cursors.Default;
			MessageBox.Show(string.Concat(new string[] { "已删除 ", num1.ToString(), " 个属性 从 ", num.ToString(), " 个对象" }), "清除");
		}

		public void clearScene()
		{
			this.nav.refreshListView();
			Form[] mdiChildren = base.MdiChildren;
			for (int i = 0; i < (int)mdiChildren.Length; i++)
			{
				Form form = mdiChildren[i];
				if (!(form is navigation) && !(form is Errors) && !(form is ModInfo) && !(form is NavigationTranslation) && !(form is ToDo))
				{
					form.Close();
				}
			}
			this.nav.ou.gameData.clear();
			this.activeMods.Clear();
			Errors.clear();
			if (this.todoList != null)
			{
				this.todoList.Clear();
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

		private void exportModsFile_Click(object sender, EventArgs e)
		{
			using (StreamWriter streamWriter = File.CreateText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data", "mods.cfg")))
			{
				foreach (string activeMod in this.activeMods)
				{
					if (InheritFiles.FixedMods.Contains(activeMod))
					{
						continue;
					}
					streamWriter.WriteLine(activeMod);
				}
				streamWriter.Close();
			}
		}

		public void finishedLoading()
		{
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			foreach (GameData.Item value in this.nav.ou.gameData.items.Values)
			{
				value.checkReferences();
			}
			if (TranslationManager.TranslationMode)
			{
				this.nav.Hide();
				base.BringToFront();
				this.navTranslation.Show();
			}
			else
			{
				this.nav.clearFilter();
				this.nav.ensureCategoryIsSelected();
				this.nav.refreshListView();
				base.BringToFront();
				this.nav.Show();
				this.navTranslation.Hide();
			}
			this.updateTitle();
			this.globalSettings.Enabled = !TranslationManager.TranslationMode;
			this.artifactsSettings.Enabled = !TranslationManager.TranslationMode;
			this.changeList.Enabled = true;
			this.assetList.Enabled = !TranslationManager.TranslationMode;
			this.cleanupAll.Enabled = !TranslationManager.TranslationMode;
			this.mergeMod.Enabled = true;
			this.saveMod.Enabled = true;
			this.exportModsFile.Enabled = !TranslationManager.TranslationMode;
			this.btnTranslations.Enabled = TranslationManager.TranslationMode;
			this.translationFixMode.Enabled = TranslationManager.TranslationMode;
			if (!this.btnTranslations.Enabled)
			{
				this.btnTranslations.ToolTipText = "要启用此选项,必须以翻译模式启动. \n您可以从加载界面激活它.";
			}
			else
			{
				this.btnTranslations.ToolTipText = "导入/导出翻译文件.";
			}
			this.Info.Enabled = (TranslationManager.TranslationMode ? false : !InheritFiles.FixedMods.Contains(this.nav.ou.gameData.activeFileName));
			this.modInfo.Data = this.nav.ou.gameData;
			if (Errors.Count > 0)
			{
				Errors.show();
			}
			if (this.todoList != null)
			{
				this.todoList.LoadAll();
			}
			System.Windows.Forms.Cursor.Current = Cursors.Default;
		}

		private void globalSettings_Click(object sender, EventArgs e)
		{
			GameData.Item item = null;
			foreach (GameData.Item value in this.nav.ou.gameData.items.Values)
			{
				if (value.Name != "GLOBAL CONSTANTS")
				{
					continue;
				}
				item = value;
			}
			if (item == null)
			{
				item = this.nav.ou.gameData.createItem(itemType.CONSTANTS);
				item.Name = "GLOBAL CONSTANTS";
			}
			this.nav.showItemProperties(item);
		}

		private void info_Click(object sender, EventArgs e)
		{
			this.modInfo.Show();
			this.modInfo.BringToFront();
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(baseForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.openMod = new System.Windows.Forms.ToolStripButton();
            this.saveMod = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.Info = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.globalSettings = new System.Windows.Forms.ToolStripButton();
            this.artifactsSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cleanupAll = new System.Windows.Forms.ToolStripButton();
            this.changeList = new System.Windows.Forms.ToolStripButton();
            this.assetList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.mergeMod = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.exportModsFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.btnTranslations = new System.Windows.Forms.ToolStripButton();
            this.translationFixMode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.openAny = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.updateStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMod,
            this.saveMod,
            this.toolStripSeparator2,
            this.Info,
            this.toolStripSeparator1,
            this.globalSettings,
            this.artifactsSettings,
            this.toolStripSeparator4,
            this.cleanupAll,
            this.changeList,
            this.assetList,
            this.toolStripSeparator6,
            this.mergeMod,
            this.toolStripSeparator5,
            this.exportModsFile,
            this.toolStripSeparator10,
            this.btnTranslations,
            this.translationFixMode,
            this.toolStripSeparator3,
            this.toolStripSeparator11,
            this.openAny,
            this.toolStripSeparator7,
            this.toolStripDropDownButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1083, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // openMod
            // 
            this.openMod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openMod.Image = global::forgotten_construction_set.Properties.Resources.OpenFolder;
            this.openMod.Name = "openMod";
            this.openMod.Size = new System.Drawing.Size(23, 22);
            this.openMod.Text = "加载";
            this.openMod.Click += new System.EventHandler(this.openMod_Click);
            // 
            // saveMod
            // 
            this.saveMod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveMod.Enabled = false;
            this.saveMod.Image = global::forgotten_construction_set.Properties.Resources.Save;
            this.saveMod.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveMod.Name = "saveMod";
            this.saveMod.Size = new System.Drawing.Size(23, 22);
            this.saveMod.Text = "保存";
            this.saveMod.Click += new System.EventHandler(this.saveMod_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // Info
            // 
            this.Info.Enabled = false;
            this.Info.Image = global::forgotten_construction_set.Properties.Resources.UIAboutBox;
            this.Info.Name = "Info";
            this.Info.Size = new System.Drawing.Size(80, 22);
            this.Info.Text = "Mod信息";
            this.Info.Click += new System.EventHandler(this.info_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // globalSettings
            // 
            this.globalSettings.BackColor = System.Drawing.SystemColors.Control;
            this.globalSettings.Enabled = false;
            this.globalSettings.Image = global::forgotten_construction_set.Properties.Resources.Settings;
            this.globalSettings.Name = "globalSettings";
            this.globalSettings.Size = new System.Drawing.Size(100, 22);
            this.globalSettings.Text = "全局游戏设置";
            this.globalSettings.Click += new System.EventHandler(this.globalSettings_Click);
            // 
            // artifactsSettings
            // 
            this.artifactsSettings.BackColor = System.Drawing.SystemColors.Control;
            this.artifactsSettings.Enabled = false;
            this.artifactsSettings.Image = global::forgotten_construction_set.Properties.Resources.SettingsFile;
            this.artifactsSettings.Name = "artifactsSettings";
            this.artifactsSettings.Size = new System.Drawing.Size(76, 22);
            this.artifactsSettings.Text = "文物设置";
            this.artifactsSettings.Click += new System.EventHandler(this.artifactsSettings_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // cleanupAll
            // 
            this.cleanupAll.Enabled = false;
            this.cleanupAll.Image = global::forgotten_construction_set.Properties.Resources.CleanData_16x;
            this.cleanupAll.Name = "cleanupAll";
            this.cleanupAll.Size = new System.Drawing.Size(52, 22);
            this.cleanupAll.Text = "清理";
            this.cleanupAll.ToolTipText = "删除来自旧版本中的过时的数据";
            this.cleanupAll.Click += new System.EventHandler(this.cleanupAll_Click);
            // 
            // changeList
            // 
            this.changeList.Enabled = false;
            this.changeList.Image = global::forgotten_construction_set.Properties.Resources.ChangesetGroup_16x;
            this.changeList.Name = "changeList";
            this.changeList.Size = new System.Drawing.Size(76, 22);
            this.changeList.Text = "已改变的";
            this.changeList.ToolTipText = "列出当前已激活mod包含的所有更改";
            this.changeList.Click += new System.EventHandler(this.changeList_Click);
            // 
            // assetList
            // 
            this.assetList.Enabled = false;
            this.assetList.Image = global::forgotten_construction_set.Properties.Resources.AppRoot_16x;
            this.assetList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.assetList.Name = "assetList";
            this.assetList.Size = new System.Drawing.Size(52, 22);
            this.assetList.Text = "资源";
            this.assetList.ToolTipText = "列出所有的引用的文件";
            this.assetList.Click += new System.EventHandler(this.assetList_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // mergeMod
            // 
            this.mergeMod.Enabled = false;
            this.mergeMod.Image = global::forgotten_construction_set.Properties.Resources.Merge_24x;
            this.mergeMod.Name = "mergeMod";
            this.mergeMod.Size = new System.Drawing.Size(83, 22);
            this.mergeMod.Text = "合并MOD";
            this.mergeMod.ToolTipText = "合并来自另一个mod的更改";
            this.mergeMod.Click += new System.EventHandler(this.mergebutton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // exportModsFile
            // 
            this.exportModsFile.Enabled = false;
            this.exportModsFile.Image = global::forgotten_construction_set.Properties.Resources.ExportData_16x;
            this.exportModsFile.Name = "exportModsFile";
            this.exportModsFile.Size = new System.Drawing.Size(107, 22);
            this.exportModsFile.Text = "导出MOD文件";
            this.exportModsFile.Click += new System.EventHandler(this.exportModsFile_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // btnTranslations
            // 
            this.btnTranslations.Enabled = false;
            this.btnTranslations.Image = global::forgotten_construction_set.Properties.Resources.TranslateDocument_32x;
            this.btnTranslations.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTranslations.Name = "btnTranslations";
            this.btnTranslations.Size = new System.Drawing.Size(52, 22);
            this.btnTranslations.Text = "翻译";
            this.btnTranslations.Click += new System.EventHandler(this.btnTranslations_Click);
            // 
            // translationFixMode
            // 
            this.translationFixMode.Enabled = false;
            this.translationFixMode.Image = global::forgotten_construction_set.Properties.Resources.ModSets_16x;
            this.translationFixMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.translationFixMode.Name = "translationFixMode";
            this.translationFixMode.Size = new System.Drawing.Size(76, 22);
            this.translationFixMode.Text = "翻译修复";
            this.translationFixMode.Click += new System.EventHandler(this.translationFixMode_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // openAny
            // 
            this.openAny.Image = global::forgotten_construction_set.Properties.Resources.Open;
            this.openAny.Name = "openAny";
            this.openAny.Size = new System.Drawing.Size(100, 22);
            this.openAny.Text = "打开任何文件";
            this.openAny.ToolTipText = "打开任何数据文件类型（一般用来打开其他不在MOD目录下的MOD）";
            this.openAny.Click += new System.EventHandler(this.openAny_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "mod";
            this.saveFileDialog.Filter = "Kenshi gamedata (*.mod)|*.mod";
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Title = "保存新文件";
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "mod";
            this.openFileDialog.FileName = "openFileDialog1";
            this.openFileDialog.Filter = "All files|*.*|Kenshi gamedata (*.mod)|*.mod";
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Title = "打开文件";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateStripMenuItem,
            this.aboutMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton1.Text = "关于&更新";
            // 
            // updateStripMenuItem
            // 
            this.updateStripMenuItem.Name = "updateStripMenuItem";
            this.updateStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.updateStripMenuItem.Text = "更新地址";
            this.updateStripMenuItem.Click += new System.EventHandler(this.UpdateStripMenuItem_Click);
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutMenuItem.Text = "关于";
            this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // baseForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 624);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "baseForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.baseForm_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.baseForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.baseForm_DragEnter);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		public void loadFile(bool mergeMode)
		{
			(new InheritFiles(this, string.Concat(this.nav.RootPath, "\\data\\"), string.Concat(this.nav.RootPath, "\\mods\\"))).ShowDialog(this);
		}

		private void mergebutton_Click(object sender, EventArgs e)
		{
			if ((new MergeDialog(this.nav.ou.gameData, this.nav)).ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.nav.refreshListView();
				this.nav.HasChanges = true;
			}
		}

		private void openAny_Click(object sender, EventArgs e)
		{
			if (!this.promptSaveChanges())
			{
				return;
			}
			this.openFileDialog.Title = "加载唯一数据文件";
			this.openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			this.openFileDialog.FileName = "";
			if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.openAnyFile(this.openFileDialog.FileName);
			}
		}

		private bool openAnyFile(string file)
		{
			if (file.LastIndexOf("\\") == -1)
			{
				file.LastIndexOf("/");
			}
			this.nav.clearFilter();
			this.clearScene();
			this.nav.Show();
			this.nav.setActiveFilename(file, navigation.ModFileMode.SINGLE);
			GameData.nullDesc.description = "";
			this.globalSettings.Enabled = false;
			this.changeList.Enabled = true;
			this.assetList.Enabled = true;
			this.cleanupAll.Enabled = false;
			this.mergeMod.Enabled = true;
			this.saveMod.Enabled = true;
			this.Info.Enabled = false;
			if (this.modInfo.Visible)
			{
				this.modInfo.Hide();
			}
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			bool flag = this.nav.ou.gameData.load(file, GameData.ModMode.ACTIVE, false);
			this.nav.ou.gameData.resolveAllReferences();
			this.nav.ou.gameData.activeFileName = file;
			this.nav.refreshListViewAll();
			this.updateTitle();
			System.Windows.Forms.Cursor.Current = Cursors.Default;
			return flag;
		}

		private void openMod_Click(object sender, EventArgs e)
		{
			if (!this.promptSaveChanges())
			{
				return;
			}
			this.loadFile(false);
		}

		protected override bool ProcessCmdKey(ref Message message, Keys keys)
		{
			if (keys == Keys.F2)
			{
				if (this.globalSettings.Enabled)
				{
					this.globalSettings_Click(null, new EventArgs());
				}
				return true;
			}
			if (keys == (Keys.LButton | Keys.RButton | Keys.Cancel | Keys.MButton | Keys.XButton1 | Keys.XButton2 | Keys.Back | Keys.Tab | Keys.LineFeed | Keys.Clear | Keys.Return | Keys.Enter | Keys.A | Keys.B | Keys.C | Keys.D | Keys.E | Keys.F | Keys.G | Keys.H | Keys.I | Keys.J | Keys.K | Keys.L | Keys.M | Keys.N | Keys.O | Keys.Control))
			{
				this.loadFile(false);
				return true;
			}
			if (keys != (Keys.LButton | Keys.RButton | Keys.Cancel | Keys.ShiftKey | Keys.ControlKey | Keys.Menu | Keys.Pause | Keys.A | Keys.B | Keys.C | Keys.P | Keys.Q | Keys.R | Keys.S | Keys.Control))
			{
				return base.ProcessCmdKey(ref message, keys);
			}
			if (this.saveMod.Enabled)
			{
				this.saveFile();
			}
			return true;
		}

		private bool promptSaveChanges()
		{
			if (this.nav.HasChanges)
			{
				System.Windows.Forms.DialogResult dialogResult = MessageBox.Show(string.Concat("您想要保存变更到 ", this.nav.ou.gameData.activeFileName, "?"), "已改变", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
				if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
				{
					return false;
				}
				if (dialogResult == System.Windows.Forms.DialogResult.Yes)
				{
					this.saveFile();
				}
			}
			return true;
		}

		public void saveFile()
		{
			base.Focus();
			if (string.IsNullOrEmpty(this.nav.ActiveFile))
			{
				this.saveFileAs();
				return;
			}
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
			if (this.nav.FileMode != navigation.ModFileMode.SINGLE)
			{
				this.modInfo.refreshDependencies();
			}
			if (this.todoList != null)
			{
				this.todoList.SaveData();
			}
			this.nav.ou.gameData.save(this.nav.ActiveFile);
			System.Windows.Forms.Cursor.Current = Cursors.Default;
			this.nav.HasChanges = false;
		}

		public void saveFileAs()
		{
			base.Focus();
			this.saveFileDialog.InitialDirectory = this.nav.ModPath;
			this.saveFileDialog.RestoreDirectory = true;
			if (this.saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
				this.nav.ou.gameData.save(this.saveFileDialog.FileName);
				this.nav.ActiveFile = this.saveFileDialog.FileName;
				System.Windows.Forms.Cursor.Current = Cursors.Default;
				this.nav.HasChanges = false;
				this.updateTitle();
			}
		}

		private void saveMod_Click(object sender, EventArgs e)
		{
			this.saveFile();
		}

		private void toDoList_Click(object sender, EventArgs e)
		{
			this.todoList.Show();
		}

		private void translationFixMode_Click(object sender, EventArgs e)
		{
			(new TranslationFix()
			{
				MdiParent = this
			}).Show();
		}

		public void updateTitle()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			stringBuilder.Append("Forgotten Construction Set ");
			stringBuilder.Append("(Steam) ");
			stringBuilder.Append("v 1.0");
			stringBuilder.Append(" - ");
			stringBuilder.Append(this.nav.ActiveFile);
			if (TranslationManager.TranslationMode)
			{
				stringBuilder.Append(" [TRANSLATION MODE]");
			}
			if (this.nav.HasChanges)
			{
				stringBuilder.Append(" *");
			}
			this.Text = stringBuilder.ToString();
		}

        private void UpdateStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/alexliyu7352/kenshi_editor");

        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("这个版本的编辑器是基于Kenshi官方编辑器, 并由虚空入侵MOD制作组的Alexliyu进行制作,除了汉化以外还修复了一些编辑器本身的问题以及添加了一些功能.\n" +
                "我们无意侵犯任何版权,也尊重开发者的权益.制作本编辑器只是为了弥补使用官方版本开发时候的一些不便捷或者产生BUG的地方.\n" +
                "因此请各位能多多支持Kenshi游戏正版,当然也希望能支持虚空入侵MOD工作组, 毕竟我们是用爱来发电的一群大叔.\n如果发现BUG或者有什么修改建议请在编辑器的主页进行反馈.\n" +
                "另外再次重申:这个项目源码已经开放，一切仅仅只是为了研究血裔使用, 切勿商用,否则一些后果概不负责.\n" +
                "我们的联系方式: qq群:672036449, https://github.com/alexliyu7352/kenshi_editor"
                , "关于本版本的编辑器", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}