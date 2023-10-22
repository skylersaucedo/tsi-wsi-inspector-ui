using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inspectionUI
{
    public partial class frmAddDefect : Form
    {
        //public string defects_path = @"C:\Users\sauce\OneDrive\Desktop\inspectorUI-master\inspectorUI\labels.txt";
        //public string defectslog_path = @"C:\Users\sauce\OneDrive\Desktop\inspectorUI-master\inspectorUI\defectslog.txt";

        public string defects_path = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\inspectorUI\labels.txt";
        public string defectslog_path = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\inspectorUI\defectslog.txt";


        public List<string> currentDefects = new List<string>();

        public DataTable table;

        public Rectangle r;

        public Defect d;

        public frmAddDefect(Defect _d, DataTable _table) //r, imgName, Inspector
        {
            InitializeComponent();

            //Defect d = new Defect();

            table = _table;
            d = _d;

            // populate box with current defects
            LoadLabels(defects_path);

        }

        public void LoadLabels(string path)
        {
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                        currentDefects.Add(line);
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
                cmbxLabels.Items.Add(line);
            }

            cmbxLabels.SelectedIndex = 0;

        }

        private void btnAddDefect_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txbxAddNewDefect.Text))
            {
                int idx = cmbxLabels.SelectedIndex; //assuming user selected a label from the index

                // update defect with selection

                Defect newDefect = d;

                newDefect.datetime = DateTime.Now.ToString();
                newDefect.defect = currentDefects[idx].ToString();
                newDefect.notes = txbxNotes.Text;

                addDefectToFile(newDefect);

                this.Close(); //added. close the form.
            }

            else
            {
                // text in field, so maybe new defect class?

                DialogResult result = MessageBox.Show("Are you adding a new defect class? ", "New Defect Class?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Yes

                    string newDefectClass = txbxAddNewDefect.Text;

                    currentDefects.Add(newDefectClass);
                    cmbxLabels.Items.Add(newDefectClass);

                    // populate defect object and add to collection

                    Defect newDefect = d;
                    newDefect.datetime = DateTime.Now.ToString();
                    newDefect.defect = newDefectClass;
                    newDefect.notes = txbxNotes.Text;

                    addDefectClasstoIndex(newDefectClass); // add new label to class
                    addDefectToFile(newDefect); // add new defect to file
                    this.Close(); //added. close the form.

                }
                else if (result == DialogResult.No)
                {
                    // User clicked no...so.
                    this.Close(); //added. close the form.
                }
            }
        }

        public void addDefectClasstoIndex(string newdefect)
        {
            try
            {

                string[] names = File.ReadAllLines(defects_path);
                string newName = newdefect;
                Array.Resize(ref names, names.Length + 1);
                names[names.Length - 1] = newName;
                File.WriteAllLines(defects_path, names);
            }

            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message);
            }
        }

        public void addDefectToFile(Defect d)
        {

            try
            {
                
                string[] records = File.ReadAllLines(defectslog_path);

                if (records.Length == 0)
                {
                    // TODO: NEED TO ADDRESS THIS LATER
                    // make sure header is in .csv file or you're going to get an out of index error!
                    records[0] = "";
                    //add header
                    string header = "index, datetime, image_name, h, w, inspector, defect, loc_x, loc_y, def_h, def_w, notes;";
                    records[0] = header;
               
                }

                //string header = records[0];


                //string newrecord = "index, datetime, image_name, h, w, inspector, defect, loc_x, loc_y, def_h, def_w, notes;"; //add new record here

                //string newrecord = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}, {12}, {13}", d.index, d.datetime, d.image_name, d.h, d.w, d.inspector, d.defect, d.location_x, d.location_y, d.def_h, d.def_w, d.notes, d.image_index, d.pipe_id);
                string newrecord = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17}",
                    d.index,
                    d.datetime,
                    d.inspector,
                    d.pipe_id,
                    d.defect,
                    d.notes,
                    d.project,
                    d.pipe_side,
                    d.pass,
                    d.cam,
                    d.image_index,
                    d.image_name,
                    d.h,
                    d.w,
                    d.location_x,
                    d.location_y,
                    d.def_h,
                    d.def_w
                    );

                string newName = d.label;
                Array.Resize(ref records, records.Length + 1);
                records[records.Length - 1] = newrecord;
                File.WriteAllLines(defectslog_path, records);
            }

            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message);
            }
        }

        private void cxbxAddNotes_CheckedChanged(object sender, EventArgs e)
        {
            if (cxbxAddNotes.Checked)
            {
                txbxNotes.Enabled = true;
            }

            else
            {
                txbxNotes.Enabled = false;
            }
        }

        private void txbxAddNewDefect_TextChanged(object sender, EventArgs e)
        {
            if (txbxAddNewDefect.Text.Length != 0)
            {
                cmbxLabels.Enabled = false;
            }

            else
            {
                cmbxLabels.Enabled = true;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
