using Drone_Simulator.WifiDirect.Listeners;

namespace Drone_Simulator.WifiDirect
{
    public interface IWifiDirectHandler
    {
        public bool IsWifiDirectEnabled { get; set; }
        public WifiDirectPeerListListener PeerListListener { get; }
    }
}