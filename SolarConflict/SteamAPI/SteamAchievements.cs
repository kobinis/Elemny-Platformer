using Microsoft.Xna.Framework.Input;
using Steamworks;
using System;
using XnaUtils;

public static class SteamAchievements
{
	private enum Achievement : int //API Name Progress Stat
	{
		StartedGame,
		RoadsPlaced,
		House1,
		Fire,
		Collapse,
	};

	private static Achievement_t[] m_Achievements = new Achievement_t[] {
		new Achievement_t(Achievement.StartedGame, "Started Game", "Big things start small"),
		new Achievement_t(Achievement.RoadsPlaced, "Road Constructor", "All roads lead to..."),
		new Achievement_t(Achievement.House1, "First Step", "People have arrived to your city"),
		new Achievement_t(Achievement.Fire, "Fire", "Due to your negligence, a building has set on fire"),
		new Achievement_t(Achievement.Collapse, "Collapse", "Due to your negligence, a building has collapsed"),
	};

	// Our GameID
	private static CGameID m_GameID;

	// Did we get the stats from Steam?
	private static bool m_bRequestedStats;
	private static bool m_bStatsValid;

	// Should we store stats this frame?
	private static bool m_bStoreStats;

	// Persisted Stat details
	private static int totalGameLaunches;
	private static int totalRoadsPlaced;
	private static int totalHousesLvl1;
	private static int totalFires;
	private static int totalCollapses;

	static Callback<UserStatsReceived_t> m_UserStatsReceived;
	static Callback<UserStatsStored_t> m_UserStatsStored;
	static Callback<UserAchievementStored_t> m_UserAchievementStored;

	public static void Init()
	{
		if (!Steam.IsSteamRunning())
			return;

		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

		// These need to be reset to get the stats upon an Assembly reload in the Editor.
		m_bRequestedStats = false;
		m_bStatsValid = false;
	}

