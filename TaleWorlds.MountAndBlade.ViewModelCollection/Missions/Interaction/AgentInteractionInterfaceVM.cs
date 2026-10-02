using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Hints;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction
{
	// Token: 0x02000040 RID: 64
	public class AgentInteractionInterfaceVM : ViewModel
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00015809 File Offset: 0x00013A09
		private bool IsPlayerActive
		{
			get
			{
				Agent main = Agent.Main;
				return main != null && main.Health > 0f;
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00015824 File Offset: 0x00013A24
		public AgentInteractionInterfaceVM(Mission mission)
		{
			this._mission = mission;
			this.IsActive = false;
			this.PrimaryInteractionMessages = new MBBindingList<MissionPrimaryInteractionItemVM>
			{
				new MissionPrimaryInteractionItemVM(),
				new MissionPrimaryInteractionItemVM()
			};
			this.SecondaryInteractionMessages = new MBBindingList<MissionInteractionItemBaseVM>();
			this.ForcedInteractionMessages = new MBBindingList<MissionPrimaryInteractionItemVM>
			{
				new MissionPrimaryInteractionItemVM(),
				new MissionPrimaryInteractionItemVM()
			};
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00015894 File Offset: 0x00013A94
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PrimaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.RefreshValues();
			});
			this.SecondaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionInteractionItemBaseVM p)
			{
				p.RefreshValues();
			});
			this.ForcedInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.RefreshValues();
			});
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00015928 File Offset: 0x00013B28
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.PrimaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.OnFinalize();
			});
			this.SecondaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionInteractionItemBaseVM p)
			{
				p.OnFinalize();
			});
			this.ForcedInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.OnFinalize();
			});
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000159BC File Offset: 0x00013BBC
		internal void Tick(float dt)
		{
			Agent agent;
			if ((agent = this._currentFocusedObject as Agent) != null)
			{
				this.ResetFocus();
				this.OnFocusGained(Agent.Main, agent, agent.IsActive() || Agent.Main.CanInteractWithAgent(agent, -1000f));
			}
			Agent agent2;
			if (this.IsActive && this._mission.Mode == MissionMode.StartUp && (agent2 = this._currentFocusedObject as Agent) != null && agent2.IsEnemyOf(this._mission.MainAgent))
			{
				this.IsActive = false;
			}
			this.HasSecondaryMessages = this.SecondaryInteractionMessages.Count > 0;
			this.SecondaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionInteractionItemBaseVM s)
			{
				s.RefreshValues();
			});
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00015A81 File Offset: 0x00013C81
		internal void CheckAndClearFocusedAgent(Agent agent)
		{
			if (this._currentFocusedObject != null && this._currentFocusedObject as Agent == agent)
			{
				this.IsActive = false;
				this.ResetFocus();
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00015AA6 File Offset: 0x00013CA6
		public void OnFocusedHealthChanged(IFocusable focusable, float healthPercentage, bool hideHealthbarWhenFull)
		{
			this.SetHealth(healthPercentage, hideHealthbarWhenFull);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00015AB0 File Offset: 0x00013CB0
		internal void OnFocusGained(Agent mainAgent, IFocusable focusableObject, bool isInteractable)
		{
			if (this.IsPlayerActive && (this._currentFocusedObject != focusableObject || this._currentObjectInteractable != isInteractable))
			{
				this.ResetFocus();
				this._currentFocusedObject = focusableObject;
				this._currentObjectInteractable = isInteractable;
				Agent agent;
				UsableMissionObject usableMissionObject;
				if ((agent = focusableObject as Agent) != null)
				{
					if (agent.IsHuman)
					{
						this.SetHumanAgent(mainAgent, agent, isInteractable);
						return;
					}
					if (agent.IsMount)
					{
						this.SetMount(mainAgent, agent, isInteractable);
						return;
					}
					this.SetGenericAgent(mainAgent, agent, isInteractable);
					return;
				}
				else if ((usableMissionObject = focusableObject as UsableMissionObject) != null)
				{
					SpawnedItemEntity spawnedItemEntity;
					if ((spawnedItemEntity = usableMissionObject as SpawnedItemEntity) != null)
					{
						bool flag = Agent.Main.CanQuickPickUp(spawnedItemEntity);
						this.SetItem(spawnedItemEntity, flag, isInteractable);
						return;
					}
					this.SetUsableMissionObject(usableMissionObject, isInteractable);
					return;
				}
				else
				{
					UsableMachine usableMachine;
					if ((usableMachine = focusableObject as UsableMachine) != null)
					{
						this.SetUsableMachine(usableMachine, isInteractable);
						return;
					}
					DestructableComponent destructableComponent;
					if ((destructableComponent = focusableObject as DestructableComponent) != null)
					{
						this.SetDestructibleComponent(destructableComponent, false);
					}
				}
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00015B85 File Offset: 0x00013D85
		internal void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			this.ResetFocus();
			this.IsActive = false;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00015B94 File Offset: 0x00013D94
		internal void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			if (this._mission.Mode == MissionMode.Stealth && agent.IsHuman && agent.IsActive() && !agent.IsEnemyOf(userAgent))
			{
				this.SetHumanAgent(userAgent, agent, true);
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00015BC6 File Offset: 0x00013DC6
		private void GetInteractionTexts(Agent requesterAgent, IFocusable focusable, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			focusableObjectInformation.IsActive = false;
			Mission mission = this._mission;
			if (mission == null)
			{
				return;
			}
			MissionFocusableObjectInformationProvider focusableObjectInformationProvider = mission.FocusableObjectInformationProvider;
			if (focusableObjectInformationProvider == null)
			{
				return;
			}
			focusableObjectInformationProvider.GetInteractionTexts(requesterAgent, focusable, isInteractable, out focusableObjectInformation);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00015BF7 File Offset: 0x00013DF7
		private void SetItem(SpawnedItemEntity item, bool canQuickPickup, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, item, isInteractable);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00015C06 File Offset: 0x00013E06
		private void SetUsableMissionObject(UsableMissionObject usableObject, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, usableObject, isInteractable);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00015C18 File Offset: 0x00013E18
		private void SetUsableMachine(UsableMachine machine, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, machine, isInteractable);
			if (machine.DestructionComponent != null)
			{
				this.TargetHealth = (int)(100f * machine.DestructionComponent.HitPoint / machine.DestructionComponent.MaxHitPoint);
				this.ShowHealthBar = true;
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00015C65 File Offset: 0x00013E65
		private void SetDestructibleComponent(DestructableComponent machine, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, machine, isInteractable);
			this.TargetHealth = (int)(100f * machine.HitPoint / machine.MaxHitPoint);
			this.ShowHealthBar = machine.HitPoint < machine.MaxHitPoint;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00015CA2 File Offset: 0x00013EA2
		private void SetHumanAgent(Agent requesterAgent, Agent focusedAgent, bool isInteractable)
		{
			this.SetInteractionMessages(requesterAgent, focusedAgent, isInteractable);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00015CAD File Offset: 0x00013EAD
		private void SetMount(Agent agent, Agent focusedAgent, bool isInteractable)
		{
			this.SetInteractionMessages(agent, focusedAgent, isInteractable);
			if (focusedAgent.IsActive() && focusedAgent.IsMount && focusedAgent.RiderAgent == null)
			{
				this.ShowHealthBar = false;
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00015CD7 File Offset: 0x00013ED7
		private void SetGenericAgent(Agent agent, Agent focusedAgent, bool isInteractable)
		{
			if (focusedAgent.IsActive() && !focusedAgent.IsMount && !focusedAgent.IsHuman)
			{
				this.SetInteractionMessages(agent, focusedAgent, isInteractable);
			}
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00015CFC File Offset: 0x00013EFC
		private void SetInteractionMessages(Agent requesterAgent, IFocusable focusableObject, bool isInteractable)
		{
			FocusableObjectInformation focusableObjectInformation;
			this.GetInteractionTexts(requesterAgent, focusableObject, isInteractable, out focusableObjectInformation);
			this.IsActive = focusableObjectInformation.IsActive;
			this.PrimaryInteractionMessages[0].SetData(focusableObjectInformation.PrimaryInteractionText, false);
			this.PrimaryInteractionMessages[1].SetData(focusableObjectInformation.SecondaryInteractionText, false);
			this.PrimaryInteractionMessages[1].FocusTypeString = ((focusableObject != null) ? focusableObject.FocusableObjectType.ToString() : null) ?? FocusableObjectType.None.ToString();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00015D90 File Offset: 0x00013F90
		public void OnActiveMissionHintChanged(MissionHint previousHint, MissionHint newHint)
		{
			if (previousHint != null && newHint == null)
			{
				for (int i = this.SecondaryInteractionMessages.Count - 1; i >= 0; i--)
				{
					MissionHintInteractionItemVM missionHintInteractionItemVM;
					if ((missionHintInteractionItemVM = this.SecondaryInteractionMessages[i] as MissionHintInteractionItemVM) != null && missionHintInteractionItemVM.Hint == previousHint)
					{
						this.SecondaryInteractionMessages.RemoveAt(i);
					}
				}
			}
			if (newHint != null)
			{
				this.SecondaryInteractionMessages.Add(new MissionHintInteractionItemVM(newHint));
			}
			this.HasSecondaryMessages = this.SecondaryInteractionMessages.Count > 0;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00015E0D File Offset: 0x0001400D
		public void AddSecondaryMessage(MissionInteractionItemBaseVM message)
		{
			if (this.HasSecondaryInteractionMessage(message))
			{
				Debug.FailedAssert("Trying to add the same interaction message twice", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Missions\\Interaction\\MissionAgentInteractionInterfaceVM.cs", "AddSecondaryMessage", 264);
				return;
			}
			this.SecondaryInteractionMessages.Add(message);
			message.IsDisplayed = true;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00015E45 File Offset: 0x00014045
		public bool RemoveSecondaryMessage(MissionInteractionItemBaseVM message)
		{
			message.IsDisplayed = false;
			return this.SecondaryInteractionMessages.Remove(message);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00015E5A File Offset: 0x0001405A
		public bool HasSecondaryInteractionMessage(MissionInteractionItemBaseVM message)
		{
			return message.IsDisplayed;
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00015E62 File Offset: 0x00014062
		private void SetHealth(float healthPercentage, bool hideHealthBarWhenFull)
		{
			this.TargetHealth = (int)(100f * healthPercentage);
			if (hideHealthBarWhenFull)
			{
				this.ShowHealthBar = this.TargetHealth < 100;
				return;
			}
			this.ShowHealthBar = true;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00015E8D File Offset: 0x0001408D
		public void ResetFocus()
		{
			this._currentFocusedObject = null;
			this.ShowHealthBar = false;
			this.PrimaryInteractionMessages[0].ResetData();
			this.PrimaryInteractionMessages[1].ResetData();
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00015EBF File Offset: 0x000140BF
		public void SetForcedInteractionTexts(TextObject text1, bool isDisabled1, TextObject text2, bool isDisabled2)
		{
			this.HasForcedMessages = true;
			this.ForcedInteractionMessages[0].SetData(text1, isDisabled1);
			this.ForcedInteractionMessages[1].SetData(text2, isDisabled2);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00015EEF File Offset: 0x000140EF
		public void ClearForcedInteractionTexts()
		{
			this.ForcedInteractionMessages[0].ResetData();
			this.ForcedInteractionMessages[1].ResetData();
			this.HasForcedMessages = false;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00015F1A File Offset: 0x0001411A
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00015F22 File Offset: 0x00014122
		[DataSourceProperty]
		public int TargetHealth
		{
			get
			{
				return this._targetHealth;
			}
			set
			{
				if (value != this._targetHealth)
				{
					this._targetHealth = value;
					base.OnPropertyChangedWithValue(value, "TargetHealth");
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00015F40 File Offset: 0x00014140
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x00015F48 File Offset: 0x00014148
		[DataSourceProperty]
		public bool ShowHealthBar
		{
			get
			{
				return this._showHealthBar;
			}
			set
			{
				if (value != this._showHealthBar)
				{
					this._showHealthBar = value;
					base.OnPropertyChangedWithValue(value, "ShowHealthBar");
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00015F66 File Offset: 0x00014166
		// (set) Token: 0x060005B8 RID: 1464 RVA: 0x00015F6E File Offset: 0x0001416E
		[DataSourceProperty]
		public MBBindingList<MissionPrimaryInteractionItemVM> PrimaryInteractionMessages
		{
			get
			{
				return this._primaryInteractionMessages;
			}
			set
			{
				if (this._primaryInteractionMessages != value)
				{
					this._primaryInteractionMessages = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionPrimaryInteractionItemVM>>(value, "PrimaryInteractionMessages");
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00015F8C File Offset: 0x0001418C
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x00015F94 File Offset: 0x00014194
		[DataSourceProperty]
		public MBBindingList<MissionInteractionItemBaseVM> SecondaryInteractionMessages
		{
			get
			{
				return this._secondaryInteractionMessages;
			}
			set
			{
				if (this._secondaryInteractionMessages != value)
				{
					this._secondaryInteractionMessages = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionInteractionItemBaseVM>>(value, "SecondaryInteractionMessages");
				}
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00015FB2 File Offset: 0x000141B2
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x00015FBA File Offset: 0x000141BA
		[DataSourceProperty]
		public string BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				if (this._backgroundColor != value)
				{
					this._backgroundColor = value;
					base.OnPropertyChangedWithValue<string>(value, "BackgroundColor");
				}
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00015FDD File Offset: 0x000141DD
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00015FE5 File Offset: 0x000141E5
		[DataSourceProperty]
		public string TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				if (this._textColor != value)
				{
					this._textColor = value;
					base.OnPropertyChangedWithValue<string>(value, "TextColor");
				}
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00016008 File Offset: 0x00014208
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00016010 File Offset: 0x00014210
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					if (!value)
					{
						this.ShowHealthBar = false;
						this.PrimaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM x)
						{
							x.ResetData();
						});
					}
				}
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0001606D File Offset: 0x0001426D
		// (set) Token: 0x060005C2 RID: 1474 RVA: 0x00016075 File Offset: 0x00014275
		[DataSourceProperty]
		public bool HasSecondaryMessages
		{
			get
			{
				return this._hasSecondaryMessages;
			}
			set
			{
				if (value != this._hasSecondaryMessages)
				{
					this._hasSecondaryMessages = value;
					base.OnPropertyChangedWithValue(value, "HasSecondaryMessages");
				}
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00016093 File Offset: 0x00014293
		// (set) Token: 0x060005C4 RID: 1476 RVA: 0x0001609B File Offset: 0x0001429B
		[DataSourceProperty]
		public bool DisplayInteractionText
		{
			get
			{
				return this._displayInteractionText;
			}
			set
			{
				if (value != this._displayInteractionText)
				{
					this._displayInteractionText = value;
					base.OnPropertyChangedWithValue(value, "DisplayInteractionText");
				}
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060005C5 RID: 1477 RVA: 0x000160B9 File Offset: 0x000142B9
		// (set) Token: 0x060005C6 RID: 1478 RVA: 0x000160C1 File Offset: 0x000142C1
		[DataSourceProperty]
		public MBBindingList<MissionPrimaryInteractionItemVM> ForcedInteractionMessages
		{
			get
			{
				return this._forcedInteractionMessages;
			}
			set
			{
				if (this._forcedInteractionMessages != value)
				{
					this._forcedInteractionMessages = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionPrimaryInteractionItemVM>>(value, "ForcedInteractionMessages");
				}
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000160DF File Offset: 0x000142DF
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x000160E8 File Offset: 0x000142E8
		[DataSourceProperty]
		public bool HasForcedMessages
		{
			get
			{
				return this._hasForcedMessages;
			}
			set
			{
				if (this._hasForcedMessages != value)
				{
					this._hasForcedMessages = value;
					base.OnPropertyChangedWithValue(value, "HasForcedMessages");
					if (!value)
					{
						this.ForcedInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM x)
						{
							x.ResetData();
						});
					}
				}
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00016140 File Offset: 0x00014340
		private string GetWeaponSpecificText(SpawnedItemEntity spawnedItem)
		{
			MissionWeapon weaponCopy = spawnedItem.WeaponCopy;
			WeaponComponentData currentUsageItem = weaponCopy.CurrentUsageItem;
			if (currentUsageItem != null && currentUsageItem.IsShield)
			{
				MBTextManager.SetTextVariable("LEFT", (int)weaponCopy.HitPoints);
				MBTextManager.SetTextVariable("RIGHT", (int)weaponCopy.ModifiedMaxHitPoints);
				return GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null).ToString();
			}
			WeaponComponentData currentUsageItem2 = weaponCopy.CurrentUsageItem;
			if (currentUsageItem2 != null && currentUsageItem2.IsAmmo && weaponCopy.ModifiedMaxAmount > 1 && !spawnedItem.IsStuckMissile())
			{
				MBTextManager.SetTextVariable("LEFT", (int)weaponCopy.Amount);
				MBTextManager.SetTextVariable("RIGHT", (int)weaponCopy.ModifiedMaxAmount);
				return GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null).ToString();
			}
			return "";
		}

		// Token: 0x0400028C RID: 652
		private readonly Mission _mission;

		// Token: 0x0400028D RID: 653
		private bool _currentObjectInteractable;

		// Token: 0x0400028E RID: 654
		private IFocusable _currentFocusedObject;

		// Token: 0x0400028F RID: 655
		private bool _isActive;

		// Token: 0x04000290 RID: 656
		private bool _hasSecondaryMessages;

		// Token: 0x04000291 RID: 657
		private MBBindingList<MissionPrimaryInteractionItemVM> _primaryInteractionMessages;

		// Token: 0x04000292 RID: 658
		private MBBindingList<MissionInteractionItemBaseVM> _secondaryInteractionMessages;

		// Token: 0x04000293 RID: 659
		private int _targetHealth;

		// Token: 0x04000294 RID: 660
		private bool _showHealthBar;

		// Token: 0x04000295 RID: 661
		private string _backgroundColor;

		// Token: 0x04000296 RID: 662
		private string _textColor;

		// Token: 0x04000297 RID: 663
		private bool _displayInteractionText;

		// Token: 0x04000298 RID: 664
		private bool _hasForcedMessages;

		// Token: 0x04000299 RID: 665
		private MBBindingList<MissionPrimaryInteractionItemVM> _forcedInteractionMessages;
	}
}
