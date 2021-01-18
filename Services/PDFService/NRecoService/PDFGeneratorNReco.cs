using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NReco.PdfGenerator;
using System.IO;
using iTextSharp.text.pdf;

namespace Services.PDFService.NRecoService
{
   public class PDFGeneratorNReco
    {
        public static byte[] GeneratePDFWithNReco(string dynamicFooterHTML, string dynamicLinkForProjectPDF)
        {
            //Generate Project PDF file using NReco
            var htmlToPdf = new HtmlToPdfConverter();
            htmlToPdf.License.SetLicenseKey(
                "PDF_Generator_Bin_Examples_Pack_206629748219",
                "T0ei2XmeU4oTo7hATvF0sWc2xMfBvVSHY74BWDfBy4qyQ5KM37vB1mvV2tOC3/uYYbREPTEzwpkiFwv6FoktYj5kkTpWPgiKYU2BHpgiRMxDNrVSZg/CgBOsoUaRgQv0fDRUKP6FpXAlLv/LPNIoDUbbObw++pd+sOif9suynIg="
            );

            htmlToPdf.PageWidth = 210;
            htmlToPdf.PageHeight = 287;
            htmlToPdf.PageFooterHtml = dynamicFooterHTML;
            htmlToPdf.Margins = new PageMargins { Top = 8, Bottom = 13, Left = 7, Right = 7 };

            var htmlContent = String.Format("<body>Hello world: {0}</body>",DateTime.Now);
            var htmlToPdfTest = new NReco.PdfGenerator.HtmlToPdfConverter();
            //var pdfBytes = htmlToPdf.GeneratePdf(htmlContent);

            byte[] pdfBytes = htmlToPdf.GeneratePdfFromFile(dynamicLinkForProjectPDF, null);

            return pdfBytes;
            
        }

        private static byte[] ApplyPDFEncrypt(byte[] PDFbytes)
        {

            try
            {
                PdfReader readerToEncrypt = new PdfReader(PDFbytes);

                using (System.IO.MemoryStream outStream = new System.IO.MemoryStream())
                {

                    PdfEncryptor.Encrypt(readerToEncrypt, outStream, true, null, "FactionHSG123", PdfWriter.AllowScreenReaders | PdfWriter.AllowPrinting);
                    return outStream.GetBuffer();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }//end class



}//end namespace
