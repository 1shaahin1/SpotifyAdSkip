using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace spotifyAdSkip
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);
        public const int KEYEVENTF_EXTENTEDKEY = 1;
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;// code to play or pause a song
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;// code to jump to next track

        static void Main(string[] args)
        {
            Console.WriteLine("Spotify Ad Skipper!");
            int count = 0;
            while (true) {
                Process[] processlist = Process.GetProcessesByName("Spotify");

                foreach (Process process in processlist)
                {
                    string spotifyPath = process.MainModule.FileName;
                    if (process.MainWindowTitle == "Advertisement" || process.MainWindowTitle == "Spotify")
                    {
                        process.Kill();
                        process.WaitForExit();

                        ProcessStartInfo start = new ProcessStartInfo();
                        start.FileName = spotifyPath;
                        start.WindowStyle = ProcessWindowStyle.Minimized;
                        Process newSpotify = Process.Start(start);
                        Console.Write("\rskipped: " + (++count));

                        Thread.Sleep(5000);
                        keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                        keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
                        break;
                    }
                }

                Thread.Sleep(2000);
            }
            
        }
    }
}
