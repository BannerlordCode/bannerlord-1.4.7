using System;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;

namespace SandBox.View.Map
{
	// Token: 0x02000047 RID: 71
	public class MapConversationTableauData
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000262 RID: 610 RVA: 0x000170D4 File Offset: 0x000152D4
		// (set) Token: 0x06000263 RID: 611 RVA: 0x000170DC File Offset: 0x000152DC
		public ConversationCharacterData PlayerCharacterData { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000170E5 File Offset: 0x000152E5
		// (set) Token: 0x06000265 RID: 613 RVA: 0x000170ED File Offset: 0x000152ED
		public ConversationCharacterData ConversationPartnerData { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000266 RID: 614 RVA: 0x000170F6 File Offset: 0x000152F6
		// (set) Token: 0x06000267 RID: 615 RVA: 0x000170FE File Offset: 0x000152FE
		public TerrainType ConversationTerrainType { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000268 RID: 616 RVA: 0x00017107 File Offset: 0x00015307
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0001710F File Offset: 0x0001530F
		public float TimeOfDay { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00017118 File Offset: 0x00015318
		// (set) Token: 0x0600026B RID: 619 RVA: 0x00017120 File Offset: 0x00015320
		public bool IsCurrentTerrainUnderSnow { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00017129 File Offset: 0x00015329
		// (set) Token: 0x0600026D RID: 621 RVA: 0x00017131 File Offset: 0x00015331
		public Settlement Settlement { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0001713A File Offset: 0x0001533A
		// (set) Token: 0x0600026F RID: 623 RVA: 0x00017142 File Offset: 0x00015342
		public string LocationId { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0001714B File Offset: 0x0001534B
		// (set) Token: 0x06000271 RID: 625 RVA: 0x00017153 File Offset: 0x00015353
		public bool IsSnowing { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0001715C File Offset: 0x0001535C
		// (set) Token: 0x06000273 RID: 627 RVA: 0x00017164 File Offset: 0x00015364
		public bool IsRaining { get; private set; }

		// Token: 0x06000274 RID: 628 RVA: 0x0001716D File Offset: 0x0001536D
		private MapConversationTableauData()
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00017178 File Offset: 0x00015378
		public static MapConversationTableauData CreateFrom(ConversationCharacterData playerCharacterData, ConversationCharacterData conversationPartnerData, TerrainType terrainType, float timeOfDay, bool isCurrentTerrainUnderSnow, Settlement settlement, string locationId, bool isRaining, bool isSnowing)
		{
			return new MapConversationTableauData
			{
				PlayerCharacterData = playerCharacterData,
				ConversationPartnerData = conversationPartnerData,
				ConversationTerrainType = terrainType,
				TimeOfDay = timeOfDay,
				IsCurrentTerrainUnderSnow = isCurrentTerrainUnderSnow,
				Settlement = settlement,
				LocationId = locationId,
				IsRaining = isRaining,
				IsSnowing = isSnowing
			};
		}
	}
}
