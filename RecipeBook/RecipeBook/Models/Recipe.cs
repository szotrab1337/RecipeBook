﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
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
        
        public double NumberOfServings
        {
            get => _NumberOfServings;
            set { _NumberOfServings = value; OnPropertyChanged("NumberOfServings"); }
        }
        private double _NumberOfServings;
        
        public DateTime CreatedOn
        {
            get => _CreatedOn;
            set { _CreatedOn = value; OnPropertyChanged("CreatedOn"); }
        }
        private DateTime _CreatedOn;
        
        public string PictureRaw
        {
            get => _PictureRaw;
            set { _PictureRaw = value; OnPropertyChanged("PictureRaw"); OnPropertyChanged("Picture"); }
        }
        private string _PictureRaw;

        [Ignore]
        public ImageSource Picture => string.IsNullOrEmpty(PictureRaw) ? ImageSource.FromResource("RecipeBook.Images.placeholder.png") :
            ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(PictureRaw)));

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
    }
}
