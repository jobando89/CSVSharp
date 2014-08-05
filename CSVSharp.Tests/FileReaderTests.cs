using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSVSharp.Tests
{
    [TestClass]
    public class FileReaderTests
    {
        [TestMethod]
        public void ReadFileLinesTest()
        {
            var lineInput = "Line1 \n Line2 ";
            var reader = new FileReader();
            var test =  reader.ReadLines(lineInput);
            Assert.AreEqual(2,test.Lines);            
        }

        [TestMethod]
        public void CountColumns()
        {
            var lineInput = "Col1 , Col2, Col3, Col4";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            Assert.AreEqual(4, test.Columns);                        
        }

        [TestMethod]
        public void ScapedQuote()
        {
            var lineInput = "Col1 , \"Col2, Col3\", Col4";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            Assert.AreEqual(3, test.Columns);                        
        }

        [TestMethod]
        public void MultiColumnLinesTest()
        {
            var lineInput = "Col1 , Col2, Col3, Col4 \n Col1 , Col2, Col3, Col4";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);            
            Assert.AreEqual(2, test.Lines);
            
        }

        [TestMethod]
        public void MultiLineColumnsTest()
        {
            var lineInput = "Col1 , Col2, Col3, Col4 \n Col1 , Col2, Col3, Col4";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            Assert.AreEqual(4, test.Columns);            
        }

        [TestMethod]
        public void MultiLineColumnsUnEvenTest()
        {
            var lineInput = "Col1 , Col2, Col3, Col4 \n Col1 , Col2, Col3, Col4, Col5 \n Col1 , Col2, Col3, Col4";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            Assert.AreEqual(5, test.Columns);
        }

        [TestMethod]
        public void ColumnTransform()
        {
            var lineInput = "Line1";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            var result = test.Cell(0, 0);
            Assert.AreEqual("Line1", result);
        }

        [TestMethod]
        public void ColumnTransformLines()
        {
            var lineInput = "Line1 \n Line2";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            var result = test.Cell(0, 0);
            Assert.AreEqual("Line1", result);
        }

        [TestMethod]
        public void ColumnTransformLinesScaped()
        {
            var lineInput = "Col1, \"Col2, Col3\" \n Line2";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            var result = test.Cell(0, 1);
            Assert.AreEqual("Col2, Col3", result);
        }

        [TestMethod]
        public void ColumnTransformLinesCellBeforeScaped()
        {
            var lineInput = "Col1, \"Col2, Col3\" \n Line2";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            var result = test.Cell(0, 0);
            Assert.AreEqual("Col1", result);
        }

    }
}
