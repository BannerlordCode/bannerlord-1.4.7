using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000D6 RID: 214
	public class Timer
	{
		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x00024949 File Offset: 0x00022B49
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x00024951 File Offset: 0x00022B51
		public float StartTime { get; protected set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0002495A File Offset: 0x00022B5A
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x00024962 File Offset: 0x00022B62
		public float Duration { get; protected set; }

		// Token: 0x06000B35 RID: 2869 RVA: 0x0002496B File Offset: 0x00022B6B
		public Timer(float gameTime, float duration, bool autoReset = true)
		{
			this.StartTime = gameTime;
			this._latestGameTime = gameTime;
			this._autoReset = autoReset;
			this.Duration = duration;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00024990 File Offset: 0x00022B90
		public virtual bool Check(float gameTime)
		{
			this._latestGameTime = gameTime;
			if (this.Duration <= 0f)
			{
				this.PreviousDeltaTime = this.ElapsedTime();
				this.StartTime = gameTime;
				return true;
			}
			bool flag = false;
			if (this.ElapsedTime() >= this.Duration)
			{
				this.PreviousDeltaTime = this.ElapsedTime();
				if (this._autoReset)
				{
					while (this.ElapsedTime() >= this.Duration)
					{
						this.StartTime += this.Duration;
					}
				}
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00024A10 File Offset: 0x00022C10
		public float ElapsedTime()
		{
			return this._latestGameTime - this.StartTime;
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00024A1F File Offset: 0x00022C1F
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x00024A27 File Offset: 0x00022C27
		public float PreviousDeltaTime { get; private set; }

		// Token: 0x06000B3A RID: 2874 RVA: 0x00024A30 File Offset: 0x00022C30
		public void Reset(float gameTime)
		{
			this.Reset(gameTime, this.Duration);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00024A3F File Offset: 0x00022C3F
		public void Reset(float gameTime, float newDuration)
		{
			this.StartTime = gameTime;
			this._latestGameTime = gameTime;
			this.Duration = newDuration;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00024A56 File Offset: 0x00022C56
		public void AdjustStartTime(float deltaTime)
		{
			this.StartTime += deltaTime;
		}

		// Token: 0x04000652 RID: 1618
		private float _latestGameTime;

		// Token: 0x04000653 RID: 1619
		private bool _autoReset;
	}
}
