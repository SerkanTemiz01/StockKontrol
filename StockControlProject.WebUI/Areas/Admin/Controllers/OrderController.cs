using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        string uri = "https://localhost:7291";
        public async Task<IActionResult> Index()
        {

            List<Order> Siparisler = new List<Order>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/TumSiparisleriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    Siparisler = JsonConvert.DeserializeObject<List<Order>>(apiCevap);
                }
            }
            return View(Siparisler);
        }
        public async Task<IActionResult> ConfirmOrder(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisOnayla/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CancelOrder(int id)
        {


            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Order/SiparisReddet/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
   
        
    }
}
