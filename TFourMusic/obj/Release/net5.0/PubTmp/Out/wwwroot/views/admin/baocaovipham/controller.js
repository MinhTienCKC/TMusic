var ctxfolderurl = "/views/admin/baocaovipham";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
       
        taiBaiHatViPhamChuaXuLy: function ( callback) {
            $http.post('/Admin/BaoCaoViPham/taiBaiHatViPhamChuaXuLy').then(callback);
        },
        taiBaiHatViPhamDaXuLy: function (callback) {
            $http.post('/Admin/BaoCaoViPham/taiBaiHatViPhamDaXuLy').then(callback);
        },
        taiBaiHatBangQuyen: function (data,callback) {
            $http.post('/Admin/BaoCaoViPham/taiBaiHatBangQuyen',data).then(callback);
        },
        taiChiTietNguoiDungViPham: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/taiChiTietNguoiDungViPham', data).then(callback);
        },
        voHieuHoaNguoiDung: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/voHieuHoaNguoiDung', data).then(callback);
        },  
        voHieuHoaBaiHatNguoiDung: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/voHieuHoaBaiHatNguoiDung', data).then(callback);
        },  
        capNhatTrangThai: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/capNhatTrangThai', data).then(callback);
        },  
        capNhatTrangThaiNguoiDung: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/capNhatTrangThaiNguoiDung', data).then(callback);
        },  
        pheDuyetBaiHatViPham: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/pheDuyetBaiHatViPham', data).then(callback);
        },  
        taiBaiHatViPhamDaXuLy_ViPham: function (callback) {
            $http.post('/Admin/BaoCaoViPham/taiBaiHatViPhamDaXuLy_ViPham').then(callback);
        },
        taiBaiHatViPhamDaXuLy_KhongViPham: function (callback) {
            $http.post('/Admin/BaoCaoViPham/taiBaiHatViPhamDaXuLy_KhongViPham').then(callback);
        },
        khoiPhucBaiHatViPham: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/khoiPhucBaiHatViPham', data).then(callback);
        },  
        taiNguoiDungViPhamChuaXuLy: function (callback) {
            $http.post('/Admin/BaoCaoViPham/taiNguoiDungViPhamChuaXuLy').then(callback);
        },
        taiNguoiDungViPhamDaXuLy_ViPham: function (callback) {
            $http.post('/Admin/BaoCaoViPham/taiNguoiDungViPhamDaXuLy_ViPham').then(callback);
        },
        taiNguoiDungViPhamDaXuLy_KhongViPham: function (callback) {
            $http.post('/Admin/BaoCaoViPham/taiNguoiDungViPhamDaXuLy_KhongViPham').then(callback);
        },
        pheDuyetNguoiDungViPham: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/pheDuyetNguoiDungViPham', data).then(callback);
        },  
        khoiPhucNguoiDungViPham: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/khoiPhucNguoiDungViPham', data).then(callback);
        },  
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/ChuDe/GetLinkHinhAnh',
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
app.directive('validatekitudacbiet', function () {
    function link(scope, elem, attrs, ngModel) {
        ngModel.$parsers.push(function (viewValue) {
            var reg = /^[^`~!@#$%\^&*_+={}|[\]\\:';"<>?,./]*$/;
            // if view values matches regexp, update model value
            if (viewValue.match(reg)) {
                return viewValue;
            }
            // keep the model value as it is
            var transformedValue = ngModel.$modelValue;
            ngModel.$setViewValue(transformedValue);
            ngModel.$render();
            return transformedValue;
        });
    }


    return {
        restrict: 'A',
        require: 'ngModel',
        link: link
    };
});
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

    $scope.capNhatTrangThai = function (data, vitri) {
        if (data.trangthai == 2) {
            return;
        }
        $scope.modelTrangThai = {
            nguoidung_id: data.nguoidung_id,
            id: data.id
        }
       
        dataservice.capNhatTrangThai($scope.modelTrangThai, function (rs) {
            rs = rs.data;
          
            if (rs == true) {
                $scope.taiBaoCaoViPham[vitri].trangthai = 1;
            }
            else {
                $scope.taiBaoCaoViPham[vitri].trangthai = 0;
            }
        });
    }
    $scope.capNhatTrangThaiNguoiDung = function (data, vitri) {
        if (data.trangthai == 2) {
            return;
        }
        $scope.modelTrangThai = {
            nguoidung_id: data.nguoidung_id,
            id: data.id
        }

        dataservice.capNhatTrangThaiNguoiDung($scope.modelTrangThai, function (rs) {
            rs = rs.data;

            if (rs == true) {
                $scope.taiBaoCaoViPham[vitri].trangthai = 1;
            }
            else {
                $scope.taiBaoCaoViPham[vitri].trangthai = 0;
            }
        });
    }
    $scope.loading = true;
    $scope.model = {
        ngaybatdau: $scope.date,
        ngayketthuc: $scope.date,
        theonam: $scope.date,
        theothang: $scope.date,
        hienTimKiem: 'baihat'
    }
    $scope.daxuly = 'vipham';
    $scope.baiHatViPhamXuLy = 'chuaxuly';
    $scope.chuyenDoi = function (data) {
        $scope.model.hienTimKiem = data;
        $scope.loading = true;
        if ($scope.model.hienTimKiem == 'baihat') {
           
                $scope.baiHatViPhamXuLy = 'chuaxuly';
            dataservice.taiBaiHatViPhamChuaXuLy(function (rs) {

                rs = rs.data;
                $scope.taiBaoCaoViPham = rs;
                $scope.baiHatViPhamXuLy = 'chuaxuly';
                $scope.model.hienTimKiem = 'baihat';
                $scope.loading = false;
                $scope.numberOfPages = function () {
                    return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                }
                if ($scope.numberOfPages() < 8) {
                    $scope.soLuong = $scope.numberOfPages();
                }

            });         
        }
        else {
            $scope.baiHatViPhamXuLy = 'chuaxuly';
            dataservice.taiNguoiDungViPhamChuaXuLy(function (rs) {

                rs = rs.data;
                $scope.taiBaoCaoViPham = rs;
                $scope.model.hienTimKiem = 'nguoidung';
                $scope.baiHatViPhamXuLy = 'chuaxuly';
                $scope.loading = false;
                $scope.numberOfPages = function () {
                    return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                }
                if ($scope.numberOfPages() < 8) {
                    $scope.soLuong = $scope.numberOfPages();
                }

            });         
        }
    }
    $scope.layDanhSach = function (xuly) {
        $scope.loading = true;
        if ($scope.model.hienTimKiem == 'baihat') {
            if (xuly == 'chuaxuly') {
                $scope.baiHatViPhamXuLy = 'chuaxuly';
                dataservice.taiBaiHatViPhamChuaXuLy(function (rs) {
                    $scope.baiHatViPhamXuLy = 'chuaxuly';
                    rs = rs.data;
                    $scope.taiBaoCaoViPham = rs;
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
            if (xuly == 'daxuly') {
                $scope.baiHatViPhamXuLy = 'daxuly';
                $scope.daxuly = 'vipham';
                dataservice.taiBaiHatViPhamDaXuLy_ViPham(function (rs) {
                    $scope.baiHatViPhamXuLy = 'daxuly';
                    $scope.daxuly = 'vipham';
                    rs = rs.data;
                    $scope.taiBaoCaoViPham = rs;
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
        }
        else {
            if (xuly == 'chuaxuly') {
                $scope.baiHatViPhamXuLy = 'chuaxuly';
                dataservice.taiNguoiDungViPhamChuaXuLy(function (rs) {
                    $scope.baiHatViPhamXuLy = 'chuaxuly';
                    rs = rs.data;
                    $scope.taiBaoCaoViPham = rs;
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
            if (xuly == 'daxuly') {
                $scope.baiHatViPhamXuLy = 'daxuly';
                $scope.daxuly = 'vipham';
                dataservice.taiNguoiDungViPhamDaXuLy_ViPham(function (rs) {
                    rs = rs.data;
                    $scope.baiHatViPhamXuLy = 'daxuly';
                    $scope.daxuly = 'vipham';
                    $scope.taiBaoCaoViPham = rs;
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
        }
       
    }
    $scope.layDanhSach_daxuly = function (xuly) {
        $scope.loading = true;
        if ($scope.model.hienTimKiem == 'baihat') {
            if (xuly == 'vipham') {
                $scope.baiHatViPhamXuLy = 'daxuly';
                $scope.daxuly = 'vipham';
                dataservice.taiBaiHatViPhamDaXuLy_ViPham(function (rs) {
                    rs = rs.data;
                    $scope.baiHatViPhamXuLy = 'daxuly';
                    $scope.daxuly = 'vipham';
                    $scope.taiBaoCaoViPham = rs;
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
            if (xuly == 'khongvipham') {
                $scope.baiHatViPhamXuLy = 'daxuly';
                $scope.daxuly = 'khongvipham';
                dataservice.taiBaiHatViPhamDaXuLy_KhongViPham(function (rs) {
                    rs = rs.data;
                    $scope.baiHatViPhamXuLy = 'daxuly';
                    $scope.daxuly = 'khongvipham';
                    $scope.taiBaoCaoViPham = rs;
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
        }
        else {
            if (xuly == 'vipham') {
                $scope.baiHatViPhamXuLy = 'daxuly';
                $scope.daxuly = 'vipham';
                dataservice.taiNguoiDungViPhamDaXuLy_ViPham(function (rs) {
                    rs = rs.data;
                    $scope.baiHatViPhamXuLy = 'daxuly';
                    $scope.daxuly = 'vipham';
                    $scope.taiBaoCaoViPham = rs;
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
            if (xuly == 'khongvipham') {
                $scope.baiHatViPhamXuLy = 'daxuly';
                $scope.daxuly = 'khongvipham';
                dataservice.taiNguoiDungViPhamDaXuLy_KhongViPham(function (rs) {
                    rs = rs.data;
                    $scope.taiBaoCaoViPham = rs;
                    $scope.baiHatViPhamXuLy = 'daxuly';
                    $scope.daxuly = 'khongvipham';
                    $scope.loading = false;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
                    }
                    if ($scope.numberOfPages() < 8) {
                        $scope.soLuong = $scope.numberOfPages();
                    }
                });
            }
        }

    }
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }

    $(".nav-nguoidung").addClass("active");
    $scope.hienTimKiem = false;
    $scope.showSearch = function () {
        if (!$scope.hienTimKiem) {
            $scope.hienTimKiem = true;
        } else {
            $scope.hienTimKiem = false;
        }
    }
      
    //$scope.model11 = {
    //    thoigian: 0,
    //    loibaihat: 'admin'
   
    //}
    //var itemok = [];
    //for (var i = 0; i < 10; i++) {
    //    $scope.model11.thoigian = i;
    //    $scope.model11.loibaihat = "so thu tu:" + i;
    //    itemok.push($scope.model11);
    //}
    
    //$scope.model = {
    //    id: '',
    //    mota: '',
    //    tenchude: '',
    //    linkhinhanh: ''
    //}
    
 
    $scope.initData = function () {
        //$scope.modelShare = {
        //    IdAlbum: '',
        //    IdUsers: JSON.stringify($scope.model11),
        //}
       // taiDanhSachPhatViPham
        dataservice.taiBaiHatViPhamChuaXuLy(function (rs) {

            rs = rs.data;
            $scope.taiBaoCaoViPham = rs;
            $scope.baiHatViPhamXuLy = 'chuaxuly';
            $scope.model.hienTimKiem = 'baihat';
            $scope.loading = false;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiBaoCaoViPham.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        });       
      
    };

    $scope.initData();
    $scope.timKiem = function () {

        $scope.initData();
    };

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

    $scope.chitietbaihatvipham = function (key,vitri) {
     /*   $scope.model.id = key;*/
        var modalInstance =  $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/chitietbaihatvipham.html',
            controller: 'chitietbaihatvipham',
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
        modalInstance.result.then(function (bienxoa) {
            if (bienxoa == 1) {
                $scope.taiBaoCaoViPham.splice(vitri, 1);
            }
        }, function () {
/*                $scope.initData();*/
        });

     
    };
    $scope.chitietnguoidungvipham = function (key, vitri) {
        /*   $scope.model.id = key;*/
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/chitietnguoidungvipham.html',
            controller: 'chitietnguoidungvipham',
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
        modalInstance.result.then(function (bienxoa) {
            if (bienxoa == 1) {
                $scope.taiBaoCaoViPham.splice(vitri, 1);
            }
        }, function () {
            /*                $scope.initData();*/
        });


    };
    alertify.set('notifier', 'position', 'bottom-left');

});

app.controller('chitietbaihatvipham', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {

    $scope.layBaiHatNut = false;
    $scope.moBangLayBaiHat = function () {
        $scope.layBaiHatNut = true;
        $(".modal-content").css({"width": "145%","color":"black" ,"left":"5%"});
       
    }
    $scope.troLai = function () {
        $scope.layBaiHatNut = false;
        $(".modal-content").css({ "width": "230%", "color": "#5A6169", "left": "-35%" });
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.data = para;        
    $scope.initData = function () {
        $scope.baihatbangquyen = {
            nguoidung_id: $scope.data.nguoidung_baocao_id,
            baihat_id: $scope.data.baihat_baocao_id
        }
        dataservice.taiBaiHatBangQuyen($scope.baihatbangquyen, function (rs) {

            rs = rs.data;
            $scope.taiBaiHatViPham = rs;
        });     
        dataservice.taiChiTietNguoiDungViPham($scope.baihatbangquyen, function (rs) {

            rs = rs.data;
            $scope.taiChiTietNguoiDungViPham = rs;
        });     
        if ($scope.data.baihat_id != "") {
            $scope.baihatbangquyen = {
                nguoidung_id: $scope.data.nguoidung_id,
                baihat_id: $scope.data.baihat_id
            }
            dataservice.taiBaiHatBangQuyen($scope.baihatbangquyen, function (rs) {

                rs = rs.data;
                $scope.taiBaiHatBangQuyen = rs;
            });     
        }
         
    };
    $scope.initData();
    $scope.submit = function () {
      
    }
    $scope.voHieuHoaNguoiDung = function (data) {
        $("#loading_main").css("display", "block");
        dataservice.voHieuHoaNguoiDung(data, function (rs) {
            rs = rs.data;
            if (rs == "") {
                alertify.success("Tài khoản phân quyền Admin mới thực hiện chức năng này !!!");
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
            $("#loading_main").css("display", "none");
        });
    }
    $scope.voHieuHoaBaiHat = function (data) {
        $("#loading_main").css("display", "block");
        // chỉnh trường daxoa khi người dùng lien hien thì m sữa lại daxoa
        dataservice.voHieuHoaBaiHatNguoiDung(data, function (rs) {
            rs = rs.data;

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
            $("#loading_main").css("display", "none");
        });
    }
    $scope.pheDuyet = function (data, vhh_nguoidung, vhh_baihat) {
        $("#loading_main").css("display", "block");
        $scope.baocaomodel1 = {
            nguoidung_id: data.nguoidung_id,
            id: $scope.data.id,
            vhh_baihat: vhh_baihat,
            vhh_nguoidung: vhh_nguoidung
        }
        //alert(data.email_nguoidung_baocao + vhh_nguoidung + vhh_baihat);
        dataservice.pheDuyetBaiHatViPham($scope.baocaomodel1, function (rs) {
            rs = rs.data;

            if (rs == true) {  
                
                 alertify.success("Phê duyệt thành công !!!.");
                $uibModalInstance.close(1);
            }
            else {
                alertify.success("Phê duyệt thất bại !!!");
            }
            $("#loading_main").css("display", "none");
                $uibModalInstance.dismiss('cancel');
            
        });
    }
    $scope.khoiPhuc = function (data, vhh_nguoidung, vhh_baihat) {
        $("#loading_main").css("display", "block");
        $scope.baocaomodel1 = {
            nguoidung_id: data.nguoidung_id,
            id: $scope.data.id,
            vhh_baihat: vhh_baihat,
            vhh_nguoidung: vhh_nguoidung
        }
        //alert(data.email_nguoidung_baocao + vhh_nguoidung + vhh_baihat);
        dataservice.khoiPhucBaiHatViPham($scope.baocaomodel1, function (rs) {
            rs = rs.data;

      
            if (rs == true) {

                alertify.success("Khôi Phục thành công !!!.");
               // $uibModalInstance.close(1);
                $uibModalInstance.close(1);
            }
            else {
                alertify.success("Khôi Phục thất bại !!!");
            }
            $("#loading_main").css("display", "none");
            $uibModalInstance.dismiss('cancel');

        });
    }


});
app.controller('chitietnguoidungvipham', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.layBaiHatNut = false;
    $scope.nguoiDung = 'tocao';
    $scope.moBangLayBaiHat = function (data) {
        if (data == 'tocao') {
            $scope.nguoiDung = 'tocao';
            $scope.taiChiTietNguoiDung = $scope.taiChiTietNguoiDungToCao;
        }
        else {
            $scope.nguoiDung = 'vipham';
            $scope.taiChiTietNguoiDung = $scope.taiChiTietNguoiDungViPham;
        }
        $scope.layBaiHatNut = true;
        $(".modal-content").css({ "width": "145%", "color": "black", "left": "5%" });

    }
    $scope.troLai = function () {
        $scope.layBaiHatNut = false;
        $(".modal-content").css({ "width": "160%", "color": "#5A6169", "left": "5%" });
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.data = para;
    $scope.initData = function () {
        $scope.baihatbangquyen = {
            nguoidung_id: $scope.data.nguoidung_baocao_id,
            baihat_id: $scope.data.baihat_baocao_id
        }
        $scope.baihatbangquyen1 = {
            nguoidung_id: $scope.data.nguoidung_id,
            baihat_id: $scope.data.baihat_id
        }      
        dataservice.taiChiTietNguoiDungViPham($scope.baihatbangquyen, function (rs) {

            rs = rs.data;
            $scope.taiChiTietNguoiDungViPham = rs;
            $scope.taiChiTietNguoiDung = $scope.taiChiTietNguoiDungViPham;
        });
        dataservice.taiChiTietNguoiDungViPham($scope.baihatbangquyen1, function (rs) {

            rs = rs.data;
            $scope.taiChiTietNguoiDungToCao = rs;
        });
        if ($scope.data.baihat_id != "") {
            $scope.baihatbangquyen = {
                nguoidung_id: $scope.data.nguoidung_id,
                baihat_id: $scope.data.baihat_id
            }
            dataservice.taiBaiHatBangQuyen($scope.baihatbangquyen, function (rs) {

                rs = rs.data;
                $scope.taiBaiHatBangQuyen = rs;
            });
        }

    };
    $scope.initData();
    $scope.submit = function () {

    }
    $scope.voHieuHoaNguoiDung = function (data) {
        $("#loading_main").css("display", "block");
        dataservice.voHieuHoaNguoiDung(data, function (rs) {
            rs = rs.data;
            if (rs == "") {
                alertify.success("Tài khoản phân quyền Admin mới thực hiện chức năng này !!!");
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
            $("#loading_main").css("display", "none");
        });
    }
   
    $scope.pheDuyet = function (data, vhh_nguoidung) {
        $("#loading_main").css("display", "block");
        $scope.baocaomodel1 = {
            nguoidung_id: data.nguoidung_id,
            id: $scope.data.id,
            vhh_baihat: 0,
            vhh_nguoidung: vhh_nguoidung
        }
        //alert(data.email_nguoidung_baocao + vhh_nguoidung + vhh_baihat);
        dataservice.pheDuyetNguoiDungViPham($scope.baocaomodel1, function (rs) {
            rs = rs.data;

            if (rs == true) {

                alertify.success("Phê duyệt thành công !!!.");
                $uibModalInstance.close(1);
            }
            else {
                alertify.success("Phê duyệt thất bại !!!");
            }
            $("#loading_main").css("display", "none");
            $uibModalInstance.dismiss('cancel');

        });
    }
    $scope.khoiPhuc = function (data, vhh_nguoidung) {
        $("#loading_main").css("display", "block");
        $scope.baocaomodel1 = {
            nguoidung_id: data.nguoidung_id,
            id: $scope.data.id,
            vhh_baihat: 0,
            vhh_nguoidung: vhh_nguoidung
        }
        //alert(data.email_nguoidung_baocao + vhh_nguoidung + vhh_baihat);
        dataservice.khoiPhucNguoiDungViPham($scope.baocaomodel1, function (rs) {
            rs = rs.data;

            //if (rs == 1) {

            //    alertify.success("Khôi Phục thành công !!!.");
            //    $uibModalInstance.close(1);
            //}
            if (rs == true) {

                alertify.success("Khôi Phục thành công !!!.");
                 $uibModalInstance.close(1);
            }
            else {
                alertify.success("Khôi Phục thất bại !!!");
            }
            $("#loading_main").css("display", "none");
            $uibModalInstance.dismiss('cancel');

        });
    }


});
