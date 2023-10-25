using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult OlvideContraseña()
        {
            return View();
        }
        public IActionResult CambiarContraseña(string email)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Email = email;  
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {

            ML.Result result = BL.Login.UsuarioGetByEmail(email);

            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;

                var contraseña = Encoding.UTF8.GetBytes(password);
                var storedPassword = usuario.Contrasena;
                if (contraseña.SequenceEqual(storedPassword))
                {
                    //validacion de la contrasena
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ViewBag.Login = false;
                    ViewBag.Mensaje = "La contrasena es incorrecta";
                    return PartialView("modal");
                }
            }
            else
            {
                ViewBag.Login = false;
                ViewBag.Mensaje = "No existe la cuenta";
                return PartialView("modal");
            }
        }
        [HttpPost]
        public ActionResult CambiarContraseña(string email, string password)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Email = email;

            //encriptar 

            usuario.Contrasena = Encoding.UTF8.GetBytes(password);
            ML.Result result = BL.Login.Update(usuario);
            if (result.Correct)
            {
                ViewBag.Login = false;
                ViewBag.Mensaje = "La contraseña se actualizo correctamente";
                return PartialView("modal");
            }
            else
            {
                ViewBag.Login = false;
                ViewBag.Mensaje = "No se pudo actualizar la contraseña";
                return PartialView("modal");

            }

        }

        [HttpPost]
        public ActionResult Registrar(string username, string nombre, string email, string password)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Username = username;
            usuario.Nombre = nombre;
            usuario.Email = email;

            //encriptar 


            usuario.Contrasena = Encoding.UTF8.GetBytes(password);
            ML.Result result = BL.Login.Add(usuario);
            if (result.Correct)
            {
                ViewBag.Login = false;
                ViewBag.Mensaje = "El usuario se agrego correctamente, es hora de iniciar sesión";
                return PartialView("modal");
            }
            else
            {
                ViewBag.Login = false;
                ViewBag.Mensaje = "El usuario no se agrego";
                return PartialView("modal");

            }

        }
        private readonly IWebHostEnvironment _hostingEnvironment;

        public LoginController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public ActionResult CambiarPassword(string email)
        {
            //llamar al metodo
            string emailOrigen = "jus.delatorre48@gmail.com";
          

            MailMessage mailMessage = new MailMessage(emailOrigen, email, "Recuperar Contraseña", "<p>Correo para recuperar contraseña</p>");
            mailMessage.IsBodyHtml = true;

            //string contenidoHTML = System.IO.File.ReadAllText(@"C:\users\digis\Documents\IISExpress\Leonardo Escogido Bravo\Proyecto2023Ecommerce\PL\Views\Usuario\Email.html");

            //string contenidoHTML = System.IO.File.ReadAllText(Path.Combine("Views", "Usuario", "Email.html"));


            string contenidoHTML = System.IO.File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "Templates", "Email.html"));




            mailMessage.Body = contenidoHTML;
            //string url = "http://localhost:5057/Usuario/NewPassword/" + HttpUtility.UrlEncode(email)
            ;
            string url = "http://localhost:5146/Login/CambiarContrase%C3%B1a?email=" + HttpUtility.UrlEncode(email)
;
            mailMessage.Body = mailMessage.Body.Replace("{Url}", url);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "nzrsxnoohlrjnrtl");

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();

            ViewBag.Login = false;
            ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
            return PartialView("modal");
        }



    }
}
