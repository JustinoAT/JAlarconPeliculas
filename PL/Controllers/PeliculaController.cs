using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PL.Models;
using System.Net.Http;

namespace PL.Controllers
{
    public class PeliculaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
           Pelicula pelicula = new Pelicula();
      
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                string url = "movie/popular?api_key=3dd259a0c589ea21f4302fa322b2b0fe";
                
                var responseTask = client.GetAsync(url);    
                responseTask.Wait();    
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();

                    dynamic resultJson = JObject.Parse(readTask.Result.ToString());                                    
                    readTask.Wait();
                    pelicula.Peliculas = new List<object>();
                    foreach (var resultItem in resultJson.results) 
                    {
                      Pelicula peliculaItem = new Pelicula();   
                      peliculaItem.IdMovie = resultItem.id;
                        peliculaItem.Descripcion = resultItem.overview;
                       peliculaItem.Nombre = resultItem.title;
                        peliculaItem.Fecha = resultItem.release_date;
                        peliculaItem.Imagen = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2" + resultItem.poster_path;
                        pelicula.Peliculas.Add(peliculaItem);
                    }
                }
            }
           
            return View(pelicula);
        }

        public ActionResult Favorite(int IdMovie, bool Favorite)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/");

                var json = new
                {
                    media_type = "movie",
                    media_id = (int)IdMovie,
                    favorite = Favorite

                };

                var postTask = client.PostAsJsonAsync("20522454/favorite?session_id=f35f2bd8d2d5dd53da06e5fb249feb625a4dc26d&api_key=3dd259a0c589ea21f4302fa322b2b0fe", json);
                postTask.Wait();
                var resultMovie = postTask.Result;
                if (Favorite)
                {
                    if (resultMovie.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Agregado a favoritos";
                    }
                }
                else
                {
                    if (resultMovie.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Eliminado de favoritos";
                    }

                }
                return PartialView("Modal");



            }

        }


        [HttpGet]
        public IActionResult GetFavorite()
        {
            Pelicula pelicula = new Pelicula();

            using (var client = new HttpClient())

            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/");
                string url = "20522454/favorite/movies?api_key=3dd259a0c589ea21f4302fa322b2b0fe&session_id=f35f2bd8d2d5dd53da06e5fb249feb625a4dc26d";

                var responseTask = client.GetAsync(url);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();

                    dynamic resultJson = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    pelicula.Peliculas = new List<object>();
                    foreach (var resultItem in resultJson.results)
                    {
                        Pelicula peliculaItem = new Pelicula();
                        peliculaItem.IdMovie = resultItem.id;
                        peliculaItem.Descripcion = resultItem.overview;
                        peliculaItem.Nombre = resultItem.title;
                        peliculaItem.Fecha = resultItem.release_date;
                        peliculaItem.Imagen = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2" + resultItem.poster_path;
                        pelicula.Peliculas.Add(peliculaItem);
                    }
                }
            }

            return View(pelicula);
        }


    }
}
