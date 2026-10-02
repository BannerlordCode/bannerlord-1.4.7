using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002A RID: 42
	internal class ScriptingInterfaceOfITableauView : ITableauView
	{
		// Token: 0x060005F6 RID: 1526 RVA: 0x0001950C File Offset: 0x0001770C
		public TableauView CreateTableauView(string viewName)
		{
			byte[] array = null;
			if (viewName != null)
			{
				int byteCount = ScriptingInterfaceOfITableauView._utf8.GetByteCount(viewName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITableauView._utf8.GetBytes(viewName, 0, viewName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITableauView.call_CreateTableauViewDelegate(array);
			TableauView tableauView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				tableauView = new TableauView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return tableauView;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00019598 File Offset: 0x00017798
		public void SetContinousRendering(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetContinousRenderingDelegate(pointer, value);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000195A6 File Offset: 0x000177A6
		public void SetDeleteAfterRendering(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetDeleteAfterRenderingDelegate(pointer, value);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000195B4 File Offset: 0x000177B4
		public void SetDoNotRenderThisFrame(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetDoNotRenderThisFrameDelegate(pointer, value);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000195C2 File Offset: 0x000177C2
		public void SetSortingEnabled(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfITableauView.call_SetSortingEnabledDelegate(pointer, value);
		}

		// Token: 0x04000549 RID: 1353
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400054A RID: 1354
		public static ScriptingInterfaceOfITableauView.CreateTableauViewDelegate call_CreateTableauViewDelegate;

		// Token: 0x0400054B RID: 1355
		public static ScriptingInterfaceOfITableauView.SetContinousRenderingDelegate call_SetContinousRenderingDelegate;

		// Token: 0x0400054C RID: 1356
		public static ScriptingInterfaceOfITableauView.SetDeleteAfterRenderingDelegate call_SetDeleteAfterRenderingDelegate;

		// Token: 0x0400054D RID: 1357
		public static ScriptingInterfaceOfITableauView.SetDoNotRenderThisFrameDelegate call_SetDoNotRenderThisFrameDelegate;

		// Token: 0x0400054E RID: 1358
		public static ScriptingInterfaceOfITableauView.SetSortingEnabledDelegate call_SetSortingEnabledDelegate;

		// Token: 0x020005AE RID: 1454
		// (Invoke) Token: 0x06001CD1 RID: 7377
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTableauViewDelegate(byte[] viewName);

		// Token: 0x020005AF RID: 1455
		// (Invoke) Token: 0x06001CD5 RID: 7381
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetContinousRenderingDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020005B0 RID: 1456
		// (Invoke) Token: 0x06001CD9 RID: 7385
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDeleteAfterRenderingDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020005B1 RID: 1457
		// (Invoke) Token: 0x06001CDD RID: 7389
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDoNotRenderThisFrameDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020005B2 RID: 1458
		// (Invoke) Token: 0x06001CE1 RID: 7393
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSortingEnabledDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);
	}
}
