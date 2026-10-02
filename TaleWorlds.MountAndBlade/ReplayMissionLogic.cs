using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000295 RID: 661
	public class ReplayMissionLogic : MissionLogic
	{
		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x00084C57 File Offset: 0x00082E57
		// (set) Token: 0x060024A3 RID: 9379 RVA: 0x00084C5F File Offset: 0x00082E5F
		public string FileName { get; private set; }

		// Token: 0x060024A4 RID: 9380 RVA: 0x00084C68 File Offset: 0x00082E68
		public ReplayMissionLogic(bool isMultiplayer, string fileName = "")
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				this.FileName = fileName;
			}
			this._isMultiplayer = isMultiplayer;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x00084C86 File Offset: 0x00082E86
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			if (this._isMultiplayer)
			{
				GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			}
			MBCommon.CurrentGameType = MBCommon.GameType.SingleReplay;
			GameNetwork.InitializeClientSide(null, 0, -1, -1);
			base.Mission.Recorder.RestoreRecordFromFile(this.FileName);
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x00084CC1 File Offset: 0x00082EC1
		public override void OnRemoveBehavior()
		{
			if (this._isMultiplayer)
			{
				GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
				GameNetwork.EndReplay();
			}
			GameNetwork.TerminateClientSide();
			base.Mission.Recorder.ClearRecordBuffers();
			base.OnRemoveBehavior();
		}

		// Token: 0x04000E23 RID: 3619
		private bool _isMultiplayer;
	}
}
