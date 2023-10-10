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
    }
}
