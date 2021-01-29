using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using elocal.Models;
using static elocal.Models.restauranteHora;
using System.Threading.Tasks;

namespace elocal.Controllers
{
    /// <summary>
    /// Restaurantes com delivery ativo e seus dados, incluindo as formas de pgto agora habilitadas
    /// </summary>
    [RoutePrefix("api/restaurante")]
    public class restauranteController : ApiController
    {
        private string ipAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
        private static string ipspodem = ConfigurationManager.AppSettings["ipspodem"];
        private static string cepbd = ConfigurationManager.AppSettings["cepbd"];

        public int restid { get; private set; }

        private static restaurante leRest(int id)
        {
            restaurante restx = new restaurante { };
            restx.restid = 0;
            restx.restarea = "";
            List<hora> horas = new List<hora>();
            Int16 tabela = 0;
            byte flgLJ = 0;
            int ljr = 0;
            int tmpent = 0;
            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;
            // Loja sem Delivery APP, fazer tabela=0
            // giraffaslj.flgLJ & 9: 1=Somente informa que a loja tem Sistema Delivery
            //    mas pode nao ter Delivery para APP, ex: APP Local + Ubereats, entao vale abaixo
            // giraffasljdg.flgdg>=100 tem Yandeh, d.flgdg%100 & 9: 1=SoDel 8=SoLoc 9=DelLoc
            string cmdStr = "select l.Loja,l.NomLj,l.CepLj,l.EndLj,l.BaiLj,l.CidLj,l.UfLj,l.CNPJ,";
            cmdStr = cmdStr + "cast(d.flgdg%100 & 9 as tinyint),a.LjR,cast(a.TxEnt as smallint),convert(char(5),dateadd(hh,fuso,h.HrA),8),convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,h.HrF)),8),";
            cmdStr = cmdStr + "IsNull(h.delMin,0),IsNull(h.locMin,0),IsNull(h.feijao,'m'),IsNull(h.cardapio,6),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrADom),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFDom)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA2a6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF2a6)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA3),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF3)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA4),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF4)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA5),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF5)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF6)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrASab),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFSab)),8)";
            cmdStr = cmdStr + " from giraffas.dbo.giraffaslj l,giraffas.dbo.giraffasljdg d,giraffas.dbo.giraffasAr a,giraffas.dbo.giraffasljAF h";
            cmdStr = cmdStr + " where l.flglj>0 and d.flgdg>100 and d.Loja=l.Loja and a.Loja=l.Loja and a.Area=0 and h.Loja=l.Loja and l.Loja=" + id.ToString();
            string aux = "";
            string restUf = "";
            string restCid = "";
            string restBairro = "";

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                var cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    restx.restid = dr.GetInt16(0);
                    restx.restNome = dr.GetString(1).TrimEnd();
                    restx.restCep = dr.GetString(2).TrimEnd();
                    restx.restEnde = dr.GetString(3).TrimEnd();
                    restBairro = dr.GetString(4).TrimEnd();
                    restCid = dr.GetString(5).TrimEnd();
                    restUf = dr.GetString(6).TrimEnd();
                    if (restUf == "DF" && restCid != "Brasília")
                    {
                        if (restBairro.ToLower().IndexOf(restCid.ToLower()) < 0)
                        {
                            restBairro = restBairro + " - " + restCid;
                        }
                        restCid = "Brasília";
                    }
                    restx.restBairro = restBairro;
                    restx.restCid = restCid;
                    restx.restUf = restUf;
                    restx.cnpj = dr.GetString(7).TrimEnd();
                    flgLJ = dr.GetByte(8);
                    ljr = dr.GetInt16(9);
                    tmpent = dr.GetInt16(10);
                    restx.restInicio = dr.GetString(11).TrimEnd();
                    restx.restFim = dr.GetString(12).TrimEnd();

                    restx.locInicio = dr.GetString(11).TrimEnd();
                    restx.locFim = dr.GetString(12).TrimEnd();
                    restx.locStatus = dr.GetInt16(10);
                    restx.delmin = Math.Round(dr.GetDecimal(13), 2);
                    restx.locmin = Math.Round(dr.GetDecimal(14), 2);
                    restx.feijao = "marrom";
                    if (dr.GetString(15).TrimEnd() == "p")
                    {
                        restx.feijao = "preto";
                    }
                    tabela = dr.GetInt16(16);
                    for (int i = 0; i < 7; i++)
                    {
                        horas.Add(new hora
                        {
                            dia = i + 1,
                            abre = dr.GetString(17 + 2 * i).TrimEnd(),
                            fecha = dr.GetString(18 + 2 * i).TrimEnd()
                        });
                    }
                }
                dr.Close();
                restx.horas = horas;

                if (flgLJ == 0 || flgLJ == 8) { tabela = 0; }
                restx.cardapioid = tabela;
                restx.cardapiolocid = 0;
                cmdStr = "select d.codCardapio";
                cmdStr = cmdStr + " from eportal.dbo.loja d, giraffas.dbo.giraffasljdg g, giraffas.dbo.giraffaslj l";
                cmdStr = cmdStr + " where d.codLoja=g.codDG and g.loja=l.Loja and l.flglj&8>0";
                cmdStr = cmdStr + " and g.flgdg>100 and (g.flgdg%100 & 8)>0 and g.loja=" + id.ToString();
                cmd = new SqlCommand(cmdStr, conn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    restx.cardapiolocid = dr.GetInt32(0);
                }
                dr.Close();

                if (tmpent == 0 && ljr > 0) { tmpent = -1; }
                restx.Tempo = tmpent;
                restx.TxEnt = 0;

                restx.pgtoCod = "";
                aux = "";
                cmdStr = "select p.codPg,p.ordem";
                cmdStr = cmdStr + " from giraffas.dbo.GiraffasAr a,giraffas.dbo.GiraffasCxT p";
                cmdStr = cmdStr + " where a.Area=30000+p.codPg and a.area>30000 and (p.tipPg<7 or p.codPg=33 or left(p.TipoPg,3)='APP') and a.loja=" + id.ToString();
                //cmdStr = cmdStr + " and not exists (select * from giraffas.dbo.GiraffasCBXw where codPg=p.codPg and loja=" + id.ToString() + ")";
                //cmdStr = cmdStr + " UNION select cast(p.codPg+100 as tinyint) codPg,p.ordem";
                //cmdStr = cmdStr + " from giraffas.dbo.GiraffasCBXw x,giraffas.dbo.GiraffasCxT p";
                //cmdStr = cmdStr + " where x.codPg=p.codPg and x.loja=" + id.ToString();
                cmdStr = cmdStr + " order by codPg";
                cmd = new SqlCommand(cmdStr, conn);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    restx.pgtoCod = restx.pgtoCod + aux + dr.GetByte(0).ToString();
                    aux = ",";
                }
                dr.Close();

