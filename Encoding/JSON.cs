using Newtonsoft.Json;

namespace Encoding;

public static class JSON
{
    public static void Marshal(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T? Unmarshal<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        if (value != null)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        return default(T);
    }
}