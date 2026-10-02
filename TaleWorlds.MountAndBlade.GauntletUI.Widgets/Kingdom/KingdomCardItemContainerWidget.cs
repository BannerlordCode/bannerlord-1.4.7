using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000131 RID: 305
	public class KingdomCardItemContainerWidget : Widget
	{
		// Token: 0x06000FEC RID: 4076 RVA: 0x0002BBEF File Offset: 0x00029DEF
		public KingdomCardItemContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0002BC0E File Offset: 0x00029E0E
		protected override void OnBeforeChildRemoved(Widget child)
		{
			base.OnBeforeChildRemoved(child);
			child.EventFire -= this.ChildrenWidgetEventFired;
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0002BC29 File Offset: 0x00029E29
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.EventFire += this.ChildrenWidgetEventFired;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0002BC44 File Offset: 0x00029E44
		private void ChildrenWidgetEventFired(Widget widget, string eventName, object[] args)
		{
			if (eventName == "HoverBegin")
			{
				this._isMouseOverChildren = true;
				widget.RenderLate = true;
				return;
			}
			if (eventName == "HoverEnd")
			{
				this._isMouseOverChildren = false;
				widget.RenderLate = false;
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x0002BC80 File Offset: 0x00029E80
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			float num = 0f;
			float num2 = 0f;
			if (base.ChildCount > 0)
			{
				num = base.GetChild(0).Size.X * (float)base.ChildCount;
				num2 = this._defaultXOffset * base._inverseScaleToUse * (float)(base.ChildCount - 1) + base.GetChild(0).Size.X;
				base.IsEnabled = true;
			}
			else
			{
				base.IsEnabled = false;
			}
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				if (this._isMouseOverChildren || this._isMouseOverSelf)
				{
					if (base.ChildCount > 1)
					{
						if (num < base.Size.X)
						{
							float num3 = base.Size.X / 2f - num / 2f;
							this._targetXOffset = (float)i * child.Size.X + num3;
						}
						else
						{
							this._targetXOffset = (float)i / ((float)base.ChildCount - 1f) * (base.Size.X - child.Size.X);
						}
					}
					else if (base.ChildCount == 1)
					{
						this._targetXOffset = base.Size.X / 2f - child.Size.X / 2f;
					}
				}
				else if (base.ChildCount > 1)
				{
					float num4 = this._defaultXOffset;
					while (num2 > base.Size.X && num4 > 5f)
					{
						num4 -= 0.5f;
						num2 = num4 * (float)(base.ChildCount - 1) + child.Size.X;
					}
					this._targetXOffset = base.Size.X / 2f - num2 / 2f + num4 * (float)i;
				}
				else if (base.ChildCount == 1)
				{
					this._targetXOffset = base.Size.X / 2f - child.Size.X / 2f;
				}
				child.PositionXOffset = Mathf.Lerp(child.PositionXOffset, this._targetXOffset * base._inverseScaleToUse, dt * this._lerpFactor);
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0002BEB6 File Offset: 0x0002A0B6
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this._isMouseOverSelf = true;
			base.RenderLate = true;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0002BECC File Offset: 0x0002A0CC
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this._isMouseOverSelf = false;
			base.RenderLate = false;
		}

		// Token: 0x04000739 RID: 1849
		private float _targetXOffset;

		// Token: 0x0400073A RID: 1850
		private bool _isMouseOverChildren;

		// Token: 0x0400073B RID: 1851
		private bool _isMouseOverSelf;

		// Token: 0x0400073C RID: 1852
		private float _lerpFactor = 15f;

		// Token: 0x0400073D RID: 1853
		private float _defaultXOffset = 20f;
	}
}
