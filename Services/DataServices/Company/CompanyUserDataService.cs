using Core.DataService;
using Services.AppServices;
using Services.DataContext;
using Services.Domain;
using Services.Domain.Models.User;
using Services.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DataServices.Company
{
   public  class CompanyUserDataService : EntityContextDataService<User>
    {
        public CompanyUserDataService(AppDbContext dbContext) : base(dbContext)
        {
            DbContext.Database.CommandTimeout = SiteSettings.DbTimeOut;
        }
        public CompanyUserModel ProcessCompanyUserByCompanyAdminWithAllInfo(CompanyUserModel companyUserModel)
        {
            CompanyUserModel operationData = null;

            if (companyUserModel != null && companyUserModel.IsAddOperation == true)
            {
                User user = new User();

                string encryptNewPass = SimpleCryptService.Factory().Encrypt(companyUserModel.Password);
                string encryptNewConfirmPass = SimpleCryptService.Factory().Encrypt(companyUserModel.ConfirmPassword);

                user.FirstName = companyUserModel.FirstName;
                user.LastName = companyUserModel.LastName;
                user.UserName = companyUserModel.EmailAddress;
                user.EmailAddress = companyUserModel.EmailAddress;
                user.Position = companyUserModel.Position;
                user.Password = encryptNewPass;
                user.ConfirmPassword = encryptNewConfirmPass;
                user.UserRegistartionGuid = Guid.NewGuid().ToString();
                user.IsImageUploadedByUser = false;
                user.UserCreatedBy = Services.Domain.Models.Role.Enums.Role.CompanyAdmin.ToString();
                user.Role = "User";
                user.AreaID = companyUserModel.AreaId;
                user.ProjectID = companyUserModel.ProjectID;                

                if (companyUserModel.UserImagePath != null)
                {
                    user.UserImagePath = companyUserModel.UserImagePath;
                }
                if (companyUserModel.UserImageCaption != null)
                {
                    user.UserImageCaption = companyUserModel.UserImageCaption;
                }
                user.CreatedDate = DateTime.Now;
                user.EditedDate = DateTime.Now;                
                user.IsTrialUser = false;               
                if (companyUserModel.IsUserActivated == "True")
                {
                    user.IsActivated = true;
                }
                if (companyUserModel.IsUserActivated == "False")
                {
                    user.IsActivated = false;
                }
                user.RegistrationConfirmed = "1";
                DbContext.Set<User>().Add(user);
                DbContext.SaveChanges();
                

                CompanyUserModel operationResult = new CompanyUserModel();
                operationResult.FirstName = user.FirstName;
                operationResult.LastName = user.LastName;
                operationResult.UserName = user.UserName;
                operationResult.EmailAddress = user.EmailAddress;
                operationResult.Password = SimpleCryptService.Factory().Decrypt(user.Password);

                return operationResult;

                //user.CompanyUsersCompanyID = companyAdminId;
                //user.HasReviewOptionForUser = companyUserModel.HasReviewOptionForUser;
                //user.CreatedIP = IPAndBrowserDetectionHelper.GetClientIPAddress();
                //  user.CreatedBy = companyAdminId;
                // user.EditedBy = companyAdminId;
                //user.Role = companyUserModel.UserRole;

            }

            if (companyUserModel != null && companyUserModel.IsAddOperation == false)
            {

                User user = DbContext.Set<User>().Where(x => x.UserID == companyUserModel.UserID ).FirstOrDefault();

                string encryptNewPass = SimpleCryptService.Factory().Encrypt(companyUserModel.Password);
                string encryptNewConfirmPass = SimpleCryptService.Factory().Encrypt(companyUserModel.ConfirmPassword);

                user.FirstName = companyUserModel.FirstName;
                user.LastName = companyUserModel.LastName;
                user.UserName = companyUserModel.EmailAddress;
                user.EmailAddress = companyUserModel.EmailAddress;
                user.Position = companyUserModel.Position;
                user.Password = encryptNewPass;
                user.ConfirmPassword = encryptNewConfirmPass;
                user.UserRegistartionGuid = Guid.NewGuid().ToString();
                user.IsImageUploadedByUser = false;
               // user.CompanyUsersCompanyID = companyAdminId;
              //  user.HasReviewOptionForUser = companyUserModel.HasReviewOptionForUser;
               // user.EditedIP = IPAndBrowserDetectionHelper.GetClientIPAddress();
                user.EditedDate = DateTime.Now;
                if (companyUserModel.UserImagePath != null)
                {
                    user.UserImagePath = companyUserModel.UserImagePath;
                }
                if (companyUserModel.UserImageCaption != null)
                {
                    user.UserImageCaption = companyUserModel.UserImageCaption;
                }

              //  user.EditedBy = companyAdminId;
                user.Role = companyUserModel.UserRole;

                if (companyUserModel.IsUserActivated == "True")
                {
                    user.IsActivated = true;
                }
                if (companyUserModel.IsUserActivated == "False")
                {
                    user.IsActivated = false;
                }

                DbContext.SaveChanges();

              
                CompanyUserModel operationResult = new CompanyUserModel();
                operationResult.FirstName = user.FirstName;
                operationResult.LastName = user.LastName;
                operationResult.UserName = user.UserName;
                operationResult.EmailAddress = user.EmailAddress;
                operationResult.Password = SimpleCryptService.Factory().Decrypt(user.Password);

                return operationResult;

            }

            return operationData;

        }
        public IList<SPUserSearchListResult> GetUserSearchList(string searchName)
        {


            SqlParameter prmSearchText = new SqlParameter("@searchText", searchName);

            List<SPUserSearchListResult> userList = DbContext.Database.SqlQuery<SPUserSearchListResult>("up_GetUserListBySearchParam @searchText", prmSearchText).ToList();

            return userList;
        }


        public CompanyUserModel GetUserDetailsByUserId(int Id)
        {
            User user = DbContext.Set<User>().Where(x => x.UserID == Id).FirstOrDefault();
            CompanyUserModel operationResult = new CompanyUserModel();
            operationResult.UserID = user.UserID;
            operationResult.FirstName = user.FirstName;
            operationResult.LastName = user.LastName;
            operationResult.UserName = user.EmailAddress;
            operationResult.EmailAddress = user.EmailAddress;
            operationResult.Position = user.Position;
            operationResult.ProjectID = user.ProjectID;
            operationResult.AreaId = user.AreaID == null ? 0 : user.AreaID.Value;
            operationResult.IsUserActivated = user.IsActivated == true ? "True": "False";
            operationResult.UserImageCaption = user.UserImageCaption;
            operationResult.UserImagePath = user.UserImagePath;
            operationResult.Password = SimpleCryptService.Factory().Decrypt(user.Password);
            operationResult.ConfirmPassword = SimpleCryptService.Factory().Decrypt(user.ConfirmPassword);
            return operationResult;
        }

        public IList<SPFishSellingChartReport> GetAdminDashBoardFishSellingReport()
        {
            List<SPFishSellingChartReport> sellingReportList = DbContext.Database.SqlQuery<SPFishSellingChartReport>("up_GetFishSellingChartReport").ToList();

            return sellingReportList;
        }



        public int DeleteUser(int id)
        {
            int result = DbContext.Database.ExecuteSqlCommand("delete users where UserID = " + id);
            return result;
        }

        public SPAdminDashBoardOveriew GetAdminDashBoardOverview()
        {
            SPAdminDashBoardOveriew sellingReportList = DbContext.Database.SqlQuery<SPAdminDashBoardOveriew>("up_GetAdminDashBoardOveriew").FirstOrDefault();

            return sellingReportList;
        }

    }
}
