namespace Session_och_cookies.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public List<CartItem> Items { get; set; } = new();

        public int TotalItems =>

        Items.Sum(i => i.Quantity);

        public decimal TotalPrice =>

        Items.Sum(i => i.TotalPrice);

        public void Clear() { Items.Clear(); }

    }
}
