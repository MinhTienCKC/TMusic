﻿var ctxfolderurl = "/views/admin/baiHat";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        taoBaiHat: function (data, callback) {
            $http.post('/Admin/BaiHat/taoBaiHat',data).then(callback);
        },
        xoaBaiHat: function (data, callback) {
            $http.post('/Admin/BaiHat/xoaBaiHat', data).then(callback);
        },
        suaBaiHat: function (data, callback) {
            $http.post('/Admin/BaiHat/suaBaiHat', data).then(callback);
        },
        taiBaiHat: function (data, callback) {
            $http.post('/Admin/BaiHat/taiBaiHat', data).then(callback);
          
        },
        taiTheLoai: function (callback) {
            $http.post('/Admin/BaiHat/taiTheloai').then(callback);

        },
        taiDanhSachPhatTheLoai: function (data,callback) {
            $http.post('/Admin/BaiHat/taiDanhSachPhatTheLoai',data).then(callback);

        },
        taiChuDe: function (callback) {
            $http.post('/Admin/BaiHat/taiChuDe').then(callback);

        },
        //LoadBaiHat: function (callback) {
        //    //$http.post('/Admin/BaiHat/LoadBaiHat').then(callback);
        //    $http({
        //        method: 'post',
        //        url: '/Admin/BaiHat/LoadBaiHat',
        //        headers: {
        //            Authorization: "Bearer " + "eyJhbGciOiJSUzI1NiIsImtpZCI6IjMwMjUxYWIxYTJmYzFkMzllNDMwMWNhYjc1OTZkNDQ5ZDgwNDI1ZjYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vbXVzaWN0dC05YWE1ZiIsImF1ZCI6Im11c2ljdHQtOWFhNWYiLCJhdXRoX3RpbWUiOjE2MjI1MzY2MzAsInVzZXJfaWQiOiJhRXJpY0h0NVVYVnpKYXBJVjdBbFc2QmR0WVAyIiwic3ViIjoiYUVyaWNIdDVVWFZ6SmFwSVY3QWxXNkJkdFlQMiIsImlhdCI6MTYyMjUzNjYzMCwiZXhwIjoxNjIyNTQwMjMwLCJlbWFpbCI6ImRhbmc2MDc4MEBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsiZGFuZzYwNzgwQGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.E2dYSxBeHFOFsXBax5ANtJrJ6xZZRyHRxSewI5i89pzYOq-E-JhRR2bm8XH-cOzoUdSSpgsRrqs3VHGfaQmvqSnS43PLa28gtEZPCFPMPJFrEG382WO46p7w8Gd4cYFzNeog-5VsbMmqWvyNyksagLcRZHXZiWYN7GesCZc4nMypYFfxcERBgDezrqT7YlE7gNLlmhOUlIamGbck-ZUL3-qVl8R_AYuhQinDL_tzKKKn7-wJtvfJePqBXBPTFF3nQBIBRXYQZJZNu48lJCEZ4Dq1vuvnvye0_v0YUCcLvqA_HSrMiT2oBwTSREZqF7PG6fyz4GDYoVO1Mi88dOo9rw"
        //        },
               
        //    }).then(callback);
        //},

        uploadaudio: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/BaiHat/GetLink',
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
        },
        uploadHinhAnh: function (data, callback) {
            $http({
                method: 'post',
                url: '/Admin/BaiHat/GetLinkHinhAnh',
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
        .when('/add', {
            templateUrl: ctxfolderurl + '/add.html',
            controller: 'add'
        })
});
app.controller('T_Music', function () {

});

app.controller('index', function ($rootScope, $scope, dataservice, $uibModal, $filter) {
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
        nguoidung_id: 'admin',
        tenbaihat: '',
        mota: '',
        luottaixuong: '0',
        chedo: '0',
        luotthich: '0',
        loibaihat: '',
        luotnghe: '0',
        casi: '',
        theloai_id: '',
        danhsachphattheloai_id: '',
        chude_id: '',
        thoiluongbaihat: '',
        quangcao: '',
        link: '',
        linkhinhanh: ''
    }
    $scope.initData = function () {
  
        dataservice.taiBaiHat($scope.model, function (rs) {

            rs = rs.data;
            $scope.dataloadbaihat = rs;       

           
            $scope.numberOfPages = function () {
                return Math.ceil($scope.dataloadbaihat.length / $scope.pageSize);
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
            /*animation: true,*/
            
            //ariaLabelledBy: 'modal-title',
            //ariaDescribedBy: 'modal-body',
            templateUrl: ctxfolderurl + '/add.html',
            controller: 'add',
          //  backdrop: 'true',
            backdropClass: ".fade:not(.show)",
            backdropClass: ".modal-backdrop",
            backdropClass: ".col-lg-8",
            backdropClass: ".modal-content",
          /*  appendTo: document.getElementById("#main-baihat"),*/
            size: '10'
        });
        modalInstance.result.then(function () {
            $scope.initData();
        }, function () {
        });
       
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
            $scope.initData();
        }, function () {
        });
    };
   
    $scope.delete = function (key) {
        $scope.model.id = key;
        dataservice.xoaBaiHat($scope.model, function (rs) {
            rs = rs.data;
            $scope.deletedata = rs;
            $scope.initData();
        });

    }
   
 
});

