using System;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class wordswaps : conversation
	{
		public wordswaps(GameData.Item item, navigation nav) : base(item, nav)
		{
			this.btnAddChild.Enabled = false;
			this.btnAddInterjection.Enabled = false;
			this.addChild.Visible = false;
			this.addInterjection.Visible = false;
			this.Text = "Word Swapper";
			this.tree.Nodes[0].Text = "Word Swaps";
			this.tree.AfterSelect += new TreeViewEventHandler(this.tree_AfterSelect);
			this.btnAddChild.Click -= new EventHandler(this.addChild_Click);
			this.btnAddChild.Click += new EventHandler(this.addChild_Click);
			this.addChild.Click -= new EventHandler(this.addChild_Click);
			this.addChild.Click += new EventHandler(this.addChild_Click);
		}

		protected new void addChild_Click(object sender, EventArgs e)
		{
			this.tree.SelectedNode = this.tree.Nodes[0];
			base.addChild_Click(sender, e);
		}

		private void tree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.btnAddInterjection.Enabled = false;
			this.conditionControl1.Enabled = this.tree.SelectedNode.Parent != null;
			this.effectsPanel.Enabled = false;
		}
	}
}