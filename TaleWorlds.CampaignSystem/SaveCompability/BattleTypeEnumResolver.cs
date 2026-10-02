using System;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem.Resolvers;

namespace TaleWorlds.CampaignSystem.SaveCompability
{
	// Token: 0x020000CE RID: 206
	public class BattleTypeEnumResolver : IEnumResolver
	{
		// Token: 0x0600145F RID: 5215 RVA: 0x0005EDB0 File Offset: 0x0005CFB0
		public string ResolveObject(string originalObject)
		{
			if (string.IsNullOrEmpty(originalObject))
			{
				Debug.FailedAssert("EndCaptivityDetail data is null or empty", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\SaveCompability\\BattleTypeEnumResolver.cs", "ResolveObject", 16);
				return MapEvent.BattleTypes.None.ToString();
			}
			if (originalObject.Equals("AlleyFight"))
			{
				return MapEvent.BattleTypes.None.ToString();
			}
			return originalObject;
		}
	}
}
