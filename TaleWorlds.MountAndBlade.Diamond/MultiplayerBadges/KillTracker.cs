using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016C RID: 364
	public class KillTracker : GameBadgeTracker
	{
		// Token: 0x06000A15 RID: 2581 RVA: 0x00010114 File Offset: 0x0000E314
		public KillTracker(string badgeId, BadgeCondition condition, Dictionary<ValueTuple<PlayerId, string, string>, int> dataDictionary)
		{
			this._badgeId = badgeId;
			this._condition = condition;
			this._dataDictionary = dataDictionary;
			this._faction = null;
			this._troop = null;
			string text;
			if (condition.Parameters.TryGetValue("faction", out text))
			{
				this._faction = text;
			}
			string text2;
			if (condition.Parameters.TryGetValue("troop", out text2))
			{
				this._troop = text2;
			}
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00010180 File Offset: 0x0000E380
		public override void OnKill(KillData killData)
		{
			if (killData.KillerId.IsValid && killData.VictimId.IsValid && !killData.KillerId.Equals(killData.VictimId) && (this._faction == null || this._faction == killData.KillerFaction) && (this._troop == null || this._troop == killData.KillerTroop))
			{
				int num;
				if (!this._dataDictionary.TryGetValue(new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId), out num))
				{
					num = 0;
				}
				this._dataDictionary[new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId)] = num + 1;
			}
		}

		// Token: 0x040004F2 RID: 1266
		private readonly string _badgeId;

		// Token: 0x040004F3 RID: 1267
		private readonly BadgeCondition _condition;

		// Token: 0x040004F4 RID: 1268
		private readonly Dictionary<ValueTuple<PlayerId, string, string>, int> _dataDictionary;

		// Token: 0x040004F5 RID: 1269
		private readonly string _faction;

		// Token: 0x040004F6 RID: 1270
		private readonly string _troop;
	}
}
