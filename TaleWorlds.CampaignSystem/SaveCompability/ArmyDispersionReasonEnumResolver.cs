using System;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000CD RID: 205
	public class ArmyDispersionReasonEnumResolver : IEnumResolver
	{
		// Token: 0x0600145D RID: 5213 RVA: 0x0005ED4C File Offset: 0x0005CF4C
		public string ResolveObject(string originalObject)
		{
			if (string.IsNullOrEmpty(originalObject))
			{
				Debug.FailedAssert("ArmyDispersionReason data is null or empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SaveCompability\\ArmyDispersionReasonEnumResolver.cs", "ResolveObject", 16);
				return Army.ArmyDispersionReason.Unknown.ToString();
			}
			if (originalObject.Equals("LowPartySizeRatio"))
			{
				return Army.ArmyDispersionReason.NotEnoughTroop.ToString();
			}
			return originalObject;
		}
	}
}
