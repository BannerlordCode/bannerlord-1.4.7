using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000013 RID: 19
	internal class ScriptingInterfaceOfIHighlights : IHighlights
	{
		// Token: 0x06000252 RID: 594 RVA: 0x00011DF0 File Offset: 0x0000FFF0
		public void AddHighlight(string id, string name)
		{
			byte[] array = null;
			if (id != null)
			{
				int byteCount = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(id, 0, id.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (name != null)
			{
				int byteCount2 = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(name);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(name, 0, name.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIHighlights.call_AddHighlightDelegate(array, array2);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00011E90 File Offset: 0x00010090
		public void CloseGroup(string id, bool destroy)
		{
			byte[] array = null;
			if (id != null)
			{
				int byteCount = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(id, 0, id.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIHighlights.call_CloseGroupDelegate(array, destroy);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00011EEB File Offset: 0x000100EB
		public void Initialize()
		{
			ScriptingInterfaceOfIHighlights.call_InitializeDelegate();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00011EF8 File Offset: 0x000100F8
		public void OpenGroup(string id)
		{
			byte[] array = null;
			if (id != null)
			{
				int byteCount = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(id, 0, id.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIHighlights.call_OpenGroupDelegate(array);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00011F54 File Offset: 0x00010154
		public void OpenSummary(string groups)
		{
			byte[] array = null;
			if (groups != null)
			{
				int byteCount = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(groups);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(groups, 0, groups.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIHighlights.call_OpenSummaryDelegate(array);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00011FB0 File Offset: 0x000101B0
		public void RemoveHighlight(string id)
		{
			byte[] array = null;
			if (id != null)
			{
				int byteCount = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(id, 0, id.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIHighlights.call_RemoveHighlightDelegate(array);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0001200C File Offset: 0x0001020C
		public void SaveScreenshot(string highlightId, string groupId)
		{
			byte[] array = null;
			if (highlightId != null)
			{
				int byteCount = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(highlightId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(highlightId, 0, highlightId.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (groupId != null)
			{
				int byteCount2 = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(groupId);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(groupId, 0, groupId.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIHighlights.call_SaveScreenshotDelegate(array, array2);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000120AC File Offset: 0x000102AC
		public void SaveVideo(string highlightId, string groupId, int startDelta, int endDelta)
		{
			byte[] array = null;
			if (highlightId != null)
			{
				int byteCount = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(highlightId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(highlightId, 0, highlightId.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (groupId != null)
			{
				int byteCount2 = ScriptingInterfaceOfIHighlights._utf8.GetByteCount(groupId);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIHighlights._utf8.GetBytes(groupId, 0, groupId.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIHighlights.call_SaveVideoDelegate(array, array2, startDelta, endDelta);
		}

		// Token: 0x040001CE RID: 462
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001CF RID: 463
		public static ScriptingInterfaceOfIHighlights.AddHighlightDelegate call_AddHighlightDelegate;

		// Token: 0x040001D0 RID: 464
		public static ScriptingInterfaceOfIHighlights.CloseGroupDelegate call_CloseGroupDelegate;

		// Token: 0x040001D1 RID: 465
		public static ScriptingInterfaceOfIHighlights.InitializeDelegate call_InitializeDelegate;

		// Token: 0x040001D2 RID: 466
		public static ScriptingInterfaceOfIHighlights.OpenGroupDelegate call_OpenGroupDelegate;

		// Token: 0x040001D3 RID: 467
		public static ScriptingInterfaceOfIHighlights.OpenSummaryDelegate call_OpenSummaryDelegate;

		// Token: 0x040001D4 RID: 468
		public static ScriptingInterfaceOfIHighlights.RemoveHighlightDelegate call_RemoveHighlightDelegate;

		// Token: 0x040001D5 RID: 469
		public static ScriptingInterfaceOfIHighlights.SaveScreenshotDelegate call_SaveScreenshotDelegate;

		// Token: 0x040001D6 RID: 470
		public static ScriptingInterfaceOfIHighlights.SaveVideoDelegate call_SaveVideoDelegate;

		// Token: 0x0200024A RID: 586
		// (Invoke) Token: 0x06000F41 RID: 3905
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddHighlightDelegate(byte[] id, byte[] name);

		// Token: 0x0200024B RID: 587
		// (Invoke) Token: 0x06000F45 RID: 3909
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CloseGroupDelegate(byte[] id, [MarshalAs(UnmanagedType.U1)] bool destroy);

		// Token: 0x0200024C RID: 588
		// (Invoke) Token: 0x06000F49 RID: 3913
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitializeDelegate();

		// Token: 0x0200024D RID: 589
		// (Invoke) Token: 0x06000F4D RID: 3917
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OpenGroupDelegate(byte[] id);

		// Token: 0x0200024E RID: 590
		// (Invoke) Token: 0x06000F51 RID: 3921
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void OpenSummaryDelegate(byte[] groups);

		// Token: 0x0200024F RID: 591
		// (Invoke) Token: 0x06000F55 RID: 3925
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveHighlightDelegate(byte[] id);

		// Token: 0x02000250 RID: 592
		// (Invoke) Token: 0x06000F59 RID: 3929
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SaveScreenshotDelegate(byte[] highlightId, byte[] groupId);

		// Token: 0x02000251 RID: 593
		// (Invoke) Token: 0x06000F5D RID: 3933
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SaveVideoDelegate(byte[] highlightId, byte[] groupId, int startDelta, int endDelta);
	}
}
