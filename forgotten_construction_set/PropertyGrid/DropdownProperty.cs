using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace forgotten_construction_set.PropertyGrid
{
	public class DropdownProperty : ButtonProperty
	{
		private int mPopupX;

		private ToolStripDropDown mPopup;

		protected Control PopupControl
		{
			get;
			set;
		}

		public DropdownProperty()
		{
		}

		protected override void ButtonPressed()
		{
			if (this.PopupControl != null)
			{
				if ((this.mPopup == null ? true : !this.mPopup.Visible))
				{
					base.FocusLocked = true;
					this.mPopup = new ToolStripDropDown()
					{
						AutoSize = true,
						Padding = new Padding(1, 0, 1, 0)
					};
					ToolStripControlHost toolStripControlHost = new ToolStripControlHost(this.PopupControl);
					this.mPopup.Items.Add(toolStripControlHost);
					ToolStripDropDown toolStripDropDown = this.mPopup;
					PropertyGrid propertyGrid = this.mGrid;
					int num = this.mPopupX;
					int bottom = this.mButton.Bottom;
					Point autoScrollPosition = this.mGrid.AutoScrollPosition;
					toolStripDropDown.Show(propertyGrid, num, bottom + autoScrollPosition.Y);
					this.mPopup.Closed += new ToolStripDropDownClosedEventHandler(this.mPopup_Closed);
					this.PopupControl.Focus();
					this.onPopup();
				}
			}
		}

		public override void CreateEditor(PropertyGrid grid, PropertyGrid.Section section, PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			this.mPopupX = rect.X;
		}

		protected void hidePopup()
		{
			if (this.mPopup != null)
			{
				this.mPopup.Hide();
			}
		}

		private void mPopup_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this.mPopup = null;
			base.FocusLocked = false;
		}

		protected virtual void onPopup()
		{
		}

		public override void Paint(PropertyGrid grid, PropertyGrid.Item item, Graphics g, Rectangle rect)
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
				base.Paint(grid, item, g, rect);
			}
		}
	}
}