﻿namespace eShopFlix.Models
{
    public class CartItemModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int CartId { get; set; }
    }
}
