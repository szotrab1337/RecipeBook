using Acr.UserDialogs;
using RecipeBook.Models;
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
    public partial class AddEditIngredientPopup : Popup<Ingredient>
    {
        public AddEditIngredientPopup(Ingredient ingredient)
        {
            InitializeComponent();

            Units.ItemsSource = UnitBase.GetUnits();

            if (ingredient is null)
            {
                Ingredient = new Ingredient();
                Units.SelectedIndex = 0;
            }
            else
            {
                Ingredient = ingredient;
                Name.Text = ingredient.Name;
                Quantity.Text = !ingredient.Quantity.HasValue ? string.Empty : ingredient.Quantity.ToString();
                Units.SelectedIndex = !ingredient.UnitId.HasValue ? 0 : ingredient.UnitId.Value - 1;
            }

            Confirm_Button.IsEnabled = Ingredient.IsValid;
        }

        public Ingredient Ingredient { get; set; }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private void Confirm_Clicked(object sender, EventArgs e)
        {
            Dismiss(Ingredient);
        }

        private void Name_Changed(object sender, TextChangedEventArgs e)
        {
            Ingredient.Name = Name.Text;
            Confirm_Button.IsEnabled = Ingredient.IsValid;
        }

        private void Quantity_Changed(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Quantity.Text))
                Ingredient.Quantity = null;
            else
                Ingredient.Quantity = Convert.ToDouble(Quantity.Text);

            Confirm_Button.IsEnabled = Ingredient.IsValid;
        }

        private void Unit_Changed(object sender, EventArgs e)
        {
            Unit unit = Units.SelectedItem as Unit;

            if (unit is null)
                return;

            Ingredient.UnitId = unit.UnitId;
            Confirm_Button.IsEnabled = Ingredient.IsValid;
        }
    }
}