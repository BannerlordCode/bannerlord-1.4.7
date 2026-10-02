using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000147 RID: 327
	public class DefaultRomanceModel : RomanceModel
	{
		// Token: 0x060019C2 RID: 6594 RVA: 0x00081B78 File Offset: 0x0007FD78
		public override int GetAttractionValuePercentage(Hero potentiallyInterestedCharacter, Hero heroOfInterest)
		{
			return MathF.Abs((potentiallyInterestedCharacter.StaticBodyProperties.GetHashCode() + heroOfInterest.StaticBodyProperties.GetHashCode()) % 100);
		}
	}
}
