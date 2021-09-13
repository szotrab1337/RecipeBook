using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class Ingredient : BaseModel
    {
        public int IngredientId { get; set; }

        public string Name
        {
            get => _Name;
            set { _Name = value; OnPropertyChanged("Name"); }
        }
        private string _Name;
    }
}
