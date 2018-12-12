
using OpenQA.Selenium;
using System;

namespace Pronet.Fiddler
{
    public class FiddlerProxy
    {
        //private static int StartFiddlerProxy(int desiredPort)
        //{
        //    // We explicitly do *NOT* want to register this running Fiddler
        //    // instance as the system proxy. This lets us keep isolation.
        //    FiddlerCoreStartupFlags flags = FiddlerCoreStartupFlags.Default &
        //                                    ~FiddlerCoreStartupFlags.RegisterAsSystemProxy;
        //    FiddlerApplication.Startup(desiredPort, flags);
        //    int proxyPort = FiddlerApplication.oProxy.ListenPort;
        //    return proxyPort;
        //}
        //public void StopFiddlerProxy()
        //{
        //    FiddlerApplication.Shutdown();
        //}
        //public OpenQA.Selenium.Proxy ProxyPortToBrowserProfile()
        //{
        //    int proxyPort = StartFiddlerProxy(0);
        //    // We are only proxying HTTP traffic, but could just as easily
        //    // proxy HTTPS or FTP traffic.
        //    OpenQA.Selenium.Proxy proxy = new OpenQA.Selenium.Proxy();
        //    proxy.HttpProxy = string.Format("127.0.0.1:{0}", proxyPort);
        //    return proxy;
        //}
        //public void GetResponseCodes(Action action)
        //{
        //    action();
        //    string requestBody = "";
        //    SessionStateHandler responseHandler = delegate (Session targetSession)
        //    {
        //        requestBody = targetSession.GetRequestBodyAsString();
        //    };

        //    FiddlerApplication.AfterSessionComplete += responseHandler;

        //    FiddlerApplication.AfterSessionComplete -= responseHandler;
        //    //return responseCode;
        //}
    }
}
