﻿using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RecipeBook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditMakingStepPopup : Popup<MakingStep>
    {
        public AddEditMakingStepPopup(MakingStep makingStep)
        {
            InitializeComponent();
        }

        public MakingStep MakingStep { get; set; }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            maki
            Confirm_Button.IsEnabled = !string.IsNullOrWhiteSpace(Name.Text);
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private void Confirm_Button_Clicked(object sender, EventArgs e)
        {
            Dismiss()
        }
    }
}