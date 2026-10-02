using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009A RID: 154
	[EngineClass("rglVideo_player_view")]
	public sealed class VideoPlayerView : View
	{
		// Token: 0x06000DC8 RID: 3528 RVA: 0x0000F7AE File Offset: 0x0000D9AE
		internal VideoPlayerView(UIntPtr meshPointer)
			: base(meshPointer)
		{
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0000F7B7 File Offset: 0x0000D9B7
		public static VideoPlayerView CreateVideoPlayerView()
		{
			return EngineApplicationInterface.IVideoPlayerView.CreateVideoPlayerView();
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0000F7C3 File Offset: 0x0000D9C3
		public void PlayVideo(string videoFileName, string soundFileName, float framerate, bool looping)
		{
			EngineApplicationInterface.IVideoPlayerView.PlayVideo(base.Pointer, videoFileName, soundFileName, framerate, looping);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0000F7DA File Offset: 0x0000D9DA
		public void StopVideo()
		{
			EngineApplicationInterface.IVideoPlayerView.StopVideo(base.Pointer);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0000F7EC File Offset: 0x0000D9EC
		public bool IsVideoFinished()
		{
			return EngineApplicationInterface.IVideoPlayerView.IsVideoFinished(base.Pointer);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0000F7FE File Offset: 0x0000D9FE
		public void FinalizePlayer()
		{
			EngineApplicationInterface.IVideoPlayerView.Finalize(base.Pointer);
		}
	}
}
