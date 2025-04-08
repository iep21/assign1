//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Linq;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Amazon!");

            List<Product?> inventory = ProductServiceProxy.Current.Products;
            List<CartItem> shoppingCart = new List<CartItem>(); //init shopping cart

            char choice;
            do
            {
                Console.WriteLine("\nC. Create new inventory item");
                Console.WriteLine("R. Read all inventory items");
                Console.WriteLine("U. Update an inventory item");
                Console.WriteLine("D. Delete an inventory item");
                Console.WriteLine("A. Add item to shopping cart");
                Console.WriteLine("V. View shopping cart");
                Console.WriteLine("E. Remove(erase) item from shopping cart");
                Console.WriteLine("O. Checkout");
                Console.WriteLine("Q. Quit");

                string? input = Console.ReadLine();
                choice = input[0];

                switch (choice)
                {
                    case 'C':
                    case 'c':
                        //create product in inventory
                        Console.WriteLine("Enter product name:");
                        string productName = Console.ReadLine();

                        Console.WriteLine("Enter product price:");
                        decimal productPrice = decimal.Parse(Console.ReadLine() ?? "0");

                        Console.WriteLine("Enter product quantity:");
                        int productQuantity = int.Parse(Console.ReadLine() ?? "0");

                        //creates the new prod with provided info
                        var newProduct = new Product
                        {
                            Name = productName,
                            Price = productPrice,
                            Stock = productQuantity
                        };

                        //adds prod to inventory
                        ProductServiceProxy.Current.AddOrUpdate(newProduct);
                        Console.WriteLine("Product added to inventory.");
                        break;

                    case 'R':
                    case 'r':
                        //display prods in inv
                        inventory.ForEach(Console.WriteLine);
                        break;

                    case 'U':
                    case 'u':
                        //updates prods in inv
                        Console.WriteLine("Which product would you like to update? (Enter product ID)");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProd = inventory.FirstOrDefault(p => p.Id == selection);

                        if (selectedProd != null)
                        {
                            Console.WriteLine("Enter new name for the product:");
                            selectedProd.Name = Console.ReadLine() ?? "ERROR";

                            ProductServiceProxy.Current.AddOrUpdate(selectedProd);
                        }
                        break;

                    case 'D':
                    case 'd':
                        //delete prods from inv
                        Console.WriteLine("Which product would you like to delete? (Enter product ID)");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        Product? deletedProduct = ProductServiceProxy.Current.Delete(selection);
    
                        if (deletedProduct != null)
                        {
                            //reorder the remaining products
                            var productsToReorder = ProductServiceProxy.Current.Products.Where(p => p?.Id > deletedProduct.Id).ToList();
                            
                            foreach (var product in productsToReorder)
                            {
                                product.Id -= 1; //shift the ID down by 1
                                ProductServiceProxy.Current.AddOrUpdate(product); //update product with the new ID
                            }

                            Console.WriteLine($"Product '{deletedProduct.Name}' has been deleted.");
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                        break;

                    case 'A':
                    case 'a':
                        //add item to cart
                        Console.WriteLine("Enter product ID to add to the cart:");
                        int productIdToAdd = int.Parse(Console.ReadLine() ?? "-1");
                        var productToAdd = inventory.FirstOrDefault(p => p.Id == productIdToAdd);

                        if (productToAdd != null)
                        {
                            var existingItem = shoppingCart.FirstOrDefault(c => c.Product.Id == productIdToAdd);
                            if (existingItem != null)
                            {
                                existingItem.Quantity += 1;
                            }
                            else
                            {
                                shoppingCart.Add(new CartItem(productToAdd, 1));
                            }
                        }
                        else
                        {
                            Console.WriteLine("Product not found.");
                        }
                        break;

                    case 'V':
                    case 'v':
                        //view item in cart
                        shoppingCart.ForEach(item => Console.WriteLine(item));
                        break;

                    case 'E':
                    case 'e':
                        //remove from shopping cart
                        Console.WriteLine("Enter product ID to remove from the cart:");
                        int productIdToRemove = int.Parse(Console.ReadLine() ?? "-1");
                        
                        //find the item in the shopping cart based on product ID
                        var itemToRemove = shoppingCart.FirstOrDefault(c => c.Product.Id == productIdToRemove);

                        if (itemToRemove != null)
                        {
                            //remove the item from the cart
                            shoppingCart.Remove(itemToRemove);

                            //restore stock in inventory if needed
                            var productInInventory = inventory.FirstOrDefault(p => p.Id == productIdToRemove);
                            if (productInInventory != null)
                            {
                                productInInventory.Stock += itemToRemove.Quantity;
                                ProductServiceProxy.Current.AddOrUpdate(productInInventory);
                            }

                            Console.WriteLine($"Item '{itemToRemove.Product.Name}' has been removed from your cart.");
                        }
                        else
                        {
                            Console.WriteLine("Item not found in the cart.");
                        }
                        break;

                    case 'O':
                    case 'o':
                        //calculate total price of all items in cart
                        decimal totalPrice = shoppingCart.Sum(item => item.TotalPrice()); //sum prices of all items in the shopping cart

                        //calculate sales tax
                        decimal salesTax = totalPrice * 0.07m;
                        
                        //calculate total with tax
                        decimal totalWithTax = totalPrice + salesTax;

                        //print receipt
                        Console.WriteLine("\n--- Receipt ---");
                        foreach (var item in shoppingCart)
                        {
                            Console.WriteLine($"{item.Product.Name} x{item.Quantity}: ${item.TotalPrice():F2}");  //print item name, quantity, and price
                        }
                        Console.WriteLine($"Subtotal: ${totalPrice:F2}");
                        Console.WriteLine($"Sales Tax (7%): ${salesTax:F2}");
                        Console.WriteLine($"Total: ${totalWithTax:F2}");

                        //clear cart after checkout
                        shoppingCart.Clear();
                        break;
                    case 'Q':
                    case 'q':
                        //receipt when quitting
                        decimal totalForQuit = shoppingCart.Sum(item => item.Product.Price * item.Quantity);
                        decimal taxForQuit = totalForQuit * 0.07m;  //sales tax
                        decimal finalTotalForQuit = totalForQuit + taxForQuit;

                        Console.WriteLine("\n--- Receipt ---");
                        shoppingCart.ForEach(item => Console.WriteLine(item));
                        Console.WriteLine($"Subtotal: ${totalForQuit:F2}");
                        Console.WriteLine($"Sales Tax (7%): ${taxForQuit:F2}");
                        Console.WriteLine($"Total: ${finalTotalForQuit:F2}");

                        shoppingCart.Clear(); //clear cart when done
                        break;

                    default:
                        Console.WriteLine("Error: Unknown Command");
                        break;

                }
            } while (choice != 'Q' && choice != 'q');

            Console.WriteLine("Come again!");
            Console.ReadLine();
        }
    }

}
