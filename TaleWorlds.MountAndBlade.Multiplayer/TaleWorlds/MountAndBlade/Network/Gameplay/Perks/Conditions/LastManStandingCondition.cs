using System;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions
{
	// Token: 0x0200004A RID: 74
	public class LastManStandingCondition : MPPerkCondition
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000AE59 File Offset: 0x00009059
		public override MPPerkCondition.PerkEventFlags EventFlags
		{
			get
			{
				return MPPerkCondition.PerkEventFlags.AliveBotCountChange | MPPerkCondition.PerkEventFlags.SpawnEnd;
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000AE60 File Offset: 0x00009060
		protected LastManStandingCondition()
		{
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000AE68 File Offset: 0x00009068
		protected override void Deserialize(XmlNode node)
		{
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000AE6A File Offset: 0x0000906A
		public override bool Check(MissionPeer peer)
		{
			return this.Check((peer != null) ? peer.ControlledAgent : null);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000AE80 File Offset: 0x00009080
		public override bool Check(Agent agent)
		{
			agent = ((agent != null && agent.IsMount) ? agent.RiderAgent : agent);
			MissionPeer missionPeer = ((agent != null) ? agent.MissionPeer : null) ?? ((agent != null) ? agent.OwningAgentMissionPeer : null);
			if (MultiplayerOptions.OptionType.NumberOfBotsPerFormation.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) <= 0 || ((missionPeer != null) ? missionPeer.ControlledFormation : null) == null || !agent.IsActive())
			{
				return false;
			}
			if (!agent.IsPlayerControlled)
			{
				return missionPeer.BotsUnderControlAlive == 1;
			}
			return missionPeer.BotsUnderControlAlive == 0;
		}

		// Token: 0x040000C7 RID: 199
		protected static string StringType = "LastManStanding";
	}
}
