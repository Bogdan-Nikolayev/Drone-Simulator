using System;
using Android.Net.Wifi.P2p;

namespace Drone_Simulator.WifiDirect.Listeners
{
    public class WifiDirectGroupInfoListener : Java.Lang.Object, WifiP2pManager.IGroupInfoListener
    {
        private readonly Action<WifiP2pGroup> _onGroupInfoAvailable;

        public WifiDirectGroupInfoListener(Action<WifiP2pGroup> onGroupInfoAvailable)
        {
            _onGroupInfoAvailable = onGroupInfoAvailable;
        }

        public void OnGroupInfoAvailable(WifiP2pGroup? group)
        {
            Log.Debug(group);
            
            _onGroupInfoAvailable?.Invoke(group);
        }
    }
}