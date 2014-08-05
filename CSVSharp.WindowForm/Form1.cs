using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            var lineInput  = "";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            //var result = test.Cell(test.Lines - 1, test.Columns - 1);

            DataTable table = new DataTable();
            for (var i=0; i<test.Columns;i++)
            {
                table.Columns.Add(new DataColumn(test.Header(i)));
            }

            for (var line = 1; line < test.Lines; line++)
            {
                var row = table.NewRow();
                for (var col = 0; col < test.Columns; col++)
                {                    
                    row[col] = test.Cell(line, col);                 
                }
                table.Rows.Add(row);
            }
            
            
            return table;
        }
    }
}
