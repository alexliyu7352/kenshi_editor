using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set.PropertyGrid
{
	public class FileProperty : ButtonProperty
	{
		public string Filter
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public FileProperty(string title = "Select file", string filter = "All files|*")
		{
			this.Title = title;
			this.Filter = filter;
		}

		protected override void ButtonPressed()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Title = this.Title,
				Filter = this.Filter
			};
			base.FocusLocked = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				base.setValue(openFileDialog.FileName);
				if (this.mTextBox != null)
				{
					this.mTextBox.Text = this.getAsString(this.mItem.Value);
				}
			}
			base.FocusLocked = false;
		}

		public override void Paint(PropertyGrid grid, PropertyGrid.Item item, Graphics g, Rectangle rect)
		{
			base.Paint(grid, item, g, rect);
			if (base.Editing == item)
			{
				g.DrawString("...", grid.Font, Brushes.Black, new PointF((float)(this.mButton.X + 3), (float)rect.Y));
			}
		}
	}
}