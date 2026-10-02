using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x02000117 RID: 279
	public class MapAnchorTrackerWidget : Widget
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x00028B86 File Offset: 0x00026D86
		public MapAnchorTrackerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00028B90 File Offset: 0x00026D90
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsRecursivelyVisible())
			{
				float num = this.PositionX - base.Size.X * 0.5f;
				float num2 = this.PositionY - base.Size.Y * 0.5f;
				if (num + base.Size.X >= 0f && num <= base.EventManager.PageSize.X && num2 + base.Size.Y >= 0f && num2 <= base.EventManager.PageSize.Y)
				{
					base.ScaledPositionXOffset = MathF.Clamp(num, 0f, base.EventManager.PageSize.X - base.Size.X);
					base.ScaledPositionYOffset = MathF.Clamp(num2, 0f, base.EventManager.PageSize.Y - base.Size.Y - 50f * base._scaleToUse);
					return;
				}
				base.ScaledPositionXOffset = -5000f;
				base.ScaledPositionYOffset = -5000f;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00028CB3 File Offset: 0x00026EB3
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x00028CBB File Offset: 0x00026EBB
		[Editor(false)]
		public float PositionX
		{
			get
			{
				return this._positionX;
			}
			set
			{
				if (value != this._positionX)
				{
					this._positionX = value;
					base.OnPropertyChanged(value, "PositionX");
				}
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00028CD9 File Offset: 0x00026ED9
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x00028CE1 File Offset: 0x00026EE1
		[Editor(false)]
		public float PositionY
		{
			get
			{
				return this._positionY;
			}
			set
			{
				if (value != this._positionY)
				{
					this._positionY = value;
					base.OnPropertyChanged(value, "PositionY");
				}
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00028CFF File Offset: 0x00026EFF
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x00028D07 File Offset: 0x00026F07
		[Editor(false)]
		public float PositionW
		{
			get
			{
				return this._positionW;
			}
			set
			{
				if (value != this._positionW)
				{
					this._positionW = value;
					base.OnPropertyChanged(value, "PositionW");
				}
			}
		}

		// Token: 0x040006BD RID: 1725
		private float _positionX;

		// Token: 0x040006BE RID: 1726
		private float _positionY;

		// Token: 0x040006BF RID: 1727
		private float _positionW;
	}
}
