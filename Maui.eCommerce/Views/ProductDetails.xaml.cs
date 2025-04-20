using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;
using Spring2025_Samples.Models;

namespace Maui.eCommerce.Views
{
    [QueryProperty(nameof(ProductId), "productId")]
    public partial class ProductDetails : ContentPage
    {
        public ProductDetails()
        {
            InitializeComponent();
        }

        // ProductId property that is passed via query parameters
        public int ProductId { get; set; }

        // Navigate back to InventoryManagement page
        private void GoBackClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//InventoryManagement");
        }

        // Save the product (create or update) and navigate back to InventoryManagement
        private void OkClicked(object sender, EventArgs e)
        {
            (BindingContext as ProductViewModel)?.AddOrUpdate();
            Shell.Current.GoToAsync("//InventoryManagement");
        }

        // On page load, check if we have a ProductId, and bind the appropriate product to the ViewModel
        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            if (ProductId == 0) // If there's no productId, we are creating a new product
            {
                BindingContext = new ProductViewModel(); // Create a new product
            }
            else
            {
                BindingContext = new ProductViewModel(ProductServiceProxy.Current.GetById(ProductId)); // Edit existing product
            }
        }
    }
}
