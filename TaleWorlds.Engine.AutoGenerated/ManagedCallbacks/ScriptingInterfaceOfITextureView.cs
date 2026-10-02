using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x0200002C RID: 44
	internal class ScriptingInterfaceOfITextureView : ITextureView
	{
		// Token: 0x0600061C RID: 1564 RVA: 0x00019CDC File Offset: 0x00017EDC
		public TextureView CreateTextureView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITextureView.call_CreateTextureViewDelegate();
			TextureView textureView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				textureView = new TextureView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return textureView;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00019D25 File Offset: 0x00017F25
		public void SetTexture(UIntPtr pointer, UIntPtr texture_ptr)
		{
			ScriptingInterfaceOfITextureView.call_SetTextureDelegate(pointer, texture_ptr);
		}

		// Token: 0x0400056D RID: 1389
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400056E RID: 1390
		public static ScriptingInterfaceOfITextureView.CreateTextureViewDelegate call_CreateTextureViewDelegate;

		// Token: 0x0400056F RID: 1391
		public static ScriptingInterfaceOfITextureView.SetTextureDelegate call_SetTextureDelegate;

		// Token: 0x020005D0 RID: 1488
		// (Invoke) Token: 0x06001D59 RID: 7513
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTextureViewDelegate();

		// Token: 0x020005D1 RID: 1489
		// (Invoke) Token: 0x06001D5D RID: 7517
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTextureDelegate(UIntPtr pointer, UIntPtr texture_ptr);
	}
}
