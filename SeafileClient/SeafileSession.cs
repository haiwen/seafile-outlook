using Newtonsoft.Json;
using SeafileClient.Requests;
using SeafileClient.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SeafileClient.Requests.Libraries;
using SeafileClient.Requests.UserAccountInfo;
using SeafileClient.Requests.Directories;
using SeafileClient.Requests.Files;
using SeafileClient.Utils;
using System.Threading;

namespace SeafileClient
{
    /// <summary>
    /// Represents an established seafile session
    /// and offers methods for data access
    /// All methods are executed asynchronously
    /// </summary>
    public class SeafileSession
    {
        /// <summary>
        /// The user this session belongs to
        /// </summary>
        public string Username { get; private set; }

        public Uri ServerUri { get; private set; }
        public string AuthToken { get; private set; }

        private ISeafileWebConnection webConnection;

        /// <summary>
        /// Tries to connect to the given seafile server using the default ISeafWebConnection implementation and returns an appropriate session object on success
        /// </summary>
        /// <param name="serverUrl">The server url to connect to (including protocol (http or https) and port)</param>
        /// <param name="username">The username to login with</param>
        /// <param name="pwd">The password for the given user (will be overwritten with zeros as soon as the authentication request has been sent)</param>
        public static async Task<SeafileSession> Establish(Uri serverUri, string username, char[] pwd)
        {
            return await Establish(SeafileConnectionFactory.GetDefaultConnection(), serverUri, username, pwd);
        }

        /// <summary>
        /// Tries to connect to the given seafile server using the given ISeafWebConnection implementation and returns an appropriate session object on success
        /// </summary>
        /// <param name="serverUrl">The server url to connect to (including protocol (http or https) and port)</param>
        /// <param name="username">The username to login with</param>
        /// <param name="pwd">The password for the given user (will be overwritten with zeros as soon as the authentication request has been sent)</param>
        public static async Task<SeafileSession> Establish(ISeafileWebConnection seafWebConnection, Uri serverUri, string username, char[] pwd)
        {
            if (seafWebConnection == null)
                throw new ArgumentNullException("seafWebConnection");
            if (serverUri == null)
                throw new ArgumentNullException("serverUri");
            if (username == null)
                throw new ArgumentNullException("username");
            if (pwd == null)
                throw new ArgumentNullException("pwd");            

            // authenticate the user
            AuthRequest req = new AuthRequest(username, pwd);
            var response = await seafWebConnection.SendRequestAsync(serverUri, req);
            return new SeafileSession(seafWebConnection, username, serverUri, response.Token);  
        }

        /// <summary>
        /// Create a seafile session for the given authentication token
        /// Will automatically connect to the seafile server and check if the token is valid
        /// and retrieve the username for the given token
        /// </summary>
        public static async Task<SeafileSession> FromToken(Uri serverUri, string authToken)
        {
            return await FromToken(SeafileConnectionFactory.GetDefaultConnection(), serverUri, authToken);
        }

        /// <summary>
        /// Create a seafile session for the given authentication token
        /// Will automatically connect to the seafile server and check if the token is valid
        /// and retrieve the username for the given token using the given ISeafWebConnection
        /// </summary>
        public static async Task<SeafileSession> FromToken(ISeafileWebConnection seafWebConnection, Uri serverUri, string authToken)
        {
            if (seafWebConnection == null)
                throw new ArgumentNullException("seafWebConnection");
            if (serverUri == null)
                throw new ArgumentNullException("serverUri");
            if (authToken == null)
                throw new ArgumentNullException("authToken");

            // get the user for the token and check if the token is valid at the same time
            AccountInfoRequest infoReq = new AccountInfoRequest(authToken);
            var accInfo = await seafWebConnection.SendRequestAsync(serverUri, infoReq);
            return new SeafileSession(seafWebConnection, accInfo.Email, serverUri, authToken);
        }

