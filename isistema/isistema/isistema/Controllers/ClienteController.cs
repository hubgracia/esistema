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
        // GET para restaurante
        public ActionResult restaurantes()
        {
            IEnumerable<restaurante> restaurante = null;
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/");
                //GET 
                var responseTask = cliente.GetAsync("restaurante");
                var result = responseTask.Result;
           
                if (result.IsSuccessStatusCode)
                {
                    
                    var readJob = result.Content.ReadAsAsync<IList<restaurante>>();
                    readJob.Wait();
                    restaurante = readJob.Result;
                }
                else
                {
                    restaurante = Enumerable.Empty<restaurante>();
                    
                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }
            }
            return View(restaurante);
        }
        //GET para a tabela de horas
        public ActionResult horas(int? id)
        {
            
            IEnumerable<restaurante>restx = null;
            
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/");
                //GET 
                var responseTask = cliente.GetAsync("restaurante");
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {

                    var readJob = result.Content.ReadAsAsync <IList<restaurante>>();
                    readJob.Wait();
                    restx = readJob.Result;
                }
                else
                {
                    restx = Enumerable.Empty<restaurante>();

                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }
            }
          //  return View("horas", new { id = 1 });
            return View();
        }
    }
}