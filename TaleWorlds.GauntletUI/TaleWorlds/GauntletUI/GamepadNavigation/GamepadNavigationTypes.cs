using System;

namespace TaleWorlds.GauntletUI.GamepadNavigation
{
	// Token: 0x0200004F RID: 79
	[Flags]
	public enum GamepadNavigationTypes
	{
		// Token: 0x04000271 RID: 625
		None = 0,
		// Token: 0x04000272 RID: 626
		Up = 1,
		// Token: 0x04000273 RID: 627
		Down = 2,
		// Token: 0x04000274 RID: 628
		Vertical = 3,
		// Token: 0x04000275 RID: 629
		Left = 4,
		// Token: 0x04000276 RID: 630
		Right = 8,
		// Token: 0x04000277 RID: 631
		Horizontal = 12
	}
}
