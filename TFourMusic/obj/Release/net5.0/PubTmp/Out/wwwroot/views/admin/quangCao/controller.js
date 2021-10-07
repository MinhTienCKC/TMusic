var ctxfolderurl = "/views/admin/QuangCao";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        taiQuangCao: function (callback) {
            $http.post('/Admin/QuangCao/taiQuangCao').then(callback);
        },
        xoaQuangCao: function (data, callback) {
            $http.post('/Admin/QuangCao/xoaQuangCao', data).then(callback);
        },
        suaQuangCao: function (data, callback) {
            $http.post('/Admin/QuangCao/suaQuangCao', data).then(callback);
        },
        suaLinkHinhAnhQuangCao: function (data, callback) {
            $http.post('/Admin/QuangCao/suaLinkHinhAnhQuangCao', data).then(callback);

        },
        taoQuangCao: function (data, callback) {
            $http.post('/Admin/QuangCao/taoQuangCao', data).then(callback);
        },
        taiDanhSachBaiHatDeThem_QuangCao: function (callback) {
            $http.post('/Admin/QuangCao/taiDanhSachBaiHatDeThem_QuangCao').then(callback);
        },
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/QuangCao/GetLinkHinhAnh',
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
    alertify.set('notifier', 'position', 'bottom-left');

    $scope.model = {
        id: '',
        mota: '',
        tenquangcao: '',
        linkhinhanh: ''
    }
    $scope.initData = function () {

        dataservice.taiQuangCao(function (rs) {

            rs = rs.data;
            $scope.taiQuangCao = rs;


            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiQuangCao.length / $scope.pageSize);
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
    $scope.taoQuangCao_index = function () {

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


            });

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
        });
    };



    $scope.xoaQuangCao = function (data) {

        dataservice.xoaQuangCao(data, function (rs) {
            rs = rs.data;
            $scope.xoaQuangCao = rs;
            if (rs == true) {
                alertify.success("Xóa Thành Công");
                $scope.$scope.taiQuangCao.splice(vitribaihat, 1);
            }
            else {
                alertify.success("Xóa Thất Bại");
            }

        });

    }
    var duLieuHinh = new FormData();
    $scope.suaHinhAnhQuangCao = function ($files, data) {
        $scope.BienTam = {
            linkhinhanhmoi: '',
            linkhinhanhcu: '',
            quangcao_id: ''
        }
        $scope.modelQuangCaocs = data;

        if ($files[0].type == "image/png" || $files[0].type == "image/jpeg") {
            duLieuHinh = new FormData();
            duLieuHinh.append("File", $files[0]);
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.BienTam.linkhinhanhmoi = rs;
                $scope.BienTam.linkhinhanhcu = data.linkhinhanh;
                $scope.BienTam.quangcao_id = data.id;
                dataservice.suaLinkHinhAnhQuangCao($scope.BienTam, function (rs) {
                    rs = rs.data;
                    $scope.modelQuangCaocs.linkhinhanh = $scope.BienTam.linkhinhanhmoi;
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
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.layBaiHatNut = false;
    $scope.moBangLayBaiHat = function () {
        $scope.layBaiHatNut = true;
    }
    $scope.troLai = function () {
        $scope.layBaiHatNut = false;
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');

    };
    $scope.initData = function () {

        dataservice.taiDanhSachBaiHatDeThem_QuangCao( function (rs) {
            rs = rs.data;
            $scope.taiDanhSachBaiHatDeThem_QuangCao = rs;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiDanhSachBaiHatDeThem_QuangCao.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        })
    };
    $scope.initData();
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;

    };
    $scope.duLieuBaiHat = "";
    $scope.daChonBaiHat = false;
    $scope.themBaiHatVaoQuangCao = function (data) {
        $scope.duLieuBaiHat = data;
        if ($scope.duLieuBaiHat.id != "") {
            $scope.daChonBaiHat = true;
            $scope.layBaiHatNut = false;
        }    
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



    $scope.model = {
        id: '',
        tenquangcao: '',
        linkhinhanh: '',
        noidung:'',
        baihat_id:''
    }

    var duLieuHinh = new FormData();
    $scope.dinhDangHinhAnh = "image/";
    $scope.submit = function () {
        if ($scope.dinhDangHinhAnh != "image/"
            || $scope.duLieuBaiHat == ""
            || !$scope.addQuangCao.addLinkHinhAnh.$valid
            || !$scope.addQuangCao.addNoiDung.$valid
            || !$scope.addQuangCao.addTenQuangCao.$valid) {
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {

            $scope.model.tenquangcao = $scope.addTenQuangCao;
            $scope.model.baihat_id = $scope.duLieuBaiHat.id;
            $scope.model.noidung = $scope.addNoiDung;
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.model.linkhinhanh = rs;

                dataservice.taoQuangCao($scope.model, function (rs) {
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
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.modelQuangCao = para;
    $scope.initData = function () {

        $scope.editTenQuangCao = $scope.modelQuangCao.tenquangcao;
        $scope.editNoiDung = $scope.modelQuangCao.noidung;
    };
    $scope.initData();
    $scope.submit = function () {
        if (!$scope.editQuangCao.editTenQuangCao.$valid
            || !$scope.editQuangCao.editNoiDung.$valid
            ) {
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
            $scope.modelQuangCao.tenquangcao = $scope.editTenQuangCao;
            $scope.modelQuangCao.noidung = $scope.editNoiDung;
            dataservice.suaQuangCao($scope.modelQuangCao, function (rs) {
                rs = rs.data;
            });

            $uibModalInstance.dismiss('cancel');
        }
    }
});