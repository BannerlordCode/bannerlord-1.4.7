using System;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002DE RID: 734
	public interface ISiegeEventVisual
	{
		// Token: 0x06002843 RID: 10307
		void Initialize();

		// Token: 0x06002844 RID: 10308
		void OnSiegeEventEnd();

		// Token: 0x06002845 RID: 10309
		void Tick();
	}
}
