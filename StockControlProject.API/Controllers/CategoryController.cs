using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericService<Category> _service;

        //localhost:PortNo/api/Category/TumKategorileriGetir

        public CategoryController(IGenericService<Category> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumKategorileriGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifKategorileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreKategoriGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        [HttpPost]
        public IActionResult KategoriEkle(Category category)
        {
            _service.Add(category);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyeGoreKategoriGetir", new { id = category.ID }, category);
        }
        [HttpPut("{id}")]
        public IActionResult KategoriGuncelle(int id, Category category)
        {
            if (id != category.ID)
            {
                return BadRequest();
            }
            try
            {
                _service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategoriVarMi(id))
                    return NotFound();
            }
            return NoContent();
        }
        private bool KategoriVarMi(int id)
        {
            return _service.Any(cat => cat.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult KategoriSil(int id)
        {
            var category=_service.GetByID(id);
            if (category == null)
                return NotFound();
            try
            {
                _service.Remove(category);
                return Ok("Kategori Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult KategoriAktivate(int id)
        {
            var category = _service.GetByID(id);
            if (category == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                return Ok(category);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
