using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using isistema.Models;
using Newtonsoft.Json;

namespace isistema.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult restaurantes()
        {

            IEnumerable<restauranteHora> restaurante = null;
            IEnumerable<Resthora> horas = null;
            //  List<Resthora> Resth = new List<Resthora>();
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/");
                //GET 
                var responseTask = cliente.GetAsync("restaurante");
                
                var result = responseTask.Result;
                

                if (result.IsSuccessStatusCode)
                {
                    
                    var readJob = result.Content.ReadAsAsync<IList<restauranteHora>>();
                    readJob.Wait();
                    restaurante = readJob.Result;
                }
                else
                {
                    restaurante = Enumerable.Empty<restauranteHora>();
                    
                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }
            }
            return View(restaurante);
        }

        public ActionResult horas(int id)
        {
            IEnumerable<Resthora> horas = null;

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/restaurante/");

                var responseTask = cliente.GetAsync("horas");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<IList<Resthora>>();
                    readJob.Wait();
                    horas = readJob.Result;
                }
                else
                {
                    horas = Enumerable.Empty<Resthora>();
                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }
            }
            return View(horas);
        }
    }
}