        /// <summary>
        /// Create a seafile session for the given username and authentication token 
        /// The validity of the username or token are not checked
        /// (if they are wrong you may not be able to execute requests)
        /// </summary>
        public static SeafileSession FromToken(Uri serverUri, string username, string authToken)
        {
            return FromToken(SeafileConnectionFactory.GetDefaultConnection(), serverUri, username, authToken);
        }

        /// <summary>
        /// Create a seafile session for the given username and authentication token using the given ISeafWebConnection
        /// The validity of the username or token are not checked
        /// (if they are wrong you may not be able to execute requests)
        /// </summary>
        public static SeafileSession FromToken(ISeafileWebConnection seafWebConnection, Uri serverUri, string username, string authToken)
        {
            if (seafWebConnection == null)
                throw new ArgumentNullException("seafWebConnection");
            if (serverUri == null)
                throw new ArgumentNullException("serverUri");
            if (username == null)
                throw new ArgumentNullException("username");
            if (authToken == null)
                throw new ArgumentNullException("authToken");
            
            return new SeafileSession(seafWebConnection, username, serverUri, authToken);
        }

        /// <summary>
        /// Wraps an already existing seafile session
        /// use SeafSession.Establish(...) to establish a new connection and retrieve an authentication token
        /// from the Seafile server
        /// </summary>
        /// <param name="username">The username of the account authToken belongs to</param>
        /// <param name="serverUri">The server url to connect to (including protocol (http or https) and port)</param>
        /// <param name="authToken">The authentication token as received from the Seafile server</param>
        private SeafileSession(ISeafileWebConnection seafWebConnection, string username, Uri serverUri, string authToken)
        {
            webConnection = seafWebConnection;
            Username = username;
            ServerUri = serverUri;
            AuthToken = authToken;
        }

        /// <summary>
        /// Ping the server without authentication
        /// </summary>
        /// <param name="serverUri"></param>
        /// <returns></returns>
        public static async Task<bool> Ping(Uri serverUri)
        {
            return await Ping(SeafileConnectionFactory.GetDefaultConnection(), serverUri);
        }

        /// <summary>
        /// Ping the server without authentication using the given ISeafWebConnection
        /// </summary>
        /// <param name="serverUri"></param>
        /// <returns></returns>
        public static async Task<bool> Ping(ISeafileWebConnection seafWebConnection, Uri serverUri)
        {
            PingRequest r = new PingRequest();
            return await seafWebConnection.SendRequestAsync(serverUri, r);
        }

        /// <summary>
        /// Retrieve some general information about the Seafile server at the given address
        /// </summary>
        /// <param name="seafWebConnection"></param>
        /// <param name="serverUri"></param>
        /// <returns></returns>
        public static async Task<SeafServerInfo> GetServerInfo(Uri serverUri)
        {
            return await GetServerInfo(SeafileConnectionFactory.GetDefaultConnection(), serverUri);
        }

        /// <summary>
        /// Retrieve some general information about the Seafile server at the given address using
        /// the given ISeafWebConnection
        /// </summary>
        /// <param name="seafWebConnection"></param>
        /// <param name="serverUri"></param>
        /// <returns></returns>
        public static async Task<SeafServerInfo> GetServerInfo(ISeafileWebConnection seafWebConnection, Uri serverUri)
        {
            GetServerInfoRequest r = new GetServerInfoRequest();
            return await seafWebConnection.SendRequestAsync(serverUri, r);
        }

        /// <summary>
        /// Ping the server using the current session
        /// (Can be used to check whether the session is still valid)
        /// </summary>        
        /// <returns></returns>
        public async Task<bool> Ping()
        {
            PingRequest r = new PingRequest(AuthToken);
            return await webConnection.SendRequestAsync(ServerUri, r);
        }

        /// <summary>
        /// Retrieve the account info for the current session
        /// </summary>
        /// <returns></returns>
        public async Task<AccountInfo> CheckAccountInfo()
        {
            AccountInfoRequest req = new AccountInfoRequest(AuthToken);
            return await webConnection.SendRequestAsync<AccountInfo>(ServerUri, req);
        }

