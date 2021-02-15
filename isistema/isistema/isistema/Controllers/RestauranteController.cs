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
        //GET para a tabela de horas
     //   [HttpGet]
        public ActionResult HoraLista()
        {

            IEnumerable<hora> horax = null;

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/restaurante/");
                //GET 
                var responseTask = cliente.GetAsync("{id}");
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {

                    var readJob2 = result.Content.ReadAsAsync<IList<hora>>();
                    readJob2.Wait();
                    horax = readJob2.Result;
                }
                else
                {
                //    horas = Enumerable.Empty<hora>();

                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }
                
            }

    
            return View(horax);
            
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

                    //var readJob2 = result.Content.ReadAsAsync<hora>();
                    //readJob2.Wait();
                    //horas = (IEnumerable<hora>)readJob2.Result;




                }
                else
                {
                    //  horas = Enumerable.Empty<restaurante>();

                    ModelState.AddModelError(string.Empty, "erro no servidor");
                }

            }
            
            return View(rest);

        }



        public ActionResult Edit(int id) 
        {
            return View();    
        }

    }
}