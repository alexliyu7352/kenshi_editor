using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using PropertyGrid;

namespace forgotten_construction_set.Components
{
    class MyPropertyGrid: PropertyGrid.PropertyGrid
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (PropertyGrid.PropertyGrid.Item item in section.Items)
            {

            }
                base.OnPaint(e);
        }
    }
}
