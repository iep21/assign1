using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel
    {
        private ProductServiceProxy _svc = ProductServiceProxy.Current;
        public string? Name { 
            get
            {
                return Model?.Name ?? string.Empty;
            }

            set
            {
                if(Model != null && Model.Name != value)
                {
                    Model.Name = value;
                }
            }
        }

        public Product? Model { get; set; }

        public void AddOrUpdate()
        {
            if (Model != null)
            {
                _svc.AddOrUpdate(Model);
            }
            else
            {
                // Optionally, handle the case where Model is null (e.g., show an error message)
                Console.WriteLine("Error: Product is null.");
            }
        }

        public ProductViewModel() {
            Model = new Product();
        }

        public ProductViewModel(Product? model)
        {
            Model = model;
        }
    }
}
