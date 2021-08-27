using Android.Net.Wifi.P2p;

namespace Drone_Simulator.Android
{
    public class WifiDirectActionListener : Java.Lang.Object, WifiP2pManager.IActionListener
    {
        public void OnFailure(WifiP2pFailureReason reason)
        {
        }

        public void OnSuccess()
        {
        }
    }
}