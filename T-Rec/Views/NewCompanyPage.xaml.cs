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
	public partial class NewCompanyPage : ContentPage
	{
        public T_Rec_DB_Company Database { get; set; }

        Company edited_company;

        public NewCompanyPage()
        {
            InitializeComponent();
        }

        public NewCompanyPage (Company c = null)
		{
			InitializeComponent ();

            edited_company = c;
		}

        protected override void OnAppearing() 
        {
            if (edited_company != null) 
            {
                tbox_Company_name.Text = edited_company.name;
                tbox_Email.Text = edited_company.email;
            }
        }

        async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                this.Database = await T_Rec_DB_Company.Instance;

                Company c = new Company()
                {
                    company_id = edited_company != null ? edited_company.company_id : -1,
                    name = tbox_Company_name.Text,
                    email = tbox_Email.Text
                };

                await Database.SaveCompanyAsync(c);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                DependencyService.Get<Toast>().Show($"Failed to save company \n {ex.Message}");
            }
        }

        async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}