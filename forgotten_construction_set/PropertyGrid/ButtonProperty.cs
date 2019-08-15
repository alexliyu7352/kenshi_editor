using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace forgotten_construction_set.PropertyGrid
{
    public abstract class ButtonProperty : CustomProperty
    {
        protected int ButtonWidth = 0;

        protected bool Editable = true;

        protected bool Locked = false;

        protected Rectangle mButton;

        protected PushButtonState mState;

        private bool mCapture;

        protected ButtonProperty()
        {
        }

        protected abstract void ButtonPressed();

        public override void CreateEditor(PropertyGrid grid, PropertyGrid.Section section, PropertyGrid.Item item, Rectangle rect)
        {
            int num = (this.ButtonWidth == 0 ? rect.Height + 4 : this.ButtonWidth);
            this.mGrid = grid;
            this.mItem = item;
            this.mSection = section;
            int right = rect.Right - num;
            int y = rect.Y;
            Point autoScrollPosition = grid.AutoScrollPosition;
            this.mButton = new Rectangle(right, y - autoScrollPosition.Y, num, rect.Height);
            this.mState = PushButtonState.Normal;
            this.mCapture = false;
            if (this.mButton.X < rect.Left)
            {
                this.mButton.Width = this.mButton.Right - rect.Left;
                this.mButton.X = rect.Left;
            }
            if (this.Editable)
            {
                base.CreateTextbox(this.getAsString(item.Value), rect, 2, num);
                if (this.mTextBox != null)
                {
                    this.mTextBox.ReadOnly = this.Locked;
                }
            }
        }

        public override void MouseDown(MouseEventArgs e)
        {
            if (this.mButton.Contains(e.Location))
            {
                int num = 1;
                bool flag = true;
                this.mCapture = true;
                this.mGrid.Capture = flag;
                this.mState = PushButtonState.Pressed;
                this.mGrid.Invalidate();
            }
        }

        public override void MouseMove(MouseEventArgs e)
        {
            PushButtonState pushButtonState = this.mState;
            bool flag = this.mButton.Contains(e.Location);
            if ((!flag ? true : !this.mCapture))
            {
                pushButtonState = (!flag ? PushButtonState.Default : PushButtonState.Hot);
            }
            else
            {
                pushButtonState = PushButtonState.Pressed;
            }
            if (pushButtonState != this.mState)
            {
                this.mState = pushButtonState;
                this.mGrid.Invalidate();
            }
        }

        public override void MouseUp(MouseEventArgs e)
        {
            if ((!this.mCapture ? false : this.mButton.Contains(e.Location)))
            {
                this.mState = PushButtonState.Hot;
                int num = 0;
                bool flag = false;
                this.mCapture = false;
                this.mGrid.Capture = flag;
                this.mGrid.Invalidate();
                this.ButtonPressed();
            }
        }

        public override void Paint(PropertyGrid grid, PropertyGrid.Item item, Graphics g, Rectangle rect)
        {
            if (base.Editing != item)
            {
                base.Paint(grid, item, g, rect);
            }
            else if (this.mButton.Width > 0)
            {
                rect.X = this.mButton.X;
                rect.Width = this.mButton.Width;
                ButtonRenderer.DrawButton(g, rect, this.mState);
            }
        }
    }
}