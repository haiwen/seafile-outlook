using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeafileOutlookAddIn
{
    class Constants
    {
        public const string SeafileAddinDownloadUrl = "http://www.seafile.com/download";
        public const string TempDirUserProp = "PackTempDir";
        public const string TransferIdUserProp = "TransferId";
        public const double AgeOfTempPackagesToPurge = 60d; //in minutes
        public const string TempDirExt = ".velodir";
        public const string VelodocExt = ".velodoc";
        public const string EditorAppName = "Seafile Editor";

        #region Outlook + Editor
        public const string IPMClass = "IPM.Note";
        public const int OutboxProcessingTimerDueTime = 5000; //5 secs
        public const int MaxRowsInLinksTable = 50;
        public const string FontName = "Arial";
        public const float StandardFontSize = 10F;
        public const float StandardPadding = 5F;
        public const float SmallFontSize = 8F;
        public const float SmallPadding = 2F;
#if DEBUG
        public const int OutboxProcessingTimerPeriod = System.Threading.Timeout.Infinite; //Run once
#else
        public const int OutboxProcessingTimerPeriod = 15000; //15 secs
#endif
        #endregion


    }
}
