using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.OrderOfBattle
{
	// Token: 0x02000033 RID: 51
	public class OrderOfBattleFormationItemVM : ViewModel
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000DA2D File Offset: 0x0000BC2D
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000DA35 File Offset: 0x0000BC35
		public Formation Formation { get; private set; }

		// Token: 0x060003B7 RID: 951 RVA: 0x0000DA40 File Offset: 0x0000BC40
		public OrderOfBattleFormationItemVM(Camera missionCamera)
		{
			this._missionCamera = missionCamera;
			this.Formation = null;
			this._bannerBearerLogic = Mission.Current.GetMissionBehavior<BannerBearerLogic>();
			this.HasFormation = false;
			this.FilterItems = new MBBindingList<OrderOfBattleFormationFilterSelectorItemVM>();
			for (FormationFilterType formationFilterType = FormationFilterType.Shield; formationFilterType < FormationFilterType.NumberOfFilterTypes; formationFilterType++)
			{
				this.FilterItems.Add(new OrderOfBattleFormationFilterSelectorItemVM(formationFilterType, new Action<OrderOfBattleFormationFilterSelectorItemVM>(this.OnFilterToggled)));
			}
			this.FormationClassSelector = new SelectorVM<OrderOfBattleFormationClassSelectorItemVM>(0, new Action<SelectorVM<OrderOfBattleFormationClassSelectorItemVM>>(this.OnClassChanged));
			for (DeploymentFormationClass deploymentFormationClass = DeploymentFormationClass.Unset; deploymentFormationClass <= DeploymentFormationClass.CavalryAndHorseArcher; deploymentFormationClass++)
			{
				if (!Mission.Current.IsSiegeBattle || (deploymentFormationClass != DeploymentFormationClass.Cavalry && deploymentFormationClass != DeploymentFormationClass.HorseArcher && deploymentFormationClass != DeploymentFormationClass.CavalryAndHorseArcher))
				{
					this.FormationClassSelector.AddItem(new OrderOfBattleFormationClassSelectorItemVM(deploymentFormationClass));
				}
			}
			this.Classes = new MBBindingList<OrderOfBattleFormationClassVM>
			{
				new OrderOfBattleFormationClassVM(this, FormationClass.NumberOfAllFormations),
				new OrderOfBattleFormationClassVM(this, FormationClass.NumberOfAllFormations)
			};
			this.HeroTroops = new MBBindingList<OrderOfBattleHeroItemVM>();
			this._unassignedCaptain = new OrderOfBattleHeroItemVM();
			this.Captain = this._unassignedCaptain;
			this.Tooltip = new BasicTooltipViewModel(() => this.GetTooltip());
			this.BannerBearerTooltip = new BasicTooltipViewModel(() => this.GetBannerBearerTooltip());
			this.IsControlledByPlayer = Mission.Current.PlayerTeam.IsPlayerGeneral;
			this._worldPosition = Vec3.Zero;
			this.RefreshValues();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000DC20 File Offset: 0x0000BE20
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.FormationIsEmptyText = new TextObject("{=P3IWytsr}Formation is currently empty", null).ToString();
			this.CaptainSlotHint = new HintViewModel(this._captainSlotHintText, null);
			this.HeroTroopSlotHint = new HintViewModel(this._heroTroopSlotHintText, null);
			this.AssignCaptainHint = new HintViewModel(this._assignCaptainHintText, null);
			this.AssignHeroTroopHint = new HintViewModel(this._assignHeroTroopHintText, null);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000DC94 File Offset: 0x0000BE94
		public void Tick()
		{
			if (this._isMarkerWorldPositionDirty)
			{
				this._isMarkerWorldPositionDirty = false;
				this.RefreshMarkerWorldPosition();
			}
			this.Classes.ApplyActionOnAllItems(delegate(OrderOfBattleFormationClassVM c)
			{
				c.UpdateWeightAdjustable();
			});
			this.UpdateAdjustable();
			bool flag;
			if (this.Formation.CountOfUnits != 0)
			{
				flag = this.Classes.Any<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => c.Class != FormationClass.NumberOfAllFormations);
			}
			else
			{
				flag = false;
			}
			this.IsMarkerShown = flag;
			if (!this.IsMarkerShown)
			{
				return;
			}
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(this._missionCamera, this._worldPosition, ref this._latestX, ref this._latestY, ref this._latestW);
			this.ScreenPosition = new Vec2(this._latestX, this._latestY);
			this._wPosAfterPositionCalculation = ((this._latestW < 0f) ? (-1f) : 1.1f);
			this.WSign = (int)this._wPosAfterPositionCalculation;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000DDB8 File Offset: 0x0000BFB8
		public void RefreshFormation(Formation formation, DeploymentFormationClass overriddenClass = DeploymentFormationClass.Unset, bool mustExist = false)
		{
			this.Formation = formation;
			if (formation.CountOfUnits != 0 || mustExist)
			{
				DeploymentFormationClass formationTypeToSet = DeploymentFormationClass.Unset;
				if (overriddenClass != DeploymentFormationClass.Unset)
				{
					formationTypeToSet = overriddenClass;
				}
				else
				{
					FormationClass formationClass = FormationClass.NumberOfAllFormations;
					if (formation.SecondaryLogicalClasses.Count<FormationClass>() > 0)
					{
						formationClass = formation.SecondaryLogicalClasses.FirstOrDefault<FormationClass>();
						if (formation.GetCountOfUnitsBelongingToLogicalClass(formationClass) == 0)
						{
							formationClass = FormationClass.NumberOfAllFormations;
						}
					}
					switch (formation.LogicalClass)
					{
					case FormationClass.Infantry:
						formationTypeToSet = ((formationClass == FormationClass.Ranged) ? DeploymentFormationClass.InfantryAndRanged : DeploymentFormationClass.Infantry);
						break;
					case FormationClass.Ranged:
						formationTypeToSet = ((formationClass == FormationClass.Infantry) ? DeploymentFormationClass.InfantryAndRanged : DeploymentFormationClass.Ranged);
						break;
					case FormationClass.Cavalry:
						formationTypeToSet = ((formationClass == FormationClass.HorseArcher) ? DeploymentFormationClass.CavalryAndHorseArcher : DeploymentFormationClass.Cavalry);
						break;
					case FormationClass.HorseArcher:
						formationTypeToSet = ((formationClass == FormationClass.Cavalry) ? DeploymentFormationClass.CavalryAndHorseArcher : DeploymentFormationClass.HorseArcher);
						break;
					default:
						Debug.FailedAssert("Formation doesn't have a proper primary class. Value : " + formation.PhysicalClass.GetName(), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleFormationItemVM.cs", "RefreshFormation", 182);
						break;
					}
				}
				OrderOfBattleFormationClassSelectorItemVM orderOfBattleFormationClassSelectorItemVM = this.FormationClassSelector.ItemList.SingleOrDefault<OrderOfBattleFormationClassSelectorItemVM>((OrderOfBattleFormationClassSelectorItemVM i) => i.FormationClass == formationTypeToSet);
				int num = this.FormationClassSelector.ItemList.IndexOf(orderOfBattleFormationClassSelectorItemVM);
				if (num != -1)
				{
					this.FormationClassSelector.SelectedIndex = num;
				}
			}
			else
			{
				this.FormationClassSelector.SelectedIndex = 0;
			}
			this.TitleText = (this.Formation.Index + 1).ToString();
			this.OnSizeChanged();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000DF19 File Offset: 0x0000C119
		public void MakeMarkerWorldPositionDirty()
		{
			this._isMarkerWorldPositionDirty = true;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000DF24 File Offset: 0x0000C124
		private void RefreshMarkerWorldPosition()
		{
			if (this.Formation == null)
			{
				return;
			}
			Agent medianAgent = this.Formation.GetMedianAgent(false, false, this.Formation.GetAveragePositionOfUnits(false, false));
			if (medianAgent == null)
			{
				return;
			}
			this._worldPosition = medianAgent.GetWorldPosition().GetGroundVec3();
			this._worldPosition += new Vec3(0f, 0f, medianAgent.GetEyeGlobalHeight(), -1f);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000DF98 File Offset: 0x0000C198
		public void OnSizeChanged()
		{
			Formation formation = this.Formation;
			this.TroopCount = ((formation != null) ? formation.CountOfUnits : 0);
			this.BannerBearerCount = ((this.Formation != null) ? this._bannerBearerLogic.GetFormationBannerBearers(this.Formation).Count : 0);
			this.RefreshMarkerWorldPosition();
			this.IsSelectable = this.FormationClassSelector.SelectedIndex != 0 && this.IsControlledByPlayer && this.TroopCount > 0;
			if (!this.IsSelectable && this.IsSelected)
			{
				Action<OrderOfBattleFormationItemVM> onDeselection = OrderOfBattleFormationItemVM.OnDeselection;
				if (onDeselection != null)
				{
					onDeselection(this);
				}
			}
			foreach (OrderOfBattleFormationClassVM orderOfBattleFormationClassVM in this.Classes)
			{
				orderOfBattleFormationClassVM.UpdateTroopCountText();
			}
			this.UpdateAdjustable();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000E078 File Offset: 0x0000C278
		private void OnClassChanged(SelectorVM<OrderOfBattleFormationClassSelectorItemVM> formationClassSelector)
		{
			if (this.Classes == null)
			{
				return;
			}
			DeploymentFormationClass formationClass = formationClassSelector.SelectedItem.FormationClass;
			this.OrderOfBattleFormationClassInt = (int)formationClass;
			switch (formationClass)
			{
			case DeploymentFormationClass.Unset:
			{
				this.Classes[0].Class = FormationClass.NumberOfAllFormations;
				this.Classes[1].Class = FormationClass.NumberOfAllFormations;
				if (this.Captain != this._unassignedCaptain)
				{
					this.UnassignCaptain();
				}
				List<OrderOfBattleHeroItemVM> list = this.HeroTroops.ToList<OrderOfBattleHeroItemVM>();
				for (int i = 0; i < list.Count; i++)
				{
					this.RemoveHeroTroop(list[i]);
				}
				for (int j = this.FilterItems.Count - 1; j >= 0; j--)
				{
					this.FilterItems[j].IsActive = false;
				}
				break;
			}
			case DeploymentFormationClass.Infantry:
				this.Classes[0].Class = FormationClass.Infantry;
				this.Classes[1].Class = FormationClass.NumberOfAllFormations;
				break;
			case DeploymentFormationClass.Ranged:
				this.Classes[0].Class = FormationClass.Ranged;
				this.Classes[1].Class = FormationClass.NumberOfAllFormations;
				break;
			case DeploymentFormationClass.Cavalry:
				this.Classes[0].Class = FormationClass.Cavalry;
				this.Classes[1].Class = FormationClass.NumberOfAllFormations;
				break;
			case DeploymentFormationClass.HorseArcher:
				this.Classes[0].Class = FormationClass.HorseArcher;
				this.Classes[1].Class = FormationClass.NumberOfAllFormations;
				break;
			case DeploymentFormationClass.InfantryAndRanged:
				this.Classes[0].Class = FormationClass.Infantry;
				this.Classes[1].Class = FormationClass.Ranged;
				break;
			case DeploymentFormationClass.CavalryAndHorseArcher:
				this.Classes[0].Class = FormationClass.Cavalry;
				this.Classes[1].Class = FormationClass.HorseArcher;
				break;
			}
			foreach (OrderOfBattleFormationClassVM orderOfBattleFormationClassVM in this.Classes)
			{
				orderOfBattleFormationClassVM.IsLocked = false;
				orderOfBattleFormationClassVM.Weight = 0;
			}
			this.HasFormation = this.Classes.Any<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => c.Class != FormationClass.NumberOfAllFormations);
			this.UpdateAdjustable();
			Action onFormationClassChanged = OrderOfBattleFormationItemVM.OnFormationClassChanged;
			if (onFormationClassChanged == null)
			{
				return;
			}
			onFormationClassChanged();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		public DeploymentFormationClass GetOrderOfBattleClass()
		{
			if (this.Classes[0].Class == FormationClass.Infantry && this.Classes[1].Class == FormationClass.NumberOfAllFormations)
			{
				return DeploymentFormationClass.Infantry;
			}
			if (this.Classes[0].Class == FormationClass.Ranged && this.Classes[1].Class == FormationClass.NumberOfAllFormations)
			{
				return DeploymentFormationClass.Ranged;
			}
			if (this.Classes[0].Class == FormationClass.Cavalry && this.Classes[1].Class == FormationClass.NumberOfAllFormations)
			{
				return DeploymentFormationClass.Cavalry;
			}
			if (this.Classes[0].Class == FormationClass.HorseArcher && this.Classes[1].Class == FormationClass.NumberOfAllFormations)
			{
				return DeploymentFormationClass.HorseArcher;
			}
			if (this.Classes[0].Class == FormationClass.Infantry && this.Classes[1].Class == FormationClass.Ranged)
			{
				return DeploymentFormationClass.InfantryAndRanged;
			}
			if (this.Classes[0].Class == FormationClass.Cavalry && this.Classes[1].Class == FormationClass.HorseArcher)
			{
				return DeploymentFormationClass.CavalryAndHorseArcher;
			}
			return DeploymentFormationClass.Unset;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
		private void OnFilterToggled(OrderOfBattleFormationFilterSelectorItemVM filterItem)
		{
			Action<OrderOfBattleFormationItemVM> onFilterUseToggled = OrderOfBattleFormationItemVM.OnFilterUseToggled;
			if (onFilterUseToggled == null)
			{
				return;
			}
			onFilterUseToggled(this);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000E3F6 File Offset: 0x0000C5F6
		private bool HasAnyActiveFilter()
		{
			return this.HasFilter(FormationFilterType.Shield) || this.HasFilter(FormationFilterType.Spear) || this.HasFilter(FormationFilterType.Thrown) || this.HasFilter(FormationFilterType.Heavy) || this.HasFilter(FormationFilterType.HighTier) || this.HasFilter(FormationFilterType.LowTier);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000E430 File Offset: 0x0000C630
		public void UpdateAdjustable()
		{
			bool flag;
			if (this.IsControlledByPlayer)
			{
				flag = this.Classes.All<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => c.Class == FormationClass.NumberOfAllFormations || c.IsAdjustable || !OrderOfBattleFormationItemVM.HasAnyTroopWithClass(c.Class));
			}
			else
			{
				flag = false;
			}
			this.IsAdjustable = flag;
			if (!this.IsControlledByPlayer)
			{
				this.CantAdjustHint = new HintViewModel(this._cantAdjustNotCommanderText, null);
				return;
			}
			if (!this.Classes.All<OrderOfBattleFormationClassVM>((OrderOfBattleFormationClassVM c) => c.Class == FormationClass.NumberOfAllFormations || c.IsAdjustable))
			{
				this.CantAdjustHint = new HintViewModel(this._cantAdjustSingledOutText, null);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000E4D4 File Offset: 0x0000C6D4
		public bool HasFilter(FormationFilterType filter)
		{
			return this.FilterItems.Any<OrderOfBattleFormationFilterSelectorItemVM>((OrderOfBattleFormationFilterSelectorItemVM f) => f.IsActive && f.FilterType == filter);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000E508 File Offset: 0x0000C708
		public bool HasOnlyOneClass()
		{
			int num = 0;
			for (int i = 0; i < this.Classes.Count; i++)
			{
				if (!this.Classes[i].IsUnset)
				{
					num++;
				}
			}
			return num == 1;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000E548 File Offset: 0x0000C748
		public bool HasClass(FormationClass formationClass)
		{
			for (int i = 0; i < this.Classes.Count; i++)
			{
				if (this.Classes[i].Class == formationClass && !this.Classes[i].IsUnset)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000E598 File Offset: 0x0000C798
		public bool HasClasses(FormationClass[] formationClasses)
		{
			FormationClass[] array = (from c in this.Classes
				select c.Class into c
				where c != FormationClass.NumberOfAllFormations
				select c).ToArray<FormationClass>();
			return formationClasses.OrderBy<FormationClass, FormationClass>((FormationClass c) => c).SequenceEqual<FormationClass>(array.OrderBy<FormationClass, FormationClass>((FormationClass c) => c));
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000E648 File Offset: 0x0000C848
		private List<TooltipProperty> GetTooltip()
		{
			GameTexts.SetVariable("NUMBER", this.TitleText);
			List<TooltipProperty> list = new List<TooltipProperty>
			{
				new TooltipProperty(this._formationTooltipTitleText.ToString(), string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.Title)
			};
			if (this.FormationClassSelector.SelectedItem == null)
			{
				return list;
			}
			List<Agent> list2 = new List<Agent>();
			int[] array = new int[4];
			int[] array2 = new int[4];
			using (List<IFormationUnit>.Enumerator enumerator = this.Formation.Arrangement.GetAllUnits().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Agent agent5;
					if ((agent5 = enumerator.Current as Agent) != null)
					{
						if (agent5.IsHero)
						{
							list2.Add(agent5);
						}
						FormationClass actualTroopType = this.GetActualTroopType(agent5);
						if (actualTroopType >= FormationClass.Infantry && actualTroopType < FormationClass.NumberOfDefaultFormations)
						{
							array[(int)actualTroopType]++;
							if (agent5.Banner != null)
							{
								array2[(int)actualTroopType]++;
							}
						}
					}
				}
			}
			foreach (Agent agent2 in this.Formation.DetachedUnits)
			{
				if (agent2.IsHero)
				{
					list2.Add(agent2);
				}
				FormationClass actualTroopType2 = this.GetActualTroopType(agent2);
				if (actualTroopType2 >= FormationClass.Infantry && actualTroopType2 < FormationClass.NumberOfDefaultFormations)
				{
					array[(int)actualTroopType2]++;
					if (agent2.Banner != null)
					{
						array2[(int)actualTroopType2]++;
					}
				}
			}
			bool flag = false;
			for (FormationClass formationClass = FormationClass.Infantry; formationClass < FormationClass.NumberOfDefaultFormations; formationClass++)
			{
				int num = array[(int)formationClass];
				int num2 = array2[(int)formationClass];
				List<Agent> list3 = new List<Agent>();
				for (int i = 0; i < list2.Count; i++)
				{
					Agent agent3 = list2[i];
					if (formationClass == this.GetActualTroopType(agent3))
					{
						list3.Add(agent3);
					}
				}
				if (num > 0)
				{
					if (flag)
					{
						list.Add(new TooltipProperty(string.Empty, string.Empty, -1, false, TooltipProperty.TooltipPropertyFlags.None));
					}
					else
					{
						flag = true;
					}
					int num3 = OrderOfBattleFormationClassVM.GetTotalCountOfTroopType(formationClass);
					if (num3 < num)
					{
						Debug.FailedAssert(string.Format("Total troop count of type {0} is lower than the individually calculated troopCount!", formationClass), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\OrderOfBattle\\OrderOfBattleFormationItemVM.cs", "GetTooltip", 537);
						num3 = num;
					}
					int num4 = MathF.Ceiling(100f * (float)num / (float)num3);
					string text = new TextObject("{=9pCzjSTa}{PERCENTAGE}% of troop type", null).SetTextVariable("PERCENTAGE", num4).ToString();
					string text2 = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).SetTextVariable("RANK", num.ToString()).SetTextVariable("NUMBER", text)
						.ToString();
					List<TooltipProperty> list4 = list;
					string text3 = "str_troop_group_name";
					int num5 = (int)formationClass;
					list4.Add(new TooltipProperty(GameTexts.FindText(text3, num5.ToString()).ToString(), text2, 0, false, TooltipProperty.TooltipPropertyFlags.None));
					if (list3.Count > 0 || num2 > 0)
					{
						list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.RundownSeperator));
					}
					foreach (Agent agent4 in list3)
					{
						list.Add(new TooltipProperty(agent4.Name, " ", 0, false, TooltipProperty.TooltipPropertyFlags.None));
					}
					if (num2 > 0)
					{
						list.Add(new TooltipProperty(new TextObject("{=scnSXrYC}Banner Bearers", null).ToString(), num2.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
					}
				}
			}
			if (this.HasAnyActiveFilter())
			{
				list.Add(new TooltipProperty(string.Empty, string.Empty, 0, false, TooltipProperty.TooltipPropertyFlags.DefaultSeperator));
			}
			DeploymentFormationClass formationClass2 = this.FormationClassSelector.SelectedItem.FormationClass;
			if (this.HasFilter(FormationFilterType.Shield))
			{
				GameTexts.SetVariable("TROOP_COUNT", this.Formation.GetCountOfUnitsWithCondition((Agent agent) => agent.HasShieldCached));
				GameTexts.SetVariable("TOTAL_TROOP_COUNT", OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter(formationClass2, FormationFilterType.Shield));
				list.Add(new TooltipProperty(FormationFilterType.Shield.GetFilterName().ToString(), this._filteredTroopCountInfoText.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			if (this.HasFilter(FormationFilterType.Spear))
			{
				GameTexts.SetVariable("TROOP_COUNT", this.Formation.GetCountOfUnitsWithCondition((Agent agent) => agent.HasSpearCached));
				GameTexts.SetVariable("TOTAL_TROOP_COUNT", OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter(formationClass2, FormationFilterType.Spear));
				list.Add(new TooltipProperty(FormationFilterType.Spear.GetFilterName().ToString(), this._filteredTroopCountInfoText.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			if (this.HasFilter(FormationFilterType.Thrown))
			{
				GameTexts.SetVariable("TROOP_COUNT", this.Formation.GetCountOfUnitsWithCondition((Agent agent) => agent.HasThrownCached));
				GameTexts.SetVariable("TOTAL_TROOP_COUNT", OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter(formationClass2, FormationFilterType.Thrown));
				list.Add(new TooltipProperty(FormationFilterType.Thrown.GetFilterName().ToString(), this._filteredTroopCountInfoText.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			if (this.HasFilter(FormationFilterType.Heavy))
			{
				GameTexts.SetVariable("TROOP_COUNT", this.Formation.GetCountOfUnitsWithCondition((Agent agent) => MissionGameModels.Current.AgentStatCalculateModel.HasHeavyArmor(agent)));
				GameTexts.SetVariable("TOTAL_TROOP_COUNT", OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter(formationClass2, FormationFilterType.Heavy));
				list.Add(new TooltipProperty(FormationFilterType.Heavy.GetFilterName().ToString(), this._filteredTroopCountInfoText.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			if (this.HasFilter(FormationFilterType.HighTier))
			{
				GameTexts.SetVariable("TROOP_COUNT", this.Formation.GetCountOfUnitsWithCondition((Agent agent) => agent.Character.GetBattleTier() >= 4));
				GameTexts.SetVariable("TOTAL_TROOP_COUNT", OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter(formationClass2, FormationFilterType.HighTier));
				list.Add(new TooltipProperty(FormationFilterType.HighTier.GetFilterName().ToString(), this._filteredTroopCountInfoText.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			if (this.HasFilter(FormationFilterType.LowTier))
			{
				GameTexts.SetVariable("TROOP_COUNT", this.Formation.GetCountOfUnitsWithCondition((Agent agent) => agent.Character.GetBattleTier() <= 3));
				GameTexts.SetVariable("TOTAL_TROOP_COUNT", OrderOfBattleFormationItemVM.GetTotalTroopCountWithFilter(formationClass2, FormationFilterType.LowTier));
				list.Add(new TooltipProperty(FormationFilterType.LowTier.GetFilterName().ToString(), this._filteredTroopCountInfoText.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000ECD8 File Offset: 0x0000CED8
		private List<TooltipProperty> GetBannerBearerTooltip()
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			if (this.BannerBearerCount > 0)
			{
				list.Add(new TooltipProperty(new TextObject("{=scnSXrYC}Banner Bearers", null).ToString(), this.BannerBearerCount.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
			}
			return list;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000ED21 File Offset: 0x0000CF21
		private FormationClass GetActualTroopType(Agent agent)
		{
			if (QueryLibrary.IsInfantry(agent))
			{
				return FormationClass.Infantry;
			}
			if (QueryLibrary.IsRanged(agent))
			{
				return FormationClass.Ranged;
			}
			if (QueryLibrary.IsCavalry(agent))
			{
				return FormationClass.Cavalry;
			}
			if (QueryLibrary.IsRangedCavalry(agent))
			{
				return FormationClass.HorseArcher;
			}
			return FormationClass.NumberOfAllFormations;
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000ED4D File Offset: 0x0000CF4D
		public void UnassignCaptain()
		{
			if (this.Captain != this._unassignedCaptain)
			{
				this.Captain.CurrentAssignedFormationItem = null;
				this.Captain = this._unassignedCaptain;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000ED75 File Offset: 0x0000CF75
		private void ExecuteSelection()
		{
			Action<OrderOfBattleFormationItemVM> onSelection = OrderOfBattleFormationItemVM.OnSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000ED88 File Offset: 0x0000CF88
		private void HandleCaptainAssignment(OrderOfBattleHeroItemVM newCaptain)
		{
			this.HasCaptain = newCaptain != this._unassignedCaptain;
			if (this.HasCaptain)
			{
				Agent agent = newCaptain.Agent;
				agent.Formation = this.Formation;
				this.Formation.Captain = agent;
				newCaptain.CurrentAssignedFormationItem = this;
				BannerBearerLogic bannerBearerLogic = this._bannerBearerLogic;
				if (bannerBearerLogic != null)
				{
					bannerBearerLogic.SetFormationBanner(this.Formation, newCaptain.BannerOfHero);
				}
				agent.TryRemoveAllDetachmentScores();
			}
			else if (this.Formation != null)
			{
				this.Formation.Captain = null;
				BannerBearerLogic bannerBearerLogic2 = this._bannerBearerLogic;
				if (bannerBearerLogic2 != null)
				{
					bannerBearerLogic2.SetFormationBanner(this.Formation, null);
				}
			}
			this.RefreshFormation();
			this.OnSizeChanged();
			newCaptain.RefreshInformation();
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000EE39 File Offset: 0x0000D039
		public void ExecuteAcceptCaptain()
		{
			Action<OrderOfBattleFormationItemVM> onAcceptCaptain = OrderOfBattleFormationItemVM.OnAcceptCaptain;
			if (onAcceptCaptain == null)
			{
				return;
			}
			onAcceptCaptain(this);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000EE4B File Offset: 0x0000D04B
		public void ExecuteAcceptHeroTroops()
		{
			Action<OrderOfBattleFormationItemVM> onAcceptHeroTroops = OrderOfBattleFormationItemVM.OnAcceptHeroTroops;
			if (onAcceptHeroTroops == null)
			{
				return;
			}
			onAcceptHeroTroops(this);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000EE60 File Offset: 0x0000D060
		public void OnHeroSelectionUpdated(int selectedHeroCount, bool hasOwnHeroTroopInSelection)
		{
			if (this.IsControlledByPlayer)
			{
				this.IsAcceptingCaptain = selectedHeroCount == 1 && this.HasFormation;
				if (!hasOwnHeroTroopInSelection)
				{
					this.IsAcceptingHeroTroops = selectedHeroCount >= 1 && this.HasFormation;
					return;
				}
			}
			else
			{
				this.IsAcceptingCaptain = selectedHeroCount == 1 && this.HasFormation && (this.Captain == this._unassignedCaptain || !this.Captain.IsAssignedBeforePlayer);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000EED3 File Offset: 0x0000D0D3
		public void AddHeroTroop(OrderOfBattleHeroItemVM heroItem)
		{
			if (!this.HeroTroops.Contains(heroItem))
			{
				heroItem.CurrentAssignedFormationItem = this;
				heroItem.Agent.Formation = this.Formation;
				this.HeroTroops.Add(heroItem);
				this.RefreshFormation();
				this.OnSizeChanged();
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000EF14 File Offset: 0x0000D114
		public void RemoveHeroTroop(OrderOfBattleHeroItemVM heroItem)
		{
			if (this.HeroTroops.Contains(heroItem))
			{
				heroItem.CurrentAssignedFormationItem = null;
				heroItem.Agent.Formation = heroItem.InitialFormation;
				this.HeroTroops.Remove(heroItem);
				this.RefreshFormation();
				this.OnSizeChanged();
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000EF60 File Offset: 0x0000D160
		private void RefreshFormation()
		{
			this.HasHeroTroops = this.HeroTroops.Count > 0;
			this.IsHeroTroopsOverflowing = this.HeroTroops.Count > 8;
			this.OverflowHeroTroopCountText = (this.HeroTroops.Count - 8 + 1).ToString("+#;-#;0");
			this.Formation.Refresh();
			Action onHeroesChanged = OrderOfBattleFormationItemVM.OnHeroesChanged;
			if (onHeroesChanged == null)
			{
				return;
			}
			onHeroesChanged();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
		private void OnIsControlledByPlayerChanged()
		{
			foreach (OrderOfBattleFormationFilterSelectorItemVM orderOfBattleFormationFilterSelectorItemVM in this.FilterItems)
			{
				orderOfBattleFormationFilterSelectorItemVM.IsEnabled = this.IsControlledByPlayer;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x0000F024 File Offset: 0x0000D224
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x0000F02C File Offset: 0x0000D22C
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x0000F04A File Offset: 0x0000D24A
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x0000F052 File Offset: 0x0000D252
		[DataSourceProperty]
		public bool HasFormation
		{
			get
			{
				return this._hasFormation;
			}
			set
			{
				if (value != this._hasFormation)
				{
					this._hasFormation = value;
					base.OnPropertyChangedWithValue(value, "HasFormation");
				}
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0000F070 File Offset: 0x0000D270
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0000F078 File Offset: 0x0000D278
		[DataSourceProperty]
		public bool HasCaptain
		{
			get
			{
				return this._hasCaptain;
			}
			set
			{
				if (value != this._hasCaptain)
				{
					this._hasCaptain = value;
					base.OnPropertyChangedWithValue(value, "HasCaptain");
				}
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0000F096 File Offset: 0x0000D296
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0000F09E File Offset: 0x0000D29E
		[DataSourceProperty]
		public bool HasHeroTroops
		{
			get
			{
				return this._hasHeroTroops;
			}
			set
			{
				if (value != this._hasHeroTroops)
				{
					this._hasHeroTroops = value;
					base.OnPropertyChangedWithValue(value, "HasHeroTroops");
				}
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000F0BC File Offset: 0x0000D2BC
		// (set) Token: 0x060003DD RID: 989 RVA: 0x0000F0C4 File Offset: 0x0000D2C4
		[DataSourceProperty]
		public bool IsControlledByPlayer
		{
			get
			{
				return this._isControlledByPlayer;
			}
			set
			{
				if (value != this._isControlledByPlayer)
				{
					this._isControlledByPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsControlledByPlayer");
					this.OnIsControlledByPlayerChanged();
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000F0E8 File Offset: 0x0000D2E8
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		[DataSourceProperty]
		public bool IsSelectable
		{
			get
			{
				return this._isSelectable;
			}
			set
			{
				if (value != this._isSelectable)
				{
					this._isSelectable = value;
					base.OnPropertyChangedWithValue(value, "IsSelectable");
				}
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0000F10E File Offset: 0x0000D30E
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x0000F116 File Offset: 0x0000D316
		[DataSourceProperty]
		public bool IsAdjustable
		{
			get
			{
				return this._isAdjustable;
			}
			set
			{
				if (value != this._isAdjustable)
				{
					this._isAdjustable = value;
					base.OnPropertyChangedWithValue(value, "IsAdjustable");
				}
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x0000F134 File Offset: 0x0000D334
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x0000F13C File Offset: 0x0000D33C
		[DataSourceProperty]
		public bool IsMarkerShown
		{
			get
			{
				return this._isMarkerShown;
			}
			set
			{
				if (value != this._isMarkerShown)
				{
					this._isMarkerShown = value;
					base.OnPropertyChangedWithValue(value, "IsMarkerShown");
				}
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0000F15A File Offset: 0x0000D35A
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x0000F162 File Offset: 0x0000D362
		[DataSourceProperty]
		public bool IsBeingFocused
		{
			get
			{
				return this._isBeingFocused;
			}
			set
			{
				if (value != this._isBeingFocused)
				{
					this._isBeingFocused = value;
					base.OnPropertyChangedWithValue(value, "IsBeingFocused");
				}
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0000F180 File Offset: 0x0000D380
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x0000F188 File Offset: 0x0000D388
		[DataSourceProperty]
		public bool IsAcceptingCaptain
		{
			get
			{
				return this._isAcceptingCaptain;
			}
			set
			{
				if (value != this._isAcceptingCaptain)
				{
					this._isAcceptingCaptain = value;
					base.OnPropertyChangedWithValue(value, "IsAcceptingCaptain");
				}
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0000F1A6 File Offset: 0x0000D3A6
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0000F1AE File Offset: 0x0000D3AE
		[DataSourceProperty]
		public bool IsAcceptingHeroTroops
		{
			get
			{
				return this._isAcceptingHeroTroops;
			}
			set
			{
				if (value != this._isAcceptingHeroTroops)
				{
					this._isAcceptingHeroTroops = value;
					base.OnPropertyChangedWithValue(value, "IsAcceptingHeroTroops");
				}
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x0000F1CC File Offset: 0x0000D3CC
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x0000F1D4 File Offset: 0x0000D3D4
		[DataSourceProperty]
		public bool IsHeroTroopsOverflowing
		{
			get
			{
				return this._isHeroTroopsOverflowing;
			}
			set
			{
				if (value != this._isHeroTroopsOverflowing)
				{
					this._isHeroTroopsOverflowing = value;
					base.OnPropertyChangedWithValue(value, "IsHeroTroopsOverflowing");
				}
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0000F1F2 File Offset: 0x0000D3F2
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x0000F1FA File Offset: 0x0000D3FA
		[DataSourceProperty]
		public bool IsClassSelectionActive
		{
			get
			{
				return this._isClassSelectionActive;
			}
			set
			{
				if (value != this._isClassSelectionActive)
				{
					this._isClassSelectionActive = value;
					base.OnPropertyChangedWithValue(value, "IsClassSelectionActive");
					Action<OrderOfBattleFormationItemVM> onClassSelectionToggled = OrderOfBattleFormationItemVM.OnClassSelectionToggled;
					if (onClassSelectionToggled == null)
					{
						return;
					}
					onClassSelectionToggled(this);
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000F228 File Offset: 0x0000D428
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x0000F230 File Offset: 0x0000D430
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060003F0 RID: 1008 RVA: 0x0000F253 File Offset: 0x0000D453
		// (set) Token: 0x060003F1 RID: 1009 RVA: 0x0000F25B File Offset: 0x0000D45B
		[DataSourceProperty]
		public string FormationIsEmptyText
		{
			get
			{
				return this._formationIsEmptyText;
			}
			set
			{
				if (value != this._formationIsEmptyText)
				{
					this._formationIsEmptyText = value;
					base.OnPropertyChangedWithValue<string>(value, "FormationIsEmptyText");
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0000F27E File Offset: 0x0000D47E
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x0000F286 File Offset: 0x0000D486
		[DataSourceProperty]
		public string OverflowHeroTroopCountText
		{
			get
			{
				return this._overflowHeroTroopCountText;
			}
			set
			{
				if (value != this._overflowHeroTroopCountText)
				{
					this._overflowHeroTroopCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "OverflowHeroTroopCountText");
				}
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x0000F2B1 File Offset: 0x0000D4B1
		[DataSourceProperty]
		public int TroopCount
		{
			get
			{
				return this._troopCount;
			}
			set
			{
				if (value != this._troopCount)
				{
					this._troopCount = value;
					base.OnPropertyChangedWithValue(value, "TroopCount");
				}
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0000F2CF File Offset: 0x0000D4CF
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0000F2D7 File Offset: 0x0000D4D7
		[DataSourceProperty]
		public int BannerBearerCount
		{
			get
			{
				return this._bannerBearerCount;
			}
			set
			{
				if (value != this._bannerBearerCount)
				{
					this._bannerBearerCount = value;
					base.OnPropertyChangedWithValue(value, "BannerBearerCount");
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000F2F5 File Offset: 0x0000D4F5
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x0000F2FD File Offset: 0x0000D4FD
		[DataSourceProperty]
		public int OrderOfBattleFormationClassInt
		{
			get
			{
				return this._orderOfBattleFormationClassInt;
			}
			set
			{
				if (value != this._orderOfBattleFormationClassInt)
				{
					this._orderOfBattleFormationClassInt = value;
					base.OnPropertyChangedWithValue(value, "OrderOfBattleFormationClassInt");
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0000F31B File Offset: 0x0000D51B
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0000F323 File Offset: 0x0000D523
		[DataSourceProperty]
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (value != this._wSign)
				{
					this._wSign = value;
					base.OnPropertyChangedWithValue(value, "WSign");
				}
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0000F341 File Offset: 0x0000D541
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0000F349 File Offset: 0x0000D549
		[DataSourceProperty]
		public Vec2 ScreenPosition
		{
			get
			{
				return this._screenPosition;
			}
			set
			{
				if (value.x != this._screenPosition.x || value.y != this._screenPosition.y)
				{
					this._screenPosition = value;
					base.OnPropertyChangedWithValue(value, "ScreenPosition");
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0000F384 File Offset: 0x0000D584
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0000F38C File Offset: 0x0000D58C
		[DataSourceProperty]
		public OrderOfBattleHeroItemVM Captain
		{
			get
			{
				return this._captain;
			}
			set
			{
				if (value != this._captain)
				{
					this._captain = value;
					base.OnPropertyChangedWithValue<OrderOfBattleHeroItemVM>(value, "Captain");
					this.HandleCaptainAssignment(value);
				}
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x0000F3B1 File Offset: 0x0000D5B1
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x0000F3B9 File Offset: 0x0000D5B9
		[DataSourceProperty]
		public MBBindingList<OrderOfBattleHeroItemVM> HeroTroops
		{
			get
			{
				return this._heroTroops;
			}
			set
			{
				if (value != this._heroTroops)
				{
					this._heroTroops = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderOfBattleHeroItemVM>>(value, "HeroTroops");
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0000F3D7 File Offset: 0x0000D5D7
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x0000F3DF File Offset: 0x0000D5DF
		[DataSourceProperty]
		public MBBindingList<OrderOfBattleFormationClassVM> Classes
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
					base.OnPropertyChangedWithValue<MBBindingList<OrderOfBattleFormationClassVM>>(value, "Classes");
				}
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x0000F3FD File Offset: 0x0000D5FD
		// (set) Token: 0x06000405 RID: 1029 RVA: 0x0000F405 File Offset: 0x0000D605
		[DataSourceProperty]
		public SelectorVM<OrderOfBattleFormationClassSelectorItemVM> FormationClassSelector
		{
			get
			{
				return this._formationClassSelector;
			}
			set
			{
				if (value != this._formationClassSelector)
				{
					this._formationClassSelector = value;
					base.OnPropertyChangedWithValue<SelectorVM<OrderOfBattleFormationClassSelectorItemVM>>(value, "FormationClassSelector");
				}
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x0000F423 File Offset: 0x0000D623
		// (set) Token: 0x06000407 RID: 1031 RVA: 0x0000F42B File Offset: 0x0000D62B
		[DataSourceProperty]
		public MBBindingList<OrderOfBattleFormationFilterSelectorItemVM> FilterItems
		{
			get
			{
				return this._filterItems;
			}
			set
			{
				if (value != this._filterItems)
				{
					this._filterItems = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderOfBattleFormationFilterSelectorItemVM>>(value, "FilterItems");
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0000F449 File Offset: 0x0000D649
		// (set) Token: 0x06000409 RID: 1033 RVA: 0x0000F451 File Offset: 0x0000D651
		[DataSourceProperty]
		public BasicTooltipViewModel Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Tooltip");
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0000F46F File Offset: 0x0000D66F
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x0000F477 File Offset: 0x0000D677
		[DataSourceProperty]
		public BasicTooltipViewModel BannerBearerTooltip
		{
			get
			{
				return this._bannerBearerTooltip;
			}
			set
			{
				if (value != this._bannerBearerTooltip)
				{
					this._bannerBearerTooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "BannerBearerTooltip");
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000F495 File Offset: 0x0000D695
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x0000F49D File Offset: 0x0000D69D
		[DataSourceProperty]
		public HintViewModel CantAdjustHint
		{
			get
			{
				return this._cantAdjustHint;
			}
			set
			{
				if (value != this._cantAdjustHint)
				{
					this._cantAdjustHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CantAdjustHint");
				}
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000F4BB File Offset: 0x0000D6BB
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x0000F4C3 File Offset: 0x0000D6C3
		[DataSourceProperty]
		public HintViewModel CaptainSlotHint
		{
			get
			{
				return this._captainSlotHint;
			}
			set
			{
				if (value != this._captainSlotHint)
				{
					this._captainSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CaptainSlotHint");
				}
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000F4E1 File Offset: 0x0000D6E1
		// (set) Token: 0x06000411 RID: 1041 RVA: 0x0000F4E9 File Offset: 0x0000D6E9
		[DataSourceProperty]
		public HintViewModel HeroTroopSlotHint
		{
			get
			{
				return this._heroTroopSlotHint;
			}
			set
			{
				if (value != this._heroTroopSlotHint)
				{
					this._heroTroopSlotHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "HeroTroopSlotHint");
				}
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0000F507 File Offset: 0x0000D707
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x0000F50F File Offset: 0x0000D70F
		[DataSourceProperty]
		public HintViewModel AssignCaptainHint
		{
			get
			{
				return this._assignCaptainHint;
			}
			set
			{
				if (value != this._assignCaptainHint)
				{
					this._assignCaptainHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AssignCaptainHint");
				}
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0000F52D File Offset: 0x0000D72D
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x0000F535 File Offset: 0x0000D735
		[DataSourceProperty]
		public HintViewModel AssignHeroTroopHint
		{
			get
			{
				return this._assignHeroTroopHint;
			}
			set
			{
				if (value != this._assignHeroTroopHint)
				{
					this._assignHeroTroopHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AssignHeroTroopHint");
				}
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0000F553 File Offset: 0x0000D753
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x0000F55B File Offset: 0x0000D75B
		[DataSourceProperty]
		public bool IsCaptainSlotHighlightActive
		{
			get
			{
				return this._isCaptainSlotHighlightActive;
			}
			set
			{
				if (value != this._isCaptainSlotHighlightActive)
				{
					this._isCaptainSlotHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsCaptainSlotHighlightActive");
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0000F579 File Offset: 0x0000D779
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x0000F581 File Offset: 0x0000D781
		[DataSourceProperty]
		public bool IsTypeSelectionHighlightActive
		{
			get
			{
				return this._isTypeSelectionHighlightActive;
			}
			set
			{
				if (value != this._isTypeSelectionHighlightActive)
				{
					this._isTypeSelectionHighlightActive = value;
					base.OnPropertyChangedWithValue(value, "IsTypeSelectionHighlightActive");
				}
			}
		}

		// Token: 0x040001A5 RID: 421
		private const int MaxShownHeroTroopCount = 8;

		// Token: 0x040001A7 RID: 423
		private readonly Camera _missionCamera;

		// Token: 0x040001A8 RID: 424
		private BannerBearerLogic _bannerBearerLogic;

		// Token: 0x040001A9 RID: 425
		public static Action OnHeroesChanged;

		// Token: 0x040001AA RID: 426
		public static Action<OrderOfBattleFormationItemVM> OnClassSelectionToggled;

		// Token: 0x040001AB RID: 427
		public static Action<OrderOfBattleFormationItemVM> OnFilterUseToggled;

		// Token: 0x040001AC RID: 428
		public static Action<OrderOfBattleFormationItemVM> OnSelection;

		// Token: 0x040001AD RID: 429
		public static Action<OrderOfBattleFormationItemVM> OnDeselection;

		// Token: 0x040001AE RID: 430
		public static Func<DeploymentFormationClass, FormationFilterType, int> GetTotalTroopCountWithFilter;

		// Token: 0x040001AF RID: 431
		public static Func<Func<OrderOfBattleFormationItemVM, bool>, IEnumerable<OrderOfBattleFormationItemVM>> GetFormationWithCondition;

		// Token: 0x040001B0 RID: 432
		public static Func<FormationClass, bool> HasAnyTroopWithClass;

		// Token: 0x040001B1 RID: 433
		public static Action<OrderOfBattleFormationItemVM> OnAcceptCaptain;

		// Token: 0x040001B2 RID: 434
		public static Action<OrderOfBattleFormationItemVM> OnAcceptHeroTroops;

		// Token: 0x040001B3 RID: 435
		public static Action OnFormationClassChanged;

		// Token: 0x040001B4 RID: 436
		private OrderOfBattleHeroItemVM _unassignedCaptain;

		// Token: 0x040001B5 RID: 437
		private readonly TextObject _formationTooltipTitleText = new TextObject("{=cZNA5Z6l}Formation {NUMBER}", null);

		// Token: 0x040001B6 RID: 438
		private readonly TextObject _filteredTroopCountInfoText = new TextObject("{=yRIPADWl}{TROOP_COUNT}/{TOTAL_TROOP_COUNT}", null);

		// Token: 0x040001B7 RID: 439
		private readonly TextObject _cantAdjustNotCommanderText = new TextObject("{=ZixS1b4u}You're not leading this battle.", null);

		// Token: 0x040001B8 RID: 440
		private readonly TextObject _cantAdjustSingledOutText = new TextObject("{=7jhe9cT9}You need to have at least one more formation of this type to change this formation's type.", null);

		// Token: 0x040001B9 RID: 441
		private readonly TextObject _captainSlotHintText = new TextObject("{=shipcaptain}Captain", null);

		// Token: 0x040001BA RID: 442
		private readonly TextObject _heroTroopSlotHintText = new TextObject("{=VyMD4iRV}Hero Troops", null);

		// Token: 0x040001BB RID: 443
		private readonly TextObject _assignCaptainHintText = new TextObject("{=rHEi6aVz}Assign as Captain", null);

		// Token: 0x040001BC RID: 444
		private readonly TextObject _assignHeroTroopHintText = new TextObject("{=ngyMTaqr}Assign as Hero Troop", null);

		// Token: 0x040001BD RID: 445
		private Vec3 _worldPosition;

		// Token: 0x040001BE RID: 446
		private float _latestX;

		// Token: 0x040001BF RID: 447
		private float _latestY;

		// Token: 0x040001C0 RID: 448
		private float _latestW;

		// Token: 0x040001C1 RID: 449
		private float _wPosAfterPositionCalculation;

		// Token: 0x040001C2 RID: 450
		private bool _isMarkerWorldPositionDirty;

		// Token: 0x040001C3 RID: 451
		private bool _isSelected;

		// Token: 0x040001C4 RID: 452
		private bool _hasFormation;

		// Token: 0x040001C5 RID: 453
		private bool _hasCaptain;

		// Token: 0x040001C6 RID: 454
		private bool _isControlledByPlayer;

		// Token: 0x040001C7 RID: 455
		private bool _hasHeroTroops;

		// Token: 0x040001C8 RID: 456
		private bool _isSelectable;

		// Token: 0x040001C9 RID: 457
		private bool _isAdjustable;

		// Token: 0x040001CA RID: 458
		private bool _isMarkerShown;

		// Token: 0x040001CB RID: 459
		private bool _isBeingFocused;

		// Token: 0x040001CC RID: 460
		private bool _isAcceptingCaptain;

		// Token: 0x040001CD RID: 461
		private bool _isAcceptingHeroTroops;

		// Token: 0x040001CE RID: 462
		private bool _isHeroTroopsOverflowing;

		// Token: 0x040001CF RID: 463
		private bool _isClassSelectionActive;

		// Token: 0x040001D0 RID: 464
		private string _titleText;

		// Token: 0x040001D1 RID: 465
		private string _formationIsEmptyText;

		// Token: 0x040001D2 RID: 466
		private string _overflowHeroTroopCountText;

		// Token: 0x040001D3 RID: 467
		private int _orderOfBattleFormationClassInt;

		// Token: 0x040001D4 RID: 468
		private int _troopCount;

		// Token: 0x040001D5 RID: 469
		private int _bannerBearerCount;

		// Token: 0x040001D6 RID: 470
		private int _wSign;

		// Token: 0x040001D7 RID: 471
		private Vec2 _screenPosition;

		// Token: 0x040001D8 RID: 472
		private OrderOfBattleHeroItemVM _captain;

		// Token: 0x040001D9 RID: 473
		private MBBindingList<OrderOfBattleHeroItemVM> _heroTroops;

		// Token: 0x040001DA RID: 474
		private MBBindingList<OrderOfBattleFormationClassVM> _classes;

		// Token: 0x040001DB RID: 475
		private SelectorVM<OrderOfBattleFormationClassSelectorItemVM> _formationClassSelector;

		// Token: 0x040001DC RID: 476
		private MBBindingList<OrderOfBattleFormationFilterSelectorItemVM> _filterItems;

		// Token: 0x040001DD RID: 477
		private BasicTooltipViewModel _tooltip;

		// Token: 0x040001DE RID: 478
		private BasicTooltipViewModel _bannerBearerTooltip;

		// Token: 0x040001DF RID: 479
		private HintViewModel _cantAdjustHint;

		// Token: 0x040001E0 RID: 480
		private HintViewModel _captainSlotHint;

		// Token: 0x040001E1 RID: 481
		private HintViewModel _heroTroopSlotHint;

		// Token: 0x040001E2 RID: 482
		private HintViewModel _assignCaptainHint;

		// Token: 0x040001E3 RID: 483
		private HintViewModel _assignHeroTroopHint;

		// Token: 0x040001E4 RID: 484
		private bool _isCaptainSlotHighlightActive;

		// Token: 0x040001E5 RID: 485
		private bool _isTypeSelectionHighlightActive;
	}
}
