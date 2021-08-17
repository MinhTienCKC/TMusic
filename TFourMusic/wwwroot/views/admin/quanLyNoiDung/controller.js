var ctxfolderurl = "/views/admin/quanLyNoiDung";

var app = angular.module('T_Music', ["ui.bootstrap","ngRoute"]);
app.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolderurl + '/index.html',
            controller: 'index'
        })
        .when('/add/', {
            templateUrl: ctxfolderurl + '/add.html',
            controller: 'add'
        })
})
app.controller('T_Music', function () {

});
app.factory('dataservice', function ($http) {
    return {
        downloadBaiHatVeMayNguoiDung: function (data, callback) {
            $http.post('/Admin/QuanLyNoiDung/downloadBaiHatVeMayNguoiDung', data).then(callback);
        },
        xoaNhacDaTaiXuong: function (data, callback) {
            $http.post('/Admin/QuanLyNoiDung/xoaNhacDaTaiXuong', data).then(callback);
        },
        login: function (token, callback) {
            $http({
                method: 'post',
                url: '/Admin/DangNhap/Login',
                headers: {
                    Authorization: "Bearer " + token.idToken
                }

            }).then(callback);
        }
      
    }

});
app.controller('add', function ($rootScope, $scope) {

   



});
app.directive("contextMenu", function ($compile) {
    //contextMenu = {};
    //contextMenu.restrict = "AE";
/*    contextMenu.trigger = "left";*/
    contextMenu = { replace: false };
    contextMenu.restrict = "AE";

 
    contextMenu.link = function (lScope, lElem, lAttr) {
        lElem.on("contextmenu", function (e) {
            e.preventDefault(); // default context menu is disabled
            //  The customized context menu is defined in the main controller. To function the ng-click functions the, contextmenu HTML should be compiled.
           /* lElem.append($compile(lScope[lAttr.contextMenu])(lScope));*/
            lElem.append($compile(lScope[lAttr.contextMenu])(lScope));
            // The location of the context menu is defined on the click position and the click position is catched by the right click event.
            //$("#contextmenu-node").css("left", e.clientX);
            //$("#contextmenu-node").css("top", e.clientY);
        });
        lElem.on("mouseleave", function (e) {
         /*   console.log("Leaved the div");*/
            // on mouse leave, the context menu is removed.
            //if ($("#contextmenu-node"))
            //    $("#contextmenu-node").remove();
        });
        lElem.on("click", function (e) {
            
         $("#contextmenu-node").remove();
            if ($("#contextmenu-node")) {
                $("#contextmenu-node").remove();
                lElem.append($compile(lScope[lAttr.contextMenu])(lScope));
            }
               

        });
    };
    return contextMenu;
});
app.controller('index', function ($rootScope, $scope, dataservice) {
    $(".nav-noidung").addClass("active");
    var config = {
        apiKey: "AIzaSyAgokfQmJQ94XIHgQTHdZ3Kyd1rWUWPj5Q",
        authDomain: "tfourmusic-1e3ff.firebaseapp.com",
        databaseURL: "tfourmusic-1e3ff-default-rtdb.firebaseio.com",
        projectId: "tfourmusic-1e3ff",
        storageBucket: "tfourmusic-1e3ff.appspot.com"


    };
    firebase.initializeApp(config);
    //   const storageRef = firebase.storage().ref();
    //var desertRef = storageRef.child('music/admin/Hanh-Phuc-Cuoi-Cung-Truong-Viet-Thai.mp3');

    //    // Delete the file
    //    desertRef.delete().then(() => {
    //        alert("Thành công");
    //    }).catch((error) => {
    //        // Uh-oh, an error occurred!
    //    });
    const user = firebase.auth().currentUser;
    var storage = firebase.storage();
    var audio = document.getElementById('okokok');
    $scope.downLoad = function () {

        $scope.text = {
            linknhac: '',
            tentaixuong: '',
            key:''
        }
        $scope.text.linknhac = "https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2Fadmin%2Fadmin2462021172920175?alt=media&token=6627b9c3-995b-4c85-bbda-7781fad2e482";
        $scope.text.tentaixuong = "Chúng-ta-không-thuộc-về-nhau" + ".mp3";
        $("#btntaixuong").css("display", "block");
        dataservice.downloadBaiHatVeMayNguoiDung($scope.text, function (rs) {
            rs = rs.data;
            $scope.downloadBaiHatVeMayNguoiDung = rs;
            if ($scope.downloadBaiHatVeMayNguoiDung != "") {
                setTimeout(function () {
                    var downloadbaihat = document.getElementById("btntaixuong");
                    downloadbaihat.href = "../../../music/Download/" + $scope.downloadBaiHatVeMayNguoiDung;
                    downloadbaihat.click();
                      setTimeout(function () {
                                dataservice.xoaNhacDaTaiXuong($scope.text,function () {
                                rs = rs.data;
                                $scope.xoaNhacDaTaiXuong = rs;
                                    $("#btntaixuong").css("display", "none");
                            });
                      },1000);

                },6000);
            }                               
        });
       

       


    };
    alertify.set('notifier', 'position', 'bottom-left');
    $scope.text = function () {
      
        alertify.success("ok em");
       
    }
   // var httpsReference = storage.refFromURL('https://firebasestorage.googleapis.com/v0/b/tfourmusic-1e3ff.appspot.com/o/music%2Fadmin%2Fadmin2462021172920175?alt=media&token=6627b9c3-995b-4c85-bbda-7781fad2e482');
  /*  $uibModal.open({*/
    //    animation: true,
    //    templateUrl: ctxfolderurl + '/add.html',
    //    controller: 'add',
    //    backdrop: 'true',
    //    backdropClass: ".fade:not(.show)1",
    //    backdropClass: ".modal-backdrop1",
    //    backdropClass: ".col-lg-81",
    //    backdropClass: ".modal-content1",

    //    size: '100'
    //});
    $scope.myContextDiv = "<ul id='contextmenu-node'><li class='contextmenu-item' ng-click='clickedItem1()'> Item 1 </li><li     class='contextmenu-item' ng-click='clickedItem2()'> Item 2 </li></ul>";
    $scope.contextMenuBaiHat = "<div id='contextmenu-node' class='zm-contextmenu song-menu'><div class='menu'><ul class='menu-list'><div class='menu-list--submenu'><div class='media song-info-menu'><div class='media-left'><figure class='image is-40x40'><img src='https://photo-resize-zmp3.zadn.vn/w94_r1x1_jpeg/cover/8/f/f/a/8ffa6834a803b7dad7128d26e6094b70.jpg' alt=''></figure></div><div class='is-w-150 media-content'><a><div class='title-wrapper'><span class='item-title title' title='Sài Gòn Ơi Xin Lỗi Cảm Ơn'>Sài Gòn Ơi Xin Lỗi Cảm Ơn</span></div></a><div class='song-stats'><div class='stat-item'><i class='icon ic-like'></i><span>5K</span></div><div class='stat-item'><i class='icon ic-view'></i><span>122K</span></div></div></div></div></div></ul><ul class='menu-list'><div class='group-button-menu'><div class='group-button-list'><button class='zm-btn button'><i class='icon ic-download'></i><span>Tải xuống</span></button><button class='zm-btn button' tabindex='0'><i class='icon ic-add-lyric'></i><span>Lời bài hát</span></button><span class='zm-btn button'><i class='icon ic-denial'></i><span>Chặn</span></span></div></div></ul><ul class='menu-list'><li><button class='zm-btn button' tabindex='0'><i class='icon ic-add-play-now'></i><span>Thêm vào danh sách phát</span></button></li><li><button class='zm-btn button' tabindex='0'><i class='icon ic-play-next'></i><span>Phát tiếp theo</span></button></li><li><div class='menu-list--submenu'><button class='zm-btn button' tabindex='0'><i class='icon ic-add'></i><span>Thêm vào playlist</span><i class='icon ic-go-right'></i></button></div></li><li><button class='zm-btn button' tabindex='0'><i class='icon ic-comment'></i><span>Bình luận</span><span class='comment-badge'></span></button></li><li><button class='zm-btn button' tabindex='0'><i class='icon ic-link'></i><span>Sao chép link</span></button></li><li><div class='menu-list--submenu'><button class='zm-btn button' tabindex='0'><i class='icon ic-share'></i><span>Chia sẻ</span><i class='icon ic-go-right'></i></button></div></li></ul></div></div>";
    $scope.clickedItem1 = function () {
        console.log("Clicked item 1.");
    };
    $scope.clickedItem2 = function () {
        console.log("Clicked item 2.");
    };
    // $scope index lưu trữ tab hiện tại, ban đầu gán nó = tab1
    var objectUrl;
   
    $("#file").change(function (e) {
        var file = e.currentTarget.files[0];

        $("#filename").text(file.name);
        $("#filetype").text(file.type);
        $("#filesize").text(file.size);

        objectUrl = URL.createObjectURL(file);
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
            var duration = moment.duration(seconds, "seconds");
            //var dc = moment.duration(secondsn, "seconds");
            //var time = "";
            //var hours = duration.hours();
            //if (hours > 0) { time = hours + ":"; }

            //time = time + duration.minutes() + ":" + duration.seconds();
            //time = time + duration.minutes() + ":" + duration.seconds();
            $("#duration").text(total_duration);

           // URL.revokeObjectURL(objectUrl);
        });
    });
  
    

   
    $scope.btn = function () {
        $rootScope.showBanner = 2;
    }
    
    var database = firebase.database();
    var  ggProvider = new firebase.auth.GoogleAuthProvider();
    $scope.initData = function () {
        $scope.changeTab = function (index) {
            $scope.current_tab = index;

        };

    };

    $scope.initData();
    // Hàm đổi tab
   
   
    //$scope.isActiveTab = function (index) {
    //    return index === $scope.current_tab ;
    //};
    // Hàm kiểm tra có phải tab hiện tại hay không
    // hàm này sẽ trả về true/false và dùng kết hợp
    // với ng-class để active menu

    $scope.btnGoogle = function () {
      
        firebase.auth().signInWithPopup(ggProvider).then(function (result) {
            var token = result.credential.accessToken;
            var user = result.user;
            console.log('User>>Goole>>>>', user);
            userId = user.uid;

        }).catch(function (error) {
            console.error('Error: hande error here>>>', error.code)
        })
    }


});