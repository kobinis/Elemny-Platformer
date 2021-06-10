using System;
using System.IO;
using Steamworks;

namespace SolarConflict
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string error = string.Empty;
#if DEBUG
            Steam.Init();
            using (var game = new Game1(args))
                game.Run();
            Steam.Close();
#else
            SteamAPI.Init();
            try
            {            
               
                using (var game = new Game1(args))
                    game.Run();
            }
            catch (Exception e)
            {
                error = e.ToString();
                throw e;
            }
            finally
            {
                if (!string.IsNullOrEmpty(error))
                {
                    string filename = "Bug" + DateTime.Now.ToString("yyyy-M-dd--HH-mm-ss") + ".txt";
                    File.WriteAllText(filename, error);
                }
                SteamAPI.Shutdown();
            }
#endif
        }
    }
#endif
}
