using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000246 RID: 582
	public class VideoPlaybackState : GameState
	{
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x00075657 File Offset: 0x00073857
		// (set) Token: 0x06002178 RID: 8568 RVA: 0x0007565F File Offset: 0x0007385F
		public string VideoPath { get; private set; }

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002179 RID: 8569 RVA: 0x00075668 File Offset: 0x00073868
		// (set) Token: 0x0600217A RID: 8570 RVA: 0x00075670 File Offset: 0x00073870
		public string AudioPath { get; private set; }

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x0600217B RID: 8571 RVA: 0x00075679 File Offset: 0x00073879
		// (set) Token: 0x0600217C RID: 8572 RVA: 0x00075681 File Offset: 0x00073881
		public float FrameRate { get; private set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600217D RID: 8573 RVA: 0x0007568A File Offset: 0x0007388A
		// (set) Token: 0x0600217E RID: 8574 RVA: 0x00075692 File Offset: 0x00073892
		public string SubtitleFileBasePath { get; private set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x0007569B File Offset: 0x0007389B
		// (set) Token: 0x06002180 RID: 8576 RVA: 0x000756A3 File Offset: 0x000738A3
		public bool CanUserSkip { get; private set; }

		// Token: 0x06002181 RID: 8577 RVA: 0x000756AC File Offset: 0x000738AC
		public void SetStartingParameters(string videoPath, string audioPath, string subtitleFileBasePath, float frameRate = 30f, bool canUserSkip = true)
		{
			this.VideoPath = videoPath;
			this.AudioPath = audioPath;
			this.FrameRate = frameRate;
			this.SubtitleFileBasePath = subtitleFileBasePath;
			this.CanUserSkip = canUserSkip;
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x000756D3 File Offset: 0x000738D3
		public void SetOnVideoFinisedDelegate(Action onVideoFinised)
		{
			this._onVideoFinised = onVideoFinised;
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000756DC File Offset: 0x000738DC
		public void OnVideoStarted()
		{
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.PauseMusicManagerSystem();
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000756F2 File Offset: 0x000738F2
		public void OnVideoFinished()
		{
			MBMusicManager.Current.UnpauseMusicManagerSystem();
			Action onVideoFinised = this._onVideoFinised;
			if (onVideoFinised == null)
			{
				return;
			}
			onVideoFinised();
		}

		// Token: 0x04000CD7 RID: 3287
		private Action _onVideoFinised;
	}
}
