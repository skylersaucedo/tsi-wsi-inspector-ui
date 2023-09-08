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

namespace bbox_maker
{
    public partial class frmAddDefect : Form
    {
        public string DefectListpath = @"C:\Users\sauce\OneDrive\Desktop\bbox_maker-master\bbox_maker\labels.txt";

        public List<string> currentDefects = new List<string>();
        public frmAddDefect()
        {
            InitializeComponent();

            // populate box with current defects
            LoadLabels(DefectListpath);
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
                // just use selected defect
                //MessageBox.Show("nothing here");

                int idx = cmbxLabels.SelectedIndex;

                MessageBox.Show("You have selected: " + currentDefects[idx].ToString());
            }

            else
            {
                // Add yes or no, are you sure you want to add this defect?
                MessageBox.Show("you are adding: " + txbxAddNewDefect.Text);

                DialogResult result = MessageBox.Show("Adding a new defect?", "Add new defect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // User clicked Yes

                    currentDefects.Add(txbxAddNewDefect.Text);
                    cmbxLabels.Items.Add(txbxAddNewDefect.Text);

                }
                else if (result == DialogResult.No)
                {
                    // User clicked No
                }
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
    }
}
