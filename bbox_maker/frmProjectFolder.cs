using inspectionUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bbox_maker
{
    public partial class frmProjectFolder : Form
    {
        public string projectPath = @"\\TSI-AIO-DT01\Users\prime\Documents\scans";
        public Process _pythonProcess;
        public string inputImageFolder;

        public static readonly string pythonEnvDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"miniconda3\envs\pytorch-gpu");
        public static readonly string pythonEnvPath = Path.Combine(pythonEnvDir, @"python.exe");

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

            inputImageFolder = comboBox1.SelectedItem.ToString();

            // check to see if .bmps are present

            string[] bmpFiles = Directory.GetFiles(inputImageFolder, "*.bmp");

            if (bmpFiles.Length > 0)
            {
                //Console.WriteLine(format: "There are {0} .bmp files in the directory.", bmpFiles.Length);
                MessageBox.Show(String.Format("There are {0} .bmp files in the directory.", bmpFiles.Length));
            }
            else
            {
                //Console.WriteLine("There are no .bmp files in the directory.");
                MessageBox.Show("There are no .bmp files in the directory.");


                DialogResult result = MessageBox.Show("Make Stills?", "Make stills?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("making stills...");

                    makeStillsfromProjectFolder(inputImageFolder);

                    // now show images from new folder.

                    frmMain frmMain = new frmMain(inputImageFolder);
                    frmMain.Show();

                    this.Hide(); // hide, but don't close instance.

                }
                else if (result == DialogResult.No)
                {
                    MessageBox.Show("NOT making stills. Please select a project to analyze");
                }

            }


        }

        public void makeStillsfromProjectFolder(string stillsFolder)
        {
 
            string makeStillsScript = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\make_stills.py";

            try
            {
                var pyStartInfo = new ProcessStartInfo(); // run the python script
                pyStartInfo.FileName = pythonEnvPath;
                pyStartInfo.UseShellExecute = false;
                pyStartInfo.CreateNoWindow = false; //true before, we are debugging right now...
                pyStartInfo.RedirectStandardOutput = false; //true before
                pyStartInfo.RedirectStandardError = false; //true before
                pyStartInfo.Arguments = $"\"{makeStillsScript}\" \"{stillsFolder}\"";

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
    }
}
