using System;
using System.Collections.Generic;
using SandBox.View.Map;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000034 RID: 52
	[OverrideView(typeof(MapEscapeMenuView))]
	public class GauntletMapEscapeMenuView : MapView
	{
		// Token: 0x06000284 RID: 644 RVA: 0x0000F837 File Offset: 0x0000DA37
		public GauntletMapEscapeMenuView(List<EscapeMenuItemVM> items)
		{
			this._menuItems = items;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000F848 File Offset: 0x0000DA48
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._escapeMenuDatasource = new EscapeMenuVM(this._menuItems, null);
			base.Layer = new GauntletLayer("MapEscapeMenu", 4400, false)
			{
				IsFocusLayer = true
			};
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._escapeMenuMovie = this._layerAsGauntletLayer.LoadMovie("EscapeMenu", this._escapeMenuDatasource);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MapScreen.AddLayer(base.Layer);
			base.MapScreen.PauseAmbientSounds();
			ScreenManager.TrySetFocus(base.Layer);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000F90C File Offset: 0x0000DB0C
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (base.Layer.Input.IsHotKeyReleased("ToggleEscapeMenu") || base.Layer.Input.IsHotKeyReleased("Exit"))
			{
				MapScreen.Instance.CloseEscapeMenu();
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000F958 File Offset: 0x0000DB58
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			if (base.Layer.Input.IsHotKeyReleased("ToggleEscapeMenu") || base.Layer.Input.IsHotKeyReleased("Exit"))
			{
				MapScreen.Instance.CloseEscapeMenu();
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		protected override bool IsEscaped()
		{
			return base.Layer.Input.IsHotKeyReleased("ToggleEscapeMenu");
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000F9BC File Offset: 0x0000DBBC
		protected override void OnFinalize()
		{
			base.OnFinalize();
			base.Layer.InputRestrictions.ResetInputRestrictions();
			base.MapScreen.RemoveLayer(base.Layer);
			base.MapScreen.RestartAmbientSounds();
			ScreenManager.TryLoseFocus(base.Layer);
			base.Layer = null;
			this._layerAsGauntletLayer = null;
			this._escapeMenuDatasource = null;
			this._escapeMenuMovie = null;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000FA22 File Offset: 0x0000DC22
		protected override TutorialContexts GetTutorialContext()
		{
			return TutorialContexts.EscapeMenu;
		}

		// Token: 0x040000E6 RID: 230
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x040000E7 RID: 231
		private EscapeMenuVM _escapeMenuDatasource;

		// Token: 0x040000E8 RID: 232
		private GauntletMovieIdentifier _escapeMenuMovie;

		// Token: 0x040000E9 RID: 233
		private readonly List<EscapeMenuItemVM> _menuItems;
	}
}
