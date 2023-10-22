using inspectionUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace inspectorUI
{

    public partial class frmProjectFolder : Form
    {
        public string projectPath;
        public Process _pythonProcess;
        public string inputImageFolder;


        public uiPaths appPaths = new uiPaths();

        public frmProjectFolder()
        {
            InitializeComponent();
        }

        public void frmProjectFolder_Load(object sender, EventArgs e)
        {
            projectPath = appPaths.projectPath;

            if (Directory.Exists(projectPath))
            {
                // populate combobox with project folders
                string[] folders = Directory.GetDirectories(projectPath);
                comboBox1.Items.AddRange(folders);
            }
            else
            {
                MessageBox.Show("unable to find video project path. Reverting to local project.");
                frmMain frmMain = new frmMain(appPaths.backupImageFolder);
                frmMain.Show();

                this.Hide(); // hide, but don't close instance.

            }

            
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

                    // images are made, need to populate new project folder

                    string[] inputImageFolderNames = inputImageFolder.Split('\\');

                    string lastFolderName = inputImageFolderNames.Last();

                    string newInputFolderName = appPaths.newProjectPath + "\\" + lastFolderName;

                    frmMain frmMain = new frmMain(newInputFolderName);
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
            try
            {
                var pyStartInfo = new ProcessStartInfo(); // run the python script
                pyStartInfo.FileName = uiPaths.pythonEnvPath;
                pyStartInfo.UseShellExecute = false;
                pyStartInfo.CreateNoWindow = false; //true before, we are debugging right now...
                pyStartInfo.RedirectStandardOutput = false; //true before
                pyStartInfo.RedirectStandardError = false; //true before
                pyStartInfo.Arguments = $"\"{appPaths.makeStillsPyPath}\" \"{stillsFolder}\"";

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
