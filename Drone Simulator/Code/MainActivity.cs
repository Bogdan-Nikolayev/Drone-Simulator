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
using Drone_Simulator.Signaling;
using Drone_Simulator.Sockets;
using Drone_Simulator.WebRTC;
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

                GetIceCandidatesThenOpenWebView(info.IsGroupOwner, socket);
            }));
            thread.Start();
        }

        // We can't do it simultaneously, because we have only one socket,
        // so event from one action will affect different subscriptions.
        private void GetIceCandidatesThenOpenWebView(bool isGroupOwner, ISocket socket)
        {
            WebRtcSignalingServer signalingServer = new WebRtcSignalingServer(socket);
            WebRtcIceCandidatesCollector webRtcIceCandidatesCollector = new WebRtcIceCandidatesCollector(
                this, isGroupOwner, new WebRtcSignalingServer(socket));

            webRtcIceCandidatesCollector.Initialized += () =>
                RunOnUiThread(() => OpenWebView(isGroupOwner ? "video-recorder.html" : "ar.html", signalingServer));
            webRtcIceCandidatesCollector.IceCandidateGathered +=
                candidate => signalingServer.SendIceCandidate(candidate.Sdp);
        }

        private void OpenWebView(string htmlPage, WebRtcSignalingServer signalingServer)
        {
            WebView webView = FindViewById<WebView>(Resource.Id.web_view);
            WebRtcJavaScriptInterface jsInterface = new WebRtcJavaScriptInterface(webView, signalingServer);

            webView.Settings.JavaScriptEnabled = true;
            // Add C# adapter to JavaScript.
            webView.AddJavascriptInterface(jsInterface, "android");
            // Provide the required permissions.
            webView.SetWebChromeClient(new GrantedWebChromeClient());

            webView.LoadUrl("file:///android_asset/ar/" + htmlPage);
            webView.Visibility = ViewStates.Visible;
        }
    }
}