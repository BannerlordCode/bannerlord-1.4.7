using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x02000168 RID: 360
	public class BadgeOwnerKillTracker : GameBadgeTracker
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x0000FE64 File Offset: 0x0000E064
		public BadgeOwnerKillTracker(string badgeId, BadgeCondition condition, Dictionary<ValueTuple<PlayerId, string, string>, int> dataDictionary)
		{
			this._badgeId = badgeId;
			this._condition = condition;
			this._playerBadgeMap = new Dictionary<PlayerId, bool>();
			this._dataDictionary = dataDictionary;
			this._requiredBadges = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in condition.Parameters)
			{
				if (keyValuePair.Key.StartsWith("required_badge."))
				{
					this._requiredBadges.Add(keyValuePair.Value);
				}
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0000FF00 File Offset: 0x0000E100
		public override void OnPlayerJoin(PlayerData playerData)
		{
			this._playerBadgeMap[playerData.PlayerId] = this._requiredBadges.Contains(playerData.ShownBadgeId);
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0000FF24 File Offset: 0x0000E124
		public override void OnKill(KillData killData)
		{
			bool flag;
			if (killData.KillerId.IsValid && killData.VictimId.IsValid && !killData.KillerId.Equals(killData.VictimId) && this._playerBadgeMap.TryGetValue(killData.VictimId, out flag) && flag)
			{
				this._playerBadgeMap[killData.KillerId] = true;
				int num;
				if (!this._dataDictionary.TryGetValue(new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId), out num))
				{
					num = 0;
				}
				this._dataDictionary[new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId)] = num + 1;
			}
		}

		// Token: 0x040004E6 RID: 1254
		private readonly string _badgeId;

		// Token: 0x040004E7 RID: 1255
		private readonly BadgeCondition _condition;

		// Token: 0x040004E8 RID: 1256
		private readonly List<string> _requiredBadges;

		// Token: 0x040004E9 RID: 1257
		private readonly Dictionary<ValueTuple<PlayerId, string, string>, int> _dataDictionary;

		// Token: 0x040004EA RID: 1258
		private readonly Dictionary<PlayerId, bool> _playerBadgeMap;
	}
}
