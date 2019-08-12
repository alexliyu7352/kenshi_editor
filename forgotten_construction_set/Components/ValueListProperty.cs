using PropertyGrid;
using System;
using System.Drawing;
using System.Media;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
	public abstract class ValueListProperty : CustomProperty
	{
		protected int Items
		{
			get;
			set;
		}

		protected ValueListProperty.ValueType Type
		{
			get;
			set;
		}

		protected ValueListProperty()
		{
		}

		public override void Apply()
		{
			char[] chrArray = new char[] { ' ', ',', ';' };
			string[] strArrays = this.mTextBox.Text.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);
			object obj = null;
			if ((int)strArrays.Length == this.Items)
			{
				obj = this.Parse(strArrays);
			}
			if (obj == null)
			{
				SystemSounds.Asterisk.Play();
			}
			else
			{
				base.setValue(obj);
			}
			base.DestroyEditor();
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			if (rect.Width < 3)
			{
				return;
			}
			base.CreateEditor(grid, section, item, rect);
			if (this.Type == ValueListProperty.ValueType.INT)
			{
				this.mTextBox.KeyPress += new KeyPressEventHandler(this.filter_int);
			}
			if (this.Type == ValueListProperty.ValueType.UINT)
			{
				this.mTextBox.KeyPress += new KeyPressEventHandler(this.filter_uint);
			}
			else if (this.Type == ValueListProperty.ValueType.FLOAT)
			{
				this.mTextBox.KeyPress += new KeyPressEventHandler(this.filter_float);
			}
			this.mTextBox.Text = this.ValueString(item.Value);
		}

		private void filter_float(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != ' ' && e.KeyChar != ',' && !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.')
			{
				e.Handled = true;
			}
		}

		private void filter_int(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != ' ' && e.KeyChar != ',' && !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '-')
			{
				e.Handled = true;
			}
		}

		private void filter_uint(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar != ' ' && e.KeyChar != ',' && !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		public override void Paint(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Item item, Graphics g, Rectangle rect)
		{
			base.DrawText(this.ValueString(item.Value), grid.Font, g, rect, 2, 0, new Color?(item.TextColour));
		}

		protected abstract object Parse(string[] parts);

		protected virtual string ValueString(object v)
		{
			return v.ToString();
		}

		protected enum ValueType
		{
			STRING,
			INT,
			UINT,
			FLOAT
		}
	}
}