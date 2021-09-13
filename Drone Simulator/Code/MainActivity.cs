using System.Collections.Generic;
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
using Drone_Simulator.Pose;
using Drone_Simulator.Sockets;
using Drone_Simulator.WebRTC;
using Drone_Simulator.WebRTC.Signaling;
using Drone_Simulator.WifiDirect;
using Java.Lang;
using Xam.WebRtc.Android;

namespace Drone_Simulator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private IWifiDirectHandler _wifiDirect;
        private ISocket _socket;

        // TODO: Remove if not required.
        private readonly List<IceCandidate> _iceCandidates = new List<IceCandidate>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            RequestPermissions();
            SetContentView(Resource.Layout.activity_main);
            SubscribeToViewEvents();
        }

        // Uncomment if you need some permission for Xamarin.Essentials.
        // public override void OnRequestPermissionsResult(
        //     int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        // {
        //     Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //
        //     base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        // }

        private void RequestPermissions()
        {
            if (!this.AllOfPermissionsGranted(Manifest.Permission.AccessFineLocation, Manifest.Permission.Camera))
                RequestPermissions(new[] {Manifest.Permission.AccessFineLocation, Manifest.Permission.Camera},
                    Constants.RequestCode.MainPermissions);
        }

        private void SubscribeToViewEvents()
        {
            // TODO: Remove this button.
            // Button openWebViewButton = FindViewById<Button>(Resource.Id.button_open_web_view);
            // openWebViewButton.Click += (sender, args) => OpenWebView("ar.html", null);

            _wifiDirect = (WifiDirectFragment)SupportFragmentManager.FindFragmentById(Resource.Id.fragment_wifi_direct);
            _wifiDirect.Connected += ConnectSocketsAndOpenWebView;
        }

        private void ConnectSocketsAndOpenWebView(WifiP2pInfo info)
        {
            const int port = 8888;

            // Networking code should execute not from main thread due to:
            // android.os.NetworkOnMainThreadException
            Thread thread = new Thread(new Runnable(() =>
            {
                if (info.IsGroupOwner)
                    _socket = new ServerSocket(port);
                else
                    _socket = new ClientSocket(info.GroupOwnerAddress, port);

                GetIceCandidatesThenOpenWebView(info.IsGroupOwner, _socket);
            }));
            thread.Start();
        }

        // We can't do it simultaneously, because we have only one socket,
        // so event from one action will affect different subscriptions.
        private void GetIceCandidatesThenOpenWebView(bool isGroupOwner, ISocket socket)
        {
            WebRtcSignalingServer signalingServer = new WebRtcSignalingServer(socket);
            RunOnUiThread(() =>
                OpenWebView(isGroupOwner ? "video-recorder.html" : "ar.html", isGroupOwner, signalingServer));

            // TODO: Remove if not required.
            // WebRtcIceCandidatesCollector webRtcIceCandidatesCollector = new WebRtcIceCandidatesCollector(
            //     this, isGroupOwner, new WebRtcSignalingServer(socket));
            //
            // webRtcIceCandidatesCollector.Initialized += () =>
            // {
            //     Log.Debug("Initialized callback");
            //     RunOnUiThread(() => OpenWebView(isGroupOwner ? "video-recorder.html" : "ar.html", signalingServer));
            //     webRtcIceCandidatesCollector.CloseConnection();
            // };
            // webRtcIceCandidatesCollector.IceCandidateGathered +=
            //     candidate => _iceCandidates.Add(candidate);
        }

        // TODO: Refactoring.
        private void OpenWebView(string htmlPage, bool isGroupOwner, WebRtcSignalingServer signalingServer)
        {
            WebView webView = FindViewById<WebView>(Resource.Id.web_view);
            WebRtcJavaScriptInterface webRtcInterface = new WebRtcJavaScriptInterface(webView, signalingServer);
            webRtcInterface.ReceiveCandidatesClicked += () =>
            {
                foreach (IceCandidate iceCandidate in _iceCandidates)
                    signalingServer.SendIceCandidate("{\"candidate\":\"" + iceCandidate.Sdp +
                        "\",\"sdpMid\":\"0\",\"sdpMLineIndex\":0}");
            };

            PoseJavaScriptInterface poseInterface = new PoseJavaScriptInterface(webView);
            PoseController poseController = new PoseController(poseInterface, new PoseSocketDecorator(_socket));
            poseController.StartCommunication();
            if (isGroupOwner)
                poseController.ToggleOrientationSensor();

            webView.Settings.JavaScriptEnabled = true;
            // Add C# adapter to JavaScript.
            webView.AddJavascriptInterface(webRtcInterface, "webRtcAndroidInterface");
            webView.AddJavascriptInterface(poseInterface, "poseAndroidInterface");
            // Provide the required permissions.
            webView.SetWebChromeClient(new GrantedWebChromeClient());

            webView.LoadUrl("file:///android_asset/ar/" + htmlPage);
            webView.Visibility = ViewStates.Visible;
        }
    }
}