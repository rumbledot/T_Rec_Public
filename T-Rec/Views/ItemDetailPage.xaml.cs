using System.ComponentModel;
using T_Rec.ViewModels;
using Xamarin.Forms;

namespace T_Rec.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}