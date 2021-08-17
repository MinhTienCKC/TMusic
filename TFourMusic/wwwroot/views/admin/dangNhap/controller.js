﻿var ctxfolderurl = "/views/admin/dangNhap";

var app = angular.module('T_Music', ["ngRoute"]);

app.factory('dataservice', function ($http) {
    return {
        token: function (data, callback) {
            $http.post('https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q', data).then(callback);
        },
        //  satthuc: function (data, callback) {
        //    $http.post('/Admin/DangNhap/SatThuc',data).then(callback);
        //},
        login: function (token, callback) {
            $http({
                method: 'post',
                url: '/Admin/DangNhap/Login',
                url: '/Admin/TheLoai/LoadTheLoai',
                headers: {
                    Authorization: "Bearer " + token.idToken
                }
               
            }).then(callback);
          
        }
        //login1: function (token,callback) {
        //    $http({
        //        method: 'post',
        //        url: 'https://securetoken.googleapis.com/v1/tokenk?ey=AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q',
              
        //        headers: {
        //            'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
        //            'Accept': 'application/json'
                  
        //        },
        //        data: {
        //            "'grant_type=refresh_token&refresh_token=" + token + "'"
        //        }

        //    }).then(callback);

        //}

        //uploadImages: function (data, callback) {
        //    $http({
        //        method: 'post',
        //        url: '/Admin/BaiHat/GetLink',
        //        headers: {
        //            'Content-Type': undefined
        //        },
        //        data: data,
        //        uploadEventHandlers: {
        //            progress: function (e) {
        //                if (e.lengthComputable) {
        //                    fileProgress.setAttribute("value", e.loaded);
        //                    fileProgress.setAttribute("max", e.total);
        //                }
        //            }
        //        }
        //    }).then(callback);
        //}
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

    $scope.model = {
        email: '',
        password: '',
        returnSecureToken: "true"
       
    }
    $scope.submit = function () {

        //alert($scope.password);
        $scope.model.email = $scope.email;
        $scope.model.password = $scope.password;
        dataservice.token($scope.model, function (rs) {

            rs = rs.data;

            $scope.satthuc = rs;
            //dataservice.satthuc($scope.satthuc, function (rs) {
            //    rs = rs.data;            
            //});
           
            dataservice.login($scope.satthuc, function (rs) {
                rs = rs.data;
                
                window.location.href = '/admin/TheLoai';
            });
           
        });
    }

});