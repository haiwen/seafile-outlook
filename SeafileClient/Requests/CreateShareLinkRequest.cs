using SeafileClient.Types;
using SeafileClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;


namespace SeafileClient.Requests
{
    class CreateShareLinkRequest : SessionRequest<string>
    {
        public string LibraryId { get; set; }

        public string Password { get; set; }

        public string Expire { get; set; }


        public string Path { get; set; }

        public override HttpAccessMethod HttpAccessMethod
        {
            get { return HttpAccessMethod.Post; }
        }

        public override string CommandUri
        {
            get
            {
             
                return "api/v2.1/share-links/";
            }
        }

        public CreateShareLinkRequest(string authToken, string libraryId, string path, string password, string expire)
            : base(authToken)
        {
            LibraryId = libraryId;
            Path = path;

            if (!Path.StartsWith("/"))
                Path = "/" + Path;

            Password = password;
            Expire = expire;
        }

        public override IEnumerable<KeyValuePair<string, string>> GetPostParameters()
        {
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.AddRange(base.GetPostParameters());

            parameters.Add(new KeyValuePair<string, string>("repo_id", LibraryId));
            parameters.Add(new KeyValuePair<string, string>("path", Path));
            if (!string.IsNullOrEmpty(Password))
            {
                parameters.Add(new KeyValuePair<string, string>("password", Password));
            }
            if (!string.IsNullOrEmpty(Expire))
            {
                parameters.Add(new KeyValuePair<string, string>("expire",Expire));
            }
            return parameters;
        }

        public override SeafError GetSeafError(HttpResponseMessage msg)
        {
            switch (msg.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new SeafError(msg.StatusCode, SeafErrorCode.FileNotFound);
                case HttpStatusCode.BadRequest:
                    return new SeafError(msg.StatusCode, SeafErrorCode.PathDoesNotExist);
                default:
                    return base.GetSeafError(msg);
            }
        }

        public override async System.Threading.Tasks.Task<string> ParseResponseAsync(HttpResponseMessage msg)
        {
            string content = await msg.Content.ReadAsStringAsync();
            return content.Trim('\"');
        }
    }
}