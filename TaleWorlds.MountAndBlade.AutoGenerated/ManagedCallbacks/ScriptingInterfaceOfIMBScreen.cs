using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001D RID: 29
	internal class ScriptingInterfaceOfIMBScreen : IMBScreen
	{
		// Token: 0x06000339 RID: 825 RVA: 0x0000D265 File Offset: 0x0000B465
		public void OnEditModeEnterPress()
		{
			ScriptingInterfaceOfIMBScreen.call_OnEditModeEnterPressDelegate();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000D271 File Offset: 0x0000B471
		public void OnEditModeEnterRelease()
		{
			ScriptingInterfaceOfIMBScreen.call_OnEditModeEnterReleaseDelegate();
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000D27D File Offset: 0x0000B47D
		public void OnExitButtonClick()
		{
			ScriptingInterfaceOfIMBScreen.call_OnExitButtonClickDelegate();
		}

		// Token: 0x040002AE RID: 686
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002AF RID: 687
		public static ScriptingInterfaceOfIMBScreen.OnEditModeEnterPressDelegate call_OnEditModeEnterPressDelegate;

		// Token: 0x040002B0 RID: 688
		public static ScriptingInterfaceOfIMBScreen.OnEditModeEnterReleaseDelegate call_OnEditModeEnterReleaseDelegate;

		// Token: 0x040002B1 RID: 689
		public static ScriptingInterfaceOfIMBScreen.OnExitButtonClickDelegate call_OnExitButtonClickDelegate;

		// Token: 0x0200030B RID: 779
		// (Invoke) Token: 0x06000F1A RID: 3866
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnEditModeEnterPressDelegate();

		// Token: 0x0200030C RID: 780
		// (Invoke) Token: 0x06000F1E RID: 3870
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnEditModeEnterReleaseDelegate();

		// Token: 0x0200030D RID: 781
		// (Invoke) Token: 0x06000F22 RID: 3874
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OnExitButtonClickDelegate();
	}
}
