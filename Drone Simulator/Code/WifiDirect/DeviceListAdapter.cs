using System.Collections.Generic;
using Android.App;
using Android.Net.Wifi.P2p;
using Android.Views;
using Android.Widget;

namespace Drone_Simulator.WifiDirect
{
    public class DeviceListAdapter : BaseAdapter<WifiP2pDevice>
    {
        private readonly Activity _context;
        private readonly List<WifiP2pDevice> _devices;

        public DeviceListAdapter(Activity context, List<WifiP2pDevice> devices)
        {
            _context = context;
            _devices = devices;
        }

        public override int Count => _devices.Count;

        public override WifiP2pDevice this[int position] => _devices[position];

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View? GetView(int position, View? convertView, ViewGroup? parent)
        {
            View view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.item_device_list, null);
            WifiP2pDevice device = _devices[position];

            view.FindViewById<TextView>(Resource.Id.text_device_name).Text = device.DeviceName;
            view.FindViewById<TextView>(Resource.Id.text_device_status).Text = device.Status.ToString();

            return view;
        }
    }
}