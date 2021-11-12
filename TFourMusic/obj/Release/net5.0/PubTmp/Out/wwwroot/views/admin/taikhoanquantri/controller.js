

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
        xoaTaiKhoanQuanTri: function (data, callback) {
            $http.post('/Admin/TaiKhoanQuanTri/xoaTaiKhoanQuanTri', data).then(callback);
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
    $(".nav-nguoidung").addClass("active");
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
    function danhsachadmin(lists) {
        let res = lists.filter(item => item["phanquyen"] == 1);
        return res
    }
    $scope.voHieuHoa = function (data) {
        $scope.dsa = danhsachadmin($scope.taiTaiKhoanQuanTri);
        if (data.phanquyen == 1) {
            if ($scope.dsa.length <= 1) {
                alertify.success("Không thể vô hiệu hóa !!!");
                if (data.vohieuhoa == 1) {
                    data.vohieuhoa = 0;
                }
                else {
                    data.vohieuhoa = 1;
                }
                return;
            }
        }
        dataservice.voHieuHoa(data, function (rs) {
            rs = rs.data;
            if (rs == "") {
                alertify.success("Tài khoản Admin mới thực hiện chức năng này !!!");
                if (data.vohieuhoa == 1) {
                    data.vohieuhoa = 0;
                }
                else {
                    data.vohieuhoa = 1;
                }
                return;
            }
            if (rs == "loi") {
                alertify.success("Không thể xóa hay vô hiệu hóa chính mình !!!.");
                if (data.vohieuhoa == 1) {
                    data.vohieuhoa = 0;
                }
                else {
                    data.vohieuhoa = 1;
                }
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
                $scope.initData();
              
        });
    };
    $scope.xoaTaiKhoanQuanTri = function (data,vitri) {
        $scope.data = data;
        $scope.dsa = danhsachadmin($scope.taiTaiKhoanQuanTri);
        if (data.phanquyen == 1) {
            if ($scope.dsa.length <= 1) {
                alertify.success("Không thể xóa !!!");

                return;
            }
        }
    
        dataservice.xoaTaiKhoanQuanTri($scope.data,function (rs) {
            rs = rs.data;
            if (rs == "loi") {
                alertify.success("Không thể xóa hay vô hiệu hóa chính mình !!!.");
                return;
            }
            if (rs == true) {
                alertify.success("Xóa thành công !!!.");
                $scope.taiTaiKhoanQuanTri.splice(vitri, 1);
            } else {
                alertify.success("Xóa thất bại  !!!.");
            }
        });

    }
    

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
        dataservice.taiTaiKhoanQuanTri(function (rs) {

            rs = rs.data;
            $scope.taiTaiKhoanQuanTri = rs;


        });
             
    };
    $scope.kiemTraTrung = false;
    $scope.kiemtra = function () {
        
        for (var i = 0; i < $scope.taiTaiKhoanQuanTri.length; i++) {
            if ($scope.addTaiKhoan == $scope.taiTaiKhoanQuanTri[i].taikhoan) {
                $scope.kiemTraTrung = true;
                break;
            } else {
                $scope.kiemTraTrung = false;
            }
         }      
    }
    $scope.changePhanQuyen = function () {
        if ($scope.valuePhanQuyen == 'Admin') {
            $scope.model.phanquyen = 1;
        } else {
            $scope.model.phanquyen = 0;
        }

    };
   
    $scope.initData();
    $scope.submit = function () {
        $("#loading_main").css("display", "block");
        if ($scope.kiemTraTrung == true) {
            $("#loading_main").css("display", "none");
            alert("Tài Khoản Đã Tồn Tại Vui Lòng Nhập Lại !!!");
            return;
        }
        if (
             !$scope.addTaiKhoanQuanTri.addTaiKhoan.$valid
            || !$scope.addTaiKhoanQuanTri.addMatKhau.$valid
        ) {
            $("#loading_main").css("display", "none");
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {

            $scope.model.taikhoan = $scope.addTaiKhoan;
            $scope.model.matkhau = $scope.addMatKhau;
           
            $scope.model1 = $scope.model;
            dataservice.taoTaiKhoanQuanTri($scope.model, function (rs) {
                rs = rs.data;
                $scope.data = rs;

                $("#loading_main").css("display", "none");
                $uibModalInstance.dismiss('cancel');
            });


 
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
        dataservice.taiTaiKhoanQuanTri(function (rs) {

            rs = rs.data;
            $scope.taiTaiKhoanQuanTri = rs;


        });
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
        $("#loading_main").css("display", "block");
        for (var i = 0; i < $scope.taiTaiKhoanQuanTri.length; i++) {
            if ($scope.editTaiKhoan == $scope.taiTaiKhoanQuanTri[i].taikhoan && $scope.editMatKhau == $scope.taiTaiKhoanQuanTri[i].matkhau
                && $scope.modeltaikhoanquantri.phanquyen == $scope.taiTaiKhoanQuanTri[i].phanquyen
            ) {
                $("#loading_main").css("display", "none");
                $uibModalInstance.dismiss('cancel');
                return;   
            } 
        }      
       
        if (!$scope.editTaiKhoanQuanTri.editTaiKhoan.$valid
            || !$scope.editTaiKhoanQuanTri.editMatKhau.$valid
        ) {
            $("#loading_main").css("display", "none");
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
            $scope.modeltaikhoanquantri.taikhoan = $scope.editTaiKhoan;
            $scope.modeltaikhoanquantri.matkhau = $scope.editMatKhau;

          
            dataservice.suaTaiKhoanQuanTri($scope.modeltaikhoanquantri, function (rs) {
                rs = rs.data;
                $scope.suaTaiKhoanQuanTri = rs;

                $("#loading_main").css("display", "none");
                $uibModalInstance.dismiss('cancel');
            });



      
        }
    }
});