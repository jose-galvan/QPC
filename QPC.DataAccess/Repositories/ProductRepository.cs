using QPC.Core.Models;
using QPC.Core.Repositories;

namespace QPC.DataAccess.Repositories
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
