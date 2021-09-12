using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class RecipesViewModel : BaseViewModel
    {
        public RecipesViewModel(INavigation navigation)
        {
            Navigation = navigation;

            Recipes = new ObservableCollection<Recipe>();
            Recipes.Add(new Recipe());
            Recipes.Add(new Recipe());
            Recipes.Add(new Recipe());
        }

        public ObservableCollection<Recipe> Recipes
        {
            get => _Recipes;
            set { _Recipes = value; OnPropertyChanged("Recipes"); }
        }
        private ObservableCollection<Recipe> _Recipes;
    }
}
