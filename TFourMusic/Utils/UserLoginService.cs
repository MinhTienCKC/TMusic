using TFourMusic.Models;
using TFourMusic.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static TFourMusic.Utils.UserLoginService;
using Firebase.Database;
using FirebaseConfig = Firebase.Auth.FirebaseConfig;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Authorization;
namespace TFourMusic.Utils
{
    public interface IUserLoginService
    {
        //DangNhapController.UserModel GetPagingLength();
        //Task<string> GetPicter();
    }
   
    public class UserLoginService : IUserLoginService
    {
        //private string Key = " https://tfourmusic-1e3ff-default-rtdb.firebaseio.com/";
        //public DangNhapController.UserModel GetPagingLength()
        //{

        //     DangNhapController.UserModel paging = DangNhapController.okok;
                   
        //      //int  paging = 100;
        //    return paging;
        //}

        //public async Task<string> GetPicter()
        //{
        //    var firebase = new FirebaseClient(Key);
            
        //    string ok = DangNhapController.okok.email;
        //    // add new item to list of data and let the client generate new key for you (done offline)
        //    var dino = await firebase
        //      .Child("nguoidung")
        //      .OnceAsync<nguoidungModel>();

        //    //var ok1 = from user1 in dino
        //    //          where user1.Object.email.Equals(ok)
        //    //          select user1.Object.hinhdaidien;
                     
        //    //string paging = ok1.FirstOrDefault();
        //    string url = "https://s120-ava-talk.zadn.vn/0/5/1/5/0/120/4ff9b54b6eb2292f46483676eda192ae.jpg";
        //    var user = dino.FirstOrDefault(x => x.Object.email == ok);
        //    if (user != null)
        //    {
        //        url = user.Object.hinhdaidien;
        //    }
        //    return url;
        //    //int  paging = 100;
            
        //}

       
    }

}
