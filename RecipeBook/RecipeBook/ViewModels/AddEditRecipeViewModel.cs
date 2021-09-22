using Acr.UserDialogs;
using RecipeBook.Models;
using RecipeBook.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class AddEditRecipeViewModel : BaseViewModel
    {
        public AddEditRecipeViewModel(INavigation navigation, Recipe recipe)
        {
            Navigation = navigation;

            AddNewMakingStepCommand = new Command(AddNewMakingStepAction);
            AddNewIngredientCommand = new Command(AddNewIngredientAction);
            ManageMakingStepCommand = new Command<MakingStep>(ManageMakingStepAction);
            ManageIngredientCommand = new Command<Ingredient>(ManageIngredientAction);
            ManagePictureCommand = new Command(ManagePictureAction);
            RemovePictureCommand = new Command(RemovePictureAction);
            SaveCommand = new Command(SaveAction);

            if (recipe is null)
            {
                Recipe = new Recipe();
                Title = "Nowy przepis";
                Recipe.InitializeLists();
                Recipe.Ingredients.Add(new Ingredient()
                {
                    IngredientId = 1,
                    Name = "Cebula",
                    Number = 1,
                    UnitId = 1,
                    Quantity = 2
                });
                Recipe.Ingredients.Add(new Ingredient()
                {
                    IngredientId = 2,
                    Name = "Masło",
                    Number = 2
                });

                Recipe.MakingSteps.Add(new MakingStep()
                {
                    MakingStepId = 1,
                    Number = 1,
                    Name = "Pokroić mięso w kostkę"
                });
                
                Recipe.MakingSteps.Add(new MakingStep()
                {
                    MakingStepId = 2,
                    Number = 2,
                    Name = "Obrać cebulę"
                });
            }
            else
            {
                Recipe = recipe;
                Title = "Edycja przepisu";
            }
        }

        public ICommand AddNewMakingStepCommand { get; set; }
        public ICommand AddNewIngredientCommand { get; set; }
        public ICommand ManageMakingStepCommand { get; set; }
        public ICommand ManageIngredientCommand { get; set; }
        public ICommand ManagePictureCommand { get; set; }
        public ICommand RemovePictureCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public Recipe Recipe
        {
            get => _Recipe;
            set { _Recipe = value; OnPropertyChanged("Recipe"); }
        }
        private Recipe _Recipe;

        public void AddNewMakingStepAction()
        {
            UserDialogs.Instance.Alert("Siema");
        }
        
        public async void AddNewIngredientAction()
        {
            Ingredient ingredient = await Navigation.ShowPopupAsync(new AddEditIngredientPopup(null));

            if (ingredient is null)
                return;

            ingredient.Number = Recipe.Ingredients.Count + 1;

            Recipe.Ingredients.Add(ingredient);
        }

        public async void ManageMakingStepAction(MakingStep makingStep)
        {
            try
            {
                if (makingStep is null)
                    return;

                bool result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = $"Czy na pewno chcesz usunąć krok \"{makingStep.Name}\"?",
                    OkText = "Tak",
                    CancelText = "Anuluj",
                    Title = "Potwierdzenie",
                    AndroidStyleId = 2131689474
                });

                if (!result)
                    return;

                Recipe.DeleteMakingStep(makingStep);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }

        public async void ManageIngredientAction(Ingredient ingredient)
        {
            try
            {
                if (ingredient is null)
                    return;

                bool result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = $"Czy na pewno chcesz usunąć składnik \"{ingredient.Name}\"?",
                    OkText = "Tak",
                    CancelText = "Anuluj",
                    Title = "Potwierdzenie",
                    AndroidStyleId = 2131689474
                });

                if (!result)
                    return;

                Recipe.DeleteIngredient(ingredient);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        public async void ManagePictureAction()
        {
            try
            {
                FileResult fileResult = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Wybierz zdjęcie"
                });

                if (fileResult is null)
                    return;

                Recipe.PictureRaw = PictureConverter.ImagePathToBase64(fileResult.FullPath);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        public async void RemovePictureAction()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Recipe.PictureRaw))
                    return;

                bool result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = "Czy na pewno chcesz usunąć obraz?",
                    OkText = "Tak",
                    CancelText = "Anuluj",
                    Title = "Potwierdzenie",
                    AndroidStyleId = 2131689474
                });

                if (!result)
                    return;

                Recipe.PictureRaw = string.Empty;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        public void SaveAction()
        {
            try
            {
                UserDialogs.Instance.Alert("Działa");
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
    }
}
