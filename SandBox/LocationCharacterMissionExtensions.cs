using System;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x02000020 RID: 32
	public static class LocationCharacterMissionExtensions
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00006132 File Offset: 0x00004332
		public static AgentBuildData GetAgentBuildData(this LocationCharacter locationCharacter)
		{
			return new AgentBuildData(locationCharacter.AgentData);
		}
	}
}
