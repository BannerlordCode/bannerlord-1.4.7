using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000024 RID: 36
	internal class ScriptingInterfaceOfIScreen : IScreen
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x00017F85 File Offset: 0x00016185
		public float GetAspectRatio()
		{
			return ScriptingInterfaceOfIScreen.call_GetAspectRatioDelegate();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00017F91 File Offset: 0x00016191
		public float GetDesktopHeight()
		{
			return ScriptingInterfaceOfIScreen.call_GetDesktopHeightDelegate();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00017F9D File Offset: 0x0001619D
		public float GetDesktopWidth()
		{
			return ScriptingInterfaceOfIScreen.call_GetDesktopWidthDelegate();
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00017FA9 File Offset: 0x000161A9
		public bool GetMouseVisible()
		{
			return ScriptingInterfaceOfIScreen.call_GetMouseVisibleDelegate();
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00017FB5 File Offset: 0x000161B5
		public float GetRealScreenResolutionHeight()
		{
			return ScriptingInterfaceOfIScreen.call_GetRealScreenResolutionHeightDelegate();
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00017FC1 File Offset: 0x000161C1
		public float GetRealScreenResolutionWidth()
		{
			return ScriptingInterfaceOfIScreen.call_GetRealScreenResolutionWidthDelegate();
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00017FCD File Offset: 0x000161CD
		public Vec2 GetUsableAreaPercentages()
		{
			return ScriptingInterfaceOfIScreen.call_GetUsableAreaPercentagesDelegate();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00017FD9 File Offset: 0x000161D9
		public bool IsEnterButtonCross()
		{
			return ScriptingInterfaceOfIScreen.call_IsEnterButtonCrossDelegate();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00017FE5 File Offset: 0x000161E5
		public void SetMouseVisible(bool value)
		{
			ScriptingInterfaceOfIScreen.call_SetMouseVisibleDelegate(value);
		}

		// Token: 0x040004B8 RID: 1208
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004B9 RID: 1209
		public static ScriptingInterfaceOfIScreen.GetAspectRatioDelegate call_GetAspectRatioDelegate;

		// Token: 0x040004BA RID: 1210
		public static ScriptingInterfaceOfIScreen.GetDesktopHeightDelegate call_GetDesktopHeightDelegate;

		// Token: 0x040004BB RID: 1211
		public static ScriptingInterfaceOfIScreen.GetDesktopWidthDelegate call_GetDesktopWidthDelegate;

		// Token: 0x040004BC RID: 1212
		public static ScriptingInterfaceOfIScreen.GetMouseVisibleDelegate call_GetMouseVisibleDelegate;

		// Token: 0x040004BD RID: 1213
		public static ScriptingInterfaceOfIScreen.GetRealScreenResolutionHeightDelegate call_GetRealScreenResolutionHeightDelegate;

		// Token: 0x040004BE RID: 1214
		public static ScriptingInterfaceOfIScreen.GetRealScreenResolutionWidthDelegate call_GetRealScreenResolutionWidthDelegate;

		// Token: 0x040004BF RID: 1215
		public static ScriptingInterfaceOfIScreen.GetUsableAreaPercentagesDelegate call_GetUsableAreaPercentagesDelegate;

		// Token: 0x040004C0 RID: 1216
		public static ScriptingInterfaceOfIScreen.IsEnterButtonCrossDelegate call_IsEnterButtonCrossDelegate;

		// Token: 0x040004C1 RID: 1217
		public static ScriptingInterfaceOfIScreen.SetMouseVisibleDelegate call_SetMouseVisibleDelegate;

		// Token: 0x02000523 RID: 1315
		// (Invoke) Token: 0x06001AA5 RID: 6821
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAspectRatioDelegate();

		// Token: 0x02000524 RID: 1316
		// (Invoke) Token: 0x06001AA9 RID: 6825
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetDesktopHeightDelegate();

		// Token: 0x02000525 RID: 1317
		// (Invoke) Token: 0x06001AAD RID: 6829
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetDesktopWidthDelegate();

		// Token: 0x02000526 RID: 1318
		// (Invoke) Token: 0x06001AB1 RID: 6833
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetMouseVisibleDelegate();

		// Token: 0x02000527 RID: 1319
		// (Invoke) Token: 0x06001AB5 RID: 6837
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRealScreenResolutionHeightDelegate();

		// Token: 0x02000528 RID: 1320
		// (Invoke) Token: 0x06001AB9 RID: 6841
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRealScreenResolutionWidthDelegate();

		// Token: 0x02000529 RID: 1321
		// (Invoke) Token: 0x06001ABD RID: 6845
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetUsableAreaPercentagesDelegate();

		// Token: 0x0200052A RID: 1322
		// (Invoke) Token: 0x06001AC1 RID: 6849
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEnterButtonCrossDelegate();

		// Token: 0x0200052B RID: 1323
		// (Invoke) Token: 0x06001AC5 RID: 6853
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMouseVisibleDelegate([MarshalAs(UnmanagedType.U1)] bool value);
	}
}
