using System;
using System.Numerics;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.Layout
{
	// Token: 0x02000044 RID: 68
	public interface ILayout
	{
		// Token: 0x0600043A RID: 1082
		Vector2 MeasureChildren(Widget widget, Vector2 measureSpec, SpriteData spriteData, float renderScale);

		// Token: 0x0600043B RID: 1083
		void OnLayout(Widget widget, float left, float bottom, float right, float top);
	}
}
