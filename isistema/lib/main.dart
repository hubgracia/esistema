import 'dart:io';

import 'package:flutter/material.dart';
import 'package:isistema/Homepage.dart';

///Apenas inícia o App e encaminha para a Homepage

void main() {
  runApp(MyApp());
  final ThemeData themeData = ThemeData(
    canvasColor: Colors.redAccent,
  );
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext ctxt) {
    return new MaterialApp(
      debugShowCheckedModeBanner: false,
      home: new Home(),
    );
  }
}
