using System;
using System.Collections.Generic;
using System.Text;

namespace MMM_chart.Model
{
    class Configure
    {
        static readonly string ipAddressDefault = "192.168.0.11";
        public string IpAddress;
        static readonly int sampleTimeDefault = 1000;
        public int SampleTime;
        public readonly int MaxSampleNumber = 100;
        public double XAxisMax
        {
            get
            {
                return MaxSampleNumber * SampleTime / 1000.0;
            }
            private set { }
        }

        public Configure()
        {
            IpAddress = ipAddressDefault;
            SampleTime = sampleTimeDefault;
        }

        public Configure(string ip, int st)
        {
            IpAddress = ip;
            SampleTime = st;
        }
    }
}
