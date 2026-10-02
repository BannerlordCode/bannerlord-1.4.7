using System;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A0 RID: 928
	public class PortState : GameState
	{
		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06003589 RID: 13705 RVA: 0x000D9E9A File Offset: 0x000D809A
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x000D9E9D File Offset: 0x000D809D
		public PortState()
		{
			Debug.FailedAssert("do not use parameterless constructor.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\GameState\\PortState.cs", ".ctor", 39);
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x000D9EBC File Offset: 0x000D80BC
		public PortState(PartyBase leftOwner, PartyBase rightOwner, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = ((leftOwner != null) ? leftOwner.Ships : null);
			this.RightShips = ((rightOwner != null) ? rightOwner.Ships : null);
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x000D9F08 File Offset: 0x000D8108
		public PortState(PartyBase leftOwner, PartyBase rightOwner, Action onEndAction, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = ((leftOwner != null) ? leftOwner.Ships : null);
			this.RightShips = ((rightOwner != null) ? rightOwner.Ships : null);
			this.OnEndAction = onEndAction;
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x000D9F5C File Offset: 0x000D815C
		public PortState(MBReadOnlyList<Ship> leftShips, MBReadOnlyList<Ship> rightShips, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = null;
			this.RightOwner = null;
			this.LeftShips = leftShips;
			this.RightShips = rightShips;
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x000D9F87 File Offset: 0x000D8187
		public PortState(PartyBase leftOwner, PartyBase rightOwner, MBReadOnlyList<Ship> leftShips, MBReadOnlyList<Ship> rightShips, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = leftShips;
			this.RightShips = rightShips;
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x000D9FB4 File Offset: 0x000D81B4
		public PortState(PartyBase leftOwner, PartyBase rightOwner, MBReadOnlyList<Ship> leftShips, MBReadOnlyList<Ship> rightShips, Action onEndAction, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = leftOwner;
			this.RightOwner = rightOwner;
			this.LeftShips = leftShips;
			this.RightShips = rightShips;
			this.OnEndAction = onEndAction;
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x000D9FE9 File Offset: 0x000D81E9
		public PortState(Settlement settlement, PartyBase rightOwner, PortScreenModes portScreenMode)
		{
			this.PortScreenMode = portScreenMode;
			this.LeftOwner = settlement.Party;
			this.RightOwner = rightOwner;
			this.LeftShips = settlement.Party.Ships;
			this.RightShips = rightOwner.Ships;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x000DA028 File Offset: 0x000D8228
		protected override void OnFinalize()
		{
			base.OnFinalize();
			Action onEndAction = this.OnEndAction;
			if (onEndAction == null)
			{
				return;
			}
			onEndAction();
		}

		// Token: 0x04000F45 RID: 3909
		public readonly PortScreenModes PortScreenMode;

		// Token: 0x04000F46 RID: 3910
		public readonly PartyBase LeftOwner;

		// Token: 0x04000F47 RID: 3911
		public readonly PartyBase RightOwner;

		// Token: 0x04000F48 RID: 3912
		public readonly MBReadOnlyList<Ship> LeftShips;

		// Token: 0x04000F49 RID: 3913
		public readonly MBReadOnlyList<Ship> RightShips;

		// Token: 0x04000F4A RID: 3914
		public readonly Action OnEndAction;
	}
}
