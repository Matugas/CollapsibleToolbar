using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CollapsibleToolbar.Models;
using Xamarin.Forms;

namespace CollapsibleToolbar.ViewModels
{
    public class SecondViewModel : BaseViewModel
    {
        private List<Item> _items;
        public List<Item> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private List<Item> _filteredItems;
        public List<Item> FilteredItems
        {
            get => _filteredItems;
            set => SetProperty(ref _filteredItems, value);
        }

        private string _searchBarText;
        public string SearchBarText
        {
            get => _searchBarText;
            set {
                SetProperty(ref _searchBarText, value);
            }
        }

        public Command SearchCommand { get; }
        public ICommand ThirdPageCommand { private set; get; }
        public ICommand ItemTappedCommand { private set; get; }

        public SecondViewModel()
        {
            Items = new List<Item>();
            for (int i = 0; i < 15; i++)
            {
                Items.Add(new Item
                {
                    Title = $"[{i}] Item",
                    Description = $"Description of Item {i}",
                    Image = GetImageUrl()
                });
            }

            Task.Run(() =>
            {
                FilteredItems = new List<Item>(Items);
            }).ConfigureAwait(true);
            SearchCommand = new Command(Search);
            ThirdPageCommand = new Command(GoToThirdPage);
            ItemTappedCommand = new Command(ItemTapped);
        }

        private async void ItemTapped(object obj)
        {
            var item = (Item)obj;
            await Application.Current.MainPage.DisplayAlert("Matugas", "Item selecionado: " + item.Title, "Fechar");
        }

        private async void GoToThirdPage(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ThirdPage());
        }

        private string GetImageUrl()
        {
            var number = new Random().Next(100, 200);
            return $"https://picsum.photos/{number}";
        }

        private string LastSearchedText { get; set; }

        private void Search(object obj)
        {
            if (SearchBarText == LastSearchedText)
                return;

            Debug.WriteLine("Search");
            if (string.IsNullOrWhiteSpace(SearchBarText))
            {
                FilteredItems = new List<Item>(Items);
                return;
            }
            Task.Run(() => {
                var filtered = Items.Where(item => item.Title.ToLower().Contains(SearchBarText.ToLower()));
                FilteredItems = new List<Item>(filtered);
                LastSearchedText = SearchBarText;
            }).ConfigureAwait(true);
        }
    }
}
