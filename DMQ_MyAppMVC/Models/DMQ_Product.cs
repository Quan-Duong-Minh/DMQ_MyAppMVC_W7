using System.ComponentModel.DataAnnotations;

namespace DMQ_MyAppMVC.Models
{
    public class DMQ_Product : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "Tên sản phẩm phải từ 6 đến 150 ký tự.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ảnh sản phẩm.")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống.")]
        [Range(100000, double.MaxValue, ErrorMessage = "Giá sản phẩm phải từ 100.000 trở lên.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Giá khuyến mãi không được để trống.")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi không được âm.")]
        public float SalePrice { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống.")]
        [StringLength(1500, ErrorMessage = "Mô tả không được vượt quá 1500 ký tự.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục sản phẩm.")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        public virtual DMQ_Category? DMQ_Category { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SalePrice >= Price)
            {
                yield return new ValidationResult("Giá khuyến mãi phải nhỏ hơn giá chuẩn.", new[] { nameof(SalePrice) });
            }

            if (!string.IsNullOrEmpty(Description))
            {
                var sensitiveWords = new[] { "die", "admin", "fack" };
                foreach (var word in sensitiveWords)
                {
                    if (Description.Contains(word, StringComparison.OrdinalIgnoreCase))
                    {
                        yield return new ValidationResult($"Mô tả chứa từ nhạy cảm không được phép: '{word}'.", new[] { nameof(Description) });
                    }
                }
            }
        }
    }
}