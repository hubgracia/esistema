import 'package:flutter/material.dart';
import 'package:isistema/Homepage.dart';

void main() => runApp(MyApp());
final ThemeData themeData = ThemeData(
  canvasColor: Colors.redAccent,
);

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext ctxt) {
    return new MaterialApp(
      debugShowCheckedModeBanner: false,
      home: new Home(),
    );
  }
}
