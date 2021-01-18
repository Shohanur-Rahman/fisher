using Kendo.Mvc.UI;
using Services.ApplicationEntity.CreateSegement;
using Services.ApplicationServices.CreateSegement.Area;
using Services.ApplicationServices.CreateSegement.Feed;
using Services.ApplicationServices.CreateSegement.Fish;
using Services.ApplicationServices.CreateSegement.Project;
using Services.Domain.CostReport;
using Services.Domain.CreateSegement.Models.Area;
using Services.Domain.CreateSegement.Models.Feed;
using Services.Domain.CreateSegement.Models.Fish;
using Services.Domain.CreateSegement.Models.Project;
using Services.Domain.CreateSegement.SPModels;
using Services.DomainServices.CreateSegement.Area;
using Services.DomainServices.CreateSegement.Feed;
using Services.DomainServices.CreateSegement.Fish;
using Services.DomainServices.CreateSegement.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.AppCode;

namespace Web.Controllers
{
    public class CreateSegmentController : BaseController
    {

        #region "Declare Domain Service's objects Needed for Compnay Admin"

        private readonly FishSegmentDomainService _fishSegmentDomainService;
        private readonly FeedSegmentDomainService _feedSegmentDomainService;

        #endregion

        #region "Application Service object and Application Entity object For Company admin"

        protected CreateSegementAppEntity _createSegementAppEntity;
        protected FishSegmentApplicationService _fishSegmentApplicationService;

        protected FeedSegmentApplicationService _feedSegmentApplicationService;

        protected AreaSegmentApplicationService _areaSegmentApplicationService;

        protected ProjectSegmentApplicationService _projectSegmentApplicationService;

        #endregion

        #region "Constructor"
        public CreateSegmentController(FishSegmentDomainService fishSegmentDomainService, FeedSegmentDomainService feedSegmentDomainService, AreaSegmentDomainService areaSegmentDomainService, ProjectSegmentDomainService projectSegmentDomainService)
        {

            //Assign related domain services for Project Module
            _fishSegmentDomainService = fishSegmentDomainService;
            _feedSegmentDomainService = feedSegmentDomainService;

            //Application Entity: Initialize and assign domain objects in Application Entity 
            _createSegementAppEntity = new CreateSegementAppEntity();
            _createSegementAppEntity.fishSegmentDomainService = fishSegmentDomainService;
            _createSegementAppEntity.feedSegmentDomainService = feedSegmentDomainService;
            _createSegementAppEntity.areaSegmentDomainService = areaSegmentDomainService;
            _createSegementAppEntity.projectSegmentDomainService = projectSegmentDomainService;



            //Application Service: Initialize Application Service object for Project Module
            _fishSegmentApplicationService = new FishSegmentApplicationService(_createSegementAppEntity);
            _feedSegmentApplicationService = new FeedSegmentApplicationService(_createSegementAppEntity);
            _areaSegmentApplicationService = new AreaSegmentApplicationService(_createSegementAppEntity);
            _projectSegmentApplicationService = new ProjectSegmentApplicationService(_createSegementAppEntity);

        }
        
        #endregion


        // GET: CreateSegement
        // All Region for Segment Fish


        #region "Segment Fish  ======================================================================================"

        public ActionResult Index()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View();
        }


