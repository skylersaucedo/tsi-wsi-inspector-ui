//using inspectionUI.Lib;
using inspectorUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
//using UI.MachineLearning;

namespace inspectionUI
{
    public partial class frmMain : Form
    {
        // soon to be defined when frm is loaded
        public string projectFolderPath;

        // all paths defined here
        public uiPaths appPaths = new uiPaths();

        // instantiate view objects
        public uiViews leftView = new uiViews();
        public uiViews rightView = new uiViews();
        public uiViews centerView = new uiViews();

        public DataTable table;

        public string image_path;

        public bool bolSyncBars = false;
        public bool bolInvertImage = false;

        Point _mousePositionDragStart { get; set; }
        Point _mousePositionDragged { get; set; }
        Rectangle r;
        public int thickness ;

        public string Inspector;

        public int totalDefects;

        // ML stuff
        Process _pythonProcess;

        public double s { get; set; } //scaling, zoom factor

        public string pipeID { get; set; }  
        public string projectID { get; set; }
        public int pass { get; set; }
        public string pipeSide { get; set; }

        public int imageIndex { get; set; }
        public int totalImages { get; set; }

        public frmMain(string _projectFolderPath)
        {
            InitializeComponent();

            projectFolderPath = _projectFolderPath;
            txbxInputImageFolder.Text = projectFolderPath;

            imageIndex = 0;

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

            MessageBox.Show("Please select Pin/Box and pass number to view images from: ; " + projectFolderPath);

            // populate combo boxes for user to select PIN or BOX

            cxbxPassNumber.Enabled = false; // do not enable until user makes selection

            projectID = projectFolderPath;

            string workingDIR = projectFolderPath + "\\" + "RAW";
            string[] subdirectoryEntries = Directory.GetDirectories(workingDIR);

            foreach (string subdirectory in subdirectoryEntries)
            {
                string[] splitme = subdirectory.Split('\\');
                string boxOrPin = splitme.Last();
                cxbxPINorBOX.Items.Add(boxOrPin);
            }

            
            cxbxPINorBOX.Text = cxbxPINorBOX.Items[0].ToString(); // options avail to user.



            txbxInputImageFolder.Text = projectFolderPath;
            if((!string.IsNullOrWhiteSpace(projectFolderPath)) && Directory.Exists(txbxInputImageFolder.Text))
            {                
                //loadImageToPictureBox();

                //tbarImage.Maximum = totalImages;

                //PopulateDataGridView();

                //addDefects2img();

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
                pyStartInfo.FileName = uiPaths.pythonEnvPath;
                pyStartInfo.UseShellExecute = false;
                pyStartInfo.CreateNoWindow = false; //true before, we are debugging right now...
                pyStartInfo.RedirectStandardOutput = false; //true before
                pyStartInfo.RedirectStandardError = false; //true before
                pyStartInfo.Arguments = $"\"{uiPaths.predictionScriptPath}\"";

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
            try
            {
                var pyStartInfo = new ProcessStartInfo(); // run the python script
                pyStartInfo.FileName = uiPaths.pythonEnvPath;
                pyStartInfo.UseShellExecute = false;
                pyStartInfo.CreateNoWindow = false; //true before, we are debugging right now...
                pyStartInfo.RedirectStandardOutput = false; //true before
                pyStartInfo.RedirectStandardError = false; //true before
                pyStartInfo.Arguments = $"\"{uiPaths.inspectionGenPyPath}\" \"{projectFolderPath}\"";


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

        //Image ZoomPicture(Image img, Size s)
        //{
        //    Bitmap bm = new Bitmap(img, s.Width, s.Height);
        //    Graphics gpu = Graphics.FromImage(bm);
        //    gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        //    return bm;

        //}


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

            if (r != null && centerView.isMouseDown)
            {
                if (centerView.isMouseDown)
                {
                    Rectangle r = GetDraggedRect();

                    Pen rectPen = new Pen(Color.Red, thickness);
                    e.Graphics.DrawRectangle(rectPen, r);
                }
                
            }
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (r != null && leftView.isMouseDown)
            {
                if (leftView.isMouseDown)
                {
                    Rectangle r = GetDraggedRect();

                    Pen rectPen = new Pen(Color.Red, thickness);
                    e.Graphics.DrawRectangle(rectPen, r);
                }

            }
        }


        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            if (r != null && rightView.isMouseDown)
            {
                if (rightView.isMouseDown)
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
                centerView.isMouseDown = true;
                _mousePositionDragStart = e.Location;
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                leftView.isMouseDown = true;
                _mousePositionDragStart = e.Location;
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rightView.isMouseDown = true;
                _mousePositionDragStart = e.Location;
            }
        }

        public void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (centerView.isMouseDown)
            {
                r = GetDraggedRect();
                centerView.isMouseDown = false;

                addingAdefect("pbox1");
            }
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftView.isMouseDown)
            {
                r = GetDraggedRect();
                leftView.isMouseDown = false;

                addingAdefect("pbox2");
            }
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            if (rightView.isMouseDown)
            {
                r = GetDraggedRect();
                rightView.isMouseDown = false;

                addingAdefect("pbox3");
            }
        }

