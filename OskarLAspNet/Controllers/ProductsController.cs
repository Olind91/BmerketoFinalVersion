using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OskarLAspNet.Helpers.Services;
using OskarLAspNet.Models.Dtos;
using OskarLAspNet.Models.ViewModels;

namespace OskarLAspNet.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly TagService _tagService;
        private readonly ProductCategoryService _productCategoryService;

        public ProductsController(ProductService productService, TagService tagService, ProductCategoryService productCategoryService)
        {
            _productService = productService;
            _tagService = tagService;
            _productCategoryService = productCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }




        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await _tagService.GetTagsAsync();


            //TEST
            var categorySelectList = await _productCategoryService.GetCategorySelectListAsync();
            ViewBag.CategorySelectList = categorySelectList;


            return View();
        }



        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(ProductRegVM viewModel, string[] tags)
        {
            if (ModelState.IsValid)
            {

                var product = await _productService.CreateAsync(viewModel);
                if (product != null)
                {
                    if (viewModel.Image != null)
                        await _productService.UploadImageAsync(product, viewModel.Image);
                        await _productService.AddProductTagsAsync(viewModel, tags);
                 
                    return RedirectToAction("Index");
                }
                //TEST
                
                ModelState.AddModelError("", "Something Went Wrong.");
            }             
            
            ViewBag.Tags = await _tagService.GetTagsAsync(tags);
            
            return View(viewModel);
        }




        public async Task<IActionResult> ProductDetails(string articleNumber)
        {
            var product = await _productService.GetByArticleNumberAsync(articleNumber);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductDetailViewModel
            {
                ArticleNumber = product.ArticleNumber,
                Name = product.ProductName,
                Price = product.Price,
                Description = product.ProductDescription,
                ImageUrl = product.ImageUrl
            };

            return View(viewModel);
        }

    }
}
