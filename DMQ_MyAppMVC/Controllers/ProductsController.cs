using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DMQ_MyAppMVC.Models;
using DMQ_MyAppMVC.ViewModels;

namespace DMQ_MyAppMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Static lists to store data in-memory for the session
        private static List<DMQ_Category> _categories = new List<DMQ_Category>
        {
            new DMQ_Category { Id = 1, Name = "Điện thoại" },
            new DMQ_Category { Id = 2, Name = "Laptop" },
            new DMQ_Category { Id = 3, Name = "Phụ kiện" }
        };

        private static List<DMQ_Product> _products = new List<DMQ_Product>();

        public ProductsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public IActionResult Index()
        {
            foreach (var p in _products)
            {
                p.DMQ_Category = _categories.FirstOrDefault(c => c.Id == p.CategoryId);
            }
            return View(_products);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var product = _products.FirstOrDefault(m => m.Id == id);
            if (product == null) return NotFound();

            product.DMQ_Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DMQ_ProductViewModel model)
        {
            if (model.SalePrice >= model.Price)
            {
                ModelState.AddModelError("SalePrice", "Giá khuyến mãi phải nhỏ hơn giá chuẩn.");
            }

            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadImage(model.ImageFile);

                int newId = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1;

                var product = new DMQ_Product
                {
                    Id = newId,
                    Name = model.Name,
                    Price = model.Price,
                    SalePrice = model.SalePrice,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Image = uniqueFileName,
                    DMQ_Category = _categories.FirstOrDefault(c => c.Id == model.CategoryId)
                };

                _products.Add(product);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_categories, "Id", "Name", model.CategoryId);
            return View(model);
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = _products.FirstOrDefault(m => m.Id == id);
            if (product == null) return NotFound();

            var model = new DMQ_ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Description = product.Description,
                CategoryId = product.CategoryId
            };

            ViewData["CategoryId"] = new SelectList(_categories, "Id", "Name", product.CategoryId);
            return View(model);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, DMQ_ProductViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var product = _products.FirstOrDefault(m => m.Id == id);
                if (product == null) return NotFound();

                product.Name = model.Name;
                product.Price = model.Price;
                product.SalePrice = model.SalePrice;
                product.Description = model.Description;
                product.CategoryId = model.CategoryId;
                product.DMQ_Category = _categories.FirstOrDefault(c => c.Id == model.CategoryId);

                if (model.ImageFile != null)
                {
                    product.Image = UploadImage(model.ImageFile);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_categories, "Id", "Name", model.CategoryId);
            return View(model);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = _products.FirstOrDefault(m => m.Id == id);
            if (product == null) return NotFound();

            product.DMQ_Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _products.FirstOrDefault(m => m.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return RedirectToAction(nameof(Index));
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}