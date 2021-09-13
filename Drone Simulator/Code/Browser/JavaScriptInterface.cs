using Android.Webkit;
using Java.Lang;

namespace Drone_Simulator.Browser
{
    public abstract class JavaScriptInterface : Object
    {
        private readonly WebView _webView;

        protected JavaScriptInterface(WebView webView)
        {
            _webView = webView;
        }

        protected void InvokeJavaScriptFunction(string functionName, params string[] parameters)
        {
            _webView.LoadUrl($"javascript:{functionName}('" + string.Join(", ", parameters) + "');");
        }

        protected void InvokeJavaScriptFunction(string functionName)
        {
            _webView.LoadUrl($"javascript:{functionName}();");
        }
    }
}