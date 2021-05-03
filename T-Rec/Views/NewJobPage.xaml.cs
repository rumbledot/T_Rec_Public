using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using T_Rec.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewJobPage : ContentPage
    {
        public JobUnit Job { get; set; }

        public NewJobPage()
        {
            InitializeComponent();
            BindingContext = new NewJobViewModel();
        }
    }
}