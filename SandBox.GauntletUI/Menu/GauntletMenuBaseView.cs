using System;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x02000025 RID: 37
	[OverrideView(typeof(MenuBaseView))]
	public class GauntletMenuBaseView : MenuView
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000C562 File Offset: 0x0000A762
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000C56A File Offset: 0x0000A76A
		public GameMenuVM GameMenuDataSource { get; private set; }

		// Token: 0x060001EA RID: 490 RVA: 0x0000C574 File Offset: 0x0000A774
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this.GameMenuDataSource = new GameMenuVM(base.MenuContext);
			GameKey gameKey = HotKeyManager.GetCategory("Generic").GetGameKey(4);
			this.GameMenuDataSource.SetLeaveHotKey(gameKey);
			base.Layer = base.MenuViewContext.FindLayer<GauntletLayer>("MapMenuView");
			if (base.Layer == null)
			{
				base.Layer = new GauntletLayer("MapMenuView", 100, false);
				base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				base.MenuViewContext.AddLayer(base.Layer);
			}
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("GameMenu", this.GameMenuDataSource);
			ScreenManager.TrySetFocus(base.Layer);
			this._layerAsGauntletLayer.UIContext.ContextAlpha = 1f;
			MBInformationManager.HideInformations();
			this.GainGamepadNavigationAfterSeconds(0.25f);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000C666 File Offset: 0x0000A866
		protected override void OnActivate()
		{
			base.OnActivate();
			this.GameMenuDataSource.Refresh(true);
			this.GameMenuDataSource.SetIdleMode(false);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000C686 File Offset: 0x0000A886
		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			this.GameMenuDataSource.SetIdleMode(true);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000C69A File Offset: 0x0000A89A
		protected override void OnResume()
		{
			base.OnResume();
			this.GameMenuDataSource.Refresh(true);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000C6AE File Offset: 0x0000A8AE
		protected override void OnMenuContextRefreshed()
		{
			base.OnMenuContextRefreshed();
			this.GameMenuDataSource.Refresh(true);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
		protected override void OnFinalize()
		{
			this.GameMenuDataSource.OnFinalize();
			this.GameMenuDataSource = null;
			ScreenManager.TryLoseFocus(base.Layer);
			this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			base.OnFinalize();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000C71A File Offset: 0x0000A91A
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.GameMenuDataSource.OnFrameTick();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000C72E File Offset: 0x0000A92E
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000C74A File Offset: 0x0000A94A
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000C766 File Offset: 0x0000A966
		protected override void OnMenuContextUpdated(MenuContext newMenuContext)
		{
			base.OnMenuContextUpdated(newMenuContext);
			this.GameMenuDataSource.UpdateMenuContext(newMenuContext);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C77B File Offset: 0x0000A97B
		protected override void OnBackgroundMeshNameSet(string name)
		{
			base.OnBackgroundMeshNameSet(name);
			this.GameMenuDataSource.Background = name;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000C790 File Offset: 0x0000A990
		private void GainGamepadNavigationAfterSeconds(float seconds)
		{
			this._layerAsGauntletLayer.UIContext.GamepadNavigation.GainNavigationAfterTime(seconds, () => this.GameMenuDataSource.ItemList.Count > 0);
		}

		// Token: 0x040000A1 RID: 161
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000A2 RID: 162
		private GauntletMovieIdentifier _movie;
	}
}
