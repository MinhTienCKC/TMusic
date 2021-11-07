var ctxfolderurl = "/views/admin/baiHat";

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
        thayDoiCheDoBaiHat: function (data, callback) {
            $http.post('/Admin/BaiHat/thayDoiCheDoBaiHat', data).then(callback);
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
        suaLinkHinhAnhBaiHat: function (data, callback) {
            $http.post('/Admin/BaiHat/suaLinkHinhAnhBaiHat', data).then(callback);

        },
        taiDanhSachBaiHatDaXoa_ThungRac: function (callback) {
            $http.post('/Admin/BaiHat/taiDanhSachBaiHatDaXoa_ThungRac').then(callback);

        },
        khoiPhucBaiHat_ThungRac: function (data, callback) {
            $http.post('/Admin/BaiHat/khoiPhucBaiHat_ThungRac', data).then(callback);
        },
        xoaBaiHatVinhVien_ThungRac: function (data, callback) {
            $http.post('/Admin/BaiHat/xoaBaiHatVinhVien_ThungRac', data).then(callback);

        },
        taiChiTietNguoiDungQuaUID: function (data, callback) {
            $http.post('/Admin/BaiHat/taiChiTietNguoiDungQuaUID', data).then(callback);

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
app.directive('validatekitudacbiet123', function () {
    function link(scope, elem, attrs, ngModel) {
        ngModel.$parsers.push(function (viewValue) {
            var reg = /^[^`~!@#$%\^&*()_+={}|[\]\\:';"<>?,./]*$/;
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
app.directive("whenScrolled", function () {
    return {

        restrict: 'A',
        link: function (scope, elem, attrs) {

            // we get a list of elements of size 1 and need the first element
            raw = elem[0];

            // we load more elements when scrolled past a limit
            elem.bind("scroll", function () {
                if (raw.scrollTop + raw.offsetHeight + 5 >= raw.scrollHeight) {
                    scope.loading = true;

                    // we can give any function which loads more elements into the list
                    scope.$apply(attrs.whenScrolled);
                }
            });
        }
    }
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
        .when('/add', {
            templateUrl: ctxfolderurl + '/add.html',
            controller: 'add'
        })
});
app.controller('T_Music', function () {
   
});

app.controller('index', function ($rootScope, $scope, dataservice, $uibModal, $filter, $interval) {


    $scope.cheDo = function (data) {
        dataservice.thayDoiCheDoBaiHat(data, function (rs) {
            rs = rs.data;

            if (rs == true) {
                alertify.success("Thay đỗi chế độ thành công.");
            }
            else {
                alertify.success("Thay đỗi chế độ thất bại .");
            }
        });
    }
    alertify.set('notifier', 'position', 'bottom-left');
    var tick = function () {
        $scope.clock = Date.now();
    }
    tick();
    $interval(tick, 1000);
    $(".nav-noidung").addClass("active");
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }
   // tippy('[data-tippy-content]');
    tippy('td', {
        content: 'Global content',
        trigger: 'click',
    });
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
            $scope.taiBaiHat = rs;       

           
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiBaiHat.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        });
    };
    function chuyenDoi(object) {

        return Math.ceil(object.length / 5);;
    }
    $scope.layBaiHatAdmin = function (object) {
        if (chuyenDoi(object) < 8) {
            $scope.numberOfPages = function () {
                return chuyenDoi(object);
            }
            $scope.soLuong = chuyenDoi(object);
            $scope.soTrang = chuyenDoi(object)
            $scope.size = 0;
            $scope.currentPage = 0;
        }
        else {
            $scope.numberOfPages = function () {
                return chuyenDoi(object);
            }
            $scope.soLuong = 8;
            $scope.soTrang = chuyenDoi(object)
            $scope.size = 0;
            $scope.currentPage = 0;
            //  $scope.numberOfPages() = chuyenDoi(object);
        }
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
  
    


    $scope.taoBaiHat_index = function () {
      
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
            
        }, function () {
                setTimeout(function () {
                    $scope.initData();
                }, 2000);
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
            //setTimeout(function () {
               
            //}, 1500);
        }, function () {
        });
    };
    $scope.thungrac = function () {

        var modalInstance = $uibModal.open({
            /*animation: true,*/

            //ariaLabelledBy: 'modal-title',
            //ariaDescribedBy: 'modal-body',
            templateUrl: ctxfolderurl + '/thungrac.html',
            controller: 'thungrac',
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
            //setTimeout(function () {
               
            //}, 1500);
        }, function () {
               
        });

    };
    $scope.xemChiTietBaiHat = function (key) {

        var modalInstance = $uibModal.open({
            /*animation: true,*/

            //ariaLabelledBy: 'modal-title',
            //ariaDescribedBy: 'modal-body',
            templateUrl: ctxfolderurl + '/chitietbaihat.html',
            controller: 'chitietbaihat',
            //  backdrop: 'true',
            backdropClass: ".fade:not(.show)",
            backdropClass: ".modal-backdrop",
            backdropClass: ".col-lg-8",
            backdropClass: ".modal-content",
      
            /*  appendTo: document.getElementById("#main-baihat"),*/
            size: '10',
            resolve: {
                para: function () {
                    return key;
                }
            }
        });
        modalInstance.result.then(function () {
            $scope.initData();
            //setTimeout(function () {

            //}, 1500);
        }, function () {

        });
    }
    $scope.xoaBaiHat = function (data,vitribaihat) {
       
        dataservice.xoaBaiHat(data, function (rs) {
            rs = rs.data;
            $scope.deletedata = rs;
            $scope.taiBaiHat.splice(vitribaihat, 1);
            alertify.success("Xóa thành công");
           //// $scope.initData();
        });

    }
    var duLieuHinh = new FormData();

    // 02/08 suữa ảnh cá nhân
    $scope.suaHinhAnhBaiHat = function ($files,data) {
        $scope.BienTam = {
            linkhinhanhmoi: '',
            linkhinhanhcu: '',
            baihat_id: '',
            nguoidung_id:''
        }
        $scope.modelbaihatcs = data;
    
        if ($files[0].type == "image/png" || $files[0].type == "image/jpeg") {
            duLieuHinh = new FormData();
            duLieuHinh.append("File1", $files[0]);           
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.BienTam.linkhinhanhmoi = rs;
                $scope.BienTam.linkhinhanhcu = data.linkhinhanh;
                $scope.BienTam.baihat_id = data.id;
                $scope.BienTam.nguoidung_id = data.nguoidung_id;
                dataservice.suaLinkHinhAnhBaiHat($scope.BienTam, function (rs) {
                    rs = rs.data;
                    $scope.modelbaihatcs.linkhinhanh = $scope.BienTam.linkhinhanhmoi;
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
    
      //  CKEDITOR.replace('ckloibaihat');
    //    element.style.color = 'red';
       
        //var textarea = document.body.appendChild(document.createElement('textarea'));
        //CKEDITOR.replace(textarea);
     /*   CKEDITOR.replace('ckloibaihat');*/
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
    $scope.dinhDangBaiHat = "audio/mpeg";
    $scope.dinhDangHinhAnh = "image/";
    $scope.submit = function () {
        
        if ($scope.dinhDangHinhAnh != "image/"
            || $scope.dinhDangBaiHat != "audio/mpeg"
            || !$scope.addBaiHat.addTenBaiHat.$valid
            || !$scope.addBaiHat.addLinkBaiHat.$valid
            || !$scope.addBaiHat.addLinkHinhAnh.$valid
            || !$scope.addBaiHat.addTenCaSi.$valid) {
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
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
        var mb1 = 1048576;
        if ($files[0].size > (mb1 * 10)) {
            alert("Hãy chọn file mp3 < 10mb !!!");
            return;
        }
        $scope.addLinkBaiHat = "Đã Chọn Bài Hát";
        duLieuNhac = new FormData();
        duLieuNhac.append("File", $files[0]);  
    //    for (var i = 0; i < $files.length; i++) {
    //        formData.append("File", $files[i]);
    //    }     
        $scope.dinhDangBaiHat = $files[0].type;
        var downloadbaihat = document.getElementById("btntext");
        downloadbaihat.click();
       
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
        $scope.addLinkHinhAnh = "Đã Chọn Hình Ảnh";
        duLieuHinh = new FormData();
        duLieuHinh.append("File1", $files[0]);     
        if ($files[0].type == "image/png" || $files[0].type == "image/jpg" || $files[0].type == "image/jpeg") {
            $scope.dinhDangHinhAnh = "image/"
        }
        else {
            $scope.dinhDangHinhAnh = $files[0].type;
        }
        var downloadbaihat = document.getElementById("btntext");
        downloadbaihat.click();        
    }
});
app.controller('thungrac', function ($rootScope, $scope, dataservice, $uibModalInstance) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    
    $scope.text = {
        key: ''
    }

    
    $scope.initData = function () {
        dataservice.taiDanhSachBaiHatDaXoa_ThungRac( function (rs) {
            rs = rs.data;
            $scope.taiDanhSachBaiHatDaXoa_ThungRac = rs;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiDanhSachBaiHatDaXoa_ThungRac.length / $scope.pageSize);
            }
            if ($scope.numberOfPages() < 8) {
                $scope.soLuong = $scope.numberOfPages();
            }
        });

    }
    $scope.initData();
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };

    $scope.xoaBaiHat = function (data, vitribaihat) {

        
        dataservice.xoaBaiHatVinhVien_ThungRac(data, function (rs) {
            rs = rs.data;
            if (rs == true) {
                alertify.success("Xóa Thành Công");
                $scope.taiDanhSachBaiHatDaXoa_ThungRac.splice(vitribaihat, 1);
            }
            else {
                alertify.success("Xóa Thất Bại");
            }

        });
    };
    alertify.set('notifier', 'position', 'bottom-left');
   
     
    $scope.khoiPhucBaiHat = function (data, vitribaihat) {       
        dataservice.khoiPhucBaiHat_ThungRac(data, function (rs) {
            rs = rs.data;

            if (rs == true) {
                alertify.success("Khôi Phục Thành Công");
                $scope.taiDanhSachBaiHatDaXoa_ThungRac.splice(vitribaihat, 1);
            }
            else {
                alertify.success("Khôi Phục Thất Bại");
            }
               
            $scope.initData();
        });
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

    

});
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.modelbaihat = para;
    $scope.text = {
        key: ''
    };
    $scope.initData = function () {
        $scope.kt = 0;
        dataservice.taiTheLoai(function (rs) {
            rs = rs.data;
            $scope.dataloadtheloai = rs;
        
            for (var i = 0; i < $scope.dataloadtheloai.length; i++) {
                if ($scope.modelbaihat.theloai_id == rs[i].object.id) {
                    $scope.valueTheLoai = rs[i].object.id;
                    break;
                }
            }      
            $scope.text.key = $scope.valueTheLoai;
            dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
                rs = rs.data;
                $scope.datataiDanhSachPhatTheLoai = rs;
                for (var i = 0; i < $scope.datataiDanhSachPhatTheLoai.length; i++) {
                    if ($scope.modelbaihat.danhsachphattheloai_id == rs[i].object.id) {
                        $scope.valueDanhSachPhatTheLoai = rs[i].object.id;
                        break;
                    }
                }      
            });
        });
        dataservice.taiChuDe(function (rs) {
            rs = rs.data;
            $scope.dataloadchude = rs;
            for (var i = 0; i < $scope.dataloadchude.length; i++) {
                if ($scope.modelbaihat.chude_id == rs[i].object.id) {
                    $scope.valueChuDe = rs[i].object.id;
                    break;
                }
            }      
        });
        $scope.editTenBaiHat = $scope.modelbaihat.tenbaihat;
        $scope.editTenCaSi = $scope.modelbaihat.casi;
    };
    $scope.changeTheLoai = function () {
        $scope.text.key = $scope.valueTheLoai
        dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
            rs = rs.data;
            $scope.datataiDanhSachPhatTheLoai = rs;
            $scope.valueDanhSachPhatTheLoai = rs[0].object.id;
        });

    };

    $scope.initData();
    
    $scope.submit = function () {
        if ( $scope.valueDanhSachPhatTheLoai == null
            ||
            $scope.valueTheLoai == null
            ||
            $scope.valueChuDe == null
            ||
           !$scope.editBaiHat.editTenBaiHat.$valid         
            || !$scope.editBaiHat.editTenCaSi.$valid ) {
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
            $scope.modelbaihat.tenbaihat =  $scope.editTenBaiHat ;
            $scope.modelbaihat.casi = $scope.editTenCaSi;
            $scope.modelbaihat.theloai_id = $scope.valueTheLoai;
            $scope.modelbaihat.danhsachphattheloai_id = $scope.valueDanhSachPhatTheLoai;
            $scope.modelbaihat.chude_id = $scope.valueChuDe;
            $scope.modelbaihat.tenbaihat = $scope.editTenBaiHat;
            dataservice.suaBaiHat($scope.modelbaihat,function (rs) {
               rs = rs.data;
                $scope.suaBaiHat = rs;

            });

            $uibModalInstance.dismiss('cancel');
        }   
    }
});
app.controller('chitietbaihat', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.modelbaihat = para;

    $scope.text = {
        key: ''
    };
    $scope.initData = function () {
        if (para.nguoidung_id !="admin") {
            dataservice.taiChiTietNguoiDungQuaUID(para, function (rs) {
                rs = rs.data
                $scope.taiChiTietNguoiDungQuaUID = rs;
            })
        }
       
    };
   

    $scope.initData();

   
});