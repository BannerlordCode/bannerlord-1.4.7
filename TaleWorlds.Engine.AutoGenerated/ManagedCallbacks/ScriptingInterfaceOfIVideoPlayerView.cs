using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000031 RID: 49
	internal class ScriptingInterfaceOfIVideoPlayerView : IVideoPlayerView
	{
		// Token: 0x060006D0 RID: 1744 RVA: 0x0001BB54 File Offset: 0x00019D54
		public VideoPlayerView CreateVideoPlayerView()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIVideoPlayerView.call_CreateVideoPlayerViewDelegate();
			VideoPlayerView videoPlayerView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				videoPlayerView = new VideoPlayerView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return videoPlayerView;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001BB9D File Offset: 0x00019D9D
		public void Finalize(UIntPtr pointer)
		{
			ScriptingInterfaceOfIVideoPlayerView.call_FinalizeDelegate(pointer);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001BBAA File Offset: 0x00019DAA
		public bool IsVideoFinished(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIVideoPlayerView.call_IsVideoFinishedDelegate(pointer);
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001BBB8 File Offset: 0x00019DB8
		public void PlayVideo(UIntPtr pointer, string videoFileName, string soundFileName, float framerate, bool looping)
		{
			byte[] array = null;
			if (videoFileName != null)
			{
				int byteCount = ScriptingInterfaceOfIVideoPlayerView._utf8.GetByteCount(videoFileName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIVideoPlayerView._utf8.GetBytes(videoFileName, 0, videoFileName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (soundFileName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIVideoPlayerView._utf8.GetByteCount(soundFileName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIVideoPlayerView._utf8.GetBytes(soundFileName, 0, soundFileName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			ScriptingInterfaceOfIVideoPlayerView.call_PlayVideoDelegate(pointer, array, array2, framerate, looping);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001BC5A File Offset: 0x00019E5A
		public void StopVideo(UIntPtr pointer)
		{
			ScriptingInterfaceOfIVideoPlayerView.call_StopVideoDelegate(pointer);
		}

		// Token: 0x0400061C RID: 1564
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400061D RID: 1565
		public static ScriptingInterfaceOfIVideoPlayerView.CreateVideoPlayerViewDelegate call_CreateVideoPlayerViewDelegate;

		// Token: 0x0400061E RID: 1566
		public static ScriptingInterfaceOfIVideoPlayerView.FinalizeDelegate call_FinalizeDelegate;

		// Token: 0x0400061F RID: 1567
		public static ScriptingInterfaceOfIVideoPlayerView.IsVideoFinishedDelegate call_IsVideoFinishedDelegate;

		// Token: 0x04000620 RID: 1568
		public static ScriptingInterfaceOfIVideoPlayerView.PlayVideoDelegate call_PlayVideoDelegate;

		// Token: 0x04000621 RID: 1569
		public static ScriptingInterfaceOfIVideoPlayerView.StopVideoDelegate call_StopVideoDelegate;

		// Token: 0x0200067A RID: 1658
		// (Invoke) Token: 0x06002001 RID: 8193
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateVideoPlayerViewDelegate();

		// Token: 0x0200067B RID: 1659
		// (Invoke) Token: 0x06002005 RID: 8197
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FinalizeDelegate(UIntPtr pointer);

		// Token: 0x0200067C RID: 1660
		// (Invoke) Token: 0x06002009 RID: 8201
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsVideoFinishedDelegate(UIntPtr pointer);

		// Token: 0x0200067D RID: 1661
		// (Invoke) Token: 0x0600200D RID: 8205
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PlayVideoDelegate(UIntPtr pointer, byte[] videoFileName, byte[] soundFileName, float framerate, [MarshalAs(UnmanagedType.U1)] bool looping);

		// Token: 0x0200067E RID: 1662
		// (Invoke) Token: 0x06002011 RID: 8209
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void StopVideoDelegate(UIntPtr pointer);
	}
}
