var ctxfolderurl = "/views/admin/quanLyNguoiDung";

var app = angular.module('T_Music', ["ngRoute"]);

app.factory('dataservice', function ($http) {
    return {
        kiemTraPhanQuyen: function (callback) {
            $http.post('/Admin/QuanLyNguoiDung/kiemTraPhanQuyen').then(callback);
        },
    
    }

});
app.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
})
app.controller('T_Music', function () {

});

app.controller('index', function ($rootScope, $scope, dataservice) {

    $(".nav-nguoidung").addClass("active");
    $scope.initData = function () {

        dataservice.kiemTraPhanQuyen(function (rs) {

            rs = rs.data;
            if (rs == 2) {
                $("#idtaikhoanquantri").css({ "pointer-events": "none", "cursor": "default", "color": "#bdbcbc", "opacity": "20%"});
            }
                     
        });
    };

    $scope.initData();
   
    //$("#span-activity").css({ "color": "#bdbcbc" });
});