var ctxfolderurl = "/views/admin/tongQuat";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        taiBangBaiHat: function (callback) {
            $http.post('/Admin/TongQuat/LayBangBaiHat').then(callback);
        },
        taiBangDanhSachPhatNguoiDung: function (callback) {
            $http.post('/Admin/TongQuat/LayBangDanhSachPhatNguoiDung').then(callback);
        },
        taiBangDanhSachPhatTheLoai: function (callback) {
            $http.post('/Admin/TongQuat/LayBangDanhSachPhatTheLoai').then(callback);
        },
        taiBangNguoiDung: function (callback) {
            $http.post('/Admin/TongQuat/LayBangNguoiDung').then(callback);
        },
        taiBangQuangCao: function (callback) {
            $http.post('/Admin/TongQuat/LayBangQuangCao').then(callback);
        },
        taiBangChuDe: function (callback) {
            $http.post('/Admin/TongQuat/LayBangChuDe').then(callback);
        },
        taiBangTheLoai: function (callback) {
            $http.post('/Admin/TongQuat/LayBangTheLoai').then(callback);
        },
        taiBangGoiVip: function (callback) {
            $http.post('/Admin/TongQuat/LayBangGoiVip').then(callback);
        },
        taiDanhSachSoBHTheoNgay: function (data,callback) {
            $http.post('/Admin/TongQuat/taiDanhSachSoBHTheoNgay' , data).then(callback);
        },
        taiThongKe: function (data, callback) {
            $http.post('/Admin/TongQuat/taiThongKe', data).then(callback);
        },
        taiTheLoai: function (callback) {
            $http.post('/Admin/BaiHat/taiTheloai').then(callback);

        },
        taiTaiKhoanQuanTri: function (callback) {
            $http.post('/Admin/TongQuat/taiTaiKhoanQuanTri').then(callback);
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
app.filter('vietNamDong', function () {
    return function (val) {
        var ret = (val) ? val.toString().replace(",", ".") : null;
        var ret2 = (ret) ? ret.toString().replace(",", ".") : null;
        var ret3 = (ret) ? ret.toString().replace(",", ".") : null;
        var ret4 = (ret) ? ret.toString().replace(",", ".") : null;
        return ret4 + " VNĐ";
    }
});
app.controller('index', function ($rootScope, $scope, dataservice, $uibModal) {
    $scope.soNgauNhien = function () {
        $scope.so = [];
       
        for (var i = 0; i < 7; i++) {
            $scope.so.push(Math.floor(Math.random() * (10 - 1)) + 1);
          
        }      
        return $scope.so;
    }           
    //function random_rgba() {
    //    var o = Math.round, r = Math.random, s = 255;
    //    return  o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s);
    //}
    function getRandomRgb() {
        var num = Math.round(0xffffff * Math.random());
        var r = num >> 16;
        var g = num >> 8 & 255;
        var b = num & 255;
    /*return 'rgb(' + r + ', ' + g + ', ' + b + ')';*/
        return  r + ', ' + g + ', ' + b ;
    }
    $scope.mauNgauNhien = function () {
        $scope.mau = [];
        for (var i = 0; i < 10; i++) {
            $scope.mau.push(getRandomRgb());
        }
        return $scope.mau;
    }
    $scope.bangNgauNhien = function () {
        $scope.ngaunhien = [];
       
        for (var i = 0; i < 10; i++) {
            $scope.bang = {
                backgroundColor: 'rgba(' + $scope.bangMau[i] + ', 0.4)',
                borderColor: 'rgba(' + $scope.bangMau[i] + ',10)',
                //backgroundColor:  $scope.bangMau[i],
                //borderColor:  $scope.bangMau[i] ,
                data: $scope.soNgauNhien()
            }
            $scope.ngaunhien.push($scope.bang);

        }
        return $scope.ngaunhien;
    }
    $scope.bangMau = $scope.mauNgauNhien();
    var boSmallStatsDatasets = $scope.bangNgauNhien();

    // Options
    function boSmallStatsOptions(max) {
        return {
            maintainAspectRatio: true,
            responsive: true,
            // Uncomment the following line in order to disable the animations.
            // animation: false,
            legend: {
                display: false
            },
            tooltips: {
                enabled: false,
                custom: false
            },
            elements: {
                point: {
                    radius: 0
                },
                line: {
                    tension: 0.3
                }
            },
            scales: {
                xAxes: [{
                    gridLines: false,
                    scaleLabel: false,
                    ticks: {
                        display: false
                    }
                }],
                yAxes: [{
                    gridLines: false,
                    scaleLabel: false,
                    ticks: {
                        display: false,
                        // Avoid getting the graph line cut of at the top of the canvas.
                        // Chart.js bug link: https://github.com/chartjs/Chart.js/issues/4790
                        suggestedMax: max
                    }
                }],
            },
        };
    }

    // Generate the small charts
    boSmallStatsDatasets.map(function (el, index) {
        var chartOptions = boSmallStatsOptions(Math.max.apply(Math, el.data) + 1);
        var ctx = document.getElementsByClassName('blog-overview-stats-small-' + (index + 1));
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: ["Label 1", "Label 2", "Label 3", "Label 4", "Label 5", "Label 6", "Label 7"],
                datasets: [{
                    label: 'Today',
                    fill: 'start',
                    data: el.data,
                    backgroundColor: el.backgroundColor,
                    borderColor: el.borderColor,
                    borderWidth: 1.5,
                }]
            },
            options: chartOptions
        });
    });

    $scope.soLuongBaiHat = [];
    $scope.soLuongBaiHatAdmin = [];
    $scope.ngayTrongThang = [];   
    $scope.bangCharjs = {
        type: 'line',
        data: {
            labels: $scope.ngayTrongThang,
            datasets: [{
                label: "Người Dùng",
                fill: 'start',
                data: $scope.soLuongBaiHat,
                backgroundColor: 'rgba(0,123,255,0.1)',
                borderColor: 'rgba(0,123,255,1)',
                pointBackgroundColor: '#ffffff',
                pointHoverBackgroundColor: 'rgb(0,123,255)',
                borderWidth: 1.5,
                pointRadius: 0,
                pointHoverRadius: 3
            },
                {
                label: 'Admin',
                fill: 'start',
                data: $scope.soLuongBaiHatAdmin,
                backgroundColor: 'rgba(255,65,105,0.1)',
                borderColor: 'rgba(255,65,105,1)',
                pointBackgroundColor: '#ffffff',
                pointHoverBackgroundColor: 'rgba(255,65,105,1)',
                borderDash: [3, 3],
                borderWidth: 1,
                pointRadius: 0,
                pointHoverRadius: 2,
                pointBorderColor: 'rgba(255,65,105,1)'
            }]
            //datasets: [{
            //    label: 'Current Month',
            //    fill: 'start',
            //    data: [500, 800, 320, 180, 240, 320, 230, 650, 590, 1200, 750, 940, 1420, 1200, 960, 1450, 1820, 2800, 2102, 1920, 3920, 3202, 3140, 2800, 3200, 3200, 3400, 2910, 3100, 4250],
            //    backgroundColor: 'rgba(0,123,255,0.1)',
            //    borderColor: 'rgba(0,123,255,1)',
            //    pointBackgroundColor: '#ffffff',
            //    pointHoverBackgroundColor: 'rgb(0,123,255)',
            //    borderWidth: 1.5,
            //    pointRadius: 0,
            //    pointHoverRadius: 3
            //}, {
            //    label: 'Past Month',
            //    fill: 'start',
            //    data: [380, 430, 120, 230, 410, 740, 472, 219, 391, 229, 400, 203, 301, 380, 291, 620, 700, 300, 630, 402, 320, 380, 289, 410, 300, 530, 630, 720, 780, 1200],
            //    backgroundColor: 'rgba(255,65,105,0.1)',
            //    borderColor: 'rgba(255,65,105,1)',
            //    pointBackgroundColor: '#ffffff',
            //    pointHoverBackgroundColor: 'rgba(255,65,105,1)',
            //    borderDash: [3, 3],
            //    borderWidth: 1,
            //    pointRadius: 0,
            //    pointHoverRadius: 2,
            //    pointBorderColor: 'rgba(255,65,105,1)'
            //}]
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
    };
    var myLineChart = new Chart(document.getElementById("line-chart"), $scope.bangCharjs);   
    $(".nav-tongquat").addClass("active");
    $scope.date = new Date();
    $scope.model = {
        ngaybatdau: $scope.date,
        ngayketthuc: $scope.date,
        theonam: $scope.date,
        theothang: $scope.date,
        hienTimKiem: 'theongay'
    }
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
    $scope.layDanhSach = function () {     
        
        dataservice.taiDanhSachSoBHTheoNgay($scope.model, function (rs) {
            rs = rs.data;
            $scope.soBaiHatTheoNgay = rs;
            $scope.soLuongBaiHat = [];
            $scope.ngayTrongThang = [];
            $scope.soLuongBaiHatAdmin = [];
            for (var i = 0; i < $scope.soBaiHatTheoNgay.length; i++) {
                $scope.ngayTrongThang.push($scope.soBaiHatTheoNgay[i].ngaythu);
                $scope.soLuongBaiHat.push($scope.soBaiHatTheoNgay[i].sobaihat);
                $scope.soLuongBaiHatAdmin.push($scope.soBaiHatTheoNgay[i].sobaihatadmin)
            }       
            myLineChart.data.datasets[0].data = $scope.soLuongBaiHat;
            myLineChart.data.datasets[1].data = $scope.soLuongBaiHatAdmin;
            myLineChart.data.labels = $scope.ngayTrongThang; // Would update the first dataset's value of 'March' to be 50
            myLineChart.update();
        });
        
        var downloadbaihat = document.getElementById("btntext");
        downloadbaihat.click();
       

    }
    $scope.hienThiTaiXuong = function () {
        var downloadbaihat = document.getElementById("btntext");
        downloadbaihat.click();
    }
    $scope.initData = function () {

        dataservice.taiTheLoai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;


           

        });
        dataservice.taiTaiKhoanQuanTri(function (rs) {

            rs = rs.data;
            $scope.taiTaiKhoanQuanTri = rs;

           
        });

        dataservice.taiBangBaiHat(function (rs) {
           
            rs = rs.data;
            $scope.taiBangBaiHat = rs;
            $scope.soBaiHatAdmin = adminBaiHat($scope.taiBangBaiHat);
            $scope.soBaiHatNguoiDung = nguoiDungBaiHat($scope.taiBangBaiHat);
            
        });
        dataservice.taiBangDanhSachPhatNguoiDung(function (rs) {

            rs = rs.data;
            $scope.taiBangDanhSachPhatNguoiDung = rs;
        });
        dataservice.taiBangDanhSachPhatTheLoai(function (rs) {

            rs = rs.data;
            $scope.taiBangDanhSachPhatTheLoai = rs;
        });
        dataservice.taiBangTheLoai(function (rs) {

            rs = rs.data;
            $scope.taiBangTheLoai = rs;
        });
        dataservice.taiBangChuDe(function (rs) {

            rs = rs.data;
            $scope.taiBangChuDe = rs;
        });
        dataservice.taiBangQuangCao(function (rs) {

            rs = rs.data;
            $scope.taiBangQuangCao = rs;
        });
        dataservice.taiBangGoiVip(function (rs) {

            rs = rs.data;
            $scope.taiBangGoiVip = rs;
        });
        dataservice.taiBangNguoiDung(function (rs) {

            rs = rs.data;
            $scope.taiBangNguoiDung = rs;
            new Chart(document.getElementById("pie-chart"), {
                type: 'pie',
                data: {
                    labels: ["Có Vip", "Không Vip"],
                    datasets: [{
                        label: "Admin",
                        backgroundColor: ["#3e95cd", "#8e5ea2"],

                        data: [truyVan($scope.taiBangNguoiDung, "vip", 1).length, truyVan($scope.taiBangNguoiDung, "vip", 0).length]
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
        dataservice.taiDanhSachSoBHTheoNgay($scope.model, function (rs) {
            rs = rs.data;
            $scope.soBaiHatTheoNgay = rs;
            $scope.soLuongBaiHat = [];
            $scope.ngayTrongThang = [];
            $scope.soLuongBaiHatAdmin = [];
            for (var i = 0; i < $scope.soBaiHatTheoNgay.length; i++) {
                $scope.ngayTrongThang.push($scope.soBaiHatTheoNgay[i].ngaythu);
                $scope.soLuongBaiHat.push($scope.soBaiHatTheoNgay[i].sobaihat);
                $scope.soLuongBaiHatAdmin.push($scope.soBaiHatTheoNgay[i].sobaihatadmin)
            }
            myLineChart.data.datasets[0].data = $scope.soLuongBaiHat;
            myLineChart.data.datasets[1].data = $scope.soLuongBaiHatAdmin;
            myLineChart.data.labels = $scope.ngayTrongThang; // Would update the first dataset's value of 'March' to be 50
            myLineChart.update();
        });

        dataservice.taiThongKe($scope.model, function (rs) {
            rs = rs.data;
            $scope.taiThongKe = rs;    
            $scope.tongTienNgay = 0;
            $scope.tongTienThang = 0;
            $scope.tongTienNam = 0;
            for (var i = 0; i < $scope.taiThongKe.ngay.length; i++) {
                $scope.tongTienNgay += $scope.taiThongKe.ngay[i].giatien;
            }
            for (var i = 0; i < $scope.taiThongKe.thang.length; i++) {
                $scope.tongTienThang += $scope.taiThongKe.thang[i].giatien;
            }
            for (var i = 0; i < $scope.taiThongKe.nam.length; i++) {
                $scope.tongTienNam += $scope.taiThongKe.nam[i].giatien;
            }
        });
    };

    $scope.initData();
   
});

