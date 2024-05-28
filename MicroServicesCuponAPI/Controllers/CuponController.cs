using MicroServicesCuponAPI.DOT;
using MicroServicesCuponAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicesCuponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CuponController(AppDbContext context)
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
                var data = _context.Cupon.ToList();
                if(data != null)
                {
                    result.DataList = new List<object>();
                    foreach(var item in data)
                    {
                        DOT.Cupon cupon = new DOT.Cupon()
                        {
                            IdCupon = item.IdCupon,
                            Codigo = item.Codigo,
                            Descuento = item.Descuento,
                            CantidadMinima = item.CantidadMinima
                        };
                        result.DataList.Add(cupon);
                    }
                    return Ok(result);
                }
                else
                {
                    result.Success = false;
                    result.ErrorTxt = "Ocurrio un error al traer los datos";
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorTxt = ex.Message;
                result.Error = ex;

                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(DOT.Cupon cupon)
        {
            Result result = new Result();
            try
            {
                Models.Cupon cuponModel = new Models.Cupon();
                cuponModel.Codigo = cupon.Codigo;
                cuponModel.Descuento = cupon.Descuento;
                cuponModel.CantidadMinima = cupon.CantidadMinima;

                _context.Cupon.Add(cuponModel);

                int rowAffected = _context.SaveChanges();

                if (rowAffected > 0)
                    return Ok(result);
                else
                {
                    result.Success = false;
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorTxt = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(DOT.Cupon cupon)
        {
            Result result = new Result();
            try
            {
                if(cupon.IdCupon != 0)
                {
                    Models.Cupon cuponModel = new Models.Cupon();
                    cuponModel.IdCupon = cupon.IdCupon;
                    cuponModel.Codigo = cupon.Codigo;
                    cuponModel.Descuento = cupon.Descuento;
                    cuponModel.CantidadMinima = cupon.CantidadMinima;

                    _context.Cupon.Update(cuponModel);

                    int rowAffected = _context.SaveChanges();

                    if (rowAffected > 0) return Ok(result);
                    else
                    {
                        result.Success = false;
                        result.ErrorTxt = "Fallo al actualizar lainformación";
                        return BadRequest(result);
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorTxt = "Se requiere el ID del cupon a editar";
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorTxt = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpDelete]
        [Route("Delete/{idCupon}")]
        public IActionResult Delete(int idCupon)
        {
            Result result = new Result();
            try
            {
                _context.Cupon.Remove(new Models.Cupon { IdCupon = idCupon });

                int rowAffected = _context.SaveChanges();

                if (rowAffected > 0) return Ok(result);
                else
                {
                    result.Success = false;
                    result.ErrorTxt = "Ocurrio un error al eliminar el registro";
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorTxt = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int idCupon)
        {
            Result result = new Result();
            try
            {
                var data = _context.Cupon.FirstOrDefault(c => c.IdCupon == idCupon);

                if(data != null)
                {
                    DOT.Cupon cupon = new DOT.Cupon();
                    cupon.IdCupon = idCupon;
                    cupon.Codigo = data.Codigo;
                    cupon.Descuento = data.Descuento;
                    cupon.CantidadMinima = data.CantidadMinima;

                    result.Data = cupon;
                    return Ok(result);
                }
                else
                {
                    result.Success = false;
                    result.ErrorTxt = "No se encontro ningún registro asociado a: " + idCupon;
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorTxt = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetByCode")]
        public IActionResult GetByCode(string code)
        {
            Result result = new Result();
            try
            {
                var data = _context.Cupon.SingleOrDefault(c => c.Codigo == code);

                if(data != null)
                {
                    DOT.Cupon cupon = new DOT.Cupon()
                    {
                        IdCupon = data.IdCupon,
                        Codigo = code,
                        Descuento = data.Descuento,
                        CantidadMinima = data.CantidadMinima
                    };

                    result.Data = cupon;
                    return Ok(result);
                }
                else
                {
                    result.Success = false;
                    result.ErrorTxt = "No se encontro el registro: " + code;
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorTxt = ex.Message;
                result.Error = ex;
                return BadRequest(result);
            }
        }
    }
}
