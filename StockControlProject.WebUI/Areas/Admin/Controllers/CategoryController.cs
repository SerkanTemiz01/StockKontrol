using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        string uri = "https://localhost:7291";
        public async Task<IActionResult> Index()
        {
            List<Category> kategoriler= new List<Category>();
            using(var httpClient=new HttpClient())
            {
                using(var cevap=await httpClient.GetAsync($"{uri}/api/Category/TumKategorileriGetir"))
                {
                    string apiCevap=await cevap.Content.ReadAsStringAsync();
                    kategoriler=JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return View(kategoriler);
        }
        public async Task<IActionResult> ActivateCategory(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/KategoriAktivate/{id}"))
                {
                   
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {


            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/Category/KategoriSil/{id}"))
                {
                   
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Category/IdyeGoreKategoriGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    Category güncellenecekKategori=JsonConvert.DeserializeObject<Category>(apiCevap);
                    return View(güncellenecekKategori);
                }
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            using (var httpClient = new HttpClient())
            {
               
                using (var cevap = await httpClient.PutAsJsonAsync($"{uri}/api/Category/KategoriGuncelle/{category.ID}",category))
                {
                   if(cevap.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                   else
                        return View(cevap);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.PostAsJsonAsync($"{uri}/api/Category/KategoriEkle", category))
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
    }
}
