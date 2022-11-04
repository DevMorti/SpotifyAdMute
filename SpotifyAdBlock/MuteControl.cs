using System;
using System.Diagnostics;
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
                WaitForMusic();
                Audio.Mute = false;
            }
            while (true)
            {
                WaitForAds();
                Audio.Mute = true;
                WaitForMusic();
                Audio.Mute = false;
            }
        }

        internal static void WaitForAds()
        {
            while(true)
            {
                Process spotify = GetSpotify();
                if (spotify.MainWindowTitle == "Advertisement" || spotify.MainWindowTitle == "Spotify Free")
                    break;
                Thread.Sleep(1000);
            }
        }

        internal static void WaitForMusic()
        {
            while(true)
            { 
                Process spotify = GetSpotify();
                if (spotify.MainWindowTitle != "Advertisement" && spotify.MainWindowTitle != "Spotify Free")
                    break;
                Thread.Sleep(1000);
            }
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
                Environment.Exit(0);
            }
            return spotify;
        }
    }
}
