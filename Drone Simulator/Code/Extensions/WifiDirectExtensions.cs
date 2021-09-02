using System;
using Android.Net.Wifi.P2p;
using Drone_Simulator.WifiDirect.Listeners;

namespace Drone_Simulator.Extensions
{
    public static class WifiDirectExtensions
    {
        public static void RemoveGroupIfExist(this WifiP2pManager manager,
            WifiP2pManager.Channel channel, Action onSuccess)
        {
            manager.RequestGroupInfo(channel, new WifiDirectGroupInfoListener(group =>
            {
                if (group != null)
                    manager.RemoveGroup(channel, new WifiDirectActionListener(onSuccess, null));
                else
                    onSuccess?.Invoke();
            }));
        }
    }
}