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
        taiDanhSachPhatTheLoai: function (callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/TaiDanhSachPhatTheLoai').then(callback);
          
        },
        loadtheloai: function (callback) {
            $http.post('/Admin/DanhSachPhatTheLoai/LoadTheloai').then(callback);

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
    $scope.rong = '';
    $scope.initData = function () {
        dataservice.loadtheloai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;
            $scope.valueTheLoai = $scope.rong;

            //  $scope.text.key = $scope.valueTheLoai;

        });
        dataservice.taiDanhSachPhatTheLoai(function (rs) {

            rs = rs.data;
            $scope.taiDanhSachPhatTheLoai = rs;

            $scope.q = '';

            //$scope.getData = function () {

            //    return $filter('filter')($scope.taiDanhSachPhatTheLoai, $scope.q)

            //}

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
        });
    }
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
app.controller('thembaihat', function ($rootScope, $scope, $uibModalInstance, dataservice, para) {
    $scope.ok = function () {
        $uibModalInstance.close();
    };
    $scope.duLieuDSPTL = para;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {

        id: '',
        tendanhsachphat: '',
        mota: ''

    }
    $scope.text = {

        key: '',
        uid: ''

    }
    $scope.initData = function () {
        dataservice.taiDanhSachBaiHatDeThem_DSPTL($scope.duLieuDSPTL.object.id, function (rs) {
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
        $scope.text.key = data.object.id;
        $scope.text.uid = $scope.duLieuDSPTL.object.id;
       
      //  alert($scope.text.key + " " + $scope.text.uib + " " + vitribaihat);
        $scope.taiDanhSachBaiHatDeThem_DSPTL.splice(vitribaihat, 1);
        dataservice.themBaiHatVaoDSPTL_DSPTL($scope.text, function (rs) {
            rs = rs.data;
          
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
    $scope.ok = function () {
        $uibModalInstance.close();
    };
    $scope.duLieuDSPTL = para;
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
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
       
        dataservice.taiDanhSachBaiHatDaThem_DSPTL($scope.duLieuDSPTL.object.id, function (rs) {
            rs = rs.data;
            $scope.taiDanhSachBaiHatDaThem_DSPTL = rs;
            //$scope.numberOfPages = function () {
            //    return Math.ceil($scope.taiDanhSachBaiHatDaThem_DSPTL / $scope.pageSize);
            //}
            //if ($scope.numberOfPages() < 8) {
            //    $scope.soLuong = $scope.numberOfPages();
            //}
            dataservice.taiDanhSachBaiHatMacDinh_DSPTL($scope.duLieuDSPTL.object.id, function (rs) {
                rs = rs.data;
                $scope.taiDanhSachBaiHatMacDinh_DSPTL = rs;
                $scope.taiDanhSachBaiHatTatCa_DSPTL = $scope.taiDanhSachBaiHatMacDinh_DSPTL;
                //$scope.numberOfPages = function () {
                //    return Math.ceil($scope.taiDanhSachBaiHatDeThem_DSPTL / $scope.pageSize);
                //}
                //if ($scope.numberOfPages() < 8) {
                //    $scope.soLuong = $scope.numberOfPages();
                //}
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

    $scope.xoaBaiHatKhoiDSPTL = function (data, vitribaihat) {
        $scope.text.key = data.object.id;
        $scope.text.uid = $scope.duLieuDSPTL.object.id;

     //   alert($scope.text.key + " " + $scope.text.uib + " " + vitribaihat);
        $scope.taiDanhSachBaiHatDaThem_DSPTL.splice(vitribaihat, 1);
        dataservice.xoaBaiHatkhoiDSPTL_DSPTL($scope.text, function (rs) {
            rs = rs.data;
           
            $scope.initData();
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