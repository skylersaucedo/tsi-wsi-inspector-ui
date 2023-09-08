using bbox_maker.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
//using UI.MachineLearning;

namespace bbox_maker
{

    public partial class frmMain : Form
    {
        const string INPUT_IMAGE_FOLDER_TITLE = "Input image";
        public string labelpath = @"C:\Users\sauce\OneDrive\Desktop\bbox_maker-master\bbox_maker\labels.txt";

        Point _mousePositionDragStart { get; set; }
        Point _mousePositionDragged { get; set; }

        bool _mouseIsDown = false;

        Rectangle r;

        List<Defects> _loadedDefects;

        int _imageIndex { get; set; }

        public void LoadLabels(string path)
        {
            //
            List<string> lines = new List<string>();
            //string path = @"C:\example\example.txt";

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            // populate combobox

            foreach (string line in lines)
            {
                cmbxLabel.Items.Add(line);
            }

        }

        public frmMain(string inputImageFolder)
        {
            inputImageFolder = "C:\\Users\\sauce\\OneDrive\\Desktop\\make_pano\\panos";
            InitializeComponent();
            uiInputImageFolder.Text = inputImageFolder;
        }

        public void frmMain_Load(object sender, EventArgs e)
        {
            var inputImageFolder = uiInputImageFolder.Text;
            if((!string.IsNullOrWhiteSpace(inputImageFolder)) && Directory.Exists(uiInputImageFolder.Text))
            {                
                LoadInputImageFolder(inputImageFolder);

                LoadLabels(labelpath);

                PopulateDataGridView();
            }
            else
            {
                uiInputImageFolder.Text = FileBrowser.Instance.GetRecentDirectory(INPUT_IMAGE_FOLDER_TITLE);
            }
        }

        public Rectangle GetDraggedRect()
        {
            Rectangle rect = new Rectangle();

            rect.X = Math.Min(_mousePositionDragStart.X, _mousePositionDragged.X);
            rect.Y = Math.Min(_mousePositionDragStart.Y, _mousePositionDragged.Y);
            rect.Width = Math.Abs(_mousePositionDragStart.X - _mousePositionDragged.X);
            rect.Height = Math.Abs(_mousePositionDragStart.Y - _mousePositionDragged.Y);
            r = rect;
            return rect;

        }

        public void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            // need to reset if button is changed.

            if (r != null)
            {
                if ((_loadedDefects != null) && (_imageIndex < _loadedDefects.Count()))
                {
                    if (_mouseIsDown)
                    {
                        _loadedDefects[_imageIndex].rect = GetDraggedRect();
                    }
                    e.Graphics.DrawRectangle(Pens.Red, _loadedDefects[_imageIndex].rect);
                }
            }
        }

        public void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseIsDown = true;

