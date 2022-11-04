using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace SpotifyAdBlock
{
    internal static class MuteControl
    {
        internal static void Control()
        {
            Process.Start("Spotify");
            Thread.Sleep(1000);
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
                if (spotify.MainWindowTitle == "Advertisement")
                    break;
                Thread.Sleep(1000);
            }
        }

        internal static void WaitForMusic()
        {
            while(true)
            { 
                Process spotify = GetSpotify();
                if (spotify.MainWindowTitle != "Advertisement")
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
