using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapEvents
{
	// Token: 0x02000125 RID: 293
	public class MapEventVisualItemWidget : Widget
	{
		// Token: 0x06000F76 RID: 3958 RVA: 0x0002ACA2 File Offset: 0x00028EA2
		public MapEventVisualItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002ACAB File Offset: 0x00028EAB
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			this.UpdatePosition();
			this.UpdateVisibility();
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0002ACC0 File Offset: 0x00028EC0
		private void UpdateVisibility()
		{
			base.IsVisible = this.IsVisibleOnMap;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0002ACD0 File Offset: 0x00028ED0
		private void UpdatePosition()
		{
			if (this.IsVisibleOnMap)
			{
				base.ScaledPositionXOffset = this.Position.x - base.Size.X / 2f;
				base.ScaledPositionYOffset = this.Position.y - base.Size.Y;
				return;
			}
			base.ScaledPositionXOffset = -10000f;
			base.ScaledPositionYOffset = -10000f;
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0002AD3C File Offset: 0x00028F3C
		// (set) Token: 0x06000F7B RID: 3963 RVA: 0x0002AD44 File Offset: 0x00028F44
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

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x0002AD67 File Offset: 0x00028F67
		// (set) Token: 0x06000F7D RID: 3965 RVA: 0x0002AD6F File Offset: 0x00028F6F
		public bool IsVisibleOnMap
		{
			get
			{
				return this._isVisibleOnMap;
			}
			set
			{
				if (this._isVisibleOnMap != value)
				{
					this._isVisibleOnMap = value;
					base.OnPropertyChanged(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x0400070A RID: 1802
		private Vec2 _position;

		// Token: 0x0400070B RID: 1803
		private bool _isVisibleOnMap;
	}
}
