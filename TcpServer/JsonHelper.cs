using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TcpServer
{
    public static class JsonHelper
    {

        public static string toJson<T>(T Data) where T: class, new()
        {
            if (Data == null)
            {
                return "{}";
            }

            return JsonConvert.SerializeObject(Data);
            //return theObject.ToString();
        }

        public static T fromJson<T>(string theJson) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(theJson))
            {
                return default(T);
            }
            try
            {
               return JsonConvert.DeserializeObject<T>(theJson);
             

            }
            catch (Exception ex)
            {
                Console.WriteLine("转换失败:"+ex);
                return default(T);
            }
        }

    }
}
