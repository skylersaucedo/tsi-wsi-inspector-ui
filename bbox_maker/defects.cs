using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bbox_maker
{
    public class Defects
    {
        public string label;
        public string imagePath;
        public Rectangle rect;
        public int height;
        public int width;
        public bool isNeedsBox => rect.IsEmpty;
    }
}
