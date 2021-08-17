var ctxfolderurl = "/views/admin/danhSachPhatNguoiDung";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        createdanhsachphatnguoidung: function (data, callback) {
            $http.post('/Admin/DanhSachPhatNguoiDung/CreateDanhSachPhatNguoiDung',data).then(callback);
        },
        deletedanhsachphatnguoidung: function (data, callback) {
            $http.post('/Admin/DanhSachPhatNguoiDung/DeleteDanhSachPhatNguoiDung', data).then(callback);
        },
        editdanhsachphatnguoidung: function (data, callback) {
            $http.post('/Admin/DanhSachPhatNguoiDung/EditDanhSachPhatNguoiDung', data).then(callback);
        },
        loaddanhsachphatnguoidung: function (callback) {
            $http.post('/Admin/DanhSachPhatNguoiDung/LoadDanhSachPhatNguoiDung').then(callback);
          
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
  

    $scope.initData = function () {
        dataservice.loaddanhsachphatnguoidung(function (rs) {
            
            rs = rs.data;
            $scope.dataDanhSachPhatNguoiDung = rs;

            $scope.numberOfPages = function () {
                return Math.ceil($scope.dataDanhSachPhatNguoiDung.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        });

    };
    $scope.initData();
    $scope.model = {
      
        id: '',
        nguoidung_id: '',
        tendanhsachphat: '',
        mota: ''
    }
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
    $scope.add = function () {
      
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
            $scope.initData();
        }, function () {
        });
    };
    $scope.edit = function (key) {

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
        modalInstance.result.then(function () {
            $scope.initData();
        }, function () {
        });
    };
    $scope.submit = function () {
        dataservice.createdanhsachphatnguoidung($scope.model, function (rs) {
            rs = rs.data;
            $scope.data = rs;
            $scope.initData();
        });
      
    }
    $scope.delete = function (key) {
        $scope.model.id = key;
        dataservice.deletedanhsachphatnguoidung($scope.model, function (rs) {
            rs = rs.data;
            $scope.deletedata = rs;
            $scope.initData();
        });

    }

});
app.controller('add', function ($rootScope, $scope, dataservice,$uibModalInstance) {
    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {

        id: '',
        nguoidung_id: 'admin',
        tendanhsachphat: '',
        mota: ''
       
    }
    $scope.submit = function () {

        dataservice.createdanhsachphatnguoidung($scope.model, function (rs) {
            rs = rs.data;
            $scope.data = rs;

        });
        $uibModalInstance.dismiss('cancel');
    }

});
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice,para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.data1 = para;
    $scope.model = {

        id: '',
        nguoidung_id: '',
        tendanhsachphat: '',
        mota: ''
       
    }
    $scope.submit = function () {
        $scope.model = $scope.data1;
        dataservice.editdanhsachphatnguoidung($scope.model.object, function (rs) {
            rs = rs.data;
            $scope.EditItem = rs;

        });
        $uibModalInstance.dismiss('cancel');
    }
  
});