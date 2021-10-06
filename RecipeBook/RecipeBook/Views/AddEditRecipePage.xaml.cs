using Acr.UserDialogs;
using RecipeBook.CustomRenders;
using RecipeBook.Models;
using RecipeBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
                {
                    Message = "Czy na pewno chcesz opuścić stronę? Wprowadzone zmiany nie zostaną zapisane.",
                    OkText = "Tak",
                    CancelText = "Anuluj",
                    Title = "Potwierdzenie",
                    AndroidStyleId = 2131689474
                });

                if(result)
                {
                    base.OnBackButtonPressed();
                    await Navigation.PopAsync();
                }
            });

            return true;
        }
    }
}