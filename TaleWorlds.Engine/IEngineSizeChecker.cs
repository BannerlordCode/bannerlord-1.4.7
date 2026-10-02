using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200003A RID: 58
	[ApplicationInterfaceBase]
	internal interface IEngineSizeChecker
	{
		// Token: 0x0600061A RID: 1562
		[EngineMethod("get_engine_struct_size", false, null, false)]
		int GetEngineStructSize(string str);

		// Token: 0x0600061B RID: 1563
		[EngineMethod("get_engine_struct_member_offset", false, null, false)]
		IntPtr GetEngineStructMemberOffset(string className, string memberName);
	}
}
