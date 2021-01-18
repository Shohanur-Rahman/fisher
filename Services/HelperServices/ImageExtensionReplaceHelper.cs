using System;
using System.Collections.Generic;
using System.Text;

namespace Services.HelperServices
{
    public class ImageExtensionReplaceHelper
    {
        public static string ImageExtensionRemover(string stringValue)
        {
            if (stringValue != null)
            {
                stringValue = stringValue.Replace(".GIF", "");
                stringValue = stringValue.Replace(".gif", "");
                stringValue = stringValue.Replace(".IMG", "");
                stringValue = stringValue.Replace(".img", "");
                stringValue = stringValue.Replace(".JPE", "");
                stringValue = stringValue.Replace(".jpe", "");
                stringValue = stringValue.Replace(".JPEG", "");
                stringValue = stringValue.Replace(".jpeg", "");
                stringValue = stringValue.Replace(".JPG", "");
                stringValue = stringValue.Replace(".jpg", "");
                stringValue = stringValue.Replace(".PNG", "");
                stringValue = stringValue.Replace(".png", "");
                stringValue = stringValue.Replace(".PGM", "");
                stringValue = stringValue.Replace(".pgm", "");
                stringValue = stringValue.Replace(".JBG", "");
                stringValue = stringValue.Replace(".jbg", "");
                stringValue = stringValue.Replace(".PSD", "");
                stringValue = stringValue.Replace(".psd", "");
                stringValue = stringValue.Replace(".TGA", "");
                stringValue = stringValue.Replace(".tga", "");
                stringValue = stringValue.Replace(".TIFF", "");
                stringValue = stringValue.Replace(".tiff", "");
                stringValue = stringValue.Replace(".WMF", "");
                stringValue = stringValue.Replace(".wmf", "");
            }

            return stringValue;

        }
    }
}




