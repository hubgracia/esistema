import 'dart:convert';
import 'dart:html';
import 'dart:js';
// import 'dart:io';
import 'package:admin/controllers/MenuController.dart';
import 'package:admin/main.dart';
import 'package:admin/models/horamodel.dart';
import 'package:admin/screens/main/components/side_menu.dart';
import 'package:flutter/cupertino.dart';
import 'package:http/http.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:admin/constants.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:admin/screens/dashboard/components/header.dart';
import '../responsive.dart';

///GET request
///
///

/*
class HttpService extends State<restid> {
  static String get text => "";

  //var IdController = TextEditingController();

  Widget build(BuildContext context) {
    return Scaffold();
  }
*/
///URL de GET está com um proxy para autorização CORS futuramente ver se é possível criar as autorizações no esistema
final String url =
    "https://thingproxy.freeboard.io/fetch/http://rede.jgracia.com.br/esistema/api/restaurante/hora/";

Future<List<ListHora>> putHora(restid) async {
  Response res = await http.put(Uri.parse(url + "$restid"),
      headers: <String, String>{
        'Content-Type': 'application/json; charset=UTF-8',
      },
      body: jsonEncode(<String, String>{
        'restid': restid,
      }));
  if (res.statusCode == 200) {
    List<dynamic> body = jsonDecode("[" + res.body + "]");

    List<ListHora> hora = body
        .map(
          (dynamic item) => ListHora.fromJson(item),
        )
        .toList();

    return hora;
  } else {
    throw "Falha na operação";
  }
}

///Widget responsável por criar a tela principal
///Puxando o Widget de SideMenu e _HoraPutState
///Colocando-os na mesma tela

class MainScreenPutHora extends StatelessWidget {
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
              child: HoraPut(),
            ),
          ],
        ),
      ),
    );
  }
}

class Edit extends StatefulWidget {
  Edit({Key? key}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return _HoraPutState();
  }
}

class _EditState extends State<Edit> {
  @override
  Widget build(BuildContext context) {
    return Scaffold();
  }
}

class HoraPut extends StatefulWidget {
  HoraPut({Key? key}) : super(key: key);

  @override
  State<StatefulWidget> createState() {
    return _HoraPutState();
  }
}

///Widget de visualização da resposta
class _HoraPutState extends State<HoraPut> {
  final TextEditingController _controller = TextEditingController();
  final TextEditingController _controller2 = TextEditingController();

  get restid => _controller.text;
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
                    Future<List<ListHora>> _futureId =
                        putHora(_controller.text);
                  },
                );
              },
              child: Text("Buscar Horários"),
            ),
          ]),
      body: FutureBuilder(
        future: putHora(_controller.text),
        builder:
            (BuildContext context, AsyncSnapshot<List<ListHora>> snapshot) {
          if (snapshot.hasData) {
            List<ListHora>? hora = snapshot.data;
            return ListView(
              children: hora!
                  .map((ListHora hora) => Card(
                        child: ListTile(
                            title: new TextField(
                              controller: _controller2,
                              decoration: new InputDecoration(
                                  labelText: "ID do restaurante"),
                              keyboardType: TextInputType.number,
                            ),
                            subtitle: Text('Horários:\n'
                                " Dia : ${hora.horas[0].dia}\n"
                                " Abre : ${hora.horas[0].abre}\n"
                                " Fecha : ${hora.horas[0].fecha}\n"
                                "\n"
                                " Dia : ${hora.horas[1].dia}\n"
                                " Abre : ${hora.horas[1].abre}\n"
                                " Fecha : ${hora.horas[1].fecha}\n"
                                "\n"
                                " Dia : ${hora.horas[2].dia}\n"
                                " Abre : ${hora.horas[2].abre}\n"
                                " Fecha : ${hora.horas[2].fecha}\n"
                                "\n"
                                " Dia : ${hora.horas[3].dia}\n"
                                " Abre : ${hora.horas[3].abre}\n"
                                " Fecha : ${hora.horas[3].fecha}\n"
                                "\n"
                                " Dia : ${hora.horas[4].dia}\n"
                                " Abre : ${hora.horas[4].abre}\n"
                                " Fecha : ${hora.horas[4].fecha}\n"
                                "\n"
                                " Dia : ${hora.horas[5].dia}\n"
                                " Abre : ${hora.horas[5].abre}\n"
                                " Fecha : ${hora.horas[5].fecha}\n"
                                "\n"
                                " Dia : ${hora.horas[6].dia}\n"
                                " Abre : ${hora.horas[6].abre}\n"
                                " Fecha : ${hora.horas[6].fecha}\n"
                                "\n"
                                "")),
                      ))
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
