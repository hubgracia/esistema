import 'package:admin/Requests/GetHoras.dart';
import 'package:admin/Requests/GetRest.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';

//import 'package:admin/'
import '../../../main.dart';
import '../main_screen.dart';

class SideMenu extends StatelessWidget {
  const SideMenu({
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: ListView(
        children: [
          DrawerHeader(
            child: Image.asset("assets/images/logo.png"),
          ),
          DrawerListTile(
            title: "Página Principal",
            svgSrc: "assets/icons/menu_dashbord.svg",
            press: () {
              Navigator.push(context,
                  new MaterialPageRoute(builder: (ctxt) => new MyApp()));
            },
          ),
          DrawerListTile(
            title: "Lista todos Restaurantes",
            svgSrc: "",
            press: () {},
          ),
          DrawerListTile(
            title: "Informações do Restaurante",
            svgSrc: "",
            press: () {
              Navigator.push(context,
                  new MaterialPageRoute(builder: (ctxt) => new IdSelectRest()));
            },
          ),
          DrawerListTile(
            title: "Editar horas do Restaurante",
            svgSrc: "",
            press: () {
              Navigator.push(context,
                  new MaterialPageRoute(builder: (ctxt) => new IdSelectHora()));
            },
          ),
        ],
      ),
    );
  }
}

class DrawerListTile extends StatelessWidget {
  const DrawerListTile({
    Key? key,
    // For selecting those three line once press "Command+D"
    required this.title,
    required this.svgSrc,
    required this.press,
  }) : super(key: key);

  final String title, svgSrc;
  final VoidCallback press;

  @override
  Widget build(BuildContext context) {
    return ListTile(
      onTap: press,
      horizontalTitleGap: 0.0,
      leading: SvgPicture.asset(
        svgSrc,
        color: Colors.white54,
        height: 16,
      ),
      title: Text(
        title,
        style: TextStyle(color: Colors.white54),
      ),
    );
  }
}