                conn.Close();
            }
            return restx;
        }

        private static List<restaurante> listRest()
        {
            List<restaurante> lista = new List<restaurante>();
            restaurante restx = new restaurante { };

            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;
            string cmdStr = "select l.Loja from giraffas.dbo.giraffaslj l, giraffas.dbo.giraffasljdg g";
            //cmdStr = cmdStr + " UNION";
            //cmdStr = cmdStr + " select Loja from giraffas.dbo.giraffasljdg where flgdg>0 and Loja>10000";
            cmdStr = cmdStr + " where l.Loja=g.loja and l.flglj & 9 > 0 and g.flgdg>100 order by Loja";
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(leRest(dr.GetInt16(0)));
                }
                dr.Close();
                conn.Close();
            }
            return lista;
        }

        private static restaurante leRestx(string cep, string uf)
        {
            restaurante restx = new restaurante { };
            int ljj = 0;
            int loja = 0;
            int ljr = 0;
            int tmpent = 0;
            int Area = 0;
            int ljx = 0;
            decimal txent = 0;
            int miHrR = 0;
            string HrRF = "";
            string restUf = "";
            string restCid = "";
            string restBairro = "";

            restx.restid = loja;
            restx.Tempo = tmpent;
            restx.restarea = "";

            string sUF = "";
            // MT tem lojas 7xx
            string teste = "ru.Loja<600 or ru.Loja>700";
            if (uf == "DF" || uf == "GO")
            {
                // GO tem loja 7xx
                teste = "ru.Loja>600 and ru.Loja<800";
                sUF = "6";
            }

            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;

            string cmdStr = "select top 1 ru.Loja,IsNull(t.LjR,0) ljr,IsNull(cast(t.TxEnt as smallint),0) tmpent,";
            cmdStr = cmdStr + " ru.Area,IsNull(a.LjR,0) ljx, IsNull(a.TxEnt,0) txent,";
            cmdStr = cmdStr + " IsNull(60*(datepart(hh,r.HrR)-datepart(hh,getdate()))+datepart(mi,r.HrR)-datepart(mi,getdate()),0) miHrR,";
            cmdStr = cmdStr + " IsNull(convert(char(5),r.HrR,8),'') HrRF";
            cmdStr = cmdStr + " from giraffas.dbo.giraffasru" + sUF + " ru";
            cmdStr = cmdStr + " left join giraffas.dbo.GiraffasAr t on t.Loja=ru.Loja and t.Area=0";
            cmdStr = cmdStr + " left join giraffas.dbo.GiraffasAr a on a.Loja=ru.Loja and a.Area=ru.Area";
            cmdStr = cmdStr + " left join giraffas.dbo.GiraffasArR r on r.Loja=ru.Loja and r.Area=ru.Area";
            cmdStr = cmdStr + " where ru.Loja%100>0 and (" + teste + ") and ru.UfRua='" + uf + "'";
            cmdStr = cmdStr + " and replace(ru.ceprua,'-','')='" + cep + "'";
            cmdStr = cmdStr + " order by tmpent desc";

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    loja = dr.GetInt16(0);
                    ljr = dr.GetInt16(1);
                    tmpent = dr.GetInt16(2);
                    Area = dr.GetInt16(3);
                    ljx = dr.GetInt16(4);
                    txent = Math.Round(dr.GetDecimal(5), 2);
                    miHrR = dr.GetInt32(6);
                    HrRF = dr.GetString(7).TrimEnd(' ');
                }
                dr.Close();

                if (loja > 0 && miHrR >= 0)
                {
                    ljj = loja;
                    //Vendo se é Comutada ou Concorrência
                    if (ljx < 0)
                    {
                        string order = " order by x.cnt,x.Loja";
                        if (ljx < -1024) { order = " order by x.inc,x.part,x.cnt,x.Loja"; }
                        cmdStr = "select top 1 a.Loja,cast(a.TxEnt as smallint)";
                        cmdStr = cmdStr + " from giraffas.dbo.GiraffasAr_ x,giraffas.dbo.GiraffasAr a";
                        cmdStr = cmdStr + " where a.Loja=x.Loja and a.TxEnt>0 and a.area=0";
                        cmdStr = cmdStr + " and x.LjX=" + Math.Abs(ljx).ToString();
                        cmdStr = cmdStr + order;
                        cmd = new SqlCommand(cmdStr, conn);
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            loja = dr.GetInt16(0);
                            tmpent = dr.GetInt16(1);
                        }
                        dr.Close();
                    }
                    else
                    {
                        //Vendo se está chaveada
                        if (ljx > 0)
                        {
                            loja = ljx;
                            cmdStr = "select cast(TxEnt as smallint) from giraffas.dbo.GiraffasAr";
                            cmdStr = cmdStr + " where Loja=" + loja.ToString() + " and Area=0";
                            cmd = new SqlCommand(cmdStr, conn);
                            dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {
                                tmpent = dr.GetInt16(0);
                            }
                            dr.Close();
                        }
                    }
                }

                if (loja > 0 && tmpent > 0)
                {
                    restx.restid = loja;
                    restx.restarea = ljj.ToString() + "." + Area.ToString();
                    List<hora> horas = new List<hora>();
                    byte flgLJ = 0;
                    Int16 tabela = 0;

                    restx.cardapiolocid = 0;
                    cmdStr = "select d.codCardapio from eportal.dbo.loja d, giraffas.dbo.giraffasljdg g, giraffas.dbo.giraffaslj l";
                    cmdStr = cmdStr + " where d.codLoja=g.codDG and g.loja=l.Loja and l.flglj&8>0 and g.flgdg>100 and (g.flgdg%100 & 8)>0 and g.loja=" + loja.ToString();
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        restx.cardapiolocid = dr.GetInt32(0);
                    }
                    dr.Close();

                    cmdStr = "select l.NomLj,l.CepLj,l.EndLj,l.BaiLj,l.CidLj,l.UfLj,l.CNPJ,convert(char(5),h.HrA,8) HrA,convert(char(5),h.HrF,8) HrF";
                    cmdStr = cmdStr + ",cast(d.flgdg%100 & 9 as tinyint),";
                    cmdStr = cmdStr + "IsNull(h.delMin,0),IsNull(h.locMin,0),IsNull(h.feijao,'m'),IsNull(h.cardapio,6),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrADom),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFDom)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA2a6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF2a6)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA3),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF3)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA4),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF4)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA5),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF5)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF6)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrASab),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFSab)),8)";
                    cmdStr = cmdStr + " from giraffas.dbo.GiraffasLj l,giraffas.dbo.GiraffasLjdg d,giraffas.dbo.GiraffasLjAF h";
                    cmdStr = cmdStr + " where l.Loja=d.Loja and l.Loja=h.Loja and l.Loja=" + loja.ToString();
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        restx.restNome = dr.GetString(0).TrimEnd();
                        restx.restCep = dr.GetString(1).TrimEnd();
                        restx.restEnde = dr.GetString(2).TrimEnd();
                        restBairro = dr.GetString(3).TrimEnd();
                        restCid = dr.GetString(4).TrimEnd();
                        restUf = dr.GetString(5).TrimEnd();
                        if (restUf == "DF" && restCid != "Brasília")
                        {
                            if (restBairro.ToLower().IndexOf(restCid.ToLower()) < 0)
                            {
                                restBairro = restBairro + " - " + restCid;
                            }
                            restCid = "Brasília";
                        }
                        restx.restBairro = restBairro;
                        restx.restCid = restCid;
                        restx.restUf = restUf;
                        restx.cnpj = dr.GetString(6).TrimEnd();
                        restx.restInicio = dr.GetString(7).TrimEnd();
                        if (HrRF.Length == 0)
                        {
                            restx.restFim = dr.GetString(8).TrimEnd();
                        }
                        else
                        {
                            restx.restFim = HrRF;
                        }
                        flgLJ = dr.GetByte(9);
                        restx.delmin = Math.Round(dr.GetDecimal(10), 2);
                        restx.locmin = Math.Round(dr.GetDecimal(11), 2);
                        restx.feijao = "marrom";
                        if (dr.GetString(12).TrimEnd() == "p")
                        {
                            restx.feijao = "preto";
                        }
                        tabela = dr.GetInt16(13);
                        for (int i = 0; i < 7; i++)
                        {
                            horas.Add(new hora
                            {
                                dia = i + 1,
                                abre = dr.GetString(14 + 2 * i).TrimEnd(),
                                fecha = dr.GetString(15 + 2 * i).TrimEnd()
                            });
                        }
                    }
                    dr.Close();

                    if (flgLJ == 0 || flgLJ == 8) { tabela = 0; }
                    restx.cardapioid = tabela;

                    restx.Tempo = tmpent;
                    restx.TxEnt = txent;
                    restx.horas = horas;

                    restx.pgtoCod = "";
                    string aux = "";
                    cmdStr = "select p.codPg,p.ordem";
                    cmdStr = cmdStr + " from giraffas.dbo.GiraffasAr a,giraffas.dbo.GiraffasCxT p";
                    cmdStr = cmdStr + " where a.Area=30000+p.codPg and a.area>30000 and (p.tipPg<7 or p.codPg=33 or left(p.TipoPg,3)='APP') and a.loja=" + loja.ToString();
                    //cmdStr = cmdStr + " and not exists (select * from giraffas.dbo.GiraffasCBXw where codPg=p.codPg and loja=" + loja.ToString() + ")";
                    //cmdStr = cmdStr + " UNION select cast(p.codPg+100 as tinyint) codPg,p.ordem";
                    //cmdStr = cmdStr + " from giraffas.dbo.GiraffasCBXw x,giraffas.dbo.GiraffasCxT p";
                    //cmdStr = cmdStr + " where x.codPg=p.codPg and x.loja=" + loja.ToString();
                    cmdStr = cmdStr + " order by codPg";
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        restx.pgtoCod = restx.pgtoCod + aux + dr.GetByte(0).ToString();
                        aux = ",";
                    }
                    dr.Close();
                }
                conn.Close();
            }

            return restx;
        }

        private static List<restaurante> leRestsx(string cep, string uf)
        {
            List<restaurante> lista = new List<restaurante>();
            //  restaurante restx = new restaurante { };
            List<int> lojas = new List<int>();
            List<int> tempos = new List<int>();
            int ljj = 0;
            int loja = 0;
            int ljr = 0;
            int tmpent = 0;
            int Area = 0;
            int ljx = 0;
            decimal txent = 0;
            int miHrR = 0;
            string HrRF = "";
            string restUf = "";
            string restCid = "";
            string restBairro = "";
            bool temDelivery = true;

            //restx.restid = loja;
            //restx.Tempo = tmpent;
            //restx.restarea = "";

            string sUF = "";
            // MT tem lojas 7xx
            string teste = "ru.Loja<600 or ru.Loja>700";
            if (uf == "DF" || uf == "GO")
            {
                // GO tem loja 7xx
                teste = "ru.Loja>600 and ru.Loja<800";
                sUF = "6";
            }

            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;

            string cmdStr = "select top 1 ru.Loja,IsNull(t.LjR,0) ljr,IsNull(cast(t.TxEnt as smallint),0) tmpent,";
            cmdStr = cmdStr + " ru.Area,IsNull(a.LjR,0) ljx, IsNull(a.TxEnt,0) txent,";
            cmdStr = cmdStr + " IsNull(60*(datepart(hh,r.HrR)-datepart(hh,getdate()))+datepart(mi,r.HrR)-datepart(mi,getdate()),0) miHrR,";
            cmdStr = cmdStr + " IsNull(convert(char(5),r.HrR,8),'') HrRF,IsNull(g.flgDg&1,0)";
            cmdStr = cmdStr + " from giraffas.dbo.giraffasru" + sUF + " ru";
            cmdStr = cmdStr + " left join giraffas.dbo.GiraffasAr t on t.Loja=ru.Loja and t.Area=0";
            cmdStr = cmdStr + " left join giraffas.dbo.GiraffasAr a on a.Loja=ru.Loja and a.Area=ru.Area";
            cmdStr = cmdStr + " left join giraffas.dbo.GiraffasArR r on r.Loja=ru.Loja and r.Area=ru.Area";
            cmdStr = cmdStr + " left join giraffas.dbo.GiraffasLjDg g on g.Loja=ru.Loja";
            cmdStr = cmdStr + " where ru.Loja%100>0 and (" + teste + ") and ru.UfRua='" + uf + "'";
            cmdStr = cmdStr + " and replace(ru.ceprua,'-','')='" + cep + "'";
            cmdStr = cmdStr + " order by tmpent desc";

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    loja = dr.GetInt16(0);
                    ljr = dr.GetInt16(1);
                    tmpent = dr.GetInt16(2);
                    Area = dr.GetInt16(3);
                    ljx = dr.GetInt16(4);
                    txent = Math.Round(dr.GetDecimal(5), 2);
                    miHrR = dr.GetInt32(6);
                    HrRF = dr.GetString(7).TrimEnd(' ');
                    if (dr.GetInt32(8) == 0) { temDelivery = false; }
                }
                dr.Close();

                if (loja > 0)
                //  && miHrR >= 0
                {
                    ljj = loja;
                    if (ljx == 0)
                    {
                        if (temDelivery)
                        {
                            if (miHrR < 0) { tmpent = 0; }
                            lojas.Add(loja);
                            tempos.Add(tmpent);
                        }
                    }
                    else
                    {
                        //Vendo se é Comutada ou Concorrência
                        if (ljx < 0)
                        {
                            string order = " order by x.cnt,x.Loja";
                            if (ljx < -1024) { order = " order by x.inc,x.part,x.cnt,x.Loja"; }
                            cmdStr = "select a.Loja,cast(a.TxEnt as smallint)";
                            cmdStr = cmdStr + " from giraffas.dbo.GiraffasAr_ x,giraffas.dbo.GiraffasAr a,giraffas.dbo.GiraffasLjDg g";
                            cmdStr = cmdStr + " where a.Loja=x.Loja and g.Loja=x.Loja and a.area=0";
                            cmdStr = cmdStr + " and x.LjX=" + Math.Abs(ljx).ToString();
                            cmdStr = cmdStr + " and g.flgdg&1>0" + order;
                            cmd = new SqlCommand(cmdStr, conn);
                            dr = cmd.ExecuteReader();
                            while (dr.Read())
                            {
                                //loja = dr.GetInt16(0);
                                tmpent = dr.GetInt16(1);
                                if (loja == dr.GetInt16(0) && miHrR < 0) { tmpent = 0; }
                                lojas.Add(dr.GetInt16(0));
                                tempos.Add(tmpent);
                            }
                            dr.Close();
                        }
                        else
                        {
                            //Vendo se está chaveada
                            if (ljx > 0)
                            {
                                loja = ljx;
                                cmdStr = "select cast(TxEnt as smallint) from giraffas.dbo.GiraffasAr,giraffas.dbo.GiraffasLjDg g";
                                cmdStr = cmdStr + " where Loja=" + loja.ToString() + " and Area=0 and g.loja=a.loja and g.flgdg&1>0";
                                cmd = new SqlCommand(cmdStr, conn);
                                dr = cmd.ExecuteReader();
                                if (dr.Read())
                                {
                                    tmpent = dr.GetInt16(0);
                                    if (miHrR < 0) { tmpent = 0; }
                                    lojas.Add(loja);
                                    tempos.Add(dr.GetInt16(1));
                                }
                                dr.Close();
                            }
                        }
                    }
                }

                //if (loja > 0 && tmpent > 0)
                for (int iLj = 0; iLj < lojas.Count; iLj++)
                {
                    restaurante restx = new restaurante { };
                    loja = lojas[iLj];
                    tmpent = tempos[iLj];
                    restx.restid = loja;
                    restx.restarea = ljj.ToString() + "." + Area.ToString();
                    List<hora> horas = new List<hora>();
                    byte flgLJ = 0;
                    Int16 tabela = 0;

                    restx.cardapiolocid = 0;
                    cmdStr = "select d.codCardapio from eportal.dbo.loja d, giraffas.dbo.giraffasljdg g, giraffas.dbo.giraffaslj l";
                    cmdStr = cmdStr + " where d.codLoja=g.codDG and g.loja=l.Loja and l.flglj&8>0 and g.flgdg>100 and (g.flgdg%100 & 8)>0 and g.loja=" + loja.ToString();
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        restx.cardapiolocid = dr.GetInt32(0);
                    }
                    dr.Close();

                    cmdStr = "select l.NomLj,l.CepLj,l.EndLj,l.BaiLj,l.CidLj,l.UfLj,l.CNPJ,convert(char(5),h.HrA,8) HrA,convert(char(5),h.HrF,8) HrF";
                    cmdStr = cmdStr + ",cast(d.flgdg%100 & 9 as tinyint),";
                    cmdStr = cmdStr + "IsNull(h.delMin,0),IsNull(h.locMin,0),IsNull(h.feijao,'m'),IsNull(h.cardapio,6),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrADom),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFDom)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA2a6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF2a6)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA3),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF3)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA4),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF4)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA5),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF5)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF6)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrASab),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFSab)),8)";
                    cmdStr = cmdStr + " from giraffas.dbo.GiraffasLj l,giraffas.dbo.GiraffasLjdg d,giraffas.dbo.GiraffasLjAF h";
                    cmdStr = cmdStr + " where l.Loja=d.Loja and l.Loja=h.Loja and l.Loja=" + loja.ToString();
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        restx.restNome = dr.GetString(0).TrimEnd();
                        restx.restCep = dr.GetString(1).TrimEnd();
                        restx.restEnde = dr.GetString(2).TrimEnd();
                        restBairro = dr.GetString(3).TrimEnd();
                        restCid = dr.GetString(4).TrimEnd();
                        restUf = dr.GetString(5).TrimEnd();
                        if (restUf == "DF" && restCid != "Brasília")
                        {
                            if (restBairro.ToLower().IndexOf(restCid.ToLower()) < 0)
                            {
                                restBairro = restBairro + " - " + restCid;
                            }
                            restCid = "Brasília";
                        }
                        restx.restBairro = restBairro;
                        restx.restCid = restCid;
                        restx.restUf = restUf;
                        restx.cnpj = dr.GetString(6).TrimEnd();
                        restx.restInicio = dr.GetString(7).TrimEnd();
                        if (HrRF.Length == 0)
                        {
                            restx.restFim = dr.GetString(8).TrimEnd();
                        }
                        else
                        {
                            restx.restFim = HrRF;
                        }
                        flgLJ = dr.GetByte(9);
                        restx.delmin = Math.Round(dr.GetDecimal(10), 2);
                        restx.locmin = Math.Round(dr.GetDecimal(11), 2);
                        restx.feijao = "marrom";
                        if (dr.GetString(12).TrimEnd() == "p")
                        {
                            restx.feijao = "preto";
                        }
                        tabela = dr.GetInt16(13);
                        for (int i = 0; i < 7; i++)
                        {
                            horas.Add(new hora
                            {
                                dia = i + 1,
                                abre = dr.GetString(14 + 2 * i).TrimEnd(),
                                fecha = dr.GetString(15 + 2 * i).TrimEnd()
                            });
                        }
                    }
                    dr.Close();

                    if (flgLJ == 0 || flgLJ == 8) { tabela = 0; }
                    restx.cardapioid = tabela;

                    restx.Tempo = tmpent;
                    restx.TxEnt = txent;
                    restx.horas = horas;

                    restx.pgtoCod = "";
                    string aux = "";
                    cmdStr = "select p.codPg,p.ordem";
                    cmdStr = cmdStr + " from giraffas.dbo.GiraffasAr a,giraffas.dbo.GiraffasCxT p";
                    cmdStr = cmdStr + " where a.Area=30000+p.codPg and a.area>30000 and (p.tipPg<7 or p.codPg=33 or left(p.TipoPg,3)='APP') and a.loja=" + loja.ToString();
                    //cmdStr = cmdStr + " and not exists (select * from giraffas.dbo.GiraffasCBXw where codPg=p.codPg and loja=" + loja.ToString() + ")";
                    //cmdStr = cmdStr + " UNION select cast(p.codPg+100 as tinyint) codPg,p.ordem";
                    //cmdStr = cmdStr + " from giraffas.dbo.GiraffasCBXw x,giraffas.dbo.GiraffasCxT p";
                    //cmdStr = cmdStr + " where x.codPg=p.codPg and x.loja=" + loja.ToString();
                    cmdStr = cmdStr + " order by codPg";
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        restx.pgtoCod = restx.pgtoCod + aux + dr.GetByte(0).ToString();
                        aux = ",";
                    }
                    dr.Close();
                    lista.Add(restx);
                }
                conn.Close();
            }

            return lista;
        }

        private List<restaurante> testaEnde(endereco lugar)
        {
            List<restaurante> lista = new List<restaurante>();
            restaurante restx = new restaurante { };
            Int32 id = 0;
            Int16 loja = 0;
            byte status = 0;
            int tmpent = 0;
            decimal txent = 0;
            string HrRF = "";
            string Bairro = lugar.Bairro.TrimEnd();
            string Cid = lugar.Cid.TrimEnd();
            string restUf = "";
            string restCid = "";
            string restBairro = "";
            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;
            string cmdStr = "insert into tmp.dbo.cadastroz (pEnd,Ende,Num,Bairro,Cep,Ref,Cid,Uf) values (";
            cmdStr = cmdStr + "'" + lugar.pEnd + "','" + lugar.Ende + "'," + lugar.Num.ToString() + ",'";
            string cmdStr2 = "','" + lugar.Cep + "','" + lugar.Ref + "','";
            string cmdStr3 = "','" + lugar.Uf + "') select @@IDENTITY as 'id'";
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                // Verificar se bairro >24 char, ver na tabela RuasDePara
                var cmd = new SqlCommand("select top 1 BaiRua from tmp.dbo.RuasDePara where BaiCep='" + Bairro + "'", conn);
                if (Bairro.Length > 24)
                {
                    SqlDataReader drb = cmd.ExecuteReader();
                    if (drb.Read())
                    {
                        Bairro = drb.GetString(0);
                    }
                    drb.Close();
                }
                // Verificar se cidade >24 char, ver na tabela RuasCDePara
                if (Cid.Length > 24)
                {
                    cmd = new SqlCommand("select top 1 CidRua from tmp.dbo.RuasCDePara where CidCep='" + Cid + "'", conn);
                    SqlDataReader drc = cmd.ExecuteReader();
                    if (drc.Read())
                    {
                        Cid = drc.GetString(0);
                    }
                    drc.Close();
                }
                cmd = new SqlCommand(cmdStr + Bairro + cmdStr2 + Cid + cmdStr3, conn);
                id = (int)(decimal)cmd.ExecuteScalar();
                if (id > 0)
                {
                    cmd = new SqlCommand("exec lugar " + id.ToString(), conn);
                    cmd.ExecuteNonQuery();
                }
                cmdStr = "select top 1 y.status,y.loja,y.tmpent,y.txent,y.HrRF";
                cmdStr = cmdStr + " from tmp.dbo.cadastroy y, giraffas.dbo.giraffasljdg g, giraffas.dbo.giraffaslj l";
                cmdStr = cmdStr + " where y.loja=g.Loja and g.loja=l.Loja and l.flgLj&1>0 and g.flgdg>100";
                cmdStr = cmdStr + " and (g.flgdg%100 & 1)>0 and y.idz=" + id.ToString();
                cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    status = dr.GetByte(0);
                    loja = dr.GetInt16(1);
                    tmpent = dr.GetInt16(2);
                    txent = Math.Round(dr.GetDecimal(3), 2);
                    HrRF = dr.GetString(4).TrimEnd(' ');
                }
                dr.Close();

                if (loja > 0)
                {
                    restx.restid = loja;
                    Int16 tabela = 0;
                    List<hora> horas = new List<hora>();

                    cmdStr = "select l.NomLj,l.CepLj,l.EndLj,l.BaiLj,l.CidLj,l.UfLj,l.CNPJ,convert(char(5),h.HrA,8) HrA,convert(char(5),h.HrF,8) HrF,";
                    cmdStr = cmdStr + "IsNull(h.delMin,0),IsNull(h.locMin,0),IsNull(h.feijao,'m'),IsNull(h.cardapio,6),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrADom),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFDom)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA2a6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF2a6)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA3),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF3)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA4),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF4)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA5),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF5)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF6)),8),";
                    cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrASab),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFSab)),8)";
                    cmdStr = cmdStr + " from giraffas.dbo.GiraffasLj l,giraffas.dbo.GiraffasLjAF h";
                    cmdStr = cmdStr + " where l.Loja=h.Loja and l.Loja=" + loja.ToString();
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        restx.restNome = dr.GetString(0).TrimEnd();
                        restx.restCep = dr.GetString(1).TrimEnd();
                        restx.restEnde = dr.GetString(2).TrimEnd();
                        restBairro = dr.GetString(3).TrimEnd();
                        restCid = dr.GetString(4).TrimEnd();
                        restUf = dr.GetString(5).TrimEnd();
                        if (restUf == "DF" && restCid != "Brasília")
                        {
                            if (restBairro.ToLower().IndexOf(restCid.ToLower()) < 0)
                            {
                                restBairro = restBairro + " - " + restCid;
                            }
                            restCid = "Brasília";
                        }
                        restx.restBairro = restBairro;
                        restx.restCid = restCid;
                        restx.restUf = restUf;
                        restx.cnpj = dr.GetString(6).TrimEnd();
                        restx.restInicio = dr.GetString(7).TrimEnd(' ');
                        if (HrRF.Length == 0)
                        {
                            restx.restFim = dr.GetString(8).TrimEnd(' ');
                        }
                        else
                        {
                            restx.restFim = HrRF;
                        }
                        restx.delmin = Math.Round(dr.GetDecimal(9), 2);
                        restx.locmin = Math.Round(dr.GetDecimal(10), 2);
                        restx.feijao = "marrom";
                        if (dr.GetString(11).TrimEnd() == "p")
                        {
                            restx.feijao = "preto";
                        }
                        tabela = dr.GetInt16(12);
                        for (int i = 0; i < 7; i++)
                        {
                            horas.Add(new hora
                            {
                                dia = i + 1,
                                abre = dr.GetString(13 + 2 * i).TrimEnd(),
                                fecha = dr.GetString(14 + 2 * i).TrimEnd()
                            });
                        }
                    }
                    dr.Close();

                    restx.cardapioid = tabela;
                    restx.Tempo = tmpent;
                    restx.TxEnt = txent;
                    restx.horas = horas;

                    restx.pgtoCod = "";
                    string aux = "";
                    cmdStr = "select p.codPg,p.ordem";
                    cmdStr = cmdStr + " from giraffas.dbo.GiraffasAr a,giraffas.dbo.GiraffasCxT p";
                    cmdStr = cmdStr + " where a.Area=30000+p.codPg and a.area>30000 and (p.tipPg<7 or p.codPg=33 or left(p.TipoPg,3)='APP') and a.loja=" + loja.ToString();
                    //cmdStr = cmdStr + " and not exists (select * from giraffas.dbo.GiraffasCBXw where codPg=p.codPg and loja=" + loja.ToString() + ")";
                    //cmdStr = cmdStr + " UNION select cast(p.codPg+100 as tinyint) codPg,p.ordem";
                    //cmdStr = cmdStr + " from giraffas.dbo.GiraffasCBXw x,giraffas.dbo.GiraffasCxT p";
                    //cmdStr = cmdStr + " where x.codPg=p.codPg and x.loja=" + loja.ToString();
                    cmdStr = cmdStr + " order by codPg";
                    cmd = new SqlCommand(cmdStr, conn);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        restx.pgtoCod = restx.pgtoCod + aux + dr.GetByte(0).ToString();
                        aux = ",";
                    }
                    dr.Close();
                }

                /*
                string msg = "";
                if (status == 100) {

                } else {
                    if (status == 1)
                    {
                        msg = "01 Local sem Delivery";
                    }
                    else
                    {
                        msg = "09 Erro desconhecido no endereçamento";
                        switch (status)
                        {
                            case 10:
                                msg = "10 Local sem Delivery atualmente";
                                break;
                            case 11:
                                msg = "11 Horario delivery encerrado";
                                break;
                            case 12:
                                msg = "12 Delivery Fechado no momento";
                                break;
                        }
                    }
                }
                */
                conn.Close();
            }

            //List<restaurante> lista = ListaDeRest(1, 2165879);
            if (loja > 0)
            {
                lista.Add(restx);
            }
            return lista;
        }

        private string ufCep(string cep)
        {
            string uf = "";
            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;
            string cmdStr = "select loc_uf from Ruas" + cepbd + ".dbo.gf_loc where loc_ini<=" + cep + " and " + cep + "<=loc_fim";
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    uf = dr.GetString(0);
                }
                dr.Close();
                conn.Close();
            }
            return uf;
        }

        public Models.restauranteHora leHora(int id)
        {
            
            restauranteHora restx = new restauranteHora { };
            List<restauranteHora> lista = new List<restauranteHora>();
            restx.restid = 0;


            List<Resthora> horas = new List<Resthora>();
            Int16 tabela = 0;
            byte flgLJ = 0;


            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;

            string cmdStr = "select l.Loja,l.NomLj,l.CepLj,l.EndLj,l.BaiLj,l.CidLj,l.UfLj,l.CNPJ,";
            cmdStr = cmdStr + "cast(d.flgdg%100 & 9 as tinyint),a.LjR,cast(a.TxEnt as smallint),convert(char(5),dateadd(hh,fuso,h.HrA),8),convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,h.HrF)),8), ";
            cmdStr = cmdStr + "IsNull(h.delMin,0),IsNull(h.locMin,0),IsNull(h.feijao,'m'),IsNull(h.cardapio,6),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrADom),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFDom)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA2a6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF2a6)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA3),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF3)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA4),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF4)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA5),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF5)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrA6),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrF6)),8),";
            cmdStr = cmdStr + " convert(char(5),dateadd(hh,fuso,HrASab),8), convert(char(5),dateadd(mi,-10,dateadd(hh,fuso,HrFSab)),8)";

            cmdStr = cmdStr + " from giraffas.dbo.giraffaslj l,giraffas.dbo.giraffasljdg d,giraffas.dbo.giraffasAr a,giraffas.dbo.giraffasljAF h";
            cmdStr = cmdStr + " where l.flglj>0 and d.flgdg>100 and d.Loja=l.Loja and a.Loja=l.Loja and a.Area=0 and h.Loja=l.Loja and l.Loja= " + id.ToString() + " ";

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(cmdStr, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    restx.restid = dr.GetInt16(0);
                    flgLJ = dr.GetByte(8);
                    tabela = dr.GetInt16(16);
                    for (int i = 0; i < 7; i++)
                    {
                        horas.Add(new Resthora
                        {
                            dia = i + 1,
                            abre = dr.GetString(17 + 2 * i).TrimEnd(),
                            fecha = dr.GetString(18 + 2 * i).TrimEnd()

                        });
                    }
                }
                dr.Close();



                restx.horas = horas;



                if (flgLJ == 0 || flgLJ == 8) { tabela = 0; }
                restx.cardapioid = tabela;




                return restx;

            }

        }

        private static async Task<string> setHoras(int id, List<hora> horas)
       //  public static string leHoraStr(int id)     //(int id,List<hora>horas)
        {
            string msg = "";
            restauranteHora restx = new restauranteHora { };
            restx.id = 0;
                   
            string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;
            string cmdStr = "select Loja from giraffas.dbo.giraffasLjAF where ";
            cmdStr = cmdStr + " Loja='" + restx.restid + "'";
            using (var conn = new SqlConnection(connStr)) 
            {
                    conn.Open();
                var cmd = new SqlCommand(cmdStr, conn);
                if (restx.id > 0)
                {
                    
                    cmdStr = "update giraffas.dbo.giraffasLjAF set ";
                    cmd = new SqlCommand(cmdStr, conn);
                    cmd.ExecuteNonQuery();
              
                    List<hora> horasx = new List<hora>();
                    foreach (hora hora in horasx)
                    {
                        switch (hora.dia)
                        {
                            case 1:
                                // Dom
                                cmdStr = cmdStr + "HrADom='1900-01-01 " + hora.abre + ",', HxADom='1900-01-01" + hora.abre + ",";
                                cmdStr = cmdStr + "HrFDom='1900-01-01 " + hora.fecha + ",', HxADom='1900-01-01 " + hora.fecha + ",";
                                break;

                            case 2:
                                //Seg
                                cmdStr = cmdStr + "HrA2a6 ='1900-01-01 " + hora.abre + ",', HrA2a6='1900-01-01 " + hora.abre + ",";
                                cmdStr = cmdStr + "HrF2a6 ='1900-01-01 " + hora.fecha + ",', HrF2a6='1900-01-01 " + hora.fecha + ",";
                                break;
                            case 3:
                                //Ter
                                cmdStr = cmdStr + "HrA3='1900-01-01 " + hora.abre + ",', HrA3='1900-01-01 " + hora.abre + ",";
                                cmdStr = cmdStr + "HrF3 ='1900-01-01 " + hora.fecha + ",', HrF3='1900-01-01 " + hora.fecha + ",";
                                break;
                            case 4:
                                //Qua
                                cmdStr = cmdStr + "HrA4 ='1900-01-01 " + hora.abre + ",', HrA4='1900-01-01 " + hora.abre + ",";
                                cmdStr = cmdStr + "HrF4 ='1900-01-01 " + hora.fecha + ",', HrF4='1900-01-01 " + hora.fecha + ",";
                                break;
                            case 5:
                                //Qui
                                cmdStr = cmdStr + "HrA5 ='1900-01-01 " + hora.abre + ",', HrA5='1900-01-01 " + hora.abre + ",";
                                cmdStr = cmdStr + "HrF5 ='1900-01-01 " + hora.fecha + ",', HrF5='1900-01-01 " + hora.fecha + ",";
                                break;
                            case 6:
                                //Sex
                                cmdStr = cmdStr + "HrA6 ='1900-01-01 " + hora.abre + ",', HrA6='1900-01-01" + hora.abre + ",";
                                cmdStr = cmdStr + "HrF6 ='1900-01-01 " + hora.fecha + ",', HrF6='1900-01-01" + hora.fecha + ",";
                                break;
                            case 7:
                                //Sab
                                cmdStr = cmdStr + "HrASab ='1900-01-01 " + hora.abre + ",', HrASab='1900-01-01 " + hora.abre + ",";
                                cmdStr = cmdStr + "HrFSab ='1900-01-01 " + hora.fecha + ",', HrFSab='1900-01-01 " + hora.fecha + ",";
                                break;
                                

                        }
                        string straux = "";
                        cmdStr = straux + cmdStr + " where loja= " + restx.restid.ToString();
                        straux = ",";
                        
                    } 
             //       SqlDataReader dr = cmd.ExecuteReader();
                    cmd = new SqlCommand(cmdStr, conn);
                    cmd.ExecuteNonQuery();
                    //return cmdStr;
                }

                /*  using (var conn = new SqlConnection(connStr))
                     {

                   }*/
                              

         //       return cmdStr;



                conn.Close();
                // restx = Convert.ToString
                // restx.horas = horas;
                //     return List<hora>;
                
            
            }
            return msg;
            //   return restx;

        }


        // GET: api/restaurante
        /// <summary>
        /// Retorna o "status" atual de todos os restaurantes.
        /// OBS1: restaurante.TxEnt retorna sempre zerada, pois a taxa varia de acordo com o local.
        /// Para saber qual a taxa de entrega somente informando o local, ou seja, get api/restaurante/{locid} ou post api/restaurante com endereço.
        /// OBS2: restaurante.Tempo de entrega em minutos. Se for ZERO o delivery está FECHADO e se for NEGATIVO o restaurante está chaveado.
        /// Quando retornar chaveado para se saber qual restaurante atende, se houver, somente informando o local, ou seja, get api/restaurante/{locid} ou post api/restaurante com endereço.
        /// </summary>
        /// <returns></returns>
        [Route("")]
        public IEnumerable<restaurante> GetLojas()
        {
            return listRest();
        }

        // GET: api/restaurante/5
        /// <summary>
        /// Retorna o "status" atual do restaurante identificado por id. 
        /// Retorna 404 Not Found, se Restaurante inexistente ou sem delivery.
        /// OBS1: restaurante.TxEnt retorna sempre zerada, pois a taxa varia de acordo com o local.
        /// Para saber qual a taxa de entrega somente informando o local, ou seja, get api/restaurante/{locid} ou post api/restaurante com endereço.
        /// OBS2: restaurante.Tempo de entrega em minutos. Se for ZERO o delivery está FECHADO e se for NEGATIVO o restaurante está chaveado.
        /// Quando retornar chaveado para se saber qual restaurante atende, se houver, somente informando o local, ou seja, get api/restaurante/{locid} ou post api/restaurante com endereço.
        /// </summary>
        /// <param name="id">identificador do restaurante no Sistema Delivery, conforme valor retornado pela api/restaurante em restid</param>
        /// <returns></returns>
        [Route("{id}")]
        public restaurante GetLoja(int id)
        {
            restaurante restx = leRest(id);
            if (restx.restid == 0)
            {
                HttpError err = new HttpError("Restaurante inexistente ou sem delivery");
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));
            }
            else
            {
                return restx;
            }
        }
        /*
                // GET: api/restaurante/loc/id
                /// <summary>
                /// Retorna os dados do(s) restaurante(s) pronto(s) para atender o local do cliente identificado por id
                /// (onde id (locid) = identificador do local do cliente retornado pela API login).
                /// Retorna 400 Bad Request, se id (locid) mal formatado.
                /// Retorna 404 Not Found, se id (locid) mal formado.
                /// Retorna 204 No content, se restaurante(s) estiver(em) fechados.
                /// </summary>
                /// <param>Identificador do local do cliente = local.locid.</param>
                /// <returns></returns>
                [Route("loc/{id}")]
                public IEnumerable<restaurante> Get(string id)
                {
                    int diax = DateTime.Now.DayOfYear;
                    int eid = 0;
                    int ficha = 0;
                    int eidx = 0;

                    int p1 = id.IndexOf(".");
                    int p2 = id.IndexOf(".", p1 + 1);
                    if (!(p1 > 0 && p2 > p1 + 1 && id.Length > p2 + 1))
                    {
                        HttpError err = new HttpError("id (id) mal formatado");
                        throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.BadRequest, err));
                    }
                    else
                    {
                        string str1 = id.Substring(0, p1);
                        string str2 = id.Substring(p1 + 1, p2 - p1 - 1);
                        string str3 = id.Substring(p2 + 1, id.Length - p2 - 1);
                        Int32.TryParse(str1, out eid);
                        Int32.TryParse(str2, out ficha);
                        Int32.TryParse(str3, out eidx);
                        if (!(eid > 0 && eidx != 0 && 2 * diax == eid - eidx))
                        {
                            HttpError err = new HttpError("locid inexistente ou mal formado");
                            throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));
                        }
                        else
                        {
                            eid = eid - diax;
                            ficha = ficha - diax;
                            if (eid <= 0 || ficha <= 10)
                            {
                                HttpError err = new HttpError("locid mal formado");
                                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));
                            }
                            else
                            {
                                List<restaurante> lista = ListaDeRest(eid, ficha);
                                if (lista.Count == 0)
                                {
                                    HttpError err = new HttpError("restaurante(s) fechados");
                                    throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NoContent, err));
                                }
                                else
                                {
                                    return lista;
                                }
                            }
                        }
                    }
                }
        */
        // GET: api/restaurante/ucep/cep.num
        /// <summary>
        /// Retorna os dados do restaurante pronto para atender o cep.numero fornecido.
        /// Enviar cep sempre com 8 digitos, e, numero=0 se não tiver, por exemplo em Brasília.
        /// Retorna 400 Bad Request, se cep.numero mal formatado.
        /// Retorna 422 Unprocessable Entity, se CEP em cidade sem Delivery.
        /// Retorna 404 NotFound, se restaurante inexistente ou cep sem delivery.
        /// Retorna 204 No content, se Delivery do Restaurante estiver fechado.
        /// </summary>
        /// <param>cep.numero</param>
        /// <returns></returns>
        [Route("ucep/{cepnum}"), HttpGet]
        public restaurante ucep(string cepnum)
        {
            string cep = "";
            int num = 0;
            if (cepnum.IndexOf(".") != 8)
            {
                HttpError err = new HttpError("cep.numero mal formatado");
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.BadRequest, err));
            }
            else
            {
                cep = cepnum.Substring(0, 8);
                Int32.TryParse(cepnum.Substring(9), out num);
                string uf = ufCep(cep);
                if (uf == "")
                {
                    HttpError err = new HttpError("CEP em cidade sem Delivery");
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, err));
                }
                else
                {
                    //                    string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;
                    //                    string cmdStr = "select ficha from giraffas.dbo.eficha where (st is null or st<255) and eid='" + eid.ToString() + "' and ficha<>'" + ficha.ToString() + "'";
                    //                    using (var conn = new SqlConnection(connStr))
                    //                    {
                    //                        conn.Open();
                    //                        var cmd = new SqlCommand(cmdStr, conn);
                    //                        SqlDataReader dr = cmd.ExecuteReader();
                    //                        while (dr.Read())
                    //                        {
                    //                            lista.Add(lelocal(eid, dr.GetInt32(0)));
                    //                        }
                    //                        dr.Close();
                    //                        conn.Close();
                    //                    }


                    //restaurante restx = leRest(120);
                    restaurante restx = leRestx(cep, uf);
                    if (restx.restid == 0)
                    {
                        HttpError err = new HttpError("Restaurante inexistente ou sem delivery");
                        throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));
                    }
                    else
                    {
                        if (restx.Tempo == 0)
                        {
                            HttpError err = new HttpError("Delivery fechado");
                            throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NoContent, err));
                        }
                        else
                        {
                            return restx;
                        }
                    }
                }
            }
        }

        // GET: api/restaurante/cep/cep.num
        /// <summary>
        /// Retorna os dados de todos os restaurantes, inclusive os Fechados, que podem atender o cep.numero fornecido.
        /// Enviar cep sempre com 8 digitos, e, numero=0 se não tiver, por exemplo em Brasília.
        /// Retorna 400 Bad Request, se cep.numero mal formatado.
        /// Retorna 422 Unprocessable Entity, se CEP em cidade sem Delivery.
        /// Retorna vazio, se restaurantes inexistentes, ou cep sem delivery.
        /// </summary>
        /// <param>cep.numero</param>
        /// <returns></returns>
        [Route("cep/{cepnum}"), HttpGet]
        public IEnumerable<restaurante> cep(string cepnum)
        {
            string cep = "";
            int num = 0;
            if (cepnum.IndexOf(".") != 8)
            {
                HttpError err = new HttpError("cep.numero mal formatado");
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.BadRequest, err));
            }
            else
            {
                cep = cepnum.Substring(0, 8);
                Int32.TryParse(cepnum.Substring(9), out num);
                string uf = ufCep(cep);
                if (uf == "")
                {
                    HttpError err = new HttpError("CEP em cidade sem Delivery");
                    throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, err));
                }
                else
                {
                    //                    string connStr = ConfigurationManager.ConnectionStrings["connWebConfig"].ConnectionString;
                    //                    string cmdStr = "select ficha from giraffas.dbo.eficha where (st is null or st<255) and eid='" + eid.ToString() + "' and ficha<>'" + ficha.ToString() + "'";
                    //                    using (var conn = new SqlConnection(connStr))
                    //                    {
                    //                        conn.Open();
                    //                        var cmd = new SqlCommand(cmdStr, conn);
                    //                        SqlDataReader dr = cmd.ExecuteReader();
                    //                        while (dr.Read())
                    //                        {
                    //                            lista.Add(lelocal(eid, dr.GetInt32(0)));
                    //                        }
                    //                        dr.Close();
                    //                        conn.Close();
                    //                    }


                    //restaurante restx = leRestx(cep, uf);
                    //if (restx.restid == 0)
                    //{
                    //    HttpError err = new HttpError("Restaurante inexistente ou sem delivery");
                    //    throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));
                    //}
                    //else
                    //{
                    //    if (restx.Tempo == 0)
                    //    {
                    //        HttpError err = new HttpError("Delivery fechado");
                    //        throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NoContent, err));
                    //    }
                    //    else
                    //    {
                    //        return restx;
                    //    }
                    //}
                    return leRestsx(cep, uf);
                }
            }
        }

        // POST: api/restaurante
        /// <summary>
        /// Retorna os dados do(s) restaurante(s) pronto(s) para atender o endereço fornecido.
        /// Retorna 400 Bad Request, se Post mal formatado.
        /// Retorna 204 No content, Não tem restaurantes com delivery para este endereço.
        /// Obs: restaurante.Tempo = 0 significa que o delivery do restaurante está Fechado.
        /// </summary>
        /// <param name="lugarend">Endereço fornecido</param>
        /// <returns></returns>
        [Route(""), HttpPost]
        public IEnumerable<restaurante> Post([FromBody] endereco lugarend)
        {
            //if (!this.ModelState.IsValid)
            // if (!this.ModelState.IsValid || !(ipAddress.Substring(0, 12) == "189.26.224.1" || ipAddress.Substring(0, 13) == "189.125.163.1" || ipAddress == "107.21.108.82" || ipAddress == "::1" || ipAddress == "177.142.79.46"))
            if (!this.ModelState.IsValid || (ipspodem != "todos" && ipspodem.IndexOf("x" + ipAddress.TrimEnd() + "x") < 0))
            {
                HttpError err = new HttpError("Post mal formatado");
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, err));
            }
            else
            {
                List<restaurante> lista = testaEnde(lugarend);
                if (lista.Count == 0)
                {
                    HttpError err = new HttpError("Não tem restaurantes com delivery para este endereço");
                    throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NoContent, err));
                }
                else
                {
                    return lista;
                }
            }
        }
        /*
                // POST: api/restaurante
                [ApiExplorerSettings(IgnoreApi = true)]
                public void Post([FromBody]string value)
                {
                }

                // PUT: api/restaurante/5
                public void Put(int id, [FromBody]string value)
                {
                }

                // DELETE: api/restaurante/5
                public void Delete(int id)
                {
                }
        */
        [Route("hora/{id}")]
        public restauranteHora GetHora(int id)
        {
            restauranteHora restx = leHora(id);
            if (restx.restid == 0)
            {
                HttpError err = new HttpError("Restaurante inexistente ou sem delivery");
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));
            }
            else
            {
                return restx;
            }

        }
        // GET:alterahora

        /// <summary>
        /// 
        /// </summary>
        /// <param name="horax"></param>
        /// <returns></returns>
        [Route("sethoras/{restid}/{hora}"), HttpPut]
        public HttpResponseMessage SetHora([FromBody] int id,hora horasx)
      //  public async Task <string> GetHoraStr(int id)
        {
            string msg = "";
          
        //     string restx = leHoraStr(id);
     /*       if (id == 0 )
            {
                HttpError err = new HttpError("Restaurante inexistente ou sem delivery");
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound, err));
            }
            else
            {*/

                if (!this.ModelState.IsValid || (ipspodem != "todos" && ipspodem.IndexOf("x" + ipAddress.TrimEnd() + "x") < 0))
                {
                    HttpError err = new HttpError("Post mal formado");
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, err));
                }
                else
                {
                    if (msg == "") 
                    if (msg != "")
                    {
                        HttpError err = new HttpError(msg);
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound, err));
                    }
                    else
                    {
                        HttpMsgOK msgok = new HttpMsgOK { };
                        msgok.msgok = "OK";
                        HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK, msgok);
                        return response;
                   
                      //  return restx;
                    }

                }
            HttpMsgOK msgok2 = new HttpMsgOK { };
            HttpResponseMessage response2 = this.Request.CreateResponse(HttpStatusCode.OK, msgok2);
                return response2;
          //  }
        }

        
    }
}



