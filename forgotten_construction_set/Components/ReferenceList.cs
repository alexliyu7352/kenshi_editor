using System;
using forgotten_construction_set.PropertyGrid;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace forgotten_construction_set.Components
{
    public partial class ReferenceList : UserControl
    {

        public ReferenceList()
        {
            this.readOnly = false;
            this.InitializeComponent();
            this.SelectionControl = this.selection;
            this.DescriptionControl = this.description;
            this.Exclusions = new ArrayList();
            this.grid.DoubleClick += new EventHandler(this.grid_DoubleClick);
            this.grid.OnPropertySelected += new PropertyGrid.PropertyGrid.PropertySelectedHandler(this.grid_OnPropertySelected);
            this.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
        }

        public Control DescriptionControl
        {
            get;
            set;
        }

        public ArrayList Exclusions
        {
            get;
            set;
        }

        public GameData.Item Item
        {
            get;
            private set;
        }

        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
            set
            {
                this.readOnly = value;
                this.splitContainer1.Panel1Collapsed = value;
                this.grid.Enabled = !value;
            }
        }

        public Control SelectionControl
        {
            get;
            set;
        }
    }
}
