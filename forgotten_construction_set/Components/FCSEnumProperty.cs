using forgotten_construction_set;
using PropertyGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.Windows.Forms.VisualStyles;
namespace forgotten_construction_set.Components
{
	public class FCSEnumProperty : DropdownListProperty
	{
		private FCSEnum type;

		public FCSEnumProperty(FCSEnum e)
		{
			this.type = e;
		}

		public override void Apply()
		{
			base.setValue(this.type.parse(this.mList.SelectedItem.ToString()));
			base.DestroyEditor();
		}

		public override void CreateEditor(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Section section, PropertyGrid.PropertyGrid.Item item, Rectangle rect)
		{
			base.CreateEditor(grid, section, item, rect);
			foreach (KeyValuePair<string, int> keyValuePair in this.type)
			{
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

        public override void Paint(PropertyGrid.PropertyGrid grid, PropertyGrid.PropertyGrid.Item item, Graphics g, Rectangle rect)
        {
            if (base.Editing == item)
            {
                ComboBoxState comboBoxState = ComboBoxState.Normal;
                switch (this.mState)
                {
                    case PushButtonState.Hot:
                        {
                            comboBoxState = ComboBoxState.Hot;
                            break;
                        }
                    case PushButtonState.Pressed:
                        {
                            comboBoxState = ComboBoxState.Pressed;
                            break;
                        }
                    case PushButtonState.Disabled:
                        {
                            comboBoxState = ComboBoxState.Disabled;
                            break;
                        }
                }
                rect.X = this.mButton.X;
                rect.Width = this.mButton.Width;
                if (!Application.RenderWithVisualStyles)
                {
                    ControlPaint.DrawScrollButton(g, rect, ScrollButton.Down, (comboBoxState == ComboBoxState.Pressed ? ButtonState.Pushed : ButtonState.Normal));
                }
                else
                {
                    ComboBoxRenderer.DrawDropDownButton(g, rect, comboBoxState);
                }
            }
            else
            {
                base.DrawText("123", grid.Font, g, rect, 2, 0, new Color?(item.TextColour));
                if (base.Editing == item)
                {
                    rect.X = this.mButton.X;
                    rect.Width = this.mButton.Width;
                    ButtonRenderer.DrawButton(g, rect, this.mState);
                }
            }
        }
    }
}
