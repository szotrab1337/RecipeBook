using Acr.UserDialogs;
using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class RecipesViewModel : BaseViewModel
    {
        public RecipesViewModel(INavigation navigation)
        {
            Navigation = navigation;

            RefreshCommand = new Command(() => LoadRecipes());
            ChangeFavouriteStateCommand = new Command<Recipe>(ChangeFavouriteStateAction);
            DeleteRecipeCommand = new Command<Recipe>(DeleteRecipeAction);

            SearchResult = string.Empty;
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand ChangeFavouriteStateCommand { get; set; }
        public ICommand DeleteRecipeCommand { get; set; }

        public ObservableCollection<Recipe> Recipes
        {
            get => _Recipes;
            set { _Recipes = value; OnPropertyChanged("Recipes"); }
        }
        private ObservableCollection<Recipe> _Recipes;
        
        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set { _IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }
        private bool _IsRefreshing;
        
        public string SearchResult
        {
            get => _SearchResult;
            set
            {
                _SearchResult = value; OnPropertyChanged("SearchResult");

                LoadRecipes();
            }
        }
        private string _SearchResult;

        private async void LoadRecipes()
        {
            try
            {
                IsRefreshing = true;
                Recipes = new ObservableCollection<Recipe>(await App.Database.GetRecipes(SearchResult.ToLower()));
                IsRefreshing = false;
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }

        private async void ChangeFavouriteStateAction(Recipe clickedRecipe)
        {
            try
            {
                if (clickedRecipe is null)
                    return;

                Recipe recipe = Recipes.FirstOrDefault(x => x.RecipeId == clickedRecipe.RecipeId);
                recipe.IsFavourite = !recipe.IsFavourite;

                await App.Database.UpdateRecipe(recipe);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }

        private async void DeleteRecipeAction(Recipe clickedRecipe)
        {
            try
            {
                if (clickedRecipe is null)
                    return;

                bool result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = $"Czy na pewno chcesz usunąć przepis \"{clickedRecipe.Name}\"?",
                    OkText = "Tak",
                    CancelText = "Nie",
                    Title = "Potwierdzenie",
                    AndroidStyleId = 2131689474
                });

                if (!result)
                    return;

                clickedRecipe.DeleteAssociatedItems();

                Recipes.Remove(clickedRecipe);
                await App.Database.DeleteRecipe(clickedRecipe);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
    }
}
