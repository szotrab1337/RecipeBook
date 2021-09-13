using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Services
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }
    }
}
