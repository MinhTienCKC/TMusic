var ctxfolderurl = "/views/admin/chuDe";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        createchude: function (data, callback) {
            $http.post('/Admin/ChuDe/CreateChuDe',data).then(callback);
        },
        deletechude: function (data, callback) {
            $http.post('/Admin/ChuDe/DeleteChuDe', data).then(callback);
        },
        editchude: function (data, callback) {
            $http.post('/Admin/ChuDe/EditChuDe', data).then(callback);
        },
        loadchude: function (callback) {
            $http.post('/Admin/ChuDe/LoadChuDe').then(callback);
          
        },
        //loadtheloai: function (callback) {
        //    $http.post('/Admin/ChuDe/LoadTheloai').then(callback);

        //},
       

        
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
  
    $scope.max = 100;
    $scope.dynamic = 0;
    var downloadbaihat = document.getElementById("btntext");
    $scope.text123 = function () {
        $scope.dynamic += 10;
    }   
        setTimeout(function () {
            downloadbaihat.click();
            setTimeout(function () {
                 downloadbaihat.click();
                setTimeout(function () {
                     downloadbaihat.click();
                    setTimeout(function () {
                        downloadbaihat.click();
                        setTimeout(function () {
                            downloadbaihat.click();
                            setTimeout(function () {
                                 downloadbaihat.click();
                                setTimeout(function () {
                                    downloadbaihat.click();
                                    setTimeout(function () {
                                        downloadbaihat.click();
                                        setTimeout(function () {
                                            downloadbaihat.click();
                                            setTimeout(function () {
                                                downloadbaihat.click();
                                            }, 1000);
                                        }, 1000);
                                    }, 1000);
                                }, 1000);
                            }, 1000);
                        }, 1000);
                    }, 1000);
                }, 1000);
            }, 1000);
        },1000);
  
   
    
    
    $scope.model = {
        id: '',
        chude_id: '',
        tenchude: '',
        linkhinhanh: ''
    }
    $scope.initData = function () {

        dataservice.loadchude( function (rs) {

            rs = rs.data;
            $scope.dataloadchude = rs;

            

            $scope.numberOfPages = function () {
                return Math.ceil($scope.dataloadchude.length / $scope.pageSize);
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
        dataservice.createchude($scope.model, function (rs) {
            rs = rs.data;
            $scope.data = rs;
            $scope.initData();
        });
      
    }
    var config = {
        apiKey: "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q",
        authDomain: "tfourmusic-1e3ff.firebaseapp.com",
        databaseURL: "tfourmusic-1e3ff-default-rtdb.firebaseio.com",
        projectId: "tfourmusic-1e3ff",
        storageBucket: "tfourmusic-1e3ff.appspot.com"


    };
    firebase.initializeApp(config);
    $scope.delete = function (key) {
     
        $scope.model.id = key;
        dataservice.deletechude($scope.model, function (rs) {
            rs = rs.data;
            $scope.deletedata = rs;
            $scope.initData();
        });

    }

});
app.controller('add', function ($rootScope, $scope, dataservice, $uibModalInstance) {

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
        tenchude: '',
        linkhinhanh: ''
    }
    var formData = new FormData();
    $scope.submit = function () {
       
        dataservice.uploadHinhAnh(formData, function (rs) {
            rs = rs.data;
            $scope.model.linkhinhanh = rs;

            dataservice.createchude($scope.model, function (rs) {
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
        tenchude: '',    
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

                dataservice.editchude($scope.model.object, function (rs) {
                    rs = rs.data;
                    $scope.EditItem = rs;

                });
                 })
            $uibModalInstance.dismiss('cancel');
        } else {
                         dataservice.editchude($scope.model.object, function (rs) {
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