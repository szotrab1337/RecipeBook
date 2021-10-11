using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class Ingredient : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }

        public string Name
        {
            get => _Name;
            set { _Name = value; OnPropertyChanged("Name"); Validate(); }
        }
        private string _Name;
        
        public int Number
        {
            get => _Number;
            set { _Number = value; OnPropertyChanged("Number"); OnPropertyChanged("NumberFormatted"); }
        }
        private int _Number;
        
        public double? Quantity
        {
            get => _Quantity;
            set { _Quantity = value; OnPropertyChanged("Quantity"); Validate(); OnPropertyChanged("QuantityFormatted"); }
        }
        private double? _Quantity;      

        public int? UnitId
        {
            get => _UnitId;
            set { _UnitId = value; OnPropertyChanged("UnitId"); }
        }
        private int? _UnitId;

        [Ignore]
        public string QuantityFormatted => UnitId is null || !Quantity.HasValue ? string.Empty : Quantity.ToString() + " " + UnitBase.GetUnit(UnitId.Value);

        [Ignore]
        public string NumberFormatted => Number.ToString() + ".";

        [Ignore]
        public bool IsValid { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                IsValid = false;
                return;
            }

            if (Quantity.HasValue && Quantity.Value < 0)
            {
                IsValid = false;
                return;
            }

            IsValid = true;
        }
    }
}
