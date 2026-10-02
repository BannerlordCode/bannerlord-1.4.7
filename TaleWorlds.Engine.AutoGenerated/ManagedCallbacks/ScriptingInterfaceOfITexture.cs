using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200002B RID: 43
	internal class ScriptingInterfaceOfITexture : ITexture
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x000195E4 File Offset: 0x000177E4
		public Texture CheckAndGetFromResource(string textureName)
		{
			byte[] array = null;
			if (textureName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(textureName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(textureName, 0, textureName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CheckAndGetFromResourceDelegate(array);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00019670 File Offset: 0x00017870
		public Texture CreateDepthTarget(string name, int width, int height)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateDepthTargetDelegate(array, width, height);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00019700 File Offset: 0x00017900
		public Texture CreateFromByteArray(byte[] data, int width, int height)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(data, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (data != null) ? data.Length : 0);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateFromByteArrayDelegate(managedArray, width, height);
			pinnedArrayData.Dispose();
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00019778 File Offset: 0x00017978
		public Texture CreateFromMemory(byte[] data)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(data, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (data != null) ? data.Length : 0);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateFromMemoryDelegate(managedArray);
			pinnedArrayData.Dispose();
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000197F0 File Offset: 0x000179F0
		public Texture CreateRenderTarget(string name, int width, int height, bool autoMipmaps, bool isTableau, bool createUninitialized, bool always_valid)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateRenderTargetDelegate(array, width, height, autoMipmaps, isTableau, createUninitialized, always_valid);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00019888 File Offset: 0x00017A88
		public Texture CreateTextureFromPath(PlatformFilePath filePath)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_CreateTextureFromPathDelegate(filePath);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000198D2 File Offset: 0x00017AD2
		public void GetCurObject(UIntPtr texturePointer, bool blocking)
		{
			ScriptingInterfaceOfITexture.call_GetCurObjectDelegate(texturePointer, blocking);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000198E0 File Offset: 0x00017AE0
		public Texture GetFromResource(string textureName)
		{
			byte[] array = null;
			if (textureName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(textureName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(textureName, 0, textureName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_GetFromResourceDelegate(array);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001996C File Offset: 0x00017B6C
		public int GetHeight(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_GetHeightDelegate(texturePointer);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00019979 File Offset: 0x00017B79
		public int GetMemorySize(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_GetMemorySizeDelegate(texturePointer);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00019986 File Offset: 0x00017B86
		public string GetName(UIntPtr texturePointer)
		{
			if (ScriptingInterfaceOfITexture.call_GetNameDelegate(texturePointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000199A0 File Offset: 0x00017BA0
		public void GetPixelData(UIntPtr texturePointer, byte[] bytes)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(bytes, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (bytes != null) ? bytes.Length : 0);
			ScriptingInterfaceOfITexture.call_GetPixelDataDelegate(texturePointer, managedArray);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000199E2 File Offset: 0x00017BE2
		public RenderTargetComponent GetRenderTargetComponent(UIntPtr texturePointer)
		{
			return DotNetObject.GetManagedObjectWithId(ScriptingInterfaceOfITexture.call_GetRenderTargetComponentDelegate(texturePointer)) as RenderTargetComponent;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000199F9 File Offset: 0x00017BF9
		public void GetSDFBoundingBoxData(UIntPtr texturePointer, ref Vec3 min, ref Vec3 max)
		{
			ScriptingInterfaceOfITexture.call_GetSDFBoundingBoxDataDelegate(texturePointer, ref min, ref max);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00019A08 File Offset: 0x00017C08
		public TableauView GetTableauView(UIntPtr texturePointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_GetTableauViewDelegate(texturePointer);
			TableauView tableauView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				tableauView = new TableauView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return tableauView;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00019A52 File Offset: 0x00017C52
		public int GetWidth(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_GetWidthDelegate(texturePointer);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00019A5F File Offset: 0x00017C5F
		public bool IsLoaded(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_IsLoadedDelegate(texturePointer);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00019A6C File Offset: 0x00017C6C
		public bool IsRenderTarget(UIntPtr texturePointer)
		{
			return ScriptingInterfaceOfITexture.call_IsRenderTargetDelegate(texturePointer);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00019A7C File Offset: 0x00017C7C
		public Texture LoadTextureFromPath(string fileName, string folder)
		{
			byte[] array = null;
			if (fileName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(fileName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(fileName, 0, fileName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (folder != null)
			{
				int byteCount2 = ScriptingInterfaceOfITexture._utf8.GetByteCount(folder);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(folder, 0, folder.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITexture.call_LoadTextureFromPathDelegate(array, array2);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00019B53 File Offset: 0x00017D53
		public void Release(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_ReleaseDelegate(texturePointer);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00019B60 File Offset: 0x00017D60
		public void ReleaseAfterNumberOfFrames(UIntPtr texturePointer, int numberOfFrames)
		{
			ScriptingInterfaceOfITexture.call_ReleaseAfterNumberOfFramesDelegate(texturePointer, numberOfFrames);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00019B6E File Offset: 0x00017D6E
		public void ReleaseGpuMemories()
		{
			ScriptingInterfaceOfITexture.call_ReleaseGpuMemoriesDelegate();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00019B7A File Offset: 0x00017D7A
		public void ReleaseNextFrame(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_ReleaseNextFrameDelegate(texturePointer);
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00019B87 File Offset: 0x00017D87
		public void RemoveContinousTableauTexture(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_RemoveContinousTableauTextureDelegate(texturePointer);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00019B94 File Offset: 0x00017D94
		public void SaveTextureAsAlwaysValid(UIntPtr texturePointer)
		{
			ScriptingInterfaceOfITexture.call_SaveTextureAsAlwaysValidDelegate(texturePointer);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00019BA4 File Offset: 0x00017DA4
		public void SaveToFile(UIntPtr texturePointer, string fileName, bool isRelativePath)
		{
			byte[] array = null;
			if (fileName != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(fileName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(fileName, 0, fileName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITexture.call_SaveToFileDelegate(texturePointer, array, isRelativePath);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00019C00 File Offset: 0x00017E00
		public void SetName(UIntPtr texturePointer, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITexture.call_SetNameDelegate(texturePointer, array);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00019C5B File Offset: 0x00017E5B
		public void SetTableauView(UIntPtr texturePointer, UIntPtr tableauView)
		{
			ScriptingInterfaceOfITexture.call_SetTableauViewDelegate(texturePointer, tableauView);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00019C6C File Offset: 0x00017E6C
		public void TransformRenderTargetToResourceTexture(UIntPtr texturePointer, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfITexture._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITexture._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITexture.call_TransformRenderTargetToResourceTextureDelegate(texturePointer, array);
		}

		// Token: 0x0400054F RID: 1359
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000550 RID: 1360
		public static ScriptingInterfaceOfITexture.CheckAndGetFromResourceDelegate call_CheckAndGetFromResourceDelegate;

		// Token: 0x04000551 RID: 1361
		public static ScriptingInterfaceOfITexture.CreateDepthTargetDelegate call_CreateDepthTargetDelegate;

		// Token: 0x04000552 RID: 1362
		public static ScriptingInterfaceOfITexture.CreateFromByteArrayDelegate call_CreateFromByteArrayDelegate;

		// Token: 0x04000553 RID: 1363
		public static ScriptingInterfaceOfITexture.CreateFromMemoryDelegate call_CreateFromMemoryDelegate;

		// Token: 0x04000554 RID: 1364
		public static ScriptingInterfaceOfITexture.CreateRenderTargetDelegate call_CreateRenderTargetDelegate;

		// Token: 0x04000555 RID: 1365
		public static ScriptingInterfaceOfITexture.CreateTextureFromPathDelegate call_CreateTextureFromPathDelegate;

		// Token: 0x04000556 RID: 1366
		public static ScriptingInterfaceOfITexture.GetCurObjectDelegate call_GetCurObjectDelegate;

		// Token: 0x04000557 RID: 1367
		public static ScriptingInterfaceOfITexture.GetFromResourceDelegate call_GetFromResourceDelegate;

		// Token: 0x04000558 RID: 1368
		public static ScriptingInterfaceOfITexture.GetHeightDelegate call_GetHeightDelegate;

		// Token: 0x04000559 RID: 1369
		public static ScriptingInterfaceOfITexture.GetMemorySizeDelegate call_GetMemorySizeDelegate;

		// Token: 0x0400055A RID: 1370
		public static ScriptingInterfaceOfITexture.GetNameDelegate call_GetNameDelegate;

		// Token: 0x0400055B RID: 1371
		public static ScriptingInterfaceOfITexture.GetPixelDataDelegate call_GetPixelDataDelegate;

		// Token: 0x0400055C RID: 1372
		public static ScriptingInterfaceOfITexture.GetRenderTargetComponentDelegate call_GetRenderTargetComponentDelegate;

		// Token: 0x0400055D RID: 1373
		public static ScriptingInterfaceOfITexture.GetSDFBoundingBoxDataDelegate call_GetSDFBoundingBoxDataDelegate;

		// Token: 0x0400055E RID: 1374
		public static ScriptingInterfaceOfITexture.GetTableauViewDelegate call_GetTableauViewDelegate;

		// Token: 0x0400055F RID: 1375
		public static ScriptingInterfaceOfITexture.GetWidthDelegate call_GetWidthDelegate;

		// Token: 0x04000560 RID: 1376
		public static ScriptingInterfaceOfITexture.IsLoadedDelegate call_IsLoadedDelegate;

		// Token: 0x04000561 RID: 1377
		public static ScriptingInterfaceOfITexture.IsRenderTargetDelegate call_IsRenderTargetDelegate;

		// Token: 0x04000562 RID: 1378
		public static ScriptingInterfaceOfITexture.LoadTextureFromPathDelegate call_LoadTextureFromPathDelegate;

		// Token: 0x04000563 RID: 1379
		public static ScriptingInterfaceOfITexture.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x04000564 RID: 1380
		public static ScriptingInterfaceOfITexture.ReleaseAfterNumberOfFramesDelegate call_ReleaseAfterNumberOfFramesDelegate;

		// Token: 0x04000565 RID: 1381
		public static ScriptingInterfaceOfITexture.ReleaseGpuMemoriesDelegate call_ReleaseGpuMemoriesDelegate;

		// Token: 0x04000566 RID: 1382
		public static ScriptingInterfaceOfITexture.ReleaseNextFrameDelegate call_ReleaseNextFrameDelegate;

		// Token: 0x04000567 RID: 1383
		public static ScriptingInterfaceOfITexture.RemoveContinousTableauTextureDelegate call_RemoveContinousTableauTextureDelegate;

		// Token: 0x04000568 RID: 1384
		public static ScriptingInterfaceOfITexture.SaveTextureAsAlwaysValidDelegate call_SaveTextureAsAlwaysValidDelegate;

		// Token: 0x04000569 RID: 1385
		public static ScriptingInterfaceOfITexture.SaveToFileDelegate call_SaveToFileDelegate;

		// Token: 0x0400056A RID: 1386
		public static ScriptingInterfaceOfITexture.SetNameDelegate call_SetNameDelegate;

		// Token: 0x0400056B RID: 1387
		public static ScriptingInterfaceOfITexture.SetTableauViewDelegate call_SetTableauViewDelegate;

		// Token: 0x0400056C RID: 1388
		public static ScriptingInterfaceOfITexture.TransformRenderTargetToResourceTextureDelegate call_TransformRenderTargetToResourceTextureDelegate;

		// Token: 0x020005B3 RID: 1459
		// (Invoke) Token: 0x06001CE5 RID: 7397
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CheckAndGetFromResourceDelegate(byte[] textureName);

		// Token: 0x020005B4 RID: 1460
		// (Invoke) Token: 0x06001CE9 RID: 7401
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateDepthTargetDelegate(byte[] name, int width, int height);

		// Token: 0x020005B5 RID: 1461
		// (Invoke) Token: 0x06001CED RID: 7405
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromByteArrayDelegate(ManagedArray data, int width, int height);

		// Token: 0x020005B6 RID: 1462
		// (Invoke) Token: 0x06001CF1 RID: 7409
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromMemoryDelegate(ManagedArray data);

		// Token: 0x020005B7 RID: 1463
		// (Invoke) Token: 0x06001CF5 RID: 7413
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateRenderTargetDelegate(byte[] name, int width, int height, [MarshalAs(UnmanagedType.U1)] bool autoMipmaps, [MarshalAs(UnmanagedType.U1)] bool isTableau, [MarshalAs(UnmanagedType.U1)] bool createUninitialized, [MarshalAs(UnmanagedType.U1)] bool always_valid);

		// Token: 0x020005B8 RID: 1464
		// (Invoke) Token: 0x06001CF9 RID: 7417
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTextureFromPathDelegate(PlatformFilePath filePath);

		// Token: 0x020005B9 RID: 1465
		// (Invoke) Token: 0x06001CFD RID: 7421
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetCurObjectDelegate(UIntPtr texturePointer, [MarshalAs(UnmanagedType.U1)] bool blocking);

		// Token: 0x020005BA RID: 1466
		// (Invoke) Token: 0x06001D01 RID: 7425
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFromResourceDelegate(byte[] textureName);

		// Token: 0x020005BB RID: 1467
		// (Invoke) Token: 0x06001D05 RID: 7429
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetHeightDelegate(UIntPtr texturePointer);

		// Token: 0x020005BC RID: 1468
		// (Invoke) Token: 0x06001D09 RID: 7433
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMemorySizeDelegate(UIntPtr texturePointer);

		// Token: 0x020005BD RID: 1469
		// (Invoke) Token: 0x06001D0D RID: 7437
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr texturePointer);

		// Token: 0x020005BE RID: 1470
		// (Invoke) Token: 0x06001D11 RID: 7441
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetPixelDataDelegate(UIntPtr texturePointer, ManagedArray bytes);

		// Token: 0x020005BF RID: 1471
		// (Invoke) Token: 0x06001D15 RID: 7445
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetRenderTargetComponentDelegate(UIntPtr texturePointer);

		// Token: 0x020005C0 RID: 1472
		// (Invoke) Token: 0x06001D19 RID: 7449
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSDFBoundingBoxDataDelegate(UIntPtr texturePointer, ref Vec3 min, ref Vec3 max);

		// Token: 0x020005C1 RID: 1473
		// (Invoke) Token: 0x06001D1D RID: 7453
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetTableauViewDelegate(UIntPtr texturePointer);

		// Token: 0x020005C2 RID: 1474
		// (Invoke) Token: 0x06001D21 RID: 7457
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetWidthDelegate(UIntPtr texturePointer);

		// Token: 0x020005C3 RID: 1475
		// (Invoke) Token: 0x06001D25 RID: 7461
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsLoadedDelegate(UIntPtr texturePointer);

		// Token: 0x020005C4 RID: 1476
		// (Invoke) Token: 0x06001D29 RID: 7465
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsRenderTargetDelegate(UIntPtr texturePointer);

		// Token: 0x020005C5 RID: 1477
		// (Invoke) Token: 0x06001D2D RID: 7469
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer LoadTextureFromPathDelegate(byte[] fileName, byte[] folder);

		// Token: 0x020005C6 RID: 1478
		// (Invoke) Token: 0x06001D31 RID: 7473
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr texturePointer);

		// Token: 0x020005C7 RID: 1479
		// (Invoke) Token: 0x06001D35 RID: 7477
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseAfterNumberOfFramesDelegate(UIntPtr texturePointer, int numberOfFrames);

		// Token: 0x020005C8 RID: 1480
		// (Invoke) Token: 0x06001D39 RID: 7481
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseGpuMemoriesDelegate();

		// Token: 0x020005C9 RID: 1481
		// (Invoke) Token: 0x06001D3D RID: 7485
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseNextFrameDelegate(UIntPtr texturePointer);

		// Token: 0x020005CA RID: 1482
		// (Invoke) Token: 0x06001D41 RID: 7489
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveContinousTableauTextureDelegate(UIntPtr texturePointer);

		// Token: 0x020005CB RID: 1483
		// (Invoke) Token: 0x06001D45 RID: 7493
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SaveTextureAsAlwaysValidDelegate(UIntPtr texturePointer);

		// Token: 0x020005CC RID: 1484
		// (Invoke) Token: 0x06001D49 RID: 7497
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SaveToFileDelegate(UIntPtr texturePointer, byte[] fileName, [MarshalAs(UnmanagedType.U1)] bool isRelativePath);

		// Token: 0x020005CD RID: 1485
		// (Invoke) Token: 0x06001D4D RID: 7501
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNameDelegate(UIntPtr texturePointer, byte[] name);

		// Token: 0x020005CE RID: 1486
		// (Invoke) Token: 0x06001D51 RID: 7505
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTableauViewDelegate(UIntPtr texturePointer, UIntPtr tableauView);

		// Token: 0x020005CF RID: 1487
		// (Invoke) Token: 0x06001D55 RID: 7509
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TransformRenderTargetToResourceTextureDelegate(UIntPtr texturePointer, byte[] name);
	}
}
