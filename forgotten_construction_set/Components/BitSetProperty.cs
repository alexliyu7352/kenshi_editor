using forgotten_construction_set;
using PropertyGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace forgotten_construction_set.Components
{
	public class BitSetProperty : DropdownProperty
	{
		protected CheckedListBox mList;

		private List<KeyValuePair<string, int>> values;

		public bool DisplayHex
		{
			get;
			set;
		}

		public BitSetProperty(object typeValue, bool hex = false)
		{
			this.DisplayHex = typeValue == null | hex;
			this.values = new List<KeyValuePair<string, int>>();
			if (typeValue != null && typeValue.GetType().IsEnum)
			{
				foreach (object value in Enum.GetValues(typeValue.GetType()))
				{
					this.values.Add(new KeyValuePair<string, int>(value.ToString(), (int)value));
				}
			}
			else if (typeValue is EnumValue)
			{
				foreach (KeyValuePair<string, int> @enum in (typeValue as EnumValue).Enum)
				{
                    //TODO 考虑这里进行汉化
                    this.values.Add(@enum);
                    //this.values.Add(new KeyValuePair<string, int>(@enum.Value+"1", (int)@enum.Value));
				}
			}
		}

		public override void Apply()
		{
			int value = 0;
			if (!this.DisplayHex)
			{
				char[] chrArray = new char[] { ' ', ',', ';' };
				string[] strArrays = this.mTextBox.Text.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					strArrays[i] = strArrays[i].ToLower().Trim();
				}
				foreach (KeyValuePair<string, int> keyValuePair in this.values)
				{
					if (!strArrays.Contains<string>(keyValuePair.Key.ToLower()))
					{
						continue;
					}
					value = value | 1 << (keyValuePair.Value & 31);
				}
				base.setValue(value);
			}
			else if (!int.TryParse(this.mTextBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value))
			{
				SystemSounds.Asterisk.Play();
			}
			else
			{
				int num = 0;
				foreach (KeyValuePair<string, int> value1 in this.values)
				{
					num = num | 1 << (value1.Value & 31);
				}
				if (num != 0)
				{
					value &= num;
				}
				base.setValue(value);
			}
			base.DestroyEditor();
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			this.mList = new CheckedListBox()
			{
				CheckOnClick = true
			};
			this.mList.ItemCheck += new ItemCheckEventHandler(this.itemChecked);
			this.mList.MinimumSize = new Size(rect.Width - 2, 16);
			this.mList.MaximumSize = new Size(400, 400);
			this.mList.BorderStyle = BorderStyle.None;
			foreach (KeyValuePair<string, int> value in this.values)
			{
				this.mList.Items.Add(value.Key);
			}
			this.mTextBox.KeyDown += new KeyEventHandler(this.mTextBox_KeyDown);
			this.mTextBox.KeyPress += new KeyPressEventHandler(this.mTextBox_KeyPress);
			base.PopupControl = this.mList;
		}

		public override string getAsString(object value)
		{
			int num = (int)value;
			if (this.DisplayHex)
			{
				return num.ToString("X4");
			}
			List<string> strs = new List<string>();
			foreach (KeyValuePair<string, int> keyValuePair in this.values)
			{
				if ((num & 1 << (keyValuePair.Value & 31)) == 0)
				{
					continue;
				}
				strs.Add(keyValuePair.Key);
			}
			return string.Join(",", strs);
		}

		private void itemChecked(object sender, ItemCheckEventArgs e)
		{
			int value = (int)this.mItem.Value;
			if (e.NewValue != CheckState.Checked)
			{
				value &= ~(1 << (e.Index & 31));
			}
			else
			{
				value = value | 1 << (e.Index & 31);
			}
			base.setValue(value);
			if (this.mTextBox != null)
			{
				this.mTextBox.Text = this.getAsString(value);
			}
		}

		private void mTextBox_KeyDown(object sender, KeyEventArgs e)
		{
		}

		private void mTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.DisplayHex && !char.IsNumber(e.KeyChar) && !"abcdefABCDEF".Contains<char>(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		protected override void onPopup()
		{
			base.onPopup();
			int value = (int)this.mItem.Value;
			for (int i = 0; i < this.mList.Items.Count; i++)
			{
				this.mList.SetItemChecked(i, (value & 1 << (i & 31)) > 0);
			}
		}

        public override void Paint(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Item item, Graphics g, Rectangle rect)
        {
            if (base.Editing == item)
            {
                ComboBoxState comboBoxState = ComboBoxState.Normal;
                switch (this.mState)
                {
                    case PushButtonState.Hot:
                        {
                            comboBoxState = ComboBoxState.Hot;
                            break;
                        }
                    case PushButtonState.Pressed:
                        {
                            comboBoxState = ComboBoxState.Pressed;
                            break;
                        }
                    case PushButtonState.Disabled:
                        {
                            comboBoxState = ComboBoxState.Disabled;
                            break;
                        }
                }
                rect.X = this.mButton.X;
                rect.Width = this.mButton.Width;
                if (!Application.RenderWithVisualStyles)
                {
                    ControlPaint.DrawScrollButton(g, rect, ScrollButton.Down, (comboBoxState == ComboBoxState.Pressed ? ButtonState.Pushed : ButtonState.Normal));
                }
                else
                {
                    ComboBoxRenderer.DrawDropDownButton(g, rect, comboBoxState);
                }
            }
            else
            {
                base.DrawText("123", grid.Font, g, rect, 2, 0, new Color?(item.TextColour));
                if (base.Editing == item)
                {
                    rect.X = this.mButton.X;
                    rect.Width = this.mButton.Width;
                    ButtonRenderer.DrawButton(g, rect, this.mState);
                }
            }
        }
  


}
}