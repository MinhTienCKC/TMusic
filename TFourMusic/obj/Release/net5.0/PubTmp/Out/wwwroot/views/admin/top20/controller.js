var ctxfolderurl = "/views/admin/top20";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {     
        suaTop20: function (data, callback) {
            $http.post('/Admin/Top20/suaTop20', data).then(callback);
        },
        taiTop20: function (callback) {
            $http.post('/Admin/Top20/taiTop20').then(callback); 
        },
        taiTheLoai: function (callback) {
            $http.post('/Admin/Top20/taiTheloai').then(callback);

        },
        suaLinkHinhAnhDanhSachPhatTop20: function (data, callback) {
            $http.post('/Admin/Top20/suaLinkHinhAnhDanhSachPhatTop20', data).then(callback);
        },
        thayDoiTrangThaiTop20: function (data, callback) {
            $http.post('/Admin/Top20/thayDoiTrangThaiTop20', data).then(callback);
        },
        
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/Top20/GetLinkHinhAnh',
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
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }

    $(".nav-noidung").addClass("active");
    $scope.trangThai = function (data) {  
        dataservice.thayDoiTrangThaiTop20(data, function (rs) {
            rs = rs.data;

            if (rs == true) {
                alertify.success("Thay đổi trang thái thành công.");
            }
            else {
                alertify.success("Thay đổi trạng thái thất bại .");
            }
        });
    }
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
        dataservice.taiTheLoai(function (rs) {


            rs = rs.data;
            $scope.taiTheLoai = rs;
            $scope.valueTheLoai = $scope.rong;
                      
        });
        dataservice.taiTop20(function (rs) {

            rs = rs.data;
            $scope.taiTop20 = rs;
          
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


    $scope.suaHinhAnhDanhSachPhatTop20 = function ($files, data) {
        $scope.BienTam = {
            linkhinhanhmoi: '',
            linkhinhanhcu: '',
            danhsachphattheloai_id: '',
            danhsachphattop20_id: '',
            theloai_id: ''
        }
        $scope.modelDanhSachPhatTop20cs = data;

        if ($files[0].type == "image/png" || $files[0].type == "image/jpeg") {
            duLieuHinh = new FormData();
            duLieuHinh.append("File1", $files[0]);
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.BienTam.linkhinhanhmoi = rs;
                $scope.BienTam.linkhinhanhcu = data.linkhinhanh;
                $scope.BienTam.danhsachphattheloai_id = data.danhsachphattheloai_id;
                $scope.BienTam.danhsachphattop20_id = data.id;
                $scope.BienTam.theloai_id = data.theloai_id;
                dataservice.suaLinkHinhAnhDanhSachPhatTop20($scope.BienTam, function (rs) {
                    rs = rs.data;
                    $scope.modelDanhSachPhatTop20cs.linkhinhanh = $scope.BienTam.linkhinhanhmoi;
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
