using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Siege
{
	// Token: 0x0200011A RID: 282
	public class MapSiegeConstructionControllerWidget : Widget
	{
		// Token: 0x06000EEB RID: 3819 RVA: 0x000292C4 File Offset: 0x000274C4
		public MapSiegeConstructionControllerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000292D0 File Offset: 0x000274D0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num;
			if (this._currentWidget != null)
			{
				base.PositionXOffset = MathF.Clamp(this._currentWidget.PositionXOffset + this._currentWidget.Size.X * base._inverseScaleToUse, 0f, base.EventManager.PageSize.X - base.Size.X);
				base.PositionYOffset = MathF.Clamp(this._currentWidget.PositionYOffset, 175f, base.EventManager.PageSize.Y - base.Size.Y - 70f);
				num = this._currentWidget.ReadOnlyBrush.GlobalAlphaFactor;
			}
			else
			{
				base.PositionXOffset = -1000f;
				base.PositionYOffset = -1000f;
				num = 0f;
			}
			base.IsEnabled = num >= 0.95f;
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000293C2 File Offset: 0x000275C2
		public void SetCurrentPOIWidget(MapSiegePOIBrushWidget widget)
		{
			if (widget == null || widget == this._currentWidget)
			{
				this._currentWidget = null;
				return;
			}
			this._currentWidget = (widget.IsPlayerSidePOI ? widget : null);
		}

		// Token: 0x040006CB RID: 1739
		private MapSiegePOIBrushWidget _currentWidget;
	}
}
