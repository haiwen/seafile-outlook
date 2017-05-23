using SeafileClient.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SeafileClient.Requests.Files
{
    /// <summary>
    /// Request used to upload files    
    /// </summary>
    public class UploadFilesRequest : SessionRequest<string>
    {
        Action<float> UploadProgress;

        public string UploadUri { get; set; }

        public string TargetDirectory { get; set; }

        List<UploadFileInfo> files = new List<UploadFileInfo>();

        public List<UploadFileInfo> Files
        {
            get
            {
                return files;
            }
        }

        public override string CommandUri
        {
            get { return UploadUri; }
        }

        public override HttpAccessMethod HttpAccessMethod
        {
            get { return HttpAccessMethod.Custom; }
        }

        /// <summary>
        /// Create an upload request for a single file
        /// </summary>
        /// <param name="authToken"></param>
        /// <param name="uploadUri"></param>
        /// <param name="filename"></param>
        /// <param name="fileContent"></param>
        /// <param name="progressCallback"></param>
        public UploadFilesRequest(string authToken, string uploadUri, string targetDirectory, string filename, Stream fileContent, Action<float> progressCallback)
            : this(authToken, uploadUri, targetDirectory, progressCallback, new UploadFileInfo(filename, fileContent))
        {
            // --
        }

        /// <summary>
        /// Create an upload request for multiple file
        /// </summary>
        /// <param name="authToken"></param>
        /// <param name="uploadUri"></param>
        /// <param name="filename"></param>
        /// <param name="fileContent"></param>
        /// <param name="progressCallback"></param>
        public UploadFilesRequest(string authToken, string uploadUri, string targetDirectory, Action<float> progressCallback, params UploadFileInfo[] uploadFiles)
            : base(authToken)
        {
            UploadUri = string.Format("{0}?ret-json=1",uploadUri);
            UploadProgress = progressCallback;
            TargetDirectory = targetDirectory;

            files.AddRange(uploadFiles);
        }

        public override async Task<string> ParseResponseAsync(HttpResponseMessage msg)
        {
            string content = await msg.Content.ReadAsStringAsync();
            return content;
        }


        private bool IsGBCode(string word)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code or other code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)    //判断是否是GB2312
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsGBKCode(string word)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1) // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        public string GB2312ToUtf8(string gb2312String)
        {
            Encoding fromEncoding = Encoding.GetEncoding("gb2312");
            Encoding toEncoding = Encoding.UTF8;
            return EncodingConvert(gb2312String, fromEncoding, toEncoding);
        }

        public string GBKToUtf8(string gbkString)
        {
            Encoding fromEncoding = Encoding.GetEncoding("gbk");
            Encoding toEncoding = Encoding.UTF8;
            return EncodingConvert(gbkString, fromEncoding, toEncoding);
        }

        public string EncodingConvert(string fromString, Encoding fromEncoding, Encoding toEncoding)
        {
            byte[] fromBytes = fromEncoding.GetBytes(fromString);
            byte[] toBytes = Encoding.Convert(fromEncoding, toEncoding, fromBytes);

            string toString = toEncoding.GetString(toBytes);
            return toString;
        }


        //private static Stream BuildMultipartStream(string name, string fileName, byte[] fileBytes, string boundary)
        //{
        //    // Create multipart/form-data headers.
        //    byte[] firstBytes = Encoding.UTF8.GetBytes(String.Format(
        //        "--{0}\r\n" +
        //        "Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\n" +
        //        "\r\n",
        //        boundary,
        //        name,
        //        fileName));

        //    byte[] lastBytes = Encoding.UTF8.GetBytes(String.Format(
        //        "\r\n" +
        //        "--{0}--\r\n",
        //        boundary));

        //    int contentLength = firstBytes.Length + fileBytes.Length + lastBytes.Length;
        //    byte[] contentBytes = new byte[contentLength];


        //    // Join the 3 arrays into 1.
        //    Array.Copy(
        //        firstBytes,
        //        0,
        //        contentBytes,
        //        0,
        //        firstBytes.Length);
        //    Array.Copy(
        //        fileBytes,
        //        0,
        //        contentBytes,
        //        firstBytes.Length,
        //        fileBytes.Length);
        //    Array.Copy(
        //        lastBytes,
        //        0,
        //        contentBytes,
        //        firstBytes.Length + fileBytes.Length,
        //        lastBytes.Length);

        //    return new MemoryStream(contentBytes);
        //}

        public override HttpRequestMessage GetCustomizedRequest(Uri serverUri)
        {
            string boundary = "Upload---------" + Guid.NewGuid().ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, UploadUri);

            foreach (var hi in GetAdditionalHeaders())
                request.Headers.Add(hi.Key, hi.Value);

            
            var content = new MultipartFormDataContentEx(boundary);
            
            // Add files to upload to the request
            foreach (var f in Files)
            {
                //var fileContent = new StreamContent(f.FileContent);
                var fileContent = new ProgressableStreamContent(f.FileContent, (p) =>
                {
                    if (UploadProgress != null)
                        UploadProgress(p);
                });

                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                fileContent.Headers.ContentType.CharSet = "\'utf8\'";
                
                string fileName = f.Filename;

                if (IsGBCode(f.Filename))
                {
                    fileName = GB2312ToUtf8(f.Filename);
                }
                else if (IsGBKCode(f.Filename))
                {
                    fileName = GBKToUtf8(f.Filename);
                }

                // fileContent.Headers.ContentType.CharSet = "UTF-8";
                //fileContent.Headers.TryAddWithoutValidation("Content-Disposition", String.Format("form-data; name=\"file\"; filename=\"{0}\"", Files[0].Filename));
                String headerValue = "form-data; name=\"file\"; filename=\"" + Files[0].Filename + "\"";
                byte[] bytes = Encoding.UTF8.GetBytes(headerValue);
                headerValue = "";
                foreach (byte b in bytes)
                {
                    headerValue += (Char)b;
                }
                fileContent.Headers.Add("Content-Disposition", headerValue);
                content.Add(fileContent);
              
            }

            
            /*
            byte[] fileBytes = new byte[Files[0].FileContent.Length];
            files[0].FileContent.Read(fileBytes, 0, (int)Files[0].FileContent.Length);
            content.Add(new StreamContent(
                                  BuildMultipartStream("file", Files[0].Filename, fileBytes, boundary)));
            */
            /*
            string fileName = Files[0].Filename;

            if (IsGBCode(Files[0].Filename))
            {
                fileName = GB2312ToUtf8(Files[0].Filename);
            }
            else if (IsGBKCode(Files[0].Filename))
            {
                fileName = GBKToUtf8(Files[0].Filename);
            }

            var streamContent = new StreamContent(Files[0].FileContent);
            
            streamContent.Headers.Add("Content-Type", "application/octet-stream");
            streamContent.Headers.ContentType.CharSet = "UTF-8";
            streamContent.Headers.Add("Content-Disposition",
            "form-data; name=\"file\"; filename=\"" + fileName + "\"");
            content.Add(streamContent, "file", fileName);
            */

            // the parent dir to upload the file to
            string tDir = TargetDirectory;
            if (!tDir.StartsWith("/"))
                tDir = "/" + tDir;

            var dirContent = new StringContent(tDir, Encoding.UTF8);
            dirContent.Headers.ContentType = null;
            dirContent.Headers.TryAddWithoutValidation("Content-Disposition", @"form-data; name=""parent_dir""");
            

            content.Add(dirContent);

            // transmit the content length, for this we use the private method TryComputeLength() called by reflection
            long conLen;

            if (!content.ComputeLength(out conLen))
                conLen = 0;

            // the seafile-server implementation rejects the content-type if the boundary value is
            // placed inside quotes which is what HttpClient does, so we have to redefine the content-type without using quotes
            // and remove the actual content-type which uses quotes beforehand
            content.Headers.ContentType = null;
            content.Headers.ContentLength = null;
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
            //content.Headers.ContentType.CharSet = "UTF-8";
            //client.DefaultRequestHeaders.TransferEncodingChunked = true;                
            if (conLen > 0)
            {
                // in order to disable buffering
                // and make the progress work
                content.Headers.Add("Content-Length", conLen.ToString());
            }

            request.Content = content;
           
            return request;
        }
    }

    /// <summary>
    /// Information about a file which shall be uploaded
    /// </summary>
    public class UploadFileInfo
    {
        public string Filename { get; set; }
        public Stream FileContent { get; set; }

        public UploadFileInfo(string filename, Stream content)
        {
            Filename = filename;
            FileContent = content;
        }
    }

    /// <summary>
    /// Child class of MultipartFormDataContent which exposes the TryComputeLength function
    /// </summary>
    class MultipartFormDataContentEx : MultipartFormDataContent
    {
        public MultipartFormDataContentEx(String boundary)
            : base(boundary)
        {
            // --
        }

        public bool ComputeLength(out long length)
        {
            return base.TryComputeLength(out length);
        }
    }
}
