using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000070 RID: 112
	[EngineClass("rglNative_script_component")]
	public sealed class NativeScriptComponent : ScriptComponent
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x0000A8FF File Offset: 0x00008AFF
		internal NativeScriptComponent(UIntPtr pointer)
			: base(pointer)
		{
		}
	}
}
