import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart';
import 'dart:async';
import 'package:http/http.dart' as http;
import 'package:flutter/material.dart';
import 'package:isistema/models.dart';

class HttpService {
  final String postsURL =
      "http://rede.giraffasdelivery.com.br/esistema/api/restaurante";
  Future<List<Restaurante>> getRest() async {
    Response res = await http.get(Uri.parse(postsURL), headers: {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    });

    HttpHeaders.authorizationHeader;

    if (res.statusCode == 200) {
      List<dynamic> body = jsonDecode(res.body);

      List<Restaurante> posts = body
          .map(
            (dynamic item) => Restaurante.fromJson(item),
          )
          .toList();

      return posts;
    } else {
      throw "Falha na operação";
    }
  }
}

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
                      title: Text(rest.restid.toString()),
                      subtitle: Text("${rest.restNome}"),
                    ),
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

class RestauranteApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'HTTP',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: RestGet(),
    );
  }
}
