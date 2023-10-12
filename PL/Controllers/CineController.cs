using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
           cine.Cines = new List<object>();
            ML.Result result = new ML.Result();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5290/api/Cine/");
                var responseTask = client.GetAsync("GetAll");
                responseTask.Wait();
                var resultServicio = responseTask.Result;
                if (resultServicio.IsSuccessStatusCode)
                {
                    var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resultMateria in readTask.Result.Objects)
                    {
                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultMateria.ToString());
                        cine.Cines.Add(resultItemList);
                    }
                }
            }

            return View(cine);
        }
        [HttpPost]
        public IActionResult GetAll(ML.Cine cine)
        {
            ML.Result result = BL.Cine.GetAll();
            cine.Cines = result.Objects;

            return View(cine);
        }


        [HttpGet]
        public IActionResult Form(int? IdCine)
        {
            ML.Cine cine = new ML.Cine();
            cine.Zona = new ML.Zona();
            ML.Result Zonas = BL.Zona.GetAll();

            if (IdCine != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5290/api/Cine/");
                    var responseTask = client.GetAsync("GetById/"+IdCine);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                  
                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(readTask.Result.Object.ToString());
                        cine = resultItemList;
                        cine.Zona.Zonas = Zonas.Objects;

                    }
                    
                }
            }
            else
            {
                cine.Zona.Zonas = Zonas.Objects;
            }

            return View(cine);
        }

        [HttpPost]
        public IActionResult Form(ML.Cine cine)
        {
            cine.Estadisctica = new ML.Estadisctica();
            cine.Cines = new List<object>();
            cine.Zona.Zonas = new List<object>();
            cine.Zona.Nombre = "";
            ML.Result result = new ML.Result();
            if (cine.IdCine == 0 || cine.IdCine == null)
            {
               
                using (var client = new HttpClient())
                {
                    var responseTask = client.PostAsJsonAsync<ML.Cine>("http://localhost:5290/api/Cine/Add/", cine);
                    responseTask.Wait();

                    var resultApi = responseTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                        ViewBag.Mensaje = "Cine agregado correctamente";
                    }
                    else
                    { 
                    result.Correct = false;
                    ViewBag.Mensaje = "No se pudo agregar el cine, Error en: " + result.ErrorMessage;
                    }
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    var responseTask = client.PutAsJsonAsync<ML.Cine>("http://localhost:5290/api/Cine/Update?IdCine=" + cine.IdCine ,cine);
                    responseTask.Wait();

                    var resultApi = responseTask.Result;
                    if (resultApi.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                        ViewBag.Mensaje = "Cine actualizado correctamente";
                    }
                    else
                    {
                        result.Correct = false;
                        ViewBag.Mensaje = "No se pudo actualizar el cine, Error en: " + result.ErrorMessage;
                    }
                }
            }


            return PartialView("Modal");


        }




        [HttpGet]
        public ActionResult Delete(int IdCine)
        {
            ML.Cine cine = new ML.Cine();
            ML.Result result = new ML.Result();

            using (var client = new HttpClient())
            {

               
                var responseTask = client.DeleteAsync("http://localhost:5290/api/Cine/Delete/" + IdCine);
                responseTask.Wait();
                var resultApi = responseTask.Result;
                if (resultApi.IsSuccessStatusCode)
                {
                    result.Correct = true;
                }
                if (result.Correct)
                {
                    ViewBag.Mensaje = "Cine eliminado correctamente";
                }
                else
                {
                    ViewBag.Mensaje = "No se logro eliminar el cine: " + result.ErrorMessage;
                }

                return PartialView("Modal");
            }
        }

    }
}
