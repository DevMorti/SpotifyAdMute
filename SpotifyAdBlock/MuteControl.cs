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
            while (true)
            {
                WaitForMusic();
                Audio.Mute = false;
                WaitForAds();
                Audio.Mute = true;
            }
        }

        internal static void WaitForAds()
        {
            bool noAds = true;
            do
            {
                Thread.Sleep(1000);
                Process[] processes = Process.GetProcessesByName("Spotify");
                foreach (Process process in processes)
                {
                    if ((!process.MainWindowTitle.Contains('-') && process.MainWindowTitle != "") || process.MainWindowTitle == "Spotify Free" || process.MainWindowTitle == "Advertisement")
                    {
                        noAds = false;
                        Debug.WriteLine(process.MainWindowTitle + " wurde als Werbung eingestuft.");
                        break;
                    }
                }
            } while (noAds);
        }

        internal static void WaitForMusic()
        {
            bool noAds = false;
            do
            {
                Thread.Sleep(1000);
                Process[] processes = Process.GetProcessesByName("Spotify");
                foreach (Process process in processes)
                {
                    if (process.MainWindowTitle.Contains('-'))
                    {
                        Debug.WriteLine(process.MainWindowTitle + " wurde als Musik eingestuft.");
                        noAds = true;
                        break;
                    }
                }
            } while (!noAds);
        }
    }
}
