using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CollapsibleToolbar.ViewModels
{
    public class FirstViewModel : BaseViewModel
    {
        public ObservableCollection<string> Attachments { get; set; }

        public Command AddAttachment { get; }
        public ICommand SaveCommand { private set; get; }

        public int AttachmentCount { get; set; }

        public FirstViewModel()
        {
            AttachmentCount = 0;
            Attachments = new ObservableCollection<string>();
            AddAttachment = new Command(AddAttachmentCommand);
            SaveCommand = new Command(Save);
        }

        private async void Save(object obj)
        {
            await Application.Current.MainPage.DisplayAlert("Matugas", "Aoba, bão?", "Bão!");
        }

        private void AddAttachmentCommand(object obj)
        {
            AttachmentCount++;
            Attachments.Add($"Anexo nº: {AttachmentCount}");
        }
    }
}
