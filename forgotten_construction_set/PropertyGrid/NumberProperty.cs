using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace forgotten_construction_set.PropertyGrid
{
	public class NumberProperty : CustomProperty
	{
		public NumberProperty()
		{
		}

		public override void Apply()
		{
			float single;
			int num;
			if (this.mItem.Value.GetType() == typeof(float))
			{
				if (!float.TryParse(this.mTextBox.Text, out single))
				{
					SystemSounds.Asterisk.Play();
				}
				else
				{
					base.setValue(single);
				}
			}
			else if (!int.TryParse(this.mTextBox.Text, out num))
			{
				SystemSounds.Asterisk.Play();
			}
			else
			{
				base.setValue(num);
			}
			base.DestroyEditor();
		}

		public override void CreateEditor(PropertyGrid grid, PropertyGrid.Section section, PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			this.mTextBox.KeyPress += new KeyPressEventHandler(this.mTextBox_KeyPress);
		}

		private void mTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			bool flag;
			char chr = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
			if (char.IsNumber(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar == '-' && this.mTextBox.SelectionStart <= 0)
			{
				flag = false;
			}
			else
			{
				flag = (e.KeyChar != chr ? true : this.mTextBox.Text.Contains<char>(chr));
			}
			if (flag)
			{
				e.Handled = true;
			}
		}
	}
}