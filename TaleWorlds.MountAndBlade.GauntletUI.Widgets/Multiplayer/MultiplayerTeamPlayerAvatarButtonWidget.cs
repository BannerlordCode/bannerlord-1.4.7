using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008F RID: 143
	public class MultiplayerTeamPlayerAvatarButtonWidget : ButtonWidget
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x00016865 File Offset: 0x00014A65
		public MultiplayerTeamPlayerAvatarButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00016879 File Offset: 0x00014A79
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized && this.AvatarImage != null)
			{
				this._originalAvatarImageAlpha = this.AvatarImage.ReadOnlyBrush.GlobalAlphaFactor;
				this.UpdateGlobalAlpha();
				this._isInitialized = true;
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000168B8 File Offset: 0x00014AB8
		private void UpdateGlobalAlpha()
		{
			if (this._isInitialized)
			{
				float num = (this.IsDead ? this.DeathAlphaFactor : 1f);
				float num2 = num * this._originalAvatarImageAlpha;
				this.SetGlobalAlphaRecursively(num);
				this.AvatarImage.Brush.GlobalAlphaFactor = num2;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00016904 File Offset: 0x00014B04
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x0001690C File Offset: 0x00014B0C
		[DataSourceProperty]
		public bool IsDead
		{
			get
			{
				return this._isDead;
			}
			set
			{
				if (this._isDead != value)
				{
					this._isDead = value;
					base.OnPropertyChanged(value, "IsDead");
					this.UpdateGlobalAlpha();
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00016930 File Offset: 0x00014B30
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x00016938 File Offset: 0x00014B38
		[DataSourceProperty]
		public float DeathAlphaFactor
		{
			get
			{
				return this._deathAlphaFactor;
			}
			set
			{
				if (this._deathAlphaFactor != value)
				{
					this._deathAlphaFactor = value;
					base.OnPropertyChanged(value, "DeathAlphaFactor");
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00016956 File Offset: 0x00014B56
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0001695E File Offset: 0x00014B5E
		[DataSourceProperty]
		public ImageIdentifierWidget AvatarImage
		{
			get
			{
				return this._avatarImage;
			}
			set
			{
				if (this._avatarImage != value)
				{
					this._avatarImage = value;
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "AvatarImage");
				}
			}
		}

		// Token: 0x0400036C RID: 876
		private bool _isInitialized;

		// Token: 0x0400036D RID: 877
		private float _originalAvatarImageAlpha = 1f;

		// Token: 0x0400036E RID: 878
		private bool _isDead;

		// Token: 0x0400036F RID: 879
		private float _deathAlphaFactor;

		// Token: 0x04000370 RID: 880
		private ImageIdentifierWidget _avatarImage;
	}
}
