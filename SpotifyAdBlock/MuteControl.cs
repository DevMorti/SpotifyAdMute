using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace SpotifyAdBlock
{
    internal static class MuteControl
    {
        internal static void Control()
        {
            Process.Start("Spotify");
            Thread.Sleep(3000);
            if(Audio.Mute)
            {
                WaitFor(false);
                Audio.Mute = false;
            }
            while (true)
            {
                WaitFor(true);
                Audio.Mute = true;
                WaitFor(false);
                Audio.Mute = false;
            }
        }

        internal static void WaitFor(bool forAds)
        {
            Process spotify;
            while(true)
            {
                spotify = GetSpotify();
                if (IsAd(spotify) == forAds)
                    break;
                Thread.Sleep(1000);
            }
            WriteLog(spotify, "Werbung");
        }

        internal static Process GetSpotify()
        {
            Process spotify = new Process();
            try
            {
                spotify = Process.GetProcessesByName("Spotify").Single(process => process.MainWindowTitle != "");
            }
            catch
            {
                Audio.Mute = false;
                Environment.Exit(0);
            }
            return spotify;
        }

        internal static void WriteLog(Process process, string message)
        {
            try
            {
                FileInfo file = new FileInfo(AppContext.BaseDirectory + "Mute.log");
                if (!file.Exists)
                    file.Create();
                using (StreamWriter stream = file.AppendText())
                {
                    stream.WriteLine(process.MainWindowTitle + " - " + DateTime.Now + " - " + message);
                }
            }
            catch { }
        }

        internal static bool IsAd(Process spotify)
        {
            return spotify.MainWindowTitle == "Advertisement" || spotify.MainWindowTitle == "Spotify Free" || !spotify.MainWindowTitle.Contains('-');
        }
    }
}
