using forgotten_construction_set;
using forgotten_construction_set.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace forgotten_construction_set.dialog
{
    partial class ConditionControl : UserControl
	{
        private navigation nav;
        private System.ComponentModel.IContainer components = null;

        private ComboBoxTrans possibleConditions;

		private NumericUpDown conditionsValue;

		private ComboBoxTrans conditionMethod;

		private ListView listView1conditions;

		private ColumnHeader columnHeader1;

		private ColumnHeader columnHeader5;

		private ColumnHeader columnHeader4;

		private ColumnHeader columnHeader3;

		private Button addCondition;

		private Button removeCondition;

		private GroupBox groupBox1;

		private ComboBoxTrans enumBox;

		private ComboBoxTrans whoBox;

		private TextBox stringvarbox;

		private ColumnHeader columnHeader6;


		private void addCondition_Click(object sender, EventArgs e)
		{
			DialogConditionEnum selectedIndex = (DialogConditionEnum)(this.possibleConditions.SelectedIndex + 1);
			if (this.CurrentLine == null)
			{
				return;
			}
			int num = this.methodToInt(this.conditionMethod.Text);
			GameData.Item text = this.nav.ou.gameData.createItem(itemType.DIALOG_ACTION);
			text["condition name"] = (int)selectedIndex;
			text["who"] = this.whoBox.SelectedIndex;
			text["compare by"] = num;
			text["stringvar"] = this.stringvarbox.Text;
			text["tag"] = this.enumBox.SelectedIndex;
			int? nullable = null;
			int? nullable1 = nullable;
			nullable = null;
			this.CurrentLine.addReference("conditions", text, new int?((int)this.conditionsValue.Value), nullable1, nullable);
			this.refresh(this.CurrentLine);
			if (this.ChangeEvent != null)
			{
				this.ChangeEvent(this);
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
            this.possibleConditions = new ComboBoxTrans();
			this.conditionsValue = new NumericUpDown();
			this.conditionMethod = new ComboBoxTrans();
			this.listView1conditions = new ListView();
			this.columnHeader6 = new ColumnHeader();
			this.columnHeader1 = new ColumnHeader();
			this.columnHeader5 = new ColumnHeader();
			this.columnHeader4 = new ColumnHeader();
			this.columnHeader3 = new ColumnHeader();
			this.addCondition = new Button();
			this.removeCondition = new Button();
			this.groupBox1 = new GroupBox();
			this.whoBox = new ComboBoxTrans();
			this.enumBox = new ComboBoxTrans();
			this.stringvarbox = new TextBox();
			((ISupportInitialize)this.conditionsValue).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.possibleConditions.DropDownWidth = 300;
			this.possibleConditions.FormattingEnabled = true;
			this.possibleConditions.Location = new Point(97, 14);
			this.possibleConditions.Name = "possibleConditions";
			this.possibleConditions.Size = new System.Drawing.Size(173, 21);
			this.possibleConditions.TabIndex = 69;
			this.possibleConditions.SelectedIndexChanged += new EventHandler(this.possibleConditions_SelectedIndexChanged);
			this.conditionsValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.conditionsValue.Location = new Point(319, 14);
			this.conditionsValue.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
			this.conditionsValue.Minimum = new decimal(new int[] { 1000000, 0, 0, -2147483648 });
			this.conditionsValue.Name = "conditionsValue";
			this.conditionsValue.Size = new System.Drawing.Size(75, 20);
			this.conditionsValue.TabIndex = 70;
			this.conditionsValue.ThousandsSeparator = true;
			this.conditionMethod.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.conditionMethod.FormattingEnabled = true;
			this.conditionMethod.Items.AddRange(new object[] { "==", ">", "<" });
			this.conditionMethod.Location = new Point(276, 13);
			this.conditionMethod.Name = "conditionMethod";
			this.conditionMethod.Size = new System.Drawing.Size(40, 21);
			this.conditionMethod.TabIndex = 71;
			this.listView1conditions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.listView1conditions.Columns.AddRange(new ColumnHeader[] { this.columnHeader6, this.columnHeader1, this.columnHeader5, this.columnHeader4, this.columnHeader3 });
			this.listView1conditions.FullRowSelect = true;
			this.listView1conditions.GridLines = true;
			this.listView1conditions.LabelWrap = false;
			this.listView1conditions.Location = new Point(5, 68);
			this.listView1conditions.Margin = new System.Windows.Forms.Padding(2);
			this.listView1conditions.MultiSelect = false;
			this.listView1conditions.Name = "listView1conditions";
			this.listView1conditions.Size = new System.Drawing.Size(389, 140);
			this.listView1conditions.TabIndex = 72;
			this.listView1conditions.UseCompatibleStateImageBehavior = false;
			this.listView1conditions.View = View.Details;
			this.columnHeader6.Text = "谁";
			this.columnHeader6.Width = 49;
			this.columnHeader1.Text = "条件";
			this.columnHeader1.Width = 155;
			this.columnHeader5.Text = "==";
			this.columnHeader5.Width = 31;
			this.columnHeader4.Text = "值";
			this.columnHeader4.Width = 52;
			this.columnHeader3.Text = "标签";
			this.columnHeader3.Width = 113;
			this.addCondition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.addCondition.Location = new Point(233, 40);
			this.addCondition.Name = "addCondition";
			this.addCondition.Size = new System.Drawing.Size(75, 23);
			this.addCondition.TabIndex = 73;
			this.addCondition.Text = "添加";
			this.addCondition.UseVisualStyleBackColor = true;
			this.addCondition.Click += new EventHandler(this.addCondition_Click);
			this.removeCondition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.removeCondition.Location = new Point(314, 40);
			this.removeCondition.Name = "removeCondition";
			this.removeCondition.Size = new System.Drawing.Size(75, 23);
			this.removeCondition.TabIndex = 74;
			this.removeCondition.Text = "删除";
			this.removeCondition.UseVisualStyleBackColor = true;
			this.removeCondition.Click += new EventHandler(this.removeCondition_Click);
			this.groupBox1.Controls.Add(this.whoBox);
			this.groupBox1.Controls.Add(this.enumBox);
			this.groupBox1.Controls.Add(this.listView1conditions);
			this.groupBox1.Controls.Add(this.stringvarbox);
			this.groupBox1.Controls.Add(this.possibleConditions);
			this.groupBox1.Controls.Add(this.removeCondition);
			this.groupBox1.Controls.Add(this.conditionsValue);
			this.groupBox1.Controls.Add(this.addCondition);
			this.groupBox1.Controls.Add(this.conditionMethod);
			this.groupBox1.Dock = DockStyle.Fill;
			this.groupBox1.Location = new Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(397, 213);
			this.groupBox1.TabIndex = 77;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "条件";
			this.whoBox.FormattingEnabled = true;
			this.whoBox.Location = new Point(5, 14);
			this.whoBox.Name = "whoBox";
			this.whoBox.Size = new System.Drawing.Size(86, 21);
			this.whoBox.TabIndex = 78;
			this.enumBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.enumBox.Enabled = false;
			this.enumBox.FormattingEnabled = true;
			this.enumBox.Location = new Point(84, 40);
			this.enumBox.Name = "enumBox";
			this.enumBox.Size = new System.Drawing.Size(143, 21);
			this.enumBox.TabIndex = 77;
			this.stringvarbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.stringvarbox.Enabled = false;
			this.stringvarbox.Location = new Point(25, 40);
			this.stringvarbox.Name = "stringvarbox";
			this.stringvarbox.Size = new System.Drawing.Size(53, 20);
			this.stringvarbox.TabIndex = 76;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.groupBox1);
			base.Name = "ConditionControl";
			base.Size = new System.Drawing.Size(397, 213);
			((ISupportInitialize)this.conditionsValue).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}

		private int methodToInt(string s)
		{
			if (s == "<")
			{
				return 1;
			}
			if (s == ">")
			{
				return 2;
			}
			return 0;
		}

		private string methodToString(int i)
		{
			if (i == 1)
			{
				return "<";
			}
			if (i == 2)
			{
				return ">";
			}
			return "==";
		}

		private void possibleConditions_SelectedIndexChanged(object sender, EventArgs e)
		{
			object obj;
			if (this.possibleConditions.SelectedItem == null)
			{
				return;
			}
			DialogConditionEnum selectedItem = (DialogConditionEnum)this.possibleConditions.SelectedItem;
			this.stringvarbox.Text = "";
			this.enumBox.Items.Clear();
			this.conditionMethod.Enabled = true;
			this.conditionsValue.Enabled = true;
			if (!ConditionControl.conditionDefaults.TryGetValue(selectedItem, out obj))
			{
				obj = 0;
			}
			if (obj is string)
			{
				this.stringvarbox.Enabled = true;
				this.enumBox.Enabled = false;
				return;
			}
			if (!obj.GetType().IsEnum)
			{
				this.stringvarbox.Enabled = false;
				this.enumBox.Enabled = false;
			}
			else
			{
				this.stringvarbox.Enabled = false;
				this.enumBox.Enabled = true;
				foreach (object value in Enum.GetValues(obj.GetType()))
				{
					this.enumBox.Items.Add(value);
				}
			}
		}

		public void refresh(GameData.Item dialogLine)
		{
			if (dialogLine == null)
			{
				this.listView1conditions.Enabled = false;
				this.listView1conditions.Items.Clear();
				this.addCondition.Enabled = false;
				this.removeCondition.Enabled = false;
				this.CurrentLine = null;
				return;
			}
			this.listView1conditions.Enabled = true;
			this.addCondition.Enabled = dialogLine.getState() != GameData.State.LOCKED;
			this.removeCondition.Enabled = dialogLine.getState() != GameData.State.LOCKED;
			if (this.possibleConditions.Items.Count == 0)
			{
				foreach (DialogConditionEnum value in Enum.GetValues(typeof(DialogConditionEnum)))
				{
					if (value == DialogConditionEnum.DC_NONE)
					{
						continue;
					}
					this.possibleConditions.Items.Add(value);
				}
			}
			if (this.whoBox.Items.Count == 0)
			{
				foreach (TalkerEnum talkerEnum in Enum.GetValues(typeof(TalkerEnum)))
				{
					this.whoBox.Items.Add(talkerEnum);
				}
			}
			if (this.whoBox.SelectedIndex < 0)
			{
				this.whoBox.SelectedIndex = 0;
			}
			this.listView1conditions.Items.Clear();
			if (dialogLine == null)
			{
				return;
			}
			foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in dialogLine.referenceData("conditions", false))
			{
				GameData.Item item = this.nav.ou.gameData.getItem(keyValuePair.Key);
				item.idata.ContainsKey("condition name");
				if (item.sdata.ContainsKey("compare by"))
				{
					item["compare by"] = this.methodToInt(item.sdata["compare by"]);
				}
				DialogConditionEnum dialogConditionEnum = (DialogConditionEnum)item.idata["condition name"];
				int num = keyValuePair.Value.v0;
				if (!item.idata.ContainsKey("who"))
				{
					item.idata["who"] = 0;
				}
				ListView.ListViewItemCollection items = this.listView1conditions.Items;
				TalkerEnum item1 = (TalkerEnum)item.idata["who"];
				ListViewItem listViewItem = items.Add(item1.ToString());
				listViewItem.SubItems.Add(dialogConditionEnum.ToString());
				listViewItem.SubItems.Add(this.methodToString(item.idata["compare by"]));
				listViewItem.SubItems.Add(num.ToString());
				if (item.sdata.ContainsKey("stringvar"))
				{
					string str = item.sdata["stringvar"];
				}
				if (dialogConditionEnum == DialogConditionEnum.DC_HAS_TAG)
				{
					ListViewItem.ListViewSubItemCollection subItems = listViewItem.SubItems;
					CharacterPerceptionTags_LongTerm characterPerceptionTagsLongTerm = (CharacterPerceptionTags_LongTerm)item.idata["tag"];
					subItems.Add(characterPerceptionTagsLongTerm.ToString());
				}
				if (dialogConditionEnum < DialogConditionEnum.DC_PERSONALITY_TAG)
				{
					continue;
				}
				ListViewItem.ListViewSubItemCollection listViewSubItemCollections = listViewItem.SubItems;
				PersonalityTags personalityTag = (PersonalityTags)item.idata["tag"];
				listViewSubItemCollections.Add(personalityTag.ToString());
			}
			this.CurrentLine = dialogLine;
		}

		private void removeCondition_Click(object sender, EventArgs e)
		{
			if (this.listView1conditions.SelectedItems.Count < 1)
			{
				return;
			}
			int index = this.listView1conditions.SelectedItems[0].Index;
			if (this.CurrentLine == null)
			{
				return;
			}
			this.CurrentLine.removeReference("conditions", index);
			this.refresh(this.CurrentLine);
			if (this.ChangeEvent != null)
			{
				this.ChangeEvent(this);
			}
		}

		public void setup(GameData.Item item, navigation n)
		{
			this.Item = item;
			this.nav = n;
			this.refresh(null);
			this.addCondition.Enabled = (item == null ? true : item.getState() != GameData.State.LOCKED);
			this.removeCondition.Enabled = (item == null ? true : item.getState() != GameData.State.LOCKED);
		}

		public event ConditionControl.ConditionsChangedHandler ChangeEvent;

		public delegate void ConditionsChangedHandler(object sender);
	}
}