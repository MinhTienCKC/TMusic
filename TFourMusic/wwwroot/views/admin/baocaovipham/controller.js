var ctxfolderurl = "/views/admin/baocaovipham";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        taiChuDe: function ( callback) {
            $http.post('/Admin/ChuDe/taiChuDe').then(callback);
        },
        xoaChuDe: function (data, callback) {
            $http.post('/Admin/ChuDe/xoaChuDe', data).then(callback);
        },
        suaChuDe: function (data, callback) {
            $http.post('/Admin/ChuDe/suaChuDe', data).then(callback);
        },
        suaLinkHinhAnhChuDe: function (data, callback) {
            $http.post('/Admin/ChuDe/suaLinkHinhAnhChuDe', data).then(callback);

        },  
        taoChuDe: function (data, callback) {
            $http.post('/Admin/ChuDe/taoChuDe', data).then(callback);
        },
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
        pheDuyetBaiHatViPham: function (data, callback) {
            $http.post('/Admin/BaoCaoViPham/pheDuyetBaiHatViPham', data).then(callback);
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
    $scope.loading = true;
    $scope.model = {
        ngaybatdau: $scope.date,
        ngayketthuc: $scope.date,
        theonam: $scope.date,
        theothang: $scope.date,
        hienTimKiem: 'baihat'
    }
    $scope.baiHatViPhamXuLy = 'chuaxuly';
    $scope.chuyenDoi = function (data) {
        $scope.model.hienTimKiem = data;
        $scope.loading = true;
    }
    $scope.layDanhSach = function (xuly) {
        $scope.loading = true;
        if (xuly == 'chuaxuly') {
            $scope.baiHatViPhamXuLy = 'chuaxuly';
            dataservice.taiBaiHatViPhamChuaXuLy(function (rs) {

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
            dataservice.taiBaiHatViPhamDaXuLy(function (rs) {
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
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }

    $(".nav-noidung").addClass("active");
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
    $scope.taoChuDe_index = function () {
      
        var modalInstance =  $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/add.html',
            controller: 'add',
            backdrop: 'true',
            backdropClass: ".fade:not(.show)",
            backdropClass: ".modal-backdrop",
            backdropClass: ".col-lg-8",
            backdropClass: ".modal-content",
            
            size: '100'
        });
        modalInstance.result.then(function () {
          
        }, function () {
                setTimeout(function () {
                   

                });

                setTimeout(function () {
                    $scope.initData();
                }, 2000);
              
        });
        //modalInstance.closed.then(function () {
        //    alert("ok");
        //});
    };
    $scope.edit = function (key,vitri) {
     /*   $scope.model.id = key;*/
        var modalInstance =  $uibModal.open({
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
        modalInstance.result.then(function (bienxoa) {
            if (bienxoa == 1) {
                $scope.taiBaoCaoViPham.splice(vitri, 1);
            }
        }, function () {
                $scope.initData();
        });

     
    };
    
    alertify.set('notifier', 'position', 'bottom-left');

    $scope.xoaChuDe = function (data) {
    
        dataservice.xoaChuDe(data, function (rs) {
            rs = rs.data;
            $scope.xoaChuDe = rs;
           
        });

    }
    var duLieuHinh = new FormData();
    $scope.suaHinhAnhChuDe = function ($files, data) {
        $scope.BienTam = {
            linkhinhanhmoi: '',
            linkhinhanhcu: '',
            chude_id: ''
        }
        $scope.modelchudecs = data;

        if ($files[0].type == "image/png" || $files[0].type == "image/jpeg") {
            duLieuHinh = new FormData();
            duLieuHinh.append("File", $files[0]);
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.BienTam.linkhinhanhmoi = rs;
                $scope.BienTam.linkhinhanhcu = data.linkhinhanh;
                $scope.BienTam.chude_id = data.id;
                dataservice.suaLinkHinhAnhChuDe($scope.BienTam, function (rs) {
                    rs = rs.data;
                    $scope.modelchudecs.linkhinhanh = $scope.BienTam.linkhinhanhmoi;
                });
            });
        } else {
            alert("Sai định đạng ảnh (*.jpg, *.png)");
        }

        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("File", $files[i]);
        //}

    }
});
app.controller('add', function ($rootScope, $scope, dataservice, $uibModalInstance) {

   
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
      
    };
      
  
    $scope.model = {
        id: '',
        tenchude: '',
        linkhinhanh: ''
    }
   
    var duLieuHinh = new FormData();
    $scope.dinhDangHinhAnh = "image/";
    $scope.submit = function () {
        if ($scope.dinhDangHinhAnh != "image/"
            || !$scope.addChuDe.addLinkHinhAnh.$valid
            || !$scope.addChuDe.addTenChuDe.$valid) {
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
           
            $scope.model.tenchude = $scope.addTenChuDe;
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.model.linkhinhanh = rs;

                dataservice.taoChuDe($scope.model, function (rs) {
                    rs = rs.data;

                });
            })
            $uibModalInstance.dismiss('cancel');
        }

        
    }
   
     $scope.getTheFilesHinhAnh = function ($files) {
        $scope.addLinkHinhAnh = "Đã Chọn Hình Ảnh";
        duLieuHinh = new FormData();
        duLieuHinh.append("File", $files[0]);     
        if ($files[0].type == "image/png" || $files[0].type == "image/jpg" || $files[0].type == "image/jpeg") {
            $scope.dinhDangHinhAnh = "image/"
        }
        else {
            $scope.dinhDangHinhAnh = $files[0].type;
        }
        var nutao = document.getElementById("btntext");
         nutao.click();        
    }
});
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice,para) {
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

        dataservice.voHieuHoaNguoiDung(data, function (rs) {
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
        });
    }
    $scope.voHieuHoaBaiHat = function (data) {
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
        });
    }
    $scope.pheDuyet = function (data, vhh_nguoidung, vhh_baihat) {
       
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
         
                $uibModalInstance.dismiss('cancel');
            
        });
    }



});