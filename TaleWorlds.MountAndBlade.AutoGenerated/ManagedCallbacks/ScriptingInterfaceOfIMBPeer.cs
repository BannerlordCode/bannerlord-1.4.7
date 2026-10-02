using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001C RID: 28
	internal class ScriptingInterfaceOfIMBPeer : IMBPeer
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0000D17A File Offset: 0x0000B37A
		public void BeginModuleEvent(int index, bool isReliable)
		{
			ScriptingInterfaceOfIMBPeer.call_BeginModuleEventDelegate(index, isReliable);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000D188 File Offset: 0x0000B388
		public void EndModuleEvent(bool isReliable)
		{
			ScriptingInterfaceOfIMBPeer.call_EndModuleEventDelegate(isReliable);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000D195 File Offset: 0x0000B395
		public double GetAverageLossPercent(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetAverageLossPercentDelegate(index);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D1A2 File Offset: 0x0000B3A2
		public double GetAveragePingInMilliseconds(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetAveragePingInMillisecondsDelegate(index);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000D1AF File Offset: 0x0000B3AF
		public uint GetHost(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetHostDelegate(index);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000D1BC File Offset: 0x0000B3BC
		public bool GetIsSynchronized(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetIsSynchronizedDelegate(index);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000D1C9 File Offset: 0x0000B3C9
		public ushort GetPort(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetPortDelegate(index);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000D1D6 File Offset: 0x0000B3D6
		public uint GetReversedHost(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_GetReversedHostDelegate(index);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000D1E3 File Offset: 0x0000B3E3
		public bool IsActive(int index)
		{
			return ScriptingInterfaceOfIMBPeer.call_IsActiveDelegate(index);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		public void SendExistingObjects(int index, UIntPtr missionPointer)
		{
			ScriptingInterfaceOfIMBPeer.call_SendExistingObjectsDelegate(index, missionPointer);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000D1FE File Offset: 0x0000B3FE
		public void SetControlledAgent(int index, UIntPtr missionPointer, int agentIndex)
		{
			ScriptingInterfaceOfIMBPeer.call_SetControlledAgentDelegate(index, missionPointer, agentIndex);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000D20D File Offset: 0x0000B40D
		public void SetIsSynchronized(int index, bool value)
		{
			ScriptingInterfaceOfIMBPeer.call_SetIsSynchronizedDelegate(index, value);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000D21B File Offset: 0x0000B41B
		public void SetRelevantGameOptions(int index, bool sendMeBloodEvents, bool sendMeSoundEvents)
		{
			ScriptingInterfaceOfIMBPeer.call_SetRelevantGameOptionsDelegate(index, sendMeBloodEvents, sendMeSoundEvents);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000D22A File Offset: 0x0000B42A
		public void SetTeam(int index, int teamIndex)
		{
			ScriptingInterfaceOfIMBPeer.call_SetTeamDelegate(index, teamIndex);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000D238 File Offset: 0x0000B438
		public void SetUserData(int index, MBNetworkPeer data)
		{
			ScriptingInterfaceOfIMBPeer.call_SetUserDataDelegate(index, (data != null) ? data.GetManagedId() : 0);
		}

		// Token: 0x0400029E RID: 670
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400029F RID: 671
		public static ScriptingInterfaceOfIMBPeer.BeginModuleEventDelegate call_BeginModuleEventDelegate;

		// Token: 0x040002A0 RID: 672
		public static ScriptingInterfaceOfIMBPeer.EndModuleEventDelegate call_EndModuleEventDelegate;

		// Token: 0x040002A1 RID: 673
		public static ScriptingInterfaceOfIMBPeer.GetAverageLossPercentDelegate call_GetAverageLossPercentDelegate;

		// Token: 0x040002A2 RID: 674
		public static ScriptingInterfaceOfIMBPeer.GetAveragePingInMillisecondsDelegate call_GetAveragePingInMillisecondsDelegate;

		// Token: 0x040002A3 RID: 675
		public static ScriptingInterfaceOfIMBPeer.GetHostDelegate call_GetHostDelegate;

		// Token: 0x040002A4 RID: 676
		public static ScriptingInterfaceOfIMBPeer.GetIsSynchronizedDelegate call_GetIsSynchronizedDelegate;

		// Token: 0x040002A5 RID: 677
		public static ScriptingInterfaceOfIMBPeer.GetPortDelegate call_GetPortDelegate;

		// Token: 0x040002A6 RID: 678
		public static ScriptingInterfaceOfIMBPeer.GetReversedHostDelegate call_GetReversedHostDelegate;

		// Token: 0x040002A7 RID: 679
		public static ScriptingInterfaceOfIMBPeer.IsActiveDelegate call_IsActiveDelegate;

		// Token: 0x040002A8 RID: 680
		public static ScriptingInterfaceOfIMBPeer.SendExistingObjectsDelegate call_SendExistingObjectsDelegate;

		// Token: 0x040002A9 RID: 681
		public static ScriptingInterfaceOfIMBPeer.SetControlledAgentDelegate call_SetControlledAgentDelegate;

		// Token: 0x040002AA RID: 682
		public static ScriptingInterfaceOfIMBPeer.SetIsSynchronizedDelegate call_SetIsSynchronizedDelegate;

		// Token: 0x040002AB RID: 683
		public static ScriptingInterfaceOfIMBPeer.SetRelevantGameOptionsDelegate call_SetRelevantGameOptionsDelegate;

		// Token: 0x040002AC RID: 684
		public static ScriptingInterfaceOfIMBPeer.SetTeamDelegate call_SetTeamDelegate;

		// Token: 0x040002AD RID: 685
		public static ScriptingInterfaceOfIMBPeer.SetUserDataDelegate call_SetUserDataDelegate;

		// Token: 0x020002FC RID: 764
		// (Invoke) Token: 0x06000EDE RID: 3806
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BeginModuleEventDelegate(int index, [MarshalAs(UnmanagedType.U1)] bool isReliable);

		// Token: 0x020002FD RID: 765
		// (Invoke) Token: 0x06000EE2 RID: 3810
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EndModuleEventDelegate([MarshalAs(UnmanagedType.U1)] bool isReliable);

		// Token: 0x020002FE RID: 766
		// (Invoke) Token: 0x06000EE6 RID: 3814
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate double GetAverageLossPercentDelegate(int index);

		// Token: 0x020002FF RID: 767
		// (Invoke) Token: 0x06000EEA RID: 3818
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate double GetAveragePingInMillisecondsDelegate(int index);

		// Token: 0x02000300 RID: 768
		// (Invoke) Token: 0x06000EEE RID: 3822
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetHostDelegate(int index);

		// Token: 0x02000301 RID: 769
		// (Invoke) Token: 0x06000EF2 RID: 3826
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetIsSynchronizedDelegate(int index);

		// Token: 0x02000302 RID: 770
		// (Invoke) Token: 0x06000EF6 RID: 3830
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ushort GetPortDelegate(int index);

		// Token: 0x02000303 RID: 771
		// (Invoke) Token: 0x06000EFA RID: 3834
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetReversedHostDelegate(int index);

		// Token: 0x02000304 RID: 772
		// (Invoke) Token: 0x06000EFE RID: 3838
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsActiveDelegate(int index);

		// Token: 0x02000305 RID: 773
		// (Invoke) Token: 0x06000F02 RID: 3842
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SendExistingObjectsDelegate(int index, UIntPtr missionPointer);

		// Token: 0x02000306 RID: 774
		// (Invoke) Token: 0x06000F06 RID: 3846
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetControlledAgentDelegate(int index, UIntPtr missionPointer, int agentIndex);

		// Token: 0x02000307 RID: 775
		// (Invoke) Token: 0x06000F0A RID: 3850
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetIsSynchronizedDelegate(int index, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000308 RID: 776
		// (Invoke) Token: 0x06000F0E RID: 3854
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRelevantGameOptionsDelegate(int index, [MarshalAs(UnmanagedType.U1)] bool sendMeBloodEvents, [MarshalAs(UnmanagedType.U1)] bool sendMeSoundEvents);

		// Token: 0x02000309 RID: 777
		// (Invoke) Token: 0x06000F12 RID: 3858
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTeamDelegate(int index, int teamIndex);

		// Token: 0x0200030A RID: 778
		// (Invoke) Token: 0x06000F16 RID: 3862
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUserDataDelegate(int index, int data);
	}
}
