using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace isistema.Models
{
    public class restauranteHora
    {
        public int restid { get; set; }

        public int cardapioid { get; set; }

     /*   public class Resthora
        {
            public List<Resthora> horas { get; set; }
            public int dia { get; set; }
            public string abre { get; set; }
            public string fecha { get; set; }
        }*/
    }

    public class Resthora
    {
        public int restid { get; set; }
        public List<Resthora> horas { get; set; }
        public int dia { get; set; }
        public string abre { get; set; }
        public string fecha { get; set; }
    }



}
    
