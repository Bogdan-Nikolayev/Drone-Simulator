using Android.App;
using Android.Content;
using Android.Net.Wifi.P2p;
using Android.Util;

namespace Drone_Simulator.Android
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class WifiDirectBroadcastReceiver : BroadcastReceiver
    {
        private readonly WifiP2pManager _manager;
        private readonly WifiP2pManager.Channel _channel;
        private readonly IWifiDirectActivity _activity;

        public WifiDirectBroadcastReceiver()
        {
            
        }
        
        public WifiDirectBroadcastReceiver(WifiP2pManager manager, WifiP2pManager.Channel channel, IWifiDirectActivity activity)
        {
            _manager = manager;
            _channel = channel;
            _activity = activity;
        }
        
        public override void OnReceive(Context? context, Intent? intent)
        {
            string action = intent.Action;
            switch (action)
            {
                case WifiP2pManager.WifiP2pStateChangedAction:
                    // Determine if Wifi P2P mode is enabled or not, alert the Activity.
                    int state = intent.GetIntExtra(WifiP2pManager.ExtraWifiState, -1);
                    UpdateWifiP2pState(state == (int)WifiP2pState.Enabled);
                    break;
                case WifiP2pManager.WifiP2pPeersChangedAction:
                    // The peer list has changed! We should probably do something about that.
                    break;
                case WifiP2pManager.WifiP2pConnectionChangedAction:
                    // Connection state changed! We should probably do something about that.
                    break;
                case WifiP2pManager.WifiP2pThisDeviceChangedAction:
                    // _activity.GetFragmentManager().FindFragmentById(global::Android.Resource.Id)
                
                    // DeviceListFragment fragment = (DeviceListFragment) activity.getFragmentManager()
                    //     .findFragmentById(R.id.frag_list);
                    // fragment.updateThisDevice((WifiP2pDevice) intent.getParcelableExtra(
                    //     WifiP2pManager.EXTRA_WIFI_P2P_DEVICE));
                    break;
            }
            
            Log.Debug("DroneSimulator", "WifiDirectBroadcastReceiver OnReceive " + intent);
        }

        private void UpdateWifiP2pState(bool isEnabled)
        {
            _activity.IsWifiDirectEnabled = isEnabled;
        }
    }
}