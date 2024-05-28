using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult GetAll()
        {
            DTO.ProductoDTO producto = new DTO.ProductoDTO();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiProducto"]);

                    var task = client.GetAsync("GetAll");
                    task.Wait();

                    var taskResult = task.Result;

                    if (taskResult.IsSuccessStatusCode)
                    {
                        var getContent = taskResult.Content.ReadAsAsync<DTO.Result>();
                        getContent.Wait();

                        var result = getContent.Result;
                        if (result.Success)
                        {
                            var list = JsonConvert.DeserializeObject<List<object>>(result.Data.ToString());

                            List<DTO.ProductoDTO> productos = new List<DTO.ProductoDTO>();

                            list.ForEach(i =>
                            {
                                productos.Add(JsonConvert.DeserializeObject<DTO.ProductoDTO>(i.ToString()));
                            });

                            producto.Productos = productos;
                        }
                    }
                    return View(producto);
                }
            }
            catch (Exception ex)
            {
                TempData["method"] = "Index";
                TempData["controller"] = "Home";

                ViewBag.Message = "Error: " + ex.Message;
                return PartialView("~/Views/Modal/_General.cshtml");
            }
        }

        [HttpGet]
        public IActionResult Form(int? idProducto)
        {
            TempData["controller"] = "Producto";
            TempData["method"] = "GetAll";
            try
            {
                DTO.ProductoDTO producto = new DTO.ProductoDTO();
                if(idProducto != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiProducto"]);

                        var task = client.GetAsync("GetById/" + idProducto);
                        task.Wait();

                        var taskResult = task.Result;

                        if (taskResult.IsSuccessStatusCode)
                        {
                            var getInfo = taskResult.Content.ReadAsAsync<DTO.Result>();

                            producto = JsonConvert.DeserializeObject<DTO.ProductoDTO>(getInfo.Result.Data.ToString());
                        }
                        else
                        {
                            ViewBag.Message = "Error al realizar la petición";
                            return PartialView("~/Views/Modal/_General.cshtml");
                        }
                    }
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return PartialView("~/Views/Modal/_General.cshtml");
            }
        }

        [HttpPost]
        public IActionResult Form(DTO.ProductoDTO producto)
        {
            TempData["controller"] = "Producto";
            TempData["method"] = "GetAll";
            try
            {
                if(producto.IdProducto != 0)
                {
                    using(HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiProducto"]);

                        var task = client.PutAsJsonAsync("Update", producto);

                        var taskResult = task.Result;

                        if (taskResult.IsSuccessStatusCode)
                            ViewBag.Message = "Producto actualizado";
                        else
                            ViewBag.Message = "Cambios no guardados";
                    }
                }
                else
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiProducto"]);

                        var task = client.PostAsJsonAsync("Add", producto);
                        task.Wait();
                        var taskReult = task.Result;
                        if (taskReult.IsSuccessStatusCode)
                            ViewBag.Message = "Se realizado el registro correctamente";
                        else
                            ViewBag.Message = "No pudimos realizar el registro";
                    }
                }
                return PartialView("~/Views/Modal/_General.cshtml");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return PartialView("~/Views/Modal/_General.cshtml");
            }
        }

        [HttpGet]
        public IActionResult Delete(int idProducto)
        {
            TempData["controller"] = "Producto";
            TempData["method"] = "GetAll";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiProducto"]);

                    var task = client.DeleteAsync("Delete/" + idProducto);
                    task.Wait();

                    var taskResult = task.Result;

                    if (taskResult.IsSuccessStatusCode)
                        ViewBag.Message = "Ha sido eliminado de forma permanente";
                    else
                        ViewBag.Message = "Error al realizar la aplicación";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
            }
            return PartialView("~/Views/Modal/_General.cshtml");
        }
    }
}
