using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RecipeBook.Models
{
    public class Recipe : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int RecipeId { get; set; }

        public string Name
        {
            get => _Name;
            set { _Name = value; OnPropertyChanged("Name"); }
        }
        private string _Name;
        
        public string Hints
        {
            get => _Hints;
            set { _Hints = value; OnPropertyChanged("Hints"); }
        }
        private string _Hints;
        
        public TimeSpan TimeOfMakingTheRecipe
        {
            get => _TimeOfMakingTheRecipe;
            set { _TimeOfMakingTheRecipe = value; OnPropertyChanged("TimeOfMakingTheRecipe"); }
        }
        private TimeSpan _TimeOfMakingTheRecipe;
        
        [Ignore]
        public string TimeOfMakingTheRecipeInput
        {
            get => _TimeOfMakingTheRecipeInput;
            set
            {
                _TimeOfMakingTheRecipeInput = value; OnPropertyChanged("TimeOfMakingTheRecipeInput"); OnPropertyChanged("FormattedTimeOfMakingTheRecipe");
                ValidateTimeOfMakingTheRecipeInput();
            }
        }
        private string _TimeOfMakingTheRecipeInput;
        
        public double NumberOfServings
        {
            get => _NumberOfServings;
            set { _NumberOfServings = value; OnPropertyChanged("NumberOfServings"); }
        }
        private double _NumberOfServings;
        
        public string NumberOfServingsInput
        {
            get => _NumberOfServingsInput;
            set
            {
                _NumberOfServingsInput = value; OnPropertyChanged("NumberOfServingsInput");
                ValidateServingsInput();
            }
        }
        private string _NumberOfServingsInput;
        
        public DateTime CreatedOn
        {
            get => _CreatedOn;
            set { _CreatedOn = value; OnPropertyChanged("CreatedOn"); }
        }
        private DateTime _CreatedOn;
        
        public string PictureRaw
        {
            get => _PictureRaw;
            set { _PictureRaw = value; OnPropertyChanged("PictureRaw"); OnPropertyChanged("Picture"); OnPropertyChanged("IsDefaultPicture"); }
        }
        private string _PictureRaw;

        [Ignore]
        public bool HintsAvailable => string.IsNullOrWhiteSpace(Hints) ? false : true;

        [Ignore]
        public string ValidateMessage { get; set; }

        [Ignore]
        public ImageSource Picture => string.IsNullOrEmpty(PictureRaw) ? ImageSource.FromResource("RecipeBook.Images.placeholder.png") :
                                        PictureConverter.Base64ToImage(PictureRaw);

        [Ignore]
        public bool IsDefaultPicture => string.IsNullOrEmpty(PictureRaw) ? true : false;

        public bool IsFavourite
        {
            get => _IsFavourite;
            set { _IsFavourite = value; OnPropertyChanged("IsFavourite"); }
        }
        private bool _IsFavourite;

        [Ignore]
        public ObservableCollection<Ingredient> Ingredients
        {
            get => _Ingredients;
            set { _Ingredients = value; OnPropertyChanged("Ingredients"); }
        }
        private ObservableCollection<Ingredient> _Ingredients;
        
        [Ignore]
        public ObservableCollection<MakingStep> MakingSteps
        {
            get => _MakingSteps;
            set { _MakingSteps = value; OnPropertyChanged("MakingSteps"); }
        }
        private ObservableCollection<MakingStep> _MakingSteps;

        [Ignore]
        public string FormattedTimeOfMakingTheRecipe => TimeOfMakingTheRecipe.TotalMinutes.ToString() + " min";

        [Ignore]
        public string FormattedCreatedOn => CreatedOn.ToString(@"dd.MM.yyyy");

        public void DeleteAssociatedItems()
        {
            DeleteMakingStepsList();
            DeleteIngredientsList();
        }

        public void LoadAssociatedItems()
        {
            LoadMakingStepsList();
            LoadIngredientsList();
        }

        public async void LoadMakingStepsList()
        {
            MakingSteps = new ObservableCollection<MakingStep>(await App.Database.GetMakingSteps(RecipeId));
        }
        
        public async void LoadIngredientsList()
        {
            Ingredients = new ObservableCollection<Ingredient>(await App.Database.GetIngredients(RecipeId));
        }
        
        public async void DeleteMakingStepsList()
        {
            await App.Database.DeleteMakingSteps(RecipeId);
        }
        
        public async void DeleteIngredientsList()
        {
            await App.Database.DeleteIngredients(RecipeId);
        }

        public Recipe()
        {
            CreatedOn = DateTime.Now;
            IsFavourite = false;
        }

        public void InitializeLists()
        {
            Ingredients = new ObservableCollection<Ingredient>();
            MakingSteps = new ObservableCollection<MakingStep>();
        }

        public async void DeleteIngredient(Ingredient ingredient)
        {
            if(RecipeId > 0)
                await App.Database.DeleteIngredient(ingredient);

            Ingredients.Remove(ingredient);
        }

        public async void DeleteMakingStep(MakingStep makingStep)
        {
            if(RecipeId > 0)
                await App.Database.DeleteMakingStep(makingStep);

            MakingSteps.Remove(makingStep);
        }
        
        public async void AddIngredient(Ingredient ingredient)
        {
            if(RecipeId > 0)
            {
                ingredient.RecipeId = RecipeId;
                await App.Database.InsertIngredient(ingredient);
            }

            Ingredients.Add(ingredient);
        }

        public async void AddMakingStep(MakingStep makingStep)
        {
            if(RecipeId > 0)
            {
                makingStep.RecipeId = RecipeId;
                await App.Database.InsertMakingStep(makingStep);
            }

            MakingSteps.Add(makingStep);
        }

        public async void UpdateMakingStep(MakingStep makingStep)
        {
            if (RecipeId <= 0)
                return;

            await App.Database.UpdateMakingStep(makingStep);
        }

        public async void UpdateIngredient(Ingredient ingredient)
        {
            if (RecipeId <= 0)
                return;

            await App.Database.UpdateIngredient(ingredient);
        }

        public bool ValidateRecipe()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ValidateMessage = "Nazwa nie może być pusta!";
                return false;
            }

            if (NumberOfServings <= 0)
            {
                ValidateMessage = "Wprowadź ilość porcji! To pole nie może być puste.";
                return false;
            }

            if(TimeOfMakingTheRecipe.TotalMinutes <= 0)
            {
                ValidateMessage = "Wprowadź czas wykonania! To pole nie może być puste.";
                return false;
            }

            return true;
        }

        public void PrepareToEdit()
        {
            NumberOfServingsInput = NumberOfServings.ToString();
            TimeOfMakingTheRecipeInput = TimeOfMakingTheRecipe.TotalMinutes.ToString();
        }

        private void ValidateServingsInput()
        {
            if (string.IsNullOrWhiteSpace(NumberOfServingsInput))
                return;

            Regex patternRegex = new Regex("^([1-9][0-9]{0,2}|1000)$");

            if (!patternRegex.IsMatch(NumberOfServingsInput))
                NumberOfServingsInput = NumberOfServingsInput.Substring(0, NumberOfServingsInput.Length - 1);
            else
                NumberOfServings = Convert.ToInt32(NumberOfServingsInput);
        }
        
        private void ValidateTimeOfMakingTheRecipeInput()
        {
            if (string.IsNullOrWhiteSpace(TimeOfMakingTheRecipeInput))
                return;

            Regex patternRegex = new Regex("^([1-9][0-9]{0,2}|1000)$");

            if (!patternRegex.IsMatch(TimeOfMakingTheRecipeInput))
                TimeOfMakingTheRecipeInput = TimeOfMakingTheRecipeInput.Substring(0, TimeOfMakingTheRecipeInput.Length - 1);
            else
                TimeOfMakingTheRecipe = new TimeSpan(0, Convert.ToInt32(TimeOfMakingTheRecipeInput), 0);
        }

        public async Task<bool> UpdateRecipe()
        {
            await App.Database.UpdateRecipe(this);
            return true;
        }

        public async Task<bool> AddNewRecipe()
        {
            await App.Database.InsertRecipe(this);

            int newRecipeId = App.Database.GetLastRecipeId();

            foreach (MakingStep makingStep in MakingSteps)
            {
                makingStep.RecipeId = newRecipeId;
                await App.Database.InsertMakingStep(makingStep);
            }

            foreach (Ingredient ingredient in Ingredients)
            {
                ingredient.RecipeId = newRecipeId;
                await App.Database.InsertIngredient(ingredient);
            }

            return true;
        }
    }
}
