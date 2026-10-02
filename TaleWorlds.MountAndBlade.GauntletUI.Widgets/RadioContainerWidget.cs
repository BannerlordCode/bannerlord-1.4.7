using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000037 RID: 55
	public class RadioContainerWidget : Widget
	{
		// Token: 0x06000345 RID: 837 RVA: 0x0000A817 File Offset: 0x00008A17
		public RadioContainerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000A820 File Offset: 0x00008A20
		private void ContainerOnPropertyChanged(PropertyOwnerObject owner, string propertyName, int value)
		{
			if (propertyName == "IntValue")
			{
				this.SelectedIndex = this.Container.IntValue;
			}
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000A840 File Offset: 0x00008A40
		private void ContainerOnEventFire(Widget owner, string eventName, object[] arguments)
		{
			if (eventName == "ItemAdd" || eventName == "ItemRemove")
			{
				this.Container.IntValue = this.SelectedIndex;
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000A870 File Offset: 0x00008A70
		private void ContainerUpdated(Container newContainer)
		{
			if (this.Container != null)
			{
				this.Container.intPropertyChanged -= this.ContainerOnPropertyChanged;
				this.Container.EventFire -= this.ContainerOnEventFire;
			}
			if (newContainer != null)
			{
				newContainer.intPropertyChanged += this.ContainerOnPropertyChanged;
				newContainer.EventFire += this.ContainerOnEventFire;
				newContainer.IntValue = this.SelectedIndex;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000A8E6 File Offset: 0x00008AE6
		// (set) Token: 0x0600034A RID: 842 RVA: 0x0000A8EE File Offset: 0x00008AEE
		[Editor(false)]
		public int SelectedIndex
		{
			get
			{
				return this._selectedIndex;
			}
			set
			{
				if (this._selectedIndex != value)
				{
					this._selectedIndex = value;
					base.OnPropertyChanged(value, "SelectedIndex");
					if (this.Container != null)
					{
						this.Container.IntValue = this._selectedIndex;
					}
				}
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000A925 File Offset: 0x00008B25
		// (set) Token: 0x0600034C RID: 844 RVA: 0x0000A92D File Offset: 0x00008B2D
		[Editor(false)]
		public Container Container
		{
			get
			{
				return this._container;
			}
			set
			{
				if (this._container != value)
				{
					this.ContainerUpdated(value);
					this._container = value;
					base.OnPropertyChanged<Container>(value, "Container");
				}
			}
		}

		// Token: 0x04000153 RID: 339
		private int _selectedIndex;

		// Token: 0x04000154 RID: 340
		private Container _container;
	}
}
