using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000086 RID: 134
	[EngineClass("rglScript_component")]
	public abstract class ScriptComponent : NativeObject
	{
		// Token: 0x06000C1E RID: 3102 RVA: 0x0000D58E File Offset: 0x0000B78E
		protected ScriptComponent()
		{
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0000D596 File Offset: 0x0000B796
		internal ScriptComponent(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0000D5A5 File Offset: 0x0000B7A5
		public string GetName()
		{
			return EngineApplicationInterface.IScriptComponent.GetName(base.Pointer);
		}
	}
}
