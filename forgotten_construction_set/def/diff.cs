using forgotten_construction_set.Components;
using PropertyGrid;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Resources;
using System.Windows.Forms;

namespace forgotten_construction_set
{
    public class DialogTranslation : Form
    {
        private navigation nav;

        private NavigationTranslation navTranslation;

        public TranslationManager.TranslationDialogue item;

        private GameData gameData;

        private GameData.Item dialogueItem;

        private DialogTranslation.LineData currentLine;

        private TranslationManager.TranslationDialogueLine.Line selectedLine;

        private bool multipleSelected;

        private Color[] stateColors = new Color[] { SystemColors.GrayText, Color.Blue, Color.DarkOrange, SystemColors.ControlText };

        private IContainer components;

        private SplitContainer splitContainer1;

        private TextBox txtDialogName;

        private GroupBox gbConditions;

        private ListView lvConditions;

        private ColumnHeader columnWho;

        private ColumnHeader columnCondition;

        private ColumnHeader columnComp;

        private ColumnHeader columnValue;

        private ColumnHeader columnTag;

        private GroupBox gbEffects;

        private ListView lvEffects;

        private ColumnHeader columnEffects;

        private ColumnHeader columnEffectValue;

        private ReferenceList referenceList1;

        private SplitContainer splitContainer2;

        private PropertyGrid grid;

        private GroupBox gbInfo;

        private Label lbSpeaker;

        private Label lbTarget;

        private GroupBox gbActions;

        private Button btnRemoveLine;

        private Button btnAddLine;

        private BufferedTreeView tvDialog;

        public DialogTranslation(NavigationTranslation nt, navigation nav, TranslationManager.TranslationDialogue item)
        {
            this.InitializeComponent();
            this.nav = nav;
            this.navTranslation = nt;
            this.gameData = nav.ou.gameData;
            this.dialogueItem = item.Item;
            this.item = item;
            this.txtDialogName.Text = this.dialogueItem.Name;
            this.txtDialogName.ReadOnly = item.Item.getState() != GameData.State.OWNED;
            this.referenceList1.Exclusions.Add("lines");
            this.referenceList1.Exclusions.Add("conditions");
            this.referenceList1.Exclusions.Add("effects");
            this.referenceList1.setup(null, nav);
            this.createConversationTree(this.dialogueItem, this.tvDialog.Nodes);
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            if (this.currentLine != null)
            {
                TranslationManager.TranslationDialogueLine.Line line = this.currentLine.dialogueLine.CreateUserLine();
                this.grid.addItem("Text", line.Key, string.Empty, string.Empty, new Color?(this.stateColors[3]), true).Data = line;
                this.nav.HasChanges = true;
            }
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (this.currentLine != null && this.selectedLine != null && this.selectedLine.IsUser)
            {
                this.grid.removeItem("Text", this.selectedLine.Key);
                this.currentLine.dialogueLine.RemoveUserLine(this.selectedLine);
                this.nav.HasChanges = true;
            }
        }

        private void createConversationTree(GameData.Item item, TreeNodeCollection nodes)
        {
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in item.referenceData("lines", false))
            {
                GameData.Item item1 = this.gameData.getItem(keyValuePair.Key);
                if (item1 != null)
                {
                    if (item1.getState() == GameData.State.REMOVED)
                    {
                        continue;
                    }
                    bool value = keyValuePair.Value.v0 == 50;
                    TreeNode treeNode = nodes.Add(keyValuePair.Key, string.Empty);
                    DialogTranslation.LineData lineDatum = new DialogTranslation.LineData()
                    {
                        data = item1,
                        isLink = value,
                        isInterjection = (!item1.ContainsKey("interjection") ? false : item1.bdata["interjection"]),
                        dialogueLine = TranslationManager.DialogueLines[item1],
                        node = treeNode,
                        multiple = value
                    };
                    treeNode.Tag = lineDatum;
                    this.UpdateNode(treeNode);
                    if (value)
                    {
                        TreeNode[] treeNodeArray = this.tvDialog.Nodes.Find(keyValuePair.Key, true);
                        for (int i = 0; i < (int)treeNodeArray.Length; i++)
                        {
                            (treeNodeArray[i].Tag as DialogTranslation.LineData).multiple = true;
                        }
                    }
                    if (value)
                    {
                        continue;
                    }
                    this.createConversationTree(item1, treeNode.Nodes);
                }
                else
                {
                    TreeNode red = nodes.Add(keyValuePair.Key, string.Concat("ERROR: Dialog line missing: ", keyValuePair.Key));
                    red.BackColor = Color.Red;
                    red.ForeColor = Color.White;
                }
            }
            this.tvDialog.ExpandAll();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool findLine(string pattern)
        {
            return this.findLine(pattern, this.tvDialog.Nodes);
        }

