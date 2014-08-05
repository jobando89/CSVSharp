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
            var characters = GetBytes(lineInput);
            return ReadLines(characters);
        }

        public CSVFile ReadLines(byte[] readerBytes)
        {
            IList<IList<IList<byte>>> _set;
            bool _scaped;
            _set = new List<IList<IList<byte>>>();
            _scaped = false;
            using (new MemoryStream(readerBytes))
            {
                var special = false;
                var line = NewLine();
                var column = NewColumn();
                for (int i = 0; i < readerBytes.Length; i = i+1 )
                {
                    byte theByte = readerBytes[i];
                    switch (theByte)
                    {
                        case 0:
                            special = true;
                            break;
                        case 44:// ,
                            if (_scaped)
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

                    column.Add(readerBytes[i]);
                 //   column.Add(readerBytes[i + 1]);

                    if (i == readerBytes.Length -1)
                    {
                        if (theByte ==10)
                        {
                            break;
                        }
                        line.Add(column);
                        _set.Add(line);

                        break;
                    }
                }
            }
            var _file = new CSVFile();
            _file.SetupSetFile(_set);
            return _file; 
        }

        public IList<IList<IList<byte>>> ReadBytes(byte[] readerBytes)
        {
            bool _scaped;
            IList<IList<IList<byte>>> _set = new List<IList<IList<byte>>>();
            _scaped = false;
            using (new MemoryStream(readerBytes))
            {
                var special = false;
                var line = NewLine();
                var column = NewColumn();
                for (int i = 0; i < readerBytes.Length; i = i + 1)
                {
                    byte theByte = readerBytes[i];
                    switch (theByte)
                    {
                        case 0:
                            special = true;
                            break;
                        case 44:// ,
                            if (_scaped)
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

                    column.Add(readerBytes[i]);
                    //   column.Add(readerBytes[i + 1]);

                    if (i == readerBytes.Length - 2)
                    {
                        if (theByte == 10)
                        {
                            break;
                        }
                        line.Add(column);
                        _set.Add(line);

                        break;
                    }
                }
            }
            return _set;               
        }

        public CSVFile GetCSVFile(IList<IList<IList<byte>>> _set)
        {
            var _file = new CSVFile();
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
}
