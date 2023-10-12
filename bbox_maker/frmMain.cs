//using inspectionUI.Lib;
using bbox_maker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
//using UI.MachineLearning;

namespace inspectionUI
{
    public partial class frmMain : Form
    {
        public string image_path;
        public string labelpath = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\labels.txt";
        public string defectslog_path = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\defectslog.txt";
        
        // cam0 - left
        // cam1 - right
        // cam2 - center
        // left and right are mixed right now... need to adjust

        public string leftimages = @"C:\Users\TSI\Desktop\oct-10_bmp\right";
        public string rightimages = @"C:\Users\TSI\Desktop\oct-10_bmp\left";
        public string centerimages = @"C:\Users\TSI\Desktop\oct-10_bmp\center";
        public string inputImageFolder = @"C:\Users\TSI\Desktop\oct-10_bmp\center"; // Change for HMI


        public List<String> imagepaths_leftimgs;
        public List<String> imagepaths_rightimgs;
        public List<String> imagepaths_centerimgs;

        public int totalLeftImages;
        public int totalRightImages;
        public int totalCenterImages;

        public Image originalImage1;
        public Image originalImage2;
        public Image originalImage3;

        public string image_path1;
        public string image_path2;  
        public string image_path3;

        Point _mousePositionDragStart { get; set; }
        Point _mousePositionDragged { get; set; }
        public int imageIndex { get; set; }

        public int totalImages { get; set; }
        public int h { get; set; }
        public int w { get; set; }

        public int img1Index { get;set; }
        public int img2Index { get; set; }
        public int img3Index { get; set; }

        public int h1 { get; set; }
        public int w1 { get; set; }
        public int h2 { get; set; }
        public int w2 { get; set; }
        public int h3 { get; set; }
        public int w3 { get; set; }

        public double s { get; set; } //scaling, zoom factor

        bool _mouseIsDown = false;
        bool bolSyncBars = false;
        bool bolInvertImage = false;

        Rectangle r;
        public int thickness ;

        public string Inspector;

        List<Defect> _loadedDefects;

        List<String> imagepaths;

        int _imageIndex { get; set; }

        public DataTable table;
        public int totalDefects;
        Image originalImage;

        // ML stuff
        Process _pythonProcess;
        public static readonly string pythonEnvDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"miniconda3\envs\pytorch-gpu");
        public static readonly string pythonEnvPath = Path.Combine(pythonEnvDir, @"python.exe");
        public static readonly string mlDIR = @"C:\Users\TSI\source\repos\threadinspection_Aug14\UI.MachineLearning";
        public static string predictionScriptPath = Path.Combine(mlDIR, "detection_server.py");

        public int hn {get;set;}
        public int wn { get; set; }

        public string pipeID { get; set; }  


        public frmMain(string inputImageFolder)
        {
            //inputImageFolder = @"C:\Users\TSI\Desktop\oct-7";


            InitializeComponent();
            txbxInputImageFolder.Text = inputImageFolder;

            imageIndex = 1;
            lblCurrentPosition.Text = imageIndex.ToString();    

            Inspector = txbxInspector.Text;

            table = new DataTable();


        }

        public void frmMain_Load(object sender, EventArgs e)
        {
            s = 0.19;//0.15; // trying to conserve memory, 0.19 ideal
            lblZoomVal.Text = s.ToString();
            thickness = 3; // thickness of box line when user selects defect.
            tbarZoom.Maximum = 100;

            pipeID = txbxPipeID.Text;

            txbxInputImageFolder.Text = inputImageFolder;
            if((!string.IsNullOrWhiteSpace(inputImageFolder)) && Directory.Exists(txbxInputImageFolder.Text))
            {                
                loadImageToPictureBox();

                tbarImage.Maximum = totalImages;

                PopulateDataGridView();

                addDefects2img();

                // invoke ML server.. Add new warm up image, old one takes too long
                //_ = Task.Factory.StartNew(() => { ActivateMLserver(); });   // activate MLserver

            }
            else
            {
                MessageBox.Show("having trouble loading form... ");
            }
        }

