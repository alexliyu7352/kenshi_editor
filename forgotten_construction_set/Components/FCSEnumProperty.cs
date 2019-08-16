using forgotten_construction_set;
using forgotten_construction_set.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.Windows.Forms.VisualStyles;
namespace forgotten_construction_set.Components
{
	public class FCSEnumProperty : DropdownListProperty
	{
       // private Dictionary<string, string> trans = new Dictionary<string, string>();
        private FCSEnum type;
		public FCSEnumProperty(FCSEnum e)
		{
			this.type = e;
		}
        //public string parse(string s)
        //{
        //    if (!this.trans.ContainsKey(s))
        //    {
        //        return s;
        //    }
        //    return this.trans[s];
        //}

        public override void Apply()
		{
            //base.setValue(this.type.parse(parse(this.mList.SelectedItem.ToString())));
            base.setValue(this.type.parse(this.mList.SelectedItem.ToString()));
            base.DestroyEditor();
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			foreach (KeyValuePair<string, int> keyValuePair in this.type)
			{
                //string newKey = NativeTranslte.getTransValue(keyValuePair.Key);
                //this.mList.Items.Add(newKey);
                //trans.Add(newKey, keyValuePair.Key);
                this.mList.Items.Add(keyValuePair.Key);
            }
			this.mList.SelectedItem = this.getAsString(item.Value);
			if (this.mList.SelectedItem == null)
			{
				this.mList.Text = item.Value.ToString();
			}
		}

        public override void DoubleClick(MouseEventArgs e)
        {
            base.DoubleClick(e);
        }

        public override string getAsString(object value)
		{
			if (value is string || value is EnumValue)
			{
                //Console.WriteLine(value.ToString());
                return value.ToString();
			}
            string str = this.type.name((int)value) ?? value.ToString();
            //TODO 考虑这里进行汉化
            //String unTransValue = this.type.name((int)value) ?? value.ToString();
            //Console.WriteLine(unTransValue);
            //if (!(bool)NativeTranslte.enumDict.TryGetValue(unTransValue, out string str))
            //{
            //    str = unTransValue;
            //}
            return str;
		}
    }
}
