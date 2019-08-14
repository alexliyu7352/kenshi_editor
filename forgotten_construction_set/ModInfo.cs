using forgotten_construction_set.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class ModInfo : Form
	{
		private baseForm parentForm;

		private GameData gameData;

		private bool noEvents;

		private ModData info;

		private bool uploading;

		private bool updateEnabled;

		private Thread updateUploadThread;

		private IContainer components;

		private TextBox txtAuthor;

		private TextBox txtDescription;

		private PictureBox imgPreview;

		private GroupBox gbDescription;

		private GroupBox gbDependencies;

		private GroupBox gbImage;

		private Button btnSetImage;

		private GroupBox gbTags;

		private ListBox listDependencies;

		private Button btnUpload;

		private Button btnRemoveImage;

		private TextBox txtTitle;

		private TextBox txtVersion;

		private GroupBox gbInfo;

		private ComboBox cbVisiblity;

		private LinkLabel linkSteamWorkshopAgreement;

		private StatusStrip ssUpload;

		private ToolStripProgressBar pbUpload;

		private ToolStripStatusLabel lbUploadStatus;

		private ToolStripStatusLabel lbStatus;

		private Label lbLastUpdateDate;

		private Button btnOpenFolder;

		private ToolTip toolTipInfo;

		private Button btnRefreshFromSteam;

		private CheckedListBox listTags;

		private TextBox txtChangeNotes;

		private Button btnClearChangeNotes;

		private Label lbModSteamID;

		public GameData Data
		{
			get
			{
				return this.gameData;
			}
			set
			{
				this.gameData = value;
				if (this.gameData != null)
				{
					this.refreshModUIData();
				}
			}
		}

		public ModInfo(baseForm parent)
		{
			this.parentForm = parent;
			this.InitializeComponent();
			base.ShowIcon = true;
			base.Icon = System.Drawing.Icon.FromHandle(Resources.UIAboutBox.GetHicon());
			this.cbVisiblity.SelectedIndex = 0;
			this.pbUpload.Visible = false;
			this.lbUploadStatus.Visible = false;
		}

		private void BackgroundUpdateUpload()
		{
			while (this.updateEnabled && SteamManager.Instance.Uploading)
			{
				ulong num = (ulong)0;
				ulong num1 = (ulong)0;
				SteamManager.UploadState uploadState = SteamManager.Instance.GetUploadState(out num, out num1);
				base.BeginInvoke(new MethodInvoker(() => this.updateUploadState(uploadState, num, num1)));
			}
		}

		private void btnClearChangeNotes_Click(object sender, EventArgs e)
		{
			this.txtChangeNotes.Clear();
		}

		private void btnOpenFolder_Click(object sender, EventArgs e)
		{
			if (this.info != null)
			{
				Process.Start(this.info.GetAbsoluteFolder());
			}
		}

		private void btnRefreshFromSteam_Click(object sender, EventArgs e)
		{
			if (this.info != null && this.info.id != 0)
			{
				this.btnRefreshFromSteam.Enabled = false;
				this.btnUpload.Enabled = false;
				SteamManager.Instance.UpdateModInfo(this.info, new Action<bool>(this.OnModUpdateComplete));
			}
		}

		private void btnRemoveImage_Click(object sender, EventArgs e)
		{
			FileInfo fileInfo = new FileInfo(this.info.GetImagePath());
			if (fileInfo.Exists)
			{
				try
				{
					fileInfo.Delete();
					this.imgPreview.Image = null;
				}
				catch (Exception exception)
				{
					MessageBox.Show("删除图片错误.", "错误", MessageBoxButtons.OK);
				}
			}
			this.imgPreview.Refresh();
		}

		private void btnSetImage_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				CheckFileExists = true,
				Multiselect = false,
				Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif"
			};
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
				if (fileInfo.Length > (long)1048576)
				{
					MessageBox.Show("图片必须小于1MB.", "错误", MessageBoxButtons.OK);
					return;
				}
				fileInfo.CopyTo(this.info.GetImagePath(), true);
				this.imgPreview.ImageLocation = this.imgPreview.ImageLocation;
				this.imgPreview.Refresh();
			}
		}

		private void btnUpload_Click(object sender, EventArgs e)
		{
			if (this.uploading)
			{
				return;
			}
			if (this.info != null)
			{
				this.uploading = true;
				this.btnUpload.Enabled = false;
				this.txtChangeNotes.Enabled = false;
				this.parentForm.saveFile();
				this.info.changeNotes = this.txtChangeNotes.Text;
				this.info.Save();
				if (SteamManager.Instance.Enabled)
				{
					SteamManager.Instance.Upload(this.info, new Action<bool, string>(this.OnUploadComplete));
					this.updateUploadThread = new Thread(new ThreadStart(this.BackgroundUpdateUpload))
					{
						IsBackground = true,
						Name = "Mod Upload Thread"
					};
					this.updateUploadThread.Start();
				}
			}
		}

		private void CancelUpload()
		{
			this.btnUpload.Enabled = false;
			SteamManager.Instance.AbortUpload();
			this.updateUploadThread.Join();
			this.updateUploadThread = null;
			this.lbStatus.Text = "已取消";
			this.pbUpload.Value = 0;
			this.pbUpload.Visible = false;
			this.lbUploadStatus.Visible = false;
			this.uploading = false;
			this.btnUpload.Text = "上传";
			this.btnUpload.Enabled = true;
		}

		private void cbVisiblity_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.noEvents)
			{
				return;
			}
			if (this.info != null)
			{
				this.info.visibility = this.cbVisiblity.SelectedIndex;
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

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.txtAuthor = new TextBox();
			this.txtDescription = new TextBox();
			this.gbDescription = new GroupBox();
			this.gbDependencies = new GroupBox();
			this.listDependencies = new ListBox();
			this.gbImage = new GroupBox();
			this.btnRemoveImage = new Button();
			this.btnSetImage = new Button();
			this.imgPreview = new PictureBox();
			this.gbTags = new GroupBox();
			this.listTags = new CheckedListBox();
			this.txtTitle = new TextBox();
			this.txtVersion = new TextBox();
			this.gbInfo = new GroupBox();
			this.txtChangeNotes = new TextBox();
			this.btnClearChangeNotes = new Button();
			this.btnRefreshFromSteam = new Button();
			this.lbModSteamID = new Label();
			this.lbLastUpdateDate = new Label();
			this.cbVisiblity = new ComboBox();
			this.linkSteamWorkshopAgreement = new LinkLabel();
			this.btnUpload = new Button();
			this.ssUpload = new StatusStrip();
			this.pbUpload = new ToolStripProgressBar();
			this.lbUploadStatus = new ToolStripStatusLabel();
			this.lbStatus = new ToolStripStatusLabel();
			this.toolTipInfo = new ToolTip(this.components);
			this.btnOpenFolder = new Button();
			Label label = new Label();
			Label point = new Label();
			Label font = new Label();
			Label size = new Label();
			Label label1 = new Label();
			Label point1 = new Label();
			Label size1 = new Label();
			this.gbDescription.SuspendLayout();
			this.gbDependencies.SuspendLayout();
			this.gbImage.SuspendLayout();
			((ISupportInitialize)this.imgPreview).BeginInit();
			this.gbTags.SuspendLayout();
			this.gbInfo.SuspendLayout();
			this.ssUpload.SuspendLayout();
			base.SuspendLayout();
			label.AutoSize = true;
			label.Location = new Point(31, 41);
			label.Name = "lbsAuthor";
			label.Size = new System.Drawing.Size(41, 13);
			label.TabIndex = 0;
			label.Text = "作者:";
			point.AutoSize = true;
			point.Location = new Point(261, 41);
			point.Name = "lbsVersion";
			point.Size = new System.Drawing.Size(45, 13);
			point.TabIndex = 6;
			point.Text = "版本:";
			font.AutoSize = true;
			font.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			font.Location = new Point(12, 15);
			font.Name = "lbsModTitle";
			font.Size = new System.Drawing.Size(60, 13);
			font.TabIndex = 0;
			font.Text = "Mod 标题:";
			size.AutoSize = true;
			size.Location = new Point(246, 76);
			size.Name = "lbsVisibility";
			size.Size = new System.Drawing.Size(46, 13);
			size.TabIndex = 14;
			size.Text = "显示:";
			label1.AutoSize = true;
			label1.Location = new Point(226, 48);
			label1.Name = "lbsLastUpdate";
			label1.Size = new System.Drawing.Size(66, 13);
			label1.TabIndex = 19;
			label1.Text = "最后更新:";
			point1.AutoSize = true;
			point1.Location = new Point(6, 16);
			point1.Name = "lbsUpdateInfo";
			point1.Size = new System.Drawing.Size(76, 13);
			point1.TabIndex = 19;
			point1.Text = "Change notes:";
			size1.AutoSize = true;
			size1.Location = new Point(226, 16);
			size1.Name = "lbsSteamID";
			size1.Size = new System.Drawing.Size(78, 13);
			size1.TabIndex = 19;
			size1.Text = "Mod Steam ID:";
			this.txtAuthor.Location = new Point(78, 38);
			this.txtAuthor.MaxLength = 100;
			this.txtAuthor.Name = "txtAuthor";
			this.txtAuthor.Size = new System.Drawing.Size(166, 20);
			this.txtAuthor.TabIndex = 2;
			this.txtAuthor.TextChanged += new EventHandler(this.txtAuthor_TextChanged);
			this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.txtDescription.Location = new Point(6, 14);
			this.txtDescription.Margin = new System.Windows.Forms.Padding(6);
			this.txtDescription.MaxLength = 8000;
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(371, 131);
			this.txtDescription.TabIndex = 0;
			this.txtDescription.TextChanged += new EventHandler(this.txtDescription_TextChanged);
			this.gbDescription.Controls.Add(this.txtDescription);
			this.gbDescription.Location = new Point(12, 64);
			this.gbDescription.Name = "gbDescription";
			this.gbDescription.Size = new System.Drawing.Size(383, 151);
			this.gbDescription.TabIndex = 4;
			this.gbDescription.TabStop = false;
			this.gbDescription.Text = "描述";
			this.gbDependencies.Controls.Add(this.listDependencies);
			this.gbDependencies.Location = new Point(12, 221);
			this.gbDependencies.Name = "gbDependencies";
			this.gbDependencies.Size = new System.Drawing.Size(383, 103);
			this.gbDependencies.TabIndex = 11;
			this.gbDependencies.TabStop = false;
			this.gbDependencies.Text = "依赖";
			this.listDependencies.BackColor = SystemColors.Control;
			this.listDependencies.FormattingEnabled = true;
			this.listDependencies.IntegralHeight = false;
			this.listDependencies.Location = new Point(6, 19);
			this.listDependencies.Name = "listDependencies";
			this.listDependencies.SelectionMode = SelectionMode.None;
			this.listDependencies.Size = new System.Drawing.Size(371, 78);
			this.listDependencies.TabIndex = 0;
			this.listDependencies.TabStop = false;
			this.gbImage.Controls.Add(this.btnRemoveImage);
			this.gbImage.Controls.Add(this.btnSetImage);
			this.gbImage.Controls.Add(this.imgPreview);
			this.gbImage.Location = new Point(401, 6);
			this.gbImage.Name = "gbImage";
			this.gbImage.Size = new System.Drawing.Size(268, 318);
			this.gbImage.TabIndex = 6;
			this.gbImage.TabStop = false;
			this.gbImage.Text = "预览图片";
			this.btnRemoveImage.BackgroundImage = Resources.Cancel;
			this.btnRemoveImage.BackgroundImageLayout = ImageLayout.Zoom;
			this.btnRemoveImage.Location = new Point(238, 288);
			this.btnRemoveImage.Name = "btnRemoveImage";
			this.btnRemoveImage.Size = new System.Drawing.Size(24, 24);
			this.btnRemoveImage.TabIndex = 1;
			this.toolTipInfo.SetToolTip(this.btnRemoveImage, "删除预览图片");
			this.btnRemoveImage.UseVisualStyleBackColor = true;
			this.btnRemoveImage.Click += new EventHandler(this.btnRemoveImage_Click);
			this.btnSetImage.BackgroundImage = Resources.AddImage_16x;
			this.btnSetImage.BackgroundImageLayout = ImageLayout.Zoom;
			this.btnSetImage.Location = new Point(6, 288);
			this.btnSetImage.Name = "btnSetImage";
			this.btnSetImage.Size = new System.Drawing.Size(24, 24);
			this.btnSetImage.TabIndex = 0;
			this.toolTipInfo.SetToolTip(this.btnSetImage, "加载预览图片");
			this.btnSetImage.UseVisualStyleBackColor = true;
			this.btnSetImage.Click += new EventHandler(this.btnSetImage_Click);
			this.imgPreview.BorderStyle = BorderStyle.FixedSingle;
			this.imgPreview.ErrorImage = null;
			this.imgPreview.Location = new Point(6, 19);
			this.imgPreview.Name = "imgPreview";
			this.imgPreview.Size = new System.Drawing.Size(256, 256);
			this.imgPreview.SizeMode = PictureBoxSizeMode.StretchImage;
			this.imgPreview.TabIndex = 8;
			this.imgPreview.TabStop = false;
			this.gbTags.Controls.Add(this.listTags);
			this.gbTags.Location = new Point(12, 330);
			this.gbTags.Name = "gbTags";
			this.gbTags.Size = new System.Drawing.Size(163, 142);
			this.gbTags.TabIndex = 5;
			this.gbTags.TabStop = false;
			this.gbTags.Text = "标签";
			this.listTags.CheckOnClick = true;
			this.listTags.FormattingEnabled = true;
			this.listTags.IntegralHeight = false;
			this.listTags.Items.AddRange(new object[] { "Buildings", "Characters", "Cheats", "Clothing/Armour", "Environment/Map", "Factions", "Gameplay", "Graphical", "GUI", "Items/Weapons", "Races", "Research", "Total Overhaul", "Translation" });
			this.listTags.Location = new Point(6, 19);
			this.listTags.Name = "listTags";
			this.listTags.Size = new System.Drawing.Size(151, 117);
			this.listTags.Sorted = true;
			this.listTags.TabIndex = 5;
			this.listTags.SelectedIndexChanged += new EventHandler(this.listTags_SelectedIndexChanged);
			this.txtTitle.Location = new Point(78, 12);
			this.txtTitle.MaxLength = 128;
			this.txtTitle.Name = "txtTitle";
			this.txtTitle.Size = new System.Drawing.Size(283, 20);
			this.txtTitle.TabIndex = 0;
			this.txtTitle.TextChanged += new EventHandler(this.txtTitle_TextChanged);
			this.txtVersion.Location = new Point(312, 38);
			this.txtVersion.MaxLength = 8;
			this.txtVersion.Name = "txtVersion";
			this.txtVersion.Size = new System.Drawing.Size(83, 20);
			this.txtVersion.TabIndex = 3;
			this.txtVersion.TextChanged += new EventHandler(this.txtVersion_TextChanged);
			this.txtVersion.KeyPress += new KeyPressEventHandler(this.txtVersion_KeyPress);
			this.gbInfo.Controls.Add(this.txtChangeNotes);
			this.gbInfo.Controls.Add(this.btnClearChangeNotes);
			this.gbInfo.Controls.Add(this.btnRefreshFromSteam);
			this.gbInfo.Controls.Add(this.lbModSteamID);
			this.gbInfo.Controls.Add(this.lbLastUpdateDate);
			this.gbInfo.Controls.Add(point1);
			this.gbInfo.Controls.Add(size1);
			this.gbInfo.Controls.Add(label1);
			this.gbInfo.Controls.Add(this.cbVisiblity);
			this.gbInfo.Controls.Add(this.linkSteamWorkshopAgreement);
			this.gbInfo.Controls.Add(size);
			this.gbInfo.Controls.Add(this.btnUpload);
			this.gbInfo.Location = new Point(181, 330);
			this.gbInfo.Name = "gbInfo";
			this.gbInfo.Size = new System.Drawing.Size(488, 142);
			this.gbInfo.TabIndex = 7;
			this.gbInfo.TabStop = false;
			this.gbInfo.Text = "信息";
			this.txtChangeNotes.Location = new Point(6, 32);
			this.txtChangeNotes.MaxLength = 8000;
			this.txtChangeNotes.Multiline = true;
			this.txtChangeNotes.Name = "txtChangeNotes";
			this.txtChangeNotes.ScrollBars = ScrollBars.Vertical;
			this.txtChangeNotes.Size = new System.Drawing.Size(214, 104);
			this.txtChangeNotes.TabIndex = 20;
			this.btnClearChangeNotes.BackgroundImage = Resources.ClearWindowContent_16x;
			this.btnClearChangeNotes.BackgroundImageLayout = ImageLayout.Zoom;
			this.btnClearChangeNotes.FlatAppearance.BorderSize = 0;
			this.btnClearChangeNotes.Location = new Point(200, 10);
			this.btnClearChangeNotes.Name = "btnClearChangeNotes";
			this.btnClearChangeNotes.Size = new System.Drawing.Size(20, 20);
			this.btnClearChangeNotes.TabIndex = 1;
			this.toolTipInfo.SetToolTip(this.btnClearChangeNotes, "清除更新说明");
			this.btnClearChangeNotes.UseVisualStyleBackColor = true;
			this.btnClearChangeNotes.Click += new EventHandler(this.btnClearChangeNotes_Click);
			this.btnRefreshFromSteam.BackgroundImage = Resources.Refresh_grey;
			this.btnRefreshFromSteam.BackgroundImageLayout = ImageLayout.Zoom;
			this.btnRefreshFromSteam.FlatAppearance.BorderSize = 0;
			this.btnRefreshFromSteam.Location = new Point(458, 10);
			this.btnRefreshFromSteam.Name = "btnRefreshFromSteam";
			this.btnRefreshFromSteam.Size = new System.Drawing.Size(24, 24);
			this.btnRefreshFromSteam.TabIndex = 1;
			this.toolTipInfo.SetToolTip(this.btnRefreshFromSteam, "从steam更新MOD信息");
			this.btnRefreshFromSteam.UseVisualStyleBackColor = true;
			this.btnRefreshFromSteam.Click += new EventHandler(this.btnRefreshFromSteam_Click);
			this.lbModSteamID.Location = new Point(310, 16);
			this.lbModSteamID.Name = "lbModSteamID";
			this.lbModSteamID.Size = new System.Drawing.Size(142, 14);
			this.lbModSteamID.TabIndex = 19;
			this.lbModSteamID.Text = "123456789";
			this.lbModSteamID.TextAlign = ContentAlignment.TopRight;
			this.lbLastUpdateDate.Location = new Point(298, 48);
			this.lbLastUpdateDate.Name = "lbLastUpdateDate";
			this.lbLastUpdateDate.Size = new System.Drawing.Size(184, 13);
			this.lbLastUpdateDate.TabIndex = 19;
			this.lbLastUpdateDate.Text = "-";
			this.lbLastUpdateDate.TextAlign = ContentAlignment.TopRight;
			this.cbVisiblity.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cbVisiblity.Items.AddRange(new object[] { "Public", "Friends Only", "Private" });
			this.cbVisiblity.Location = new Point(361, 73);
			this.cbVisiblity.MaxDropDownItems = 3;
			this.cbVisiblity.Name = "cbVisiblity";
			this.cbVisiblity.Size = new System.Drawing.Size(121, 21);
			this.cbVisiblity.TabIndex = 6;
			this.cbVisiblity.SelectedIndexChanged += new EventHandler(this.cbVisiblity_SelectedIndexChanged);
			this.linkSteamWorkshopAgreement.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.linkSteamWorkshopAgreement.LinkArea = new LinkArea(51, 67);
			this.linkSteamWorkshopAgreement.Location = new Point(226, 104);
			this.linkSteamWorkshopAgreement.Name = "linkSteamWorkshopAgreement";
			this.linkSteamWorkshopAgreement.Size = new System.Drawing.Size(175, 32);
			this.linkSteamWorkshopAgreement.TabIndex = 18;
			this.linkSteamWorkshopAgreement.TabStop = true;
			this.linkSteamWorkshopAgreement.Text = "提交此项目即表示您同意steam的服务条款";
			this.linkSteamWorkshopAgreement.TextAlign = ContentAlignment.BottomLeft;
			this.linkSteamWorkshopAgreement.UseCompatibleTextRendering = true;
			this.linkSteamWorkshopAgreement.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkSteamWorkshopAgreement_LinkClicked);
			this.btnUpload.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.btnUpload.Location = new Point(407, 113);
			this.btnUpload.Name = "btnUpload";
			this.btnUpload.Size = new System.Drawing.Size(75, 23);
			this.btnUpload.TabIndex = 7;
			this.btnUpload.Text = "上传";
			this.btnUpload.UseVisualStyleBackColor = true;
			this.btnUpload.Click += new EventHandler(this.btnUpload_Click);
			this.ssUpload.Items.AddRange(new ToolStripItem[] { this.pbUpload, this.lbUploadStatus, this.lbStatus });
			this.ssUpload.Location = new Point(0, 476);
			this.ssUpload.Name = "ssUpload";
			this.ssUpload.Size = new System.Drawing.Size(681, 22);
			this.ssUpload.TabIndex = 19;
			this.pbUpload.Name = "pbUpload";
			this.pbUpload.Size = new System.Drawing.Size(100, 16);
			this.pbUpload.Style = ProgressBarStyle.Continuous;
			this.lbUploadStatus.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.lbUploadStatus.Name = "lbUploadStatus";
			this.lbUploadStatus.Size = new System.Drawing.Size(98, 17);
			this.lbUploadStatus.Text = "0 Bytes of 0 Bytes";
			this.lbStatus.AutoSize = false;
			this.lbStatus.DisplayStyle = ToolStripItemDisplayStyle.Text;
			this.lbStatus.Name = "lbStatus";
			this.lbStatus.Size = new System.Drawing.Size(466, 17);
			this.lbStatus.Spring = true;
			this.lbStatus.Text = "已连接";
			this.lbStatus.TextAlign = ContentAlignment.MiddleRight;
			this.btnOpenFolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnOpenFolder.BackgroundImage = Resources.OpenFolder;
			this.btnOpenFolder.BackgroundImageLayout = ImageLayout.Zoom;
			this.btnOpenFolder.FlatAppearance.BorderSize = 0;
			this.btnOpenFolder.Location = new Point(367, 8);
			this.btnOpenFolder.Name = "btnOpenFolder";
			this.btnOpenFolder.Size = new System.Drawing.Size(28, 26);
			this.btnOpenFolder.TabIndex = 1;
			this.toolTipInfo.SetToolTip(this.btnOpenFolder, "打开MOD的目录");
			this.btnOpenFolder.UseVisualStyleBackColor = true;
			this.btnOpenFolder.Click += new EventHandler(this.btnOpenFolder_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(681, 498);
			base.Controls.Add(this.btnOpenFolder);
			base.Controls.Add(this.ssUpload);
			base.Controls.Add(this.gbInfo);
			base.Controls.Add(this.txtVersion);
			base.Controls.Add(this.gbTags);
			base.Controls.Add(this.gbImage);
			base.Controls.Add(this.gbDependencies);
			base.Controls.Add(this.gbDescription);
			base.Controls.Add(point);
			base.Controls.Add(this.txtTitle);
			base.Controls.Add(font);
			base.Controls.Add(this.txtAuthor);
			base.Controls.Add(label);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ModInfo";
			base.ShowIcon = false;
			this.Text = "信息";
			base.Activated += new EventHandler(this.ModInfo_Activated);
			base.FormClosing += new FormClosingEventHandler(this.ModInfo_FormClosing);
			this.gbDescription.ResumeLayout(false);
			this.gbDescription.PerformLayout();
			this.gbDependencies.ResumeLayout(false);
			this.gbImage.ResumeLayout(false);
			((ISupportInitialize)this.imgPreview).EndInit();
			this.gbTags.ResumeLayout(false);
			this.gbInfo.ResumeLayout(false);
			this.gbInfo.PerformLayout();
			this.ssUpload.ResumeLayout(false);
			this.ssUpload.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void linkSteamWorkshopAgreement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SteamManager.Instance.ShowSteamWorkshopAgreement();
		}

		private void listTags_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.noEvents)
			{
				return;
			}
			List<string> strs = new List<string>(this.listTags.CheckedItems.Count);
			string empty = string.Empty;
			foreach (object checkedItem in this.listTags.CheckedItems)
			{
				strs.Add(checkedItem.ToString());
			}
			if (this.info != null)
			{
				this.info.tags.Clear();
				this.info.tags.AddRange(strs);
			}
		}

		private void ModInfo_Activated(object sender, EventArgs e)
		{
			this.imgPreview.ImageLocation = string.Empty;
			this.info = null;
			this.updateEnabled = SteamManager.Instance.Enabled;
			this.btnUpload.Enabled = SteamManager.Instance.Enabled;
			this.btnRefreshFromSteam.Enabled = (!SteamManager.Instance.Enabled || this.info == null ? false : this.info.id != (long)0);
			this.lbStatus.Text = (SteamManager.Instance.Enabled ? "已连接" : "未连接到Steam");
			if (this.Data != null)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.Data.activeFileName);
				string modPath = this.parentForm.nav.ModPath;
				this.info = new ModData(this.Data.header, modPath, fileNameWithoutExtension);
				this.info.Load();
				this.refreshModUIData();
			}
		}

		private void ModInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.updateEnabled = false;
			if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
			{
				if (this.info != null)
				{
					this.info.Save();
				}
				e.Cancel = true;
				base.Hide();
			}
		}

		private void OnModUpdateComplete(bool success)
		{
			if (this.info != null & success)
			{
				this.info.Save();
			}
			base.BeginInvoke(new MethodInvoker(() => {
				if (!success)
				{
					MessageBox.Show("或者MOD信息错误.", "错误", MessageBoxButtons.OK);
				}
				else
				{
					if (this.txtDescription.Text != this.info.header.Description)
					{
						this.parentForm.nav.HasChanges = true;
					}
					this.refreshModUIData();
				}
				this.btnRefreshFromSteam.Enabled = true;
				this.btnUpload.Enabled = true;
			}));
		}

		private void OnUploadComplete(bool success, string message)
		{
			if (!success)
			{
				MessageBox.Show(message, "上传MOD错误", MessageBoxButtons.OK);
			}
			this.updateUploadThread.Join();
			this.updateUploadThread = null;
			if (this.info != null)
			{
				this.info.lastUpdate = new DateTime?(DateTime.Now);
				this.info.Save();
			}
			base.BeginInvoke(new MethodInvoker(() => {
				this.lbStatus.Text = (success ? "已完成" : "错误");
				this.pbUpload.Value = 0;
				this.pbUpload.Visible = false;
				this.lbUploadStatus.Visible = false;
				this.lbLastUpdateDate.Text = this.info.lastUpdate.Value.ToString();
				this.lbModSteamID.Text = this.info.id.ToString();
				this.btnRefreshFromSteam.Enabled = this.info.id != (long)0;
				this.uploading = false;
				this.btnUpload.Enabled = true;
				this.txtChangeNotes.Enabled = true;
			}));
		}

		public void refreshDependencies()
		{
			if (this.info != null)
			{
				this.Text = string.Concat(this.info.mod, " - Info");
			}
			this.Data.header.Dependencies = this.Data.getRequiredMods();
			this.Data.header.Referenced = this.Data.getReferencedMods();
			this.listDependencies.Items.Clear();
			foreach (string dependency in this.Data.header.Dependencies)
			{
				this.listDependencies.Items.Add(dependency);
			}
			foreach (string referenced in this.Data.header.Referenced)
			{
				if (this.Data.header.Dependencies.Contains(referenced))
				{
					continue;
				}
				this.listDependencies.Items.Add(string.Concat("(", referenced, ")"));
			}
		}

		private void refreshModUIData()
		{
			if (this.info != null)
			{
				this.noEvents = true;
				this.txtAuthor.Text = this.info.header.Author;
				this.txtDescription.Text = this.info.header.Description;
				this.txtVersion.Text = this.info.header.Version.ToString();
				for (int i = 0; i < this.listTags.Items.Count; i++)
				{
					this.listTags.SetItemChecked(i, false);
				}
				this.imgPreview.ImageLocation = this.info.GetImagePath();
				this.txtTitle.Text = this.info.title;
				foreach (string tag in this.info.tags)
				{
					this.listTags.SetItemChecked(this.listTags.Items.IndexOf(tag), true);
				}
				this.cbVisiblity.SelectedIndex = this.info.visibility;
				if (!this.info.lastUpdate.HasValue)
				{
					this.lbLastUpdateDate.Text = "-";
				}
				else
				{
					this.lbLastUpdateDate.Text = this.info.lastUpdate.Value.ToString();
				}
				this.lbModSteamID.Text = this.info.id.ToString();
				this.btnRefreshFromSteam.Enabled = (this.info.id == 0 ? false : SteamManager.Instance.Enabled);
				this.noEvents = false;
				this.listTags_SelectedIndexChanged(null, null);
				this.refreshDependencies();
			}
		}

		private void txtAuthor_TextChanged(object sender, EventArgs e)
		{
			if (this.noEvents)
			{
				return;
			}
			this.parentForm.nav.HasChanges = true;
			this.Data.header.Author = this.txtAuthor.Text;
		}

		private void txtDescription_TextChanged(object sender, EventArgs e)
		{
			if (this.noEvents)
			{
				return;
			}
			this.parentForm.nav.HasChanges = true;
			this.Data.header.Description = this.txtDescription.Text;
		}

		private void txtTitle_TextChanged(object sender, EventArgs e)
		{
			if (this.noEvents)
			{
				return;
			}
			if (this.info != null)
			{
				this.info.title = this.txtTitle.Text;
			}
		}

		private void txtVersion_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = (char.IsControl(e.KeyChar) ? false : !char.IsDigit(e.KeyChar));
		}

		private void txtVersion_TextChanged(object sender, EventArgs e)
		{
			if (this.noEvents)
			{
				return;
			}
			if (int.TryParse(this.txtVersion.Text, out this.Data.header.Version))
			{
				this.parentForm.nav.HasChanges = true;
				return;
			}
			this.txtVersion.Text = this.Data.header.Version.ToString();
		}

		private void updateUploadState(SteamManager.UploadState state, ulong processedBytes, ulong totalBytes)
		{
			if (SteamManager.Instance.Uploading && state != SteamManager.UploadState.None)
			{
				this.pbUpload.Visible = true;
				if (totalBytes <= (long)0)
				{
					this.pbUpload.Value = 0;
					this.lbUploadStatus.Visible = false;
				}
				else
				{
					this.pbUpload.Value = (int)Math.Round(100 * ((double)((float)processedBytes) / (double)((float)totalBytes)));
					this.lbUploadStatus.Text = string.Format("{0,0:0.##} KBs of {1,0:0.##} KBs", (double)((float)processedBytes) / 1024, (double)((float)totalBytes) / 1024);
					this.lbUploadStatus.Visible = true;
				}
				this.lbStatus.Text = (state == SteamManager.UploadState.Creating ? "正在创建..." : "正在上传...");
			}
		}
	}
}