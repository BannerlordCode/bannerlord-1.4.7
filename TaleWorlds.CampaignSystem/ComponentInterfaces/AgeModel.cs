using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001DD RID: 477
	public abstract class AgeModel : MBGameModel<AgeModel>
	{
		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06001EAA RID: 7850
		public abstract int BecomeInfantAge { get; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06001EAB RID: 7851
		public abstract int BecomeChildAge { get; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001EAC RID: 7852
		public abstract int BecomeTeenagerAge { get; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06001EAD RID: 7853
		public abstract int HeroComesOfAge { get; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06001EAE RID: 7854
		public abstract int BecomeOldAge { get; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06001EAF RID: 7855
		public abstract int MiddleAdultHoodAge { get; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06001EB0 RID: 7856
		public abstract int MaxAge { get; }

		// Token: 0x06001EB1 RID: 7857
		public abstract void GetAgeLimitForLocation(CharacterObject character, out int minimumAge, out int maximumAge, string additionalTags = "");
	}
}
