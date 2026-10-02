using System;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000D2 RID: 210
	public class EndCaptivityDetailEnumResolver : IEnumResolver
	{
		// Token: 0x06001470 RID: 5232 RVA: 0x0005EF4C File Offset: 0x0005D14C
		public string ResolveObject(string originalObject)
		{
			if (string.IsNullOrEmpty(originalObject))
			{
				Debug.FailedAssert("EndCaptivityDetail data is null or empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SaveCompability\\EndCaptivityDetailEnumResolver.cs", "ResolveObject", 15);
				return EndCaptivityDetail.ReleasedByChoice.ToString();
			}
			if (originalObject.Equals("EscapeFromLootedParty"))
			{
				return EndCaptivityDetail.ReleasedAfterEscape.ToString();
			}
			if (originalObject.Equals("ReleasedFromPartyScreen"))
			{
				return EndCaptivityDetail.ReleasedByChoice.ToString();
			}
			if (originalObject.Equals("RemovedParty"))
			{
				return EndCaptivityDetail.ReleasedByChoice.ToString();
			}
			return originalObject;
		}
	}
}
