namespace Session_och_cookies.Models
{
    public class CartItem

    {
        public int CartItemId { get; set; }

        public Product Product { get; set; } = new();

        public int Quantity { get; set; }

        public decimal TotalPrice =>

        Product.Price * Quantity;

        public void IncreaseQuantity() { Quantity++; }

        public void DecreaseQuantity()
        {
            if (Quantity > 0)
            {

                Quantity--;

            }
        }
    }
}
