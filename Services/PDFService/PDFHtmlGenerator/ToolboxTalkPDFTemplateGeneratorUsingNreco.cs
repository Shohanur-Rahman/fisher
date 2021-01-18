using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Core.DataService;
using Services.DataContext;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Services.Domain;
using System.IO;
using Services.Utilities;
using Services.Helper;
using NReco.PdfGenerator;
using Services.PDFService.NRecoService;

namespace Services.PDFService
{
   public  class ToolboxTalkPDFTemplateGeneratorUsingNreco : EntityContextDataService<User>
    {
        public ToolboxTalkPDFTemplateGeneratorUsingNreco(AppDbContext dbContext) : base(dbContext)
        {

        }
        //public ToolboxTalksPDFOperationModel GenerateRAMSToolboxTalkPDFText(long toolboxTalkID, long userId)
        //{
        //    ToolboxTalksPDFOperationModel toolboxtalkPDFOperationModel = new ToolboxTalksPDFOperationModel();
        //    toolboxtalkPDFOperationModel.userData = DbContext.Set<User>().Find(userId);
        //    toolboxtalkPDFOperationModel.toolboxTalks = DbContext.Set<ToolboxTalks>().Find(toolboxTalkID);
        //    toolboxtalkPDFOperationModel.userCompany = DbContext.Set<UserCompany>().Where(x => x.CompanyUserID == userId).FirstOrDefault();

        //    return toolboxtalkPDFOperationModel;
        //}

        //public byte[] GetToolBoxTalkPDFFileUsingNreco(ToolboxTalks assessment, long userId, string dynamicLinkForPDF, string dynamicHeaderHTML, string dynamicFooterHTML)
        //{
        //    List<byte[]> PDFAndExtraPDFBytes = new List<byte[]>();
            
        //    byte[] projectPDF = PDFGeneratorNReco.GeneratePDFWithNReco(dynamicFooterHTML, dynamicLinkForPDF);

        //    PDFAndExtraPDFBytes.Add(projectPDF);

        //    //Get the pdf file bytes after merging all the pdf documents
        //    byte[] mergePDFBytes = PDFFileMergerService.MergeFiles(PDFAndExtraPDFBytes);

        //    return mergePDFBytes;

        //}
    }
}
