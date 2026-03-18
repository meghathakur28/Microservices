namespace OrderService.DTOs
{
    public class PlaceOrderDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
