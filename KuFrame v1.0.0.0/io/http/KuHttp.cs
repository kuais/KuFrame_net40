using System.IO;
using System.Net;
using System.Net.Cache;

namespace Ku.io.http
{
    public class KuHttp
    {
        public static void Download(string url, string path, IProgress listener = null)
        {
            using (var input = new WebClient().OpenRead(url))
            {
                using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    KuIO.Transfer(input, output, listener);
                }
            }
        }
        public static void Upload(string url, string path, IProgress listener = null)
        {
            using (var input = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var output = new WebClient().OpenWrite(url))
                {
                    KuIO.Transfer(input, output, listener);
                }
            }
        }

        public static void UploadFile(string url, string path, IProgress listener = null)
        {
            using (var input = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var end = "\r\n";
                var twoHyphens = "--";
                var boundary = "$******^";
                var contentType = "multipart/form-data";
                var fileName = Path.GetFileName(path);

                var client = new WebClient();
                client.Headers.Add("Connection", "Keep-Alive");
                client.Headers.Add("Charset", "UTF-8");
                client.Headers.Add("Content-Type", contentType + "; boundary=" + boundary);
                client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                using (var output = client.OpenWrite(url))
                {
                    var w = new StreamWriter(output);
                    w.Write(twoHyphens + boundary + end);
                    w.Write($"Content-Disposition: form-data; name=\"uploadfile\"; filename=\"{fileName}\"{end}");
                    w.Write(end);
                    w.Write(twoHyphens + boundary + twoHyphens + end);
                    w.Flush();
                    KuIO.Transfer(input, output, listener);
                    w.Write(end);
                    w.Write(twoHyphens + boundary + twoHyphens + end);
                    w.Flush();
                }
            }
        }
    }
}
