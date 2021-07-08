import 'dart:html';

import 'package:flutter/material.dart';
import 'package:isistema/main.dart';
import 'package:isistema/Homepage.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
class Gethoras extends StatefulWidget {
  @override
  _Gethoras createState() => _Gethoras();
}

/// Futuro GET request

class _Gethoras extends State<Gethoras> {
  Future getHorasData() async {
    List horas = [];

    Future<http.Response> GetHoras() {
      return http.get(Uri.parse('')
          /*if (Response.statusCode == 200) {
      var jsonData = jsonDecode(response.body);
    } else {
      Error();
    });
    /*   for (var u in jsonData) {
      Gethoras hora = Gethoras(u['dia'], u["inicio"], u["fim"]);
      horas.add(hora);
    */*/
          );
    }

    print(horas.length);
    return horas;
  }

///
///Widget para receber o id do restaurante a ser alterado.
///
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: new AppBar(title: new Text("ListaHoras"), actions: <Widget>[
        FlatButton(
          textColor: Colors.white,
          onPressed: () {
            Navigator.push(
                context, new MaterialPageRoute(builder: (ctxt) => new Home()));
          },
          child: Text("API"),
          shape: CircleBorder(side: BorderSide(color: Colors.white)),
        ),
      ]),
      body: new Container(
          padding: const EdgeInsets.all(40.0),
          child: new Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              new TextField(
                decoration: new InputDecoration(labelText: "ID do restaurante"),
                keyboardType: TextInputType.number,
              ),
              FlatButton(child: Text('Confirmar'), onPressed: getHorasData)
            ],
          )),
    );
  }
}

///Widget com outro design de lista com botão para editar e voltar
class ListaHoras extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    Scaffold(
      appBar: new AppBar(title: new Text("isistema"), actions: <Widget>[
        // ignore: deprecated_member_use
        FlatButton(
          textColor: Colors.black,
          onPressed: () {
            Navigator.push(
                context, new MaterialPageRoute(builder: (ctxt) => new Home()));
          },
          child: Text("API"),
          shape: CircleBorder(side: BorderSide(color: Colors.transparent)),
        ),
      ]),
    );
    return Center(
      child: Card(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: <Widget>[
            const ListTile(
              leading: Icon(Icons.list_alt_sharp),
              title: Text('Horas'),
              subtitle: Text('Lista'),
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: <Widget>[
                TextButton(
                  child: const Text('Editar'),
                  onPressed: () {
                    Navigator.push(
                        context,
                        new MaterialPageRoute(
                            //futuro Puthoras
                            builder: (ctxt) => new Gethoras()));
                  },
                ),
                const SizedBox(width: 8),
                TextButton(
                  child: const Text('Voltar'),
                  onPressed: () {
                    Navigator.push(context,
                        new MaterialPageRoute(builder: (ctxt) => new Home()));
                  },
                ),
                const SizedBox(width: 8),
              ],
            ),
          ],
        ),
      ),
    );
  }
}
