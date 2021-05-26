
var App = angular
    .module("App", []);
    app.controller("Controller", function ($scope, model) {
        $scope.mensagem = "Teste"
        $scope.model = "@Html.Raw(Json.Encode(Model));"
        
        $scope.semana = [1, 2, 3, 4, 5, 6, 7];
        $scope.dia = "@Model.dia"
        $scope.selected = [];

        dia[0] = 1; 
        $('#Dia1').html(model.get('dia[0]'));

        angular.forEach(lista, function (dia) {
            if (dia.checked) {
                $scope.model[dia] = item.checked;
            }
        });
     
    });