        /// <summary>
        /// Retrieve the avatar of the current user
        /// </summary>        
        /// <param name="size">Size of the requested image in pixels (width=height)</param>
        public async Task<UserAvatar> GetUserAvatar(int size)
        {
            return await GetUserAvatar(Username, size);
        }

        /// <summary>
        /// Retrieve the avatar of the given user
        /// </summary>        
        /// <param name="username">The username to retrieve the avatar for</param>
        /// <param name="size">Size of the requested image in pixels (width=height)</param>
        public async Task<UserAvatar> GetUserAvatar(string username, int size)
        {
            UserAvatarRequest req = new UserAvatarRequest(AuthToken, username, size);
            return await webConnection.SendRequestAsync(ServerUri, req);
        }

        /// <summary>
        /// Return the current user's default library        
        /// </summary>
        /// <returns>The user's default library or null if no default library exists</returns>
        public async Task<SeafLibrary> GetDefaultLibrary()
        {
            GetDefaultLibraryRequest req = new GetDefaultLibraryRequest(AuthToken);
            var res = await webConnection.SendRequestAsync(ServerUri, req);

            if (!res.Exists)
                return null;

            return await GetLibraryInfo(res.LibraryId);
        }

        /// <summary>
        /// Retrieve information for the library with the given id
        /// </summary>        
        public async Task<SeafLibrary> GetLibraryInfo(string libraryId)
        {
            GetLibraryInfoRequest req = new GetLibraryInfoRequest(AuthToken, libraryId);
            return await webConnection.SendRequestAsync(ServerUri, req);
        }

        /// <summary>
        /// List all libraries of the current user (excluding shared libraries from other users)
        /// </summary>
        /// <returns></returns>
        public async Task<IList<SeafLibrary>> ListLibraries()
        {
            ListLibrariesRequest req = new ListLibrariesRequest(AuthToken);
            return await webConnection.SendRequestAsync(ServerUri, req);            
        }

        /// <summary>
        /// Return all shared libraries of the current user
        /// </summary>        
        public async Task<IList<SeafSharedLibrary>> ListSharedLibraries()
        {
            ListSharedLibrariesRequest req = new ListSharedLibrariesRequest(AuthToken);
            return await webConnection.SendRequestAsync(ServerUri, req);
        }

     
     
        /// <summary>
        /// List the content of the root directory ("/") of the given linrary
        /// </summary>
        public async Task<IList<SeafDirEntry>> ListDirectory(SeafLibrary library)
        {
            return await ListDirectory(library, "/");
        }

        /// <summary>
        /// List the content of the given directory
        /// </summary>        
        public async Task<IList<SeafDirEntry>> ListDirectory(SeafDirEntry dirEntry)
        {
            dirEntry.ThrowOnNull(nameof(dirEntry));

            if (dirEntry.Type != DirEntryType.Dir)
                throw new ArgumentException("The given directory entry is not a directory.");

            return await ListDirectory(dirEntry.LibraryId, dirEntry.Path);
        }

        /// <summary>
        /// List the content of the given directory of the given library
        /// </summary>
        public async Task<IList<SeafDirEntry>> ListDirectory(SeafLibrary library, string directory)
        {
            library.ThrowOnNull(nameof(library));            

            return await ListDirectory(library.Id, directory);
        }

        /// <summary>
        /// List the content of the given directory of the given library
        /// </summary>
        public async Task<IList<SeafDirEntry>> ListDirectory(String libraryId, string directory)
        {
            libraryId.ThrowOnNull(nameof(libraryId));
            directory.ThrowOnNull(nameof(directory));

            if (!directory.EndsWith("/"))
                directory += "/";

            ListDirectoryEntriesRequest req = new ListDirectoryEntriesRequest(AuthToken, libraryId, directory);
            var dLst = await webConnection.SendRequestAsync(ServerUri, req);
            return dLst;
        }

