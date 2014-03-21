using System;
using System.IO;
using System.Text;

namespace McMDK.Utils.Log
{
    public class Logger
    {
        private string name;
        private string file;
        private Logger logger;

        public Logger(string name)
        {
            this.name = name;
            if (!FileController.Exists(Define.LogDirectory))
            {
                FileController.CreateDirectory(Define.LogDirectory);
            }
            this.file = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
        }

        public void SetParent(Logger parentLogger)
        {
            this.logger = parentLogger;
        }

        public string GetLoggerName()
        {
            return this.name;
        }

        public string GetLoggerFile()
        {
            return this.file;
        }

        public void Fine(object msg, Exception e = null)
        {
            this.Write(LogLevel.FINE, msg, e);
        }

        public void Info(object msg, Exception e = null)
        {
            this.Write(LogLevel.INFO, msg, e);
        }

        public void Warning(object msg, Exception e = null)
        {
            this.Write(LogLevel.WARNING, msg, e);
        }

        public void Error(object msg, Exception e = null)
        {
            this.Write(LogLevel.ERROR, msg, e);
        }

        public void Severe(object msg, Exception e = null)
        {
            this.Write(LogLevel.SEVERE, msg, e);
        }

        public void Debug(object msg, Exception e = null)
        {
            this.Write(LogLevel.DEBUG, msg, e);
        }

        public void Write(LogLevel level, object msg, Exception e)
        {
            if (msg == null)
            {
                return;
            }

            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss "));
            sb.Append("[" + this.name + "]");
            switch (level)
            {
                case LogLevel.FINE:
                    sb.Append("[FINE ]");
                    break;

                case LogLevel.INFO:
                    sb.Append("[INFO ]");
                    break;

                case LogLevel.WARNING:
                    sb.Append("[WARN ]");
                    break;

                case LogLevel.ERROR:
                    sb.Append("[ERROR]");
                    break;

                case LogLevel.SEVERE:
                    sb.Append("[FATAL]");
                    break;

                case LogLevel.DEBUG:
                    sb.Append("[DEBUG]");
                    break;
            }
            sb.Append(msg.ToString().Replace("\n", "").Replace("\r", ""));
            if (e != null)
            {
                sb.Append(Environment.NewLine + e);
            }
            Console.WriteLine(sb.ToString());

            try
            {
                StreamWriter sw;
                if (this.logger == null)
                {
// ReSharper disable once PossibleNullReferenceException
                    sw = new StreamWriter(Define.LogDirectory + "\\" + this.logger.GetLoggerFile() + ".log", true);
                }
                else
                {
                    sw = new StreamWriter(Define.LogDirectory + "\\" + this.file + ".log", true);
                }
                sw.AutoFlush = true;
                sw.WriteLine(sb.ToString());
                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
