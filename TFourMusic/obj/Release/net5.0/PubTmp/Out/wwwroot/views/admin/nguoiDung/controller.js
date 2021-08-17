var ctxfolderurl = "/views/admin/nguoiDung";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        createnguoidung: function (data, callback) {
            $http.post('/Admin/NguoiDung/CreateNguoiDung',data).then(callback);
        },
        deletenguoidung: function (data, callback) {
            $http.post('/Admin/NguoiDung/DeleteNguoiDung', data).then(callback);
        },
        editnguoidung: function (data, callback) {
            $http.post('/Admin/NguoiDung/EditNguoiDung', data).then(callback);
        },
        loadnguoidung: function (callback) {
            $http.post('/Admin/NguoiDung/LoadNguoiDung').then(callback);
          
        },
        //loadtheloai: function (callback) {
        //    $http.post('/Admin/NguoiDung/LoadTheloai').then(callback);

        //},
       

        
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

}])
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
  

    $scope.initData = function () {
        dataservice.loadnguoidung(function (rs) {
            
            rs = rs.data;
            $scope.dataloadnguoidung = rs;
          

            
        });

    };

    $scope.initData();
    
    $scope.model = {
        id: '',            
        taikhoan: '',
        matkhau: '',
        email: '',
        hoten: '',
        quocgia: '',
        thanhpho: '',
        website: '',
        mota: '',
        ngaysinh: '',
        facebook: '',
        hinhdaidien: '',
        cover: '',
        gioitinh: '',
        online: ''
    }
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };
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
    //$scope.submit = function () {
    //    dataservice.createchude($scope.model, function (rs) {
    //        rs = rs.data;
    //        $scope.data = rs;
    //        $scope.initData();
    //    });
      
    //}
    $scope.delete = function (key) {
        $scope.model.id = key;
        dataservice.deletenguoidung($scope.model, function (rs) {
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
       
        //dataservice.loadtheloai(function (rs) {


        //    rs = rs.data;
        //    $scope.dataloadtheloai = rs;
           

        //    $scope.valueTheLoai = rs[0].object.id;
           
        //});
      
    };
 
    $scope.initData();
    $scope.model = {
        id: '',
        taikhoan: '',
        matkhau: '',
        email: '',
        hoten: '',
        quocgia: '',
        thanhpho: '',
        website: '',
        mota: '',
        ngaysinh: '',
        facebook: '',
        hinhdaidien: '',
        cover: '',
        gioitinh: ''
      
    }
    $scope.model.gioitinh = 'Nam';
    $scope.model.quocgia = 'Việt Nam';
    var formData = new FormData();
    $scope.submit = function () {
        alert($scope.model.gioitinh);
        dataservice.uploadHinhAnh(formData, function (rs) {
            rs = rs.data;
            $scope.model.hinhdaidien = rs;

            dataservice.createnguoidung($scope.model, function (rs) {
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
            taikhoan: '',
            matkhau: '',
            email: '',
            hoten: '',
            quocgia: '',
            thanhpho: '',
            website: '',
            mota: '',
            ngaysinh: '',
            facebook: '',
            hinhdaidien: '',
            cover: '',
            gioitinh: ''

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
    $scope.submit = function () {
        // delete $scope.model['tentheloai'];
        //  $scope.model.theloai_id = $scope.valueTheLoai;
        $scope.model = $scope.data1;
        if ($scope.kt == 1) {
            dataservice.uploadHinhAnh(formData, function (rs) {
                rs = rs.data;
                $scope.model.object.hinhdaidien = rs;

                dataservice.editnguoidung($scope.model.object, function (rs) {
                    rs = rs.data;
                    $scope.EditItem = rs;

                });
            })
            $uibModalInstance.dismiss('cancel');
        } else {
            dataservice.editnguoidung($scope.model.object, function (rs) {
                rs = rs.data;
                $scope.EditItem = rs;

            });
            $uibModalInstance.dismiss('cancel');
        }
    }

    $scope.getTheFilesHinhAnh = function ($files) {
        //chọn nhiều ảnh
        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("file", $files[i]);
        //}    
        formData = new FormData();
        formData.append("File", $files[0]);
        //   $scope.link = $files[0].name;                      
        $scope.kt = 1;
    }
    


});