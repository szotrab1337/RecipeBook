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
        }

        public Task InsertRecipe(Recipe recipe)
        {
            return _database.InsertAsync(recipe);
        }

        public Task<List<Recipe>> GetRecipes(string filter)
        {
            bool dontUserFilter = string.IsNullOrWhiteSpace(filter);

            return _database.Table<Recipe>().Where(x => dontUserFilter || x.Name.ToLower().Contains(filter)).ToListAsync();
        }

        public Task UpdateRecipe(Recipe recipe)
        {
            return _database.UpdateAsync(recipe);
        }
    }
}