        private bool findLine(string pattern, TreeNodeCollection nodes)
        {
            bool flag;
            IEnumerator enumerator = nodes.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    TreeNode current = (TreeNode)enumerator.Current;
                    DialogTranslation.LineData tag = current.Tag as DialogTranslation.LineData;
                    List<TranslationManager.TranslationDialogueLine.Line>.Enumerator enumerator1 = tag.dialogueLine.Lines.GetEnumerator();
                    try
                    {
                        while (enumerator1.MoveNext())
                        {
                            TranslationManager.TranslationDialogueLine.Line line = enumerator1.Current;
                            if (!NavigationTranslation.contains(pattern, line.Original) && !NavigationTranslation.contains(pattern, line.Translation))
                            {
                                continue;
                            }
                            this.tvDialog.SelectedNode = current;
                            flag = true;
                            return flag;
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator1).Dispose();
                    }
                    if (tag.isLink || !this.findLine(pattern, current.Nodes))
                    {
                        continue;
                    }
                    flag = true;
                    return flag;
                }
                return false;
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return flag;
        }

        private void grid_OnPropertyChanged(object sender, PropertyChangedArgs e)
        {
            TranslationManager.TranslationDialogueLine.Line data = e.Item.Data as TranslationManager.TranslationDialogueLine.Line;
            if (data != null)
            {
                this.nav.HasChanges = true;
                data.Translation = e.Item.Value.ToString();
                TranslationManager.DialogueTranslationState state = this.currentLine.dialogueLine.State;
                this.currentLine.dialogueLine.UpdateLineState(data);
                if (data.IsLinked)
                {
                    bool str = e.OldValue.ToString() != data.Translation;
                    TranslationManager.UpdateLinkedLines(data, str);
                }
                if (!data.IsUser)
                {
                    this.grid.getItem(data.Key, "Translation").TextColour = this.stateColors[(int)data.State];
                }
                else
                {
                    this.grid.getItem("Text", data.Key).TextColour = this.stateColors[(int)data.State];
                }
                this.UpdateNode(this.currentLine.node);
                if (state != this.currentLine.dialogueLine.State)
                {
                    this.tvDialog.Refresh();
                    this.navTranslation.Refresh();
                }
            }
        }

        private void grid_OnPropertyKeyEnter(object sender, PropertyKeyEnterArgs e)
        {
            this.grid_OnPropertyChanged(sender, new PropertyChangedArgs(e.Section, e.Item, e.Item.Value));
        }

