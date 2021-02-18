using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using isistema.Models;
using Newtonsoft.Json;
using static isistema.Models.hora;

namespace isistema.Controllers
{
    public class RestauranteController : Controller
    {
        // GET para restaurante
        public ActionResult RestLista()
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

        public ActionResult MenuHoras(int id)
        {
            JuntaModel model = new JuntaModel();
            restauranteHora restaurante = new restauranteHora();
            restauranteHora.Resthora horax = null;

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/");
                var responseTask = cliente.GetAsync($"restaurante/hora/{id}");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<JuntaModel>();
                    readJob.Wait();

                    var readJob2 = result.Content.ReadAsAsync<restauranteHora.Resthora>();
                    readJob.Wait();

                    model = readJob.Result;
                    horax = readJob2.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }
                
            }    
            return View(model);          
        }

        public ActionResult RestSelecionado(int id)
        {
           restaurante rest = null;           

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/");
              
                var responseTask = cliente.GetAsync($"restaurante/{id}");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {

                    var readJob = result.Content.ReadAsAsync<restaurante>();
                    readJob.Wait();
                    rest = readJob.Result;
                }
                else
                {   
                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }
            }
            return View(rest);
        }

        public ActionResult Edit(int id) 
        {
            return View();    
        }
        public ActionResult teste()
        {
            return View();
        }
    }
}