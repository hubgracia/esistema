///Arquivo com os Models e suas conversões

import 'dart:convert';

Restaurante restauranteFromJson(String str) =>
    Restaurante.fromJson(json.decode(str));

String restauranteToJson(Restaurante data) => json.encode(data.toJson());

class Restaurante {
  Restaurante(
      {this.restid = 0,
      this.cardapioid = 0,
      this.cardapiolocid = 0,
      this.restNome = "",
      this.restCep = "",
      this.restEnde = "",
      this.restBairro = "",
      this.restCid = "",
      this.restUf = "",
      this.cnpj = "",
      this.restInicio = "",
      this.restFim = "",
      this.tempo = 0,
      this.txEnt = 0,
      this.pgtoCod = "",
      this.restarea = "",
      this.locInicio = "",
      this.locFim = "",
      this.locStatus = 0,
      this.delmin = 0,
      this.locmin = 0,
      this.feijao = "",
      required this.horas});

  int restid;
  int cardapioid;
  int cardapiolocid;
  String restNome;
  String restCep;
  String restEnde;
  String restBairro;
  String restCid;
  String restUf;
  String cnpj;
  String restInicio;
  String restFim;
  int tempo;
  int txEnt;
  String pgtoCod;
  String restarea;
  String locInicio;
  String locFim;
  int locStatus;
  int delmin;
  int locmin;
  String feijao;
  List<Hora> horas;

  factory Restaurante.fromJson(Map<String, dynamic> json) => Restaurante(
        restid: json["restid"],
        cardapioid: json["cardapioid"],
        cardapiolocid: json["cardapiolocid"],
        restNome: json["restNome"],
        restCep: json["restCep"],
        restEnde: json["restEnde"],
        restBairro: json["restBairro"],
        restCid: json["restCid"],
        restUf: json["restUf"],
        cnpj: json["cnpj"],
        restInicio: json["restInicio"],
        restFim: json["restFim"],
        tempo: json["Tempo"],
        txEnt: json["TxEnt"],
        pgtoCod: json["pgtoCod"],
        restarea: json["restarea"],
        locInicio: json["locInicio"],
        locFim: json["locFim"],
        locStatus: json["locStatus"],
        delmin: json["delmin"],
        locmin: json["locmin"],
        feijao: json["feijao"],
        horas: List<Hora>.from(json["horas"].map((x) => Hora.fromJson(x))),
      );

  Map<String, dynamic> jsonDecode(jsonString) => {
        "restid": restid,
        "cardapioid": cardapioid,
        "cardapiolocid": cardapiolocid,
        "restNome": restNome,
        "restCep": restCep,
        "restEnde": restEnde,
        "restBairro": restBairro,
        "restCid": restCid,
        "restUf": restUf,
        "cnpj": cnpj,
        "restInicio": restInicio,
        "restFim": restFim,
        "Tempo": tempo,
        "TxEnt": txEnt,
        "pgtoCod": pgtoCod,
        "restarea": restarea,
        "locInicio": locInicio,
        "locFim": locFim,
        "locStatus": locStatus,
        "delmin": delmin,
        "locmin": locmin,
        "feijao": feijao,
        "horas": List<dynamic>.from(horas.map((x) => x.toJson())),
      };

  Object? toJson() {}
}

class Hora {
  Hora({
    this.dia = 0,
    this.abre = "error",
    this.fecha = "error",
  });

  int dia;
  String abre;
  String fecha;

  factory Hora.fromJson(Map<String, dynamic> json) => Hora(
        dia: json["dia"],
        abre: json["abre"],
        fecha: json["fecha"],
      );

  Map<String, dynamic> toJson() => {
        "dia": dia,
        "abre": abre,
        "fecha": fecha,
      };
}

Hora horasFromJson(String str) => Hora.fromJson(json.decode(str));

String horasToJson(Hora data) => json.encode(data.toJson());
// To parse this JSON data, do
//
//     final hora = horaFromJson(jsonString);

Hora horaFromJson(String str) => Hora.fromJson(json.decode(str));

String horaToJson(Hora data) => json.encode(data.toJson());
/*
class Hora {
  Hora({this.restid = 0, this.cardapioid = 0, required this.horas});

  int restid;
  int cardapioid;
  List<HoraElement> horas;

  factory Hora.fromJson(Map<String, dynamic> json) => Hora(
        restid: json["restid"],
        cardapioid: json["cardapioid"],
        horas: List<HoraElement>.from(
            json["horas"].map((x) => HoraElement.fromJson(x))),
      );

  Map<String, dynamic> toJson() => {
        "restid": restid,
        "cardapioid": cardapioid,
        "horas": List<dynamic>.from(horas.map((x) => x.toJson())),
      };
}

class HoraElement {
  HoraElement({
    this.dia = 0,
    this.abre = "",
    this.fecha = "",
  });

  int dia;
  String abre;
  String fecha;

  factory HoraElement.fromJson(Map<String, dynamic> json) => HoraElement(
        dia: json["dia"],
        abre: json["abre"],
        fecha: json["fecha"],
      );

  Map<String, dynamic> toJson() => {
        "dia": dia,
        "abre": abre,
        "fecha": fecha,
      };
}
*/