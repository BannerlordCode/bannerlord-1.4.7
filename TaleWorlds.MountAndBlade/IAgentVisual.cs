using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C8 RID: 712
	public interface IAgentVisual
	{
		// Token: 0x06002920 RID: 10528
		void SetAction(in ActionIndexCache actionName, float startProgress = 0f, bool forceFaceMorphRestart = true);

		// Token: 0x06002921 RID: 10529
		MBAgentVisuals GetVisuals();

		// Token: 0x06002922 RID: 10530
		MatrixFrame GetFrame();

		// Token: 0x06002923 RID: 10531
		BodyProperties GetBodyProperties();

		// Token: 0x06002924 RID: 10532
		void SetBodyProperties(BodyProperties bodyProperties);

		// Token: 0x06002925 RID: 10533
		bool GetIsFemale();

		// Token: 0x06002926 RID: 10534
		string GetCharacterObjectID();

		// Token: 0x06002927 RID: 10535
		void SetCharacterObjectID(string id);

		// Token: 0x06002928 RID: 10536
		Equipment GetEquipment();

		// Token: 0x06002929 RID: 10537
		void SetClothingColors(uint color1, uint color2);

		// Token: 0x0600292A RID: 10538
		void GetClothingColors(out uint color1, out uint color2);

		// Token: 0x0600292B RID: 10539
		AgentVisualsData GetCopyAgentVisualsData();

		// Token: 0x0600292C RID: 10540
		void Refresh(bool needBatchedVersionForWeaponMeshes, AgentVisualsData data, bool forceUseFaceCache = false);
	}
}