        void ActivateMLserver()
        {
            try
            {
                var pyStartInfo = new ProcessStartInfo(); // run the python script
                pyStartInfo.FileName = pythonEnvPath;
                pyStartInfo.UseShellExecute = false;
                pyStartInfo.CreateNoWindow = false; //true before, we are debugging right now...
                pyStartInfo.RedirectStandardOutput = false; //true before
                pyStartInfo.RedirectStandardError = false; //true before
                pyStartInfo.Arguments = $"\"{predictionScriptPath}\"";

                using (var pythonProcess = Process.Start(pyStartInfo))
                {
                    _pythonProcess = pythonProcess;
                    pythonProcess.WaitForExit();
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "DEFECT DETECTOR ML SERVER ERROR"); // not sure what to anticpate here just yet...
                Console.WriteLine("DEFECT DETECTOR ML SERVER ERROR!" + e.Message);
                Environment.Exit(1);
            }
        }

        void MakeInspectionReport()
        {
            string inspectionGenPyPath = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\make_inspection_report.py";
            try
            {
                var pyStartInfo = new ProcessStartInfo(); // run the python script
                pyStartInfo.FileName = pythonEnvPath;
                pyStartInfo.UseShellExecute = false;
                pyStartInfo.CreateNoWindow = false; //true before, we are debugging right now...
                pyStartInfo.RedirectStandardOutput = false; //true before
                pyStartInfo.RedirectStandardError = false; //true before
                pyStartInfo.Arguments = $"\"{inspectionGenPyPath}\"";

                using (var pythonProcess = Process.Start(pyStartInfo))
                {
                    _pythonProcess = pythonProcess;
                    pythonProcess.WaitForExit();
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Unable to generate report"); // not sure what to anticpate here just yet...
                Console.WriteLine("ERROR unable to gen report!" + e.Message);
                Environment.Exit(1);
            }

        }

        public static string FindDirectory(string currentDirectory, string targetDirectory)
        {
            targetDirectory = targetDirectory.ToLower();
            // Search the current directory and all subdirectories.
            foreach (string dir in Directory.GetDirectories(currentDirectory, "*", SearchOption.TopDirectoryOnly))
            {
                if (Path.GetFileName(dir).ToLower().Equals(targetDirectory))
                    return dir;
            }

            // If the directory was not found, go up to the parent directory (if there is one) and search again.
            DirectoryInfo parentDir = Directory.GetParent(currentDirectory);
            if (parentDir != null)
            {
                return FindDirectory(parentDir.FullName, targetDirectory);
            }

            // If there is no parent directory (i.e., we are at the root) and the directory was not found, return null.
            throw new Exception($"{targetDirectory} not found in {currentDirectory} or it's parent.");
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

                    Pen rectPen = new Pen(Color.Red, thickness);
                    e.Graphics.DrawRectangle(rectPen, r);
                }
                
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (r != null && _mouseIsDown)
            {
                if (_mouseIsDown)
                {
                    Rectangle r = GetDraggedRect();

                    Pen rectPen = new Pen(Color.Red, thickness);
                    e.Graphics.DrawRectangle(rectPen, r);
                }

            }
        }


        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            if (r != null && _mouseIsDown)
            {
                if (_mouseIsDown)
                {
                    Rectangle r = GetDraggedRect();
                    Pen rectPen = new Pen(Color.Red, thickness);
                    e.Graphics.DrawRectangle(rectPen, r);
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


                DialogResult result = MessageBox.Show("Adding a Defect?", "Add defect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // User clicked Yes
                    //MessageBox.Show("r is: " + r.ToString());
                    
                    Defect newDefect = new Defect();

                    newDefect.index = totalDefects +1;
                    newDefect.datetime = DateTime.Now.ToString();
                    //newDefect.image_name = imagepaths[imageIndex];

                    newDefect.image_name = imagepaths_centerimgs[imageIndex];

                    newDefect.h = h;
                    newDefect.w = w;
                    newDefect.inspector = txbxInspector.Text;
                    newDefect.defect = "tba - sept 25";
                    
                    // account for scaling factor

                    newDefect.location_x = Convert.ToInt32(r.X / s);
                    newDefect.location_y = Convert.ToInt32(r.Y / s);
                    newDefect.def_h = Convert.ToInt32(r.Height / s);
                    newDefect.def_w = Convert.ToInt32(r.Width / s);
                    newDefect.notes = "tba - sept 25";
                    newDefect.r = r; // r wrt scaled image
                    newDefect.image_index = imageIndex;
                    newDefect.pipe_id = pipeID;


                    frmAddDefect form = new frmAddDefect(newDefect, table);
                    // inject formclose event handler

                    form.FormClosed += new FormClosedEventHandler(frmAddDefect_FormClosed);

                    form.Show();
                }
                else if (result == DialogResult.No)
                {
                    // User clicked No
                    pictureBox1.Invalidate();
                    pictureBox2.Invalidate();
                    pictureBox3.Invalidate();

                    // you will need to add previous defects to show

                }
            }
        }

