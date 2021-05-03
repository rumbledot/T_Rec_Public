using System;
using System.Collections.Generic;
using System.Text;
using T_Rec.Models;
using Xamarin.Forms;

namespace T_Rec.ViewModels
{
    public class NewJobViewModel : BaseViewModel
    {
        private string text;
        private string description;

        public NewJobViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            JobUnit newItem = new JobUnit()
            {
                job_id = Convert.ToInt32(Guid.NewGuid()),
                description = Description,
                time_start = DateTime.Now,
                time_end = DateTime.Now
            };

            await JobDataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
