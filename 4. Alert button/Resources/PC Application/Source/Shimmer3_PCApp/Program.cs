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

namespace ShimmerConsole
{
    class Program
    {
        static Serial serial;
        static bool connected = false;

        static void Main(string[] args)
        {
            // Setup serial communication
            serial = new Serial();
            Console.WriteLine("\n--------------------------------------");
            Console.WriteLine("Shimmer 3 Alert button");
            Console.WriteLine("--------------------------------------\n");

            while (!connected)
            {
                Connect();
            }

            while (true) ;
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
            }
            else
            {
                Console.WriteLine("Unable to connect to specified COM port\n");
            }
        }

        static void OnDataReceived(object sender, DataEventArgs e)
        {
            byte[] data = e.Data;
            if (data[0] == 'A')
            {
                Console.WriteLine("ALERT: The user button has been pressed");
            }
        }
    }
}
