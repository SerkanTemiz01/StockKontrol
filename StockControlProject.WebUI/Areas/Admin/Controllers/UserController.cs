using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockControlProject.Domain.Entities;
using StockControlProject.WebUI.Models;

namespace StockControlProject.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        string uri = "https://localhost:7291";
        private readonly IWebHostEnvironment _environment;

        public UserController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

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
        static User updatedUser;
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User guncelKullanici,List<IFormFile> files)
        {
            if(files.Count == 0)
            {
                guncelKullanici.PhotoURL = updatedUser.PhotoURL;
            }
            else
            {
                string returnedMessage = Upload.ImageUpload(files, _environment, out bool imgResult);
                if (imgResult)
                {
                    guncelKullanici.PhotoURL = returnedMessage;
                }
                else
                {
                    ViewBag.Message = returnedMessage;
                    return View(guncelKullanici);
                }
                
            }

           
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
        public async Task<IActionResult> AddUser(User user,List<IFormFile> files)
        {
            user.IsActive = true;
            string imgPath = Upload.ImageUpload(files, _environment, out bool imgResult);

            if(imgResult)
            {
                user.PhotoURL = imgPath;
            }
            else
            {
                ViewBag.Message = imgPath;
                return View(user);
            }

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
