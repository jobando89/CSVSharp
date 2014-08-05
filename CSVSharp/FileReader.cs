using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CSVSharp
{
    public class FileReader
    {

        public CSVFile ReadLines(string lineInput)
        {
            var characters = Encoding.ASCII.GetBytes(lineInput);
            return ReadLines(characters);
        }

        private IList<IList<IList<byte>>> _set;

        public CSVFile ReadLines(byte[] readerBytes)
        {
            _set = new List<IList<IList<byte>>>();            
            using (new MemoryStream(readerBytes))
            {                  
                GetValue(readerBytes);
            }
            var file = new CSVFile(_set);            
            return file; 
        }

        private void GetValue(byte[] readerBytes)
        {
            var scaped = false;
            var line = NewLine();
            var column = NewColumn();
            var special = false;
            for (var i = 0; i < readerBytes.Length; i = i + 1)
            {
                var theByte = readerBytes[i];
                switch (theByte)
                {
                    case 0:
                        special = true;
                        break;
                    case 44: // ,
                        if (scaped)
                        {
                            break;
                        }
                        line.Add(column);
                        column = NewColumn();
                        special = true;
                        break;
                    case 13:
                        special = true;
                        break;
                    case 10: // \n
                        if (EvaluateChracter(ref special)) break;
                        SetColumnAndLine(line, column);
                        column = NewColumn();
                        line = NewLine();
                        break;
                    case 34: // "
                        special = true;
                        scaped = !scaped;
                        break;
                }
                if (EvaluateChracter(ref special)) continue;
                column.Add(readerBytes[i]);
                if (i != readerBytes.Length - 1) continue;
                SetColumnAndLine(line, column);
            }
        }

        private static bool EvaluateChracter(ref bool special)
        {
            if (special)
            {
                special = false;
                return true;
            }
            return false;
        }

        private void SetColumnAndLine(IList<IList<byte>> line, List<byte> column)
        {
            line.Add(column);
            _set.Add(line);
        }

        IList<IList<byte>> NewLine()
        {
            return new List<IList<byte>>();
        }

        List<byte> NewColumn()
        {
            return new List<byte>();
        }

        byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

     
    }
}
