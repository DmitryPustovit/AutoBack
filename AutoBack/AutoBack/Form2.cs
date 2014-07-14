using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBack
{
    public partial class Form2 : Form
    {


        static string source = @"C:\Users\Dmitry\Desktop\OrginFi";
        static string destination = @"C:\Users\Dmitry\Desktop\Dest";


        Timer Clock = new Timer();

        Timer BackUpCheck = new Timer();

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String sClassName, String sAppName);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hwnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hwnd, int id);

        public int timeTillBackup = 10;

        IntPtr thisWindow = FindWindow(null, "Form2.cs");

        private const int WM_HOTKEY = 0x312;
        
        public Form2()
        {
            InitializeComponent();
            label3.Text = "Time Till Next Back Up : " + timeTillBackup.ToString() + " Minutes";
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            RegisterHotKey(this.Handle, 1, (uint)fsModifers.Control, (uint)Keys.A);

            int timetill;

            timetill = timeTillBackup * 100 * 60; // Not actaully sure if this time interval is accurate.
            Clock.Interval = timetill; 
            Clock.Start();
            Clock.Tick += new EventHandler(timerCopy);
        }

        public enum fsModifers
        {

            Alt = 0x0001,
            Control = 0x0002,
            Shift = 0x0004,
            Window = 0x0008,
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 1);
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);

            ReadyCopy();
        }

        //Automatic timer set back

        public void timerCopy(object sender, EventArgs e)
        {
            ReadyCopy();
        }

        // Prepares the folder that backup will be copied into

        private void ReadyCopy()
        {
            source = textBox1.Text;
            destination = textBox2.Text;

            string time = DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss_tt");
            string filecreate = @destination + @"\" + @time;

            Directory.CreateDirectory(filecreate);

            System.Threading.Thread.Sleep(5000);

            DirectoryCopy(@source, filecreate, true);
        }

        private static void DirectoryCopy(
            string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                //throw new DirectoryNotFoundException(
                //    "Source directory does not exist or could not be found: "
                //    + sourceDirName);
                MessageBox.Show("The source folder does not exist. Make sure it is spelled correctly and that it is a folder, not an individual file.");
                ERROR();
            }
            else
            {

                DirectoryInfo[] dirs = dir.GetDirectories();

                // If the destination directory does not exist, create it.
                //if (!Directory.Exists(destDirName))
                //{
                //   Directory.CreateDirectory(destDirName);
                //}


                // Get the file contents of the directory to copy.
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    // Create the path to the new copy of the file.
                    string temppath = Path.Combine(destDirName, file.Name);

                    // Copy the file.
                    file.CopyTo(temppath, false);
                }

                // If copySubDirs is true, copy the subdirectories.
                if (copySubDirs)
                {

                    foreach (DirectoryInfo subdir in dirs)
                    {
                        // Create the subdirectory.
                        string temppath = Path.Combine(destDirName, subdir.Name);

                        // Copy the subdirectories.
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                System.Threading.Thread.Sleep(3000);
                ReadyCopy();
            }

            base.WndProc(ref m);
        }

        public static void ERROR()
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3();
            f.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/DmitryPustovit/AKTSA-Open-Source-Software-Development-2014");
        }

    }
}
// 