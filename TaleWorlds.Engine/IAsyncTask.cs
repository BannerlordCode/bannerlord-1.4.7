using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200002D RID: 45
	[ApplicationInterfaceBase]
	internal interface IAsyncTask
	{
		// Token: 0x060004FE RID: 1278
		[EngineMethod("create_with_function", false, null, false)]
		AsyncTask CreateWithDelegate(ManagedDelegate function, bool isBackground);

		// Token: 0x060004FF RID: 1279
		[EngineMethod("invoke", false, null, false)]
		void Invoke(UIntPtr Pointer);

		// Token: 0x06000500 RID: 1280
		[EngineMethod("wait", false, null, false)]
		void Wait(UIntPtr Pointer);
	}
}
