using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFiles
{
    public partial class Form1 : Form
    {
        static int RECENT_QUEUE_SIZE = 20;
        string tree2SelectedNodeStr = "";
        TreeNode tree2ActiveNode = null;
        string tree1SelectedNodeStr = "";
        FixedSizeQueue<string> recentList = new FixedSizeQueue<string>(RECENT_QUEUE_SIZE);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string lastFolder = Properties.Settings.Default.LastFolder;
            string[] nodes = DirSearch(lastFolder);
            RootTreeUpdate(treeView1, lastFolder, nodes);

        }
        void RootTreeUpdate(TreeView tree, string root, string[] nodes)
        {
            tree.Nodes.Clear();
            tree.Nodes.Add(root);
            foreach (string n in nodes)
            {
                tree.Nodes[0].Nodes.Add(n);
            }
            tree.ExpandAll();
        }
        string[] DirSearch(string sDir)
        {
            List<string> nodes = new List<string>();
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {

                    bool isHidden = (File.GetAttributes(d) & FileAttributes.Hidden) == FileAttributes.Hidden;
                    if (!isHidden)
                    {
                        //Console.WriteLine("Folder: " + d);
                        //treeView1.Nodes[0].Nodes.Add(d);
                        nodes.Add(d);
                    }
                }
                foreach (string d in Directory.GetFiles(sDir))
                {

                    bool isHidden = (File.GetAttributes(d) & FileAttributes.Hidden) == FileAttributes.Hidden;
                    if (!isHidden)
                    {
                        //Console.WriteLine("File: " + d);
                        //treeView1.Nodes[0].Nodes.Add(d);
                        nodes.Add(d);
                    }
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
            return nodes.ToArray();
        }



        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            string nodeStr = e.Node.Text;
            string[] nodes = DirSearch(nodeStr);
            RootTreeUpdate(treeView2, nodeStr, nodes);

            recentList.Enqueue(nodeStr);
            updateRecentView(recentList);
            tree1SelectedNodeStr = e.Node.Text;
        }

        private void updateRecentView(FixedSizeQueue<string> recentList)
        {
            string[] rl = recentList.toArray();
            flowLayoutPanel1.Controls.Clear();
            foreach (string i in rl)
            {
                RecentLabel label = new RecentLabel();
                label.Text = i;
                label.DoubleClick += new EventHandler(recentLabelClickHandler);
                flowLayoutPanel1.Controls.Add(label);
            }
        }

        private void recentLabelClickHandler(object sender, EventArgs e)
        {
            RecentLabel l = (RecentLabel)sender;
            //MessageBox.Show(l.Text);
            RootTreeUpdate(treeView1, l.Text, DirSearch(l.Text));
            treeView2.Nodes.Clear();
        }


        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tree2SelectedNodeStr = e.Node.Text;

        }

        private void treeView2_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(tree2SelectedNodeStr);
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            RootTreeUpdate(treeView1, tree1SelectedNodeStr, DirSearch(tree1SelectedNodeStr));
            treeView2.Nodes.Clear();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DirectoryInfo pd = Directory.GetParent(e.Node.Text);
                if (pd != null)
                {
                    RootTreeUpdate(treeView1, pd.ToString(), DirSearch(pd.ToString()));
                    treeView2.Nodes.Clear();
                }
            }
        }

        private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && tree2ActiveNode != null)
            {
                treeView2.SelectedNode = tree2ActiveNode;
                contextMenuStrip1.Show(treeView2, e.Location);
            }
        }

        private void treeView2_MouseDown(object sender, MouseEventArgs e)
        {
            tree2ActiveNode = treeView2.SelectedNode;
            treeView2.SelectedNode = tree2ActiveNode;
        }
        private void copyFile(string filePath)
        {
            // Copy some files to the clipboard.
            List<string> file_list = new List<string>();
            file_list.Add(filePath);
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.FileDrop, file_list.ToArray());
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyFile(tree2ActiveNode.Text);
        }
        private void SendToPrinter(string filePath)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = filePath;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendToPrinter(tree2ActiveNode.Text);
        }
    }
}
