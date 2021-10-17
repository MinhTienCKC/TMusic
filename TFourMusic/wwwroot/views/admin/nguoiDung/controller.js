var ctxfolderurl = "/views/admin/NguoiDung";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {      
        taiNguoiDung: function (callback) {
            $http.post('/Admin/NguoiDung/taiNguoiDung').then(callback);
        },   
        voHieuHoa: function (data,callback) {
            $http.post('/Admin/NguoiDung/voHieuHoa',data).then(callback);
        },  
        xemBaiHatNguoiDung: function (data, callback) {
            $http.post('/Admin/NguoiDung/xemBaiHatNguoiDung', data).then(callback);
        },  
        xemDanhSachPhatNguoiDung: function (data, callback) {
            $http.post('/Admin/NguoiDung/xemDanhSachPhatNguoiDung', data).then(callback);
        },  
        voHieuHoaDanhSachPhatNguoiDung: function (data, callback) {
            $http.post('/Admin/NguoiDung/voHieuHoaDanhSachPhatNguoiDung', data).then(callback);
        },  
        voHieuHoaBaiHatNguoiDung: function (data, callback) {
            $http.post('/Admin/NguoiDung/voHieuHoaBaiHatNguoiDung', data).then(callback);
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

        dataservice.taiNguoiDung(function (rs) {

            rs = rs.data;
            $scope.taiNguoiDung = rs;

            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiNguoiDung.length / $scope.pageSize);
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

    $scope.voHieuHoa = function (data) {

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

    $scope.xemBaiHat = function (key) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/baihatnguoidung.html',
            controller: 'baihatnguoidung',
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

            setTimeout(function () {
                $scope.initData();
            }, 2000);
        });
    }
    $scope.xemDanhSachPhat = function (key) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/danhsachphatnguoidung.html',
            controller: 'danhsachphatnguoidung',
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

            setTimeout(function () {
                $scope.initData();
            }, 2000);
        });
    }

    

});
app.controller('baihatnguoidung', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.ok = function () {
        $uibModalInstance.close();
    };
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.duLieuDSPTL = para;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.modelChiTietDSPTL = {
        baihat_id: '',
        danhsachphattheloai_id: $scope.duLieuDSPTL.id,
        id: ''
    }
    $scope.text = {

        key: '',
        uid: ''

    }
    $scope.initData = function () {
        dataservice.xemBaiHatNguoiDung(para, function (rs) {
            rs = rs.data;
            $scope.xemBaiHatNguoiDung = rs;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.xemBaiHatNguoiDung.length / $scope.pageSize);
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

    $scope.voHieuHoaBaiHat = function (data) {

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
app.controller('danhsachphatnguoidung', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.ok = function () {
        $uibModalInstance.close();
    };
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.duLieuDSPTL = para;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.modelChiTietDSPTL = {
        baihat_id: '',
        danhsachphattheloai_id: $scope.duLieuDSPTL.id,
        id: ''
    }
    $scope.text = {

        key: '',
        uid: ''

    }
    $scope.initData = function () {
        dataservice.xemDanhSachPhatNguoiDung(para, function (rs) {
            rs = rs.data;
            $scope.xemDanhSachPhatNguoiDung = rs;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.xemDanhSachPhatNguoiDung.length / $scope.pageSize);
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

    $scope.voHieuHoaDanhSachPhat = function (data) {

        dataservice.voHieuHoaDanhSachPhatNguoiDung(data, function (rs) {
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