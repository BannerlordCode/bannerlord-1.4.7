using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000083 RID: 131
	internal class DialogFlowContext
	{
		// Token: 0x060010E2 RID: 4322 RVA: 0x00050E57 File Offset: 0x0004F057
		public DialogFlowContext(string token, bool byPlayer, DialogFlowContext parent, bool optionsUsedOnlyOnce)
		{
			this.Token = token;
			this.ByPlayer = byPlayer;
			this.Parent = parent;
			this.OptionsUsedOnlyOnce = optionsUsedOnlyOnce;
		}

		// Token: 0x0400054A RID: 1354
		internal readonly string Token;

		// Token: 0x0400054B RID: 1355
		internal readonly bool ByPlayer;

		// Token: 0x0400054C RID: 1356
		internal readonly DialogFlowContext Parent;

		// Token: 0x0400054D RID: 1357
		internal readonly bool OptionsUsedOnlyOnce;
	}
}
