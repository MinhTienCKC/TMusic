var ctxfolderurl = "/views/admin/tongQuat";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        layBangBaiHat: function (callback) {
            $http.post('/Admin/TongQuat/LayBangBaiHat').then(callback);
        },
        layBangDanhSachPhatNguoiDung: function (callback) {
            $http.post('/Admin/TongQuat/LayBangDanhSachPhatNguoiDung').then(callback);
        },
        layBangDanhSachPhatTheLoai: function (callback) {
            $http.post('/Admin/TongQuat/LayBangDanhSachPhatTheLoai').then(callback);
        },
        layBangNguoiDung: function (callback) {
            $http.post('/Admin/TongQuat/LayBangNguoiDung').then(callback);
        },
    }

});
app.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        }
        )
    }
    return {
        link: fn_link
    }

}])
app.filter('startFrom', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = +start; //parse to int
        return input.slice(start);
    }
});
app.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
});
app.controller('T_Music', function () {

});
app.controller('index', function ($rootScope, $scope, dataservice, $uibModal) {
    $(".nav-tongquat").addClass("active");
    
    function adminBaiHat(lists) {
        let res = lists.filter(item => item["nguoidung_id"] == "admin");
        return res
    }
    function nguoiDungBaiHat(lists) {
        let res = lists.filter(item => item["nguoidung_id"] != "admin");
        return res
    }
    function truyVan(lists,key,values) {
        let res = lists.filter(item => item[key] == values);
        return res
    }
    $scope.initData = function () {

        dataservice.layBangBaiHat(function (rs) {
            $scope.soLuongBaiHat = [];
            $scope.ngayTrongThang = [];
            for (var i = 1; i < 31; i++) {
                $scope.ngayTrongThang.push(i);
                $scope.soLuongBaiHat.push(Math.floor(Math.random() * (100 - 1)) + 1);
            }
            rs = rs.data;
            $scope.layBangBaiHat = rs;
            $scope.soBaiHatAdmin = adminBaiHat($scope.layBangBaiHat);
            $scope.soBaiHatNguoiDung = nguoiDungBaiHat($scope.layBangBaiHat);
            new Chart(document.getElementById("line-chart"), {
                type: 'line',
                data: {
                    labels: Array.from(new Array(30), function (_, i) {
                        return i === 0 ? 1 : i;
                    }),
                    datasets: [{
                        label: "Bài Hát",
                        fill: 'start',
                        data: $scope.soLuongBaiHat,
                        backgroundColor: 'rgba(0,123,255,0.1)',
                        borderColor: 'rgba(0,123,255,1)',
                        pointBackgroundColor: '#ffffff',
                        pointHoverBackgroundColor: 'rgb(0,123,255)',
                        borderWidth: 1.5,
                        pointRadius: 0,
                        pointHoverRadius: 3
                    }]
                },
                options: {
                        responsive: true,
                        legend: {
                            position: 'top'
                        },
                        plugins: {
                                legend: {
                                    maxHeight: 300,
                                    maxWidth: 300
                                }
                           
                        },
                        elements: {
                            line: {
                                // A higher value makes the line look skewed at this ratio.
                                tension: 0.3
                            },
                            point: {
                                radius: 0
                            }
                        },
                        scales: {
                            xAxes: [{
                                gridLines: false,
                                ticks: {
                                    callback: function (tick, index) {
                                        // Jump every 7 values on the X axis labels to avoid clutter.
                                        return index % 7 !== 0 ? '' : tick;
                                    }
                                }
                            }],
                            yAxes: [{
                                ticks: {
                                    suggestedMax: 45,
                                    callback: function (tick, index, ticks) {
                                        if (tick === 0) {
                                            return tick;
                                        }
                                        // Format the amounts using Ks for thousands.
                                        return tick > 999 ? (tick / 1000).toFixed(1) + 'K' : tick;
                                    }
                                }
                            }]
                        },
                        // Uncomment the next lines in order to disable the animations.
                        // animation: {
                        //   duration: 0
                        // },
                        hover: {
                            mode: 'nearest',
                            intersect: false
                        },
                        tooltips: {
                            custom: false,
                            mode: 'nearest',
                            intersect: false
                        }
                        
                }
            });
        });
        dataservice.layBangDanhSachPhatNguoiDung(function (rs) {

            rs = rs.data;
            $scope.layBangDanhSachPhatNguoiDung = rs;
        });
        dataservice.layBangDanhSachPhatTheLoai(function (rs) {

            rs = rs.data;
            $scope.layBangDanhSachPhatTheLoai = rs;
        });
        dataservice.layBangNguoiDung(function (rs) {

            rs = rs.data;
            $scope.layBangNguoiDung = rs;
            new Chart(document.getElementById("pie-chart"), {
                type: 'pie',
                data: {
                    labels: ["Có Vip", "Không Vip"],
                    datasets: [{
                        label: "Admin",
                        backgroundColor: ["#3e95cd", "#8e5ea2"],

                        data: [truyVan($scope.layBangNguoiDung, "vip", 1).length, truyVan($scope.layBangNguoiDung, "vip", 0).length]
                    }]
                },
                options: {
                    title: {
                        display: true,
                        text: 'Người Dùng',
                        color: '#fff'
                    }
                }
            });
        });
       
    };

    $scope.initData();
   
});

