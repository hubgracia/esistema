using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace esistema.Models
{
    public class EntregaStatus
    {
        public string Loja { get; set; }

        [Required, StringLength(4)]
        public string codLoja { get; set; }

        [Required, StringLength(12)]
        public string stLoja { get; set; }

        [Required, StringLength(12)]
        public string stEntrega { get; set; }

        [Required, StringLength(12)]
        public string Observacao{ get; set; }

        [Required, StringLength(3)]
        public string qntEntregadores { get; set; }
}
}