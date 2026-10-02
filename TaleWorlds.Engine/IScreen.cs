using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003D RID: 61
	[ApplicationInterfaceBase]
	internal interface IScreen
	{
		// Token: 0x0600063D RID: 1597
		[EngineMethod("get_real_screen_resolution_width", false, null, false)]
		float GetRealScreenResolutionWidth();

		// Token: 0x0600063E RID: 1598
		[EngineMethod("get_real_screen_resolution_height", false, null, false)]
		float GetRealScreenResolutionHeight();

		// Token: 0x0600063F RID: 1599
		[EngineMethod("get_desktop_width", false, null, false)]
		float GetDesktopWidth();

		// Token: 0x06000640 RID: 1600
		[EngineMethod("get_desktop_height", false, null, false)]
		float GetDesktopHeight();

		// Token: 0x06000641 RID: 1601
		[EngineMethod("get_aspect_ratio", false, null, false)]
		float GetAspectRatio();

		// Token: 0x06000642 RID: 1602
		[EngineMethod("get_mouse_visible", false, null, false)]
		bool GetMouseVisible();

		// Token: 0x06000643 RID: 1603
		[EngineMethod("set_mouse_visible", false, null, false)]
		void SetMouseVisible(bool value);

		// Token: 0x06000644 RID: 1604
		[EngineMethod("get_usable_area_percentages", false, null, false)]
		Vec2 GetUsableAreaPercentages();

		// Token: 0x06000645 RID: 1605
		[EngineMethod("is_enter_button_cross", false, null, false)]
		bool IsEnterButtonCross();
	}
}
