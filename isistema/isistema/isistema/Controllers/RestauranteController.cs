using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            WrapperModel model = new WrapperModel();

            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/");
                var responseTask = cliente.GetAsync($"restaurante/hora/{id}");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<WrapperModel>();
                    readJob.Wait();

                    model = readJob.Result;
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
      //  [HttpPut]
     //      public async Task AlterarHoras(int id) 
        public ActionResult AlterarHoras(int id)
        {
            WrapperModel.Resthora model = new WrapperModel.Resthora();
            using (var cliente = new HttpClient())   
            {
                cliente.BaseAddress = new Uri("https://localhost:44355/api/");
                var responseTask = cliente.PutAsJsonAsync<WrapperModel.Resthora>($"restaurante/sethoras/{id}",model);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readJob = result.Content.ReadAsAsync<WrapperModel.Resthora>();
                    readJob.Wait();

                    model = readJob.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "erro no servidor:StatusCode fail");

                }
            }
           // await this.AlterarHoras(id);

             return View("AlterarHoras", model);   
           
        }
        public ActionResult teste()
        {
            return View();
        }
    }
}