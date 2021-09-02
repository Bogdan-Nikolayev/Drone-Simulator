using Android.Content;
using Android.Net;
using Android.Net.Wifi.P2p;

namespace Drone_Simulator.WifiDirect
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class WifiDirectBroadcastReceiver : BroadcastReceiver
    {
        private readonly WifiP2pManager _manager;
        private readonly WifiP2pManager.Channel _channel;
        private readonly IWifiDirectHandler _handler;

        // Should be an open constructor without parameters.
        // ReSharper disable once UnusedMember.Global
        public WifiDirectBroadcastReceiver()
        {
        }

        public WifiDirectBroadcastReceiver(WifiP2pManager manager, WifiP2pManager.Channel channel,
            IWifiDirectHandler handler)
        {
            _manager = manager;
            _channel = channel;
            _handler = handler;
        }

        public override void OnReceive(Context? context, Intent? intent)
        {
            string action = intent.Action;
            switch (action)
            {
                case WifiP2pManager.WifiP2pStateChangedAction:
                    // Check to see if Wi-Fi is enabled and notify appropriate activity.
                    int state = intent.GetIntExtra(WifiP2pManager.ExtraWifiState, -1);
                    _handler.IsWifiDirectEnabled = state == (int)WifiP2pState.Enabled;
                    break;
                case WifiP2pManager.WifiP2pPeersChangedAction:
                    // Call WifiP2pManager.requestPeers() to get a list of current peers.
                    _manager.RequestPeers(_channel, _handler.PeerListListener);
                    break;
                case WifiP2pManager.WifiP2pConnectionChangedAction:
                    // Respond to new connection or disconnections.
                    NetworkInfo networkInfo = (NetworkInfo)intent.GetParcelableExtra(WifiP2pManager.ExtraNetworkInfo);
                    if (networkInfo.IsConnected)
                        // We are connected with the other device, request connection
                        // info to find group owner IP.
                        _manager.RequestConnectionInfo(_channel, _handler.ConnectionInfoListener);
                    break;
                case WifiP2pManager.WifiP2pThisDeviceChangedAction:
                    // Respond to this device's wifi state changing.
                    break;
            }
        }
    }
}