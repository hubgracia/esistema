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
using AcceptVerbsAttribute = System.Web.Mvc.AcceptVerbsAttribute;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using HttpPutAttribute = System.Web.Mvc.HttpPutAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

namespace isistema.Controllers
{
    public class RestauranteController : Controller
    {
        /// <summary>
        /// Retorna uma lista com todos os Restaurantes existentes e ativos.
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Retorna o restaurante com suas informações pelo seu id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Retorna a lista de horas e os dias do restaurante.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Get para  API que está sendo chamada. Retorna o model a ser alterado.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AlterarHoras(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44355/api/restaurante/");

                var responseTask = client.GetAsync("sethoras?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<hora>().Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Falha no modelo");
                }
            }
            return View();

        }
        /// <summary>
        /// Put para a API que está sendo chamada. 
        /// Envia para API uma lista de horas com os dias referentes a sua alteração.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hora"></param>
        /// <returns></returns>
        [Route("api/restaurante/sethoras?id=")]
        [AcceptVerbs("POST", "PUT", "PATCH")]
        [HttpPut]
        public ActionResult AlterarHoras(int id, List<hora> hora)
        {
            var model = new hora();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44355/api/restaurante/");

                var putTask = client.PutAsJsonAsync("sethoras/" + id.ToString(), hora);

                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("MenuHoras/" + id.ToString());
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Falha ao salvar");
                }
            }
            return View(hora);
        }
    }
}
