using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets
{
	// Token: 0x0200009C RID: 156
	public class MissionSiegeEngineMarkerTargetVM : MissionMarkerTargetVM
	{
		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0002EB63 File Offset: 0x0002CD63
		public override Vec3 WorldPosition
		{
			get
			{
				if (!(this._siegeEngine != null))
				{
					return Vec3.One;
				}
				return this._siegeEngine.GlobalPosition;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0002EB84 File Offset: 0x0002CD84
		protected override float HeightOffset
		{
			get
			{
				return 2.5f;
			}
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x0002EB8C File Offset: 0x0002CD8C
		public MissionSiegeEngineMarkerTargetVM(SiegeWeapon siegeEngine)
			: base(MissionMarkerType.SiegeEngine)
		{
			this._siegeEngine = GameEntity.CreateFromWeakEntity(siegeEngine.GameEntity);
			this.Side = siegeEngine.Side;
			this.SiegeEngineID = siegeEngine.GetSiegeEngineType().StringId;
			uint num = ((this.Side == BattleSideEnum.Attacker) ? Mission.Current.AttackerTeam.Color : Mission.Current.DefenderTeam.Color);
			uint num2 = ((this.Side == BattleSideEnum.Attacker) ? Mission.Current.AttackerTeam.Color2 : Mission.Current.DefenderTeam.Color2);
			base.RefreshColor(num, num2);
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0002EC2A File Offset: 0x0002CE2A
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x0002EC32 File Offset: 0x0002CE32
		[DataSourceProperty]
		public string SiegeEngineID
		{
			get
			{
				return this._siegeEngineID;
			}
			set
			{
				if (value != this._siegeEngineID)
				{
					this._siegeEngineID = value;
					base.OnPropertyChangedWithValue<string>(value, "SiegeEngineID");
				}
			}
		}

		// Token: 0x040006FC RID: 1788
		private readonly GameEntity _siegeEngine;

		// Token: 0x040006FD RID: 1789
		public readonly BattleSideEnum Side;

		// Token: 0x040006FE RID: 1790
		private string _siegeEngineID;
	}
}