app.controller('add', function ($rootScope, $scope, dataservice, $uibModalInstance) { 
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        id: '',
        nguoidung_id: 'admin',
        tenbaihat: '',
        mota: '',
        luottaixuong: (Math.floor(Math.random() * 100) + 1),   
        luotthich: (Math.floor(Math.random() * 100) + 1),
        loibaihat: '',
        casi: '',
        luotnghe: (Math.floor(Math.random() * 100) + 1),
        theloai_id: '',
        danhsachphattheloai_id: '',
        chude_id: '',
        thoiluongbaihat: '',
        quangcao:'',
        link: '',
        linkhinhanh: ''
    }
    $scope.text = {
        key:''
    }
   
   
    /*CKEDITOR.replace('editor11', {});  */
    $scope.initData = function () {
      
      
    //    element.style.color = 'red';
       
        //var textarea = document.body.appendChild(document.createElement('textarea'));
        //CKEDITOR.replace(textarea);
     /*   CKEDITOR.replace('editor11');*/
        dataservice.taiTheLoai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;


            $scope.valueTheLoai = rs[0].object.id;
            $scope.text.key = $scope.valueTheLoai;
            dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
                rs = rs.data;
                $scope.datataiDanhSachPhatTheLoai = rs;
                $scope.valueDanhSachPhatTheLoai = rs[0].object.id;
            });
           
        });
       
        dataservice.taiChuDe(function (rs) {
            rs = rs.data;
            $scope.dataloadchude = rs;
            $scope.valueChuDe = rs[0].object.id;

        });
    };
    $scope.changeTheLoai = function () {
        $scope.text.key = $scope.valueTheLoai
        dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
            rs = rs.data;
            $scope.datataiDanhSachPhatTheLoai = rs;
            $scope.valueDanhSachPhatTheLoai = rs[0].object.id;
        });

    };
    //$scope.changeChude = function () {


    //};
    $scope.initData();
    var duLieuNhac= new FormData();
    var duLieuHinh = new FormData();
    $scope.submit = function () {
        


        if (!$scope.addBaiHat.addTenBaiHat.$valid || !$scope.addBaiHat.addLinkBaiHat.$valid || !$scope.addBaiHat.addTenCaSi.$valid ) {
            alert("Vùng Lòng  Điền Đầy Đủ Thông Tin !!!");
        }
        else {

            $scope.model.theloai_id = $scope.valueTheLoai;
            $scope.model.danhsachphattheloai_id = $scope.valueDanhSachPhatTheLoai;
            $scope.model.chude_id = $scope.valueChuDe;
            $scope.model.tenbaihat = $scope.addTenBaiHat;
            $scope.model.casi = $scope.addTenCaSi;
            $scope.model1 = $scope.model;
            dataservice.uploadaudio(duLieuNhac, function (rs) {
                rs = rs.data;
                $scope.model.link = rs;
                dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                    rs = rs.data;
                    $scope.model.linkhinhanh = rs;
                    dataservice.taoBaiHat($scope.model, function (rs) {
                        rs = rs.data;
                        $scope.data = rs;
                    });
                });
            });


            $uibModalInstance.dismiss('cancel');  
        }
       
    }
    $scope.getTheFiles = function ($files) {
        $scope.addLinkBaiHat = "Đã Chọn Bài Hát";
        duLieuNhac = new FormData();
        duLieuNhac.append("File", $files[0]);  
    //    for (var i = 0; i < $files.length; i++) {
    //        formData.append("File", $files[i]);
    //    }     
       var objectUrl = URL.createObjectURL($files[0]);
        $("#audio").prop("src", objectUrl);
        $("#audio").on("canplaythrough", function (e) {
            var seconds = e.currentTarget.duration;
            let currentMinutes = Math.floor(seconds / 60);
            let currentSeconds = Math.floor(seconds - currentMinutes * 60);
            let durationMinutes = Math.floor(seconds / 60);
            let durationSeconds = Math.floor(seconds - durationMinutes * 60);

            // Adding a zero to the single digit time values
            if (currentSeconds < 10) { currentSeconds = "0" + currentSeconds; }
            if (durationSeconds < 10) { durationSeconds = "0" + durationSeconds; }
            if (currentMinutes < 10) { currentMinutes = "0" + currentMinutes; }
            if (durationMinutes < 10) { durationMinutes = "0" + durationMinutes; }


            var total_duration = durationMinutes + ":" + durationSeconds;
     
            $scope.model.thoiluongbaihat = total_duration;
            //var dc = moment.duration(secondsn, "seconds");
            //var time = "";
            //var hours = duration.hours();
            //if (hours > 0) { time = hours + ":"; }

            //time = time + duration.minutes() + ":" + duration.seconds();
            //time = time + duration.minutes() + ":" + duration.seconds();
         
        });
    }
    $scope.getTheFilesHinhAnh = function ($files) {
        duLieuHinh = new FormData();
        duLieuHinh.append("File1", $files[0]);     
        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("File", $files[i]);
        //}
             
    }
});
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    CKEDITOR.replace('okem');
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.data1 = para;
    //$scope.initData = function () {

    //    dataservice.editloadbaihat(para, function (rs) {
           
    //        rs = rs.data;
    //        $scope.EditItem = rs;
    //    });

    //}
    //$scope.initData();
    $scope.text = {
        key: ''
    };

    $scope.initData = function () {
        $scope.kt = 0;
        dataservice.taiTheLoai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;


         //   $scope.valueTheLoai = rs[0].object.id;
          //  $scope.text.key = $scope.valueTheLoai
            for (var i = 0; i < $scope.dataloadtheloai.length; i++) {
                if ($scope.data1.object.theloai_id == rs[i].object.id) {
                    $scope.valueTheLoai = rs[i].object.id;
                    break;
                }
            }      
            $scope.text.key = $scope.valueTheLoai;
            dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
                rs = rs.data;
                $scope.datataiDanhSachPhatTheLoai = rs;
             //   $scope.valueDanhSachPhatTheLoai = rs[0].object.id;

                for (var i = 0; i < $scope.datataiDanhSachPhatTheLoai.length; i++) {
                    if ($scope.data1.object.danhsachphattheloai_id == rs[i].object.id) {
                        $scope.valueDanhSachPhatTheLoai = rs[i].object.id;
                        break;
                    }
                }      

            });

        });

        dataservice.taiChuDe(function (rs) {
            rs = rs.data;
            $scope.dataloadchude = rs;
        //    $scope.valueChuDe = rs[0].object.id;
            for (var i = 0; i < $scope.dataloadchude.length; i++) {
                if ($scope.data1.object.chude_id == rs[i].object.id) {
                    $scope.valueChuDe = rs[i].object.id;
                    break;
                }
            }      
        });
    };
    $scope.changeTheLoai = function () {
        $scope.text.key = $scope.valueTheLoai
        dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
            rs = rs.data;
            $scope.datataiDanhSachPhatTheLoai = rs;
            $scope.valueDanhSachPhatTheLoai = rs[0].object.id;
        });

    };
    //$scope.changeChude = function () {


    //};
    $scope.initData();
    $scope.model = {
        id: '',
        nguoidung_id: '',
        tenbaihat: '',
        mota: '',
        luottaixuong: '',
        thoiluongbaihat: '',
        luotthich: '',
        loibaihat: '',
        casi: '',
        luotnghe: '',
        theloai_id: '',
        chude_id: '',
        link: '',
        linkhinhanh: ''
    }
    var duLieuHinh = new FormData();
   

   
    $scope.submit = function () {
        $scope.model = $scope.data1;
        $scope.model.object.theloai_id = $scope.valueTheLoai;
        $scope.model.object.chude_id = $scope.valueChuDe;
        $scope.model.object.danhsachphattheloai_id = $scope.valueDanhSachPhatTheLoai;
        //dataservice.editbaihat($scope.model.object, function (rs) {
        //    rs = rs.data;
        //    $scope.EditItem = rs;

        //});
        $scope.modelok = $scope.model;
   
        if ($scope.kt == 1) {
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.model.object.linkhinhanh = rs;

                dataservice.suaBaiHat($scope.model.object, function (rs) {
                    rs = rs.data;
                    $scope.EditItem = rs;

                });
            })
            $uibModalInstance.dismiss('cancel');
        } else {
            dataservice.suaBaiHat($scope.model.object, function (rs) {
                rs = rs.data;
                $scope.EditItem = rs;

            });
            $uibModalInstance.dismiss('cancel');
        }

    
    }
    $scope.getTheFilesHinhAnh = function ($files) {
        duLieuHinh = new FormData();
        duLieuHinh.append("File1", $files[0]);
        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("File", $files[i]);
        //}
        $scope.kt = 1;

    }
    //function validationSelect(data) {
    //    var mess = { Status: false, Title: "" }
    //    if (data.Name == "") {
    //        $scope.errorName = true;
    //        mess.Status = true;
    //    }
    //    else
    //        $scope.errorName = false;
    //    if (data.Subject == "") {
    //        $scope.errorSubject = true;
    //        mess.Status = true;
    //    }
    //    else
    //        $scope.errorSubject = false;

    //    return mess;
    //};


});