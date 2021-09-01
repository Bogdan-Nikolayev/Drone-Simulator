using System.Collections.Generic;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Drone_Simulator.WifiDirect;

namespace Drone_Simulator.UI
{
    public class DeviceListFragment : ListFragment
    {
        private readonly List<WifiP2pDevice> devices = new List<WifiP2pDevice>();

        public DeviceListFragment()
        {
            Listener = new WifiDirectPeerListListener(Fill);
        }

        public WifiDirectPeerListListener Listener { get; }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            ListAdapter = new DeviceListAdapter(Activity, devices);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_device_list, null);
        }

        private void Fill(WifiP2pDeviceList peers)
        {
            throw new System.NotImplementedException();
        }
    }
}