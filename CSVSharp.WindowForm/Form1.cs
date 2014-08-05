using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CSVSharp.WindowForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.dataGridView1.DataSource = GetResults();
        }

        private DataTable GetResults()
        {
            var bytes = File.ReadAllBytes("file.csv");
            var reader = new FileReader();
            var test = reader.ReadLines(bytes);
            return test.GetDataTable(false);
        }
    }
}
