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


namespace forgotten_construction_set.PropertyGrid
{
    public partial class PropertyGrid : ScrollableControl
    {
        public PropertyGrid()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            base.ResizeRedraw = true;
            this.Sections = new List<forgotten_construction_set.PropertyGrid.PropertyGrid.Section>();
            base.LostFocus += new EventHandler(this.PropertyGrid_LostFocus);
            base.GotFocus += new EventHandler(this.PropertyGrid_GotFocus);
            this.DrawLines = true;
            this.LineHeight = (int)this.Font.GetHeight() + 4;
            this.MarginSize = 12;
            this.DividerPosition = 100;
            this.LineColour = SystemColors.ControlLight;
            this.FocusColour = SystemColors.Highlight;
            this.FocusText = SystemColors.HighlightText;
            this.DefaultType = new CustomProperty();
            this.EnumType = new EnumProperty();
            this.Types = new Dictionary<Type, CustomProperty>()
            {
                { typeof(bool), new BooleanProperty() },
                { typeof(Color), new ColorProperty() },
                { typeof(int), new NumberProperty() },
                { typeof(float), new NumberProperty() }
            };
        }

        public class Item
        {
            public string Name;

            public string Description;

            public bool Editable;

            public bool Visible;

            public object Value;

            public object Data;

            public Color TextColour;

            public CustomProperty Property;

            public Item()
            {
            }
        }

        public class Section
        {
            public string Name;

            public bool Collapsed;

            public List<PropertyGrid.Item> Items;

            public Color TextColour;

            public Color MarginColor;

            public Color BackColour1;

            public Color BackColour2;

            public bool Visible;

            public Section()
            {
            }
        }
    }


}
