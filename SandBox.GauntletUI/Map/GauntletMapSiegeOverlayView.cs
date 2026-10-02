using System;
using SandBox.View.Map;
using SandBox.View.Map.Managers;
using SandBox.View.Map.Visuals;
using SandBox.ViewModelCollection.MapSiege;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000041 RID: 65
	[OverrideView(typeof(MapSiegeOverlayView))]
	public class GauntletMapSiegeOverlayView : MapView
	{
		// Token: 0x06000302 RID: 770 RVA: 0x00011C28 File Offset: 0x0000FE28
		protected override void CreateLayout()
		{
			base.CreateLayout();
			GauntletMapBasicView mapView = base.MapScreen.GetMapView<GauntletMapBasicView>();
			base.Layer = mapView.GauntletNameplateLayer;
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			SettlementVisual settlementVisual = SettlementVisualManager.Current.GetSettlementVisual(PlayerSiege.PlayerSiegeEvent.BesiegedSettlement);
			this._dataSource = new MapSiegeVM(base.MapScreen.MapCameraView.Camera, settlementVisual.GetAttackerBatteringRamSiegeEngineFrames(), settlementVisual.GetAttackerRangedSiegeEngineFrames(), settlementVisual.GetAttackerTowerSiegeEngineFrames(), settlementVisual.GetDefenderRangedSiegeEngineFrames(), settlementVisual.GetBreachableWallFrames());
			CampaignEvents.SiegeEngineBuiltEvent.AddNonSerializedListener(this, new Action<SiegeEvent, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineBuilt));
			this._movie = this._layerAsGauntletLayer.LoadMovie("MapSiegeOverlay", this._dataSource);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00011CE5 File Offset: 0x0000FEE5
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			MapSiegeVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.Update(base.MapScreen.MapCameraView.CameraDistance);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00011D0E File Offset: 0x0000FF0E
		protected override void OnFinalize()
		{
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._movie = null;
			this._dataSource = null;
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			CampaignEvents.SiegeEngineBuiltEvent.ClearListeners(this);
			base.OnFinalize();
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00011D4E File Offset: 0x0000FF4E
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00011D6A File Offset: 0x0000FF6A
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00011D88 File Offset: 0x0000FF88
		protected override void OnSiegeEngineClick(MatrixFrame siegeEngineFrame)
		{
			base.OnSiegeEngineClick(siegeEngineFrame);
			UISoundsHelper.PlayUISound("event:/ui/panels/siege/engine_click");
			MapSiegeVM dataSource = this._dataSource;
			if (dataSource != null && dataSource.ProductionController.IsEnabled && this._dataSource.ProductionController.LatestSelectedPOI.MapSceneLocationFrame.NearlyEquals(siegeEngineFrame, 1E-05f))
			{
				this._dataSource.ProductionController.ExecuteDisable();
				return;
			}
			MapSiegeVM dataSource2 = this._dataSource;
			if (dataSource2 != null)
			{
				dataSource2.OnSelectionFromScene(siegeEngineFrame);
			}
			base.MapState.OnSiegeEngineClick(siegeEngineFrame);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00011E13 File Offset: 0x00010013
		protected override void OnMapTerrainClick()
		{
			base.OnMapTerrainClick();
			MapSiegeVM dataSource = this._dataSource;
			if (dataSource == null)
			{
				return;
			}
			dataSource.ProductionController.ExecuteDisable();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00011E30 File Offset: 0x00010030
		private void OnSiegeEngineBuilt(SiegeEvent siegeEvent, BattleSideEnum side, SiegeEngineType siegeEngineType)
		{
			if (siegeEvent.IsPlayerSiegeEvent && side == PlayerSiege.PlayerSide)
			{
				UISoundsHelper.PlayUISound("event:/ui/panels/siege/engine_build_complete");
			}
		}

		// Token: 0x04000125 RID: 293
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000126 RID: 294
		private MapSiegeVM _dataSource;

		// Token: 0x04000127 RID: 295
		private GauntletMovieIdentifier _movie;
	}
}
