using forgotten_construction_set.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml;

namespace forgotten_construction_set
{
	public class OgreSceneImporter : Form
	{
		private navigation nav;

		private int checkCount;

		private IContainer components;

		private Button bImport;

		private Button bCancel;

		private Button bLoad;

		private BufferedTreeView tree;

		private Button bTarget;

		private ToolTip toolTip1;

		private Button bCopyNodes;

		public GameData.Item Item
		{
			get;
			set;
		}

		public string Path
		{
			get;
			set;
		}

		public OgreSceneImporter(GameData.Item item, navigation nav)
		{
			this.InitializeComponent();
			this.Item = item;
			this.nav = nav;
			this.bImport.Enabled = false;
		}

		private void bCopyNodes_Click(object sender, EventArgs e)
		{
			itemType item = this.Item.type;
			string str = "";
			ItemDialog itemDialog = new ItemDialog("Select Object To Copy Nodes From", this.nav.ou.gameData, item, true, str, itemType.NULL_ITEM);
			if (itemDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				foreach (GameData.Item item1 in itemDialog.Items)
				{
					this.Item.clearInstances();
					foreach (KeyValuePair<string, GameData.Instance> keyValuePair in item1.instanceData())
					{
						this.Item.addInstance(keyValuePair.Key, keyValuePair.Value);
					}
					this.nav.HasChanges = true;
				}
				this.nav.refreshState(this.Item);
				this.nav.HasChanges = true;
			}
			base.Close();
		}

		private void bImport_Click(object sender, EventArgs e)
		{
			GameData.Item file;
			OgreSceneImporter.Node tag;
			GameData gameDatum = this.nav.ou.gameData;
			foreach (TreeNode node in this.tree.Nodes)
			{
				if (!node.Checked)
				{
					continue;
				}
				if (((OgreSceneImporter.Node)node.Tag).target == null)
				{
					switch (this.getNodeType((OgreSceneImporter.Node)node.Tag))
					{
						case OgreSceneImporter.NodeType.PART:
						{
							OgreSceneImporter.Entity entity = node.Tag as OgreSceneImporter.Entity;
							file = gameDatum.createItem(itemType.BUILDING_PART);
							file["phs or mesh"] = new GameData.File(string.Concat(this.Path, entity.mesh));
							int? nullable = null;
							int? nullable1 = nullable;
							nullable = null;
							int? nullable2 = nullable;
							nullable = null;
							this.Item.addReference("parts", file, nullable1, nullable2, nullable);
							break;
						}
						case OgreSceneImporter.NodeType.LIGHT:
						{
							OgreSceneImporter.Light light = node.Tag as OgreSceneImporter.Light;
							file = gameDatum.createItem(itemType.LIGHT);
							file["diffuse"] = light.diffuse;
							file["specular"] = light.specular;
							file["radius"] = light.radius;
							file["brightness"] = light.power;
							file["inner"] = (float)Math.Round((double)(light.inner * 180f) / 3.14159265358979, 2);
							file["outer"] = (float)Math.Round((double)(light.outer * 180f) / 3.14159265358979, 2);
							file["falloff"] = light.falloff;
							file["type"] = light.mode;
							file["landscape"] = light.shadows;
							file["buildings"] = light.shadows;
							file["characters"] = light.shadows;
							this.Item.addInstance(light.name, file, light.x, light.y, light.z, light.qx, light.qy, light.qz, light.qw);
							break;
						}
						case OgreSceneImporter.NodeType.EFFECT:
						{
							tag = node.Tag as OgreSceneImporter.Node;
							file = gameDatum.createItem(itemType.EFFECT);
							this.Item.addInstance(tag.name, file.stringID, tag.x, tag.y, tag.z, tag.qx, tag.qy, tag.qz, tag.qw);
							break;
						}
						case OgreSceneImporter.NodeType.NODE:
						{
							tag = node.Tag as OgreSceneImporter.Node;
							this.Item.addInstance(tag.name, "", tag.x, tag.y, tag.z, tag.qx, tag.qy, tag.qz, tag.qw);
							break;
						}
					}
				}
				else
				{
					tag = node.Tag as OgreSceneImporter.Node;
					this.Item.addInstance(tag.name, tag.target, tag.x, tag.y, tag.z, tag.qx, tag.qy, tag.qz, tag.qw);
				}
				this.nav.HasChanges = true;
			}
			base.Close();
		}

