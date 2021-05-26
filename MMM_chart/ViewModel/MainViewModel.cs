using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Timers;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Newtonsoft.Json;
using System.ComponentModel;



namespace MMM_chart.ViewModel
{
    using System.Collections.Generic;
    using OxyPlot;
    using Newtonsoft.Json;
    using System.Net;
    using Model;

    public class MainViewModel : INotifyPropertyChanged
    {

        public string Title { get; private set; }
        public IList<DataPoint> Points { get; private set; }
        public string IpAddress { get; private set; }
        public string Roll { get; private set; }
        public ButtonCommand StartButton { get; set; }
        public ButtonCommand StopButton { get; set; }

        System.Windows.Threading.DispatcherTimer dispatcherTimer;


        private void updateTester()
        {
            var url = "http://192.168.0.106/data.py";
            var serverData = ServerHandling._download_serialized_json_data<ServerData>(url);
            this.Roll = serverData.roll.ToString();
            OnPropertyChanged("Roll");
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            updateTester();
        }



        public void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        public void StopTimer()
        {
            
        }

        public MainViewModel()
        {
            this.IpAddress = "192.168.0.106";
            this.Title = "Example 2";
            this.Points = new List<DataPoint>
                              {
                                  new DataPoint(0, 4),
                                  new DataPoint(10, 13),
                                  new DataPoint(20, 15),
                                  new DataPoint(30, 16),
                                  new DataPoint(40, 12),
                                  new DataPoint(50, 12)
                              };
            
            StartButton = new ButtonCommand(StartTimer);
            StopButton = new ButtonCommand(StopTimer);
            Roll = "Robert Kubica";

        }



        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /**
         * @brief Simple function to trigger event handler
         * @params propertyName Name of ViewModel property as string
         */
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
