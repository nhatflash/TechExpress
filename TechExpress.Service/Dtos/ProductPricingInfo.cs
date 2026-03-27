namespace TechExpress.Service.Dtos;

public class ProductPricingInfo
{
    public Guid ProductId { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal FinalPrice { get; set; }
    public Guid? PromotionId { get; set; }
    public string? PromotionName { get; set; }
}