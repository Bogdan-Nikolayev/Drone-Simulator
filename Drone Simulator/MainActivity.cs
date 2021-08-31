using Android.App;
using Android.Content;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

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
            // Set our view from the "main" layout resource.
            SetContentView(Resource.Layout.activity_main);

            // Indicates a change in the Wi-Fi P2P status.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pStateChangedAction);
            // Indicates a change in the list of available peers.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pPeersChangedAction);
            // Indicates the state of Wi-Fi P2P connectivity has changed.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pConnectionChangedAction);
            // Indicates this device's details have changed.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pThisDeviceChangedAction);

            _manager = (WifiP2pManager)GetSystemService(WifiP2pService);
            _channel = _manager.Initialize(this, Looper.MainLooper, null);

            SubscribeToViewEvents();

            Log.Debug("DroneSimulator", "MainActivity OnCreate");
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Register the BroadcastReceiver with the intent values to be matched.
            _receiver = new WifiDirectBroadcastReceiver(_manager, _channel, this);
            RegisterReceiver(_receiver, _intentFilter);

            Log.Debug("DroneSimulator", "MainActivity OnResume");
        }

        protected override void OnPause()
        {
            base.OnPause();

            UnregisterReceiver(_receiver);

            Log.Debug("DroneSimulator", "MainActivity OnPause");
        }
        
        public void DiscoverPeers()
        {
            _manager.DiscoverPeers(_channel, new WifiDirectActionListener(null, null));

            Log.Debug("DroneSimulator", "MainActivity DiscoverPeers");
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
            webView.LoadUrl("https://www.google.com.ua/?hl=ru");
            webView.Visibility = ViewStates.Visible;
        }
    }
}