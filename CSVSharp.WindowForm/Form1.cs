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
            openFileDialog1.Filter = "CSV Files |*.csv";
            // this.dataGridView1.DataSource = GetResults();

        }

        private void GetResults(string file)
        {
            var bytes = File.ReadAllBytes(file);
            //var bytes = File.ReadAllBytes("file.csv");
            var reader = new FileReader();
            var test = reader.ReadLines(bytes,true);

            toolStripStatusLabel1.Text = string.Format("Total Lines: {0}", test.Lines);
            dataGridView1.DataSource = test.GetDataTable();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var file = openFileDialog1.FileName;
                GetResults(file);
            }             
        }
    }
}
