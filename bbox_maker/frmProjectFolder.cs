using inspectionUI;
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
    public partial class frmProjectFolder : Form
    {
        public string projectPath = @"\\TSI-AIO-DT01\Users\prime\Documents\scans";

        public frmProjectFolder()
        {
            InitializeComponent();
        }

        public void frmProjectFolder_Load(object sender, EventArgs e)
        {
            // populate combobox with project folders
            string[] folders = Directory.GetDirectories(projectPath);
            comboBox1.Items.AddRange(folders);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("You selected: " + comboBox1.SelectedItem.ToString());

            string inputImageFolder = comboBox1.SelectedItem.ToString();

            // check to see if .bmps are present

            // check to see if file structure is present

            // if not, ask user. 

            // now generate updated project folder with labels and images 

            // rename_image_stills with datetime, pipeID, pass. 

            frmMain frmMain = new frmMain(inputImageFolder);
            frmMain.Show();

        }
    }
}
