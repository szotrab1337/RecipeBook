using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class RecipeDetailsViewModel : BaseViewModel
    {
        public RecipeDetailsViewModel(INavigation navigation, Recipe recipe)
        {
            Navigation = navigation;
            Recipe = recipe;
            Title = "Szczegóły przepisu";
            Recipe.LoadAssociatedItems();
        }

        public Recipe Recipe
        {
            get => _Recipe;
            set { _Recipe = value; OnPropertyChanged("Recipe"); }
        }
        private Recipe _Recipe;
    }
}
