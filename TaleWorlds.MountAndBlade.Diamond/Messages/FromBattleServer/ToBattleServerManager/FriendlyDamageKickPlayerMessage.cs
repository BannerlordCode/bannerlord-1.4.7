using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D7 RID: 215
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class FriendlyDamageKickPlayerMessage : Message
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00004B96 File Offset: 0x00002D96
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00004B9E File Offset: 0x00002D9E
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x00004BA7 File Offset: 0x00002DA7
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00004BAF File Offset: 0x00002DAF
		[TupleElementNames(new string[] { "killCount", "damage" })]
		[JsonProperty]
		public Dictionary<int, ValueTuple<int, float>> RoundDamageMap
		{
			[return: TupleElementNames(new string[] { "killCount", "damage" })]
			get;
			[param: TupleElementNames(new string[] { "killCount", "damage" })]
			private set;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public FriendlyDamageKickPlayerMessage()
		{
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00004BC0 File Offset: 0x00002DC0
		public FriendlyDamageKickPlayerMessage(PlayerId playerId, [TupleElementNames(new string[] { "killCount", "damage" })] Dictionary<int, ValueTuple<int, float>> roundDamageMap)
		{
			this.PlayerId = playerId;
			this.RoundDamageMap = roundDamageMap;
		}
	}
}
