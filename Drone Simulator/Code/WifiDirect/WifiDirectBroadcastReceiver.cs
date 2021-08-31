using Android.Content;
using Android.Net.Wifi.P2p;
using Android.Util;

namespace Drone_Simulator.WifiDirect
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class WifiDirectBroadcastReceiver : BroadcastReceiver
    {
        private readonly WifiP2pManager _manager;
        private readonly WifiP2pManager.Channel _channel;
        private readonly IWifiDirectActivity _activity;

        // Should be an open constructor without parameters.
        // ReSharper disable once UnusedMember.Global
        public WifiDirectBroadcastReceiver()
        {
        }

        public WifiDirectBroadcastReceiver(WifiP2pManager manager, WifiP2pManager.Channel channel,
            IWifiDirectActivity activity)
        {
            _manager = manager;
            _channel = channel;
            _activity = activity;
        }

        public override void OnReceive(Context? context, Intent? intent)
        {
            Log.Debug(Tag.DroneSimulator, string.Join(" ",
                nameof(WifiDirectBroadcastReceiver), nameof(OnReceive), intent));

            string action = intent.Action;
            switch (action)
            {
                case WifiP2pManager.WifiP2pStateChangedAction:
                    // Check to see if Wi-Fi is enabled and notify appropriate activity.
                    int state = intent.GetIntExtra(WifiP2pManager.ExtraWifiState, -1);
                    _activity.IsWifiDirectEnabled = state == (int)WifiP2pState.Enabled;
                    break;
                case WifiP2pManager.WifiP2pPeersChangedAction:
                    // Call WifiP2pManager.requestPeers() to get a list of current peers.
                    _manager.RequestPeers(_channel, new WifiDirectPeerListListener(peers =>
                    {
                        foreach (WifiP2pDevice device in peers.DeviceList)
                            Log.Debug(Tag.DroneSimulator, string.Join(" ",
                                nameof(WifiDirectPeerListListener),
                                nameof(WifiDirectPeerListListener.OnPeersAvailable),
                                device.DeviceName));
                    }));
                    break;
                case WifiP2pManager.WifiP2pConnectionChangedAction:
                    // Respond to new connection or disconnections.
                    break;
                case WifiP2pManager.WifiP2pThisDeviceChangedAction:
                    // Respond to this device's wifi state changing.
                    break;
            }
        }
    }
}