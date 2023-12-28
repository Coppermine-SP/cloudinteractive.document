namespace DocumentTestApp
{
    internal static partial class Util
    {
        public static string HttpGet(string endpoint, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endpoint);
                return client.GetStringAsync(path).Result;
            }
        }
    }
}
