using forgotten_construction_set.Components;
using forgotten_construction_set.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class TranslationFix : Form
	{
		private GameData Data;

		private int doneCount;

		private bool freezeTextEvents;

		private IContainer components;

		private GroupBox groupBox1;

		private Button loadB;

		private Button loadA;

		private TextBox fileB;

		private TextBox fileA;

		private ListView listView;

		private ColumnHeader Original;

		private ColumnHeader A;

		private ColumnHeader B;

		private SplitContainer splitContainer1;

		private OpenFileDialog openFile;

		private Label itemKey;

		private Label itemName;

		private Label itemID;

		private Label itmType;

		private Label stats;

		private TextBox textB;

		private TextBox textA;

		private SplitContainer splitContainer2;

		private Button save;

		private ToolTip toolTip1;

		private SaveFileDialog saveFile;

		private navigation nav
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
			}
		}

		public TranslationFix()
		{
			this.InitializeComponent();
			this.listView.EnableDoubleBuferring();
		}

		private void advance()
		{
			if (this.listView.SelectedIndices.Count == 0)
			{
				return;
			}
			int item = this.listView.SelectedIndices[0];
			if (item >= this.listView.Items.Count - 1)
			{
				return;
			}
			this.listView.Items[item + 1].Selected = true;
			this.listView.Items[item + 1].EnsureVisible();
			this.listView.FocusedItem = this.listView.Items[item + 1];
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void export(string filename)
		{
			GameData gameDatum = new GameData();
			this.Cursor = Cursors.WaitCursor;
			navigation.ModFileMode fileMode = this.nav.FileMode;
			this.nav.FileMode = navigation.ModFileMode.SINGLE;
			gameDatum.load(this.fileA.Text, GameData.ModMode.ACTIVE, false);
			gameDatum.load(this.fileB.Text, GameData.ModMode.ACTIVE, false);
			foreach (object item in this.listView.Items)
			{
				TranslationFix.LineData tag = ((ListViewItem)item).Tag as TranslationFix.LineData;
				if (tag.@value == null)
				{
					continue;
				}
				GameData.Item item1 = gameDatum.getItem(tag.item.stringID);
				if (tag.key != null)
				{
					item1[tag.key] = tag.@value;
				}
				else
				{
					item1.Name = tag.@value;
				}
			}
			gameDatum.save(filename);
			this.nav.FileMode = fileMode;
			this.Cursor = Cursors.Default;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new GroupBox();
			this.save = new Button();
			this.loadB = new Button();
			this.loadA = new Button();
			this.fileB = new TextBox();
			this.fileA = new TextBox();
			this.listView = new ListView();
			this.Original = new ColumnHeader();
			this.A = new ColumnHeader();
			this.B = new ColumnHeader();
			this.splitContainer1 = new SplitContainer();
			this.splitContainer2 = new SplitContainer();
			this.textA = new TextBox();
			this.textB = new TextBox();
			this.stats = new Label();
			this.itmType = new Label();
			this.itemKey = new Label();
			this.itemName = new Label();
			this.itemID = new Label();
			this.openFile = new OpenFileDialog();
			this.toolTip1 = new ToolTip(this.components);
			this.saveFile = new SaveFileDialog();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((ISupportInitialize)this.splitContainer2).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Controls.Add(this.save);
			this.groupBox1.Controls.Add(this.loadB);
			this.groupBox1.Controls.Add(this.loadA);
			this.groupBox1.Controls.Add(this.fileB);
			this.groupBox1.Controls.Add(this.fileA);
			this.groupBox1.Dock = DockStyle.Top;
			this.groupBox1.Location = new Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1298, 75);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "输入文件";
			this.save.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.save.Image = Resources.Save;
			this.save.Location = new Point(1257, 22);
			this.save.Name = "save";
			this.save.Size = new System.Drawing.Size(29, 30);
			this.save.TabIndex = 4;
			this.save.Text = "...";
			this.toolTip1.SetToolTip(this.save, "导出合并文件");
			this.save.UseVisualStyleBackColor = true;
			this.save.Click += new EventHandler(this.save_Click);
			this.loadB.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.loadB.Location = new Point(1198, 47);
			this.loadB.Name = "loadB";
			this.loadB.Size = new System.Drawing.Size(29, 21);
			this.loadB.TabIndex = 3;
			this.loadB.Text = "...";
			this.toolTip1.SetToolTip(this.loadB, "浏览文件 B");
			this.loadB.UseVisualStyleBackColor = true;
			this.loadB.Click += new EventHandler(this.loadB_Click);
			this.loadA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.loadA.Location = new Point(1198, 22);
			this.loadA.Name = "loadA";
			this.loadA.Size = new System.Drawing.Size(29, 22);
			this.loadA.TabIndex = 2;
			this.loadA.Text = "...";
			this.toolTip1.SetToolTip(this.loadA, "浏览文件 A");
			this.loadA.UseVisualStyleBackColor = true;
			this.loadA.Click += new EventHandler(this.loadA_Click);
			this.fileB.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.fileB.Location = new Point(11, 48);
			this.fileB.Name = "fileB";
			this.fileB.Size = new System.Drawing.Size(1181, 20);
			this.fileB.TabIndex = 1;
			this.fileA.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.fileA.Location = new Point(11, 22);
			this.fileA.Name = "fileA";
			this.fileA.Size = new System.Drawing.Size(1181, 20);
			this.fileA.TabIndex = 0;
			this.listView.Columns.AddRange(new ColumnHeader[] { this.Original, this.A, this.B });
			this.listView.Dock = DockStyle.Fill;
			this.listView.FullRowSelect = true;
			this.listView.HideSelection = false;
			this.listView.Location = new Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(1298, 314);
			this.listView.TabIndex = 1;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = View.Details;
			this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
			this.listView.DoubleClick += new EventHandler(this.listView_DoubleClick);
			this.listView.PreviewKeyDown += new PreviewKeyDownEventHandler(this.listView_PreviewKeyDown);
			this.Original.Text = "原始的";
			this.Original.Width = 308;
			this.A.Text = "A";
			this.A.Width = 484;
			this.B.Text = "B";
			this.B.Width = 485;
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 75);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = Orientation.Horizontal;
			this.splitContainer1.Panel1.Controls.Add(this.listView);
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Panel2.Controls.Add(this.stats);
			this.splitContainer1.Panel2.Controls.Add(this.itmType);
			this.splitContainer1.Panel2.Controls.Add(this.itemKey);
			this.splitContainer1.Panel2.Controls.Add(this.itemName);
			this.splitContainer1.Panel2.Controls.Add(this.itemID);
			this.splitContainer1.Size = new System.Drawing.Size(1298, 485);
			this.splitContainer1.SplitterDistance = 314;
			this.splitContainer1.TabIndex = 2;
			this.splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.splitContainer2.Location = new Point(11, 16);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = Orientation.Horizontal;
			this.splitContainer2.Panel1.Controls.Add(this.textA);
			this.splitContainer2.Panel2.Controls.Add(this.textB);
			this.splitContainer2.Size = new System.Drawing.Size(1275, 148);
			this.splitContainer2.SplitterDistance = 74;
			this.splitContainer2.TabIndex = 7;
			this.textA.Dock = DockStyle.Fill;
			this.textA.Location = new Point(0, 0);
			this.textA.Multiline = true;
			this.textA.Name = "textA";
			this.textA.Size = new System.Drawing.Size(1275, 74);
			this.textA.TabIndex = 5;
			this.textA.TextChanged += new EventHandler(this.textA_TextChanged);
			this.textB.Dock = DockStyle.Fill;
			this.textB.Location = new Point(0, 0);
			this.textB.Multiline = true;
			this.textB.Name = "textB";
			this.textB.Size = new System.Drawing.Size(1275, 70);
			this.textB.TabIndex = 6;
			this.textB.TextChanged += new EventHandler(this.textB_TextChanged);
			this.stats.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.stats.Location = new Point(1159, 0);
			this.stats.Name = "stats";
			this.stats.Size = new System.Drawing.Size(127, 13);
			this.stats.TabIndex = 4;
			this.stats.Text = "状态";
			this.stats.TextAlign = ContentAlignment.TopRight;
			this.itmType.AutoSize = true;
			this.itmType.Location = new Point(193, 0);
			this.itmType.Name = "itmType";
			this.itmType.Size = new System.Drawing.Size(44, 13);
			this.itmType.TabIndex = 3;
			this.itmType.Text = "项目类型";
			this.itemKey.AutoSize = true;
			this.itemKey.Location = new Point(589, 0);
			this.itemKey.Name = "itemKey";
			this.itemKey.Size = new System.Drawing.Size(44, 13);
			this.itemKey.TabIndex = 2;
			this.itemKey.Text = "项目Key";
			this.itemName.AutoSize = true;
			this.itemName.Location = new Point(380, -1);
			this.itemName.Name = "itemName";
			this.itemName.Size = new System.Drawing.Size(54, 13);
			this.itemName.TabIndex = 1;
			this.itemName.Text = "项目名称";
			this.itemID.AutoSize = true;
			this.itemID.Location = new Point(8, 0);
			this.itemID.Name = "itemID";
			this.itemID.Size = new System.Drawing.Size(37, 13);
			this.itemID.TabIndex = 0;
			this.itemID.Text = "项目ID";
			this.openFile.FileName = "openFileDialog1";
			this.openFile.Filter = "Translations|*.translation";
			this.saveFile.DefaultExt = "translation";
			this.saveFile.Filter = "Translation files|*.translation";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1298, 560);
			base.Controls.Add(this.splitContainer1);
			base.Controls.Add(this.groupBox1);
			base.MinimizeBox = false;
			base.Name = "TranslationFix";
			base.ShowIcon = false;
			this.Text = "翻译修复";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer2).EndInit();
			this.splitContainer2.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void listView_DoubleClick(object sender, EventArgs e)
		{
			if (this.listView.SelectedItems.Count == 0)
			{
				return;
			}
			string tag = (this.listView.SelectedItems[0].Tag as TranslationFix.LineData).item.stringID;
			GameData.Item item = this.nav.ou.gameData.getItem(tag);
			if (item != null)
			{
				this.nav.showItemProperties(item);
			}
		}

		private void listView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			if (this.listView.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem item = this.listView.SelectedItems[0];
			TranslationFix.LineData tag = item.Tag as TranslationFix.LineData;
			if (e.KeyCode == Keys.Left)
			{
				if (tag.state == TranslationFix.State.UNKNOWN)
				{
					this.doneCount++;
				}
				item.SubItems[0].ForeColor = Color.Green;
				tag.@value = item.SubItems[1].Text;
				tag.state = TranslationFix.State.ORIGINAL;
				this.advance();
			}
			else if (e.KeyCode == Keys.Right)
			{
				if (tag.state == TranslationFix.State.UNKNOWN)
				{
					this.doneCount++;
				}
				item.SubItems[0].ForeColor = Color.Blue;
				tag.@value = item.SubItems[2].Text;
				tag.state = TranslationFix.State.CHANGED;
				this.advance();
			}
			this.listView_SelectedIndexChanged(null, null);
			Label label = this.stats;
			string str = this.doneCount.ToString();
			int count = this.listView.Items.Count;
			label.Text = string.Concat(str, " / ", count.ToString());
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListViewItem item;
			if (this.listView.SelectedItems.Count > 0)
			{
				item = this.listView.SelectedItems[0];
			}
			else
			{
				item = null;
			}
			ListViewItem listViewItem = item;
			if (listViewItem == null)
			{
				this.itemID.Text = "";
				this.itmType.Text = "";
				this.itemName.Text = "";
				this.itemKey.Text = "";
				TextBox textBox = this.textA;
				string str = "";
				string str1 = str;
				this.textB.Text = str;
				textBox.Text = str1;
				this.textA.BackColor = SystemColors.Window;
				this.textB.BackColor = SystemColors.Window;
				return;
			}
			TranslationFix.LineData tag = listViewItem.Tag as TranslationFix.LineData;
			this.itemID.Text = tag.item.stringID;
			this.itmType.Text = tag.item.type.ToString();
			this.itemName.Text = tag.item.OriginalName;
			this.itemKey.Text = tag.key ?? "";
			this.freezeTextEvents = true;
			this.textA.Text = listViewItem.SubItems[1].Text;
			this.textB.Text = listViewItem.SubItems[2].Text;
			this.textA.BackColor = SystemColors.Window;
			this.textB.BackColor = SystemColors.Window;
			switch (tag.state)
			{
				case TranslationFix.State.ORIGINAL:
				{
					this.textA.BackColor = Color.FromArgb(240, 255, 240);
					break;
				}
				case TranslationFix.State.CHANGED:
				{
					this.textB.BackColor = Color.FromArgb(240, 255, 240);
					break;
				}
				case TranslationFix.State.CUSTOMA:
				{
					this.textA.BackColor = Color.FromArgb(255, 240, 230);
					this.textA.Text = tag.@value;
					break;
				}
				case TranslationFix.State.CUSTOMB:
				{
					this.textB.BackColor = Color.FromArgb(255, 240, 230);
					this.textB.Text = tag.@value;
					break;
				}
			}
			this.freezeTextEvents = false;
		}

		private void loadA_Click(object sender, EventArgs e)
		{
			if (this.openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.fileA.Text = this.openFile.FileName;
				this.loadData();
			}
		}

		private void loadB_Click(object sender, EventArgs e)
		{
			if (this.openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.fileB.Text = this.openFile.FileName;
				this.loadData();
			}
		}

		private void loadData()
		{
			string str;
			this.Data = new GameData();
			if ((this.Data.load(this.fileA.Text, GameData.ModMode.BASE, false) | this.Data.load(this.fileB.Text, GameData.ModMode.ACTIVE, false)))
			{
				GameData gameDatum = this.nav.ou.gameData;
				System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
				this.listView.Items.Clear();
				this.listView.BeginUpdate();
				foreach (GameData.Item value in this.Data.items.Values)
				{
					GameData.Item item = gameDatum.getItem(value.stringID);
					if (value.OriginalName != value.Name)
					{
						ListViewItem listViewItem = new ListViewItem((item != null ? item.OriginalName ?? item.Name : "??"));
						listViewItem.SubItems.Add(value.OriginalName);
						listViewItem.SubItems.Add(value.Name);
						this.listView.Items.Add(listViewItem);
						listViewItem.Tag = new TranslationFix.LineData(value, null);
					}
					if (value.getState() != GameData.State.MODIFIED)
					{
						continue;
					}
					foreach (KeyValuePair<string, object> keyValuePair in value)
					{
						if (value.type == itemType.DIALOGUE_LINE && keyValuePair.Key.StartsWith("_original_text") || value.getState(keyValuePair.Key) != GameData.State.MODIFIED)
						{
							continue;
						}
						if (item == null)
						{
							str = "??";
						}
						else
						{
							object obj = item.OriginalValue(keyValuePair.Key);
							if (obj == null)
							{
								obj = item.sdata[keyValuePair.Key];
							}
							str = obj as string;
						}
						ListViewItem lineDatum = new ListViewItem(str);
						lineDatum.SubItems.Add(value.OriginalValue(keyValuePair.Key) as string);
						lineDatum.SubItems.Add(keyValuePair.Value as string);
						this.listView.Items.Add(lineDatum);
						lineDatum.Tag = new TranslationFix.LineData(value, keyValuePair.Key);
					}
				}
				this.listView.EndUpdate();
				System.Windows.Forms.Cursor.Current = Cursors.Default;
				this.doneCount = 0;
				Label label = this.stats;
				string str1 = this.doneCount.ToString();
				int count = this.listView.Items.Count;
				label.Text = string.Concat(str1, " / ", count.ToString());
			}
		}

		private void save_Click(object sender, EventArgs e)
		{
			if (this.saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.export(this.saveFile.FileName);
			}
		}

		private void textA_TextChanged(object sender, EventArgs e)
		{
			if (this.freezeTextEvents)
			{
				return;
			}
			if (this.listView.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem item = this.listView.SelectedItems[0];
			TranslationFix.LineData tag = item.Tag as TranslationFix.LineData;
			tag.state = TranslationFix.State.CUSTOMA;
			tag.@value = this.textA.Text;
			item.SubItems[0].ForeColor = Color.DarkOrange;
			this.textA.BackColor = Color.FromArgb(255, 240, 230);
			this.textB.BackColor = SystemColors.Window;
		}

		private void textB_TextChanged(object sender, EventArgs e)
		{
			if (this.freezeTextEvents)
			{
				return;
			}
			if (this.listView.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem item = this.listView.SelectedItems[0];
			TranslationFix.LineData tag = item.Tag as TranslationFix.LineData;
			tag.state = TranslationFix.State.CUSTOMB;
			tag.@value = this.textB.Text;
			item.SubItems[0].ForeColor = Color.DarkOrange;
			this.textA.BackColor = SystemColors.Window;
			this.textB.BackColor = Color.FromArgb(255, 240, 230);
		}

		private class LineData
		{
			public GameData.Item item;

			public string key;

			public string @value;

			public TranslationFix.State state;

			public LineData(GameData.Item item, string key = null)
			{
				this.item = item;
				this.key = key;
			}
		}

		private enum State
		{
			UNKNOWN,
			ORIGINAL,
			CHANGED,
			CUSTOMA,
			CUSTOMB
		}
	}
}