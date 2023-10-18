using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace inspectionUI
{
    public class Defect
    {
        public string label;
        public string imagePath;
        public Rectangle rect;
        public int height;
        public int width;
        public bool isNeedsBox => rect.IsEmpty;

        // added new properties 9/22

        public int index;
        public string datetime;
        public string image_name;
        public int h;
        public int w;
        public string inspector;
        public string defect;
        public int location_x;
        public int location_y;
        public int def_h;
        public int def_w;
        public string notes;
        public Rectangle r;
        public int image_index;
        public string pipe_id;
        public int pass;
        public int cam;
        public string pipe_side;
        public string project;

        //int index = i;
        //string datetime = x[1];
        //string inspector = x[2];
        //string pipe_ID = x[3];
        //string defect = x[4];
        //string notes = x[5];
        //string project = x[6];
        //string pipe_side = x[7];
        //int pass = Convert.ToInt32(x[8]);
        //int cam = Convert.ToInt32(x[9]);
        //int img_idx = Convert.ToInt32(x[10]);
        //string img_name = x[11];
        //int h = Convert.ToInt32(x[12]);
        //int w = Convert.ToInt32(x[13]);
        //int loc_x = Convert.ToInt32(x[14]);
        //int loc_y = Convert.ToInt32(x[15]);
        //int def_h = Convert.ToInt32(x[16]);
        //int def_w = Convert.ToInt32(x[17]);
    }
}
