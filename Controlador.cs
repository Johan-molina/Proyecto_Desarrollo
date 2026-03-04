using Microsoft.AspNetCore.Mvc;
using MiApiProductos.Data;
using MiApiProductos.Models;

namespace MiApiProductos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Productos.ToList());
        }

        [HttpPost]
        public IActionResult Post(Producto producto)
        {
            _context.Productos.Add(producto);
            _context.SaveChanges();
            return Ok(producto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Producto producto)
        {
            var prod = _context.Productos.Find(id);
            if (prod == null) return NotFound();

            prod.Nombre = producto.Nombre;
            prod.Precio = producto.Precio;
            prod.Stock = producto.Stock;

            _context.SaveChanges();
            return Ok(prod);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var prod = _context.Productos.Find(id);
            if (prod == null) return NotFound();

            _context.Productos.Remove(prod);
            _context.SaveChanges();
            return Ok();
        }
    }
}