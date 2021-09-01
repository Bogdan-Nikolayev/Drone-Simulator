using System.Collections.Generic;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Drone_Simulator.WifiDirect;

namespace Drone_Simulator.UI
{
    // ReSharper disable once ClassNeverInstantiated.Global
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
            base.OnCreateView(inflater, container, savedInstanceState);

            return inflater.Inflate(Resource.Layout.fragment_device_list, null);
        }

        private void Fill(WifiP2pDeviceList peers)
        {
            foreach (WifiP2pDevice device in peers.DeviceList)
                Log.Debug(Constants.Tag.DroneSimulator, string.Join(" ",
                    nameof(DeviceListFragment), nameof(Fill), device.DeviceName));

            devices.Clear();
            devices.AddRange(peers.DeviceList);

            ((DeviceListAdapter)ListAdapter).NotifyDataSetChanged();
        }
    }
}