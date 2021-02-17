using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.OS;
using dgfbhsd.Droid.BlueTooth;
using Java.IO;
using Java.Util;
using Application = Xamarin.Forms.Application;

[assembly: Xamarin.Forms.Dependency(typeof(Bth))]
namespace dgfbhsd.Droid.BlueTooth
{
    class Bth : IBth
    {
        private BluetoothDevice _device { get; set; }
        BluetoothAdapter _adapter { get; set; }
        BluetoothSocket _bthSocket { get; set; }


        public Bth()
        {
            _adapter = BluetoothAdapter.DefaultAdapter;
        }

        public void Connect(string name)
        {
            var tmp = Task.Run(async () => ConnectDevice(name));
        }
        public void Disconnect()
        {
            if (_bthSocket != null)
            {

                _device.Dispose();
                System.Diagnostics.Debug.WriteLine(_bthSocket.IsConnected);
                _bthSocket.OutputStream.WriteByte(200);
                _bthSocket.OutputStream.Close();
                _bthSocket.Close();
                System.Diagnostics.Debug.WriteLine("Déconnecté");
            }

        }
        private async Task ConnectDevice(string name)
        {
            _device = null;
            _bthSocket = null;
            try
            {
                Thread.Sleep(250);

                _adapter = BluetoothAdapter.DefaultAdapter;

                if (_adapter == null)
                    System.Diagnostics.Debug.WriteLine("Pas d'adaptateur détecté");
                else
                    System.Diagnostics.Debug.WriteLine("Adaptateur trouvé");

                if (!_adapter.IsEnabled)
                    System.Diagnostics.Debug.WriteLine("Adaptateur désactivé");
                else
                    System.Diagnostics.Debug.WriteLine("Adaptateur Activé");

                System.Diagnostics.Debug.WriteLine("Tentative de connexion à " + name);

                foreach (var bd in _adapter.BondedDevices)
                {
                    System.Diagnostics.Debug.WriteLine("Appareil trouvé : " + bd.Name.ToUpper());
                    if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Trouvé " + bd.Name + ". Tentative de connexion");
                        _device = bd;
                        break;
                    }
                }

                if (_device == null)
                    System.Diagnostics.Debug.WriteLine("Appareil non trouvé");
                else
                {
                    UUID uuid = UUID.FromString(_device.GetUuids()[0].Uuid.ToString());
                    _bthSocket = _device.CreateRfcommSocketToServiceRecord(uuid);
                    if (_bthSocket != null)
                    {
                        await _bthSocket.ConnectAsync();

                        if (_bthSocket.IsConnected)
                        {
                            System.Diagnostics.Debug.WriteLine(_bthSocket.IsConnected);
                            System.Diagnostics.Debug.WriteLine("Connecté");

                        }
                    }
                }
            }
            catch
            {
            }

        }
        public List<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            List<string> devices = new List<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }

        public async Task WaitForScore()
        {
            bool gotResponse = false;
            byte[] receive = new byte[1024];
            string msg = "";
            if (_bthSocket != null)
            {
                try
                {
                    while (!gotResponse)
                    {
                        Array.Clear(receive, 0, receive.Length);
                        _bthSocket.InputStream.Read(receive, 0, receive.Length);
                        msg += Encoding.ASCII.GetString(receive);
                        if (msg.ToUpper().Contains("SCORE +1"))
                        {
                            gotResponse = true;
                        }
                    }
                }
                catch
                {

                }
            }
            
        }

    }
}
