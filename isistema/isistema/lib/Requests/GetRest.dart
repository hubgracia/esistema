import 'dart:convert';
// import 'dart:io';
import 'package:admin/screens/main/components/side_menu.dart';
import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:admin/Models/restmodel.dart';

import '../responsive.dart';

///GET request
///URL de GET está com um proxy para autorização CORS futuramente ver se é possível criar as autorizações no esistema
final String url =
    "https://thingproxy.freeboard.io/fetch/http://rede.jgracia.com.br/esistema/api/restaurante/";

Future<List<Restaurante>> getRest(restid) async {
  Response res = await http.get(
    Uri.parse(url + "$restid"),
  );
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

///Widget responsável por criar a tela principal
///Puxando o Widget de SideMenu e _RestGetState
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

class RestGet extends StatefulWidget {
  RestGet({Key? key}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return _RestGetState();
  }
}

///Widget de visualização da resposta
class _RestGetState extends State<RestGet> {
  final TextEditingController _controller = TextEditingController();
  //get restid => _IdState(_IdState);
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
          title: new TextField(
            controller: _controller,
            decoration: new InputDecoration(labelText: "ID do restaurante"),
            keyboardType: TextInputType.number,
          ),
          actions: <Widget>[
            FlatButton(
              textColor: Colors.white,
              onPressed: () {
                setState(
                  () {
                    Future<List<Restaurante>> _futureId =
                        getRest(_controller.text);
                    /*      Navigator.push(
                        context,
                        new MaterialPageRoute(
                            builder: (ctxt) => new RestGet()));*/
                  },
                );
              },
              child: Text("Buscar Restaurante"),
            ),
          ]),
      body: FutureBuilder(
        future: getRest(_controller.text),
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
                            "\n"
                            'Horários :\n'
                            "\n"
                            " Dia : ${rest.horas[0].dia}\n"
                            " Abre : ${rest.horas[0].abre}\n"
                            " Fecha : ${rest.horas[0].fecha}\n"
                            "\n"
                            " Dia : ${rest.horas[1].dia}\n"
                            " Abre : ${rest.horas[1].abre}\n"
                            " Fecha : ${rest.horas[1].fecha}\n"
                            "\n"
                            " Dia : ${rest.horas[2].dia}\n"
                            " Abre : ${rest.horas[2].abre}\n"
                            " Fecha : ${rest.horas[2].fecha}\n"
                            "\n"
                            " Dia : ${rest.horas[3].dia}\n"
                            " Abre : ${rest.horas[3].abre}\n"
                            " Fecha : ${rest.horas[3].fecha}\n"
                            "\n"
                            " Dia : ${rest.horas[4].dia}\n"
                            " Abre : ${rest.horas[4].abre}\n"
                            " Fecha : ${rest.horas[4].fecha}\n"
                            "\n"
                            " Dia : ${rest.horas[5].dia}\n"
                            " Abre : ${rest.horas[5].abre}\n"
                            " Fecha : ${rest.horas[5].fecha}\n"
                            "\n"
                            " Dia : ${rest.horas[6].dia}\n"
                            " Abre : ${rest.horas[6].abre}\n"
                            " Fecha : ${rest.horas[6].fecha}\n"
                            "\n"
                            "")),
                  )
                  .toList(),
            );
          } else {
            return Center(
                child: Text("Insira acima o ID do restaurante desejado."));
          }
        },
      ),
    );
  }
}
