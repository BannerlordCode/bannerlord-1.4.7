using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle.SelectionItem;
using TaleWorlds.MountAndBlade.View.CustomBattle;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.CustomBattle
{
	// Token: 0x0200000D RID: 13
	public class CustomBattleVM : ViewModel
	{
		// Token: 0x06000093 RID: 147 RVA: 0x00006914 File Offset: 0x00004B14
		private static CustomBattleCompositionData GetBattleCompositionDataFromCompositionGroup(ArmyCompositionGroupVM compositionGroup)
		{
			return new CustomBattleCompositionData((float)compositionGroup.RangedInfantryComposition.CompositionValue / 100f, (float)compositionGroup.MeleeCavalryComposition.CompositionValue / 100f, (float)compositionGroup.RangedCavalryComposition.CompositionValue / 100f);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00006954 File Offset: 0x00004B54
		private static List<BasicCharacterObject>[] GetTroopSelections(ArmyCompositionGroupVM armyComposition)
		{
			List<BasicCharacterObject>[] array = new List<BasicCharacterObject>[4];
			array[0] = (from x in armyComposition.MeleeInfantryComposition.TroopTypes
				where x.IsSelected
				select x.Character).ToList<BasicCharacterObject>();
			array[1] = (from x in armyComposition.RangedInfantryComposition.TroopTypes
				where x.IsSelected
				select x.Character).ToList<BasicCharacterObject>();
			array[2] = (from x in armyComposition.MeleeCavalryComposition.TroopTypes
				where x.IsSelected
				select x.Character).ToList<BasicCharacterObject>();
			array[3] = (from x in armyComposition.RangedCavalryComposition.TroopTypes
				where x.IsSelected
				select x.Character).ToList<BasicCharacterObject>();
			return array;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00006AD4 File Offset: 0x00004CD4
		private static void FillSiegeMachines(List<MissionSiegeWeapon> machines, MBBindingList<CustomBattleSiegeMachineVM> vmMachines)
		{
			foreach (CustomBattleSiegeMachineVM customBattleSiegeMachineVM in vmMachines)
			{
				if (customBattleSiegeMachineVM.SiegeEngineType != null)
				{
					machines.Add(MissionSiegeWeapon.CreateDefaultWeapon(customBattleSiegeMachineVM.SiegeEngineType));
				}
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00006B30 File Offset: 0x00004D30
		public CustomBattleVM(CustomBattleState battleState)
		{
			this._customBattleState = battleState;
			this.IsAttackerCustomMachineSelectionEnabled = false;
			this.TroopTypeSelectionPopUp = new TroopTypeSelectionPopUpVM();
			this.PlayerSide = new CustomBattleSideVM(new TextObject("{=BC7n6qxk}PLAYER", null), true, this.TroopTypeSelectionPopUp, new Action(this.OnSelectedCharactersChanged));
			this.EnemySide = new CustomBattleSideVM(new TextObject("{=35IHscBa}ENEMY", null), false, this.TroopTypeSelectionPopUp, new Action(this.OnSelectedCharactersChanged));
			this.OnSelectedCharactersChanged();
			this.MapSelectionGroup = new MapSelectionGroupVM();
			this.GameTypeSelectionGroup = new GameTypeSelectionGroupVM(new Action<CustomBattlePlayerType>(this.OnPlayerTypeChange), new Action<string>(this.OnGameTypeChange));
			this.AttackerMeleeMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
			for (int i = 0; i < 3; i++)
			{
				this.AttackerMeleeMachines.Add(new CustomBattleSiegeMachineVM(null, new Action<CustomBattleSiegeMachineVM>(this.OnMeleeMachineSelection), new Action<CustomBattleSiegeMachineVM>(this.OnResetMachineSelection)));
			}
			this.AttackerRangedMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
			for (int j = 0; j < 4; j++)
			{
				this.AttackerRangedMachines.Add(new CustomBattleSiegeMachineVM(null, new Action<CustomBattleSiegeMachineVM>(this.OnAttackerRangedMachineSelection), new Action<CustomBattleSiegeMachineVM>(this.OnResetMachineSelection)));
			}
			this.DefenderMachines = new MBBindingList<CustomBattleSiegeMachineVM>();
			for (int k = 0; k < 4; k++)
			{
				this.DefenderMachines.Add(new CustomBattleSiegeMachineVM(null, new Action<CustomBattleSiegeMachineVM>(this.OnDefenderRangedMachineSelection), new Action<CustomBattleSiegeMachineVM>(this.OnResetMachineSelection)));
			}
			this.CanSwitchMode = CustomBattleFactory.GetProviderCount() > 1;
			if (this.CanSwitchMode)
			{
				this._nextCustomBattleProvider = CustomBattleFactory.CollectNextProvider(typeof(CustomBattleProvider));
				this.SwitchHint = new HintViewModel(new TextObject("{=Jfe53wbr}Switch to {PROVIDER_NAME}", null).SetTextVariable("PROVIDER_NAME", this._nextCustomBattleProvider.GetName()), null);
			}
			this.RefreshValues();
			this.SetDefaultSiegeMachines();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00006D08 File Offset: 0x00004F08
		private void SetDefaultSiegeMachines()
		{
			this.AttackerMeleeMachines[0].SetMachineType(DefaultSiegeEngineTypes.SiegeTower);
			this.AttackerMeleeMachines[1].SetMachineType(DefaultSiegeEngineTypes.Ram);
			this.AttackerMeleeMachines[2].SetMachineType(DefaultSiegeEngineTypes.SiegeTower);
			this.AttackerRangedMachines[0].SetMachineType(DefaultSiegeEngineTypes.Trebuchet);
			this.AttackerRangedMachines[1].SetMachineType(DefaultSiegeEngineTypes.Onager);
			this.AttackerRangedMachines[2].SetMachineType(DefaultSiegeEngineTypes.Onager);
			this.AttackerRangedMachines[3].SetMachineType(DefaultSiegeEngineTypes.FireBallista);
			this.DefenderMachines[0].SetMachineType(DefaultSiegeEngineTypes.FireCatapult);
			this.DefenderMachines[1].SetMachineType(DefaultSiegeEngineTypes.FireCatapult);
			this.DefenderMachines[2].SetMachineType(DefaultSiegeEngineTypes.Catapult);
			this.DefenderMachines[3].SetMachineType(DefaultSiegeEngineTypes.FireBallista);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00006E07 File Offset: 0x00005007
		public void SetActiveState(bool isActive)
		{
			if (isActive)
			{
				this.EnemySide.UpdateCharacterVisual();
				this.PlayerSide.UpdateCharacterVisual();
				return;
			}
			this.EnemySide.CurrentSelectedCharacter = null;
			this.PlayerSide.CurrentSelectedCharacter = null;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00006E3C File Offset: 0x0000503C
		private void OnSelectedCharactersChanged()
		{
			CustomBattleSideVM playerSide = this.PlayerSide;
			if (((playerSide != null) ? playerSide.CharacterSelectionGroup : null) != null)
			{
				CustomBattleSideVM enemySide = this.EnemySide;
				if (((enemySide != null) ? enemySide.CharacterSelectionGroup : null) != null)
				{
					CharacterItemVM selectedItem = this.PlayerSide.CharacterSelectionGroup.SelectedItem;
					BasicCharacterObject basicCharacterObject = ((selectedItem != null) ? selectedItem.Character : null);
					CharacterItemVM selectedItem2 = this.EnemySide.CharacterSelectionGroup.SelectedItem;
					BasicCharacterObject basicCharacterObject2 = ((selectedItem2 != null) ? selectedItem2.Character : null);
					foreach (CharacterItemVM characterItemVM in this.PlayerSide.CharacterSelectionGroup.ItemList)
					{
						characterItemVM.CanBeSelected = characterItemVM.Character != basicCharacterObject2;
					}
					foreach (CharacterItemVM characterItemVM2 in this.EnemySide.CharacterSelectionGroup.ItemList)
					{
						characterItemVM2.CanBeSelected = characterItemVM2.Character != basicCharacterObject;
					}
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00006F54 File Offset: 0x00005154
		private void OnPlayerTypeChange(CustomBattlePlayerType playerType)
		{
			this.PlayerSide.OnPlayerTypeChange(playerType);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006F62 File Offset: 0x00005162
		private void OnGameTypeChange(string gameTypeStringId)
		{
			this.MapSelectionGroup.OnGameTypeChange(gameTypeStringId);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00006F70 File Offset: 0x00005170
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RandomizeButtonText = GameTexts.FindText("str_randomize", null).ToString();
			this.StartButtonText = GameTexts.FindText("str_start", null).ToString();
			this.BackButtonText = GameTexts.FindText("str_back", null).ToString();
			this.SwitchButtonText = GameTexts.FindText("str_switch", null).ToString();
			this.TitleText = GameTexts.FindText("str_custom_battle", null).ToString();
			this.EnemySide.RefreshValues();
			this.PlayerSide.RefreshValues();
			this.AttackerMeleeMachines.ApplyActionOnAllItems(delegate(CustomBattleSiegeMachineVM x)
			{
				x.RefreshValues();
			});
			this.AttackerRangedMachines.ApplyActionOnAllItems(delegate(CustomBattleSiegeMachineVM x)
			{
				x.RefreshValues();
			});
			this.DefenderMachines.ApplyActionOnAllItems(delegate(CustomBattleSiegeMachineVM x)
			{
				x.RefreshValues();
			});
			this.MapSelectionGroup.RefreshValues();
			TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this.TroopTypeSelectionPopUp;
			if (troopTypeSelectionPopUp == null)
			{
				return;
			}
			troopTypeSelectionPopUp.RefreshValues();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000070A0 File Offset: 0x000052A0
		private void OnResetMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
		{
			selectedSlot.SetMachineType(null);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000070AC File Offset: 0x000052AC
		private void OnMeleeMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
		{
			List<InquiryElement> list = new List<InquiryElement>();
			list.Add(new InquiryElement(null, GameTexts.FindText("str_empty", null).ToString(), null));
			foreach (SiegeEngineType siegeEngineType in CustomBattleData.GetAllAttackerMeleeMachines())
			{
				list.Add(new InquiryElement(siegeEngineType, siegeEngineType.Name.ToString(), null));
			}
			MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=MVOWsP48}Select a Melee Machine", null).ToString(), string.Empty, list, true, 1, 1, GameTexts.FindText("str_done", null).ToString(), "", delegate(List<InquiryElement> selectedElements)
			{
				CustomBattleSiegeMachineVM selectedSlot2 = selectedSlot;
				InquiryElement inquiryElement = selectedElements.FirstOrDefault<InquiryElement>();
				selectedSlot2.SetMachineType(((inquiryElement != null) ? inquiryElement.Identifier : null) as SiegeEngineType);
			}, null, "", false), false, false);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00007188 File Offset: 0x00005388
		private void OnAttackerRangedMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
		{
			List<InquiryElement> list = new List<InquiryElement>();
			list.Add(new InquiryElement(null, GameTexts.FindText("str_empty", null).ToString(), null));
			foreach (SiegeEngineType siegeEngineType in CustomBattleData.GetAllAttackerRangedMachines())
			{
				list.Add(new InquiryElement(siegeEngineType, siegeEngineType.Name.ToString(), null));
			}
			MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=SLZzfNPr}Select a Ranged Machine", null).ToString(), string.Empty, list, true, 1, 1, GameTexts.FindText("str_done", null).ToString(), "", delegate(List<InquiryElement> selectedElements)
			{
				CustomBattleSiegeMachineVM selectedSlot2 = selectedSlot;
				InquiryElement inquiryElement = selectedElements.FirstOrDefault<InquiryElement>();
				selectedSlot2.SetMachineType(((inquiryElement != null) ? inquiryElement.Identifier : null) as SiegeEngineType);
			}, null, "", false), false, false);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00007264 File Offset: 0x00005464
		private void OnDefenderRangedMachineSelection(CustomBattleSiegeMachineVM selectedSlot)
		{
			List<InquiryElement> list = new List<InquiryElement>();
			list.Add(new InquiryElement(null, GameTexts.FindText("str_empty", null).ToString(), null));
			foreach (SiegeEngineType siegeEngineType in CustomBattleData.GetAllDefenderRangedMachines())
			{
				list.Add(new InquiryElement(siegeEngineType, siegeEngineType.Name.ToString(), null));
			}
			MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=SLZzfNPr}Select a Ranged Machine", null).ToString(), string.Empty, list, true, 1, 1, GameTexts.FindText("str_done", null).ToString(), "", delegate(List<InquiryElement> selectedElements)
			{
				CustomBattleSiegeMachineVM selectedSlot2 = selectedSlot;
				InquiryElement inquiryElement = selectedElements.FirstOrDefault<InquiryElement>();
				selectedSlot2.SetMachineType(((inquiryElement != null) ? inquiryElement.Identifier : null) as SiegeEngineType);
			}, null, "", false), false, false);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00007340 File Offset: 0x00005540
		private void ExecuteRandomizeAttackerSiegeEngines()
		{
			MBList<SiegeEngineType> mblist = new MBList<SiegeEngineType>();
			mblist.AddRange(CustomBattleData.GetAllAttackerMeleeMachines());
			mblist.Add(null);
			foreach (CustomBattleSiegeMachineVM customBattleSiegeMachineVM in this._attackerMeleeMachines)
			{
				customBattleSiegeMachineVM.SetMachineType(mblist.GetRandomElement<SiegeEngineType>());
			}
			mblist.Clear();
			mblist.AddRange(CustomBattleData.GetAllAttackerRangedMachines());
			mblist.Add(null);
			foreach (CustomBattleSiegeMachineVM customBattleSiegeMachineVM2 in this._attackerRangedMachines)
			{
				customBattleSiegeMachineVM2.SetMachineType(mblist.GetRandomElement<SiegeEngineType>());
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00007400 File Offset: 0x00005600
		private void ExecuteRandomizeDefenderSiegeEngines()
		{
			MBList<SiegeEngineType> mblist = new MBList<SiegeEngineType>();
			mblist.AddRange(CustomBattleData.GetAllDefenderRangedMachines());
			mblist.Add(null);
			foreach (CustomBattleSiegeMachineVM customBattleSiegeMachineVM in this._defenderMachines)
			{
				customBattleSiegeMachineVM.SetMachineType(mblist.GetRandomElement<SiegeEngineType>());
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00007468 File Offset: 0x00005668
		public void ExecuteBack()
		{
			Debug.Print("EXECUTE BACK - PRESSED", 0, Debug.DebugColor.Green, 17592186044416UL);
			Game.Current.GameStateManager.PopState(0);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00007490 File Offset: 0x00005690
		private CustomBattleData PrepareBattleData()
		{
			BasicCharacterObject selectedCharacter = this.PlayerSide.SelectedCharacter;
			BasicCharacterObject selectedCharacter2 = this.EnemySide.SelectedCharacter;
			int num = this.PlayerSide.CompositionGroup.ArmySize;
			int armySize = this.EnemySide.CompositionGroup.ArmySize;
			bool flag = this.GameTypeSelectionGroup.SelectedPlayerSide == CustomBattlePlayerSide.Attacker;
			bool flag2 = this.GameTypeSelectionGroup.SelectedPlayerType == CustomBattlePlayerType.Commander;
			BasicCharacterObject basicCharacterObject = null;
			if (!flag2)
			{
				MBList<BasicCharacterObject> mblist = CustomBattleData.Characters.ToMBList<BasicCharacterObject>();
				mblist.Remove(selectedCharacter);
				mblist.Remove(selectedCharacter2);
				basicCharacterObject = mblist.GetRandomElement<BasicCharacterObject>();
				num--;
			}
			int[] troopCounts = CustomBattleHelper.GetTroopCounts(num, CustomBattleVM.GetBattleCompositionDataFromCompositionGroup(this.PlayerSide.CompositionGroup));
			int[] troopCounts2 = CustomBattleHelper.GetTroopCounts(armySize, CustomBattleVM.GetBattleCompositionDataFromCompositionGroup(this.EnemySide.CompositionGroup));
			List<BasicCharacterObject>[] troopSelections = CustomBattleVM.GetTroopSelections(this.PlayerSide.CompositionGroup);
			List<BasicCharacterObject>[] troopSelections2 = CustomBattleVM.GetTroopSelections(this.EnemySide.CompositionGroup);
			BasicCultureObject faction = this.PlayerSide.FactionSelectionGroup.SelectedItem.Faction;
			BasicCultureObject faction2 = this.EnemySide.FactionSelectionGroup.SelectedItem.Faction;
			CustomBattleCombatant[] customBattleParties = CustomBattleHelper.GetCustomBattleParties(selectedCharacter, basicCharacterObject, selectedCharacter2, faction, troopCounts, troopSelections, faction2, troopCounts2, troopSelections2, flag);
			List<MissionSiegeWeapon> list = null;
			List<MissionSiegeWeapon> list2 = null;
			float[] array = null;
			if (this.GameTypeSelectionGroup.SelectedGameTypeString == "Siege")
			{
				list = new List<MissionSiegeWeapon>();
				list2 = new List<MissionSiegeWeapon>();
				CustomBattleVM.FillSiegeMachines(list, this._attackerMeleeMachines);
				CustomBattleVM.FillSiegeMachines(list, this._attackerRangedMachines);
				CustomBattleVM.FillSiegeMachines(list2, this._defenderMachines);
				array = CustomBattleHelper.GetWallHitpointPercentages(this.MapSelectionGroup.SelectedWallBreachedCount);
			}
			BasicCharacterObject basicCharacterObject2 = selectedCharacter;
			BasicCharacterObject basicCharacterObject3 = basicCharacterObject;
			CustomBattleCombatant customBattleCombatant = customBattleParties[0];
			CustomBattleCombatant customBattleCombatant2 = customBattleParties[1];
			CustomBattlePlayerSide selectedPlayerSide = this.GameTypeSelectionGroup.SelectedPlayerSide;
			CustomBattlePlayerType selectedPlayerType = this.GameTypeSelectionGroup.SelectedPlayerType;
			string selectedGameTypeString = this.GameTypeSelectionGroup.SelectedGameTypeString;
			MapItemVM selectedMap = this.MapSelectionGroup.SelectedMap;
			string text = ((selectedMap != null) ? selectedMap.MapId : null);
			string selectedSeasonId = this.MapSelectionGroup.SelectedSeasonId;
			float num2 = (float)this.MapSelectionGroup.SelectedTimeOfDay;
			List<MissionSiegeWeapon> list3 = list;
			List<MissionSiegeWeapon> list4 = list2;
			float[] array2 = array;
			int selectedSceneLevel = this.MapSelectionGroup.SelectedSceneLevel;
			bool isSallyOutSelected = this.MapSelectionGroup.IsSallyOutSelected;
			MapItemVM selectedMap2 = this.MapSelectionGroup.SelectedMap;
			return CustomBattleHelper.PrepareBattleData(basicCharacterObject2, basicCharacterObject3, customBattleCombatant, customBattleCombatant2, selectedPlayerSide, selectedPlayerType, selectedGameTypeString, text, selectedSeasonId, num2, list3, list4, array2, selectedSceneLevel, isSallyOutSelected, (selectedMap2 != null) ? selectedMap2.ForcedSceneLevel : null);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000076AF File Offset: 0x000058AF
		public void ExecuteStart()
		{
			CustomBattleHelper.StartGame(this.PrepareBattleData());
			Debug.Print("EXECUTE START - PRESSED", 0, Debug.DebugColor.Green, 17592186044416UL);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000076D4 File Offset: 0x000058D4
		public void ExecuteRandomize()
		{
			this.GameTypeSelectionGroup.RandomizeAll();
			this.MapSelectionGroup.RandomizeAll();
			this.PlayerSide.Randomize(null);
			this.EnemySide.Randomize(this.PlayerSide);
			this.ExecuteRandomizeAttackerSiegeEngines();
			this.ExecuteRandomizeDefenderSiegeEngines();
			Debug.Print("EXECUTE RANDOMIZE - PRESSED", 0, Debug.DebugColor.Green, 17592186044416UL);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00007735 File Offset: 0x00005935
		private void ExecuteDoneDefenderCustomMachineSelection()
		{
			this.IsDefenderCustomMachineSelectionEnabled = false;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000773E File Offset: 0x0000593E
		private void ExecuteDoneAttackerCustomMachineSelection()
		{
			this.IsAttackerCustomMachineSelectionEnabled = false;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00007748 File Offset: 0x00005948
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.StartInputKey.OnFinalize();
			this.CancelInputKey.OnFinalize();
			this.ResetInputKey.OnFinalize();
			this.RandomizeInputKey.OnFinalize();
			TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this.TroopTypeSelectionPopUp;
			if (troopTypeSelectionPopUp == null)
			{
				return;
			}
			troopTypeSelectionPopUp.OnFinalize();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00007797 File Offset: 0x00005997
		public void ExecuteSwitchToNextCustomBattle()
		{
			if (this.CanSwitchMode)
			{
				this.ExecuteBack();
				GameStateManager.Current = Module.CurrentModule.GlobalGameStateManager;
				this._nextCustomBattleProvider.StartCustomBattle();
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000077C1 File Offset: 0x000059C1
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000077C9 File Offset: 0x000059C9
		[DataSourceProperty]
		public TroopTypeSelectionPopUpVM TroopTypeSelectionPopUp
		{
			get
			{
				return this._troopTypeSelectionPopUp;
			}
			set
			{
				if (value != this._troopTypeSelectionPopUp)
				{
					this._troopTypeSelectionPopUp = value;
					base.OnPropertyChangedWithValue<TroopTypeSelectionPopUpVM>(value, "TroopTypeSelectionPopUp");
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000077E7 File Offset: 0x000059E7
		// (set) Token: 0x060000AE RID: 174 RVA: 0x000077EF File Offset: 0x000059EF
		[DataSourceProperty]
		public bool IsAttackerCustomMachineSelectionEnabled
		{
			get
			{
				return this._isAttackerCustomMachineSelectionEnabled;
			}
			set
			{
				if (value != this._isAttackerCustomMachineSelectionEnabled)
				{
					this._isAttackerCustomMachineSelectionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsAttackerCustomMachineSelectionEnabled");
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000780D File Offset: 0x00005A0D
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00007815 File Offset: 0x00005A15
		[DataSourceProperty]
		public bool IsDefenderCustomMachineSelectionEnabled
		{
			get
			{
				return this._isDefenderCustomMachineSelectionEnabled;
			}
			set
			{
				if (value != this._isDefenderCustomMachineSelectionEnabled)
				{
					this._isDefenderCustomMachineSelectionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsDefenderCustomMachineSelectionEnabled");
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00007833 File Offset: 0x00005A33
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x0000783B File Offset: 0x00005A3B
		[DataSourceProperty]
		public string RandomizeButtonText
		{
			get
			{
				return this._randomizeButtonText;
			}
			set
			{
				if (value != this._randomizeButtonText)
				{
					this._randomizeButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "RandomizeButtonText");
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000785E File Offset: 0x00005A5E
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x00007866 File Offset: 0x00005A66
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00007889 File Offset: 0x00005A89
		// (set) Token: 0x060000B6 RID: 182 RVA: 0x00007891 File Offset: 0x00005A91
		[DataSourceProperty]
		public string BackButtonText
		{
			get
			{
				return this._backButtonText;
			}
			set
			{
				if (value != this._backButtonText)
				{
					this._backButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "BackButtonText");
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000078B4 File Offset: 0x00005AB4
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x000078BC File Offset: 0x00005ABC
		[DataSourceProperty]
		public string StartButtonText
		{
			get
			{
				return this._startButtonText;
			}
			set
			{
				if (value != this._startButtonText)
				{
					this._startButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "StartButtonText");
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000078DF File Offset: 0x00005ADF
		// (set) Token: 0x060000BA RID: 186 RVA: 0x000078E7 File Offset: 0x00005AE7
		[DataSourceProperty]
		public string SwitchButtonText
		{
			get
			{
				return this._switchButtonText;
			}
			set
			{
				if (value != this._switchButtonText)
				{
					this._switchButtonText = value;
					base.OnPropertyChangedWithValue<string>(value, "SwitchButtonText");
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BB RID: 187 RVA: 0x0000790A File Offset: 0x00005B0A
		// (set) Token: 0x060000BC RID: 188 RVA: 0x00007912 File Offset: 0x00005B12
		[DataSourceProperty]
		public CustomBattleSideVM EnemySide
		{
			get
			{
				return this._enemySide;
			}
			set
			{
				if (value != this._enemySide)
				{
					this._enemySide = value;
					base.OnPropertyChangedWithValue<CustomBattleSideVM>(value, "EnemySide");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00007930 File Offset: 0x00005B30
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00007938 File Offset: 0x00005B38
		[DataSourceProperty]
		public CustomBattleSideVM PlayerSide
		{
			get
			{
				return this._playerSide;
			}
			set
			{
				if (value != this._playerSide)
				{
					this._playerSide = value;
					base.OnPropertyChangedWithValue<CustomBattleSideVM>(value, "PlayerSide");
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00007956 File Offset: 0x00005B56
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000795E File Offset: 0x00005B5E
		[DataSourceProperty]
		public GameTypeSelectionGroupVM GameTypeSelectionGroup
		{
			get
			{
				return this._gameTypeSelectionGroup;
			}
			set
			{
				if (value != this._gameTypeSelectionGroup)
				{
					this._gameTypeSelectionGroup = value;
					base.OnPropertyChangedWithValue<GameTypeSelectionGroupVM>(value, "GameTypeSelectionGroup");
				}
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000797C File Offset: 0x00005B7C
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00007984 File Offset: 0x00005B84
		[DataSourceProperty]
		public MapSelectionGroupVM MapSelectionGroup
		{
			get
			{
				return this._mapSelectionGroup;
			}
			set
			{
				if (value != this._mapSelectionGroup)
				{
					this._mapSelectionGroup = value;
					base.OnPropertyChangedWithValue<MapSelectionGroupVM>(value, "MapSelectionGroup");
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x000079A2 File Offset: 0x00005BA2
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000079AA File Offset: 0x00005BAA
		[DataSourceProperty]
		public MBBindingList<CustomBattleSiegeMachineVM> AttackerMeleeMachines
		{
			get
			{
				return this._attackerMeleeMachines;
			}
			set
			{
				if (value != this._attackerMeleeMachines)
				{
					this._attackerMeleeMachines = value;
					base.OnPropertyChangedWithValue<MBBindingList<CustomBattleSiegeMachineVM>>(value, "AttackerMeleeMachines");
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000079C8 File Offset: 0x00005BC8
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x000079D0 File Offset: 0x00005BD0
		[DataSourceProperty]
		public MBBindingList<CustomBattleSiegeMachineVM> AttackerRangedMachines
		{
			get
			{
				return this._attackerRangedMachines;
			}
			set
			{
				if (value != this._attackerRangedMachines)
				{
					this._attackerRangedMachines = value;
					base.OnPropertyChangedWithValue<MBBindingList<CustomBattleSiegeMachineVM>>(value, "AttackerRangedMachines");
				}
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000079EE File Offset: 0x00005BEE
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x000079F6 File Offset: 0x00005BF6
		[DataSourceProperty]
		public MBBindingList<CustomBattleSiegeMachineVM> DefenderMachines
		{
			get
			{
				return this._defenderMachines;
			}
			set
			{
				if (value != this._defenderMachines)
				{
					this._defenderMachines = value;
					base.OnPropertyChangedWithValue<MBBindingList<CustomBattleSiegeMachineVM>>(value, "DefenderMachines");
				}
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00007A14 File Offset: 0x00005C14
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00007A1C File Offset: 0x00005C1C
		[DataSourceProperty]
		public bool CanSwitchMode
		{
			get
			{
				return this._CanSwitchMode;
			}
			set
			{
				if (value != this._CanSwitchMode)
				{
					this._CanSwitchMode = value;
					base.OnPropertyChangedWithValue(value, "CanSwitchMode");
				}
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00007A3A File Offset: 0x00005C3A
		// (set) Token: 0x060000CC RID: 204 RVA: 0x00007A42 File Offset: 0x00005C42
		[DataSourceProperty]
		public HintViewModel SwitchHint
		{
			get
			{
				return this._switchHint;
			}
			set
			{
				if (value != this._switchHint)
				{
					this._switchHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SwitchHint");
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00007A60 File Offset: 0x00005C60
		public void SetStartInputKey(HotKey hotkey)
		{
			this.StartInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00007A6F File Offset: 0x00005C6F
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
			TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this.TroopTypeSelectionPopUp;
			if (troopTypeSelectionPopUp == null)
			{
				return;
			}
			troopTypeSelectionPopUp.SetCancelInputKey(hotkey);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007A8F File Offset: 0x00005C8F
		public void SetResetInputKey(HotKey hotkey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
			TroopTypeSelectionPopUpVM troopTypeSelectionPopUp = this.TroopTypeSelectionPopUp;
			if (troopTypeSelectionPopUp == null)
			{
				return;
			}
			troopTypeSelectionPopUp.SetResetInputKey(hotkey);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00007AAF File Offset: 0x00005CAF
		public void SetRandomizeInputKey(HotKey hotkey)
		{
			this.RandomizeInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00007ABE File Offset: 0x00005CBE
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x00007AC6 File Offset: 0x00005CC6
		public InputKeyItemVM StartInputKey
		{
			get
			{
				return this._startInputKey;
			}
			set
			{
				if (value != this._startInputKey)
				{
					this._startInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "StartInputKey");
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00007AE4 File Offset: 0x00005CE4
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x00007AEC File Offset: 0x00005CEC
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00007B0A File Offset: 0x00005D0A
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00007B12 File Offset: 0x00005D12
		public InputKeyItemVM ResetInputKey
		{
			get
			{
				return this._resetInputKey;
			}
			set
			{
				if (value != this._resetInputKey)
				{
					this._resetInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ResetInputKey");
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00007B30 File Offset: 0x00005D30
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00007B38 File Offset: 0x00005D38
		public InputKeyItemVM RandomizeInputKey
		{
			get
			{
				return this._randomizeInputKey;
			}
			set
			{
				if (value != this._randomizeInputKey)
				{
					this._randomizeInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "RandomizeInputKey");
				}
			}
		}

		// Token: 0x04000065 RID: 101
		private readonly ICustomBattleProvider _nextCustomBattleProvider;

		// Token: 0x04000066 RID: 102
		private CustomBattleState _customBattleState;

		// Token: 0x04000067 RID: 103
		private TroopTypeSelectionPopUpVM _troopTypeSelectionPopUp;

		// Token: 0x04000068 RID: 104
		private CustomBattleSideVM _enemySide;

		// Token: 0x04000069 RID: 105
		private CustomBattleSideVM _playerSide;

		// Token: 0x0400006A RID: 106
		private bool _isAttackerCustomMachineSelectionEnabled;

		// Token: 0x0400006B RID: 107
		private bool _isDefenderCustomMachineSelectionEnabled;

		// Token: 0x0400006C RID: 108
		private GameTypeSelectionGroupVM _gameTypeSelectionGroup;

		// Token: 0x0400006D RID: 109
		private MapSelectionGroupVM _mapSelectionGroup;

		// Token: 0x0400006E RID: 110
		private string _randomizeButtonText;

		// Token: 0x0400006F RID: 111
		private string _backButtonText;

		// Token: 0x04000070 RID: 112
		private string _startButtonText;

		// Token: 0x04000071 RID: 113
		private string _titleText;

		// Token: 0x04000072 RID: 114
		private string _switchButtonText;

		// Token: 0x04000073 RID: 115
		private bool _CanSwitchMode;

		// Token: 0x04000074 RID: 116
		private HintViewModel _switchHint;

		// Token: 0x04000075 RID: 117
		private MBBindingList<CustomBattleSiegeMachineVM> _attackerMeleeMachines;

		// Token: 0x04000076 RID: 118
		private MBBindingList<CustomBattleSiegeMachineVM> _attackerRangedMachines;

		// Token: 0x04000077 RID: 119
		private MBBindingList<CustomBattleSiegeMachineVM> _defenderMachines;

		// Token: 0x04000078 RID: 120
		private InputKeyItemVM _startInputKey;

		// Token: 0x04000079 RID: 121
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400007A RID: 122
		private InputKeyItemVM _resetInputKey;

		// Token: 0x0400007B RID: 123
		private InputKeyItemVM _randomizeInputKey;
	}
}
