using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TcpServer.Helper
{
    /// <summary>
    /// json序列化帮助类
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 模型转换为json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static string ToJson<T>(T Data) where T: class, new()
        {
            if (Data == null)
            {
                return "{}";
            }

            return JsonConvert.SerializeObject(Data);
            //return theObject.ToString();
        }
        /// <summary>
        /// 字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theJson"></param>
        /// <returns></returns>
        public static T FromJson<T>(string theJson) where T : class, new()
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
