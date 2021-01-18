using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO.Compression;

namespace Services.PDFService
{
    public class PDFFileMergerService
    {

        public static byte[] MergeFiles(List<byte[]> sourceFiles)
        {

            Document document = new Document();
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PdfCopy copy = new PdfCopy(document, ms);

                    // copy.SetFullCompression();
                    // copy.CompressionLevel = PdfStream.BEST_COMPRESSION;
                    //copy.CompressionLevel = PdfStream.BEST_SPEED;
                    document.Open();
                    int documentPageCounter = 0;
                    
                    // Iterate through all pdf documents
                    for (int fileCounter = 0; fileCounter < sourceFiles.Count; fileCounter++)
                    {
                        // Create pdf reader
                        PdfReader reader = new PdfReader(sourceFiles[fileCounter]);
                        int numberOfPages = reader.NumberOfPages;

                        // Iterate through all pages
                        for (int currentPageIndex = 1; currentPageIndex <= numberOfPages; currentPageIndex++)
                        {
                            documentPageCounter++;
                            reader.SetPageContent(currentPageIndex, reader.GetPageContent(currentPageIndex), PdfStream.BEST_COMPRESSION, true);
                            PdfImportedPage importedPage = copy.GetImportedPage(reader, currentPageIndex);
                            PdfCopy.PageStamp pageStamp = copy.CreatePageStamp(importedPage);

                            pageStamp.AlterContents();
                            copy.AddPage(importedPage);
                        }

                        copy.FreeReader(reader);
                        reader.Close();
                    }

                    document.Close();
                    //  return ms.GetBuffer();

                    return ApplyPDFSecurityInMergedDocument(ms.GetBuffer());


                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }//end function

        private static byte[] ApplyPDFSecurityInMergedDocument(byte[] PDFbytes)
        {

            try
            {

                //   PdfReader readerToEncrypt = new PdfReader(PDFbytes);
                PdfReader readerToEncrypt = new PdfReader(PDFbytes);
                using (System.IO.MemoryStream outStream = new System.IO.MemoryStream())
                {

                    PdfEncryptor.Encrypt(readerToEncrypt, outStream, true, null, "FactionHSG123!", PdfWriter.AllowScreenReaders|PdfWriter.AllowPrinting);
                    return outStream.GetBuffer();
                }    
  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static byte[] Compress(byte[] data)
        {

            using (var outStream = new MemoryStream())
            {
                using (var compress = new DeflateStream(outStream, CompressionMode.Compress, true))
                {
                    compress.Write(data, 0, data.Length);
                    compress.Close();
                }
                return outStream.GetBuffer();
            }
           
        }

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
              
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}