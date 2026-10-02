using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Inventory
{
	// Token: 0x0200013B RID: 315
	public class InventoryEquippedItemSlotWidget : InventoryItemButtonWidget
	{
		// Token: 0x0600105E RID: 4190 RVA: 0x0002CC26 File Offset: 0x0002AE26
		public InventoryEquippedItemSlotWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x0002CC30 File Offset: 0x0002AE30
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.ScreenWidget == null || this.Background == null)
			{
				return;
			}
			bool flag = base.ScreenWidget.TargetEquipmentIndex == this.TargetEquipmentIndex;
			bool flag2 = this.TargetEquipmentIndex == 0 && base.ScreenWidget.TargetEquipmentIndex >= 0 && base.ScreenWidget.TargetEquipmentIndex <= 3;
			if (flag || flag2)
			{
				this.Background.SetState("Selected");
				return;
			}
			this.Background.SetState("Default");
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0002CCB8 File Offset: 0x0002AEB8
		private void ImageIdentifierOnPropertyChanged(PropertyOwnerObject owner, string propertyName, object value)
		{
			if (propertyName == "ImageId")
			{
				base.IsHidden = string.IsNullOrEmpty((string)value);
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x0002CCD8 File Offset: 0x0002AED8
		// (set) Token: 0x06001062 RID: 4194 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
		[Editor(false)]
		public ImageIdentifierWidget ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (this._imageIdentifier != value)
				{
					if (this._imageIdentifier != null)
					{
						this._imageIdentifier.PropertyChanged -= this.ImageIdentifierOnPropertyChanged;
					}
					this._imageIdentifier = value;
					if (this._imageIdentifier != null)
					{
						this._imageIdentifier.PropertyChanged += this.ImageIdentifierOnPropertyChanged;
					}
					base.OnPropertyChanged<ImageIdentifierWidget>(value, "ImageIdentifier");
				}
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x0002CD47 File Offset: 0x0002AF47
		// (set) Token: 0x06001064 RID: 4196 RVA: 0x0002CD4F File Offset: 0x0002AF4F
		[Editor(false)]
		public Widget Background
		{
			get
			{
				return this._background;
			}
			set
			{
				if (this._background != value)
				{
					this._background = value;
					this._background.AddState("Selected");
					base.OnPropertyChanged<Widget>(value, "Background");
				}
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x0002CD7D File Offset: 0x0002AF7D
		// (set) Token: 0x06001066 RID: 4198 RVA: 0x0002CD85 File Offset: 0x0002AF85
		[Editor(false)]
		public int TargetEquipmentIndex
		{
			get
			{
				return this._targetEquipmentIndex;
			}
			set
			{
				if (this._targetEquipmentIndex != value)
				{
					this._targetEquipmentIndex = value;
					base.OnPropertyChanged(value, "TargetEquipmentIndex");
				}
			}
		}

		// Token: 0x04000768 RID: 1896
		private ImageIdentifierWidget _imageIdentifier;

		// Token: 0x04000769 RID: 1897
		private Widget _background;

		// Token: 0x0400076A RID: 1898
		private int _targetEquipmentIndex;
	}
}
