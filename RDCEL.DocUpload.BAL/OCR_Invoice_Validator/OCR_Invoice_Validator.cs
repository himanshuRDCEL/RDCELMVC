using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace RDCEL.DocUpload.BAL.OCR_Invoice_Validator
{
    public class OCR_Invoice_Validator
    {
        #region Validate Invoice 
        /// <summary>
        /// Method to  Validate Invoice
        /// </summary>       
        /// <returns></returns>  
        public bool Invoice_OCR(byte[] ImageArray)
        {
            //IronTesseract ocr = new IronTesseract();
            //using (var input = new OcrInput(ImageArray))
            {
               // var result = ocr.Read(input);
                //if (result != null)
                //{
                //    var ft = result.Blocks.Where(x => x.Text.ToLower().Contains("gst") || x.Text.ToLower().Contains("date") || x.Text.ToLower().Contains("amount")).Count();
                //    if (ft != 0)
                //    {
                //        //Console.WriteLine(result.Text);
                //        //Console.WriteLine("Valid Invoice");
                //        //Console.ReadLine();
                //        return true;
                //    }
                //    else
                //    {
                //        //Console.WriteLine(result.Text);
                //        //Console.WriteLine("Invalid Invoice");
                //        //Console.ReadLine();
                //        return false;
                //    }

                }
            //else
            //{
            //    //Console.WriteLine("Invalid Invoice");
            //    //Console.ReadLine();
            //    return true;

            //}
            return true;
        }
        
        #endregion
    }
}
