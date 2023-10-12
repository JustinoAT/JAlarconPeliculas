using ML;
using Microsoft.AspNetCore.Mvc;


namespace PL.Controllers
{
    public class DulceriaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Dulceria dulceria = new ML.Dulceria();
            dulceria.Dulcerias = new List<object>();
            ML.Result result = BL.Dulceria.GetAll();


            if (result.Correct)
            {
                dulceria.Dulcerias = result.Objects.ToList();
            }
            else
            {
                ViewBag.Message = result.ErrorMessage;

            }

            return View(dulceria);

        }
        public ActionResult AddCarrito(int idDulceria)
        {
            bool existe = false;
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            ML.Result result = BL.Dulceria.GetById(idDulceria);
            if (HttpContext.Session.GetString("Carrito") == null)
            {

                if (result.Correct)
                {
                    ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                    dulceria.Cantidad = 1;
                    carrito.Carritos.Add(dulceria);
                    //serializar carrito
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }

            }
            else
            {

                ML.Dulceria dulceria = (ML.Dulceria)result.Object;
                GetCarrito(carrito); //ya recupere el carrito
                foreach (ML.Dulceria dulceria1 in carrito.Carritos)
                {
                    if (dulceria.IdDulceria == dulceria1.IdDulceria)
                    {
                        dulceria1.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe == true)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }
                else
                {
                    dulceria.Cantidad = 1;
                    carrito.Carritos.Add(dulceria);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carritos));
                }

            }

            return RedirectToAction("GetAll");
        }
        public ML.Carrito GetCarrito(ML.Carrito carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));

            foreach (var obj in ventaSession)
            {
                ML.Dulceria objMateria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(obj.ToString());
                carrito.Carritos.Add(objMateria);
            }
            return carrito;
        }
        public ActionResult Carrito()
        {
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            if (HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }

        }
    }
}
