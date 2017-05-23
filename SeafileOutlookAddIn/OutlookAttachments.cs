using System;
using System.Collections.Generic;
using System.IO; //Directory, Path
using Microsoft.Win32; //Registry, RegistryKey

using Outlook = Microsoft.Office.Interop.Outlook;



namespace SeafileOutlookAddIn.AddIns

{
    /// <summary>
    /// This class is a utility class which is reponsible for
    /// 1) Copying a package attachment to the file system where the content can be deserialized
    /// 2) Adding/replacing an package attachment from a file located in the file system
    /// </summary>
    internal static class OutlookAttachments
    {
        /// <summary>
        /// Checks for the presence of a package in the message attachments
        /// </summary>
        /// <param name="messageItem"></param>
        /// <returns></returns>
        public static bool HasPackageFileInAttachments(MessageItem messageItem)
        {
            if (messageItem == null)
                throw new ArgumentNullException("messageItem");

            //Check for the presence of several .velodoc attachments
            int iPackageCount = 0;
            foreach (Outlook.Attachment objAttachment in messageItem.Attachments)
            {
                if ((objAttachment.Type != Outlook.OlAttachmentType.olByReference)
                    && (objAttachment.Type != Outlook.OlAttachmentType.olByValue))
                    continue;

                if (Path.GetExtension(objAttachment.FileName).Equals(Constants.VelodocExt))
                {
                    iPackageCount++;
                }
            }

            //Raise an exception if there is more than one package in the attachments
            if (iPackageCount > 1)
                throw new NotSupportedException(Properties.Resources.ExceptionNoMoreThanOnePackageAsAttachment);

            return (iPackageCount == 1);
        }
        /// <summary>
        /// Get the package file from outlook attachments (the attachment file is copied to the file system)
        /// </summary>
        /// <param name="messageItem">the message item containing the attachments</param>
        /// <returns>a file system path to a file containing a package</returns>
        public static string GetPackageFileFromAttachments(MessageItem messageItem)
        {
            if (messageItem == null)
                throw new ArgumentNullException("messageItem");

            string sTempDirectory = messageItem.TempDirectory;
            System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(sTempDirectory));

