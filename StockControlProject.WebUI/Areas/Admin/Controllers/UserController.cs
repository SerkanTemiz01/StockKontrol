using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        string uri = "https://localhost:7291";
        public async Task<IActionResult> Index()
        {

            List<User> kullaniciler = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/TumKullanicileriGetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kullaniciler = JsonConvert.DeserializeObject<List<User>>(apiCevap);
                }
            }
            return View(kullaniciler);
        }
        public async Task<IActionResult> ActivateUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/KullaniciAktivate/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {


            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.DeleteAsync($"{uri}/api/User/KullaniciSil/{id}"))
                {

                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/User/IdyeGoreKullaniciGetir/{id}"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    updatedUser = JsonConvert.DeserializeObject<User>(apiCevap);
                    return View(updatedUser);
                }
            }

        }
        User updatedUser;
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User guncelKullanici)
        {
            using (var httpClient = new HttpClient())
            {
                guncelKullanici.AddedDate=updatedUser.AddedDate;
                guncelKullanici.IsActive = updatedUser.IsActive;
                guncelKullanici.Password=updatedUser.Password;

                using (var cevap = await httpClient.PutAsJsonAsync($"{uri}/api/User/KullaniciGuncelle/{guncelKullanici.ID}", guncelKullanici))
                {
                    if (cevap.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return View(guncelKullanici);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(User user,IFormFile formFile)
        {
            user.IsActive = true;
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.PostAsJsonAsync($"{uri}/api/User/KullaniciEkle", user))
                {
                    if (cevap.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                        return View(user);
                }
            }
        }
    }
}
