using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000020 RID: 32
	internal class ScriptingInterfaceOfIMBTeam : IMBTeam
	{
		// Token: 0x0600035A RID: 858 RVA: 0x0000D77D File Offset: 0x0000B97D
		public bool IsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex)
		{
			return ScriptingInterfaceOfIMBTeam.call_IsEnemyDelegate(missionPointer, teamIndex, otherTeamIndex);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000D78C File Offset: 0x0000B98C
		public void SetIsEnemy(UIntPtr missionPointer, int teamIndex, int otherTeamIndex, bool isEnemy)
		{
			ScriptingInterfaceOfIMBTeam.call_SetIsEnemyDelegate(missionPointer, teamIndex, otherTeamIndex, isEnemy);
		}

		// Token: 0x040002C8 RID: 712
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002C9 RID: 713
		public static ScriptingInterfaceOfIMBTeam.IsEnemyDelegate call_IsEnemyDelegate;

		// Token: 0x040002CA RID: 714
		public static ScriptingInterfaceOfIMBTeam.SetIsEnemyDelegate call_SetIsEnemyDelegate;

		// Token: 0x02000322 RID: 802
		// (Invoke) Token: 0x06000F76 RID: 3958
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsEnemyDelegate(UIntPtr missionPointer, int teamIndex, int otherTeamIndex);

		// Token: 0x02000323 RID: 803
		// (Invoke) Token: 0x06000F7A RID: 3962
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetIsEnemyDelegate(UIntPtr missionPointer, int teamIndex, int otherTeamIndex, [MarshalAs(UnmanagedType.U1)] bool isEnemy);
	}
}
