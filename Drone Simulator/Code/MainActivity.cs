using Android;
using Android.App;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Drone_Simulator.Browser;
using Drone_Simulator.Extensions;
using Drone_Simulator.WifiDirect;

namespace Drone_Simulator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private IWifiDirectHandler _wifiDirect;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestPermissions();
            SetContentView(Resource.Layout.activity_main);
            SubscribeToViewEvents();
        }

        private void RequestPermissions()
        {
            if (!this.AllOfPermissionsGranted(Manifest.Permission.AccessFineLocation, Manifest.Permission.Camera))
                RequestPermissions(new[] {Manifest.Permission.AccessFineLocation, Manifest.Permission.Camera},
                    Constants.RequestCode.MainPermissions);
        }

        private void SubscribeToViewEvents()
        {
            Button openWebViewButton = FindViewById<Button>(Resource.Id.button_open_web_view);
            openWebViewButton.Click += (sender, args) => OpenWebView("ar.html");

            _wifiDirect = (WifiDirectFragment)SupportFragmentManager.FindFragmentById(Resource.Id.fragment_wifi_direct);
            _wifiDirect.Connected += OpenWebView;
        }

        private void OpenWebView(WifiP2pInfo info)
        {
            OpenWebView(info.IsGroupOwner ? "ar.html" : "video-recorder.html");
        }

        private void OpenWebView(string htmlPage)
        {
            WebView webView = FindViewById<WebView>(Resource.Id.web_view);

            webView.Settings.JavaScriptEnabled = true;
            // Add C# adapter to JavaScript.
            webView.AddJavascriptInterface(new JavaScriptInterface(), "android");
            // Provide the required permissions.
            webView.SetWebChromeClient(new GrantedWebChromeClient());

            webView.LoadUrl("file:///android_asset/ar/" + htmlPage);
            webView.Visibility = ViewStates.Visible;
        }
    }
}