using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models.Api
{
    public class ApiMakingStep
    {
        public int MakingStepId { get; set; }
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }

        public virtual ApiRecipe Recipe { get; set; }
    }
}
