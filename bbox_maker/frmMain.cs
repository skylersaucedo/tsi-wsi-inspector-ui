﻿//using inspectionUI.Lib;
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

namespace inspectionUI
{

    public partial class frmMain : Form
    {
        //const string INPUT_IMAGE_FOLDER_TITLE = "Input image";
        public string image_path;
        public string labelpath = @"C:\Users\sauce\OneDrive\Desktop\inspectionUI-master\inspectionUI\labels.txt";
        public string defectslog_path = @"C:\Users\sauce\OneDrive\Desktop\bbox_maker-master\bbox_maker\defectslog.txt";
        Point _mousePositionDragStart { get; set; }
        Point _mousePositionDragged { get; set; }
        public int imageIndex { get; set; }

        public int totalImages {get; set;}
        public int h { get; set; }
        public int w { get; set; }

        public double s { get; set; } //zoom factor

        bool _mouseIsDown = false;

        Rectangle r;

        public string Inspector;

        List<Defect> _loadedDefects;

        List<String> imagepaths;

        int _imageIndex { get; set; }

        public DataTable table;
        public int totalDefects;
        Image originalImage;

        //public void LoadLabels(string path)
        //{
        //    List<string> lines = new List<string>();

        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(path))
        //        {
        //            string line;
        //            while ((line = sr.ReadLine()) != null)
        //            {
        //                lines.Add(line);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("The file could not be read:");
        //        Console.WriteLine(e.Message);
        //    }

        //    // populate combobox

        //    foreach (string line in lines)
        //    {
        //        //cmbxLabel.Items.Add(line);
        //    }

        //}

        public frmMain(string inputImageFolder)
        {
            inputImageFolder = "C:\\Users\\sauce\\OneDrive\\Desktop\\make_pano\\panos";
            InitializeComponent();
            txbxInputImageFolder.Text = inputImageFolder;

            //pictureBox1.SetStyle(ControlStyles.Selectable, true);
            Inspector = txbxInspector.Text;

            table = new DataTable();
            //tbarZoom.Value = 1;
            //lblZoomVal.Text = tbarZoom.Value.ToString();

        }

        public void frmMain_Load(object sender, EventArgs e)
        {
            var inputImageFolder = txbxInputImageFolder.Text;
            if((!string.IsNullOrWhiteSpace(inputImageFolder)) && Directory.Exists(txbxInputImageFolder.Text))
            {                
                //LoadInputImageFolder(inputImageFolder);

                loadImageToPictureBox(inputImageFolder);

                //LoadLabels(labelpath);

                // load file

                PopulateDataGridView();

                addDefects2img();

                s = 1; //set original zoom factor to unity

            }
            else
            {
                //uiInputImageFolder.Text = FileBrowser.Instance.GetRecentDirectory(INPUT_IMAGE_FOLDER_TITLE);
            }
        }

