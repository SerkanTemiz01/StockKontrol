using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericService<User> _service;

        //localhost:PortNo/api/user/TumKullanicileriGetir

        public UserController(IGenericService<User> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumKullanicileriGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifKullanicileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreKullaniciGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        [HttpPost]
        public IActionResult KullaniciEkle(User user)
        {
            _service.Add(user);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyeGoreKullaniciGetir", new { id = user.ID }, user);
        }
        [HttpPut("{id}")]
        public IActionResult KullaniciGuncelle(int id, User user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }
            try
            {
                _service.Update(user);
                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KullaniciVarMi(id))
                    return NotFound();
            }
            return NoContent();
        }
        private bool KullaniciVarMi(int id)
        {
            return _service.Any(cat => cat.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult KullaniciSil(int id)
        {
            var user = _service.GetByID(id);
            if (user == null)
                return NotFound();
            try
            {
                _service.Remove(user);
                return Ok("Kullanici Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult KullaniciAktivate(int id)
        {
            var user = _service.GetByID(id);
            if (user == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                return Ok(user);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
