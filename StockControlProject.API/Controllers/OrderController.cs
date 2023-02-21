using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockControlProject.Domain.Entities;
using StockControlProject.Domain.Enums;
using StockControlProject.Service.Abstract;

namespace StockControlProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> _service;
        private readonly IGenericService<Product> _productService;
        private readonly IGenericService<OrderDetails> _odService;
        private readonly IGenericService<User> _userService;
        public OrderController(IGenericService<Order> orderService, IGenericService<Product> productService, IGenericService<OrderDetails> odService, IGenericService<User> userService)
        {
            _service = orderService;
            _productService = productService;
            _odService = odService;
            _userService = userService;
        }



        //localhost:PortNo/api/order/TumSiparisleriGetir

        [HttpGet]
        public IActionResult TumSiparisleriGetir()
        {
            return Ok(_service.GetAll(t0 => t0.User, t1 => t1.OrderDetails));
        }

        [HttpGet]
        public IActionResult AktifSiparisleriGetir()
        {
            return Ok(_service.GetActive(t0 => t0.User, t1 => t1.OrderDetails));
        }

        [HttpGet("{id}")]
        public IActionResult IdyeGoreSiparisGetir(int id)
        {
            return Ok(_service.GetByID(id, t0 => t0.User, t1 => t1.OrderDetails));
        }
        [HttpGet]
        public IActionResult BekleyenSiparisleriGetir()
        {
            return Ok(_service.GetDefault(x => x.Status == Status.Pending));
        }
        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(_service.GetDefault(x => x.Status == Status.Confirmed));
        }
        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(_service.GetDefault(x => x.Status == Status.Cancelled));
        }
        [HttpPost]
        public IActionResult SiparisEkle(int userID, [FromQuery] int[] productIDs, [FromQuery] short[] quantities)
        {
           Order yeniSiparis=new Order();
            yeniSiparis.Status = Status.Pending;
            yeniSiparis.UserID = userID;
            yeniSiparis.IsActive= true;
            _service.Add(yeniSiparis);
            for (int i = 0; i < productIDs.Length; i++)
            {
                OrderDetails orderDetails = new OrderDetails();
                orderDetails.ProductID = productIDs[i];
                orderDetails.Quantity = quantities[i];
                orderDetails.OrderID = yeniSiparis.ID;
                orderDetails.UnitPrice= _productService.GetByID(productIDs[i]).UnitPrice;
                orderDetails.IsActive = true;
                _odService.Add(orderDetails);
            }
            return CreatedAtAction("IdyeGoreSiparisGetir", new { id = yeniSiparis.ID }, yeniSiparis);
        }
        [HttpGet("{id}")]
        public IActionResult SiparisOnayla(int id)
        {
            Order onaylananSiparis=_service.GetByID(id);

            if(onaylananSiparis== null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetails> details=_odService.GetDefault(x=>x.OrderID==onaylananSiparis.ID).ToList();

                foreach (var item in details)
                {
                    Product siparistekiUrun = _productService.GetByID(item.ProductID);
                    siparistekiUrun.Stock -= item.Quantity;
                    _productService.Update(siparistekiUrun);

                    item.IsActive= false;
                    _odService.Update(item);
                }
                onaylananSiparis.Status = Status.Confirmed;
                onaylananSiparis.IsActive = false;
                _service.Update(onaylananSiparis);
                return Ok(onaylananSiparis);
            }
            
        }
        [HttpGet("{id}")]
        public IActionResult SiparisReddet(int id)
        {
            Order reddedilenSiparis = _service.GetByID(id);

            if (reddedilenSiparis == null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetails> details = _odService.GetDefault(x => x.OrderID == reddedilenSiparis.ID).ToList();

                foreach (var item in details)
                {
                    item.IsActive = false;
                    _odService.Update(item);
                }
                reddedilenSiparis.Status = Status.Cancelled;
                reddedilenSiparis.IsActive = false;
                _service.Update(reddedilenSiparis);
                return Ok(reddedilenSiparis);
            }

        }
        //[HttpPut("{id}")]
        //public IActionResult SiparisGuncelle(int id, Order order)
        //{
        //    if (id != order.ID)
        //    {
        //        return BadRequest();
        //    }
        //    try
        //    {
        //        _service.Update(order);
        //        return Ok(order);
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SiparisVarMi(id))
        //            return NotFound();
        //    }
        //    return NoContent();
        //}
        //private bool SiparisVarMi(int id)
        //{
        //    return _service.Any(cat => cat.ID == id);
        //}
        //[HttpDelete("{id}")]
        //public IActionResult SiparisSil(int id)
        //{
        //    var order = _service.GetByID(id);
        //    if (order == null)
        //        return NotFound();
        //    try
        //    {
        //        _service.Remove(order);
        //        return Ok("Siparis Silindi");
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest();
        //    }
        //}
        //[HttpGet("{id}")]
        //public IActionResult SiparisAktivate(int id)
        //{
        //    var order = _service.GetByID(id);
        //    if (order == null)
        //        return NotFound();
        //    try
        //    {
        //        _service.Activate(id);
        //        return Ok(order);
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest();
        //    }
        //}
    }
}
