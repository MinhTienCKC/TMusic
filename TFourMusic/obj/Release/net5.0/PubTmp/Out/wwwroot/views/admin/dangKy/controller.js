var ctxfolderurl = "/views/admin/dangKy";

var app = angular.module('T_Music', ["ngRoute"]);

app.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
})
app.controller('T_Music', function () {

});
app.controller('index', function ($scope) {
    $scope.ten = "ttai";


});