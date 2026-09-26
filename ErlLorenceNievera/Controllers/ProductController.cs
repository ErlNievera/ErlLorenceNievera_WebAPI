using Microsoft.AspNetCore.Mvc;
using Nievera_ErlLorenceAPI.Models;
using System.Net.Http.Json;

namespace Nievera_ErlLorenceAPI.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly string apiUrl =
            "http://localhost:5231/api/Product";

        public ProductController()
        {
            _httpClient = new HttpClient();
        }

        // =========================
        // READ - Display All Products
        // =========================
        public async Task<IActionResult> Index()
        {
            var products =
                await _httpClient.GetFromJsonAsync<List<Product>>(apiUrl);

            return View(products);
        }

        // =========================
        // CREATE - Display Form
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================
        // CREATE - Save Product
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var response =
                await _httpClient.PostAsJsonAsync(apiUrl, product);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to create the product. Please try again.");

                return View(product);
            }

            TempData["SuccessMessage"] =
                "Product created successfully.";

            return RedirectToAction("Index");
        }

        // =========================
        // EDIT - Display Form
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product =
                await _httpClient.GetFromJsonAsync<Product>(
                    $"{apiUrl}/{id}");

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // =========================
        // EDIT - Update Product
        // =========================
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            await _httpClient.PutAsJsonAsync(
                $"{apiUrl}/{product.Id}",
                product);

            return RedirectToAction("Index");
        }

        // =========================
        // DELETE - Delete Product
        // =========================
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync(
                $"{apiUrl}/{id}");

            return RedirectToAction("Index");
        }

        // =========================
        // SEARCH - Search Product
        // =========================
        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            var products =
                await _httpClient.GetFromJsonAsync<List<Product>>(apiUrl);

            if (!string.IsNullOrWhiteSpace(name))
            {
                products = products
                    .Where(p => p.Name.Contains(
                        name,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (products == null || products.Count == 0)
            {
                ViewBag.Message = "No product found.";
            }

            return View("Index", products);
        }
    }
}