        /// <summary>
        /// Create a new directory in the given library
        /// </summary>
        /// <param name="library">Library to create the directory in</param>
        /// <param name="path">Path of the directory to create</param>
        /// <returns>A value which indicates if the creation was successful</returns>
        public async Task<bool> CreateDirectory(SeafLibrary library, string path)
        {
            library.ThrowOnNull(nameof(library));

            return await CreateDirectory(library.Id, path);
        }

        /// <summary>
        /// Create a new directory in the given library
        /// </summary>
        /// <param name="libraryId">The id of the library to create the directory in</param>
        /// <param name="path">Path of the directory to create</param>
        /// <returns>A value which indicates if the creation was successful</returns>
        public async Task<bool> CreateDirectory(String libraryId, string path)
        {
            libraryId.ThrowOnNull(nameof(libraryId));
            path.ThrowOnNull(nameof(path));

            CreateDirectoryRequest req = new CreateDirectoryRequest(AuthToken, libraryId, path);
            return await webConnection.SendRequestAsync(ServerUri, req);
        }

       

     
        /// <summary>
        /// Retrieve information about the given file in the given library
        /// </summary>
        /// <param name="library">The library the file belongs to</param>
        /// <param name="filePath">Path to the file</param>
        /// <returns>The directory entry of the file</returns>
        public async Task<SeafDirEntry> GetFileDetail(SeafLibrary library, string filePath, CancellationToken cancelToken)
        {
            library.ThrowOnNull(nameof(library));

            return await GetFileDetail(library.Id, filePath, cancelToken);
        }

        /// <summary>
        /// Retrieve information about the given file in the given library
        /// </summary>
        /// <param name="library">The id of the library the file belongs to</param>
        /// <param name="filePath">Path to the file</param>
        /// <returns>The directory entry of the file</returns>
        public async Task<SeafDirEntry> GetFileDetail(string libraryId, string filePath, CancellationToken cancelToken)
        {
            libraryId.ThrowOnNull(nameof(libraryId));
            filePath.ThrowOnNull(nameof(filePath));

            GetFileDetailRequest req = new GetFileDetailRequest(AuthToken, libraryId, filePath);
            return await webConnection.SendRequestAsync(ServerUri, req,cancelToken);
        }

       
       

      
        /// <summary>
        /// Get a download link for the given file
        /// </summary>
        /// <param name="dirEntry">The directory entry for the file to download</param>        
        /// <returns>The download link which is valid once</returns>
        public async Task<string> GetFileDownloadLink(SeafDirEntry dirEntry)
        {
            dirEntry.ThrowOnNull(nameof(dirEntry));

            if (dirEntry.Type != DirEntryType.File)
                throw new ArgumentException("The given directory entry is not a file.");

            return await GetFileDownloadLink(dirEntry.LibraryId, dirEntry.Path);
        }

        /// <summary>
        /// Get a download link for the given file
        /// </summary>
        /// <param name="library">The library the file is in</param>
        /// <param name="path">The path to the file</param>
        /// <returns>The download link which is valid once</returns>
        public async Task<string> GetFileDownloadLink(SeafLibrary library, string path)
        {
            library.ThrowOnNull(nameof(library));

            return await GetFileDownloadLink(library.Id, path);
        }

        /// <summary>
        /// Get a download link for the given file
        /// </summary>
        /// <param name="libraryId">The id of the library the file is in</param>
        /// <param name="path">The path to the file</param>
        /// <returns>The download link which is valid once</returns>
        protected async Task<string> GetFileDownloadLink(String libraryId, string path)
        {
            libraryId.ThrowOnNull(nameof(libraryId));
            path.ThrowOnNull(nameof(path));

            GetFileDownloadLinkRequest req = new GetFileDownloadLinkRequest(AuthToken, libraryId, path);
            return await webConnection.SendRequestAsync(ServerUri, req);
        }

