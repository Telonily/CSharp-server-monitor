using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServerMonitor.Themes;

namespace ServerMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMonitorHandler
    {
        private readonly Monitor _monitor;
        private string _logFile = "log.txt";
        string _lastMap = "";
        private ThemeDirector _themeDirector;

        public MainWindow()
        {
            InitializeComponent();
            _themeDirector = new ThemeDirector();
            _themeDirector.SetThemeBuilder(new DarkThemeBuilder());


            _monitor = new Monitor(this, "46.174.54.137:1337");
            


            var myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(ThreadSendInfoRequest);
            myTimer.Interval = 5000;       
            myTimer.Enabled = true;

            
        }

        public void HandleServerInfo(NameValueCollection data)
        {
            string map = data["map"];
            string playersCount = data["playersCount"];
            string maximumPlayers = data["maximumPlayers"];

            String report = DateTime.Now.ToString("h:mm");
            report += "\t" + playersCount + "/" + maximumPlayers + "\t" + map;

            if (map != _lastMap)
            {
                _lastMap = map;

                LogToUi(report);
                LogToFile(report);
            }

            SetTbServerInfo(report);
            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // close all active threads
            Environment.Exit(0);
        }

        private void ThreadSendInfoRequest(object o,  ElapsedEventArgs args)
        {
            _monitor.SendInfoRequest();
            _monitor.SendPlayersRequest();
        }


        private void LogToFile(string s)
        {
            StreamWriter streamWriter = new StreamWriter(_logFile, true);
            try
            {
                streamWriter.WriteLine(s);
            }
            catch (Exception e)
            {
                LogToUi(e.Message);
            }
            finally
            {
                streamWriter.Close();
            }
        }

        private void LogToUi(string s)
        {
            Dispatcher.Invoke(() =>
            {
                TbInfo.Text += s + "\n";
            });
        }

        public void HandleError(String mes)
        {
            LogToUi(mes);
            LogToFile(mes);
        }

        public void HandlePlayers(List<Player> players)
        {
            string text = "Connected\t Score \t\t Player \n";
            foreach (Player player in players)
            {
                text += player.GetMinConnected() + ":" + player.GetSecConnected() + "\t\t";
                text += player.Kills + "\t\t";
                text += player.Name + "\t";
                text += "\n";
            }
            WriteTextToPlayers(text);
        }

        public void WriteTextToPlayers(string text)
        {
            Dispatcher.Invoke(() => { TbPlayers.Text = text; });
        }

        public void SetTbServerInfo(string text)
        {
            Dispatcher.Invoke(() => { TbServerInfo.Text = text; });
        }
    }
}
