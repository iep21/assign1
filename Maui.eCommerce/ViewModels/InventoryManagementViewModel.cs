using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maui.eCommerce.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
{
    private ProductServiceProxy _svc = ProductServiceProxy.Current;

    public Product? SelectedProduct { get; set; }
    public string? Query { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    // Commands for delete, edit, buy, return, and add actions
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand BuyCommand { get; }
    public ICommand ReturnCommand { get; }
    public ICommand AddCommand { get; }

    public InventoryManagementViewModel()
    {
        DeleteCommand = new Command<Product>(DeleteProduct);  // Accept Product as parameter
        EditCommand = new Command<Product>(EditProduct);      // Accept Product as parameter
        BuyCommand = new Command<Product>(BuyProduct);        // Accept Product as parameter
        ReturnCommand = new Command<Product>(ReturnProduct);  // Accept Product as parameter
        AddCommand = new Command(AddProduct);                 // Add new product command
    }

    // Observable collection of products, updated in real-time
    public ObservableCollection<Product?> Products
    {
        get
        {
            var filteredList = _svc.Products.Where(p => p?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false);
            return new ObservableCollection<Product?>(filteredList);
        }
    }

    public void RefreshProductList()
    {
        Console.WriteLine("Refreshing product list.");
        NotifyPropertyChanged(nameof(Products));
    }

    // Delete product from inventory
    public void DeleteProduct(Product product)
    {
        if (product != null)
        {
            Console.WriteLine("Deleting product with ID: " + product.Id);
            _svc.Delete(product.Id);
            RefreshProductList();
        }
        else
        {
            Console.WriteLine("Product is empty during Delete operation.");
        }
    }

    // Navigate to ProductDetails to edit a product
    public void EditProduct(Product product)
    {
        if (product != null)
        {
            Shell.Current.GoToAsync($"//Product?productId={product.Id}");  // Navigate to ProductDetails page
        }
    }

    // Handle buying a product (decreasing stock)
    public void BuyProduct(Product product)
    {
        if (product != null && product.Stock > 0)
        {
            product.Stock--;
            _svc.AddOrUpdate(product);
            RefreshProductList(); // Update the UI with the new stock
        }
    }

    // Handle returning a product (increasing stock)
    public void ReturnProduct(Product product)
    {
        if (product != null)
        {
            product.Stock++;
            _svc.AddOrUpdate(product);
            RefreshProductList(); // Update the UI with the new stock
        }
    }

    // Handle adding a new product
    public void AddProduct()
    {
        Shell.Current.GoToAsync("//Product");  // Navigate to Product creation page
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

}
