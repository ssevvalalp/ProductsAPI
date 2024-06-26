﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // [Route("api/products")]
    public class ProductsController : ControllerBase
    {

        //private static List<Product>? _products;

        //injection
        private readonly ProductsContext _context;
        public ProductsController(ProductsContext context)
        {

            _context = context;

        }

        //public ProductsController()
        //{

        ////    _products = new List<Product>();
        ////    {

        ////        _products.Add(new Product { ProductId = 1, ProductName = "iphone 14", Price = 12000, IsActive = true });
        ////        _products.Add(new Product { ProductId = 2, ProductName = "iphone 15", Price = 14000, IsActive = true });
        ////    }

        //}


        [HttpGet]
        public async Task<IActionResult> GetProducts()
           //  public IActionResult GetProducts()
        {
            //if (_products == null)
            // {
            //     return NotFound();
            // }
            // return Ok( _products);

            var products = await _context.Products.Where(i => i.IsActive).Select(p =>ProductToDto(p)).ToListAsync();
            return Ok(products);
        }

        //localhost:5000/api/products/5 => GET
        [Authorize]
        [HttpGet("{id}")] //[HttpGet("api/[controller]{id}")]  
        public async Task<IActionResult> GetProduct(int? id)
            // public IActionResult GetProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var p = _products?.FirstOrDefault(i => i.ProductId == id);

            /*var p = await _context.Products.Select(p => ProductToDTO(p)).FirstOrDefaultAsync(i => i.ProductId == id);
            önce filtreleme yapmak gerekir where ile.  seçilen bir product yoksa bu durumda null reference hatası gelir.
             */
            var p = await _context.Products.Where(p => p.ProductId == id).Select(p => ProductToDto(p)).FirstOrDefaultAsync();
            if (p == null)
            {
                return NotFound();
            }
            return Ok(p);

        }

        [HttpPost]  
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            //201 status code
            return CreatedAtAction(nameof(GetProduct), new { id = entity.ProductId }, entity);
        }

        //localhost:5000/api/products/5 => PUT
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, Product entity) //entity güncellenmiş hali
        {
            if (id != entity.ProductId)
            {
                return BadRequest(); // örn: productid = 1 urlye gönderilen id = 2 
            }
            var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            product.ProductName = entity.ProductName;
            product.Price = entity.Price;
            product.IsActive = entity.IsActive;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch(Exception)
            {
                return NotFound();
            }

            //204 status code. Her sey yolunda güncelleme yapıldı geri değer döndürülmüyor.
            return NoContent();


        }

        [HttpDelete("{id}")] //id bilgisi zorunlu değil

        public async Task<IActionResult> DeleteProduct(int? id) //id bilgisi sadece body üzerinden de gönderilebilir
        {
            if(id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);
            
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception)
            {
                return NotFound();
            }
            return NoContent();
        }

        private static ProductDTO ProductToDto(Product p)
        { 
            var entity = new ProductDTO();
            if (p != null)
            {
                entity.ProductId = p.ProductId;
                entity.ProductName = p.ProductName;
                entity.Price = p.Price;
            }
            return entity;
        }
    }
}