using Acr.UserDialogs;
using Newtonsoft.Json;
using RecipeBook.Models;
using RecipeBook.Models.Api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class ImportRecipeViewModel : BaseViewModel
    {
        public ImportRecipeViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "Importowanie";

            RefreshCommand = new Command(() =>
            {
                UserDialogs.Instance.ShowLoading("Trwa importowanie...", MaskType.Black);
                LoadRecipes();
                UserDialogs.Instance.HideLoading();
                IsRefreshing = false;
            });
            DeleteRecipeCommand = new Command<ApiRecipe>(DeleteRecipeAction);
            ImportRecipeCommand = new Command<ApiRecipe>(ImportRecipeAction);

            LoadRecipes();
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand DeleteRecipeCommand { get; set; }
        public ICommand ImportRecipeCommand { get; set; }

        public ObservableCollection<ApiRecipe> Recipes
        {
            get => _Recipes;
            set { _Recipes = value; OnPropertyChanged("Recipes"); }
        }
        private ObservableCollection<ApiRecipe> _Recipes;

        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set { _IsRefreshing = value; OnPropertyChanged("IsRefreshing"); }
        }
        private bool _IsRefreshing;

        private void LoadRecipes()
        {
            try
            {
                List<ApiRecipe> recipes = JsonConvert.DeserializeObject<List<ApiRecipe>>(ApiAdapter.GetRecipes());
                Recipes = new ObservableCollection<ApiRecipe>(recipes.OrderBy(x => x.Name));
            }
            catch
            {
                UserDialogs.Instance.Alert("Nie udało się pobrać list przepisów. " +
                    "Sprawdź czy jesteś w lokalnej sieci domowej oraz czy komputer stacjonarny jest włączony.", "Błąd", "OK");
                Recipes = new ObservableCollection<ApiRecipe>();
            }
        }

        private async void DeleteRecipeAction(ApiRecipe recipe)
        {
            try
            {
                bool result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = "Czy na pewno chcesz usunąć przepis \"" + recipe.Name + "\"?",
                    OkText = "Tak",
                    CancelText = "Nie",
                    Title = "Potwierdzenie",
                    AndroidStyleId = 2131689474
                });

                if (!result)
                    return;

                ApiAdapter.DeleteRecipe(recipe.RecipeId.ToString());
                Recipes.Remove(recipe);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        private async void ImportRecipeAction(ApiRecipe recipe)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Trwa importowanie...", MaskType.Black);

                Recipe recipeDB = await App.Database.GetRecipe(recipe.Name);

                Recipe newRecipe = new Recipe()
                {
                    Name = recipeDB != null ? recipe.Name + "_Imp_" + recipe.RecipeId.ToString() : recipe.Name,
                    CreatedOn = DateTime.Now,
                    Hints = recipe.Hints,
                    IsFavourite = false,
                    NumberOfServings = recipe.NumberOfServings,
                    PictureRaw = recipe.PictureRaw,
                    Source = recipe.Source,
                    TimeOfMakingTheRecipe = TimeSpan.FromTicks(recipe.TimeOfMakingTheRecipe)
                };

                await App.Database.InsertRecipe(newRecipe);
                recipeDB = await App.Database.GetRecipe(newRecipe.Name);

                List<Ingredient> ingredients = new List<Ingredient>();

                foreach (ApiIngredient ingredient in recipe.Ingredients)
                {
                    ingredients.Add(new Ingredient()
                    {
                        RecipeId = recipeDB.RecipeId,
                        Name = ingredient.Name,
                        Quantity = ingredient.Quantity,
                        UnitId = ingredient.UnitId,
                        Number = ingredient.Number
                    });
                }

                foreach (Ingredient ingredient in ingredients)
                {
                    await App.Database.InsertIngredient(ingredient);
                }

                List<MakingStep> makingSteps = new List<MakingStep>();

                foreach (ApiMakingStep makingStep in recipe.MakingSteps)
                {
                    makingSteps.Add(new MakingStep()
                    {
                        RecipeId = recipeDB.RecipeId,
                        Name = makingStep.Name,
                        Number = makingStep.Number
                    });
                }

                foreach (MakingStep makingStep in makingSteps)
                {
                    await App.Database.InsertMakingStep(makingStep);
                }

                UserDialogs.Instance.HideLoading();
                MessagingCenter.Send(this, "RefreshRecipes");
                UserDialogs.Instance.Toast("Import zakończony powodzeniem.");
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
    }
}
