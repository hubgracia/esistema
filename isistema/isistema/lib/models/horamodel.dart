// To parse this JSON data, do
//
//     final listHora = listHoraFromJson(jsonString);

import 'dart:convert';

ListHora listHoraFromJson(String str) => ListHora.fromJson(json.decode(str));

String listHoraToJson(ListHora data) => json.encode(data.toJson());

class ListHora {
  ListHora({
    required this.restid,
    this.cardapioid = 0,
    required this.horas,
  });

  int restid;
  int cardapioid;
  List<Hora> horas;

  factory ListHora.fromJson(Map<String, dynamic> json) => ListHora(
        restid: json["restid"],
        cardapioid: json["cardapioid"],
        horas: List<Hora>.from(json["horas"].map((x) => Hora.fromJson(x))),
      );

  Map<String, dynamic> toJson() => {
        "restid": restid,
        "cardapioid": cardapioid,
        "horas": List<dynamic>.from(horas.map((x) => x.toJson())),
      };
}

class Hora {
  Hora({
    this.dia = 0,
    this.abre = "",
    this.fecha = "",
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
