using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RecipeBook.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged("Title"); }
        }
        public string _Title;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
