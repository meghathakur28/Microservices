using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.DTOs;
using OrderService.Models;
using OrderService.Service;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ExternalService _externalService;

        public OrderController(OrderDbContext context, ExternalService externalService)
        {
            _context = context;
            _externalService = externalService;
        }

        // 🛒 PLACE ORDER
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
        {
            // 1️⃣ Check User
            var user = await _externalService.GetUser(dto.UserId);
            if (user == null)
                return BadRequest("Invalid User");

            // 2️⃣ Check Product
            var product = await _externalService.GetProduct(dto.ProductId);
            if (product == null)
                return BadRequest("Invalid Product");

            // 3️⃣ Check Stock
            if (product.Stock < dto.Quantity)
                return BadRequest("Not enough stock");

            // 💰 4️⃣ Calculate Amount
            var totalAmount = product.Price * dto.Quantity;
            // 💳 5️⃣ Call Payment Service
            var paymentSuccess = await _externalService.ProcessPayment(dto.ProductId, totalAmount);

            if (!paymentSuccess)
                return BadRequest("Payment Failed");

            // 4️⃣ Create Order
            var order = new Order
            {
                UserId = dto.UserId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Status = "Placed"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // 📄 GET ORDERS
        [HttpGet("get-orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = _context.Orders.ToList();
            return Ok(orders);
        }
    }
}
