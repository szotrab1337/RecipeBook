using RecipeBook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Services
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTableAsync<Recipe>().Wait();
            _database.CreateTableAsync<MakingStep>().Wait();
            _database.CreateTableAsync<Ingredient>().Wait();
        }

        #region Recipes

        public Task InsertRecipe(Recipe recipe)
        {
            return _database.InsertAsync(recipe);
        }

        public Task<List<Recipe>> GetRecipes(string filter)
        {
            bool dontUserFilter = string.IsNullOrWhiteSpace(filter);

            return _database.Table<Recipe>().Where(x => dontUserFilter
                        || x.Name.ToLower().Contains(filter)).OrderBy(x => x.Name).ToListAsync();
        }

        public Task UpdateRecipe(Recipe recipe)
        {
            return _database.UpdateAsync(recipe);
        }

        public Task DeleteRecipe(Recipe recipe)
        {
            return _database.DeleteAsync(recipe);
        }

        #endregion

        #region MakingSteps

        public Task<List<MakingStep>> GetMakingSteps(Recipe recipe)
        {
            return _database.Table<MakingStep>().Where(x => x.RecipeId == recipe.RecipeId).ToListAsync();
        }
        
        public Task<List<MakingStep>> GetMakingSteps(int recipeId)
        {
            return _database.Table<MakingStep>().Where(x => x.RecipeId == recipeId).ToListAsync();
        }

        public async Task DeleteMakingSteps(int recipeId)
        {
            List<MakingStep> makingStepsToDelete = await GetMakingSteps(recipeId);

            foreach (MakingStep makingStep in makingStepsToDelete)
            {
                await _database.DeleteAsync(makingStep);
            }
        }

        public async Task DeleteMakingStep(MakingStep makingStep)
        {
            await _database.DeleteAsync(makingStep);
        }

        public async Task InsertMakingStep(MakingStep makingStep)
        {
            await _database.InsertAsync(makingStep);
        }

        #endregion

        #region Ingredients

        public Task<List<Ingredient>> GetIngredients(Recipe recipe)
        {
            return _database.Table<Ingredient>().Where(x => x.RecipeId == recipe.RecipeId).ToListAsync();
        }
        
        public Task<List<Ingredient>> GetIngredients(int recipeId)
        {
            return _database.Table<Ingredient>().Where(x => x.RecipeId == recipeId).ToListAsync();
        }

        public async Task DeleteIngredients(int recipeId)
        {
            List<Ingredient> ingredientsToDelete = await GetIngredients(recipeId);

            foreach (Ingredient ingredient in ingredientsToDelete)
            {
                await _database.DeleteAsync(ingredient);
            }
        }

        public async Task DeleteIngredient(Ingredient ingredient)
        {
            await _database.DeleteAsync(ingredient);
        }

        public async Task InsertIngredient(Ingredient ingredient)
        {
            await _database.InsertAsync(ingredient);
        }

        #endregion
    }
}