	public static void Update()
	{
		if (!Steam.IsSteamRunning())
			return;

		if (!m_bRequestedStats)
		{
			// Is Steam Loaded? if no, can't get stats, done
			if (!Steam.IsSteamRunning())
			{
				m_bRequestedStats = true;
				return;
			}

			// If yes, request our stats
			bool bSuccess = SteamUserStats.RequestCurrentStats();

			// This function should only return false if we weren't logged in, and we already checked that.
			// But handle it being false again anyway, just ask again later.
			m_bRequestedStats = bSuccess;
		}

		if (!m_bStatsValid)
			return;

		// Get info from sources

		// Evaluate achievements
		if(ActivityManager.Inst.InputState.IsKeyPressed(Keys.OemTilde))
        {
			UnlockAchievement(m_Achievements[0]);
		}

		//foreach (Achievement_t achievement in m_Achievements)
		//{
		//	if (achievement.m_bAchieved)
		//		continue;

		//	switch (achievement.m_eAchievementID)
		//	{
		//		case Achievement.StartedGame:
		//			if (totalGameLaunches != 0)
		//			{
		//				UnlockAchievement(achievement);
		//			}
		//			break;
		//		case Achievement.RoadsPlaced:
		//			if (totalRoadsPlaced >= 100)
		//			{
		//				UnlockAchievement(achievement);
		//			}
		//			break;
		//	}
		//}

		//Store stats in the Steam database if necessary
		if (m_bStoreStats)
		{
			// already set any achievements in UnlockAchievement

			// set stats
			SteamUserStats.SetStat("StartGameCount", totalGameLaunches);
			SteamUserStats.SetStat("RoadsPlacedCount", totalRoadsPlaced);
			SteamUserStats.SetStat("House1Count", totalHousesLvl1);
			SteamUserStats.SetStat("FireCount", totalFires);
			SteamUserStats.SetStat("CollapseCount", totalCollapses);

			// Update average feet / second stat
			//SteamUserStats.UpdateAvgRateStat("AverageSpeed", m_flGameFeetTraveled, m_flGameDurationSeconds);
			// The averaged result is calculated for us
			//SteamUserStats.GetStat("AverageSpeed", out m_flAverageSpeed);

			bool bSuccess = SteamUserStats.StoreStats();
			// If this failed, we never sent anything to the server, try
			// again later.
			m_bStoreStats = !bSuccess;
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Accumulate distance traveled
	//-----------------------------------------------------------------------------
	/*public static void AddDistanceTraveled(float flDistance)
	{
		m_flGameFeetTraveled += flDistance;
	}*/

	public static void AddGameLaunch()
	{
		totalGameLaunches++;
		m_bStoreStats = true;
	}

	public static void AddRoadPlaced()
	{
		totalRoadsPlaced++;
		m_bStoreStats = true;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Game state has changed
	//-----------------------------------------------------------------------------
	/*public void OnGameStateChange(EClientGameState eNewState)
	{
		if (!m_bStatsValid)
			return;

		if (eNewState == EClientGameState.k_EClientGameActive)
		{
			// Reset per-game stats
			m_flGameFeetTraveled = 0;
			m_ulTickCountGameStart = Time.time;
		}
		else if (eNewState == EClientGameState.k_EClientGameWinner || eNewState == EClientGameState.k_EClientGameLoser)
		{
			if (eNewState == EClientGameState.k_EClientGameWinner)
			{
				m_nTotalNumWins++;
			}
			else
			{
				m_nTotalNumLosses++;
			}

			// Tally games
			m_nTotalGamesPlayed++;

			// Accumulate distances
			m_flTotalFeetTraveled += m_flGameFeetTraveled;

			// New max?
			if (m_flGameFeetTraveled > m_flMaxFeetTraveled)
				m_flMaxFeetTraveled = m_flGameFeetTraveled;

			// Calc game duration
			m_flGameDurationSeconds = Time.time - m_ulTickCountGameStart;

			// We want to update stats the next frame.
			m_bStoreStats = true;
		}
	}*/

	//-----------------------------------------------------------------------------
	// Purpose: Unlock this achievement
	//-----------------------------------------------------------------------------
	private static void UnlockAchievement(Achievement_t achievement)
	{
		achievement.m_bAchieved = true;

		// the icon may change once it's unlocked
		//achievement.m_iIconImage = 0;

		// mark it down
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

		// Store stats end of frame
		m_bStoreStats = true;
	}

	//-----------------------------------------------------------------------------
	// Purpose: We have stats data from Steam. It is authoritative, so update
	//			our data with those results now.
	//-----------------------------------------------------------------------------
	private static void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if (!Steam.IsSteamRunning())
			return;

		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Console.WriteLine("Received stats and achievements from Steam\n");

				m_bStatsValid = true;

				// load achievements
				foreach (Achievement_t ach in m_Achievements)
				{
					bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
					if (ret)
					{
						ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
						ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
					}
					else
					{
						Console.WriteLine("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
					}
				}

				// load stats
				SteamUserStats.GetStat("StartGameCount", out totalGameLaunches);
				SteamUserStats.GetStat("RoadsPlacedCount", out totalRoadsPlaced);
				SteamUserStats.GetStat("House1Count", out totalHousesLvl1);
				SteamUserStats.GetStat("FireCount", out totalFires);
				SteamUserStats.GetStat("CollapseCount", out totalCollapses);
				/*SteamUserStats.GetStat("NumLosses", out m_nTotalNumLosses);
				SteamUserStats.GetStat("FeetTraveled", out m_flTotalFeetTraveled);
				SteamUserStats.GetStat("MaxFeetTraveled", out m_flMaxFeetTraveled);
				SteamUserStats.GetStat("AverageSpeed", out m_flAverageSpeed);*/
			}
			else
			{
				Console.WriteLine("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Our stats data was stored!
	//-----------------------------------------------------------------------------
	private static void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (EResult.k_EResultOK == pCallback.m_eResult)
			{
				Console.WriteLine("StoreStats - success");
			}
			else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
			{
				// One or more stats we set broke a constraint. They've been reverted,
				// and we should re-iterate the values now to keep in sync.
				Console.WriteLine("StoreStats - some failed to validate");
				// Fake up a callback here so that we re-load the values.
				UserStatsReceived_t callback = new UserStatsReceived_t();
				callback.m_eResult = EResult.k_EResultOK;
				callback.m_nGameID = (ulong)m_GameID;
				OnUserStatsReceived(callback);
			}
			else
			{
				Console.WriteLine("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: An achievement was stored
	//-----------------------------------------------------------------------------
	private static void OnAchievementStored(UserAchievementStored_t pCallback)
	{
		// We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID)
		{
			if (0 == pCallback.m_nMaxProgress)
			{
				Console.WriteLine("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
			}
			else
			{
				Console.WriteLine("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
			}
		}
	}

	private class Achievement_t
	{
		public Achievement m_eAchievementID;
		public string m_strName;
		public string m_strDescription;
		public bool m_bAchieved;

		/// <summary>
		/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
		/// </summary>
		/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
		/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
		/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
		public Achievement_t(Achievement achievementID, string name, string desc)
		{
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
		}
	}
}