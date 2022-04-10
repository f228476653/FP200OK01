using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FP200OK01.Utilities
{
    class FileService
    {
        static StreamReader sr;

        public string ReadFile(string fileName)
        {
            string fileContent = "";

            try
            {
                sr = new StreamReader(fileName);
                fileContent += sr.ReadToEnd();
            }
            catch (Exception e)
            {
                MessageBox.Show("Reading file occurs error: " + e.Message);
            }

            return fileContent;
        }
    }
}
