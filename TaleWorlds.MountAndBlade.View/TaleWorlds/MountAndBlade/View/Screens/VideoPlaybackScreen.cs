using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.View.Screens
{
	// Token: 0x0200005C RID: 92
	public class VideoPlaybackScreen : ScreenBase, IGameStateListener
	{
		// Token: 0x0600036B RID: 875 RVA: 0x00019DE6 File Offset: 0x00017FE6
		public VideoPlaybackScreen(VideoPlaybackState videoPlaybackState)
		{
			this._videoPlaybackState = videoPlaybackState;
			this._videoPlayerView = VideoPlayerView.CreateVideoPlayerView();
			this._videoPlayerView.SetRenderOrder(-10000);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00019E10 File Offset: 0x00018010
		protected sealed override void OnFrameTick(float dt)
		{
			this._totalElapsedTimeSinceVideoStart += dt;
			base.OnFrameTick(dt);
			if (this._videoPlayerView != null && this._videoPlaybackState != null)
			{
				if (this._videoPlaybackState.CanUserSkip && (Input.IsKeyReleased(InputKey.Escape) || Input.IsKeyReleased(InputKey.ControllerROption)))
				{
					this._videoPlayerView.StopVideo();
				}
				if (this._videoPlayerView.IsVideoFinished())
				{
					this._videoPlaybackState.OnVideoFinished();
					this._videoPlayerView.SetEnable(false);
					this._videoPlayerView.FinalizePlayer();
					this._videoPlayerView = null;
				}
				if (ScreenManager.TopScreen == this)
				{
					this.OnVideoPlaybackTick(dt);
				}
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00019EB9 File Offset: 0x000180B9
		protected virtual void OnVideoPlaybackTick(float dt)
		{
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00019EBC File Offset: 0x000180BC
		void IGameStateListener.OnInitialize()
		{
			this._videoPlayerView.PlayVideo(this._videoPlaybackState.VideoPath, this._videoPlaybackState.AudioPath, this._videoPlaybackState.FrameRate, false);
			this._videoPlaybackState.OnVideoStarted();
			LoadingWindow.DisableGlobalLoadingWindow();
			Utilities.DisableGlobalLoadingWindow();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00019F0B File Offset: 0x0001810B
		void IGameStateListener.OnFinalize()
		{
			VideoPlayerView videoPlayerView = this._videoPlayerView;
			if (videoPlayerView != null)
			{
				videoPlayerView.SetEnable(false);
			}
			VideoPlayerView videoPlayerView2 = this._videoPlayerView;
			if (videoPlayerView2 == null)
			{
				return;
			}
			videoPlayerView2.FinalizePlayer();
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00019F2F File Offset: 0x0001812F
		void IGameStateListener.OnActivate()
		{
			base.OnActivate();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00019F37 File Offset: 0x00018137
		void IGameStateListener.OnDeactivate()
		{
			base.OnDeactivate();
		}

		// Token: 0x040001CC RID: 460
		protected VideoPlaybackState _videoPlaybackState;

		// Token: 0x040001CD RID: 461
		protected VideoPlayerView _videoPlayerView;

		// Token: 0x040001CE RID: 462
		protected float _totalElapsedTimeSinceVideoStart;
	}
}
