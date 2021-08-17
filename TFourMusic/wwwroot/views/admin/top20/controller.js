var ctxfolderurl = "/views/admin/top20";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {     
        suaTop20: function (data, callback) {
            $http.post('/Admin/Top20/SuaTop20', data).then(callback);
        },
        taiTop20: function (callback) {
            $http.post('/Admin/Top20/TaiTop20').then(callback); 
        },
        loadtheloai: function (callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/LoadTheloai').then(callback);

        },
       

        
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/DanhSachPhatTheLoai/GetLinkHinhAnh',
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
     
    $scope.model = {
        id: '',
        theloai_id: '',
        tentop20: '',
        mota: '',
        linkhinhanh: ''
    }
    $scope.text = {
        key: ''
    }
    $scope.rong = '';
    $scope.initData = function () {
        dataservice.loadtheloai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;
            $scope.valueTheLoai = $scope.rong;
           
          //  $scope.text.key = $scope.valueTheLoai;
           
        });
        dataservice.taiTop20(function (rs) {

            rs = rs.data;
            $scope.taiTop20 = rs;

            $scope.q = '';

            $scope.getData = function () {

                return $filter('filter')($scope.taiTop20, $scope.q)

            }

            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiTop20.length / $scope.pageSize);
            }
            $scope.soTrang = $scope.numberOfPages();
        });
        $scope.currentPage = 0;
        $scope.pageSize = 5;
        $scope.size = 0;
        $scope.soLuong = 8;
    };

    $scope.initData();
    //$scope.timKiem = function () {

    //    $scope.initData();
    //};
    function chuyenDoi(object) {
       
        return Math.ceil(object.length / 5);;
    }
    $scope.changeTheLoai = function (object) {
        //alert(ok.length);
        if (chuyenDoi(object) < 8) {
            $scope.soLuong = chuyenDoi(object);
            $scope.soTrang = chuyenDoi(object)
            $scope.size = 0;
            $scope.currentPage = 0;
        }
        else {
            $scope.soLuong = 8;
            $scope.soTrang = chuyenDoi(object)
            $scope.size = 0;
            $scope.currentPage = 0;
          //  $scope.numberOfPages() = chuyenDoi(object);
        }
       // $scope.timkiem.object.theloai_id = $scope.valueTheLoai;
    };
    //$scope.timKiem = function () {

    //    $scope.initData();
    //};

    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };


    

    $scope.Truoc = function () {
        if ($scope.soTrang < 8 ) {
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
        if ($scope.soTrang < 8 ) {
            return;
        }
        else {
           
            if ($scope.soTrang % 8 == 0) {
                if ($scope.size + $scope.soLuong >= $scope.soTrang)
                    $scope.size = 0;
                else {
                    $scope.size += $scope.soLuong;
                    
                }
                $scope.currentPage = $scope.size;
            }
            else {
                $scope.bienTam = $scope.soTrang % 8;
                $scope.bienTam2 = $scope.soTrang - $scope.bienTam;
                if ($scope.size + $scope.soLuong == $scope.bienTam2 ) {

                    $scope.size += 8;
                    $scope.soLuong = $scope.bienTam;
                }
                else if ($scope.size + $scope.soLuong >= $scope.soTrang) {
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
    $scope.edit = function (key) {
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
        modalInstance.result.then(function () {
            
        }, function () {
        });
    };
   

});
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice,para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.data1 = para;  
    $scope.model = {
        id: '',
        theloai_id: '',
        tendanhsachphattheloai: '',
        mota: '',
        linkhinhanh: ''
    }
    $scope.initData = function () {
        $scope.kt = 0;
    };
    $scope.initData();
    var formData = new FormData();
    $scope.submit = function () {
        $scope.model = $scope.data1;
        if ($scope.kt == 1) {
                 dataservice.uploadHinhAnh(formData, function (rs) {
                rs = rs.data;
                $scope.model.object.linkhinhanh = rs;

                dataservice. suaDanhSachPhatTheLoai($scope.model.object, function (rs) {
                    rs = rs.data;
                    $scope.EditItem = rs;

                });
                 })
            $uibModalInstance.dismiss('cancel');
        } else {
            dataservice.suaDanhSachPhatTheLoai($scope.model.object, function (rs) {
                             rs = rs.data;
                             $scope.EditItem = rs;

                         });
            $uibModalInstance.dismiss('cancel');
        }                                  
    }
    $scope.getTheFilesHinhAnh = function ($files) {    
        formData = new FormData();
        formData.append("File", $files[0]); 
        $scope.kt = 1;
    }
});