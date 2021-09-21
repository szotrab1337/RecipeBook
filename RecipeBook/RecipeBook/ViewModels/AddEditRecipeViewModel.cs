using Acr.UserDialogs;
using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
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
        
        public void AddNewIngredientAction()
        {
            UserDialogs.Instance.Alert("Siema");
        }

        public void ManageMakingStepAction(MakingStep makingStep)
        {
            UserDialogs.Instance.Alert(makingStep.Name);
        }
        
        public void ManageIngredientAction(Ingredient ingredient)
        {
            UserDialogs.Instance.Alert(ingredient.Name);
        }
        
        public void ManagePictureAction()
        {
            UserDialogs.Instance.Alert("siema fotka");
        }
    }
}
