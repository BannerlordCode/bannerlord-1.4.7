using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Map.Tracker;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.Tracker;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000042 RID: 66
	[OverrideView(typeof(MapTrackersView))]
	public class GauntletMapTrackersView : MapTrackersView
	{
		// Token: 0x0600030B RID: 779 RVA: 0x00011E54 File Offset: 0x00010054
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new MapTrackerCollectionVM();
			MapTrackerItemVM.OnFastMoveCameraToPosition = new Action<CampaignVec2>(this.FastMoveCameraToPosition);
			GauntletMapBasicView mapView = base.MapScreen.GetMapView<GauntletMapBasicView>();
			base.Layer = mapView.GauntletNameplateLayer;
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("MapTrackers", this._dataSource);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00011EC8 File Offset: 0x000100C8
		protected override void OnResume()
		{
			base.OnResume();
			this._dataSource.UpdateProperties();
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00011EDC File Offset: 0x000100DC
		private void UpdateTrackerPropertiesAux(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				MapTrackerItemVM mapTrackerItemVM = this._dataSource.Trackers[i];
				mapTrackerItemVM.UpdateProperties();
				float num;
				float num2;
				float num3;
				this.GetScreenPosition(mapTrackerItemVM.TrackedObject, out num, out num2, out num3);
				mapTrackerItemVM.UpdatePosition(num, num2, num3);
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00011F29 File Offset: 0x00010129
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			TWParallel.For(0, this._dataSource.Trackers.Count, new TWParallel.ParallelForAuxPredicate(this.UpdateTrackerPropertiesAux), 32);
			this._dataSource.Tick(dt);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00011F64 File Offset: 0x00010164
		protected override void OnFinalize()
		{
			MapTrackerItemVM.OnFastMoveCameraToPosition = null;
			this._dataSource.OnFinalize();
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			this._dataSource = null;
			base.OnFinalize();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00011FB5 File Offset: 0x000101B5
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00011FD1 File Offset: 0x000101D1
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00011FF0 File Offset: 0x000101F0
		private void GetScreenPosition(ITrackableCampaignObject trackable, out float screenX, out float screenY, out float screenW)
		{
			float num = 0f;
			Vec3 position = trackable.GetPosition();
			IMapScene mapSceneWrapper = Campaign.Current.MapSceneWrapper;
			CampaignVec2 campaignVec = new CampaignVec2(position.AsVec2, true);
			mapSceneWrapper.GetHeightAtPoint(in campaignVec, ref num);
			position.z = MathF.Max(num, 0f);
			screenX = -5000f;
			screenY = -5000f;
			screenW = -1f;
			MBWindowManager.WorldToScreenInsideUsableArea(base.MapScreen.MapCameraView.Camera, position, ref screenX, ref screenY, ref screenW);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0001206F File Offset: 0x0001026F
		private void FastMoveCameraToPosition(CampaignVec2 target)
		{
			base.MapScreen.FastMoveCameraToPosition(target);
		}

		// Token: 0x04000128 RID: 296
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000129 RID: 297
		private GauntletMovieIdentifier _movie;

		// Token: 0x0400012A RID: 298
		private MapTrackerCollectionVM _dataSource;
	}
}
