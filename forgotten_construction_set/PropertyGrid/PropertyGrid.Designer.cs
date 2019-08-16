using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
namespace forgotten_construction_set.PropertyGrid
{
    partial class PropertyGrid
    {
        public List<PropertyGrid.Section> Sections;

        private PropertyGrid.Section mFocusedSection;

        private PropertyGrid.Item mFocusedItem;

        private bool mMovingDivider;

        private CustomProperty mEditor;

        protected Dictionary<Type, CustomProperty> Types;

        private CustomProperty DefaultType;

        private CustomProperty EnumType;

        private IContainer components = null;

        [Category("Appearance")]
        [DefaultValue(System.Windows.Forms.AutoScaleMode.None)]
        [Description("Something...")]
        public System.Windows.Forms.AutoScaleMode AutoScaleMode
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Is the divider fixed")]
        public bool DividerFixed
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(100)]
        [Description("Divider position")]
        public int DividerPosition
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        [Description("Are the grid lines visible")]
        public bool DrawLines
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Highlight")]
        [Description("Background colour of the selected item")]
        public Color FocusColour
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "HighlightText")]
        [Description("Text colour of the selected item")]
        public Color FocusText
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "ControlLight")]
        [Description("Colour of the grid lines")]
        public Color LineColour
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(16)]
        [Description("Height in pixels of the items")]
        public int LineHeight
        {
            get;
            set;
        }

        [Category("Appearance")]
        [DefaultValue(12)]
        [Description("Size of the margin")]
        public int MarginSize
        {
            get;
            set;
        }

        [Browsable(false)]
        public PropertyGrid.Item SelectedItem
        {
            get
            {
                return this.mFocusedItem;
            }
        }

        [Browsable(false)]
        public PropertyGrid.Section SelectedSection
        {
            get
            {
                return this.mFocusedSection;
            }
        }

        [Category("Data")]
        [DefaultValue(false)]
        [Description("Are items sorted by name")]
        public bool SortItems
        {
            get;
            set;
        }

        [Category("Data")]
        [DefaultValue(false)]
        [Description("Are sections sorted by name")]
        public bool SortSections
        {
            get;
            set;
        }


        public PropertyGrid.Item addItem(string section, string name, object value, string description = "", Color? color = null, bool editable = true)
        {
           

            PropertyGrid.Item item = new PropertyGrid.Item()
            {
                Name = name,
                Value = value,
                Editable = editable,
                Visible = true
            };
            item.Name = item.Name + "1";

            //string tansedStr = null;
            //if ((bool)NativeTranslte.enumDict.TryGetValue(name, out tansedStr))
            //{
            //    item.TransName = tansedStr;
            //}
            //tansedStr = null;
            //if ((value is string || value is EnumValue) && (bool)NativeTranslte.enumDict.TryGetValue((string)value, out tansedStr))
            //{
            //    item.TransValue = tansedStr;
            //}
            //try
            //{
            //    fCSEnums.addValue(tansedStr, max);
            //}
            //catch (Exception exception)
            //{
            //    fCSEnums.addValue(item1, max);
            //}
            PropertyGrid.Item item1 = item;
            Color? nullable = color;
            item1.TextColour = (nullable.HasValue ? nullable.GetValueOrDefault() : this.ForeColor);
            item.Description = description;
            if (value != null)
            {
                item.Property = this.getCustomProperty(value.GetType());
            }
            this.addItem(section, item);
            return item;
        }

        public void addItem(string section, PropertyGrid.Item item)
        {
            PropertyGrid.Section list = this.getSection(section, true);
            list.Items.Add(item);
            if (this.SortItems)
            {
                list.Items = (
                    from o in list.Items
                    orderby o.Name
                    select o).ToList<PropertyGrid.Item>();
            }
            this.recalculateSize();
            base.Invalidate();
        }

        public void AddPropertyType(Type type, CustomProperty property)
        {
            if (!this.Types.ContainsKey(type))
            {
                this.Types.Add(type, property);
            }
            else
            {
                this.Types[type] = property;
            }
        }

        public void AutosizeDivider()
        {
            float single = 0f;
            Graphics graphic = Graphics.FromImage(new Bitmap(16, 16));
            foreach (PropertyGrid.Section section in this.Sections)
            {
                foreach (PropertyGrid.Item item in section.Items)
                {
                    SizeF sizeF = graphic.MeasureString(item.Name, this.Font);
                    single = Math.Max(single, sizeF.Width);
                }
            }
            if (single > 0f)
            {
                this.DividerPosition = (int)single + this.MarginSize + 2;
            }
        }

        public void clear()
        {
            this.closeEditor(true);
            this.Sections.Clear();
            base.Invalidate();
        }

        private void closeEditor(bool apply = true)
        {
            if ((this.mEditor == null ? false : this.mEditor.Editing != null))
            {
                if (!apply)
                {
                    this.mEditor.DestroyEditor();
                }
                else
                {
                    this.mEditor.Apply();
                }
            }
        }

        private void createEditor(PropertyGrid.Section s, PropertyGrid.Item item)
        {
            if ((this.mEditor == null ? true : this.mEditor.Editing != item))
            {
                this.closeEditor(true);
                if (item.Editable)
                {
                    int verticalPosition = this.getVerticalPosition(s, item);
                    this.mEditor = item.Property ?? this.DefaultType;
                    CustomProperty customProperty = this.mEditor;
                    int dividerPosition = this.DividerPosition;
                    int y = verticalPosition + 1 + base.AutoScrollPosition.Y;
                    System.Drawing.Size clientSize = base.ClientSize;
                    customProperty.CreateEditor(this, s, item, new Rectangle(dividerPosition, y, clientSize.Width - this.DividerPosition, this.LineHeight));
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void FirePropertyChanged(PropertyGrid.Section section, PropertyGrid.Item item, object oldValue)
        {
            if (this.OnPropertyChanged != null)
            {
                this.OnPropertyChanged(this, new PropertyChangedArgs(section, item, oldValue));
            }
        }

        public void FirePropertyKeyEnter(PropertyGrid.Section section, PropertyGrid.Item item)
        {
            if (this.OnPropertyKeyEnter != null)
            {
                this.OnPropertyKeyEnter(this, new PropertyKeyEnterArgs(section, item));
            }
        }

        protected void FirePropertySelected(PropertyGrid.Section section, PropertyGrid.Item item)
        {
            if (this.OnPropertySelected != null)
            {
                this.OnPropertySelected(this, new PropertySelectedArgs(section, item));
            }
        }

        public CustomProperty getCustomProperty(Type type)
        {
            CustomProperty item;
            if (!this.Types.ContainsKey(type))
            {
                item = (!type.IsEnum ? this.DefaultType : this.EnumType);
            }
            else
            {
                item = this.Types[type];
            }
            return item;
        }

        public PropertyGrid.Item getItem(string section, string name)
        {
            PropertyGrid.Item item;
            PropertyGrid.Section section1 = this.getSection(section, false);
            if (section1 != null)
            {
                PropertyGrid.Item item1 = section1.Items.Find((PropertyGrid.Item o) => o.Name == name);
                item = item1;
            }
            else
            {
                item = null;
            }
            return item;
        }

        public PropertyGrid.Item getItem(string name)
        {
            PropertyGrid.Item item;
            Predicate<PropertyGrid.Item> predicate = null;
            foreach (PropertyGrid.Section section in this.Sections)
            {
                List<PropertyGrid.Item> items = section.Items;
                Predicate<PropertyGrid.Item> predicate1 = predicate;
                if (predicate1 == null)
                {
                    Predicate<PropertyGrid.Item> predicate2 = (PropertyGrid.Item o) => o.Name == name;
                    Predicate<PropertyGrid.Item> predicate3 = predicate2;
                    predicate = predicate2;
                    predicate1 = predicate3;
                }
                PropertyGrid.Item item1 = items.Find(predicate1);
                if (item1 != null)
                {
                    item = item1;
                    return item;
                }
            }
            item = null;
            return item;
        }

        protected KeyValuePair<PropertyGrid.Section, PropertyGrid.Item> getItemAt(int y)
        {
            KeyValuePair<PropertyGrid.Section, PropertyGrid.Item> keyValuePair;
            foreach (PropertyGrid.Section section in this.Sections)
            {
                if (y >= this.LineHeight)
                {
                    if (!section.Collapsed)
                    {
                        foreach (PropertyGrid.Item item in section.Items)
                        {
                            y -= this.LineHeight;
                            if (y < this.LineHeight)
                            {
                                keyValuePair = new KeyValuePair<PropertyGrid.Section, PropertyGrid.Item>(section, item);
                                return keyValuePair;
                            }
                        }
                    }
                    y -= this.LineHeight;
                }
                else
                {
                    keyValuePair = new KeyValuePair<PropertyGrid.Section, PropertyGrid.Item>(section, null);
                    return keyValuePair;
                }
            }
            keyValuePair = new KeyValuePair<PropertyGrid.Section, PropertyGrid.Item>(null, null);
            return keyValuePair;
        }

        public PropertyGrid.Section getSection(string name)
        {
            return this.getSection(name, false);
        }

        private PropertyGrid.Section getSection(string name, bool create)
        {
            PropertyGrid.Section section;
            foreach (PropertyGrid.Section section1 in this.Sections)
            {
                if (section1.Name != name)
                {
                    continue;
                }
                section = section1;
                return section;
            }
            if (create)
            {
                PropertyGrid.Section section2 = new PropertyGrid.Section()
                {
                    Name = name,
                    TextColour = SystemColors.WindowText,
                    MarginColor = SystemColors.ButtonFace,
                    BackColour1 = Color.White,
                    BackColour2 = Color.FromArgb(252, 252, 252),
                    Collapsed = false,
                    Items = new List<PropertyGrid.Item>()
                };
                this.Sections.Add(section2);
                if (this.SortSections)
                {
                    this.Sections = (
                        from o in this.Sections
                        orderby o.Name
                        select o).ToList<PropertyGrid.Section>();
                }
                section = section2;
            }
            else
            {
                section = null;
            }
            return section;
        }

        private int getVerticalPosition(PropertyGrid.Section section, PropertyGrid.Item item)
        {
            int num;
            int lineHeight = 0;
            if (!section.Collapsed)
            {
                foreach (PropertyGrid.Section section1 in this.Sections)
                {
                    if ((section1 != section ? true : item != null))
                    {
                        lineHeight += this.LineHeight;
                        if (section1 == section)
                        {
                            num = lineHeight + section1.Items.IndexOf(item) * this.LineHeight;
                            return num;
                        }
                        else if (!section1.Collapsed)
                        {
                            lineHeight = lineHeight + section1.Items.Count * this.LineHeight;
                        }
                    }
                    else
                    {
                        num = lineHeight;
                        return num;
                    }
                }
                num = -1;
            }
            else
            {
                num = -1;
            }
            return num;
        }

        private bool hasFocus()
        {
            bool flag;
            if (!this.Focused)
            {
                foreach (Control control in base.Controls)
                {
                    if (!control.Focused)
                    {
                        continue;
                    }
                    flag = true;
                    return flag;
                }
                flag = ((this.mEditor == null ? true : !this.mEditor.FocusLocked) ? false : true);
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.MouseDoubleClick += new MouseEventHandler(this.PropertyGrid_MouseDoubleClick);
            base.MouseDown += new MouseEventHandler(this.PropertyGrid_MouseDown);
            base.MouseMove += new MouseEventHandler(this.PropertyGrid_MouseMove);
            base.MouseUp += new MouseEventHandler(this.PropertyGrid_MouseUp);
            base.PreviewKeyDown += new PreviewKeyDownEventHandler(this.PropertyGrid_PreviewKeyDown);
            base.Resize += new EventHandler(this.PropertyGrid_Resize);
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int y = base.AutoScrollPosition.Y;
            int width = e.ClipRectangle.Width;
            bool flag = this.hasFocus();
            e.Graphics.Clear(Color.White);
            System.Drawing.Font font = new System.Drawing.Font(this.Font, FontStyle.Bold);
            SolidBrush solidBrush = new SolidBrush(Color.White);
            Pen pen = new Pen(this.LineColour);
            Pen pen1 = new Pen(Color.Black);
            Pen pen2 = new Pen(Color.Black)
            {
                DashStyle = DashStyle.Dot
            };
            Point[] point = new Point[] { new Point(this.MarginSize / 2 - 3, this.LineHeight / 2 + 3), new Point(this.MarginSize / 2 + 3, this.LineHeight / 2 - 3), new Point(this.MarginSize / 2 + 3, this.LineHeight / 2 + 3) };
            Point[] pointArray = new Point[] { new Point(this.MarginSize / 2 - 1, this.LineHeight / 2 - 3), new Point(this.MarginSize / 2 - 1, this.LineHeight / 2 + 3), new Point(this.MarginSize / 2 + 2, this.LineHeight / 2) };
            int x = e.ClipRectangle.X;
            e.Graphics.TranslateTransform(0f, (float)base.AutoScrollPosition.Y);
            foreach (PropertyGrid.Section section in this.Sections)
            {
                solidBrush.Color = section.MarginColor;
                e.Graphics.FillRectangle(solidBrush, x, 0, width, this.LineHeight);
                if (this.DrawLines)
                {
                    e.Graphics.DrawLine(pen, x, 0, width + x, 0);
                    e.Graphics.DrawLine(pen, x, this.LineHeight, width + x, this.LineHeight);
                }
                solidBrush.Color = pen1.Color;
                if (!section.Collapsed)
                {
                    e.Graphics.FillPolygon(solidBrush, point);
                }
                else
                {
                    e.Graphics.DrawPolygon(pen1, pointArray);
                }
                int marginSize = this.MarginSize + 4;
                int num = 1;
                if ((this.mFocusedSection != section ? false : this.mFocusedItem == null) & flag)
                {
                    System.Drawing.Size size = TextRenderer.MeasureText(section.Name, font);
                    e.Graphics.DrawRectangle(pen2, marginSize, num, size.Width, size.Height);
                }
                solidBrush.Color = section.TextColour;
                TextRenderer.DrawText(e.Graphics, NativeTranslte.getTransSection(section.Name), font, new Point(marginSize, num + (int)e.Graphics.Transform.OffsetY), section.TextColour);
                e.Graphics.TranslateTransform(0f, (float)this.LineHeight);
                if (!section.Collapsed)
                {
                    int num1 = 0;
                    foreach (PropertyGrid.Item item in section.Items)
                    {
                        if (e.Graphics.Transform.OffsetY > (float)(-this.LineHeight))
                        {
                            solidBrush.Color = section.MarginColor;
                            e.Graphics.FillRectangle(solidBrush, x, 0, this.MarginSize, this.LineHeight + 1);
                            if (!(this.mFocusedItem == item & flag))
                            {
                                solidBrush.Color = ((num1 & 1) == 0 ? section.BackColour1 : section.BackColour2);
                                e.Graphics.FillRectangle(solidBrush, this.MarginSize, 1, width - this.MarginSize + x, this.LineHeight - 1);
                            }
                            else
                            {
                                solidBrush.Color = this.FocusColour;
                                e.Graphics.FillRectangle(solidBrush, this.MarginSize, 1, this.DividerPosition - this.MarginSize, this.LineHeight - 1);
                                solidBrush.Color = ((num1 & 1) == 0 ? section.BackColour1 : section.BackColour2);
                                e.Graphics.FillRectangle(solidBrush, this.DividerPosition, 1, width - this.DividerPosition + x, this.LineHeight - 1);
                            }
                            if (this.DrawLines)
                            {
                                e.Graphics.DrawLine(pen, this.MarginSize, this.LineHeight, width + x, this.LineHeight);
                            }
                            e.Graphics.DrawLine(pen, this.DividerPosition, 0, this.DividerPosition, this.LineHeight);
                            Color color = (this.mFocusedItem == item & flag ? this.FocusText : item.TextColour);
                            Rectangle rectangle = new Rectangle(this.MarginSize, (int)e.Graphics.Transform.OffsetY + 1, this.DividerPosition - this.MarginSize, this.LineHeight - 1);
                            TextRenderer.DrawText(e.Graphics, item.Name, this.Font, rectangle, color, TextFormatFlags.Default);
                            CustomProperty property = item.Property ?? this.DefaultType;
                            property.Paint(this, item, e.Graphics, new Rectangle(this.DividerPosition + 1, 0, base.Width - this.DividerPosition - 1, this.LineHeight));
                        }
                        e.Graphics.TranslateTransform(0f, (float)this.LineHeight);
                        if (e.Graphics.Transform.OffsetY <= (float)base.Height)
                        {
                            num1++;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        private void PropertyGrid_GotFocus(object sender, EventArgs e)
        {
            base.Invalidate();
        }

        private void PropertyGrid_LostFocus(object sender, EventArgs e)
        {
            base.Invalidate();
            if ((this.mEditor == null ? false : this.mEditor.Editing != null))
            {
                foreach (Control control in base.Controls)
                {
                    if (!control.Focused)
                    {
                        continue;
                    }
                    return;
                }
                if (!this.mEditor.FocusLocked)
                {
                    this.closeEditor(true);
                }
            }
        }

        private void PropertyGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int y = e.Y;
            Point autoScrollPosition = base.AutoScrollPosition;
            KeyValuePair<PropertyGrid.Section, PropertyGrid.Item> itemAt = this.getItemAt(y - autoScrollPosition.Y);
            if (itemAt.Value != null)
            {
                this.createEditor(itemAt.Key, itemAt.Value);
                if ((this.mEditor == null ? false : this.mEditor.Editing != null))
                {
                    CustomProperty customProperty = this.mEditor;
                    System.Windows.Forms.MouseButtons button = e.Button;
                    int clicks = e.Clicks;
                    int x = e.X;
                    int num = e.Y;
                    autoScrollPosition = base.AutoScrollPosition;
                    customProperty.DoubleClick(new MouseEventArgs(button, clicks, x, num - autoScrollPosition.Y, e.Delta));
                }
            }
            else if ((itemAt.Key == null ? false : e.X > this.MarginSize))
            {
                this.closeEditor(true);
                itemAt.Key.Collapsed = !itemAt.Key.Collapsed;
                this.recalculateSize();
                base.Invalidate();
            }
        }

        private void PropertyGrid_MouseDown(object sender, MouseEventArgs e)
        {
            Point autoScrollPosition;
            base.Focus();
            if ((this.mEditor == null ? false : this.mEditor.Editing != null))
            {
                if (e.X <= this.DividerPosition + 2)
                {
                    this.closeEditor(true);
                }
                else
                {
                    CustomProperty customProperty = this.mEditor;
                    System.Windows.Forms.MouseButtons button = e.Button;
                    int clicks = e.Clicks;
                    int x = e.X;
                    int y = e.Y;
                    autoScrollPosition = base.AutoScrollPosition;
                    customProperty.MouseDown(new MouseEventArgs(button, clicks, x, y - autoScrollPosition.Y, e.Delta));
                }
            }
            if ((this.DividerFixed ? true : Math.Abs(e.X - this.DividerPosition) >= 3))
            {
                int num = e.Y;
                autoScrollPosition = base.AutoScrollPosition;
                KeyValuePair<PropertyGrid.Section, PropertyGrid.Item> itemAt = this.getItemAt(num - autoScrollPosition.Y);
                if (itemAt.Value != null)
                {
                    this.mFocusedItem = itemAt.Value;
                    this.mFocusedSection = itemAt.Key;
                    this.FirePropertySelected(this.mFocusedSection, this.mFocusedItem);
                    if (e.X > this.DividerPosition)
                    {
                        this.createEditor(itemAt.Key, itemAt.Value);
                    }
                }
                else if (itemAt.Key != null)
                {
                    this.mFocusedItem = null;
                    this.mFocusedSection = itemAt.Key;
                    if (e.X < this.MarginSize)
                    {
                        this.mFocusedSection.Collapsed = !this.mFocusedSection.Collapsed;
                        this.recalculateSize();
                    }
                }
                base.Invalidate();
            }
            else
            {
                this.Cursor = Cursors.VSplit;
                this.mMovingDivider = true;
                base.Capture = true;
            }
        }

        private void PropertyGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.mMovingDivider)
            {
                if ((this.DividerFixed ? true : Math.Abs(e.X - this.DividerPosition) >= 3))
                {
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    this.Cursor = Cursors.VSplit;
                }
                if ((this.mEditor == null ? false : this.mEditor.Editing != null))
                {
                    CustomProperty customProperty = this.mEditor;
                    System.Windows.Forms.MouseButtons button = e.Button;
                    int clicks = e.Clicks;
                    int x = e.X;
                    int y = e.Y;
                    Point autoScrollPosition = base.AutoScrollPosition;
                    customProperty.MouseMove(new MouseEventArgs(button, clicks, x, y - autoScrollPosition.Y, e.Delta));
                }
            }
            else
            {
                this.DividerPosition = e.X;
                this.DividerPosition = Math.Max(this.DividerPosition, this.MarginSize + base.Width / 6);
                this.DividerPosition = Math.Min(this.DividerPosition, base.Width - base.Width / 6);
                base.Invalidate();
            }
        }

        private void PropertyGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.mMovingDivider)
            {
                this.mMovingDivider = false;
                this.Cursor = Cursors.Default;
                base.Capture = false;
            }
            else if ((this.mEditor == null ? false : this.mEditor.Editing != null))
            {
                CustomProperty customProperty = this.mEditor;
                System.Windows.Forms.MouseButtons button = e.Button;
                int clicks = e.Clicks;
                int x = e.X;
                int y = e.Y;
                Point autoScrollPosition = base.AutoScrollPosition;
                customProperty.MouseUp(new MouseEventArgs(button, clicks, x, y - autoScrollPosition.Y, e.Delta));
            }
        }

        private void PropertyGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            PropertyGrid.Item item;
            if ((this.Sections.Count == 0 ? false : this.mFocusedSection != null))
            {
                int num = this.Sections.IndexOf(this.mFocusedSection);
                int num1 = (this.mFocusedItem == null ? -1 : this.mFocusedSection.Items.IndexOf(this.mFocusedItem));
                PropertyGrid.Item item1 = this.mFocusedItem;
                if (e.KeyCode == Keys.Up)
                {
                    if (num1 > 0)
                    {
                        item1 = this.mFocusedSection.Items[num1 - 1];
                    }
                    else if (num1 == 0)
                    {
                        item1 = null;
                    }
                    else if (num > 0)
                    {
                        this.mFocusedSection = this.Sections[num - 1];
                        if (this.mFocusedSection.Items.Count == 0)
                        {
                            item = null;
                        }
                        else
                        {
                            item = this.mFocusedSection.Items[this.mFocusedSection.Items.Count - 1];
                        }
                        item1 = item;
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (num1 < this.mFocusedSection.Items.Count - 1)
                    {
                        item1 = this.mFocusedSection.Items[num1 + 1];
                    }
                    else if (num < this.Sections.Count - 1)
                    {
                        this.mFocusedSection = this.Sections[num + 1];
                        item1 = null;
                    }
                }
                else if ((e.KeyCode != Keys.Left ? false : num1 < 0))
                {
                    this.mFocusedSection.Collapsed = true;
                    base.Invalidate();
                }
                else if ((e.KeyCode != Keys.Right ? false : num1 < 0))
                {
                    this.mFocusedSection.Collapsed = false;
                    base.Invalidate();
                }
                else if (this.mFocusedItem != null)
                {
                    this.createEditor(this.mFocusedSection, this.mFocusedItem);
                }
                if (item1 != this.mFocusedItem)
                {
                    this.mFocusedItem = item1;
                    if (item1 != null)
                    {
                        this.FirePropertySelected(this.mFocusedSection, this.mFocusedItem);
                    }
                    int verticalPosition = this.getVerticalPosition(this.mFocusedSection, this.mFocusedItem);
                    Point autoScrollPosition = base.AutoScrollPosition;
                    int num2 = Math.Min(verticalPosition, -autoScrollPosition.Y);
                    int lineHeight = verticalPosition + this.LineHeight;
                    System.Drawing.Size clientSize = base.ClientSize;
                    num2 = Math.Max(num2, lineHeight - clientSize.Height);
                    base.AutoScrollPosition = new Point(0, num2);
                    base.Invalidate();
                }
            }
        }

        private void PropertyGrid_Resize(object sender, EventArgs e)
        {
            this.closeEditor(true);
        }

        protected void recalculateSize()
        {
            int lineHeight = 0;
            foreach (PropertyGrid.Section section in this.Sections)
            {
                lineHeight += this.LineHeight;
                if (!section.Collapsed)
                {
                    lineHeight = lineHeight + section.Items.Count * this.LineHeight;
                }
            }
            base.AutoScrollMinSize = new System.Drawing.Size(0, lineHeight);
        }

        public void removeItem(string section, PropertyGrid.Item item)
        {
            this.closeEditor(true);
            PropertyGrid.Section section1 = this.getSection(section, false);
            if (section1 != null)
            {
                if (this.mFocusedItem == item)
                {
                    this.mFocusedItem = null;
                }
                section1.Items.Remove(item);
                base.Invalidate();
            }
        }

        public void removeItem(string section, string name)
        {
            PropertyGrid.Item item = this.getItem(section, name);
            if (item != null)
            {
                this.removeItem(section, item);
            }
        }

        public void removeSection(string name)
        {
            this.closeEditor(true);
            this.Sections.RemoveAll((PropertyGrid.Section o) => o.Name == name);
            if ((this.mFocusedSection == null ? false : this.mFocusedSection.Name == name))
            {
                this.mFocusedSection = null;
            }
            base.Invalidate();
        }

        public event PropertyGrid.PropertyChangedHandler OnPropertyChanged;

        public event PropertyGrid.PropertyKeyEnterHandler OnPropertyKeyEnter;

        public event PropertyGrid.PropertySelectedHandler OnPropertySelected;

       

        public delegate void PropertyChangedHandler(object sender, PropertyChangedArgs e);

        public delegate void PropertyKeyEnterHandler(object sender, PropertyKeyEnterArgs e);

        public delegate void PropertySelectedHandler(object sender, PropertySelectedArgs e);

       


    }
}
