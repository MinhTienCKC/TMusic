#pragma checksum "C:\Users\ad\Desktop\file delete đô an\16-08-21\8.7.2021\TFourMusic\TFourMusic\TFourMusic\Views\Home1\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "195de1f7b09ac12ad0d163f6fed76d4a66932339"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home1_Index), @"mvc.1.0.view", @"/Views/Home1/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\ad\Desktop\file delete đô an\16-08-21\8.7.2021\TFourMusic\TFourMusic\TFourMusic\Views\_ViewImports.cshtml"
using TFourMusic;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ad\Desktop\file delete đô an\16-08-21\8.7.2021\TFourMusic\TFourMusic\TFourMusic\Views\_ViewImports.cshtml"
using TFourMusic.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"195de1f7b09ac12ad0d163f6fed76d4a66932339", @"/Views/Home1/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"240190d8cc9a07e1c7b93301c75fa9a4160d2d7f", @"/Views/_ViewImports.cshtml")]
    public class Views_Home1_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/nhacmoi.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/views/front-end/Home1/controller.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\ad\Desktop\file delete đô an\16-08-21\8.7.2021\TFourMusic\TFourMusic\TFourMusic\Views\Home1\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<script src=""https://ajax.googleapis.com/ajax/libs/angularjs/1.6.9/angular.min.js""></script>
<script src=""https://ajax.googleapis.com/ajax/libs/angularjs/1.6.9/angular-cookies.min.js""></script>

