using Services.PDFService.NRecoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ApplicationServices.Admin
{
   public class AdminDataAplicationService
    {
        public byte[] GetProjectPDFFileUsingNreco( string dynamicLinkForProjectPDF, string dynamicFooterHTML)
        {
            byte[] projectPDF = PDFGeneratorNReco.GeneratePDFWithNReco(dynamicFooterHTML, dynamicLinkForProjectPDF);
            return projectPDF;


        }


        }
}
