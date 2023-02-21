using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        string uri = "https://localhost:7291";
        public async Task<IActionResult> Index()
        {
            
            List<Supplier> Tedarikciler = new List<Supplier>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TumTedarikcileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    Tedarikciler = JsonConvert.DeserializeObject<List<Supplier>>(apiCevap);
                }
            }
            return View(Tedarikciler);
        }
        public async Task<IActionResult> ActivateSupplier(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/TedarikciAktivate/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteSupplier(int id)
        {


            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Supplier/TedarikciSil/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateSupplier(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Supplier/IdyeGoreTedarikciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    Supplier güncellenecekTedarikci = JsonConvert.DeserializeObject<Supplier>(apiCevap);
                    return View(güncellenecekTedarikci);
                }
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateSupplier(Supplier supplier)
        {
            using (var httpClient = new HttpClient())
            {

                using (var cevap = await httpClient.PutAsJsonAsync($"{uri}/api/Supplier/TedarikciGuncelle/{supplier.ID}", supplier))
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
        [HttpGet]
        public async Task<IActionResult> AddSupplier()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            supplier.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.PostAsJsonAsync($"{uri}/api/Supplier/TedarikciEkle", supplier))
                {
                    if (cevap.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return View(supplier);
                }
            }
        }
    }
}
