using PropertyGrid;
using System;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class ExtendedTranslationText : ButtonProperty
	{
		public ExtendedTranslationText()
		{
		}

		protected override void ButtonPressed()
		{
			PropertyGrid.PropertyGrid.Item item = this.mItem;
			PropertyGrid.PropertyGrid propertyGrid = this.mGrid;
			TranslationManager.TranslationDialogueLine.Line data = this.mItem.Data as TranslationManager.TranslationDialogueLine.Line;
			TextDialog textDialog = new TextDialog(this.mItem.Name, data.Translation, data.Original);
			if (textDialog.ShowDialog() == DialogResult.OK)
			{
				this.mItem = item;
				this.mGrid = propertyGrid;
				char[] chrArray = new char[] { ' ', '\n', '\r' };
				base.setValue(textDialog.Value.TrimEnd(chrArray));
				this.mItem = null;
				this.mGrid = null;
			}
		}
	}
}