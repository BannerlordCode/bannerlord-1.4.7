using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000311 RID: 785
	public abstract class MPPerkCondition<T> : MPPerkCondition where T : MissionMultiplayerGameModeBase
	{
		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002CBD RID: 11453 RVA: 0x000AC55C File Offset: 0x000AA75C
		protected T GameModeInstance
		{
			get
			{
				Mission mission = Mission.Current;
				if (mission == null)
				{
					return default(T);
				}
				return mission.GetMissionBehavior<T>();
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000AC584 File Offset: 0x000AA784
		protected override bool IsGameModesValid(List<string> gameModes)
		{
			if (typeof(MissionMultiplayerFlagDomination).IsAssignableFrom(typeof(T)))
			{
				string text = MultiplayerGameType.Skirmish.ToString();
				string text2 = MultiplayerGameType.Captain.ToString();
				foreach (string text3 in gameModes)
				{
					if (!text3.Equals(text, StringComparison.InvariantCultureIgnoreCase) && !text3.Equals(text2, StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}
				return true;
			}
			if (typeof(MissionMultiplayerTeamDeathmatch).IsAssignableFrom(typeof(T)))
			{
				string text4 = MultiplayerGameType.TeamDeathmatch.ToString();
				using (List<string>.Enumerator enumerator = gameModes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Equals(text4, StringComparison.InvariantCultureIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (typeof(MissionMultiplayerSiege).IsAssignableFrom(typeof(T)))
			{
				string text5 = MultiplayerGameType.Siege.ToString();
				using (List<string>.Enumerator enumerator = gameModes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Equals(text5, StringComparison.InvariantCultureIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			}
			Debug.FailedAssert("Not implemented game mode check", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\Perks\\MPPerkCondition.cs", "IsGameModesValid", 134);
			return false;
		}
	}
}
