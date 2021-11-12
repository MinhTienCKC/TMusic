var ctxfolderurl = "/views/admin/chuDe";

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
        mota: '',
        tenchude: '',
        linkhinhanh: ''
    }
    $scope.initData = function () {

        dataservice.taiChuDe( function (rs) {

            rs = rs.data;
            $scope.taiChuDe = rs;

           
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiChuDe.length / $scope.pageSize);
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
                    $scope.initData();
                }, 2000);
              
        });
        //modalInstance.closed.then(function () {
        //    alert("ok");
        //});
    };
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
                $scope.initData();
        });
    };
    
    
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.xoaChuDe = function (data, vitritheloai) {
    
        dataservice.xoaChuDe(data, function (rs) {
            rs = rs.data;
            $scope.xoaChuDe = rs;
            if (rs == true) {
                alertify.success("Xóa Thành Công");
                $scope.xoaChuDe.splice(vitritheloai, 1);
            }
            else {
                alertify.success("Xóa Thất Bại");
            }
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
        $("#loading_main").css("display", "block");
        if ($scope.dinhDangHinhAnh != "image/"
            || !$scope.addChuDe.addLinkHinhAnh.$valid
            || !$scope.addChuDe.addTenChuDe.$valid) {
            $("#loading_main").css("display", "none");
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
           
            $scope.model.tenchude = $scope.addTenChuDe;
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.model.linkhinhanh = rs;

                dataservice.taoChuDe($scope.model, function (rs) {
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
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice,para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.modelChuDe = para;        
    $scope.initData = function () {

        $scope.editTenChuDe = $scope.modelChuDe.tenchude;
    };
    $scope.initData();
    $scope.submit = function () {
        $("#loading_main").css("display", "block");
        if (!$scope.editChuDe.editTenChuDe.$valid) {
            $("#loading_main").css("display", "none");
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
            $scope.modelChuDe.tenchude = $scope.editTenChuDe;
        
            dataservice.suaChuDe($scope.modelChuDe, function (rs) {
                    rs = rs.data;
                $("#loading_main").css("display", "none");
                $uibModalInstance.dismiss('cancel');
                });
            

        }
    }
});