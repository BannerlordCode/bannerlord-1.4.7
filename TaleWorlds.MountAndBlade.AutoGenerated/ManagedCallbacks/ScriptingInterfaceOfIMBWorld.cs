using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000024 RID: 36
	internal class ScriptingInterfaceOfIMBWorld : IMBWorld
	{
		// Token: 0x06000378 RID: 888 RVA: 0x0000DA4D File Offset: 0x0000BC4D
		public void CheckResourceModifications()
		{
			ScriptingInterfaceOfIMBWorld.call_CheckResourceModificationsDelegate();
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000DA59 File Offset: 0x0000BC59
		public void FixSkeletons()
		{
			ScriptingInterfaceOfIMBWorld.call_FixSkeletonsDelegate();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000DA65 File Offset: 0x0000BC65
		public int GetGameType()
		{
			return ScriptingInterfaceOfIMBWorld.call_GetGameTypeDelegate();
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000DA71 File Offset: 0x0000BC71
		public float GetGlobalTime(MBCommon.TimeType timeType)
		{
			return ScriptingInterfaceOfIMBWorld.call_GetGlobalTimeDelegate(timeType);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000DA7E File Offset: 0x0000BC7E
		public string GetLastMessages()
		{
			if (ScriptingInterfaceOfIMBWorld.call_GetLastMessagesDelegate() != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000DA94 File Offset: 0x0000BC94
		public void PauseGame()
		{
			ScriptingInterfaceOfIMBWorld.call_PauseGameDelegate();
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000DAA0 File Offset: 0x0000BCA0
		public void SetBodyUsed(string bodyName)
		{
			byte[] array = null;
			if (bodyName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBWorld._utf8.GetByteCount(bodyName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBWorld._utf8.GetBytes(bodyName, 0, bodyName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBWorld.call_SetBodyUsedDelegate(array);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000DAFA File Offset: 0x0000BCFA
		public void SetGameType(int gameType)
		{
			ScriptingInterfaceOfIMBWorld.call_SetGameTypeDelegate(gameType);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000DB08 File Offset: 0x0000BD08
		public void SetMaterialUsed(string materialName)
		{
			byte[] array = null;
			if (materialName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBWorld._utf8.GetByteCount(materialName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBWorld._utf8.GetBytes(materialName, 0, materialName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBWorld.call_SetMaterialUsedDelegate(array);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000DB64 File Offset: 0x0000BD64
		public void SetMeshUsed(string meshName)
		{
			byte[] array = null;
			if (meshName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBWorld._utf8.GetByteCount(meshName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBWorld._utf8.GetBytes(meshName, 0, meshName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBWorld.call_SetMeshUsedDelegate(array);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000DBBE File Offset: 0x0000BDBE
		public void UnpauseGame()
		{
			ScriptingInterfaceOfIMBWorld.call_UnpauseGameDelegate();
		}

		// Token: 0x040002E2 RID: 738
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002E3 RID: 739
		public static ScriptingInterfaceOfIMBWorld.CheckResourceModificationsDelegate call_CheckResourceModificationsDelegate;

		// Token: 0x040002E4 RID: 740
		public static ScriptingInterfaceOfIMBWorld.FixSkeletonsDelegate call_FixSkeletonsDelegate;

		// Token: 0x040002E5 RID: 741
		public static ScriptingInterfaceOfIMBWorld.GetGameTypeDelegate call_GetGameTypeDelegate;

		// Token: 0x040002E6 RID: 742
		public static ScriptingInterfaceOfIMBWorld.GetGlobalTimeDelegate call_GetGlobalTimeDelegate;

		// Token: 0x040002E7 RID: 743
		public static ScriptingInterfaceOfIMBWorld.GetLastMessagesDelegate call_GetLastMessagesDelegate;

		// Token: 0x040002E8 RID: 744
		public static ScriptingInterfaceOfIMBWorld.PauseGameDelegate call_PauseGameDelegate;

		// Token: 0x040002E9 RID: 745
		public static ScriptingInterfaceOfIMBWorld.SetBodyUsedDelegate call_SetBodyUsedDelegate;

		// Token: 0x040002EA RID: 746
		public static ScriptingInterfaceOfIMBWorld.SetGameTypeDelegate call_SetGameTypeDelegate;

		// Token: 0x040002EB RID: 747
		public static ScriptingInterfaceOfIMBWorld.SetMaterialUsedDelegate call_SetMaterialUsedDelegate;

		// Token: 0x040002EC RID: 748
		public static ScriptingInterfaceOfIMBWorld.SetMeshUsedDelegate call_SetMeshUsedDelegate;

		// Token: 0x040002ED RID: 749
		public static ScriptingInterfaceOfIMBWorld.UnpauseGameDelegate call_UnpauseGameDelegate;

		// Token: 0x02000338 RID: 824
		// (Invoke) Token: 0x06000FCE RID: 4046
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CheckResourceModificationsDelegate();

		// Token: 0x02000339 RID: 825
		// (Invoke) Token: 0x06000FD2 RID: 4050
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FixSkeletonsDelegate();

		// Token: 0x0200033A RID: 826
		// (Invoke) Token: 0x06000FD6 RID: 4054
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetGameTypeDelegate();

		// Token: 0x0200033B RID: 827
		// (Invoke) Token: 0x06000FDA RID: 4058
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetGlobalTimeDelegate(MBCommon.TimeType timeType);

		// Token: 0x0200033C RID: 828
		// (Invoke) Token: 0x06000FDE RID: 4062
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetLastMessagesDelegate();

		// Token: 0x0200033D RID: 829
		// (Invoke) Token: 0x06000FE2 RID: 4066
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PauseGameDelegate();

		// Token: 0x0200033E RID: 830
		// (Invoke) Token: 0x06000FE6 RID: 4070
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBodyUsedDelegate(byte[] bodyName);

		// Token: 0x0200033F RID: 831
		// (Invoke) Token: 0x06000FEA RID: 4074
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetGameTypeDelegate(int gameType);

		// Token: 0x02000340 RID: 832
		// (Invoke) Token: 0x06000FEE RID: 4078
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMaterialUsedDelegate(byte[] materialName);

		// Token: 0x02000341 RID: 833
		// (Invoke) Token: 0x06000FF2 RID: 4082
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMeshUsedDelegate(byte[] meshName);

		// Token: 0x02000342 RID: 834
		// (Invoke) Token: 0x06000FF6 RID: 4086
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnpauseGameDelegate();
	}
}
