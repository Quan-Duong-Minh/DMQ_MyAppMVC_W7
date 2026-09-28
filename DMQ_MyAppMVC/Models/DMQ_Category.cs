using System.ComponentModel.DataAnnotations;

namespace DMQ_MyAppMVC.Models
{
    public class DMQ_Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên danh mục tối đa 100 ký tự.")]
        public string Name { get; set; }
    }
}