<div id=""container"" style=""background: linear-gradient(to right, rgb(255, 186, 90) 0%, rgb(255, 186, 90) 50%, rgb(4, 194, 206) 50%, rgb(4, 194, 206) 100%); height: unset; overflow: unset;"">

    <div id=""contentMain"" class=""content"" ng-app=""App_ESEIM"" ng-controller=""Ctrl_ESEIM"">
        
        <div style=""min-height:100px"" ng-view>
            
        </div>
        <div class=""zm-sidebar"">
            <div class=""zm-sidebar-wrapper"">
                <nav class=""zm-navbar"">
                    <div class=""zm-navbar-brand"">
                        <a class=""zm-btn button"" tabindex=""0"">
                            <div class=""zmp3-logo"">

                            </div>
                            <label class=""zmp3-label-top"">Mp3</label>
                        </a>
                    </div");
            WriteLiteral(@">
                </nav>
                <nav class=""zm-navbar zm-navbar-main"">
                    <ul class=""zm-navbar-menu"">
                        <li ng-click=""clicknav(1)"" ng-class=""{'zm-navbar-itemclick': model == 1}"" class=""zm-navbar-item sidebar-lib"">
                            <a class=""item-navbar-hover"" ng-class=""{'hover-item-nav':model==1}"" href=""#"" title=""Cá Nhân"">
                                <i class=""fas fa-user-circle icon""></i><span");
            BeginWriteAttribute("class", " class=\"", 1535, "\"", 1543, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                                    Cá
                                    Nhân
                                </span>
                            </a>
                        </li>
                        <li ng-click=""clicknav(2)"" ng-class=""{'zm-navbar-itemclick': model == 2}"" class=""zm-navbar-item is-active"">
                            <a class=""item-navbar-hover"" ng-class=""{'hover-item-nav':model == 2}""");
            BeginWriteAttribute("href", " href=\"", 1965, "\"", 1972, 0);
            EndWriteAttribute();
            WriteLiteral(" title=\"Khám Phá\">\r\n                                <i class=\"fas fa-bullseye icon\"></i><span");
            BeginWriteAttribute("class", " class=\"", 2066, "\"", 2074, 0);
            EndWriteAttribute();
            WriteLiteral(@">
                                    Khám
                                    Phá
                                </span>
                            </a>
                        </li>
                        <li ng-click=""clicknav(3)"" ng-class=""{'zm-navbar-itemclick': model == 3}"" class=""zm-navbar-item"">
                            <a ng-class=""{'hover-item-nav':model == 3}"" class=""item-navbar-hover""");
            BeginWriteAttribute("href", " href=\"", 2487, "\"", 2494, 0);
            EndWriteAttribute();
            WriteLiteral(" title=\"#zingchart\">\r\n                                <i class=\"fas fa-trophy icon\"></i><span");
            BeginWriteAttribute("class", " class=\"", 2588, "\"", 2596, 0);
            EndWriteAttribute();
            WriteLiteral(@">#zingchart</span>
                            </a>
                        </li>
                        <li ng-click=""clicknav(4)"" ng-class=""{'zm-navbar-itemclick': model == 4}"" class=""zm-navbar-item"">
                            <a class=""item-navbar-hover""");
            BeginWriteAttribute("href", " href=\"", 2861, "\"", 2868, 0);
            EndWriteAttribute();
            WriteLiteral(" title=\"Theo Dõi\">\r\n                                <i class=\"fas fa-users icon\"></i><span");
            BeginWriteAttribute("class", " class=\"", 2959, "\"", 2967, 0);
            EndWriteAttribute();
            WriteLiteral(@" ng-class=""{'hover-item-nav':model==4}"">
                                    Theo
                                    Dõi
                                </span>
                            </a>
                        </li>
                    </ul>
                </nav>
                <div class=""sidebar-divide""> <i class=""boder-item""> </i></div>

                <div class=""zm-sidebar-scrollbar-big"">
                    <div class=""zm-sidebar-scrollbar"">
                        <nav class=""zm-navbar zm-navbar-main mar-t-5 mar-b-0"">
                            <ul class=""zm-navbar-menu"">
                                <li class=""zm-navbar-item"" onclick=""window.location='/home1#/nhacmoi';"">
                                    <a class=""item-navbar-hover""");
            BeginWriteAttribute("href", " href=\"", 3751, "\"", 3758, 0);
            EndWriteAttribute();
            WriteLiteral(@" title=""Nhạc Mới"">
                                        <i class=""fas fa-music icon""></i></i><span>
                                            Nhạc
                                            Mới
                                        </span>
                                    </a>
                                </li>
                                <li class=""zm-navbar-item"" onclick=""window.location='/home1#/theloai';"">
                                    <a class=""item-navbar-hover""");
            BeginWriteAttribute("href", " href=\"", 4263, "\"", 4270, 0);
            EndWriteAttribute();
            WriteLiteral(@" title=""Thể Loại"">
                                        <i class=""fas fa-icons icon""></i>
                                        <span>
                                            Thể
                                            Loại
                                        </span>
                                    </a>
                                </li>
                                <li class=""zm-navbar-item"">
                                    <a class=""item-navbar-hover"" href=""#"" title=""Top 100"">
                                        <i class=""far fa-star icon""></i>
                                        <span>
                                            Top
                                            100
                                        </span>
                                    </a>
                                </li>

                            </ul>
                        </nav>

                        <nav class=""zm-navbar zm-navbar-my-music pad-t-15"">
 ");
            WriteLiteral(@"                           <div class=""main-title title""><label class=""label-name"">THƯ VIỆN</label></div>
                            <ul class=""zm-navbar-menu library-personal"">
                                <li class=""zm-navbar-item"">
                                    <a class=""item-navbar-hover"" href=""/mymusic/library/song"">
                                        <i class=""icon"">
                                            <img src=""https://zjs.zadn.vn/zmp3-desktop/releases/v1.0.13/static/media/my-song.cf0cb0b4.svg"">
                                        </i><span>
                                            Bài
                                            hát
                                        </span>
                                    </a>
                                </li>
                                <li class=""zm-navbar-item"">
                                    <a class=""item-navbar-hover"" href=""/mymusic/library/playlist"">
                                        <i cla");
            WriteLiteral(@"ss=""icon"">
                                            <img src=""https://zjs.zadn.vn/zmp3-desktop/releases/v1.0.13/static/media/my-playlist.7e92a5f0.svg"">
                                        </i><span>Playlist</span>
                                    </a>
                                </li>
                                <li class=""zm-navbar-item"">
                                    <a class=""item-navbar-hover"" href=""/mymusic/history"">
                                        <i class=""icon"">
                                            <img src=""https://zjs.zadn.vn/zmp3-desktop/releases/v1.0.13/static/media/my-history.374cb625.svg"">
                                        </i><span>
                                            Gần
                                            đây
                                        </span>
                                    </a>
                                </li>
                            </ul>
                        </nav>

                ");
            WriteLiteral(@"    </div>

                </div>

                <div class=""add-playlist-sidebar"">
                    <button class=""add-playlist-sidebar-button "" tabindex=""0"">
                        <i class=""fas fa-plus iconadd-playlist""></i><span>Tạo playlist mới</span>
                    </button>

                </div>

            </div>
        </div>
        <div class=""header"">
            <div class=""header__left"">
                <div class=""header__left__action"">
                    <div class=""header__left__action-item"">
                        <i class=""fas fa-arrow-left icon iconheadersearch""></i>
                    </div>
                    <div class=""header__left__action-item"">
                        <i class=""fas fa-arrow-right icon iconheadersearch""></i>
                    </div>
                </div>
                <div class=""header__left__search"">
                    <i class=""fas fa-search searchitem""></i>
                    <input type=""text"" class=""form-search");
            WriteLiteral(@""" placeholder=""Nhập tên bài hát, Nghệ sĩ hoặc MV..."">
                </div>
            </div>
            <div class=""header__action"">
                <div class=""header__action-item"">
                    <i class=""fas fa-tshirt icon""></i>
                </div>
                <div ng-click=""upLoadNhac()"" class=""header__action-item"">
                    <i class=""fas fa-upload icon""></i>
                </div>
                <div class=""header__action-item"">
                    <i class=""fas fa-cog icon""></i>
                </div>
                <div class=""header__action-item"">
                    <i class=""far fa-user-tie icon""></i>
                </div>
            </div>
        </div>
        <div class=""wapper"">
            <div class=""wapper__left"">
                <div id=""image_music_avatar"" class=""wapper__musicRolling"">

                </div>
                <div class=""wapper__musicName"">
                    <span id=""music_name"" class=""music_name"">Lời xin lỗi vụng v");
            WriteLiteral(@"ề</span>
                    <span id=""siger_name"" style=""display:block;"">Quân AP</span>
                </div>
                <div class=""wapper__musicAction"">
                    <div class=""wapper__musicAction-icon"">
                        <i class=""far fa-heart iconmusic""></i>
                    </div>
                    <div class=""wapper__musicAction-icon"">
                        <i class=""fas fa-ellipsis-h iconmusic""></i>
                    </div>
                </div>
            </div>
            <div class=""wapper__contain"">
                <div class=""wapper__contain__action"">
                    <div ng-click=""ramdomSongs()"" class=""wapper__contain__action-icon"" ng-class=""{'icon_click_toggle':  ramdomsong}"">
                        <i class=""fas fa-random iconmusic""></i>
                    </div>
                    <div class=""wapper__contain__action-icon"" ng-click=""clickBackSongs()"">
                        <i class=""fas fa-step-backward iconmusic""></i>
               ");
            WriteLiteral(@"     </div>
                    <div ng-click=""Play()"" id=""play_pause_music"" class=""wapper__contain__action-icon-play-pause"">
                        <i ng-show=""playmusic==1"" class=""fas fa-pause""></i>
                        <i ng-show=""playmusic==0"" class=""fas fa-play""></i>
                    </div>
                    <div class=""wapper__contain__action-icon"" ng-click=""clickNextSongs()"">
                        <i class=""fas fa-step-forward iconmusic""></i>
                    </div>
                    <div ng-click=""repeateSong()"" class=""wapper__contain__action-icon"" ng-class=""{'icon_click_repeate_toggle':  repeate}"">
                        <i class=""fas fa-redo iconmusic""></i>
                    </div>
                </div>
                <div class=""wapper__contain__process"">
                    <div id=""process__start_time"" class=""process__start"">
                        00:00
                    </div>
                    <input ng-click=""seekTo()"" id=""progress"" step=""1"" type=""ran");
            WriteLiteral(@"ge"" value=""0"" min=""0"" max=""100"" class=""process__bar progress"">

                    <div id=""process__end_time"" class=""process__end"">
                        00:00
                    </div>
                </div>
            </div>
            <div class=""wapper__right"">

                <div class=""wapper__right__icon"">
                    <i class=""fas fa-microphone""></i>
                </div>
                <div class=""wapper__right__icon"">
                    <div class=""wapper__volume"">
                        <i class=""fas fa-volume-down""></i>
                        <div class=""process__volume""></div>
                    </div>
                </div>
                <div class=""wapper__right__icon"">
                    <i class=""fas fa-search-plus""></i>
                </div>
                <div class=""wapper__right__icon__space"">
                </div>

                <div class=""wapper__right__icon"">
                    <label class=""wapper__right__icon label-playlist"" n");
            WriteLiteral(@"g-click=""clickLabelPlaylist()"" ng-class=""{'click_label_playlist': lickPlaylist}"" for=""show_nav"">
                        <i class=""fas fa-th-list icon_list""></i>
                    </label>
                </div>

            </div>
        </div>
        <audio id=""audio"" hidden");
            BeginWriteAttribute("src", " src=\"", 12751, "\"", 12757, 0);
            EndWriteAttribute();
            WriteLiteral(@">
        </audio>
        <input class=""nav_input"" type=""checkbox"" hidden id=""show_nav"" />

        <div class=""nav__right"">
            <div class=""nav__right__title"">
                <div class=""nav__right__title__tap"">
                    <button ng-class=""{'button--tapclick': buttonnav == 1}"" class=""button--tap"">Danh sách phát</button>
                    <button class=""button--tap"">Nghe gần đây</button>
                </div>
                <div class=""nav__right__title__action"">
                    <div class=""nav__right__title__action-item"">
                        <i class=""fas fa-clock""></i>
                    </div>
                    <div class=""nav__right__title__action-item"">
                        <i class=""far fa-caret-square-down""></i>
                    </div>
                </div>
            </div>
            <div class=""nav__right__content"">
                <ul id=""playlist_music_ul"" class=""nav__right__list"">
                    <li ng-click=""lickSongPlayList($");
            WriteLiteral(@"index)"" class=""nav__right__list__item"" data-index=""{{$index}}"" ng-class=""{'item_play_list': sttmusic == $index}"" ng-repeat=""x in songs"">

                        <div class=""music__player"">
                            <div class=""music__player__image"">
                                <div ng-click=""Play()"">
                                    <div");
            BeginWriteAttribute("class", " class=\"", 14136, "\"", 14144, 0);
            EndWriteAttribute();
            WriteLiteral(@" ng-class=""{'git_music': sttmusic == $index && playmusic==1 }"">
                                    </div>
                                    <img class=""music--image""
                                         src=""{{x.image}}""
                                         alt=""Avatar"">


                                    <div class=""music--playMusic"" ng-show=""playmusic==0 &&sttmusic == $index"">
                                        <i class=""far fa-play-circle""></i>
                                    </div>
                                </div>
                            </div>
                            <div class=""music__player__contain"">
                                <div title=""{{x.name}}"" class=""music__player__contain-tilte"">{{x.name}}</div>
                                <div title=""{{x.singer}}"" class=""music__player__contain-singer"">{{x.singer}} </div>
                                <div id=""icon_option"" class=""icon_nav_right_final"">
                                    <i class=");
            WriteLiteral(@"""far fa-heart icon_nav_right_first""></i>
                                    <i class=""fas fa-ellipsis-v icon_nav_right_end""></i>
                                </div>

                            </div>
                        </div>

                    </li>

                </ul>
            </div>

        </div>
    </div>
</div>
");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "195de1f7b09ac12ad0d163f6fed76d4a6693233922611", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "195de1f7b09ac12ad0d163f6fed76d4a6693233923726", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
