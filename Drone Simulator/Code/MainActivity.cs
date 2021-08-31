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
            
            Log.Debug(Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(OnCreate)));

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
            _receiver = new WifiDirectBroadcastReceiver(_manager, _channel, this);

            SubscribeToViewEvents();
        }

        protected override void OnResume()
        {
            base.OnResume();
            
            Log.Debug(Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(OnResume)));

            // Register the BroadcastReceiver with the intent values to be matched.
            RegisterReceiver(_receiver, _intentFilter);
        }

        protected override void OnPause()
        {
            base.OnPause();
            
            Log.Debug(Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(OnPause)));

            UnregisterReceiver(_receiver);
        }
        
        public void DiscoverPeers()
        {
            Log.Debug(Tag.DroneSimulator, string.Join(" ", nameof(MainActivity), nameof(DiscoverPeers)));

            _manager.DiscoverPeers(_channel, new WifiDirectActionListener(null, null));
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

        private void RequestCameraPermission()
        {
            // if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation)) 
            // {
            //     // Provide an additional rationale to the user if the permission was not granted
            //     // and the user would benefit from additional context for the use of the permission.
            //     // For example if the user has previously denied the permission.
            //     // Log.Info(TAG, "Displaying camera permission rationale to provide additional context.");
            //     //
            //     // Snackbar.Make(layout, 
            //     //         "Give access to the camera!!!",
            //     //         Snackbar.LengthIndefinite)
            //     //     .SetAction("Ok", 
            //     //         new Action<View>(delegate(View obj) {
            //     //                 
            //     //             }    
            //     //         )
            //     //     ).Show();
            //     var requiredPermissions = new String[] { Manifest.Permission.AccessFineLocation };
            //
            //     ActivityCompat.RequestPermissions(this, requiredPermissions, REQUEST_LOCATION);
            // }
            // else 
            // {
            //     ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, REQUEST_LOCATION);
            // }
        }
    }
}