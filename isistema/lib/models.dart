import 'package:flutter/foundation.dart';

class Restaurante {
  final int restid;
  final int cardapioid;
  final int cardapiolocid;
  final String restNome;
  final String restCep;
  final String restEnde;
  final String restBairro;
  final String restCid;
  final String restUf;
  final String cnpj;
  final String restInicio;
  final String restFim;
  final int Tempo;
  final int TxEnt;
  final String pgtoCod;
  final String restarea;
  final String locInicio;
  final String locFim;
  final int locStatus;
  final int delmin;
  final int locmin;
  final String feijao;
//  static const Horas horas;

  Restaurante({
    this.restid = 0,
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
    this.Tempo = 0,
    this.TxEnt = 0,
    this.pgtoCod = "",
    this.restarea = "",
    this.locInicio = "",
    this.locFim = "",
    this.locStatus = 0,
    this.feijao = "",
    this.delmin = 0,
    this.locmin = 0,
    //  this.horas = Horas.fromJson(json).dia
  });

  factory Restaurante.fromJson(Map<String, dynamic> json) {
    return Restaurante(
      restid: json['restid'] as int,
      cardapioid: json['cardapioid'],
      cardapiolocid: json['cardapiolocid'],
      restNome: json['restNome'] as String,
      restCep: json['restCep'],
      restEnde: json['restEnde'],
      restBairro: json['restBairro'],
      restCid: json['restCid'],
      restUf: json['restUf'],
      cnpj: json['cnpj'],
      restInicio: json['restInicio'],
      restFim: json['restFim'],
      Tempo: json['Tempo'],
      TxEnt: json['TxEnt'],
      pgtoCod: json['pgtoCod'],
      restarea: json['restarea'],
      locInicio: json['locInicio'],
      locFim: json['locFim'],
      locStatus: json['locStatus'],
      feijao: json['feijao'],
      delmin: json['delmin'],
      locmin: json['locmin'],
//       horas: json['horas']
    );
  }
}

class Horas {
  int dia = 0;
  String inicio = "";
  String fim = "";

  Horas({this.dia = 0, this.inicio = "", this.fim = ""});

  factory Horas.fromJson(Map<String, dynamic> json) {
    return Horas(
      dia: json['dia'],
      inicio: json['inicio'],
      fim: json['fim'],
    );
  }
}
