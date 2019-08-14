using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class SetFieldValue : Form
	{
		private navigation nav;

		private List<GameData.Item> items;

		private IContainer components;

		private ComboBox fieldName;

		private Button bOk;

		private TextBox fieldValue;

		private Button bCancel;

		private ComboBox enumValue;

		private Button bExtra;

		public SetFieldValue(navigation nav, List<GameData.Item> items)
		{
			this.InitializeComponent();
			this.items = items;
			this.nav = nav;
			this.updateFieldList();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void bExtra_Click(object sender, EventArgs e)
		{
			GameData.Desc desc = this.getDesc(this.fieldName.Text);
			if (desc == null)
			{
				return;
			}
			if (desc.flags == 16)
			{
				TextDialog textDialog = new TextDialog(this.fieldName.Text, this.fieldValue.Text, null);
				if (textDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					this.fieldValue.Text = textDialog.Text;
					return;
				}
			}
			else if (desc.defaultValue is GameData.TripleInt)
			{
				string str = "";
				itemType _itemType = desc.list;
				if (_itemType == itemType.NULL_ITEM)
				{
					_itemType = itemType.BUILDING;
					str = "is node=true";
				}
				ItemDialog itemDialog = new ItemDialog(string.Concat("选择 ", this.fieldName.Text, " 的关联"), this.nav.ou.gameData, _itemType, true, str, _itemType);
				if (itemDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					List<string> strs = new List<string>();
					foreach (GameData.Item item in itemDialog.Items)
					{
						string str1 = item.stringID;
						if (desc.flags > 0)
						{
							str1 = string.Concat(str1, " ", (desc.defaultValue as GameData.TripleInt).v0.ToString());
						}
						if (desc.flags > 1)
						{
							str1 = string.Concat(str1, " ", (desc.defaultValue as GameData.TripleInt).v1.ToString());
						}
						if (desc.flags > 2)
						{
							str1 = string.Concat(str1, " ", (desc.defaultValue as GameData.TripleInt).v2.ToString());
						}
						strs.Add(str1);
					}
					this.fieldValue.Text = string.Join("; ", strs);
				}
			}
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			object defaultValue = this.getDefaultValue(this.fieldName.Text);
			object str = null;
			if (defaultValue == null)
			{
				MessageBox.Show("无效字段名称");
				return;
			}
			if (defaultValue is bool)
			{
				str = this.enumValue.SelectedItem.ToString() == "True";
			}
			else if (defaultValue is int)
			{
				str = int.Parse(this.fieldValue.Text);
			}
			else if (defaultValue is float)
			{
				str = float.Parse(this.fieldValue.Text);
			}
			else if (defaultValue is string)
			{
				str = this.fieldValue.Text;
			}
			else if (defaultValue is GameData.File)
			{
				str = new GameData.File(this.fieldValue.Text);
			}
			else if (defaultValue.GetType().IsEnum)
			{
				str = this.enumValue.SelectedItem;
			}
			else if (defaultValue is GameData.TripleInt)
			{
				str = this.parseReferences(this.getDesc(this.fieldName.Text), this.fieldValue.Text);
			}
			if (str == null)
			{
				return;
			}
			if (!(defaultValue is GameData.TripleInt))
			{
				this.nav.HasChanges = true;
				foreach (GameData.Item item in this.items)
				{
					item[this.fieldName.Text] = str;
					this.nav.refreshItemWindow(item);
					this.nav.refreshState(item);
				}
				base.Close();
				return;
			}
			foreach (GameData.Item item1 in this.items)
			{
				foreach (KeyValuePair<GameData.Item, GameData.TripleInt> keyValuePair in str as List<KeyValuePair<GameData.Item, GameData.TripleInt>>)
				{
					if (!item1.hasReferenceTo(this.fieldName.Text, keyValuePair.Key.stringID))
					{
						item1.addReference(this.fieldName.Text, keyValuePair.Key, new int?(keyValuePair.Value.v0), new int?(keyValuePair.Value.v1), new int?(keyValuePair.Value.v2));
					}
					else
					{
						item1.setReferenceValue(this.fieldName.Text, keyValuePair.Key.stringID, keyValuePair.Value);
					}
					this.nav.HasChanges = true;
					this.nav.refreshItemWindow(item1);
					this.nav.refreshState(item1);
				}
			}
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void fieldName_SelectedIndexChanged(object sender, EventArgs e)
		{
			int num;
			GameData.Desc desc = this.getDesc(this.fieldName.Text);
			object defaultValue = this.getDefaultValue(this.fieldName.Text);
			object currentValue = this.getCurrentValue(this.fieldName.Text) ?? defaultValue;
			if (defaultValue.GetType().IsEnum && desc.flags != 256)
			{
				this.fieldValue.Visible = false;
				this.enumValue.Visible = true;
				this.enumValue.Items.Clear();
				foreach (object value in Enum.GetValues(defaultValue.GetType()))
				{
					this.enumValue.Items.Add(value);
				}
				this.enumValue.SelectedItem = Enum.ToObject(defaultValue.GetType(), currentValue);
				return;
			}
			if (defaultValue is bool)
			{
				this.fieldValue.Visible = false;
				this.enumValue.Visible = true;
				this.enumValue.Items.Clear();
				this.enumValue.Items.Add("False");
				this.enumValue.Items.Add("True");
				this.enumValue.SelectedItem = ((bool)currentValue ? "True" : "False");
				return;
			}
			if (desc != null && GameData.isListType(desc))
			{
				this.fieldValue.Visible = true;
				this.enumValue.Visible = false;
				this.bExtra.Visible = true;
				this.fieldValue.Width = this.bExtra.Left - this.fieldValue.Left;
				this.fieldValue.Text = "";
				return;
			}
			this.fieldValue.Visible = true;
			this.enumValue.Visible = false;
			this.fieldValue.Text = currentValue.ToString();
			this.bExtra.Visible = (desc == null ? false : (desc.flags & 16) > 0);
			num = (this.bExtra.Visible ? this.bExtra.Left : this.bExtra.Right);
			this.fieldValue.Width = num - this.fieldValue.Left;
		}

		private object getCurrentValue(string key)
		{
			object obj;
			if (!this.items[0].ContainsKey(key))
			{
				return null;
			}
			object bah = this.items[0][key];
			List<GameData.Item>.Enumerator enumerator = this.items.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameData.Item current = enumerator.Current;
					if (!current.ContainsKey(key) || current[key].Equals(bah))
					{
						continue;
					}
					obj = null;
					return obj;
				}
				return bah;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return obj;
		}

		private object getDefaultValue(string key)
		{
			GameData.Desc desc = this.getDesc(key);
			if (desc == null)
			{
				return null;
			}
			return desc.defaultValue;
		}

		private GameData.Desc getDesc(string key)
		{
			if (!GameData.desc.ContainsKey(this.items[0].type))
			{
				return null;
			}
			if (!GameData.desc[this.items[0].type].ContainsKey(key))
			{
				return null;
			}
			return GameData.desc[this.items[0].type][key];
		}

		private void InitializeComponent()
		{
			this.fieldName = new ComboBox();
			this.bOk = new Button();
			this.fieldValue = new TextBox();
			this.bCancel = new Button();
			this.enumValue = new ComboBox();
			this.bExtra = new Button();
			base.SuspendLayout();
			this.fieldName.FormattingEnabled = true;
			this.fieldName.Location = new Point(12, 12);
			this.fieldName.Name = "fieldName";
			this.fieldName.Size = new System.Drawing.Size(151, 21);
			this.fieldName.TabIndex = 0;
			this.fieldName.SelectedIndexChanged += new EventHandler(this.fieldName_SelectedIndexChanged);
			this.bOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.bOk.Location = new Point(253, 39);
			this.bOk.Name = "bOk";
			this.bOk.Size = new System.Drawing.Size(77, 27);
			this.bOk.TabIndex = 1;
			this.bOk.Text = "设置值";
			this.bOk.UseVisualStyleBackColor = true;
			this.bOk.Click += new EventHandler(this.bOk_Click);
			this.fieldValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.fieldValue.Location = new Point(169, 12);
			this.fieldValue.Name = "fieldValue";
			this.fieldValue.Size = new System.Drawing.Size(161, 20);
			this.fieldValue.TabIndex = 2;
			this.bCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new Point(169, 38);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(77, 26);
			this.bCancel.TabIndex = 3;
			this.bCancel.Text = "取消";
			this.bCancel.UseVisualStyleBackColor = true;
			this.bCancel.Click += new EventHandler(this.bCancel_Click);
			this.enumValue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.enumValue.DropDownStyle = ComboBoxStyle.DropDownList;
			this.enumValue.FormattingEnabled = true;
			this.enumValue.Location = new Point(169, 11);
			this.enumValue.Name = "enumValue";
			this.enumValue.Size = new System.Drawing.Size(161, 21);
			this.enumValue.Sorted = true;
			this.enumValue.TabIndex = 4;
			this.enumValue.Visible = false;
			this.bExtra.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.bExtra.Location = new Point(305, 10);
			this.bExtra.Name = "bExtra";
			this.bExtra.Size = new System.Drawing.Size(25, 23);
			this.bExtra.TabIndex = 5;
			this.bExtra.Text = "...";
			this.bExtra.UseVisualStyleBackColor = true;
			this.bExtra.Visible = false;
			this.bExtra.Click += new EventHandler(this.bExtra_Click);
			base.AcceptButton = this.bOk;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.bCancel;
			base.ClientSize = new System.Drawing.Size(335, 68);
			base.Controls.Add(this.bExtra);
			base.Controls.Add(this.enumValue);
			base.Controls.Add(this.bCancel);
			base.Controls.Add(this.fieldValue);
			base.Controls.Add(this.bOk);
			base.Controls.Add(this.fieldName);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			base.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(99999, 102);
			base.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(351, 102);
			base.Name = "SetFieldValue";
			base.ShowInTaskbar = false;
			this.Text = "设置字段值";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private List<KeyValuePair<GameData.Item, GameData.TripleInt>> parseReferences(GameData.Desc desc, string s)
		{
			GameData.Item j;
			List<KeyValuePair<GameData.Item, GameData.TripleInt>> keyValuePairs = new List<KeyValuePair<GameData.Item, GameData.TripleInt>>();
			string[] strArrays = s.Split(new char[] { ';' });
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				string[] strArrays1 = str.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
				string str1 = strArrays1[0];
				int num = 1;
				for (j = this.nav.ou.gameData.getItem(str1); j == null && num < (int)strArrays1.Length; j = this.nav.ou.gameData.getItem(str1))
				{
					str1 = string.Concat(str1, " ", strArrays1[num]);
					num++;
				}
				if (j == null)
				{
					MessageBox.Show(string.Concat("Invalid reference ", str));
					return null;
				}
				if (j.type != desc.list)
				{
					MessageBox.Show("Incorrect referenced item type");
					return null;
				}
				GameData.TripleInt tripleInt = new GameData.TripleInt(desc.defaultValue as GameData.TripleInt);
				if ((int)strArrays1.Length > num)
				{
					int.TryParse(strArrays1[num], out tripleInt.v0);
				}
				if ((int)strArrays1.Length > num + 1)
				{
					int.TryParse(strArrays1[num + 1], out tripleInt.v1);
				}
				if ((int)strArrays1.Length > num + 2)
				{
					int.TryParse(strArrays1[num + 2], out tripleInt.v2);
				}
				keyValuePairs.Add(new KeyValuePair<GameData.Item, GameData.TripleInt>(j, tripleInt));
			}
			return keyValuePairs;
		}

		private void updateFieldList()
		{
			itemType item = this.items[0].type;
			SortedSet<string> strs = new SortedSet<string>();
			foreach (GameData.Item item1 in this.items)
			{
				foreach (KeyValuePair<string, object> keyValuePair in item1)
				{
					strs.Add(keyValuePair.Key);
				}
				if (item == item1.type)
				{
					continue;
				}
				item = itemType.NULL_ITEM;
			}
			if (GameData.desc.ContainsKey(item))
			{
				foreach (KeyValuePair<string, GameData.Desc> keyValuePair1 in GameData.desc[item])
				{
					if (!GameData.isListType(keyValuePair1.Value))
					{
						continue;
					}
					strs.Add(keyValuePair1.Key);
				}
			}
			this.fieldName.Items.Clear();
			foreach (string str in strs)
			{
				this.fieldName.Items.Add(str);
			}
			this.fieldName.Text = "";
			this.fieldName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			this.fieldName.AutoCompleteSource = AutoCompleteSource.ListItems;
		}
	}
}