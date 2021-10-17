var ctxfolderurl = "/views/admin/taiKhoanQuanTri";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {      
        taiTaiKhoanQuanTri: function (callback) {
            $http.post('/Admin/TaiKhoanQuanTri/taiTaiKhoanQuanTri').then(callback);
        },   
        voHieuHoa: function (data,callback) {
            $http.post('/Admin/TaiKhoanQuanTri/voHieuHoa',data).then(callback);
        },  
        taoTaiKhoanQuanTri: function (data,callback) {
            $http.post('/Admin/TaiKhoanQuanTri/taoTaiKhoanQuanTri', data).then(callback);
        },  
        suaTaiKhoanQuanTri: function (data,callback) {
            $http.post('/Admin/TaiKhoanQuanTri/suaTaiKhoanQuanTri', data).then(callback);
        },  
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/NguoiDung/GetLinkHinhAnh',
                headers: {
                    'Content-Type': undefined
                },
                data: data,
                uploadEventHandlers: {
                    progress: function (e) {
                        if (e.lengthComputable) {
                            fileProgress.setAttribute("value", e.loaded);
                            fileProgress.setAttribute("max", e.total);
                        }
                    }
                }
            }).then(callback);
        }
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

}]);
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
    $scope.hienTimKiem = false;
    $scope.showSearch = function () {
        if (!$scope.hienTimKiem) {
            $scope.hienTimKiem = true;
        } else {
            $scope.hienTimKiem = false;
        }
    }
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }

    $scope.trangThai = function () {
        if ($scope.model.trangThai == 1) {
            $scope.model.trangThai = 0;
        } else {
            $scope.model.trangThai = 1;
        }
    }
    $scope.model = {
        trangThai: 1
    }
    $scope.text = {
        key: ''
    }
    $scope.rong = '';
    $scope.initData = function () {

        dataservice.taiTaiKhoanQuanTri(function (rs) {

            rs = rs.data;
            $scope.taiTaiKhoanQuanTri = rs;

            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiTaiKhoanQuanTri.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        });

    };

    $scope.initData();
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };

    $scope.currentPage = 0;
    $scope.pageSize = 5;
    $scope.size = 0;
    $scope.soLuong = 8;

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
            }
        }


    }
    $scope.taoTaiKhoanQuanTri_index = function () {

        var modalInstance = $uibModal.open({
            /*animation: true,*/

            //ariaLabelledBy: 'modal-title',
            //ariaDescribedBy: 'modal-body',
            templateUrl: ctxfolderurl + '/add.html',
            controller: 'add',
            //  backdrop: 'true',
            backdropClass: ".fade:not(.show)",
            backdropClass: ".modal-backdrop",
            backdropClass: ".col-lg-8",
            backdropClass: ".modal-content",
            /*  appendTo: document.getElementById("#main-baihat"),*/
            size: '10'
        });
        modalInstance.result.then(function () {

        }, function () {
            setTimeout(function () {
                $scope.initData();
            }, 2000);
        });

    };
    $scope.voHieuHoa = function (data) {

        dataservice.voHieuHoa(data, function (rs) {
            rs = rs.data;
            if (rs == "") {
                alertify.success("Tài khoản Admin mới thực hiện chức năng này !!!");
                return;
            }
            if (rs == true) {
                if (data.vohieuhoa == 1) {
                    alertify.success("Đã vô hiệu hóa !!!.");
                }
                else {
                    alertify.success("Đã bỏ vô hiệu hóa !!!");
                }
            }
            else {
                alertify.success("Lỗi không thực hiện vô hiệu hóa !!!.");
            }
        });
    }
    alertify.set('notifier', 'position', 'bottom-left');

    $scope.edit = function (key) {
        /*   $scope.model.id = key;*/
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/edit.html',
            controller: 'edit',
            backdrop: 'true',
            backdropClass: ".fade:not(.show)",
            backdropClass: ".modal-backdrop",
            backdropClass: ".col-lg-8",
            backdropClass: ".modal-content",

            size: '100',
            resolve: {
                para: function () {
                    return key;
                }
            }
        });
        modalInstance.result.then(function () {
          
        }, function () {
        });
    };

    

});
app.controller('add', function ($rootScope, $scope, dataservice, $uibModalInstance) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        id: '',
        taikhoan: '',
        matkhau: '',
        phanquyen:0
    }
    $scope.text = {
        key: ''
    } 
    $scope.initData = function () {
        $scope.valuePhanQuyen = 'Nhân Viên';
             
    };
    $scope.changePhanQuyen = function () {
        if ($scope.valuePhanQuyen == 'Admin') {
            $scope.model.phanquyen = 1;
        } else {
            $scope.model.phanquyen = 0;
        }

    };
    $scope.initData();
    $scope.submit = function () {
       
        if (
             !$scope.addTaiKhoanQuanTri.addTaiKhoan.$valid
            || !$scope.addTaiKhoanQuanTri.addMatKhau.$valid
            ) {
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {

            $scope.model.taikhoan = $scope.addTaiKhoan;
            $scope.model.matkhau = $scope.addMatKhau;
           
            $scope.model1 = $scope.model;
            dataservice.taoTaiKhoanQuanTri($scope.model, function (rs) {
                rs = rs.data;
                $scope.data = rs;
            });


            $uibModalInstance.dismiss('cancel');
        }

    }  
});
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.modeltaikhoanquantri = para;
    $scope.text = {
        key: ''
    };
    $scope.initData = function () {
        if ($scope.modeltaikhoanquantri.phanquyen == 0) {
            $scope.valuePhanQuyen = 'Nhân Viên';
        } else {
            $scope.valuePhanQuyen = 'Admin';
        }
        $scope.editTaiKhoan = $scope.modeltaikhoanquantri.taikhoan;
        $scope.editMatKhau = $scope.modeltaikhoanquantri.matkhau;
    };
    $scope.changePhanQuyen = function () {
        if ($scope.valuePhanQuyen == 'Admin') {
            $scope.modeltaikhoanquantri.phanquyen = 1;
        } else {
            $scope.modeltaikhoanquantri.phanquyen = 0;
        }

    };

    $scope.initData();

    $scope.submit = function () {
        if (!$scope.editTaiKhoanQuanTri.editTaiKhoan.$valid
            || !$scope.editTaiKhoanQuanTri.editMatKhau.$valid
        ) {
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
            $scope.modeltaikhoanquantri.taikhoan = $scope.editTaiKhoan;
            $scope.modeltaikhoanquantri.matkhau = $scope.editMatKhau;

          
            dataservice.suaTaiKhoanQuanTri($scope.modeltaikhoanquantri, function (rs) {
                rs = rs.data;
                $scope.suaTaiKhoanQuanTri = rs;
            });



            $uibModalInstance.dismiss('cancel');
        }
    }
});