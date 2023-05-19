using Microsoft.EntityFrameworkCore;
using OskarLAspNet.Contexts;
using OskarLAspNet.Models.Entities;
using System.Linq.Expressions;

namespace OskarLAspNet.Helpers.Repos
{
    public class ProductCategoryRepo : Repo<ProductCategoryEntity>
    {
        private readonly DataContext _context;
        public ProductCategoryRepo(DataContext context) : base(context)
        {
            _context = context;
        }

        public virtual async Task<ProductCategoryEntity> GetAsync(int categoryId)
        {
            return await _context.Set<ProductCategoryEntity>()
                .FirstOrDefaultAsync(category => category.Id == categoryId) ?? throw new ArgumentException("Invalid categoryId");
        }


    }
}
