﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeafileOutlookAddIn.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SeafileOutlookAddIn.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 关于 的本地化字符串。
        /// </summary>
        internal static string AboutBtnText {
            get {
                return ResourceManager.GetString("AboutBtnText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap aboutpng {
            get {
                object obj = ResourceManager.GetObject("aboutpng", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找类似 访问URL 的本地化字符串。
        /// </summary>
        internal static string AccessUrl {
            get {
                return ResourceManager.GetString("AccessUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 取消 的本地化字符串。
        /// </summary>
        internal static string Cancel {
            get {
                return ResourceManager.GetString("Cancel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 设置文件解析错误 的本地化字符串。
        /// </summary>
        internal static string ConfigParseError {
            get {
                return ResourceManager.GetString("ConfigParseError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码为空 的本地化字符串。
        /// </summary>
        internal static string EmptyPassword {
            get {
                return ResourceManager.GetString("EmptyPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 上传文件不能为空 的本地化字符串。
        /// </summary>
        internal static string EmptyUploadFile {
            get {
                return ResourceManager.GetString("EmptyUploadFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 找不到&quot;{0}&quot;。 的本地化字符串。
        /// </summary>
        internal static string ExceptionFileNotFound {
            get {
                return ResourceManager.GetString("ExceptionFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 不支持将多个包附加至一封Outlook电子邮件。 的本地化字符串。
        /// </summary>
        internal static string ExceptionNoMoreThanOnePackageAsAttachment {
            get {
                return ResourceManager.GetString("ExceptionNoMoreThanOnePackageAsAttachment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名或密码错误 的本地化字符串。
        /// </summary>
        internal static string InvalidCredentials {
            get {
                return ResourceManager.GetString("InvalidCredentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The attached package has been sent with Seafile. To edit .velodoc files or send large files from Outlook, please download Memba Velodoc Outlook Add-In from {0}. 的本地化字符串。
        /// </summary>
        internal static string MessageBodyAdvertisement {
            get {
                return ResourceManager.GetString("MessageBodyAdvertisement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 -----Original Message----- 的本地化字符串。
        /// </summary>
        internal static string MessageBodyForwardReplyTextRegex {
            get {
                return ResourceManager.GetString("MessageBodyForwardReplyTextRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The attached package contains links to download the following files: 的本地化字符串。
        /// </summary>
        internal static string MessageBodyLinksHeader {
            get {
                return ResourceManager.GetString("MessageBodyLinksHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 错误 的本地化字符串。
        /// </summary>
        internal static string MessageBoxErrorTitle {
            get {
                return ResourceManager.GetString("MessageBoxErrorTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 提示 的本地化字符串。
        /// </summary>
        internal static string MessageBoxInfoTitle {
            get {
                return ResourceManager.GetString("MessageBoxInfoTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 警告 的本地化字符串。
        /// </summary>
        internal static string MessageBoxWarningTitle {
            get {
                return ResourceManager.GetString("MessageBoxWarningTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 分享链接： 的本地化字符串。
        /// </summary>
        internal static string OutlookShare {
            get {
                return ResourceManager.GetString("OutlookShare", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码输入不一致 的本地化字符串。
        /// </summary>
        internal static string PasswordNotMatch {
            get {
                return ResourceManager.GetString("PasswordNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 密码（至少8个字符） 的本地化字符串。
        /// </summary>
        internal static string PasswordTooShort {
            get {
                return ResourceManager.GetString("PasswordTooShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 文件不存在或文件读取错误！ 的本地化字符串。
        /// </summary>
        internal static string ReadFileError {
            get {
                return ResourceManager.GetString("ReadFileError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap seafile_logo {
            get {
                object obj = ResourceManager.GetObject("seafile-logo", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找类似 请先设置账号 的本地化字符串。
        /// </summary>
        internal static string SetAccount {
            get {
                return ResourceManager.GetString("SetAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 设置 的本地化字符串。
        /// </summary>
        internal static string SettingBtnText {
            get {
                return ResourceManager.GetString("SettingBtnText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap settingpng {
            get {
                object obj = ResourceManager.GetObject("settingpng", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找类似 添加 的本地化字符串。
        /// </summary>
        internal static string ShareBtnText {
            get {
                return ResourceManager.GetString("ShareBtnText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 分享 的本地化字符串。
        /// </summary>
        internal static string ShareFile {
            get {
                return ResourceManager.GetString("ShareFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 分享链接数据解析错误 的本地化字符串。
        /// </summary>
        internal static string SharelinkParseError {
            get {
                return ResourceManager.GetString("SharelinkParseError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似  密码 的本地化字符串。
        /// </summary>
        internal static string SharePassword {
            get {
                return ResourceManager.GetString("SharePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap sharepng {
            get {
                object obj = ResourceManager.GetObject("sharepng", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   查找类似 上 传 的本地化字符串。
        /// </summary>
        internal static string UploadBtn {
            get {
                return ResourceManager.GetString("UploadBtn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 上传 的本地化字符串。
        /// </summary>
        internal static string UploadBtnText {
            get {
                return ResourceManager.GetString("UploadBtnText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找 System.Drawing.Bitmap 类型的本地化资源。
        /// </summary>
        internal static System.Drawing.Bitmap uploadpng {
            get {
                object obj = ResourceManager.GetObject("uploadpng", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}