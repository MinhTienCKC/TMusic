var ctxfolderurl = "/views/admin/danhSachPhatTheLoai";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        taoDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Admin/danhSachPhatTheLoai/TaoDanhSachPhatTheLoai',data).then(callback);
        },
        xoaDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/XoaDanhSachPhatTheLoai', data).then(callback);
        },
        suaDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/SuaDanhSachPhatTheLoai', data).then(callback);
        },
        taiDanhSachPhatTheLoai: function (data,callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/TaiDanhSachPhatTheLoai',data).then(callback);
          
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
  

    //$scope.initData = function () {
    //    dataservice.loadchude(function (rs) {
            
    //        rs = rs.data;
    //        $scope.dataloadchude = rs;
          

            
    //    });

    //};

    //$scope.initData();
    
    $scope.model = {
        id: '',
        theloai_id: '',
        tendanhsachphattheloai: '',
        mota: '',
        linkhinhanh: ''
    }
    $scope.text = {
        key: ''
    }
    $scope.initData = function () {
        dataservice.loadtheloai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;
            $scope.valueTheLoai = 1;
            $scope.text.key = $scope.valueTheLoai;
            dataservice.taiDanhSachPhatTheLoai($scope.text,function (rs) {

                rs = rs.data;
                $scope.datataiDanhSachPhatTheLoai = rs;

                $scope.q = '';

                $scope.getData = function () {

                    return $filter('filter')($scope.datataiDanhSachPhatTheLoai, $scope.q)

                }

                $scope.numberOfPages = function () {
                    return Math.ceil($scope.datataiDanhSachPhatTheLoai.length / $scope.pageSize);
                }
            });
        });

       
    };

    $scope.initData();

    $scope.changeTheLoai = function () {
        $scope.text.key = $scope.valueTheLoai;
        dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {

            rs = rs.data;
            $scope.datataiDanhSachPhatTheLoai = rs;

            //$scope.q = '';

            //$scope.getData = function () {

            //    return $filter('filter')($scope.datataiDanhSachPhatTheLoai, $scope.q)

            //}

            $scope.numberOfPages = function () {
                return Math.ceil($scope.datataiDanhSachPhatTheLoai.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        });

    };
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



    //$scope.Prev = function () {
    //    if ($scope.currentPage == 0) {

    //        $scope.currentPage = 0;
    //    }
    //    else

    //        $scope.currentPage = $scope.currentPage - 1;
    //}
    //$scope.Next = function () {
    //    if ($scope.currentPage < $scope.numberOfPages() - 1) {
    //        $scope.currentPage = $scope.currentPage + 1;

    //    } else
    //        $scope.currentPage = 0;

    //}
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
                if ($scope.size + $scope.soLuong == $scope.bienTam2 ) {

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
    $scope.submit = function () {
        dataservice.taoDanhSachPhatTheLoai($scope.model, function (rs) {
            rs = rs.data;
            $scope.data = rs;
            $scope.initData();
        });
      
    }
    $scope.delete = function (key) {
        $scope.model.id = key;
        dataservice.xoaDanhSachPhatTheLoai($scope.model, function (rs) {
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
    $scope.initData = function () {
       
        dataservice.loadtheloai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;
           

            $scope.valueTheLoai = rs[0].object.id;
           
        });

    };
    
    $scope.initData();
    $scope.model = {
        id: '',
        theloai_id: '',
        tendanhsachphattheloai: '',
        mota:'',
        linkhinhanh: ''
    }
    var formData = new FormData();
    $scope.submit = function () {
        $scope.model.theloai_id = $scope.valueTheLoai;
        dataservice.uploadHinhAnh(formData, function (rs) {
            rs = rs.data;
            $scope.model.linkhinhanh = rs;

            dataservice.taoDanhSachPhatTheLoai($scope.model, function (rs) {
                rs = rs.data;
                $scope.data = rs;

            });
        })
       
        $uibModalInstance.dismiss('cancel');
    }
   
    $scope.getTheFilesHinhAnh = function ($files) {
        //chọn nhiều ảnh
        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("file", $files[i]);
        //}    
        formData = new FormData();
            formData.append("File", $files[0]);          
     //   $scope.link = $files[0].name;                      
                      
    }
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
    /* $scope.valueTheLoai = $scope.data1.tentheloai;*/
        //$scope.model = $scope.data1;
      
        //dataservice.loadtheloai(function (rs) {

        //    rs = rs.data;
        //    $scope.dataloadtheloai = rs;
        //  //  var so = $scope.dataloadtheloai.length;
            
        //    for (var i = 0; i < $scope.dataloadtheloai.length; i++) {
        //        if ($scope.data1.tentheloai == rs[i].object.tentheloai) {
        //            $scope.valueTheLoai = rs[i].object.id;
        //            break;
        //        }
        //    }      

        //});
        $scope.kt = 0;
    };
    $scope.initData();
    //$scope.changeGrade = function () {


    //}
    var formData = new FormData();
    $scope.submit = function () {
       // delete $scope.model['tentheloai'];
        //  $scope.model.theloai_id = $scope.valueTheLoai;
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
       
       
       
                  

        //$uibModalInstance.dismiss('cancel');
    }
    
    $scope.getTheFilesHinhAnh = function ($files) {
         
        formData = new FormData();
        formData.append("File", $files[0]);
      
        $scope.kt = 1;
    }


});