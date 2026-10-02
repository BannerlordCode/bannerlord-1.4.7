using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000023 RID: 35
	public class SimpleSceneTestWithMission
	{
		// Token: 0x060000EE RID: 238 RVA: 0x000077E8 File Offset: 0x000059E8
		public SimpleSceneTestWithMission(string sceneName, DecalAtlasGroup atlasGroup = DecalAtlasGroup.All)
		{
			this._sceneName = sceneName;
			this._customDecalGroup = atlasGroup;
			this._mission = this.OpenSceneWithMission(this._sceneName, "");
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00007815 File Offset: 0x00005A15
		public bool LoadingFinished()
		{
			return this._mission.IsLoadingFinished && Utilities.GetNumberOfShaderCompilationsInProgress() == 0;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00007830 File Offset: 0x00005A30
		private Mission OpenSceneWithMission(string scene, string sceneLevels = "")
		{
			LoadingWindow.DisableGlobalLoadingWindow();
			return MissionState.OpenNew("SimpleSceneTestWithMission", new MissionInitializerRecord(scene)
			{
				PlayingInCampaignMode = false,
				AtmosphereOnCampaign = AtmosphereInfo.GetInvalidAtmosphereInfo(),
				DecalAtlasGroup = (int)this._customDecalGroup,
				SceneLevels = sceneLevels
			}, (Mission missionController) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new BasicLeaveMissionLogic(false, 0),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(10f),
				new EquipmentControllerLeaveLogic()
			}, true, true);
		}

		// Token: 0x04000046 RID: 70
		private Mission _mission;

		// Token: 0x04000047 RID: 71
		private string _sceneName;

		// Token: 0x04000048 RID: 72
		private DecalAtlasGroup _customDecalGroup;
	}
}
