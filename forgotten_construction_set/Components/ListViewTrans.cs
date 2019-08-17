using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace forgotten_construction_set.Components
{
   public class ListViewTrans:ListView
    {
        protected ToolTip transTip;
        protected string oldTrans;

        public ListViewTrans()
        {
            transTip = new ToolTip();
        }

        public void SetTipMessage(string strTip)
        {
            //  transTip.SetToolTip(this.mList, strTip);
            // Check if the ToolTip's text isn't already the one
            // we are now processing.
            if (oldTrans != strTip)
            {
                oldTrans = strTip;
                string newTrans = NativeTranslte.getTransEvent(strTip);
                if (transTip.GetToolTip(this) != newTrans)
                {
                    // If it isn't, then a new item needs to be
                    // displayed on the toolTip. Update it.
                    transTip = new ToolTip();
                    transTip.SetToolTip(this, newTrans);
                }
            }
        }

        protected override void OnItemMouseHover(ListViewItemMouseHoverEventArgs e)
        {
            base.OnItemMouseHover(e);
            SetTipMessage(e.Item.Text);
        }
    }
}