        private void grid_OnPropertySelected(object sender, PropertySelectedArgs e)
        {
            TranslationManager.TranslationDialogueLine.Line data = e.Item.Data as TranslationManager.TranslationDialogueLine.Line;
            if (data != null)
            {
                this.selectedLine = data;
                this.btnRemoveLine.Enabled = data.IsUser;
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DialogTranslation));
            this.lbSpeaker = new Label();
            this.lbTarget = new Label();
            this.splitContainer1 = new SplitContainer();
            this.gbActions = new GroupBox();
            this.btnRemoveLine = new Button();
            this.btnAddLine = new Button();
            this.gbInfo = new GroupBox();
            this.gbEffects = new GroupBox();
            this.lvEffects = new ListView();
            this.columnEffects = new ColumnHeader();
            this.columnEffectValue = new ColumnHeader();
            this.gbConditions = new GroupBox();
            this.lvConditions = new ListView();
            this.columnWho = new ColumnHeader();
            this.columnCondition = new ColumnHeader();
            this.columnComp = new ColumnHeader();
            this.columnValue = new ColumnHeader();
            this.columnTag = new ColumnHeader();
            this.txtDialogName = new TextBox();
            this.splitContainer2 = new SplitContainer();
            this.grid = new PropertyGrid();
            this.referenceList1 = new ReferenceList();
            this.tvDialog = new BufferedTreeView();
            Label label = new Label();
            Label point = new Label();
            Label size = new Label();
            ((ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbActions.SuspendLayout();
            this.gbInfo.SuspendLayout();
            this.gbEffects.SuspendLayout();
            this.gbConditions.SuspendLayout();
            ((ISupportInitialize)this.splitContainer2).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            base.SuspendLayout();
            label.AutoSize = true;
            label.Location = new Point(30, 16);
            label.Name = "lbsSpeaker";
            label.Size = new Size(50, 13);
            label.TabIndex = 0;
            label.Text = "Speaker:";
            point.AutoSize = true;
            point.Location = new Point(6, 38);
            point.Name = "lbsTarget";
            point.Size = new Size(74, 13);
            point.TabIndex = 0;
            point.Text = "Target is type:";
            size.AutoSize = true;
            size.Location = new Point(11, 14);
            size.Name = "lbName";
            size.Size = new Size(35, 13);
            size.TabIndex = 0;
            size.Text = "Name";
            this.lbSpeaker.Location = new Point(86, 16);
            this.lbSpeaker.Name = "lbSpeaker";
            this.lbSpeaker.Size = new Size(158, 13);
            this.lbSpeaker.TabIndex = 0;
            this.lbSpeaker.Text = "-";
            this.lbSpeaker.TextAlign = ContentAlignment.TopRight;
            this.lbTarget.Location = new Point(86, 38);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Size = new Size(158, 13);
            this.lbTarget.TabIndex = 0;
            this.lbTarget.Text = "-";
            this.lbTarget.TextAlign = ContentAlignment.TopRight;
            this.splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = Orientation.Horizontal;
            this.splitContainer1.Panel1.Controls.Add(this.gbActions);
            this.splitContainer1.Panel1.Controls.Add(this.gbInfo);
            this.splitContainer1.Panel1.Controls.Add(this.referenceList1);
            this.splitContainer1.Panel1.Controls.Add(this.gbEffects);
            this.splitContainer1.Panel1.Controls.Add(this.gbConditions);
            this.splitContainer1.Panel1.Controls.Add(this.txtDialogName);
            this.splitContainer1.Panel1.Controls.Add(size);
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new Size(1390, 624);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 0;
            this.gbActions.Controls.Add(this.btnRemoveLine);
            this.gbActions.Controls.Add(this.btnAddLine);
            this.gbActions.Location = new Point(1180, 5);
            this.gbActions.Name = "gbActions";
            this.gbActions.Size = new Size(197, 161);
            this.gbActions.TabIndex = 3;
            this.gbActions.TabStop = false;
            this.gbActions.Text = "Actions";
            this.btnRemoveLine.Enabled = false;
            this.btnRemoveLine.Location = new Point(6, 48);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new Size(185, 23);
            this.btnRemoveLine.TabIndex = 0;
            this.btnRemoveLine.Text = "Remove line";
            this.btnRemoveLine.UseVisualStyleBackColor = true;
            this.btnRemoveLine.Click += new EventHandler(this.btnRemoveLine_Click);
            this.btnAddLine.Enabled = false;
            this.btnAddLine.Location = new Point(6, 19);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new Size(185, 23);
            this.btnAddLine.TabIndex = 0;
            this.btnAddLine.Text = "Add line";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new EventHandler(this.btnAddLine_Click);
            this.gbInfo.Controls.Add(point);
            this.gbInfo.Controls.Add(this.lbTarget);
            this.gbInfo.Controls.Add(this.lbSpeaker);
            this.gbInfo.Controls.Add(label);
            this.gbInfo.Location = new Point(52, 60);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new Size(250, 106);
            this.gbInfo.TabIndex = 42;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "Info";
            this.gbEffects.Controls.Add(this.lvEffects);
            this.gbEffects.Location = new Point(709, 5);
            this.gbEffects.Name = "gbEffects";
            this.gbEffects.Size = new Size(238, 161);
            this.gbEffects.TabIndex = 3;
            this.gbEffects.TabStop = false;
            this.gbEffects.Text = "Effects";
            this.lvEffects.Columns.AddRange(new ColumnHeader[] { this.columnEffects, this.columnEffectValue });
            this.lvEffects.FullRowSelect = true;
            this.lvEffects.GridLines = true;
            this.lvEffects.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lvEffects.LabelWrap = false;
            this.lvEffects.Location = new Point(6, 19);
            this.lvEffects.MultiSelect = false;
            this.lvEffects.Name = "lvEffects";
            this.lvEffects.ShowGroups = false;
            this.lvEffects.Size = new Size(226, 136);
            this.lvEffects.TabIndex = 0;
            this.lvEffects.UseCompatibleStateImageBehavior = false;
            this.lvEffects.View = View.Details;
            this.columnEffects.Text = "Effects";
            this.columnEffects.Width = 174;
            this.columnEffectValue.Text = "Value";
            this.columnEffectValue.Width = 47;
            this.gbConditions.Controls.Add(this.lvConditions);
            this.gbConditions.Location = new Point(308, 5);
            this.gbConditions.Name = "gbConditions";
            this.gbConditions.Size = new Size(395, 161);
            this.gbConditions.TabIndex = 2;
            this.gbConditions.TabStop = false;
            this.gbConditions.Text = "Conditions";
            this.lvConditions.Columns.AddRange(new ColumnHeader[] { this.columnWho, this.columnCondition, this.columnComp, this.columnValue, this.columnTag });
            this.lvConditions.FullRowSelect = true;
            this.lvConditions.GridLines = true;
            this.lvConditions.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.lvConditions.LabelWrap = false;
            this.lvConditions.Location = new Point(6, 19);
            this.lvConditions.MultiSelect = false;
            this.lvConditions.Name = "lvConditions";
            this.lvConditions.ShowGroups = false;
            this.lvConditions.Size = new Size(383, 136);
            this.lvConditions.TabIndex = 0;
            this.lvConditions.UseCompatibleStateImageBehavior = false;
            this.lvConditions.View = View.Details;
            this.columnWho.Text = "Who";
            this.columnWho.Width = 68;
            this.columnCondition.Text = "Condition";
            this.columnCondition.Width = 158;
            this.columnComp.Text = "==";
            this.columnComp.Width = 25;
            this.columnValue.Text = "Value";
            this.columnValue.Width = 40;
            this.columnTag.Text = "Tag";
            this.columnTag.Width = 86;
            this.txtDialogName.Location = new Point(52, 11);
            this.txtDialogName.Name = "txtDialogName";
            this.txtDialogName.ReadOnly = true;
            this.txtDialogName.Size = new Size(250, 20);
            this.txtDialogName.TabIndex = 1;
            this.txtDialogName.WordWrap = false;
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Panel1.Controls.Add(this.tvDialog);
            this.splitContainer2.Panel2.Controls.Add(this.grid);
            this.splitContainer2.Size = new Size(1388, 446);
            this.splitContainer2.SplitterDistance = 471;
            this.splitContainer2.TabIndex = 0;
            this.grid.AutoScroll = true;
            this.grid.AutoScrollMinSize = new Size(0, 100);
            this.grid.DividerFixed = true;
            this.grid.DividerPosition = 75;
            this.grid.Dock = DockStyle.Fill;
            this.grid.Location = new Point(0, 0);
            this.grid.Name = "grid";
            this.grid.Size = new Size(913, 446);
            this.grid.TabIndex = 1;
            this.grid.OnPropertyChanged += new PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
            this.grid.OnPropertyKeyEnter += new PropertyGrid.PropertyKeyEnterHandler(this.grid_OnPropertyKeyEnter);
            this.grid.OnPropertySelected += new PropertyGrid.PropertySelectedHandler(this.grid_OnPropertySelected);
            this.referenceList1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.referenceList1.Exclusions = (ArrayList)componentResourceManager.GetObject("referenceList1.Exclusions");
            this.referenceList1.Location = new Point(953, 6);
            this.referenceList1.Name = "referenceList1";
            this.referenceList1.ReadOnly = true;
            this.referenceList1.ShowDescription = true;
            this.referenceList1.Size = new Size(221, 161);
            this.referenceList1.TabIndex = 41;
            this.tvDialog.Dock = DockStyle.Fill;
            this.tvDialog.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.tvDialog.Location = new Point(0, 0);
            this.tvDialog.Name = "tvDialog";
            this.tvDialog.Size = new Size(471, 446);
            this.tvDialog.TabIndex = 0;
            this.tvDialog.DrawNode += new DrawTreeNodeEventHandler(this.tvDialog_DrawNode);
            this.tvDialog.AfterSelect += new TreeViewEventHandler(this.tvDialog_AfterSelect);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(1390, 624);
            base.Controls.Add(this.splitContainer1);
            base.Name = "DialogTranslation";
            this.Text = "Dialogue Translation";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbActions.ResumeLayout(false);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.gbEffects.ResumeLayout(false);
            this.gbConditions.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((ISupportInitialize)this.splitContainer2).EndInit();
            this.splitContainer2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void ShowLine(DialogTranslation.LineData line)
        {
            TalkerEnum item;
            PropertyGrid.Item item1;
            int num;
            this.currentLine = line;
            if (!line.data.idata.ContainsKey("speaker"))
            {
                this.lbSpeaker.Text = "-";
            }
            else
            {
                Label str = this.lbSpeaker;
                item = (TalkerEnum)line.data.idata["speaker"];
                str.Text = item.ToString();
            }
            if (!line.data.idata.ContainsKey("target is type"))
            {
                this.lbTarget.Text = "-";
            }
            else
            {
                Label label = this.lbTarget;
                CharacterTypeEnum characterTypeEnum = (CharacterTypeEnum)line.data.idata["target is type"];
                label.Text = characterTypeEnum.ToString();
            }
            this.lvConditions.Items.Clear();
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in line.data.referenceData("conditions", false))
            {
                GameData.Item item2 = this.gameData.getItem(keyValuePair.Key);
                if (item2.sdata.ContainsKey("compare by"))
                {
                    string str1 = item2.sdata["compare by"];
                    GameData.Item item3 = item2;
                    if (str1 == "==")
                    {
                        num = 0;
                    }
                    else
                    {
                        num = (str1 == "<" ? 1 : 2);
                    }
                    item3["compare by"] = num;
                }
                DialogConditionEnum dialogConditionEnum = (DialogConditionEnum)item2.idata["condition name"];
                int value = keyValuePair.Value.v0;
                if (!item2.idata.ContainsKey("who"))
                {
                    item2.idata["who"] = 0;
                }
                string[] strArrays = new string[] { "==", "<", ">" };
                ListView.ListViewItemCollection items = this.lvConditions.Items;
                item = (TalkerEnum)item2.idata["who"];
                ListViewItem listViewItem = items.Add(item.ToString());
                listViewItem.SubItems.Add(dialogConditionEnum.ToString());
                listViewItem.SubItems.Add(strArrays[item2.idata["compare by"]]);
                listViewItem.SubItems.Add(value.ToString());
                if (dialogConditionEnum == DialogConditionEnum.DC_HAS_TAG)
                {
                    ListViewItem.ListViewSubItemCollection subItems = listViewItem.SubItems;
                    CharacterPerceptionTags_LongTerm characterPerceptionTagsLongTerm = (CharacterPerceptionTags_LongTerm)item2.idata["tag"];
                    subItems.Add(characterPerceptionTagsLongTerm.ToString());
                }
                if (dialogConditionEnum < DialogConditionEnum.DC_PERSONALITY_TAG)
                {
                    continue;
                }
                ListViewItem.ListViewSubItemCollection listViewSubItemCollections = listViewItem.SubItems;
                PersonalityTags personalityTag = (PersonalityTags)item2.idata["tag"];
                listViewSubItemCollections.Add(personalityTag.ToString());
            }
            this.lvEffects.Items.Clear();
            foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair1 in line.data.referenceData("effects", false))
            {
                GameData.Item item4 = this.gameData.getItem(keyValuePair1.Key);
                if (item4 == null || item4.getState() == GameData.State.REMOVED)
                {
                    this.lvEffects.Items.Add("Error - invalid reference").ForeColor = Color.Red;
                }
                else if (!item4.sdata.ContainsKey("action name"))
                {
                    DialogActionEnum dialogActionEnum = (DialogActionEnum)item4.idata["action name"];
                    this.lvEffects.Items.Add(dialogActionEnum.ToString()).SubItems.Add(keyValuePair1.Value.v0.ToString());
                }
                else
                {
                    this.lvEffects.Items.Add(item4.sdata["action name"]).SubItems.Add(keyValuePair1.Value.v0.ToString());
                }
            }
            this.referenceList1.refresh(line.data);
            this.grid.clear();
            ExtendedTranslationText extendedTranslationText = new ExtendedTranslationText();
            if ((!line.data.ContainsKey("interjection") ? false : line.data.bdata["interjection"]))
            {
                return;
            }
            foreach (TranslationManager.TranslationDialogueLine.Line line1 in line.dialogueLine.Lines)
            {
                if (!line1.IsUser)
                {
                    item1 = this.grid.addItem(line1.Key, "Translation", line1.Translation, string.Empty, new Color?(this.stateColors[(int)line1.State]), true);
                    this.grid.addItem(line1.Key, "Original", line1.Original, string.Empty, new Color?(this.stateColors[0]), false).Data = line1;
                }
                else
                {
                    item1 = this.grid.addItem("Text", line1.Key, line1.Translation, string.Empty, new Color?(this.stateColors[(int)line1.State]), true);
                }
                item1.Data = line1;
                item1.Property = extendedTranslationText;
            }
        }

