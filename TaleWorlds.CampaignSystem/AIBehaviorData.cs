using System;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000091 RID: 145
	public struct AIBehaviorData : IEquatable<AIBehaviorData>
	{
		// Token: 0x06001261 RID: 4705 RVA: 0x000547C8 File Offset: 0x000529C8
		public AIBehaviorData(IMapPoint party, AiBehavior aiBehavior, MobileParty.NavigationType navigationType, bool willGatherArmy, bool isFromPort, bool isTargetingPort)
		{
			this.Party = party;
			this.AiBehavior = aiBehavior;
			this.NavigationType = navigationType;
			this.WillGatherArmy = willGatherArmy;
			this.IsFromPort = isFromPort;
			this.IsTargetingPort = isTargetingPort;
			this.Position = CampaignVec2.Zero;
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00054802 File Offset: 0x00052A02
		public AIBehaviorData(CampaignVec2 position, AiBehavior aiBehavior, MobileParty.NavigationType navigationType, bool willGatherArmy, bool isFromPort, bool isTargetingPort)
		{
			this.Position = position;
			this.Party = null;
			this.AiBehavior = aiBehavior;
			this.NavigationType = navigationType;
			this.WillGatherArmy = willGatherArmy;
			this.IsFromPort = isFromPort;
			this.IsTargetingPort = isTargetingPort;
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00054838 File Offset: 0x00052A38
		public override bool Equals(object obj)
		{
			return obj is AIBehaviorData && (AIBehaviorData)obj == this;
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00054855 File Offset: 0x00052A55
		public bool Equals(AIBehaviorData other)
		{
			return other == this;
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00054864 File Offset: 0x00052A64
		public override int GetHashCode()
		{
			int aiBehavior = (int)this.AiBehavior;
			int num = aiBehavior.GetHashCode();
			num = ((this.Party != null) ? ((num * 397) ^ this.Party.GetHashCode()) : num);
			num = (num * 397) ^ this.WillGatherArmy.GetHashCode();
			num = (num * 397) ^ this.IsTargetingPort.GetHashCode();
			num = (num * 397) ^ this.IsFromPort.GetHashCode();
			num = (num * 397) ^ this.NavigationType.GetHashCode();
			return (num * 397) ^ this.Position.GetHashCode();
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00054910 File Offset: 0x00052B10
		public static bool operator ==(AIBehaviorData a, AIBehaviorData b)
		{
			return a.Party == b.Party && a.AiBehavior == b.AiBehavior && a.NavigationType == b.NavigationType && a.WillGatherArmy == b.WillGatherArmy && a.IsFromPort == b.IsFromPort && a.IsTargetingPort == b.IsTargetingPort && a.Position == b.Position;
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x00054984 File Offset: 0x00052B84
		public static bool operator !=(AIBehaviorData a, AIBehaviorData b)
		{
			return !(a == b);
		}

		// Token: 0x04000611 RID: 1553
		public static readonly AIBehaviorData Invalid = new AIBehaviorData(null, AiBehavior.None, MobileParty.NavigationType.None, false, false, false);

		// Token: 0x04000612 RID: 1554
		public IMapPoint Party;

		// Token: 0x04000613 RID: 1555
		public CampaignVec2 Position;

		// Token: 0x04000614 RID: 1556
		public AiBehavior AiBehavior;

		// Token: 0x04000615 RID: 1557
		public bool WillGatherArmy;

		// Token: 0x04000616 RID: 1558
		public bool IsFromPort;

		// Token: 0x04000617 RID: 1559
		public bool IsTargetingPort;

		// Token: 0x04000618 RID: 1560
		public MobileParty.NavigationType NavigationType;
	}
}
