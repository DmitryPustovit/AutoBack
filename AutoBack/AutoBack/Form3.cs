﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBack
{
    public partial class Form3 : Form
    {

        public string timeTill;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 fc = (Form2)Application.OpenForms["form2"];
            if (fc != null)
            {
                    fc.timeTillBackup = Convert.ToInt32(textBox1.Text);

                fc.label3.Text = "Time Till Next Back Up : " + fc.timeTillBackup.ToString() + " Minutes";
            }

        }

    }
}
