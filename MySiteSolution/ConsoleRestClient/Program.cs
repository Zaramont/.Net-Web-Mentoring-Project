using Catalog.BLL.Models;
using CategoriesProducstApiRestTest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientSample
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.ProductName}\tPrice: " +
                $"{product.UnitPrice}");
        }


        static void ShowCategory(Category category)
        {
            Console.WriteLine($"CategoryName: {category.CategoryName}\tDetails: " +
                $"{category.Description}");
        }

        static async Task<IEnumerable<T>> GetItemsAsync<T>(string path)
        {
            IEnumerable<T> content = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                string jsonContent = await response.Content.ReadAsStringAsync();
                content = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonContent);
            }
            return content;
        }

        public static void Main()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:44341/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var products = await GetItemsAsync<Product>("api/v1/products");
                Console.WriteLine("Products:");
                foreach (var product in products)
                {
                    ShowProduct(product);
                }

                Console.WriteLine("Categories:");
                var categories = await GetItemsAsync<Category>("api/v1/categories");
                foreach (var category in categories)
                {
                    ShowCategory(category);
                }

                var client1 = new CategoriesApiClient();

                Console.WriteLine("Categories using generated code:");
                var result = client1.GetCategories().Content.ToString();
                var categoriesByGenerated = JsonConvert.DeserializeObject<IEnumerable<Category>>(result);
                foreach (var category in categoriesByGenerated)
                {
                    ShowCategory(category);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}