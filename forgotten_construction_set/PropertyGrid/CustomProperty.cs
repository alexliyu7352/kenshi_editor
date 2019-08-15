using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class CustomProperty
	{
		protected PropertyGrid.PropertyGrid mGrid;

		protected TextBox mTextBox;

		protected PropertyGrid.PropertyGrid.Item mItem;

		protected PropertyGrid.PropertyGrid.Section mSection;

		public PropertyGrid.PropertyGrid.Item Editing
		{
			get
			{
				return this.mItem;
			}
		}

		public bool FocusLocked
		{
			get;
			protected set;
		}

		public CustomProperty()
		{
		}

		public virtual void Apply()
		{
			this.setValue(this.mTextBox.Text);
			this.DestroyEditor();
		}

		public virtual void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			this.mGrid = grid;
			this.mItem = item;
			this.mSection = section;
			if (this.mTextBox == null)
			{
				this.CreateTextbox(this.getAsString(item.Value), rect, 2, 0);
			}
		}

		protected void CreateTextbox(string text, Rectangle rect, int left = 0, int right = 0)
		{
			if (rect.Width >= 3)
			{
				if (this.mTextBox == null)
				{
					this.mTextBox = new TextBox();
					this.mTextBox.KeyDown += new KeyEventHandler(this.mTextBox_KeyDown);
					this.mTextBox.MouseDoubleClick += new MouseEventHandler(this.mTextBox_MouseDoubleClick);
					this.mTextBox.LostFocus += new EventHandler(this.mTextBox_LostFocus);
					this.mTextBox.BorderStyle = BorderStyle.None;
					this.mTextBox.Font = this.mGrid.Font;
					this.mGrid.Controls.Add(this.mTextBox);
				}
				this.mTextBox.SetBounds(rect.X + left, rect.Y, rect.Width - left - right, rect.Height);
				this.mTextBox.Text = text;
				this.mTextBox.Focus();
				if (this.mTextBox.RectangleToScreen(this.mTextBox.ClientRectangle).Contains(Cursor.Position))
				{
					int x = Cursor.Position.X;
					Point position = Cursor.Position;
					CustomProperty.mouse_event(6, (uint)x, (uint)position.Y, 0, 0);
					int num = Cursor.Position.X;
					position = Cursor.Position;
					CustomProperty.mouse_event(2, (uint)num, (uint)position.Y, 0, 0);
				}
			}
		}

		public void DestroyEditor()
		{
			if (this.mItem != null)
			{
				this.mItem = null;
				this.mGrid.Controls.Clear();
				this.mGrid.Invalidate();
				this.mTextBox = null;
				this.mGrid = null;
			}
		}

		public virtual void DoubleClick(MouseEventArgs e)
		{
			if (this.mTextBox != null)
			{
				this.mTextBox.Focus();
				this.mTextBox.SelectAll();
			}
		}

		protected void DrawText(string text, Font font, Graphics g, Rectangle rect, int x = 0, int y = 0, Color? colour = null)
		{
			rect.X = rect.X + (int)g.Transform.OffsetX - 4 + x;
			rect.Y = rect.Y + (int)g.Transform.OffsetY + 1 + y;
			Graphics graphic = g;
			string str = text;
			Font font1 = font;
			Rectangle rectangle = rect;
			Color? nullable = colour;
			TextRenderer.DrawText(graphic, str, font1, rectangle, (nullable.HasValue ? nullable.GetValueOrDefault() : Color.Black), TextFormatFlags.Default);
		}

		public virtual string getAsString(object value)
		{
			return value.ToString();
		}

		[DllImport("user32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Auto, ExactSpelling=false)]
		private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

		public virtual void MouseDown(MouseEventArgs e)
		{
		}

		public virtual void MouseMove(MouseEventArgs e)
		{
		}

		public virtual void MouseUp(MouseEventArgs e)
		{
		}

		private void mTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				PropertyGrid.PropertyGrid propertyGrid = this.mGrid;
				PropertyGrid.PropertyGrid.Section section = this.mSection;
				PropertyGrid.PropertyGrid.Item item = this.mItem;
				this.Apply();
				propertyGrid.FirePropertyKeyEnter(section, item);
				e.SuppressKeyPress = true;
			}
			else if (e.KeyCode == Keys.Escape)
			{
				this.DestroyEditor();
				e.SuppressKeyPress = true;
			}
		}

		private void mTextBox_LostFocus(object sender, EventArgs e)
		{
			if ((this.mGrid == null || this.mGrid.Focused ? false : !this.FocusLocked))
			{
				this.Apply();
			}
		}

		private void mTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.DoubleClick(e);
		}

		public virtual void Paint(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Item item, Graphics g, Rectangle rect)
		{
			this.DrawText(this.getAsString(item.Value), grid.Font, g, rect, 2, 0, new Color?(item.TextColour));
		}

		protected void setValue(object value)
		{
			if (!this.mItem.Value.Equals(value))
			{
				object obj = this.mItem.Value;
				this.mItem.Value = value;
				this.mGrid.FirePropertyChanged(this.mSection, this.mItem, obj);
			}
		}
	}
}