		private void bLoad_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog()
			{
				Filter = "Ogre scene files|*.scene"
			};
			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.loadScene(openFileDialog.FileName);
				this.Path = openFileDialog.FileName.Remove(openFileDialog.FileName.LastIndexOf("\\") + 1);
				if (this.Path.StartsWith(this.nav.BasePath) || this.Path.StartsWith(this.nav.ModPath))
				{
					this.Path = string.Concat(".", this.Path.Substring(this.nav.RootPath.Length));
				}
			}
		}

		private void bTarget_Click(object sender, EventArgs e)
		{
			ItemDialog itemDialog;
			OgreSceneImporter.NodeType nodeType = OgreSceneImporter.NodeType.PART;
			foreach (TreeNode node in this.tree.Nodes)
			{
				if (!node.Checked)
				{
					continue;
				}
				nodeType = this.getNodeType(node.Tag as OgreSceneImporter.Node);
				goto Label0;
			}
		Label0:
			switch (nodeType)
			{
				case OgreSceneImporter.NodeType.PART:
				{
					itemDialog = new ItemDialog("Parts", this.nav.ou.gameData, itemType.BUILDING_PART, false, "", itemType.NULL_ITEM);
					break;
				}
				case OgreSceneImporter.NodeType.LIGHT:
				{
					itemDialog = new ItemDialog("Lights", this.nav.ou.gameData, itemType.LIGHT, false, "", itemType.NULL_ITEM);
					break;
				}
				case OgreSceneImporter.NodeType.EFFECT:
				{
					itemDialog = new ItemDialog("Effects", this.nav.ou.gameData, itemType.EFFECT, false, "", itemType.NULL_ITEM);
					break;
				}
				case OgreSceneImporter.NodeType.NODE:
				{
					itemDialog = new ItemDialog("Nodes", this.nav.ou.gameData, itemType.BUILDING, false, "is node=true", itemType.NULL_ITEM);
					break;
				}
				default:
				{
					return;
				}
			}
			if (itemDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				foreach (TreeNode str in this.tree.Nodes)
				{
					if (!str.Checked || (!(str.Tag is OgreSceneImporter.Light) || nodeType != OgreSceneImporter.NodeType.LIGHT) && (!(str.Tag is OgreSceneImporter.Entity) || nodeType != OgreSceneImporter.NodeType.PART) && (!(str.Tag is OgreSceneImporter.Node) || nodeType != OgreSceneImporter.NodeType.EFFECT && nodeType != OgreSceneImporter.NodeType.NODE))
					{
						continue;
					}
					OgreSceneImporter.Node tag = (OgreSceneImporter.Node)str.Tag;
					tag.target = itemDialog.Items[0];
					str.Text = tag.ToString();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private OgreSceneImporter.NodeType getNodeType(OgreSceneImporter.Node n)
		{
			if (n is OgreSceneImporter.Light)
			{
				return OgreSceneImporter.NodeType.LIGHT;
			}
			if (n is OgreSceneImporter.Entity)
			{
				return OgreSceneImporter.NodeType.PART;
			}
			if (n != null && n.name.ToLower().Contains("particle"))
			{
				return OgreSceneImporter.NodeType.EFFECT;
			}
			return OgreSceneImporter.NodeType.NODE;
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.bImport = new Button();
			this.bCancel = new Button();
			this.bLoad = new Button();
			this.bTarget = new Button();
			this.toolTip1 = new ToolTip(this.components);
			this.tree = new BufferedTreeView();
			this.bCopyNodes = new Button();
			base.SuspendLayout();
			this.bImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.bImport.Location = new Point(368, 237);
			this.bImport.Name = "bImport";
			this.bImport.Size = new System.Drawing.Size(75, 23);
			this.bImport.TabIndex = 0;
			this.bImport.Text = "Import";
			this.toolTip1.SetToolTip(this.bImport, "Import selected items.");
			this.bImport.UseVisualStyleBackColor = true;
			this.bImport.Click += new EventHandler(this.bImport_Click);
			this.bCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bCancel.Location = new Point(287, 237);
			this.bCancel.Name = "bCancel";
			this.bCancel.Size = new System.Drawing.Size(75, 23);
			this.bCancel.TabIndex = 1;
			this.bCancel.Text = "Cancel";
			this.bCancel.UseVisualStyleBackColor = true;
			this.bLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.bLoad.Location = new Point(12, 237);
			this.bLoad.Name = "bLoad";
			this.bLoad.Size = new System.Drawing.Size(92, 23);
			this.bLoad.TabIndex = 2;
			this.bLoad.Text = "Load Scene File";
			this.toolTip1.SetToolTip(this.bLoad, "Load ogre scfene to import items");
			this.bLoad.UseVisualStyleBackColor = true;
			this.bLoad.Click += new EventHandler(this.bLoad_Click);
			this.bTarget.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.bTarget.Enabled = false;
			this.bTarget.Location = new Point(201, 237);
			this.bTarget.Name = "bTarget";
			this.bTarget.Size = new System.Drawing.Size(63, 22);
			this.bTarget.TabIndex = 4;
			this.bTarget.Text = "Target";
			this.toolTip1.SetToolTip(this.bTarget, "Set target item for checked items.\r\nLeave blank to create new targets.");
			this.bTarget.UseVisualStyleBackColor = true;
			this.bTarget.Click += new EventHandler(this.bTarget_Click);
			this.tree.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.tree.CheckBoxes = true;
			this.tree.HideSelection = false;
			this.tree.Location = new Point(12, 12);
			this.tree.Name = "tree";
			this.tree.ShowRootLines = false;
			this.tree.Size = new System.Drawing.Size(431, 219);
			this.tree.TabIndex = 3;
			this.tree.AfterCheck += new TreeViewEventHandler(this.tree_AfterCheck);
			this.tree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(this.tree_NodeMouseDoubleClick);
			this.bCopyNodes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.bCopyNodes.Location = new Point(110, 237);
			this.bCopyNodes.Name = "bCopyNodes";
			this.bCopyNodes.Size = new System.Drawing.Size(75, 23);
			this.bCopyNodes.TabIndex = 5;
			this.bCopyNodes.Text = "CopyFrom";
			this.bCopyNodes.UseVisualStyleBackColor = true;
			this.bCopyNodes.Click += new EventHandler(this.bCopyNodes_Click);
			base.AcceptButton = this.bImport;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.bCancel;
			base.ClientSize = new System.Drawing.Size(455, 272);
			base.Controls.Add(this.bCopyNodes);
			base.Controls.Add(this.bTarget);
			base.Controls.Add(this.tree);
			base.Controls.Add(this.bLoad);
			base.Controls.Add(this.bCancel);
			base.Controls.Add(this.bImport);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(385, 200);
			base.Name = "OgreSceneImporter";
			base.ShowIcon = false;
			this.Text = "Ogre Scene Importer";
			base.ResumeLayout(false);
		}

		public bool loadScene(string filename)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(filename);
			XmlElement documentElement = xmlDocument.DocumentElement;
			this.tree.Nodes.Clear();
			XmlElement xmlElement = null;
			foreach (XmlElement childNode in documentElement.ChildNodes)
			{
				if (childNode.Name != "nodes")
				{
					continue;
				}
				xmlElement = childNode;
				goto Label0;
			}
		Label0:
			if (xmlElement == null)
			{
				MessageBox.Show("Scene is empty", "Ogre scene importer", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return true;
			}
			foreach (XmlElement xmlElement1 in xmlElement)
			{
				if (xmlElement1.Name != "node")
				{
					continue;
				}
				XmlNodeList elementsByTagName = xmlElement1.GetElementsByTagName("entity");
				XmlNodeList xmlNodeLists = xmlElement1.GetElementsByTagName("light");
				foreach (XmlElement xmlElement2 in elementsByTagName)
				{
					OgreSceneImporter.Entity entity = new OgreSceneImporter.Entity();
					this.parseNode(entity, xmlElement1);
					entity.mesh = xmlElement2.GetAttribute("meshFile");
					this.tree.Nodes.Add(string.Concat(new string[] { "ENTITY ", entity.name, " [", entity.mesh, "]" })).Tag = entity;
				}
				foreach (XmlElement xmlElement3 in xmlNodeLists)
				{
					OgreSceneImporter.Light light = new OgreSceneImporter.Light();
					this.parseNode(light, xmlElement1);
					if (xmlElement3.GetAttribute("type") != "spot")
					{
						light.mode = head.LightType.POINT;
					}
					else
					{
						light.mode = head.LightType.SPOT;
					}
					light.shadows = xmlElement3.GetAttribute("castShadows") == "true";
					float.TryParse(xmlElement3.GetAttribute("power"), out light.power);
					foreach (XmlElement childNode1 in xmlElement3.ChildNodes)
					{
						if (childNode1.Name == "colourDiffuse")
						{
							light.diffuse = this.parseColour(childNode1);
						}
						else if (childNode1.Name == "colourSpecular")
						{
							light.specular = this.parseColour(childNode1);
						}
						else if (childNode1.Name != "lightAttenuation")
						{
							if (childNode1.Name != "lightRange")
							{
								continue;
							}
							float.TryParse(childNode1.GetAttribute("inner"), out light.inner);
							float.TryParse(childNode1.GetAttribute("outer"), out light.outer);
							float.TryParse(childNode1.GetAttribute("falloff"), out light.falloff);
							if (light.falloff > 0f)
							{
								continue;
							}
							light.falloff = 1f;
						}
						else
						{
							float.TryParse(childNode1.GetAttribute("range"), out light.radius);
						}
					}
					this.tree.Nodes.Add(string.Concat(light.mode.ToString(), "LIGHT ", light.name)).Tag = light;
				}
				if (elementsByTagName.Count != 0 || xmlNodeLists.Count != 0)
				{
					continue;
				}
				OgreSceneImporter.Node node = new OgreSceneImporter.Node();
				this.parseNode(node, xmlElement1);
				this.multiplyQuaternion(node, 0f, -0.707107f, 0f, 0.707107f);
				string str = (this.getNodeType(node) == OgreSceneImporter.NodeType.EFFECT ? "EFFECT" : "NODE");
				this.tree.Nodes.Add(string.Concat(str, " ", node.name)).Tag = node;
			}
			foreach (object obj in this.tree.Nodes)
			{
				((TreeNode)obj).Checked = true;
			}
			return true;
		}

		private void multiplyQuaternion(OgreSceneImporter.Node q, float x, float y, float z, float w)
		{
			float single = q.qw * w - q.qx * x - q.qy * y - q.qz * z;
			float single1 = q.qw * x + q.qx * w + q.qy * z - q.qz * y;
			float single2 = q.qw * y + q.qy * w + q.qz * x - q.qx * z;
			float single3 = q.qw * z + q.qz * w + q.qx * y - q.qy * x;
			q.qw = single;
			q.qx = single1;
			q.qy = single2;
			q.qz = single3;
		}

		private Color parseColour(XmlElement e)
		{
			float single = 1f;
			float single1 = 1f;
			float single2 = 1f;
			float.TryParse(e.GetAttribute("r"), out single);
			float.TryParse(e.GetAttribute("g"), out single1);
			float.TryParse(e.GetAttribute("b"), out single2);
			return Color.FromArgb((int)(single * 255f), (int)(single1 * 255f), (int)(single2 * 255f));
		}

		private void parseNode(OgreSceneImporter.Node n, XmlElement e)
		{
			n.name = e.GetAttribute("name");
			foreach (XmlElement childNode in e.ChildNodes)
			{
				if (childNode.Name != "position")
				{
					if (childNode.Name != "rotation")
					{
						continue;
					}
					float.TryParse(childNode.GetAttribute("qx"), out n.qx);
					float.TryParse(childNode.GetAttribute("qy"), out n.qy);
					float.TryParse(childNode.GetAttribute("qz"), out n.qz);
					float.TryParse(childNode.GetAttribute("qw"), out n.qw);
				}
				else
				{
					float.TryParse(childNode.GetAttribute("x"), out n.x);
					float.TryParse(childNode.GetAttribute("y"), out n.y);
					float.TryParse(childNode.GetAttribute("z"), out n.z);
				}
			}
		}

		private void tree_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (!e.Node.Checked)
			{
				this.checkCount--;
			}
			else
			{
				this.checkCount++;
			}
			this.bImport.Enabled = this.checkCount > 0;
			this.bTarget.Enabled = this.checkCount > 0;
		}

		private void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			ItemDialog itemDialog;
			OgreSceneImporter.Node tag = (OgreSceneImporter.Node)e.Node.Tag;
			switch (this.getNodeType(tag))
			{
				case OgreSceneImporter.NodeType.PART:
				{
					itemDialog = new ItemDialog("Parts", this.nav.ou.gameData, itemType.BUILDING_PART, false, "", itemType.NULL_ITEM);
					break;
				}
				case OgreSceneImporter.NodeType.LIGHT:
				{
					itemDialog = new ItemDialog("Lights", this.nav.ou.gameData, itemType.LIGHT, false, "", itemType.NULL_ITEM);
					break;
				}
				case OgreSceneImporter.NodeType.EFFECT:
				{
					itemDialog = new ItemDialog("Effects", this.nav.ou.gameData, itemType.EFFECT, false, "", itemType.NULL_ITEM);
					break;
				}
				case OgreSceneImporter.NodeType.NODE:
				{
					itemDialog = new ItemDialog("Nodes", this.nav.ou.gameData, itemType.BUILDING, false, "is node=true", itemType.NULL_ITEM);
					break;
				}
				default:
				{
					return;
				}
			}
			if (itemDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				tag.target = itemDialog.Items[0];
				e.Node.Text = tag.ToString();
			}
		}

		private class Entity : OgreSceneImporter.Node
		{
			public string mesh;

			public Entity()
			{
			}

			public override string ToString()
			{
				return string.Concat(new string[] { "MESH ", this.name, " [", this.mesh, "]", base.targetString() });
			}
		}

		private class Light : OgreSceneImporter.Node
		{
			public head.LightType mode;

			public Color diffuse;

			public Color specular;

			public bool shadows;

			public float power;

			public float radius;

			public float inner;

			public float outer;

			public float falloff;

			public Light()
			{
			}

			public override string ToString()
			{
				return string.Concat(this.mode.ToString(), "LIGHT ", this.name, base.targetString());
			}
		}

		private class Node
		{
			public string name;

			public float x;

			public float y;

			public float z;

			public float qx;

			public float qy;

			public float qz;

			public float qw;

			public GameData.Item target;

			public Node()
			{
			}

			protected string targetString()
			{
				if (this.target == null)
				{
					return "";
				}
				return string.Concat(" (", this.target.Name, ")");
			}

			public override string ToString()
			{
				return string.Concat("NODE ", this.name, this.targetString());
			}
		}

		private enum NodeType
		{
			PART,
			LIGHT,
			EFFECT,
			NODE
		}
	}
}