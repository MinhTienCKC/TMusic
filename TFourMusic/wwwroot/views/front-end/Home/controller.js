var ctxfolderurl = "/views/front-end/Home";

var app = angular.module('App_ESEIM', ['ui.bootstrap', 'ngRoute', 'slick', 'ngCookies', 'ngAnimate']);

app.config(function ($routeProvider, $locationProvider) {
    $locationProvider.hashPrefix('');

    $routeProvider
        .when('/', {
            feature: 'index',
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
        .when('/zingchart', {
            feature: 'zingchart',
            templateUrl: ctxfolderurl + '/zingchart.html',
            controller: 'zingchart'
        })
        .when('/canhan', {
            feature: 'canhan',
            templateUrl: ctxfolderurl + '/canhan.html',
            controller: 'canhan'
        })
        .when('/theodoi', {
            feature: 'theodoi',
            templateUrl: ctxfolderurl + '/theodoi.html',
            controller: 'theodoi'
        }).when('/nhacmoi', {
            feature: 'nhacmoi',
            templateUrl: ctxfolderurl + '/nhacmoi.html',
            controller: 'nhacmoi'
        })
        .when('/theloai', {
            templateUrl: ctxfolderurl + '/theloai.html',
            controller: 'theloai'
        })
        .when('/chitiettheloai/:id', {
            templateUrl: ctxfolderurl + '/chitiettheloai.html',
            controller: 'chitiettheloai'
        })
        .when('/danhsachphat/:id', {
            templateUrl: ctxfolderurl + '/danhsachphat.html',
            controller: 'danhsachphat'
        })
        .when('/baihat/:id', {
            templateUrl: ctxfolderurl + '/baihat.html',
            controller: 'baihat'
        })
        .when('/timkiem/:tukhoa', {
            templateUrl: ctxfolderurl + '/timkiem.html',
            controller: 'timkiem'
        })
        //13/07 tạo playlist người dùng
        .when('/playlist/:id', {
            templateUrl: ctxfolderurl + '/playlist.html',
            controller: 'playlist'
        })
        //18/07 tạo trang nghệ sĩ 
        .when('/nghesi/:uid', {
            templateUrl: ctxfolderurl + '/nghesi.html',
            controller: 'nghesi'
        })
        // 24/07 tạo trang top 20
        .when('/top20', {
            templateUrl: ctxfolderurl + '/top20.html',
            controller: 'top20'
        })
        .when('/danhsachphattop20/:id', {
            templateUrl: ctxfolderurl + '/danhsachphattop20.html',
            controller: 'danhsachphattop20'
        })
        .when('/thanhtoanthanhcong/:idhoadonthanhtoan', {
            templateUrl: ctxfolderurl + '/thanhtoanthanhcong.html',
            controller: 'thanhtoanthanhcong'
        })
        .when('/thanhtoanthatbai', {
            templateUrl: ctxfolderurl + '/thanhtoanthatbai.html',
            controller: 'thanhtoanthatbai'
        })

});

app.factory('dataservice', function ($http) {
    return {
        getListQuangCao: function (data, callback) {
            $http.post('/Home/DanhSachQuangCao?uid=' + data).then(callback);
        },
        taoBaiHat: function (data, callback) {
            $http.post('/Home/taoBaiHat', data).then(callback);
        },
        taiTheLoai: function (callback) {
            $http.post('/Home/taiTheLoai').then(callback);

        },
        taiChuDe: function (callback) {
            $http.post('/Home/taiChuDe').then(callback);

        },
        uploadaudio: function (data, callback) {
            $http({
                method: 'post',
                url: '/Home/GetLink',
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
                url: '/Home/GetLinkHinhAnh',
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
        // 02/08
        uploadHinhAnhNguoiDung: function (data, callback) {
            $http({
                method: 'post',
                url: '/Home/GetLinkHinhAnhNguoiDung',
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
        getListBaiHatMoi: function (data, callback) {
            $http.post('/Home/GetListBaiHatMoi?uid=' + data).then(callback);
        },
        taoTaiKhoan: function (data, callback) {
            $http.post('/Home/TaoTaiKhoan', data).then(callback);
        },
        dangNhapAPI: function (data, callback) {
            $http.post('/Home/DangNhapTaiKhoan', data).then(callback);
        },
        token: function (data, callback) {
            $http.post('https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q', data).then(callback);
        },
        getListBaiHatNgheNhieuNhat: function (data, callback) {
            $http.post('/Home/getListNgheNhieuNhat?uid=' + data).then(callback);
        },
        getListBaiHatLikeNhieuNhat: function (data, callback) {
            $http.post('/Home/getListLikeNhieuNhat?uid=' + data).then(callback);
        },
        getListBaiHatDowloadNhieuNhat: function (data, callback) {
            $http.post('/Home/getListDowloadNhieuNhat?uid=' + data).then(callback);
        },
        timKiemBaiHat: function (data, callback) {
            $http.post('/Home/TimKiemBaiHat?tuKhoa=' + data).then(callback);
        },
        getListYeuThichDSP_canhan: function (data, callback) {
            $http.post('/Home/getListYeuThichDSP_canhan?uid=' + data).then(callback);
        },
        getListYeuThichDSPNgheSi_canhan: function (data, callback) {
            $http.post('/Home/getListYeuThichDSPNgheSi_canhan?uid=' + data).then(callback);
        },
        timKiemDanhSachPhatNguoiDung: function (data, callback) {
            $http.post('/Home/timKiemDanhSachPhatNguoiDung', data).then(callback);
        },
        timKiemDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Home/timKiemDanhSachPhatTheLoai', data).then(callback);
        },
        TimKiemBaiHatAll: function (data, callback) {
            $http.post('/Home/TimKiemBaiHatAll', data).then(callback);
        },
        TimKiemBaiHatAll: function (data, callback) {
            $http.post('/Home/TimKiemBaiHatAll', data).then(callback);
        },
        timKiemNguoiDungCustom: function (data, callback) {
            $http.post('/Home/timKiemNguoiDungCustom', data).then(callback);
        },
        timKiemNgheSi: function (data, callback) {
            $http.post('/Home/timKiemNgheSi', data).then(callback);
        },
        taoNguoiDungXacThuc: function (data, callback) {
            $http.post('/Home/TaoNguoiDungVoiXacThuc', data).then(callback);
        },
        taiZingChat: function (callback) {
            $http.post('/Home/taiZingChat').then(callback);

        },
        getListTheoDoi: function (data, callback) {
            $http.post('/Home/getListTheoDoi?uid=' + data).then(callback);
        },
        taiNhacMoi: function (callback) {
            $http.post('/Home/taiNhacMoi').then(callback);

        },
        taiTheLoaiKetHopDanhSachPhatTheLoai: function (callback) {
            $http.post('/Home/taiTheLoaiKetHopDanhSachPhatTheLoai').then(callback);

        },
        taiDanhSachPhatTheLoaiChiTiet: function (data, callback) {
            $http.post('/Home/taiDanhSachPhatTheLoaiChiTiet', data).then(callback);

        },
        taiDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Home/taiDanhSachPhatTheLoai', data).then(callback);

        },
        taiDanhSachBaiHat: function (data, callback) {
            $http.post('/Home/taiDanhSachBaiHat', data).then(callback);

        },
        taiDanhSachBaiHatTheoTheLoai: function (data, callback) {
            $http.post('/Home/taiDanhSachBaiHatTheoTheLoai', data).then(callback);

        },
        yeuThichBaiHat: function (data, callback) {
            $http.post('/Home/YeuThichBaiHat', data).then(callback);

        },
        gettopNgheSi: function (data, callback) {
            $http.post('/Home/gettopNgheSi?uid=' + data).then(callback);

        },
        getListDanhSachPhatNguoiDung: function (data, callback) {
            $http.post('/Home/getListDanhSachPhatNguoiDung', data).then(callback);
        },
        getListBaiHatYeuThich: function (data, callback) {
            $http.post('/Home/getListBaiHatYeuThich?uid=' + data).then(callback);

        },
        taoDanhSachPhat_NguoiDung: function (data, callback) {
            $http.post('/Home/taoDanhSachPhat_NguoiDung', data).then(callback);

        },
        getListDaTaiXuong_CaNhan: function (data, callback) {
            $http.post('/Home/getListDaTaiXuong_CaNhan?uid=' + data).then(callback);

        },
        getListHoaDon: function (data, callback) {
            $http.post('/Home/getListHoaDon?uid=' + data).then(callback);

        },
        getListDaTaiLen_NgheSi: function (data, callback) {
            $http.post('/Home/getListDaTaiLen_NgheSi', data).then(callback);

        },
        getPlaylist_CaNhan: function (data, callback) {
            $http.post('/Home/getPlaylist_CaNhan?uid=' + data).then(callback);

        },
        BinhLuanBaiHat_Cha: function (data, callback) {
            $http.post('/Home/BinhLuanBaiHat_Cha', data).then(callback);

        },
        ChinhSuaBinhLuanBaiHat_Cha: function (data, callback) {
            $http.post('/Home/ChinhSuaBinhLuanBaiHat_Cha', data).then(callback);

        },
        XoaBinhLuanBaiHat_Cha: function (data, callback) {
            $http.post('/Home/XoaBinhLuanBaiHat_Cha', data).then(callback);

        },
        XoaBinhLuanBaiHat_Con: function (data, callback) {
            $http.post('/Home/XoaBinhLuanBaiHat_Con', data).then(callback);

        },
        BinhLuanBaiHat_Con: function (data, callback) {
            $http.post('/Home/BinhLuanBaiHat_Con', data).then(callback);

        },
        ChinhSuaBinhLuanConBaiHat: function (data, callback) {
            $http.post('/Home/ChinhSuaBinhLuanConBaiHat', data).then(callback);

        },
        loadBinhLuan: function (data, callback) {
            $http.post('/Home/loadBinhLuan?idbh='+ data).then(callback);

        },
     
        themLuotNghe: function (data, callback) {
            $http.post('/Home/themLuotNghe?idbh=' + data).then(callback);

        },
        SendRequestPaypal: function (data, callback) {
            $http.post('/Home/SendRequestPaypal', data).then(callback);

        },
        SendRequestMomo: function (data, callback) {
            $http.post('/Home/SendRequestMomo', data).then(callback);

        },
        SetBaiHat: function (data, callback) {
            $http.post('/Home/SetBaiHat', data).then(callback);

        },
        ChinhSuaEmail: function (data, callback) {
            $http.post('/Home/ChinhSuaEmail', data).then(callback);

        },
        getUser: function (data, callback) {
            $http.post('/Home/getUser?uid=' + data).then(callback);

        },
        LayBangGoiVip: function (callback) {
            $http.post('/Home/LayBangGoiVip').then(callback);
        },
        XoaDanhSachPhatNguoiDung: function (data, callback) {
            $http.post('/Home/XoaDanhSachPhatNguoiDung', data).then(callback);
        },
        // Lấy bài hát theo id 
        taiBaiHatTheoId: function (data, callback) {
            $http.post('/Home/taiBaiHatTheoId', data).then(callback);

        },
        // Lấy Bài hát mới trong nhạc mới
        taiDSPBaiHatMoi_NhacMoi: function (data, callback) {
            $http.post('/Home/taiDSPBaiHatMoi_NhacMoi?uid=' + data).then(callback);

        },
        // 11/07
        taiDSPBaiHatTheoDSPTheLoai_DSP: function (data, callback) {
            $http.post('/Home/taiDSPBaiHatTheoDSPTheLoai_DSP', data).then(callback);

        },
        taiXuongBaiHat_NguoiDung: function (data, callback) {
            $http.post('/Home/taiXuongBaiHat_NguoiDung', data).then(callback);

        },
        getListDaTaiLen_CaNhan: function (data, callback) {
            $http.post('/Home/getListDaTaiLen_CaNhan?uid=' + data).then(callback);

        },
        theoDoiNguoiDung: function (data, callback) {
            $http.post('/Home/theoDoiNguoiDung', data).then(callback);

        },
        // 13/07 taiChiTietPlayList_PlayList
        taiChiTietPlayList_PlayList: function (data, callback) {
            $http.post('/Home/taiChiTietPlayList_PlayList', data).then(callback);

        },
        // 13/07 lấy thông tin nguoi dung bằng id bên playlist
        taiThongTinNguoiDungBangIdPlayList_PLayList: function (data, callback) {
            $http.post('/Home/taiThongTinNguoiDungBangIdPlayList_PLayList?uid=' + data).then(callback);

        },
        // 14/07 add bài hát vào playlist người dùng
        themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi: function (data, callback) {
            $http.post('/Home/themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi', data).then(callback);

        },
        // 14 /07 tai Danh sách bài hát theo id playlist nguoi dung _ PlayList
        taiDSBaiHatTheoDSPNguoiDung_PlayList: function (data, callback) {
            $http.post('/Home/taiDSBaiHatTheoDSPNguoiDung_PlayList', data).then(callback);

        },
        // 17/07 
        taiTheLoaiKetHopDanhSachPhatTheLoaiMoi: function (data, callback) {
            $http.post('/Home/taiTheLoaiKetHopDanhSachPhatTheLoaiMoi?uid=' + data).then(callback);

        },
        yeuThichDanhSachPhat: function (data, callback) {
            $http.post('/Home/YeuThichDanhSachPhat', data).then(callback);

        },
        YeuThichDanhSachPhatNguoiDung: function (data, callback) {
            $http.post('/Home/YeuThichDanhSachPhatNguoiDung', data).then(callback);

        },
        //20 / 07 làm lại download bài hát xuống máy người dùng
        downloadBaiHatVeMayNguoiDung: function (data, callback) {
            $http.post('/Home/downloadBaiHatVeMayNguoiDung', data).then(callback);
        },
        xoaNhacDaTaiXuong: function (data, callback) {
            $http.post('/Home/xoaNhacDaTaiXuong', data).then(callback);
        },
        // 21/07 xóa bài hát khỏi playnguoi dung
        xoaBaiHatKhoiDSPNguoiDung_Playlist: function (data, callback) {
            $http.post('/Home/xoaBaiHatKhoiDSPNguoiDung_Playlist', data).then(callback);
        },
        taiDSPBaiHatTheoDSPTop20_DSPTop20: function (data, callback) {
            $http.post('/Home/taiDSPBaiHatTheoDSPTop20_DSPTop20', data).then(callback);
        },
        // 26/07
        taiThongTinThanhToanThanhCong: function (data, callback) {
            $http.post('/Home/taiThongTinThanhToanThanhCong', data).then(callback);
        },

        //29/07 yeu thich top 20
        yeuThichTop20: function (data, callback) {
            $http.post('/Home/YeuThichTop20', data).then(callback);

        },
        // 29/07 
        taiTheLoaiKetHopTop20: function (data, callback) {
            $http.post('/Home/taiTheLoaiKetHopTop20?uid=' + data).then(callback);

        },
        taiTop20ChiTiet: function (data, callback) {
            $http.post('/Home/taiTop20ChiTiet', data).then(callback);

        },
        //02/08
        suaLinkHinhAnhNguoiDung_CaNhan: function (data, callback) {
            $http.post('/Home/suaLinkHinhAnhNguoiDung_CaNhan', data).then(callback);

        },
        // 06/08 
        chinhSuaHoTen_CaNhan: function (data, callback) {
            $http.post('/Home/chinhSuaHoTen_CaNhan', data).then(callback);
        },
        chinhSuaMoTa_CaNhan: function (data, callback) {
            $http.post('/Home/chinhSuaMoTa_CaNhan', data).then(callback);
        },
        getListYeuThichTop20_canhan: function (data, callback) {
            $http.post('/Home/getListYeuThichTop20_canhan?uid=' + data).then(callback);
        },
        taiDanhSachPhatTop20_khampha: function (data, callback) {
            $http.post('/Home/taiDanhSachPhatTop20_khampha?uid=' + data).then(callback);
        },
        //22/008
        taiTheLoaiTheoId_ChiTietTheLoai: function (data, callback) {
            $http.post('/Home/taiTheLoaiTheoId_ChiTietTheLoai', data).then(callback);
        },
        taiChuDeTheoId_ChuDe: function (data, callback) {
            $http.post('/Home/taiChuDeTheoId_ChuDe', data).then(callback);
        },
        taiDanhSachBaiHatTheoChuDe_ChuDe: function (data, callback) {
            $http.post('/Home/taiDanhSachBaiHatTheoChuDe_ChuDe', data).then(callback);
        },


        // 27/088
        taiTheLoaiKetHopDanhSachPhatTheLoaiTheoThoiGian: function (data, callback) {
            $http.post('/Home/taiTheLoaiKetHopDanhSachPhatTheLoaiTheoThoiGian', data).then(callback);

        },
        taiTheLoaiKetHopTop20TheoThoiGian: function (data, callback) {
            $http.post('/Home/taiTheLoaiKetHopTop20TheoThoiGian', data).then(callback);

        },


    }

});

let updateTimer;
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
            var reg = /^[^`~!@#$%\^&*()_+={}|[\]\\:';"<>?,./0-9]*$/;
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
app.controller('Ctrl_ESEIM', function ($scope, dataservice, $uibModal, $rootScope, $location, $log, $timeout, $q, $interval) {
    let curr_time = document.getElementById("process__start_time");
    let total_duration = document.getElementById("process__end_time");
    let audio = document.createElement("audio");
    var idBaiHatDangNghe;
    var timeOutCongLuotNghe;

    $rootScope.BaiHatDangPhat;
    $rootScope.kiemTraBaiHatDangPhat = '';//  23.7.2021 biến này để lưu id bài hát khi phát nhạc để check vị trí bài hát đang phát
    $scope.closeloginAccount = function () {
        $rootScope.closelogin = !$rootScope.closelogin;
    }
    $scope.ramdomsong = false;
    $rootScope.checklogin = {
        hovaten: '',
        uid: null,
        hinhanh: '',
        dadangnhap: false,
        vip: 0,
        mota: '',
        hansudungvip: '',
        email: ''
    }
    $rootScope.kiemTraDSPTLClick = "";

    $rootScope.$on("$locationChangeStart", function (event, next, current) {
        if ($rootScope.checklogin.dadangnhap) {

            if ($rootScope.checklogin.dadangnhap == false && $location.path() == "/canhan") {
                $rootScope.closelogin = true;
                $location.path('/');
                $scope.$apply();
            }
            if ($rootScope.checklogin.dadangnhap == true && $location.path() == "/canhan") {
                $scope.modelnav = 1;
            }
            if ($location.path() == "/") {
                $scope.modelnav = 2;
            }
            if ($location.path() == "/zingchart") {
                $scope.modelnav = 3;
            }
            if ($location.path() == "/theodoi") {
                $scope.modelnav = 4;
            }
            if ($location.path() == "/nhacmoi") {
                $scope.modelnav = 5;
            }
            if ($location.path() == "/theloai") {
                $scope.modelnav = 6;
            }
            if ($location.path() == "/top20") {
                $scope.modelnav = 7;
            }

        }
        else {
            var promise = new Promise(function (resolve, reject) {
                firebase.auth().onAuthStateChanged(function (userlogin) {
                    if (userlogin) {
                        const user = firebase.auth().currentUser;
                        if (user != null) {
                            $rootScope.checklogin.uid = user.uid;
                            $rootScope.checklogin.dadangnhap = true;
                            dataservice.getUser(user.uid, function (rs) {
                                rs = rs.data;
                                if (rs.Error) { } else {
                                    $rootScope.checklogin.hovaten = rs.hoten;

                                    $rootScope.checklogin.hinhanh = rs.hinhdaidien;

                                    $rootScope.checklogin.mota = rs.mota;
                                    $rootScope.checklogin.vip = rs.vip;
                                    $rootScope.checklogin.hansudungvip = rs.hansudungvip;





                                    //  $cookies.putObject("user", $rootScope.checklogin);
                                    var playlist = ''; // contexxtmenu
                                    dataservice.getPlaylist_CaNhan($rootScope.checklogin.uid, function (rs) {
                                        rs = rs.data;

                                        $rootScope.playlist_canhan = rs;
                                        for (var i = 0; i < $rootScope.playlist_canhan.length; i++) {
                                            playlist += '<li ng-click="themBaiHatVaoPlayList(' + "'" + $rootScope.playlist_canhan[i].id + "'" + ')"><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>' + $rootScope.playlist_canhan[i].tendanhsachphat + '</span></button></li>';

                                        }

                                        $scope.subtextMenuPlayList = '<div id="subtextmenuplaylist-node" class="menu add-playlist-content submenu-content"><ul class="menu-list"><li class="mar-t-10" ng-click="moTaoPlaylist()"><button class="zm-btn button" tabindex="0"><i class="icon ic- z-ic-svg ic-svg-add"></i><span>Tạo playlist mới</span></button></li></ul><div class="playlist-container"><div class="top-shadow "></div><div class="content"><div style="position: relative; background-color: #6a39af; width: 100%; height: 100%;"><div style="  background-color: #6a39af; inset: 0px;  margin-right: 0px; margin-bottom: 0px;"><ul class="menu-list">' + playlist + '</ul></div><div class="track-horizontal" style="position: absolute; height: 6px; transition: opacity 200ms ease 0s; opacity: 0;"><div style="position: relative; display: block; height: 100%; cursor: pointer; border-radius: inherit; background-color: rgba(0, 0, 0, 0.2); width: 0px;"></div></div><div class="track-vertical" style="position: absolute; width: 4px; transition: opacity 200ms ease 0s; opacity: 0; right: 2px; top: 2px; bottom: 2px; z-index: 100;"><div class="thumb-vertical" style="position: relative; display: block; width: 100%; height: 0px;"></div></div></div></div></div></div>';



                                    });

                                }
                            })


                            //// 29/07
                            //dataservice.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi($rootScope.checklogin.uid, function (rs) {
                            //    rs = rs.data;
                            //    $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi = rs;
                            //});
                            //dataservice.taiTheLoaiKetHopTop20($rootScope.checklogin.uid, function (rs) {
                            //    rs = rs.data;
                            //    $scope.taiTheLoaiKetHopTop20 = rs;
                            //});
                            //dataservice.taiDSPBaiHatMoi_NhacMoi($rootScope.checklogin.uid, function (rs) {

                            //    rs = rs.data;
                            //    $scope.taiDSPBaiHatMoi_NhacMoi = rs;

                            //});

                        }
                    }
                    else {
                        $scope.subtextMenuPlayList = '';
                    }
                    resolve();
                });
            });
            promise.then(function () {
                if ($rootScope.checklogin.dadangnhap == false && $location.path() == "/canhan") {
                    $rootScope.closelogin = true;
                    $location.path('/');
                    $scope.$apply();
                }
                if ($rootScope.checklogin.dadangnhap == true && $location.path() == "/canhan") {
                    $scope.modelnav = 1;
                }
                if ($location.path() == "/") {
                    $scope.modelnav = 2;
                }
                if ($location.path() == "/zingchart") {
                    $scope.modelnav = 3;
                }
                if ($location.path() == "/theodoi") {
                    $scope.modelnav = 4;
                }
                if ($location.path() == "/nhacmoi") {
                    $scope.modelnav = 5;
                }
                if ($location.path() == "/theloai") {
                    $scope.modelnav = 6;
                }
                if ($location.path() == "/top20") {
                    $scope.modelnav = 7;
                }

            }).catch(function () {
                alert("Lỗi Đăng Nhập");
            })
        }


    });

    var config = {
        apiKey: "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q",
        authDomain: "tfourmusic-1e3ff.firebaseapp.com",
        databaseURL: "tfourmusic-1e3ff-default-rtdb.firebaseio.com",
        projectId: "tfourmusic-1e3ff",
        storageBucket: "tfourmusic-1e3ff.appspot.com"


    };


    firebase.initializeApp(config);
    var ggProvider = new firebase.auth.GoogleAuthProvider();
    var fbProvider = new firebase.auth.FacebookAuthProvider();
    $scope.btnGoogle = function () {

        firebase.auth().signInWithPopup(ggProvider).then(function (result) {
            $scope.modellogin = {
                id: '',
                daxacthuc: '1',
                matkhau: '',
                email: '',
                hoten: '',
                quocgia: 'null',
                thanhpho: 'null',
                website: 'null',
                mota: 'null',
                ngaysinh: '',
                facebook: 'null',
                hinhdaidien: '',
                cover: 'null',
                gioitinh: '',
                online: 0,
                vip: 0,
                hansudung: '',
                uid: ''
            }
            var user = result.user;
            $scope.modellogin.email = user.email;
            $scope.modellogin.hoten = user.displayName;
            $scope.modellogin.uid = user.uid;
            $scope.modellogin.hinhdaidien = user.photoURL;
            dataservice.taoNguoiDungXacThuc($scope.modellogin, function (rs) {
                rs = rs.data;
                if (rs.Error) { }
                else {
                }
            });
            $rootScope.closelogin = false;
            delete $scope.modellogin;
            window.location.assign("/");
            $("#loading_main").css("display", "block");
            setTimeout(function () {
                $("#loading_main").css("display", "none");
            }, 3000);
        }).catch(function (error) {
            console.error('Error: hande error here>>>' + error);
        })
    }





    $scope.signOut = function () {
        $("#loading_main").css("display", "block");

        firebase.auth().signOut().then(function () {
            $rootScope.checklogin.dadangnhap = false;
            $rootScope.checklogin.hovaten = '';
            $rootScope.checklogin.uid = '';
            $rootScope.checklogin.hinhanh = '';
            $("#loading_main").css("display", "none");
        }, function (error) {
            console.error('Sign Out Error', error);
        });
    }

    $scope.loginFb = function () {
        firebase.auth().signInWithPopup(fbProvider).then(function (result) {
            // This gives you a Facebook Access Token. You can use it to access the Facebook API.

            $scope.modellogin = {
                id: '',
                daxacthuc: '1',
                matkhau: '',
                email: '',
                hoten: '',
                quocgia: 'null',
                thanhpho: 'null',
                website: 'null',
                mota: 'null',
                ngaysinh: '',
                facebook: 'null',
                hinhdaidien: '',
                cover: 'null',
                gioitinh: '',
                online: 0,
                vip: 0,
                hansudung: '',
                uid: ''
            }
            var user = result.user;
            if (user.email != null) {
                $scope.modellogin.email = user.email;
            }
            else {
                $scope.modellogin.email = 'null';
            }

            $scope.modellogin.hoten = user.displayName;
            $scope.modellogin.uid = user.uid;
            $scope.modellogin.hinhdaidien = user.photoURL;
            dataservice.taoNguoiDungXacThuc($scope.modellogin, function (rs) {
                rs = rs.data;
                if (rs.Error) { }
                else {
                }
            });
            $rootScope.closelogin = true;
            delete $scope.modellogin;
            window.location.assign("/");
            $("#loading_main").css("display", "block");
            setTimeout(function () {
                $("#loading_main").css("display", "none");
            }, 3000);
        }).catch(function (error) {
            // Handle Errors here.
            console.error("Lỗi");
        });
    }
    $scope.clickMuaGoiVip = function (goivip) {
        $scope.maGoiVip = goivip.id;
        $scope.goiVipThanhToan = goivip;
    }
    $scope.initData = function () {
        $scope.repeate = false;
        //$scope.huyhengioshow = 0;
        //$scope.xacthucemail = true;
        $scope.xemthembinhluan = 1;
        $scope.showBinhLuan = false;
        var promiselogin = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.uid = rs.uid;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }
                        })
                    }
                }
                else {

                }
                resolve();
            });
        });
        promiselogin.then(function () {
            dataservice.getListQuangCao($rootScope.checklogin.uid, function (rs) {

                rs = rs.data;
                $scope.listQuangCao = rs;
            });
        })

        dataservice.LayBangGoiVip(function (rs) {
            rs = rs.data;
            if (rs.Error) { } else {
                $scope.goiVip = rs;
            }
        })
        // Create a callback which logs the current auth state



        $scope.buttonnav = 1;
        $scope.nav__right = false;
        $scope.playmusic = 0;
        $scope.show = 0;
        $scope.sttmusic = 0;

        var progress = document.getElementById("progress");
        $scope.lickPlaylist = false;
        const $ = document.querySelector.bind(document);
        const $$ = document.querySelectorAll.bind(document);



        $scope.seekTo = function () {
            const seekTime = (audio.duration / 100) * progress.value;
            audio.currentTime = seekTime;
        }
        $scope.indexsongs = 0;

        $rootScope.songs = [
            {
                tenbaihat: "Gặp Người Đúng Lúc",
                casi: "Luân Tiang",
                link: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2Fadmin%2Fadmin2962021142954180?alt=media&token=24ac13cc-4edf-4b5e-9069-e2c2a78531cc",
                linkhinhanh: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/image%2Fbaihat%2Fadmin2962021142958180?alt=media&token=a0526c86-eba9-42e0-9178-f75fae5331e2"
            },

        ];
        $scope.slbaihat = $rootScope.songs.length;

        // Xử lý khi tua song
        //// Handling when seek
        progress.onchange = function (e) {
            const seekTime = (audio.duration / 100) * e.target.value;
            audio.currentTime = seekTime;
        };
        $scope.loadmusic = function (index) {
            $scope.loaddefaultmusic(index);
            if ($scope.playmusic == 1)
                audio.play();


        }

        function resetValues() {
            curr_time.textContent = "00:00";
            total_duration.textContent = "00:00";
            progress.value = 0;
        }
        $scope.loaddefaultmusic = function (e) {
            clearInterval(updateTimer);
            resetValues();
            const namemusic = document.getElementById("music_name");
            const sigermusic = document.getElementById("siger_name");
            const imgmusic = document.getElementById("image_music_avatar");
            namemusic.textContent = $rootScope.songs[e].tenbaihat;
            sigermusic.textContent = $rootScope.songs[e].casi;
            imgmusic.style.backgroundImage = "";
            imgmusic.style.backgroundImage = "url(" + $rootScope.songs[e].linkhinhanh + ")";
            audio.src = $rootScope.songs[e].link;
            idBaiHatDangNghe = $rootScope.songs[e].id;
            $rootScope.kiemTraBaiHatDangPhat = $rootScope.songs[e].id;
            $rootScope.BaiHatDangPhat = $rootScope.songs[e];

            $rootScope.kiemTraDSPTLClick = $rootScope.songs[e].danhsachphattheloai_id;
        }

    }
    $scope.changetimemusic = function () {
        const seekTime = (audio.duration / 100) * progress.value;
        audio.currentTime = seekTime;
    }

    $scope.initData();
    $scope.loadmusic($scope.sttmusic);

    $scope.showdata = function () {
        if ($scope.show == true) {

            $scope.show = 0;
        }
        else {
            $scope.show = 1;
        }

    }
    const cdThumb = document.getElementById("image_music_avatar");
    const cdThumbAnimate = cdThumb.animate([{ transform: "rotate(360deg)" }],
        {
            duration: 10000, // 10 seconds
            iterations: Infinity
        });
    cdThumbAnimate.pause();
    $scope.Play = function () {
        if ($scope.playmusic == 0) {
            audio.play();
            $scope.playmusic = 1;
            cdThumbAnimate.play();
            timeOutCongLuotNghe = undefined;
            /*$timeout.cancel(timeOutCongLuotNghe);*/
            if (idBaiHatDangNghe.length > 5) {
                var thoiGian;

                var promise = new Promise(function (resolve, reject) {
                    $timeout(function () {
                        thoiGian = (audio.duration * 1000) / 2;

                        resolve();
                    }, 2000);

                })
                promise.then(function () {

                    timeOutCongLuotNghe = $timeout(function () {
                        dataservice.themLuotNghe(idBaiHatDangNghe, function (rs) {
                            rs = rs.data;
                            idBaiHatDangNghe = "";
                        })

                    }, thoiGian)

                });




            }

        }
        else {
            $scope.playmusic = 0;
            audio.pause();
            cdThumbAnimate.pause();
            //console.log("cancel");
            $timeout.cancel(timeOutCongLuotNghe);
        }
    }
    $scope.changeThoiGianNhac = function () {

        audio.play();
        $scope.playmusic = 1;
        cdThumbAnimate.play();

    }
    updateTimer = setInterval(seekUpdate, 1000); setInterval(seekUpdate, 1000);
    function seekUpdate() {
        let seekPosition = 0;

        // Check if the current track duration is a legible number
        if (!isNaN(audio.duration)) {
            seekPosition = audio.currentTime * (100 / audio.duration);

            progress.value = seekPosition;
            let currentMinutes = Math.floor(audio.currentTime / 60);
            let currentSeconds = Math.floor(audio.currentTime - currentMinutes * 60);
            let durationMinutes = Math.floor(audio.duration / 60);
            let durationSeconds = Math.floor(audio.duration - durationMinutes * 60);

            // Adding a zero to the single digit time values
            if (currentSeconds < 10) { currentSeconds = "0" + currentSeconds; }
            if (durationSeconds < 10) { durationSeconds = "0" + durationSeconds; }
            if (currentMinutes < 10) { currentMinutes = "0" + currentMinutes; }
            if (durationMinutes < 10) { durationMinutes = "0" + durationMinutes; }

            curr_time.textContent = currentMinutes + ":" + currentSeconds;
            total_duration.textContent = durationMinutes + ":" + durationSeconds;
        }
    }
    $scope.clickNextSongs = function () {
        if ($scope.ramdomsong == true) {
            $scope.ramdomsongstt();

            $scope.loadmusic($scope.sttmusic);
        }
        else {
            $scope.sttmusic++;
            if ($scope.sttmusic >= $scope.songs.length)
                $scope.sttmusic = 0;
            $scope.loadmusic($scope.sttmusic);
        }

    }
    $scope.clickBackSongs = function () {
        $scope.sttmusic--;
        if ($scope.sttmusic < 0)
            $scope.sttmusic = $scope.slbaihat - 1;
        $scope.loadmusic($scope.sttmusic);
    }
    $scope.ramdomSongs = function () {
        $scope.ramdomsong = !$scope.ramdomsong;

    }
    $scope.ramdomsongstt = function () {
        if ($scope.ramdomsong == true) {
            if ($rootScope.songs.length > 1) {
                let newIndex;
                do {
                    newIndex = Math.floor(Math.random() * $rootScope.songs.length);
                } while (newIndex === $scope.sttmusic);

                $scope.sttmusic = newIndex;
            }

        }
    }
    $scope.repeateSong = function () {
        $scope.repeate = !$scope.repeate;

    }
    audio.onended = function () {
        if ($scope.repeate == true) {
            $scope.loadmusic($scope.sttmusic);
            auto.play();
        }
        else {
            if ($scope.ramdomsong == true) {
                $scope.ramdomsongstt();
                console.log($scope.sttmusic);
                $scope.loadmusic($scope.sttmusic);
            }
            else {
                $scope.sttmusic++;
                if ($scope.sttmusic >= $scope.slbaihat)
                    $scope.sttmusic = 0;
                $scope.loadmusic($scope.sttmusic);
            }
        }

    }
    $scope.changVolumeSong = function () {

        audio.volume = event.target.value / 100;
        $scope.volumechange = event.target.value;
    }
    $scope.setAutoPlayNhac = function () {
        $scope.playmusic = 0;
    }
    const playlist = document.getElementById("playlist_music_ul");

    //playlist.onclick = function (e) {
    //}

    //$scope.lickSongPlayList = function (index) {

    //    const songs = event.target.closest(".nav__right__list__item:not(.item_play_list)")
    //    if (songs || event.target.closest("#icon_option")) {
    //        //xu li khi click vao song
    //        if (songs) {
    //            if (event.target.closest("#icon_option")) {

    //            }
    //            else {
    //                $scope.clickBaiHatMoiActive = -1;
    //                $scope.sttmusic = Number(index);
    //                $scope.loadmusic($scope.sttmusic);
    //            }


    //        }
    //        //xu li khi click vao option
    //        if (event.target.closest(".icon_nav_right_first") || event.target.closest(".icon_account_a")) {
    //            if ($rootScope.checklogin.dadangnhap) {
    //                $scope.yeuThichmodel = {
    //                    id: '',
    //                    nguoidung_id: $rootScope.checklogin.uid,
    //                    baihat_id: $rootScope.songs[index].id,
    //                }
    //                dataservice.yeuThichBaiHat($scope.yeuThichmodel, function (rs) {
    //                    rs = rs.data;
    //                    if (rs.Error) { }
    //                    else {
    //                        if (rs) {
    //                            $rootScope.songs[index].yeuthich = 1;
    //                        }
    //                        else
    //                            $rootScope.songs[index].yeuthich = 0;
    //                    }
    //                })
    //            }
    //            else {
    //                $rootScope.closelogin = true;
    //            }

    //        }
    //    }
    //}



    $scope.upLoadNhac = function () {
        if ($rootScope.checklogin.dadangnhap) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: ctxfolderurl + '/add.html',
                controller: 'add',
                backdrop: 'true',
                backdropClass: ".fade:not(.show)1",
                backdropClass: ".modal-backdrop1",
                backdropClass: ".col-lg-81",
                backdropClass: ".modal-content1",

                size: '100'
            });
            modalInstance.result.then(function () {

            }, function () {
            });
        }
        else {
            $rootScope.closelogin = true;
        }

    };

    //
    $scope.ngayHienTai = new Date();
    $scope.disabledbutton = true;
    $scope.validatebutton = function () {
        $scope.disabledbutton = false;
    }
    $scope.model = {
        id: '',
        daxacthuc: '0',
        matkhau: '',
        email: '',
        hoten: '',
        quocgia: 'null',
        thanhpho: 'null',
        website: 'null',
        mota: 'null',
        ngaysinh: new Date(),
        facebook: 'null',
        hinhdaidien: '',
        cover: 'null',
        gioitinh: '',
        online: 0,
        vip: 0,
        hansudung: '0',
        uid: 'null'
    }
    $scope.dangky = function () {
        $("#loading_main").css("display", "block");



        if ($scope.model.gioitinh == 1) {
            $scope.model.gioitinh = "Nam"
        }
        else
            $scope.model.gioitinh = "Nữ"
        dataservice.taoTaiKhoan($scope.model, function (rs) {
            rs = rs.data;
            $scope.kq = rs;
            if (rs) {
                $("#loading_main").css("display", "none");

                $scope.taoThanhCong = true;
                $rootScope.closelogin = false;
                var downloadbaihat = document.getElementById("btntext");
                downloadbaihat.click();
            }
            else {
                $scope.taoThatBai = true;
                $("#loading_main").css("display", "none");
            }

        });
    }

    $scope.xacthucemail = false;
    $scope.dangNhapWeb = function () {
        $("#loading_main").css("display", "block");
        $timeout(function () {
            $rootScope.closelogin = true;
        }, 1400);
        $scope.dangNhapLoi = "";
        firebase.auth().signInWithEmailAndPassword($scope.login.email, $scope.login.password)
            .then(function (result) {

                var user = result.user;// This appears in the console
                if (user.emailVerified) {
                    $timeout(function () {
                        $rootScope.closelogin = false;
                    }, 1450);
                    $scope.modellogin = {
                        id: '',
                        daxacthuc: '1',
                        matkhau: '',
                        email: '',
                        hoten: '',
                        quocgia: 'null',
                        thanhpho: 'null',
                        website: 'null',
                        mota: 'null',
                        ngaysinh: '',
                        facebook: 'null',
                        hinhdaidien: '',
                        cover: 'null',
                        gioitinh: '',
                        online: 0,
                        vip: 0,
                        hansudung: '',
                        uid: ''
                    }
                    var user = result.user;
                    $scope.modellogin.email = user.email;
                    $scope.modellogin.hoten = user.displayName;
                    $scope.modellogin.uid = user.uid;
                    if (user.photoURL != null) {
                        $scope.modellogin.hinhdaidien = user.photoURL;
                    }

                    dataservice.taoNguoiDungXacThuc($scope.modellogin, function (rs) {
                        rs = rs.data;
                        if (rs.Error) { }
                        else {
                        }
                    });
                    $rootScope.closelogin = false;
                    delete $scope.modellogin;
                    $("#loading_main").css("display", "none");
                    window.location.assign("/");
                }
                else {
                    $("#loading_main").css("display", "none");
                    //$scope.signOut();
                    $scope.xacthucemail = true;

                }
            })
            .catch(function (error) {

                if (error.code == "auth/user-not-found") {
                    $scope.dangNhapLoi = "Email bạn nhập không kết nối với tài khoản nào"
                }
                if (error.code == "auth/wrong-password") {
                    $scope.dangNhapLoi = "Mật khẩu không hợp lệ."
                }
                $("#loading_main").css("display", "none");
            });




    }
    $scope.dangNhapEmailvaPass = function (email, pass) {
        if (email != null && email != "" && email != undefined && pass != null && pass != "" && pass != undefined) {
            $("#loading_main").css("display", "block");
            $timeout(function () {
                $rootScope.closelogin = true;
            }, 1400);
            $scope.dangNhapLoi = "";
            firebase.auth().signInWithEmailAndPassword(email, pass)
                .then(function (result) {

                    var user = result.user;// This appears in the console
                    if (user.emailVerified) {
                        $timeout(function () {
                            $rootScope.closelogin = false;
                        }, 1450);
                        $scope.modellogin = {
                            id: '',
                            daxacthuc: '1',
                            matkhau: '',
                            email: '',
                            hoten: '',
                            quocgia: 'null',
                            thanhpho: 'null',
                            website: 'null',
                            mota: 'null',
                            ngaysinh: '',
                            facebook: 'null',
                            hinhdaidien: '',
                            cover: 'null',
                            gioitinh: '',
                            online: 0,
                            vip: 0,
                            hansudung: '',
                            uid: ''
                        }
                        var user = result.user;
                        $scope.modellogin.email = user.email;
                        $scope.modellogin.hoten = user.displayName;
                        $scope.modellogin.uid = user.uid;
                        if (user.photoURL != null) {
                            $scope.modellogin.hinhdaidien = user.photoURL;
                        }

                        dataservice.taoNguoiDungXacThuc($scope.modellogin, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { }
                            else {
                            }
                        });
                        $rootScope.closelogin = false;
                        delete $scope.modellogin;
                        $("#loading_main").css("display", "none");
                        window.location.assign("/");
                    }
                    else {
                        $("#loading_main").css("display", "none");
                        //$scope.signOut();
                        $scope.xacthucemail = true;

                    }
                })
                .catch(function (error) {

                    if (error.code == "auth/user-not-found") {
                        $scope.dangNhapLoi = "Email bạn nhập không kết nối với tài khoản nào"
                    }
                    if (error.code == "auth/wrong-password") {
                        $scope.dangNhapLoi = "Mật khẩu không hợp lệ."
                    }
                    $("#loading_main").css("display", "none");
                });

        }
        else {
            if ($scope.login.email != null && $scope.login.email != "" && $scope.login.email != undefined && $scope.login.password != null && $scope.login.password != "" && $scope.login.password != undefined) {
                $("#loading_main").css("display", "block");
                $timeout(function () {
                    $rootScope.closelogin = true;
                }, 1400);
                $scope.dangNhapLoi = "";
                firebase.auth().signInWithEmailAndPassword($scope.login.email, $scope.login.password)
                    .then(function (result) {

                        var user = result.user;// This appears in the console
                        if (user.emailVerified) {
                            $timeout(function () {
                                $rootScope.closelogin = false;
                            }, 1450);
                            $scope.modellogin = {
                                id: '',
                                daxacthuc: '1',
                                matkhau: '',
                                email: '',
                                hoten: '',
                                quocgia: 'null',
                                thanhpho: 'null',
                                website: 'null',
                                mota: 'null',
                                ngaysinh: '',
                                facebook: 'null',
                                hinhdaidien: '',
                                cover: 'null',
                                gioitinh: '',
                                online: 0,
                                vip: 0,
                                hansudung: '',
                                uid: ''
                            }
                            var user = result.user;
                            $scope.modellogin.email = user.email;
                            $scope.modellogin.hoten = user.displayName;
                            $scope.modellogin.uid = user.uid;
                            if (user.photoURL != null) {
                                $scope.modellogin.hinhdaidien = user.photoURL;
                            }

                            dataservice.taoNguoiDungXacThuc($scope.modellogin, function (rs) {
                                rs = rs.data;
                                if (rs.Error) { }
                                else {
                                }
                            });
                            $rootScope.closelogin = false;
                            delete $scope.modellogin;
                            $("#loading_main").css("display", "none");
                            window.location.assign("/");
                        }
                        else {
                            $("#loading_main").css("display", "none");
                            //$scope.signOut();
                            $scope.xacthucemail = true;
                            var downloadbaihat = document.getElementById("btntext");
                            downloadbaihat.click();

                        }
                    })
                    .catch(function (error) {

                        if (error.code == "auth/user-not-found") {
                            $scope.dangNhapLoi = "Email bạn nhập không kết nối với tài khoản nào"
                        }
                        if (error.code == "auth/wrong-password") {
                            $scope.dangNhapLoi = "Mật khẩu không hợp lệ."
                        }
                        $("#loading_main").css("display", "none");
                        var downloadbaihat = document.getElementById("btntext");
                        downloadbaihat.click();
                    });


            }

        }

    }
    $scope.ResendEmailXacThuc = function () {
        $scope.time = 60;

        if (!$scope.kiemtraXacThucEmail) {
            firebase.auth().currentUser.sendEmailVerification()
                .then(() => {

                    var timeXacThuc = $interval(function () {
                        if ($scope.time == 0) {
                            $interval.cancel(timeXacThuc);
                            $scope.kiemtraXacThucEmail = false;
                            $scope.titleGuiLaiXacThucEmail = "Gửi lại mã xác thực"
                        }
                        else {
                            $scope.time--;
                            $scope.titleGuiLaiXacThucEmail = "Đợi " + $scope.time + "s " + "để gửi lại mã";
                            $scope.kiemtraXacThucEmail = true;
                        }

                    }, 1000);
                });
        }
        else {

        }
    }
    $scope.dangXuatNguoiDung = function () {
        $("#loading_main").css("display", "block");
        var promise = new Promise(function (resolve, reject) {
            $scope.signOut();
            resolve();
        });
        promise.then(function () {
            window.location.assign("/");
        }).catch(function () {
            alert("Lỗi Đăng Xuất");
        });
    }
    $scope.showMkDangKi = function (index) {
        if (index == 1) {
            if ($scope.showmk == "text") {
                $scope.showmk = "password";
            }
            else {
                $scope.showmk = "text";
            }

        }
        if (index == 0) {
            if ($scope.showmk1 == "password") {
                $scope.showmk1 = "text";
            }
            else {
                $scope.showmk1 = "password";
            }
        }

    }
    $scope.focusTimKiemBaiHat = function (index) {
        $scope.timKiemBaiHat = index;
    }
    $scope.timkiemfocus = false;
    $scope.clickCloseTimKiem = function () {
        $scope.timKiem = "";

    }
    $scope.blurTimKiem = function () {
        if (!$scope.timkiemfocus) {
            $scope.timKiemBaiHat = 0;
            delete $scope.ketQuaTimKiem;
        }
    }
    $scope.mousedowmTimKiem = function () {
        $scope.timkiemfocus = true;
    }
    $scope.mouseupTimKiem = function () {
        $scope.timkiemfocus = false;
        document.getElementById("tim_kiem_bai_hat").focus();
        $scope.timKiem = "";
    }
    $scope.mousedowmTimKiemKq = function () {
        $scope.timkiemfocus = true;
    }
    $scope.mouseupTimKiemKq = function (index) {
        $scope.timkiemfocus = false;
        document.getElementById("tim_kiem_bai_hat").focus();
        $rootScope.songs = [];
        $rootScope.songs.push($scope.ketQuaTimKiem[index]);
        //$rootScope.songs.push($scope.baiHatMoi[0]);
        //$rootScope.songs.push($scope.baiHatMoi[1]);
        //$rootScope.songs.push($scope.baiHatMoi[2]);
        //$rootScope.songs.push($scope.baiHatMoi[3]);
        $scope.playDowloadNhieuNhat = Number(index);
        $scope.loaddefaultmusic(0);
        $scope.setAutoPlayNhac();
        $scope.playmusicDowloadNhieuNhat = 1;
        $scope.Play();
        delete $scope.ketQuaTimKiem;
    }
    $scope.loadKetQuaTimKiem = function () {
        if ($scope.timKiem.length > 0) {
            dataservice.timKiemBaiHat($scope.timKiem, function (rs) {
                rs = rs.data;
                $scope.ketQuaTimKiem = rs;
            })
        }
        else {
            $scope.ketQuaTimKiem = null;
        }
    }
    $scope.loadBinhLuanBaiHat = function (idbh) {
        $("#loading_main").css("display", "block");
        if (idbh != null && idbh != "") {
            $scope.idBHBinhLuan = idbh;
            dataservice.loadBinhLuan(idbh, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {

                    $scope.duLieuBinhLuan = rs;
                    $scope.showBinhLuan = true;
                    $("#loading_main").css("display", "none");
                }
            })
        }
    }
    $scope.suKienNhanPhimEnter = function (event) {
        if (event.keyCode == 13) {
            $scope.timkiemfocus = false;
            document.getElementById("tim_kiem_bai_hat").blur();
            $location.path("/timkiem/" + $scope.timKiem);

        }
    }
    $scope.resetBinhLuan = true;
    $scope.showBL = function () {
        $scope.showBinhLuan = !$scope.showBinhLuan;
        $scope.duLieuBinhLuan = null;
      //  $scope.dataBinhLuanCha = "";
    }
    $scope.BinhLuanBaiHat = function (idbh, noidung1) {
      
        $("#loading_main").css("display", "block");

        if ($rootScope.checklogin.dadangnhap) {
            if (idbh != null && idbh != "" && noidung1 != null && noidung1 != "" && noidung1.length > 0) {
                $scope.model = {
                    id: '',
                    baihat_id: idbh,
                    nguoidung_id: $rootScope.checklogin.uid,
                    noidung: noidung1

                }
                /* noidung1 = " ";*/
            $scope.dataBinhLuanCha = "";
                dataservice.BinhLuanBaiHat_Cha($scope.model, function (rs) {
                    rs = rs.data;
                    dataservice.loadBinhLuan(idbh, function (rs) {
                        rs = rs.data;
                        if (rs.Error) { } else {
                            document.getElementById("binhluanbh").value = "";
                            $scope.duLieuBinhLuan = rs;
                            $scope.showBinhLuan = true;

                            $("#loading_main").css("display", "none");
                        }
                    })
                })
            }
        } else {
            $rootScope.closelogin = true;
            $("#loading_main").css("display", "none");
        }
    }
    $scope.BinhLuanConBaiHat = function (idbh,idblcha, noidung1) {
        $("#loading_main").css("display", "block");
        $scope.resetBinhLuan = false;
        if ($rootScope.checklogin.dadangnhap) {
            if (idblcha != null && idblcha != "" && noidung1 != null && noidung1 != "" && idbh != null && idbh != "" && noidung1.length > 0) {
                $scope.model = {
                    id: '',
                    baihat_id: idbh,
                    nguoidung_id: $rootScope.checklogin.uid,
                    binhluancha_id: idblcha,
                    noidung: noidung1

                }
                dataservice.BinhLuanBaiHat_Con($scope.model, function (rs) {
                    rs = rs.data;
                    dataservice.loadBinhLuan(idbh, function (rs) {
                        rs = rs.data;
                        if (rs.Error) { } else {
                            $scope.resetBinhLuan = true;
                            $scope.duLieuBinhLuan = rs;
                            $scope.showBinhLuan = true;
                           /* $scope.dataBinhLuanCha = "";*/
                            $("#loading_main").css("display", "none");
                        }
                    })
                })
            }
        } else {
            $("#loading_main").css("display", "none");
            $rootScope.closelogin = !$rootScope.closelogin;
          
        }
    }
    $scope.PhimEnterBinhLuanCha = function (event,idbh,noidung1) {
        if (event.keyCode == 13) {
            $("#loading_main").css("display", "block");
            $scope.resetBinhLuan = false;
            if ($rootScope.checklogin.dadangnhap) {
                if (idbh != null && idbh != "" && noidung1 != null && noidung1 != "" && noidung1.length > 0) {
                    $scope.model = {
                        id: '',
                        baihat_id: idbh,
                        nguoidung_id: $rootScope.checklogin.uid,
                        noidung: noidung1

                    }
                    /* $("#loading_main").css("display", "none");*/
                    dataservice.BinhLuanBaiHat_Cha($scope.model, function (rs) {
                        rs = rs.data;
                        dataservice.loadBinhLuan(idbh, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $scope.resetBinhLuan = true;
                                $scope.duLieuBinhLuan = rs;
                                $scope.showBinhLuan = true;
                                document.getElementById("binhluanbh").value = "";
                                $("#loading_main").css("display", "none");
                            }
                        })
                    })
                }
            }
            else {
                $("#loading_main").css("display", "none");
                $rootScope.closelogin = true;
            }
        }
    }
    $scope.PhimEnterBinhLuanCon = function (event, idbh, idblcha, noidung1) {
        if (event.keyCode == 13) {
            $("#loading_main").css("display", "block");
           
            if ($rootScope.checklogin.dadangnhap) {
                if (idblcha != null && idblcha != "" && noidung1 != null && noidung1 != "" && idbh != null && idbh != "" && noidung1.length >0) {
                        $scope.model = {
                            id: '',
                            baihat_id: idbh,
                            nguoidung_id: $rootScope.checklogin.uid,
                            binhluancha_id: idblcha,
                            noidung: noidung1

                        }
                        dataservice.BinhLuanBaiHat_Con($scope.model, function (rs) {
                            rs = rs.data;
                            dataservice.loadBinhLuan(idbh, function (rs) {
                                rs = rs.data;
                                if (rs.Error) { } else {
                                    
                                    $scope.duLieuBinhLuan = rs;
                                    $scope.showBinhLuan = true;
                                  /*  $scope.dataBinhLuanCha = "";*/
                                    $("#loading_main").css("display", "none");
                                }
                            })
                        })
                    }
                
            }
            else {
                $("#loading_main").css("display", "none");
                $rootScope.closelogin = true;
            }
        }
    }
    $scope.ChinhSuaBinhLuanBaiHat = function (idbh, idbl ,noidung1) {
        /*$scope.dataBinhLuanCha = "";*/
        $("#loading_main").css("display", "block");

        if ($rootScope.checklogin.dadangnhap ) {
            if (idbh != null && idbh != "" && noidung1 != null && noidung1 != "" && noidung1.length > 0 && idbl != "" && idbl != null) {
                $scope.model = {
                    id: idbl,
                    baihat_id: idbh,
                    nguoidung_id: $rootScope.checklogin.uid,
                    noidung: noidung1

                }
                /* noidung1 = " ";*/

                dataservice.ChinhSuaBinhLuanBaiHat_Cha($scope.model, function (rs) {
                    rs = rs.data;
                    dataservice.loadBinhLuan(idbh, function (rs) {
                        rs = rs.data;
                        if (rs.Error) { } else {

                            $scope.duLieuBinhLuan = rs;
                            $scope.showBinhLuan = true;

                            $("#loading_main").css("display", "none");
                        }
                    })
                })
            }
        } else {
            $rootScope.closelogin = true;
            $("#loading_main").css("display", "none");
        }
    }
    $scope.ChinhSuaBinhLuanConBaiHat = function (idbh, idbl, noidung1) {
        /*$scope.dataBinhLuanCha = "";*/
        $("#loading_main").css("display", "block");

        if ($rootScope.checklogin.dadangnhap) {
            if (idbh != null && idbh != "" && noidung1 != null && noidung1 != "" && noidung1.length > 0 && idbl != "" && idbl != null) {
                $scope.model = {
                    id: idbl,
                    baihat_id: idbh,
                    nguoidung_id: $rootScope.checklogin.uid,
                    noidung: noidung1

                }
                /* noidung1 = " ";*/

                dataservice.ChinhSuaBinhLuanConBaiHat($scope.model, function (rs) {
                    rs = rs.data;
                    dataservice.loadBinhLuan(idbh, function (rs) {
                        rs = rs.data;
                        if (rs.Error) { } else {

                            $scope.duLieuBinhLuan = rs;
                            $scope.showBinhLuan = true;

                            $("#loading_main").css("display", "none");
                        }
                    })
                })
            }
        } else {
            $rootScope.closelogin = true;
            $("#loading_main").css("display", "none");
        }
    }
    $scope.PhimEnterChinhSuaBinhLuanConBaiHat = function (event, idbh, idbl, noidung1) {
        if (event.keyCode == 13) {
            $("#loading_main").css("display", "block");

            if ($rootScope.checklogin.dadangnhap) {
                if (idbh != null && idbh != "" && noidung1 != null && noidung1 != "" && noidung1.length > 0 && idbl != "" && idbl != null) {
                    $scope.model = {
                        id: idbl,
                        baihat_id: idbh,
                        nguoidung_id: $rootScope.checklogin.uid,
                        noidung: noidung1

                    }
                    /* noidung1 = " ";*/

                    dataservice.ChinhSuaBinhLuanConBaiHat($scope.model, function (rs) {
                        rs = rs.data;
                        dataservice.loadBinhLuan(idbh, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {

                                $scope.duLieuBinhLuan = rs;
                                $scope.showBinhLuan = true;

                                $("#loading_main").css("display", "none");
                            }
                        })
                    })
                }
            } else {
                $rootScope.closelogin = true;
                $("#loading_main").css("display", "none");
            }
        }
    }
    $scope.PhimEnterChinhSuaBinhLuanCha = function (event, idbh,idbl, noidung1) {
        if (event.keyCode == 13) {
            $("#loading_main").css("display", "block");

            if ($rootScope.checklogin.dadangnhap) {
                if (idbh != null && idbh != "" && noidung1 != null && noidung1 != "" && noidung1.length > 0 && idbl != "" && idbl != null) {
                    $scope.model = {
                        id: idbl,
                        baihat_id: idbh,
                        nguoidung_id: $rootScope.checklogin.uid,
                        noidung: noidung1

                    }
                    /* noidung1 = " ";*/

                    dataservice.ChinhSuaBinhLuanBaiHat_Cha($scope.model, function (rs) {
                        rs = rs.data;
                        dataservice.loadBinhLuan(idbh, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {

                                $scope.duLieuBinhLuan = rs;
                                $scope.showBinhLuan = true;

                                $("#loading_main").css("display", "none");
                            }
                        })
                    })
                }
            } else {
                $rootScope.closelogin = true;
                $("#loading_main").css("display", "none");
            }
        }
    }
    $scope.XoaBinhLuanBaiHat = function (idbh, idbl) {
        /*$scope.dataBinhLuanCha = "";*/
        $("#loading_main").css("display", "block");

        if ($rootScope.checklogin.dadangnhap) {
            if (idbh != null && idbh != "" &&  idbl != "" && idbl != null) {
                $scope.model = {
                    id: idbl,
                    baihat_id: idbh,
                    nguoidung_id: $rootScope.checklogin.uid,
                    noidung: ''

                }
                /* noidung1 = " ";*/

                dataservice.XoaBinhLuanBaiHat_Cha($scope.model, function (rs) {
                    rs = rs.data;
                    dataservice.loadBinhLuan(idbh, function (rs) {
                        rs = rs.data;
                        if (rs.Error) { } else {

                            $scope.duLieuBinhLuan = rs;
                            $scope.showBinhLuan = true;

                            $("#loading_main").css("display", "none");
                        }
                    })
                })
            }
        } else {
            $rootScope.closelogin = true;
            $("#loading_main").css("display", "none");
        }
    }
    $scope.XoaBinhLuanConBaiHat = function (idbh, idbl) {
        /*$scope.dataBinhLuanCha = "";*/
        $("#loading_main").css("display", "block");

        if ($rootScope.checklogin.dadangnhap) {
            if (idbh != null && idbh != "" && idbl != "" && idbl != null) {
                $scope.model = {
                    id: idbl,
                    baihat_id: idbh,
                    nguoidung_id: $rootScope.checklogin.uid,
                    noidung: ''

                }
                /* noidung1 = " ";*/

                dataservice.XoaBinhLuanBaiHat_Con($scope.model, function (rs) {
                    rs = rs.data;
                    dataservice.loadBinhLuan(idbh, function (rs) {
                        rs = rs.data;
                        if (rs.Error) { } else {

                            $scope.duLieuBinhLuan = rs;
                            $scope.showBinhLuan = true;

                            $("#loading_main").css("display", "none");
                        }
                    })
                })
            }
        } else {
            $rootScope.closelogin = true;
            $("#loading_main").css("display", "none");
        }
    }
    $scope.showTimer = function (index) {
        if (index == 1) {
            $scope.showtime = 1;
        }
        if (index == 2) {
            $scope.showtime = 2;
        }
        if (index == 0) {
            $scope.showtime = 0;
        }


    }
    function secondsToHms(d) {
        d = Number(d);
        var h = Math.floor(d / 3600);
        var m = Math.floor(d % 3600 / 60);
        var s = Math.floor(d % 3600 % 60);

        var hDisplay = h > 0 ? (h < 10 ? "0" : "") + h : "00";
        var mDisplay = m > 0 ? (m < 10 ? "0" : "") + m : "00";
        var sDisplay = s > 0 ? (s < 10 ? "0" : "") + s : "00";
        return hDisplay + ":" + mDisplay + ":" + sDisplay;
    }
    var HenGioTimeOut, henGioInterval;
    $rootScope.soGioConLai = undefined;
    $scope.batDauHenGio = function (gio, phut, tg) {
        var hengio = (((Number(gio) * 60 + Number(phut)) * 60) * 1000);

        HenGioTimeOut = $timeout(function () {
            $scope.playmusic = 1;
            $scope.Play();
            $rootScope.soGioConLai = "00:00:00";
            $scope.dahengio = undefined;
            $scope.henGioSoGio = "00";
            $scope.henGioSoPhut = "00";
        }, hengio);
        $scope.checkShowHenGio = false;
        var demsogio = (Number(gio) * 60) * 60 + (phut * 60);
        henGioInterval = $interval(function () {
            $rootScope.soGioConLai = secondsToHms(demsogio--);
        }, 1000)
    }
    $scope.huyHenGio = function () {
        $timeout.cancel(HenGioTimeOut);
        $interval.cancel(henGioInterval);
        $rootScope.soGioConLai = "00:00:00";
        $scope.dahengio = undefined;
        $scope.henGioSoGio = "00";
        $scope.henGioSoPhut = "00";

    }
    $scope.blurhengio = function () {
        if (!$scope.hengiofocus) {
            $scope.showtime = 0;
        }
    }
    $scope.mousedowmhengio = function () {
        $scope.hengiofocus = true;
    }
    $scope.mouseuphengio = function () {
        $scope.hengiofocus = false;

    }
    $scope.showtimertg = function (gio, phut) {
        $scope.tg = new Date();
        $scope.ismeridian = true;
        $scope.tg.setHours($scope.tg.getHours() + Number(gio));
        $scope.tg.setMinutes($scope.tg.getMinutes() + Number(phut));


    }

    $scope.clickTaoPlaylist = function () {
        $("#loading_main").css("display", "block");



        if ($rootScope.checklogin.dadangnhap) {
            $scope.danhSachPhatNguoiDung = {
                id: '',
                nguoidung_id: '',
                tendanhsachphat: '',
                chedo: 0

            }
            $scope.danhSachPhatNguoiDung.tendanhsachphat = $scope.tenplaylist;
            $scope.danhSachPhatNguoiDung.chedo = $scope.swithoffon == true ? 1 : 0;
            $scope.danhSachPhatNguoiDung.nguoidung_id = $rootScope.checklogin.uid;
            dataservice.taoDanhSachPhat_NguoiDung($scope.danhSachPhatNguoiDung, function (rs) {
                rs = rs.data;
                $("#loading_main").css("display", "none");
                if (rs.Error) { } else {

                }
                dataservice.getPlaylist_CaNhan($rootScope.checklogin.uid, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $rootScope.playlist_canhan = rs;
                    }
                });

                //  $cookies.putObject("user", $rootScope.checklogin);
                var playlist = ''; // contexxtmenu
                dataservice.getPlaylist_CaNhan($rootScope.checklogin.uid, function (rs) {
                    rs = rs.data;

                    $rootScope.playlist_canhan = rs;
                    for (var i = 0; i < $rootScope.playlist_canhan.length; i++) {
                        playlist += '<li ng-click="themBaiHatVaoPlayList(' + "'" + $rootScope.playlist_canhan[i].id + "'" + ')"><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>' + $rootScope.playlist_canhan[i].tendanhsachphat + '</span></button></li>';

                    }

                    $scope.subtextMenuPlayList = '<div id="subtextmenuplaylist-node" class="menu add-playlist-content submenu-content"><ul class="menu-list"><li class="mar-t-10" ng-click="moTaoPlaylist()"><button class="zm-btn button" tabindex="0"><i class="icon ic- z-ic-svg ic-svg-add"></i><span>Tạo playlist mới</span></button></li></ul><div class="playlist-container"><div class="top-shadow "></div><div class="content"><div style="position: relative; background-color: #6a39af; width: 100%; height: 100%;"><div style="  background-color: #6a39af; inset: 0px;  margin-right: 0px; margin-bottom: 0px;"><ul class="menu-list">' + playlist + '</ul></div><div class="track-horizontal" style="position: absolute; height: 6px; transition: opacity 200ms ease 0s; opacity: 0;"><div style="position: relative; display: block; height: 100%; cursor: pointer; border-radius: inherit; background-color: rgba(0, 0, 0, 0.2); width: 0px;"></div></div><div class="track-vertical" style="position: absolute; width: 4px; transition: opacity 200ms ease 0s; opacity: 0; right: 2px; top: 2px; bottom: 2px; z-index: 100;"><div class="thumb-vertical" style="position: relative; display: block; width: 100%; height: 0px;"></div></div></div></div></div></div>';



                });
            })
            $scope.checktaoplaylist = false;
            $scope.tenplaylist = "";

        } else {
            $("#loading_main").css("display", "none");
            $rootScope.closelogin = true;
        }
    }
    $scope.clickShowVip = function () {
        $scope.showVip = !$scope.showVip;
    }

    //thanh toan bằng paypal
    $scope.thanhToanBangPaypal = function (goivip) {
        if ($rootScope.checklogin.dadangnhap) {
            if (goivip != null && goivip != undefined) {

                $scope.modelThanhToan = {
                    id: '',
                    hoadonthanhtoan_id: '',
                    mota: '',
                    giatien: goivip.giatiengiamgia,
                    nguoidung_id: $rootScope.checklogin.uid,
                    trangthai: 0,
                    loaigoivip_id: goivip.id,
                    phuongthucthanhtoan: 'Paypal'
                }

                $("#loading_main").css("display", "block");
                setTimeout(function () {
                    $("#loading_main").css("display", "none");
                }, 30000);

                dataservice.SendRequestPaypal($scope.modelThanhToan, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        window.location.assign(rs.payUrl);
                    }
                })

            }

        }
        else {
            $rootScope.closelogin = true;
        }

    }
    //thanh toan bằng momo
    $scope.thanhToanBangMomo = function (goivip) {
        if ($rootScope.checklogin.dadangnhap) {
            if (goivip != null && goivip != undefined) {

                $scope.modelThanhToan = {
                    id: '',
                    hoadonthanhtoan_id: '',
                    mota: goivip.tengoivip,
                    giatien: goivip.giatiengiamgia,
                    nguoidung_id: $rootScope.checklogin.uid,
                    trangthai: 0,
                    loaigoivip_id: goivip.id,
                    phuongthucthanhtoan: 'Momo'
                }

                $("#loading_main").css("display", "block");
                setTimeout(function () {
                    $("#loading_main").css("display", "none");
                }, 30000);

                dataservice.SendRequestMomo($scope.modelThanhToan, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        window.location.assign(rs.payUrl);
                    }
                })

            }

        }
        else {
            $rootScope.closelogin = true;
        }

    }
    //công khai hoặc cá nhân 1 bài hát
    $scope.congKhaiBaiHat = function (bh) {
        if ($rootScope.checklogin.dadangnhap) {
            if (bh != null) {
                dataservice.SetBaiHat(bh, function (rs) {
                    rs = rs.data
                })
            }
        }
    }

    //  phát bài hát có thể 1 bài hát hoặc 1 list bài hát
    $scope.playMotBanNhac = function (song, vitri) {


        if (song instanceof Array) {
            if (vitri == null) {
                vitri = 0;
            }
            $rootScope.songs = song;
            $scope.sttmusic = vitri;
            $scope.loaddefaultmusic(vitri);
            $scope.setAutoPlayNhac();
            $scope.Play();
        }
        else {
            $scope.flag = false;
            angular.forEach($rootScope.songs, function (val, index) {
                if (val.id == song.id) {
                    $scope.sttmusic = index;
                    $scope.loaddefaultmusic($scope.sttmusic);
                    $scope.setAutoPlayNhac();
                    $scope.Play();
                    flag = true;
                }
            });
            if (!$scope.flag) {

                $rootScope.songs = $rootScope.songs.concat(song);
                $scope.sttmusic = $rootScope.songs.length - 1;
                $scope.loaddefaultmusic($scope.sttmusic);
                $scope.setAutoPlayNhac();
                $scope.Play();
            }
        }


    }
    // cái này của lay out
    $scope.playMotBanNhacLayout = function (song) {

        $scope.sttmusic = song
        $scope.loaddefaultmusic(song);
        $scope.setAutoPlayNhac();
        $scope.Play();
    }


    // yêu thích hoặc hủy yêu thích bài hát theo id bài hát
    $scope.yeuThichBaiHat = function (bhid) {
        if ($rootScope.checklogin.dadangnhap) {
            $scope.yeuThichbhmodel = {
                id: '',
                nguoidung_id: $rootScope.checklogin.uid,
                baihat_id: bhid,
            };
            dataservice.yeuThichBaiHat($scope.yeuThichbhmodel, function (rs) {
                rs = rs.data;
                if (rs.Error) { }
                else {
                    if (rs) {
                        return true;
                    }
                    else
                        return false;
                }
            });
        }
        else {
            $rootScope.closelogin = true;
        }
    }
    // 17/07 yêu thích hoặc hủy yêu thích dasachphat theo id danhsachphat
    $scope.yeuThichDanhSachPhat = function (dspid) {
        if ($rootScope.checklogin.dadangnhap) {
            $scope.yeuThichDSPModel = {
                id: '',
                nguoidung_id: $rootScope.checklogin.uid,
                danhsachphat_id: dspid,
            };
            dataservice.yeuThichDanhSachPhat($scope.yeuThichDSPModel, function (rs) {
                rs = rs.data;
                if (rs.Error) { }
                else {
                    if (rs) {
                        return true;
                    }
                    else
                        return false;
                }
            });
        }
        else {
            $rootScope.closelogin = true;
        }

    }
    // 29/07 yêu thích hoặc hủy yêu thích top20 theo id top20
    $scope.yeuThichTop20 = function (top20id) {
        if ($rootScope.checklogin.dadangnhap) {
            $scope.yeuThichTop20Model = {
                id: '',
                nguoidung_id: $rootScope.checklogin.uid,
                top20_id: top20id,
            };
            dataservice.yeuThichTop20($scope.yeuThichTop20Model, function (rs) {
                rs = rs.data;
                if (rs.Error) { }
                else {
                    if (rs) {
                        return true;
                    }
                    else
                        return false;
                }
            });
        }
        else {
            $rootScope.closelogin = true;
        }

    }


    // 17/07 yêu thích hoặc hủy yêu thích dasachphat theo id danhsachphat nguoidung
    $scope.yeuThichDanhSachPhatNguoiDungID = function (dspndid) {
        if ($rootScope.checklogin.dadangnhap) {
            $scope.yeuThichDSPNDModel = {
                id: '',
                nguoidung_id: $rootScope.checklogin.uid,
                danhsachphat_id: dspndid
            };
            dataservice.YeuThichDanhSachPhatNguoiDung($scope.yeuThichDSPNDModel, function (rs) {
                rs = rs.data;
                if (rs.Error) { }
                else {
                    if (rs) {
                        return true;
                    }
                    else
                        return false;
                }
            });
        }
        else {
            $rootScope.closelogin = true;
        }

    }
    //thêm bài hát  va danh sach phat vào dsp hiện tại
    $scope.themBaiHatVaoDanhSachPhat = function (bh) {

        if (bh instanceof Array) {
            $rootScope.songs = $rootScope.songs.concat(bh);

        }
        else {
            angular.forEach($rootScope.songs, function (val, index) {
                if (val.id == bh.id) {
                    $rootScope.songs.splice(index, 1);
                }
            })

            $rootScope.songs = $rootScope.songs.concat(bh);
        }
    }
    // thêm bài hát vào hát tiếp theo
    $scope.themBaiHatVaoPhatTiepTheo = function (bh) {
        angular.forEach($rootScope.songs, function (val, index) {
            if (val.id == bh.id) {
                $rootScope.songs.splice(index, 1);
            }
        })
        $rootScope.songs.splice($scope.sttmusic + 1, 0, bh);
        $scope.apply();
    }
    // theo dõi người dùng
    $scope.theoDoiNguoiDung = function (modeltheodoi) {
        if ($rootScope.checklogin.dadangnhap) {
            dataservice.theoDoiNguoiDung(modeltheodoi, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    return rs;
                }
            })
        }
        else {
            $rootScope.closelogin = true;
        }
    }

    // 21/07 tuấn tài chạy thanh loading tải xuống bài hát
    $scope.max = 100;
    $scope.dynamic = 0;
    $scope.hienThiTaiXuong = function () {
        if ($scope.dynamic == 100) {
            $scope.dynamic = 0;
        }
        $scope.dynamic += 20;

    }
    /////////////////////////////////////////
    //04/08 tuấn tài 
    // 30/7 tuấn tài
    // 30/7 tuấn tài contextmenu dsp the loai
    // 21/07 tuấn tài
    // 22/07 tuấn tài contextmenu dsp the loai
    $scope.dspCookie = "";
    // xem contextmenu bai hat khi click <li><button class="zm-btn button" tabindex="0"><i class="icon far fa-comment"></i><span>Bình luận</span><span class="comment-badge"></span></button></li>
    $scope.moChiTietDSP_TheLoai = function (data) {
        $scope.dspCookie = data;                                                                                                                                                                  //thêm vào danh sách phát                                                                                                                                                             //Bình Luận                                                                                                                                                                                                                     // Sao chép Link                                                                                                                                                                                                                                                                // chia sẻ                                                                                                                                                                                                                                           

        $scope.contextMenuDSPTheLoai = '<div id="contextmenudsptl-node" class="zm-contextmenu song-menu" style="width:250px; padding:10px 0;"><div class="menu" ><ul class="menu-list"><li><button class="zm-btn button" tabindex="0" ng-click="themVaoDSP_DSPTheLoai(dspCookie)"><i class="icon far fa-list-alt"></i><span>Thêm vào danh sách phát</span></button></li><li><button class="zm-btn button" ng-click="copyLinkDSPTheLoai(' + "'https://localhost:44348/#/danhsachphat/" + data.id + "'" + ')" tabindex="0"><i class="icon fa fa-link"></i><span>Sao chép link</span></button></li><li><div class="menu-list--submenu" subtext-menu="subtextMenuChiaSe"><button class="zm-btn button" tabindex="0" ng-click="shareDanhSachPhat(' + "'https://localhost:44348/#/danhsachphat/" + data.id + "'" + ')"><i class="icon fa fa-share"></i><span>Chia sẻ</span></button></div></li></ul ></div ></div >';
        //  $scope.subtextMenuPlayList = document.getElementById("subtextmenuplaylist-node").innerHTML;
        //29/07 
        $scope.contextMenuTop20 = '<div id="contextmenudsptl-node" class="zm-contextmenu song-menu" style="width:250px; padding:10px 0;"><div class="menu" ><ul class="menu-list"><li><button class="zm-btn button" tabindex="0" ng-click="themVaoDSP_Top20(dspCookie)"><i class="icon far fa-list-alt"></i><span>Thêm vào danh sách phát</span></button></li><li><button class="zm-btn button" ng-click="copyLinkDSPTheLoai(' + "'https://localhost:44348/#/danhsachphattop20/" + data.id + "'" + ')" tabindex="0"><i class="icon fa fa-link"></i><span>Sao chép link</span></button></li><li><div class="menu-list--submenu" subtext-menu="subtextMenuChiaSe"><button class="zm-btn button" tabindex="0" ng-click="shareDanhSachPhat(' + "'https://localhost:44348/#/danhsachphattop20/" + data.id + "'" + ')"><i class="icon fa fa-share"></i><span>Chia sẻ</span></button></div></li></ul ></div ></div >';
        // 03/08 playlist
        $scope.contextMenuPlaylist = '<div id="contextmenudsptl-node" class="zm-contextmenu song-menu" style="width:250px; padding:10px 0;"><div class="menu" ><ul class="menu-list"><li><button class="zm-btn button" tabindex="0" ng-click="themDSBaiHatVaoDSP_Playlist(dspCookie)"><i class="icon far fa-list-alt"></i><span>Thêm vào danh sách phát</span></button></li><li><button class="zm-btn button" ng-click="copyLinkDSPTheLoai(' + "'https://localhost:44348/#/playlist/" + data.id + "'" + ')" tabindex="0"><i class="icon fa fa-link"></i><span>Sao chép link</span></button></li><li><div class="menu-list--submenu" subtext-menu="subtextMenuChiaSe"><button class="zm-btn button" tabindex="0" ng-click="shareDanhSachPhat(' + "'https://localhost:44348/#/playlist/" + data.id + "'" + ')"><i class="icon fa fa-share"></i><span>Chia sẻ</span></button></div></li></ul ></div ></div >';

    };
    $scope.shareDanhSachPhat = function (link) {
        u = link;
        t = $scope.baihatCookie.tenbaihat;
        window.open('http://www.facebook.com/sharer.php?u=' + encodeURIComponent(u) + '&t=' + encodeURIComponent(t), 'sharer', 'toolbar=0,status=0,width=626,height=436');
    }
    // thêm tất cả bài hát có trong playlist vào trong danh sách phát đang phát
    $scope.themDSBaiHatVaoDSP_Playlist = function (data) {
        // alert(data.id + data.tendanhsachphattheloai);
        $scope.text = {
            key: '',
            uid: $rootScope.checklogin.uid
        }
        $scope.text.key = data.id;
        $scope.modelChiTietPlayList = {
            dspnguoidung_id: '',
            // người dùng sỡ hữu playlist dspnguoidung
            nguoidung_id: '',
            //người dùng đang đăng nhập hiện tại
            uid: $rootScope.checklogin.uid
        }
        $scope.modelChiTietPlayList.dspnguoidung_id = data.id;
        $scope.modelChiTietPlayList.nguoidung_id = data.nguoidung_id;
        //  $rootScope.kiemTraDSPNguoiDungClick = id_dsp_nguoidung;
        dataservice.taiDSBaiHatTheoDSPNguoiDung_PlayList($scope.modelChiTietPlayList, function (rs) {
            rs = rs.data;
            $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList = rs;
            $scope.themBaiHatVaoDanhSachPhat($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList);

        });                                                                                                                                                           //Bình Luận                                                                                                                                                                                                                     // Sao chép Link                                                                                                                                                                                                                                                                // chia sẻ
    };
    // thêm tất cả bài hát có trong dsp thể loại vào trong danh sách phát đang phát
    $scope.themVaoDSP_DSPTheLoai = function (data) {
        // alert(data.id + data.tendanhsachphattheloai);
        $scope.text = {
            key: '',
            uid: $rootScope.checklogin.uid
        }
        $scope.text.key = data.id;
        dataservice.taiDSPBaiHatTheoDSPTheLoai_DSP($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDSPBaiHatTheoDSPTheLoai_DSP = rs;
            $scope.themBaiHatVaoDanhSachPhat($scope.taiDSPBaiHatTheoDSPTheLoai_DSP);

        });//thêm vào danh sách phát                                                                                                                                                             //Bình Luận                                                                                                                                                                                                                     // Sao chép Link                                                                                                                                                                                                                                                                // chia sẻ
    };
    $scope.themVaoDSP_Top20 = function (data) {
        // alert(data.id + data.tendanhsachphattheloai);
        $scope.text = {
            key: '',
            uid: $rootScope.checklogin.uid
        }
        $scope.text.key = data.danhsachphattheloai_id;
        dataservice.taiDSPBaiHatTheoDSPTop20_DSPTop20($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20 = rs;
            $scope.themBaiHatVaoDanhSachPhat($scope.taiDSPBaiHatTheoDSPTop20_DSPTop20);

        });//Bình Luận                                                                                                                                                                                                                     // Sao chép Link                                                                                                                                                                                                                                                                // chia sẻ
    };
    $scope.copyLinkDSPTheLoai = function (text) {
        navigator.clipboard.writeText(text).then(function () {
            // console.log('Async: Copying to clipboard was successful!');
        }, function (err) {
            // console.error('Async: Could not copy text: ', err);
        });
        //   $("#loaddingdownload").css("display", "block");
    }

    // $scope.subtextMenuChiaSe = '<div id="subtextmenuchiase-node" class="menu share-content submenu-content"> <div id="fb-root"></div><script async defer crossorigin="anonymous" src="https://connect.facebook.net/vi_VN/sdk.js#xfbml=1&version=v11.0" nonce="koYp7ASq"></script ><ul class="menu-list"><li><div class="fb-share-button" data-href="https://localhost:44348/#/baihat/-Me41zpq4XU2sYHyhxgb" data-layout="button" data-size="small"><a target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Flocalhost%3A44348%2F%23%2Fbaihat%2F-Me41zpq4XU2sYHyhxgb&amp;src=sdkpreparse" class="fb-xfbml-parse-ignore">Chia sẻ</a></div></li><li><a class="zalo-share-button zm-btn button" role="button" data-href="https://zingmp3.vn/bai-hat/Yeu-Duoi-Ai-Xem-JayKii/ZU0C7A7B.html" data-customize="true" data-oaid="4073327408156217288"><i class="icon ic- z-ic-svg ic-svg-zalo"></i>Zalo<iframe frameborder="0" allowfullscreen="" scrolling="no" width="0px" height="0px" src="https://sp.zalo.me/plugins/share?dev=null&amp;color=null&amp;oaid=4073327408156217288&amp;href=https%3A%2F%2Fzingmp3.vn%2Fbai-hat%2FYeu-Duoi-Ai-Xem-JayKii%2FZU0C7A7B.html&amp;layout=icon&amp;customize=true&amp;callback=null&amp;id=9304d0de-e447-4632-8fc7-66ecb15b7b9f&amp;domain=zingmp3.vn&amp;android=false&amp;ios=false" style="position: absolute;"></iframe></a></li></ul></div>';
    $scope.playDanhSachPhatTheLoai = function (id_dsp_theloai) {

        //  alert("play nhạc");
        // $scope.biendiem = iddsp;
        $scope.text = {
            key: '',
            uid: $rootScope.checklogin.uid
        }
        $scope.text.key = id_dsp_theloai;
        $rootScope.kiemTraDSPTLClick = id_dsp_theloai;
        dataservice.taiDSPBaiHatTheoDSPTheLoai_DSP($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDSPBaiHatTheoDSPTheLoai_DSP = rs;
            $scope.playMotBanNhac($scope.taiDSPBaiHatTheoDSPTheLoai_DSP);

        });
    }
    // 24/07 top 2 choi nhạc
    $scope.playDanhSachPhatTop20 = function (id_dsp_top20) {

        //  alert("play nhạc");
        // $scope.biendiem = iddsp;
        $scope.text = {
            key: '',
            uid: $rootScope.checklogin.uid
        }
        $scope.text.key = id_dsp_top20;
        $rootScope.kiemTraDSPTop20Click = id_dsp_top20;
        dataservice.taiDSPBaiHatTheoDSPTop20_DSPTop20($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20 = rs;
            $scope.playMotBanNhac($scope.taiDSPBaiHatTheoDSPTop20_DSPTop20);

        });
    }
    // 24/07 top 2 choi nhạc
    $scope.playDanhSachPhatNguoiDung = function (data) {

        //  alert("play nhạc");
        // $scope.biendiem = iddsp;
        $scope.text = {
            key: '',
            uid: $rootScope.checklogin.uid
        }
        $scope.modelChiTietPlayList = {
            dspnguoidung_id: '',
            // người dùng sỡ hữu playlist dspnguoidung
            nguoidung_id: '',
            //người dùng đang đăng nhập hiện tại
            uid: $rootScope.checklogin.uid
        }
        $scope.text.key = data.id;
        $rootScope.kiemTraDSPNguoiDungClick = data.id;
        $scope.modelChiTietPlayList.dspnguoidung_id = data.id;
        $scope.modelChiTietPlayList.nguoidung_id = data.nguoidung_id;
        dataservice.taiDSBaiHatTheoDSPNguoiDung_PlayList($scope.modelChiTietPlayList, function (rs) {
            rs = rs.data;
            $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList = rs;
            $scope.playMotBanNhac($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList);

        });
    }
    // 22/07 tuấn tài contextmenu dsp the loai
    //23/07 tuấn contextmenu bai hat
    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.moChiTietBaiHatDangPhat = function () {
        var data = $rootScope.BaiHatDangPhat;
        $scope.baihatCookie = $rootScope.BaiHatDangPhat;//link hinh                                                                                           // chuyển qua bai hat theo id                                                                                                     //tenbai                                                                                                                                      //luoc yeu thich                                                                                      // luoc nghe                                                                                                                                                           //taiXuong                                                                                                                                                                                             // lời bài hát                                                                                               //chặn                                                                                                                                             //thêm vào danh sách phát                                                                                                                                                         // phát tiếp theo                                                                                                                                //subtextmenuPlaylist                                                                                                               //thêm vào plaulist                                                                                                                                                                                    //Bình Luận                                                                                                                                                                                                                     // Sao chép Link                                                                                                                                                                                                                                                                // chia sẻ
        $scope.contextMenuBaiHatDangPhat = '<div id="contextmenubaihatdangphat" class="zm-contextmenu song-menu" style=" padding:10px 0;"><div class="menu"><ul class="menu-list"><div class="menu-list--submenu"><div class="media song-info-menu"><div class="media-left"><figure class="image is-40x40" style = "width: 65px;"><img style="height:40px;" src="' + data.linkhinhanh + '" alt=""></figure></div><div class="is-w-150 media-content"><a href="/#/baihat/' + data.id + '" class="a-hover"><div class="title-wrapper"><span class="item-title title" title="Sài Gòn Ơi Xin Lỗi Cảm Ơn">' + data.tenbaihat + '</span></div></a><div class="song-stats"><div class="stat-item"><i class="icon far fa-heart iconmusic" aria-hidden="true"></i><span>' + data.luotthich + 'K</span></div><div class="stat-item"><i class="icon fa fa-headphones" aria-hidden="true"></i><span>' + data.luotnghe + 'K</span></div></div></div></div></div></ul><ul class="menu-list"><div class="group-button-menu"><div class="group-button-list"><button class="zm-btn button" ng-click="taiXuong()"><i class="icon fa fa-download" aria-hidden="true"></i><span>Tải xuống</span></button><button class="zm-btn button" tabindex="0" data-toggle="modal" data-target="#myModal"><i class="icon fa fa-book" aria-hidden="true"></i><span>Lời bài hát</span></button><span class="zm-btn button"><i class="icon fa fa-ban" aria-hidden="true"></i><span>Chặn</span></span></div></div></ul><ul class="menu-list"><li><button class="zm-btn button" tabindex="0" ng-click="themBaiHatVaoDanhSachPhat(baihatCookie)"><i class="icon far fa-list-alt"></i><span>Thêm vào danh sách phát</span></button></li><li><button class="zm-btn button" tabindex="0" ng-click="themBaiHatVaoPhatTiepTheo(baihatCookie)"><i class="icon fa fa-play" aria-hidden="true"></i><span>Phát tiếp theo</span></button></li><li ><div class="menu-list--submenu" subtext-menu="subtextMenuPlayList"><button class="zm-btn button" tabindex="0" ng-click="kiemTraThemBaiHatVaoPlayList()"><i class="icon fa fa-plus" aria-hidden="true"></i><span>Thêm vào playlist</span><i class="icon fa fa-chevron-right" style="margin-left: auto;"></i></button></div></li><li><button ng-click="loadBinhLuanBaiHat(' + "'" + data.id + "'" + ')" class="zm-btn button" tabindex="0"><i class="icon far fa-comment"></i><span>Bình luận</span><span class="comment-badge"></span></button></li><li><button class="zm-btn button" ng-click="copyLinkBaiHat(' + "'https://localhost:44348/#/baihat/" + data.id + "'" + ')" tabindex="0"><i class="icon fa fa-link"></i><span>Sao chép link</span></button></li><li><div class="menu-list--submenu" subtext-menu="subtextMenuChiaSe"><button class="zm-btn button" tabindex="0" ng-click="share()"><i class="icon fa fa-share"></i><span>Chia sẻ Facebook</span></button></div></li></ul></div></div>';
    };
    //contexxtmenu bài hát
    $scope.baihatCookie = ""; // lưu dữ liệu một bài hát khi click hiển thị context menu
    // xem contextmenu bai hat khi click
    $scope.moChiTietBaiHat = function (data, vitri) {
        $scope.baihatCookie = data;                                                                                                                                                                                                                                                                                                                                   //link hinh                                                                                           // chuyển qua bai hat theo id                                                                                                     //tenbai                                                                                                                                      //luoc yeu thich                                                                                                          // luoc nghe                                                                                                                                                                              //taiXuong                                                                                                                                                                                                                                     // lời bài hát                                                                                               //chặn                                                                                                                                             //thêm vào danh sách phát                                                                                                                                                         // phát tiếp theo                                                                                                                                //subtextmenuPlaylist                                                                                                               //thêm vào plaulist                                                                                                                                                                                    //Bình Luận                                                                                                                                                                                                                     // Sao chép Link                                                                                                                                                                                                                                                                // chia sẻ       <i class="icon fa fa-chevron-right" style="margin-left: auto;"></i>                                                                                                                                                                                                                                    

        //xài cho các trang ngoại trừ trang playlist <span class="zm-btn button"><i class="icon fa fa-ban" aria-hidden="true"></i><span>Chặn</span></span>
        $scope.contextMenuBaiHat_BaiHat = '<div id="contextmenubaihat_baihat" class="zm-contextmenu song-menu" style=" padding:10px 0;"><div class="menu"><ul class="menu-list"><div class="menu-list--submenu"><div class="media song-info-menu"><div class="media-left"><figure class="image is-40x40" style = "width: 65px;"><img style="height:40px;" src="' + data.linkhinhanh + '" alt=""></figure></div><div class="is-w-150 media-content"><a href="/#/baihat/' + data.id + '" class="a-hover"><div class="title-wrapper"><span class="item-title title" title="Sài Gòn Ơi Xin Lỗi Cảm Ơn">' + data.tenbaihat + '</span></div></a><div class="song-stats"><div class="stat-item"><i class="icon far fa-heart iconmusic" aria-hidden="true"></i><span>' + data.luotthich + 'K</span></div><div class="stat-item"><i class="icon fa fa-headphones" aria-hidden="true"></i><span>' + data.luotnghe + 'K</span></div></div></div></div></div></ul><ul class="menu-list"><div class="group-button-menu"><div class="group-button-list"><button class="zm-btn button" ng-click="taiXuong()"><i class="icon fa fa-download" aria-hidden="true"></i><span>Tải xuống</span></button><button class="zm-btn button" tabindex="0" data-toggle="modal" data-target="#myModal"><i class="icon fa fa-book" aria-hidden="true"></i><span>Lời bài hát</span></button></div></div></ul><ul class="menu-list"><li><button class="zm-btn button" tabindex="0" ng-click="themBaiHatVaoDanhSachPhat(baihatCookie)"><i class="icon far fa-list-alt"></i><span>Thêm vào danh sách phát</span></button></li><li><button class="zm-btn button" tabindex="0" ng-click="themBaiHatVaoPhatTiepTheo(baihatCookie)"><i class="icon fa fa-play" aria-hidden="true"></i><span>Phát tiếp theo</span></button></li><li ><div class="menu-list--submenu" subtext-menu="subtextMenuPlayList"><button class="zm-btn button" tabindex="0" ng-click="kiemTraThemBaiHatVaoPlayList()"><i class="icon fa fa-plus" aria-hidden="true"></i><span>Thêm vào playlist</span><i class="icon fa fa-chevron-right" style="margin-left: auto;"></i></button></div></li><li><button class="zm-btn button" tabindex="0"       ng-click="loadBinhLuanBaiHat(' + "'" + data.id + "'" + ')" ><i class="icon far fa-comment"></i><span>Bình luận</span><span class="comment-badge"></span></button></li><li><button class="zm-btn button" ng-click="copyLinkBaiHat(' + "'https://localhost:44348/#/baihat/" + data.id + "'" + ')" tabindex="0"><i class="icon fa fa-link"></i><span>Sao chép link</span></button></li><li><div class="menu-list--submenu" subtext-menu="subtextMenuChiaSe"><button class="zm-btn button" tabindex="0" ng-click="share()"><i class="icon fa fa-share"></i><span>Chia sẻ Facebook</span></button></div></li></ul></div></div>';
        //xài cho  trang trang playlist
        if ($rootScope.checklogin.uid == $rootScope.idNguoiDungVoiPlayListHienTai) {
            $scope.contextMenuBaiHatPlaylist = '<div id="contextmenubaihat_baihat" class="zm-contextmenu song-menu" style=" padding:10px 0;"><div class="menu"><ul class="menu-list"><div class="menu-list--submenu"><div class="media song-info-menu"><div class="media-left"><figure class="image is-40x40" style = "width: 65px;"><img style="height:40px;" src="' + data.linkhinhanh + '" alt=""></figure></div><div class="is-w-150 media-content"><a href="/#/baihat/' + data.id + '" class="a-hover"><div class="title-wrapper"><span class="item-title title" title="Sài Gòn Ơi Xin Lỗi Cảm Ơn">' + data.tenbaihat + '</span></div></a><div class="song-stats"><div class="stat-item"><i class="icon far fa-heart iconmusic" aria-hidden="true"></i><span>5K</span></div><div class="stat-item"><i class="icon fa fa-headphones" aria-hidden="true"></i><span>122K</span></div></div></div></div></div></ul><ul class="menu-list"><div class="group-button-menu"><div class="group-button-list"><button class="zm-btn button" ng-click="taiXuong()"><i class="icon fa fa-download" aria-hidden="true"></i><span>Tải xuống</span></button><button class="zm-btn button" tabindex="0" data-toggle="modal" data-target="#myModal"><i class="icon fa fa-book" aria-hidden="true"></i><span>Lời bài hát</span></button><span class="zm-btn button"><i class="icon fa fa-ban" aria-hidden="true"></i><span>Chặn</span></span></div></div></ul><ul class="menu-list"><li><button class="zm-btn button" tabindex="0" ng-click="themBaiHatVaoDanhSachPhat(dspCookie)"><i class="icon far fa-list-alt"></i><span>Thêm vào danh sách phát</span></button></li><li><button class="zm-btn button" tabindex="0" ng-click="themBaiHatVaoPhatTiepTheo(dspCookie)"><i class="icon fa fa-play" aria-hidden="true"></i><span>Phát tiếp theo</span></button></li><li ><div class="menu-list--submenu" subtext-menu="subtextMenuPlayList"><button class="zm-btn button" tabindex="0" ng-click="kiemTraThemBaiHatVaoPlayList()"><i class="icon fa fa-plus" aria-hidden="true"></i><span>Thêm vào playlist</span><i class="icon fa fa-chevron-right" style="margin-left: auto;"></i></button></div></li><li><button ng-click="loadBinhLuanBaiHat(' + "'" + data.id + "'" + ')" class="zm-btn button" tabindex="0"><i class="icon far fa-comment"></i><span>Bình luận</span><span class="comment-badge"></span></button></li><li><button class="zm-btn button" ng-click="copyLinkBaiHat(' + "'https://localhost:44348/#/baihat/" + data.id + "'" + ')" tabindex="0"><i class="icon fa fa-link"></i><span>Sao chép link</span></button></li><li><div class="menu-list--submenu" subtext-menu="subtextMenuChiaSe"><button class="zm-btn button" tabindex="0" ng-click="share()"><i class="icon fa fa-share"></i><span>Chia sẻ Facebook</span></button></div></li><li><button class="zm-btn button" tabindex="0" ng-click="xoaBaiHatKhoiPlaylist(' + vitri + ')"><i class="icon fa fa-trash"></i><span>Xóa khỏi playlist này</span><span class="comment-badge"></span></button></li></ul></div></div>';

        } else {
            $scope.contextMenuBaiHatPlaylist = $scope.contextMenuBaiHat_BaiHat;
        }



        $rootScope.viTri = vitri;
        // vị trí bài hát trong danh sách chứa khi click mở context menu
    };
    // tải bài hát xuống máy khi click
    $scope.taiXuong = function () {
        // console.log($rootScope.checklogin.dadangnhap);
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.text.key = $scope.baihatCookie.id;
            $scope.text.uid = $rootScope.checklogin.uid;
            dataservice.taiXuongBaiHat_NguoiDung($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiXuongBaiHat_NguoiDung = rs;
            });
            $scope.texttaixuong = {
                linknhac: '',
                tentaixuong: '',
                key: ''
            }
            $scope.texttaixuong.linknhac = $scope.baihatCookie.link;
            $scope.texttaixuong.tentaixuong = $scope.baihatCookie.tenbaihat + ".mp3";
            $("#loaddingdownload").css("display", "block");
            var downloadbaihat = document.getElementById("btntext");
            setTimeout(function () {
                downloadbaihat.click();
                $("#loaddingdownload1").css("display", "block");
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
            dataservice.downloadBaiHatVeMayNguoiDung($scope.texttaixuong, function (rs) {
                rs = rs.data;
                $scope.downloadBaiHatVeMayNguoiDung = rs;
                if ($scope.downloadBaiHatVeMayNguoiDung != "") {
                    setTimeout(function () {
                        var downloadbaihat = document.getElementById("btntaixuong");
                        downloadbaihat.href = "../../../music/Download/" + $scope.downloadBaiHatVeMayNguoiDung;
                        downloadbaihat.click();
                        setTimeout(function () {
                            dataservice.xoaNhacDaTaiXuong($scope.texttaixuong, function () {
                                rs = rs.data;
                                $scope.xoaNhacDaTaiXuong = rs;
                                $("#loaddingdownload").css("display", "none");
                                $("#loaddingdownload1").css("display", "none");
                            });
                        }, 1000);

                    }, 5000);
                }
            });
        }
        else {
            $rootScope.closelogin = true;
        }
    };
    // copy link bài hát khi click
    $scope.copyLinkBaiHat = function (text) {
        navigator.clipboard.writeText(text).then(function () {
            // console.log('Async: Copying to clipboard was successful!');
        }, function (err) {
            // console.error('Async: Could not copy text: ', err);
        });
        //   $("#loaddingdownload").css("display", "block");
    }

    $scope.kiemTraThemBaiHatVaoPlayList = function () {
        if ($rootScope.checklogin.dadangnhap == true) {

        } else {
            $rootScope.closelogin = true;
        }
    }
    // thêm bài hát vào playlist khi click

    $scope.themBaiHatVaoPlayList = function (DSPPlayList_id) {


        if (DSPPlayList_id != " ") {
            // key = id danhsachphat nguoi dung playlist
            // uid = là id  nguoi dung 
            $scope.text.key = DSPPlayList_id;
            $scope.text.uid = $rootScope.checklogin.uid;
            $scope.kiemTraBaiHatTonTai = true;
            $scope.modelChiTietPlayList = {
                dspnguoidung_id: '',
                // người dùng sỡ hữu playlist dspnguoidung
                nguoidung_id: '',
                //người dùng đang đăng nhập hiện tại
                uid: $rootScope.checklogin.uid
            }
            dataservice.taiChiTietPlayList_PlayList($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiChiTietPlayList_PlayList = rs;
                $scope.modelChiTietPlayList.dspnguoidung_id = DSPPlayList_id;
                $scope.modelChiTietPlayList.nguoidung_id = $scope.taiChiTietPlayList_PlayList[0].nguoidung_id;


                dataservice.taiDSBaiHatTheoDSPNguoiDung_PlayList($scope.modelChiTietPlayList, function (rs) {
                    rs = rs.data;
                    $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList = rs;

                    for (var i = 0; i < $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.length; i++) {

                        if ($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[i].id == $scope.baihatCookie.id) {
                            alert("Bài Hát Đã Có Trong PlayList");
                            $scope.kiemTraBaiHatTonTai = false;
                            return;
                        }
                    };
                    if ($scope.kiemTraBaiHatTonTai == true) {
                        // ubi = id danhsachphat nguoi dung playlist
                        // key là id bài hát
                        $scope.text.key = $scope.baihatCookie.id;
                        $scope.text.uid = DSPPlayList_id;

                        $scope.modelThemBaiHatVaoDSPNguoiDung = {
                            baihat_id: '',
                            uid: $rootScope.checklogin.uid,
                            danhsachphatnguoidung_id: ''
                        }
                        $scope.modelThemBaiHatVaoDSPNguoiDung.baihat_id = $scope.baihatCookie.id;
                        $scope.modelThemBaiHatVaoDSPNguoiDung.danhsachphatnguoidung_id = DSPPlayList_id;

                        dataservice.themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi($scope.modelThemBaiHatVaoDSPNguoiDung, function (rs) {
                            rs = rs.data;
                            $scope.themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi = rs;
                            alert("thêm thành công");
                        });
                    }

                });

            });

            //dataservice.taiDSBaiHatTheoDSPNguoiDung_PlayList($scope.text, function (rs) {
            //    rs = rs.data;
            //    $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList = rs;

            //    for (var i = 0; i < $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.length; i++) {

            //        if ($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[i].id == $scope.baihatCookie.id) {
            //            alert("Bài Hát Đã Có Trong PlayList");
            //            $scope.kiemTraBaiHatTonTai = false;
            //            return;
            //        }
            //    };
            //    if ($scope.kiemTraBaiHatTonTai == true) {
            //        // ubi = id danhsachphat nguoi dung playlist
            //        // key là id bài hát
            //        $scope.text.key = $scope.baihatCookie.id;
            //        $scope.text.uid = DSPPlayList_id;
            //        dataservice.themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi($scope.text, function (rs) {
            //            rs = rs.data;
            //            $scope.themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi = rs;
            //            alert("thêm thành công");
            //        });
            //    }

            //});
        }
        // alert("thêm thành công");



    }
    // yêu thích bài hát khi click bên nhạc mới
    //    < a class="zalo-share-button zm-btn button" role = "button" data - href="https://zingmp3.vn/bai-hat/Yeu-Duoi-Ai-Xem-JayKii/ZU0C7A7B.html" data - customize="true" data - oaid="4073327408156217288" > <i class="icon ic- z-ic-svg ic-svg-zalo"></i>Zalo < iframe frameborder = "0" allowfullscreen = "" scrolling = "no" width = "0px" height = "0px" src = "https://sp.zalo.me/plugins/share?dev=null&amp;color=null&amp;oaid=4073327408156217288&amp;href=https%3A%2F%2Fzingmp3.vn%2Fbai-hat%2FYeu-Duoi-Ai-Xem-JayKii%2FZU0C7A7B.html&amp;layout=icon&amp;customize=true&amp;callback=null&amp;id=9304d0de-e447-4632-8fc7-66ecb15b7b9f&amp;domain=zingmp3.vn&amp;android=false&amp;ios=false" style = "position: absolute;" ></iframe ></a >
    // var modalLoiBaiHat = '<div class="modal fade ng-scope show" id="myModal" role="dialog" style="display: block; padding-right: 16px;"><div class="modal-dialog"><!-- Modal content--><div class="modal-content" style="width:540px !important; background-color: #6a39af; border-radius: 8px;"><div class="add-lyric-content clearfix"><h3 class="title">Lời bài hát</h3><div class="content1"><textarea rows="1" disabled=""> Bài hát: Sài Gòn Mưa Rơi Ca sĩ: Hồ Quang Hiếu Chiều mưa tầm tã không một lời Lặng lẽ cứ thế xa rời Dường như ngày tháng Mình cùng với nhau Chỉ là yêu đương nhất thời Nghe đâMột tcơn mưa Giữa một thành phố xa lạ Liệu trong dòng xe đang lao vội vã Có lúc y nhớ thương Giờ anh ước tan thành khói mây Bay theo em Chiều mưa tầm tã em không một lời Lặng lẽ cứ thế xa rời Dườnn mưa rơi rơi rơi nhanh rất buồn Con tim anh nơi đây vẫn luôn Vẫn cứ nhớ em khôn nguôi Tình làm sao buôn</textarea></div><div class="modal-card-foot"><div class="level"><div class="level-left"><span><a class="add-lyric-btn">Đóng góp lời bài hát</a></span></div><div class="level-right"><button class="zm-btn is-outlined is-medium is-upper button" data-dismiss="modal" tabindex="0"><i class="icon"></i><span>Đóng</span></button></div></div></div></div></div></div></div>';
    //$scope.subtextMenuPlayList = '<div id="subtextmenuplaylist-node" class="menu add-playlist-content submenu-content"><ul class="menu-list"><li class="search-box"><input class="input" placeholder="Tìm playlist"></li><li class="mar-t-10"><button class="zm-btn button" tabindex="0"><i class="icon ic- z-ic-svg ic-svg-add"></i><span>Tạo playlist mới</span></button></li></ul><div class="playlist-container"><div class="top-shadow "></div><div class="content"><div style="position: relative; background-color: #6a39af; overflow: hidden; width: 105%; height: 100%;"><div style="position: absolute; inset: 0px; overflow: hidden scroll; margin-right: -6px; margin-bottom: 0px;"><ul class="menu-list"><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>dsa</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Ok</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Playlist #3</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Playlist #2</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Fill</span></button></li></ul></div><div class="track-horizontal" style="position: absolute; height: 6px; transition: opacity 200ms ease 0s; opacity: 0;"><div style="position: relative; display: block; height: 100%; cursor: pointer; border-radius: inherit; background-color: rgba(0, 0, 0, 0.2); width: 0px;"></div></div><div class="track-vertical" style="position: absolute; width: 4px; transition: opacity 200ms ease 0s; opacity: 0; right: 2px; top: 2px; bottom: 2px; z-index: 100;"><div class="thumb-vertical" style="position: relative; display: block; width: 100%; height: 0px;"></div></div></div></div></div></div>';
    //$scope.subtextMenuChiaSe = '<div id="subtextmenuchiase-node" class="menu share-content submenu-content"><ul class="menu-list"><li><button class="zm-btn button" tabindex="0" ng-click="share()"><i class="icon ic-list-music"></i><span>Chia Sẻ FB</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Chia Sẻ ZaLo</span></button></li></ul></div>';
    $scope.share = function () {
        u = "https://localhost:44348/#/baihat/" + $scope.baihatCookie.id;
        t = $scope.baihatCookie.tenbaihat;
        window.open('http://www.facebook.com/sharer.php?u=' + encodeURIComponent(u) + '&t=' + encodeURIComponent(t), 'sharer', 'toolbar=0,status=0,width=626,height=436');
    }
    //23/07 tuấn contextmenu bai hat
    //24/07 xoay hình
    //$scope.playDanhSachPhatBaiHat_BaiHat = function (id_dsp_baihat) {

    //    $rootScope.kiemTraDSPTLClick = id_dsp_baihat;

    //}   
    // 27/07
    $scope.moTaoPlaylist = function () {
        $scope.checktaoplaylist = true;
    }








});

//17/08 ĐÃ SỮA CSDL MỚI
app.controller('index', function ($scope, dataservice, $rootScope, $location) {
    $scope.yeuThichTop20Ui = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiDanhSachPhatTop20_khampha[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiDanhSachPhatTop20_khampha[viTri].yeuthich = 0;
            } else {

                $scope.taiDanhSachPhatTop20_khampha[viTri].yeuthich = 1;
            }
        }
    }
    $scope.initData = function () {

        //$scope.huyhengioshow = 1;
        //$scope.xacthucemail = true;
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;

                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }
                        })
                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            $scope.baiHatDangPhat = 0;

            dataservice.getListBaiHatMoi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.baiHatMoi = rs;
                $rootScope.songs = rs;

            })
            dataservice.getListBaiHatNgheNhieuNhat($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.ngheNhieuNhat = rs;
            });

            dataservice.getListBaiHatLikeNhieuNhat($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.likeNhieuNhat = rs;
            });
            dataservice.getListBaiHatDowloadNhieuNhat($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.dowloadNhieuNhat = rs;
            });
            dataservice.gettopNgheSi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.topNgheSi = rs;
                }
            })
            dataservice.taiDanhSachPhatTop20_khampha($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.taiDanhSachPhatTop20_khampha = rs;
                }
            })
        })
  
        $scope.clickBaiHatMoi = function (index) {
            $rootScope.songs = [];
            $scope.baiHatDangPhat = 1;
            $rootScope.songs.push($scope.baiHatMoi[index]);
            var song = event.target.closest(".baihat_moi:not(.baihat_moi_click)")
            if (song) {

                $scope.clickBaiHatMoiActive = Number(index);
                $scope.playmusicBaiHatMoiNhat = $scope.clickBaiHatMoiActive;
                $scope.loaddefaultmusic(0);
                $scope.setAutoPlayNhac();
                $scope.Play();

            }
        }
        $scope.theoDoi_index = function (theodoi) {
            if ($rootScope.checklogin.dadangnhap) {
                console.log($rootScope.checklogin);
                $scope.modelTheoDoi = {
                    id: '',
                    nguoidung_id: $rootScope.checklogin.uid,
                    nguoidung_theodoi_id: $scope.topNgheSi[theodoi].uid
                }
                //dã sữa csdl moi
                dataservice.theoDoiNguoiDung($scope.modelTheoDoi, function (rs) {
                    rs = rs.data;
                    if (rs) {
                        $scope.topNgheSi[theodoi].theodoi = 1;
                    }
                    else {
                        $scope.topNgheSi[theodoi].theodoi = 0;
                    }
                })
            }
            else {
                $rootScope.closelogin = true;
            }

        }
        $scope.clickBaiHatNgheNhieuNhat = function (index) {

            $rootScope.songs = [];
            $scope.baiHatDangPhat = 2;
            $rootScope.songs.push($scope.ngheNhieuNhat[index]);

            var song = event.target.closest(".baihat_moi_list:not(.baihat_moi_click)")
            if (song) {

                $scope.playNgheNhieuNhat = Number(index);
                $scope.loaddefaultmusic(0);
                $scope.setAutoPlayNhac();
                $scope.playmusicNgheNhieuNhat = 1;
                $scope.Play();

            }
        }
        $scope.clickBaiHatLikeNhieuNhat = function (index) {

            $rootScope.songs = [];
            $scope.baiHatDangPhat = 3;
            $rootScope.songs.push($scope.likeNhieuNhat[index]);
            //$rootScope.songs.push($scope.baiHatMoi[0]);
            //$rootScope.songs.push($scope.baiHatMoi[1]);
            //$rootScope.songs.push($scope.baiHatMoi[2]);
            //$rootScope.songs.push($scope.baiHatMoi[3]);

            var song = event.target.closest(".baihat_moi_list:not(.baihat_moi_click)")
            if (song) {

                $scope.playLikeNhieuNhat = Number(index);
                $scope.loaddefaultmusic(0);
                $scope.setAutoPlayNhac();
                $scope.playmusicLikeNhieuNhat = 1;
                $scope.Play();

            }
        }

        $scope.clickBaiHatDowloadNhieuNhat = function (index) {

            $rootScope.songs = [];
            $scope.baiHatDangPhat = 4;
            $rootScope.songs.push($scope.dowloadNhieuNhat[index]);
            //$rootScope.songs.push($scope.baiHatMoi[0]);
            //$rootScope.songs.push($scope.baiHatMoi[1]);
            //$rootScope.songs.push($scope.baiHatMoi[2]);
            //$rootScope.songs.push($scope.baiHatMoi[3]);

            var song = event.target.closest(".baihat_moi_list:not(.baihat_moi_click)")
            if (song) {

                $scope.playDowloadNhieuNhat = Number(index);
                $scope.loaddefaultmusic(0);
                $scope.setAutoPlayNhac();
                $scope.playmusicDowloadNhieuNhat = 1;
                $scope.Play();

            }
        }

    }
    $scope.initData();


});
app.controller('theodoi', function ($rootScope, $scope, $cookies, dataservice, $interval, $timeout) {

    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;

                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }
                        })
                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            dataservice.getListTheoDoi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.listTheoDoi = rs;
                    /* console.log($scope.listTheoDoi[0].baihat[0].baihat.linkhinhanh);*/
                }
            })
        });


        $scope.data = $rootScope.songs;

        $scope.checkindexli = -1;
        $scope.checkindexsecond = 0;
        $interval(function () {

            $scope.checkindexli++;
            $scope.checkindexsecond++;
            if ($scope.checkindexli >= 5) {
                $scope.checkindexli = 0;
            }

            if ($scope.checkindexsecond >= 5) {
                $scope.checkindexsecond = 0;
            }

        }, 2000);

    }
    $scope.theoDoi_TheoDoi = function (theodoi) {
        if ($rootScope.checklogin.dadangnhap) {

            $scope.modelTheoDoi = {
                id: '',
                nguoidung_id: $rootScope.checklogin.uid,
                nguoidung_theodoi_id: $scope.listTheoDoi[theodoi].nguoidung.uid
            }
            dataservice.theoDoiNguoiDung($scope.modelTheoDoi, function (rs) {
                rs = rs.data;
                if (rs) {
                    $scope.listTheoDoi[theodoi].nguoidung.theodoi = 1;
                }
                else {
                    $scope.listTheoDoi[theodoi].nguoidung.theodoi = 0;
                }
            })
        }
        else {
            $rootScope.closelogin = true;
        }

    }


    $scope.initData();
})
app.controller('canhan', function ($scope, dataservice, $rootScope, $location, $interval) {
    $scope.tabActiveClick = function (index) {
        if (index == 0) {
            $scope.tabactive = 0;
            dataservice.getListBaiHatYeuThich($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) {

                } else {
                    $scope.listBaiHatYeuThich = rs;
                }
            });
            dataservice.getListYeuThichDSP_canhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) {

                } else {
                    $scope.listDanhSachPhatYeuThich = rs;
                }
            })
            dataservice.getListYeuThichTop20_canhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) {

                } else {
                    $scope.listTop20YeuThich = rs;
                }
            })
            dataservice.getListYeuThichDSPNgheSi_canhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.listDSPyeuThichNgheSi = rs;
                }
            })
        }
        if (index == 1) {
            $scope.tabactive = 1;
            dataservice.getListDaTaiXuong_CaNhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.listDaTaiXuong_CaNhan = rs;
                }
            })

        }
        if (index == 2) {
            $scope.tabactive = 2;
            dataservice.getPlaylist_CaNhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $rootScope.playlist_canhan = rs;
                }
            });
        }
        if (index == 3) {
            $scope.tabactive = 3;
            dataservice.getListDaTaiLen_CaNhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data
                if (rs.Error) { } else {
                    $scope.listBaiHatDaTaiLen = rs;
                }
            })
        }

    }
    $scope.doiMatKhau = function () {

        firebase.auth().onAuthStateChanged((user) => {
            if (user) {
                const user = firebase.auth().currentUser;
                if (user != null) {
                    $scope.emailUser = user.email;
                    $scope.provider = user.providerData[0].providerId;
                }
            } else {
                // User is signed out
                // ...
            }
        });

    }
    $scope.doiMatKhauTaiKhoan = function (matkhaucu, matkhaumoi, matkhaumoi2) {
        $("#loading_main").css("display", "block");
        if (matkhaucu != matkhaumoi) {
            //$scope.matkhaumoi = matkhaumoi;
            //$scope.matkhaumoi2 = matkhaumoi2;
            if (matkhaumoi == matkhaumoi2 && matkhaumoi.length > 5 && matkhaumoi.length < 31 && matkhaumoi2.length > 5 && matkhaumoi2.length < 31) {
                var promise = new Promise(function (resolve, reject) {
                    firebase.auth().onAuthStateChanged((user) => {
                        if (user) {
                            const user = firebase.auth().currentUser;
                            if (user != null) {
                                $scope.emailUser = user.email;

                                resolve();
                            }
                        } else {
                            // User is signed out
                            // ...
                        }
                    });

                });
                promise.then(function () {
                    const user = firebase.auth().currentUser;
                    var credential = firebase.auth.EmailAuthProvider.credential($scope.emailUser, matkhaucu);

                    user.reauthenticateWithCredential(credential).then(() => {
                        user.updatePassword(matkhaumoi).then(() => {
                            $("#loading_main").css("display", "none");
                            $scope.doimatkhau = false;
                            var downloadbaihat = document.getElementById("btntext");
                            downloadbaihat.click();
                        }).catch((error) => {
                            // An error ocurred
                            // ...
                        });
                    }).catch((error) => {
                        $scope.saiDuLieuDoiMatKhau = 1;
                        $("#loading_main").css("display", "none");
                        var downloadbaihat = document.getElementById("btntext");
                        downloadbaihat.click();
                    });
                });
            } else {
                $scope.saiDuLieuDoiMatKhau = 4;
                $("#loading_main").css("display", "none");
            }

        }
        else {
            $scope.saiDuLieuDoiMatKhau = 3;
            $("#loading_main").css("display", "none");
        }

    }
    $scope.doiEmailTaiKhoan = function (matkhaucu, emailmoi) {
        $("#loading_main").css("display", "block");
        var partternEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (matkhaucu.length > 5 && emailmoi.length > 5) {
            if (emailmoi != '' && emailmoi != null && partternEmail.test(emailmoi)) {
                var promise = new Promise(function (resolve, reject) {
                    firebase.auth().onAuthStateChanged((user) => {
                        if (user) {
                            const user = firebase.auth().currentUser;
                            if (user != null) {
                                $scope.emailUser = user.email;

                                resolve();
                            }
                        } else {
                            // User is signed out
                            // ...
                        }
                    });

                });
                promise.then(function () {
                    const user = firebase.auth().currentUser;
                    var credential = firebase.auth.EmailAuthProvider.credential($scope.emailUser, matkhaucu);

                    user.reauthenticateWithCredential(credential).then(() => {


                        user.updateEmail(emailmoi).then(() => {
                            firebase.auth().currentUser.sendEmailVerification()
                                .then(() => {
                                    // Email verification sent!
                                    // ...
                                });
                            $scope.text = {
                                key: emailmoi,
                                uid: $rootScope.checklogin.uid
                            }
                            dataservice.ChinhSuaEmail($scope.text, function (rs) {
                                rs = rs.data;
                                if (rs.Error) {
                                } else {
                                    $("#loading_main").css("display", "none");
                                    $scope.doiemail = false;
                                    $scope.xacnhanEmail = true;
                                    var downloadbaihat = document.getElementById("btntext");
                                    downloadbaihat.click();
                                }
                            })



                        }).catch((error) => {
                            $scope.saiDuLieuDoiEmail = 3;
                        });

                    }).catch((error) => {
                        $scope.saiDuLieuDoiEmail = 1;
                        $("#loading_main").css("display", "none");
                        var downloadbaihat = document.getElementById("btntext");
                        downloadbaihat.click();
                    });
                });
            }
            else {
                $("#loading_main").css("display", "none");
                $scope.saiDuLieuDoiEmail = 2;
            }



        }


    }
    $scope.guiEmailXacMinh = function () {
        $scope.time = 60;

        if (!$scope.kiemtraXacThuc) {
            firebase.auth().currentUser.sendEmailVerification()
                .then(() => {

                    var timeXacThuc = $interval(function () {
                        if ($scope.time == 0) {
                            $interval.cancel(timeXacThuc);
                            $scope.kiemtraXacThuc = false;
                            $scope.titleXacThucEmail = "Gửi lại mã xác thực"
                        }
                        else {
                            $scope.time--;
                            $scope.titleXacThucEmail = "Đợi " + $scope.time + "s " + "để gửi lại mã";
                            $scope.kiemtraXacThuc = true;
                        }

                    }, 1000);
                });
        }
        else {

        }

    }
    $scope.toggleYeuThichBHDaLuu_CaNhan = function (index) {

        $scope.yeuThichbhmodel = {
            id: '',
            nguoidung_id: $rootScope.checklogin.uid,
            baihat_id: $scope.listDaTaiXuong_CaNhan[index].id,
        };
        dataservice.yeuThichBaiHat($scope.yeuThichbhmodel, function (rs) {
            rs = rs.data;
            if (rs.Error) { }
            else {
                if (rs) {
                    $scope.listDaTaiXuong_CaNhan[index].yeuthich = 1;
                }
                else
                    $scope.listDaTaiXuong_CaNhan[index].yeuthich = 0;
            }
        });
    }
    $scope.toggleYeuThichBHDaTaiLen_CaNhan = function (index) {

        $scope.yeuThichbhmodel = {
            id: '',
            nguoidung_id: $rootScope.checklogin.uid,
            baihat_id: $scope.listBaiHatDaTaiLen[index].id,
        };
        dataservice.yeuThichBaiHat($scope.yeuThichbhmodel, function (rs) {
            rs = rs.data;
            if (rs.Error) { }
            else {
                if (rs) {
                    $scope.listBaiHatDaTaiLen[index].yeuthich = 1;
                }
                else
                    $scope.listBaiHatDaTaiLen[index].yeuthich = 0;
            }
        });
    }
    $scope.blurSetting = function () {
        if ($scope.checked) {
            if (!$scope.settingFocus) {
                $scope.checked = false;
            }
        }

    }

    $scope.showSettingfocus = function () {
        $scope.checked = true;
    }
    $scope.mousedowmDangXuat = function () {
        $scope.settingFocus = true;
    }
    $scope.mouseupDangXuat = function () {

        $scope.settingFocus = false;
        var promise = new Promise(function (resolve, reject) {
            $scope.signOut();
            resolve();
        });
        promise.then(function () {
            window.location.assign("/");
        }).catch(function () {
            alert("Lỗi Đăng Xuất");
        });
    };
    $scope.mousedowmDoiMatKhau = function () {
        $scope.settingFocus = true;
    }
    $scope.mouseupDoiMatKhau = function () {

        $scope.settingFocus = false;
        $scope.checked = false;
        $("#loading_main").css("display", "block");

        var promise = new Promise(function (resolve, reject) {

            $scope.doiMatKhau();
            resolve();

        });
        promise.then(function () {

            if ($scope.provider == 'password') {
                $("#loading_main").css("display", "none");
                $scope.doimatkhau = true;
                var downloadbaihat = document.getElementById("btntext");
                downloadbaihat.click();
            }
            else {
                $("#loading_main").css("display", "none");
                $scope.taiKhoanDangNhapsai = true;
                var downloadbaihat = document.getElementById("btntext");
                downloadbaihat.click();
            }

        });
    };
    $scope.mousedowmDoiEmail = function () {
        $scope.settingFocus = true;
    }
    $scope.mouseupDoiEmail = function () {

        $scope.settingFocus = false;
        $scope.checked = false;
        $("#loading_main").css("display", "block");

        var promise = new Promise(function (resolve, reject) {

            $scope.doiMatKhau();
            resolve();

        });
        promise.then(function () {

            if ($scope.provider == 'password') {
                $("#loading_main").css("display", "none");
                $scope.doiemail = true;
                var downloadbaihat = document.getElementById("btntext");
                downloadbaihat.click();
            }
            else {
                $("#loading_main").css("display", "none");
                $scope.taiKhoanDangNhapsai = true;
                var downloadbaihat = document.getElementById("btntext");
                downloadbaihat.click();
            }

        });
    };
    $scope.viTriBanNhac = function (index) {
        $scope.checkplaymusic = index;
    }
    $scope.viTriBanNhacDaLuu = function (index) {
        $scope.checkplaymusicdaluu = index;
    }
    $scope.viTriBanNhacTaiLen = function (index) {
        $scope.checkplaymusictailen = index;
    }
    $scope.clickedit = function () {
        $scope.editMoTa = !$scope.editMoTa;

    }
    $scope.initData = function () {
        if ($scope.tabactive == null || $scope.tabactive == undefined) {
            $scope.tabactive = 0;
        }
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;

                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }
                        })
                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            $scope.editHoTen = false;
            $scope.editMoTa = false;
            dataservice.getListBaiHatYeuThich($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) {

                } else {
                    $scope.listBaiHatYeuThich = rs;
                }
            });
            dataservice.getPlaylist_CaNhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $rootScope.playlist_canhan = rs;
                }
            });
            dataservice.getListYeuThichDSP_canhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) {

                } else {
                    $scope.listDanhSachPhatYeuThich = rs;
                }
            })
            dataservice.getListYeuThichTop20_canhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) {

                } else {
                    $scope.listTop20YeuThich = rs;
                }
            })
            dataservice.getListDaTaiLen_CaNhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data
                if (rs.Error) { } else {
                    $scope.listBaiHatDaTaiLen = rs;
                }
            })
            dataservice.getListYeuThichDSPNgheSi_canhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.listDSPyeuThichNgheSi = rs;
                }
            })
            dataservice.getListDaTaiXuong_CaNhan($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.listDaTaiXuong_CaNhan = rs;
                }
            })
            dataservice.getListHoaDon($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.listHoaDonNguoiDung = rs;
                }
            })
        });

        //$scope.apply();
    }

    $scope.initData();

    // 02/08 suữa ảnh cá nhân
    var duLieuHinh = new FormData();

    // 02/08 suữa ảnh cá nhân
    $scope.getTheFilesHinhAnhNguoiDung = function ($files) {
        $scope.BienTam = {
            linkhinhanh: '',
            bangnguoidung_id: '',
            uid: $rootScope.checklogin.uid,
            baihat_id: ''
        }
        //dataservice.suaLinkHinhAnhNguoiDung_CaNhan($scope.BienTam, function (rs) {
        //            rs = rs.data;
        //        });
        if ($files[0].type == "image/png" || $files[0].type == "image/jpeg") {
            duLieuHinh = new FormData();
            duLieuHinh.append("File1", $files[0]);
            duLieuHinh.append("uid", $rootScope.checklogin.uid);
            dataservice.uploadHinhAnhNguoiDung(duLieuHinh, function (rs) {
                rs = rs.data;
                $rootScope.checklogin.hinhanh = rs;
                $scope.BienTam.linkhinhanh = rs;
                $scope.BienTam.uid = $rootScope.checklogin.uid;
                dataservice.suaLinkHinhAnhNguoiDung_CaNhan($scope.BienTam, function (rs) {
                    rs = rs.data;
                });
            });
        } else {
            alert("Sai định đạng ảnh (*.jpg, *.png)");
        }

        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("File", $files[i]);
        //}

    }

    $scope.chinhSuaHoTen = function () {
        $scope.BienTam = {
            linkhinhanh: '',
            bangnguoidung_id: '',
            uid: $rootScope.checklogin.uid,
            baihat_id: '',
            hoten: '',
            mota: ''
        }
        $scope.BienTam.hoten = $scope.modelhoten;

        $rootScope.checklogin.hovaten = $scope.modelhoten;
        $scope.editHoTen = false;
        dataservice.chinhSuaHoTen_CaNhan($scope.BienTam, function (rs) {
            rs = rs.data;

        })
    }
    $scope.chinhSuaMoTa = function () {
        $scope.BienTam = {
            linkhinhanh: '',
            bangnguoidung_id: '',
            uid: $rootScope.checklogin.uid,
            baihat_id: '',
            hoten: '',
            mota: ''
        }
        $scope.BienTam.mota = $scope.modelmota;
        $rootScope.checklogin.mota = $scope.modelmota;
        $scope.editMoTa = false;
        dataservice.chinhSuaMoTa_CaNhan($scope.BienTam, function (rs) {
            rs = rs.data;

        })
    }
    $scope.showXoaDanhSachPhat = function (id) {
        if (id != null && id != "" && id != undefined) {
            $scope.idxoaDanhSachPhat = id;
            $scope.xoaDanhSachPhatNguoiDung = true;
        }
    }
    $scope.xoaDanhSachPhat = function (id) {
        if ($rootScope.checklogin.dadangnhap) {
            if (id != null && id != "" && id != undefined) {
                $scope.Text = {
                    key: id,
                    uid: $rootScope.checklogin.uid
                }
                dataservice.XoaDanhSachPhatNguoiDung($scope.Text, function (rs) {
                    rs = rs.data;
                    if (rs == true) {
                        /* =$rootScope.playlist_canhan.findIndex(p => p.id == "123");*/
                        var found = $rootScope.playlist_canhan.find(function (item) { return item.id === id });
                        var idx = $rootScope.playlist_canhan.indexOf(found);
                        // Is currently selected
                        if (idx > -1) {
                            $rootScope.playlist_canhan.splice(idx, 1);
                        }

                    }
                });
            }
        }
        else {
            $rootScope.closelogin = true;
        }

    }


});
app.controller('timkiem', function ($scope, $routeParams, dataservice, $rootScope, $location) {
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;

                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }
                        })
                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            $scope.activeTimKiem = 0;
            $scope.tuKhoaTimKiem = $routeParams.tukhoa;
            $scope.modelTimKiem = {
                tuKhoa: $scope.tuKhoaTimKiem,
                uid: $rootScope.checklogin.uid == "" ? null : $rootScope.checklogin.uid
            }
            dataservice.timKiemDanhSachPhatNguoiDung($scope.modelTimKiem, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.danhSachPhatNguoiDungTimKiem = rs;
                }
            });

            dataservice.TimKiemBaiHatAll($scope.modelTimKiem, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.danhSachBaiHatTimKiem = rs;
                }
            })
            dataservice.timKiemDanhSachPhatTheLoai($scope.modelTimKiem, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.danhSachPhatTheLoaiTimKiem = rs;
                }
            })
            dataservice.timKiemNguoiDungCustom($scope.modelTimKiem, function (rs) {
                rs = rs.data;
                if (rs.Error) { } else {
                    $scope.danhsachNgheSi = rs;
                }
            })
        });
        $scope.yeuThichDanhSachPhatTimKiem = function (index) {
            if ($rootScope.checklogin.dadangnhap) {
                $scope.yeuThichDSPModel = {
                    id: '',
                    nguoidung_id: $rootScope.checklogin.uid,
                    danhsachphat_id: $scope.danhSachPhatTheLoaiTimKiem[index].id,
                };
                dataservice.yeuThichDanhSachPhat($scope.yeuThichDSPModel, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { }
                    else {
                        if (rs) {
                            $scope.danhSachPhatTheLoaiTimKiem[index].yeuthich = 1;
                        }
                        else
                            $scope.danhSachPhatTheLoaiTimKiem[index].yeuthich = 0;
                    }
                });
            }
            else {
                $rootScope.closelogin = true;
            }

        }
        $scope.yeuThichDanhSachPhatNguoiDung = function (index) {
            $scope.danhSachPhatNguoiDungTimKiem[index].yeuthich == 0 ? $scope.danhSachPhatNguoiDungTimKiem[index].yeuthich = 1 : $scope.danhSachPhatNguoiDungTimKiem[index].yeuthich = 0;

        }
        $scope.TimKiemTheoDanhMuc = function (index) {
            if (index == 0) {
                $scope.activeTimKiem = 0;
                dataservice.TimKiemBaiHatAll($scope.modelTimKiem, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $scope.danhSachBaiHatTimKiem = rs;
                    }
                })

                dataservice.timKiemDanhSachPhatTheLoai($scope.tuKhoaTimKiem, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $scope.danhSachPhatTheLoaiTimKiem = rs;
                    }
                })
            }
            if (index == 1) {
                $scope.activeTimKiem = 1;
                dataservice.TimKiemBaiHatAll($scope.modelTimKiem, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $scope.danhSachBaiHatTimKiem = rs;
                    }
                })
            }
            if (index == 2) {
                $scope.activeTimKiem = 2;
                dataservice.timKiemDanhSachPhatTheLoai($scope.modelTimKiem, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $scope.danhSachPhatTheLoaiTimKiem = rs;
                    }
                })
            }
            if (index == 3) {
                $scope.activeTimKiem = 3;
            }
        }

    }
    $scope.viTriBanNhacTimKiem = function (index) {
        $scope.checkplaymusictimkiem = index;
    }
    $scope.initData();
    $scope.toggleYeuThichBH_TimKiem = function (index) {

        $scope.yeuThichbhmodel = {
            id: '',
            nguoidung_id: $rootScope.checklogin.uid,
            baihat_id: $scope.danhSachBaiHatTimKiem[index].id,
        };
        if ($rootScope.checklogin.dadangnhap) {
            dataservice.yeuThichBaiHat($scope.yeuThichbhmodel, function (rs) {
                rs = rs.data;
                if (rs.Error) { }
                else {
                    if (rs) {
                        $scope.danhSachBaiHatTimKiem[index].yeuthich = 1;
                    }
                    else
                        $scope.danhSachBaiHatTimKiem[index].yeuthich = 0;
                }
            });
        }
        else {
            $rootScope.closelogin = true;
        }

    }
    $scope.theoDoi_Timkiem = function (theodoi) {
        if ($rootScope.checklogin.dadangnhap) {

            $scope.modelTheoDoi = {
                id: '',
                nguoidung_id: $rootScope.checklogin.uid,
                nguoidung_theodoi_id: $scope.danhsachNgheSi[theodoi].uid
            }
            dataservice.theoDoiNguoiDung($scope.modelTheoDoi, function (rs) {
                rs = rs.data;
                if (rs) {
                    $scope.danhsachNgheSi[theodoi].theodoi = 1;
                }
                else {
                    $scope.danhsachNgheSi[theodoi].theodoi = 0;
                }
            })
        }
        else {
            $rootScope.closelogin = true;
        }

    }
})
app.controller('nghesi', function ($scope, $routeParams, dataservice, $rootScope, $location, $interval) {
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;

                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }
                        })
                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            $scope.checkindexli = -1;
            $scope.checkindexsecond = 0;
            $scope.tabactiveIndex = 0;
            $scope.modelNgheSi = {
                uidNguoiDung: $rootScope.checklogin.uid,
                uidNgheSi: $routeParams.uid
            }

            if ($routeParams.uid.length > 0) {
                $scope.text = {
                    key: $rootScope.checklogin.uid,
                    uid: $routeParams.uid
                }
                dataservice.timKiemNgheSi($scope.modelNgheSi, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $scope.NgheSi = rs;
                    }
                })
                dataservice.getListDaTaiLen_NgheSi($scope.modelNgheSi, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $scope.listDaTaiLenNgheSi = rs;
                    }
                })
                dataservice.getListDanhSachPhatNguoiDung($scope.text, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { } else {
                        $scope.listDSPNgheSi = rs;
                    }
                })

            }
        });
        $scope.checkindexli = -1;
        $scope.checkindexsecond = 0;
        $interval(function () {

            $scope.checkindexli++;
            $scope.checkindexsecond++;
            if ($scope.checkindexli >= $scope.listDaTaiLenNgheSi.lenght) {
                $scope.checkindexli = 0;
            }

            if ($scope.checkindexsecond >= $scope.listDaTaiLenNgheSi.lenght) {
                $scope.checkindexsecond = 0;
            }

        }, 2000);
        $scope.tabactiveNgheSi = function (index) {
            if (index == 0) {
                $scope.tabactiveIndex = 0;
            }
            if (index == 1) {
                $scope.tabactiveIndex = 1;
            }
            if (index == 2) {
                $scope.tabactiveIndex = 2;
            }
            if (index == 3) {
                $scope.tabactiveIndex = 3;
            }
        }
        $scope.viTriBanNhacNgheSi = function (index) {
            $scope.checkplaymusicnghesi = index;
        }
        $scope.toggleYeuThichBH_NgheSi = function (index) {
            if ($rootScope.checklogin.dadangnhap) {
                $scope.yeuThichbhmodel = {
                    id: '',
                    nguoidung_id: $rootScope.checklogin.uid,
                    baihat_id: $scope.listDaTaiLenNgheSi[index].id,
                };
                dataservice.yeuThichBaiHat($scope.yeuThichbhmodel, function (rs) {
                    rs = rs.data;
                    if (rs.Error) { }
                    else {
                        if (rs) {
                            $scope.listDaTaiLenNgheSi[index].yeuthich = 1;
                        }
                        else
                            $scope.listDaTaiLenNgheSi[index].yeuthich = 0;
                    }
                });
            } else {
                $rootScope.closelogin = true;
            }

        }
        $scope.theoDoi_NgheSi = function (theodoi) {
            if ($rootScope.checklogin.dadangnhap) {

                $scope.modelTheoDoi = {
                    id: '',
                    nguoidung_id: $rootScope.checklogin.uid,
                    nguoidung_theodoi_id: $scope.NgheSi[theodoi].uid
                }
                dataservice.theoDoiNguoiDung($scope.modelTheoDoi, function (rs) {
                    rs = rs.data;
                    if (rs) {
                        $scope.NgheSi[theodoi].theodoi = 1;
                    }
                    else {
                        $scope.NgheSi[theodoi].theodoi = 0;
                    }
                })
            }
            else {
                $rootScope.closelogin = true;
            }

        }
        $scope.yeuThichDanhSachPhatNgheSi = function (index) {
            $scope.listDSPNgheSi[index].yeuthich == 0 ? $scope.listDSPNgheSi[index].yeuthich = 1 : $scope.listDSPNgheSi[index].yeuthich = 0;

        }


    }

    $scope.initData();





})
app.directive("animate", function () {
    animate = {};
    //contextMenu.restrict = "AE";
    animate.trigger = "left";
    /*  contextMenu = { replace: false };*/
    /*contextMenu.restrict = "A";*/
    animate.link = function (lScope, lElem, lAttr) {

        $('.add-to-cart').on('click', function (e) {
            //  e.preventDefault();
            var cart = $('.shopping-cart');
            var imgtodrag = $(this).parent('.item').find("img").eq(0).removeAttr("hidden");

            if (imgtodrag) {

                var imgclone = imgtodrag.clone()
                    .offset({
                        top: imgtodrag.offset().top,
                        left: imgtodrag.offset().left
                    })
                    .removeClass()
                    .css({
                        'opacity': '0.7',
                        'position': 'absolute',
                        'height': '80px  ',
                        'width': '80px',
                        'z-index': '100',
                        'border-radius': '999px',
                        'display': 'block'
                    })
                    .appendTo($('body'))
                    .animate({
                        'top': cart.offset().top + 10,
                        'left': cart.offset().left + 10,
                        'width': 80,
                        'height': 80
                    }, 2000, 'easeInOutExpo');

                setTimeout(function () {
                    cart.effect("shake", {
                        times: 2
                    }, 200);
                }, 1500);

                imgclone.animate({
                    'width': 0,
                    'height': 0,
                    'opacity': 0
                }, function () {
                    $(this).detach()
                });
            }

        });
    };
    return animate;
});
app.directive("animatedsp", function () {
    animatedsp = {};
    //contextMenu.restrict = "AE";
    animatedsp.trigger = "left";
    /*  contextMenu = { replace: false };*/
    /*contextMenu.restrict = "A";*/
    animatedsp.link = function (lScope, lElem, lAttr) {

        $('.add-to-cartdsp').on('click', function (e) {
            e.preventDefault();
            var cart = $('.shopping-cart');
            var imgtodrag = $(this).parent('.itemdsp').parent('.itemdsp1').parents('.item').find("img").eq(0);


            if (imgtodrag) {

                var imgclone = imgtodrag.clone()
                    .offset({
                        top: imgtodrag.offset().top,
                        left: imgtodrag.offset().left
                    })
                    .removeClass()
                    .css({
                        'opacity': '0.8',
                        'position': 'absolute',
                        'height': '80px  ',
                        'width': '80px',
                        'z-index': '100',
                        'border-radius': '999px'
                    })
                    .appendTo($('body'))
                    .animate({
                        'top': cart.offset().top + 10,
                        'left': cart.offset().left + 10,
                        'width': 80,
                        'height': 80
                    }, 1500, 'easeInOutExpo');

                setTimeout(function () {
                    cart.effect("shake", {
                        times: 2
                    }, 200);
                }, 1500);

                imgclone.animate({
                    'width': 0,
                    'height': 0
                }, function () {
                    $(this).detach()
                });
            }

        });
    };
    return animatedsp;
});
app.directive("animatechitiet", function () {
    animatechitiet = {};
    //contextMenu.restrict = "AE";
    animatechitiet.trigger = "left";
    /*  contextMenu = { replace: false };*/
    /*contextMenu.restrict = "A";*/
    animatechitiet.link = function (lScope, lElem, lAttr) {

        $('.add-to-cartchitiet').on('click', function (e) {
            e.preventDefault();
            var cart = $('.shopping-cart');
            var imgtodrag = $(this).parent('.itemchitiet').parents('.item').find("img").eq(0);


            if (imgtodrag) {

                var imgclone = imgtodrag.clone()
                    .offset({
                        top: imgtodrag.offset().top,
                        left: imgtodrag.offset().left
                    })
                    .removeClass()
                    .css({
                        'opacity': '0.8',
                        'position': 'absolute',
                        'height': '80px  ',
                        'width': '80px',
                        'z-index': '100',
                        'border-radius': '999px'
                    })
                    .appendTo($('body'))
                    .animate({
                        'top': cart.offset().top + 10,
                        'left': cart.offset().left + 10,
                        'width': 80,
                        'height': 80
                    }, 3000, 'easeInOutExpo');

                setTimeout(function () {
                    cart.effect("shake", {
                        times: 2
                    }, 200);
                }, 1500);

                imgclone.animate({
                    'width': 0,
                    'height': 0
                }, function () {
                    $(this).detach()
                });
            }

        });
    };
    return animatechitiet;
});


app.controller('add', function ($rootScope, $scope, dataservice, $uibModalInstance) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        id: '',
        nguoidung_id: 'nguoidung',
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
        quangcao: '',
        link: '',
        linkhinhanh: ''
    }
    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.uidBaiHat = {
        uid: ''
    }


    $scope.initData = function () {

        dataservice.taiTheLoai(function (rs) {

            rs = rs.data;
            $scope.taiTheLoai = rs;

            $scope.valueTheLoai = rs[0].id;
            $scope.text.key = $scope.valueTheLoai;
            $scope.text.uid = $rootScope.checklogin.uid;
            dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiDanhSachPhatTheLoai = rs;
                $scope.valueDanhSachPhatTheLoai = rs[0].id;
            });

        });
        dataservice.taiChuDe(function (rs) {
            rs = rs.data;
            $scope.taiChuDe = rs;
            $scope.valueChuDe = rs[0].id;

        });
    };
    $scope.changeTheLoai = function () {
        $scope.text.key = $scope.valueTheLoai
        dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDanhSachPhatTheLoai = rs;
            $scope.valueDanhSachPhatTheLoai = rs[0].id;
        });

    };
    $scope.initData();
    var duLieuNhac = new FormData();
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
            $scope.model.nguoidung_id = $rootScope.checklogin.uid;

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
        duLieuNhac.append("uid", $rootScope.checklogin.uid);
        var objectUrl = URL.createObjectURL($files[0]);
        $scope.dinhDangBaiHat = $files[0].type;
        var downloadbaihat = document.getElementById("btntext");
        downloadbaihat.click();
        $("#audio").prop("src", objectUrl);
        $("#audio").on("canplaythrough", function (e) {
            var seconds = e.currentTarget.duration;
            let currentMinutes = Math.floor(seconds / 60);
            let currentSeconds = Math.floor(seconds - currentMinutes * 60);
            let durationMinutes = Math.floor(seconds / 60);
            let durationSeconds = Math.floor(seconds - durationMinutes * 60);
            if (currentSeconds < 10) { currentSeconds = "0" + currentSeconds; }
            if (durationSeconds < 10) { durationSeconds = "0" + durationSeconds; }
            if (currentMinutes < 10) { currentMinutes = "0" + currentMinutes; }
            if (durationMinutes < 10) { durationMinutes = "0" + durationMinutes; }
            var total_duration = durationMinutes + ":" + durationSeconds;
            $scope.model.thoiluongbaihat = total_duration;
        });
    }
    $scope.getTheFilesHinhAnh = function ($files) {
        $scope.addLinkHinhAnh = "Đã Chọn Hình Ảnh";
        duLieuHinh = new FormData();
        duLieuHinh.append("File1", $files[0]);
        duLieuHinh.append("uid", $rootScope.checklogin.uid);
        // $scope.dinhDangHinhAnh = $files[0].type;
        if ($files[0].type == "image/png" || $files[0].type == "image/jpg" || $files[0].type == "image/jpeg") {
            $scope.dinhDangHinhAnh = "image/"
        }
        else {
            $scope.dinhDangHinhAnh = $files[0].type;
        }
        var downloadbaihat = document.getElementById("btntext");
        downloadbaihat.click();
        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("File", $files[i]);
        //}

    }
});



app.factory('dataShare', function () {
    var dataShare = {};
    return {
        data: dataShare
    }
})

////////////////////////////
// ngay 04/08
app.directive("contextMenu", function ($compile) {
    contextMenu = {};
    //contextMenu.restrict = "AE";
    contextMenu.trigger = "left";
    /*  contextMenu = { replace: false };*/
    /*contextMenu.restrict = "A";*/
    contextMenu.link = function (lScope, lElem, lAttr) {
        lElem.on("contextmenu", function (e) {
            e.preventDefault(); // default context menu is disabled          
            //bài hát bên nhạc mới
            if ($("#contextmenubaihat_baihat"))
                $("#contextmenubaihat_baihat").remove();
            if ($("#contextmenudsptl-node"))
                $("#contextmenudsptl-node").remove();
            if ($("#contextmenuplaylist-node"))
                $("#contextmenuplaylist-node").remove();
            if ($("#contextmenubaihatdangphat"))
                $("#contextmenubaihatdangphat").remove();
            $('#idcontextmenu').prepend($compile(lScope[lAttr.contextMenu])(lScope));
            //bài hát bên nhạc mới vị trí click          
            if ($("#contextmenubaihat_baihat") || $("#contextmenudsptl-node")) {
                if (e.clientX > 900) {
                    $("#contextmenubaihat_baihat").css("left", e.clientX - 525);
                    $("#contextmenudsptl-node").css("left", e.clientX - 495);
                }
                else {
                    $("#contextmenubaihat_baihat").css("left", e.clientX - 230);
                    $("#contextmenudsptl-node").css("left", e.clientX - 230);
                }
                if (e.clientY > 370) {
                    $("#contextmenubaihat_baihat").css("top", e.clientY - 400);
                    $("#contextmenudsptl-node").css("top", e.clientY - 200);
                }
                else {
                    $("#contextmenubaihat_baihat").css("top", e.clientY - 100);
                    $("#contextmenudsptl-node").css("top", e.clientY - 60);
                }
            }
        });
        lElem.on("mouseleave", function (e) {

        });

        $('body').on("click", function (e) {

            if ($("#contextmenudsptl-node"))
                $("#contextmenudsptl-node").remove();

            if ($("#contextmenubaihat_baihat"))
                $("#contextmenubaihat_baihat").remove();
        });
        $('#idcontextmenu').on("wheel", function (e) {

            if ($("#contextmenudsptl-node"))
                $("#contextmenudsptl-node").remove();
            if ($("#contextmenubaihat_baihat")) {
                if ($("#subtextmenuplaylist-node").length < 1) {

                    $("#contextmenubaihat_baihat").remove();
                }
            }
        });

        lElem.on("wheel", function (e) {

            if ($("#contextmenudsptl-node"))
                $("#contextmenudsptl-node").remove();

            if ($("#contextmenubaihat_baihat"))
                $("#contextmenubaihat_baihat").remove();

        });

        lElem.on("click", function (e) {

            if ($("#contextmenudsptl-node"))
                $("#contextmenudsptl-node").remove();

            if ($("#contextmenubaihat_baihat"))
                $("#contextmenubaihat_baihat").remove();
        });

    };
    return contextMenu;
});
app.directive("contextMenu1", function ($compile) {
    contextMenu1 = {};
    contextMenu1.trigger = "left";
    contextMenu1.link = function (lScope, lElem, lAttr) {
        lElem.on("click", function (e) {
            e.preventDefault(); // default context menu is disabled

            if ($("#contextmenubaihat_baihat"))
                $("#contextmenubaihat_baihat").remove();

            if ($("#contextmenudsptl-node"))
                $("#contextmenudsptl-node").remove();
            //if ($("#contextmenubaihatdangphat"))
            //    $("##contextmenubaihatdangphat").remove();
            $('#idcontextmenu').prepend($compile(lScope[lAttr.contextMenu1])(lScope));

            if ($("#contextmenubaihat_baihat") || $("#contextmenudsptl-node")) {
                if (e.clientX > 900) {
                    $("#contextmenubaihat_baihat").css("left", e.clientX - 525);
                    $("#contextmenudsptl-node").css("left", e.clientX - 495);
                }
                else {
                    $("#contextmenubaihat_baihat").css("left", e.clientX - 230);
                    $("#contextmenudsptl-node").css("left", e.clientX - 230);
                }
                if (e.clientY > 370) {
                    $("#contextmenubaihat_baihat").css("top", e.clientY - 400);
                    $("#contextmenudsptl-node").css("top", e.clientY - 200);
                }
                else {
                    $("#contextmenubaihat_baihat").css("top", e.clientY - 100);
                    $("#contextmenudsptl-node").css("top", e.clientY - 60);
                }
            }
            $("#contextmenubaihatdangphat").css("left", 65);
            $("#contextmenubaihatdangphat").css("top", 265);

            e.preventDefault();
            e.stopPropagation();
        });
        $('body').on("click", function (e) {

            if ($("#contextmenubaihat_baihat"))
                $("#contextmenubaihat_baihat").remove();

            if ($("#contextmenudsptl-node"))
                $("#contextmenudsptl-node").remove();

            if ($("#contextmenubaihatdangphat"))
                $("#contextmenubaihatdangphat").remove();
        });
    };
    return contextMenu1;
});



app.directive("subtextMenu", function ($compile) {
    subtextMenu = {};
    //contextMenu.restrict = "AE";
    subtextMenu.trigger = "left";
    /*  contextMenu = { replace: false };*/
    /*contextMenu.restrict = "A";*/
    subtextMenu.link = function (lScope, lElem, lAttr) {
        lElem.on("contextmenu", function (e) {
            e.preventDefault(); // default context menu is disabled           
        });
        lElem.on("mouseenter", function (e) {

            lElem.append($compile(lScope[lAttr.subtextMenu])(lScope));
            //if (!$("#subtextmenuplaylist-node1"))
            //    lElem.append($compile(lScope[lAttr.subtextMenu])(lScope));
            //else
            //    $("#subtextmenuplaylist-node1").css("display", "block");
            if (e.clientX > 1000) {
                $("#subtextmenuplaylist-node").css("left", -225);
                $("#subtextmenuchiase-node").css("left", -225);
            }
            $("#subtextmenuchiase-node").css("top", -60);

        });
        lElem.on("mouseleave", function (e) {
            if ($("#subtextmenuplaylist-node"))
                $("#subtextmenuplaylist-node").remove();
            if ($("#subtextmenuchiase-node"))
                $("#subtextmenuchiase-node").remove();
        });

    };
    return subtextMenu;
});
app.directive('ngRightClick', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngRightClick);
        element.bind('contextmenu', function (event) {
            scope.$apply(function () {
                event.preventDefault();
                fn(scope, { $event: event });
            });
        });
    };
});
// 21/08 Đã Sữa CSDL Mới
app.controller('nhacmoi', function ($rootScope, $scope, $cookies, dataservice) {



    $scope.text = {
        key: '',
        uid: ''
    }

    $scope.yeuThichBaiHatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiDSPBaiHatMoi_NhacMoi[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {
                //
                $scope.taiDSPBaiHatMoi_NhacMoi[viTri].yeuthich = 0;
            } else {

                $scope.taiDSPBaiHatMoi_NhacMoi[viTri].yeuthich = 1;
            }
        }
    }
    $scope.date = new Date();
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }


                        })

                    }
                }
                else {

                }

                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiNhacMoi(function (rs) {
                rs = rs.data;
                $scope.taiNhacMoi = rs;
            });
            dataservice.taiDSPBaiHatMoi_NhacMoi($rootScope.checklogin.uid, function (rs) {

                rs = rs.data;
                $scope.taiDSPBaiHatMoi_NhacMoi = rs;

            });


        })
    }

    $scope.initData();



});
// 21/08 Đã Sữa CSDL Mới
app.controller('theloai', function ($rootScope, $scope, $cookies, dataservice, dataShare, $interval) {

    $scope.text = {
        key: '',
        uid: ''
    }

    $scope.yeuThichDanhSachPhatUi = function (viTriCha, viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 0;
            } else {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 1;
            }
        }
    }
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        //  $scope.modeldsptl.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;

                            }
                            /*resolve();*/
                        })                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            //playlist                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     

                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi = rs;


                $scope.tai();
            });
            dataservice.taiChuDe(function (rs) {
                rs = rs.data;
                $scope.taiChuDe = rs;

            });
        })
    }
    $scope.initData();
    $scope.tai = function () {
        $scope.modeldsptl = {
            uid: $rootScope.checklogin.uid,
            id_dsp: ''
        }
        $scope.modeldsptl.id_dsp = $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[$scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi.length - 1].key;
        dataservice.taiTheLoaiKetHopDanhSachPhatTheLoaiTheoThoiGian($scope.modeldsptl, function (rs) {
            rs = rs.data;
            $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiTheoThoiGian = rs;
            $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi = $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi.concat(rs);
            // $interval($scope.tai, 1500);
            $scope.tai();
        });
    }
    $scope.size = 0;
    $scope.soLuong = 8;
    $scope.size = 0;
    $scope.next = function () {
        if ($scope.size + $scope.soLuong == $scope.taiChuDe.length)
            return;
        $scope.size += 1;

    }
    $scope.prev = function () {
        if ($scope.size == 0)
            $scope.size = 0;
        else
            $scope.size -= 1;

    }
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
// 21/08 Đã Sữa CSDL Mới
app.controller('top20', function ($rootScope, $scope, $cookies, dataservice, dataShare) {

    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.yeuThichTop20Ui = function (viTriCha, viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiTheLoaiKetHopTop20[viTriCha].object[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiTheLoaiKetHopTop20[viTriCha].object[viTri].yeuthich = 0;
            } else {

                $scope.taiTheLoaiKetHopTop20[viTriCha].object[viTri].yeuthich = 1;
            }
        }
    }
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;


                            }
                            /* resolve();*/
                        })                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           //playlist                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     

                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiTheLoaiKetHopTop20($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.taiTheLoaiKetHopTop20 = rs;
                $scope.tai();
            });

        })
    }
    $scope.initData();
    $scope.tai = function () {
        $scope.modeldsptl = {
            uid: $rootScope.checklogin.uid,
            id_dsp: ''
        }
        $scope.modeldsptl.id_dsp = $scope.taiTheLoaiKetHopTop20[$scope.taiTheLoaiKetHopTop20.length - 1].key;
        dataservice.taiTheLoaiKetHopTop20TheoThoiGian($scope.modeldsptl, function (rs) {
            rs = rs.data;
            $scope.taiTheLoaiKetHopTop20TheoThoiGian = rs;
            $scope.taiTheLoaiKetHopTop20 = $scope.taiTheLoaiKetHopTop20.concat(rs);
            // $interval($scope.tai, 1500);
            $scope.tai();
        });
    }
});
// 21/08 Đã Sữa CSDL Mới
app.controller('chitiettheloai', function ($rootScope, $scope, $routeParams, $cookies, dataservice, dataShare) {
    // yêu thích bài hát khi click bên nhạc mới bên Chi tiet thể loại dùng taiDanhSachBaiHatTheoTheLoai
    $scope.yeuThichBaiHatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich = 0;
            } else {

                $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich = 1;
            }
        }
    }
    // 22/07 yêu thích dsp thể loài bên trang chittiettheloai object 
    $scope.yeuThichDanhSachPhatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiDanhSachPhatTheLoai[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiDanhSachPhatTheLoai[viTri].yeuthich = 0;
            } else {

                $scope.taiDanhSachPhatTheLoai[viTri].yeuthich = 1;
            }
        }
    }
    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.text.key = $routeParams.id;
    $scope.soLuong = 0;// soluong là hiển thị sô lượng danh sách phát thể loại
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;

                            }
                            /* resolve();*/
                        })                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          //playlist                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     

                    }
                }
                else {
                }
                resolve();
            });
        });
        promise.then(function () {
            $scope.text.uid = $rootScope.checklogin.uid;

            dataservice.taiTheLoaiTheoId_ChiTietTheLoai($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiTheLoaiTheoId_ChiTietTheLoai = rs;

            });


            dataservice.taiDanhSachBaiHatTheoTheLoai($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiDanhSachBaiHatTheoTheLoai = rs;
                // demSo là so luong bai hat kiểu lẻ 3 cột 
                $scope.kiemTrademSo = $scope.taiDanhSachBaiHatTheoTheLoai.length;
                if ($scope.kiemTrademSo < 15 && $scope.kiemTrademSo > 12) {
                    $scope.demSo = 12;
                }
                if ($scope.kiemTrademSo < 12 && $scope.kiemTrademSo > 9) {
                    $scope.demSo = 9;
                }
                if ($scope.kiemTrademSo < 9 && $scope.kiemTrademSo > 6) {
                    $scope.demSo = 6;
                }
                if ($scope.kiemTrademSo < 6) {
                    $scope.demSo = 3;
                }
                if ($scope.kiemTrademSo > 15) {
                    $scope.demSo = 15;
                }
            });

            dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiDanhSachPhatTheLoai = rs;
                if ($scope.taiDanhSachPhatTheLoai.length <= 6) {
                    $scope.soLuong = $scope.taiDanhSachPhatTheLoai.length;
                }
                else {
                    $scope.soLuong = 6;
                }
            });


        })
    }
    $scope.size = 0;
    $scope.next = function () {
        if ($scope.size + $scope.soLuong == $scope.taiDanhSachPhatTheLoai.length)
            return;
        $scope.size += 1;

    }
    $scope.prev = function () {
        if ($scope.size == 0)
            $scope.size = 0;
        else
            $scope.size -= 1;

    }
    $scope.initData();
    // check bị trí đang chọn
    $scope.checkplaymusic = -1;
    $scope.viTriBanNhac = function (index) {
        $scope.checkplaymusic = index;
    }



});
// 21/08 Đã Sữa CSDL Mới
app.controller('danhsachphat', function ($rootScope, $scope, $routeParams, $cookies, dataservice) {
    $scope.yeuThichDanhSachPhatChiTietUi = function () {
        if ($rootScope.checklogin.dadangnhap == true) {

            $scope.yeuThuong = $scope.taiDanhSachPhatTheLoaiChiTiet[0].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiDanhSachPhatTheLoaiChiTiet[0].yeuthich = 0;
            } else {

                $scope.taiDanhSachPhatTheLoaiChiTiet[0].yeuthich = 1;
            }
        }
    }
    // yêu thích bài hát khi click bên taiDSPBaiHatTheoDSPTheLoai_DSP
    $scope.yeuThichBaiHatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {

            $scope.yeuThuong = $scope.taiDSPBaiHatTheoDSPTheLoai_DSP[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiDSPBaiHatTheoDSPTheLoai_DSP[viTri].yeuthich = 0;
            } else {

                $scope.taiDSPBaiHatTheoDSPTheLoai_DSP[viTri].yeuthich = 1;
            }
        }
    }
    $scope.yeuThichDanhSachPhatUi = function (viTriCha, viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 0;
            } else {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 1;
            }
        }
    }
    $scope.xoayHinhDSPTL_DSPTheLoai = function (id_dsp_theloai) {

        //  $rootScope.$rootScope.kiemTraDSPTop20Click = iddsp; = id_dsp_baihat;
        $rootScope.kiemTraDSPTLClick = id_dsp_theloai;
    }
    $scope.idDSPTheLoai = $routeParams.id;
    function chuyenDoi(thoigian) {
        let phut = Math.floor(thoigian.slice(0, 2) * 60);
        let giay = Math.floor(thoigian.slice(3, 5));
        return phut + giay;
    }
    function secondsToHms(d) {
        d = Number(d);
        var h = Math.floor(d / 3600);
        var m = Math.floor(d % 3600 / 60);
        var s = Math.floor(d % 3600 % 60);

        var hDisplay = h > 0 ? h + (h == 1 ? " giờ  " : " giờ  ") : "0 giờ";
        var mDisplay = m > 0 ? m + (m == 1 ? " phút  " : " phút  ") : "";
        var sDisplay = s > 0 ? s + (s == 1 ? " giây" : " giây") : "";
        return hDisplay + " " + mDisplay;
    }
    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.text.key = $scope.idDSPTheLoai;
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;

                            }
                            /*  resolve();*/
                        })
                        //  $cookies.putObject("user", $rootScope.checklogin);


                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiDanhSachPhatTheLoaiChiTiet($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiDanhSachPhatTheLoaiChiTiet = rs;

            });
            //dataservice.taiDanhSachBaiHat($scope.text, function (rs) {
            //    rs = rs.data;
            //    $scope.taiDanhSachBaiHat = rs;
            //});
            // load the loai con
            //dataservice.taiTheLoaiKetHopDanhSachPhatTheLoai(function (rs) {
            //    rs = rs.data;
            //    $scope.datataiTheLoai = rs;
            //});
            dataservice.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi = rs;
            });
            // tai ds bài hat theo dsp thể loai
            dataservice.taiDSPBaiHatTheoDSPTheLoai_DSP($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiDSPBaiHatTheoDSPTheLoai_DSP = rs;
                var tongThoiLuong = 0;
                for (var i = 0; i < $scope.taiDSPBaiHatTheoDSPTheLoai_DSP.length; i++) {
                    tongThoiLuong += chuyenDoi($scope.taiDSPBaiHatTheoDSPTheLoai_DSP[i].thoiluongbaihat);

                }
                $scope.tongThoiGianPhat = secondsToHms(tongThoiLuong);
                $scope.soBaiHat = $scope.taiDSPBaiHatTheoDSPTheLoai_DSP.length;
            });

        })


    }
    $scope.initData();

    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }
    // vi trí bản nhạc 15 / 07
    //$scope.checkplaymusic = -1;
    //$scope.viTriBanNhac = function (index) {
    //    $scope.checkplaymusic = index;
    //}

});
// 21/08 Đã Sữa CSDL Mới
app.controller('danhsachphattop20', function ($rootScope, $scope, $routeParams, $cookies, dataservice) {
    $scope.yeuThichTop20ChiTietUi = function () {
        if ($rootScope.checklogin.dadangnhap == true) {

            $scope.yeuThuong = $scope.taiTop20ChiTiet[0].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiTop20ChiTiet[0].yeuthich = 0;
            } else {

                $scope.taiTop20ChiTiet[0].yeuthich = 1;
            }
        }
    }
    // yêu thích bài hát khi click bên taiDSPBaiHatTheoDSPTheLoai_DSP
    $scope.yeuThichBaiHatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {

            $scope.yeuThuong = $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20[viTri].yeuthich = 0;
            } else {

                $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20[viTri].yeuthich = 1;
            }
        }
    }
    $scope.yeuThichDanhSachPhatUi = function (viTriCha, viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 0;
            } else {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 1;
            }
        }
    }
    $scope.xoayHinhDSPTop20_DSPTop20 = function (id_dsp_top20) {

        //  $rootScope.$rootScope.kiemTraDSPTop20Click = iddsp; = id_dsp_baihat;
        $rootScope.kiemTraDSPTop20Click = id_dsp_top20;
    }
    $scope.idDSPTheLoai = $routeParams.id;
    function chuyenDoi(thoigian) {
        let phut = Math.floor(thoigian.slice(0, 2) * 60);
        let giay = Math.floor(thoigian.slice(3, 5));
        return phut + giay;
    }
    function secondsToHms(d) {
        d = Number(d);
        var h = Math.floor(d / 3600);
        var m = Math.floor(d % 3600 / 60);
        var s = Math.floor(d % 3600 % 60);

        var hDisplay = h > 0 ? h + (h == 1 ? " giờ  " : " giờ  ") : "0 giờ";
        var mDisplay = m > 0 ? m + (m == 1 ? " phút  " : " phút  ") : "";
        var sDisplay = s > 0 ? s + (s == 1 ? " giây" : " giây") : "";
        return hDisplay + " " + mDisplay;
    }
    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.text.key = $scope.idDSPTheLoai;
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;

                            }
                            /*resolve();*/
                        })
                        //  $cookies.putObject("user", $rootScope.checklogin);


                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiTop20ChiTiet($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiTop20ChiTiet = rs;
                $scope.text.key = $scope.taiTop20ChiTiet[0].danhsachphattheloai_id;
                dataservice.taiDSPBaiHatTheoDSPTop20_DSPTop20($scope.text, function (rs) {
                    rs = rs.data;
                    $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20 = rs;
                    var tongThoiLuong = 0;
                    for (var i = 0; i < $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20.length; i++) {
                        tongThoiLuong += chuyenDoi($scope.taiDSPBaiHatTheoDSPTheLoai_DSP[i].thoiluongbaihat);

                    }
                    $scope.tongThoiGianPhat = secondsToHms(tongThoiLuong);
                    $scope.soBaiHat = $scope.taiDSPBaiHatTheoDSPTop20_DSPTop20.length;
                });
            });
            dataservice.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi = rs;
            });
            // tai ds bài hat theo dsp thể loai

        })


    }
    $scope.initData();
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }
    // vi trí bản nhạc 15 / 07
    //$scope.checkplaymusic = -1;
    //$scope.viTriBanNhac = function (index) {
    //    $scope.checkplaymusic = index;
    //}

});
//bài hát khi click vào tên bài hát
// 21/08 Đã Sữa CSDL Mới
app.controller('baihat', function ($rootScope, $scope, $routeParams, $cookies, dataservice, dataShare) {
    $scope.yeuThichDanhSachPhatUi = function (viTriCha, viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 0;
            } else {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 1;
            }
        }
    }
    $scope.yeuThichBaiHatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {
                //
                $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich = 0;
            } else {

                $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich = 1;
            }
        }
    }

    // yêu thích bài hát khi click bên bài hát taiBaiHatTheoId
    $scope.yeuThichChiTietBaiHatUi = function () {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiBaiHatTheoId[0].yeuthich;
            if ($scope.yeuThuong == 1) {
                //
                $scope.taiBaiHatTheoId[0].yeuthich = 0;
            } else {

                $scope.taiBaiHatTheoId[0].yeuthich = 1;
            }
        }
        //  alert($scope.taiDSPBaiHatMoi_NhacMoi[viTri].yeuthich);

    }

    $scope.xoayHinhDSPBaiHat_BaiHat = function (id_baihat) {

        //  $rootScope.$rootScope.kiemTraDSPTop20Click = iddsp; = id_dsp_baihat;
        $scope.kiemTraDSPBaiHatClick = id_baihat;
    }

    $scope.text = {
        key: '',
        uid: ''
    }

    $scope.text.key = $routeParams.id;
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }
                            /*resolve();*/
                        })

                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiBaiHatTheoId($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiBaiHatTheoId = rs;
                $scope.data123 = $scope.taiBaiHatTheoId[0];
                $scope.text.key = $scope.taiBaiHatTheoId[0].theloai_id;
                dataservice.taiDanhSachBaiHatTheoTheLoai($scope.text, function (rs) {
                    rs = rs.data;
                    $scope.taiDanhSachBaiHatTheoTheLoai = rs;
                    for (var i = 0; i < $scope.taiDanhSachBaiHatTheoTheLoai.length; i++) {
                        if ($scope.taiBaiHatTheoId[0].id == $scope.taiDanhSachBaiHatTheoTheLoai[i].id) {
                            $scope.taiDanhSachBaiHatTheoTheLoai.splice(i, 1);
                        }
                    }
                });

            });
            // load the loai con
            //dataservice.taiTheLoaiKetHopDanhSachPhatTheLoai(function (rs) {
            //    rs = rs.data;
            //    $scope.datataiTheLoai = rs;
            //});
            dataservice.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi = rs;
            });

        })

    }
    $scope.initData();

});

// 21/08 Đã Sữa CSDL Mới
//13/07 tạo playlist (Danh sach phat người dùng)
app.controller('playlist', function ($rootScope, $scope, $routeParams, $cookies, dataservice, dataShare) {
    $scope.yeuThichPlayListChiTietUi = function () {
        if ($rootScope.checklogin.dadangnhap == true) {

            $scope.yeuThuong = $scope.taiChiTietPlayList_PlayList[0].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiChiTietPlayList_PlayList[0].yeuthich = 0;
            } else {

                $scope.taiChiTietPlayList_PlayList[0].yeuthich = 1;
            }
        }
        else {
            alert("làm ơn đăng nhập");
        }
    }
    $scope.yeuThichDanhSachPhatUi = function (viTriCha, viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 0;
            } else {

                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi[viTriCha].object[viTri].yeuthich = 1;
            }
        }
    }

    $scope.xoayHinhDSPNguoiDung_Playlist = function (id_dsp_nguoidung) {

        //  $rootScope.$rootScope.kiemTraDSPTop20Click = iddsp; = id_dsp_baihat;
        $rootScope.kiemTraDSPNguoiDungClick = id_dsp_nguoidung;
    }

    //19/07 xóa bài hát khỏi playlist hiện tại // chưa viết api
    $scope.xoaBaiHatKhoiPlaylist = function (vitri) {
        // $rootScope vi tri là vị trí cảu bài hát trong danh sach khi mở contexxxtmenu lấy về vị trí
        $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.splice(vitri, 1);
        // ubi  danhsachphat nguoi dung playlist
        // key là id bài hát
        $scope.text.key = $scope.baihatCookie.id;
        // $scope.text.uid = $routeParams.id;
        $scope.modelThemBaiHatVaoDSPNguoiDung = {
            baihat_id: '',
            uid: $rootScope.checklogin.uid,
            danhsachphatnguoidung_id: ''
        }
        $scope.modelThemBaiHatVaoDSPNguoiDung.baihat_id = $scope.baihatCookie.id;
        $scope.modelThemBaiHatVaoDSPNguoiDung.danhsachphatnguoidung_id = $routeParams.id;
        dataservice.xoaBaiHatKhoiDSPNguoiDung_Playlist($scope.modelThemBaiHatVaoDSPNguoiDung, function (rs) {
            rs = rs.data;
            $scope.xoaBaiHatKhoiDSPNguoiDung_Playlist = rs;
            alert("Xóa thành công");
        });
    };
    // yêu thích bài hát khi click bên taiDSBaiHatTheoDSPNguoiDung
    $scope.yeuThichBaiHatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {

            $scope.yeuThuong = $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {

                $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[viTri].yeuthich = 0;
            } else {

                $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[viTri].yeuthich = 1;
            }
        }
    }

    // yêu thích bài hát khi click bên taiDanhSachBaiHatTheoTheLoai
    $scope.yeuThichBaiHatUiTheLoai = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            //  alert($scope.taiDSPBaiHatMoi_NhacMoi[viTri].yeuthich);
            $scope.yeuThuong = $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {
                //
                $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich = 0;
            }
            else {

                $scope.taiDanhSachBaiHatTheoTheLoai[viTri].yeuthich = 1;
            }
        }
    };

    $scope.idPlayList = $routeParams.id;
    function chuyenDoi(thoigian) {
        let phut = Math.floor(thoigian.slice(0, 2) * 60);
        let giay = Math.floor(thoigian.slice(3, 5));
        return phut + giay;
    }
    function secondsToHms(d) {
        d = Number(d);
        var h = Math.floor(d / 3600);
        var m = Math.floor(d % 3600 / 60);
        var s = Math.floor(d % 3600 % 60);

        var hDisplay = h > 0 ? h + (h == 1 ? " giờ  " : " giờ  ") : "0 giờ";
        var mDisplay = m > 0 ? m + (m == 1 ? " phút  " : " phút  ") : "";
        var sDisplay = s > 0 ? s + (s == 1 ? " giây" : " giây") : "";
        return hDisplay + " " + mDisplay;
    }
    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.modelChiTietPlayList = {
        dspnguoidung_id: '',
        // người dùng sỡ hữu playlist dspnguoidung
        nguoidung_id: '',
        //người dùng đang đăng nhập hiện tại
        uid: ''
    }
    $scope.text.key = $scope.idPlayList;
    $scope.soLuong = 8;
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        $scope.modelChiTietPlayList.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                                //  $cookies.putObject("user", $rootScope.checklogin);

                            }
                            /*  resolve();*/
                        })


                    }
                }
                else {

                }
                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiChiTietPlayList_PlayList($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiChiTietPlayList_PlayList = rs;
                $rootScope.idNguoiDungVoiPlayListHienTai = $scope.taiChiTietPlayList_PlayList[0].nguoidung_id;
                $scope.modelChiTietPlayList.dspnguoidung_id = $scope.idPlayList;
                $scope.modelChiTietPlayList.nguoidung_id = $scope.taiChiTietPlayList_PlayList[0].nguoidung_id;
                dataservice.taiThongTinNguoiDungBangIdPlayList_PLayList($scope.taiChiTietPlayList_PlayList[0].nguoidung_id, function (rs) {

                    rs = rs.data;
                    $scope.taiThongTinNguoiDungBangIdPlayList_PLayList = rs;

                });

                dataservice.taiDSBaiHatTheoDSPNguoiDung_PlayList($scope.modelChiTietPlayList, function (rs) {
                    rs = rs.data;
                    $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList = rs;
                    var tongThoiLuong = 0;
                    for (var i = 0; i < $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.length; i++) {
                        tongThoiLuong += chuyenDoi($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[i].thoiluongbaihat);

                    }
                    $scope.tongThoiGianPhat = secondsToHms(tongThoiLuong);
                    $scope.soBaiHat = $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.length;
                    $scope.text.key = $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[$scope.soBaiHat - 1].theloai_id;

                    dataservice.taiDanhSachBaiHatTheoTheLoai($scope.text, function (rs) {
                        rs = rs.data;
                        $scope.taiDanhSachBaiHatTheoTheLoai = rs;
                        for (var y = 0; y < $scope.soBaiHat; y++) {
                            for (var i = 0; i < $scope.taiDanhSachBaiHatTheoTheLoai.length; i++) {
                                if ($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[y].id == $scope.taiDanhSachBaiHatTheoTheLoai[i].id) {
                                    $scope.taiDanhSachBaiHatTheoTheLoai.splice(i, 1);
                                }
                            }
                        }
                        //  $scope.soLuong = Math.floor($scope.taiDanhSachBaiHatTheoTheLoai.length / 3);
                        if ($scope.taiDanhSachBaiHatTheoTheLoai.length < 8) {
                            $scope.soLuong = $scope.taiDanhSachBaiHatTheoTheLoai.length;
                        }
                    });

                });

            });
            // load the loai kết hợp với dsp tl

            dataservice.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi($rootScope.checklogin.uid, function (rs) {
                rs = rs.data;
                $scope.taiTheLoaiKetHopDanhSachPhatTheLoaiMoi = rs;
            });
            // tai ds bài hat theo dsp nguoidung (playlist)
            //dataservice.taiDSBaiHatTheoDSPNguoiDung_PlayList($scope.text, function (rs) {
            //    rs = rs.data;
            //    $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList = rs;
            //    var tongThoiLuong = 0;
            //    for (var i = 0; i < $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.length; i++) {
            //        tongThoiLuong += chuyenDoi($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[i].thoiluongbaihat);

            //    }
            //    $scope.tongThoiGianPhat = secondsToHms(tongThoiLuong);
            //    $scope.soBaiHat = $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.length;
            //    $scope.text.key = $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[$scope.soBaiHat - 1].theloai_id;

            //    dataservice.taiDanhSachBaiHatTheoTheLoai($scope.text, function (rs) {
            //        rs = rs.data;
            //        $scope.taiDanhSachBaiHatTheoTheLoai = rs;
            //        for (var y = 0; y < $scope.soBaiHat; y++) {
            //            for (var i = 0; i < $scope.taiDanhSachBaiHatTheoTheLoai.length; i++) {
            //                if ($scope.taiDSBaiHatTheoDSPNguoiDung_PlayList[y].id == $scope.taiDanhSachBaiHatTheoTheLoai[i].id) {
            //                    $scope.taiDanhSachBaiHatTheoTheLoai.splice(i, 1);
            //                }
            //            }
            //        }
            //        //  $scope.soLuong = Math.floor($scope.taiDanhSachBaiHatTheoTheLoai.length / 3);
            //        if ($scope.taiDanhSachBaiHatTheoTheLoai.length < 8) {
            //            $scope.soLuong = $scope.taiDanhSachBaiHatTheoTheLoai.length;
            //        }
            //    });

            //});
        })


    }
    $scope.initData();
    $scope.tenbien = 'null';
    $scope.hoatdong = false;
    $scope.modelsapxep = 'null';
    $scope.sapXep = function (data) {
        $scope.hoatdong = ($scope.tenbien === data) ? !$scope.hoatdong : false;
        $scope.tenbien = data;
    }
    // nút làm mới gợi ý
    $scope.size = 0;
    $scope.lamMoiGoiY = function () {
        if ($scope.taiDanhSachBaiHatTheoTheLoai.length < 8) {
            return;
        } else {

            if ($scope.taiDanhSachBaiHatTheoTheLoai.length % 8 == 0) {
                if ($scope.size + $scope.soLuong >= $scope.taiDanhSachBaiHatTheoTheLoai.length)
                    $scope.size = 0;
                else {
                    $scope.size += $scope.soLuong;
                }
            }
            else {
                $scope.bienTam = $scope.taiDanhSachBaiHatTheoTheLoai.length % 8;
                $scope.bienTam2 = $scope.taiDanhSachBaiHatTheoTheLoai.length - $scope.bienTam;
                if ($scope.size + $scope.soLuong == $scope.bienTam2) {

                    $scope.size += 8;
                    $scope.soLuong = $scope.bienTam;
                }
                else if ($scope.size + $scope.soLuong >= $scope.taiDanhSachBaiHatTheoTheLoai.length) {
                    $scope.size = 0;
                    $scope.soLuong = 8
                }
                else {
                    $scope.size += 8;

                }
            }
        }
    }
    // nút làm mới gợi ý
    // vi trí bản nhạc 15 / 07
    $scope.checkplaymusic = -1;
    $scope.checkplaymusictheloai = -1;
    $scope.viTriBanNhac = function (index) {
        $scope.checkplaymusic = index;
        $scope.checkplaymusictheloai = -1;
    }
    $scope.viTriBanNhacTheLoai = function (index) {
        $scope.checkplaymusictheloai = index;
        $scope.checkplaymusic = -1;
    }

    //19/07 themBaiHatVaoPlaylistHienTai chưa viết api
    $scope.themBaiHatVaoPlaylistHienTai = function (data, vitri) {
        // ubi = id danhsachphat nguoi dung playlist
        // key là id bài hát
        $scope.modelThemBaiHatVaoDSPNguoiDung = {
            baihat_id: '',
            uid: $rootScope.checklogin.uid,
            danhsachphatnguoidung_id: ''
        }
        $scope.modelThemBaiHatVaoDSPNguoiDung.baihat_id = data.id;
        $scope.modelThemBaiHatVaoDSPNguoiDung.danhsachphatnguoidung_id = $routeParams.id;
        $scope.text.key = data.id;
        // $scope.text.uid = $routeParams.id;
        $scope.taiDSBaiHatTheoDSPNguoiDung_PlayList.push(data);
        $scope.taiDanhSachBaiHatTheoTheLoai.splice(vitri, 1);
        dataservice.themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi($scope.modelThemBaiHatVaoDSPNguoiDung, function (rs) {
            rs = rs.data;
            $scope.themBaiHatVaoDanhSachPhatNguoiDung_NhacMoi = rs;
            alert("thêm thành công");
        });
    }
});

app.controller('thanhtoanthanhcong', function ($rootScope, $scope, $routeParams, $cookies, dataservice, dataShare) {
    $scope.text = {
        key: '',
        uid: ''
    }
    $scope.text.key = $routeParams.idhoadonthanhtoan;
    $scope.initData = function () {
        dataservice.taiThongTinThanhToanThanhCong($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiThongTinThanhToanThanhCong = rs;
            $scope.thongTinHoaDon = $scope.taiThongTinThanhToanThanhCong[0].hoadonthanhtoan;
        });
    }
    $scope.initData();
})
app.controller('thanhtoanthatbai', function () {

})
app.controller('chude', function ($rootScope, $scope, $routeParams, $cookies, dataservice, dataShare) {

    $scope.text = {
        key: $routeParams.idchude,
        uid: ''
    }

    $scope.yeuThichBaiHatUi = function (viTri) {
        if ($rootScope.checklogin.dadangnhap == true) {
            // alert($scope.dspCookie.id);
            $scope.yeuThuong = $scope.taiDanhSachBaiHatTheoChuDe_ChuDe[viTri].yeuthich;
            if ($scope.yeuThuong == 1) {
                //
                $scope.taiDanhSachBaiHatTheoChuDe_ChuDe[viTri].yeuthich = 0;
            } else {

                $scope.taiDanhSachBaiHatTheoChuDe_ChuDe[viTri].yeuthich = 1;
            }
        }
    }
    $scope.date = new Date();
    $scope.initData = function () {
        var promise = new Promise(function (resolve, reject) {
            firebase.auth().onAuthStateChanged(function (userlogin) {
                if (userlogin) {
                    const user = firebase.auth().currentUser;
                    if (user != null) {
                        $rootScope.checklogin.uid = user.uid;
                        $scope.text.uid = $rootScope.checklogin.uid;
                        dataservice.getUser(user.uid, function (rs) {
                            rs = rs.data;
                            if (rs.Error) { } else {
                                $rootScope.checklogin.hovaten = rs.hoten;
                                $rootScope.checklogin.hinhanh = rs.hinhdaidien;
                                $rootScope.checklogin.dadangnhap = true;
                                $rootScope.checklogin.mota = rs.mota;
                                $rootScope.checklogin.vip = rs.vip;
                            }


                        })

                    }
                }
                else {

                }

                resolve();
            });
        });
        promise.then(function () {
            dataservice.taiChuDeTheoId_ChuDe($scope.text, function (rs) {
                rs = rs.data;
                $scope.taiChuDeTheoId_ChuDe = rs;
                //$scope.numberOfPages = function () {
                //    return Math.ceil($scope.taiBaiHat.length / $scope.pageSize);
                //}
                //if ($scope.numberOfPages() < 8) {
                //    $scope.soLuong = $scope.numberOfPages();
                //}
            });
            dataservice.taiDanhSachBaiHatTheoChuDe_ChuDe($scope.text, function (rs) {

                rs = rs.data;
                $scope.taiDanhSachBaiHatTheoChuDe_ChuDe = rs;

            });
        })
    }

    $scope.initData();



});


