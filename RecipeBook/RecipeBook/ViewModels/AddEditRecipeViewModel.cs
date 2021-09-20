using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class AddEditRecipeViewModel : BaseViewModel
    {
        public AddEditRecipeViewModel(INavigation navigation, Recipe recipe)
        {
            Navigation = navigation;

            if (recipe is null)
            {
                Recipe = new Recipe();
                Title = "Nowy przepis";
            }
            else
            {
                Recipe = recipe;
                Title = "Edycja przepisu";
            }
        }

        public Recipe Recipe
        {
            get => _Recipe;
            set { _Recipe = value; OnPropertyChanged("Recipe"); }
        }
        private Recipe _Recipe;
    }
}
