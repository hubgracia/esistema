using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using esistema.Models;
using RoutePrefixAttribute = System.Web.Mvc.RoutePrefixAttribute; //Routeprefix com problema,pesquisar

namespace esistema.Controllers
{
    [RoutePrefix("api/cadastroLoja")]
    public class LojaCadastroController : ApiController
    {
        private LojaCadastro LjCadastro(string id)
        {
            LojaCadastro cadastrox = new LojaCadastro { };

            cadastrox.id = "";

            return cadastrox != null ? cadastrox : null; //Erro na hora de compilar,era necessário valor de retorno
        }
        public LojaCadastro Get(string id)
        {
            LojaCadastro cadastrox = LjCadastro(id);
            if (cadastrox.id == "")
            {
                HttpError err = new HttpError("Cadastro inexistente");
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));

            }
            else
            {
                return cadastrox;
            }
            
        }  
    }
}