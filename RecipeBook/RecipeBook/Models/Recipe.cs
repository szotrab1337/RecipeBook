using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class Recipe : BaseModel
    {
        public int RecipeId { get; set; }

        public string Name
        {
            get => _Name;
            set { _Name = value; OnPropertyChanged("Name"); }
        }
        private string _Name;
        
        public string Description
        {
            get => _Description;
            set { _Description = value; OnPropertyChanged("Description"); }
        }
        private string _Description;
        
        public string TimeOfMakingTheRecipe
        {
            get => _TimeOfMakingTheRecipe;
            set { _TimeOfMakingTheRecipe = value; OnPropertyChanged("TimeOfMakingTheRecipe"); }
        }
        private string _TimeOfMakingTheRecipe;
        
        public string NumberOfServings
        {
            get => _NumberOfServings;
            set { _NumberOfServings = value; OnPropertyChanged("NumberOfServings"); }
        }
        private string _NumberOfServings;
    }
}
