using Acr.UserDialogs;
using RecipeBook.Models;
using RecipeBook.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            SaveCommand = new Command(SaveAction);
            ConfirmLeaveCommand = new Command(ConfirmLeaveAction);

            if (recipe is null)
            {
                Recipe = new Recipe();
                Title = "Nowy przepis";
                Recipe.InitializeLists();
            }
            else
            {
                Recipe = recipe;
                Title = "Edycja przepisu";
                Recipe.LoadAssociatedItems();
                Recipe.PrepareToEdit();
            }
        }

        public ICommand AddNewMakingStepCommand { get; set; }
        public ICommand AddNewIngredientCommand { get; set; }
        public ICommand ManageMakingStepCommand { get; set; }
        public ICommand ManageIngredientCommand { get; set; }
        public ICommand ManagePictureCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ConfirmLeaveCommand { get; set; }

        public Recipe Recipe
        {
            get => _Recipe;
            set { _Recipe = value; OnPropertyChanged("Recipe"); }
        }
        private Recipe _Recipe;

        private async void AddNewMakingStepAction()
        {
            MakingStep makingStep = await Navigation.ShowPopupAsync(new AddEditMakingStepPopup(null));

            if (makingStep is null)
                return;

            makingStep.Number = Recipe.MakingSteps.Count + 1;

            Recipe.AddMakingStep(makingStep);
        }
        
        private async void AddNewIngredientAction()
        {
            Ingredient ingredient = await Navigation.ShowPopupAsync(new AddEditIngredientPopup(null));

            if (ingredient is null)
                return;

            ingredient.Number = Recipe.Ingredients.Count + 1;

            Recipe.AddIngredient(ingredient);
        }

        private async void ManageMakingStepAction(MakingStep makingStep)
        {
            try
            {
                if (makingStep is null)
                    return;

                List<string> choices = new List<string>
                {
                    "Edytuj",
                    "Usuń"
                };

                string result = await UserDialogs.Instance.ActionSheetAsync("Wybierz...", string.Empty, "Anuluj", CancellationToken.None, choices.ToArray());

                if (result.Equals("Anuluj") || string.IsNullOrWhiteSpace(result))
                    return;

                if (result.Equals("Edytuj"))
                    EditMakingStep(makingStep);

                if (result.Equals("Usuń"))
                    RemoveMakingStep(makingStep);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }

        private async void ManageIngredientAction(Ingredient ingredient)
        {
            try
            {
                if (ingredient is null)
                    return;

                List<string> choices = new List<string>
                {
                    "Edytuj",
                    "Usuń"
                };

                string result = await UserDialogs.Instance.ActionSheetAsync("Wybierz...", string.Empty, "Anuluj", CancellationToken.None, choices.ToArray());

                if (result.Equals("Anuluj") || string.IsNullOrWhiteSpace(result))
                    return;

                if (result.Equals("Edytuj"))
                    EditIngredient(ingredient);

                if (result.Equals("Usuń"))
                    RemoveIngredient(ingredient);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }

        private async void RemoveIngredient(Ingredient ingredient)
        {
            try
            {
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
        
        private async void RemoveMakingStep(MakingStep makingStep)
        {
            try
            {
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

        private async void EditIngredient(Ingredient ingredient)
        {
            try
            {
                ingredient = await Navigation.ShowPopupAsync(new AddEditIngredientPopup(ingredient));

                Ingredient newIngredient = Recipe.Ingredients.FirstOrDefault(x => x.IngredientId == ingredient.IngredientId);
                newIngredient = ingredient;

                Recipe.UpdateIngredient(newIngredient);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        private async void EditMakingStep(MakingStep makingStep)
        {
            try
            {
                makingStep = await Navigation.ShowPopupAsync(new AddEditMakingStepPopup(makingStep));

                MakingStep newMakingStep = Recipe.MakingSteps.FirstOrDefault(x => x.MakingStepId == makingStep.MakingStepId);
                newMakingStep = makingStep;

                Recipe.UpdateMakingStep(makingStep);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        private async void ManagePictureAction()
        {
            try
            {
                List<string> choices = new List<string>
                {
                    "Wybierz zdjęcie"
                };

                if (!Recipe.IsDefaultPicture)
                    choices.Add("Usuń zdjęcie");

                string result = await UserDialogs.Instance.ActionSheetAsync("Wybierz...", string.Empty, "Anuluj", CancellationToken.None, choices.ToArray());

                if (result.Equals("Anuluj") || string.IsNullOrWhiteSpace(result))
                    return;

                if (result.Equals("Wybierz zdjęcie"))
                    PickNewPicture();

                if (result.Equals("Usuń zdjęcie"))
                    RemovePicture();       
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }

        private async void PickNewPicture()
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

        private async void RemovePicture()
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
        
        private async void SaveAction()
        {
            try
            {
                if (Recipe.RecipeId > 0)
                    Recipe.UpdateRecipe();
                else
                {
                    await Recipe.AddNewRecipe();
                    MessagingCenter.Send(this, "RefreshRecipes");
                }

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
        
        private async void ConfirmLeaveAction()
        {
            try
            {
                bool result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = "Czy na pewno chcesz opuścić stronę? Wprowadzone zmiany nie zostaną zapisane.",
                    OkText = "Tak",
                    CancelText = "Anuluj",
                    Title = "Potwierdzenie",
                    AndroidStyleId = 2131689474
                });

                if (!result)
                    return;

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
    }
}