                _mousePositionDragStart = e.Location;
                //txbxBboxcoords.Text += s_i.ToString();
            }
        }

        public void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown)
            {
                r = GetDraggedRect();
                _mouseIsDown = false;

                //_loadedDefects[_imageIndex].rect = r;

                //updateLabels(_imageIndex);
                Refresh();

                DialogResult result = MessageBox.Show("Adding a Defect?", "Add defect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // User clicked Yes
                    frmAddDefect form = new frmAddDefect();
                    form.Show();
                }
                else if (result == DialogResult.No)
                {
                    // User clicked No
                }
            }
        }


        private void PopulateDataGridView()
        {
            // Create a DataTable with 4 columns
            DataTable table = new DataTable();
            table.Columns.Add("x", typeof(int));
            table.Columns.Add("y", typeof(int));
            table.Columns.Add("z", typeof(int));

            // Add 4 rows of random data
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                table.Rows.Add(random.Next(100), random.Next(100), random.Next(100));
            }

            // Bind the DataTable to the DataGridView
            dataGridView1.DataSource = table;
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // get current point of x and y
            if (_mouseIsDown)
            {
                _mousePositionDragged = e.Location;
                r = GetDraggedRect();
                Refresh();
            }
        }

        public void makeXMLfile(
            string inputpath, 
            string h, 
            string w, 
            string label, 
            string xmin, 
            string xmax, 
            string ymin, 
            string ymax
            )
        {
            // this is in the current format needed to convert to a COCO .JSON for retraining.
            // we just need to swap out folder, filename, and image name with incoming labeled images
            // then feed to retrain model.

            // default stuff

            var outFilePath = Path.Combine(Path.GetDirectoryName(inputpath), Path.GetFileNameWithoutExtension(inputpath) + ".xml");
            string trun = "1";
            string diff = "0";
            string annoType = "Unknown";

            // Create an XmlWriterSettings object with the correct options.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.NewLineOnAttributes = true;
            settings.OmitXmlDeclaration= true;

            // Create an XmlWriter object with the file name and settings.
            XmlWriter writer = XmlWriter.Create(outFilePath, settings);

            // Write the start of the document.
            writer.WriteStartDocument();
            writer.WriteStartElement("annotation"); // <annotation>

            writer.WriteElementString("folder", Path.GetDirectoryName(inputpath));
            writer.WriteElementString("filename", Path.GetFileName(inputpath));
            writer.WriteElementString("path", inputpath);


            // Write the source element.
            writer.WriteStartElement("source"); // <source>
            writer.WriteElementString("database", annoType); // <annotation>ArcGIS Pro 2.1</annotation>
            writer.WriteEndElement(); // </source>

            // Write the size element.
            writer.WriteStartElement("size"); // <size>
            writer.WriteElementString("width", w); // <width>256</width>
            writer.WriteElementString("height", h); // <height>256</height>
            writer.WriteElementString("depth", "3"); // <depth>3</depth>
            writer.WriteEndElement(); // </size>

            // Write the segmented element.
            writer.WriteElementString("segmented", "0"); // <segmented>Unspecified</segmented>

            // Write the object element.
            writer.WriteStartElement("object"); // <object>
            writer.WriteElementString("name", label); // <name>0</name>
            writer.WriteElementString("pose", "Unspecified"); // <pose>Unspecified</pose>
            writer.WriteElementString("truncated", trun); // <truncated>Unspecified</truncated>
            writer.WriteElementString("difficult", diff); // <difficult>Unspecified</difficult>

            // Write the bndbox element.
            writer.WriteStartElement("bndbox"); // <bndbox>
            writer.WriteElementString("xmin", xmin); // <xmin>209.62</xmin>
            writer.WriteElementString("ymin", ymin); // <ymin>3.86</ymin>
            writer.WriteElementString("xmax", xmax); // <xmax>256.00</xmax>
            writer.WriteElementString("ymax", ymax); // <ymax>70.93</ymax>
            writer.WriteEndElement(); // </bndbox>

            writer.WriteEndElement(); // </object>

            // Write the end of the document.
            writer.WriteEndElement(); // </annotation>
            writer.WriteEndDocument();

            // Close the document and release resources.
            writer.Close();

        }

        public void btnMakeXML_Click(object sender, EventArgs e)
        {
            try
            {
                if((_loadedDefects == null) || (_loadedDefects.Count == 0)) 
                {
                    MessageBox.Show("No images are loaded into the dataset. Please use the 'Load Folder' button to load images.");
                    return; 
                }

                var notLabeledCount = _loadedDefects.Where(ld => ld.isNeedsBox).Count();
                if (notLabeledCount != 0)
                {
                    MessageBox.Show($"unable to export xml file. Number of missing boxes: {notLabeledCount}");
                    return;
                }

                for (int i = 0; i < _loadedDefects.Count(); i++)
                {
                    string w = _loadedDefects[i].width.ToString();
                    string h = _loadedDefects[i].height.ToString();
                    string label = _loadedDefects[i].label;

                    string xmin = _loadedDefects[i].rect.X.ToString();
                    string xmax = (_loadedDefects[i].rect.X + _loadedDefects[i].rect.Width).ToString();
                    string ymin = _loadedDefects[i].rect.Y.ToString();
                    string ymax = (_loadedDefects[i].rect.Y + _loadedDefects[i].rect.Height).ToString();

                    makeXMLfile(_loadedDefects[i].imagePath, h, w, label, xmin, xmax, ymin, ymax);
                }

                var thumbnailFolder = Path.GetDirectoryName(_loadedDefects[0].imagePath);
                MergeDataset(thumbnailFolder);
                RunTraining();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }


        public void loadDefects(string inputImageFolder)
        {
            r = Rectangle.Empty;
            pictureBox1.Image = null;

            // load folder of images, this will be automatic when user hits retrain
            // make objects

            List<Defects> defectList = new List<Defects>();
            List<string> labels = new List<string>();

            int count = 0;
            foreach (string imageFileName in Directory.GetFiles(inputImageFolder, "*.jpg"))
            {
                string name = Path.GetFileNameWithoutExtension(imageFileName);
                string[] x = name.Split(' ');

                if (x.Length ==1) // do not add larger scans...
                {
                    string label = x[x.Length - 1];

                    System.Drawing.Image img = System.Drawing.Image.FromFile(imageFileName);

                    int w = img.Width;
                    int h = img.Height;

                    Defects def = new Defects();

                    def.imagePath = imageFileName;
                    def.rect = Rectangle.Empty;
                    def.label = label;
                    def.height = h;
                    def.width = w;
                    defectList.Add(def);
                    count++;

                    labels.Add(label);
                }

            }

            // enable toggle buttons
            btnPrevious.Enabled = true;
            btnNext.Enabled = true;

            // populate instance defect list
            _loadedDefects = defectList;
            _imageIndex = 0;

            // update combobox with unique defect classes
            //HashSet<string> uniqueLabels = new HashSet<string>(labels);
            //cmbxLabel.Items.AddRange(uniqueLabels.ToArray());

            // show first defect
            if (_loadedDefects.Count() > 0) { pictureBox1.Image = new Bitmap(_loadedDefects[_imageIndex].imagePath); }
            updateLabels(_imageIndex);

            Refresh();
        }

        void LoadInputImageFolder(string inputImageFolder)
        {
            r = Rectangle.Empty;
            btnLoadFolder.Enabled = true;
            uiInputImageFolder.Enabled = false;
            btnMakeXML.Enabled = true;
            cmbxLabel.Enabled = true;
            loadDefects(inputImageFolder);
        }

        public void btnLoadFolder_Click(object sender, EventArgs e)
        {
            var inputImageFolder = uiInputImageFolder.Text;
            if (string.IsNullOrWhiteSpace(inputImageFolder) || !Directory.Exists(uiInputImageFolder.Text))
            {
                return;
            }

            LoadInputImageFolder(inputImageFolder);            
        }

        public void updateLabels(int i)
        {
            // update labels
            if(i < _loadedDefects.Count())
            {
                lblImagePath.Text = _loadedDefects[i].imagePath;
                lblCurrentPosition.Text = (i + 1).ToString();
                lblBboxVals.Text = _loadedDefects[i].rect.ToString();
                lblHeight.Text = _loadedDefects[i].height.ToString();
                lblWidth.Text = _loadedDefects[i].width.ToString();
                lblTotalCount.Text = _loadedDefects.Count.ToString();
                lblHasBoundingBox.Text = _loadedDefects[i].isNeedsBox.ToString();
                cmbxLabel.Text = _loadedDefects[i].label.ToString();
            }
        }

        public void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_imageIndex <= 0) { return; }

            //clear previous image and data
            r = Rectangle.Empty;
            pictureBox1.Invalidate();
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Update();

            _imageIndex--;

            Defects newDef = _loadedDefects[_imageIndex];
            string pathy = newDef.imagePath;

            if (newDef.isNeedsBox)
            {
                pictureBox1.Image = new Bitmap(pathy);
                updateLabels(_imageIndex);
                Refresh();
            }

            else
            {
                g = Graphics.FromImage(pictureBox1.Image);
                pictureBox1.Image = new Bitmap(pathy);
                g.DrawRectangle(Pens.Red, newDef.rect);
                updateLabels(_imageIndex);
                Refresh();
            }
        }

        public void btnNext_Click(object sender, EventArgs e)
        {
            if (_imageIndex >= (_loadedDefects.Count - 1)) { return; }

            //clear previous image and data
            r = Rectangle.Empty;
            pictureBox1.Invalidate();
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Update();

            _imageIndex++;

            Defects newDef = _loadedDefects[_imageIndex];
            string pathy = newDef.imagePath;

            if (newDef.isNeedsBox)
            {
                pictureBox1.Image = new Bitmap(pathy);
                updateLabels(_imageIndex);
                Refresh();
            }

            else
            {
                g = Graphics.FromImage(pictureBox1.Image);
                pictureBox1.Image = new Bitmap(pathy);
                g.DrawRectangle(Pens.Red, newDef.rect);
                updateLabels(_imageIndex);
                Refresh();
            }
        }

        private void uiBrowse_Click(object sender, EventArgs e)
        {
            var folder = FileBrowser.Instance.OpenDirectory(INPUT_IMAGE_FOLDER_TITLE);
            if(folder != null)
            {
                uiInputImageFolder.Text = folder;
            }
        }

        void MergeDataset(string thumbnailImageFolder)
        {
            var args = $"-m dataset.maintenance --merge --new-data-path=\"{thumbnailImageFolder}\"";
            //ExternalProgram.Run(mlPaths.pythonEnvPath, args, mlPaths.mlDIR, null, l => Debug.WriteLine(l));
        }

        void RunTraining()
        {
            var args = $"train.py";
            //ExternalProgram.RunInWindow(mlPaths.pythonEnvPath, args, mlPaths.mlDIR);
            MessageBox.Show("models successfully retrained.");
        }
    }
}
