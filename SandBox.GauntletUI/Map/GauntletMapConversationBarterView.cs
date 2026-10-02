using System;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Barter;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000032 RID: 50
	public class GauntletMapConversationBarterView
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000ECE6 File Offset: 0x0000CEE6
		// (set) Token: 0x06000262 RID: 610 RVA: 0x0000ECEE File Offset: 0x0000CEEE
		public bool IsCreated { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000ECF7 File Offset: 0x0000CEF7
		// (set) Token: 0x06000264 RID: 612 RVA: 0x0000ECFF File Offset: 0x0000CEFF
		public bool IsActive { get; private set; }

		// Token: 0x06000265 RID: 613 RVA: 0x0000ED08 File Offset: 0x0000CF08
		public GauntletMapConversationBarterView(GauntletLayer layer, GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged)
		{
			this._gauntletLayer = layer;
			this._onActiveStateChanged = onActiveStateChanged;
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000ED20 File Offset: 0x0000CF20
		public void CreateBarterView(BarterData args)
		{
			this._barterDataSource = new BarterVM(args);
			this._barterDataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._barterDataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._barterDataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged = this._onActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged(true);
			}
			this._barterCategory = UIResourceManager.GetSpriteCategory("ui_barter");
			this.Activate();
			this.IsCreated = true;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000EDC8 File Offset: 0x0000CFC8
		public void DestroyBarterView()
		{
			this.Deactivate();
			this._barterDataSource.OnFinalize();
			this._barterDataSource = null;
			this._barterCategory = null;
			GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged = this._onActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged(false);
			}
			BarterItemVM.IsFiveStackModifierActive = false;
			BarterItemVM.IsEntireStackModifierActive = false;
			this.IsCreated = false;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000EE1C File Offset: 0x0000D01C
		public void Activate()
		{
			this._barterMovie = this._gauntletLayer.LoadMovie("BarterScreen", this._barterDataSource);
			this._barterCategory.Load();
			GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged = this._onActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged(true);
			}
			this.IsActive = true;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000EE69 File Offset: 0x0000D069
		public void Deactivate()
		{
			this._gauntletLayer.ReleaseMovie(this._barterMovie);
			this._barterCategory.Unload();
			this.IsActive = false;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000EE90 File Offset: 0x0000D090
		public void TickInput()
		{
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._barterDataSource.ExecuteCancel();
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
			{
				BarterVM barterDataSource = this._barterDataSource;
				if (barterDataSource != null && !barterDataSource.IsOfferDisabled)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._barterDataSource.ExecuteOffer();
					return;
				}
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Reset"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._barterDataSource.ExecuteReset();
			}
		}

		// Token: 0x040000D6 RID: 214
		private readonly GauntletLayer _gauntletLayer;

		// Token: 0x040000D7 RID: 215
		private readonly GauntletMapConversationBarterView.OnBarterActiveStateChanged _onActiveStateChanged;

		// Token: 0x040000D8 RID: 216
		private SpriteCategory _barterCategory;

		// Token: 0x040000D9 RID: 217
		private BarterVM _barterDataSource;

		// Token: 0x040000DA RID: 218
		private GauntletMovieIdentifier _barterMovie;

		// Token: 0x0200007F RID: 127
		// (Invoke) Token: 0x06000452 RID: 1106
		public delegate void OnBarterActiveStateChanged(bool isBarterActive);
	}
}
