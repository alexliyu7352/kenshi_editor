using forgotten_construction_set.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace forgotten_construction_set.dialog
{
    public partial class Conversation : Form
    {

        public GameData.Item Item
        {
            get;
            set;
        }

        public GameData.Item SelectedItem
        {
            get
            {
                if (this.tree.SelectedNode == null)
                {
                    return null;
                }
                return (GameData.Item)this.tree.SelectedNode.Tag;
            }
        }
        public Conversation()
        {
        }
        public Conversation(GameData.Item item, navigation nav)
        {
            this.InitializeComponent();
            this.Item = item;
            this.nav = nav;
            this.gameData = nav.ou.gameData;
            this.referenceList1.Exclusions.Add("lines");
            this.referenceList1.Exclusions.Add("conditions");
            this.referenceList1.Exclusions.Add("effects");
            this.referenceList1.setup(null, nav);
            this.lineProperties.setup(null, nav);
            this.lineProperties.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
            this.conditionControl1.setup(null, nav);
            this.conditionControl1.ChangeEvent += new ConditionControl.ConditionsChangedHandler(this.conditionControl1_ChangeEvent);
            this.referenceList1.ChangeEvent += new ReferenceList.ChangeNotifier(this.referenceList1_ChangeEvent);
            this.checkBox1.Checked = (!item.ContainsKey("once only") ? false : item.bdata["once only"]);
            this.checkBox2.Checked = (!item.ContainsKey("for enemies") ? false : item.bdata["for enemies"]);
            this.conversationName.Text = this.Item.Name;
            bool state = this.Item.getState() != GameData.State.LOCKED;
            this.conversationName.Enabled = state;
            this.checkBox1.Enabled = state;
            this.checkBox2.Enabled = state;
            this.btnAddChild.Enabled = state;
            this.btnAddInterjection.Enabled = state;
            TreeNode treeNode = this.tree.Nodes.Add("Dialogue");
            treeNode.Tag = this.Item;
            Conversation.createConversationTree(this.gameData, this.Item, treeNode.Nodes);
            this.tree.ExpandAll();
            this.countLines();
            string[] names = Enum.GetNames(typeof(DialogActionEnum));
            for (int i = 0; i < (int)names.Length; i++)
            {
                string str = names[i];
                this.PossibleEffects.Items.Add(str);
            }
            this.effectsPanel.Enabled = false;

        }
    }
}
