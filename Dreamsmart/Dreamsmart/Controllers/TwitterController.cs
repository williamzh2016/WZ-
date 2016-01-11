using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;
using System.Data;
using Dreamsmart.Models;

namespace Dreamsmart.Controllers
{
    public class TwitterController : Controller
    {

        private readonly List<Dreamsmart.Models.Tweet> lstTweets = new List<Dreamsmart.Models.Tweet>();


        public ActionResult ProcessSubmit(String btnSubmit)
        {
            switch (btnSubmit)
            {
                case "Retrieve":
                    if (Request.Form["keyword"] != null)
                    {
                        TempData["keyword"] = Request.Form["keyword"];
                    }
                    return RedirectToAction("RetrieveTweets");
                case "Save to DB":
                    if (Request.Form["keyword"] != null)
                    {
                        TempData["keyword"] = Request.Form["keyword"];
                    }
                    return RedirectToAction("SaveTweets");
                default:
                    return RedirectToAction("ProcessSubmit");
            }

        }

        // GET: Twitter
        public ActionResult RetrieveTweets()
        {
            string strHashTag = String.Empty;
            if (TempData["keyword"] == null)
            {
                GetTweets();
            }
            else
            {
                strHashTag = TempData["keyword"].ToString();
                GetTweets(strHashTag);
                ViewData["txtHashTag"] = strHashTag;
            }

            ModelState.Clear();
            return View(lstTweets);
        }

        // GET: Twitter
        public ActionResult LoadSavedTweets()
        {
            string strHashTag = String.Empty;

            if (Request.Form["keyword"] != null)
            {
                TempData["keyword"] = Request.Form["keyword"];
            }

            if (TempData["keyword"] == null)
            {
                LoadTweets();
            }
            else
            {
                strHashTag = TempData["keyword"].ToString();
                LoadTweets(strHashTag);
                ViewData["txtHashTag"] = strHashTag;
            }

            ModelState.Clear();
            return View(lstTweets);
        }

        public void GetTweets(string keyword = "jakarta")
        {
            try
            {
                //Set Credentials using the following Parameter : CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_TOKEN_SECRET
                Auth.SetUserCredentials("UbxfK2d5fQPoJ5ZTuJHdUNGDv", "ZS1CjmCkNDoMsKouDm1WDSxU22yESqs8cqbzRiq2t4hdJYH32d",
                    "4716392004-vPXcDEeoXbJmfUffVLbYaX4lGtEA8WoXK1zJReA", "1bWqICSAhDGkxT3Zp3C4X9gx1uVG6wjDlc4vvPwz7dnNj");
                if (keyword.Substring(0, 1) != "#")
                {
                    keyword = "#" + keyword;
                }

                var tokenRateLimits = RateLimit.GetCurrentCredentialsRateLimits();
                if (tokenRateLimits.ApplicationRateLimitStatusLimit.Remaining > 0)
                {
                    var searchParameter = Search.CreateTweetSearchParameter(keyword);
                    var tweets = Search.SearchTweets(searchParameter);

                    List<ITweet> objList = tweets.ToList();


                    foreach (ITweet objTweet in objList)
                    {
                        Dreamsmart.Models.Tweet objTw = new Dreamsmart.Models.Tweet();
                        objTw.Id = objTweet.Id.ToString();
                        objTw.TweetText = objTweet.Text;
                        objTw.RetweetCount = objTweet.RetweetCount;
                        objTw.CreatedBy = objTweet.CreatedBy.ToString();
                        objTw.CreatedDate = objTweet.CreatedAt;
                        objTw.HashTag = keyword;
                        lstTweets.Add(objTw);
                    }
                    Session["lstTweets"] = lstTweets;
                }

            }
            catch(Exception ex) 
            {
                // Exception 
            }

        }

        public void LoadTweets(string keyword = "")
        {
            DataTable objDataTable = new DataTable();
            Dreamsmart.Models.Tweet objTweet = new Dreamsmart.Models.Tweet();
            objDataTable = objTweet.Load(keyword);
            if (objDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < objDataTable.Rows.Count; i++)
                {
                    Dreamsmart.Models.Tweet objTw = new Dreamsmart.Models.Tweet();
                    objTw.Id = objDataTable.Rows[i]["Id"].ToString();
                    objTw.TweetText = objDataTable.Rows[i]["TweetText"].ToString();
                    objTw.RetweetCount = Convert.ToInt64(objDataTable.Rows[i]["RetweetCount"].ToString());
                    objTw.CreatedBy = objDataTable.Rows[i]["CreatedBy"].ToString();
                    objTw.CreatedDate = Convert.ToDateTime(objDataTable.Rows[i]["CreatedDate"].ToString());
                    objTw.HashTag = objDataTable.Rows[i]["HashTag"].ToString();
                    lstTweets.Add(objTw);
                }
            }

        }

        // Save Tweets
        public ActionResult SaveTweets()
        {
            Save();
            return RedirectToAction("RetrieveTweets");
        }

        public void Save()
        {
            List<Dreamsmart.Models.Tweet> objLstTweets = new List<Dreamsmart.Models.Tweet>();
            objLstTweets = (List<Dreamsmart.Models.Tweet>)Session["lstTweets"];
            foreach (Dreamsmart.Models.Tweet obj in objLstTweets)
            {
                obj.Save();
            }
            TempData["alertMessage"] = "Tweets have been succesfully saved.";
        }

    }
}