using Core.DataService;
using Services.DataContext;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.FarmExpend;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.FarmExpend
{
   public class FarmExpendDataService : EntityContextDataService<FarmExpandTableDomain>
    {
        public FarmExpendDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }
        public bool ProcessFarmExpandReport(FarmExpandTableDomain farmExpendReport, int adminUserId)
        {


            if (farmExpendReport != null && farmExpendReport.isAddOperation == true)
            {
                ExpendReportTable expendReport = new ExpendReportTable();
                expendReport.ExpendRepoterName = farmExpendReport.ExpendReportTable.ExpendRepoterName;
                expendReport.ExpendId = farmExpendReport.ExpendReportTable.ExpendId;
                expendReport.TotalExpend = farmExpendReport.ExpendReportTable.TotalExpend;
                expendReport.ExpandNote = farmExpendReport.ExpendReportTable.ExpandNote;

                expendReport.CreateDate = DateTime.Now;
                expendReport.CreatedId = adminUserId;
                DbContext.Set<ExpendReportTable>().Add(expendReport);
                DbContext.SaveChanges();
                

                if (farmExpendReport.ExpendReportMapperList != null && farmExpendReport.ExpendReportMapperList.Count > 0)
                {
                    for (int item = 0; item < farmExpendReport.ExpendReportMapperList.Count; item++)
                    {
                        ExpendReportMapper singleItem = new ExpendReportMapper();
                        var thisItem = farmExpendReport.ExpendReportMapperList[item];

                        if (thisItem.ExpendAmount> 0)
                        {
                            //Assign Value in New Fish Object
                            singleItem.ExpendMapperId = 0;
                            singleItem.ExpendReportId = expendReport.ExpendReportId;
                            singleItem.AreaId = thisItem.AreaId;
                            singleItem.ExpendAmount = thisItem.ExpendAmount;

                            DbContext.Set<ExpendReportMapper>().Add(singleItem);
                            DbContext.SaveChanges();
                        }

                    }

                }

                return true;
            }

            if (farmExpendReport != null && farmExpendReport.isAddOperation == false)
            {
                ExpendReportTable expendReport = DbContext.Set<ExpendReportTable>().Where(x => x.ExpendReportId == farmExpendReport.ExpendReportTable.ExpendReportId).FirstOrDefault();

                
                expendReport.ExpendRepoterName = farmExpendReport.ExpendReportTable.ExpendRepoterName;
                expendReport.ExpendId = farmExpendReport.ExpendReportTable.ExpendId;
                expendReport.TotalExpend = farmExpendReport.ExpendReportTable.TotalExpend;
                expendReport.ExpandNote = farmExpendReport.ExpendReportTable.ExpandNote;
                
                if (farmExpendReport.ExistingMapperIdListForEdit != "")
                {
                    var splitIdArray = farmExpendReport.ExistingMapperIdListForEdit.Split(',');
                    if (splitIdArray.Length > 0)
                    {
                        for (int item = 0; item < splitIdArray.Length; item++)
                        {
                            var mapId = splitIdArray[item];
                            int result = DbContext.Database.ExecuteSqlCommand("DELETE FROM [ExpendReportMapper] where ExpendMapperId = " + mapId + ";");
                        }
                    }

                }
               

                if (farmExpendReport.ExpendReportMapperList != null && farmExpendReport.ExpendReportMapperList.Count > 0)
                {
                    for (int item = 0; item < farmExpendReport.ExpendReportMapperList.Count; item++)
                    {
                        ExpendReportMapper singleItem = new ExpendReportMapper();
                        var thisItem = farmExpendReport.ExpendReportMapperList[item];

                        if (thisItem.ExpendAmount > 0)
                        {
                            //Assign Value in New Expend Object
                            singleItem.ExpendMapperId = 0;
                            singleItem.ExpendReportId = expendReport.ExpendReportId;
                            singleItem.AreaId = thisItem.AreaId;
                            singleItem.ExpendAmount = thisItem.ExpendAmount;

                            DbContext.Set<ExpendReportMapper>().Add(singleItem);
                            DbContext.SaveChanges();
                        }

                    }

                }

                DbContext.SaveChanges();

                return true;
            }

            return false;

        }


        public FarmExpandTableDomain GetFarmExpendDetailsByExpendID(int id)
        {
            FarmExpandTableDomain sellingReport = new FarmExpandTableDomain();
            ExpendReportTable sellingOverview = new ExpendReportTable();
            List<ExpendReportMapper> fishDetailsMapperList = new List<ExpendReportMapper>();
            List<CreateSegmentArea> areaNameList = new List<CreateSegmentArea>();

            sellingOverview = DbContext.Set<ExpendReportTable>().Where(x => x.ExpendReportId == id).FirstOrDefault();
            fishDetailsMapperList = DbContext.Set<ExpendReportMapper>().Where(x => x.ExpendReportId == id).ToList();
            areaNameList = DbContext.Set<CreateSegmentArea>().Where(x => x.ID != 0).ToList();

            sellingReport.ExpendReportTable = sellingOverview;



            string fishIdList = "";
            string existingMapperID = "";

            if (fishDetailsMapperList.Count > 0)
            {
                for (int item = 0; item < fishDetailsMapperList.Count; item++)
                {
                    if (fishIdList == "")
                    {
                        fishIdList = fishDetailsMapperList[item].AreaId.ToString();
                    }
                    else
                    {
                        fishIdList = fishIdList + "," + fishDetailsMapperList[item].AreaId.ToString();
                    }

                    if (existingMapperID == "")
                    {
                        existingMapperID = fishDetailsMapperList[item].ExpendMapperId.ToString();
                    }
                    else
                    {
                        existingMapperID = existingMapperID + "," + fishDetailsMapperList[item].ExpendMapperId.ToString();
                    }
                }
            }
            List<ExpendReportMapper> mapperList = new List<ExpendReportMapper>();

            for (int item = 0; item < areaNameList.Count; item++)
            {

                var indexNumber = CheckAreaIdExistInCurrentList(areaNameList[item].ID.ToString(), fishIdList);
                ExpendReportMapper singleItem = new ExpendReportMapper();

                if (indexNumber >= 1)
                {
                    mapperList.Add(fishDetailsMapperList[indexNumber - 1]);
                }
                else
                {
                    singleItem.ExpendReportId = 0;
                    singleItem.ExpendAmount = 0;
                    singleItem.AreaId = 0;

                    mapperList.Add(singleItem);
                }

            }

            sellingReport.ExpendReportMapperList = mapperList;
            //FishSellingReportTableDomain tempFishOverViewList = new FishSellingReportTableDomain();

            sellingReport.AreaIdListWithComma = fishIdList;
            sellingReport.ExistingMapperIdListForEdit = existingMapperID;

            return sellingReport;
        }

        private int CheckAreaIdExistInCurrentList(string thisAreaId, string allAreaId)
        {

            int returnValue = 0;

            if (allAreaId != "")
            {
                var splitIdArray = allAreaId.Split(',');

                for (int item = 0; item < splitIdArray.Length; item++)
                {
                    if (splitIdArray[item] == thisAreaId)
                    {
                        returnValue = item + 1;
                    }
                }
            }

            return returnValue;
        }


    }
}
