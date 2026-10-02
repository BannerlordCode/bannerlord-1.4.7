using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000021 RID: 33
	internal class ScriptingInterfaceOfIMBTestRun : IMBTestRun
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000D7B1 File Offset: 0x0000B9B1
		public int AutoContinue(int type)
		{
			return ScriptingInterfaceOfIMBTestRun.call_AutoContinueDelegate(type);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000D7BE File Offset: 0x0000B9BE
		public bool CloseScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_CloseSceneDelegate();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000D7CA File Offset: 0x0000B9CA
		public bool EnterEditMode()
		{
			return ScriptingInterfaceOfIMBTestRun.call_EnterEditModeDelegate();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000D7D6 File Offset: 0x0000B9D6
		public int GetFPS()
		{
			return ScriptingInterfaceOfIMBTestRun.call_GetFPSDelegate();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000D7E2 File Offset: 0x0000B9E2
		public bool LeaveEditMode()
		{
			return ScriptingInterfaceOfIMBTestRun.call_LeaveEditModeDelegate();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000D7EE File Offset: 0x0000B9EE
		public bool NewScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_NewSceneDelegate();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000D7FA File Offset: 0x0000B9FA
		public bool OpenDefaultScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_OpenDefaultSceneDelegate();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000D808 File Offset: 0x0000BA08
		public bool OpenScene(string sceneName)
		{
			byte[] array = null;
			if (sceneName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBTestRun._utf8.GetByteCount(sceneName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBTestRun._utf8.GetBytes(sceneName, 0, sceneName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBTestRun.call_OpenSceneDelegate(array);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000D862 File Offset: 0x0000BA62
		public bool SaveScene()
		{
			return ScriptingInterfaceOfIMBTestRun.call_SaveSceneDelegate();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000D86E File Offset: 0x0000BA6E
		public void StartMission()
		{
			ScriptingInterfaceOfIMBTestRun.call_StartMissionDelegate();
		}

		// Token: 0x040002CB RID: 715
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002CC RID: 716
		public static ScriptingInterfaceOfIMBTestRun.AutoContinueDelegate call_AutoContinueDelegate;

		// Token: 0x040002CD RID: 717
		public static ScriptingInterfaceOfIMBTestRun.CloseSceneDelegate call_CloseSceneDelegate;

		// Token: 0x040002CE RID: 718
		public static ScriptingInterfaceOfIMBTestRun.EnterEditModeDelegate call_EnterEditModeDelegate;

		// Token: 0x040002CF RID: 719
		public static ScriptingInterfaceOfIMBTestRun.GetFPSDelegate call_GetFPSDelegate;

		// Token: 0x040002D0 RID: 720
		public static ScriptingInterfaceOfIMBTestRun.LeaveEditModeDelegate call_LeaveEditModeDelegate;

		// Token: 0x040002D1 RID: 721
		public static ScriptingInterfaceOfIMBTestRun.NewSceneDelegate call_NewSceneDelegate;

		// Token: 0x040002D2 RID: 722
		public static ScriptingInterfaceOfIMBTestRun.OpenDefaultSceneDelegate call_OpenDefaultSceneDelegate;

		// Token: 0x040002D3 RID: 723
		public static ScriptingInterfaceOfIMBTestRun.OpenSceneDelegate call_OpenSceneDelegate;

		// Token: 0x040002D4 RID: 724
		public static ScriptingInterfaceOfIMBTestRun.SaveSceneDelegate call_SaveSceneDelegate;

		// Token: 0x040002D5 RID: 725
		public static ScriptingInterfaceOfIMBTestRun.StartMissionDelegate call_StartMissionDelegate;

		// Token: 0x02000324 RID: 804
		// (Invoke) Token: 0x06000F7E RID: 3966
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AutoContinueDelegate(int type);

		// Token: 0x02000325 RID: 805
		// (Invoke) Token: 0x06000F82 RID: 3970
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CloseSceneDelegate();

		// Token: 0x02000326 RID: 806
		// (Invoke) Token: 0x06000F86 RID: 3974
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool EnterEditModeDelegate();

		// Token: 0x02000327 RID: 807
		// (Invoke) Token: 0x06000F8A RID: 3978
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetFPSDelegate();

		// Token: 0x02000328 RID: 808
		// (Invoke) Token: 0x06000F8E RID: 3982
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool LeaveEditModeDelegate();

		// Token: 0x02000329 RID: 809
		// (Invoke) Token: 0x06000F92 RID: 3986
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool NewSceneDelegate();

		// Token: 0x0200032A RID: 810
		// (Invoke) Token: 0x06000F96 RID: 3990
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool OpenDefaultSceneDelegate();

		// Token: 0x0200032B RID: 811
		// (Invoke) Token: 0x06000F9A RID: 3994
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool OpenSceneDelegate(byte[] sceneName);

		// Token: 0x0200032C RID: 812
		// (Invoke) Token: 0x06000F9E RID: 3998
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool SaveSceneDelegate();

		// Token: 0x0200032D RID: 813
		// (Invoke) Token: 0x06000FA2 RID: 4002
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StartMissionDelegate();
	}
}