        public ActionResult FishSeller()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View("~/Views/CreateSegment/FishSeller/Index.cshtml");
        }

        public ActionResult AddFishSeller()
        {
            return View("~/Views/CreateSegment/FishSeller/Add.cshtml");
        }

        public ActionResult EditFishSeller(int id)
        {
            CreateSegmentFishSellerModel createSegmentFishModel = new CreateSegmentFishSellerModel();
            CreateSegmentFishSeller createSegment = new CreateSegmentFishSeller();

            return DataActionViewService(
               () =>
               {
                   createSegment = _fishSegmentApplicationService.GetSegmentFishSellerDetailsByFishId(id);
                   createSegment.Description= HttpUtility.HtmlDecode(createSegment.Description);
                   createSegmentFishModel.CreateSegmentFishSeller = createSegment;
               },
               () =>
               {
                   if (createSegmentFishModel != null)
                   {
                       return View("~/Views/CreateSegment/FishSeller/Edit.cshtml", createSegmentFishModel);
                   }

                   return View();
               },
               () => View("~/Views/CreateSegment/FishSeller/Edit.cshtml"));
        }

        public ActionResult AddFish()
        {
            return View();
        }

        public ActionResult EditFish(int id)
        {
            int fishId = id;
            CreateSegmentFishModel createSegmentFishModel = new CreateSegmentFishModel();
            CreateSegmentFish createSegment = new CreateSegmentFish();

            return DataActionViewService(
               () =>
               {
                   createSegment = _fishSegmentApplicationService.GetSegmentFishDetailsByFishId(fishId);
                   createSegmentFishModel.CreateSegmentFish = createSegment;
                   createSegmentFishModel.CreateSegmentFish.Description = HttpUtility.HtmlDecode(createSegment.Description);
               },
               () =>
               {
                   if (createSegmentFishModel != null)
                   {
                       return View(createSegmentFishModel);
                   }

                   return View();
               },
               () => View("~/Views/CreateSegment/Index.cshtml"));
        }


        public ActionResult GetSegmentFishSellerDataList(string searchText)
        {
            int totalRows = 0;

            IList<SPSegmentFishSellerList> fishSellerList = _fishSegmentApplicationService.GetSegmentFishSellerListBySearch(searchText);

            totalRows = fishSellerList.Count;

            var result = new DataSourceResult()
            {
                Data = fishSellerList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSegmentFishDataList(string searchText)
        {
            int totalRows = 0;

            IList<CreateSegmentFishList> segmentFishList = _fishSegmentApplicationService.GetSegmentFishListBySearch(searchText, 0, 0);
            
            totalRows = segmentFishList.Count;
            
            var result = new DataSourceResult()
            {
                Data = segmentFishList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetSegmentFeedCategoryList(string searchText)
        {
            int totalRows = 0;

            IList<SPCreateSegmentFeedCategoryList> segmentFishList = _fishSegmentApplicationService.GetSegmentFeedCategoryList(searchText, 0,0);

            totalRows = segmentFishList.Count;

            var result = new DataSourceResult()
            {
                Data = segmentFishList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DeleteFish(int id)
        {
            int result = _fishSegmentApplicationService.DeleteFish(id);
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFishSeller(int id)
        {
            int result = _fishSegmentApplicationService.DeleteFishSeller(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        #region "Save Data"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAFishForSegment(CreateSegmentFishModel createSegmentFishModel)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentFishModel != null)
                    {
                        createSegmentFishModel.CreateSegmentFish.ID = 1;
                        createSegmentFishModel.isAddOperation = true;
                        isAddSuccess = _fishSegmentApplicationService.ProcessSegmentFish(createSegmentFishModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে মাছটি সংযুক্ত করেছেন");
                        return RedirectToAction("Index", "CreateSegment");
                    }
                    else {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "CreateSegment");
                },
                () => View());


        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAFishSellerSegment(CreateSegmentFishSellerModel createSegmentFishModel)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);


            return DataActionViewService(
                () =>
                {
                    if (createSegmentFishModel != null)
                    {
                        createSegmentFishModel.CreateSegmentFishSeller.ID = 1;
                        createSegmentFishModel.isAddOperation = true;
                        isAddSuccess = _fishSegmentApplicationService.ProcessSegmentFishSeller(createSegmentFishModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে বিক্রেতা সংযুক্ত করেছেন");
                        return RedirectToAction("FishSeller", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("FishSeller", "CreateSegment");
                },
                () => View());


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFishSellerSegment(CreateSegmentFishSellerModel createSegmentFishModel)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentFishModel != null)
                    {

                        createSegmentFishModel.isAddOperation = false;
                        isEditSuccess = _fishSegmentApplicationService.ProcessSegmentFishSeller(createSegmentFishModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে বিক্রেতা সম্পাদন করেছেন");
                        return RedirectToAction("FishSeller", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("FishSeller", "CreateSegment");
                },
                () => View());


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSegmentFish(CreateSegmentFishModel createSegmentFishModel)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentFishModel != null)
                    {

                        createSegmentFishModel.isAddOperation = false;
                        isEditSuccess = _fishSegmentApplicationService.ProcessSegmentFish(createSegmentFishModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে মাছটি সম্পাদন করেছেন");
                        return RedirectToAction("Index", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Index", "CreateSegment");
                },
                () => View());


        }


        #endregion

        #region "Upload Fish Image"

        public ActionResult UploadFishImage(List<HttpPostedFileBase> files)
        {
            var file = files[0];

            Guid fileNameInGuid = Guid.NewGuid();

            string savingPath = Server.MapPath("~/Uploads/CreateSegment/Fish");
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var saVingFileName = fileNameInGuid.ToString() + "-" + fileName;
            string uploadFilePathAndName = Path.Combine(savingPath, saVingFileName);
            file.SaveAs(uploadFilePathAndName);

            var result = new DataSourceResult()
            {
                Data = "Uploads/CreateSegment/Fish/" + saVingFileName
            };
            return Json(result, JsonRequestBehavior.AllowGet);
            //return "Uploads/CreateSegment/Fish/" + saVingFileName;
        }

        #endregion

        #endregion



        #region "Segment Feed  ================================================================================================"


        public ActionResult Feed()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View();
        }

        public ActionResult AddFeed()
        {
            return View();
        }

        public ActionResult DeleteFeed(int id)
        {
            int result = _feedSegmentApplicationService.DeleteFeed(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFeedCategory(int id)
        {
            int result = _feedSegmentApplicationService.DeleteFeedCategory(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult EditFeed(int id)
        {
            int feedId = id;
            CreateSegmentFeedModel createSegmentFeedModel = new CreateSegmentFeedModel();
            CreateSegmentFeed createSegment = new CreateSegmentFeed();

            return DataActionViewService(
               () =>
               {
                   createSegment = _feedSegmentApplicationService.GetSegmentFeedDetailsByFishId(feedId);
                   createSegmentFeedModel.CreateSegmentFeed = createSegment;
                   createSegmentFeedModel.CreateSegmentFeed.Description = HttpUtility.HtmlDecode(createSegment.Description);
               },
               () =>
               {
                   if (createSegmentFeedModel != null)
                   {
                       return View(createSegmentFeedModel);
                   }

                   return View();
               },
               () => View("~/Views/CreateSegment/Index.cshtml"));
            
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSegmentFeedCategory(CreateFeedSegmentCategoriesModal feedCategoryModal)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (feedCategoryModal != null)
                    {

                        feedCategoryModal.isAddOperation = false;
                        isEditSuccess = _feedSegmentApplicationService.ProcessSegmentFeedCategory(feedCategoryModal, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্যের ধরণটি সম্পাদন করেছেন");
                        return RedirectToAction("FeedCategory", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("FeedCategory", "CreateSegment");
                },
                () => View());


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSegmentFeedCategory(CreateFeedSegmentCategoriesModal feedCategoryModal)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (feedCategoryModal != null)
                    {
                        feedCategoryModal.isAddOperation = true;
                        isAddSuccess = _feedSegmentApplicationService.ProcessSegmentFeedCategory(feedCategoryModal, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্যের ধরণটি সংযুক্ত করেছেন");
                        return RedirectToAction("FeedCategory", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("FeedCategory", "CreateSegment");
                },
                () => View());


        }


        #region"Save Data"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAFeedForSegment(CreateSegmentFeedModel createSegmentFeedhModel)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentFeedhModel != null)
                    {
                        createSegmentFeedhModel.CreateSegmentFeed.ID = 1;
                        createSegmentFeedhModel.isAddOperation = true;
                        isAddSuccess = _feedSegmentApplicationService.ProcessSegmentFeed(createSegmentFeedhModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্যটি সংযুক্ত করেছেন");
                        return RedirectToAction("Feed", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Feed", "CreateSegment");
                },
                () => View());


        }

        #endregion


        #region "Edit Segment feed"

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFeedForSegment(CreateSegmentFeedModel createSegmentFeedhModel)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentFeedhModel != null)
                    {

                        createSegmentFeedhModel.isAddOperation = false;
                        isEditSuccess = _feedSegmentApplicationService.ProcessSegmentFeed(createSegmentFeedhModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে খাদ্যটি সম্পাদন করেছেন");
                        return RedirectToAction("Feed", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Feed", "CreateSegment");
                },
                () => View());


        }

        #endregion

        #region "Upload Fish Image"

        public string UploadFeedImage(List<HttpPostedFileBase> files)
        {
            var file = files[0];

            Guid fileNameInGuid = Guid.NewGuid();

            string savingPath = Server.MapPath("~/Uploads/CreateSegment/Feed");
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var saVingFileName = fileNameInGuid.ToString() + "-" + fileName;
            string uploadFilePathAndName = Path.Combine(savingPath, saVingFileName);
            file.SaveAs(uploadFilePathAndName);

            return "Uploads/CreateSegment/Feed/" + saVingFileName;
        }

        #endregion


        #region"Feed List"

        public ActionResult GetSegmentFeedDataList(string searchText)
        {
            int totalRows = 0;

            IList<CreateSegmentFeedList> segmentFishList = _feedSegmentApplicationService.GetSegmentFeedListBySearch(searchText, 0,0);


            totalRows = segmentFishList.Count;
            var result = new DataSourceResult()
            {
                Data = segmentFishList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetFeedListForCategory()
        {
            IList<CreateSegmentFeed> feedList = _feedSegmentApplicationService.GetFeedListForCategory();
            
            return Json(feedList, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #endregion


        #region""
        public ActionResult FeedCategory()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View();
        }

        public ActionResult GetSegmentFeedCategoryDataList([DataSourceRequest] DataSourceRequest request, string searchText)
        {
            int totalRows = 0;

            IList<SPCreateSegmentFeedCategoryList> segmentFishList = _feedSegmentApplicationService.GetSegmentFeedCategoryDataList(searchText, request.Page, request.PageSize);

            totalRows = (segmentFishList != null && segmentFishList.Count > 0) ? segmentFishList[0].TotalRows : 0;

            var result = new DataSourceResult()
            {
                Data = segmentFishList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddFeedCategory()
        {
            return View();
        }

        public ActionResult EditFeedCategory(int id)
        {
            int areaId = id;
            CreateFeedSegmentCategoriesModal createSegmentAreaModel = new CreateFeedSegmentCategoriesModal();
            CreateSegmentFeedCategories feedCategory = new CreateSegmentFeedCategories();

            return DataActionViewService(
               () =>
               {
                   feedCategory = _feedSegmentApplicationService.GetSegmentFeedCategoryDetails(areaId);
                   feedCategory.FeedCategoryDetails= HttpUtility.HtmlDecode(feedCategory.FeedCategoryDetails);
                   createSegmentAreaModel.CreateSegmentFeedCategory = feedCategory;
               },
               () =>
               {
                   if (createSegmentAreaModel != null)
                   {
                       return View(createSegmentAreaModel);
                   }

                   return View();
               },
               () => View("~/Views/CreateSegment/EditFeedCategory.cshtml"));
        }


        


        #endregion

        public ActionResult Area()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View();
        }

        public ActionResult DeleteArea(int id)
        {
            int result = _areaSegmentApplicationService.DeleteArea(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region"Save Data"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAAreaForSegment(CreateSegmentAreaModel createSegmentAreaModel)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentAreaModel != null)
                    {
                        createSegmentAreaModel.CreateSegmentArea.ID = 1;
                        createSegmentAreaModel.isAddOperation = true;
                        isAddSuccess = _areaSegmentApplicationService.ProcessSegmentArea(createSegmentAreaModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে প্রকল্পটি সংযুক্ত করেছেন");
                        return RedirectToAction("Area", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Area", "CreateSegment");
                },
                () => View());


        }

        #endregion

        #region "Upload Area Image"

        public string UploadAreaImage(List<HttpPostedFileBase> files)
        {
            var file = files[0];

            Guid fileNameInGuid = Guid.NewGuid();

            string savingPath = Server.MapPath("~/Uploads/CreateSegment/Area");
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var saVingFileName = fileNameInGuid.ToString() + "-" + fileName;
            string uploadFilePathAndName = Path.Combine(savingPath, saVingFileName);
            file.SaveAs(uploadFilePathAndName);

            return "Uploads/CreateSegment/Area/" + saVingFileName;
        }

        #endregion

        public ActionResult AddArea()
        {
            return View();
        }

        public ActionResult EditArea(int id)
        {
            int areaId = id;
            CreateSegmentAreaModel createSegmentAreaModel = new CreateSegmentAreaModel();
            CreateSegmentArea createSegment = new CreateSegmentArea();

            return DataActionViewService(
               () =>
               {
                   createSegment = _areaSegmentApplicationService.GetSegmentAreaDetailsByAreaId(areaId);
                   createSegmentAreaModel.CreateSegmentArea = createSegment;
               },
               () =>
               {
                   if (createSegmentAreaModel != null)
                   {
                       return View(createSegmentAreaModel);
                   }

                   return View();
               },
               () => View("~/Views/CreateSegment/Index.cshtml"));
        }


        #region "Get Area List"


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSegmentArea(CreateSegmentAreaModel createSegmentAreaModel)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentAreaModel != null)
                    {

                        createSegmentAreaModel.isAddOperation = false;
                        isEditSuccess = _areaSegmentApplicationService.ProcessSegmentArea(createSegmentAreaModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে প্রকল্পটি সম্পাদন করেছেন");
                        return RedirectToAction("Area", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }


                    return RedirectToAction("Area", "CreateSegment");
                },
                () => View());


        }

        public ActionResult GetSegmentAreaDataList(string searchText)
        {
            int totalRows = 0;

            IList<SPCreateSegmentAreaList> segmentFishList = _areaSegmentApplicationService.GetSegmentAreaListBySearch(searchText, 0, 0);

            totalRows = (segmentFishList != null && segmentFishList.Count > 0) ? segmentFishList[0].TotalRows : 0;
            
            var result = new DataSourceResult()
            {
                Data = segmentFishList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult Project()
        {
            string message = MessageDisplayHelper.InformationMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.InformationMessage = message;
            string deleteMsg = MessageDisplayHelper.ErrorMessageSetOrDisplay(this, false, string.Empty);
            ViewBag.DeleteInformationMessage = deleteMsg;
            return View();
        }

        public ActionResult DeleteProject(int id)
        {
            int result = _projectSegmentApplicationService.DeleteProject(id);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddProject()
        {
            ViewBag.ListOfArea = new SelectList(_areaSegmentApplicationService.GetAllAreaList(), "ID", "Name"); //_areaSegmentApplicationService.GetAllAreaList();
            
            //ViewBag.ListOfArea = listOfArea;

            return View();
        }


        public ActionResult GetAllAreaList()
        {
            IList<CreateSegmentArea> projectList = _areaSegmentApplicationService.GetAllAreaList();

            return Json(projectList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllExpandList()
        {
            IList<CostExpandTable> projectList = _areaSegmentApplicationService.GetAllExpandList();

            return Json(projectList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjectForSegment(CreateSegmentProjectModel createSegmentProjectModel)
        {
            bool isEditSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentProjectModel != null)
                    {

                        createSegmentProjectModel.isAddOperation = false;
                        createSegmentProjectModel.createSegmentProject.Cost = "";
                        createSegmentProjectModel.createSegmentProject.Land = "";
                        createSegmentProjectModel.createSegmentProject.Time = "";
                        isEditSuccess = _projectSegmentApplicationService.ProcessSegmentProject(createSegmentProjectModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isEditSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে পুকুরটি সম্পাদন করেছেন");
                        return RedirectToAction("Project", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Project", "CreateSegment");
                },
                () => View());


        }

        public ActionResult EditProject(int id)
        {
            int projectId = id;
            CreateSegmentProjectModel createSegmentProjectModel = new CreateSegmentProjectModel();
            CreateSegmentProject createSegment = new CreateSegmentProject();
            ViewBag.ListOfArea = new SelectList(_areaSegmentApplicationService.GetAllAreaList(), "ID", "Name");

            return DataActionViewService(
               () =>
               {
                   createSegment = _projectSegmentApplicationService.GetSegmentProjectDetailsByProjectId(projectId);
                   createSegmentProjectModel.createSegmentProject = createSegment;
               },
               () =>
               {
                   if (createSegmentProjectModel != null)
                   {
                       return View(createSegmentProjectModel);
                   }

                   return View();
               },
               () => View("~/Views/CreateSegment/Index.cshtml"));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAProjectForSegment(CreateSegmentProjectModel createSegmentProjectModel)
        {
            bool isAddSuccess = false;
            int adminUserId = Convert.ToInt32(LoggedInUserInfoFromCookie.UserIdInCookie);

            return DataActionViewService(
                () =>
                {
                    if (createSegmentProjectModel != null)
                    {
                        createSegmentProjectModel.createSegmentProject.ID = 1;
                        createSegmentProjectModel.isAddOperation = true;
                        createSegmentProjectModel.createSegmentProject.Cost = "";
                        createSegmentProjectModel.createSegmentProject.Land ="";
                        createSegmentProjectModel.createSegmentProject.Time = "";
                        isAddSuccess = _projectSegmentApplicationService.ProcessSegmentProject(createSegmentProjectModel, adminUserId);
                    }

                },
                () =>
                {
                    if (isAddSuccess == true)
                    {
                        MessageDisplayHelper.InformationMessageSetOrDisplay(this, true, "আপনি সফল ভাবে পুকুরটি সংযুক্ত করেছেন");
                        return RedirectToAction("Project", "CreateSegment");
                    }
                    else
                    {
                        MessageDisplayHelper.ErrorMessageSetOrDisplay(this, true, "কিছু ত্রূটি আছে, আমরা আন্তরিকভাবে দুঃখিত");
                    }

                    return RedirectToAction("Project", "CreateSegment");
                },
                () => View());


        }


        public string UploadProjectImage(List<HttpPostedFileBase> files)
        {
            var file = files[0];

            Guid fileNameInGuid = Guid.NewGuid();

            string savingPath = Server.MapPath("~/Uploads/CreateSegment/Project");
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }

            var fileName = Path.GetFileName(file.FileName);
            var saVingFileName = fileNameInGuid.ToString() + "-" + fileName;
            string uploadFilePathAndName = Path.Combine(savingPath, saVingFileName);
            file.SaveAs(uploadFilePathAndName);

            return "Uploads/CreateSegment/Project/" + saVingFileName;
        }


        public ActionResult GetSegmentProjectDataList(string searchText)
        {
            int totalRows = 0;

            IList<SPCreateSegmentProjectList> segmentProjectList = _projectSegmentApplicationService.GetSegmentProjectListBySearch(searchText, 0,0);

            totalRows = segmentProjectList.Count;

            var result = new DataSourceResult()
            {
                Data = segmentProjectList,
                Total = totalRows  // Total number of records
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSegmentProjectNameList()
        {
            int totalRows = 0;

            IList<CreateSegmentProject> segmentProjectList = _projectSegmentApplicationService.GetSegmentProjectNameList();
            
            return Json(segmentProjectList, JsonRequestBehavior.AllowGet);
        }


    }
}