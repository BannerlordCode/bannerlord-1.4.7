using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order
{
	// Token: 0x0200001C RID: 28
	public class MissionOrderVM : ViewModel
	{
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00009E2A File Offset: 0x0000802A
		public MissionOrderVM.CursorStates CursorState
		{
			get
			{
				OrderSetVM selectedOrderSet = this.SelectedOrderSet;
				if (((selectedOrderSet != null) ? selectedOrderSet.OrderIconId : null) == "order_type_facing")
				{
					return MissionOrderVM.CursorStates.Face;
				}
				return MissionOrderVM.CursorStates.Move;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00009E4D File Offset: 0x0000804D
		public Team Team
		{
			get
			{
				return Mission.Current.PlayerTeam;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00009E59 File Offset: 0x00008059
		public OrderController OrderController
		{
			get
			{
				return this.Mission.PlayerTeam.PlayerOrderController;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600026E RID: 622 RVA: 0x00009E6B File Offset: 0x0000806B
		// (set) Token: 0x0600026F RID: 623 RVA: 0x00009E73 File Offset: 0x00008073
		public bool IsTroopPlacingActive
		{
			get
			{
				return this._isTroopPlacingActive;
			}
			set
			{
				this._isTroopPlacingActive = value;
				this._callbacks.SetSuspendTroopPlacer(!value);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000270 RID: 624 RVA: 0x00009E90 File Offset: 0x00008090
		public bool PlayerHasAnyTroopUnderThem
		{
			get
			{
				return this.Team.FormationsIncludingEmpty.Any<Formation>((Formation f) => f.PlayerOwner == Agent.Main && f.CountOfUnits > 0);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00009EC1 File Offset: 0x000080C1
		private Mission Mission
		{
			get
			{
				return Mission.Current;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000272 RID: 626 RVA: 0x00009EC8 File Offset: 0x000080C8
		// (set) Token: 0x06000273 RID: 627 RVA: 0x00009ED0 File Offset: 0x000080D0
		public OrderSetVM SelectedOrderSet { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00009ED9 File Offset: 0x000080D9
		// (set) Token: 0x06000275 RID: 629 RVA: 0x00009EE1 File Offset: 0x000080E1
		public bool DisplayedOrderMessageForLastOrder { get; private set; }

		// Token: 0x06000276 RID: 630 RVA: 0x00009EEC File Offset: 0x000080EC
		public MissionOrderVM(OrderController orderController, bool isDeployment, bool isMultiplayer)
		{
			this._isMultiplayer = isMultiplayer;
			this.IsDeployment = isDeployment;
			this._orderKeys = new Dictionary<int, InputKeyItemVM>();
			this.OrderSets = new MBBindingList<OrderSetVM>();
			this.DeploymentController = new MissionOrderDeploymentControllerVM(this);
			this.TroopController = this.CreateTroopController(this.OrderController);
			this.Team.OnFormationAIActiveBehaviorChanged += this.TeamOnFormationAIActiveBehaviorChanged;
			this.RefreshValues();
			this.Mission.OnMainAgentChanged += this.MissionOnMainAgentChanged;
			this.UpdateCanUseShortcuts(this._isMultiplayer);
			this._slowMotionSoundEventGlobalIndex = SoundManager.GetEventGlobalIndex("event:/ui/mission/slow_motion");
			this.RegisterEvents();
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00009FA2 File Offset: 0x000081A2
		protected virtual MissionOrderTroopControllerVM CreateTroopController(OrderController orderController)
		{
			return new MissionOrderTroopControllerVM(this, this.IsDeployment, new Action(this.OnTransferFinished));
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00009FBC File Offset: 0x000081BC
		public void SetDeploymentParemeters(Camera deploymentCamera, List<DeploymentPoint> deploymentPoints)
		{
			this.DeploymentController.SetMissionParameters(deploymentCamera, deploymentPoints);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00009FCB File Offset: 0x000081CB
		public void SetCallbacks(MissionOrderCallbacks callbacks)
		{
			this._callbacks = callbacks;
			this.DeploymentController.SetCallbacks(callbacks);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00009FE0 File Offset: 0x000081E0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ReturnText = new TextObject("{=EmVbbIUc}Return", null).ToString();
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
			{
				o.RefreshValues();
			});
			this.DeploymentController.RefreshValues();
			this.TroopController.RefreshValues();
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A04C File Offset: 0x0000824C
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Mission.OnMainAgentChanged -= this.MissionOnMainAgentChanged;
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
			{
				o.OnFinalize();
			});
			this.DeploymentController.OnFinalize();
			this.TroopController.OnFinalize();
			for (int i = 0; i < this._orderKeys.Count; i++)
			{
				this._orderKeys[i].OnFinalize();
			}
			foreach (OrderSetVM orderSetVM in this._orderSets)
			{
				orderSetVM.OnFinalize();
			}
			if (this._slowMotionSoundEvent != null)
			{
				this._slowMotionSoundEvent.Release();
				this._slowMotionSoundEvent = null;
			}
			this.InputRestrictions = null;
			this.UnregisterEvents();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A144 File Offset: 0x00008344
		private void RegisterEvents()
		{
			OrderTroopItemVM.OnSelectionChange += this.OnTroopItemSelectionStateChanged;
			OrderSetVM.OnSelectionStateChanged += this.OnOrderSetSelectionStateChanged;
			OrderItemVM.OnExecuteOrder += this.OnOrderExecuted;
			TransferTroopsVisualOrder.OnTransferStarted += this.OnTransferStarted;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A1B8 File Offset: 0x000083B8
		private void UnregisterEvents()
		{
			OrderTroopItemVM.OnSelectionChange -= this.OnTroopItemSelectionStateChanged;
			OrderSetVM.OnSelectionStateChanged -= this.OnOrderSetSelectionStateChanged;
			OrderItemVM.OnExecuteOrder -= this.OnOrderExecuted;
			TransferTroopsVisualOrder.OnTransferStarted -= this.OnTransferStarted;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.OnGamepadActiveChanged));
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A229 File Offset: 0x00008429
		private void OnGamepadActiveChanged()
		{
			if (this.IsToggleOrderShown)
			{
				this.TryCloseToggleOrder(false);
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A23C File Offset: 0x0000843C
		private void OnOrderSetSelectionStateChanged(OrderSetVM orderSet, bool isSelected)
		{
			if (this.SelectedOrderSet == orderSet)
			{
				OrderSetVM selectedOrderSet = this.SelectedOrderSet;
				if (selectedOrderSet != null && selectedOrderSet.IsSelected == isSelected)
				{
					return;
				}
			}
			if (this.SelectedOrderSet != null)
			{
				this.SelectedOrderSet.IsSelected = false;
				this.SelectedOrderSet = null;
			}
			if (orderSet != null && isSelected)
			{
				this.SelectedOrderSet = orderSet;
				this.SelectedOrderSet.IsSelected = true;
			}
			this.IsAnyOrderSetActive = this.SelectedOrderSet != null;
			this.UpdateOrderShortcuts();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A2B8 File Offset: 0x000084B8
		public void OnOrderExecuted(OrderItemVM orderItem)
		{
			if (this.IsToggleOrderShown)
			{
				this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
				{
					o.OnOrderExecuted(orderItem);
				});
			}
			List<TextObject> list = new List<TextObject>();
			if (this.ActiveTargetState == 1 && orderItem.Order.StringId != "order_toggle_facing")
			{
				for (int i = 0; i < this.DeploymentController.SiegeMachineList.Count; i++)
				{
					OrderSiegeMachineVM orderSiegeMachineVM = this.DeploymentController.SiegeMachineList[i];
					if (orderSiegeMachineVM.IsSelected)
					{
						list.Add(GameTexts.FindText("str_siege_engine", orderSiegeMachineVM.MachineClass));
					}
				}
			}
			else if (!(orderItem.Order is ReturnVisualOrder))
			{
				foreach (OrderTroopItemVM orderTroopItemVM in this.TroopController.TroopList.Where<OrderTroopItemVM>((OrderTroopItemVM item) => item.IsSelected))
				{
					list.Add(orderTroopItemVM.GetVisibleNameOfFormationForMessage());
				}
			}
			if (!list.IsEmpty<TextObject>() && !this.DisplayedOrderMessageForLastOrder)
			{
				orderItem.RefreshState();
				TextObject textObject = new TextObject("{=ApD0xQXT}{STR1}: {STR2}", null);
				textObject.SetTextVariable("STR1", GameTexts.GameTextHelper.MergeTextObjectsWithComma(list, false));
				textObject.SetTextVariable("STR2", orderItem.Name);
				InformationManager.DisplayMessage(new InformationMessage(textObject.ToString()));
				this.DisplayedOrderMessageForLastOrder = true;
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000A458 File Offset: 0x00008658
		private void PopulateOrderSets()
		{
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM o)
			{
				o.OnFinalize();
			});
			this.OrderSets.Clear();
			MBReadOnlyList<VisualOrderSet> orders = VisualOrderFactory.GetOrders();
			this.HasAnyCascadingOrders = false;
			for (int i = 0; i < orders.Count; i++)
			{
				OrderSetVM orderSetVM = new OrderSetVM(this.OrderController, orders[i]);
				this.OrderSets.Add(orderSetVM);
				if (!orderSetVM.HasSingleOrder)
				{
					this.HasAnyCascadingOrders = true;
				}
			}
			this.UpdateOrderShortcuts();
			if (this._isMultiplayer)
			{
				this.UpdateCanUseShortcuts(true);
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000A4FC File Offset: 0x000086FC
		private void UpdateOrderShortcuts()
		{
			InputKeyItemVM inputKeyItemVM = InputKeyItemVM.CreateFromHotKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"), false);
			inputKeyItemVM.SetForcedVisibility(new bool?(false));
			if (this.SelectedOrderSet != null)
			{
				for (int i = 0; i < this.OrderSets.Count; i++)
				{
					OrderSetVM orderSetVM = this.OrderSets[i];
					if (orderSetVM == this.SelectedOrderSet)
					{
						for (int j = 0; j < orderSetVM.Orders.Count; j++)
						{
							OrderItemVM orderItemVM = orderSetVM.Orders[j];
							InputKeyItemVM inputKeyItemVM2;
							if (orderItemVM.Order is ReturnVisualOrder)
							{
								orderItemVM.SetShortcutKey(this._returnKey);
							}
							else if (this._orderKeys.TryGetValue(j, out inputKeyItemVM2))
							{
								orderItemVM.SetShortcutKey(inputKeyItemVM2);
							}
						}
					}
					else
					{
						for (int k = 0; k < orderSetVM.Orders.Count; k++)
						{
							orderSetVM.Orders[k].SetShortcutKey(inputKeyItemVM);
						}
					}
					orderSetVM.SetShortcutKey(inputKeyItemVM);
				}
			}
			else
			{
				for (int l = 0; l < this.OrderSets.Count; l++)
				{
					OrderSetVM orderSetVM2 = this.OrderSets[l];
					InputKeyItemVM inputKeyItemVM3;
					if (orderSetVM2.HasSingleOrder && orderSetVM2.Orders[0].Order is ReturnVisualOrder)
					{
						orderSetVM2.SetShortcutKey(this._returnKey);
					}
					else if (this._orderKeys.TryGetValue(l, out inputKeyItemVM3))
					{
						orderSetVM2.SetShortcutKey(inputKeyItemVM3);
					}
				}
			}
			inputKeyItemVM.OnFinalize();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A679 File Offset: 0x00008879
		private void TeamOnFormationAIActiveBehaviorChanged(Formation formation)
		{
			if (formation.IsAIControlled)
			{
				if (this._modifiedAIFormations.IndexOf(formation) < 0)
				{
					this._modifiedAIFormations.Add(formation);
				}
				this._delayValueForAIFormationModifications = 3;
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A6A8 File Offset: 0x000088A8
		private void DisplayFormationAIFeedback()
		{
			this._delayValueForAIFormationModifications = Math.Max(0, this._delayValueForAIFormationModifications - 1);
			if (this._delayValueForAIFormationModifications == 0 && this._modifiedAIFormations.Count > 0)
			{
				for (int i = 0; i < this._modifiedAIFormations.Count; i++)
				{
					Formation formation = this._modifiedAIFormations[i];
					if (((formation != null) ? formation.AI.ActiveBehavior : null) != null && formation.FormationIndex < FormationClass.NumberOfRegularFormations)
					{
						MissionOrderVM.DisplayFormationAIFeedbackAux(this._modifiedAIFormations);
					}
					else
					{
						this._modifiedAIFormations[i] = null;
					}
				}
				this._modifiedAIFormations.Clear();
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A744 File Offset: 0x00008944
		private static void DisplayFormationAIFeedbackAux(List<Formation> formations)
		{
			Dictionary<FormationClass, TextObject> dictionary = new Dictionary<FormationClass, TextObject>();
			Type type = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < formations.Count; i++)
			{
				Formation formation = formations[i];
				if (((formation != null) ? formation.AI.ActiveBehavior : null) != null && (type == null || type == formation.AI.ActiveBehavior.GetType()))
				{
					type = formation.AI.ActiveBehavior.GetType();
					switch (formation.AI.Side)
					{
					case FormationAI.BehaviorSide.Left:
						flag = true;
						break;
					case FormationAI.BehaviorSide.Middle:
						flag3 = true;
						break;
					case FormationAI.BehaviorSide.Right:
						flag2 = true;
						break;
					}
					if (!dictionary.ContainsKey(formation.PhysicalClass))
					{
						TextObject localizedName = formation.PhysicalClass.GetLocalizedName();
						TextObject textObject = GameTexts.FindText("str_troop_group_name_definite", null);
						textObject.SetTextVariable("FORMATION_CLASS", localizedName);
						dictionary.Add(formation.PhysicalClass, textObject);
					}
					formations[i] = null;
				}
			}
			if (dictionary.Count == 1)
			{
				MBTextManager.SetTextVariable("IS_PLURAL", 0);
				MBTextManager.SetTextVariable("TROOP_NAMES_BEGIN", TextObject.GetEmpty(), false);
				MBTextManager.SetTextVariable("TROOP_NAMES_END", dictionary.First<KeyValuePair<FormationClass, TextObject>>().Value, false);
			}
			else
			{
				MBTextManager.SetTextVariable("IS_PLURAL", 1);
				TextObject value = dictionary.Last<KeyValuePair<FormationClass, TextObject>>().Value;
				TextObject textObject2;
				if (dictionary.Count == 2)
				{
					textObject2 = dictionary.First<KeyValuePair<FormationClass, TextObject>>().Value;
				}
				else
				{
					textObject2 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
					textObject2.SetTextVariable("LEFT", dictionary.First<KeyValuePair<FormationClass, TextObject>>().Value);
					textObject2.SetTextVariable("RIGHT", dictionary.Last<KeyValuePair<FormationClass, TextObject>>().Value);
					for (int j = 2; j < dictionary.Count - 1; j++)
					{
						TextObject textObject3 = GameTexts.FindText("str_LEFT_comma_RIGHT", null);
						textObject3.SetTextVariable("LEFT", textObject2);
						textObject3.SetTextVariable("RIGHT", dictionary.Values.ElementAt<TextObject>(j));
						textObject2 = textObject3;
					}
				}
				MBTextManager.SetTextVariable("TROOP_NAMES_BEGIN", textObject2, false);
				MBTextManager.SetTextVariable("TROOP_NAMES_END", value, false);
			}
			bool flag4 = (flag ? 1 : 0) + (flag3 ? 1 : 0) + (flag2 ? 1 : 0) > 1;
			MBTextManager.SetTextVariable("IS_LEFT", flag4 ? 2 : (flag ? 1 : 0));
			MBTextManager.SetTextVariable("IS_MIDDLE", (!flag4 && flag3) ? 1 : 0);
			MBTextManager.SetTextVariable("IS_RIGHT", (!flag4 && flag2) ? 1 : 0);
			string name = type.Name;
			InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_formation_ai_behavior_text", name).ToString()));
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000AA04 File Offset: 0x00008C04
		private void OnTroopItemSelectionStateChanged(OrderTroopItemVM troopItem, bool isSelected)
		{
			for (int i = 0; i < this.TroopController.TroopList.Count; i++)
			{
				this.TroopController.TroopList[i].IsTargetRelevant = this.TroopController.TroopList[i].IsSelected;
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000AA58 File Offset: 0x00008C58
		public virtual void OnOrderLayoutTypeChanged()
		{
			this.TroopController = this.CreateTroopController(this.OrderController);
			this.OrderSets.Clear();
			this.TroopController.UpdateTroops();
			this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM x)
			{
				this.TroopController.SetTroopActiveOrders(x);
			});
			this.TroopController.OnFiltersSet(this._filterData);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000AABC File Offset: 0x00008CBC
		public void OpenToggleOrder(bool fromHold, bool displayMessage = true)
		{
			if (this.IsToggleOrderShown)
			{
				return;
			}
			if (this.OrderController.SelectedFormations.Count == 0)
			{
				this.OrderController.SelectAllFormations(false);
			}
			this.PopulateOrderSets();
			if (this.CheckCanBeOpened(displayMessage))
			{
				Mission.Current.IsOrderMenuOpen = true;
				this.IsToggleOrderShown = true;
				this.TroopController.UpdateTroops();
				this.TroopController.IsTransferActive = false;
				this.DeploymentController.ProcessSiegeMachines();
				if (this.OrderController.SelectedFormations.IsEmpty<Formation>())
				{
					this.TroopController.SelectAllFormations(true);
				}
				if (Input.IsGamepadActive)
				{
					if (this.TroopController.TroopList.All<OrderTroopItemVM>((OrderTroopItemVM t) => !t.IsSelectionHighlightActive))
					{
						OrderTroopItemVM orderTroopItemVM = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>();
						if (orderTroopItemVM != null)
						{
							orderTroopItemVM.IsSelectionHighlightActive = true;
						}
					}
				}
				this.SetActiveOrders();
				this.OnOrderShownToggle();
				this.DisplayedOrderMessageForLastOrder = false;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000ABBC File Offset: 0x00008DBC
		private bool CheckCanBeOpened(bool displayMessage = false)
		{
			if (Agent.Main == null)
			{
				if (displayMessage)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=GMhOZGnb}Cannot issue order while dead.", null).ToString()));
				}
				return false;
			}
			if (Mission.Current.Mode != MissionMode.Deployment && !Agent.Main.IsPlayerControlled)
			{
				if (displayMessage)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=b1DHZsaH}Cannot issue order right now.", null).ToString()));
				}
				return false;
			}
			if (!this.Team.HasBots || !this.PlayerHasAnyTroopUnderThem || (!this.Team.IsPlayerGeneral && !this.Team.IsPlayerSergeant))
			{
				if (displayMessage)
				{
					InformationManager.DisplayMessage(new InformationMessage(new TextObject("{=DQvGNQ0g}There isn't any unit under command.", null).ToString()));
				}
				return false;
			}
			return !Mission.Current.IsMissionEnding || Mission.Current.CheckIfBattleInRetreat();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000AC90 File Offset: 0x00008E90
		public bool TryCloseToggleOrder(bool applySelectedOrders = false)
		{
			if (this.IsToggleOrderShown)
			{
				Mission.Current.IsOrderMenuOpen = false;
				if (applySelectedOrders && this.SelectedOrderSet != null)
				{
					OrderItemVM orderItemVM = this.SelectedOrderSet.Orders.FirstOrDefault<OrderItemVM>((OrderItemVM o) => o.IsSelected);
					if (orderItemVM != null && this._callbacks.GetVisualOrderExecutionParameters != null)
					{
						VisualOrderExecutionParameters visualOrderExecutionParameters = this._callbacks.GetVisualOrderExecutionParameters();
						orderItemVM.ExecuteAction(visualOrderExecutionParameters);
					}
				}
				OrderSetVM selectedOrderSet = this.SelectedOrderSet;
				if (selectedOrderSet != null)
				{
					selectedOrderSet.ExecuteDeSelect();
				}
				this.IsToggleOrderShown = false;
				this.OnOrderShownToggle();
				if (!this.IsDeployment)
				{
					this.InputRestrictions.ResetInputRestrictions();
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000AD4C File Offset: 0x00008F4C
		public void SetActiveOrders()
		{
			if (this.ActiveTargetState == 1)
			{
				this.DeploymentController.SetCurrentActiveOrders();
			}
			else
			{
				this.TroopController.SetCurrentActiveOrders();
			}
			this.OrderSets.ApplyActionOnAllItems(delegate(OrderSetVM os)
			{
				os.RefreshOrderStates();
			});
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000ADA6 File Offset: 0x00008FA6
		public void SetFocusedFormations(MBReadOnlyList<Formation> focusedFormationsCache)
		{
			this._focusedFormationsCache = focusedFormationsCache;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000ADAF File Offset: 0x00008FAF
		public void AfterInitialize()
		{
			this.TroopController.UpdateTroops();
			if (!this.IsDeployment)
			{
				this.TroopController.SelectAllFormations(false);
			}
			this.DeploymentController.SetCurrentActiveOrders();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000ADDC File Offset: 0x00008FDC
		public void Update()
		{
			if (this.IsToggleOrderShown)
			{
				if (!this.CheckCanBeOpened(false))
				{
					this.TryCloseToggleOrder(false);
				}
				else if (this._updateTroopsTimer.Check(MBCommon.GetApplicationTime()))
				{
					this.TroopController.IntervalUpdate();
				}
				this.TroopController.Update();
				this.TroopController.RefreshTroopFormationTargetVisuals();
				this.UseAlternativeFormationLayout = Input.IsGamepadActive;
			}
			if (this.IsToggleOrderShown)
			{
				if (BannerlordConfig.SlowDownOnOrder && !this._isDeployment && !this._isMultiplayer && this._slowMotionSoundEvent == null)
				{
					this._slowMotionSoundEvent = SoundEvent.CreateEvent(this._slowMotionSoundEventGlobalIndex, Mission.Current.Scene);
					this._slowMotionSoundEvent.Play();
				}
			}
			else if (this._slowMotionSoundEvent != null)
			{
				this._slowMotionSoundEvent.Release();
				this._slowMotionSoundEvent = null;
			}
			this.DeploymentController.Update();
			this.DisplayFormationAIFeedback();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000AEBD File Offset: 0x000090BD
		public void OnEscape()
		{
			if (this.IsToggleOrderShown)
			{
				if (this.SelectedOrderSet != null)
				{
					this.SelectedOrderSet.ExecuteDeSelect();
					return;
				}
				this.TryCloseToggleOrder(false);
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000AEE3 File Offset: 0x000090E3
		public void ViewOrders()
		{
			if (!this.IsToggleOrderShown)
			{
				this.TroopController.UpdateTroops();
				this.OpenToggleOrder(false, true);
				return;
			}
			this.TryCloseToggleOrder(false);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000AF09 File Offset: 0x00009109
		public OrderSetVM GetOrderSetAtIndex(int orderSetIndex)
		{
			if (orderSetIndex < 0 || orderSetIndex >= this.OrderSets.Count)
			{
				return null;
			}
			return this.OrderSets[orderSetIndex];
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000AF2C File Offset: 0x0000912C
		public bool TrySelectOrderSet(OrderSetVM orderSet)
		{
			if (!this.CheckCanBeOpened(true))
			{
				return false;
			}
			VisualOrderExecutionParameters visualOrderExecutionParameters = this._callbacks.GetVisualOrderExecutionParameters();
			orderSet.ExecuteAction(visualOrderExecutionParameters);
			if (!this.IsToggleOrderShown && !orderSet.OrderSet.IsSoloOrder)
			{
				this.OpenToggleOrder(false, true);
			}
			else if (this.IsToggleOrderShown && orderSet.OrderSet.IsSoloOrder && !this.IsDeployment)
			{
				this.TryCloseToggleOrder(false);
			}
			return true;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000AFA4 File Offset: 0x000091A4
		public void OnTroopFormationSelected(int formationTroopIndex)
		{
			if (!this.CheckCanBeOpened(true))
			{
				return;
			}
			if (this.ActiveTargetState == 0)
			{
				this.TroopController.OnSelectFormationWithIndex(formationTroopIndex);
			}
			else if (this.ActiveTargetState == 1)
			{
				this.DeploymentController.OnSelectFormationWithIndex(formationTroopIndex);
			}
			this.TryCloseToggleOrder(false);
			this.OpenToggleOrder(false, true);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000AFF6 File Offset: 0x000091F6
		private void MissionOnMainAgentChanged(Agent oldAgent)
		{
			if (this.Mission.MainAgent == null)
			{
				this.TryCloseToggleOrder(false);
				this.Mission.IsOrderMenuOpen = false;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B01C File Offset: 0x0000921C
		internal void OnDeployAll()
		{
			this.TroopController.UpdateTroops();
			foreach (OrderTroopItemVM orderTroopItemVM in this.TroopController.TroopList)
			{
				this.TroopController.SetTroopActiveOrders(orderTroopItemVM);
			}
			if (!this.IsDeployment)
			{
				this.TroopController.SelectAllFormations(false);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000B098 File Offset: 0x00009298
		private void OnOrderShownToggle()
		{
			this.IsTroopListShown = this.IsToggleOrderShown && !this.IsDeployment;
			if (!this._isDeployment)
			{
				if (this.IsToggleOrderShown)
				{
					this._callbacks.OnActivateToggleOrder();
				}
				else
				{
					this._callbacks.OnDeactivateToggleOrder();
				}
			}
			this._updateTroopsTimer = (this.IsToggleOrderShown ? new Timer(MBCommon.GetApplicationTime() - 2f, 2f, true) : null);
			this.IsTroopPlacingActive = this.IsToggleOrderShown && this.ActiveTargetState == 0;
			if (!this.IsDeployment && this.TroopController.TroopList.Count > 0 && Input.IsGamepadActive && this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.FormationIndex == this._lastHighlightedFormationIndex) == null)
			{
				this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM t)
				{
					t.IsSelectionHighlightActive = false;
				});
				this.TroopController.TroopList[0].IsSelectionHighlightActive = true;
			}
			this._callbacks.RefreshVisuals();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000B1C8 File Offset: 0x000093C8
		public void OnTroopHighlightSelection(bool isDirectionLeft)
		{
			if (!this.CheckCanBeOpened(true))
			{
				return;
			}
			if (this.TroopController.TroopList.Count > 0)
			{
				OrderTroopItemVM highlightedFormation = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectionHighlightActive);
				if (highlightedFormation != null)
				{
					OrderTroopItemVM targetFormation = (isDirectionLeft ? this.TroopController.TroopList.LastOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.FormationIndex < highlightedFormation.FormationIndex) : this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.FormationIndex > highlightedFormation.FormationIndex));
					if (targetFormation == null)
					{
						targetFormation = (isDirectionLeft ? this.TroopController.TroopList.LastOrDefault<OrderTroopItemVM>() : this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>());
					}
					if (targetFormation != null)
					{
						this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM t)
						{
							t.IsSelectionHighlightActive = t == targetFormation;
						});
						this._lastHighlightedFormationIndex = targetFormation.FormationIndex;
						return;
					}
				}
				else
				{
					this.TroopController.TroopList[0].IsSelectionHighlightActive = true;
				}
			}
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000B304 File Offset: 0x00009504
		public void ExecuteSelectHighlightedFormation()
		{
			OrderTroopItemVM orderTroopItemVM = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectable && t.IsSelectionHighlightActive);
			if (orderTroopItemVM == null)
			{
				return;
			}
			if (orderTroopItemVM.IsSelected)
			{
				if (this.TroopController.TroopList.Count<OrderTroopItemVM>((OrderTroopItemVM x) => x.IsSelected) == 1)
				{
					this.TroopController.SelectAllFormations(true);
					return;
				}
			}
			this.OnTroopFormationSelected(orderTroopItemVM.FormationIndex);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000B398 File Offset: 0x00009598
		public void ExecuteToggleHighlightedFormation()
		{
			OrderTroopItemVM orderTroopItemVM = this.TroopController.TroopList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectable && t.IsSelectionHighlightActive);
			if (orderTroopItemVM == null)
			{
				return;
			}
			if (orderTroopItemVM.IsSelected)
			{
				if (this.TroopController.TroopList.Count<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected) == 1)
				{
					this.TroopController.SelectAllFormations(true);
				}
				this.TroopController.OnDeselectFormation(orderTroopItemVM.FormationIndex);
			}
			else
			{
				this.TroopController.AddSelectedFormation(orderTroopItemVM);
			}
			this.TryCloseToggleOrder(false);
			this.OpenToggleOrder(false, true);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000B450 File Offset: 0x00009650
		private void OnTransferStarted()
		{
			if (this.IsDeployment)
			{
				return;
			}
			foreach (OrderTroopItemVM orderTroopItemVM in this.TroopController.TransferTargetList)
			{
				orderTroopItemVM.IsSelected = false;
				orderTroopItemVM.IsSelectable = !this.OrderController.IsFormationListening(orderTroopItemVM.Formation);
			}
			OrderTroopItemVM orderTroopItemVM2 = this.TroopController.TransferTargetList.FirstOrDefault<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelectable);
			if (orderTroopItemVM2 != null)
			{
				this.TroopController.IsTransferActive = true;
				this.TroopController.ExecuteSelectTransferTroop(orderTroopItemVM2);
				this.TroopController.TransferMaxValue = this.TroopController.TroopList.Where<OrderTroopItemVM>((OrderTroopItemVM t) => t.IsSelected).Sum<OrderTroopItemVM>((OrderTroopItemVM t) => t.CurrentMemberCount);
				this.TroopController.TransferValue = this.TroopController.TransferMaxValue;
				this.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				return;
			}
			MBInformationManager.AddQuickInformation(new TextObject("{=SLY8z9fP}All formations are selected!", null), 0, null, null, "");
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000B5B0 File Offset: 0x000097B0
		protected void OnTransferFinished()
		{
			this._callbacks.OnTransferTroopsFinished();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000B5C4 File Offset: 0x000097C4
		[Conditional("DEBUG")]
		private void DebugTick()
		{
			if (this.IsToggleOrderShown)
			{
				string text = "SelectedFormations (" + this.OrderController.SelectedFormations.Count + ") :";
				foreach (Formation formation in this.OrderController.SelectedFormations)
				{
					text = text + " " + formation.FormationIndex.GetName();
				}
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000B65C File Offset: 0x0000985C
		public void OnDeploymentFinished()
		{
			this.TroopController.OnDeploymentFinished();
			this.DeploymentController.FinalizeDeployment();
			this.IsDeployment = false;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B67B File Offset: 0x0000987B
		public void OnAfterDeploymentFinished()
		{
			this.TroopController.OnAfterDeploymentFinished();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000B688 File Offset: 0x00009888
		public void OnFiltersSet(List<MissionOrderVM.FormationConfiguration> filterData)
		{
			this._filterData = filterData;
			this.TroopController.OnFiltersSet(filterData);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B6A0 File Offset: 0x000098A0
		public void UpdateCanUseShortcuts(bool value)
		{
			this.CanUseShortcuts = value;
			for (int i = 0; i < this.OrderSets.Count; i++)
			{
				this.OrderSets[i].UpdateCanUseShortcuts(value);
			}
			if (!value)
			{
				this.TroopController.TroopList.ForEach(delegate(OrderTroopItemVM t)
				{
					t.ShowSelectionInputs = false;
				});
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B710 File Offset: 0x00009910
		public void SetOrderIndexKey(int orderIndex, GameKey gameKey)
		{
			InputKeyItemVM inputKeyItemVM;
			if (this._orderKeys.TryGetValue(orderIndex, out inputKeyItemVM) && inputKeyItemVM != null)
			{
				inputKeyItemVM.OnFinalize();
			}
			InputKeyItemVM inputKeyItemVM2 = InputKeyItemVM.CreateFromGameKey(gameKey, false);
			this._orderKeys[orderIndex] = inputKeyItemVM2;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000B74B File Offset: 0x0000994B
		public void SetReturnKey(GameKey gameKey)
		{
			InputKeyItemVM returnKey = this._returnKey;
			if (returnKey != null)
			{
				returnKey.OnFinalize();
			}
			this._returnKey = InputKeyItemVM.CreateFromGameKey(gameKey, false);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B76B File Offset: 0x0000996B
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000B77A File Offset: 0x0000997A
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x0000B782 File Offset: 0x00009982
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000B7A0 File Offset: 0x000099A0
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000B7A8 File Offset: 0x000099A8
		[DataSourceProperty]
		public MBBindingList<OrderSetVM> OrderSets
		{
			get
			{
				return this._orderSets;
			}
			set
			{
				if (value == this._orderSets)
				{
					return;
				}
				this._orderSets = value;
				base.OnPropertyChangedWithValue<MBBindingList<OrderSetVM>>(value, "OrderSets");
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000B7C7 File Offset: 0x000099C7
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x0000B7CF File Offset: 0x000099CF
		[DataSourceProperty]
		public MissionOrderTroopControllerVM TroopController
		{
			get
			{
				return this._troopController;
			}
			set
			{
				if (value == this._troopController)
				{
					return;
				}
				this._troopController = value;
				base.OnPropertyChangedWithValue<MissionOrderTroopControllerVM>(value, "TroopController");
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000B7EE File Offset: 0x000099EE
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000B7F6 File Offset: 0x000099F6
		[DataSourceProperty]
		public MissionOrderDeploymentControllerVM DeploymentController
		{
			get
			{
				return this._deploymentController;
			}
			set
			{
				if (value == this._deploymentController)
				{
					return;
				}
				this._deploymentController = value;
				base.OnPropertyChangedWithValue<MissionOrderDeploymentControllerVM>(value, "DeploymentController");
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000B815 File Offset: 0x00009A15
		// (set) Token: 0x060002AD RID: 685 RVA: 0x0000B81D File Offset: 0x00009A1D
		[DataSourceProperty]
		public int ActiveTargetState
		{
			get
			{
				return this._activeTargetState;
			}
			set
			{
				if (value == this._activeTargetState)
				{
					return;
				}
				this._activeTargetState = value;
				base.OnPropertyChangedWithValue(value, "ActiveTargetState");
				this.IsTroopPlacingActive = value == 0;
				this._callbacks.RefreshVisuals();
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060002AE RID: 686 RVA: 0x0000B856 File Offset: 0x00009A56
		// (set) Token: 0x060002AF RID: 687 RVA: 0x0000B85E File Offset: 0x00009A5E
		[DataSourceProperty]
		public bool IsDeployment
		{
			get
			{
				return this._isDeployment;
			}
			set
			{
				this._isDeployment = value;
				base.OnPropertyChangedWithValue(value, "IsDeployment");
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000B873 File Offset: 0x00009A73
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000B87B File Offset: 0x00009A7B
		[DataSourceProperty]
		public bool HasAnyCascadingOrders
		{
			get
			{
				return this._hasAnyCascadingOrders;
			}
			set
			{
				if (value != this._hasAnyCascadingOrders)
				{
					this._hasAnyCascadingOrders = value;
					base.OnPropertyChangedWithValue(value, "HasAnyCascadingOrders");
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000B899 File Offset: 0x00009A99
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000B8A1 File Offset: 0x00009AA1
		[DataSourceProperty]
		public bool IsToggleOrderShown
		{
			get
			{
				return this._isToggleOrderShown;
			}
			set
			{
				if (value == this._isToggleOrderShown)
				{
					return;
				}
				this._isToggleOrderShown = value;
				base.OnPropertyChangedWithValue(value, "IsToggleOrderShown");
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000B8C0 File Offset: 0x00009AC0
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		[DataSourceProperty]
		public bool IsTroopListShown
		{
			get
			{
				return this._isTroopListShown;
			}
			set
			{
				if (value == this._isTroopListShown)
				{
					return;
				}
				this._isTroopListShown = value;
				base.OnPropertyChangedWithValue(value, "IsTroopListShown");
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000B8E7 File Offset: 0x00009AE7
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x0000B8EF File Offset: 0x00009AEF
		[DataSourceProperty]
		public bool CanUseShortcuts
		{
			get
			{
				return this._canUseShortcuts;
			}
			set
			{
				if (value != this._canUseShortcuts)
				{
					this._canUseShortcuts = value;
					base.OnPropertyChangedWithValue(value, "CanUseShortcuts");
				}
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000B90D File Offset: 0x00009B0D
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x0000B915 File Offset: 0x00009B15
		[DataSourceProperty]
		public bool IsHolding
		{
			get
			{
				return this._isHolding;
			}
			set
			{
				if (value != this._isHolding)
				{
					this._isHolding = value;
					base.OnPropertyChangedWithValue(value, "IsHolding");
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000B933 File Offset: 0x00009B33
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000B93B File Offset: 0x00009B3B
		[DataSourceProperty]
		public bool IsAnyOrderSetActive
		{
			get
			{
				return this._isAnyOrderSetActive;
			}
			set
			{
				if (value != this._isAnyOrderSetActive)
				{
					this._isAnyOrderSetActive = value;
					base.OnPropertyChangedWithValue(value, "IsAnyOrderSetActive");
				}
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000B959 File Offset: 0x00009B59
		// (set) Token: 0x060002BD RID: 701 RVA: 0x0000B961 File Offset: 0x00009B61
		[DataSourceProperty]
		public string ReturnText
		{
			get
			{
				return this._returnText;
			}
			set
			{
				if (value != this._returnText)
				{
					this._returnText = value;
					base.OnPropertyChangedWithValue<string>(value, "ReturnText");
				}
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000B984 File Offset: 0x00009B84
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000B98C File Offset: 0x00009B8C
		[DataSourceProperty]
		public bool UseAlternativeFormationLayout
		{
			get
			{
				return this._useAlternativeFormationLayout;
			}
			set
			{
				if (value != this._useAlternativeFormationLayout)
				{
					this._useAlternativeFormationLayout = value;
					base.OnPropertyChangedWithValue(value, "UseAlternativeFormationLayout");
				}
			}
		}

		// Token: 0x0400011C RID: 284
		public InputRestrictions InputRestrictions;

		// Token: 0x0400011D RID: 285
		private Timer _updateTroopsTimer;

		// Token: 0x0400011E RID: 286
		private MissionOrderCallbacks _callbacks;

		// Token: 0x0400011F RID: 287
		private bool _isTroopPlacingActive;

		// Token: 0x04000120 RID: 288
		private bool _isMultiplayer;

		// Token: 0x04000121 RID: 289
		private MBReadOnlyList<Formation> _focusedFormationsCache;

		// Token: 0x04000122 RID: 290
		private int _delayValueForAIFormationModifications;

		// Token: 0x04000123 RID: 291
		private readonly List<Formation> _modifiedAIFormations = new List<Formation>();

		// Token: 0x04000124 RID: 292
		private SoundEvent _slowMotionSoundEvent;

		// Token: 0x04000125 RID: 293
		private int _slowMotionSoundEventGlobalIndex;

		// Token: 0x04000126 RID: 294
		private List<MissionOrderVM.FormationConfiguration> _filterData;

		// Token: 0x04000127 RID: 295
		private Dictionary<int, InputKeyItemVM> _orderKeys;

		// Token: 0x04000128 RID: 296
		private InputKeyItemVM _returnKey;

		// Token: 0x04000129 RID: 297
		private int _lastHighlightedFormationIndex;

		// Token: 0x0400012C RID: 300
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x0400012D RID: 301
		private MBBindingList<OrderSetVM> _orderSets;

		// Token: 0x0400012E RID: 302
		private MissionOrderTroopControllerVM _troopController;

		// Token: 0x0400012F RID: 303
		private MissionOrderDeploymentControllerVM _deploymentController;

		// Token: 0x04000130 RID: 304
		private bool _isDeployment;

		// Token: 0x04000131 RID: 305
		private int _activeTargetState;

		// Token: 0x04000132 RID: 306
		private bool _hasAnyCascadingOrders;

		// Token: 0x04000133 RID: 307
		private bool _isToggleOrderShown;

		// Token: 0x04000134 RID: 308
		private bool _isTroopListShown;

		// Token: 0x04000135 RID: 309
		private bool _canUseShortcuts;

		// Token: 0x04000136 RID: 310
		private bool _isHolding;

		// Token: 0x04000137 RID: 311
		private bool _isAnyOrderSetActive;

		// Token: 0x04000138 RID: 312
		private string _returnText;

		// Token: 0x04000139 RID: 313
		private bool _useAlternativeFormationLayout;

		// Token: 0x020000C0 RID: 192
		public enum CursorStates
		{
			// Token: 0x040005A1 RID: 1441
			Move,
			// Token: 0x040005A2 RID: 1442
			Face,
			// Token: 0x040005A3 RID: 1443
			Form
		}

		// Token: 0x020000C1 RID: 193
		public enum OrderTargets
		{
			// Token: 0x040005A5 RID: 1445
			Troops,
			// Token: 0x040005A6 RID: 1446
			SiegeMachines
		}

		// Token: 0x020000C2 RID: 194
		public struct ClassConfiguration
		{
			// Token: 0x06000C2B RID: 3115 RVA: 0x00029669 File Offset: 0x00027869
			public ClassConfiguration(int formationIndex, DeploymentFormationClass formationClass)
			{
				this.FormationIndex = formationIndex;
				this.FormationClass = formationClass;
			}

			// Token: 0x040005A7 RID: 1447
			public int FormationIndex;

			// Token: 0x040005A8 RID: 1448
			public DeploymentFormationClass FormationClass;
		}

		// Token: 0x020000C3 RID: 195
		public struct FormationConfiguration
		{
			// Token: 0x06000C2C RID: 3116 RVA: 0x00029679 File Offset: 0x00027879
			public FormationConfiguration(int formationIndex, List<FormationFilterType> filters)
			{
				this.FormationIndex = formationIndex;
				this.Filters = filters;
			}

			// Token: 0x040005A9 RID: 1449
			public int FormationIndex;

			// Token: 0x040005AA RID: 1450
			public List<FormationFilterType> Filters;
		}
	}
}
