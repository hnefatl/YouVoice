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
        public static string Key = "AI39si7nTz7HOVT3MPUw7Fg1sXOXKfu78LeqT7-0HWHik1CG3QVvVhc1jb4t8kHkm0x9C_E_oU9rOgSbpDOHOLyVo-eEoYK8iQ";
        public static YouTubeService Youtube = new YouTubeService(new BaseClientService.Initializer() { ApiKey = Key });
    }
}
