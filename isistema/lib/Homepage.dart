import 'package:flutter/material.dart';
import 'package:isistema/GetHoras.dart';
import 'package:isistema/GetPostPlaceholder.dart';
import 'package:isistema/GetRest.dart';
// import 'package:isistema/test.dart';

///Página Principal do aplicativo
///Aqui estaram as opções do usuário para interagir com a API
///
class Home extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: new AppBar(title: new Text("ListaHoras"), actions: <Widget>[
          FlatButton(
            textColor: Colors.white,
            onPressed: () {
              Navigator.push(context,
                  new MaterialPageRoute(builder: (ctxt) => new Home()));
            },
            child: Text("API"),
            shape: CircleBorder(side: BorderSide(color: Colors.white)),
          ),
        ]),
        body: Column(children: <Widget>[
          Container(
            color: Colors.grey[200],
            child: (Text('ISistema',
                style: TextStyle(color: Colors.black, fontSize: 40.0))),
          ),
          Container(
              color: Colors.grey[300],
              child: Padding(
                padding: EdgeInsets.fromLTRB(50, 25, 50, 25),
                child: Text(
                  'Giraffas Isistema API é o conjunto de APIs para inclusão no Sistema Delivery©'
                  ' de cadastros de usuários, com endereços validos de entrega, e seus pedidos delivery.'
                  ' Esse conjunto de APIs partiu do conjunto genérico "Giraffas Delivery ECommerce API na v1.0" para então ser customizado com objetivo a integração com Sistema Call Center',
                  style: TextStyle(color: Colors.black, fontSize: 20.0),
                ),
              )),
          Container(
              color: Colors.white10,
              child: Padding(
                padding: EdgeInsets.fromLTRB(12, 6, 12, 6),
                child: TextButton(
                  child: Text(
                    'Lista de Restaurantes',
                    style: TextStyle(color: Colors.black, fontSize: 20.0),
                  ),
                  onPressed: () {
                    Navigator.push(
                        context,
                        new MaterialPageRoute(
                            builder: (ctxt) => new RestauranteApp()));
                  },
                ),
              )),
          Container(
              color: Colors.white10,
              child: Padding(
                padding: EdgeInsets.fromLTRB(12, 6, 12, 6),
                child: TextButton(
                  child: Text(
                    'Lista de Horas',
                    style: TextStyle(color: Colors.black, fontSize: 20.0),
                  ),
                  onPressed: () {
                    Navigator.push(
                        context,
                        new MaterialPageRoute(
                            builder: (ctxt) => new Gethoras()));
                  },
                ),
              )),
          Container(
              color: Colors.white10,
              child: Padding(
                padding: EdgeInsets.fromLTRB(12, 6, 12, 6),
                child: TextButton(
                  child: Text(
                    'Lista de Texto',
                    style: TextStyle(color: Colors.black, fontSize: 20.0),
                  ),
                  onPressed: () {
                    Navigator.push(
                        context,
                        new MaterialPageRoute(
                            builder: (ctxt) => new PostApp()));
                  },
                ),
              )),
          Container(
              color: Colors.white10,
              child: Padding(
                padding: EdgeInsets.fromLTRB(12, 6, 12, 6),
                child: TextButton(
                  child: Text(
                    'Widget Teste',
                    style: TextStyle(color: Colors.black, fontSize: 20.0),
                  ),
                  onPressed: () {
                    Navigator.push(
                        context,
                        new MaterialPageRoute(
                            builder: (ctxt) => new ListaHoras()));
                  },
                ),
              ))
        ]));
  }
}
