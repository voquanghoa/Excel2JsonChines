﻿using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelExportChines
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fileName = @"\\Mac\Home\Downloads\Update_grammar (1)";
            var log = QuestionController.Process(fileName);
            textBox1.Text = log;
            MessageBox.Show("Done");
        }
    }
}
