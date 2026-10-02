using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.NameMarker
{
	// Token: 0x020000F3 RID: 243
	public class AlwaysVisibleNameMarkerListPanel : ListPanel
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00021FE7 File Offset: 0x000201E7
		private float _normalOpacity
		{
			get
			{
				return 0.5f;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00021FEE File Offset: 0x000201EE
		private float _screenCenterOpacity
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x00021FF5 File Offset: 0x000201F5
		private float _stayOnScreenTimeInSeconds
		{
			get
			{
				return 5f;
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00021FFC File Offset: 0x000201FC
		public AlwaysVisibleNameMarkerListPanel(UIContext context)
			: base(context)
		{
			this._parentScreenWidget = base.EventManager.Root.GetChild(0).GetChild(0);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00022024 File Offset: 0x00020224
		protected override void OnLateUpdate(float dt)
		{
			base.ApplyActionToAllChildrenRecursive(delegate(Widget child)
			{
				child.IsVisible = true;
			});
			base.ScaledPositionYOffset = this.Position.y - base.Size.Y / 2f;
			base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
			this.UpdateOpacity();
			if (this._totalDt > this._stayOnScreenTimeInSeconds)
			{
				base.EventFired("Remove", Array.Empty<object>());
			}
			this._totalDt += dt;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000220D0 File Offset: 0x000202D0
		private void UpdateOpacity()
		{
			Vec2 vec = new Vec2(base.Context.TwoDimensionContext.Platform.Width / 2f, base.Context.TwoDimensionContext.Platform.Height / 2f);
			Vec2 vec2 = new Vec2(base.ScaledPositionXOffset, base.ScaledPositionYOffset);
			float num = ((vec2.Distance(vec) <= 150f) ? this._screenCenterOpacity : this._normalOpacity);
			this.SetGlobalAlphaRecursively(num);
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00022157 File Offset: 0x00020357
		// (set) Token: 0x06000C7E RID: 3198 RVA: 0x0002215F File Offset: 0x0002035F
		[DataSourceProperty]
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x040005A1 RID: 1441
		private Widget _parentScreenWidget;

		// Token: 0x040005A2 RID: 1442
		private float _totalDt;

		// Token: 0x040005A3 RID: 1443
		private Vec2 _position;
	}
}
