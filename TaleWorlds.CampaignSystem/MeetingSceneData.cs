using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008C RID: 140
	public struct MeetingSceneData
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x0005361D File Offset: 0x0005181D
		// (set) Token: 0x06001238 RID: 4664 RVA: 0x00053625 File Offset: 0x00051825
		public string SceneID { get; private set; }

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001239 RID: 4665 RVA: 0x0005362E File Offset: 0x0005182E
		// (set) Token: 0x0600123A RID: 4666 RVA: 0x00053636 File Offset: 0x00051836
		public string CultureString { get; private set; }

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600123B RID: 4667 RVA: 0x0005363F File Offset: 0x0005183F
		public CultureObject Culture
		{
			get
			{
				return MBObjectManager.Instance.GetObject<CultureObject>(this.CultureString);
			}
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00053651 File Offset: 0x00051851
		public MeetingSceneData(string sceneID, string cultureString)
		{
			this.SceneID = sceneID;
			this.CultureString = cultureString;
		}
	}
}
