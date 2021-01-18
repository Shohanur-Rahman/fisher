using System;
using System.Linq;
using Services.Domain;
using System.IO;
using Services.Domain.CreateSegement.Models.Fish;

namespace Services.DataContext
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            //It's use for keep all data that's created 1st build


            return;
            /*
           context.Database.EnsureDeleted();

           context.Database.EnsureCreated();

           //Insert Role Data
           GenerateRoleDataInDb(context);

           //Insert User Data 
           GenerateUserDataInDb(context);

           //Insert User company Data
           GenerateUserCopmanyDataInDb(context);

           //Insert Payments Data
           GeneratePaymentsInformationDataInDb(context);

           //Insert Locaton Of Use Data
           GenerateLocationOfUseDataInDb(context);

           //Insert Duration Of Exposure Data
           GenerateDurationOfExposureDataInDb(context);

           //Insert Frequency Of Use Data
           GenerateFrequencyOfUseDataInDb(context);

           //Insert Number Of Person Exposed Data
           GenerateNumberOfPersonsExposedDataInDb(context);

           //Insert Susceptible Workers Data
           GenerateSusceptibleWorkersDataInDb(context);

           //Insert COSHHSubstanceImages Data
           GenerateCOSHHSubstanceImagesDataInDb(context);

           //Insert Hazard Statement Data
           //GenerateHazardStatementsDataInDb(context);

           //Insert Precaution Data
           //GeneratePrecautionsDataInDb(context);

           //Insert COSHH Substance Data 
           GenerateCOSHHSubstanceDataInDb(context);

           //Insert COSHH Engineering Controls Data 
           GenerateCOSHHEngineeringControlsDataInDb(context);


           //COSHH Activity Contaminants Data
           GenerateCOSHHActivityContaminantsDataInDb(context);

           //Insert MethodType Data
           GenerateMethodTypeDataInDb(context);

           GenerateCOSHHActivityHealthSurveillancesDataInDb(context);

           InsertDataFromSQLScript(context);

           GenerateImageTypeDataInDb(context);

            GeneratePersonsAtRiskTypeDataInDb(context);
           */
        }


        private static void InsertDataFromSQLScript(AppDbContext context)
        {

            TimeSpan timeSpan = new TimeSpan(0, 50, 0);
            
            //context.Database.SetCommandTimeout(timeSpan);

            var sqlScriptFileList = Directory.GetFiles("SQL-Scripts", "*.sql").OrderBy(x => x);

            foreach (string sqlFile in sqlScriptFileList)
            {
                string sqlText = File.ReadAllText(sqlFile).Trim();
                string[] scriptTextSplit = sqlText.Split(new string[] { "GO\r\n", "GO ", "GO\t" }, StringSplitOptions.RemoveEmptyEntries);
                string appendString = "";

                foreach (string splitScript in scriptTextSplit)
                {
                    appendString += splitScript;

                }
                context.Database.ExecuteSqlCommand(appendString);
            }



        }


        private static void GenerateUserDataInDb(AppDbContext context)
        {
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            for (int counter = 0; counter < 2; counter++)
            {
                User userToInsert = new User();

                if (counter == 0)
                {
                    userToInsert.Role = "SuperAdmin";
                    userToInsert.FirstName = "Jhon";
                    userToInsert.LastName = "Sample";
                    userToInsert.UserName = "admin";
                    userToInsert.EmailAddress = "consultant.two007@gmail.com";
                    userToInsert.Password = "+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=";
                    userToInsert.ConfirmPassword = "4NgiGbh0Q+c67YiW1S6raQ==";
                    userToInsert.RegistrationConfirmed = "1";
                    userToInsert.AreaID = null;
                    userToInsert.ProjectID = 0;
                    userToInsert.UserImageCaption = "fde0f88c-bc83-4961-be11-da445e315964.png";
                    userToInsert.UserImagePath =
                        "~/Content/UploadImages/UserLogo/fde0f88c-bc83-4961-be11-da445e315964.png";
                    userToInsert.UserImageSize = 13851;
                    userToInsert.AddressOne = "Dhaka";
                    userToInsert.AddressTwo = "Dhaka";
                    userToInsert.Country = "Bangladesh";
                    userToInsert.Religion = "Islam";
                    userToInsert.City = "Dhaka";
                    userToInsert.PostCode = "1207";
                    userToInsert.PhoneNumber = "07737327171";
                    userToInsert.Position = "Administrator";
                    userToInsert.PublicationStatus = Convert.ToBoolean(1);
                    userToInsert.CreatedDate = DateTime.Parse("04-Apr-2017");
                    userToInsert.EditedDate = DateTime.Parse("14-Jun-2017");
                    userToInsert.CreatedBy = 1;
                    userToInsert.EditedBy = 1;
                    userToInsert.IsActivated = true;

                }

                if (counter == 1)
                {
                    userToInsert.Role = "User";
                    userToInsert.FirstName = "sample";
                    userToInsert.LastName = "User";
                    userToInsert.UserName = "FactionHSG";
                    userToInsert.EmailAddress = "frabbibd@yahoo.com";
                    userToInsert.Password = "+hdCdZpGjSbghDPURXUsPoGHmHJr2YbTHINkEkuILQ8=";
                    userToInsert.ConfirmPassword = "+hdCdZpGjSbghDPURXUsPolpWdM06AbB+t9bE0HYnbo=";
                    userToInsert.RegistrationConfirmed = "1";
                    userToInsert.AreaID = null;
                    userToInsert.ProjectID = 0;
                    userToInsert.UserImageCaption = "096a4432-cb32-4978-a7b4-190da60d8692.png";
                    userToInsert.UserImagePath =
                        "~/settings/096a4432-cb32-4978-a7b4-190da60d8692.png";
                    userToInsert.UserImageSize = 64776;
                    //userToInsert.UserImageSize = Int64.Parse("64776");
                    userToInsert.AddressOne = "";
                    userToInsert.AddressTwo = "";
                    userToInsert.Country = "United Kingdom";
                    userToInsert.Religion = "";
                    userToInsert.City = "";
                    userToInsert.PostCode = "";
                    userToInsert.PhoneNumber = "07342424242";
                    //userToInsert.CompanyName = "Paronsoft";
                    //userToInsert.CompanyAddress = "USA";
                    //userToInsert.CompanyWebsite = "psoft.net";
                    //userToInsert.CompanyPhone = "4242424";
                    userToInsert.Position = "Developer";
                    userToInsert.PublicationStatus = Convert.ToBoolean(1);
                    userToInsert.CreatedDate = DateTime.Parse("08-Aug-2017");
                    userToInsert.EditedDate = DateTime.Parse("08-Aug-2017");
                    userToInsert.CreatedBy = 1;
                    userToInsert.EditedBy = 1;
                    userToInsert.IsActivated = true;

                }
                context.Users.Add(userToInsert);
                context.SaveChanges();

            }

        }//end function



        private static void GenerateSegmentFishDataInDb(AppDbContext context)
        {
            if (context.CreateSegmentFish.Any())
            {
                return; // DB has been seeded
            }

            for (int counter = 0; counter < 2; counter++)
            {
                CreateSegmentFish fishToInsert = new CreateSegmentFish();

                if (counter == 0)
                {

                    fishToInsert.ID = 0;
                    fishToInsert.Name = "Test";
                    fishToInsert.ImageUrl = "Content/UploadImages/UserLogo/fde0f88c-bc83-4961-be11-da445e315964.png";
                    fishToInsert.Description = "Test";
                    fishToInsert.CreatedDate = DateTime.Now;
                    fishToInsert.CreatedId = 1;

                }

                if (counter == 1)
                {
                    fishToInsert.ID = 0;
                    fishToInsert.Name = "Test";
                    fishToInsert.ImageUrl = "Content/UploadImages/UserLogo/fde0f88c-bc83-4961-be11-da445e315964.png";
                    fishToInsert.Description = "Test";
                    fishToInsert.CreatedDate = DateTime.Now;
                    fishToInsert.CreatedId = 1;

                }
                context.CreateSegmentFish.Add(fishToInsert);
                context.SaveChanges();

            }

        }//end function


    }//end class


}//end namespace