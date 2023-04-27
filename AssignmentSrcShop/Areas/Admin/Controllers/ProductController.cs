using AssignmentSrcShop.Data;
using AssignmentSrcShop.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AssignmentSrcShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IWebHostEnvironment _he;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.Category).Include(s => s.SpecialTag).ToList());
        }

        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.Category).Include(c => c.SpecialTag).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                products = _db.Products.Include(c => c.Category).Include(c => c.SpecialTag).ToList();
            }
            return View(products);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTagName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["CategoryId"] = new SelectList(_db.Categories.ToList(), "Id", "ProductType");
                    ViewData["SpecialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTagName");
                    return View(product);
                }
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/NoImageAvailable.jpg";
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int? id)
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories.ToList(), "Id", "ProductType");
            ViewData["SpecialTagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTagName");
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.Category).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    product.Image = "Images/NoImageAvailable.jpg";
                }
                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.Category).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.Include(c => c.SpecialTag).Include(c => c.Category).Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
