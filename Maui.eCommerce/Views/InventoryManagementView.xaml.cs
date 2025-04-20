using Maui.eCommerce.ViewModels;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.eCommerce.Views
{
    public partial class InventoryManagementView : ContentPage
    {
        // Constructor: Bind the view to the ViewModel
        public InventoryManagementView()
        {
            InitializeComponent();
            BindingContext = new InventoryManagementViewModel();  // Set BindingContext to the ViewModel
            Console.WriteLine("BindingContext set.");
        }

        // Delete product from inventory
        private void DeleteClicked(object sender, EventArgs e)
        {
            var product = (sender as Button)?.BindingContext as Product;
            if (product != null)
            {
                (BindingContext as InventoryManagementViewModel)?.DeleteProduct(product);  // Pass the product to the ViewModel
            }
            else
            {
                Console.WriteLine("Product is null, cannot delete.");
            }
        }

        // Navigate back to MainPage
        private void CancelClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//MainPage");
        }

        // Navigate to ProductDetails page to add a new product
        private void AddClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Product");
        }

        // Refresh product list when navigated to this page
        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            if (BindingContext != null)
            {
                (BindingContext as InventoryManagementViewModel)?.RefreshProductList();
            }
        }

        // Navigate to ProductDetails to edit a product
        private void EditClicked(object sender, EventArgs e)
        {
            var product = (sender as Button)?.BindingContext as Product;
            if (product != null)
            {
                Shell.Current.GoToAsync($"//Product?productId={product.Id}");  // Pass productId to the ProductDetails page
            }
        }

        // Search functionality to filter the products
        private void SearchClicked(object sender, EventArgs e)
        {
            (BindingContext as InventoryManagementViewModel)?.RefreshProductList();
        }

        // This method is triggered when a product is selected from the ListView
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (BindingContext as InventoryManagementViewModel).SelectedProduct = e.SelectedItem as Product;
        }
    
    }
}
