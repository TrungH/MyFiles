using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFiles
{
    class RecentLabel:Label
    {
        public RecentLabel()
        {
            this.AutoSize = true;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Padding = new Padding(2, 4, 2, 4);
            this.Margin = new Padding(2, 2, 2, 2);
            this.MouseDown += new MouseEventHandler(onMouseDown);
            this.MouseUp += new MouseEventHandler(onMouseUp);
        }
        Color defaultColor;
        private void onMouseDown(object sender, EventArgs e)
        {
            defaultColor = this.BackColor;
            this.BackColor = Color.LightBlue;
        }

        private void onMouseUp(object sender, EventArgs e)
        {
            this.BackColor = defaultColor;
        }
    }
}
