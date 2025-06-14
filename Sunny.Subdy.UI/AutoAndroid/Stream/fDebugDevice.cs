using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.Serialization;
namespace AutoAndroid.Stream
{
    public partial class fDebugDevice : Form
    {
        private ADBClient _device;
        private Bitmap originalImage;
        private List<UiElement> elements = new();
        private UiElement hoveredElement = null;
        private UiElement selectedElement = null;
        private string rawXmlSource = "";
        private float scaleX = 1f, scaleY = 1f;
        private  System.Drawing.Point imageOffset = System.Drawing.Point.Empty;

        public fDebugDevice(DeviceModel device)
        {
            InitializeComponent();
            _device = new ADBClient(device);
            textBox2.Text = device.Serial;

            // Load ảnh và XML UI ban đầu
            ReloadScreen();

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            pictureBox1.Paint += pictureBox1_Paint;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            treeView1.AfterSelect += treeView1_AfterSelect;
        }

        private void ReloadScreen()
        {
            Bitmap bmp = _device.Screenshot();
            string xml = _device.GetXMLSource();
            originalImage = bmp;
            rawXmlSource = xml;
            pictureBox1.Image = originalImage;
            elements = LoadElementsFromDump(xml);
            treeView1.Nodes.Clear();
            BuildTreeView(elements, treeView1.Nodes);
            pictureBox1.Invalidate();
        }

        private List<UiElement> LoadElementsFromDump(string xml)
        {
            var serializer = new XmlSerializer(typeof(Hierarchy));
            using var reader = new StringReader(xml);
            var hierarchy = (Hierarchy)serializer.Deserialize(reader);
            return hierarchy.Nodes;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (originalImage == null) return;

            CalculateZoomParams();

            int imgX = (int)((e.X - imageOffset.X) / scaleX);
            int imgY = (int)((e.Y - imageOffset.Y) / scaleY);
            toolStripLabel3.Text = $"({imgX}, {imgY})";

            hoveredElement = FindElementAtPoint(FlattenElements(elements), imgX, imgY);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (hoveredElement != null)
            {
                selectedElement = hoveredElement;
                UpdateComboBoxWithElement(selectedElement);
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (originalImage == null) return;

            CalculateZoomParams();

            using Pen dashedPen = new Pen(Color.Red, 2) { DashStyle = DashStyle.Dash };
            using Pen solidPen = new Pen(Color.Red, 2);
            using Brush semiTransparent = new SolidBrush(Color.FromArgb(100, Color.Black));

            foreach (var el in FlattenElements(elements))
            {
                var rect = new Rectangle(
                    (int)(el.Bounds.X * scaleX + imageOffset.X),
                    (int)(el.Bounds.Y * scaleY + imageOffset.Y),
                    (int)(el.Bounds.Width * scaleX),
                    (int)(el.Bounds.Height * scaleY));

                if (el == selectedElement)
                {
                    e.Graphics.FillRectangle(semiTransparent, rect);
                    e.Graphics.DrawRectangle(solidPen, rect);
                }
                else
                {
                    e.Graphics.DrawRectangle(dashedPen, rect);
                }
            }

            SelectNodeByElement(treeView1.Nodes, hoveredElement);
        }

        private void CalculateZoomParams()
        {
            if (originalImage == null || pictureBox1.Width == 0 || pictureBox1.Height == 0) return;

            float zoomX = (float)pictureBox1.Width / originalImage.Width;
            float zoomY = (float)pictureBox1.Height / originalImage.Height;
            float zoom = Math.Min(zoomX, zoomY);

            int imageW = (int)(originalImage.Width * zoom);
            int imageH = (int)(originalImage.Height * zoom);

            imageOffset = new System.Drawing.Point((pictureBox1.Width - imageW) / 2, (pictureBox1.Height - imageH) / 2);
            scaleX = scaleY = zoom;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is UiElement el)
            {
                hoveredElement = el;
                selectedElement = el;
                UpdateComboBoxWithElement(el);
                pictureBox1.Invalidate();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is string item)
            {
                string xpath = BuildXPathFromItem(item);
                textBox1.Text = xpath;

                var doc = new XmlDocument();
                doc.LoadXml(rawXmlSource);
                XmlNodeList found = doc.SelectNodes(xpath);
                toolStripLabel1.Text = $"Found {found.Count} node(s)";
            }
        }

        private void UpdateComboBoxWithElement(UiElement el)
        {
            comboBox1.Items.Clear();

            var attrs = new Dictionary<string, string>
        {
            { "class", el.Class },
            { "text", el.Text },
            { "resource-id", el.ResourceId },
            { "content-desc", el.ContentDesc }
        };

            foreach (var (key, value) in attrs)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    comboBox1.Items.Add($"{key}={value}");
            }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private string BuildXPathFromItem(string item)
        {
            var parts = item.Split('=');
            if (parts.Length != 2) return "";
            return $"//*[@{parts[0]}='{parts[1]}']";
        }

        private void BuildTreeView(List<UiElement> elems, TreeNodeCollection nodes)
        {
            foreach (var el in elems)
            {
                var node = new TreeNode($"{el.Class} - {el.Text}") { Tag = el };
                nodes.Add(node);
                BuildTreeView(el.Children, node.Nodes);
            }
        }

        private void SelectNodeByElement(TreeNodeCollection nodes, UiElement target)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is UiElement el && el == target)
                {
                    treeView1.SelectedNode = node;
                    return;
                }
                SelectNodeByElement(node.Nodes, target);
            }
        }

        private UiElement FindElementAtPoint(IEnumerable<UiElement> allElements, int x, int y)
        {
            return allElements
                .Where(el => el.Bounds.Contains(x, y))
                .OrderBy(el => el.Bounds.Width * el.Bounds.Height)
                .FirstOrDefault();
        }

        private IEnumerable<UiElement> FlattenElements(List<UiElement> elems)
        {
            foreach (var el in elems)
            {
                yield return el;
                foreach (var child in FlattenElements(el.Children))
                    yield return child;
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            ReloadScreen();
        }
    }
    [XmlRoot("hierarchy")]
    public class Hierarchy
    {
        [XmlElement("node")]
        public List<UiElement> Nodes { get; set; }
    }

    public class UiElement
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("text")]
        public string Text { get; set; }
        [XmlAttribute("resource-id")]
        public string ResourceId { get; set; }
        [XmlAttribute("content-des")]
        public string ContentDesc { get; set; }

        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("bounds")]
        public string BoundsRaw { get; set; }

        [XmlElement("node")]
        public List<UiElement> Children { get; set; }

        [XmlIgnore]
        public Rectangle Bounds
        {
            get
            {
                // Convert "[0,0][100,100]" => Rectangle
                if (string.IsNullOrWhiteSpace(BoundsRaw)) return Rectangle.Empty;
                try
                {
                    var parts = BoundsRaw
                        .Replace("[", "")
                        .Replace("]", ",")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();

                    return new Rectangle(parts[0], parts[1], parts[2] - parts[0], parts[3] - parts[1]);
                }
                catch
                {
                    return Rectangle.Empty;
                }
            }
        }
    }
}
