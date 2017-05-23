using System;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
#if OK2003
using Mapi = Microsoft.Interop.Mapi;
#endif

namespace SeafileOutlookAddIn.AddIns
{
    /// <summary>
    /// MAPIHelper is a helper class to access MAPI Properties
    /// It uses Microsoft.Interop.MAPI
    /// We could also have used MAPI33 from Sergey Golovkin or Redemption from Dmitry Streblechenko
    /// The MAPI properties have been identified using OutlookSpy which can be downloaded from http://www.dimastr.com/outspy/
    /// </summary>
    internal static class MAPIHelper
    {

        /// <summary>
        /// Gets the approximate size of an Outlook Attachment (includes not only file size but also Outlook attachment properties)
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static long GetAttachmentSize(Outlook.Attachment attachment)
        {

            long lSizeRet = attachment.Size;
            System.Diagnostics.Trace.WriteLine("MAPIHelper: Attachment size is " + lSizeRet.ToString());
            return lSizeRet;
        }
        /// <summary>
        /// Gets the SMTP address from an addressEntry
        /// </summary>
        /// <returns></returns>
        public static string GetSmtpAddress(Outlook.AddressEntry addressEntry)
        {
            const string SMTP_TYPE = "SMTP";
            const string EX_TYPE = "EX";
            const string SMTP_TAG = "SMTP:";

            string sSmtpAddressRet = null;

            //There is no way to access the sending account in Outlook 2003 API.
            //Accounts are defined HKCU\Software\Microsoft\Windows NT\CurrentVersion\Windows Messaging Subsystem\Profiles\<PROFILE NAME>\9375CFF0413111d3B88A00104B2A6676
            //They can also be accessed through MAPI (and CDO)
            //IMAPIProp objMAPIProp = MAPIHelper.GetMapiProp(messageItem.Session.CurrentUser.AddressEntry.MAPIOBJECT);
            //PropValue objPropValue = Globals.GetOneProp(objMAPIProp, new PropTag(0x39FE001E));
            
            if (addressEntry.Type == SMTP_TYPE)
            {
                System.Diagnostics.Trace.WriteLine("MAPIHelper: Address type is SMTP");

                System.Diagnostics.Debug.Assert(addressEntry.AddressEntryUserType == Microsoft.Office.Interop.Outlook.OlAddressEntryUserType.olSmtpAddressEntry);
                sSmtpAddressRet = addressEntry.Address;
            }
            else if (addressEntry.Type == EX_TYPE)
            {
                System.Diagnostics.Trace.WriteLine("MAPIHelper: Address type is EXCHANGE");
                System.Diagnostics.Debug.Assert(addressEntry.AddressEntryUserType == Microsoft.Office.Interop.Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry);
                //Outlook.ExchangeUser objExchangeUser = (Outlook.ExchangeUser)addressEntry;
                //return objExchangeUser.PrimarySmtpAddress;

                //PR_EMS_AB_PROXY_ADDRESSES - See: http://microsoft-personal-applications.hostweb.com/TopicMessages/microsoft.public.outlook.program_vba/2002535/1/Default.aspx
                System.Diagnostics.Trace.WriteLine("MAPIHelper: Getting value of property PR_EMS_AB_PROXY_ADDRESSES");
                string[] arrProxyAddresses = (string[])addressEntry.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x800F101E");
                if ((arrProxyAddresses != null) && (arrProxyAddresses.Length > 0))
                {
                    foreach (string sProxyAddress in arrProxyAddresses)
                    {
                        System.Diagnostics.Trace.WriteLine("Found proxy address: " + sProxyAddress);
                        if (sProxyAddress.StartsWith(SMTP_TAG))
                        {
                            sSmtpAddressRet = sProxyAddress.Substring(SMTP_TAG.Length);
                            break;
                        }
                    }
                }
                else
                {
                    //PR_SMTP_ADDRESS
                    System.Diagnostics.Trace.WriteLine("MAPIHelper: no smtp address in PR_EMS_AB_PROXY_ADDRESSES, getting address from _SMTP_ADDRESS");
                    sSmtpAddressRet = (string)addressEntry.PropertyAccessor.GetProperty("http://schemas.microsoft.com/mapi/proptag/0x39FE001E");
                }
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("MAPIHelper: Address type " + addressEntry.Type + " not supported for " + addressEntry.Address);
            }

            if (!String.IsNullOrEmpty(sSmtpAddressRet))
                System.Diagnostics.Trace.WriteLine("MAPIHelper: Smtp address is " + sSmtpAddressRet);
            return sSmtpAddressRet;
        }
    }
}
