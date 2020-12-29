using System;
using System.IO;
using System.Net;
using System.Net.Cache;

namespace Ku.io.http
{
    public class KuHttp
    {
        public Stream Download(string url)
        {
            return new WebClient().OpenRead(url);
        }
        public Stream Upload(string url)
        {
            return new WebClient().OpenWrite(url);
        }
        public void Download(string url, string path, IProgress listener = null)
        {
            using (var input = Download(url))
            {
                using (var output = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    Transfer(input, output, listener);
                }
            }
        }
        public void Upload(string url, string path, IProgress listener = null)
        {
            using (var input = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (var output = Upload(url))
                {
                    Transfer(input, output, listener);
                }
            }
        }

        public void UploadFile(string url, string path, IProgress listener = null)
        {
            var fileName = Path.GetFileName(path);
            using (var input = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var end = "\r\n";
                var twoHyphens = "--";
                var boundary = "$******^";
                var contentType = "multipart/form-data";

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
                    Transfer(input, output, listener);
                    w.Write(end);
                    w.Write(twoHyphens + boundary + twoHyphens + end);
                    w.Flush();
                }
            }
        }

        public void Transfer(Stream input, Stream output, IProgress listener = null)
        {
            long total = input.Length;
            long current = 0;
            byte[] buffer = new byte[4096];
            int l;
            listener?.OnStart();
            try
            {
                while (true)
                {
                    l = input.Read(buffer, 0, buffer.Length);
                    if (l <= 0)
                        break;
                    output.Write(buffer, 0, l);
                    current += l;
                    listener?.OnProgress(current, total);
                }
                output.Flush();
            }
            catch (Exception ex)
            {
                listener?.OnError(ex);
            }
            listener?.OnStop();
        }
    }
}
