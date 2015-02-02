using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System.Net.Sockets;
using System.Timers;
using System.IO;


namespace Shimmer3_PCApp
{
    class Program
    {
        const string SHIMMER_ID = "5F05";                                                       // ID of the Shimmer. Located on the back of the device
        const string SHIMMER_PIN = "1234";                                                      // Bluetooth PIN code. Set up in the firmware.

        static bool connected = false;                                                          // Flag to handle the Bluetooth connection process
        static bool connecting = false;                                                         // Flag to handle the Bluetooth connection process

        static BluetoothClient localClient;                                                     // Client is used to manage connections
        static BluetoothComponent localComponent;                                               // Component is used to manage device discovery
        static NetworkStream Ns = null;                                                         // Stream to handle communication

        // Connection timer
        static System.Timers.Timer connectionTimer;
        static System.Timers.Timer disconnectTimer;

        static Thread readThread;

        static void Main(string[] args)
        {
            // Set up connect timer
            connectionTimer = new System.Timers.Timer(1000);
            connectionTimer.Elapsed += connectionTimer_Elapsed;
            connectionTimer.Enabled = true;

            // Set up disconnect timer
            disconnectTimer = new System.Timers.Timer(2000);
            disconnectTimer.Elapsed += disconnectTimer_Elapsed;
            disconnectTimer.Enabled = false;

            // Keep alive
            Console.ReadLine();
        }

        /// <summary>
        /// Handle connectionTimer ticks
        /// </summary>
        static void connectionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (connecting == false && connected == false)
            {
                connecting = true;
                InitBTConnection();
            }
        }

