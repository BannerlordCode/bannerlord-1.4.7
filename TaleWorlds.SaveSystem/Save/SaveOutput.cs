using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Save
{
	// Token: 0x02000030 RID: 48
	public class SaveOutput
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A381 File Offset: 0x00008581
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000A389 File Offset: 0x00008589
		public GameData Data { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000A392 File Offset: 0x00008592
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000A39A File Offset: 0x0000859A
		public SaveResult Result { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A3A3 File Offset: 0x000085A3
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000A3AB File Offset: 0x000085AB
		public SaveError[] Errors { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A3B4 File Offset: 0x000085B4
		public bool Successful
		{
			get
			{
				return this.Result == SaveResult.Success;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000A3BF File Offset: 0x000085BF
		public bool IsContinuing
		{
			get
			{
				Task<SaveResultWithMessage> continuingTask = this._continuingTask;
				return continuingTask != null && !continuingTask.IsCompleted;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A3D5 File Offset: 0x000085D5
		private SaveOutput()
		{
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A3DD File Offset: 0x000085DD
		internal static SaveOutput CreateSuccessful(GameData data)
		{
			return new SaveOutput
			{
				Data = data,
				Result = SaveResult.Success
			};
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A3F2 File Offset: 0x000085F2
		internal static SaveOutput CreateFailed(IEnumerable<SaveError> errors, SaveResult result)
		{
			return new SaveOutput
			{
				Result = result,
				Errors = errors.ToArray<SaveError>()
			};
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A40C File Offset: 0x0000860C
		internal static SaveOutput CreateContinuing(Task<SaveResultWithMessage> continuingTask)
		{
			SaveOutput saveOutput = new SaveOutput();
			saveOutput._continuingTask = continuingTask;
			saveOutput._continuingTask.ContinueWith(delegate(Task<SaveResultWithMessage> t)
			{
				saveOutput.Result = t.Result.SaveResult;
			});
			return saveOutput;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A45C File Offset: 0x0000865C
		public void PrintStatus()
		{
			Task<SaveResultWithMessage> continuingTask = this._continuingTask;
			if (continuingTask != null && continuingTask.IsCompleted)
			{
				this.Result = this._continuingTask.Result.SaveResult;
				this.Errors = new SaveError[0];
			}
			if (this.Result == SaveResult.Success)
			{
				Debug.Print("------Successfully saved------", 0, Debug.DebugColor.White, 17592186044416UL);
				return;
			}
			Debug.Print("Couldn't save because of errors listed below.", 0, Debug.DebugColor.White, 17592186044416UL);
			for (int i = 0; i < this.Errors.Length; i++)
			{
				SaveError saveError = this.Errors[i];
				Debug.Print(string.Concat(new object[] { "[", i, "]", saveError.Message }), 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.FailedAssert(string.Concat(new object[] { "SAVE FAILED: [", i, "]", saveError.Message, "\n" }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.SaveSystem\\Save\\SaveOutput.cs", "PrintStatus", 74);
			}
			Debug.Print("--------------------", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x04000092 RID: 146
		private Task<SaveResultWithMessage> _continuingTask;
	}
}
