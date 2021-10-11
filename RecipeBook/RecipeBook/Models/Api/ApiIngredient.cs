using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models.Api
{
    public class ApiIngredient
    {
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public double? Quantity { get; set; }
        public int? UnitId { get; set; }
        public int Number { get; set; }

        public virtual ApiRecipe Recipe { get; set; }
    }
}
