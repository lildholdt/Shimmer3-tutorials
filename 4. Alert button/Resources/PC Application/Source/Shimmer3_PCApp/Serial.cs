using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ShimmerConsole
{
    public class DataEventArgs : EventArgs
    {
        private byte[] data;
        public DataEventArgs(byte[] Data)
        {
            data = Data;
        }

        public byte[] Data
        {
            get { return data; }
        }
    }

    public delegate void DataRecievedEvent(object sender, DataEventArgs e);

    class Serial
    {
        private byte[] buffer = new byte[1];
        private SerialPort serial = new SerialPort();
        private int recieve_ctr = 0;

        public event DataRecievedEvent dataRecieved;

        public Serial()
        {
            serial.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
        }

        public bool OpenPort(string COMport)
        {
            try
            {
                if (serial.IsOpen == true) serial.Close();

                serial.BaudRate = 9600;
                serial.DataBits = 8;
                serial.Parity = Parity.None;
                serial.PortName = COMport;

                serial.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            do {
                
                buffer[recieve_ctr] = ((byte)serial.ReadByte());
                recieve_ctr++;

            } while (serial.BytesToRead != 0);

            if (recieve_ctr == 1)
            {
                if (dataRecieved != null)
                {
                    dataRecieved(this, new DataEventArgs(buffer));
                }

                Array.Clear(buffer,0,buffer.Length);
                recieve_ctr = 0;
            }
        }

        public void DataSend(string text)
        {
            if (serial.IsOpen)
            {
                serial.Write(text);
            }
        }

        public void Close()
        {
            if (serial.IsOpen)
                serial.Close();
        }
    }
}