﻿var ctxfolderurl = "/views/admin/hoaDonNguoiDung";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {     
        
        taiThongKe: function (data,callback) {
            $http.post('/Admin/HoaDonNguoiDung/taiThongKe',data).then(callback); 
        },
        taiThongTinThanhToanThanhCong: function (data, callback) {
            $http.post('/Admin/HoaDonNguoiDung/taiThongTinThanhToanThanhCong', data).then(callback);
        },
       
        
        
    }

});
app.factory('Excel', function ($window) {
    var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
        format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
    return {
        tableToExcel: function (tableId, worksheetName) {
            var table = $(tableId),
                ctx = { worksheet: worksheetName, table: table.html() },
                href = uri + base64(format(template, ctx));
            return href;
        }
    };
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

}]);
app.filter('startFrom', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = +start; //parse to int
        return input.slice(start);
    }
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
app.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
});
app.controller('T_Music', function () {

});
app.controller('index', function ($rootScope, $scope, dataservice, $uibModal, Excel, $timeout) {
    $(".nav-nguoidung").addClass("active");
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }
    $scope.date = new Date();
    $scope.model = {
        email: '',
        mahoadon: '',
        hoten:''
      
    }
    $scope.model1 = {
        ngaybatdau: $scope.date,
        ngayketthuc: $scope.date,
        theonam: $scope.date,
        theothang: $scope.date,
        hienTimKiem: ''
    }
  //  $scope.hienTimKiem = "theongay";
    $scope.chuyenDoi = function (data) {
        $scope.model.hienTimKiem = data;
    }
    $scope.layDanhSach = function () {
        
        dataservice.taiThongKe($scope.model,function (rs) {
            rs = rs.data;
            $scope.taiThongKe = rs;
            $scope.currentPage = 0;
            $scope.pageSize = 5;
            $scope.size = 0;
            $scope.soLuong = 8;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiThongKe.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
            $scope.tongTien = 0;
            for (var i = 0; i < $scope.taiThongKe.length; i++) {
                $scope.tongTien += Math.ceil($scope.taiThongKe[i].hdtt.giatien);
            }
        });
    }
    //$scope.filterNumber = function (val) {
    //    var ret = (val) ? val.toString().replace(",", ".") : null;
    //    var ret2 = (ret) ? ret.toString().replace(",", ".") : null;
    //    var ret3 = (ret) ? ret.toString().replace(",", ".") : null;
    //    var ret4 = (ret) ? ret.toString().replace(",", ".") : null;
    //    return ret4;
    //}

    $scope.xuatThongKe = function () {
       
        var exportHref = Excel.tableToExcel("#thongke_id", 'Thống kê doanh thu TMusic');
        $timeout(function () { location.href = exportHref; }, 100); // trigger download
    };
    $scope.initData = function () {
        
        dataservice.taiThongKe($scope.model1,function (rs) {

            rs = rs.data;
            $scope.taiThongKe = rs;
          
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiThongKe.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
            $scope.tongTien = 0;
            for (var i = 0; i < $scope.taiThongKe.length; i++) {
                $scope.tongTien += Math.ceil($scope.taiThongKe[i].hdtt.giatien);
            }
        });
        
    };
 
    $scope.initData();

    $scope.currentPage = 0;
    $scope.pageSize = 5;
    $scope.size = 0;
    $scope.soLuong = 8;
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };
    $scope.Truoc = function () {
        if ($scope.numberOfPages() < 8) {
            return;
        }
        else {
            if ($scope.size == 0) {
                return;
            } else {
                $scope.size -= 8;
                $scope.soLuong = 8
            }
            $scope.currentPage = $scope.size;
        }

    }
    $scope.Sau = function () {
        if ($scope.numberOfPages() < 8) {
            return;
        }
        else {

            if ($scope.numberOfPages() % 8 == 0) {
                if ($scope.size + $scope.soLuong >= $scope.numberOfPages())
                    $scope.size = 0;
                else {
                    $scope.size += $scope.soLuong;
                }
                $scope.currentPage = $scope.size;
            }
            else {
                $scope.bienTam = $scope.numberOfPages() % 8;
                $scope.bienTam2 = $scope.numberOfPages() - $scope.bienTam;
                if ($scope.size + $scope.soLuong == $scope.bienTam2) {

                    $scope.size += 8;
                    $scope.soLuong = $scope.bienTam;
                }
                else if ($scope.size + $scope.soLuong >= $scope.numberOfPages()) {
                    $scope.size = 0;
                    $scope.soLuong = 8
                }
                else {
                    $scope.size += 8;

                }
                $scope.currentPage = $scope.size;
            }
        }


    }
    function timTheoIdHD(lists,id) {
        let res = lists.filter(item => item.hdtt["id"] == id);
        return res
    }
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.ChiTietHoaDon = function (key) {
    /*   $scope.model.id = key;*/
        if (key != '') {
            $scope.dulieu = timTheoIdHD($scope.taiThongKe, key);
            if ($scope.dulieu.length >= 1) {
                if ($scope.dulieu[0].hdtt.id == key) {
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: ctxfolderurl + '/chitiethoadon.html',
                        controller: 'chitiethoadon',
                        backdrop: 'true',
                        backdropClass: ".fade:not(.show)",
                        backdropClass: ".modal-backdrop",
                        backdropClass: ".col-lg-8",
                        backdropClass: ".modal-content",

                        size: '100',
                        resolve: {
                            para: function () {
                                return $scope.dulieu[0];
                            }
                        }
                    });
                    modalInstance.result.then(function () {

                    }, function () {
                        /*                $scope.initData();*/
                    });
                }
                else {
                    alertify.success("Khôn tìm thấy mã hóa đơn !!!.");
                }
            }
            else {
                alertify.success("Khôn tìm thấy mã hóa đơn !!!.");
            }
        } else {
            alertify.success("Vui lòng nhập mã hóa đơn !!!.");
        }
      
   


    };

});
app.controller('chitiethoadon', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.data = para;
    $scope.initData = function () {
      
    };
    $scope.initData();
  
   

});
