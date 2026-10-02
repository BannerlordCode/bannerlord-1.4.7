using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Armory
{
	// Token: 0x0200007A RID: 122
	public class MPArmoryHeroPerkSelectionVM : ViewModel
	{
		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00025BC7 File Offset: 0x00023DC7
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x00025BCF File Offset: 0x00023DCF
		public MultiplayerClassDivisions.MPHeroClass CurrentHeroClass { get; private set; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00025BD8 File Offset: 0x00023DD8
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x00025BE0 File Offset: 0x00023DE0
		public List<IReadOnlyPerkObject> CurrentSelectedPerks { get; private set; }

		// Token: 0x06000C34 RID: 3124 RVA: 0x00025BEC File Offset: 0x00023DEC
		public MPArmoryHeroPerkSelectionVM(Action<HeroPerkVM, MPPerkVM> onPerkSelection, Action forceRefreshCharacter)
		{
			this._onPerkSelection = onPerkSelection;
			this._forceRefreshCharacter = forceRefreshCharacter;
			this.Perks = new MBBindingList<HeroPerkVM>();
			this.GameModes = new SelectorVM<SelectorItemVM>(0, new Action<SelectorVM<SelectorItemVM>>(this.OnGameModeSelectionChanged));
			foreach (string text in this._availableGameModes)
			{
				this.GameModes.AddItem(new SelectorItemVM(GameTexts.FindText("str_multiplayer_official_game_type_name", text)));
			}
			this.GameModes.SelectedIndex = 0;
			this.RefreshValues();
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00025CE0 File Offset: 0x00023EE0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.GameModes.RefreshValues();
			this.GameModes.SelectedIndex = 0;
			this.Perks.ApplyActionOnAllItems(delegate(HeroPerkVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00025D34 File Offset: 0x00023F34
		public void RefreshPerksListWithHero(MultiplayerClassDivisions.MPHeroClass heroClass)
		{
			this.Perks = new MBBindingList<HeroPerkVM>();
			this.CurrentHeroClass = heroClass;
			MBBindingList<HeroPerkVM> mbbindingList = new MBBindingList<HeroPerkVM>();
			List<HeroPerkVM> list = new List<HeroPerkVM>();
			List<List<IReadOnlyPerkObject>> allPerksForHeroClass = MultiplayerClassDivisions.GetAllPerksForHeroClass(this.CurrentHeroClass, this._availableGameModes[this.GameModes.SelectedIndex]);
			for (int i = 0; i < allPerksForHeroClass.Count; i++)
			{
				if (allPerksForHeroClass[i].Count > 0)
				{
					IReadOnlyPerkObject readOnlyPerkObject = allPerksForHeroClass[i][0];
					HeroPerkVM heroPerkVM = new HeroPerkVM(new Action<HeroPerkVM, MPPerkVM>(this.OnPerkSelection), readOnlyPerkObject, allPerksForHeroClass[i], i);
					mbbindingList.Add(heroPerkVM);
					list.Add(heroPerkVM);
				}
			}
			this.Perks = mbbindingList;
			if (this.CurrentSelectedPerks == null)
			{
				this.CurrentSelectedPerks = new List<IReadOnlyPerkObject>();
			}
			else
			{
				this.CurrentSelectedPerks.Clear();
			}
			foreach (HeroPerkVM heroPerkVM2 in list)
			{
				this.OnPerkSelection(heroPerkVM2, heroPerkVM2.SelectedPerkItem);
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00025E50 File Offset: 0x00024050
		private void OnGameModeSelectionChanged(SelectorVM<SelectorItemVM> selector)
		{
			if (this.GameModes.SelectedIndex == -1)
			{
				this.GameModes.SelectedIndex = 0;
			}
			if (this.CurrentHeroClass != null)
			{
				this.RefreshPerksListWithHero(this.CurrentHeroClass);
				Action forceRefreshCharacter = this._forceRefreshCharacter;
				if (forceRefreshCharacter == null)
				{
					return;
				}
				forceRefreshCharacter();
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00025E90 File Offset: 0x00024090
		private void OnPerkSelection(HeroPerkVM heroPerk, MPPerkVM candidate)
		{
			this.CurrentSelectedPerks = this.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>();
			Action<HeroPerkVM, MPPerkVM> onPerkSelection = this._onPerkSelection;
			if (onPerkSelection == null)
			{
				return;
			}
			onPerkSelection(heroPerk, candidate);
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x00025EE4 File Offset: 0x000240E4
		// (set) Token: 0x06000C3A RID: 3130 RVA: 0x00025EEC File Offset: 0x000240EC
		[DataSourceProperty]
		public MBBindingList<HeroPerkVM> Perks
		{
			get
			{
				return this._perks;
			}
			set
			{
				if (value != this._perks)
				{
					this._perks = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroPerkVM>>(value, "Perks");
				}
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00025F0A File Offset: 0x0002410A
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x00025F12 File Offset: 0x00024112
		[DataSourceProperty]
		public SelectorVM<SelectorItemVM> GameModes
		{
			get
			{
				return this._gameModes;
			}
			set
			{
				if (value != this._gameModes)
				{
					this._gameModes = value;
					base.OnPropertyChangedWithValue<SelectorVM<SelectorItemVM>>(value, "GameModes");
				}
			}
		}

		// Token: 0x0400058C RID: 1420
		private readonly Action<HeroPerkVM, MPPerkVM> _onPerkSelection;

		// Token: 0x0400058D RID: 1421
		private readonly Action _forceRefreshCharacter;

		// Token: 0x0400058E RID: 1422
		private List<string> _availableGameModes = new List<string> { "Skirmish", "Captain", "Siege", "TeamDeathmatch", "Duel" };

		// Token: 0x0400058F RID: 1423
		private MBBindingList<HeroPerkVM> _perks;

		// Token: 0x04000590 RID: 1424
		private SelectorVM<SelectorItemVM> _gameModes;
	}
}
