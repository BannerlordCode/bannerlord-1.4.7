using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200001E RID: 30
	internal class ScriptingInterfaceOfIParticleSystem : IParticleSystem
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x00014CCC File Offset: 0x00012ECC
		public ParticleSystem CreateParticleSystemAttachedToBone(int runtimeId, UIntPtr skeletonPtr, sbyte boneIndex, ref MatrixFrame boneLocalFrame)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIParticleSystem.call_CreateParticleSystemAttachedToBoneDelegate(runtimeId, skeletonPtr, boneIndex, ref boneLocalFrame);
			ParticleSystem particleSystem = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				particleSystem = new ParticleSystem(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return particleSystem;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00014D1C File Offset: 0x00012F1C
		public ParticleSystem CreateParticleSystemAttachedToEntity(int runtimeId, UIntPtr entityPtr, ref MatrixFrame boneLocalFrame)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIParticleSystem.call_CreateParticleSystemAttachedToEntityDelegate(runtimeId, entityPtr, ref boneLocalFrame);
			ParticleSystem particleSystem = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				particleSystem = new ParticleSystem(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return particleSystem;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00014D68 File Offset: 0x00012F68
		public void GetLocalFrame(UIntPtr pointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIParticleSystem.call_GetLocalFrameDelegate(pointer, ref frame);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00014D78 File Offset: 0x00012F78
		public int GetRuntimeIdByName(string particleSystemName)
		{
			byte[] array = null;
			if (particleSystemName != null)
			{
				int byteCount = ScriptingInterfaceOfIParticleSystem._utf8.GetByteCount(particleSystemName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIParticleSystem._utf8.GetBytes(particleSystemName, 0, particleSystemName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIParticleSystem.call_GetRuntimeIdByNameDelegate(array);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00014DD2 File Offset: 0x00012FD2
		public bool HasAliveParticles(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIParticleSystem.call_HasAliveParticlesDelegate(pointer);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00014DDF File Offset: 0x00012FDF
		public void Restart(UIntPtr psysPointer)
		{
			ScriptingInterfaceOfIParticleSystem.call_RestartDelegate(psysPointer);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00014DEC File Offset: 0x00012FEC
		public void SetDontRemoveFromEntity(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetDontRemoveFromEntityDelegate(pointer, value);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00014DFA File Offset: 0x00012FFA
		public void SetEnable(UIntPtr psysPointer, bool enable)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetEnableDelegate(psysPointer, enable);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00014E08 File Offset: 0x00013008
		public void SetLocalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetLocalFrameDelegate(pointer, in newFrame);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00014E18 File Offset: 0x00013018
		public void SetParticleEffectByName(UIntPtr pointer, string effectName)
		{
			byte[] array = null;
			if (effectName != null)
			{
				int byteCount = ScriptingInterfaceOfIParticleSystem._utf8.GetByteCount(effectName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIParticleSystem._utf8.GetBytes(effectName, 0, effectName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIParticleSystem.call_SetParticleEffectByNameDelegate(pointer, array);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00014E73 File Offset: 0x00013073
		public void SetPreviousGlobalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetPreviousGlobalFrameDelegate(pointer, in newFrame);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00014E81 File Offset: 0x00013081
		public void SetRuntimeEmissionRateMultiplier(UIntPtr pointer, float multiplier)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetRuntimeEmissionRateMultiplierDelegate(pointer, multiplier);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00014EA3 File Offset: 0x000130A3
		void IParticleSystem.SetLocalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			this.SetLocalFrame(pointer, in newFrame);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00014EAD File Offset: 0x000130AD
		void IParticleSystem.SetPreviousGlobalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			this.SetPreviousGlobalFrame(pointer, in newFrame);
		}

		// Token: 0x04000325 RID: 805
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000326 RID: 806
		public static ScriptingInterfaceOfIParticleSystem.CreateParticleSystemAttachedToBoneDelegate call_CreateParticleSystemAttachedToBoneDelegate;

		// Token: 0x04000327 RID: 807
		public static ScriptingInterfaceOfIParticleSystem.CreateParticleSystemAttachedToEntityDelegate call_CreateParticleSystemAttachedToEntityDelegate;

		// Token: 0x04000328 RID: 808
		public static ScriptingInterfaceOfIParticleSystem.GetLocalFrameDelegate call_GetLocalFrameDelegate;

		// Token: 0x04000329 RID: 809
		public static ScriptingInterfaceOfIParticleSystem.GetRuntimeIdByNameDelegate call_GetRuntimeIdByNameDelegate;

		// Token: 0x0400032A RID: 810
		public static ScriptingInterfaceOfIParticleSystem.HasAliveParticlesDelegate call_HasAliveParticlesDelegate;

		// Token: 0x0400032B RID: 811
		public static ScriptingInterfaceOfIParticleSystem.RestartDelegate call_RestartDelegate;

		// Token: 0x0400032C RID: 812
		public static ScriptingInterfaceOfIParticleSystem.SetDontRemoveFromEntityDelegate call_SetDontRemoveFromEntityDelegate;

		// Token: 0x0400032D RID: 813
		public static ScriptingInterfaceOfIParticleSystem.SetEnableDelegate call_SetEnableDelegate;

		// Token: 0x0400032E RID: 814
		public static ScriptingInterfaceOfIParticleSystem.SetLocalFrameDelegate call_SetLocalFrameDelegate;

		// Token: 0x0400032F RID: 815
		public static ScriptingInterfaceOfIParticleSystem.SetParticleEffectByNameDelegate call_SetParticleEffectByNameDelegate;

		// Token: 0x04000330 RID: 816
		public static ScriptingInterfaceOfIParticleSystem.SetPreviousGlobalFrameDelegate call_SetPreviousGlobalFrameDelegate;

		// Token: 0x04000331 RID: 817
		public static ScriptingInterfaceOfIParticleSystem.SetRuntimeEmissionRateMultiplierDelegate call_SetRuntimeEmissionRateMultiplierDelegate;

		// Token: 0x02000396 RID: 918
		// (Invoke) Token: 0x06001471 RID: 5233
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateParticleSystemAttachedToBoneDelegate(int runtimeId, UIntPtr skeletonPtr, sbyte boneIndex, ref MatrixFrame boneLocalFrame);

		// Token: 0x02000397 RID: 919
		// (Invoke) Token: 0x06001475 RID: 5237
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateParticleSystemAttachedToEntityDelegate(int runtimeId, UIntPtr entityPtr, ref MatrixFrame boneLocalFrame);

		// Token: 0x02000398 RID: 920
		// (Invoke) Token: 0x06001479 RID: 5241
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetLocalFrameDelegate(UIntPtr pointer, ref MatrixFrame frame);

		// Token: 0x02000399 RID: 921
		// (Invoke) Token: 0x0600147D RID: 5245
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetRuntimeIdByNameDelegate(byte[] particleSystemName);

		// Token: 0x0200039A RID: 922
		// (Invoke) Token: 0x06001481 RID: 5249
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasAliveParticlesDelegate(UIntPtr pointer);

		// Token: 0x0200039B RID: 923
		// (Invoke) Token: 0x06001485 RID: 5253
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RestartDelegate(UIntPtr psysPointer);

		// Token: 0x0200039C RID: 924
		// (Invoke) Token: 0x06001489 RID: 5257
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDontRemoveFromEntityDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200039D RID: 925
		// (Invoke) Token: 0x0600148D RID: 5261
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEnableDelegate(UIntPtr psysPointer, [MarshalAs(UnmanagedType.U1)] bool enable);

		// Token: 0x0200039E RID: 926
		// (Invoke) Token: 0x06001491 RID: 5265
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLocalFrameDelegate(UIntPtr pointer, in MatrixFrame newFrame);

		// Token: 0x0200039F RID: 927
		// (Invoke) Token: 0x06001495 RID: 5269
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetParticleEffectByNameDelegate(UIntPtr pointer, byte[] effectName);

		// Token: 0x020003A0 RID: 928
		// (Invoke) Token: 0x06001499 RID: 5273
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPreviousGlobalFrameDelegate(UIntPtr pointer, in MatrixFrame newFrame);

		// Token: 0x020003A1 RID: 929
		// (Invoke) Token: 0x0600149D RID: 5277
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRuntimeEmissionRateMultiplierDelegate(UIntPtr pointer, float multiplier);
	}
}
