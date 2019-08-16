using forgotten_construction_set.PropertyGrid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
    public partial class ObjectPropertyBox : UserControl
    {

        public ObjectPropertyBox()
        {
            this.InitializeComponent();
            this.SelectionControl = this.selection;
            this.DescriptionControl = this.description;
            this.grid.AddPropertyType(typeof(GameData.vec), new VecProperty());
            this.grid.AddPropertyType(typeof(GameData.quat), new QuatProperty());
        }

        public Control DescriptionControl
        {
            get;
            set;
        }

        public GameData.Item Item
        {
            get;
            set;
        }

        public Control SelectionControl
        {
            get;
            set;
        }
    }
}
