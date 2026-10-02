using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000041 RID: 65
	[Flags]
	public enum InputUsageMask
	{
		// Token: 0x040000E3 RID: 227
		Invalid = 0,
		// Token: 0x040000E4 RID: 228
		MouseButtons = 1,
		// Token: 0x040000E5 RID: 229
		MouseWheels = 2,
		// Token: 0x040000E6 RID: 230
		Keyboardkeys = 4,
		// Token: 0x040000E7 RID: 231
		BlockEverythingWithoutHitTest = 8,
		// Token: 0x040000E8 RID: 232
		Mouse = 3,
		// Token: 0x040000E9 RID: 233
		All = 7
	}
}
