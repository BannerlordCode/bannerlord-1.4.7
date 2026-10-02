using System;
using System.Linq;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FactionBanVote
{
	// Token: 0x0200009D RID: 157
	public class MultiplayerFactionBanVM : ViewModel
	{
		// Token: 0x06000F28 RID: 3880 RVA: 0x0002EC58 File Offset: 0x0002CE58
		public MultiplayerFactionBanVM()
		{
			this.SelectTitle = "SELECT FACTION";
			this.BanTitle = "BAN FACTION";
			this._banList = new MBBindingList<MultiplayerFactionBanVoteVM>();
			foreach (BasicCultureObject basicCultureObject in MultiplayerClassDivisions.AvailableCultures)
			{
				this._banList.Add(new MultiplayerFactionBanVoteVM(basicCultureObject, new Action<MultiplayerFactionBanVoteVM>(this.OnBanFaction)));
			}
			this._selectList = new MBBindingList<MultiplayerFactionBanVoteVM>();
			foreach (BasicCultureObject basicCultureObject2 in MultiplayerClassDivisions.AvailableCultures)
			{
				this._selectList.Add(new MultiplayerFactionBanVoteVM(basicCultureObject2, new Action<MultiplayerFactionBanVoteVM>(this.OnSelectFaction)));
			}
			foreach (MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM in this._selectList)
			{
				if (multiplayerFactionBanVoteVM.IsEnabled)
				{
					multiplayerFactionBanVoteVM.IsSelected = true;
					break;
				}
			}
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x0002ED88 File Offset: 0x0002CF88
		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x0002ED90 File Offset: 0x0002CF90
		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0002ED98 File Offset: 0x0002CF98
		private void OnSelectFaction(MultiplayerFactionBanVoteVM vote)
		{
			MultiplayerFactionBanVM.VoteForCulture(CultureVoteTypes.Select, vote.Culture);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0002EDA6 File Offset: 0x0002CFA6
		private void OnBanFaction(MultiplayerFactionBanVoteVM vote)
		{
			MultiplayerFactionBanVM.VoteForCulture(CultureVoteTypes.Ban, vote.Culture);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0002EDB4 File Offset: 0x0002CFB4
		private void Refresh()
		{
			foreach (MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM in this._banList)
			{
				multiplayerFactionBanVoteVM.IsSelected = false;
				multiplayerFactionBanVoteVM.IsEnabled = false;
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			bool flag = false;
			foreach (MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM2 in this._selectList)
			{
				if (flag)
				{
					multiplayerFactionBanVoteVM2.IsSelected = true;
					flag = false;
					break;
				}
				if (component.VotedForBan == multiplayerFactionBanVoteVM2.Culture)
				{
					multiplayerFactionBanVoteVM2.IsEnabled = false;
					if (multiplayerFactionBanVoteVM2.IsSelected)
					{
						multiplayerFactionBanVoteVM2.IsSelected = false;
						flag = true;
					}
				}
			}
			if (flag)
			{
				MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM3 = this._selectList.FirstOrDefault<MultiplayerFactionBanVoteVM>((MultiplayerFactionBanVoteVM s) => s.IsEnabled);
				if (multiplayerFactionBanVoteVM3 != null)
				{
					multiplayerFactionBanVoteVM3.IsSelected = true;
				}
			}
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0002EEB8 File Offset: 0x0002D0B8
		private static void VoteForCulture(CultureVoteTypes voteType, BasicCultureObject culture)
		{
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (GameNetwork.IsServer)
			{
				component.HandleVoteChange(voteType, culture);
				return;
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new CultureVoteClient(voteType, culture));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0002EEFD File Offset: 0x0002D0FD
		// (set) Token: 0x06000F30 RID: 3888 RVA: 0x0002EF05 File Offset: 0x0002D105
		[DataSourceProperty]
		public MBBindingList<MultiplayerFactionBanVoteVM> SelectList
		{
			get
			{
				return this._selectList;
			}
			set
			{
				if (value != this._selectList)
				{
					this._selectList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerFactionBanVoteVM>>(value, "SelectList");
				}
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x0002EF23 File Offset: 0x0002D123
		// (set) Token: 0x06000F32 RID: 3890 RVA: 0x0002EF2B File Offset: 0x0002D12B
		[DataSourceProperty]
		public MBBindingList<MultiplayerFactionBanVoteVM> BanList
		{
			get
			{
				return this._banList;
			}
			set
			{
				if (value != this._banList)
				{
					this._banList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerFactionBanVoteVM>>(value, "BanList");
				}
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x0002EF49 File Offset: 0x0002D149
		// (set) Token: 0x06000F34 RID: 3892 RVA: 0x0002EF51 File Offset: 0x0002D151
		[DataSourceProperty]
		public string SelectTitle
		{
			get
			{
				return this._selectTitle;
			}
			set
			{
				if (value != this._selectTitle)
				{
					this._selectTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectTitle");
				}
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0002EF74 File Offset: 0x0002D174
		// (set) Token: 0x06000F36 RID: 3894 RVA: 0x0002EF7C File Offset: 0x0002D17C
		[DataSourceProperty]
		public string BanTitle
		{
			get
			{
				return this._banTitle;
			}
			set
			{
				if (value != this._banTitle)
				{
					this._banTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "BanTitle");
				}
			}
		}

		// Token: 0x040006FF RID: 1791
		private MBBindingList<MultiplayerFactionBanVoteVM> _banList;

		// Token: 0x04000700 RID: 1792
		private MBBindingList<MultiplayerFactionBanVoteVM> _selectList;

		// Token: 0x04000701 RID: 1793
		private string _selectTitle;

		// Token: 0x04000702 RID: 1794
		private string _banTitle;
	}
}
