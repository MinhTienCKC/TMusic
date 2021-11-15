var ctxfolderurl = "/views/admin/danhSachPhat";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        createdanhsachphat: function (data, callback) {
            $http.post('/Admin/DanhSachPhat/CreateDanhSachPhat',data).then(callback);
        },
        deletedanhsachphat: function (data, callback) {
            $http.post('/Admin/DanhSachPhat/DeleteDanhSachPhat', data).then(callback);
        },
        editdanhsachphat: function (data, callback) {
            $http.post('/Admin/DanhSachPhat/EditDanhSachPhat', data).then(callback);
        },
        loaddanhsachphat: function (callback) {
            $http.post('/Admin/DanhSachPhat/LoadDanhSachPhat').then(callback);
          
        },
        loadchitietdanhsachphat: function (data,callback) {
            $http.post('/Admin/DanhSachPhat/LoadChiTietDanhSachPhat',data).then(callback);

        },
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/DanhSachPhat/GetLinkHinhAnh',
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

}])
app.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
});
app.filter('startFrom', function () {
    return function (input, start) {
        if (!input || !input.length) { return; }
        start = +start; //parse to int
        return input.slice(start);
    }
});
app.controller('T_Music', function () {


});
app.controller('index', function ($rootScope, $scope, dataservice, $uibModal) {
  

    $scope.initData = function () {
        dataservice.loaddanhsachphat(function (rs) {
            
            rs = rs.data;
            $scope.dataDanhSachPhat = rs;
        });

    };
    $scope.initData();
    $scope.model = {
      
        id: '',
       
        tendanhsachphat: '',
        mota: ''
    }
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };
    $scope.chitietDSP = function (key) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/chitietdsp.html',
            controller: 'chitietdsp',
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
        dataservice.createdanhsachphat($scope.model, function (rs) {
            rs = rs.data;
            $scope.data = rs;
            $scope.initData();
        });
      
    }
    $scope.delete = function (key) {
        $scope.model.id = key;
        dataservice.deletedanhsachphat($scope.model, function (rs) {
            rs = rs.data;
            $scope.deletedata = rs;
            $scope.initData();
        });

    }

});
app.controller('chitietdsp', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.ok = function () {
        $uibModalInstance.close();
    };
    $scope.data1 = para;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {

        id: '',

        tendanhsachphat: '',
        mota: ''

    }
    $scope.initData = function () {
        dataservice.loadchitietdanhsachphat($scope.data1.object, function (rs) {
            rs = rs.data;
            $scope.dataloadchitietdanhsachphat = rs;

        });
        
    }
    $scope.initData();
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
        linkhinhanh:'',
        tendanhsachphat: '',
        mota: ''
       
    }
    
        var formData = new FormData();
        $scope.submit = function () {

            dataservice.uploadHinhAnh(formData, function (rs) {
                rs = rs.data;
                $scope.model.linkhinhanh = rs;

                dataservice.createdanhsachphat($scope.model, function (rs) {
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
    $scope.model = {


        id: '',
        linkhinhanh: '',
        tendanhsachphat: '',
        mota: ''
       
    }
   
    var formData = new FormData();
    $scope.submit = function () {
        // delete $scope.model['tentheloai'];
        //  $scope.model.theloai_id = $scope.valueTheLoai;
        $scope.model = $scope.data1;
        if ($scope.kt == 1) {
            dataservice.uploadHinhAnh(formData, function (rs) {
                rs = rs.data;
                $scope.model.object.linkhinhanh = rs;

                dataservice.editdanhsachphat($scope.model.object, function (rs) {
                    rs = rs.data;
                    $scope.EditItem = rs;

                });
            })
            $uibModalInstance.dismiss('cancel');
        } else {
            dataservice.editdanhsachphat($scope.model.object, function (rs) {
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