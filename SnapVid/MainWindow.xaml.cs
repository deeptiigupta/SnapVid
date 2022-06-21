using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace CustomProgressBar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            card.prog.Maximum = 100;
        }
        double totalTime;
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var youtube = new YoutubeClient();

            var video = await youtube.Videos.GetAsync(url.Text);

            card.title.Content = video.Title;
            var duration = video.Duration;
            totalTime=duration.Value.TotalMilliseconds;
            card.duration.Content = duration.Value.TotalMinutes;
            card.img.Source = new BitmapImage(new Uri(video.Thumbnails[0].Url));
            card.Visibility = Visibility.Visible;
            youtube = new YoutubeClient();

            var x = url.Text;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] == '=')
                {
                    x = x.Substring(i + 1);
                    break;
                }
            }
            try
            {
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(x);
                // Get highest quality muxed stream
                var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

                var stream = await youtube.Videos.Streams.GetAsync(streamInfo);

                // Download the stream to a file
                var progress = new Progress<double>(p => OnProgressChanged(p));

                await youtube.Videos.Streams.DownloadAsync(streamInfo, $"..//..//..//video.{streamInfo.Container}", progress);
            }
            catch(Exception pratyush)
            {
                MessageBox.Show(pratyush.Message);
            }


            MessageBox.Show("Downloaded");
        }

        private void OnProgressChanged(double p)
        {
            Console.WriteLine(p);
            card.prog.Value = p*100;
        }
    }
}
