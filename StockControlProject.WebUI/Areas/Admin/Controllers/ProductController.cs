using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        string uri = "https://localhost:7291";
        public async Task<IActionResult> Index()
        {
            List<Product> Urunler = new List<Product>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/TumUrunleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    Urunler = JsonConvert.DeserializeObject<List<Product>>(apiCevap);
                }
            }
            return View(Urunler);
        }
        public async Task<IActionResult> ActivateProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/UrunAktivate/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Product/UrunSil/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            ViewBag.AktifKategoriler = new SelectList(aktifKategoriler, "ID", "CategoryName");
            ViewBag.AktifTedarikciler = new SelectList(aktifTedarikciler, "ID", "SupplierName");
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Product/IdyeGoreUrunGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    Product güncellenecekUrun = JsonConvert.DeserializeObject<Product>(apiCevap);                 
                    return View(güncellenecekUrun);
                }
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            using (var httpClient = new HttpClient())
            {

                using (var cevap = await httpClient.PutAsJsonAsync($"{uri}/api/Product/UrunGuncelle/{product.ID}", product))
                {
                    if (cevap.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return View(cevap);
                }
            }
        }
        static List<Category> aktifKategoriler;
        static List<Supplier> aktifTedarikciler;
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/AktifTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifTedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/AktifKategorileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    aktifKategoriler = JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            ViewBag.AktifKategoriler = new SelectList(aktifKategoriler, "ID", "CategoryName");
            ViewBag.AktifTedarikciler = new SelectList(aktifTedarikciler, "ID", "SupplierName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            product.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.PostAsJsonAsync($"{uri}/api/Product/UrunEkle",product))
                {
                    if (cevap.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return View(product);
                }
            }
        }
    }
}
