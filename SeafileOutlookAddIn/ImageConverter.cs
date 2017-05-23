
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeafileOutlookAddIn.AddIns
{
    /// <summary>
    /// Image converter to use with the ribbon
    /// </summary>
    /// <remarks>Source for this class at:
    /// http://msdn2.microsoft.com/en-us/library/aa338202.aspx#OfficeCustomizingRibbonUIforDevelopers_Images</remarks>
    internal sealed class ImageConverter : AxHost
    {
        #region Constructor
        /// <summary>
        /// Private constructor
        /// </summary>
        private ImageConverter()
            : base(String.Empty)
        {
        }
        #endregion

        #region Conversion functions
        /// <summary>
        /// Converts an image into a IPictureDisp for Outlook ribbons and command bars
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static stdole.IPictureDisp Convert(Image image)
        {
            return (stdole.IPictureDisp)AxHost.GetIPictureDispFromPicture(image);
        }
        /// <summary>
        /// Converts an icon into a IPictureDisp for Outlook ribbons and command bars
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        static public stdole.IPictureDisp Convert(Icon icon)
        {
            if (icon == null)
                return null;
            else
                return Convert(icon.ToBitmap());
        }
        /// <summary>
        /// Convert an IPictureDisp into an image
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        static public Image Convert(stdole.IPictureDisp picture)
        {
            return AxHost.GetPictureFromIPicture(picture);
        }
        #endregion
    }
}


/* Look at the following more complex implementation
[CLSCompliant(false)]
public class BCMImageConverter : AxHost
{
    // Fields
    private static Dictionary<string, IPictureDisp> iPictureCache = new Dictionary<string, IPictureDisp>();
    private static IrisResourceManager resMgr = new IrisResourceManager("Microsoft.BusinessSolutions.eCRM.Resources.Pictures", typeof(IrisResourceManager).Assembly);

    // Methods
    private BCMImageConverter() : base(string.Empty)
    {
    }

    public static Image GetImage(string imageName)
    {
        return GetImage(imageName, false, Color.Transparent);
    }

    public static Image GetImage(string imageName, bool makeTransparent, Color transparentColor)
    {
        Bitmap bitmap = (Bitmap) resMgr.GetObject(imageName);
        if (makeTransparent)
        {
            bitmap.MakeTransparent(transparentColor);
        }
        return bitmap;
    }

    [CLSCompliant(false)]
    public static IPictureDisp GetIPicture(string imageName)
    {
        return GetIPicture(imageName, false, Color.Transparent);
    }

    public static IPictureDisp GetIPicture(string imageName, bool makeTransparent, Color transparentColor)
    {
        string key = imageName + makeTransparent.ToString();
        IPictureDisp disp = null;
        if (!iPictureCache.ContainsKey(key))
        {
            Bitmap i = (Bitmap) resMgr.GetObject(imageName);
            if (makeTransparent)
            {
                i.MakeTransparent(transparentColor);
            }
            disp = ImageToIPicture(i);
            iPictureCache.Add(key, disp);
            return disp;
        }
        return iPictureCache[key];
    }

    public static IPictureDisp ImageToIPicture(Image i)
    {
        try
        {
            return (IPictureDisp) AxHost.GetIPictureDispFromPicture(i);
        }
        catch (Exception exception)
        {
            BCMTrace.Assert(false, exception.Message, exception.StackTrace);
            return null;
        }
    }

    public static Image IPictureToImage(IPictureDisp pic)
    {
        try
        {
            return AxHost.GetPictureFromIPicture(pic);
        }
        catch (Exception exception)
        {
            BCMTrace.Assert(false, exception.Message, exception.StackTrace);
            return null;
        }
    }

    public static void SetButtonPicture(CommandBarButton cmdButton, string resPic, string resPicMask)
    {
        try
        {
            cmdButton.Picture = GetIPicture(resPic);
            cmdButton.Mask = GetIPicture(resPicMask);
        }
        catch (COMException exception)
        {
            BCMTrace.Assert(false, exception.Message, exception.StackTrace);
            SQM.ShipAssertWithException(0x34626239, exception);
        }
    }
}
*/