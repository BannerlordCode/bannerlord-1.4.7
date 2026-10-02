using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000022 RID: 34
	internal class ScriptingInterfaceOfIMBVoiceManager : IMBVoiceManager
	{
		// Token: 0x0600036A RID: 874 RVA: 0x0000D890 File Offset: 0x0000BA90
		public int GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(string className)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIMBVoiceManager._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBVoiceManager._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate(array);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		public void GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(string className, int[] definitionIndices)
		{
			byte[] array = null;
			if (className != null)
			{
				int byteCount = ScriptingInterfaceOfIMBVoiceManager._utf8.GetByteCount(className);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBVoiceManager._utf8.GetBytes(className, 0, className.Length, array, 0);
				array[byteCount] = 0;
			}
			PinnedArrayData<int> pinnedArrayData = new PinnedArrayData<int>(definitionIndices, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate(array, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000D960 File Offset: 0x0000BB60
		public int GetVoiceTypeIndex(string voiceType)
		{
			byte[] array = null;
			if (voiceType != null)
			{
				int byteCount = ScriptingInterfaceOfIMBVoiceManager._utf8.GetByteCount(voiceType);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBVoiceManager._utf8.GetBytes(voiceType, 0, voiceType.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBVoiceManager.call_GetVoiceTypeIndexDelegate(array);
		}

		// Token: 0x040002D6 RID: 726
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002D7 RID: 727
		public static ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate call_GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate;

		// Token: 0x040002D8 RID: 728
		public static ScriptingInterfaceOfIMBVoiceManager.GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate call_GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate;

		// Token: 0x040002D9 RID: 729
		public static ScriptingInterfaceOfIMBVoiceManager.GetVoiceTypeIndexDelegate call_GetVoiceTypeIndexDelegate;

		// Token: 0x0200032E RID: 814
		// (Invoke) Token: 0x06000FA6 RID: 4006
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassNameDelegate(byte[] className);

		// Token: 0x0200032F RID: 815
		// (Invoke) Token: 0x06000FAA RID: 4010
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassNameDelegate(byte[] className, IntPtr definitionIndices);

		// Token: 0x02000330 RID: 816
		// (Invoke) Token: 0x06000FAE RID: 4014
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVoiceTypeIndexDelegate(byte[] voiceType);
	}
}
