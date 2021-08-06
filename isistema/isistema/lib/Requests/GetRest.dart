import 'dart:convert';
// import 'dart:io';
import 'package:admin/controllers/MenuController.dart';
import 'package:admin/main.dart';
import 'package:admin/screens/main/components/side_menu.dart';
import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:admin/Models/models.dart';

import 'package:admin/constants.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:admin/screens/dashboard/components/header.dart';
import '../responsive.dart';

///GET request
class HttpService {
  ///URL de GET está com um proxy para autorização CORS futuramente ver se é possível criar as autorizações no esistema
  final String url =
      "https://thingproxy.freeboard.io/fetch/http://rede.jgracia.com.br/esistema/api/restaurante/120";

  Future<List<Restaurante>> getRest() async {
    Response res = await http.get(Uri.parse(url));
    if (res.statusCode == 200) {
      List<dynamic> body = jsonDecode("[" + res.body + "]");

      List<Restaurante> rest = body
          .map(
            (dynamic item) => Restaurante.fromJson(item),
          )
          .toList();

      return rest;
    } else {
      throw "Falha na operação";
    }
  }
}

//Widget para selecionar ID do Restaurante

class IdSelectRest extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: new AppBar(title: new Text("Horas"), actions: <Widget>[
          FlatButton(
            textColor: Colors.white,
            onPressed: () {
              Navigator.push(context,
                  new MaterialPageRoute(builder: (ctxt) => new MyApp()));
            },
            child: Text("Cancelar"),
            //shape: CircleBorder(side: BorderSide(color: Colors.white)),
          ),
        ]),
        body: new Container(
          padding: const EdgeInsets.all(280.0),
          child: new Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: <Widget>[
                new TextField(
                  decoration:
                      new InputDecoration(labelText: "ID do restaurante"),
                  keyboardType: TextInputType.number,
                ),
                FlatButton(
                    child: Text('Confirmar'),
                    onPressed: () {
                      Navigator.push(
                          context,
                          new MaterialPageRoute(
                              builder: (ctxt) => new MainScreenRest()));
                    })
              ]),
        ));
  }
}

///Widget responsável por criar a tela principal
///Puxando o Widget de SideMenu e RestGet
///Colocando-os na mesma tela

class MainScreenRest extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: SideMenu(),
      body: SafeArea(
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (Responsive.isDesktop(context))
              Expanded(
                child: SideMenu(),
              ),
            Expanded(
              flex: 5,
              child: RestGet(),
            ),
          ],
        ),
      ),
    );
  }
}

///Widget de visualização da resposta
class RestGet extends StatelessWidget {
  final HttpService httpService = HttpService();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Restaurantes"),
      ),
      body: FutureBuilder(
        future: httpService.getRest(),
        builder:
            (BuildContext context, AsyncSnapshot<List<Restaurante>> snapshot) {
          if (snapshot.hasData) {
            List<Restaurante>? rest = snapshot.data;
            return ListView(
              children: rest!
                  .map(
                    (Restaurante rest) => ListTile(
                        title: Text('Id do Restaurante :'
                            "${rest.restid}\n"
                            'Nome: '
                            "${rest.restNome}\n"
                            'locStatus: '
                            " ${rest.locStatus}\n"),
                        subtitle: Text('Outros Dados: '
                            'cardapioid :'
                            " ${rest.cardapioid}\n"
                            'cardapiolocid :'
                            " ${rest.cardapiolocid}\n"
                            'restNome :'
                            " ${rest.restNome}\n"
                            'restCep :'
                            " ${rest.restCep}\n"
                            'restEnde :'
                            " ${rest.restEnde}\n"
                            'restBairro :'
                            " ${rest.restBairro}\n"
                            'restCid :'
                            " ${rest.restCid}\n"
                            'restUf :'
                            " ${rest.restUf}\n"
                            'cnpj :'
                            " ${rest.cnpj}\n"
                            'restInicio :'
                            " ${rest.restInicio}\n"
                            'restFim :'
                            " ${rest.restFim}\n"
                            'tempo :'
                            " ${rest.tempo}\n"
                            'txEnt :'
                            " ${rest.txEnt}\n"
                            'pgtoCod :'
                            " ${rest.pgtoCod}\n"
                            'restarea :'
                            " ${rest.restarea}\n"
                            'locInicio :'
                            " ${rest.locInicio}\n"
                            'locFim :'
                            " ${rest.locFim}\n"
                            'locStatus :'
                            " ${rest.locStatus}\n"
                            'delmin :'
                            " ${rest.delmin}\n"
                            'locmin :'
                            " ${rest.locmin}\n"
                            'feijao :'
                            " ${rest.feijao}\n"
                            'horas :\n'
                            " ${rest.horas[0]}\n"
                            " ${rest.horas[1]}\n"
                            " ${rest.horas[2]}\n"
                            " ${rest.horas[3]}\n"
                            " ${rest.horas[4]}\n"
                            " ${rest.horas[5]}\n"
                            " ${rest.horas[6]}\n"
                            " // ")),
                  )
                  .toList(),
            );
          } else {
            return Center(child: CircularProgressIndicator());
          }
        },
      ),
    );
  }
}
