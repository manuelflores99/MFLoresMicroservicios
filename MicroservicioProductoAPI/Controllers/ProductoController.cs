using MicroservicioProductoAPI.DTO;
using MicroservicioProductoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioProductoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            Result result = new Result();
            try
            {
                var data = _context.Producto.ToList();

                if(data.Count > 0)
                {
                    List<ProductoDTO> list = new List<ProductoDTO>();

                    foreach(var item in data)
                    {
                        ProductoDTO producto = new ProductoDTO
                        {
                            IdProducto = item.IdProducto,
                            Nombre = item.Nombre,
                            Precio = item.Precio,
                            Categoria = item.Categoria,
                            UrlImagen = item.UrlImagen
                        };

                        list.Add(producto);
                    }
                    result.Data = list;

                    return Ok(result);
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "No hay datos";
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetById/{idProducto}")]
        public IActionResult GetById(int idProducto)
        {
            Result result = new Result();
            try
            {
                var data = _context.Producto.SingleOrDefault(p => p.IdProducto == idProducto);

                if(data != null)
                {
                    ProductoDTO producto = new ProductoDTO
                    {
                        IdProducto = data.IdProducto,
                        Nombre = data.Nombre,
                        Precio = data.Precio,
                        Categoria = data.Categoria,
                        UrlImagen = data.UrlImagen
                    };

                    result.Data = producto;

                    return Ok(result);
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "No se encontro información";
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] ProductoDTO producto)
        {
            Result result = new Result();
            try
            {
                Producto productoEF = new Producto();

                productoEF.Nombre = producto.Nombre;
                productoEF.Precio = producto.Precio;
                productoEF.Categoria = producto.Categoria;
                productoEF.UrlImagen = producto.UrlImagen;

                _context.Producto.Add(productoEF);

                int rowAffecred = _context.SaveChanges();

                if(rowAffecred > 0) return Ok(result);
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "No se realizo el registro correctamente";
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody] ProductoDTO producto)
        {
            Result result = new Result();
            try
            {
                Producto productoEF = new Producto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Categoria = producto.Categoria,
                    UrlImagen = producto.UrlImagen
                };

                _context.Producto.Update(productoEF);

                int rowAffected = _context.SaveChanges();

                if( rowAffected > 0 ) return Ok(result);
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "Error al actualizar la información";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpDelete]
        [Route("Delete/{idProducto}")]
        public IActionResult Delete(int idProducto)
        {
            Result result = new Result();
            try
            {
                _context.Producto.Remove(new Producto { IdProducto = idProducto } );

                int rowAffected = _context.SaveChanges();

                if(rowAffected > 0 ) return Ok(result);
                else
                {
                    result.Success = false;
                    result.ErrorMessage = "Error al eliminar el registro";
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }
    }
}
