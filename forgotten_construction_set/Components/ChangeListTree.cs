using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace forgotten_construction_set.Components
{
    public partial class ChangeListTree : ListView
    {
        public ItemFilter Filter
        {
            get;
            set;
        }

        public GameData GameData
        {
            get;
            set;
        }

        public List<ChangeListTree.RootNode> Nodes
        {
            get;
            set;
        }

        public bool ShowChanged
        {
            get;
            set;
        }

        public bool ShowDeleted
        {
            get;
            set;
        }

        public bool ShowNew
        {
            get;
            set;
        }

        public string SortKey
        {
            get;
            set;
        }

        public ChangeListTree()
        {
            this.setup();
        }

        public ChangeListTree(IContainer container)
        {
            container.Add(this);
            this.setup();
        }

        public event ChangeListTree.ItemCheckedEventHandler ItemChecked;

        public class ChangeData : ChangeListTree.TreeNode
        {
            public Color Colour
            {
                get;
                set;
            }

            public string Key
            {
                get;
                set;
            }

            public object NewValue
            {
                get;
                set;
            }

            public object OldValue
            {
                get;
                set;
            }

            public string Section
            {
                get;
                set;
            }

            public string Text
            {
                get;
                set;
            }

            public ChangeListTree.ChangeType Type
            {
                get;
                set;
            }

            public ChangeData(ChangeListTree.ChangeType type, object oldVal, object newVal, GameData.State state)
            {
                this.Type = type;
                this.OldValue = oldVal;
                this.NewValue = newVal;
                this.Colour = StateColours.GetStateColor(state);
            }

            public ChangeData(ChangeListTree.ChangeType type, string key, object oldVal, object newVal, GameData.State state)
            {
                this.Type = type;
                this.OldValue = oldVal;
                this.NewValue = newVal;
                this.Key = key;
                this.Colour = StateColours.GetStateColor(state);
            }

            public ChangeData(ChangeListTree.ChangeType type, string section, string key, object oldVal, object newVal, GameData.State state)
            {
                this.Type = type;
                this.OldValue = oldVal;
                this.NewValue = newVal;
                this.Key = key;
                this.Section = section;
                this.Colour = StateColours.GetStateColor(state);
            }

            public ChangeData(ChangeListTree.ChangeType type, string key, GameData.State state)
            {
                this.Type = type;
                this.Key = key;
                this.Colour = StateColours.GetStateColor(state);
            }

            public override string ToString()
            {
                if (this.OldValue == null || this.NewValue == null)
                {
                    if (this.NewValue == null)
                    {
                        return "";
                    }
                    return string.Concat("[", this.NewValue.ToString(), "]");
                }
                return string.Concat(new string[] { "[", this.OldValue.ToString(), "] => [", this.NewValue.ToString(), "]" });
            }
        }

        public enum ChangeType
        {
            NAME,
            VALUE,
            NEWREF,
            MODREF,
            DELREF,
            NEWINST,
            MODINST,
            INSTVALUE,
            INVALIDREF
        }

        public class ItemCheckedEventArgs : EventArgs
        {
            public ChangeListTree.TreeNode Node
            {
                get;
                private set;
            }

            public ItemCheckedEventArgs(ChangeListTree.TreeNode node)
            {
                this.Node = node;
            }
        }

        public delegate void ItemCheckedEventHandler(object sender, ChangeListTree.ItemCheckedEventArgs e);

        public enum ItemElement
        {
            NONE,
            TEXT,
            EXPAND,
            CHECKBOX,
            INDENT
        }

        public class RootNode : ChangeListTree.TreeNode
        {
            public GameData.Item Item
            {
                get;
                set;
            }

            public RootNode(GameData.Item item)
            {
                this.Item = item;
                base.Expanded = false;
                base.Checked = false;
            }
        }

        public class TreeNode
        {
            public bool Checked
            {
                get;
                set;
            }

            public List<ChangeListTree.TreeNode> Children
            {
                get;
                set;
            }

            public bool Expanded
            {
                get;
                set;
            }

            public bool HasChildren
            {
                get
                {
                    if (this.Children == null)
                    {
                        return false;
                    }
                    return this.Children.Count > 0;
                }
            }

            public int Level
            {
                get;
                set;
            }

            public ChangeListTree.TreeNode Parent
            {
                get;
                private set;
            }

            public TreeNode()
            {
                this.Expanded = false;
                this.Checked = false;
                this.Level = 0;
            }

            public void Add(ChangeListTree.TreeNode n)
            {
                if (this.Children == null)
                {
                    this.Children = new List<ChangeListTree.TreeNode>();
                }
                this.Children.Add(n);
                n.Level = this.Level + 1;
                n.Parent = this;
            }

            public int getVisibleChildNodeCount()
            {
                int visibleChildNodeCount = 0;
                if (this.Expanded && this.Children != null)
                {
                    foreach (ChangeListTree.ChangeData child in this.Children)
                    {
                        visibleChildNodeCount = visibleChildNodeCount + 1 + child.getVisibleChildNodeCount();
                    }
                }
                return visibleChildNodeCount;
            }

            public void getVisibleChildNodes(List<ChangeListTree.TreeNode> list, int after)
            {
                if (this.Expanded && this.Children != null)
                {
                    foreach (ChangeListTree.ChangeData child in this.Children)
                    {
                        int num = after + 1;
                        after = num;
                        list.Insert(num, child);
                        if (!child.Expanded)
                        {
                            continue;
                        }
                        child.getVisibleChildNodes(list, after);
                    }
                }
            }
        }
    }
}
