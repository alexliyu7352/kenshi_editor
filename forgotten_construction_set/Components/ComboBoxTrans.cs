using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
    public class ComboBoxTrans:ComboBox
    {
        protected ToolTip transTip;
        protected string oldTrans;
        public ComboBoxTrans()
        {
            transTip = new ToolTip();
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;

        }

        public void SetTipMessage(string strTip, DrawItemEventArgs e)
        {
            //  transTip.SetToolTip(this.mList, strTip);
            // Check if the ToolTip's text isn't already the one
            // we are now processing.
            if (oldTrans != strTip)
            {
                oldTrans = strTip;
                string newTrans = NativeTranslte.getTransValue(strTip);
                if (transTip.GetToolTip(this) != newTrans)
                {
                    // If it isn't, then a new item needs to be
                    // displayed on the toolTip. Update it.
                    transTip.Show(newTrans, this, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
                }
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            // 绘制背景
            e.DrawBackground();
            //绘制列表项目
            e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, System.Drawing.Brushes.Black, e.Bounds);
            //将高亮的列表项目的文字传递到toolTip1(之前建立ToolTip的一个实例)
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
             //   tt.Show(cb.Items[e.Index].ToString(), cb, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + e.Bounds.Height);
                SetTipMessage(this.Items[e.Index].ToString(), e);
            }
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);
            transTip.Hide(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //int index = mList.IndexFromPoint(e.Location);
            //// Check if the index is valid.
            //if (index != -1 && index < mList.Items.Count)
            //{
            //    SetTipMessage(mList.Items[index].ToString());
            //}
        }
    }
}
