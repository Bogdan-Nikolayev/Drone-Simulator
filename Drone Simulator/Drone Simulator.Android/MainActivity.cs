using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Wifi.P2p;
using Android.OS;

namespace Drone_Simulator.Android
{
    [Activity(Label = "Drone_Simulator", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly IntentFilter intentFilter = new IntentFilter();
        
        WifiP2pManager.Channel channel;
        WifiP2pManager manager;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            
            // Indicates a change in the Wi-Fi P2P status.
            intentFilter.AddAction(WifiP2pManager.WifiP2pStateChangedAction);
            // Indicates a change in the list of available peers.
            intentFilter.AddAction(WifiP2pManager.WifiP2pPeersChangedAction);
            // Indicates the state of Wi-Fi P2P connectivity has changed.
            intentFilter.AddAction(WifiP2pManager.WifiP2pConnectionChangedAction);
            // Indicates this device's details have changed.
            intentFilter.AddAction(WifiP2pManager.WifiP2pThisDeviceChangedAction);

            manager = (WifiP2pManager)GetSystemService(WifiP2pService);
            channel = manager.Initialize(this, Looper.MainLooper, null);
        }
    }
}