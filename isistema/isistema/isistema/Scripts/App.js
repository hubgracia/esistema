
var myApp = angular
    .module("myModule", [])
    .CONTROLLER("myController", function ($scope) {

        $scope.model = @Html.Raw(Json.Encode(Model));

        $scope.dia = [1, 2, 3, 4, 5, 6, 7];
        $scope.selected = [];


});