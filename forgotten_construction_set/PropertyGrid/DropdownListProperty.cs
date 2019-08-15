using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace forgotten_construction_set.PropertyGrid
{
	public class DropdownListProperty : DropdownProperty
	{
		protected ListBox mList;

		public DropdownListProperty()
		{
		}

		public override void Apply()
		{
			if (this.mList != null)
			{
				base.setValue(this.mList.SelectedItem);
			}
			base.DestroyEditor();
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			this.mList = new ListBox();
			this.mList.Click += new EventHandler(this.list_SelectedIndexChanged);
			this.mList.MinimumSize = new Size(rect.Width - 2, 16);
			this.mList.MaximumSize = new Size(400, 400);
			this.mList.BorderStyle = BorderStyle.None;
			this.mTextBox.KeyDown += new KeyEventHandler(this.mTextBox_KeyDown);
			base.PopupControl = this.mList;
		}

		public override void DoubleClick(MouseEventArgs e)
		{
			this.mList.SelectedIndex = (this.mList.SelectedIndex + 1) % this.mList.Items.Count;
			base.setValue(this.mList.SelectedItem);
			if (this.mTextBox != null)
			{
				this.mTextBox.Text = this.getAsString(this.mItem.Value);
			}
			base.DoubleClick(e);
		}

		private void list_SelectedIndexChanged(object sender, EventArgs e)
		{
			base.setValue(this.mList.SelectedItem);
			if (this.mTextBox != null)
			{
				this.mTextBox.Text = this.getAsString(this.mItem.Value);
			}
			base.hidePopup();
		}

		private void mTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			e.SuppressKeyPress = true;
			if ((e.KeyCode != Keys.Up ? false : this.mList.SelectedIndex > 0))
			{
				this.setSelectedIndex(this.mList.SelectedIndex - 1);
			}
			else if ((e.KeyCode != Keys.Down ? true : this.mList.SelectedIndex >= this.mList.Items.Count - 1))
			{
				string str = (new KeysConverter()).ConvertToString(e.KeyCode);
				int num = 0;
				while (num < this.mList.Items.Count)
				{
					int selectedIndex = (this.mList.SelectedIndex + num + 1) % this.mList.Items.Count;
					if (!this.mList.Items[selectedIndex].ToString().StartsWith(str, StringComparison.OrdinalIgnoreCase))
					{
						num++;
					}
					else
					{
						this.setSelectedIndex(selectedIndex);
						break;
					}
				}
			}
			else
			{
				this.setSelectedIndex(this.mList.SelectedIndex + 1);
			}
		}

		protected void setSelectedIndex(int index)
		{
			this.mList.SelectedIndex = index;
			base.setValue(this.mList.SelectedItem);
			if (this.mTextBox != null)
			{
				this.mTextBox.Text = this.getAsString(this.mItem.Value);
			}
		}
	}
}