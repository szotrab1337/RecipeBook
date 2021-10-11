using Acr.UserDialogs;
using RecipeBook.Models;
using RecipeBook.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
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
            OpenRecipeCommand = new Command<Recipe>(OpenRecipeAction);
            EditRecipeCommand = new Command<Recipe>(EditRecipeAction);
            AddRecipeCommand = new Command(AddRecipeAction);
            ShareRecipeCommand = new Command<Recipe>(ShareRecipeAction);
            MessagingCenter.Subscribe<AddEditRecipeViewModel>(this, "RefreshAllRecipes", (LoadAgain) => { LoadRecipes(); });
            MessagingCenter.Subscribe<FavouriteRecipesViewModel>(this, "RefreshRecipes", (LoadAgain) => { LoadRecipes(); });
            MessagingCenter.Subscribe<ImportRecipeViewModel>(this, "RefreshRecipes", (LoadAgain) => { LoadRecipes(); });

            SearchResult = string.Empty;
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand ChangeFavouriteStateCommand { get; set; }
        public ICommand DeleteRecipeCommand { get; set; }
        public ICommand OpenRecipeCommand { get; set; }
        public ICommand EditRecipeCommand { get; set; }
        public ICommand AddRecipeCommand { get; set; }
        public ICommand ShareRecipeCommand { get; set; }

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
                UserDialogs.Instance.ShowLoading("Trwa importowanie...", MaskType.Black);
                Recipes = new ObservableCollection<Recipe>(await App.Database.GetRecipes(SearchResult.ToLower()));
                UserDialogs.Instance.HideLoading();
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
                MessagingCenter.Send(this, "RefreshFavouriteRecipes");
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
                MessagingCenter.Send(this, "RefreshFavouriteRecipes");
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }

        private async void OpenRecipeAction(Recipe clickedRecipe)
        {
            try
            {
                if (clickedRecipe is null)
                    return;

                await Navigation.PushAsync(new RecipeDetailsPage(clickedRecipe));
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        private async void EditRecipeAction(Recipe clickedRecipe)
        {
            try
            {
                if (clickedRecipe is null)
                    return;

                await Navigation.PushAsync(new AddEditRecipePage(clickedRecipe));
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        private async void AddRecipeAction()
        {
            try
            {
                await Navigation.PushAsync(new AddEditRecipePage(null));
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        private async void ShareRecipeAction(Recipe recipe)
        {
            try
            {
                List<string> choices = new List<string>
                {
                    "Zapisz na stronie WWW",
                    "Udostępnij jako tekst"
                };

                string result = await UserDialogs.Instance.ActionSheetAsync("Wybierz...", string.Empty, "Anuluj", CancellationToken.None, choices.ToArray());

                if (result.Equals("Anuluj") || string.IsNullOrWhiteSpace(result))
                    return;

                if (result.Equals("Zapisz na stronie WWW"))
                {
                    UserDialogs.Instance.ShowLoading("Trwa eksportowanie...", MaskType.Black);
                    recipe.ExportToWebsite();
                    UserDialogs.Instance.HideLoading();
                    UserDialogs.Instance.Toast("Eksport zakończony powodzeniem.");
                }
                
                if (result.Equals("Udostępnij jako tekst"))
                {
                    string message = await recipe.ShareRecipeAsText();

                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Text = message,
                        Title = "Udostępnianie przepisu"
                    });
                }
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert(ex.ToString() + " Nie udało się wykonać eksportu. " +
                    "Sprawdź czy jesteś w lokalnej sieci domowej oraz czy komputer stacjonarny jest włączony.", "Błąd", "OK");
            }
        }
    }
}
