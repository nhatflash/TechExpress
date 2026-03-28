using System;
using System.Collections.Generic;
using TechExpress.Repository.Enums;

namespace TechExpress.Application.Dtos.Responses
{
    public class CartResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public CartStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public List<CartItemResponse> Items { get; set; } = [];
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class CartItemResponse
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public int AvailableStock { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public class CartWithPromotionsResponse : CartResponse
    {
        public decimal TotalDiscountAmount { get; set; }
        public decimal FinalTotalPrice => TotalPrice - TotalDiscountAmount;
        public new List<CartItemWithPromotionResponse> Items { get; set; } = [];
    }

    public class CartItemWithPromotionResponse : CartItemResponse
    {
        public decimal? DiscountValue { get; set; } // Ví dụ: 10 (%) hoặc 50000 (VND)
        public PromotionType? PromotionType { get; set; }
        public decimal DiscountAmountPerItem { get; set; }
        public new decimal SubTotal { get; set; } // Giá sau giảm: (UnitPrice - DiscountAmountPerItem) * Quantity
    }
}
