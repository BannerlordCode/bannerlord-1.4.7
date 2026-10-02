using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x020000F7 RID: 247
	public class DefaultBodyPropertiesModel : BodyPropertiesModel
	{
		// Token: 0x06001688 RID: 5768 RVA: 0x000685D7 File Offset: 0x000667D7
		public override int[] GetHairIndicesForCulture(int race, int gender, float age, CultureObject culture)
		{
			return FaceGen.GetHairIndicesByTag(race, gender, age, culture.StringId);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x000685E8 File Offset: 0x000667E8
		public override int[] GetBeardIndicesForCulture(int race, int gender, float age, CultureObject culture)
		{
			return FaceGen.GetFacialIndicesByTag(race, gender, age, culture.StringId);
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x000685F9 File Offset: 0x000667F9
		public override int[] GetTattooIndicesForCulture(int race, int gender, float age, CultureObject culture)
		{
			return FaceGen.GetTattooIndicesByTag(race, gender, age, culture.StringId);
		}
	}
}
