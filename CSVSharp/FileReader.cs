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
        private readonly ReaderLineManager _lineManager;
        private readonly ReaderColumnManager _columnManager;
        private IList<string[]> _set;
        private bool _characterSkipper;


        public FileReader()
        {
            _lineManager = new ReaderLineManager();
            _columnManager = new ReaderColumnManager();
        }

        public CSVFile ReadLines(string lineInput)
        {
            return ReadLines(lineInput, false);
        }

        public CSVFile ReadLines(byte[] readerBytes)
        {
            return ReadLines(readerBytes, false);
        }

        public CSVFile ReadLines(string lineInput, bool hasHeaders)
        {
            var characters = Encoding.ASCII.GetBytes(lineInput);
            return ReadLines(characters, hasHeaders);
        }                

        public CSVFile ReadLines(byte[] readerBytes, bool hasHeaders)
        {
            _set = new List<string[]>();            
            using (new MemoryStream(readerBytes))
            {                  
                GetValue(readerBytes);
            }
            var file = new CSVFile(_set, hasHeaders);            
            return file; 
        }
        
        private void GetValue(IList<byte> readerBytes)
        {
            
            _lineManager.SetupNewLine();
            _columnManager.SetupNewLine();
            _characterSkipper = false;

            var quoteSwitch = false;
            for (var i = 0; i < readerBytes.Count; i = i + 1)
            {
                var theByte = readerBytes[i];
                switch (theByte)
                {
                    case 0:
                        EmptyCharacter();
                        break;
                    case 44: // ,
                        if (quoteSwitch) break;                        
                        CommaProcessor();
                        break;
                    case 13:
                        _characterSkipper = true;
                        break;
                    case 10: // \n
                        if (IsItAComma()) break;
                        SetColumnAndLine();
                        _columnManager.SetupNewLine();
                        _lineManager.SetupNewLine();
                        break;
                    case 34: // "
                        _characterSkipper = true;
                        quoteSwitch = !quoteSwitch;
                        break;
                }
                if (IsItAComma()) continue;
                _columnManager.AddByteToColumn(readerBytes[i]);
                if (i != readerBytes.Count - 1) continue;
                SetColumnAndLine( );
            }
        }

        private void CommaProcessor()
        {
            _lineManager.AddColumnsToLine(_columnManager);
            _columnManager.SetupNewLine();
            _characterSkipper = true;
        }

        private void EmptyCharacter()
        {
            _characterSkipper = true;
        }


        bool IsItAComma()
        {
            if (!_characterSkipper) return false;
            _characterSkipper = false;
            return true;
        }

        private void SetColumnAndLine()
        {
            _lineManager.AddColumnsToLine(_columnManager);
            _set.Add(_lineManager.LineColumnArray);
        }


     
    }

    class ReaderLineManager
    {
        IList<string> ListLine { get; set; }


        public ReaderLineManager()
        {
            SetupNewLine();
        }

        public void SetupNewLine()
        {
            ListLine = new List<string>();
        }

        public void AddColumnsToLine(ReaderColumnManager column)
        {
            var columnData = Encoding.ASCII.GetString(column.ByteColumnArray).Trim();
            ListLine.Add(columnData);
        }

        public string[] LineColumnArray
        {
            get
            {
                return ListLine.ToArray();
            }
        }

    }

    class ReaderColumnManager
    {
        IList<byte> ListColumn { get; set; }

        public ReaderColumnManager()
        {
            SetupNewLine();
        }

        public void SetupNewLine()
        {
            ListColumn = new List<byte>();
        }

        public void AddByteToColumn(byte character)
        {
            ListColumn.Add(character);
        }

        public byte[] ByteColumnArray
        {
            get
            {
                return ListColumn.ToArray();
            }
        }

    }
}
