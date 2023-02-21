using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IGenericService<OrderDetails> _service;

        //localhost:PortNo/api/orderDetails/TumSiparisDetayleriGetir

        public OrderDetailsController(IGenericService<OrderDetails> service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult TumSiparisDetayleriGetir()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifSiparisDetayleriGetir()
        {
            return Ok(_service.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisDetayGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        [HttpPost]
        public IActionResult SiparisDetayEkle(OrderDetails orderDetails)
        {
            _service.Add(orderDetails);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyeGoreSiparisDetayGetir", new { id = orderDetails.ID }, orderDetails);
        }
        [HttpPut("{id}")]
        public IActionResult SiparisDetayGuncelle(int id, OrderDetails orderDetails)
        {
            if (id != orderDetails.ID)
            {
                return BadRequest();
            }
            try
            {
                _service.Update(orderDetails);
                return Ok(orderDetails);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiparisDetayVarMi(id))
                    return NotFound();
            }
            return NoContent();
        }
        private bool SiparisDetayVarMi(int id)
        {
            return _service.Any(cat => cat.ID == id);
        }
        [HttpDelete("{id}")]
        public IActionResult SiparisDetaySil(int id)
        {
            var orderDetails = _service.GetByID(id);
            if (orderDetails == null)
                return NotFound();
            try
            {
                _service.Remove(orderDetails);
                return Ok("SiparisDetay Silindi");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult SiparisDetayAktivate(int id)
        {
            var orderDetails = _service.GetByID(id);
            if (orderDetails == null)
                return NotFound();
            try
            {
                _service.Activate(id);
                return Ok(orderDetails);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
