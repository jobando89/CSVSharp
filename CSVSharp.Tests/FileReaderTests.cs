using System;
using System.Diagnostics;
using System.IO;
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

        [TestMethod]
        public void ColumnHeaders()
        {
            var lineInput = "T1, T2, T3 \n Col1, Col2, Col3";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            var result = test.Cell(0, 1);
            Assert.AreEqual("T2", result);
        }

        [TestMethod]
        public void ColumnHeadersMalformed()
        {
            var lineInput = "T1, \"T2, T3\" \n Col1, Col2, Col3";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            var result = test.Cell(0, 2);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void ColumnHeadersExtended()
        {
            var lineInput = "T1, T2, T3 \r\n Col1, Col2, Col3";
            var reader = new FileReader();
            var test = reader.ReadLines(lineInput);
            var result = test.Cell(1, 2);
            Assert.AreEqual("Col3", result);
        }

        [TestMethod]
        public void ColumnHeadersTest1()
        {
            var bytes = File.ReadAllBytes("file1.csv");     
            var reader = new FileReader();
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            var test = reader.ReadLines(bytes);
            //stopWatch.Stop();
            //Console.WriteLine(stopWatch.Elapsed);
            var result = test.Cell(test.Lines-1, test.Columns-1);
            Assert.AreEqual("160.204.204.13!0", result);
        }

    }
}
