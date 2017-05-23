﻿using Newtonsoft.Json;
using SeafileClient.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeafileClient.Types
{
    /// <summary>
    /// Entry of a directory (file or directory)
    /// </summary>
    public class SeafDirEntry
    {
        public virtual string Id { get; set; }

        [JsonIgnore]
        public virtual String LibraryId { get; set; }

        public virtual DirEntryType Type { get; set; }        

        public virtual string Name { get; set; }

        /// <summary>
        /// Time of the last modification of this entry
        /// (as UNIX timestamp)
        /// </summary>
        [JsonProperty("mtime"), JsonConverter(typeof(SeafileTimestampConverter))]
        public virtual DateTime? Timestamp { get; set; }

        /// <summary>
        /// File size (only if Type is File)
        /// </summary>
        public virtual long Size { get; set; }        

        /// <summary>
        /// The full path of this item in its library
        /// (including the filename if the entry represents a file)
        /// </summary>        
        [JsonIgnore]
        public virtual string Path { get; set; }

        /// <summary>
        /// Return the directory of this entry
        /// (if the entry is a directory this is the same as Path,
        ///  if it is a file then the directory cotnaining the file is returned)
        /// </summary>
        [JsonIgnore]
        public string Directory
        {
            get
            {
                if (Type == DirEntryType.Dir)
                    return Path;

                return System.IO.Path.GetDirectoryName(Path).Replace("\\", "/");
            }
        }
    }

    public enum DirEntryType
    {
        File,        
        Dir
    }
}
