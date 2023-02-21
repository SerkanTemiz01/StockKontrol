using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IGenericService<Supplier> _service;

        public SupplierController(IGenericService<Supplier> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumTedarikcileriGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifTedarikcileriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreTedarikciGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        [HttpPost]
        public IActionResult TedarikciEkle(Supplier supplier)
        {
            _service.Add(supplier);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyeGoreTedarikciGetir", new { id = supplier.ID }, supplier);
        }
        [HttpPut("{id}")]
        public IActionResult TedarikciGuncelle(int id, Supplier supplier)
        {
            if (id != supplier.ID)
            {
                return BadRequest();
            }
            try
            {
                _service.Update(supplier);
                return Ok(supplier);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TedarikciVarMi(id))
                    return NotFound();
            }
            return NoContent();
        }
        private bool TedarikciVarMi(int id)
        {
            return _service.Any(cat => cat.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult TedarikciSil(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
                return NotFound();
            try
            {
                _service.Remove(supplier);
                return Ok("Kategori Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult TedarikciAktivate(int id)
        {
            var supplier = _service.GetByID(id);
            if (supplier == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                return Ok(supplier);
            }   
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
