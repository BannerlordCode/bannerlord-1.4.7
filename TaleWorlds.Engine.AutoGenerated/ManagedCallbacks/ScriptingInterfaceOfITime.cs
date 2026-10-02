using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002E RID: 46
	internal class ScriptingInterfaceOfITime : ITime
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x00019F03 File Offset: 0x00018103
		public float GetApplicationTime()
		{
			return ScriptingInterfaceOfITime.call_GetApplicationTimeDelegate();
		}

		// Token: 0x0400057A RID: 1402
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400057B RID: 1403
		public static ScriptingInterfaceOfITime.GetApplicationTimeDelegate call_GetApplicationTimeDelegate;

		// Token: 0x020005DB RID: 1499
		// (Invoke) Token: 0x06001D85 RID: 7557
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetApplicationTimeDelegate();
	}
}
