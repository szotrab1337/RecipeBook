using Acr.UserDialogs;
using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class RecipeDetailsViewModel : BaseViewModel
    {
        public RecipeDetailsViewModel(INavigation navigation, Recipe recipe)
        {
            Navigation = navigation;
            Recipe = recipe;
            Title = "Szczegóły przepisu";
            Recipe.LoadAssociatedItems();

            SourceTapCommand = new Command(SourceTapAction);
        }

        public ICommand SourceTapCommand { get; set; }

        public Recipe Recipe
        {
            get => _Recipe;
            set { _Recipe = value; OnPropertyChanged("Recipe"); }
        }
        private Recipe _Recipe;

        private async void SourceTapAction()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Recipe.Source))
                    return;

                List<string> choices = new List<string>
                {
                    "Otwórz",
                    "Kopiuj"
                };

                string result = await UserDialogs.Instance.ActionSheetAsync("Wybierz...", string.Empty, "Anuluj", CancellationToken.None, choices.ToArray());

                if (result.Equals("Anuluj") || string.IsNullOrWhiteSpace(result))
                    return;

                if (result.Equals("Otwórz"))
                {
                    try
                    {
                        await Browser.OpenAsync(new Uri(Recipe.Source));
                    }
                    catch
                    {
                        UserDialogs.Instance.Alert("Wprowadzono nieprawidłowy adres URL!", "Błąd", "OK");
                    }
                }

                if (result.Equals("Kopiuj"))
                {
                    await Clipboard.SetTextAsync(Recipe.Source);
                    UserDialogs.Instance.Toast("Tekst został skopiowany do schowka.");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert("Błąd!\r\n\r\n" + ex.ToString(), "Błąd", "OK");
            }
        }
    }
}