        private void frmSelectProject_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void frmAddDefect_FormClosed(object sender, FormClosedEventArgs e)
        {
            PopulateDataGridView(); //refresh datatable
            addDefects2img();
        }


        public void addDefects2img()
        {

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
                int loc_x = Convert.ToInt32(Convert.ToDouble(x[7])*s);
                int loc_y = Convert.ToInt32(Convert.ToDouble(x[8]) * s);
                int def_h = Convert.ToInt32(Convert.ToDouble(x[9]) * s);
                int def_w = Convert.ToInt32(Convert.ToDouble(x[10]) * s);
                string notes = x[11];
                string image_index = x[12];

                // add everything to the table

                //table.Rows.Add(index, datetime, image_name, h, w, inspector, defect, loc_x, loc_y, def_h, def_w, notes);

                // update image with rects

                if (image_name == image_path)
                {
                    Rectangle r_n = new Rectangle(loc_x, loc_y, def_w, def_h);

                    UpdatePictureBoxWithRectangle(pictureBox1, r_n, Color.Red, thickness);

                    totalDefects = i;
                }


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
            table.Columns.Add("image_index", typeof(int));
            table.Columns.Add(columnName: "pipe_id", typeof(string));



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
                    string image_index = x[12];
                    string pipe_id = x[13];

                    // add everything to the table

                    table.Rows.Add(index, datetime, image_name, h, w, inspector, defect, loc_x, loc_y, def_h, def_w, notes, image_index, pipe_id);

                    // update image with rects

                    //Rectangle r_n = new Rectangle(loc_x, loc_y, def_h, def_w);
                    Rectangle r_n = new Rectangle(loc_x, loc_y, def_w, def_h);

                    UpdatePictureBoxWithRectangle(pictureBox1, r_n, Color.Red, thickness);

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

        public List<string> populateImageList(string inputFolder)
        {
            //string ext = "*.jpg";
            string ext = "*.bmp";
            //string ext = "*.png";

            List<string> outList = new List<string>();
            foreach (string imageFileName in Directory.GetFiles(inputFolder, searchPattern: ext)) //bmp files are huge!
            {
                string name = Path.GetFileNameWithoutExtension(imageFileName);
                outList.Add(imageFileName);
            }
            return outList;
        }

        public void loadImageToPictureBox()
        {
            //s = 0.25; // mag factor

            r = Rectangle.Empty;
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;

            // modify for 3 image views
            // cam0 - left
            // cam1 - right
            // cam2 - center

            imagepaths_leftimgs = populateImageList(leftimages);
            imagepaths_rightimgs = populateImageList(rightimages);
            imagepaths_centerimgs = populateImageList(centerimages);

            List<int> counts = new List<int>();
            counts.Add(imagepaths_leftimgs.Count());
            counts.Add(imagepaths_rightimgs.Count());
            counts.Add(imagepaths_centerimgs.Count());

            tbarImage.Maximum = counts.Min();


            img1Index = 0;
            img2Index = 0;
            img3Index = 0;

            totalLeftImages = imagepaths_leftimgs.Count;
            totalRightImages = imagepaths_rightimgs.Count;
            totalCenterImages = imagepaths_centerimgs.Count;

            originalImage1 = new Bitmap(imagepaths_leftimgs[img1Index]);
            originalImage2 = new Bitmap(imagepaths_rightimgs[img2Index]);
            originalImage3 = new Bitmap(imagepaths_centerimgs[img3Index]);


            pictureBox1.Image = originalImage1;
            pictureBox2.Image = originalImage2;
            pictureBox3.Image = originalImage3;

            image_path1 = imagepaths_leftimgs[img1Index];
            image_path2 = imagepaths_rightimgs[img2Index];
            image_path3 = imagepaths_centerimgs[img3Index];

            // need a way to organize w and h vals
            w = originalImage1.Width;
            h = originalImage1.Height;

            w1 = originalImage1.Width;
            h1 = originalImage1.Height;

            w2 = originalImage2.Width;
            h2 = originalImage2.Height;

            w3 = originalImage3.Width;
            h3 = originalImage3.Height;

            pictureBox1.Image = ZoomPicture(originalImage3, new Size(Convert.ToInt32(s * w1), Convert.ToInt32(s * h1)));
            pictureBox2.Image = ZoomPicture(originalImage2, new Size(Convert.ToInt32(s * w2), Convert.ToInt32(s * h2)));
            pictureBox3.Image = ZoomPicture(originalImage1, new Size(Convert.ToInt32(s * w3), Convert.ToInt32(s * h3)));

            totalImages = totalCenterImages;

            lblTotalCount.Text = totalImages.ToString();
            //lblImagePath.Text = imagepaths[imageIndex];
            lblCurrentPosition.Text = imageIndex.ToString();

            btnPrevious.Enabled = true;
            btnNext.Enabled = true;

            Refresh();
        }


        void LoadInputImageFolder(string inputImageFolder)
        {
            r = Rectangle.Empty;
            btnLoadFolder.Enabled = true;
            txbxInputImageFolder.Enabled = false;
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
            }
        }

