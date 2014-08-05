using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSVSharp
{
    public class CSVFile
    {
        
        private IList<IList<IList<byte>>> _set;

        public void SetupSetFile(IList<IList<IList<byte>>> set)
        {
            _set = set;
            var max = _set.Max(e => e.Count);
            Columns = max;
            Lines = _set.Count;
        }

        public int Lines
        {
            get; set;
        }

        public int Columns
        {
            get;
            set;
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

            return ASCIIEncoding.ASCII.GetString(cell.ToArray()).Trim();

           // char[] chars = new char[cell.Count / sizeof(char)];
           // System.Buffer.BlockCopy(cell.ToArray(), 0, chars, 0, cell.Count);
           // return new string(chars).Trim();
        }

        
    }
}