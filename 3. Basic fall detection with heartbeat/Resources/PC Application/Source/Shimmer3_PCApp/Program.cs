/*-----------------------------------------------------------------------------

	File: 		Program.cs
	Version:   	1.0
	Created:    07/02/2015
	Author:		Steffan Lildholdt
	Email:     	steffan@lildholdt.dk
	Website:   	steffanlildholdt.dk

-----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ShimmerConsole
{
    class Program
    {
        const int HEARTBEAT_INTERVAL = 10;  //Seconds
        
        static Serial serial;
        static bool connected = false;
        static System.Timers.Timer heartbeat_timer;

        static void Main(string[] args)
        {
            // Setup serial communication
            serial = new Serial();
            Console.WriteLine("\n------------------------------------------");
            Console.WriteLine("Shimmer 3 Fall detection with heart beat");
            Console.WriteLine("------------------------------------------\n");

            while (true)
            {
                while (!connected)
                {
                    Connect();
                }
            }
        }

        static void Connect()
        {
            string COMport;
            Console.WriteLine("Type COM port:");
            COMport = Console.ReadLine();
            Console.WriteLine("Trying to connect to " + COMport);

            if (serial.OpenPort(COMport))
            {
                Console.WriteLine("\n--------------------------------------");
                Console.WriteLine("Connected to " + COMport);
                Console.WriteLine("--------------------------------------\n");
                serial.dataRecieved += OnDataReceived;
                connected = true;

                //Set up heart beat timer
                heartbeat_timer = new System.Timers.Timer((5+HEARTBEAT_INTERVAL)*1000);
                heartbeat_timer.Elapsed += heartbeat_timer_Elapsed;
                heartbeat_timer.Enabled = true;

            }
            else
            {
                Console.WriteLine("Unable to connect to specified COM port\n");
            }
        }

        static void heartbeat_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("\r\nWarning: Lost connection to the Shimmer \r\n");
            //Detach eventhandlers
            serial.dataRecieved -= OnDataReceived;
            heartbeat_timer.Elapsed -= heartbeat_timer_Elapsed;

            //Stop the heartbeat timer and close the serial connection
            heartbeat_timer.Stop();
            serial.Close();
            connected = false;
        }

        static void OnDataReceived(object sender, DataEventArgs e)
        {
            byte[] data = e.Data;
            if (data[0] == 'F')
            {
                Console.WriteLine("ALERT: A fall has occured");
            }
            if (data[0] == 'H')
            {
                heartbeat_timer.Stop();
                heartbeat_timer.Start();
            }
        }
    }
}
