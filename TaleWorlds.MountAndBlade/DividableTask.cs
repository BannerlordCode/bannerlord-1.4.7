using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013A RID: 314
	public class DividableTask
	{
		// Token: 0x06000F1B RID: 3867 RVA: 0x00028D5E File Offset: 0x00026F5E
		public DividableTask(DividableTask continueToTask = null)
		{
			this._continueToTask = continueToTask;
			this.ResetTaskStatus();
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00028D73 File Offset: 0x00026F73
		public void ResetTaskStatus()
		{
			this._isMainTaskFinished = false;
			this._isTaskCompletelyFinished = false;
			this._lastActionCalled = false;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00028D8A File Offset: 0x00026F8A
		public void SetTaskFinished(bool callLastAction = false)
		{
			if (callLastAction)
			{
				Action lastAction = this._lastAction;
				if (lastAction != null)
				{
					lastAction();
				}
				this._lastActionCalled = true;
			}
			this._isTaskCompletelyFinished = true;
			this._isMainTaskFinished = true;
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00028DB8 File Offset: 0x00026FB8
		public bool Update()
		{
			if (!this._isTaskCompletelyFinished)
			{
				if (!this._isMainTaskFinished && this.UpdateExtra())
				{
					this._isMainTaskFinished = true;
				}
				if (this._isMainTaskFinished)
				{
					DividableTask continueToTask = this._continueToTask;
					this._isTaskCompletelyFinished = continueToTask == null || continueToTask.Update();
				}
			}
			if (this._isTaskCompletelyFinished && !this._lastActionCalled)
			{
				Action lastAction = this._lastAction;
				if (lastAction != null)
				{
					lastAction();
				}
				this._lastActionCalled = true;
			}
			return this._isTaskCompletelyFinished;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00028E32 File Offset: 0x00027032
		public void SetLastAction(Action action)
		{
			this._lastAction = action;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00028E3B File Offset: 0x0002703B
		protected virtual bool UpdateExtra()
		{
			return true;
		}

		// Token: 0x040003B7 RID: 951
		private bool _isTaskCompletelyFinished;

		// Token: 0x040003B8 RID: 952
		private bool _isMainTaskFinished;

		// Token: 0x040003B9 RID: 953
		private bool _lastActionCalled;

		// Token: 0x040003BA RID: 954
		private DividableTask _continueToTask;

		// Token: 0x040003BB RID: 955
		private Action _lastAction;
	}
}
