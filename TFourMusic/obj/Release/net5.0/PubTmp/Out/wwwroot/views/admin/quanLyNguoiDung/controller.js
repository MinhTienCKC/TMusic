var ctxfolderurl = "/views/admin/quanLyNguoiDung";

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

app.controller('index', function ($rootScope, $scope) {

    $(".nav-nguoidung").addClass("active");


});