using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class CuponController : Controller
    {
        private readonly IConfiguration _configuration;
        public CuponController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult GetAll()
        {
            DTO.CuponDTO cupon = new DTO.CuponDTO();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiCupon"]);

                    var task = client.GetAsync("GetAll");
                    task.Wait();

                    var taskResult = task.Result;

                    if (taskResult.IsSuccessStatusCode)
                    {
                        var contentResult = taskResult.Content.ReadAsAsync<DTO.ResultCupon>();
                        contentResult.Wait();

                        var result = contentResult.Result;
                        List<DTO.CuponDTO> listCupon = new List<DTO.CuponDTO>();

                        result.DataList.ForEach(i =>
                        {
                            var get = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.CuponDTO>(i.ToString());
                            listCupon.Add(get);
                        });

                        cupon.Cupones = listCupon;
                    }
                }
                return View(cupon);
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
        public IActionResult Form(int? idCupon)
        {
            DTO.CuponDTO cupon = new DTO.CuponDTO();
            try
            {
                if(idCupon != null)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiCupon"]);

                        var task = client.GetAsync("GetById?idCupon=" + idCupon);
                        task.Wait();
                        var taskResult = task.Result;
                        if (taskResult.IsSuccessStatusCode)
                        {
                            var result = taskResult.Content.ReadAsAsync<DTO.Result>();
                            DTO.CuponDTO model = JsonConvert.DeserializeObject<DTO.CuponDTO>(result.Result.Data.ToString());
                            cupon = model;
                            return View(cupon);
                        }
                        else
                        {
                            TempData["method"] = "GetAll";
                            TempData["controller"] = "Cupon";

                            ViewBag.Message = "Error al realizar la petición";
                            return PartialView("~/Views/Modal/_General.cshtml");
                        }
                    }
                }
                else
                {
                    return View(cupon);
                }
            }
            catch (Exception ex)
            {
                TempData["method"] = "GetAll";
                TempData["controller"] = "Cupon";

                ViewBag.Message = "Error: " + ex.Message;
                return PartialView("~/Views/Modal/_General.cshtml");
            }
        }

        [HttpPost]
        public IActionResult Form(DTO.CuponDTO cupon)
        {
            TempData["controller"] = "Cupon";
            try
            {
                if (cupon.IdCupon != 0)
                {
                    TempData["method"] = "GetAll";
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiCupon"]);

                        var task = client.PutAsJsonAsync("Update", cupon);
                        task.Wait();

                        var taskResult = task.Result;
                        if (taskResult.IsSuccessStatusCode)
                            ViewBag.Message = "Actualización exitosa";
                        else
                            ViewBag.Message = "Error al realizar la actualización";
                    }
                }
                else
                {
                    TempData["method"] = "Form";
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_configuration["ApiCupon"]);

                        var task = client.PostAsJsonAsync("Add", cupon);
                        task.Wait();

                        if (task.Result.IsSuccessStatusCode)
                            ViewBag.Message = "Registro exitoso";
                        else
                            ViewBag.Message = "Error al realizar el registro";
                    }
                }
                return PartialView("~/Views/Modal/_General.cshtml");
            }
            catch (Exception ex)
            {
                TempData["method"] = "GetAll";

                ViewBag.Message = "Error: " + ex.Message;
                return PartialView("~/Views/Modal/_General.cshtml");
            }
        }

        [HttpGet]
        public IActionResult Delete(int idCupon)
        {
            TempData["controller"] = "Cupon";
            TempData["method"] = "GetAll";
            try
            {
                using(HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiCupon"]);
                    var task = client.DeleteAsync("Delete/" + idCupon);

                    var taskResutl = task.Result;

                    if (taskResutl.IsSuccessStatusCode)
                        ViewBag.Message = "El registro ha sido eliminado de forma permanente";
                    else
                        ViewBag.Message = "No pudimos eliminar el registro";
                    return PartialView("~/Views/Modal/_General.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error: " + ex.Message;
                return PartialView("~/Views/Modal/_General.cshtml");
            }
        }
    }
}
