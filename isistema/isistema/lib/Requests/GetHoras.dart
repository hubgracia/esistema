import 'dart:convert';
import 'dart:html';

import 'package:flutter/material.dart';
import 'package:admin/main.dart';
import 'package:admin/screens/dashboard/dashboard_screen.dart';
import 'package:http/http.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'package:admin/Models/models.dart';

/*class Gethoras extends StatefulWidget {
  @override
  _Gethoras createState() => _Gethoras();
}*/

/// Futuro GET request

class HttpService {
  ///URL de GET está com um proxy para autorização CORS futuramente ver se é possível criar as autorizações no esistema
  final String url =
      "https://thingproxy.freeboard.io/fetch/http://rede.jgracia.com.br/esistema/api/restaurante/hora/120";

  Future<List<Hora>> getRest() async {
    Response res = await http.get(Uri.parse(url));
    if (res.statusCode == 200) {
      List<dynamic> body = jsonDecode("[" + res.body + "]");

      List<Hora> horasx = body
          .map(
            (dynamic item) => Hora.fromJson(item),
          )
          .toList();

      return horasx;
    } else {
      throw "Falha na operação";
    }
  }
/*
  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    throw UnimplementedError();
  }
  */
}

///
///Widget para receber o id do restaurante a ser alterado.
///
///
class IdSelectHora extends StatelessWidget {
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
                              builder: (ctxt) => new ListaHoras()));
                    })
              ]),
        ));
  }
}

///Widget com outro design de lista com botão para editar e voltar
class ListaHoras extends StatelessWidget {
  final HttpService httpService = HttpService();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Horas"),
      ),
      body: FutureBuilder(
        future: httpService.getRest(),
        builder: (BuildContext context, AsyncSnapshot<List<Hora>> snapshot) {
          if (snapshot.hasData) {
            List<Hora>? horasx = snapshot.data;
            return ListView(
              children: horasx!
                  .map(
                    (Hora rest) => ListTile(
                        title: Text('Id do Hora :'
                            " ${horasx}\n"
                            /*  " ${horasx[1]}\n"
                            " ${horasx[2]}\n"
                            " ${horasx[3]}\n"
                            " ${horasx[4]}\n"
                            " ${horasx[5]}\n"
                            " ${horasx[6]}\n"*/
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
