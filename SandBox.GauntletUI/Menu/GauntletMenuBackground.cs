using System;
using SandBox.View.Menu;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Menu
{
	// Token: 0x02000024 RID: 36
	[OverrideView(typeof(MenuBackgroundView))]
	public class GauntletMenuBackground : MenuView
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x0000C464 File Offset: 0x0000A664
		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._layerAsGauntletLayer = base.MenuViewContext.FindLayer<GauntletLayer>("MapMenuView");
			if (this._layerAsGauntletLayer == null)
			{
				this._layerAsGauntletLayer = new GauntletLayer("MapMenuView", 100, false);
				base.MenuViewContext.AddLayer(this._layerAsGauntletLayer);
			}
			base.Layer = this._layerAsGauntletLayer;
			this._movie = this._layerAsGauntletLayer.LoadMovie("GameMenuBackground", null);
			this._layerAsGauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000C4EE File Offset: 0x0000A6EE
		protected override void OnFinalize()
		{
			GauntletLayer layerAsGauntletLayer = this._layerAsGauntletLayer;
			if (layerAsGauntletLayer != null)
			{
				layerAsGauntletLayer.ReleaseMovie(this._movie);
			}
			this._layerAsGauntletLayer = null;
			base.Layer = null;
			this._movie = null;
			base.OnFinalize();
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000C522 File Offset: 0x0000A722
		protected override void OnMapConversationActivated()
		{
			base.OnMapConversationActivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000C53E File Offset: 0x0000A73E
		protected override void OnMapConversationDeactivated()
		{
			base.OnMapConversationDeactivated();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x0400009E RID: 158
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x0400009F RID: 159
		private GauntletMovieIdentifier _movie;
	}
}
