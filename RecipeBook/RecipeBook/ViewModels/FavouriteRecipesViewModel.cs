using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class FavouriteRecipesViewModel : BaseViewModel
    {
        public FavouriteRecipesViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