        public void adjustpicturebox(PictureBox pbox, string camera)
        {


            //clear previous image and data
            r = Rectangle.Empty;
            pbox.Image.Dispose(); // dispose old img.
            pbox.Image = null;
            GC.Collect(); // take out the trash.

            Graphics g = pbox.CreateGraphics();
            g.Clear(pbox.BackColor);
            //pbox.Update();

            //imageIndex--;

            //pictureBox1.Image = new Bitmap(imagepaths[imageIndex]);

            if (camera == "left")
            {
                if (imageIndex < imagepaths_leftimgs.Count)
                {
                    originalImage = new Bitmap(imagepaths_leftimgs[imageIndex]);
                }
            }


            if (camera == "right")
            {
                if (imageIndex < imagepaths_rightimgs.Count)
                {
                    originalImage = new Bitmap(imagepaths_rightimgs[imageIndex]);
                }
              
            }


            if (camera == "center")
            {
                if (imageIndex < imagepaths_centerimgs.Count)
                {
                    originalImage = new Bitmap(imagepaths_centerimgs[imageIndex]);
                }
            }

            pbox.Image = originalImage;

            w = originalImage.Width;
            h = originalImage.Height;

            g.Clear(pbox.BackColor);
            hn = Convert.ToInt16(s * h);
            wn = Convert.ToInt16(s * w);

            Image image2show = ZoomPicture(originalImage, new Size(wn, hn));

            if (bolInvertImage)
            {
                
                Image image2showInverted = InvertMyImage(image2show);
                pbox.Image = image2showInverted;
            }

            else
            {
                pbox.Image = image2show;
            }

            pbox.Update();

            //lblImagePath.Text = imagepaths[imageIndex];
            lblCurrentPosition.Text = imageIndex.ToString();

            int newVal = Convert.ToInt32(s * tbarZoom.Maximum);

            if (newVal >= tbarZoom.Maximum)
            {
                tbarZoom.Maximum = newVal;
            }

            tbarZoom.Value = Convert.ToInt32(newVal); // adjust trackbar to center
            lblZoomVal.Text = s.ToString();

            pbox.Refresh();

            g = null;

            //addDefects2img();
        }

