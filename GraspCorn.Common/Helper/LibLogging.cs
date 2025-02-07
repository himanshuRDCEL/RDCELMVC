using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace GraspCorn.Common.Helper
{
    public class LibLogging
    {
        /// <summary>
        /// Method to write error to folder
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Code">Code</param>
        /// <param name="ex">ex</param>
        public static void WriteErrorToDB(string Source, string Code, Exception ex = null)
        {
            string ErrorFolder = ConfigurationManager.AppSettings["LogPath"].ToString();
            string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
            ErrorFolder = rootPath + ErrorFolder;
            if (!Directory.Exists(ErrorFolder))
            {
                Directory.CreateDirectory(ErrorFolder);
            }
            string filePath = @"\" + Convert.ToString(DateTime.Now.Date.Year) + "_" + Convert.ToString(DateTime.Now.Date.Month) + "_" + Convert.ToString(DateTime.Now.Date.Day) + ".txt";
            filePath = ErrorFolder + filePath;
            if (!File.Exists(filePath))
            {
                FileStream fs1 = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                fs1.Close();
            }

            try
            {
                int addLog = Convert.ToInt32(ConfigurationManager.AppSettings["AddLog"]);
                //int addLog = 1;
                if (addLog == 1)
                {

                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        string message = ex != null ? ex.Message : string.Empty;
                        string stackTrace = ex != null ? ex.StackTrace : string.Empty;
                        writer.WriteLine("Methodl: " + Code + " | Source: " + Source + " | Message :" + message + "<br/>" + Environment.NewLine + "StackTrace :" + stackTrace +
                           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                        writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                        if (ex != null && ex.InnerException != null)
                        {
                            writer.WriteLine("Inner ex Message - Level 1: " + ex.InnerException.Message);
                            writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                            if (ex.InnerException.InnerException != null)
                            {
                                writer.WriteLine("Inner ex Message - Level 2: " + ex.InnerException.InnerException.Message);
                                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);

                            }
                        }

                    }
                }

            }
            catch (Exception ex1)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Methodl: WriteErrorToDB | Source: LibLogging | Message :" + ex1.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex1.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }


        public static void AddTransactionLog(string transactionType, object content)
        {
            string jsonString = JsonConvert.SerializeObject(content);
        }
    }
}
