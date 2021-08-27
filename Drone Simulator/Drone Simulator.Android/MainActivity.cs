using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Util;

namespace Drone_Simulator.Android
{
    /// <summary>
    /// https://developer.android.com/training/connect-devices-wirelessly/wifi-direct.html
    /// </summary>
    [Activity(Label = "Drone_Simulator", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly IntentFilter _intentFilter = new IntentFilter();

        private WifiP2pManager _manager;
        private WifiP2pManager.Channel _channel;
        private WifiDirectBroadcastReceiver _receiver;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            
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

            Log.Debug("DroneSimulator", "Main activity OnCreate");
        }
        
        // Register the BroadcastReceiver with the intent values to be matched.
        protected override void OnResume() {
            base.OnResume();
            
            _receiver = new WifiDirectBroadcastReceiver(_manager, _channel, this);
            RegisterReceiver(_receiver, _intentFilter);
        }
        
        protected override void OnPause() {
            base.OnPause();
            
            UnregisterReceiver(_receiver);
        }

        public void DiscoverPeers()
        {
            _manager.DiscoverPeers(_channel, new WifiDirectActionListener());
        }
    }
}