            if (!Directory.Exists(sTempDirectory))
            {
                string sParentDirectory = Path.GetDirectoryName(sTempDirectory);
                if (!Directory.Exists(sParentDirectory)) //We could have received the mail item 
                {
                    string sTempFolder = Path.GetFileName(sTempDirectory);
                    System.Diagnostics.Debug.Assert(sTempFolder.EndsWith(Constants.TempDirExt));
                    sTempDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), sTempFolder);
                }
                Directory.CreateDirectory(sTempDirectory);
            }

            //Find the package and also check for the presence of several .velodoc attachments
            int iPackageCount = 0;
            Outlook.Attachment objPackageAttachment = null;
            foreach (Outlook.Attachment objAttachment in messageItem.Attachments)
            {
                if ((objAttachment.Type != Outlook.OlAttachmentType.olByReference)
                    && (objAttachment.Type != Outlook.OlAttachmentType.olByValue))
                    continue;

                if (Path.GetExtension(objAttachment.FileName).Equals(Constants.VelodocExt))
                {
                    iPackageCount++;
                    if (objPackageAttachment == null)
                        objPackageAttachment = objAttachment;
                }
            }

            //If no package can be found among the file attachments, return null
            if (objPackageAttachment == null)
            {
                System.Diagnostics.Debug.Assert(iPackageCount == 0);
                return null;
            }

            //Raise an exception if there is more than one package in the attachments
            if (iPackageCount > 1)
                throw new NotSupportedException(Properties.Resources.ExceptionNoMoreThanOnePackageAsAttachment);

            //if we have reached here, we have one single package as file attachment
            System.Diagnostics.Debug.Assert(iPackageCount == 1);

            //Build a full file path where to copy the attachment
            string sPackageFileRet = Path.Combine(sTempDirectory, objPackageAttachment.FileName);
            //TODO: Can we assume we get a valid file name?

            //If the file already exists on disk, remove it
            FileInfo objFileInfo = new FileInfo(sPackageFileRet);
            if (objFileInfo.Exists)
            {
                objFileInfo.Attributes &= ~FileAttributes.ReadOnly;
                objFileInfo.Delete();
            }

            //Copy the file attachement to the temp directory
            objPackageAttachment.SaveAsFile(sPackageFileRet);

            return sPackageFileRet;
        }
        /// <summary>
        /// Save a package file to outlook attachments (the file is added to the attachments or replaces an existing attachment)
        /// </summary>
        /// <param name="packageId">The package id</param>
        /// <param name="fileName">a file system path to a file containing a package</param>
        /// <param name="messageItem">the message item containing the attachments</param>
        public static void SavePackageFileToAttachments(Guid packageId, string fileName, MessageItem messageItem)
        {
            if (messageItem == null)
                throw new ArgumentNullException("messageItem");

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            if (!File.Exists(fileName))
                throw new FileNotFoundException(String.Format(
                    Properties.Resources.Culture,
                    Properties.Resources.ExceptionFileNotFound,
                    fileName));

            //Find the package among the attachments
            int iPackageCount = 0;
            int iPackageIndex = -1;
            //Note: this is VBA/COM, so collection indexes start at 1
            for (int i = 1; i <= messageItem.Attachments.Count; i++)
			{
                if ((messageItem.Attachments[i].Type != Outlook.OlAttachmentType.olByReference)
                    && (messageItem.Attachments[i].Type != Outlook.OlAttachmentType.olByValue))
                    continue;

                if (Path.GetExtension(messageItem.Attachments[i].FileName).Equals(Constants.VelodocExt))
                {
                    iPackageCount++;
                    if (iPackageIndex == -1)
                        iPackageIndex = i;
                }
			}

            //Raise an exception if there is more than one package in the attachments
            if (iPackageCount > 1)
                throw new NotSupportedException(Properties.Resources.ExceptionNoMoreThanOnePackageAsAttachment);

            //Remove the old package from message attachments
            if(iPackageIndex > 0)
            {
                System.Diagnostics.Debug.Assert(iPackageCount == 1);
                messageItem.Attachments.Remove(iPackageIndex);
            }

            //Update the package/transfer id in the file message
            messageItem.TransferId = packageId.ToString();

            //Update the temp directory on the message item in case
            //we reopen outlook to send a draft without reopening the package editor
            messageItem.TempDirectory = Path.GetDirectoryName(fileName);
            
            //Insert the new package file as message attachment
            messageItem.Attachments.Add(
                fileName,
                Outlook.OlAttachmentType.olByValue,
                1,
                Path.GetFileNameWithoutExtension(fileName));
        }
        /// <summary>
        /// Purge .velodoc files from Outlook temp directory
        /// </summary>
        public static void PurgeOutlookTempDir()
        {
            //For OK 2003 see http://support.microsoft.com/kb/817878/
            const string OK2003SECURITYKEY = "Software\\Microsoft\\Office\\11.0\\Outlook\\Security";
            const string OK2007SECURITYKEY = "Software\\Microsoft\\Office\\12.0\\Outlook\\Security";
            const string OUTLOOKTEMPDIR = "OutlookSecureTempFolder";

            try
            {
                string sOutlookTempDir = null;
                //Find the registry key for Outlook 2007
                RegistryKey objRegistryKey = Registry.CurrentUser.OpenSubKey(OK2007SECURITYKEY);
                if (objRegistryKey != null)
                {
                    sOutlookTempDir = (string)objRegistryKey.GetValue(OUTLOOKTEMPDIR);
                    objRegistryKey.Close();
                }

                //If not found, try with Outlook 2003
                if (String.IsNullOrEmpty(sOutlookTempDir))
                {
                    objRegistryKey = Registry.CurrentUser.OpenSubKey(OK2003SECURITYKEY);
                    if (objRegistryKey != null)
                    {
                        sOutlookTempDir = (string)objRegistryKey.GetValue(OUTLOOKTEMPDIR);
                        objRegistryKey.Close();
                    }
                }
                objRegistryKey = null;

                //If still not found, I am afraid we won't be able to purge
                if (String.IsNullOrEmpty(sOutlookTempDir))
                {
                    System.Diagnostics.Trace.WriteLine("OutlookAttachments: Outlook secure temp directory not found");
                    return;
                }
                System.Diagnostics.Trace.WriteLine("OutlookAttachments: Outlook secure temp directory is " + sOutlookTempDir);

                DirectoryInfo objDirectoryInfo = new DirectoryInfo(sOutlookTempDir);
                System.Diagnostics.Debug.Assert(objDirectoryInfo.Exists);

                FileInfo[] arrVelodocFile = objDirectoryInfo.GetFiles("*.velodoc", SearchOption.TopDirectoryOnly);

                //It is a bit complicated to keep track of which ones are currently opened in a package editor for a version 1
                //so for now, we will only purge the ones which have last been accessed more than one hour or so ago 
                foreach (FileInfo objFileInfo in arrVelodocFile)
                {
                    System.Diagnostics.Debug.Assert(objFileInfo.Exists);
                    if (objFileInfo.LastAccessTimeUtc.AddMinutes(Constants.AgeOfTempPackagesToPurge) < DateTime.UtcNow)
                    {
                        objFileInfo.Attributes &= ~FileAttributes.ReadOnly;
                        objFileInfo.Delete();
                    }
                }
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Trace.WriteLine(Ex);
            }
        }
    }
}
