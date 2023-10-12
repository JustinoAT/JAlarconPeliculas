using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EstadisticaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();
            ML.Result result = BL.Cine.GetAll();


            if (result.Correct)
            {
                cine.Cines = result.Objects.ToList();
                cine.Estadisctica = (ML.Estadisctica)result.Object;
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;

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

      


    }
}
