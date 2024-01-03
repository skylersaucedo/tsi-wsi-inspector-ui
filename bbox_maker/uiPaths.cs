using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inspectorUI
{

    public class uiPaths
    {
        // use this to organize all paths

        // ----- LOCAL PATHS BELOW --------


        //public string backupImageFolder = @"C:\Users\endle\Desktop\oct-21-drillpipe-vids";
        //public string newProjectPath = @"C:\wsi-project-scans";

        //public string projectPath = @"C:\Users\endle\Desktop\oct-21-drillpipe-vids";


        ////public static readonly string pythonEnvDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"miniconda3\envs\pytorch-gpu");
        //public static readonly string pythonEnvPath = @"C:\Users\endle\miniconda3\envs\pytorch-gpu\python.exe";

        //public string makeStillsPyPath = @"C:\Users\endle\source\repos\tsi-wsi-inspector-ui\bbox_maker\make_stills.py";

        //public static readonly string labelpath = @"C:\Users\endle\source\repos\tsi-wsi-inspector-ui\bbox_maker\labels.txt";
        //public static readonly string defectslog_path = @"C:\Users\endle\source\repos\tsi-wsi-inspector-ui\bbox_maker\defectslog.txt";


        // ----- TSI HMI PATHS BELOW --------

        //public string projectPath = @"\\TSI-AIO-DT01\Users\prime\Documents\scans";
        public string projectPath = @"C:\Users\TSI\Desktop\boxscans-jan3";

        public string backupImageFolder = @"C:\wsi-project-scans\20231016-100646";

        public string makeStillsPyPath = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\make_stills.py";

        public string newProjectPath = @"C:\wsi-project-scans";

        public static readonly string pythonEnvDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"miniconda3\envs\pytorch-gpu");
        public static readonly string pythonEnvPath = Path.Combine(pythonEnvDir, @"python.exe");

        public static readonly string mlDIR = @"C:\Users\TSI\source\repos\threadinspection_Aug14\UI.MachineLearning";
        public static string predictionScriptPath = Path.Combine(mlDIR, "detection_server.py");

        public static readonly string inspectionGenPyPath = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\make_inspection_report.py";

        public static readonly string labelpath = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\labels.txt";
        public static readonly string defectslog_path = @"C:\Users\TSI\source\repos\tsi-wsi-inspector-ui\bbox_maker\defectslog.txt";
    }
}
