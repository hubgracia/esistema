import 'dart:html';

import 'package:flutter/material.dart';
import 'package:isistema/main.dart';
import 'package:isistema/Homepage.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'dart:convert';

/*import 'package:isistemaflutter/views.dart';
import 'package:isistemaflutter/main.dart';
import 'package:isistemaflutter/models.dart';
import 'package:flutter/material.dart';*/

class Gethoras extends StatefulWidget {
  @override
  _Gethoras createState() => _Gethoras();
}

class _Gethoras extends State<Gethoras> {
  Future getHorasData() async {
    List horas = [];

    Future<http.Response> GetHoras() {
      return http.get(Uri.parse('https://jsonplaceholder.typicode.com/albums/1')
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
