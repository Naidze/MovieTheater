using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieTheater.Helpers
{
    public class CommonFunctions
    {
        public static string GetImageURL(string title, int year = 0)
        {
            string imageUrl = "";

            HttpWebResponse webResponse = null;
            string movieSearchUrl = "https://api.themoviedb.org/3/search/movie?api_key=6ea7ae272dc09cbc4fe1fe99933241da&language=en-US&query="
                + title.Replace(' ', '%')
                + "&page=1&include_adult=false"
                + string.Format("{0}", year > 0 ? "&year=" + year : "");
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(movieSearchUrl);
                request.Method = "GET";
                request.KeepAlive = true;
                request.ContentType = "application/json";
                webResponse = (HttpWebResponse)request.GetResponse();
            }
            catch { }

            JObject searchResult = ResponseToJSON(webResponse);
            try
            {
                string backDropPath = searchResult["results"].ToList()[0]["backdrop_path"].ToString();
                imageUrl = "https://image.tmdb.org/t/p/w500" + backDropPath;
            }
            catch { }

            return imageUrl;
        }

        public static string ResponseToString(HttpWebResponse response)
        {
            string resp = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                resp = sr.ReadToEnd();
            }
            return resp;
        }

        public static JObject ResponseToJSON(HttpWebResponse response)
        {
            return JObject.Parse(ResponseToString(response));
        }
    }
}
