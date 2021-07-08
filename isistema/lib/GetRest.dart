import 'dart:convert';
// import 'dart:io';
import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:isistema/models.dart';

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

///Widget de para visualizar a resposta
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
                            "${rest.restid} "
                            ' // '
                            'Nome: '
                            "${rest.restNome} // "
                            'locStatus: '
                            " ${rest.locStatus} // "),
                        subtitle: Text('Outros Dados: '
                            'cardapioid :'
                            " ${rest.cardapioid} // "
                            'cardapiolocid :'
                            " ${rest.cardapiolocid} // "
                            'restNome :'
                            " ${rest.restNome} // "
                            'restCep :'
                            " ${rest.restCep} // "
                            'restEnde :'
                            " ${rest.restEnde} // "
                            'restBairro :'
                            " ${rest.restBairro} // "
                            'restCid :'
                            " ${rest.restCid} // "
                            'restUf :'
                            " ${rest.restUf} // "
                            'cnpj :'
                            " ${rest.cnpj} // "
                            'restInicio :'
                            " ${rest.restInicio} // "
                            'restFim :'
                            " ${rest.restFim} // "
                            'tempo :'
                            " ${rest.tempo} // "
                            'txEnt :'
                            " ${rest.txEnt} // "
                            'pgtoCod :'
                            " ${rest.pgtoCod} // "
                            'restarea :'
                            " ${rest.restarea} // "
                            'locInicio :'
                            " ${rest.locInicio} // "
                            'locFim :'
                            " ${rest.locFim} // "
                            'locStatus :'
                            " ${rest.locStatus} // "
                            'delmin :'
                            " ${rest.delmin} // "
                            'locmin :'
                            " ${rest.locmin} // "
                            'feijao :'
                            " ${rest.feijao} // "
                            'horas :'
                            " ${rest.horas}"
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

///Widget para corpo da Página

class RestauranteApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Restaurante',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: RestGet(),
    );
  }
}
