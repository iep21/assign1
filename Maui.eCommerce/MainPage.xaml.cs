using System.Threading.Tasks;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce
{
    public partial class MainPage : ContentPage
    {
        private int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void InventoryClicked(object sender, EventArgs e)
        {
            // Navigate to Inventory Management page
            await Shell.Current.GoToAsync("//InventoryManagementView");
        }
    }

}
