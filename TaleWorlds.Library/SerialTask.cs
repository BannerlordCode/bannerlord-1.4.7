using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200008D RID: 141
	public class SerialTask : ITask
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x00012438 File Offset: 0x00010638
		public SerialTask(SerialTask.DelegateDefinition function)
		{
			this._instance = function;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00012447 File Offset: 0x00010647
		void ITask.Invoke()
		{
			this._instance();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00012454 File Offset: 0x00010654
		void ITask.Wait()
		{
		}

		// Token: 0x04000191 RID: 401
		private SerialTask.DelegateDefinition _instance;

		// Token: 0x020000E3 RID: 227
		// (Invoke) Token: 0x060007A4 RID: 1956
		public delegate void DelegateDefinition();
	}
}
