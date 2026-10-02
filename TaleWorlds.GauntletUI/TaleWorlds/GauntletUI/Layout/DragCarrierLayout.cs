using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.Layout
{
	// Token: 0x0200003F RID: 63
	public class DragCarrierLayout : ILayout
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x0001077A File Offset: 0x0000E97A
		Vector2 ILayout.MeasureChildren(Widget widget, Vector2 measureSpec, SpriteData spriteData, float renderScale)
		{
			Widget child = widget.GetChild(0);
			child.Measure(measureSpec);
			return child.MeasuredSize;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00010790 File Offset: 0x0000E990
		void ILayout.OnLayout(Widget widget, float left, float bottom, float right, float top)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = right - left;
			float num4 = bottom - top;
			widget.GetChild(0).Layout(num, num4, num3, num2);
		}
	}
}
