using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class MakingStep : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int MakingStepId { get; set; }
        public int RecipeId { get; set; }

        public string Name
        {
            get => _Name;
            set { _Name = value; OnPropertyChanged("Name"); }
        }
        private string _Name;
    }
}
