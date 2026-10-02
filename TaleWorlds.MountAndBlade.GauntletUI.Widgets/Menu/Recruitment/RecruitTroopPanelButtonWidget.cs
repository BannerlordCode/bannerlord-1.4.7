using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Recruitment
{
	// Token: 0x0200010E RID: 270
	public class RecruitTroopPanelButtonWidget : ButtonWidget
	{
		// Token: 0x06000E53 RID: 3667 RVA: 0x000277FE File Offset: 0x000259FE
		public RecruitTroopPanelButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00027808 File Offset: 0x00025A08
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this.IsTroopEmpty)
			{
				if (this.PlayerHasEnoughRelation)
				{
					this.SetState("EmptyEnoughRelation");
				}
				else
				{
					this.SetState("EmptyNoRelation");
				}
			}
			else if (this.CanBeRecruited)
			{
				this.SetState("Available");
			}
			else
			{
				this.SetState("Unavailable");
			}
			if (!this.PlayerHasEnoughRelation && !this.IsTroopEmpty && this.CharacterImageWidget != null)
			{
				this.CharacterImageWidget.Brush.ValueFactor = -50f;
				this.CharacterImageWidget.Brush.SaturationFactor = -100f;
			}
			if (this.CharacterImageWidget != null)
			{
				this.CharacterImageWidget.IsHidden = this.IsTroopEmpty;
			}
			this.RemoveFromCartButton.SetState(((base.IsHovered || base.IsPressed || base.IsSelected) && ((!this.IsTroopEmpty && this.PlayerHasEnoughRelation) || this.IsInCart)) ? "Hovered" : "Default");
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00027908 File Offset: 0x00025B08
		private bool IsMouseOverWidget()
		{
			Vector2 globalPosition = base.GlobalPosition;
			return this.IsBetween(base.EventManager.MousePosition.X, globalPosition.X, globalPosition.X + base.Size.X) && this.IsBetween(base.EventManager.MousePosition.Y, globalPosition.Y, globalPosition.Y + base.Size.Y);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0002797C File Offset: 0x00025B7C
		private bool IsBetween(float number, float min, float max)
		{
			return number >= min && number <= max;
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0002798B File Offset: 0x00025B8B
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x00027993 File Offset: 0x00025B93
		[Editor(false)]
		public bool CanBeRecruited
		{
			get
			{
				return this._canBeRecruited;
			}
			set
			{
				if (this._canBeRecruited != value)
				{
					this._canBeRecruited = value;
					base.OnPropertyChanged(value, "CanBeRecruited");
				}
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x000279B1 File Offset: 0x00025BB1
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x000279B9 File Offset: 0x00025BB9
		[Editor(false)]
		public bool IsInCart
		{
			get
			{
				return this._isInCart;
			}
			set
			{
				if (this._isInCart != value)
				{
					this._isInCart = value;
					base.OnPropertyChanged(value, "IsInCart");
				}
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x000279D7 File Offset: 0x00025BD7
		// (set) Token: 0x06000E5C RID: 3676 RVA: 0x000279DF File Offset: 0x00025BDF
		[Editor(false)]
		public ButtonWidget RemoveFromCartButton
		{
			get
			{
				return this._removeFromCartButton;
			}
			set
			{
				if (this._removeFromCartButton != value)
				{
					this._removeFromCartButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "RemoveFromCartButton");
				}
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x000279FD File Offset: 0x00025BFD
		// (set) Token: 0x06000E5E RID: 3678 RVA: 0x00027A05 File Offset: 0x00025C05
		[Editor(false)]
		public ImageIdentifierWidget CharacterImageWidget
		{
			get
			{
				return this._characterImageWidget;
			}
			set
			{
				if (this._characterImageWidget != value)
				{
					this._characterImageWidget = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "CharacterImageWidget");
				}
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00027A23 File Offset: 0x00025C23
		// (set) Token: 0x06000E60 RID: 3680 RVA: 0x00027A2B File Offset: 0x00025C2B
		[Editor(false)]
		public bool IsTroopEmpty
		{
			get
			{
				return this._isTroopEmpty;
			}
			set
			{
				if (this._isTroopEmpty != value)
				{
					this._isTroopEmpty = value;
					base.OnPropertyChanged(value, "IsTroopEmpty");
				}
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00027A49 File Offset: 0x00025C49
		// (set) Token: 0x06000E62 RID: 3682 RVA: 0x00027A51 File Offset: 0x00025C51
		[Editor(false)]
		public bool PlayerHasEnoughRelation
		{
			get
			{
				return this._playerHasEnoughRelation;
			}
			set
			{
				if (this._playerHasEnoughRelation != value)
				{
					this._playerHasEnoughRelation = value;
					base.OnPropertyChanged(value, "PlayerHasEnoughRelation");
				}
			}
		}

		// Token: 0x0400067F RID: 1663
		private bool _canBeRecruited;

		// Token: 0x04000680 RID: 1664
		private bool _isInCart;

		// Token: 0x04000681 RID: 1665
		private bool _playerHasEnoughRelation;

		// Token: 0x04000682 RID: 1666
		private bool _isTroopEmpty;

		// Token: 0x04000683 RID: 1667
		private ButtonWidget _removeFromCartButton;

		// Token: 0x04000684 RID: 1668
		private ImageIdentifierWidget _characterImageWidget;
	}
}
