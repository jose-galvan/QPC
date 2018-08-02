using QPC.Core.DTOs;
using QPC.Core.Repositories;
using QPC.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using QPC.Core.Models;
using System.Linq.Expressions;

namespace QPC.Api.Controllers
{
    [Authorize][RoutePrefix("api/products")]
    public class ProductsController : QualityControlWebApiBaseController
    {
        public ProductsController(IUnitOfWork unitOfWork, QualityControlFactory factory) 
            : base(unitOfWork, factory)
        {

        }

        [HttpGet][Route("{id:int}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var result = await _unitOfWork.ProductRepository.FindByIdAsync(id);
            if (result == null)
                return NotFound();

            var product =  _factory.Create(result);
            return Ok(product);
        }

        [HttpGet]
        [Route("")]
        public async Task<List<ProductDto>> Get()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            return products.Select(p => _factory.Create(p)).ToList();
        }

        [HttpGet][Route("{query:alpha}")]
        public async Task<List<ProductDto>> Get(string query)
        {
            query = query.ToLower();
            Expression<Func<Product,bool>> expr = (p => 
                        p.Name.ToLower().Contains(query) ||
                        p.Description.ToLower().Contains(query));
            var products = await _unitOfWork.ProductRepository.GetAsync(expr);
            return products.Select(p => _factory.Create(p)).ToList();
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Add([FromBody]ProductDto dto)
        {
            try
            {
                var user = await GetUserAsync();
                var product = _factory.Create(dto, user);
                _unitOfWork.ProductRepository.Add(product);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await LogExceptionAsync(ex);
                return BadRequest();
            }
            return Ok();
        }

        [Route("")]
        [HttpPut, HttpPatch]
        public async Task<IHttpActionResult> Update([FromBody]ProductDto dto)
        {
            var product = await _unitOfWork.ProductRepository.FindByIdAsync(dto.Id);
            if (product == null)
                return NotFound();
            try
            {
                var user = await GetUserAsync();
                product.Update(user, dto.Name, dto.Description);
                _unitOfWork.ProductRepository.Update(product);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await LogExceptionAsync(ex);
                return BadRequest();
            }
            return Ok();
        }

    }
}