        /// <summary>
        /// Ensures that a communication window of 2 seconds is maintained
        /// </summary>
        static void disconnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DisposeBTConnection();
            disconnectTimer.Enabled = false;
            readThread.Abort();  
            Console.WriteLine("Closed connection to Shimmer3-" + SHIMMER_ID);
            connected = false;
        }

        /// <summary>
        /// Initializes the Bluetooth connection process
        /// </summary>
        private static void InitBTConnection()
        {
            localClient = new BluetoothClient();
            localComponent = new BluetoothComponent(localClient);

            // Setup event handlers
            localComponent.DiscoverDevicesProgress += new EventHandler<DiscoverDevicesEventArgs>(DiscoverDevices);
            localComponent.DiscoverDevicesComplete += new EventHandler<DiscoverDevicesEventArgs>(DiscoverDevicesComplete);

            // Get previously paired devices
            BluetoothDeviceInfo[] pairedDevices = localClient.DiscoverDevices(255, false, true, false, false);
            
            // Check if specified Shimmer is already paired
            List<BluetoothDeviceInfo> pairedDevicesList = pairedDevices.OfType<BluetoothDeviceInfo>().ToList();
            BluetoothDeviceInfo pairedDevice = pairedDevicesList.Find(item => item.DeviceName.Equals("Shimmer3-" + SHIMMER_ID));
            
            // If the device is not paired begin the discovery process
            if (pairedDevice == null)
            {
                localComponent.DiscoverDevicesAsync(255, true, true, true, true, null);
                Console.WriteLine("Shimmer3-" + SHIMMER_ID + " not paired -> Begin discovery");
            }

            // If the device is already paired begin the connection process
            else
            {               
                // set pin of device to connect with
                localClient.SetPin(SHIMMER_PIN);

                // async connection method
                localClient.BeginConnect(pairedDevice.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connected_Callback), pairedDevice);
                Console.WriteLine("Shimmer3-" + SHIMMER_ID + " already paired -> Begin connect");
            }
        }

        /// <summary>
        /// Disposes the Bluetooth connection
        /// </summary>
        private static void DisposeBTConnection()
        {
            localComponent.DiscoverDevicesProgress -= new EventHandler<DiscoverDevicesEventArgs>(DiscoverDevices);
            localComponent.DiscoverDevicesComplete -= new EventHandler<DiscoverDevicesEventArgs>(DiscoverDevicesComplete);
            localComponent.Dispose();

            localClient.Close();
            localClient.Dispose();
            connected = false;
            connecting = false;
        }

        /// <summary>
        /// Checks whether the Bluetooth connection is established
        /// </summary>
        /// <returns>True/False</returns>
        private static bool IsBTConnected()
        {
            try
            {
                Ns = localClient.GetStream();
                if (Ns.CanRead) return true;
                Ns.Close();
            }
            catch (Exception e) { }
            return false;
        }

        /// <summary>
        /// Lists all discovered Bluetooth devices
        /// </summary>
        private static void DiscoverDevices(object sender, DiscoverDevicesEventArgs e)
        {
            for (int i = 0; i < e.Devices.Length; i++)
            {
                if (e.Devices[i].Remembered)
                {
                    Console.WriteLine(e.Devices[i].DeviceName + " (" + e.Devices[i].DeviceAddress + "): Device is known");
                }
                else
                {
                    Console.WriteLine(e.Devices[i].DeviceName + " (" + e.Devices[i].DeviceAddress + "): Device is unknown");
                }
            }
        }
        
        /// <summary>
        /// This method is called when the discovery process is completed. 
        /// It attempts to create a Bluetooth connection the specified device 
        /// </summary>
        private static void DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            // Get a list of previously paired devices
            List<BluetoothDeviceInfo> devices = e.Devices.OfType<BluetoothDeviceInfo>().ToList();
            BluetoothDeviceInfo device = devices.Find(item => item.DeviceName.Equals("Shimmer3-" + SHIMMER_ID));

            if (device == null)
            {
                Console.WriteLine("Shimmer3-" + SHIMMER_ID + " could not be discovered");
                connecting = false;
            }
            else
            {
                // Check if device can be paired
                if (BluetoothSecurity.PairRequest(device.DeviceAddress, SHIMMER_PIN))
                {
                    // set pin of device to connect with
                    localClient.SetPin(SHIMMER_PIN);
                    // async connection method
                    localClient.BeginConnect(device.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(Connected_Callback), device);
                }
                else
                {
                    Console.WriteLine("Shimmer3-" + SHIMMER_ID + " could not be paired");
                    connecting = false;
                }
            }
        }

        /// <summary>
        /// Callback method for the BeginConnect method
        /// </summary>
        /// <param name="result"></param>
        private static void Connected_Callback(IAsyncResult result)
        {
            if (result.IsCompleted && IsBTConnected())
            {
                Console.WriteLine("Shimmer3-" + SHIMMER_ID + " is now connected");

                disconnectTimer.Enabled = true;

                //Set flags
                connected = true;
                connecting = false;

                // Initialize stream
                Ns = localClient.GetStream();
                
                // Start read thread
                readThread = new Thread(new ThreadStart(Reading));
                readThread.Start();           
            }
            else
            {
                Console.WriteLine("Shimmer3-" + SHIMMER_ID + " could not be connected");

                DisposeBTConnection();

                // Set flags
                connecting = false;
                connected = false;
            }
        }

        /// <summary>
        /// Read data from Shimmer3
        /// </summary>
        private static void Reading()
        {
            byte[] data = new byte[3];
            int numBytes = 0;
            
            while (true)
            {

                if (connected)
                {
                    var next = Ns.ReadByte();
                    if (next < 0) break; // no more data

                    data[numBytes] = (byte)next;
                    numBytes++;

                    if (numBytes == 3 && data[0] == 'P')
                    {
                        int steps = BitConverter.ToInt16(data, 1);
                        Console.WriteLine("Number of steps taken: " + steps);

                        // Transmit ACK to indicate that the fall is registered
                        byte[] myWriteBuffer = new byte[1] { 6 };
                        Ns.Write(myWriteBuffer, 0, myWriteBuffer.Length);

                        numBytes = 0;
                    }
                }
               
            }
        }
    }
}
