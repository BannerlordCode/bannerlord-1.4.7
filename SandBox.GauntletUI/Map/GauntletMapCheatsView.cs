using System;
using System.Collections.Generic;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Map.Cheat;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000031 RID: 49
	[OverrideView(typeof(MapCheatsView))]
	public class GauntletMapCheatsView : MapCheatsView
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000EAD0 File Offset: 0x0000CCD0
		protected override void CreateLayout()
		{
			base.CreateLayout();
			IEnumerable<GameplayCheatBase> mapCheatList = GameplayCheatsManager.GetMapCheatList();
			this._dataSource = new GameplayCheatsVM(new Action(this.HandleClose), mapCheatList);
			this.InitializeKeyVisuals();
			base.Layer = new GauntletLayer("MapCheats", 4500, false);
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._layerAsGauntletLayer.LoadMovie("MapCheats", this._dataSource);
			this._layerAsGauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			this._layerAsGauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._layerAsGauntletLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._layerAsGauntletLayer);
			base.MapScreen.AddLayer(this._layerAsGauntletLayer);
			base.MapScreen.SetIsMapCheatsActive(true);
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.SetTimeControlModeLock(true);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000EBBC File Offset: 0x0000CDBC
		protected override void OnFinalize()
		{
			base.OnFinalize();
			base.MapScreen.RemoveLayer(base.Layer);
			GameplayCheatsVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._dataSource = null;
			base.MapScreen.SetIsMapCheatsActive(false);
			Campaign.Current.SetTimeControlModeLock(false);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000EC1D File Offset: 0x0000CE1D
		private void HandleClose()
		{
			base.MapScreen.CloseGameplayCheats();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000EC2A File Offset: 0x0000CE2A
		protected override bool IsEscaped()
		{
			return true;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000EC2D File Offset: 0x0000CE2D
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			this.HandleInput();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		protected override void OnMenuModeTick(float dt)
		{
			base.OnMenuModeTick(dt);
			this.HandleInput();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000EC4B File Offset: 0x0000CE4B
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.HandleInput();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000EC5A File Offset: 0x0000CE5A
		private void HandleInput()
		{
			if (base.Layer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				GameplayCheatsVM dataSource = this._dataSource;
				if (dataSource == null)
				{
					return;
				}
				dataSource.ExecuteClose();
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000EC8D File Offset: 0x0000CE8D
		private void InitializeKeyVisuals()
		{
			this._dataSource.SetCloseInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000ECAE File Offset: 0x0000CEAE
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000ECCA File Offset: 0x0000CECA
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000D2 RID: 210
		protected GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000D3 RID: 211
		protected GameplayCheatsVM _dataSource;
	}
}
