using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inspectorUI
{
    public class uiViews
    {
        // generic placeholding class

        //public View() { }


        // leftimagesPath = projectFolderPath + "\\" + "RAW" + "\\" + pinOrBox + "\\" + passNum + "\\" + @"cam1\stills";
        // rightimagesPath = projectFolderPath + "\\" + "RAW" + "\\" + pinOrBox + "\\" + passNum + "\\" + @"cam2\stills";
        // centerimagesPath = projectFolderPath + "\\" + "RAW" + "\\" + pinOrBox + "\\" + passNum + "\\" + @"cam0\stills";

        // pictureBox1, "center"
        // pictureBox2, "left"
        // pictureBox3, "right"

        // when class is instantiated, script should:
        // 1. generate list of image paths from inputpath
        // 2. count for total images
        // 3. original image for quick display
        // 4. h, w of img
        // 5. make ismousedown false

        public int totalImages { get; set; }

        public string imagePath { get; set; }
        public string allImaggesPath { get; set; }

        public List<String> images { get; set; }

        public Image originalImage { get; set; }

        public int h { get; set; }
        public int w { get; set; }

        //public int s { get; set; }

        public int hNew { get; set; }
        public int wNew { get; set; }

        public int imageIdx { get; set; }

        public bool isMouseDown { get; set; }

        public List<string> PopulateImageList(string inputFolder)
        {
            string ext = "*.bmp";

            List<string> outList = new List<string>();
            foreach (string imageFileName in Directory.GetFiles(inputFolder, searchPattern: ext)) //bmp files are huge!
            {
                string name = Path.GetFileNameWithoutExtension(imageFileName);
                outList.Add(imageFileName);
            }

            images = outList;
            return outList;
        }

        public Image MakeFirstImage(string imagePath)
        {
            originalImage = new Bitmap(imagePath);
            h = originalImage.Height;
            w = originalImage.Width;

            return originalImage;
        }

        public Image ZoomPicture(Image img, Size s)
        {
            Bitmap bm = new Bitmap(img, s.Width, s.Height);
            Graphics gpu = Graphics.FromImage(bm);
            gpu.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bm;

        }
        // not sure this is needed since List constructor has a .count method

        public int CountImgs(List<String> list)
        {
            return list.Count;
        }

    }

    
}
