using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Diagnostics;
using System.Data.SqlClient;
using ClosedXML.Excel;
using ExcelDataReader;
using System.Data;
using SFTPFileUpload.Model;
using SFTPFileUpload.DAL;
using SFTPFileUpload.Helper;
using System.Configuration;

namespace SFTPFileUpload
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SFTP_DAY_End_Report_Model orderModel = new SFTP_DAY_End_Report_Model();
                SFTP_Day_End_Report_Dal DalManager = new SFTP_Day_End_Report_Dal();
                List<SFTP_DAY_End_Report_Model> orderModelList = new List<SFTP_DAY_End_Report_Model>();
                ListToDatatable helperObj = new ListToDatatable();
                bool IsFileExist = false;
                string host = System.Configuration.ConfigurationManager.AppSettings["host"].ToString();
                string username = System.Configuration.ConfigurationManager.AppSettings["username"].ToString();
                string password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
                int port = 2022;  //Port 22 is defaulted for SFTP upload
                DateTime dt = DateTime.Now;
                string date = dt.ToString("yyyy-MM-dd");
                date = date.Replace("-", "");
                string remoteFilePath = System.Configuration.ConfigurationManager.AppSettings["remoteFilePath"].ToString();
                //UTC_Daily_Settled_Transaction_Report_20231208.xlsx
                remoteFilePath = remoteFilePath + date + ".xlsx";
                string fileName = $"data_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                string localpath = System.Configuration.ConfigurationManager.AppSettings["localFilePath"].ToString();
                string localFilePath = Path.Combine(localpath, fileName);
                string NewFilePath = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString();
                string DownloadToFolder = System.Configuration.ConfigurationManager.AppSettings["DownloadToFolder"].ToString();
                string ArchivePath = System.Configuration.ConfigurationManager.AppSettings["uploadToArchive"].ToString();
                DateTime newdate = DateTime.Now;
                string newfilename = newdate.ToString("dd-MMM-yyyy");
                newfilename = newfilename.Replace("-", "");
                NewFilePath = NewFilePath + "UTC_MPR_" + newfilename + ".xlsx";
                //Provide the local directory to save the downloaded file
                IsFileExist = IsFileExistsOnSFTP(host, port, username, password, remoteFilePath);
                if (IsFileExist == true)
                {
                    //Code to download file to local folder
                   string downloadedfile= DownloadFileFromSFTP(host, port, username, password, remoteFilePath, DownloadToFolder);

                    //code to download file to another folder
                    UploadSFTPFile(host, username, password, downloadedfile, ArchivePath, port);

                    //Code to read and upload new  file 
                    using (SftpClient client = new SftpClient(host, port, username, password))
                    {
                        client.Connect();
                        Stream stream = GetFileStream(client, remoteFilePath);

                        using (XLWorkbook wb = new XLWorkbook(stream))
                        {
                            var ws = wb.Worksheet(1);
                            var count = ws.RangeUsed().RowsUsed();
                            foreach (var r in ws.RangeUsed().RowsUsed().Skip(2))
                            {
                                var cell5 = r.Cell(5).GetString();
                                if (!string.IsNullOrEmpty(cell5))
                                {
                                    orderModel = DalManager.GetOrderDetails(cell5);
                                    if (orderModel != null)
                                    {
                                        orderModel.TransactionId = (r.Cell(3).GetString());
                                        orderModel.StoreId = Convert.ToInt32(r.Cell(4).GetString());
                                        orderModel.StorePickUpAddress = r.Cell(8).GetString();
                                        try
                                        {
                                            orderModel.DateOfTRX = r.Cell(1).GetString();

                                            if (!string.IsNullOrEmpty(orderModel.DateOfTRX))
                                            {
                                                double myOADate = Convert.ToDouble(orderModel.DateOfTRX);
                                                DateTime myDate = DateTime.FromOADate(myOADate);
                                                DateTime dtCreateddate = myDate;
                                                orderModel.DateOfTRX = dtCreateddate.ToString("dd-MMM-yyyy");
                                            }
                                            else
                                            {
                                                orderModel.DateOfTRX = string.Empty;
                                            }
                                        }
                                        catch (Exception ex1)
                                        {
                                            Console.WriteLine("problem reading date formate of transaction " + ex1.Message);
                                            Console.ReadLine();
                                        }

                                        orderModelList.Add(orderModel);
                                    }
                                }

                            }
                            if (orderModelList.Count > 0)
                            {
                                DataTable dtNew = helperObj.ConvertModelListToDataTable(orderModelList);

                                dtNew.Columns["DateOfTRX"].ColumnName = "Date Of TRX";
                                dtNew.Columns["PickUpStatus"].ColumnName = "Pick Up Status";
                                dtNew.Columns["DateOfDevicePickUp"].ColumnName = "Date Of Device Pick Up";
                                dtNew.Columns["TransactionId"].ColumnName = "Transaction Id";
                                dtNew.Columns["StoreId"].ColumnName = "Store Id";
                                dtNew.Columns["VoucherCode"].ColumnName = "Voucher Code";
                                dtNew.Columns["Amount"].ColumnName = "Amount";
                                dtNew.Columns["StorePickUpAddress"].ColumnName = "Store Pick Up Address";
                                dtNew.Columns["OEMBonusAmount"].ColumnName = "OEM Bonus Amount";
                                dtNew.Columns["UTRNumber"].ColumnName = "UTR Number";
                                // convert datatable to excel file
                                ConvertDataTableToExcelAndUpload(dtNew, localFilePath);

                                // Open the local Excel file as a FileStream
                                using (var fileStream = File.OpenRead(localFilePath))
                                {
                                    // Upload the file to the SFTP server
                                    client.UploadFile(fileStream, NewFilePath);
                                }
                                // Delete the local Excel file
                                File.Delete(localFilePath);
                                client.Delete(remoteFilePath);
                                Console.WriteLine("File uploaded successfully");
                            }
                            else
                            {
                                client.Delete(remoteFilePath);
                                Console.WriteLine("File is empty does not contain any data");
                                Console.ReadLine();
                            }
                        }
                        client.Disconnect();
                    }
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No such file available");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" "+ ex.Message);
                Console.ReadLine();
            }

        }

        public static Stream GetFileStream(SftpClient sftpClient, string sourcePath)
        {
            var memoryStream = new MemoryStream();

            sftpClient.DownloadFile(sourcePath, memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

        public static void ConvertListToExcel(List<SFTP_DAY_End_Report_Model> models, string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                // Add headers
                int column = 1;
                foreach (var property in typeof(SFTP_DAY_End_Report_Model).GetProperties())
                {
                    worksheet.Cell(1, column).Value = property.Name;
                    column++;
                }

                // Add data rows
                int row = 2;
                foreach (var model in models)
                {
                    column = 1;
                    foreach (var property in typeof(SFTP_DAY_End_Report_Model).GetProperties())
                    {
                        var value = property.GetValue(model);
                        worksheet.Cell(row, column).Value = value;
                        column++;
                    }
                    row++;
                }

                // Save the workbook to the specified file path
                workbook.SaveAs(filePath);
            }
        }

        public static void ConvertDataTableToExcelAndUpload(DataTable dataTable, string excelFilePath)
        {
            // Convert DataTable to Excel
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dataTable, "Sheet1");
                workbook.SaveAs(excelFilePath);
            }
        }

        public static bool IsFileExistsOnSFTP(string sftpHost, int port, string sftpUsername, string sftpPassword, string sftpFilePath)
        {
            using (var client = new SftpClient(sftpHost, port, sftpUsername, sftpPassword))
            {
                client.Connect();

                if (!client.IsConnected)
                {
                    // Connection failed
                    return false;
                }

                var fileExists = client.Exists(sftpFilePath);

                client.Disconnect();

                return fileExists;
            }
        }

        public static string DownloadFileFromSFTP(string sftpHost, int port, string sftpUsername, string sftpPassword, string sftpFilePath, string localFolderPath)
        {
            string Localpath = null;
            try
            {
                using (var client = new SftpClient(sftpHost, port, sftpUsername, sftpPassword))
                {
                    client.Connect();

                    if (!client.IsConnected)
                    {
                        // Connection failed
                        return null;
                    }

                    var fileName = Path.GetFileName(sftpFilePath);
                    var localFilePath = Path.Combine(localFolderPath, fileName);
                    Localpath = localFilePath;
                    using (var fileStream = File.Create(localFilePath))
                    {
                        client.DownloadFile(sftpFilePath, fileStream);
                    }

                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("" + ex.Message);
                Console.ReadLine();
            }
            return Localpath;
        }



        public static void UploadSFTPFile(string host, string username,string password, string sourcefile, string destinationpath, int port)
        {
            using (SftpClient client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                client.ChangeDirectory(destinationpath);
                var fileName = Path.GetFileName(sourcefile);
                destinationpath = Path.Combine(destinationpath, fileName);
                using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                {
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(fs, Path.GetFileName(sourcefile));
                }
                client.Disconnect();
               
            }

        }
    }
}