        private void tvDialog_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.tvDialog.SelectedNode == null)
            {
                return;
            }
            DialogTranslation.LineData tag = this.tvDialog.SelectedNode.Tag as DialogTranslation.LineData;
            if (tag != null)
            {
                this.ShowLine(tag);
                if (tag.multiple || this.multipleSelected)
                {
                    this.tvDialog.Refresh();
                }
                this.multipleSelected = tag.multiple;
            }
            this.btnAddLine.Enabled = tag != null;
        }

        private void tvDialog_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            bool state = (int)(e.State & TreeNodeStates.Selected) != 0;
            if (e.Node.Tag != null && this.currentLine != null && (e.Node.Tag as DialogTranslation.LineData).data == this.currentLine.data)
            {
                state = true;
            }
            if (state)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, this.tvDialog.Font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.Default);
                return;
            }
            if (!this.tvDialog.Focused && this.tvDialog.SelectedNode == e.Node)
            {
                e.Graphics.FillRectangle(SystemBrushes.ControlLight, e.Bounds);
            }
            else if (e.Node.BackColor.IsEmpty)
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            }
            else
            {
                using (SolidBrush solidBrush = new SolidBrush(e.Node.BackColor))
                {
                    e.Graphics.FillRectangle(solidBrush, e.Bounds);
                }
            }
            Color foreColor = e.Node.ForeColor;
            if (e.Node.Tag != null)
            {
                DialogTranslation.LineData tag = e.Node.Tag as DialogTranslation.LineData;
                if (tag.dialogueLine.State != TranslationManager.DialogueTranslationState.OK)
                {
                    using (Pen pen = new Pen(Color.Red))
                    {
                        pen.DashStyle = DashStyle.Dot;
                        Rectangle bounds = e.Bounds;
                        bounds.Size = new Size(bounds.Width - 1, bounds.Height - 1);
                        e.Graphics.DrawRectangle(pen, bounds);
                    }
                }
                if (!tag.isLink)
                {
                    foreColor = StateColours.getTalkerColour((TalkerEnum)tag.data["speaker"]);
                }
            }
            TextRenderer.DrawText(e.Graphics, e.Node.Text, this.tvDialog.Font, e.Bounds, foreColor, TextFormatFlags.Default);
        }

        private void UpdateNode(TreeNode node)
        {
            if (node == null || node.Tag == null)
            {
                return;
            }
            DialogTranslation.LineData tag = node.Tag as DialogTranslation.LineData;
            if (tag.isInterjection)
            {
                node.Text = "*Interjection Node*";
            }
            else if (tag.dialogueLine.Lines.Count <= 0 || string.IsNullOrEmpty(tag.dialogueLine.Lines[0].Translation))
            {
                node.Text = "<Empty>";
            }
            else
            {
                node.Text = tag.dialogueLine.Lines[0].Translation;
            }
            if (tag.isLink)
            {
                node.ForeColor = Color.Gray;
                return;
            }
            TalkerEnum item = (TalkerEnum)tag.data.idata["speaker"];
            if (tag.isInterjection)
            {
                node.BackColor = StateColours.getIntejectionColour(item);
                return;
            }
            node.ForeColor = StateColours.getTalkerColour(item);
        }

        private class LineData
        {
            public GameData.Item data;

            public bool isLink;

            public bool isInterjection;

            public bool multiple;

            public TranslationManager.TranslationDialogueLine dialogueLine;

            public TreeNode node;

            public LineData()
            {
            }
        }
    }
}