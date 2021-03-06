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
    public partial class RecipesPage : ContentPage
    {
        RecipesViewModel viewModel;
        public RecipesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new RecipesViewModel(Navigation);
        }
    }
}