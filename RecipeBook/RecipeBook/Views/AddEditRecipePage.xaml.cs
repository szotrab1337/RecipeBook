using RecipeBook.Models;
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
    public partial class AddEditRecipePage : ContentPage
    {
        AddEditRecipeViewModel viewModel;

        public AddEditRecipePage(Recipe recipe)
        {
            InitializeComponent();
            BindingContext = viewModel = new AddEditRecipeViewModel(Navigation, recipe);
        }

        public delegate void HandlePopDelegate(string parameter);
        public event HandlePopDelegate DidFinishPoping;
    }
}