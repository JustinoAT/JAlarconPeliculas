using ML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;


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
        public ActionResult ClearCarrito()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Carrito");
        }
        public ActionResult GenerarPDF()
        {
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            ML.Carrito carrito = new ML.Carrito();
            carrito.Carritos = new List<object>();
            GetCarrito(carrito);

            Document document = new Document();

            PdfPTable tblProductos = new PdfPTable(4);
            tblProductos.WidthPercentage = 100;

            PdfPCell celdaNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            celdaNombre.BorderWidth = 0;
            celdaNombre.BorderWidthBottom = 0.75f;
            PdfPCell clDescripcion = new PdfPCell(new Phrase("Descripción", _standardFont));
            clDescripcion.BorderWidth = 0;
            clDescripcion.BorderWidthBottom = 0.75f;
            PdfPCell celdaCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
            celdaCantidad.BorderWidth = 0;
            celdaCantidad.BorderWidthBottom = 0.75f;
            PdfPCell celdaPrecio = new PdfPCell(new Phrase("Precio", _standardFont));
            celdaPrecio.BorderWidth = 0;
            celdaPrecio.BorderWidthBottom = 0.75f;
            int total = 0;
            foreach (ML.Dulceria dulce in carrito.Carritos)
            {
                celdaNombre = new PdfPCell(new Phrase(dulce.Nombre, _standardFont));
                celdaNombre.BorderWidth = 0;
                celdaCantidad = new PdfPCell(new Phrase(dulce.Cantidad.ToString(), _standardFont));
                celdaCantidad.BorderWidth = 0;
                celdaPrecio = new PdfPCell(new Phrase(dulce.Precio.ToString(), _standardFont));
                total += (dulce.Cantidad * int.Parse(dulce.Precio.ToString("0")));
                celdaPrecio.BorderWidth = 0;
                tblProductos.AddCell(clDescripcion);
                tblProductos.AddCell(celdaNombre);
                tblProductos.AddCell(celdaCantidad);
                tblProductos.AddCell(celdaPrecio);
            }

            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();
            document.Add(new Paragraph("Productos"));
            document.Add(Chunk.NEWLINE);
            document.Add(tblProductos);
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph("Total ($): " + total));
       
            document.Close();

            HttpContext.Session.Clear();
            

            return File(memoryStream.ToArray(), "application/pdf", "ReciboCompra-" + DateTime.Now + ".pdf");
        

        }
        public ActionResult FinalizarCompra()
        {

            HttpContext.Session.Clear();

            return RedirectToAction("GetAll");


        }
    }
}
