using System;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000066 RID: 102
	public class SelectedStateBrushWidget : BrushWidget
	{
		// Token: 0x060006FB RID: 1787 RVA: 0x0001E484 File Offset: 0x0001C684
		public SelectedStateBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isBrushStatesRegistered)
			{
				this.RegisterBrushStatesOfWidget();
				this._isBrushStatesRegistered = true;
			}
			if (this._isDirty)
			{
				this.SetState(this.SelectedState ?? "Default");
				this._isDirty = false;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0001E4ED File Offset: 0x0001C6ED
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x0001E4F5 File Offset: 0x0001C6F5
		[Editor(false)]
		public string SelectedState
		{
			get
			{
				return this._selectedState;
			}
			set
			{
				if (this._selectedState != value)
				{
					this._selectedState = value;
					base.OnPropertyChanged<string>(value, "SelectedState");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x04000346 RID: 838
		private bool _isDirty = true;

		// Token: 0x04000347 RID: 839
		private bool _isBrushStatesRegistered;

		// Token: 0x04000348 RID: 840
		private string _selectedState = "Default";
	}
}
