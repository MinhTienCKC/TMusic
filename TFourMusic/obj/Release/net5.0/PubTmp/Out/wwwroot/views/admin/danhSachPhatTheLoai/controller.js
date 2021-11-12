var ctxfolderurl = "/views/admin/danhSachPhatTheLoai";

var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute"]);
//var app = angular.module('T_Music', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"])
app.factory('dataservice', function ($http) {
    return {
        taoDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Admin/danhSachPhatTheLoai/taoDanhSachPhatTheLoai',data).then(callback);
        },
        xoaDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/XoaDanhSachPhatTheLoai', data).then(callback);
        },
        suaDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/suaDanhSachPhatTheLoai', data).then(callback);
        },
        taiDanhSachPhatTheLoai: function (callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/taiDanhSachPhatTheLoai').then(callback);
          
        },
        taiTheLoai: function (callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/taiTheLoai').then(callback);

        },
        taiDanhSachBaiHatDeThem_DSPTL: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/taiDanhSachBaiHatDeThem_DSPTL?key=' + data).then(callback);
        }, 
      
        themBaiHatVaoDSPTL_DSPTL: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/themBaiHatVaoDSPTL_DSPTL', data).then(callback);
        },
        taiDanhSachBaiHatDaThem_DSPTL: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/taiDanhSachBaiHatDaThem_DSPTL?key=' + data).then(callback);
        }, 
        taiDanhSachBaiHatMacDinh_DSPTL: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/taiDanhSachBaiHatMacDinh_DSPTL?key=' + data).then(callback);
        }, 
        xoaBaiHatkhoiDSPTL_DSPTL: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/xoaBaiHatkhoiDSPTL_DSPTL',  data).then(callback);
        }, 
        suaLinkHinhAnhDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/suaLinkHinhAnhDanhSachPhatTheLoai', data).then(callback);
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

    $(".nav-noidung").addClass("active");
    $scope.model = {
        id: '',
        theloai_id: '',
        tendanhsachphattheloai: '',
        mota: '',
        linkhinhanh: ''
    }
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.text = {
        key: ''
    }
    $scope.rong = '';
    $scope.initData = function () {
        dataservice.taiTheLoai(function (rs) {


            rs = rs.data;
            $scope.taiTheLoai = rs;
            $scope.valueTheLoai = $scope.rong;

            //  $scope.text.key = $scope.valueTheLoai;

        });
        dataservice.taiDanhSachPhatTheLoai(function (rs) {

            rs = rs.data;
            $scope.taiDanhSachPhatTheLoai = rs;

            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiDanhSachPhatTheLoai.length / $scope.pageSize);
            }
            $scope.soTrang = $scope.numberOfPages();
        });
        $scope.currentPage = 0;
        $scope.pageSize = 5;
        $scope.size = 0;
        $scope.soLuong = 8;
    };

    $scope.initData();
    //chuyen doi độ dài thành số trang
    function chuyenDoi(object) {

        return Math.ceil(object.length / 5);;
    }
    $scope.changeTheLoai = function (object) {
        //alert(ok.length);
        $scope.duLieuTheoTheLoai = object;
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
   // renge trã về kiểu mãng để lập dữ liệu truyền vào là kiểu số
    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.phanTrang = function (data) {
        $scope.currentPage = data;
    };

    $scope.Truoc = function () {
        if ($scope.soTrang < 8) {
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
        if ($scope.soTrang < 8) {
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
                if ($scope.size + $scope.soLuong == $scope.bienTam2) {

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
    $scope.themBaiHat = function (key) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/thembaihat.html',
            controller: 'thembaihat',
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
               
                setTimeout(function () {
                    $scope.initData();
                }, 2000);
        });
    }
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.chitietdsptheloai = function (key) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolderurl + '/chitietdsptheloai.html',
            controller: 'chitietdsptheloai',
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
    $scope.xoaDanhSachPhatTheLoai = function (key,vitridanhsachphattheloai) {
        dataservice.xoaDanhSachPhatTheLoai(key, function (rs) {
            rs = rs.data;     
            if (rs == true) {
                alertify.success("Xóa Thành Công");
                $scope.initData();
                
            }
            else {
                alertify.success("Xóa Thất Bại");
            }
        });

    }
    $scope.suaHinhAnhDanhSachPhatTheLoai = function ($files, data) {
        $scope.BienTam = {
            linkhinhanhmoi: '',
            linkhinhanhcu: '',
            danhsachphattheloai_id: '',
            theloai_id: ''
        }
        $scope.modelDanhSachPhatTheLoaics = data;

        if ($files[0].type == "image/png" || $files[0].type == "image/jpeg") {
            duLieuHinh = new FormData();
            duLieuHinh.append("File1", $files[0]);
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.BienTam.linkhinhanhmoi = rs;
                $scope.BienTam.linkhinhanhcu = data.linkhinhanh;
                $scope.BienTam.danhsachphattheloai_id = data.id;
                $scope.BienTam.theloai_id = data.theloai_id;
                dataservice.suaLinkHinhAnhDanhSachPhatTheLoai($scope.BienTam, function (rs) {
                    rs = rs.data;
                    $scope.modelDanhSachPhatTheLoaics.linkhinhanh = $scope.BienTam.linkhinhanhmoi;
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
app.controller('thembaihat', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.ok = function () {
        $uibModalInstance.close();
    };
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.duLieuDSPTL = para;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.modelChiTietDSPTL = {
        baihat_id: '',
        danhsachphattheloai_id: $scope.duLieuDSPTL.id,
        id: ''
    }
    $scope.text = {

        key: '',
        uid: ''

    }
    $scope.initData = function () {
        dataservice.taiDanhSachBaiHatDeThem_DSPTL($scope.duLieuDSPTL.id, function (rs) {
            rs = rs.data;
            $scope.taiDanhSachBaiHatDeThem_DSPTL = rs;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.taiDanhSachBaiHatDeThem_DSPTL.length / $scope.pageSize);
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

    $scope.themBaiHatVaoDSPTL = function (data,vitribaihat) {
        $scope.text.key = data.id;
        $scope.text.uid = $scope.duLieuDSPTL.id;
        $scope.modelChiTietDSPTL.baihat_id = data.id;
        $scope.modelChiTietDSPTL.danhsachphattheloai_id = $scope.duLieuDSPTL.id;
      //  alert($scope.text.key + " " + $scope.text.uib + " " + vitribaihat);
        
        dataservice.themBaiHatVaoDSPTL_DSPTL($scope.modelChiTietDSPTL, function (rs) {
            rs = rs.data;
            if (rs == true) {
                alertify.success("Đã thêm bài hát vào danh sách phát thể loại.");
                $scope.initData();
                $scope.taiDanhSachBaiHatDeThem_DSPTL.splice(vitribaihat, 1);
            }
            else {
                alertify.success("Thêm bài hát thất bại.");
            }
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
app.controller('chitietdsptheloai', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.duLieuDSPTL = para;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.model = {
        id: '',
        tendanhsachphat: '',
        mota: ''
    }
    $scope.text = {

        key: '',
        uid: ''

    }
    $scope.tabactive = 0;
    function chuyenDoi(object) {

        return Math.ceil(object.length / 5);;
    }
    $scope.tabActiveClick = function (object,vitri) {
        $scope.tabactive = vitri;
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
    }
    $scope.initData = function () {
       
        dataservice.taiDanhSachBaiHatDaThem_DSPTL($scope.duLieuDSPTL.id, function (rs) {
            rs = rs.data;
            $scope.taiDanhSachBaiHatDaThem_DSPTL = rs; 
            
            dataservice.taiDanhSachBaiHatMacDinh_DSPTL($scope.duLieuDSPTL.id, function (rs) {
                rs = rs.data;
                $scope.taiDanhSachBaiHatMacDinh_DSPTL = rs;
                $scope.taiDanhSachBaiHatTatCa_DSPTL = $scope.taiDanhSachBaiHatMacDinh_DSPTL;            
                $scope.taiDanhSachBaiHatTatCa_DSPTL = $scope.taiDanhSachBaiHatTatCa_DSPTL.concat($scope.taiDanhSachBaiHatDaThem_DSPTL);
                $scope.numberOfPages = function () {
                    return Math.ceil($scope.taiDanhSachBaiHatTatCa_DSPTL.length / $scope.pageSize);
                }
                if ($scope.numberOfPages() < 8) {
                    $scope.soLuong = $scope.numberOfPages();
                }
                // sotrang là tổng số trang khi kia cho pagesize mỗi trang chứa 5 dòng dữ liệu ví dụ data có độ dài là 20
                // vậy mỗi trang có 5 dòng dữ liệu tương ứng với 4 trang 20/5
                $scope.soTrang = $scope.numberOfPages();
            });
            $scope.tabactive = 0;
        });
    }
    $scope.initData();
    $scope.range = function (n) {
        return new Array(n);
    };
    // hiện dữ liệu với số trang khi click
    $scope.phanTrang = function (data) {

        $scope.currentPage = data;
    };
    //$scope.model = {
    //    baihat_id: '',
    //    danhsachphattheloai_id: $scope.duLieuDSPTL.id,
    //    id:''
    //}
    $scope.xoaBaiHatKhoiDSPTL = function (data, vitribaihat) {
        $scope.text.key = data.id;
        $scope.text.uid = $scope.duLieuDSPTL.id;
        //$scope.model.baihat_id = data.id;
     //   alert($scope.text.key + " " + $scope.text.uib + " " + vitribaihat);
        
        dataservice.xoaBaiHatkhoiDSPTL_DSPTL($scope.text, function (rs) {
            rs = rs.data;        
            if (rs == true) {
                $scope.taiDanhSachBaiHatDaThem_DSPTL.splice(vitribaihat, 1);
                alertify.success("Xóa Thành Công");
                $scope.initData();

            }
            else {
                alertify.success("Xóa Thất Bại");
            }
        });
    };
    // currentPage số trang hiện tại 0 là trang đầu tiên
    $scope.currentPage = 0;
    // giới hạn số dòng dữ liều
    $scope.pageSize = 5;
    // size là số tang giam với $index khi click truoc hoac sau nút chuyển trang
    $scope.size = 0;
     // là số lượng giợi hạn của nút phân trang  1 2 3  =< 8 nút  
    $scope.soLuong = 8;

    $scope.Truoc = function () {
        if ($scope.soTrang < 8) {
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
        if ($scope.soTrang < 8) {
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
                if ($scope.size + $scope.soLuong == $scope.bienTam2) {

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

});
app.controller('add', function ($rootScope, $scope, dataservice,$uibModalInstance) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');   
    };
    $scope.initData = function () {   
        dataservice.taiTheLoai(function (rs) {
            rs = rs.data;
            $scope.taiTheLoai = rs;        
            $scope.valueTheLoai = rs[0].id;         
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
    // var formData = new FormData();
    var duLieuHinh = new FormData();
    $scope.dinhDangHinhAnh = "image/";
    $scope.submit = function () {
        $("#loading_main").css("display", "block");
        if ($scope.dinhDangHinhAnh != "image/"       
            || $scope.valueTheLoai == null
            || !$scope.addDanhSachPhatTheLoai.addTenDanhSachPhatTheLoai.$valid
            || !$scope.addDanhSachPhatTheLoai.addLinkHinhAnh.$valid
        ) {
            $("#loading_main").css("display", "none");
            alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
        }
        else {
            $scope.model.theloai_id = $scope.valueTheLoai;
            $scope.model.tendanhsachphattheloai = $scope.addTenDanhSachPhatTheLoai;
            $scope.model1 = $scope.model;
            dataservice.uploadHinhAnh(duLieuHinh, function (rs) {
                rs = rs.data;
                $scope.model.linkhinhanh = rs;
                dataservice.taoDanhSachPhatTheLoai($scope.model, function (rs) {
                    rs = rs.data;
                   
                    if (rs == true) {
                        alertify.success("Tạo danh sách phát thể loại thành công.");
                        
                    }
                    else {
                        alertify.success("Tạo danh sách phát thể loại thất bại.");
                    }
                    $("#loading_main").css("display", "none");
                    $uibModalInstance.dismiss('cancel');
                });
            })
           
          
        }     
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
app.controller('edit', function ($rootScope, $scope, $uibModalInstance, dataservice,para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.modelDanhSachPhatTheLoai = para;
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
        $scope.editTenDanhSachPhatTheLoai = $scope.modelDanhSachPhatTheLoai.tendanhsachphattheloai;
        //dataservice.taiTheLoai(function (rs) {

        //    rs = rs.data;
        //    $scope.taiTheLoai = rs;
                
        //    for (var i = 0; i < $scope.taiTheLoai.length; i++) {
        //        if ($scope.modelDanhSachPhatTheLoai.theloai_id == rs[i].id) {
        //            $scope.valueTheLoai = rs[i].id;
        //            break;
        //        }
        //    }      

        //});
    
    };
    $scope.initData();


   
                              
    $scope.submit = function () {
        $("#loading_main").css("display", "block");
            if (
                 !$scope.editDanhSachPhatTheLoai.editTenDanhSachPhatTheLoai.$valid
                
            ) {
                $("#loading_main").css("display", "none");
                alert("Vùng Lòng  Điền Đầy Đủ Và Kiểm Tra Thông Tin !!!");
            }
            else {
              //  $scope.modelDanhSachPhatTheLoai.theloai_id = $scope.valueTheLoai;
                $scope.modelDanhSachPhatTheLoai.tendanhsachphattheloai = $scope.editTenDanhSachPhatTheLoai;

                dataservice.suaDanhSachPhatTheLoai($scope.modelDanhSachPhatTheLoai, function (rs) {
                    rs = rs.data;
                    $("#loading_main").css("display", "none");
                    $uibModalInstance.dismiss('cancel');
                })
      
            }
        } 
                                             
    
    
   


});