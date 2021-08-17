
var ctxfolderurl = "/views/front-end/Home1";

var app = angular.module('App_ESEIM', ['ui.bootstrap', 'ngRoute', 'slick', 'ngCookies']);

app.config(function ($routeProvider, $locationProvider) {
    $locationProvider.hashPrefix('');
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
        .when('/zingchart', {
            templateUrl: ctxfolderurl + '/zingchart.html',
            controller: 'zingchart'
        })
        .when('/nhacmoi', {
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
});

app.factory('dataservice', function ($http) {
    return {
        getListQuangCao: function (callback) {
            $http.post('/Home1/DanhSachQuangCao').then(callback);
        },
        createbaihat: function (data, callback) {
            $http.post('/Home1/CreateBaiHat', data).then(callback);
        },
        loadtheloai: function (callback) {
            $http.post('/Home1/LoadTheloai').then(callback);

        },
        loadchude: function (callback) {
            $http.post('/Home1/LoadChuDe').then(callback);

        },
        taiZingChat: function (callback) {
            $http.post('/Home1/taiZingChat').then(callback);

        },
        taiNhacMoi: function (callback) {
            $http.post('/Home1/taiNhacMoi').then(callback);

        },
        taiTheLoai: function (callback) {
            $http.post('/Home1/taiTheloai').then(callback);

        },   
        taiDanhSachPhatTheLoaiHA: function (data, callback) {
            $http.post('/Home1/taiDanhSachPhatTheLoaiHA', data).then(callback);

        },
        taiDanhSachPhatTheLoai: function (data, callback) {
            $http.post('/Home1/taiDanhSachPhatTheLoai', data).then(callback);

        },
        taiDanhSachBaiHat: function (data, callback) {
            $http.post('/Home1/taiDanhSachBaiHat', data).then(callback);

        },
        taiDanhSachBaiHatTheoTheLoai: function (data, callback) {
            $http.post('/Home1/taiDanhSachBaiHatTheoTheLoai', data).then(callback);

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

app.controller('Ctrl_ESEIM', function ($scope, $cookies, dataservice, $uibModal) {
  
    let curr_time = document.getElementById("process__start_time");
    let total_duration = document.getElementById("process__end_time");
    $scope.ramdomsong = false;
    $scope.initData = function () {
        $scope.repeate = false;
        $scope.model = 2;
        $scope.buttonnav = 1;
        $scope.nav__right = false;
        $scope.playmusic = 0;
        $scope.show = 0;
        $scope.sttmusic = 0;
        var audio = document.getElementById("audio");
        var progress = document.getElementById("progress");
        $scope.lickPlaylist = false;
        const $ = document.querySelector.bind(document);
        const $$ = document.querySelectorAll.bind(document);

        dataservice.getListQuangCao(function (rs) {

            rs = rs.data;
            $scope.listQuangCao = angular.fromJson(rs);
        });
   
        $scope.seekTo = function () {
            const seekTime = (audio.duration / 100) * progress.value;
            audio.currentTime = seekTime;
        }
        $scope.indexsongs = 0;

        $scope.songs = [
            {
                name: "Anh Thanh Niên",
                singer: "Huy R",
                path: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2Fadmin762021173514158?alt=media&token=b850c8d8-80f9-42ec-b3c7-88f556da24ea",
                image: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/image%2F117182310_774971373269808_7637056199220805523_o.jpg?alt=media&token=1285903e-96cf-452d-b5b3-4014d7622516"
            },
            {
                name: "Dừng Thương",
                singer: "DatKa",
                path: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2Fadmin762021174133158?alt=media&token=330b8600-88be-46c2-83da-e385cb693847",
                image: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/image%2FCHILL.jpg?alt=media&token=c2c93927-edd3-4a91-8b38-1eb9e4976d1c"
            },
            {
                name: "Ghé Qua",
                singer: "Tay Nguyen Sound",
                path: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2FGheQua.mp3?alt=media&token=41117831-5689-40c1-b7fd-8a700df0e597",
                image: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/image%2FGirl-xinh-cute.jpg?alt=media&token=2e27402f-61db-4a02-ad5d-b15c86dbaf2e"
            },
            {
                name: "Tướng Quân",
                singer: "Nhật Phong",
                path: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2FTuongQuan.mp3?alt=media&token=ad0cf69b-bc24-42fb-aaa9-1c01b4d42de1",
                image: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/image%2Fphoto.PNG?alt=media&token=2c2faf40-1018-42b3-850c-2720894491a4"
            },
            {
                name: "Tương Tư Nàng Ca Sĩ",
                singer: "TamKe",
                path: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2FTuongTuNangCaSi.mp3?alt=media&token=03dc5931-4616-43c3-a9b0-157f184173ad",
                image: "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/image%2F073c4faa406544d6546a10c3104962df.jpg?alt=media&token=431cf089-3004-49e2-b389-7274b5fba481"
            }
        ];
        $scope.slbaihat = $scope.songs.length;

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
            $scope.sttmusic = e;
            const namemusic = document.getElementById("music_name");
            const sigermusic = document.getElementById("siger_name");
            const imgmusic = document.getElementById("image_music_avatar");
            namemusic.textContent = $scope.songs[e].name;
            sigermusic.textContent = $scope.songs[e].singer;
            imgmusic.style.backgroundImage = "";
            imgmusic.style.backgroundImage = "url(" + $scope.songs[e].image + ")";
            audio.src = $scope.songs[e].path;
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
        }
        else {
            $scope.playmusic = 0;
            audio.pause();
            cdThumbAnimate.pause();
        }


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
            if ($scope.sttmusic >= $scope.slbaihat)
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
            let newIndex;
            do {
                newIndex = Math.floor(Math.random() * $scope.songs.length);
            } while (newIndex === $scope.sttmusic);

            $scope.sttmusic = newIndex;
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
    const playlist = document.getElementById("playlist_music_ul");

    //playlist.onclick = function (e) {


    //}
    $scope.lickSongPlayList = function (index) {

        const songs = event.target.closest(".nav__right__list__item:not(.item_play_list)")
        if (songs || event.target.closest("#icon_option")) {
            //xu li khi click vao song
            if (songs) {
                $scope.sttmusic = Number(index);
                $scope.loadmusic($scope.sttmusic);


            }
            //xu li khi click vao option
            if (event.target.closest(".option")) {

            }
        }
    }
    $scope.clicknav = function (index) {
        if (index == 1) {
            $scope.model = 1;
        }
        else if (index == 2) {
            $scope.model = 2;
        }
        else if (index == 3) {
            $scope.model = 3;
        }
        else {
            $scope.model = 4;
        }
    }

    $scope.upLoadNhac = function () {
      
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
    };

});
//app.filter('thoiGianNhac', function () {
//    return function (text) {
//        var audio = document.getElementById(text);
        
//        return Math.floor(audio.duration / 60);
//    };
//});
//app.filter('vidu', function () {
//    return function (text) {
//        return text + "tien ga1";
//    };
//});
// truyền dữ liệu qua các controler js vs nhau
 app.factory('dataShare', function () {
     var dataShare = {};
     return {
         data: dataShare
     }
 })
app.directive("contextMenu", function ($compile) {
    contextMenu = {};
    //contextMenu.restrict = "AE";
        contextMenu.trigger = "left";
  /*  contextMenu = { replace: false };*/
    /*contextMenu.restrict = "A";*/  
    contextMenu.link = function (lScope, lElem, lAttr) {
        lElem.on("contextmenu", function (e) {
            e.preventDefault(); // default context menu is disabled
            
            if ($("#contextmenu-node"))
                $("#contextmenu-node").remove();
            $('#idcontextmenu').prepend($compile(lScope[lAttr.contextMenu])(lScope));
            if (e.clientX > 900) {
                $("#contextmenu-node").css("left", e.clientX - 530);
            } else {
                $("#contextmenu-node").css("left", e.clientX - 230);
            }
            if (e.clientY > 300) {
                $("#contextmenu-node").css("top", e.clientY - 400);
            } else {
                $("#contextmenu-node").css("top", e.clientY - 50);
            }                   
        });
        lElem.on("mouseleave", function (e) {
                           
        });
        $('body').on("click", function (e) {
            if ($("#contextmenu-node"))
                $("#contextmenu-node").remove();

        });
        lElem.on("wheel", function (e) {
          
            if ($("#contextmenu-node"))
                $("#contextmenu-node").remove();
        });   
     
        lElem.on("click", function (e) {
            if ($("#contextmenu-node"))
                $("#contextmenu-node").remove();
          
        });
    };
    return contextMenu;
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

          //  if ($("#subtextmenuplaylist-node"))
          //      $("#subtextmenuplaylist-node").remove();
          ////  $('.menu-list--submenu').prepend($compile(lScope[lAttr.subtextMenu])(lScope));
          //  lElem.append($compile(lScope[lAttr.subtextMenu])(lScope));
          //  if (e.clientX > 800) {
          //      $("#subtextmenuplaylist-node").css("left", -225 );
          //  }
           
        });
        lElem.on("mouseenter", function (e) {
            //  $('.menu-list--submenu').prepend($compile(lScope[lAttr.subtextMenu])(lScope));
            lElem.append($compile(lScope[lAttr.subtextMenu])(lScope));
            if (e.clientX > 1000) {
                $("#subtextmenuplaylist-node").css("left", -225);
            }
        });
        lElem.on("mouseleave", function (e) {
            if ($("#subtextmenuplaylist-node"))
                $("#subtextmenuplaylist-node").remove();
        });
        //lElem.on("wheel", function (e) {

        //    if ($("#subtextmenuplaylist-node"))
        //        $("#subtextmenuplaylist-node").remove();
        //});

        //lElem.on("click", function (e) {
        //    if ($("#subtextmenuplaylist-node"))
        //        $("#subtextmenuplaylist-node").remove();

        //});
        //right: calc(100 % - 10px);
    };
    return subtextMenu;
});
app.controller('index', function ($scope, $rootScope, dataservice, dataShare) {
    $scope.initData = function () {
        dataservice.taiZingChat(function (rs) {
            rs = rs.data;
            $scope.taiZingChat = rs;
        });
       
       
        
    }
    $scope.initData();
    $scope.OnClick = function () {
        dataShare.data = "ok em yeu";
        window.location.href = '/home1#/nhacmoi';
    } 
    $scope.$on("SendDown", function (evt, data) {
        $scope.Message11 = "Inside MyController1 : " + data;
    });
    $scope.thoiGian = function (text) {
        var audio = document.getElementById(text);
                

        
    };
});
app.directive("ngContextmenu", function () {
    contextMenu = { replace: false };
    contextMenu.restrict = "AE";

    contextMenu.scope = { "visible": "=" };
    contextMenu.link = function ($scope, lElem, lAttr) {
        lElem.on("contextmenu", function (e) {

            e.preventDefault();

           
            $scope.$apply(function () {
                $scope.visible = true;
            })



        });
        lElem.on("mouseleave", function (e) {

            
           

        });
    };
    return contextMenu;
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
//function showCoords(event) {
//    var x = event.pageX;
//    var y = event.pageY;
//    $("#contextmenu-node").css("left", x);
//    $("#contextmenu-node").css("top", y);
//};
app.controller('nhacmoi', function ($scope, $cookies, dataservice, dataShare) {
    $scope.isVisible = false;
    $scope.$watch('isVisible', function () {
        console.log('change');
    })
    var tenbaihat,linkhinhanh = "";
    $scope.dspCookie = "";
    $scope.moChiTietBaiHat = function (data) {
        $cookies.putObject('chiTietBaiHat', data);
        $scope.dspCookie = $cookies.getObject('chiTietBaiHat');
        tenbaihat = $scope.dspCookie.object.tenbaihat;
        linkhinhanh11 = $scope.dspCookie.object.linkhinhanh;                                                                                                                                                                                                                                        //link hinh                                                                                                                                                                     //tenbai                                                                                                                                         //luoc yeu thich                                                                                      // luoc nghe                                                                                                                                                           //taiXuong                                                                                                                                                                                             // lời bài hát                                                                                               //chặn                                                                                                                                             //thêm vào danh sách phát                                                                                                                       // phát tiếp theo                                            //subtextmenuPlaylist                                                                                                               //thêm vào plaulist                                                                                                                                                                                    //Bình Luận                                                                                                                                         // Sao chép Link                                                                                                                                      // chia sẻ                                                                                                                                                                                                                                           
        $scope.contextMenuBaiHat1 = '<div id="contextmenu-node" class="zm-contextmenu song-menu"><div class="menu"><ul class="menu-list"><div class="menu-list--submenu"><div class="media song-info-menu"><div class="media-left"><figure class="image is-40x40"><img style="height:40px;" src="' + linkhinhanh11 + '" alt=""></figure></div><div class="is-w-150 media-content"><a><div class="title-wrapper"><span class="item-title title" title="Sài Gòn Ơi Xin Lỗi Cảm Ơn">' + tenbaihat + '</span></div></a><div class="song-stats"><div class="stat-item"><i class="icon far fa-heart iconmusic" aria-hidden="true"></i><span>5K</span></div><div class="stat-item"><i class="icon fa fa-headphones" aria-hidden="true"></i><span>122K</span></div></div></div></div></div></ul><ul class="menu-list"><div class="group-button-menu"><div class="group-button-list"><button class="zm-btn button" ng-click="taiXuong()"><i class="icon fa fa-download" aria-hidden="true"></i><span>Tải xuống</span></button><button class="zm-btn button" tabindex="0"><i class="icon fa fa-book" aria-hidden="true"></i><span>Lời bài hát</span></button><span class="zm-btn button"><i class="icon fa fa-ban" aria-hidden="true"></i><span>Chặn</span></span></div></div></ul><ul class="menu-list"><li><button class="zm-btn button" tabindex="0"><i class="icon far fa-list-alt"></i><span>Thêm vào danh sách phát</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon fa fa-play" aria-hidden="true"></i><span>Phát tiếp theo</span></button></li><li ><div class="menu-list--submenu" subtext-menu="subtextMenuPlayList"><button class="zm-btn button" tabindex="0"><i class="icon fa fa-plus" aria-hidden="true"></i><span>Thêm vào playlist</span><i class="icon fa fa-chevron-right" style="margin-left: auto;"></i></button></div></li><li><button class="zm-btn button" tabindex="0"><i class="icon far fa-comment"></i><span>Bình luận</span><span class="comment-badge"></span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon fa fa-link"></i><span>Sao chép link</span></button></li><li><div class="menu-list--submenu"><button class="zm-btn button" tabindex="0"><i class="icon fa fa-share"></i><span>Chia sẻ</span><i class="icon fa fa-chevron-right" style="margin-left: auto;"></i></button></div></li></ul></div></div>';
    };
   
    $scope.date = new Date();
    $scope.initData = function () {
        dataservice.taiNhacMoi(function (rs) {
            rs = rs.data;
            $scope.taiNhacMoi = rs;
        });

     

    }
    $scope.vidu = '';
    $scope.taiXuong = function () {

        alert($scope.dspCookie.object.id);
    };
    $scope.subtextMenuPlayList = '<div id="subtextmenuplaylist-node" class="menu add-playlist-content submenu-content"><ul class="menu-list"><li class="search-box"><input class="input" placeholder="Tìm playlist"></li><li class="mar-t-10"><button class="zm-btn button" tabindex="0"><i class="icon ic- z-ic-svg ic-svg-add"></i><span>Tạo playlist mới</span></button></li></ul><div class="playlist-container"><div class="top-shadow "></div><div class="content"><div style="position: relative; background-color: #6a39af; overflow: hidden; width: 105%; height: 100%;"><div style="position: absolute; inset: 0px; overflow: hidden scroll; margin-right: -6px; margin-bottom: 0px;"><ul class="menu-list"><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>dsa</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Ok</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Playlist #3</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Playlist #2</span></button></li><li><button class="zm-btn button" tabindex="0"><i class="icon ic-list-music"></i><span>Fill</span></button></li></ul></div><div class="track-horizontal" style="position: absolute; height: 6px; transition: opacity 200ms ease 0s; opacity: 0;"><div style="position: relative; display: block; height: 100%; cursor: pointer; border-radius: inherit; background-color: rgba(0, 0, 0, 0.2); width: 0px;"></div></div><div class="track-vertical" style="position: absolute; width: 4px; transition: opacity 200ms ease 0s; opacity: 0; right: 2px; top: 2px; bottom: 2px; z-index: 100;"><div class="thumb-vertical" style="position: relative; display: block; width: 100%; height: 0px;"></div></div></div></div></div></div>';
    $scope.contextMenuBaiHat = "<div id='contextmenu-node'  class='zm-contextmenu song-menu'><div class='menu'><ul class='menu-list'><div class='menu-list--submenu'><div class='media song-info-menu'><div class='media-left'><figure class='image is-40x40'><img src='https://photo-resize-zmp3.zadn.vn/w94_r1x1_jpeg/cover/8/f/f/a/8ffa6834a803b7dad7128d26e6094b70.jpg' alt=''></figure></div><div class='is-w-150 media-content'><a><div class='title-wrapper'><span class='item-title title' title='Sài Gòn Ơi Xin Lỗi Cảm Ơn'>{{dspCookie.object.tenbaihat}}</span></div></a><div class='song-stats'><div class='stat-item'><i class='icon ic-like'></i><span>5K</span></div><div class='stat-item'><i class='icon ic-view'></i><span>122K</span></div></div></div></div></div></ul><ul class='menu-list'><div class='group-button-menu'><div class='group-button-list'><button class='zm-btn button'><i class='icon ic-download'></i><span>Tải xuống</span></button><button class='zm-btn button' tabindex='0'><i class='icon ic-add-lyric'></i><span>Lời bài hát</span></button><span class='zm-btn button'><i class='icon ic-denial'></i><span>Chặn</span></span></div></div></ul><ul class='menu-list'><li><button class='zm-btn button' tabindex='0'><i class='icon ic-add-play-now'></i><span>Thêm vào danh sách phát</span></button></li><li><button class='zm-btn button' tabindex='0'><i class='icon ic-play-next'></i><span>Phát tiếp theo</span></button></li><li><div class='menu-list--submenu'><button class='zm-btn button' tabindex='0'><i class='icon ic-add'></i><span>Thêm vào playlist</span><i class='icon ic-go-right'></i></button></div></li><li><button class='zm-btn button' tabindex='0'><i class='icon ic-comment'></i><span>Bình luận</span><span class='comment-badge'></span></button></li><li><button class='zm-btn button' tabindex='0'><i class='icon ic-link'></i><span>Sao chép link</span></button></li><li><div class='menu-list--submenu'><button class='zm-btn button' tabindex='0'><i class='icon ic-share'></i><span>Chia sẻ</span><i class='icon ic-go-right'></i></button></div></li></ul></div></div>";
    $scope.initData();
    $scope.Message11 = dataShare.data;
   

    $scope.thoiGian = function (text) {
        var audio = document.getElementById(text);



    };
});
app.controller('theloai', function ($scope, $cookies, dataservice, dataShare) {
  
    $scope.text = {
        key: ''
    }
    $scope.moDanhSachPhatTheLoai = function (data) {
      
        dataShare.data = data;
        $scope.dulieu11 = dataShare.data;

        $cookies.putObject('danhSachBaiHat', data);
        window.location.href = '/home1#/danhsachphat';
      
    
       
    } 
    $scope.okok = function (event) {

        alert("danhphat");
        event.stopPropagation();
    } 
    $scope.okokok = function () {

        alert("nhac");

    } 
 
    $scope.initData = function () {
        dataservice.taiZingChat(function (rs) {
            rs = rs.data;
            $scope.taiZingChat = rs;
          
        });
        dataservice.taiTheLoai(function (rs) {


            rs = rs.data;
            $scope.datataiTheLoai = rs;


            //$scope.valueTheLoai = rs[0].object.id;
            //$scope.text.key = $scope.valueTheLoai;
            //dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
            //    rs = rs.data;
            //    $scope.datataiDanhSachPhatTheLoai = rs;
            //    $scope.valueDanhSachPhatTheLoai = rs[0].object.id;
            //});

        });



    }
    $scope.so = 5;
    $scope.getNumber = function (num) {
        return new Array(num);
    }
    function TestCtrl($scope) {
        $scope.range = function (n) {
            return new Array(n);
        };
    };
    $scope.initData();
    $scope.Message11 = dataShare.data;


    $scope.thoiGian = function (text) {
        var audio = document.getElementById(text);



    };
});
app.controller('chitiettheloai', function ($scope, $routeParams, $cookies, dataservice, dataShare) {
   
  
    $scope.anhanh = $routeParams.id;
    $scope.text = {
        key: ''
    }
    $scope.text.key = $scope.anhanh;
    $scope.moDanhSachPhatTheLoai = function (data) {

        dataShare.data = data;
        $scope.dulieu11 = dataShare.data;

        $cookies.putObject('danhSachBaiHat', data);
        window.location.href = '/home1#/danhsachphat';



    }
    $scope.okok = function (event) {

        alert("danhphat");
        event.stopPropagation();
    }
    $scope.okokok = function () {

        alert("nhac");

    }

    $scope.initData = function () {
        dataservice.taiDanhSachBaiHatTheoTheLoai($scope.text,function (rs) {
            rs = rs.data;
            $scope.taiDanhSachBaiHatTheoTheLoai = rs;
        });
        dataservice.taiDanhSachPhatTheLoai($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDanhSachPhatTheLoai = rs;

        });
        
       


    }
    $scope.size = 0;
    $scope.next = function () {
        if ($scope.size + 6 == $scope.taiDanhSachPhatTheLoai.length)
            return;
        $scope.size += 1;

    }
    $scope.prev = function () {
        if ($scope.size == 0)
            $scope.size = 0;
        else
            $scope.size -= 1;

    }
    $scope.so = 5;
    $scope.getNumber = function (num) {
        return new Array(num);
    }
    function TestCtrl($scope) {
        $scope.range = function (n) {
            return new Array(n);
        };
    };
    $scope.initData();
    $scope.Message11 = dataShare.data;


    $scope.thoiGian = function (text) {
        var audio = document.getElementById(text);



    };
});
app.controller('danhsachphat', function ($scope, $routeParams, $cookies, dataservice, dataShare) {

    //$scope.duLieuDSPTL = dataShare.data;
    //$scope.dspCookie = $cookies.getObject('danhSachBaiHat');
    //$scope.okok = $scope.dspCookie.linkhinhanh;
    $scope.anhanh = $routeParams.id;
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
        return hDisplay +" "+ mDisplay ;
    }
    $scope.text = {
        key: '' 
    }
    $scope.text.key = $scope.anhanh;
    $scope.initData = function () {
        dataservice.taiDanhSachPhatTheLoaiHA($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDanhSachPhatTheLoaiHA = rs;
            $scope.hinhAnhDSPTL = $scope.taiDanhSachPhatTheLoaiHA[0].object.linkhinhanh;
        });
        dataservice.taiDanhSachBaiHat($scope.text, function (rs) {
            rs = rs.data;
            $scope.taiDanhSachBaiHat = rs;
            var okl = 0;
            for (var i = 0; i < $scope.taiDanhSachBaiHat.length; i++) {
                 okl += chuyenDoi($scope.taiDanhSachBaiHat[i].object.thoiluongbaihat);

            }      
            

            $scope.tongThoiGianPhat = secondsToHms(okl);
            //let gio = Math.floor(okl / 60*60);
            //let phut = Math.floor(okl - gio * 60 * 60);
           

            //// Adding a zero to the single digit time values
            //if (gio < 10) { gio = "0" + gio; }
            //if (phut < 10) { phut = "0" + phut; }
          

            //var gg = gio;
            //var pp = phut;
            $scope.soBaiHat = $scope.taiDanhSachBaiHat.length;

        });

    }
    $scope.initData();
  

    $scope.thoiGian = function (text) {
        var audio = document.getElementById(text);



    };
});

app.controller('add', function ($rootScope, $scope, dataservice, $uibModalInstance) {
    $scope.ok = function () {
        $uibModalInstance.close();
    };

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
        chude_id: '',
        link: '',
        linkhinhanh: ''
    }
    $scope.text = {
        key: ''
    }
    $scope.initData = function () {

        dataservice.loadtheloai(function (rs) {


            rs = rs.data;
            $scope.dataloadtheloai = rs;


            $scope.valueTheLoai = rs[0].object.id;
            $scope.text.key = $scope.valueTheLoai


        });

        dataservice.loadchude(function (rs) {
            rs = rs.data;
            $scope.dataloadchude = rs;
            $scope.valueChuDe = rs[0].object.id;

        });
    };
    //$scope.changeTheLoai = function () {
    //    $scope.text.key = $scope.valueTheLoai
    //    dataservice.loadchude($scope.text, function (rs) {
    //        rs = rs.data;
    //        $scope.dataloadchude = rs;
    //        $scope.valueChuDe = rs[0].id;
    //    });

    //};
    //$scope.changeChude = function () {


    //};
    $scope.initData();
    var formData = new FormData();
    var formData1 = new FormData();
    $scope.submit = function () {
        $scope.model.theloai_id = $scope.valueTheLoai;
        $scope.model.chude_id = $scope.valueChuDe;
        dataservice.uploadaudio(formData, function (rs) {
            rs = rs.data;
            $scope.model.link = rs;
            dataservice.uploadHinhAnh(formData1, function (rs) {
                rs = rs.data;
                $scope.model.linkhinhanh = rs;
                dataservice.createbaihat($scope.model, function (rs) {
                    rs = rs.data;
                    $scope.data = rs;
                });
            });
        });


        $uibModalInstance.dismiss('cancel');
    }
    $scope.getTheFiles = function ($files) {
        formData = new FormData();
        formData.append("File", $files[0]);
        //    for (var i = 0; i < $files.length; i++) {
        //        formData.append("File", $files[i]);
        //    }     
    }
    $scope.getTheFilesHinhAnh = function ($files) {
        formData1 = new FormData();
        formData1.append("File1", $files[0]);
        //for (var i = 0; i < $files.length; i++) {
        //    formData.append("File", $files[i]);
        //}

    }
});

    // Reset Values


    // Calculate the time left and the total duration




//Javascip -----
