using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000023 RID: 35
	internal class ScriptingInterfaceOfIMBWindowManager : IMBWindowManager
	{
		// Token: 0x0600036F RID: 879 RVA: 0x0000D9CE File Offset: 0x0000BBCE
		public void DontChangeCursorPos()
		{
			ScriptingInterfaceOfIMBWindowManager.call_DontChangeCursorPosDelegate();
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000D9DA File Offset: 0x0000BBDA
		public void EraseMessageLines()
		{
			ScriptingInterfaceOfIMBWindowManager.call_EraseMessageLinesDelegate();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000D9E6 File Offset: 0x0000BBE6
		public Vec2 GetScreenResolution()
		{
			return ScriptingInterfaceOfIMBWindowManager.call_GetScreenResolutionDelegate();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000D9F2 File Offset: 0x0000BBF2
		public void PreDisplay()
		{
			ScriptingInterfaceOfIMBWindowManager.call_PreDisplayDelegate();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000D9FE File Offset: 0x0000BBFE
		public void ScreenToWorld(UIntPtr pointer, float screenX, float screenY, float z, ref Vec3 worldSpacePosition)
		{
			ScriptingInterfaceOfIMBWindowManager.call_ScreenToWorldDelegate(pointer, screenX, screenY, z, ref worldSpacePosition);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000DA11 File Offset: 0x0000BC11
		public float WorldToScreen(UIntPtr cameraPointer, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return ScriptingInterfaceOfIMBWindowManager.call_WorldToScreenDelegate(cameraPointer, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000DA24 File Offset: 0x0000BC24
		public float WorldToScreenWithFixedZ(UIntPtr cameraPointer, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w)
		{
			return ScriptingInterfaceOfIMBWindowManager.call_WorldToScreenWithFixedZDelegate(cameraPointer, cameraPosition, worldSpacePosition, ref screenX, ref screenY, ref w);
		}

		// Token: 0x040002DA RID: 730
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002DB RID: 731
		public static ScriptingInterfaceOfIMBWindowManager.DontChangeCursorPosDelegate call_DontChangeCursorPosDelegate;

		// Token: 0x040002DC RID: 732
		public static ScriptingInterfaceOfIMBWindowManager.EraseMessageLinesDelegate call_EraseMessageLinesDelegate;

		// Token: 0x040002DD RID: 733
		public static ScriptingInterfaceOfIMBWindowManager.GetScreenResolutionDelegate call_GetScreenResolutionDelegate;

		// Token: 0x040002DE RID: 734
		public static ScriptingInterfaceOfIMBWindowManager.PreDisplayDelegate call_PreDisplayDelegate;

		// Token: 0x040002DF RID: 735
		public static ScriptingInterfaceOfIMBWindowManager.ScreenToWorldDelegate call_ScreenToWorldDelegate;

		// Token: 0x040002E0 RID: 736
		public static ScriptingInterfaceOfIMBWindowManager.WorldToScreenDelegate call_WorldToScreenDelegate;

		// Token: 0x040002E1 RID: 737
		public static ScriptingInterfaceOfIMBWindowManager.WorldToScreenWithFixedZDelegate call_WorldToScreenWithFixedZDelegate;

		// Token: 0x02000331 RID: 817
		// (Invoke) Token: 0x06000FB2 RID: 4018
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DontChangeCursorPosDelegate();

		// Token: 0x02000332 RID: 818
		// (Invoke) Token: 0x06000FB6 RID: 4022
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EraseMessageLinesDelegate();

		// Token: 0x02000333 RID: 819
		// (Invoke) Token: 0x06000FBA RID: 4026
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetScreenResolutionDelegate();

		// Token: 0x02000334 RID: 820
		// (Invoke) Token: 0x06000FBE RID: 4030
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PreDisplayDelegate();

		// Token: 0x02000335 RID: 821
		// (Invoke) Token: 0x06000FC2 RID: 4034
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ScreenToWorldDelegate(UIntPtr pointer, float screenX, float screenY, float z, ref Vec3 worldSpacePosition);

		// Token: 0x02000336 RID: 822
		// (Invoke) Token: 0x06000FC6 RID: 4038
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float WorldToScreenDelegate(UIntPtr cameraPointer, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);

		// Token: 0x02000337 RID: 823
		// (Invoke) Token: 0x06000FCA RID: 4042
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float WorldToScreenWithFixedZDelegate(UIntPtr cameraPointer, Vec3 cameraPosition, Vec3 worldSpacePosition, ref float screenX, ref float screenY, ref float w);
	}
}
