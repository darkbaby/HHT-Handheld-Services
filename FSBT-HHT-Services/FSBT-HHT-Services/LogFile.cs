using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBT_HHT_Services
{
    public class LogFile
    {
        private static string LOG_PATH;
        private static string LOG_FILE_NAME;
        private static string LOG_FILE_PATH;
        public static bool IS_READLY { get; set; }

        public LogFile(string path, string name)
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");
            LOG_PATH = path;
            LOG_FILE_NAME = name;
            LOG_FILE_PATH = path + @"\" + LOG_FILE_NAME;
            generateLogFile();
        }

        public static string getLogFileName()
        {
            return LOG_FILE_NAME;
        }

        public static string getLogPath()
        {
            return LOG_PATH;
        }

        public static string getFileName()
        {
            return LOG_FILE_NAME;
        }

        private static void createLogFile()
        {
            try
            {
                if (generateLogFile())
                {
                    IS_READLY = true;
                }
                else
                    IS_READLY = false;
            }
            catch
            {
                IS_READLY = false;
            }
        }

        private static bool generateLogFile()
        {
            try
            {
                bool isPass = true;
                isPass = generateFolder();

                if (isPass)
                    isPass = generateFile();

                return isPass;
            }
            catch
            {
                return false;
            }
        }

        private static bool generateFolder()
        {
            try
            {
                if (!Directory.Exists(LOG_PATH))
                {
                    Directory.CreateDirectory(LOG_PATH);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool generateFile()
        {
            try
            {
                string logFilePath = LOG_FILE_PATH;
                if (!File.Exists(logFilePath))
                {
                    var myFile = File.Create(logFilePath);
                    myFile.Close();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Write Log
        public static void write(string status, string Message)
        {
            try
            {
                StringBuilder logMessage = new StringBuilder();
                System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("en-US");

                logMessage.Append(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss ", cultureinfo));
                logMessage.Append("[" + status + "] : ");
                logMessage.Append(Message);

                File.AppendAllText(LOG_FILE_PATH, logMessage.ToString() + Environment.NewLine);
                Console.WriteLine(logMessage.ToString());
            }
            catch
            {

            }
        }
        #endregion
    }
}
