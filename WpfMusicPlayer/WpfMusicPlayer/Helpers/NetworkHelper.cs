using System;
using System.Net;
using System.Runtime.Serialization.Json;
using WpfMusicPlayer.Json;

namespace WpfMusicPlayer.Helpers
{
    public class NetworkHelper
    {
        public Music GetJsonData(string requestUrl)
        {
            try
            {
                var request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                    var jsonSerializer = new DataContractJsonSerializer(typeof(Music));

                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    var jsonResponse = objResponse as Music;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                throw ;
            }
        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
