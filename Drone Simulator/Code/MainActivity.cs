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
using Drone_Simulator.Sockets;
using Drone_Simulator.WifiDirect;
using Java.Lang;

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
            // TODO: Remove this button.
            Button openWebViewButton = FindViewById<Button>(Resource.Id.button_open_web_view);
            openWebViewButton.Click += (sender, args) => OpenWebView("ar.html", null);

            _wifiDirect = (WifiDirectFragment)SupportFragmentManager.FindFragmentById(Resource.Id.fragment_wifi_direct);
            _wifiDirect.Connected += ConnectSocketsAndOpenWebView;
        }

        private void ConnectSocketsAndOpenWebView(WifiP2pInfo info)
        {
            ISocket socket;
            const int port = 8888;

            // Networking code should execute not from main thread due to:
            // android.os.NetworkOnMainThreadException
            Thread thread = new Thread(new Runnable(() =>
            {
                if (info.IsGroupOwner)
                    socket = new ServerSocket(port);
                else
                    socket = new ClientSocket(info.GroupOwnerAddress, port);

                RunOnUiThread(() => OpenWebView(info.IsGroupOwner ? "ar.html" : "video-recorder.html", socket));
            }));
            thread.Start();
        }

        private void OpenWebView(string htmlPage, ISocket socket)
        {
            WebView webView = FindViewById<WebView>(Resource.Id.web_view);

            webView.Settings.JavaScriptEnabled = true;
            // Add C# adapter to JavaScript.
            webView.AddJavascriptInterface(new WebRtcJavaScriptInterface(webView, socket), "android");
            // Provide the required permissions.
            webView.SetWebChromeClient(new GrantedWebChromeClient());

            webView.LoadUrl("file:///android_asset/ar/" + htmlPage);
            webView.Visibility = ViewStates.Visible;
        }
    }
}