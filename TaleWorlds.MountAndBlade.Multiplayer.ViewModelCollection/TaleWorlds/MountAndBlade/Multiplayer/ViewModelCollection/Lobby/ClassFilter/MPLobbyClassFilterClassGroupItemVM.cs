using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.ClassFilter
{
	// Token: 0x02000062 RID: 98
	public class MPLobbyClassFilterClassGroupItemVM : ViewModel
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0001DF61 File Offset: 0x0001C161
		// (set) Token: 0x06000989 RID: 2441 RVA: 0x0001DF69 File Offset: 0x0001C169
		public MultiplayerClassDivisions.MPHeroClassGroup ClassGroup { get; set; }

		// Token: 0x0600098A RID: 2442 RVA: 0x0001DF72 File Offset: 0x0001C172
		public MPLobbyClassFilterClassGroupItemVM(MultiplayerClassDivisions.MPHeroClassGroup classGroup)
		{
			this.ClassGroup = classGroup;
			this.Classes = new MBBindingList<MPLobbyClassFilterClassItemVM>();
			this.RefreshValues();
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0001DF94 File Offset: 0x0001C194
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.ClassGroup.Name.ToString();
			this.Classes.ApplyActionOnAllItems(delegate(MPLobbyClassFilterClassItemVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0001DFE7 File Offset: 0x0001C1E7
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ClassGroup = null;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0001DFF8 File Offset: 0x0001C1F8
		public void AddClass(BasicCultureObject culture, MultiplayerClassDivisions.MPHeroClass heroClass, Action<MPLobbyClassFilterClassItemVM> onSelect)
		{
			MPLobbyClassFilterClassItemVM mplobbyClassFilterClassItemVM = new MPLobbyClassFilterClassItemVM(culture, heroClass, onSelect);
			this.Classes.Add(mplobbyClassFilterClassItemVM);
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0001E01A File Offset: 0x0001C21A
		// (set) Token: 0x0600098F RID: 2447 RVA: 0x0001E022 File Offset: 0x0001C222
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0001E045 File Offset: 0x0001C245
		// (set) Token: 0x06000991 RID: 2449 RVA: 0x0001E04D File Offset: 0x0001C24D
		[DataSourceProperty]
		public MBBindingList<MPLobbyClassFilterClassItemVM> Classes
		{
			get
			{
				return this._classes;
			}
			set
			{
				if (value != this._classes)
				{
					this._classes = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPLobbyClassFilterClassItemVM>>(value, "Classes");
				}
			}
		}

		// Token: 0x04000467 RID: 1127
		private string _name;

		// Token: 0x04000468 RID: 1128
		private MBBindingList<MPLobbyClassFilterClassItemVM> _classes;
	}
}
