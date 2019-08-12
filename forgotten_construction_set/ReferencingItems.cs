using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class ReferencingItems : ItemDialog
	{
		public ReferencingItems(GameData source, GameData.Item item) : base("", source, itemType.NULL_ITEM, false, "", itemType.NULL_ITEM)
		{
			this.Text = string.Concat("Items referencing ", item.Name);
			this.listView1.AddColumn("Type", "Type");
			ArrayList arrayLists = new ArrayList();
			source.getReferences(item, arrayLists);
			base.Source = new GameData();
			foreach (GameData.Item arrayList in arrayLists)
			{
				base.Source.items.Add(arrayList.stringID, arrayList);
			}
			this.listView1.UpdateItems(base.Source, new itemType?(base.Type), this.filter.Text);
			this.listView1.MouseDoubleClick += new MouseEventHandler(this.listView1_MouseDoubleClick);
		}

		private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			base.openItem_Click(sender, new EventArgs());
		}
	}
}