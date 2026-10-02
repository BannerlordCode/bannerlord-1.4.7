using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200006A RID: 106
	public abstract class MessageManagerBase : DotNetObject
	{
		// Token: 0x060009E9 RID: 2537
		[EngineCallback(null, false)]
		protected internal abstract void PostWarningLine(string text);

		// Token: 0x060009EA RID: 2538
		[EngineCallback(null, false)]
		protected internal abstract void PostSuccessLine(string text);

		// Token: 0x060009EB RID: 2539
		[EngineCallback(null, false)]
		protected internal abstract void PostMessageLineFormatted(string text, uint color);

		// Token: 0x060009EC RID: 2540
		[EngineCallback(null, false)]
		protected internal abstract void PostMessageLine(string text, uint color);
	}
}
