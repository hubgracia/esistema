using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace isistema.Models
{


    public class restaurante
    {
        //   public int id { get; set; }

        /// <summary>
        /// Identificador do restaurante
        /// </summary>
        // [Key]
        public int restid { get; set; }

        /// <summary>
        /// Identificador do cardapio Delivery, para usar na API cardapio.
        /// Quando retornar zero, significa que o restaurante não tem delivery opção "del".
        /// Quando maior que zero é o cardapio usado na opção "del".
        /// obs1: O cardapio é geral, ou seja, inclui todos os produtos delivery com seus preços.
        /// obs2: Para ter o menu atual do restaurante use a API restmenu que retorna os produtos ativos com valor atual.
        /// </summary>
        public int cardapioid { get; set; }

        /// <summary>
        /// Identificador do cardapio de balcão local, para usar na API cardapio.
        /// Quando retornar zero, significa que o restaurante não tem opções "loc" e "via".
        /// Quando maior que zero é usado nas opções "loc" e "via".
        /// OBS1: Nos pedidos "loc" desconsiderar na montagem dos produtos o AcV (Embalagem para VIAGEM).
        /// OBS2: Nos pedidos "via" considerar o AcV.
        /// obs3: O cardapio é geral, ou seja, inclui todos os produtos com seus preços.
        /// obs4: Para ter o menu atual do restaurante use a API restmenu que retorna os produtos ativos com valor atual.
        /// </summary>
        public int cardapiolocid { get; set; }

        /// <summary>
        /// Nome do restaurante.
        /// </summary>
        [Required, StringLength(36)]
        public string restNome { get; set; }

        /// <summary>
        /// Número do CEP do restaurante.
        /// </summary>
        [Required, StringLength(9)]
        public string restCep { get; set; }

        /// <summary>
        /// Endereço do restaurante.
        /// </summary>
        [Required, StringLength(64)]
        public string restEnde { get; set; }

        /// <summary>
        /// Bairro do restaurante.
        /// </summary>
        [Required, StringLength(56)]
        public string restBairro { get; set; }

        /// <summary>
        /// Cidade do restaurante.
        /// </summary>
        [Required, StringLength(24)]
        public string restCid { get; set; }

        /// <summary>
        /// Sigla do estado do restaurante.
        /// </summary>
        [Required, StringLength(2)]
        public string restUf { get; set; }

        /// <summary>
        /// CNPJ do restaurante.
        /// </summary>
        [StringLength(24)]
        public string cnpj { get; set; }

        /// <summary>
        /// Horario que inicia hoje o Delivery do restaurante.
        /// </summary>
        [Required, StringLength(5)]
        public string restInicio { get; set; }

        /// <summary>
        /// Horario que encerra hoje o Delivery do restaurante.
        /// </summary>
        [Required, StringLength(5)]
        public string restFim { get; set; }

        /// <summary>
        /// Tempo de entrega atual em minutos.
        /// Se for ZERO o delivery está FECHADO e se for NEGATIVO o restaurante está chaveado.
        /// Quando retornar chaveado para se saber qual restaurante atende, se houver, somente informando o local, ou seja, get api/restaurante/{locid} ou post api/restaurante com endereço.
        /// </summary>
        [Range(-1, 999999999)]
        public int Tempo { get; set; }

        /// <summary>
        /// Taxa de entrega do Delivery para o local.
        /// No get api/restaurante e api/restaurante/{id} retorna sempre zerada, pois a taxa varia de acordo com o local.
        /// Para saber qual a taxa de entrega somente informando o local, ou seja, get api/restaurante/{locid} ou post api/restaurante com endereço.
        /// </summary>
        [Range(0, 999999999)]
        public decimal TxEnt { get; set; }

        /// <summary>
        /// Formas de pagamento aceitas no Delivery do restaurante.
        /// (string com os codigos das formas de pagamento separados por virgula).
        /// </summary>
        [Required, StringLength(200)]
        public string pgtoCod { get; set; }

        /// <summary>
        /// Área do restaurante base retornada pela api/restaurante/cep, para ser enviado no post api/pedido.
        /// </summary>
        [StringLength(16)]
        public string restarea { get; set; }

        /// <summary>
        /// Horario que abre hoje o balcão do restaurante, pedidos para consumir ou viagem.
        /// </summary>
        [Required, StringLength(5)]
        public string locInicio { get; set; }

        /// <summary>
        /// Horario que encerra hoje o balcão do restaurante, pedidos para consumir ou viagem.
        /// </summary>
        [Required, StringLength(5)]
        public string locFim { get; set; }

        /// <summary>
        /// Status do balcão do restaurante, pedidos para consumir ou viagem.
        /// Se for ZERO o balcão está FECHADO, maior que ZERO está ABERTO.
        /// </summary>
        public int locStatus { get; set; }

        /// <summary>
        /// Valor mínimo para pedidos delivery.
        /// </summary>
        [Range(0, 999999999)]
        public decimal delmin { get; set; }

        /// <summary>
        /// Valor mínimo para pedidos no local (viagem ou consumir no local).
        /// </summary>
        [Range(0, 999999999)]
        public decimal locmin { get; set; }

        /// <summary>
        /// Identifica a cor do feijão, preto ou marrom, do restaurante para exibir a imagem correta.
        /// Utilizar na api/cardapio image_urlfm para feijão marrom e image_urlfp para feijão preto.
        /// Obs: Nos produtos que não levam feijão as imagens apontadas pelas image_url são iguais.
        /// </summary>
        [StringLength(6)]
        public string feijao { get; set; }

        /// <summary>
        /// Lista dos horarios de abertura e fechamento do restaurante.
        /// </summary>
        public List<hora> horas { get; set; }

    }

    /// <summary>
    /// Horarios de abertura e fechamento do restaurante considerando dia: 1=Dom, 2=Seg, ... 7=Sab.
    /// Obs1: Horários locais respeitando os fusos de cada região.
    /// Obs2: Quando abertura for "23:45" significa que o restaurante não abre nesse dia.
    /// </summary>

    public class hora
    {
        public List<hora> horas { get; set; }
        public int dia { get; set; }
        public string abre { get; set; }
        public string fecha { get; set; }
        //    public int restid { get; internal set; }

    }

    /// <summary>
    /// Endereço de cliente.
    /// </summary>
    public class endereco
    {
        /// <summary>
        /// Abreviatura do prefixo do endereço: R., Pç, Av, ...
        /// Consulte a tabela de prefixos de endereços pela api/pEnd.
        /// </summary>
        [Required, StringLength(3)]
        public string pEnd { get; set; }

        /// <summary>
        /// Nome do logradouro conforme consta na base CEP dos Correios.
        /// </summary>
        [Required, StringLength(64)]
        public string Ende { get; set; }

        /// <summary>
        /// Número do endereço, se não houver informar número zero.
        /// </summary>
        [Range(0, 999999999)]
        public int? Num { get; set; }

        /// <summary>
        /// Bairro conforme consta na base CEP dos Correios.
        /// </summary>
        [Required, StringLength(72)]
        public string Bairro { get; set; }

        /// <summary>
        /// Número do CEP do endereço.
        /// </summary>
        [Required, StringLength(9)]
        public string Cep { get; set; }

        /// <summary>
        /// Referência: Condominio, Edificio ...
        /// </summary>
        [Required, StringLength(40)]
        public string Ref { get; set; }

        /// <summary>
        /// Cidade conforme consta na base CEP dos Correios.
        /// </summary>
        [Required, StringLength(24)]
        public string Cid { get; set; }

        /// <summary>
        /// Sigla do estado.
        /// </summary>
        [Required, StringLength(2)]
        public string Uf { get; set; }


    }
    public class restauranteHora
    {

        public int restid { get; set; }

        public int cardapioid { get; set; }

        public List<Resthora> horas { get; set; }

          public class Resthora
          {
              public int dia { get; set; }
              public string abre { get; set; }
              public string fecha { get; set; }
          }
    }




    public class HttpMsgOK
    {
        public string msgok { get; set; }
    }
    public class WrapperModel
        {
        public List<restauranteHora> restauranteHora{ get; set; }
        public List<Resthora> horas { get; set; }
        public int restid { get; set; }


        public class Resthora
        {
            public int restid { get; set; }
            public int dia { get; set; }
            public string abre { get; set; }
            public string fecha { get; set; }
            public Resthora Result { get; internal set; }

        }

    }

    }






