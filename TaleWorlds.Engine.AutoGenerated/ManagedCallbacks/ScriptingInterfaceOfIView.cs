using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000032 RID: 50
	internal class ScriptingInterfaceOfIView : IView
	{
		// Token: 0x060006D7 RID: 1751 RVA: 0x0001BC7B File Offset: 0x00019E7B
		public void SetAutoDepthTargetCreation(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetAutoDepthTargetCreationDelegate(ptr, value);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001BC89 File Offset: 0x00019E89
		public void SetClearColor(UIntPtr ptr, uint rgba)
		{
			ScriptingInterfaceOfIView.call_SetClearColorDelegate(ptr, rgba);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001BC97 File Offset: 0x00019E97
		public void SetDebugRenderFunctionality(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetDebugRenderFunctionalityDelegate(ptr, value);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001BCA5 File Offset: 0x00019EA5
		public void SetDepthTarget(UIntPtr ptr, UIntPtr texture_ptr)
		{
			ScriptingInterfaceOfIView.call_SetDepthTargetDelegate(ptr, texture_ptr);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001BCB3 File Offset: 0x00019EB3
		public void SetEnable(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetEnableDelegate(ptr, value);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001BCC4 File Offset: 0x00019EC4
		public void SetFileNameToSaveResult(UIntPtr ptr, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIView._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIView._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIView.call_SetFileNameToSaveResultDelegate(ptr, array);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001BD20 File Offset: 0x00019F20
		public void SetFilePathToSaveResult(UIntPtr ptr, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIView._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIView._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIView.call_SetFilePathToSaveResultDelegate(ptr, array);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001BD7B File Offset: 0x00019F7B
		public void SetFileTypeToSave(UIntPtr ptr, int type)
		{
			ScriptingInterfaceOfIView.call_SetFileTypeToSaveDelegate(ptr, type);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001BD89 File Offset: 0x00019F89
		public void SetOffset(UIntPtr ptr, float x, float y)
		{
			ScriptingInterfaceOfIView.call_SetOffsetDelegate(ptr, x, y);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001BD98 File Offset: 0x00019F98
		public void SetRenderOnDemand(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetRenderOnDemandDelegate(ptr, value);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001BDA6 File Offset: 0x00019FA6
		public void SetRenderOption(UIntPtr ptr, int optionEnum, bool value)
		{
			ScriptingInterfaceOfIView.call_SetRenderOptionDelegate(ptr, optionEnum, value);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001BDB5 File Offset: 0x00019FB5
		public void SetRenderOrder(UIntPtr ptr, int value)
		{
			ScriptingInterfaceOfIView.call_SetRenderOrderDelegate(ptr, value);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001BDC3 File Offset: 0x00019FC3
		public void SetRenderTarget(UIntPtr ptr, UIntPtr texture_ptr)
		{
			ScriptingInterfaceOfIView.call_SetRenderTargetDelegate(ptr, texture_ptr);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001BDD1 File Offset: 0x00019FD1
		public void SetSaveFinalResultToDisk(UIntPtr ptr, bool value)
		{
			ScriptingInterfaceOfIView.call_SetSaveFinalResultToDiskDelegate(ptr, value);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001BDDF File Offset: 0x00019FDF
		public void SetScale(UIntPtr ptr, float x, float y)
		{
			ScriptingInterfaceOfIView.call_SetScaleDelegate(ptr, x, y);
		}

		// Token: 0x04000622 RID: 1570
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000623 RID: 1571
		public static ScriptingInterfaceOfIView.SetAutoDepthTargetCreationDelegate call_SetAutoDepthTargetCreationDelegate;

		// Token: 0x04000624 RID: 1572
		public static ScriptingInterfaceOfIView.SetClearColorDelegate call_SetClearColorDelegate;

		// Token: 0x04000625 RID: 1573
		public static ScriptingInterfaceOfIView.SetDebugRenderFunctionalityDelegate call_SetDebugRenderFunctionalityDelegate;

		// Token: 0x04000626 RID: 1574
		public static ScriptingInterfaceOfIView.SetDepthTargetDelegate call_SetDepthTargetDelegate;

		// Token: 0x04000627 RID: 1575
		public static ScriptingInterfaceOfIView.SetEnableDelegate call_SetEnableDelegate;

		// Token: 0x04000628 RID: 1576
		public static ScriptingInterfaceOfIView.SetFileNameToSaveResultDelegate call_SetFileNameToSaveResultDelegate;

		// Token: 0x04000629 RID: 1577
		public static ScriptingInterfaceOfIView.SetFilePathToSaveResultDelegate call_SetFilePathToSaveResultDelegate;

		// Token: 0x0400062A RID: 1578
		public static ScriptingInterfaceOfIView.SetFileTypeToSaveDelegate call_SetFileTypeToSaveDelegate;

		// Token: 0x0400062B RID: 1579
		public static ScriptingInterfaceOfIView.SetOffsetDelegate call_SetOffsetDelegate;

		// Token: 0x0400062C RID: 1580
		public static ScriptingInterfaceOfIView.SetRenderOnDemandDelegate call_SetRenderOnDemandDelegate;

		// Token: 0x0400062D RID: 1581
		public static ScriptingInterfaceOfIView.SetRenderOptionDelegate call_SetRenderOptionDelegate;

		// Token: 0x0400062E RID: 1582
		public static ScriptingInterfaceOfIView.SetRenderOrderDelegate call_SetRenderOrderDelegate;

		// Token: 0x0400062F RID: 1583
		public static ScriptingInterfaceOfIView.SetRenderTargetDelegate call_SetRenderTargetDelegate;

		// Token: 0x04000630 RID: 1584
		public static ScriptingInterfaceOfIView.SetSaveFinalResultToDiskDelegate call_SetSaveFinalResultToDiskDelegate;

		// Token: 0x04000631 RID: 1585
		public static ScriptingInterfaceOfIView.SetScaleDelegate call_SetScaleDelegate;

		// Token: 0x0200067F RID: 1663
		// (Invoke) Token: 0x06002015 RID: 8213
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAutoDepthTargetCreationDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000680 RID: 1664
		// (Invoke) Token: 0x06002019 RID: 8217
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetClearColorDelegate(UIntPtr ptr, uint rgba);

		// Token: 0x02000681 RID: 1665
		// (Invoke) Token: 0x0600201D RID: 8221
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDebugRenderFunctionalityDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000682 RID: 1666
		// (Invoke) Token: 0x06002021 RID: 8225
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDepthTargetDelegate(UIntPtr ptr, UIntPtr texture_ptr);

		// Token: 0x02000683 RID: 1667
		// (Invoke) Token: 0x06002025 RID: 8229
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEnableDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000684 RID: 1668
		// (Invoke) Token: 0x06002029 RID: 8233
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFileNameToSaveResultDelegate(UIntPtr ptr, byte[] name);

		// Token: 0x02000685 RID: 1669
		// (Invoke) Token: 0x0600202D RID: 8237
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFilePathToSaveResultDelegate(UIntPtr ptr, byte[] name);

		// Token: 0x02000686 RID: 1670
		// (Invoke) Token: 0x06002031 RID: 8241
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFileTypeToSaveDelegate(UIntPtr ptr, int type);

		// Token: 0x02000687 RID: 1671
		// (Invoke) Token: 0x06002035 RID: 8245
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetOffsetDelegate(UIntPtr ptr, float x, float y);

		// Token: 0x02000688 RID: 1672
		// (Invoke) Token: 0x06002039 RID: 8249
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderOnDemandDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000689 RID: 1673
		// (Invoke) Token: 0x0600203D RID: 8253
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderOptionDelegate(UIntPtr ptr, int optionEnum, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200068A RID: 1674
		// (Invoke) Token: 0x06002041 RID: 8257
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderOrderDelegate(UIntPtr ptr, int value);

		// Token: 0x0200068B RID: 1675
		// (Invoke) Token: 0x06002045 RID: 8261
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRenderTargetDelegate(UIntPtr ptr, UIntPtr texture_ptr);

		// Token: 0x0200068C RID: 1676
		// (Invoke) Token: 0x06002049 RID: 8265
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSaveFinalResultToDiskDelegate(UIntPtr ptr, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200068D RID: 1677
		// (Invoke) Token: 0x0600204D RID: 8269
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetScaleDelegate(UIntPtr ptr, float x, float y);
	}
}
