using System;
using System.Collections.ObjectModel;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000050 RID: 80
	public class MissionAgentTakenDamageVM : ViewModel
	{
		// Token: 0x060006A5 RID: 1701 RVA: 0x000183EA File Offset: 0x000165EA
		public MissionAgentTakenDamageVM(Camera missionCamera)
		{
			this._missionCamera = missionCamera;
			this.TakenDamageList = new MBBindingList<MissionAgentTakenDamageItemVM>();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00018404 File Offset: 0x00016604
		public void SetIsEnabled(bool isEnabled)
		{
			this._isEnabled = isEnabled;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00018410 File Offset: 0x00016610
		internal void Tick(float dt)
		{
			if (this._isEnabled)
			{
				for (int i = 0; i < this.TakenDamageList.Count; i++)
				{
					this.TakenDamageList[i].Update();
				}
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001844C File Offset: 0x0001664C
		internal void OnMainAgentHit(int damage, float distance)
		{
			if (this._isEnabled && damage > 0)
			{
				Collection<MissionAgentTakenDamageItemVM> takenDamageList = this.TakenDamageList;
				Camera missionCamera = this._missionCamera;
				Agent main = Agent.Main;
				takenDamageList.Add(new MissionAgentTakenDamageItemVM(missionCamera, (main != null) ? main.Position : default(Vec3), damage, false, new Action<MissionAgentTakenDamageItemVM>(this.OnRemoveDamageItem)));
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000184A2 File Offset: 0x000166A2
		private void OnRemoveDamageItem(MissionAgentTakenDamageItemVM item)
		{
			this.TakenDamageList.Remove(item);
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x000184B1 File Offset: 0x000166B1
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x000184B9 File Offset: 0x000166B9
		[DataSourceProperty]
		public MBBindingList<MissionAgentTakenDamageItemVM> TakenDamageList
		{
			get
			{
				return this._takenDamageList;
			}
			set
			{
				if (value != this._takenDamageList)
				{
					this._takenDamageList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAgentTakenDamageItemVM>>(value, "TakenDamageList");
				}
			}
		}

		// Token: 0x040002F2 RID: 754
		private Camera _missionCamera;

		// Token: 0x040002F3 RID: 755
		private bool _isEnabled;

		// Token: 0x040002F4 RID: 756
		private MBBindingList<MissionAgentTakenDamageItemVM> _takenDamageList;
	}
}
