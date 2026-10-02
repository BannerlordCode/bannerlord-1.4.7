using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200002F RID: 47
	internal class ScriptingInterfaceOfITwoDimensionView : ITwoDimensionView
	{
		// Token: 0x0600062E RID: 1582 RVA: 0x00019F23 File Offset: 0x00018123
		public bool AddCachedTextMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData)
		{
			return ScriptingInterfaceOfITwoDimensionView.call_AddCachedTextMeshDelegate(pointer, material, ref meshDrawData);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00019F32 File Offset: 0x00018132
		public void AddNewMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData)
		{
			ScriptingInterfaceOfITwoDimensionView.call_AddNewMeshDelegate(pointer, material, ref meshDrawData);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00019F41 File Offset: 0x00018141
		public void AddNewQuadMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData)
		{
			ScriptingInterfaceOfITwoDimensionView.call_AddNewQuadMeshDelegate(pointer, material, ref meshDrawData);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00019F50 File Offset: 0x00018150
		public void AddNewTextMesh(UIntPtr pointer, float[] vertices, float[] uvs, uint[] indices, int vertexCount, int indexCount, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData)
		{
			PinnedArrayData<float> pinnedArrayData = new PinnedArrayData<float>(vertices, false);
			IntPtr pointer2 = pinnedArrayData.Pointer;
			PinnedArrayData<float> pinnedArrayData2 = new PinnedArrayData<float>(uvs, false);
			IntPtr pointer3 = pinnedArrayData2.Pointer;
			PinnedArrayData<uint> pinnedArrayData3 = new PinnedArrayData<uint>(indices, false);
			IntPtr pointer4 = pinnedArrayData3.Pointer;
			ScriptingInterfaceOfITwoDimensionView.call_AddNewTextMeshDelegate(pointer, pointer2, pointer3, pointer4, vertexCount, indexCount, material, ref meshDrawData);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
			pinnedArrayData3.Dispose();
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00019FBE File Offset: 0x000181BE
		public void BeginFrame(UIntPtr pointer)
		{
			ScriptingInterfaceOfITwoDimensionView.call_BeginFrameDelegate(pointer);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00019FCB File Offset: 0x000181CB
		public void Clear(UIntPtr pointer)
		{
			ScriptingInterfaceOfITwoDimensionView.call_ClearDelegate(pointer);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00019FD8 File Offset: 0x000181D8
		public TwoDimensionView CreateTwoDimensionView(string viewName)
		{
			byte[] array = null;
			if (viewName != null)
			{
				int byteCount = ScriptingInterfaceOfITwoDimensionView._utf8.GetByteCount(viewName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITwoDimensionView._utf8.GetBytes(viewName, 0, viewName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITwoDimensionView.call_CreateTwoDimensionViewDelegate(array);
			TwoDimensionView twoDimensionView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				twoDimensionView = new TwoDimensionView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return twoDimensionView;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001A064 File Offset: 0x00018264
		public void EndFrame(UIntPtr pointer)
		{
			ScriptingInterfaceOfITwoDimensionView.call_EndFrameDelegate(pointer);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001A071 File Offset: 0x00018271
		public UIntPtr GetOrCreateMaterial(UIntPtr pointer, UIntPtr mainTexture, UIntPtr overlayTexture)
		{
			return ScriptingInterfaceOfITwoDimensionView.call_GetOrCreateMaterialDelegate(pointer, mainTexture, overlayTexture);
		}

		// Token: 0x0400057C RID: 1404
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400057D RID: 1405
		public static ScriptingInterfaceOfITwoDimensionView.AddCachedTextMeshDelegate call_AddCachedTextMeshDelegate;

		// Token: 0x0400057E RID: 1406
		public static ScriptingInterfaceOfITwoDimensionView.AddNewMeshDelegate call_AddNewMeshDelegate;

		// Token: 0x0400057F RID: 1407
		public static ScriptingInterfaceOfITwoDimensionView.AddNewQuadMeshDelegate call_AddNewQuadMeshDelegate;

		// Token: 0x04000580 RID: 1408
		public static ScriptingInterfaceOfITwoDimensionView.AddNewTextMeshDelegate call_AddNewTextMeshDelegate;

		// Token: 0x04000581 RID: 1409
		public static ScriptingInterfaceOfITwoDimensionView.BeginFrameDelegate call_BeginFrameDelegate;

		// Token: 0x04000582 RID: 1410
		public static ScriptingInterfaceOfITwoDimensionView.ClearDelegate call_ClearDelegate;

		// Token: 0x04000583 RID: 1411
		public static ScriptingInterfaceOfITwoDimensionView.CreateTwoDimensionViewDelegate call_CreateTwoDimensionViewDelegate;

		// Token: 0x04000584 RID: 1412
		public static ScriptingInterfaceOfITwoDimensionView.EndFrameDelegate call_EndFrameDelegate;

		// Token: 0x04000585 RID: 1413
		public static ScriptingInterfaceOfITwoDimensionView.GetOrCreateMaterialDelegate call_GetOrCreateMaterialDelegate;

		// Token: 0x020005DC RID: 1500
		// (Invoke) Token: 0x06001D89 RID: 7561
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool AddCachedTextMeshDelegate(UIntPtr pointer, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x020005DD RID: 1501
		// (Invoke) Token: 0x06001D8D RID: 7565
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNewMeshDelegate(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x020005DE RID: 1502
		// (Invoke) Token: 0x06001D91 RID: 7569
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNewQuadMeshDelegate(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x020005DF RID: 1503
		// (Invoke) Token: 0x06001D95 RID: 7573
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNewTextMeshDelegate(UIntPtr pointer, IntPtr vertices, IntPtr uvs, IntPtr indices, int vertexCount, int indexCount, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x020005E0 RID: 1504
		// (Invoke) Token: 0x06001D99 RID: 7577
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BeginFrameDelegate(UIntPtr pointer);

		// Token: 0x020005E1 RID: 1505
		// (Invoke) Token: 0x06001D9D RID: 7581
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearDelegate(UIntPtr pointer);

		// Token: 0x020005E2 RID: 1506
		// (Invoke) Token: 0x06001DA1 RID: 7585
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTwoDimensionViewDelegate(byte[] viewName);

		// Token: 0x020005E3 RID: 1507
		// (Invoke) Token: 0x06001DA5 RID: 7589
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EndFrameDelegate(UIntPtr pointer);

		// Token: 0x020005E4 RID: 1508
		// (Invoke) Token: 0x06001DA9 RID: 7593
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetOrCreateMaterialDelegate(UIntPtr pointer, UIntPtr mainTexture, UIntPtr overlayTexture);
	}
}
