using Steamworks;
using System;

public static class Steam
{
    static bool isSteamRunning = false;

    public static void Init()
    {
        try
        {
            if (!SteamAPI.Init()) Console.WriteLine("SteamAPI.Init() failed!");
            else
            {
                isSteamRunning = true;
                SteamAchievements.Init();
            }

        }
        catch (DllNotFoundException e)
        {
            Console.WriteLine(e);
        }
    }

    public static bool IsSteamRunning()
    {
        return isSteamRunning;
    }

    public static void Close()
    {
        if (Steam.IsSteamRunning())
            SteamAPI.Shutdown();
    }
}