        public void btnPrevious_Click(object sender, EventArgs e)
        {

            if (tbarImage.Value - 1 >= 0)
            {
                // cam0 - left
                // cam1 - right
                // cam2 - center

                imageIndex--;

                tbarImage.Value = tbarImage.Value - 1;

                adjustpicturebox(pictureBox1, "center");
                adjustpicturebox(pictureBox2, "left");
                adjustpicturebox(pictureBox3, "right");

                Refresh();
            }
        }

        public void btnNext_Click(object sender, EventArgs e)
        {
            imageIndex++;

            if (imageIndex <= totalImages)
            {
                //
                tbarImage.Value = tbarImage.Value + 1;

                adjustpicturebox(pictureBox1, "center");
                adjustpicturebox(pictureBox2, "left");
                adjustpicturebox(pictureBox3, "right");

                Refresh();
            }

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

        public void tbarZoom_Scroll(object sender, EventArgs e)
        {
            if (tbarZoom.Value != 0)
            {
                //pictureBox1.Image = null;
                //pictureBox2.Image = null;
                //pictureBox3.Image = null;

                double v = Convert.ToDouble(tbarZoom.Value);
                double m = Convert.ToDouble(tbarZoom.Maximum);

                //double s = 1;


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

                hn = Convert.ToInt16(s*h);
                wn = Convert.ToInt16(s*w);

                //adjustpicturebox(pictureBox1, "center");
                //adjustpicturebox(pictureBox2, "left");
                //adjustpicturebox(pictureBox3, "right");

                originalImage2 = new Bitmap(imagepaths_leftimgs[imageIndex]);
                originalImage3 = new Bitmap(imagepaths_rightimgs[imageIndex]);
                originalImage1 = new Bitmap(imagepaths_centerimgs[imageIndex]);

                pictureBox1.Image.Dispose();
                pictureBox2.Image.Dispose();
                pictureBox3.Image.Dispose();


                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;

                GC.Collect(); // take out the trash.

                // inverted images?

                Image image2show1 = ZoomPicture(originalImage1, new Size(wn, hn));
                Image image2show2 = ZoomPicture(originalImage2, new Size(wn, hn));
                Image image2show3 = ZoomPicture(originalImage3, new Size(wn, hn));

                if (bolInvertImage)
                {

                    Image image2showInverted1 = InvertMyImage(image2show1);
                    Image image2showInverted2 = InvertMyImage(image2show2);
                    Image image2showInverted3 = InvertMyImage(image2show3);

                    pictureBox1.Image = image2showInverted1;
                    pictureBox2.Image = image2showInverted2;
                    pictureBox3.Image = image2showInverted3;
                }

                else
                {
                    pictureBox1.Image = image2show1;
                    pictureBox2.Image = image2show2;
                    pictureBox3.Image = image2show3;
                }

                pictureBox1.Update();
                pictureBox2.Update();
                pictureBox3.Update();

                pictureBox1.Refresh();
                pictureBox2.Refresh();
                pictureBox3.Refresh();


                lblZoomVal.Text = s.ToString();
                //MessageBox.Show("new image size: " + hn.ToString() + " " + wn.ToString());
                //addDefects2img();
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

                // Get the values from the selected row
                int loc_x = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[7].Value);
                int loc_y = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[8].Value);
                var def_h = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[9].Value);
                var def_w = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[10].Value);

                var image_path_name = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();

                Rectangle rect = new Rectangle();

                rect.X = Convert.ToInt32(loc_x * s);
                rect.Y = Convert.ToInt32(loc_y * s);
                rect.Width = Convert.ToInt32(def_w * s);
                rect.Height = Convert.ToInt32(def_h * s);

                addDefects2img();

                UpdatePictureBoxWithRectangle(pictureBox1, rect, Color.Yellow, thickness);
            }
        }

        private void txbxInputImageFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDefectDetector_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Invoking Machine Learning Model... Please wait a moment... Press OK to proceed. ");

            GetDefects(image_path);

        }

        public void GetDefects(string imagePath)
        {
            // USES TCP-IP PROTOCOL FOR .NET 4.8 FRAMEWORK. 
            // AVOIDS USING ASYNC TASKS, TO PRESERVE OBJECT CHARACTERISTICS of GetDefects. 

            //var imagePath = Path.Combine(C.TEMP_DIR, $"dd-{DateTime.Now.ToString("yyddMMhhmmss")}.jpg");
            //bitmap.SaveAsJpg(imagePath);

            string csvpath = "";

            //List<Defect> defects = new List<Defect>();
            //List<DefectProperties> cDefects = new List<DefectProperties>();

            // @TODO: CHANGE true TO: _inspectionSettings.AppSettingsModel.ActivateDefectDetector

            //InspectionAppSettingsModel curr = new InspectionAppSettingsModel();
            //bool whatisThis = curr.ActivateDefectDetector;


            if (true) // make sure defect detection is enabled in constants. REMOVING CONSTANTS TO SETTINGS 
            {
                Console.WriteLine($"CLIENT: sending message:  {imagePath}");

                try
                {
                    var request = JsonConvert.SerializeObject(new GetDefectsRequest()
                    {
                        path = imagePath,
                        is_nose_scan = false
                    });

                    var responseData = SendNetworkRequest(request);
                    GetDefectsResponse response = JsonConvert.DeserializeObject<GetDefectsResponse>(responseData);

                    csvpath = response.csv;

                    if (File.Exists(csvpath))
                    {
                        string[] lines = File.ReadAllLines(csvpath);

                        if (lines.Length > 2) // make sure they exist
                        {
                            for (int i = 1; i < lines.Length; i++) // first line is header
                            {
                                string hm_line = lines[i].Trim();

                                string[] hm_vals = hm_line.Split(',');

                                double x = Convert.ToDouble(hm_vals[1]);
                                double y = Convert.ToDouble(hm_vals[2]);
                                string label = hm_vals[4];

                                DefectProperties addMe = new DefectProperties();
                                addMe.X = x;
                                addMe.Y = y;
                                addMe.BestTagName = label;

                                //cDefects.Add(addMe);
                            }
                        }
                    }

                    //// now construct defect object for UI

                    //double s = 96; //128
                    //foreach (DefectProperties d in cDefects)
                    //{
                    //    Rect rect = new Rect(
                    //        (int)(d.X - s / 2),
                    //        (int)(d.Y - s / 2),
                    //        (int)s,
                    //        (int)s);

                    //    BitmapSource thumbnail = DefectDetectorCommon.CreateDefectThumbnail(bitmap, rect);

                    //    defects.Add(item: new Defect(rect, d.BestTagName, thumbnail));
                    //}
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show(ex.Message);
                }

            }
            var returnsomethinghere = 3;
            //return defects;
        }

        private void btnMakeReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Generating Inspection Report... Please wait a momment... Press OK to proceed.");
            MakeInspectionReport();
        }

        private void lblZoomVal_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            //addDefects2img();
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            //addDefects2img();
        }

        string SendNetworkRequest(string requestMessage)
        {
            using (TcpClient clientSocket = new TcpClient())
            {
                clientSocket.Connect(hostname: "127.0.0.1", port: 5047); // "127.0.0.1, port = 80
                NetworkStream serverStream = clientSocket.GetStream();
                var messageData = Encoding.UTF8.GetBytes(requestMessage);
                serverStream.Write(messageData, 0, messageData.Length);
                using (var sr = new StreamReader(serverStream, Encoding.UTF8))
                {
                    string responseData = sr.ReadToEnd();
                    Console.WriteLine(responseData);
                    return responseData;
                }
            }
        }

        private void tbarImage_Scroll(object sender, EventArgs e)
        {
            //imageIndex = imageIndex + tbarImage.Value; 

            imageIndex = tbarImage.Value;

            adjustpicturebox(pictureBox1, "center");
            adjustpicturebox(pictureBox2, "left");
            adjustpicturebox(pictureBox3, "right");

            Refresh();
        }



        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Scroll(object sender, ScrollEventArgs e)
        {

            if (bolSyncBars)
            {
                panel1.HorizontalScroll.Value = panel3.HorizontalScroll.Value;
                panel2.HorizontalScroll.Value = panel3.HorizontalScroll.Value;

                panel1.VerticalScroll.Value = panel3.VerticalScroll.Value;
                panel2.VerticalScroll.Value = panel3.VerticalScroll.Value;

                panel1.Update();
                panel2.Update();
            }

        }

        private void panel2_Scroll(object sender, ScrollEventArgs e)
        {
            if (bolSyncBars)
            {
                panel1.HorizontalScroll.Value = panel2.HorizontalScroll.Value;
                panel3.HorizontalScroll.Value = panel2.HorizontalScroll.Value;

                panel1.VerticalScroll.Value = panel2.VerticalScroll.Value;
                panel3.VerticalScroll.Value = panel2.VerticalScroll.Value;

                panel1.Update();
                panel3.Update();
            }
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            if (bolSyncBars)
            {
                panel2.HorizontalScroll.Value = panel1.HorizontalScroll.Value;
                panel3.HorizontalScroll.Value = panel1.HorizontalScroll.Value;

                panel2.VerticalScroll.Value = panel1.VerticalScroll.Value;
                panel3.VerticalScroll.Value = panel1.VerticalScroll.Value;

                panel2.Update();
                panel3.Update();
            }




            // picture is redrawn, update picturebox accordingly.

            //addDefects2img();
        }

        private void cxbxSnycScrollbars_CheckedChanged(object sender, EventArgs e)
        {
            if (cxbxSnycScrollbars.Checked)
            {
                bolSyncBars = true;
            }
            else
            {
                bolSyncBars = false;
            }
        }

        public void txbxPipeID_TextChanged(object sender, EventArgs e)
        {
            pipeID = txbxPipeID.Text;
        }

        public void cxbxInvertImages_CheckedChanged(object sender, EventArgs e)
        {
         
            if (cxbxInvertImages.Checked)
            {
                MessageBox.Show("Images will be inverted. Please note this computationally intensive and will slow performance.");
                bolInvertImage = true;
            }

            else
            {
                bolInvertImage = false;
            }

            adjustpicturebox(pictureBox1, "center");
            adjustpicturebox(pictureBox2, "left");
            adjustpicturebox(pictureBox3, "right");

            Refresh();
        }

        public Image InvertMyImage(Image inputImage)
        {
            Bitmap bmp = new Bitmap(inputImage);

            for (int x = 0; x < bmp.Width; x++) 
            {
                for (int y = 0; y < bmp.Height; y++) 
                {
                    Color c = bmp.GetPixel(x, y);

                    Color invertedColor = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);

                    bmp.SetPixel(x,y, invertedColor);
                }
            }

            Image invertedImage = (Image)bmp;
            return invertedImage;

        }
    }

    public class GetDefectsRequest
    {
        public string msg_type { get; set; } = "eval";
        public string path { get; set; }
        public bool is_nose_scan { get; set; }
    }

    public class GetDefectsResponse
    {
        public double elapsed_time { get; set; }
        public int num_defects { get; set; }
        public string path { get; set; }
        public string csv { get; set; }
    }
    public class DefectProperties
    {
        // added to utilize new DL model
        // TODO - UPDATE by removing Cognex object artifacts

        public string FilePath { get; set; }
        public double X { get; set; }

        public double Y { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public string BestTagName { get; set; }
        public double BestTagScore { get; set; }

        public double Redscore { get; set; }

        public List<string> Tags { get; set; }
        public List<double> GreenScores { get; set; }

    }
}