        Image ZoomPicture(Image img, Size s)
        {
            Bitmap bm = new Bitmap(img, s.Width, s.Height);
            Graphics gpu = Graphics.FromImage(bm);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bm;

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

            if (r != null && _mouseIsDown)
            {
                if (_mouseIsDown)
                {
                    Rectangle r = GetDraggedRect();

                    e.Graphics.DrawRectangle(Pens.Red, r);

                    //UpdatePictureBoxWithRectangle(pictureBox1, r);
                }
                

                //e.Graphics.DrawRectangle(Pens.Red, _loadedDefects[_imageIndex].rect);

                //if ((_loadedDefects != null) && (_imageIndex < _loadedDefects.Count()))
                //{
                //    if (_mouseIsDown)
                //    {
                //        _loadedDefects[_imageIndex].rect = GetDraggedRect();
                //    }
                //    e.Graphics.DrawRectangle(Pens.Red, _loadedDefects[_imageIndex].rect);
                //}
            }

            // need to reset if button is changed.

            //if (r != null)
            //{
            //    if ((_loadedDefects != null) && (_imageIndex < _loadedDefects.Count()))
            //    {
            //        if (_mouseIsDown)
            //        {
            //            _loadedDefects[_imageIndex].rect = GetDraggedRect();
            //        }
            //        e.Graphics.DrawRectangle(Pens.Red, _loadedDefects[_imageIndex].rect);
            //    }
            //}
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
                //Refresh();

                //DrawRectangle(Pens.Red, r);

                MessageBox.Show("r is: " + r.ToString());
                MessageBox.Show("img, w, h: " + w.ToString() + " " + h.ToString());

                DialogResult result = MessageBox.Show("Adding a Defect?", "Add defect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // User clicked Yes
                    //MessageBox.Show("r is: " + r.ToString());
                    
                    Defect newDefect = new Defect();

                    newDefect.index = totalDefects +1;
                    newDefect.datetime = DateTime.Now.ToString();
                    newDefect.image_name = imagepaths[imageIndex];
                    newDefect.h = h;
                    newDefect.w = w;
                    newDefect.inspector = txbxInspector.Text;
                    newDefect.defect = "tba - sept 25";
                    
                    // account for zoom in/out factor

                    newDefect.location_x = Convert.ToInt32(r.X * (1 / s));
                    newDefect.location_y = Convert.ToInt32(r.Y * (1 / s));
                    newDefect.def_h = Convert.ToInt32(r.Height * (1 / s));
                    newDefect.def_w = Convert.ToInt32(r.Width * (1 / s));
                    newDefect.notes = "tba - sept 25";
                    newDefect.r = r;


                    frmAddDefect form = new frmAddDefect(newDefect, table);
                    // inject formclose event handler

                    form.FormClosed += new FormClosedEventHandler(frmAddDefect_FormClosed);

                    form.Show();
                }
                else if (result == DialogResult.No)
                {
                    // User clicked No
                    //pictureBox1.Invalidate();
                }
            }
        }

        private void frmAddDefect_FormClosed(object sender, FormClosedEventArgs e)
        {
            PopulateDataGridView(); //refresh datatable
            addDefects2img();
        }


        public void addDefects2img()
        {
            // add scaling factor here eventually.

            string[] defectlogs = File.ReadAllLines(defectslog_path);

            // Add data

            for (int i = 1; i < defectlogs.Length; i++)
            {

                string[] x = defectlogs[i].Split(',');

                int index = i;
                string datetime = x[1];
                string image_name = x[2];
                int h = Convert.ToInt32(x[3]);
                int w = Convert.ToInt32(x[4]);
                string inspector = x[5];
                string defect = x[6];
                int loc_x = Convert.ToInt32(x[7]);
                int loc_y = Convert.ToInt32(x[8]);
                int def_h = Convert.ToInt32(x[9]);
                int def_w = Convert.ToInt32(x[10]);
                string notes = x[11];

                // add everything to the table

                //table.Rows.Add(index, datetime, image_name, h, w, inspector, defect, loc_x, loc_y, def_h, def_w, notes);

                // update image with rects

                //Rectangle r_n = new Rectangle(loc_x, loc_y, def_h, def_w);
                Rectangle r_n = new Rectangle(loc_x, loc_y, def_w, def_h);

                UpdatePictureBoxWithRectangle(pictureBox1, r_n, Color.Red, 2);

                totalDefects = i;

            }
        }

        public void PopulateDataGridView()
        {
            DataTable table = new DataTable(); // clear out old stuff

            // Create a DataTable with 4 columns
            //DataTable table = new DataTable();
            table.Columns.Add("index", typeof(int));
            table.Columns.Add("datetime", typeof(string));
            table.Columns.Add("image_name", typeof(string));
            table.Columns.Add("h", typeof(int));
            table.Columns.Add("w", typeof(int));
            table.Columns.Add("inspector", typeof(string));
            table.Columns.Add("defect", typeof(string));
            table.Columns.Add("loc_x", typeof(int));
            table.Columns.Add("loc_y", typeof(int));
            table.Columns.Add("def_h", typeof(int));
            table.Columns.Add("def_w", typeof(int));

            table.Columns.Add("notes", typeof(string));


            string[] defectlogs = File.ReadAllLines(defectslog_path);

            // Add data

            if (defectlogs.Length > 0)
            {
                for (int i = 1; i < defectlogs.Length; i++)
                {

                    string[] x = defectlogs[i].Split(',');

                    int index = i;
                    string datetime = x[1];
                    string image_name = x[2];
                    int h = Convert.ToInt32(x[3]);
                    int w = Convert.ToInt32(x[4]);
                    string inspector = x[5];
                    string defect = x[6];
                    int loc_x = Convert.ToInt32(x[7]);
                    int loc_y = Convert.ToInt32(x[8]);
                    int def_h = Convert.ToInt32(x[9]);
                    int def_w = Convert.ToInt32(x[10]);
                    string notes = x[11];

                    // add everything to the table

                    table.Rows.Add(index, datetime, image_name, h, w, inspector, defect, loc_x, loc_y, def_h, def_w, notes);

                    // update image with rects

                    //Rectangle r_n = new Rectangle(loc_x, loc_y, def_h, def_w);
                    Rectangle r_n = new Rectangle(loc_x, loc_y, def_w, def_h);

                    UpdatePictureBoxWithRectangle(pictureBox1, r_n, Color.Red, 2);

                    totalDefects = i;

                }

                // Bind the DataTable to the DataGridView
                dataGridView1.DataSource = table;
            }

            
        }
        

        private void UpdatePictureBoxWithRectangle(PictureBox pictureBox, Rectangle rectangle, Color color, int thickness)
        {
            // update picturebox with rect
            using (Graphics graphics = pictureBox.CreateGraphics())
            {

                Pen rectPen = new Pen(color, thickness);

                graphics.DrawRectangle(rectPen, rectangle);
            }

            pictureBox.BringToFront();
            pictureBox.Update();
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
                //MergeDataset(thumbnailFolder);
                //RunTraining();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void loadImageToPictureBox(string inputImageFolder)
        {
            r = Rectangle.Empty;
            pictureBox1.Image = null;


            imagepaths = new List<string>();

            int count = 0;
            foreach (string imageFileName in Directory.GetFiles(inputImageFolder, "*.jpg"))
            {
                string name = Path.GetFileNameWithoutExtension(imageFileName);

                imagepaths.Add(imageFileName);

                count++;

            }

            imageIndex = 0;
            totalImages = count;

            originalImage = new Bitmap(imagepaths[imageIndex]);

            pictureBox1.Image = originalImage;

            w = originalImage.Width;
            h = originalImage.Height;

            lblTotalCount.Text = count.ToString();
            lblImagePath.Text = imagepaths[imageIndex];
            lblCurrentPosition.Text = imageIndex.ToString();    


            //if (x.Length == 1) 
            //    {
            //        string label = x[x.Length - 1];

            //        System.Drawing.Image img = System.Drawing.Image.FromFile(imageFileName);

            //        orgImg = img;

            //        int w = img.Width;
            //        int h = img.Height;

            //        //Defect def = new Defect();

            //        //def.imagePath = imageFileName;
            //        //def.rect = Rectangle.Empty;
            //        //def.label = label;
            //        //def.height = h;
            //        //def.width = w;
            //        //defectList.Add(def);
            //        //count++;

            //        //labels.Add(label);
            //    }

            //}

            // enable toggle buttons
            btnPrevious.Enabled = true;
            btnNext.Enabled = true;

            //// populate instance defect list
            //_loadedDefects = defectList;
            //_imageIndex = 0;

            //// update combobox with unique defect classes
            ////HashSet<string> uniqueLabels = new HashSet<string>(labels);
            ////cmbxLabel.Items.AddRange(uniqueLabels.ToArray());

            //// show first defect
            //if (_loadedDefects.Count() > 0) 
            //{
            //    pictureBox1.Image = new Bitmap(_loadedDefects[_imageIndex].imagePath);
            //    image_path = _loadedDefects[_imageIndex].imagePath;
            //    lblImagePath.Text = image_path;
            //}
            ////updateLabels(_imageIndex);

            Refresh();
        }


        public void loadDefects(string inputImageFolder)
        {
            r = Rectangle.Empty;
            pictureBox1.Image = null;

            // load folder of images, this will be automatic when user hits retrain
            // make objects

            List<Defect> defectList = new List<Defect>();
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

                    Defect def = new Defect();

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
            //updateLabels(_imageIndex);

            //Refresh();
        }

        //protected override void OnMouseWheel(MouseEventArgs e)
        //{

        //    int delta = e.Delta;
        //    int x = e.X;
        //    int y = e.Y;

        //    int zs = 50;//10
            

        //    if (e.Delta != 0)
        //    {

        //        if (delta > 0)
        //        {
        //            // Zoom in
        //            pictureBox1.Width += zs;
        //            pictureBox1.Height += zs;
        //            return;
        //        }
        //        else if (delta < 0)
        //        {
        //            // Zoom out
        //            pictureBox1.Width -= zs;
        //            pictureBox1.Height -= zs;
        //            return;
        //        }

        //        //if (e.Delta <= 0)
        //        //{
        //        //    //set minimum size to zoom
        //        //    if (pictureBox1.Width < 50)
        //        //        // lbl_Zoom.Text = pictureBox1.Image.Size; 
        //        //        return;
        //        //}
        //        //else
        //        //{
        //        //    //set maximum size to zoom
        //        //    if (pictureBox1.Width > 1000)
        //        //        return;
        //        //}
        //        pictureBox1.Width += Convert.ToInt32(pictureBox1.Width * e.Delta / 1000);
        //        pictureBox1.Height += Convert.ToInt32(pictureBox1.Height * e.Delta / 1000);
        //    }
        //}

        //public void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        //{
        //int delta = e.Delta;
        //int x = e.X;
        //int y = e.Y;

        //    if (delta > 0)
        //    {
        //        // Zoom in
        //        pictureBox1.Width += 10;
        //        pictureBox1.Height += 10;
        //    }
        //    else if (delta< 0)
        //    {
        //        // Zoom out
        //        pictureBox1.Width -= 10;
        //        pictureBox1.Height -= 10;
        //    }

//    // Center the zoom around the mouse cursor
//    int newX = x - (int)(x * 0.1);
//    int newY = y - (int)(y * 0.1);
//    pictureBox1.Location = new Point(pictureBox1.Location.X + newX, pictureBox1.Location.Y + newY);
//}

// zooming feature, may not work...
//protected override void OnMouseWheel(MouseEventArgs e)
//{
//    if (e.Delta != 0)
//    {
//        if (e.Delta <= 0)
//        {
//            //set minimum size to zoom
//            if (pictureBox1.Width < 50)
//                // lbl_Zoom.Text = pictureBox1.Image.Size; 
//                return;
//        }
//        else
//        {
//            //set maximum size to zoom
//            if (pictureBox1.Width > 1000)
//                return;
//        }
//        pictureBox1.Width += Convert.ToInt32(pictureBox1.Width * e.Delta / 1000);
//        pictureBox1.Height += Convert.ToInt32(pictureBox1.Height * e.Delta / 1000);
//    }
//}

void LoadInputImageFolder(string inputImageFolder)
        {
            r = Rectangle.Empty;
            btnLoadFolder.Enabled = true;
            txbxInputImageFolder.Enabled = false;
            //btnMakeXML.Enabled = true;
            //cmbxLabel.Enabled = true;
        }

        public void btnLoadFolder_Click(object sender, EventArgs e)
        {
            var newInputImageFolder = txbxInputImageFolder.Text;
            if (string.IsNullOrWhiteSpace(newInputImageFolder) || !Directory.Exists(newInputImageFolder))
            {
                return;
            }

            LoadInputImageFolder(newInputImageFolder);            
        }

        public void updateLabels(int i)
        {
            // update labels
            if(i < _loadedDefects.Count())
            {
                lblImagePath.Text = _loadedDefects[i].imagePath;
                image_path = lblImagePath.Text;
                lblCurrentPosition.Text = (i + 1).ToString();
                //lblBboxVals.Text = _loadedDefects[i].rect.ToString();
                //lblHeight.Text = _loadedDefects[i].height.ToString();
                //lblWidth.Text = _loadedDefects[i].width.ToString();
                //lblTotalCount.Text = _loadedDefects.Count.ToString();
                //lblHasBoundingBox.Text = _loadedDefects[i].isNeedsBox.ToString();
                //cmbxLabel.Text = _loadedDefects[i].label.ToString();
            }
        }

        public void btnPrevious_Click(object sender, EventArgs e)
        {
            if (imageIndex <= 0) { return; }

            //clear previous image and data
            r = Rectangle.Empty;
            pictureBox1.Invalidate();
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Update();

            imageIndex--;

            //pictureBox1.Image = new Bitmap(imagepaths[imageIndex]);

            originalImage = new Bitmap(imagepaths[imageIndex]);

            pictureBox1.Image = originalImage;

            w = originalImage.Width;
            h = originalImage.Height;

            lblImagePath.Text = imagepaths[imageIndex];
            lblCurrentPosition.Text = imageIndex.ToString();
            //h = pictureBox1.Image.Height;
            //w = pictureBox1.Image.Width;

            //updateLabels(_imageIndex);

            tbarZoom.Value = 25; // adjust trackbar to center
            lblZoomVal.Text = "1";

            Refresh();

            //if (newDef.isNeedsBox)
            //{
            //    pictureBox1.Image = new Bitmap(pathy);
            //    //updateLabels(_imageIndex);
            //    Refresh();
            //}

            //else
            //{
            //    g = Graphics.FromImage(pictureBox1.Image);
            //    pictureBox1.Image = new Bitmap(pathy);
            //    //g.DrawRectangle(Pens.Red, newDef.rect);
            //    //updateLabels(_imageIndex);
            //    Refresh();
            //}
        }

        public void btnNext_Click(object sender, EventArgs e)
        {
            if (imageIndex >= (totalImages - 1)) { return; }

            //clear previous image and data
            //r = Rectangle.Empty;
            pictureBox1.Invalidate();
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(pictureBox1.BackColor);
            pictureBox1.Update();

            imageIndex++;

            //Defect newDef = _loadedDefects[_imageIndex];
            //string pathy = newDef.imagePath;

            //pictureBox1.Image = new Bitmap(imagepaths[imageIndex]);

            originalImage = new Bitmap(imagepaths[imageIndex]);

            pictureBox1.Image = originalImage;

            w = originalImage.Width;
            h = originalImage.Height;

            lblImagePath.Text = imagepaths[imageIndex];
            lblCurrentPosition.Text = imageIndex.ToString();

            tbarZoom.Value = 25; // adjust trackbar to center
            lblZoomVal.Text = "1";
            //h = pictureBox1.Image.Height;
            //w = pictureBox1.Image.Width;

            //updateLabels(_imageIndex);
            Refresh();

            //if (newDef.isNeedsBox)
            //{
            //    pictureBox1.Image = new Bitmap(pathy);
            //    updateLabels(_imageIndex);
            //    Refresh();
            //}

            //else
            //{
            //    g = Graphics.FromImage(pictureBox1.Image);
            //    pictureBox1.Image = new Bitmap(pathy);
            //    //g.DrawRectangle(Pens.Red, newDef.rect);
            //    updateLabels(_imageIndex);
            //    Refresh();
            //}
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ValidateNames = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.FileName = "Folder Selection";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string folderPath = Path.GetDirectoryName(openFileDialog.FileName);
                txbxInputImageFolder.Text = folderPath;
            }
            //var folder = FileBrowser.Instance.OpenDirectory(INPUT_IMAGE_FOLDER_TITLE);
            //if(folder != null)
            //{
            //    uiInputImageFolder.Text = folder;
            //}
        }

        //void MergeDataset(string thumbnailImageFolder)
        //{
        //    var args = $"-m dataset.maintenance --merge --new-data-path=\"{thumbnailImageFolder}\"";
        //    ExternalProgram.Run(mlPaths.pythonEnvPath, args, mlPaths.mlDIR, null, l => Debug.WriteLine(l));
        //}

        //void RunTraining()
        //{
        //    var args = $"train.py";
        //    ExternalProgram.RunInWindow(mlPaths.pythonEnvPath, args, mlPaths.mlDIR);
        //    MessageBox.Show("models successfully retrained.");
        //}

        public void txbxInspector_TextChanged(object sender, EventArgs e)
        {
            Inspector = txbxInspector.Text;
        }

        private void tbarZoom_Scroll(object sender, EventArgs e)
        {
            if (tbarZoom.Value != 0)
            {
                pictureBox1.Image = null;

                double v = Convert.ToDouble(tbarZoom.Value);
                double m = Convert.ToDouble(tbarZoom.Maximum);

                double s = 1;


                if (v < m/2)
                {
                    // we are zooming out

                    double diff = m - v;
                    s = 1 - (diff / m);
                }

                else
                {
                    // we are zooming in
                    double diff = v - (m / 2);
                    s = 1 + (diff / m);
                }


                //double s = Convert.ToDouble(tbarZoom.Value) / Convert.ToDouble(tbarZoom.Maximum); // 1/50

                int hn = Convert.ToInt16(s*h);
                int wn = Convert.ToInt16(s*w);

                pictureBox1.Image = ZoomPicture(originalImage, new Size(wn, hn));
                lblZoomVal.Text = s.ToString();
                //MessageBox.Show("new image size: " + hn.ToString() + " " + wn.ToString());
                addDefects2img();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int i = dataGridView1.SelectedRows[0].Index + 1;
                MessageBox.Show("You selected row: " + i.ToString());

                // Get the values from the selected row
                int loc_x = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[7].Value);
                int loc_y = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[8].Value);
                var def_h = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[9].Value);
                var def_w = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[10].Value);

                Rectangle rect = new Rectangle();

                rect.X = loc_x;
                rect.Y = loc_y;
                rect.Width = def_w;
                rect.Height = def_h;

                addDefects2img();

                UpdatePictureBoxWithRectangle(pictureBox1, rect, Color.Yellow, 2);
            }
        }

        private void txbxInputImageFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDefectDetector_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Invoking Machine Learning Model... Please wait a moment... Press OK to proceed. ");
        }

        private void btnMakeReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Generating Inspection Report... Please wait a momment... Press OK to proceed.");
        }
    }
}
