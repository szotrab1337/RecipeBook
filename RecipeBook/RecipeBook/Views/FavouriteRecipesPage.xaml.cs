using RecipeBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeBook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteRecipesPage : ContentPage
    {
        FavouriteRecipesViewModel viewModel;
        public FavouriteRecipesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new FavouriteRecipesViewModel(Navigation);
        }
    }
}