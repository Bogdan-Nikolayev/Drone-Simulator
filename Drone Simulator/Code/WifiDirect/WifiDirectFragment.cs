using System.Collections.Generic;
using Android.Content;
using Android.Net.Wifi.P2p;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Drone_Simulator.WifiDirect.Listeners;

namespace Drone_Simulator.WifiDirect
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WifiDirectFragment : ListFragment, IWifiDirectHandler
    {
        private readonly IntentFilter _intentFilter = new IntentFilter();
        private readonly List<WifiP2pDevice> _devices = new List<WifiP2pDevice>();
        private WifiP2pManager _manager;
        private WifiP2pManager.Channel _channel;
        private WifiDirectBroadcastReceiver _receiver;

        public WifiDirectFragment()
        {
            PeerListListener = new WifiDirectPeerListListener(FillList);
        }

        public bool IsWifiDirectEnabled { get; set; }
        public WifiDirectPeerListListener PeerListListener { get; }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            _manager = (WifiP2pManager)Activity.GetSystemService(Context.WifiP2pService);
            _channel = _manager.Initialize(Activity, Looper.MainLooper, null);
            _receiver = new WifiDirectBroadcastReceiver(_manager, _channel, this);
            ListAdapter = new WifiDirectDeviceListAdapter(Activity, _devices);

            SetupIntentFilter();
            SubscribeToViewEvents();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            return inflater.Inflate(Resource.Layout.fragment_wifi_direct, null);
        }

        public override void OnResume()
        {
            base.OnResume();

            Log.Debug();

            // Register the BroadcastReceiver with the intent values to be matched.
            Activity.RegisterReceiver(_receiver, _intentFilter);
        }

        public override void OnPause()
        {
            base.OnPause();

            Log.Debug();

            Activity.UnregisterReceiver(_receiver);
        }

        private void SetupIntentFilter()
        {
            // Indicates a change in the Wi-Fi P2P status.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pStateChangedAction);
            // Indicates a change in the list of available peers.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pPeersChangedAction);
            // Indicates the state of Wi-Fi P2P connectivity has changed.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pConnectionChangedAction);
            // Indicates this device's details have changed.
            _intentFilter.AddAction(WifiP2pManager.WifiP2pThisDeviceChangedAction);
        }

        private void SubscribeToViewEvents()
        {
            Button discoverPeersButton = Activity.FindViewById<Button>(Resource.Id.button_discover_peers);
            discoverPeersButton.Click += (sender, args) => DiscoverPeers();
        }

        private void DiscoverPeers()
        {
            Log.Debug();

            _manager.DiscoverPeers(_channel, new WifiDirectActionListener(null, null));
        }

        private void FillList(WifiP2pDeviceList peers)
        {
            foreach (WifiP2pDevice device in peers.DeviceList)
                Log.Debug(device.DeviceName);

            _devices.Clear();
            _devices.AddRange(peers.DeviceList);

            ((WifiDirectDeviceListAdapter)ListAdapter).NotifyDataSetChanged();
        }
    }
}