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
        #region SwitchView
        

        #endregion

        public string Title { get; private set; }
        public string IpAddress { get; private set; }
        public string Roll { get; private set; }
        public string Pitch { get; private set; }
        public string Yaw { get; private set; }
        public ButtonCommand StartButton { get; set; }
        public ButtonCommand StopButton { get; set; }

        System.Windows.Threading.DispatcherTimer dispatcherTimer;


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //updateTester();
            updateData();
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
            dispatcherTimer.Stop();
            dispatcherTimer = null;
        }

        #region Plot

        public PlotModel Plot { get; set; }

        private Configure config = new Configure();

        private string base_url = "http://192.168.0.106/data.py";

        public double i = 0.0;

        private void UpdatePlot(double t, double d, PlotModel Plot_n, int i)
        {
            LineSeries lineSeries = Plot_n.Series[i] as LineSeries;
            

            lineSeries.Points.Add(new DataPoint(t, d));

            if (lineSeries.Points.Count > config.MaxSampleNumber)
                lineSeries.Points.RemoveAt(0);

            if (t >= config.XAxisMax)
            {
                Plot_n.Axes[0].Minimum = (t - config.XAxisMax);
                Plot_n.Axes[0].Maximum = t + config.SampleTime / 1000.0; ;
            }

            Plot_n.InvalidatePlot(true);
        }

        public void updateData()
        {
            var data = ServerHandling._download_serialized_json_data<ServerData>(base_url);
           // this.Roll = data.roll.ToString();
            UpdatePlot(i, data.roll, Plot,0);
            UpdatePlot(i, data.pitch, Plot,1);
            UpdatePlot(i, data.yaw, Plot,2);
            //OnPropertyChanged("Roll");
            i += config.SampleTime / 1000.0;
        }

        #endregion

        public MainViewModel()
        {
            this.IpAddress = "192.168.0.106";
            this.Title = "RPY Chart";

           
            Plot = new PlotModel { Title = "Angle" };

            Plot.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = config.XAxisMax,
                Key = "Horizontal",
                Unit = "sec",
                Title = "Time"
            });
            Plot.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 360,
                Key = "Vertical",
                Unit = "Degrees",
                Title = "Angle"
            });
            Plot.Series.Add(new LineSeries() { Title = "Roll", Color = OxyColor.Parse("#cd0000") });
            Plot.Series.Add(new LineSeries() { Title = "Pitch", Color = OxyColor.Parse("#4876FF") });
            Plot.Series.Add(new LineSeries() { Title = "Yaw", Color = OxyColor.Parse("#047806") });

            #region buttons
            StartButton = new ButtonCommand(StartTimer);
            StopButton = new ButtonCommand(StopTimer);
            #endregion
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
