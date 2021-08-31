using Android;
using Android.App;
using Android.Content;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Drone_Simulator.WifiDirect;
using Drone_Simulator.Extensions;

namespace Drone_Simulator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IWifiDirectActivity
    {
        private readonly IntentFilter _intentFilter = new IntentFilter();

        private WifiP2pManager _manager;
        private WifiP2pManager.Channel _channel;
        private WifiDirectBroadcastReceiver _receiver;

        public bool IsWifiDirectEnabled { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(OnCreate)));

            RequestPermissions();
            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.activity_main);

            SetupIntentFilter();
            _manager = (WifiP2pManager)GetSystemService(WifiP2pService);
            _channel = _manager.Initialize(this, Looper.MainLooper, null);
            _receiver = new WifiDirectBroadcastReceiver(_manager, _channel, this);

            SubscribeToViewEvents();
        }

        protected override void OnResume()
        {
            base.OnResume();

            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(OnResume)));

            // Register the BroadcastReceiver with the intent values to be matched.
            RegisterReceiver(_receiver, _intentFilter);
        }

        protected override void OnPause()
        {
            base.OnPause();

            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(OnPause)));

            UnregisterReceiver(_receiver);
        }

        public void DiscoverPeers()
        {
            Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(DiscoverPeers)));

            _manager.DiscoverPeers(_channel, new WifiDirectActionListener(null, null));
        }

        private void RequestPermissions()
        {
            if (!this.AllOfPermissionsGranted(Manifest.Permission.AccessFineLocation, Manifest.Permission.Camera))
                RequestPermissions(new[] {Manifest.Permission.AccessFineLocation, Manifest.Permission.Camera},
                    Constants.MainPermissionsRequestCode);
        }

        private void SetupIntentFilter()
        {
            // Indicates a change in the Wi-Fi P2P status.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pStateChangedAction);
            // Indicates a change in the list of available peers.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pPeersChangedAction);
            // Indicates the state of Wi-Fi P2P connectivity has changed.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pConnectionChangedAction);
            // Indicates this device's details have changed.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pThisDeviceChangedAction);
        }

        private void SubscribeToViewEvents()
        {
            Button discoverPeersButton = FindViewById<Button>(Resource.Id.discoverPeersButton);
            discoverPeersButton.Click += (sender, args) => DiscoverPeers();

            Button openWebViewButton = FindViewById<Button>(Resource.Id.openWebViewButton);
            openWebViewButton.Click += (sender, args) => OpenWebView();
        }

        private void OpenWebView()
        {
            WebView webView = FindViewById<WebView>(Resource.Id.webView);
            webView.LoadUrl("file:///android_asset/ar/ar-js-location.html");
            webView.Visibility = ViewStates.Visible;
        }
    }
}