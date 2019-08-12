using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class NavigationTranslation : Form
	{
		private SolidBrush BrushNormal = new SolidBrush(SystemColors.ControlText);

		private SolidBrush BrushModified = new SolidBrush(Color.Blue);

		private string contentFilter;

		private IContainer components;

		private SplitContainer splitContainer1;

		private TreeView treeCategories;

		private ListBox listItems;

		private TextBox txtSearch;

		private System.Windows.Forms.ContextMenuStrip contextMenu;

		private ToolStripMenuItem newWordSwap;

		private ToolStripMenuItem deleteWordSwap;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem filterTranslated;

		private ToolStripMenuItem filterUntranslated;

		private ToolStripMenuItem filterUpdated;

		public NavigationTranslation()
		{
			this.InitializeComponent();
			this.treeCategories.Nodes.Add("Dialogue", "Dialogue").Tag = itemType.DIALOGUE;
			this.treeCategories.Nodes.Add("Word Swaps", "Word Swaps").Tag = itemType.WORD_SWAPS;
		}

		public static bool contains(string pattern, string text)
		{
			return CultureInfo.InvariantCulture.CompareInfo.IndexOf(text, pattern, CompareOptions.IgnoreCase) >= 0;
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			if ((itemType)this.treeCategories.SelectedNode.Tag != itemType.WORD_SWAPS)
			{
				this.deleteWordSwap.Visible = false;
				this.newWordSwap.Visible = false;
				this.toolStripSeparator1.Visible = false;
				return;
			}
			this.toolStripSeparator1.Visible = true;
			this.deleteWordSwap.Visible = true;
			this.newWordSwap.Visible = true;
			TranslationManager.TranslationDialogue selectedItem = this.listItems.SelectedItem as TranslationManager.TranslationDialogue;
			this.deleteWordSwap.Enabled = (selectedItem == null ? false : selectedItem.Item.getState() == GameData.State.OWNED);
		}

		private void deleteWordSwap_Click(object sender, EventArgs e)
		{
			baseForm mdiParent = base.MdiParent as baseForm;
			TranslationManager.TranslationDialogue selectedItem = this.listItems.SelectedItem as TranslationManager.TranslationDialogue;
			mdiParent.nav.ou.gameData.deleteItem(selectedItem.Item);
			TranslationManager.EnabledWordSwaps.Remove(selectedItem);
			this.listItems.Items.Remove(selectedItem);
			this.RefreshList();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void filterTranslated_Click(object sender, EventArgs e)
		{
			this.UpdateList(this.txtSearch.Text);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.splitContainer1 = new SplitContainer();
			this.treeCategories = new TreeView();
			this.txtSearch = new TextBox();
			this.listItems = new ListBox();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.newWordSwap = new ToolStripMenuItem();
			this.deleteWordSwap = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.filterTranslated = new ToolStripMenuItem();
			this.filterUntranslated = new ToolStripMenuItem();
			this.filterUpdated = new ToolStripMenuItem();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.contextMenu.SuspendLayout();
			base.SuspendLayout();
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.treeCategories);
			this.splitContainer1.Panel2.Controls.Add(this.txtSearch);
			this.splitContainer1.Panel2.Controls.Add(this.listItems);
			this.splitContainer1.Size = new System.Drawing.Size(506, 622);
			this.splitContainer1.SplitterDistance = 186;
			this.splitContainer1.TabIndex = 0;
			this.treeCategories.Dock = DockStyle.Fill;
			this.treeCategories.Location = new Point(0, 0);
			this.treeCategories.Name = "treeCategories";
			this.treeCategories.Size = new System.Drawing.Size(186, 622);
			this.treeCategories.TabIndex = 0;
			this.treeCategories.AfterSelect += new TreeViewEventHandler(this.treeCategories_AfterSelect);
			this.txtSearch.Dock = DockStyle.Bottom;
			this.txtSearch.Location = new Point(0, 602);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(316, 20);
			this.txtSearch.TabIndex = 1;
			this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
			this.listItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.listItems.ContextMenuStrip = this.contextMenu;
			this.listItems.DrawMode = DrawMode.OwnerDrawFixed;
			this.listItems.IntegralHeight = false;
			this.listItems.Items.AddRange(new object[] { "" });
			this.listItems.Location = new Point(0, 0);
			this.listItems.Name = "listItems";
			this.listItems.Size = new System.Drawing.Size(316, 601);
			this.listItems.Sorted = true;
			this.listItems.TabIndex = 0;
			this.listItems.DrawItem += new DrawItemEventHandler(this.listItems_DrawItem);
			this.listItems.DoubleClick += new EventHandler(this.listItems_DoubleClick);
			this.listItems.MouseDown += new MouseEventHandler(this.listItems_MouseDown);
			this.contextMenu.Items.AddRange(new ToolStripItem[] { this.newWordSwap, this.deleteWordSwap, this.toolStripSeparator1, this.filterTranslated, this.filterUntranslated, this.filterUpdated });
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(153, 142);
			this.contextMenu.Opening += new CancelEventHandler(this.contextMenu_Opening);
			this.newWordSwap.Name = "newWordSwap";
			this.newWordSwap.Size = new System.Drawing.Size(152, 22);
			this.newWordSwap.Text = "New";
			this.newWordSwap.Click += new EventHandler(this.newWordSwap_Click);
			this.deleteWordSwap.Name = "deleteWordSwap";
			this.deleteWordSwap.Size = new System.Drawing.Size(152, 22);
			this.deleteWordSwap.Text = "Delete";
			this.deleteWordSwap.Click += new EventHandler(this.deleteWordSwap_Click);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
			this.filterTranslated.Checked = true;
			this.filterTranslated.CheckOnClick = true;
			this.filterTranslated.CheckState = CheckState.Checked;
			this.filterTranslated.Name = "filterTranslated";
			this.filterTranslated.Size = new System.Drawing.Size(152, 22);
			this.filterTranslated.Text = "Translated";
			this.filterTranslated.Click += new EventHandler(this.filterTranslated_Click);
			this.filterUntranslated.Checked = true;
			this.filterUntranslated.CheckOnClick = true;
			this.filterUntranslated.CheckState = CheckState.Checked;
			this.filterUntranslated.Name = "filterUntranslated";
			this.filterUntranslated.Size = new System.Drawing.Size(152, 22);
			this.filterUntranslated.Text = "Untranslated";
			this.filterUntranslated.Click += new EventHandler(this.filterTranslated_Click);
			this.filterUpdated.Checked = true;
			this.filterUpdated.CheckOnClick = true;
			this.filterUpdated.CheckState = CheckState.Checked;
			this.filterUpdated.Name = "filterUpdated";
			this.filterUpdated.Size = new System.Drawing.Size(152, 22);
			this.filterUpdated.Text = "Updated";
			this.filterUpdated.Click += new EventHandler(this.filterTranslated_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(506, 622);
			base.ControlBox = false;
			base.Controls.Add(this.splitContainer1);
			base.Name = "NavigationTranslation";
			this.Text = "Translation";
			base.VisibleChanged += new EventHandler(this.NavigationTranslation_VisibleChanged);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.contextMenu.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void listItems_DoubleClick(object sender, EventArgs e)
		{
			TranslationManager.TranslationDialogue selectedItem = this.listItems.SelectedItem as TranslationManager.TranslationDialogue;
			if (selectedItem != null)
			{
				baseForm mdiParent = base.MdiParent as baseForm;
				if (selectedItem.Item.getState() == GameData.State.OWNED)
				{
					mdiParent.nav.showItemProperties(selectedItem.Item);
					return;
				}
				DialogTranslation dialogTranslation = new DialogTranslation(this, mdiParent.nav, selectedItem)
				{
					MdiParent = base.MdiParent
				};
				if (this.contentFilter != null)
				{
					dialogTranslation.findLine(this.contentFilter);
				}
				dialogTranslation.Show();
			}
		}

		private void listItems_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			TranslationManager.TranslationDialogue item = this.listItems.Items[e.Index] as TranslationManager.TranslationDialogue;
			Color controlText = SystemColors.ControlText;
			switch (item.State)
			{
				case TranslationManager.DialogueTranslationState.NULL:
				{
					controlText = Color.Red;
					break;
				}
				case TranslationManager.DialogueTranslationState.NEW:
				{
					controlText = Color.Blue;
					break;
				}
				case TranslationManager.DialogueTranslationState.ORIGINAL_MODIFIED:
				{
					controlText = Color.DarkOrange;
					break;
				}
				case TranslationManager.DialogueTranslationState.OK:
				{
					controlText = SystemColors.ControlText;
					break;
				}
			}
			if (item.Item.getState() == GameData.State.OWNED)
			{
				controlText = Color.DarkGreen;
			}
			TextRenderer.DrawText(e.Graphics, item.ToString(), e.Font, e.Bounds, controlText, TextFormatFlags.Default);
			e.DrawFocusRectangle();
		}

		private void listItems_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				int num = this.listItems.IndexFromPoint(e.Location);
				if (num >= 0)
				{
					this.listItems.SelectedIndex = num;
				}
			}
		}

		private void NavigationTranslation_VisibleChanged(object sender, EventArgs e)
		{
			if (base.Visible)
			{
				this.treeCategories.SelectedNode = this.treeCategories.Nodes["Dialogue"];
			}
		}

		private void newWordSwap_Click(object sender, EventArgs e)
		{
			baseForm mdiParent = base.MdiParent as baseForm;
			GameData.Item item = mdiParent.nav.ou.gameData.createItem(itemType.WORD_SWAPS);
			TranslationManager.TranslationDialogue translationDialogue = new TranslationManager.TranslationDialogue(item);
			TranslationManager.EnabledWordSwaps.Add(translationDialogue);
			this.listItems.Items.Add(translationDialogue);
			mdiParent.nav.showItemProperties(item);
		}

		public void RefreshList()
		{
			this.listItems.Refresh();
		}

		public List<TranslationManager.TranslationDialogue> searchText(string text, List<TranslationManager.TranslationDialogue> source)
		{
			List<TranslationManager.TranslationDialogue> translationDialogues = new List<TranslationManager.TranslationDialogue>();
		Label0:
			foreach (TranslationManager.TranslationDialogue translationDialogue in source)
			{
				bool flag = false;
			Label1:
				foreach (TranslationManager.TranslationDialogueLine line in translationDialogue.Lines)
				{
					if (!flag)
					{
						foreach (TranslationManager.TranslationDialogueLine.Line line1 in line.Lines)
						{
							if (!NavigationTranslation.contains(text, line1.Translation) && !NavigationTranslation.contains(text, line1.Original))
							{
								continue;
							}
							translationDialogues.Add(translationDialogue);
							flag = true;
							goto Label1;
						}
					}
					else
					{
						goto Label0;
					}
				}
			}
			return translationDialogues;
		}

		private void treeCategories_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.treeCategories.SelectedNode == null)
			{
				return;
			}
			this.txtSearch.Clear();
			this.UpdateList("");
		}

		private void txtSearch_TextChanged(object sender, EventArgs e)
		{
			this.UpdateList(this.txtSearch.Text);
		}

		public void UpdateList(string filter = "")
		{
			this.contentFilter = null;
			List<TranslationManager.TranslationDialogue> translationDialogues = null;
			List<TranslationManager.TranslationDialogue> translationDialogues1 = ((itemType)this.treeCategories.SelectedNode.Tag == itemType.DIALOGUE ? TranslationManager.EnabledDialogues : TranslationManager.EnabledWordSwaps);
			if (string.IsNullOrWhiteSpace(filter))
			{
				translationDialogues = translationDialogues1;
			}
			else if (!filter.StartsWith("text:", true, CultureInfo.InvariantCulture))
			{
				translationDialogues = translationDialogues1.FindAll((TranslationManager.TranslationDialogue i) => {
					if (NavigationTranslation.contains(filter, i.Item.Name))
					{
						return true;
					}
					return NavigationTranslation.contains(filter, i.Item.stringID);
				});
			}
			else
			{
				this.contentFilter = filter.Substring(5).Trim();
				translationDialogues = this.searchText(this.contentFilter, translationDialogues1);
			}
			if (!this.filterTranslated.Checked)
			{
				translationDialogues = translationDialogues.FindAll((TranslationManager.TranslationDialogue i) => i.State != TranslationManager.DialogueTranslationState.OK);
			}
			if (!this.filterUntranslated.Checked)
			{
				translationDialogues = translationDialogues.FindAll((TranslationManager.TranslationDialogue i) => i.State != TranslationManager.DialogueTranslationState.NEW);
			}
			if (!this.filterUpdated.Checked)
			{
				translationDialogues = translationDialogues.FindAll((TranslationManager.TranslationDialogue i) => i.State != TranslationManager.DialogueTranslationState.ORIGINAL_MODIFIED);
			}
			this.listItems.BeginUpdate();
			this.listItems.Items.Clear();
			foreach (TranslationManager.TranslationDialogue translationDialogue in translationDialogues)
			{
				this.listItems.Items.Add(translationDialogue);
			}
			this.listItems.EndUpdate();
		}
	}
}