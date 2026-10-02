using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000072 RID: 114
	public class MBWorkspace<T> where T : IMBCollection, new()
	{
		// Token: 0x06000419 RID: 1049 RVA: 0x0000E7C1 File Offset: 0x0000C9C1
		public T StartUsingWorkspace()
		{
			this._isBeingUsed = true;
			if (this._workspace == null)
			{
				this._workspace = new T();
			}
			return this._workspace;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000E7E8 File Offset: 0x0000C9E8
		public void StopUsingWorkspace()
		{
			this._isBeingUsed = false;
			this._workspace.Clear();
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000E802 File Offset: 0x0000CA02
		public T GetWorkspace()
		{
			return this._workspace;
		}

		// Token: 0x04000145 RID: 325
		private bool _isBeingUsed;

		// Token: 0x04000146 RID: 326
		private T _workspace;
	}
}
