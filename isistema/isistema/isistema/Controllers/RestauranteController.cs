using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using isistema.Models;
using Newtonsoft.Json;
using static isistema.Models.hora;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using HttpPutAttribute = System.Web.Mvc.HttpPutAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

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
        [HttpGet]
        public ActionResult AlterarHoras(int id)
        {
            WrapperModel.Resthora model = new WrapperModel.Resthora();
            using (var client = new HttpClient())   
            {
                client.BaseAddress = new Uri("https://localhost:44355/api/restaurante/");

                var responseTask = client.GetAsync("sethoras?id=" + id.ToString());
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
                    ModelState.AddModelError(string.Empty, "Error modelo inválido");

                }
            }
             return View("AlterarHoras", model);   
        }
        public ActionResult teste()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AlterarHorasTeste(int id)
        {
            IEnumerable<WrapperModel.Resthora> restx = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44355/api/restaurante/");

                var responseTask = client.GetAsync("sethoras?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<WrapperModel.Resthora>().Result;
                    readTask.Wait();
                    restx = (IEnumerable<WrapperModel.Resthora>)readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Falha no modelo");
                }
            }
            return View(restx);

        }
        [Route("sethoras/{restid}")]
        [HttpPost]
        
        public ActionResult AlterarHorasTeste(int id, WrapperModel.Resthora hora)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44355/api/restaurante/");

                //     var responseTask = client.GetAsync("sethoras?id=" + id.ToString());
                // responseTask.Wait();
               // var result = responseTask.Result;
                var putTask = client.PostAsJsonAsync<WrapperModel.Resthora>("sethoras/" + id.ToString() , hora);
                putTask.Wait();
                var result = putTask.Result;


                if (result.IsSuccessStatusCode)
                {
                    //HTTP POST
                    

                    return RedirectToAction("MenuHoras/" + id.ToString() );
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "não foi salvo");
                }
            }
            return View(hora);
        }
    }
    
}
