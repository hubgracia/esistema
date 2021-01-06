using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace esistema.Models
{
    public class LojaCadastro
    {
        /// <summary>
        /// Identificador do local do cliente.
        /// </summary>
        [Required, StringLength(36)]
        public string id { get; set; }

        /// <summary>
        /// Status do local do cliente:
        /// 0=Sem delivery, 1=Delivery fechado, 2=Delivery aberto
        /// </summary>
        public byte status { get; set; }

        /// <summary>
        /// Nome do cliente
        /// </summary>
        [Required, StringLength(48)]
        public string Nome { get; set; }

        /// <summary>
        /// Número do DDD do telefone do cliente
        /// </summary>
        [Required, Range(11, 99)]
        public byte? ddd { get; set; }

        /// <summary>
        /// Número do telefone do cliente
        /// </summary>
        [Required, Range(10000000, 999999999)]
        public long? fone { get; set; }

        /// <summary>
        /// Ramal ou segundo telefone do cliente
        /// </summary>
        [Required, StringLength(9)]
        public string Fon2 { get; set; }

        /// <summary>
        /// eMail do cliente
        /// </summary>
        [Required, StringLength(40)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$")]
        public string Mail { get; set; }

        /// <summary>
        /// Senha do cliente, fornecer para novos cadastros com tamanho minimo de 6 caracteres.
        /// </summary>
        [Required, StringLength(16)]
        public string Senha { get; set; }

        /// <summary>
        /// Tipo de endereço: "Com" ou "Res"
        /// </summary>
        [Required, StringLength(3)]
        public string tEnd { get; set; }

        /// <summary>
        /// Abreviatura do prefixo do endereço: R., Pç, Av, ...
        /// Consulte a tabela de prefixos de endereços pela api/pEnd
        /// </summary>
        [Required, StringLength(3)]
        public string pEnd { get; set; }

        /// <summary>
        /// Nome do logradouro conforme consta na base CEP dos Correios
        /// </summary>
        [Required, StringLength(64)]
        public string Ende { get; set; }

        /// <summary>
        /// Número do endereço, se não houver informar número zero
        /// </summary>
        [Range(0, 999999999)]
        public int? Num { get; set; }

        /// <summary>
        /// Complemento: apt, casa, sala ...
        /// </summary>
        [Required, StringLength(24)]
        public string Cmp { get; set; }

        /// <summary>
        /// Bairro conforme consta na base CEP dos Correios
        /// </summary>
        [Required, StringLength(72)]
        public string Bairro { get; set; }

        /// <summary>
        /// Número do CEP do endereço
        /// </summary>
        [Required, StringLength(9)]
        public string Cep { get; set; }

        /// <summary>
        /// Referência: Condominio, Edificio ...
        /// </summary>
        [Required, StringLength(40)]
        public string Ref { get; set; }

        /// <summary>
        /// Cidade conforme consta na base CEP dos Correios
        /// </summary>
        [Required, StringLength(24)]
        public string Cid { get; set; }

        /// <summary>
        /// Sigla do estado
        /// </summary>
        [Required, StringLength(2)]
        public string Uf { get; set; }
    }

    /// <summary>
    /// Dados do cadastro do cliente
    /// </summary>
    public class caddados
    {
        /// <summary>
        /// Login do cliente - email
        /// </summary>
        [Required, StringLength(40)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$")]
        public string email { get; set; }

        /// <summary>
        /// Senha do cliente
        /// </summary>
        [Required, StringLength(16)]
        public string senha { get; set; }

        /// <summary>
        /// Dados do cadastro do cliente
        /// </summary>
    }
}
