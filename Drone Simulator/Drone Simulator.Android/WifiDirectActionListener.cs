using Android.Net.Wifi.P2p;
using Android.Util;

namespace Drone_Simulator.Android
{
    public class WifiDirectActionListener : Java.Lang.Object, WifiP2pManager.IActionListener
    {
        public void OnFailure(WifiP2pFailureReason reason)
        {
            Log.Debug("DroneSimulator", "WifiDirectActionListener OnFailure " + reason);
        }

        public void OnSuccess()
        {
            Log.Debug("DroneSimulator", "WifiDirectActionListener OnSuccess");
        }
    }
}