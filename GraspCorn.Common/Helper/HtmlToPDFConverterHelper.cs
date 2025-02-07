using SelectPdf;
using System;
using System.IO;

namespace GraspCorn.Common.Helper
{
    public class HtmlToPDFConverterHelper
    {
        public static bool GeneratePDF(string htmlString, string requestPath, string customFileName)
        {
            bool flag = false;
            string fileName = "";
            var fileNameWithPath = "";
            try
            {
                if (customFileName != null && customFileName != "")
                {
                    fileName = customFileName;
                }
                else
                {
                    fileName = Guid.NewGuid().ToString("N");
                }
                if (requestPath != null)
                {
                    var filePath = string.Concat(requestPath);
                  
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath); //Create directory if it doesn't exist
                    }
                    fileNameWithPath = string.Concat(filePath, "\\", fileName);
                }
                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.MarginLeft = 5;
                converter.Options.MarginRight = 5;
                converter.Options.MarginTop = 5;
                converter.Options.MarginBottom = 5;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

                // header settings
                converter.Options.DisplayHeader = true;
                converter.Header.DisplayOnFirstPage = true;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = 15;

                // footer settings
                converter.Options.DisplayFooter = true;
                converter.Footer.DisplayOnFirstPage = true;
                converter.Footer.DisplayOnOddPages = true;
                converter.Footer.DisplayOnEvenPages = true;
                converter.Footer.Height = 15;

                //  converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                /*
                                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.ShrinkOnly;
                                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;*/

                PdfDocument doc = converter.ConvertHtmlString(htmlString);
                doc.Margins.Left = 0;
                doc.Margins.Right = 0;
                doc.Save(fileNameWithPath);
                doc.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("HtmlToPDFConverterHelper", "GeneratePDF", ex);
            }
            return flag;
        }
    }
}