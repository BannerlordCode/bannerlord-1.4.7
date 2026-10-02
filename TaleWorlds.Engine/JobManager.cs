using System;
using System.Collections.Generic;

namespace TaleWorlds.Engine
{
	// Token: 0x02000053 RID: 83
	public class JobManager
	{
		// Token: 0x0600089C RID: 2204 RVA: 0x00006D1C File Offset: 0x00004F1C
		public JobManager()
		{
			this._jobs = new List<Job>();
			this._locker = new object();
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00006D3C File Offset: 0x00004F3C
		public void AddJob(Job job)
		{
			object locker = this._locker;
			lock (locker)
			{
				this._jobs.Add(job);
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00006D84 File Offset: 0x00004F84
		internal void OnTick(float dt)
		{
			object locker = this._locker;
			lock (locker)
			{
				for (int i = 0; i < this._jobs.Count; i++)
				{
					Job job = this._jobs[i];
					job.DoJob(dt);
					if (job.Finished)
					{
						this._jobs.RemoveAt(i);
						i--;
					}
				}
			}
		}

		// Token: 0x040000B6 RID: 182
		private List<Job> _jobs;

		// Token: 0x040000B7 RID: 183
		private object _locker;
	}
}
