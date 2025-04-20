using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_Samples.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string? Display
        {
            get
            {
                return $"{Id}. {Name} - ${Price} - {Stock} in stock";
            }
        }

        public Product()
        {
            Name = string.Empty;
            Price = 0.0m; //default price
            Stock = 0; //default stock
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
    public class CartItem
    {
        public Product Product { get; set; }  //stores the product object
        public int Quantity { get; set; }     //stores the quantity of the product in the cart

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Product.Name} x{Quantity} - ${Product.Price:F2} each";
        }

        public decimal TotalPrice()
        {
            return Product.Price * Quantity;
        }
    }


}
