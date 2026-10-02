using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Nameplate;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003D RID: 61
	[OverrideView(typeof(MapPartyNameplateView))]
	public class GauntletMapPartyNameplateView : MapView
	{
		// Token: 0x060002E1 RID: 737 RVA: 0x00011344 File Offset: 0x0000F544
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new PartyNameplatesVM(base.MapScreen.MapCameraView.Camera, new Action(base.MapScreen.FastMoveCameraToMainParty));
			GauntletMapBasicView mapView = base.MapScreen.GetMapView<GauntletMapBasicView>();
			base.Layer = mapView.GauntletNameplateLayer;
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("PartyNameplate", this._dataSource);
			this._dataSource.Initialize();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000113D4 File Offset: 0x0000F5D4
		protected override void OnMapScreenUpdate(float dt)
		{
			base.OnMapScreenUpdate(dt);
			this._dataSource.Update();
			bool flag = base.MapScreen.SceneLayer.Input.IsGameKeyDown(5);
			EncounterModel encounterModel = Campaign.Current.Models.EncounterModel;
			for (int i = 0; i < this._dataSource.Nameplates.Count; i++)
			{
				PartyNameplateVM partyNameplateVM = this._dataSource.Nameplates[i];
				partyNameplateVM.ShouldShowFullName = flag;
				TextObject textObject;
				partyNameplateVM.CanParley = partyNameplateVM.ShouldShowFullName && encounterModel.CanMainHeroDoParleyWithParty(partyNameplateVM.Party.Party, out textObject);
			}
			if (this._dataSource.PlayerNameplate != null)
			{
				this._dataSource.PlayerNameplate.ShouldShowFullName = flag;
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00011490 File Offset: 0x0000F690
		protected override void OnResume()
		{
			base.OnResume();
			foreach (PartyNameplateVM partyNameplateVM in this._dataSource.Nameplates)
			{
				partyNameplateVM.RefreshDynamicProperties(true);
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000114E8 File Offset: 0x0000F6E8
		protected override void OnFinalize()
		{
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._dataSource.OnFinalize();
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			this._dataSource = null;
			base.OnFinalize();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00011528 File Offset: 0x0000F728
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00011544 File Offset: 0x0000F744
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x0400011B RID: 283
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x0400011C RID: 284
		private PartyNameplatesVM _dataSource;

		// Token: 0x0400011D RID: 285
		private GauntletMovieIdentifier _movie;
	}
}
