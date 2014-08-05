using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CSVSharp
{
    public class CSVFile
    {
        public CSVFile(IList<IList<IList<byte>>> set)
        {
            SetupSetFile(set);           
        }
        
        private IList<IList<IList<byte>>> _set;

        void SetupSetFile(IList<IList<IList<byte>>> set)
        {
            _set = set;
            var max = _set.Max(e => e.Count);
            Columns = max;
            Lines = _set.Count;
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
            return Cell(0, column);
        }

        public string Cell(int line, int column)
        {
            if (line >= Lines|| column >= Columns)
            {
                throw new Exception("");
            }
            var ln = _set[line];
            var cell = ln.ElementAtOrDefault(column);
            if (cell ==null)
            {
                return "";
            }

            return Encoding.ASCII.GetString(cell.ToArray()).Trim();

           // char[] chars = new char[cell.Count / sizeof(char)];
           // System.Buffer.BlockCopy(cell.ToArray(), 0, chars, 0, cell.Count);
           // return new string(chars).Trim();
        }

        public DataTable GetDataTable(bool withHeaders)
        {
            int startLine = 0;
            var table = new DataTable();
            if (withHeaders)
            {
                for (var i = 0; i < Columns; i++)
                {
                    table.Columns.Add(new DataColumn(Header(i)));
                }
                startLine++;
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