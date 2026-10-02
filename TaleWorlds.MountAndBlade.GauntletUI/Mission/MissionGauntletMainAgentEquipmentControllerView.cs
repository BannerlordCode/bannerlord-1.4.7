using System;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000034 RID: 52
	[OverrideView(typeof(MissionMainAgentEquipmentControllerView))]
	public class MissionGauntletMainAgentEquipmentControllerView : MissionView
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600023F RID: 575 RVA: 0x0000D230 File Offset: 0x0000B430
		// (remove) Token: 0x06000240 RID: 576 RVA: 0x0000D268 File Offset: 0x0000B468
		public event Action<bool> OnEquipmentDropInteractionViewToggled;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000241 RID: 577 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		// (remove) Token: 0x06000242 RID: 578 RVA: 0x0000D2D8 File Offset: 0x0000B4D8
		public event Action<bool> OnEquipmentEquipInteractionViewToggled;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000D30D File Offset: 0x0000B50D
		private bool IsDisplayingADialog
		{
			get
			{
				IMissionScreen missionScreenAsInterface = this._missionScreenAsInterface;
				return missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog();
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000D320 File Offset: 0x0000B520
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000D328 File Offset: 0x0000B528
		private bool EquipHoldHandled
		{
			get
			{
				return this._equipHoldHandled;
			}
			set
			{
				this._equipHoldHandled = value;
				if (this._equipHoldHandled)
				{
					MissionScreen missionScreen = base.MissionScreen;
					if (missionScreen == null)
					{
						return;
					}
					missionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentEquipmentControllerView>(this);
					return;
				}
				else
				{
					MissionScreen missionScreen2 = base.MissionScreen;
					if (missionScreen2 == null)
					{
						return;
					}
					missionScreen2.UnregisterRadialMenuObject(this);
					return;
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000D35C File Offset: 0x0000B55C
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000D364 File Offset: 0x0000B564
		private bool DropHoldHandled
		{
			get
			{
				return this._dropHoldHandled;
			}
			set
			{
				this._dropHoldHandled = value;
				if (this._dropHoldHandled)
				{
					MissionScreen missionScreen = base.MissionScreen;
					if (missionScreen == null)
					{
						return;
					}
					missionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentEquipmentControllerView>(this);
					return;
				}
				else
				{
					MissionScreen missionScreen2 = base.MissionScreen;
					if (missionScreen2 == null)
					{
						return;
					}
					missionScreen2.UnregisterRadialMenuObject(this);
					return;
				}
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000D398 File Offset: 0x0000B598
		public MissionGauntletMainAgentEquipmentControllerView()
		{
			this._missionScreenAsInterface = base.MissionScreen;
			this.EquipHoldHandled = false;
			this.DropHoldHandled = false;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000D3BC File Offset: 0x0000B5BC
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._gauntletLayer = new GauntletLayer("MissionEquipmentController", this.ViewOrderPriority, false);
			this._dataSource = new MissionMainAgentEquipmentControllerVM(new Action<EquipmentIndex>(this.OnDropEquipment), new Action<SpawnedItemEntity, EquipmentIndex>(this.OnEquipItem));
			this._gauntletLayer.LoadMovie("MainAgentEquipmentController", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000D45C File Offset: 0x0000B65C
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this.IsMainAgentAvailable() && base.Mission.IsMainAgentItemInteractionEnabled)
			{
				this.DropWeaponTick(dt);
				this.EquipWeaponTick(dt);
				return;
			}
			this._prevDropKeyDown = false;
			this._prevEquipKeyDown = false;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000D4EC File Offset: 0x0000B6EC
		public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
			base.OnFocusGained(agent, focusableObject, isInteractable);
			UsableMissionObject usableMissionObject;
			SpawnedItemEntity spawnedItemEntity;
			if ((usableMissionObject = focusableObject as UsableMissionObject) != null && (spawnedItemEntity = usableMissionObject as SpawnedItemEntity) != null)
			{
				this._isCurrentFocusedItemInteractable = isInteractable;
				if (!spawnedItemEntity.WeaponCopy.IsEmpty)
				{
					this._isFocusedOnEquipment = true;
					this._focusedWeaponItem = spawnedItemEntity;
					this._dataSource.SetCurrentFocusedWeaponEntity(this._focusedWeaponItem);
				}
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000D54C File Offset: 0x0000B74C
		public override void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			base.OnFocusLost(agent, focusableObject);
			this._isCurrentFocusedItemInteractable = false;
			this._isFocusedOnEquipment = false;
			this._focusedWeaponItem = null;
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.SetCurrentFocusedWeaponEntity(this._focusedWeaponItem);
			}
			if (this.EquipHoldHandled)
			{
				this.EquipHoldHandled = false;
				this._equipHoldTime = 0f;
				MissionMainAgentEquipmentControllerVM dataSource2 = this._dataSource;
				if (dataSource2 != null)
				{
					dataSource2.OnCancelEquipController();
				}
				Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
				if (onEquipmentEquipInteractionViewToggled != null)
				{
					onEquipmentEquipInteractionViewToggled(false);
				}
				this._equipmentWasInFocusFirstFrameOfEquipDown = false;
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent == null)
			{
				if (this.EquipHoldHandled)
				{
					this.EquipHoldHandled = false;
					Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
					if (onEquipmentEquipInteractionViewToggled != null)
					{
						onEquipmentEquipInteractionViewToggled(false);
					}
				}
				this._equipHoldTime = 0f;
				this._dataSource.OnCancelEquipController();
				if (this.DropHoldHandled)
				{
					Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
					if (onEquipmentDropInteractionViewToggled != null)
					{
						onEquipmentDropInteractionViewToggled(false);
					}
					this.DropHoldHandled = false;
				}
				this._dropHoldTime = 0f;
				this._dataSource.OnCancelDropController();
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000D65C File Offset: 0x0000B85C
		private void EquipWeaponTick(float dt)
		{
			if (base.MissionScreen.SceneLayer.Input.IsGameKeyDown(13) && !this._prevDropKeyDown && !this.IsDisplayingADialog && this.IsMainAgentAvailable() && !base.MissionScreen.Mission.IsOrderMenuOpen)
			{
				if (!this._firstFrameOfEquipDownHandled)
				{
					this._equipmentWasInFocusFirstFrameOfEquipDown = this._isFocusedOnEquipment;
					this._firstFrameOfEquipDownHandled = true;
				}
				if (this._equipmentWasInFocusFirstFrameOfEquipDown)
				{
					this._equipHoldTime += dt;
					if (this._equipHoldTime > 0.5f && !this.EquipHoldHandled && this._isFocusedOnEquipment && this._isCurrentFocusedItemInteractable)
					{
						this.HandleOpeningHoldEquip();
						this.EquipHoldHandled = true;
					}
				}
				this._prevEquipKeyDown = true;
				return;
			}
			if (this._prevEquipKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(13))
			{
				if (this._equipmentWasInFocusFirstFrameOfEquipDown)
				{
					if (this._equipHoldTime < 0.5f)
					{
						if (this._focusedWeaponItem != null)
						{
							Agent main = Agent.Main;
							if (main != null && main.CanQuickPickUp(this._focusedWeaponItem))
							{
								this.HandleQuickReleaseEquip();
							}
						}
					}
					else
					{
						this.HandleClosingHoldEquip();
					}
				}
				if (this.EquipHoldHandled)
				{
					this.EquipHoldHandled = false;
				}
				this._equipHoldTime = 0f;
				this._firstFrameOfEquipDownHandled = false;
				this._prevEquipKeyDown = false;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		private void DropWeaponTick(float dt)
		{
			if (base.MissionScreen.SceneLayer.Input.IsGameKeyDown(22) && !this._prevEquipKeyDown && !this.IsDisplayingADialog && this.IsMainAgentAvailable() && this.IsMainAgentHasAtLeastOneItem() && !base.MissionScreen.Mission.IsOrderMenuOpen)
			{
				this._dropHoldTime += dt;
				if (this._dropHoldTime > 0.5f && !this.DropHoldHandled)
				{
					this.HandleOpeningHoldDrop();
					this.DropHoldHandled = true;
				}
				this._prevDropKeyDown = true;
				return;
			}
			if (this._prevDropKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(22))
			{
				if (this._dropHoldTime < 0.5f)
				{
					this.HandleQuickReleaseDrop();
				}
				else
				{
					this.HandleClosingHoldDrop();
				}
				this.DropHoldHandled = false;
				this._dropHoldTime = 0f;
				this._prevDropKeyDown = false;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D895 File Offset: 0x0000BA95
		private void HandleOpeningHoldEquip()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnEquipControllerToggle(true);
			}
			Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
			if (onEquipmentEquipInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentEquipInteractionViewToggled(true);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D8BA File Offset: 0x0000BABA
		private void HandleClosingHoldEquip()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnEquipControllerToggle(false);
			}
			Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
			if (onEquipmentEquipInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentEquipInteractionViewToggled(false);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D8DF File Offset: 0x0000BADF
		private void HandleQuickReleaseEquip()
		{
			this.OnEquipItem(this._focusedWeaponItem, EquipmentIndex.None);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000D8EE File Offset: 0x0000BAEE
		private void HandleOpeningHoldDrop()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnDropControllerToggle(true);
			}
			Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
			if (onEquipmentDropInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentDropInteractionViewToggled(true);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000D913 File Offset: 0x0000BB13
		private void HandleClosingHoldDrop()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnDropControllerToggle(false);
			}
			Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
			if (onEquipmentDropInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentDropInteractionViewToggled(false);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000D938 File Offset: 0x0000BB38
		private void HandleQuickReleaseDrop()
		{
			this.OnDropEquipment(EquipmentIndex.None);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D944 File Offset: 0x0000BB44
		private void OnEquipItem(SpawnedItemEntity itemToEquip, EquipmentIndex indexToEquipItTo)
		{
			if (itemToEquip.GameEntity.IsValid)
			{
				Agent main = Agent.Main;
				if (main == null)
				{
					return;
				}
				main.HandleStartUsingAction(itemToEquip, (int)indexToEquipItTo);
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D974 File Offset: 0x0000BB74
		private void OnDropEquipment(EquipmentIndex indexToDrop)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new DropWeapon(base.Input.IsGameKeyDown(10), indexToDrop));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			Agent.Main.HandleDropWeapon(base.Input.IsGameKeyDown(10), indexToDrop);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D9C3 File Offset: 0x0000BBC3
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			return main != null && main.IsActive();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000D9D8 File Offset: 0x0000BBD8
		private bool IsMainAgentHasAtLeastOneItem()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!Agent.Main.Equipment[equipmentIndex].IsEmpty)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000DA0E File Offset: 0x0000BC0E
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000DA33 File Offset: 0x0000BC33
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000125 RID: 293
		private const float _minHoldTime = 0.5f;

		// Token: 0x04000128 RID: 296
		private readonly IMissionScreen _missionScreenAsInterface;

		// Token: 0x04000129 RID: 297
		private bool _equipmentWasInFocusFirstFrameOfEquipDown;

		// Token: 0x0400012A RID: 298
		private bool _firstFrameOfEquipDownHandled;

		// Token: 0x0400012B RID: 299
		private bool _equipHoldHandled;

		// Token: 0x0400012C RID: 300
		private bool _isFocusedOnEquipment;

		// Token: 0x0400012D RID: 301
		private float _equipHoldTime;

		// Token: 0x0400012E RID: 302
		private bool _prevEquipKeyDown;

		// Token: 0x0400012F RID: 303
		private SpawnedItemEntity _focusedWeaponItem;

		// Token: 0x04000130 RID: 304
		private bool _dropHoldHandled;

		// Token: 0x04000131 RID: 305
		private float _dropHoldTime;

		// Token: 0x04000132 RID: 306
		private bool _prevDropKeyDown;

		// Token: 0x04000133 RID: 307
		private bool _isCurrentFocusedItemInteractable;

		// Token: 0x04000134 RID: 308
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000135 RID: 309
		private MissionMainAgentEquipmentControllerVM _dataSource;
	}
}
