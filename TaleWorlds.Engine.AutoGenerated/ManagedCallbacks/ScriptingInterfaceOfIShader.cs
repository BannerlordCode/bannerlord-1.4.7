using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000026 RID: 38
	internal class ScriptingInterfaceOfIShader : IShader
	{
		// Token: 0x06000570 RID: 1392 RVA: 0x00018104 File Offset: 0x00016304
		public Shader GetFromResource(string shaderName)
		{
			byte[] array = null;
			if (shaderName != null)
			{
				int byteCount = ScriptingInterfaceOfIShader._utf8.GetByteCount(shaderName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIShader._utf8.GetBytes(shaderName, 0, shaderName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIShader.call_GetFromResourceDelegate(array);
			Shader shader = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				shader = new Shader(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return shader;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00018190 File Offset: 0x00016390
		public ulong GetMaterialShaderFlagMask(UIntPtr shaderPointer, string flagName, bool showError)
		{
			byte[] array = null;
			if (flagName != null)
			{
				int byteCount = ScriptingInterfaceOfIShader._utf8.GetByteCount(flagName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIShader._utf8.GetBytes(flagName, 0, flagName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIShader.call_GetMaterialShaderFlagMaskDelegate(shaderPointer, array, showError);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000181EC File Offset: 0x000163EC
		public string GetName(UIntPtr shaderPointer)
		{
			if (ScriptingInterfaceOfIShader.call_GetNameDelegate(shaderPointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00018203 File Offset: 0x00016403
		public void Release(UIntPtr shaderPointer)
		{
			ScriptingInterfaceOfIShader.call_ReleaseDelegate(shaderPointer);
		}

		// Token: 0x040004C7 RID: 1223
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004C8 RID: 1224
		public static ScriptingInterfaceOfIShader.GetFromResourceDelegate call_GetFromResourceDelegate;

		// Token: 0x040004C9 RID: 1225
		public static ScriptingInterfaceOfIShader.GetMaterialShaderFlagMaskDelegate call_GetMaterialShaderFlagMaskDelegate;

		// Token: 0x040004CA RID: 1226
		public static ScriptingInterfaceOfIShader.GetNameDelegate call_GetNameDelegate;

		// Token: 0x040004CB RID: 1227
		public static ScriptingInterfaceOfIShader.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x02000530 RID: 1328
		// (Invoke) Token: 0x06001AD9 RID: 6873
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFromResourceDelegate(byte[] shaderName);

		// Token: 0x02000531 RID: 1329
		// (Invoke) Token: 0x06001ADD RID: 6877
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ulong GetMaterialShaderFlagMaskDelegate(UIntPtr shaderPointer, byte[] flagName, [MarshalAs(UnmanagedType.U1)] bool showError);

		// Token: 0x02000532 RID: 1330
		// (Invoke) Token: 0x06001AE1 RID: 6881
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr shaderPointer);

		// Token: 0x02000533 RID: 1331
		// (Invoke) Token: 0x06001AE5 RID: 6885
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr shaderPointer);
	}
}
