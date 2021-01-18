

using System;
using System.Collections.Generic;

namespace Web.AppCode
{
    public class DownloadStratedIndicator 
    {

      public static Dictionary<long, string> UserSettings = new Dictionary<long, string>();

        public static string CheekDownloadStartedConfirmation(long userID)
        {
            if (UserSettings.ContainsKey(userID))
            {
              var value = UserSettings[userID];
                if (value != null)
                {
                    if (value == "DownloadStarted")
                        return "DownloadStarted";
                }
            }
      
         
            return "-1";

        }

        public static void SetDownloadStartedConfirmation(long userID)
        {    
           UserSettings[userID] = "DownloadStarted";    
        }

        public static void SetPreviewStartedConfirmation(long userID)
        {
            UserSettings[userID] = "PreviewStarted";
        }

        public static string DownloadStartedConfirmation(long userID)
        {


            //System.Threading.Tasks.Task<string> result = AzureStorageService.ReadFileFromAzureStorageAsText(AzureBlobFolderName.SETTINGS_FOLDER_NAME, userID.ToString() + ".txt");

            //if (result != null )
            //{

            //   // if (result.Result == "DownloadStarted")
            //        return "DownloadStarted";
            //}
            //return "-1";

            if (UserSettings.ContainsKey(userID))
            {
                var value = UserSettings[userID];
                if (value != null)
                {
                    if (value == "DownloadStarted")
                        return "DownloadStarted";
                }
            }


            return "-1";

        }

        public static void RemoveDownloadStartedIndicator(long userID)
        {
          
            UserSettings[userID] = "";

            //System.Threading.Tasks.Task<string> result = AzureStorageService.ReadFileFromAzureStorageAsText(AzureBlobFolderName.SETTINGS_FOLDER_NAME, userID.ToString() + ".txt");

            //if (result != null )
            //{
            //    byte[] fileDataBytes = System.Text.Encoding.Unicode.GetBytes("");
            //    System.Threading.Tasks.Task<bool> emptyValueResult = AzureStorageService.UploadFileInAzureStorageAsync(fileDataBytes, AzureBlobFolderName.SETTINGS_FOLDER_NAME, userID.ToString() + ".txt");

            //}


        }

        public static string PreviewStartedConfirmation(long userID)
        {


            //System.Threading.Tasks.Task<string> result = AzureStorageService.ReadFileFromAzureStorageAsText(AzureBlobFolderName.SETTINGS_FOLDER_NAME, userID.ToString() + ".txt");

            //if (result != null )
            //{

            //    // if (result.Result == "DownloadStarted")
            //    return "PreviewStarted";
            //}
            //return "-1";

            if (UserSettings.ContainsKey(userID))
            {
                var value = UserSettings[userID];
                if (value != null)
                {
                    if (value == "PreviewStarted")
                        return "PreviewStarted";
                }
            }


            return "-1";

        }

        public static void RemovePreviewStartedIndicator(long userID)
        {


            //System.Threading.Tasks.Task<string> result = AzureStorageService.ReadFileFromAzureStorageAsText(AzureBlobFolderName.SETTINGS_FOLDER_NAME, userID.ToString() + ".txt");

            //if (result != null )
            //{
            //    byte[] fileDataBytes = System.Text.Encoding.Unicode.GetBytes("");
            //    System.Threading.Tasks.Task<bool> emptyValueResult = AzureStorageService.UploadFileInAzureStorageAsync(fileDataBytes, AzureBlobFolderName.SETTINGS_FOLDER_NAME, userID.ToString() + ".txt");

            //}
            UserSettings[userID] = "";


        }

    }
}
