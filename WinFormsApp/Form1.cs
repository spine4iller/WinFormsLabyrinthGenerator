using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Properties;

namespace WinFormsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            generateToolStripMenuItem_Click(null, null);

        }

        private List<Point> GeneratePath(int xMax, int yMax, Point start, Point end)
        {
            var rnd = new Random();
            var p = new List<Point>();
            for (int x = 0; x < xMax; x += 2)
            {
                p.Add(new Point(x, rnd.Next(yMax)));
            }
            if (xMax % 2 == 0)
                p.Add(new Point(xMax - 1, rnd.Next(yMax)));
            var pa = new List<Point>();
            pa.Add(start);
            pa.AddRange(PathYBetween(start, p[0]));

            for (int pi = 1; pi < p.Count; pi++)
            {
                pa.AddRange(PathBetween(p[pi - 1], p[pi]));
            }
            pa.AddRange(PathYBetween(p.Last(), end));
            p.AddRange(pa);
            //DrawPath(p);
            return p;
        }

        private void DrawPath(List<Point> pa)
        {
            foreach (var p in pa)
            {
                var tree = DrawTree(p.X, p.Y);
                Controls.Add(tree);
            }
        }

        private List<Point> PathBetween(Point p1, Point p2)
        {

            if (p1.X - p2.X == 0)
            {
                return PathYBetween(p1, p2);
            }
            if (p1.Y - p2.Y == 0)
            {
                return PathXBetween(p1, p2);
            }
            var midPoint1 = new Point(p1.X, p2.Y);
            var bx1 = PathXBetween(p2, midPoint1);
            bx1.AddRange(PathYBetween(midPoint1, p1));
            return bx1;

        }

        private List<Point> PathXBetween(Point p1, Point p2)
        {
            int from, to;
            var l = new List<Point>();
            if (p1.X <= p2.X)
            {
                from = p1.X;
                to = p2.X;
            }
            else
            {
                from = p2.X;
                to = p1.X;
            }

            for (int x = from; x < to + 1; x++)
            {
                l.Add(new Point(x, p1.Y));
            }
            return l;
        }

        private List<Point> PathYBetween(Point p1, Point p2)
        {
            int from, to;
            var l = new List<Point>();
            if (p1.Y <= p2.Y)
            {
                from = p1.Y;
                to = p2.Y;
            }
            else
            {
                from = p2.Y;
                to = p1.Y;
            }

            for (int y = from; y < to + 1; y++)
            {
                l.Add(new Point(p1.X, y));
            }
            return l;
        }

        public int TreeWidth => 30;

        public int TreeHeight => TreeWidth;

        public int YMax => (panel.Height) / TreeHeight;

        public int XMax => panel.Width / TreeWidth;

        //public int MenuWidth => 20;

        private PictureBox DrawTree(int x, int y)
        {
            var aTree = new PictureBox();
            aTree.Image = Resources.evergreen_tree;
            aTree.Location = new Point(x * TreeWidth, y * TreeWidth);
            aTree.Size = new Size(TreeWidth, TreeWidth);
            aTree.SizeMode = PictureBoxSizeMode.StretchImage;
            return aTree;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics; //this.CreateGraphics();
            g.Clear(Color.Beige);
            // create  a  pen object with which to draw

            Pen p = new Pen(Color.Red, 3);  // draw the line 

            // call a member of the graphics class

            g.DrawEllipse(p, 0, 0, TreeWidth - 10, TreeHeight - 10);
            g.DrawEllipse(p, (XMax - 1) * TreeWidth, (YMax - 1) * TreeHeight, TreeWidth - 10, TreeHeight - 10);
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //   DrawPath(PathBetween(new Point(7, 7), new Point(8, 6)));
            // return;
            panel.Controls.Clear();
            var path = GeneratePath(XMax, YMax, new Point(0, 0), new Point(XMax, YMax));
            // return;
            for (int y = 0; y < YMax; y++)
            {
                for (int x = 0; x < XMax; x++)
                {
                    if (path.Contains(new Point(x, y)))
                        continue;
                    var tree = DrawTree(x, y);
                    panel.Controls.Add(tree);
                }
            }
            //  panel.Refresh();
        }

    }
}
