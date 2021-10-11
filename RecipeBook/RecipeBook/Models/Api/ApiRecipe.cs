using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RecipeBook.Models.Api
{
    public class ApiRecipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Hints { get; set; }
        public long TimeOfMakingTheRecipe { get; set; }
        public double NumberOfServings { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PictureRaw { get; set; }
        public string Source { get; set; }

        public virtual List<ApiMakingStep> MakingSteps { get; set; }
        public virtual List<ApiIngredient> Ingredients { get; set; }

        public ImageSource Picture => string.IsNullOrEmpty(PictureRaw) ? ImageSource.FromResource("RecipeBook.Images.placeholder.png") :
                                        PictureConverter.Base64ToImage(PictureRaw);

        public bool IsDefaultPicture => string.IsNullOrEmpty(PictureRaw) ? true : false;

        public string FormattedTimeOfMakingTheRecipe => TimeSpan.FromTicks(TimeOfMakingTheRecipe).TotalMinutes + " min";

        public string FormattedCreatedOn => CreatedOn.ToString(@"dd.MM.yyyy");
    }
}
