using forgotten_construction_set.Components;
using forgotten_construction_set.PropertyGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace forgotten_construction_set
{
	public class InstanceDialog : Form
	{
		private navigation nav;

		private IContainer components;

		private PropertyGrid.PropertyGrid grid;

		public GameData.Instance Instance
		{
			get;
			set;
		}

		public string InstanceName
		{
			get;
			set;
		}

		public GameData.Item Item
		{
			get;
			set;
		}

		public InstanceDialog(GameData.Item item, string inst, itemType mask, navigation nav)
		{
			this.InitializeComponent();
			this.nav = nav;
			this.grid.OnPropertyChanged += new PropertyGrid.PropertyGrid.PropertyChangedHandler(this.grid_OnPropertyChanged);
			this.grid.MouseDoubleClick += new MouseEventHandler(this.grid_MouseDoubleClick);
			this.refresh(item, inst, mask);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void grid_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.grid.SelectedItem == null)
			{
				return;
			}
			if (this.grid.SelectedItem.Property is GameDataItemProperty)
			{
				object value = this.grid.SelectedItem.Value;
				GameData.Item item = null;
				if (value is GameData.Item)
				{
					item = value as GameData.Item;
				}
				else if (value != null)
				{
					item = this.nav.ou.gameData.getItem(value.ToString());
				}
				if (item != null)
				{
					this.nav.showItemProperties(item);
					return;
				}
			}
			else if (this.grid.SelectedItem.Name == "Parent Item")
			{
				this.nav.showItemProperties(this.grid.SelectedItem.Value as GameData.Item);
			}
		}

		private void grid_OnPropertyChanged(object sender, PropertyChangedArgs e)
		{
			if (e.Item.Name == "Instance ID")
			{
				string str = e.Item.Value.ToString();
				if (str == null)
				{
					e.Item.Value = e.OldValue;
				}
				else if (!this.Item.renameInstance(this.InstanceName, str))
				{
					MessageBox.Show("此id已经存在一个实例");
					e.Item.Value = e.OldValue;
				}
				else
				{
					this.InstanceName = str;
				}
			}
			else if (e.Item.Name == "Target")
			{
				if (!(e.Item.Value is GameData.Item))
				{
					this.Instance["ref"] = e.Item.Value.ToString();
				}
				else
				{
					this.Instance["ref"] = (e.Item.Value as GameData.Item).stringID;
				}
				this.Item.refreshState();
				this.nav.refreshItemWindow(this.Item);
			}
			else if (e.Item.Data is string)
			{
				string data = (string)e.Item.Data;
				this.Instance[data] = (float)e.Item.Value;
				e.Item.TextColour = StateColours.GetStateColor(this.Instance.getState(data));
			}
			this.Item.refreshState();
			this.nav.refreshItemWindow(this.Item);
			this.nav.refreshState(this.Item);
			this.nav.HasChanges = true;
		}

		private void InitializeComponent()
		{
			this.grid = new PropertyGrid.PropertyGrid();
			base.SuspendLayout();
			this.grid.Dock = DockStyle.Fill;
			this.grid.Location = new Point(0, 0);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(254, 284);
			this.grid.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(254, 284);
			base.Controls.Add(this.grid);
			base.Name = "InstanceDialog";
			base.ShowIcon = false;
			this.Text = "实例";
			base.ResumeLayout(false);
		}

		public void refresh(GameData.Item item, string instanceID, itemType mask)
		{
			this.Item = item;
			this.InstanceName = instanceID;
			this.Instance = item.getInstance(instanceID);
			GameData gameDatum = this.nav.ou.gameData;
			string str = this.Instance.sdata["ref"];
			object obj = gameDatum.getItem(str);
			if (obj == null)
			{
				obj = str;
			}
			bool state = this.Instance.getState() == GameData.State.LOCKED;
			Color stateColor = StateColours.GetStateColor((state ? GameData.State.LOCKED : GameData.State.ORIGINAL));
			string str1 = "";
			if (mask == itemType.NULL_ITEM)
			{
				str1 = "is node=true";
			}
			this.grid.addItem("Base", "Parent Item", this.Item, "The item this instance belongs to", new Color?(SystemColors.GrayText), false);
			this.grid.addItem("Base", "Instance ID", this.InstanceName, "Unique identifier for this instance. Works the same as StringID but for instances", new Color?(stateColor), !state);
			this.grid.addItem("Base", "Target", obj, "Game object that is instanced", new Color?(stateColor), !state).Property = new GameDataItemProperty(gameDatum, mask, str1);
			GameData.State state1 = this.Instance.getState("ref");
			if (state && state1 != GameData.State.INVALID)
			{
				state1 = GameData.State.LOCKED;
			}
			this.grid.getItem("Target").TextColour = StateColours.GetStateColor(state1);
			this.setItem("Position", "X", "x", "Instance position");
			this.setItem("Position", "Y", "y", "Instance position");
			this.setItem("Position", "Z", "z", "Instance position");
			this.setItem("Orientation", "W", "qw", "Instance oriantetion (quaternion)");
			this.setItem("Orientation", "X", "qx", "Instance oriantetion (quaternion)");
			this.setItem("Orientation", "Y", "qy", "Instance oriantetion (quaternion)");
			this.setItem("Orientation", "Z", "qz", "Instance oriantetion (quaternion)");
			foreach (KeyValuePair<string, GameData.TripleInt> keyValuePair in this.Instance.referenceData("states", false))
			{
				GameData.Item item1 = this.nav.ou.gameData.getItem(keyValuePair.Key);
				if (item1 == null)
				{
					continue;
				}
				PropertyGrid.PropertyGrid propertyGrid = this.grid;
				itemType _itemType = item1.type;
				Color? nullable = null;
				PropertyGrid.PropertyGrid.Item gameDataItemProperty = propertyGrid.addItem("State Data", _itemType.ToString(), item1, "Instance state data", nullable, true);
				gameDataItemProperty.Property = new GameDataItemProperty(gameDatum, item1.type, "");
				gameDataItemProperty.Editable = false;
			}
			this.grid.AutosizeDivider();
		}

		private PropertyGrid.PropertyGrid.Item setItem(string section, string name, string key, string desc)
		{
			Color? nullable = null;
			PropertyGrid.PropertyGrid.Item stateColor = this.grid.addItem(section, name, this.Instance[key], desc, nullable, true);
			stateColor.Data = key;
			stateColor.TextColour = StateColours.GetStateColor(this.Instance.getState(key));
			if (this.Instance.getState() == GameData.State.LOCKED)
			{
				stateColor.TextColour = StateColours.GetStateColor(GameData.State.LOCKED);
				stateColor.Editable = false;
			}
			return stateColor;
		}
	}
}