        /// <summary>
        /// Uploads a single file
        /// Does not replace already existing files, instead the file will be renamed (e.g. test(1).txt if test.txt already exists)
        /// Use UpdateSingle to replace the contents of an already existing file
        /// </summary>            
        /// <param name="library">The library the file should be uploaded to</param>
        /// <param name="targetDirectory">The directory the file should be uploaded to</param>
        /// <param name="targetFilename">The name of the file</param>
        /// <param name="fileContent">The new content of the file</param>
        /// <param name="progressCallback">Optional progress callback (will report percentage of upload)</param>
        public async Task<string> UploadSingle(SeafLibrary library, string targetDirectory, string targetFilename, Stream fileContent, CancellationToken cancelToken,Action<float> progressCallback = null)
        {
            library.ThrowOnNull(nameof(library));

            return await UploadSingle(library.Id, targetDirectory, targetFilename, fileContent, cancelToken,progressCallback);
        }

        /// <summary>
        /// Uploads a single file
        /// Does not replace already existing files, instead the file will be renamed (e.g. test(1).txt if test.txt already exists)
        /// Use UpdateSingle to replace the contents of an already existing file
        /// </summary>                
        /// <param name="libraryId">The id of the library the file should be uploaded to</param>
        /// <param name="targetDirectory">The directory the file should be uploaded to</param>
        /// <param name="targetFilename">The name of the file</param>
        /// <param name="fileContent">The new content of the file</param>
        /// <param name="progressCallback">Optional progress callback (will report percentage of upload)</param>
        public async Task<string> UploadSingle(string libraryId, string targetDirectory, string targetFilename, Stream fileContent, CancellationToken cancelToken,Action<float> progressCallback = null)
        {
            // to upload files we need to get an upload link first            
            var req = new GetUploadLinkRequest(AuthToken, libraryId);
            string uploadLink = await webConnection.SendRequestAsync(ServerUri, req, cancelToken);
            
            UploadFilesRequest upReq = new UploadFilesRequest(AuthToken, uploadLink, targetDirectory, targetFilename, fileContent, progressCallback);
            return await webConnection.SendRequestAsync(ServerUri, upReq, cancelToken);
         
        }

        /// <summary>
        /// Retrieve a link to upload files for the given library
        /// </summary>                
        /// <param name="libraryId">The library the file should be uploaded to</param>
        public async Task<string> GetUploadLink(SeafLibrary library)
        {
            library.ThrowOnNull(nameof(library));
            return await GetUploadLink(library.Id);
        }

        /// <summary>
        /// Retrieve a link to upload files for the given library
        /// </summary>                
        /// <param name="libraryId">The id of the library the file should be uploaded to</param>
        public async Task<string> GetUploadLink(string libraryId)
        {
            libraryId.ThrowOnNull(nameof(libraryId));

            // to upload files we need to get an upload link first            
            var req = new GetUploadLinkRequest(AuthToken, libraryId);
            return await webConnection.SendRequestAsync(ServerUri, req);
        }
    
     

        /// <summary>
        /// Provide the password for the given encrypted library
        /// in order to be able to access its contents
        /// (listing directory contents does NOT require the library to be decrypted)
        /// </summary>
        public async Task<bool> DecryptLibrary(SeafLibrary library, char[] password)
        {
            DecryptLibraryRequest r = new DecryptLibraryRequest(AuthToken, library.Id, password);
            return await webConnection.SendRequestAsync(ServerUri, r);
        }

 

        /// <summary>
        /// Removes the file from the list of starred files
        /// </summary>
        /// <param name="dirEntry">The file to unstar</param>
        public async Task<string> CreatShareLink(SeafDirEntry dirEntry, CancellationToken cancelToken,string password = null,string expire= null)
        {
            CreateShareLinkRequest req = new CreateShareLinkRequest(AuthToken, dirEntry.LibraryId, dirEntry.Path,password,expire);
            return await webConnection.SendRequestAsync(ServerUri, req,cancelToken);
        }
    }
}
