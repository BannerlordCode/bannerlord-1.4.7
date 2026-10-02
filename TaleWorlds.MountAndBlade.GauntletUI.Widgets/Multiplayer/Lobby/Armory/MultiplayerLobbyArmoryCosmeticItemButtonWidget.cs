using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B8 RID: 184
	public class MultiplayerLobbyArmoryCosmeticItemButtonWidget : ButtonWidget
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x0001B077 File Offset: 0x00019277
		public MultiplayerLobbyArmoryCosmeticItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001B080 File Offset: 0x00019280
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.EventManager.HoveredWidget == this && Input.IsKeyPressed(InputKey.ControllerRUp))
			{
				this.OnMouseAlternatePressed();
				return;
			}
			if (base.EventManager.HoveredWidget == this && Input.IsKeyReleased(InputKey.ControllerRUp))
			{
				this.OnMouseAlternateReleased(true);
			}
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001B0D8 File Offset: 0x000192D8
		private void UpdateSelectableState()
		{
			this._selectableTimer = 0f;
			base.IsDisabled = !this.IsSelectable;
			this._animationStartAlpha = (this.IsSelectable ? this.NonSelectableStateAlpha : this.SelectableStateAlpha);
			this._animationTargetAlpha = (this.IsSelectable ? this.SelectableStateAlpha : this.NonSelectableStateAlpha);
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateSelectableState), 1);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001B150 File Offset: 0x00019350
		private void AnimateSelectableState(float dt)
		{
			this._selectableTimer += dt;
			float num;
			if (this._selectableTimer < this.SelectableStateAnimationDuration)
			{
				num = this._selectableTimer / this.SelectableStateAnimationDuration;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.AnimateSelectableState), 1);
			}
			else
			{
				num = 1f;
			}
			float num2 = MathF.Lerp(this._animationStartAlpha, this._animationTargetAlpha, num, 1E-05f);
			base.IsVisible = num2 != 0f;
			this.SetGlobalAlphaRecursively(num2);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001B1D8 File Offset: 0x000193D8
		protected override void HandleClick()
		{
			base.HandleClick();
			if (this.IsUnlocked)
			{
				this.HandleSoundEvent();
				return;
			}
			base.EventFired("Obtain", Array.Empty<object>());
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001B1FF File Offset: 0x000193FF
		protected override void HandleAlternateClick()
		{
			base.HandleAlternateClick();
			this.HandleSoundEvent();
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0001B210 File Offset: 0x00019410
		private void HandleSoundEvent()
		{
			int itemType = this.ItemType;
			switch (itemType)
			{
			case 12:
				base.EventFired("WearHelmet", Array.Empty<object>());
				return;
			case 13:
				base.EventFired("WearArmorBig", Array.Empty<object>());
				return;
			case 14:
			case 15:
				break;
			default:
				if (itemType != 22)
				{
					base.EventFired("WearGeneric", Array.Empty<object>());
					return;
				}
				break;
			}
			base.EventFired("WearArmorSmall", Array.Empty<object>());
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0001B287 File Offset: 0x00019487
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0001B28F File Offset: 0x0001948F
		public int ItemType
		{
			get
			{
				return this._itemType;
			}
			set
			{
				if (value != this._itemType)
				{
					this._itemType = value;
					base.OnPropertyChanged(value, "ItemType");
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0001B2AD File Offset: 0x000194AD
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0001B2B5 File Offset: 0x000194B5
		public bool IsUnlocked
		{
			get
			{
				return this._isUnlocked;
			}
			set
			{
				if (value != this._isUnlocked)
				{
					this._isUnlocked = value;
					base.OnPropertyChanged(value, "IsUnlocked");
				}
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0001B2D3 File Offset: 0x000194D3
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0001B2DB File Offset: 0x000194DB
		public float SelectableStateAnimationDuration
		{
			get
			{
				return this._selectableStateAnimationDuration;
			}
			set
			{
				if (value != this._selectableStateAnimationDuration)
				{
					this._selectableStateAnimationDuration = value;
					base.OnPropertyChanged(value, "SelectableStateAnimationDuration");
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0001B2F9 File Offset: 0x000194F9
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0001B301 File Offset: 0x00019501
		public float SelectableStateAlpha
		{
			get
			{
				return this._selectableStateAlpha;
			}
			set
			{
				if (value != this._selectableStateAlpha)
				{
					this._selectableStateAlpha = value;
					base.OnPropertyChanged(value, "SelectableStateAlpha");
				}
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0001B31F File Offset: 0x0001951F
		// (set) Token: 0x060009B0 RID: 2480 RVA: 0x0001B327 File Offset: 0x00019527
		public float NonSelectableStateAlpha
		{
			get
			{
				return this._nonSelectableStateAlpha;
			}
			set
			{
				if (value != this._nonSelectableStateAlpha)
				{
					this._nonSelectableStateAlpha = value;
					base.OnPropertyChanged(value, "NonSelectableStateAlpha");
				}
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x0001B345 File Offset: 0x00019545
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x0001B34D File Offset: 0x0001954D
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
					base.OnPropertyChanged(value, "IsSelectable");
					this.UpdateSelectableState();
				}
			}
		}

		// Token: 0x0400045A RID: 1114
		private float _selectableTimer;

		// Token: 0x0400045B RID: 1115
		private float _animationTargetAlpha;

		// Token: 0x0400045C RID: 1116
		private float _animationStartAlpha;

		// Token: 0x0400045D RID: 1117
		private int _itemType;

		// Token: 0x0400045E RID: 1118
		private bool _isUnlocked;

		// Token: 0x0400045F RID: 1119
		private float _selectableStateAnimationDuration;

		// Token: 0x04000460 RID: 1120
		private float _selectableStateAlpha;

		// Token: 0x04000461 RID: 1121
		private float _nonSelectableStateAlpha;

		// Token: 0x04000462 RID: 1122
		private bool _isSelectable;
	}
}
