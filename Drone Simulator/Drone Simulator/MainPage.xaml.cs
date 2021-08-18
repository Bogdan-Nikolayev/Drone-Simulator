using System;
using Xamarin.Forms;

namespace Drone_Simulator
{
    public partial class MainPage : ContentPage
    {
        private readonly SocketManager socketManager = new SocketManager();
        
        public MainPage()
        {
            InitializeComponent();
        }

        private void Connect_Clicked(object sender, EventArgs e)
        {
            string result = socketManager.SocketSendReceive("10.5.1.157", 10000);
            
            Console.WriteLine(result);
        }
    }
}
