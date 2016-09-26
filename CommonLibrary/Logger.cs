using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CodeMergerEntity;

namespace CommonLibrary
{
    public class Logger
    {
        string sPath = string.Empty;
        public Logger(string path)
        {
            sPath = path;
        }
        public bool WriteLog(List<string> contentList)
        {
            string dateTime = "Created On: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
            string footer = "-----------------------------------------------------------------------------------------";

            List<string> finalLog = new List<string>();
            finalLog.Add(dateTime);
            finalLog.AddRange(contentList);
            finalLog.Add(footer);

            // string path = @"D:\Temp\Test.txt";
            if (File.Exists(sPath))
            {
                File.Delete(sPath);
            }

            File.AppendAllLines(sPath, finalLog);

            return true;
        }

    }
}
