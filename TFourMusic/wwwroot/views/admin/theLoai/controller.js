var ctxfolderurl = "/views/admin/theLoai";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        createtheloai: function (data, callback) {
            $http.post('/Admin/TheLoai/CreateTheLoai',data).then(callback);
        },
        deletetheloai: function (data, callback) {
            $http.post('/Admin/TheLoai/DeleteTheLoai', data).then(callback);
        },
        edittheloai: function (data, callback) {
            $http.post('/Admin/TheLoai/EditTheLoai', data).then(callback);
        },
        loadtheloai: function (callback) {
            $http.post('/Admin/TheLoai/LoadTheLoai').then(callback);
            //   $http({
            //    method: 'post',
            //    url: '/Admin/TheLoai/LoadTheLoai',
            //    headers: {
            //        Authorization: "Bearer " + "eyJhbGciOiJSUzI1NiIsImtpZCI6IjMwMjUxYWIxYTJmYzFkMzllNDMwMWNhYjc1OTZkNDQ5ZDgwNDI1ZjYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vbXVzaWN0dC05YWE1ZiIsImF1ZCI6Im11c2ljdHQtOWFhNWYiLCJhdXRoX3RpbWUiOjE2MjI1MzY2MzAsInVzZXJfaWQiOiJhRXJpY0h0NVVYVnpKYXBJVjdBbFc2QmR0WVAyIiwic3ViIjoiYUVyaWNIdDVVWFZ6SmFwSVY3QWxXNkJkdFlQMiIsImlhdCI6MTYyMjUzNjYzMCwiZXhwIjoxNjIyNTQwMjMwLCJlbWFpbCI6ImRhbmc2MDc4MEBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiZGFuZzYwNzgwQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.E2dYSxBeHFOFsXBax5ANtJrJ6xZZRyHRxSewI5i89pzYOq-E-JhRR2bm8XH-cOzoUdSSpgsRrqs3VHGfaQmvqSnS43PLa28gtEZPCFPMPJFrEG382WO46p7w8Gd4cYFzNeog-5VsbMmqWvyNyksagLcRZHXZiWYN7GesCZc4nMypYFfxcERBgDezrqT7YlE7gNLlmhOUlIamGbck-ZUL3-qVl8R_AYuhQinDL_tzKKKn7-wJtvfJePqBXBPTFF3nQBIBRXYQZJZNu48lJCEZ4Dq1vuvnvye0_v0YUCcLvqA_HSrMiT2oBwTSREZqF7PG6fyz4GDYoVO1Mi88dOo9rw"
            //    },

            //}).then(callback);
        },
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/TheLoai/GetLinkHinhAnh',
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
  

    //$scope.initData = function () {
    //    dataservice.loadtheloai(function (rs) {
            
    //        rs = rs.data;
    //        $scope.dataTheLoai = rs;
    //    });

    //};
    //$scope.initData();
    $scope.model = {
        id: '',
        tenTheLoai: '',
        
    }
    $scope.initData = function () {

        dataservice.loadtheloai(function (rs) {

            rs = rs.data;
            $scope.dataTheLoai = rs;

         

            $scope.numberOfPages = function () {
                return Math.ceil($scope.dataTheLoai.length / $scope.pageSize);
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
        dataservice.createtheloai($scope.model, function (rs) {
            rs = rs.data;
            $scope.data = rs;
            $scope.initData();
        });
      
    }
    $scope.delete = function (key) {
        $scope.model.id = key;
        dataservice.deletetheloai($scope.model, function (rs) {
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
        tentheloai: '',
        linkhinhanh: ''
    }
   
    var formData = new FormData();
    $scope.submit = function () {

        dataservice.uploadHinhAnh(formData, function (rs) {
            rs = rs.data;
            $scope.model.linkhinhanh = rs;

            dataservice.createtheloai($scope.model, function (rs) {
                rs = rs.data;
                $scope.data = rs;

            });
        })

        $uibModalInstance.dismiss('cancel');
    }

    $scope.getTheFilesHinhAnh = function ($files) {
        
        formData = new FormData();
        formData.append("File", $files[0]);                          
    }

});
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice,para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.data1 = para;
    $scope.model = {
        id: '',     
        tenTheLoai: ''
       
    }
    $scope.submit = function () {
        $scope.model = $scope.data1;
        dataservice.edittheloai($scope.model.object, function (rs) {
            rs = rs.data;
            $scope.EditItem = rs;

        });
        $uibModalInstance.dismiss('cancel');
    }
  
});