        public void addingAdefect(string picturebox)
        {

            DialogResult result = MessageBox.Show("Adding a Defect?", "Add defect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Defect newDefect = new Defect();

                int cam = 0;

                if (picturebox == "pbox1")
                {
                    newDefect.image_name = centerView.images[imageIndex];
                    cam = 0;
                }

                if (picturebox == "pbox2")
                {
                    newDefect.image_name = leftView.images[imageIndex];
                    cam = 1;
                }

                if (picturebox == "pbox3")
                {
                    newDefect.image_name = rightView.images[imageIndex];
                    cam = 2;
                }

                newDefect.index = totalDefects + 1;
                newDefect.datetime = DateTime.Now.ToString();
                //newDefect.image_name = imagepaths[imageIndex];


                //newDefect.image_name = imagepaths_centerimgs[imageIndex];

                Bitmap img = new Bitmap(newDefect.image_name);

                newDefect.h = img.Height;
                newDefect.w = img.Width;
                img.Dispose();
                newDefect.inspector = txbxInspector.Text;

                // account for scaling factor

                newDefect.location_x = Convert.ToInt32(r.X / s);
                newDefect.location_y = Convert.ToInt32(r.Y / s);
                newDefect.def_h = Convert.ToInt32(r.Height / s);
                newDefect.def_w = Convert.ToInt32(r.Width / s);
                newDefect.r = r; // r wrt scaled image
                newDefect.image_index = imageIndex;
                newDefect.pipe_id = pipeID;
                newDefect.project = projectID;
                newDefect.cam = cam;
                newDefect.pass= pass;
                newDefect.pipe_side = pipeSide;

                newDefect.defect = "to be added by inspector";
                newDefect.notes = "to be added by inspector";



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

        //private void frmSelectProject_FormClosed(object sender, FormClosedEventArgs e)
        //{

        //}

        private void frmAddDefect_FormClosed(object sender, FormClosedEventArgs e)
        {
            PopulateDataGridView(); //refresh datatable
            addDefects2img();
        }


        public void addDefects2img()
        {

            string[] defectlogs = File.ReadAllLines(uiPaths.defectslog_path);

            // Add data

            for (int i = 1; i < defectlogs.Length; i++)
            {

                string[] x = defectlogs[i].Split(',');

                int index = i;
                string datetime = x[1];
                string inspector = x[2];
                string pipe_ID = x[3];
                string defect = x[4];
                string notes = x[5];
                string project = x[6];
                string pipe_side = x[7];
                int pass = Convert.ToInt32(x[8]);
                int cam = Convert.ToInt32(x[9]);
                int img_idx = Convert.ToInt32(x[10]);
                string img_name = x[11];
                int h = Convert.ToInt32(x[12]);
                int w = Convert.ToInt32(x[13]);
                int loc_x = Convert.ToInt32(x[14]);
                int loc_y = Convert.ToInt32(x[15]);
                int def_h = Convert.ToInt32(x[16]);
                int def_w = Convert.ToInt32(x[17]);

                // add everything to the table

                //table.Rows.Add(index, datetime, image_name, h, w, inspector, defect, loc_x, loc_y, def_h, def_w, notes);

                // update image with rects

                if (img_name == image_path)
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

            table.Columns.Add("index", typeof(int));
            table.Columns.Add("datetime", typeof(string));
            table.Columns.Add("inspector", typeof(string));
            table.Columns.Add("pipe_id", typeof(string));
            table.Columns.Add("defect", typeof(string));
            table.Columns.Add("notes", typeof(string));
            table.Columns.Add("project", typeof(string));
            table.Columns.Add("pipe_side", typeof(string));
            table.Columns.Add("pass_number", typeof(int));
            table.Columns.Add("cam_number", typeof(int));
            table.Columns.Add("img_idx", typeof(int));
            table.Columns.Add("image_name", typeof(string));
            table.Columns.Add("img_h", typeof(int));
            table.Columns.Add("img_w", typeof(int));
            table.Columns.Add("loc_x", typeof(int));
            table.Columns.Add("loc_y", typeof(int));
            table.Columns.Add("def_h", typeof(int));
            table.Columns.Add("def_w", typeof(int));

            string[] defectlogs = File.ReadAllLines(uiPaths.defectslog_path);

            // Add data

            if (defectlogs.Length > 0)
            {
                for (int i = 1; i < defectlogs.Length; i++)
                {

                    string[] x = defectlogs[i].Split(',');

                    int index = i;
                    string datetime = x[1];
                    string inspector = x[2];
                    string pipe_ID = x[3];
                    string defect = x[4];
                    string notes = x[5];
                    string project = x[6];
                    string pipe_side = x[7];
                    int pass = Convert.ToInt32(x[8]);
                    int cam = Convert.ToInt32(x[9]);
                    int img_idx = Convert.ToInt32(x[10]);
                    string img_name = x[11];
                    int h = Convert.ToInt32(x[12]);
                    int w = Convert.ToInt32(x[13]);
                    int loc_x = Convert.ToInt32(x[14]);
                    int loc_y = Convert.ToInt32(x[15]);
                    int def_h = Convert.ToInt32(x[16]);
                    int def_w = Convert.ToInt32(x[17]);

                    // add everything to the table

                    table.Rows.Add(index, datetime, inspector, pipe_ID, defect, notes, project, pipe_side, pass, cam, img_idx, img_name, h, w, loc_x, loc_y, def_h, def_w);

                    // update image with rects

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
            if (centerView.isMouseDown)
            {
                _mousePositionDragged = e.Location;
                r = GetDraggedRect();
                Refresh();
            }
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {
            // get current point of x and y
            if (rightView.isMouseDown)
            {
                _mousePositionDragged = e.Location;
                r = GetDraggedRect();
                Refresh();
            }
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            // get current point of x and y
            if (leftView.isMouseDown)
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

        //public void btnMakeXML_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if((_loadedDefects == null) || (_loadedDefects.Count == 0)) 
        //        {
        //            MessageBox.Show("No images are loaded into the dataset. Please use the 'Load Folder' button to load images.");
        //            return; 
        //        }

        //        var notLabeledCount = _loadedDefects.Where(ld => ld.isNeedsBox).Count();
        //        if (notLabeledCount != 0)
        //        {
        //            MessageBox.Show($"unable to export xml file. Number of missing boxes: {notLabeledCount}");
        //            return;
        //        }

        //        for (int i = 0; i < _loadedDefects.Count(); i++)
        //        {
        //            string w = _loadedDefects[i].width.ToString();
        //            string h = _loadedDefects[i].height.ToString();
        //            string label = _loadedDefects[i].label;

        //            string xmin = _loadedDefects[i].rect.X.ToString();
        //            string xmax = (_loadedDefects[i].rect.X + _loadedDefects[i].rect.Width).ToString();
        //            string ymin = _loadedDefects[i].rect.Y.ToString();
        //            string ymax = (_loadedDefects[i].rect.Y + _loadedDefects[i].rect.Height).ToString();

        //            makeXMLfile(_loadedDefects[i].imagePath, h, w, label, xmin, xmax, ymin, ymax);
        //        }

        //        var thumbnailFolder = Path.GetDirectoryName(_loadedDefects[0].imagePath);
        //        //MergeDataset(thumbnailFolder);
        //        //RunTraining();
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error");
        //    }
        //}

        //public List<string> populateImageList(string inputFolder)
        //{
        //    string ext = "*.bmp";

        //    List<string> outList = new List<string>();
        //    foreach (string imageFileName in Directory.GetFiles(inputFolder, searchPattern: ext)) //bmp files are huge!
        //    {
        //        string name = Path.GetFileNameWithoutExtension(imageFileName);
        //        outList.Add(imageFileName);
        //    }
        //    return outList;
        //}

        public void loadImageToPictureBox()
        {
            // TODO: There is some confusion below about left/right/center and images assigned to pboxes...  NEED TO CLEAN THIS UP!

            r = Rectangle.Empty;
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;

            //List<int> counts = new List<int>
            //{
            //    leftView.images.Count,
            //    rightView.images.Count,
            //    centerView.images.Count
            //};

            //tbarImage.Maximum = counts.Min();

            pictureBox1.Image = centerView.MakeFirstImage(centerView.images[0]);
            pictureBox2.Image = leftView.MakeFirstImage(leftView.images[0]);
            pictureBox3.Image = rightView.MakeFirstImage(rightView.images[0]);

            // zoom pic 

            pictureBox1.Image = centerView.ZoomPicture(centerView.originalImage, new Size(Convert.ToInt32(s * centerView.w), Convert.ToInt32(s * centerView.h)));
            pictureBox2.Image = leftView.ZoomPicture(leftView.originalImage, new Size(Convert.ToInt32(s * leftView.w), Convert.ToInt32(s * leftView.h)));
            pictureBox3.Image = rightView.ZoomPicture(rightView.originalImage, new Size(Convert.ToInt32(s * rightView.w), Convert.ToInt32(s * rightView.h)));

            //totalImages = totalCenterImages;
            lblCurrentPosition.Text = imageIndex.ToString();

            btnPrevious.Enabled = true;
            btnNext.Enabled = true;

            Refresh();
        }


        public void adjustpicturebox(PictureBox pbox, string camera)
        {

            Image originalImage = null;
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
                if (imageIndex < leftView.images.Count)
                {
                    originalImage = new Bitmap(leftView.images[imageIndex]);
                }
            }


            if (camera == "right")
            {
                if (imageIndex < rightView.images.Count)
                {
                    originalImage = new Bitmap(rightView.images[imageIndex]);
                }
              
            }


            if (camera == "center")
            {
                if (imageIndex < centerView.images.Count)
                {
                    originalImage = new Bitmap(centerView.images[imageIndex]);
                }
            }

            pbox.Image = originalImage;

            int w = originalImage.Width;
            int h = originalImage.Height;

            g.Clear(pbox.BackColor);
            int hn = Convert.ToInt16(s * h);
            int wn = Convert.ToInt16(s * w);

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
                double v = Convert.ToDouble(tbarZoom.Value);
                double m = Convert.ToDouble(tbarZoom.Maximum);

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

                Image originalImage1 = new Bitmap(centerView.images[imageIndex]);
                Image originalImage2 = new Bitmap(leftView.images[imageIndex]);
                Image originalImage3 = new Bitmap(rightView.images[imageIndex]);

                pictureBox1.Image.Dispose();
                pictureBox2.Image.Dispose();
                pictureBox3.Image.Dispose();

                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;

                GC.Collect(); // take out the trash.

                // inverted images?

                Image image2show1 = ZoomPicture(originalImage1, new Size(Convert.ToInt16(s * originalImage1.Width), Convert.ToInt16(s * originalImage1.Height)));
                Image image2show2 = ZoomPicture(originalImage2, new Size(Convert.ToInt16(s * originalImage2.Width), Convert.ToInt16(s * originalImage2.Height)));
                Image image2show3 = ZoomPicture(originalImage3, new Size(Convert.ToInt16(s * originalImage3.Width), Convert.ToInt16(s * originalImage3.Height)));

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
            //string csvDefectPath = @"C:\Users\TSI\Desktop\defects.csv";
            string csvDefectPath = projectFolderPath +"\\" +"Metadata";

            MessageBox.Show("Invoking Machine Learning Model... Please wait a moment... Press OK to proceed. ");

            RunMLmodel(csvDefectPath); // takes 35 seconds per scan :(

            // read csv for defects, populate image

            addMLdefects2image(csvDefectPath);



            // ----- uses ML server below... need to adjust 

            //GetDefects(image_path);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //string sample_image_path = @"C:\Users\TSI\Desktop\oct-10_bmp\center\005_img.bmp";
            //string csv_path = @"C:\Users\TSI\Desktop\test.csv";

            ////GetDefects(sample_image_path, csv_path);

            //// Get the elapsed time as a TimeSpan
            //TimeSpan ts = sw.Elapsed;//

            //// Format and display the TimeSpan
            //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //ts.Hours, ts.Minutes, ts.Seconds,
            //ts.Milliseconds / 10);
            //Console.WriteLine("That took: {0}", elapsedTime);

        }

        public void addMLdefects2image(string csvpath)
        {
            if (File.Exists(csvpath))
            {
                string[] lines = File.ReadAllLines(csvpath);

                if (lines.Length > 2) // make sure they exist
                {
                    for (int i = 1; i < lines.Length; i++) // first line is header
                    {
                        string hm_line = lines[i].Trim();

                        string[] hm_vals = hm_line.Split(',');

                        int x_i = Convert.ToInt32(hm_vals[1]);
                        int y_i = Convert.ToInt32(hm_vals[2]);
                        int x_f = Convert.ToInt32(hm_vals[3]);
                        int y_f = Convert.ToInt32(hm_vals[4]);
                        string label = hm_vals[5];

                        Rectangle r_n = new Rectangle(x_i, y_i, x_f, y_f);

                        UpdatePictureBoxWithRectangle(pictureBox1, r_n, Color.GreenYellow, thickness);


                    }
                }
            }
        }

        public Image ZoomPicture(Image img, Size s)
        {
            Bitmap bm = new Bitmap(img, s.Width, s.Height);
            Graphics gpu = Graphics.FromImage(bm);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bm;

        }

        public void RunMLmodel(string csvDefectPath)
        {
            string mlModelPath = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\inspectorUI\invoke_ml_model.py";
            string imagepath = centerView.images[imageIndex];
            //string csvDefectPath = @"C:\Users\TSI\Desktop\defects.csv";

            try
            {
                var pyStartInfo = new ProcessStartInfo(); // run the python script
                pyStartInfo.FileName = uiPaths.pythonEnvPath;
                pyStartInfo.UseShellExecute = false;
                pyStartInfo.CreateNoWindow = false; //true before, we are debugging right now...
                pyStartInfo.RedirectStandardOutput = false; //true before
                pyStartInfo.RedirectStandardError = false; //true before
                pyStartInfo.Arguments = $"\"{mlModelPath}\" \"{imagepath}\" \"{csvDefectPath}\" ";

                using (var pythonProcess = Process.Start(pyStartInfo))
                {
                    _pythonProcess = pythonProcess;
                    pythonProcess.WaitForExit();
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("Error: " + e.Message, "Unable to invoke ML model!"); // not sure what to anticpate here just yet...
                Console.WriteLine("ERROR unable invoke ML model!" + e.Message);
                Environment.Exit(1);
            }
        }

        //public void GetDefects(string imagePath, string csvpath)
        //{
        //    // USES TCP-IP PROTOCOL FOR .NET 4.8 FRAMEWORK. 
        //    // AVOIDS USING ASYNC TASKS, TO PRESERVE OBJECT CHARACTERISTICS of GetDefects. 

        //    //var imagePath = Path.Combine(C.TEMP_DIR, $"dd-{DateTime.Now.ToString("yyddMMhhmmss")}.jpg");
        //    //bitmap.SaveAsJpg(imagePath);

        //    //string csvpath = "";

        //    //List<Defect> defects = new List<Defect>();
        //    //List<DefectProperties> cDefects = new List<DefectProperties>();

        //    // @TODO: CHANGE true TO: _inspectionSettings.AppSettingsModel.ActivateDefectDetector

        //    //InspectionAppSettingsModel curr = new InspectionAppSettingsModel();
        //    //bool whatisThis = curr.ActivateDefectDetector;


        //    if (true) // make sure defect detection is enabled in constants. REMOVING CONSTANTS TO SETTINGS 
        //    {
        //        Console.WriteLine($"CLIENT: sending message:  {imagePath}");

        //        try
        //        {
        //            var request = JsonConvert.SerializeObject(new GetDefectsRequest()
        //            {
        //                path = imagePath,
        //                is_nose_scan = false
        //            });

        //            var responseData = SendNetworkRequest(request);
        //            GetDefectsResponse response = JsonConvert.DeserializeObject<GetDefectsResponse>(responseData);

        //            csvpath = response.csv;

        //            if (File.Exists(csvpath))
        //            {
        //                string[] lines = File.ReadAllLines(csvpath);

        //                if (lines.Length > 2) // make sure they exist
        //                {
        //                    for (int i = 1; i < lines.Length; i++) // first line is header
        //                    {
        //                        string hm_line = lines[i].Trim();

        //                        string[] hm_vals = hm_line.Split(',');

        //                        double x = Convert.ToDouble(hm_vals[1]);
        //                        double y = Convert.ToDouble(hm_vals[2]);
        //                        string label = hm_vals[4];

        //                        DefectProperties addMe = new DefectProperties();
        //                        addMe.X = x;
        //                        addMe.Y = y;
        //                        addMe.BestTagName = label;

        //                        //cDefects.Add(addMe);
        //                    }
        //                }
        //            }

        //            //// now construct defect object for UI

        //            //double s = 96; //128
        //            //foreach (DefectProperties d in cDefects)
        //            //{
        //            //    Rect rect = new Rect(
        //            //        (int)(d.X - s / 2),
        //            //        (int)(d.Y - s / 2),
        //            //        (int)s,
        //            //        (int)s);

        //            //    BitmapSource thumbnail = DefectDetectorCommon.CreateDefectThumbnail(bitmap, rect);

        //            //    defects.Add(item: new Defect(rect, d.BestTagName, thumbnail));
        //            //}
        //        }

        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //            MessageBox.Show(ex.Message);
        //        }

        //    }
        //    var returnsomethinghere = 3;
        //    //return defects;
        //}

        private void btnMakeReport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Generating Inspection Report... Please wait a momment... Press OK to proceed.");
            MakeInspectionReport();
        }

        private void lblZoomVal_Click(object sender, EventArgs e)
        {

        }

        //private void panel1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    //addDefects2img();
        //}

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
            imageIndex = tbarImage.Value;

            adjustpicturebox(pictureBox1, "center");
            adjustpicturebox(pictureBox2, "left");
            adjustpicturebox(pictureBox3, "right");

            Refresh();
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

        private void cxbxPINorBOX_SelectedIndexChanged(object sender, EventArgs e)
        {
            cxbxPassNumber.Enabled = true;
            cxbxPassNumber.Items.Clear(); // clear to avoid redundancy

            // user selected pipe side, populate pass options

            string pipeSide = cxbxPINorBOX.SelectedItem.ToString();

            string workingDIR = projectFolderPath + "\\" + "RAW" + "\\" + pipeSide;
            string[] subdirectoryEntries = Directory.GetDirectories(workingDIR);

            foreach (string subdirectory in subdirectoryEntries)
            {
                string[] splitme = subdirectory.Split('\\');
                string passNum = splitme.Last();
                cxbxPassNumber.Items.Add(passNum);
            }

        }

        private void cxbxPassNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            // user willl select pass number, refresh 
            string passNum = cxbxPassNumber.SelectedItem.ToString();
            string pinOrBox = cxbxPINorBOX.SelectedItem.ToString();

            string number = Regex.Match(passNum, @"\d+").Value;

            pass = Convert.ToInt32(number);
            pipeSide = pinOrBox;

            MessageBox.Show("Showing Images for: " + pinOrBox + " " + passNum);

            imageIndex = 0;
            tbarImage.Value = imageIndex;

            string leftimagesPath = projectFolderPath + "\\" + "RAW" + "\\" + pinOrBox + "\\" + passNum + "\\" + @"cam1\stills";
            string rightimagesPath = projectFolderPath + "\\" + "RAW" + "\\" + pinOrBox + "\\" + passNum + "\\" + @"cam2\stills";
            string centerimagesPath = projectFolderPath + "\\" + "RAW" + "\\" + pinOrBox + "\\" + passNum + "\\" + @"cam0\stills";

            leftView.PopulateImageList(leftimagesPath);
            rightView.PopulateImageList(rightimagesPath);
            centerView.PopulateImageList(centerimagesPath);

            List<int> counts = new List<int>
            {
                leftView.images.Count,
                rightView.images.Count,
                centerView.images.Count
            };

            totalImages = counts.Min();
            lblTotalCount.Text = totalImages.ToString();

            loadImageToPictureBox();

            tbarImage.Maximum = totalImages;

            PopulateDataGridView();

            addDefects2img();
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

}
