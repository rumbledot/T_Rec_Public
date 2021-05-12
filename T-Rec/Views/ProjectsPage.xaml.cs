using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T_Rec.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace T_Rec.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsPage : ContentPage
    {
        public ProjectsPage()
        {
            InitializeComponent();
        }

        public void OnMore(object sender, EventArgs e) 
        {
            var item = sender as Button;
            Project p = item.CommandParameter as Project;

            DependencyService.Get<Toast>().Show($"Project details {p.name}");
        }
    }
}