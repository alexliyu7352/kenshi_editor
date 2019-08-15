using System;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Windows.Forms;

namespace forgotten_construction_set.PropertyGrid
{
	public class ColorProperty : ButtonProperty
	{
		public ColorProperty()
		{
		}

		public override void Apply()
		{
			if (this.mItem != null)
			{
				base.setValue(this.ParseString(this.mTextBox.Text));
			}
			base.DestroyEditor();
		}

		protected override void ButtonPressed()
		{
			ColorDialog colorDialog = new ColorDialog()
			{
				Color = (Color)this.mItem.Value,
				FullOpen = true
			};
			base.FocusLocked = true;
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				base.setValue(colorDialog.Color);
				this.mTextBox.Text = this.ColourString((Color)this.mItem.Value);
				this.mGrid.Invalidate();
			}
			base.FocusLocked = false;
		}

		protected string ColourString(Color c)
		{
			string str = string.Format("{0} {1} {2}", c.R, c.G, c.B);
			return str;
		}

		public override void CreateEditor(PropertyGrid grid, PropertyGrid.Section section, PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			Color value = (Color)item.Value;
			base.CreateTextbox(this.ColourString(value), rect, rect.Height + 5, this.mButton.Width);
		}

		public override void Paint(PropertyGrid grid, PropertyGrid.Item item, Graphics g, Rectangle rect)
		{
			Color value = (Color)item.Value;
			Brush solidBrush = new SolidBrush(value);
			g.FillRectangle(solidBrush, rect.X + 2, rect.Y + 3, rect.Height, rect.Height - 5);
			g.DrawRectangle(Pens.Black, rect.X + 1, rect.Y + 2, rect.Height, rect.Height - 4);
			if (base.Editing == item)
			{
				base.Paint(grid, item, g, rect);
			}
			else
			{
				base.DrawText(this.ColourString(value), grid.Font, g, rect, rect.Height + 5, 0, new Color?(item.TextColour));
			}
		}

		protected Color ParseString(string s)
		{
			Color value;
			try
			{
				string[] strArrays = s.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
				if ((int)strArrays.Length == 3)
				{
					value = Color.FromArgb(Convert.ToInt32(strArrays[0]), Convert.ToInt32(strArrays[1]), Convert.ToInt32(strArrays[2]));
					return value;
				}
				else if (s.StartsWith("#"))
				{
					int num = int.Parse(s.Substring(1), NumberStyles.HexNumber);
					value = Color.FromArgb(255, Color.FromArgb(num));
					return value;
				}
			}
			catch (Exception exception)
			{
			}
			SystemSounds.Asterisk.Play();
			value = (Color)this.mItem.Value;
			return value;
		}
	}
}