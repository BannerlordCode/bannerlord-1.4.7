using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D3 RID: 211
	public class AgentAlarmStateWidget : Widget
	{
		// Token: 0x06000AD4 RID: 2772 RVA: 0x0001E47C File Offset: 0x0001C67C
		public AgentAlarmStateWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0001E485 File Offset: 0x0001C685
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.UpdatePosition();
			this.UpdateVisuals();
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0001E49A File Offset: 0x0001C69A
		private void UpdateVisuals()
		{
			if (this.AlarmState != null)
			{
				this.SetState(this.AlarmState);
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0001E4B0 File Offset: 0x0001C6B0
		private void UpdatePosition()
		{
			float num = this.Position.X - base.Size.X / 2f;
			float num2 = this.Position.X + base.Size.X / 2f;
			float num3 = this.Position.Y - base.Size.Y / 2f;
			float num4 = this.Position.Y + base.Size.Y / 2f;
			bool flag = this.WSign > 0 && num > 0f && num2 < base.Context.EventManager.PageSize.X && num3 > 0f && num4 < base.Context.EventManager.PageSize.Y;
			bool flag2 = this.WSign > 0 && (num2 > 0f || num < base.Context.EventManager.PageSize.X) && (num4 > 0f || num3 < base.Context.EventManager.PageSize.Y);
			if (!flag)
			{
				Vec2 vec = new Vec2(num, num3);
				Vector2 vector = base.Context.EventManager.PageSize - base.Size;
				Vec2 vec2 = vector / 2f;
				vec -= vec2;
				if (this.WSign < 0)
				{
					vec *= -1f;
				}
				float num5 = Mathf.Atan2(vec.y, vec.x) - 1.5707964f;
				float num6 = Mathf.Cos(num5);
				float num7 = Mathf.Sin(num5);
				float num8 = num6 / num7;
				Vec2 vec3 = vec2 * 1f;
				vec = ((num6 > 0f) ? new Vec2(-vec3.y / num8, vec2.y) : new Vec2(vec3.y / num8, -vec2.y));
				if (vec.x > vec3.x)
				{
					vec = new Vec2(vec3.x, -vec3.x * num8);
				}
				else if (vec.x < -vec3.x)
				{
					vec = new Vec2(-vec3.x, vec3.x * num8);
				}
				vec += vec2;
				base.ScaledPositionXOffset = Mathf.Clamp(vec.x, 0f, vector.X);
				base.ScaledPositionYOffset = Mathf.Clamp(vec.y, 0f, vector.Y);
				return;
			}
			if (flag || flag2)
			{
				base.ScaledPositionXOffset = num;
				base.ScaledPositionYOffset = num3;
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x0001E770 File Offset: 0x0001C970
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x0001E778 File Offset: 0x0001C978
		public string AlarmState
		{
			get
			{
				return this._alarmState;
			}
			set
			{
				if (this._alarmState != value)
				{
					this._alarmState = value;
					base.OnPropertyChanged<string>(value, "AlarmState");
				}
			}
		}

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x0001E79B File Offset: 0x0001C99B
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x0001E7A3 File Offset: 0x0001C9A3
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (value != this._wSign)
				{
					this._wSign = value;
					base.OnPropertyChanged(value, "WSign");
				}
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x0001E7C1 File Offset: 0x0001C9C1
		// (set) Token: 0x06000ADD RID: 2781 RVA: 0x0001E7C9 File Offset: 0x0001C9C9
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value != this._position)
				{
					this._position = value;
					base.OnPropertyChanged(value, "Position");
				}
			}
		}

		// Token: 0x040004EA RID: 1258
		private string _alarmState;

		// Token: 0x040004EB RID: 1259
		private int _wSign;

		// Token: 0x040004EC RID: 1260
		private Vec2 _position;
	}
}
