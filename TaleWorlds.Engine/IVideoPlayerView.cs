using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000033 RID: 51
	[ApplicationInterfaceBase]
	internal interface IVideoPlayerView
	{
		// Token: 0x0600053F RID: 1343
		[EngineMethod("create_video_player_view", false, null, false)]
		VideoPlayerView CreateVideoPlayerView();

		// Token: 0x06000540 RID: 1344
		[EngineMethod("play_video", false, null, false)]
		void PlayVideo(UIntPtr pointer, string videoFileName, string soundFileName, float framerate, bool looping);

		// Token: 0x06000541 RID: 1345
		[EngineMethod("stop_video", false, null, false)]
		void StopVideo(UIntPtr pointer);

		// Token: 0x06000542 RID: 1346
		[EngineMethod("is_video_finished", false, null, false)]
		bool IsVideoFinished(UIntPtr pointer);

		// Token: 0x06000543 RID: 1347
		[EngineMethod("finalize", false, null, false)]
		void Finalize(UIntPtr pointer);
	}
}
