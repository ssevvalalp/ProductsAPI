﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task <IActionResult> GetProducts()
        {
           //if (_products == null)
           // {
           //     return NotFound();
           // }
           // return Ok( _products);

            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task <IActionResult> GetProducts(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            //var p = _products?.FirstOrDefault(i => i.ProductId == id);

            var p =await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);

            if (p == null)
            {
                return NotFound();
            }
            return Ok(p);

        }
            

    }
}
