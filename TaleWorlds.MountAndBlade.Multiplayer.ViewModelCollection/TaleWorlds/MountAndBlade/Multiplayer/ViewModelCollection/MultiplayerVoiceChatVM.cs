using System;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000017 RID: 23
	public class MultiplayerVoiceChatVM : ViewModel
	{
		// Token: 0x06000146 RID: 326 RVA: 0x00006064 File Offset: 0x00004264
		public MultiplayerVoiceChatVM(Mission mission)
		{
			this._mission = mission;
			this._voiceChatHandler = this._mission.GetMissionBehavior<VoiceChatHandler>();
			if (this._voiceChatHandler != null)
			{
				this._voiceChatHandler.OnPeerVoiceStatusUpdated += this.OnPeerVoiceStatusUpdated;
				this._voiceChatHandler.OnVoiceRecordStarted += this.OnVoiceRecordStarted;
				this._voiceChatHandler.OnVoiceRecordStopped += this.OnVoiceRecordStopped;
			}
			this.ActiveVoicePlayers = new MBBindingList<MPVoicePlayerVM>();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000060E8 File Offset: 0x000042E8
		public override void OnFinalize()
		{
			if (this._voiceChatHandler != null)
			{
				this._voiceChatHandler.OnPeerVoiceStatusUpdated -= this.OnPeerVoiceStatusUpdated;
				this._voiceChatHandler.OnVoiceRecordStarted -= this.OnVoiceRecordStarted;
				this._voiceChatHandler.OnVoiceRecordStopped -= this.OnVoiceRecordStopped;
			}
			base.OnFinalize();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006148 File Offset: 0x00004348
		public void OnTick(float dt)
		{
			for (int i = 0; i < this.ActiveVoicePlayers.Count; i++)
			{
				if (!this.ActiveVoicePlayers[i].IsMyPeer && this.ActiveVoicePlayers[i].UpdatesSinceSilence >= 30)
				{
					this.ActiveVoicePlayers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000061A4 File Offset: 0x000043A4
		private void OnPeerVoiceStatusUpdated(MissionPeer peer, bool isTalking)
		{
			MPVoicePlayerVM mpvoicePlayerVM = this.ActiveVoicePlayers.FirstOrDefault<MPVoicePlayerVM>((MPVoicePlayerVM vp) => vp.Peer == peer);
			if (!isTalking)
			{
				if (!isTalking && mpvoicePlayerVM != null)
				{
					mpvoicePlayerVM.UpdatesSinceSilence++;
				}
				return;
			}
			if (mpvoicePlayerVM == null)
			{
				this.ActiveVoicePlayers.Add(new MPVoicePlayerVM(peer));
				return;
			}
			mpvoicePlayerVM.UpdatesSinceSilence = 0;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000620F File Offset: 0x0000440F
		private void OnVoiceRecordStarted()
		{
			this.ActiveVoicePlayers.Add(new MPVoicePlayerVM(GameNetwork.MyPeer.GetComponent<MissionPeer>()));
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000622C File Offset: 0x0000442C
		private void OnVoiceRecordStopped()
		{
			MPVoicePlayerVM mpvoicePlayerVM = this.ActiveVoicePlayers.FirstOrDefault<MPVoicePlayerVM>((MPVoicePlayerVM vp) => vp.Peer == GameNetwork.MyPeer.GetComponent<MissionPeer>());
			this.ActiveVoicePlayers.Remove(mpvoicePlayerVM);
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00006271 File Offset: 0x00004471
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00006279 File Offset: 0x00004479
		[DataSourceProperty]
		public MBBindingList<MPVoicePlayerVM> ActiveVoicePlayers
		{
			get
			{
				return this._activeVoicePlayers;
			}
			set
			{
				if (value != this._activeVoicePlayers)
				{
					this._activeVoicePlayers = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPVoicePlayerVM>>(value, "ActiveVoicePlayers");
				}
			}
		}

		// Token: 0x040000AE RID: 174
		private readonly Mission _mission;

		// Token: 0x040000AF RID: 175
		private readonly VoiceChatHandler _voiceChatHandler;

		// Token: 0x040000B0 RID: 176
		private MBBindingList<MPVoicePlayerVM> _activeVoicePlayers;
	}
}
