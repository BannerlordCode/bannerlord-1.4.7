using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000008 RID: 8
	internal class ScriptingInterfaceOfIAsyncTask : IAsyncTask
	{
		// Token: 0x06000068 RID: 104 RVA: 0x0000EC3C File Offset: 0x0000CE3C
		public AsyncTask CreateWithDelegate(ManagedDelegate function, bool isBackground)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIAsyncTask.call_CreateWithDelegateDelegate((function != null) ? function.GetManagedId() : 0, isBackground);
			AsyncTask asyncTask = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				asyncTask = new AsyncTask(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return asyncTask;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000EC92 File Offset: 0x0000CE92
		public void Invoke(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIAsyncTask.call_InvokeDelegate(Pointer);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000EC9F File Offset: 0x0000CE9F
		public void Wait(UIntPtr Pointer)
		{
			ScriptingInterfaceOfIAsyncTask.call_WaitDelegate(Pointer);
		}

		// Token: 0x04000002 RID: 2
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000003 RID: 3
		public static ScriptingInterfaceOfIAsyncTask.CreateWithDelegateDelegate call_CreateWithDelegateDelegate;

		// Token: 0x04000004 RID: 4
		public static ScriptingInterfaceOfIAsyncTask.InvokeDelegate call_InvokeDelegate;

		// Token: 0x04000005 RID: 5
		public static ScriptingInterfaceOfIAsyncTask.WaitDelegate call_WaitDelegate;

		// Token: 0x02000089 RID: 137
		// (Invoke) Token: 0x0600083D RID: 2109
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateWithDelegateDelegate(int function, [MarshalAs(UnmanagedType.U1)] bool isBackground);

		// Token: 0x0200008A RID: 138
		// (Invoke) Token: 0x06000841 RID: 2113
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InvokeDelegate(UIntPtr Pointer);

		// Token: 0x0200008B RID: 139
		// (Invoke) Token: 0x06000845 RID: 2117
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void WaitDelegate(UIntPtr Pointer);
	}
}
