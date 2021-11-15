var ctxfolderurl = "/views/admin/goiVip";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        taiGoiVip: function (callback) {
            $http.post('/Admin/GoiVip/taiGoiVip').then(callback);
        },
        thungRacGoiVip: function (callback) {
            $http.post('/Admin/GoiVip/thungRacGoiVip').then(callback);
        },
        xoaGoiVip: function (data, callback) {
            $http.post('/Admin/GoiVip/xoaGoiVip', data).then(callback);
        },
        suaGoiVip: function (data, callback) {
            $http.post('/Admin/GoiVip/suaGoiVip', data).then(callback);
        },
        suaLinkHinhAnhGoiVip: function (data, callback) {
            $http.post('/Admin/GoiVip/suaLinkHinhAnhGoiVip', data).then(callback);

        },
        taoGoiVip: function (data, callback) {
            $http.post('/Admin/GoiVip/taoGoiVip', data).then(callback);
        },
        xoaVinhVienGoiVip: function (data, callback) {
            $http.post('/Admin/GoiVip/xoaVinhVienGoiVip', data).then(callback);
        },
        khoiPhucGoiVip: function (data, callback) {
            $http.post('/Admin/GoiVip/khoiPhucGoiVip', data).then(callback);
        },
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/GoiVip/GetLinkHinhAnh',
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
app.directive('kiemtraso', function () {
    function link(scope, elem, attrs, ngModel) {
        ngModel.$parsers.push(function (viewValue) {
            var reg = /^[^`~!@#$%\^&*()_+={}|[\]\\:';"<>?,./a-z]*$/;
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
app.controller('index', function ($rootScope, $scope, dataservice, $uibModal) {
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


    $scope.model = {
        id: '',
        sothang: 0,
        tengoivip: '',
        linkhinhanh: '',
        giatiengoc: 0,
        giatiengiamgia: 0,
        trangthai: 0
    }
    $scope.initData = function () {

        dataservice.taiGoiVip(function (rs) {

            rs = rs.data;
            $scope.taiGoiVip = rs;


            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiGoiVip.length / $scope.pageSize);
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
    $scope.taoGoiVip_index = function () {

        if ($scope.taiGoiVip.length >= 5 ) {
            alertify.success("Chỉ được tạo tối da 5 gói vip !!!");
            return;
        }
        var modalInstance = $uibModal.open({
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
                $scope.initData();
            }, 2000);

        });
        //modalInstance.closed.then(function () {
        //    alert("ok");
        //});
    };
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

    alertify.set('notifier', 'position', 'bottom-left');

    $scope.xoaGoiVip = function (data, vitrigoivip) {
        if ($scope.taiGoiVip.length <= 1) {
            alertify.success("Không thể xóa !!!");
            return;
        }
        dataservice.xoaGoiVip(data, function (rs) {
            rs = rs.data;
            $scope.xoaGoiVip = rs;
            if (rs == true) {
                alertify.success("Xóa Thành Công");
                $scope.taiGoiVip.splice(vitrigoivip, 1);
            }
            else {
                alertify.success("Xóa Thất Bại");
            }
        });

    }
    var duLieuHinh = new FormData();
    $scope.suaHinhAnhGoiVip = function ($files, data) {
        $scope.BienTam = {
            linkhinhanhmoi: '',
            linkhinhanhcu: '',
            goivip_id: ''
        }
        $scope.modelGoiVipcs = data;

        if ($files[0].type == "image/png" || $files[0].type == "image/jpeg") {
            duLieuHinh = new FormData();
            duLieuHinh.append("File", $files[0]);
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.BienTam.linkhinhanhmoi = rs;
                $scope.BienTam.linkhinhanhcu = data.linkhinhanh;
                $scope.BienTam.goivip_id = data.id;
                dataservice.suaLinkHinhAnhGoiVip($scope.BienTam, function (rs) {
                    rs = rs.data;
                    $scope.modelGoiVipcs.linkhinhanh = $scope.BienTam.linkhinhanhmoi;
                });
            });
        } else {
            alert("Sai định đạng ảnh (*.jpg, *.png)");
        }

        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("File", $files[i]);
        //}

    }
    $scope.thungrac = function () {

        var modalInstance = $uibModal.open({

            templateUrl: ctxfolderurl + '/thungrac.html',
            controller: 'thungrac',

            backdropClass: ".fade:not(.show)",
            backdropClass: ".modal-backdrop",
            backdropClass: ".col-lg-8",
            backdropClass: ".modal-content",

            size: '10'

        });
        modalInstance.result.then(function () {
            $scope.initData();
            //setTimeout(function () {

            //}, 1500);
        }, function () {

        });

    };
});
app.controller('add', function ($rootScope, $scope, dataservice, $uibModalInstance) {
    $scope.soThangVuot = 37;

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');

    };

    
    $scope.model = {
        id: '',
        sothang: 0,
        tengoivip: '',
        linkhinhanh: '',
        giatiengoc: 0,
        giatiengiamgia: 0,
        trangthai: 0
    }

    var duLieuHinh = new FormData();
    $scope.dinhDangHinhAnh = "image/";
    $scope.submit = function () {
        $("#loading_main").css("display", "block");
        if ($scope.dinhDangHinhAnh != "image/"
            || $scope.addGiaTienGoc < $scope.addGiaTienGiamGia
            || !$scope.addGoiVip.addSoThang.$valid
            || !$scope.addGoiVip.addGiaTienGiamGia.$valid
            || !$scope.addGoiVip.addGiaTienGoc.$valid
            || !$scope.addGoiVip.addLinkHinhAnh.$valid
            || !$scope.addGoiVip.addTenGoiVip.$valid) {
            $("#loading_main").css("display", "none");
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {

            $scope.model.tengoivip = $scope.addTenGoiVip;
            $scope.model.sothang = $scope.addSoThang;
            $scope.model.giatiengiamgia = $scope.addGiaTienGiamGia;
            $scope.model.giatiengoc = $scope.addGiaTienGoc;
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.model.linkhinhanh = rs;

                dataservice.taoGoiVip($scope.model, function (rs) {
                    rs = rs.data;
                    $("#loading_main").css("display", "none");
                    $uibModalInstance.dismiss('cancel');
                });
            })
          
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
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.soThangVuot = 37;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.modelGoiVip = para;
    $scope.initData = function () {

        $scope.editTenGoiVip = $scope.modelGoiVip.tengoivip;
        $scope.editSoThang = $scope.modelGoiVip.sothang;
        $scope.editGiaTienGoc = $scope.modelGoiVip.giatiengoc;
        $scope.editGiaTienGiamGia = $scope.modelGoiVip.giatiengiamgia;

    };
    $scope.initData();
    $scope.submit = function () {
        $("#loading_main").css("display", "block");
        if ($scope.editGiaTienGoc < $scope.editGiaTienGiamGia
            || !$scope.editGoiVip.editSoThang.$valid
            || !$scope.editGoiVip.editGiaTienGiamGia.$valid
            || !$scope.editGoiVip.editGiaTienGoc.$valid
            || !$scope.editGoiVip.editTenGoiVip.$valid) {
            $("#loading_main").css("display", "none");
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
            $scope.modelGoiVip.tengoivip = $scope.editTenGoiVip;
            $scope.modelGoiVip.sothang = $scope.editSoThang;
            $scope.modelGoiVip.giatiengiamgia = $scope.editGiaTienGiamGia;
            $scope.modelGoiVip.giatiengoc = $scope.editGiaTienGoc;

            dataservice.suaGoiVip($scope.modelGoiVip, function (rs) {
                rs = rs.data;

                $("#loading_main").css("display", "none");
                $uibModalInstance.dismiss('cancel');
            });

          
        }
    }
});
app.controller('thungrac', function ($rootScope, $scope, dataservice, $uibModalInstance) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.text = {
        key: ''
    }


    $scope.initData = function () {
        dataservice.thungRacGoiVip(function (rs) {
            rs = rs.data;
            $scope.thungRacGoiVip = rs;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.thungRacGoiVip.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        });

    }
    $scope.initData();
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };

    $scope.xoaVinhVien = function (data, vitribaihat) {


        dataservice.xoaVinhVienGoiVip(data, function (rs) {
            rs = rs.data;
            if (rs == true) {
                alertify.success("Xóa Thành Công");
                $scope.thungRacGoiVip.splice(vitribaihat, 1);
            }
            else {
                alertify.success("Xóa Thất Bại");
            }

        });
    };
    alertify.set('notifier', 'position', 'bottom-left');


    $scope.khoiPhucGoiVip = function (data, vitribaihat) {
        dataservice.khoiPhucGoiVip(data, function (rs) {
            rs = rs.data;
            if (rs == "loi5") {
                alertify.success("Không Thể Khôi Phục Gói Vip <= 5");
                return;
            }
            if (rs == true) {
                alertify.success("Khôi Phục Thành Công");
                $scope.thungRacGoiVip.splice(vitribaihat, 1);
            }
            else {
                alertify.success("Khôi Phục Thất Bại");
            }

            $scope.initData();
        });
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



});