using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class RecipesViewModel : BaseViewModel
    {
        public RecipesViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
