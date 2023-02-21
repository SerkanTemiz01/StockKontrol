using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericService<Product> _service;

        //localhost:PortNo/api/Product/TumUrunleriGetir

        public ProductController(IGenericService<Product> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumUrunleriGetir()
        {
            return Ok(_service.GetAll(t0=>t0.Category,t1=>t1.Supplier));
        }

        [HttpGet]
        public IActionResult AktifUrunleriGetir()
        {
            return Ok(_service.GetActive(t0 => t0.Category, t1 => t1.Supplier));
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreUrunGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        [HttpPost]
        public IActionResult UrunEkle(Product product)
        {
            _service.Add(product);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyeGoreUrunGetir", new { id = product.ID }, product);
        }
        [HttpPut("{id}")]
        public IActionResult UrunGuncelle(int id, Product product)
        {
            if (id != product.ID)
            {
                return BadRequest();
            }
            try
            {
                _service.Update(product);
                return Ok(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrunVarMi(id))
                    return NotFound();
            }
            return NoContent();
        }
        private bool UrunVarMi(int id)
        {
            return _service.Any(cat => cat.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult UrunSil(int id)
        {
            var product = _service.GetByID(id);
            if (product == null)
                return NotFound();
            try
            {
                _service.Remove(product);
                return Ok("Urun Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult UrunAktivate(int id)
        {
            var product = _service.GetByID(id);
            if (product == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                return Ok(product);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
