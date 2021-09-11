using RecipeBook.ViewModels;
using RecipeBook.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace RecipeBook
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecipesPage), typeof(RecipesPage));
            Routing.RegisterRoute(nameof(FavouriteRecipesPage), typeof(FavouriteRecipesPage));
        }

    }
}
