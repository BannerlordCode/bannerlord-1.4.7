using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002D RID: 45
	internal class ScriptingInterfaceOfIThumbnailCreatorView : IThumbnailCreatorView
	{
		// Token: 0x06000620 RID: 1568 RVA: 0x00019D48 File Offset: 0x00017F48
		public void CancelRequest(UIntPtr pointer, string render_id)
		{
			byte[] array = null;
			if (render_id != null)
			{
				int byteCount = ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetByteCount(render_id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetBytes(render_id, 0, render_id.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIThumbnailCreatorView.call_CancelRequestDelegate(pointer, array);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00019DA3 File Offset: 0x00017FA3
		public void ClearRequests(UIntPtr pointer)
		{
			ScriptingInterfaceOfIThumbnailCreatorView.call_ClearRequestsDelegate(pointer);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00019DB0 File Offset: 0x00017FB0
		public ThumbnailCreatorView CreateThumbnailCreatorView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIThumbnailCreatorView.call_CreateThumbnailCreatorViewDelegate();
			ThumbnailCreatorView thumbnailCreatorView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				thumbnailCreatorView = new ThumbnailCreatorView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return thumbnailCreatorView;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00019DF9 File Offset: 0x00017FF9
		public int GetNumberOfPendingRequests(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIThumbnailCreatorView.call_GetNumberOfPendingRequestsDelegate(pointer);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00019E06 File Offset: 0x00018006
		public bool IsMemoryCleared(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIThumbnailCreatorView.call_IsMemoryClearedDelegate(pointer);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00019E14 File Offset: 0x00018014
		public void RegisterCachedEntity(UIntPtr pointer, UIntPtr scene, UIntPtr entity_ptr, string cacheId)
		{
			byte[] array = null;
			if (cacheId != null)
			{
				int byteCount = ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetByteCount(cacheId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetBytes(cacheId, 0, cacheId.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIThumbnailCreatorView.call_RegisterCachedEntityDelegate(pointer, scene, entity_ptr, array);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00019E75 File Offset: 0x00018075
		public void RegisterRenderRequest(UIntPtr pointer, ref ThumbnailRenderRequest request)
		{
			ScriptingInterfaceOfIThumbnailCreatorView.call_RegisterRenderRequestDelegate(pointer, ref request);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00019E83 File Offset: 0x00018083
		public void RegisterScene(UIntPtr pointer, UIntPtr scene_ptr, bool use_postfx)
		{
			ScriptingInterfaceOfIThumbnailCreatorView.call_RegisterSceneDelegate(pointer, scene_ptr, use_postfx);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00019E94 File Offset: 0x00018094
		public void UnregisterCachedEntity(UIntPtr pointer, string cacheId)
		{
			byte[] array = null;
			if (cacheId != null)
			{
				int byteCount = ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetByteCount(cacheId);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIThumbnailCreatorView._utf8.GetBytes(cacheId, 0, cacheId.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIThumbnailCreatorView.call_UnregisterCachedEntityDelegate(pointer, array);
		}

		// Token: 0x04000570 RID: 1392
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000571 RID: 1393
		public static ScriptingInterfaceOfIThumbnailCreatorView.CancelRequestDelegate call_CancelRequestDelegate;

		// Token: 0x04000572 RID: 1394
		public static ScriptingInterfaceOfIThumbnailCreatorView.ClearRequestsDelegate call_ClearRequestsDelegate;

		// Token: 0x04000573 RID: 1395
		public static ScriptingInterfaceOfIThumbnailCreatorView.CreateThumbnailCreatorViewDelegate call_CreateThumbnailCreatorViewDelegate;

		// Token: 0x04000574 RID: 1396
		public static ScriptingInterfaceOfIThumbnailCreatorView.GetNumberOfPendingRequestsDelegate call_GetNumberOfPendingRequestsDelegate;

		// Token: 0x04000575 RID: 1397
		public static ScriptingInterfaceOfIThumbnailCreatorView.IsMemoryClearedDelegate call_IsMemoryClearedDelegate;

		// Token: 0x04000576 RID: 1398
		public static ScriptingInterfaceOfIThumbnailCreatorView.RegisterCachedEntityDelegate call_RegisterCachedEntityDelegate;

		// Token: 0x04000577 RID: 1399
		public static ScriptingInterfaceOfIThumbnailCreatorView.RegisterRenderRequestDelegate call_RegisterRenderRequestDelegate;

		// Token: 0x04000578 RID: 1400
		public static ScriptingInterfaceOfIThumbnailCreatorView.RegisterSceneDelegate call_RegisterSceneDelegate;

		// Token: 0x04000579 RID: 1401
		public static ScriptingInterfaceOfIThumbnailCreatorView.UnregisterCachedEntityDelegate call_UnregisterCachedEntityDelegate;

		// Token: 0x020005D2 RID: 1490
		// (Invoke) Token: 0x06001D61 RID: 7521
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CancelRequestDelegate(UIntPtr pointer, byte[] render_id);

		// Token: 0x020005D3 RID: 1491
		// (Invoke) Token: 0x06001D65 RID: 7525
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearRequestsDelegate(UIntPtr pointer);

		// Token: 0x020005D4 RID: 1492
		// (Invoke) Token: 0x06001D69 RID: 7529
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateThumbnailCreatorViewDelegate();

		// Token: 0x020005D5 RID: 1493
		// (Invoke) Token: 0x06001D6D RID: 7533
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumberOfPendingRequestsDelegate(UIntPtr pointer);

		// Token: 0x020005D6 RID: 1494
		// (Invoke) Token: 0x06001D71 RID: 7537
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsMemoryClearedDelegate(UIntPtr pointer);

		// Token: 0x020005D7 RID: 1495
		// (Invoke) Token: 0x06001D75 RID: 7541
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RegisterCachedEntityDelegate(UIntPtr pointer, UIntPtr scene, UIntPtr entity_ptr, byte[] cacheId);

		// Token: 0x020005D8 RID: 1496
		// (Invoke) Token: 0x06001D79 RID: 7545
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RegisterRenderRequestDelegate(UIntPtr pointer, ref ThumbnailRenderRequest request);

		// Token: 0x020005D9 RID: 1497
		// (Invoke) Token: 0x06001D7D RID: 7549
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RegisterSceneDelegate(UIntPtr pointer, UIntPtr scene_ptr, [MarshalAs(UnmanagedType.U1)] bool use_postfx);

		// Token: 0x020005DA RID: 1498
		// (Invoke) Token: 0x06001D81 RID: 7553
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnregisterCachedEntityDelegate(UIntPtr pointer, byte[] cacheId);
	}
}
