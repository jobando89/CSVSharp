using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace CSVSharp
{
    public class CSVFile
    {
        private bool _hasHeaders;
        private IList<string[]> _set;

        public CSVFile(IList<string[]> set)
        {
            SetupSetFile(set, false);
        }     

        public CSVFile(IList<string[]> set, bool hasHeaders)
        {
            SetupSetFile(set, hasHeaders);           
        }                

        void SetupSetFile(IList<string[]> set, bool hasHeaders)
        {
            _hasHeaders = hasHeaders;
            _set = set;
            var max = _set.Max(e => e.Length);
            Columns = max;
            Lines = _set.Count;
            if (_hasHeaders)
            {
                Lines--;
            }
        }

        public int Lines
        {
            get; private set;
        }

        public int Columns
        {
            get; private set;
        }

        public string Header(int column)
        {
            if (_hasHeaders)
            {
                return Cell(-1, column);    
            }
            throw new Exception("File does not have headers");
        }

        public string Cell(int line, int column)
        {
          
            if (line >= Lines|| column >= Columns)
            {
                throw new Exception("");
            }
            if (_hasHeaders)
            {
                line++;
            }   
            var ln = _set[line];
            var cell = "";
            if (column < ln.Length)
            {
                cell = ln[column].Trim();
            }
            return cell;
        }

        public DataTable GetDataTable()
        {
            var startLine = 0;
            var table = new DataTable();
            if (_hasHeaders)
            {
                for (var i = 0; i < Columns; i++)
                {
                    table.Columns.Add(new DataColumn(Header(i)));
                }                
            }
            else
            {
                for (var i = 0; i < Columns; i++)
                {
                    table.Columns.Add(new DataColumn(""));
                }
            }
            for (var line = startLine; line < Lines; line++)
            {
                var row = table.NewRow();
                for (var col = 0; col < Columns; col++)
                {
                    row[col] = Cell(line, col);
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}