import 'package:admin/models/models.dart';
import 'package:admin/responsive.dart';
import 'package:flutter/material.dart';
import '../../constants.dart';
import 'components/header.dart';

class DashboardScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: SingleChildScrollView(
        padding: EdgeInsets.all(defaultPadding),
        child: Column(
          children: [
            Header(),
            SizedBox(height: defaultPadding),
            Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Expanded(
                  flex: 1,
                  child: Column(
                    children: [
                      SizedBox(height: defaultPadding),

                      ///Método GET seria aqui
                      //   Restaurante(horas: []),
                      if (Responsive.isMobile(context))
                        SizedBox(height: defaultPadding),
                      if (Responsive.isMobile(context))
                        Text(
                          'Giraffas Isistema API é o conjunto de APIs para inclusão no Sistema Delivery©'
                          ' de cadastros de usuários, com endereços validos de entrega, e seus pedidos delivery.'
                          ' Esse conjunto de APIs partiu do conjunto genérico "Giraffas Delivery ECommerce API na v1.0" para então ser customizado com objetivo a integração com Sistema Call Center',
                          style: TextStyle(color: Colors.white, fontSize: 20.0),
                        ),
                      //  )
                    ],
                  ),
                ),
                if (!Responsive.isMobile(context))
                  SizedBox(width: defaultPadding),
                if (!Responsive.isMobile(context))
                  Expanded(
                      flex: 10,
                      child: Text(
                          'Giraffas Isistema API é o conjunto de APIs para inclusão no Sistema Delivery©'
                          '\nde cadastros de usuários, com endereços validos de entrega, e seus pedidos delivery.'
                          '\nEsse conjunto de APIs partiu do conjunto genérico "Giraffas Delivery \nECommerce API na v1.0" para então ser customizado com objetivo a integração com Sistema Call Center',
                          style:
                              TextStyle(color: Colors.white, fontSize: 20.0))),
              ],
            )
          ],
        ),
      ),
    );
  }
}
