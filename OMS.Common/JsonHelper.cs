using System;

namespace OMS.Common
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 高性能反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(json);
        }

        /// <summary>
        /// 高性能序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(object obj)
        {
            return ServiceStack.Text.JsonSerializer.SerializeToString(obj);
        }
    }
}