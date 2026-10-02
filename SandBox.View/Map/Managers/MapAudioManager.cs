using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000077 RID: 119
	internal class MapAudioManager : CampaignEntityVisualComponent
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00027B6F File Offset: 0x00025D6F
		public override int Priority
		{
			get
			{
				return 70;
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00027B73 File Offset: 0x00025D73
		public MapAudioManager()
		{
			this._mapScene = Campaign.Current.MapSceneWrapper as MapScene;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00027B90 File Offset: 0x00025D90
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			if (CampaignTime.Now.GetSeasonOfYear != this._lastCachedSeason)
			{
				SoundManager.SetGlobalParameter("Season", (float)CampaignTime.Now.GetSeasonOfYear);
				this._lastCachedSeason = CampaignTime.Now.GetSeasonOfYear;
			}
			if (Math.Abs(this._lastCameraZ - this._mapScene.Scene.LastFinalRenderCameraPosition.Z) > 0.1f)
			{
				SoundManager.SetGlobalParameter("CampaignCameraHeight", this._mapScene.Scene.LastFinalRenderCameraPosition.Z);
				this._lastCameraZ = this._mapScene.Scene.LastFinalRenderCameraPosition.Z;
			}
			if ((int)CampaignTime.Now.CurrentHourInDay == this._lastHourUpdate)
			{
				SoundManager.SetGlobalParameter("Daytime", CampaignTime.Now.CurrentHourInDay);
				this._lastHourUpdate = (int)CampaignTime.Now.CurrentHourInDay;
			}
		}

		// Token: 0x04000264 RID: 612
		private const string SeasonParameterId = "Season";

		// Token: 0x04000265 RID: 613
		private const string CameraHeightParameterId = "CampaignCameraHeight";

		// Token: 0x04000266 RID: 614
		private const string TimeOfDayParameterId = "Daytime";

		// Token: 0x04000267 RID: 615
		private const string WeatherEventIntensityParameterId = "Rainfall";

		// Token: 0x04000268 RID: 616
		private CampaignTime.Seasons _lastCachedSeason;

		// Token: 0x04000269 RID: 617
		private float _lastCameraZ;

		// Token: 0x0400026A RID: 618
		private int _lastHourUpdate;

		// Token: 0x0400026B RID: 619
		private MapScene _mapScene;
	}
}
