
var ctxfolderurl = "/views/front-end/quenmatkhau";

var app = angular.module('App_ESEIM', [ 'ngRoute']);

app.config(function ($routeProvider, $locationProvider) {
    $locationProvider.hashPrefix('');
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
       
});

app.factory('dataservice', function ($http) {
    return {
    }
});



app.controller('Ctrl_ESEIM', function ($scope, dataservice) {
  
  

});

app.controller('index', function ($scope, $rootScope, dataservice, $timeout) {
    $scope.initData = function () {
        $scope.datLaiMatKhau = false;
        $scope.ramdomCapCha = function () {
            var alpha = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V'
                , 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '!', '@', '#', '$', '%', '^', '&', '*', '+'];
            var a = alpha[Math.floor(Math.random() * 71)];
            var b = alpha[Math.floor(Math.random() * 71)];
            var c = alpha[Math.floor(Math.random() * 71)];
            var d = alpha[Math.floor(Math.random() * 71)];
            var e = alpha[Math.floor(Math.random() * 71)];
            var f = alpha[Math.floor(Math.random() * 71)];

            var final = a + b + c + d + e + f;
            $scope.capCha = final;
        }
        $scope.ramdomCapCha();
    }
    $scope.initData();
  
   
    
    var config = {
        apiKey: "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q",
        authDomain: "tfourmusic-1e3ff.firebaseapp.com",
        databaseURL: "tfourmusic-1e3ff-default-rtdb.firebaseio.com",
        projectId: "tfourmusic-1e3ff",
        storageBucket: "tfourmusic-1e3ff.appspot.com"


    };
    firebase.initializeApp(config);
    var auth = firebase.auth();
    $scope.quenMatKhau = function (email) {
        $scope.ramdomCapCha();
        $("#loading_main").css("display", "block");
        setTimeout(function () {
            $("#loading_main").css("display", "none");
        }, 2000);
        auth.sendPasswordResetEmail(email).then(function () {
            $timeout(function () {
                $scope.datLaiMatKhau = true;
            }, 1000);
            }).catch(function (error) {
                $scope.ramdomCapCha();

            });

       
    }
    $scope.back = function () {
        window.location.assign("/");
    }
});