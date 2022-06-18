using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Helpers;

public static class SessionHelper
{
    public static void SerObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObjecFromJson<T>(this ISession session, string key)
    {
        var vaule = session.GetString(key);
        return vaule == null ? default(T) : JsonConvert.DeserializeObject<T>(vaule);
    }

}