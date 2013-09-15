using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouVoice
{
    public static class Globals
    {
        public static bool Initialise()
        {
            try
            {
                Key = "AIzaSyBJJ6oOlj33qVIL1uwAAE_B2knN6FTiaQE";
                Youtube = new YouTubeService(new BaseClientService.Initializer() { ApiKey = Key });
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string Key;
        public static YouTubeService Youtube;
    }
}
