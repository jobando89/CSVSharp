using System;
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

       
        private CSVFile _file;
        private IList<IList<IList<byte>>> _set;
        private bool _scaped;

        public CSVFile ReadLines(string lineInput)
        {
            _file = new CSVFile();
            _set = new List<IList<IList<byte>>>();
            var characters = GetBytes(lineInput);
            _scaped = false;
            using (new MemoryStream(characters))
            {
                var special = false;
                var line = NewLine();
                var column = NewColumn();
                for (int i = 0; i < characters.Length; i = i + 2)
                {
                    byte theByte = characters[i];                    
                    switch (theByte)
                    {                                                
                        case 44:// ,
                            if (_scaped)
                            {                                
                                break;
                            }
                            line.Add(column);
                            column = NewColumn();
                            special = true;
                            break;                                                
                        case 10:// \n
                            if (_scaped)
                            {
                                special = true;
                                break;
                            }

                            line.Add(column);                            
                            _set.Add(line);
                            
                            column = NewColumn();
                            line = NewLine();
                            
                            break;
                        case 34:// "
                            special = true;
                            _scaped = !_scaped;
                            break;
                    }

                    if (special)
                    {
                        special = false;
                        continue;
                    }

                    column.Add(characters[i]);
                    column.Add(characters[i+1]);
                   
                    if (i == characters.Length - 2)
                    {
                        line.Add(column);
                        _set.Add(line);

                        break;
                    }
                }                
            }

            var max = _set.Max(e => e.Count);   
            _file.SetupSetFile(_set);            
            return _file;
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

        public string Cell(int line, int column)
        {
            var ln = _set[line];
            var cell = ln[column];
            char[] chars = new char[cell.Count / sizeof(char)];
            System.Buffer.BlockCopy(cell.ToArray(), 0, chars, 0, cell.Count);
            return new string(chars).Trim();
        }

    }

  
    
}
