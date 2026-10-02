using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001FF RID: 511
	public abstract class BodyPropertiesModel : MBGameModel<BodyPropertiesModel>
	{
		// Token: 0x06001FA2 RID: 8098
		public abstract int[] GetHairIndicesForCulture(int race, int gender, float age, CultureObject culture);

		// Token: 0x06001FA3 RID: 8099
		public abstract int[] GetBeardIndicesForCulture(int race, int gender, float age, CultureObject culture);

		// Token: 0x06001FA4 RID: 8100
		public abstract int[] GetTattooIndicesForCulture(int race, int gender, float age, CultureObject culture);
	}
}
