using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001A RID: 26
	public class MissionOrderTroopControllerVM : ViewModel
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000083A2 File Offset: 0x000065A2
		// (set) Token: 0x06000218 RID: 536 RVA: 0x000083AA File Offset: 0x000065AA
		public MBList<OrderTroopItemVM> TroopList { get; private set; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000083B3 File Offset: 0x000065B3
		private Agent MainAgent
		{
			get
			{
				return Mission.Current.MainAgent;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000083BF File Offset: 0x000065BF
		protected Team Team
		{
			get
			{
				return Mission.Current.PlayerTeam;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600021B RID: 539 RVA: 0x000083CB File Offset: 0x000065CB
		protected OrderController OrderController
		{
			get
			{
				return Mission.Current.PlayerTeam.PlayerOrderController;
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000083DC File Offset: 0x000065DC
		public MissionOrderTroopControllerVM(MissionOrderVM missionOrder, bool isDeployment, Action onTransferFinised)
		{
			this.MissionOrder = missionOrder;
			this.OnTransferFinished = onTransferFinised;
			this._emptyTroopItemVM = new OrderTroopItemVM();
			this.IsDeployment = isDeployment;
			this.TroopList = new MBList<OrderTroopItemVM>();
			this.TransferTargetList = new MBBindingList<OrderTroopItemVM>();
			for (int i = 0; i < 8; i++)
			{
				Formation formation = this.Team.GetFormation((FormationClass)i);
				OrderTroopItemVM orderTroopItemVM = this.CreateTroopItemVM(formation, new Action<OrderTroopItemVM>(this.ExecuteSelectTransferTroop), new Func<Formation, int>(this.GetFormationMorale));
				this.TransferTargetList.Add(orderTroopItemVM);
				orderTroopItemVM.IsSelected = false;
			}
			this.FormationIndexComparer = new MissionOrderTroopControllerVM.TroopItemFormationIndexComparer();
			this.SortFormations();
			this.OrderController.OnOrderIssued += new OnOrderIssuedDelegate(this.OrderController_OnTroopOrderIssued);
			this.OrderController.OnSelectedFormationsChanged += this.OrderController_OnSelectedFormationsChanged;
			this.RefreshValues();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000084B8 File Offset: 0x000066B8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TroopList.ForEach(delegate(OrderTroopItemVM x)
			{
				x.RefreshValues();
			});
			this.AcceptText = GameTexts.FindText("str_selection_widget_accept", null).ToString();
			this.CancelText = GameTexts.FindText("str_selection_widget_cancel", null).ToString();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00008524 File Offset: 0x00006724
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.OrderController.OnOrderIssued -= new OnOrderIssuedDelegate(this.OrderController_OnTroopOrderIssued);
			this.OrderController.OnSelectedFormationsChanged -= this.OrderController_OnSelectedFormationsChanged;
			this._emptyTroopItemVM.OnFinalize();
			this.TroopList.ForEach(delegate(OrderTroopItemVM x)
			{
				x.OnFinalize();
			});
			this.TroopList.Clear();
			this.TransferTargetList.Clear();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000085B0 File Offset: 0x000067B0
		public void ExecuteSelectAll()
		{
			this.SelectAllFormations(true);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000085BC File Offset: 0x000067BC
		public void ExecuteSelectTransferTroop(OrderTroopItemVM targetTroop)
		{
			foreach (OrderTroopItemVM orderTroopItemVM in this.TransferTargetList)
			{
				orderTroopItemVM.IsSelected = false;
			}
			targetTroop.IsSelected = targetTroop.IsSelectable;
			this.IsTransferValid = targetTroop.IsSelectable;
			GameTexts.SetVariable("FORMATION_INDEX", targetTroop.FormationName);
			this.TransferTitleText = new TextObject("{=DvnRkWQg}Transfer Troops To {FORMATION_INDEX}", null).ToString();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00008648 File Offset: 0x00006848
		public void ExecuteConfirmTransfer()
		{
			this.IsTransferActive = false;
			OrderTroopItemVM orderTroopItemVM = this.TransferTargetList.Single<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected);
			int num = this.TransferValue;
			int num2 = this.TroopList.Where<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected).Sum<OrderTroopItemVM>((OrderTroopItemVM t) => t.CurrentMemberCount);
			num = MathF.Min(num, num2);
			this.OrderController.SetOrderWithFormationAndNumber(OrderType.Transfer, orderTroopItemVM.Formation, num);
			for (int i = 0; i < this.TroopList.Count; i++)
			{
				OrderTroopItemVM orderTroopItemVM2 = this.TroopList[i];
				if (!orderTroopItemVM2.ContainsDeadTroop && orderTroopItemVM2.CurrentMemberCount == 0)
				{
					this.TroopList.RemoveAt(i);
					i--;
				}
			}
			Action onTransferFinished = this.OnTransferFinished;
			if (onTransferFinished != null)
			{
				onTransferFinished.DynamicInvokeWithLog(Array.Empty<object>());
			}
			this.UpdateTroops();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000875C File Offset: 0x0000695C
		public void ExecuteCancelTransfer()
		{
			this.IsTransferActive = false;
			Action onTransferFinished = this.OnTransferFinished;
			if (onTransferFinished == null)
			{
				return;
			}
			onTransferFinished.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000877B File Offset: 0x0000697B
		public void ExecuteReset()
		{
			this.RefreshValues();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00008784 File Offset: 0x00006984
		public void SetTroopActiveOrders(OrderTroopItemVM item)
		{
			item.ClearActiveOrders();
			foreach (OrderSetVM orderSetVM in this.MissionOrder.OrderSets)
			{
				foreach (OrderItemVM orderItemVM in orderSetVM.Orders)
				{
					if (orderItemVM.Order.GetFormationHasOrder(item.Formation))
					{
						item.AddActiveOrder(orderItemVM);
					}
				}
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008824 File Offset: 0x00006A24
		public virtual void SelectAllFormations(bool uiFeedback = true)
		{
			foreach (OrderSetVM orderSetVM in this.MissionOrder.OrderSets)
			{
				orderSetVM.ExecuteDeSelect();
			}
			if (this.TroopList.Any<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectable))
			{
				this.OrderController.ClearSelectedFormations();
				this.OrderController.SelectAllFormations(uiFeedback);
				if (uiFeedback && this.OrderController.SelectedFormations.Count > 0)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=xTv4tCbZ}Everybody!! Listen to me", null).ToString()));
				}
			}
			this.MissionOrder.SetActiveOrders();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000088F4 File Offset: 0x00006AF4
		public virtual void AddSelectedFormation(OrderTroopItemVM item)
		{
			if (!item.IsSelectable)
			{
				return;
			}
			Formation formation = this.Team.GetFormation(item.InitialFormationClass);
			this.OrderController.SelectFormation(formation);
			this.MissionOrder.SetActiveOrders();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008933 File Offset: 0x00006B33
		public void SetSelectedFormation(OrderTroopItemVM item)
		{
			this.UpdateTroops();
			if (!item.IsSelectable)
			{
				return;
			}
			this.OrderController.ClearSelectedFormations();
			this.AddSelectedFormation(item);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00008958 File Offset: 0x00006B58
		public void OnDeselectFormation(int index)
		{
			OrderTroopItemVM orderTroopItemVM = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.Formation.Index == index);
			this.OnDeselectFormation(orderTroopItemVM);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008994 File Offset: 0x00006B94
		public void OnDeselectFormation(OrderTroopItemVM item)
		{
			if (item != null)
			{
				Formation formation = this.Team.GetFormation(item.InitialFormationClass);
				if (this.OrderController.SelectedFormations.Contains(formation))
				{
					this.OrderController.DeselectFormation(formation);
				}
				if (this.IsDeployment)
				{
					if (this.TroopList.Count<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected) != 0)
					{
						this.MissionOrder.SetActiveOrders();
						return;
					}
					this.MissionOrder.TryCloseToggleOrder(false);
					this.MissionOrder.IsTroopPlacingActive = false;
					return;
				}
				else
				{
					this.MissionOrder.SetActiveOrders();
				}
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008A40 File Offset: 0x00006C40
		public void OnSelectFormation(OrderTroopItemVM item)
		{
			foreach (OrderSetVM orderSetVM in this.MissionOrder.OrderSets)
			{
				orderSetVM.ExecuteDeSelect();
			}
			this.UpdateTroops();
			this.MissionOrder.IsTroopPlacingActive = true;
			if (Input.IsKeyDown(InputKey.LeftControl))
			{
				if (item.IsSelected)
				{
					this.OnDeselectFormation(item);
				}
				else
				{
					this.AddSelectedFormation(item);
				}
			}
			else
			{
				this.SetSelectedFormation(item);
			}
			if (this.IsTransferActive)
			{
				foreach (OrderTroopItemVM orderTroopItemVM in this.TransferTargetList)
				{
					orderTroopItemVM.IsSelectable = !this.OrderController.IsFormationListening(orderTroopItemVM.Formation);
				}
				this.IsTransferValid = this.TransferTargetList.Any<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected && t.IsSelectable);
				this.TransferMaxValue = this.TroopList.Where<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected).Sum<OrderTroopItemVM>((OrderTroopItemVM t) => t.CurrentMemberCount);
				this.TransferValue = this.TransferMaxValue;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008BB8 File Offset: 0x00006DB8
		private void UpdateFormationSelectedStates()
		{
			for (int i = 0; i < this.TroopList.Count; i++)
			{
				OrderTroopItemVM orderTroopItemVM = this.TroopList[i];
				orderTroopItemVM.IsSelectable = this.OrderController.IsFormationSelectable(orderTroopItemVM.Formation);
				if (orderTroopItemVM.IsSelectable && this.OrderController.IsFormationListening(orderTroopItemVM.Formation))
				{
					orderTroopItemVM.IsSelected = true;
				}
				else if (!orderTroopItemVM.IsSelectable && orderTroopItemVM.IsSelected)
				{
					this.OnDeselectFormation(orderTroopItemVM);
				}
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008C3A File Offset: 0x00006E3A
		protected virtual OrderTroopItemVM CreateTroopItemVM(Formation formation, Action<OrderTroopItemVM> onSelectFormation, Func<Formation, int> getFormationMorale)
		{
			return new OrderTroopItemVM(formation, onSelectFormation, getFormationMorale);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008C44 File Offset: 0x00006E44
		public void UpdateTroops()
		{
			List<Formation> list;
			if (this.MainAgent != null && this.MainAgent.Controller != AgentControllerType.Player)
			{
				list = this.Team.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0).ToList<Formation>();
			}
			else
			{
				list = this.Team.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0 && (!f.IsPlayerTroopInFormation || f.CountOfUnits > 1)).ToList<Formation>();
			}
			for (int i = 0; i < list.Count; i++)
			{
				Formation formation = list[i];
				if (formation != null && !this.TroopList.Any<OrderTroopItemVM>((OrderTroopItemVM item) => item.Formation == formation))
				{
					this.AddTroopItemIfNotExist(this.CreateTroopItemVM(formation, new Action<OrderTroopItemVM>(this.OnSelectFormation), new Func<Formation, int>(this.GetFormationMorale)), -1);
				}
			}
			for (int j = 0; j < this.TroopList.Count; j++)
			{
				this.TroopList[j].UpdateVisuals();
			}
			this.SortFormations();
			this.UpdateFormationSelectedStates();
			this.RefreshTroopItemBindings();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008D80 File Offset: 0x00006F80
		public void AddTroops(Agent agent)
		{
			if (agent.Team != this.Team || agent.Formation == null || agent.IsPlayerControlled)
			{
				return;
			}
			Formation formation = agent.Formation;
			OrderTroopItemVM orderTroopItemVM = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM item) => item.Formation.FormationIndex == formation.FormationIndex);
			if (orderTroopItemVM == null)
			{
				this.AddTroopItemIfNotExist(this.CreateTroopItemVM(formation, new Action<OrderTroopItemVM>(this.OnSelectFormation), new Func<Formation, int>(this.GetFormationMorale)), -1);
			}
			else
			{
				orderTroopItemVM.SetFormationClassFromFormation(formation);
			}
			this.SortFormations();
			this.UpdateFormationSelectedStates();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00008E20 File Offset: 0x00007020
		public void RemoveTroops(Agent agent)
		{
			if (agent.Team != this.Team || agent.Formation == null)
			{
				return;
			}
			Formation formation = agent.Formation;
			OrderTroopItemVM orderTroopItemVM = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM item) => item.Formation.FormationIndex == formation.FormationIndex);
			if (orderTroopItemVM != null)
			{
				orderTroopItemVM.OnFormationAgentRemoved(agent);
				orderTroopItemVM.SetFormationClassFromFormation(formation);
			}
			this.UpdateFormationSelectedStates();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008E8C File Offset: 0x0000708C
		public void OnTroopOrderIssued(List<OrderTroopItemVM> selectedFormations, OrderItemVM orderItem)
		{
			foreach (OrderSetVM orderSetVM in this.MissionOrder.OrderSets)
			{
				orderSetVM.RefreshOrderStates();
			}
			OrderSetVM selectedOrderSet = this.MissionOrder.SelectedOrderSet;
			if (selectedOrderSet == null)
			{
				return;
			}
			selectedOrderSet.ExecuteDeSelect();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008EF0 File Offset: 0x000070F0
		private void OrderController_OnTroopOrderIssued(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			if (orderType == OrderType.Transfer)
			{
				if (!(delegateParams[1] is object[]))
				{
					int num = (int)delegateParams[1];
				}
				Formation formation = delegateParams[0] as Formation;
				OrderTroopItemVM orderTroopItemVM = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM item) => item.Formation == formation);
				if (orderTroopItemVM == null)
				{
					int num2 = -1;
					for (int i = 0; i < this.TroopList.Count; i++)
					{
						if (this.TroopList[i].Formation.Index > formation.Index)
						{
							num2 = i;
							break;
						}
					}
					this.AddTroopItemIfNotExist(this.CreateTroopItemVM(formation, new Action<OrderTroopItemVM>(this.OnSelectFormation), new Func<Formation, int>(this.GetFormationMorale)), num2);
				}
				else
				{
					orderTroopItemVM.SetFormationClassFromFormation(formation);
				}
				using (IEnumerator<Formation> enumerator = appliedFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation sourceFormation2 = enumerator.Current;
						OrderTroopItemVM orderTroopItemVM2 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM item) => item.Formation == sourceFormation2);
						if (orderTroopItemVM2 == null)
						{
							int num3 = -1;
							for (int j = 0; j < this.TroopList.Count; j++)
							{
								if (this.TroopList[j].Formation.Index > sourceFormation2.Index)
								{
									num3 = j;
									break;
								}
							}
							this.AddTroopItemIfNotExist(this.CreateTroopItemVM(sourceFormation2, new Action<OrderTroopItemVM>(this.OnSelectFormation), new Func<Formation, int>(this.GetFormationMorale)), num3);
						}
						else
						{
							orderTroopItemVM2.SetFormationClassFromFormation(sourceFormation2);
						}
					}
				}
				int num4 = 1;
				using (IEnumerator<Formation> enumerator = appliedFormations.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation sourceFormation = enumerator.Current;
						this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM item) => item.Formation.Index == sourceFormation.Index).SetFormationClassFromFormation(sourceFormation);
						num4++;
					}
				}
			}
			this.UpdateTroops();
			foreach (OrderTroopItemVM orderTroopItemVM3 in this.TroopList.Where<OrderTroopItemVM>((OrderTroopItemVM item) => item.IsSelected))
			{
				this.SetTroopActiveOrders(orderTroopItemVM3);
			}
			this.MissionOrder.SetActiveOrders();
			this.UpdateFormationSelectedStates();
			if (orderType == OrderType.Move || orderType == OrderType.MoveToLineSegment || orderType == OrderType.MoveToLineSegmentWithHorizontalLayout)
			{
				OrderItemVM orderItemVM = this.FindOrderWithId("order_movement_move");
				if (orderItemVM != null)
				{
					this.MissionOrder.OnOrderExecuted(orderItemVM);
				}
			}
			else if (orderType == OrderType.Charge || orderType == OrderType.Advance)
			{
				OrderItemVM orderItemVM2 = null;
				if (orderType == OrderType.Charge)
				{
					orderItemVM2 = this.FindOrderWithId("order_movement_charge") ?? this.FindOrderWithId("order_movement_advance");
				}
				else if (orderType == OrderType.Advance)
				{
					orderItemVM2 = this.FindOrderWithId("order_movement_advance") ?? this.FindOrderWithId("order_movement_charge");
				}
				if (orderItemVM2 != null)
				{
					this.MissionOrder.OnOrderExecuted(orderItemVM2);
				}
			}
			if (this.MissionOrder.IsToggleOrderShown && !this.MissionOrder.IsDeployment && !this.MissionOrder.IsHolding)
			{
				this.MissionOrder.TryCloseToggleOrder(false);
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009268 File Offset: 0x00007468
		private OrderItemVM FindOrderWithId(string orderId)
		{
			for (int i = 0; i < this.MissionOrder.OrderSets.Count; i++)
			{
				OrderSetVM orderSetVM = this.MissionOrder.OrderSets[i];
				for (int j = 0; j < orderSetVM.Orders.Count; j++)
				{
					OrderItemVM orderItemVM = orderSetVM.Orders[j];
					if (orderItemVM.OrderIconId == orderId)
					{
						return orderItemVM;
					}
				}
			}
			return null;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000092D8 File Offset: 0x000074D8
		private void OrderController_OnSelectedFormationsChanged()
		{
			for (int i = 0; i < this.TroopList.Count; i++)
			{
				OrderTroopItemVM orderTroopItemVM = this.TroopList[i];
				orderTroopItemVM.IsSelected = this.OrderController.IsFormationListening(orderTroopItemVM.Formation);
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009320 File Offset: 0x00007520
		internal void Update()
		{
			for (int i = 0; i < this.TroopList.Count; i++)
			{
				this.TroopList[i].Update();
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00009354 File Offset: 0x00007554
		public void IntervalUpdate()
		{
			this.UpdateTroops();
			for (int i = this.TroopList.Count - 1; i >= 0; i--)
			{
				OrderTroopItemVM orderTroopItemVM = this.TroopList[i];
				Formation formation = orderTroopItemVM.Formation;
				if (formation != null && formation.CountOfUnits > 0)
				{
					orderTroopItemVM.UnderAttackOfType = (int)formation.GetUnderAttackTypeOfUnits(3f);
					orderTroopItemVM.BehaviorType = (int)formation.GetMovementTypeOfUnits();
					if (!this.IsDeployment)
					{
						orderTroopItemVM.Morale = (int)MissionGameModels.Current.BattleMoraleModel.GetAverageMorale(formation);
						if (orderTroopItemVM.SetFormationClassFromFormation(formation))
						{
							this.UpdateTroops();
						}
						if (formation.QuerySystem.RangedUnitRatio > 0f || formation.QuerySystem.RangedCavalryUnitRatio > 0f)
						{
							int totalCurrentAmmo = 0;
							int totalMaxAmmo = 0;
							orderTroopItemVM.Formation.ApplyActionOnEachUnit(delegate(Agent agent)
							{
								if (!agent.IsMainAgent)
								{
									int num;
									int num2;
									this.GetMaxAndCurrentAmmoOfAgent(agent, out num, out num2);
									totalCurrentAmmo += num;
									totalMaxAmmo += num2;
								}
							}, null);
							if (totalMaxAmmo > 0)
							{
								orderTroopItemVM.IsAmmoAvailable = true;
								orderTroopItemVM.AmmoPercentage = (float)totalCurrentAmmo / (float)totalMaxAmmo;
							}
							else
							{
								orderTroopItemVM.IsAmmoAvailable = false;
							}
						}
						else
						{
							orderTroopItemVM.IsAmmoAvailable = false;
						}
					}
				}
				else if (formation != null && formation.CountOfUnits == 0)
				{
					orderTroopItemVM.Morale = 0;
					orderTroopItemVM.SetFormationClassFromFormation(formation);
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000094A8 File Offset: 0x000076A8
		public void RefreshTroopFormationTargetVisuals()
		{
			for (int i = 0; i < this.TroopList.Count; i++)
			{
				this.TroopList[i].RefreshTargetedOrderVisual();
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000094DC File Offset: 0x000076DC
		public void OnSelectFormationWithIndex(int formationTroopIndex)
		{
			this.UpdateTroops();
			OrderTroopItemVM orderTroopItemVM = null;
			if (formationTroopIndex >= 0)
			{
				orderTroopItemVM = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.Formation.Index == formationTroopIndex && x.IsSelectable);
			}
			if (orderTroopItemVM != null)
			{
				this.OnSelectFormation(orderTroopItemVM);
				return;
			}
			this.SelectAllFormations(true);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00009534 File Offset: 0x00007734
		public void SetCurrentActiveOrders()
		{
			foreach (OrderTroopItemVM orderTroopItemVM in this.TroopList)
			{
				if (orderTroopItemVM.IsSelected)
				{
					this.SetTroopActiveOrders(orderTroopItemVM);
				}
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00009590 File Offset: 0x00007790
		private void GetMaxAndCurrentAmmoOfAgent(Agent agent, out int currentAmmo, out int maxAmmo)
		{
			currentAmmo = 0;
			maxAmmo = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				if (!agent.Equipment[equipmentIndex].IsEmpty && agent.Equipment[equipmentIndex].CurrentUsageItem.IsRangedWeapon)
				{
					currentAmmo = agent.Equipment.GetAmmoAmount(equipmentIndex);
					maxAmmo = agent.Equipment.GetMaxAmmo(equipmentIndex);
					return;
				}
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009600 File Offset: 0x00007800
		public void OnFiltersSet(List<MissionOrderVM.FormationConfiguration> filterData)
		{
			if (filterData == null)
			{
				return;
			}
			this.FilterData = filterData;
			using (List<MissionOrderVM.FormationConfiguration>.Enumerator enumerator = filterData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionOrderVM.FormationConfiguration filter = enumerator.Current;
					OrderTroopItemVM orderTroopItemVM = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM f) => f.Formation.Index == filter.FormationIndex);
					if (orderTroopItemVM != null)
					{
						orderTroopItemVM.UpdateFilterData(filter.Filters);
					}
					OrderTroopItemVM orderTroopItemVM2 = this.TransferTargetList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM f) => f.Formation.Index == filter.FormationIndex);
					if (orderTroopItemVM2 != null)
					{
						orderTroopItemVM2.UpdateFilterData(filter.Filters);
					}
				}
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000096B8 File Offset: 0x000078B8
		public void OnDeploymentFinished()
		{
			this.IsDeployment = false;
			for (int i = this.TroopList.Count - 1; i >= 0; i--)
			{
				if (this.TroopList[i].CurrentMemberCount <= 0)
				{
					this.TroopList.RemoveAt(i);
				}
			}
			this.SortFormations();
			this.OrderController.ClearSelectedFormations();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009715 File Offset: 0x00007915
		public void OnAfterDeploymentFinished()
		{
			this.UpdateTroops();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000971D File Offset: 0x0000791D
		private void SortFormations()
		{
			this.TroopList.Sort(new Comparison<OrderTroopItemVM>(this.FormationIndexComparer.Compare));
			this.TransferTargetList.Sort(this.FormationIndexComparer);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000974D File Offset: 0x0000794D
		private int GetFormationMorale(Formation formation)
		{
			if (!this.IsDeployment)
			{
				return (int)MissionGameModels.Current.BattleMoraleModel.GetAverageMorale(formation);
			}
			return 0;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000976C File Offset: 0x0000796C
		private OrderTroopItemVM AddTroopItemIfNotExist(OrderTroopItemVM troopItem, int index = -1)
		{
			OrderTroopItemVM orderTroopItemVM = null;
			if (troopItem != null)
			{
				bool flag = true;
				orderTroopItemVM = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.Formation.Index == troopItem.Formation.Index);
				if (orderTroopItemVM == null)
				{
					flag = false;
					orderTroopItemVM = troopItem;
				}
				if (flag)
				{
					this.TroopList.Remove(orderTroopItemVM);
				}
				if (index == -1)
				{
					this.TroopList.Add(troopItem);
				}
				else
				{
					this.TroopList.Insert(index, troopItem);
				}
			}
			else
			{
				Debug.FailedAssert("Added troop item is null!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\MissionOrderTroopControllerVM.cs", "AddTroopItemIfNotExist", 736);
			}
			this.OnAfterNewTroopItemAdded();
			this.RefreshTroopItemBindings();
			return orderTroopItemVM;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009817 File Offset: 0x00007A17
		protected virtual void OnAfterNewTroopItemAdded()
		{
			this.OnFiltersSet(this.FilterData);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009828 File Offset: 0x00007A28
		private void RefreshTroopItemBindings()
		{
			this.TroopItem0 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 0) ?? this._emptyTroopItemVM;
			this.TroopItem1 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 1) ?? this._emptyTroopItemVM;
			this.TroopItem2 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 2) ?? this._emptyTroopItemVM;
			this.TroopItem3 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 3) ?? this._emptyTroopItemVM;
			this.TroopItem4 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 4) ?? this._emptyTroopItemVM;
			this.TroopItem5 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 5) ?? this._emptyTroopItemVM;
			this.TroopItem6 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 6) ?? this._emptyTroopItemVM;
			this.TroopItem7 = this.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM x) => x.FormationIndex == 7) ?? this._emptyTroopItemVM;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000242 RID: 578 RVA: 0x00009A05 File Offset: 0x00007C05
		// (set) Token: 0x06000243 RID: 579 RVA: 0x00009A10 File Offset: 0x00007C10
		[DataSourceProperty]
		public bool IsTransferActive
		{
			get
			{
				return this._isTransferActive;
			}
			set
			{
				if (value != this._isTransferActive)
				{
					this._isTransferActive = value;
					base.OnPropertyChangedWithValue(value, "IsTransferActive");
					this.MissionOrder.IsTroopPlacingActive = !value;
					for (int i = 0; i < this.MissionOrder.OrderSets.Count; i++)
					{
						this.MissionOrder.OrderSets[i].ExecuteDeSelect();
					}
					if (this._isTransferActive)
					{
						foreach (OrderTroopItemVM orderTroopItemVM in this.TransferTargetList)
						{
							orderTroopItemVM.SetFormationClassFromFormation(orderTroopItemVM.Formation);
							orderTroopItemVM.Morale = (int)MissionGameModels.Current.BattleMoraleModel.GetAverageMorale(orderTroopItemVM.Formation);
							orderTroopItemVM.IsAmmoAvailable = orderTroopItemVM.Formation.QuerySystem.RangedUnitRatio > 0f || orderTroopItemVM.Formation.QuerySystem.RangedCavalryUnitRatio > 0f;
						}
					}
					if (Mission.Current != null)
					{
						Mission.Current.IsTransferMenuOpen = value;
					}
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00009B34 File Offset: 0x00007D34
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00009B3C File Offset: 0x00007D3C
		[DataSourceProperty]
		public bool IsTransferValid
		{
			get
			{
				return this._isTransferValid;
			}
			set
			{
				if (value != this._isTransferValid)
				{
					this._isTransferValid = value;
					base.OnPropertyChangedWithValue(value, "IsTransferValid");
					if (!value)
					{
						this.TransferTitleText = "";
					}
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00009B68 File Offset: 0x00007D68
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00009B70 File Offset: 0x00007D70
		[DataSourceProperty]
		public MBBindingList<OrderTroopItemVM> TransferTargetList
		{
			get
			{
				return this._transferTargetList;
			}
			set
			{
				if (value != this._transferTargetList)
				{
					this._transferTargetList = value;
					base.OnPropertyChangedWithValue<MBBindingList<OrderTroopItemVM>>(value, "TransferTargetList");
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00009B8E File Offset: 0x00007D8E
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00009B96 File Offset: 0x00007D96
		[DataSourceProperty]
		public int TransferMaxValue
		{
			get
			{
				return this._transferMaxValue;
			}
			set
			{
				if (value != this._transferMaxValue)
				{
					this._transferMaxValue = value;
					base.OnPropertyChangedWithValue(value, "TransferMaxValue");
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00009BB4 File Offset: 0x00007DB4
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00009BBC File Offset: 0x00007DBC
		[DataSourceProperty]
		public int TransferValue
		{
			get
			{
				return this._transferValue;
			}
			set
			{
				if (value != this._transferValue)
				{
					this._transferValue = value;
					base.OnPropertyChangedWithValue(value, "TransferValue");
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00009BDA File Offset: 0x00007DDA
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00009BE2 File Offset: 0x00007DE2
		[DataSourceProperty]
		public string TransferTitleText
		{
			get
			{
				return this._transferTitleText;
			}
			set
			{
				if (value != this._transferTitleText)
				{
					this._transferTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TransferTitleText");
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00009C05 File Offset: 0x00007E05
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00009C0D File Offset: 0x00007E0D
		[DataSourceProperty]
		public string AcceptText
		{
			get
			{
				return this._acceptText;
			}
			set
			{
				if (value != this._acceptText)
				{
					this._acceptText = value;
					base.OnPropertyChangedWithValue<string>(value, "AcceptText");
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00009C30 File Offset: 0x00007E30
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00009C38 File Offset: 0x00007E38
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00009C5B File Offset: 0x00007E5B
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00009C63 File Offset: 0x00007E63
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem0
		{
			get
			{
				return this._troopItem0;
			}
			set
			{
				if (value != this._troopItem0)
				{
					this._troopItem0 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem0");
				}
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00009C81 File Offset: 0x00007E81
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00009C89 File Offset: 0x00007E89
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem1
		{
			get
			{
				return this._troopItem1;
			}
			set
			{
				if (value != this._troopItem1)
				{
					this._troopItem1 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem1");
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00009CA7 File Offset: 0x00007EA7
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00009CAF File Offset: 0x00007EAF
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem2
		{
			get
			{
				return this._troopItem2;
			}
			set
			{
				if (value != this._troopItem2)
				{
					this._troopItem2 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem2");
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00009CCD File Offset: 0x00007ECD
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00009CD5 File Offset: 0x00007ED5
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem3
		{
			get
			{
				return this._troopItem3;
			}
			set
			{
				if (value != this._troopItem3)
				{
					this._troopItem3 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem3");
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00009CF3 File Offset: 0x00007EF3
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00009CFB File Offset: 0x00007EFB
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem4
		{
			get
			{
				return this._troopItem4;
			}
			set
			{
				if (value != this._troopItem4)
				{
					this._troopItem4 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem4");
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00009D19 File Offset: 0x00007F19
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00009D21 File Offset: 0x00007F21
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem5
		{
			get
			{
				return this._troopItem5;
			}
			set
			{
				if (value != this._troopItem5)
				{
					this._troopItem5 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem5");
				}
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00009D3F File Offset: 0x00007F3F
		// (set) Token: 0x0600025F RID: 607 RVA: 0x00009D47 File Offset: 0x00007F47
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem6
		{
			get
			{
				return this._troopItem6;
			}
			set
			{
				if (value != this._troopItem6)
				{
					this._troopItem6 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem6");
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00009D65 File Offset: 0x00007F65
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00009D6D File Offset: 0x00007F6D
		[DataSourceProperty]
		public OrderTroopItemVM TroopItem7
		{
			get
			{
				return this._troopItem7;
			}
			set
			{
				if (value != this._troopItem7)
				{
					this._troopItem7 = value;
					base.OnPropertyChangedWithValue<OrderTroopItemVM>(value, "TroopItem7");
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00009D8B File Offset: 0x00007F8B
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00009D93 File Offset: 0x00007F93
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00009DB1 File Offset: 0x00007FB1
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00009DB9 File Offset: 0x00007FB9
		[DataSourceProperty]
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

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00009DD7 File Offset: 0x00007FD7
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00009DDF File Offset: 0x00007FDF
		[DataSourceProperty]
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

		// Token: 0x06000268 RID: 616 RVA: 0x00009DFD File Offset: 0x00007FFD
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009E0C File Offset: 0x0000800C
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009E1B File Offset: 0x0000801B
		public void SetResetInputKey(HotKey hotKey)
		{
			this.ResetInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x040000FA RID: 250
		protected readonly MissionOrderVM MissionOrder;

		// Token: 0x040000FB RID: 251
		protected readonly Action OnTransferFinished;

		// Token: 0x040000FD RID: 253
		protected List<MissionOrderVM.FormationConfiguration> FilterData;

		// Token: 0x040000FE RID: 254
		protected bool IsDeployment;

		// Token: 0x040000FF RID: 255
		protected MissionOrderTroopControllerVM.TroopItemFormationIndexComparer FormationIndexComparer;

		// Token: 0x04000100 RID: 256
		private readonly OrderTroopItemVM _emptyTroopItemVM;

		// Token: 0x04000101 RID: 257
		private bool _isTransferActive;

		// Token: 0x04000102 RID: 258
		private MBBindingList<OrderTroopItemVM> _transferTargetList;

		// Token: 0x04000103 RID: 259
		private int _transferValue;

		// Token: 0x04000104 RID: 260
		private int _transferMaxValue;

		// Token: 0x04000105 RID: 261
		private string _transferTitleText;

		// Token: 0x04000106 RID: 262
		private string _acceptText;

		// Token: 0x04000107 RID: 263
		private string _cancelText;

		// Token: 0x04000108 RID: 264
		private bool _isTransferValid;

		// Token: 0x04000109 RID: 265
		private OrderTroopItemVM _troopItem0;

		// Token: 0x0400010A RID: 266
		private OrderTroopItemVM _troopItem1;

		// Token: 0x0400010B RID: 267
		private OrderTroopItemVM _troopItem2;

		// Token: 0x0400010C RID: 268
		private OrderTroopItemVM _troopItem3;

		// Token: 0x0400010D RID: 269
		private OrderTroopItemVM _troopItem4;

		// Token: 0x0400010E RID: 270
		private OrderTroopItemVM _troopItem5;

		// Token: 0x0400010F RID: 271
		private OrderTroopItemVM _troopItem6;

		// Token: 0x04000110 RID: 272
		private OrderTroopItemVM _troopItem7;

		// Token: 0x04000111 RID: 273
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000112 RID: 274
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000113 RID: 275
		private InputKeyItemVM _resetInputKey;

		// Token: 0x020000AD RID: 173
		protected class TroopItemFormationIndexComparer : IComparer<OrderTroopItemVM>
		{
			// Token: 0x06000BE3 RID: 3043 RVA: 0x000293A0 File Offset: 0x000275A0
			public int Compare(OrderTroopItemVM x, OrderTroopItemVM y)
			{
				return x.Formation.Index.CompareTo(y.Formation.Index);
			}
		}
	}
}
