using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace RecipeBook.Models
{
    public static class PictureConverter
    {
        public static ImageSource Base64ToImage(string imageBase64)
        {
            return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageBase64)));
        }

        public static string ImagePathToBase64(string imagePath)
        {
            byte[] bytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(bytes);
        }
    }
}
