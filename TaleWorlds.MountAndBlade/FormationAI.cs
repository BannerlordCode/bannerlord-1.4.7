using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013B RID: 315
	public class FormationAI
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000F21 RID: 3873 RVA: 0x00028E40 File Offset: 0x00027040
		// (remove) Token: 0x06000F22 RID: 3874 RVA: 0x00028E78 File Offset: 0x00027078
		public event Action<Formation> OnActiveBehaviorChanged;

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x00028EAD File Offset: 0x000270AD
		// (set) Token: 0x06000F24 RID: 3876 RVA: 0x00028EB8 File Offset: 0x000270B8
		public BehaviorComponent ActiveBehavior
		{
			get
			{
				return this._activeBehavior;
			}
			private set
			{
				if (this._activeBehavior != value)
				{
					BehaviorComponent activeBehavior = this._activeBehavior;
					if (activeBehavior != null)
					{
						activeBehavior.OnBehaviorCanceled();
					}
					BehaviorComponent activeBehavior2 = this._activeBehavior;
					this._activeBehavior = value;
					this._activeBehavior.OnBehaviorActivated();
					this.ActiveBehavior.PreserveExpireTime = Mission.Current.CurrentTime + 10f;
					if (this.OnActiveBehaviorChanged != null && (activeBehavior2 == null || !activeBehavior2.Equals(value)))
					{
						this.OnActiveBehaviorChanged(this._formation);
					}
				}
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00028F38 File Offset: 0x00027138
		// (set) Token: 0x06000F26 RID: 3878 RVA: 0x00028F40 File Offset: 0x00027140
		public FormationAI.BehaviorSide Side
		{
			get
			{
				return this._side;
			}
			set
			{
				if (this._side != value)
				{
					this._side = value;
					if (this._side != FormationAI.BehaviorSide.BehaviorSideNotSet)
					{
						foreach (BehaviorComponent behaviorComponent in this._behaviors)
						{
							behaviorComponent.OnValidBehaviorSideChanged();
						}
					}
				}
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x00028FAC File Offset: 0x000271AC
		// (set) Token: 0x06000F28 RID: 3880 RVA: 0x00028FB4 File Offset: 0x000271B4
		public bool IsMainFormation { get; set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00028FBD File Offset: 0x000271BD
		public int BehaviorCount
		{
			get
			{
				return this._behaviors.Count;
			}
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00028FCC File Offset: 0x000271CC
		public FormationAI(Formation formation)
		{
			this._formation = formation;
			float num = 0f;
			if (formation.Team != null)
			{
				float num2 = 0.1f * (float)formation.FormationIndex;
				float num3 = 0f;
				if (formation.Team.TeamIndex >= 0)
				{
					num3 = (float)formation.Team.TeamIndex * 0.5f * 0.1f;
				}
				num = num2 + num3;
			}
			this._tickTimer = new Timer(Mission.Current.CurrentTime + 0.5f * num, 0.5f, true);
			this._specialBehaviorData = new List<FormationAI.BehaviorData>();
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00029074 File Offset: 0x00027274
		public T SetBehaviorWeight<T>(float w) where T : BehaviorComponent
		{
			using (List<BehaviorComponent>.Enumerator enumerator = this._behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null)
					{
						t.WeightFactor = w;
						return t;
					}
				}
			}
			throw new MBException("Behavior weight could not be set.");
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x000290F0 File Offset: 0x000272F0
		public void AddAiBehavior(BehaviorComponent behaviorComponent)
		{
			this._behaviors.Add(behaviorComponent);
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00029100 File Offset: 0x00027300
		public T GetBehavior<T>() where T : BehaviorComponent
		{
			using (List<BehaviorComponent>.Enumerator enumerator = this._behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null)
					{
						return t;
					}
				}
			}
			using (List<FormationAI.BehaviorData>.Enumerator enumerator2 = this._specialBehaviorData.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					T t2;
					if ((t2 = enumerator2.Current.Behavior as T) != null)
					{
						return t2;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x000291C8 File Offset: 0x000273C8
		public void AddSpecialBehavior(BehaviorComponent behavior, bool purgePreviousSpecialBehaviors = false)
		{
			if (purgePreviousSpecialBehaviors)
			{
				this._specialBehaviorData.Clear();
			}
			this._specialBehaviorData.Add(new FormationAI.BehaviorData
			{
				Behavior = behavior
			});
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x000291F0 File Offset: 0x000273F0
		private bool FindBestBehavior()
		{
			BehaviorComponent behaviorComponent = null;
			float num = float.MinValue;
			foreach (BehaviorComponent behaviorComponent2 in this._behaviors)
			{
				if (behaviorComponent2.WeightFactor > 1E-07f)
				{
					float num2 = behaviorComponent2.GetAIWeight() * behaviorComponent2.WeightFactor;
					if (behaviorComponent2 == this.ActiveBehavior)
					{
						num2 *= MBMath.Lerp(1.2f, 2f, MBMath.ClampFloat((behaviorComponent2.PreserveExpireTime - Mission.Current.CurrentTime) / 5f, 0f, 1f), float.MinValue);
					}
					if (num2 > num)
					{
						if (behaviorComponent2.NavmeshlessTargetPositionPenalty > 0f)
						{
							num2 /= behaviorComponent2.NavmeshlessTargetPositionPenalty;
						}
						behaviorComponent2.PrecalculateMovementOrder();
						num2 *= behaviorComponent2.NavmeshlessTargetPositionPenalty;
						if (num2 > num)
						{
							behaviorComponent = behaviorComponent2;
							num = num2;
						}
					}
				}
			}
			if (behaviorComponent != null)
			{
				this.ActiveBehavior = behaviorComponent;
				if (behaviorComponent != this._behaviors[0])
				{
					this._behaviors.Remove(behaviorComponent);
					this._behaviors.Insert(0, behaviorComponent);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00029320 File Offset: 0x00027520
		private void PreprocessBehaviors()
		{
			if (this._formation.HasAnyEnemyFormationsThatIsNotEmpty())
			{
				FormationAI.BehaviorData behaviorData = this._specialBehaviorData.FirstOrDefault<FormationAI.BehaviorData>((FormationAI.BehaviorData sd) => !sd.IsPreprocessed);
				if (behaviorData != null)
				{
					behaviorData.Behavior.TickOccasionally();
					float num = behaviorData.Behavior.GetAIWeight();
					if (behaviorData.Behavior == this.ActiveBehavior)
					{
						num *= MBMath.Lerp(1.01f, 1.5f, MBMath.ClampFloat((behaviorData.Behavior.PreserveExpireTime - Mission.Current.CurrentTime) / 5f, 0f, 1f), float.MinValue);
					}
					behaviorData.Weight = num * behaviorData.Preference;
					behaviorData.IsPreprocessed = true;
				}
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x000293E8 File Offset: 0x000275E8
		public void Tick()
		{
			if (Mission.Current.AllowAiTicking && (Mission.Current.ForceTickOccasionally || this._tickTimer.Check(Mission.Current.CurrentTime)))
			{
				this.TickOccasionally(this._tickTimer.PreviousDeltaTime);
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00029438 File Offset: 0x00027638
		private void TickOccasionally(float dt)
		{
			this._formation.IsAITickedAfterSplit = true;
			if (this.FindBestBehavior())
			{
				if (!this._formation.IsAIControlled)
				{
					if (GameNetwork.IsMultiplayer && Mission.Current.MainAgent != null && !this._formation.Team.IsPlayerGeneral && this._formation.Team.IsPlayerSergeant && this._formation.PlayerOwner == Agent.Main)
					{
						this.ActiveBehavior.RemindSergeantPlayer();
						return;
					}
				}
				else
				{
					this.ActiveBehavior.TickOccasionally();
				}
				return;
			}
			BehaviorComponent behaviorComponent = this.ActiveBehavior;
			if (this._formation.HasAnyEnemyFormationsThatIsNotEmpty())
			{
				this.PreprocessBehaviors();
				foreach (FormationAI.BehaviorData behaviorData in this._specialBehaviorData)
				{
					behaviorData.IsPreprocessed = false;
				}
				if (behaviorComponent is BehaviorStop && this._specialBehaviorData.Count > 0)
				{
					IEnumerable<FormationAI.BehaviorData> enumerable = this._specialBehaviorData.Where<FormationAI.BehaviorData>((FormationAI.BehaviorData sbd) => sbd.Weight > 0f);
					if (enumerable.Any<FormationAI.BehaviorData>())
					{
						behaviorComponent = enumerable.MaxBy<FormationAI.BehaviorData, float>((FormationAI.BehaviorData abd) => abd.Weight).Behavior;
					}
				}
				bool isAIControlled = this._formation.IsAIControlled;
				bool flag = false;
				if (this.ActiveBehavior != behaviorComponent)
				{
					BehaviorComponent activeBehavior = this.ActiveBehavior;
					this.ActiveBehavior = behaviorComponent;
					flag = true;
				}
				if (flag || (behaviorComponent != null && behaviorComponent.IsCurrentOrderChanged))
				{
					if (this._formation.IsAIControlled)
					{
						this._formation.SetMovementOrder(behaviorComponent.CurrentOrder);
					}
					behaviorComponent.IsCurrentOrderChanged = false;
				}
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x000295F8 File Offset: 0x000277F8
		public void OnDeploymentFinished()
		{
			foreach (BehaviorComponent behaviorComponent in this._behaviors)
			{
				behaviorComponent.OnDeploymentFinished();
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00029648 File Offset: 0x00027848
		public void OnAgentRemoved(Agent agent)
		{
			foreach (BehaviorComponent behaviorComponent in this._behaviors)
			{
				behaviorComponent.OnAgentRemoved(agent);
			}
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0002969C File Offset: 0x0002789C
		public BehaviorComponent GetBehaviorAtIndex(int index)
		{
			if (index >= 0 && index < this._behaviors.Count)
			{
				return this._behaviors[index];
			}
			return null;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x000296C0 File Offset: 0x000278C0
		[Conditional("DEBUG")]
		public void DebugMore()
		{
			if (!MBDebug.IsDisplayingHighLevelAI)
			{
				return;
			}
			foreach (FormationAI.BehaviorData behaviorData in this._specialBehaviorData.OrderBy<FormationAI.BehaviorData, string>((FormationAI.BehaviorData d) => d.Behavior.GetType().ToString()))
			{
				behaviorData.Behavior.GetType().ToString().Replace("MBModule.Behavior", "");
				behaviorData.Weight.ToString("0.00");
				behaviorData.Preference.ToString("0.00");
			}
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00029774 File Offset: 0x00027974
		[Conditional("DEBUG")]
		public void DebugScores()
		{
			if (this._formation.PhysicalClass.IsRanged())
			{
				MBDebug.Print("Ranged", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			else if (this._formation.PhysicalClass.IsMeleeCavalry())
			{
				MBDebug.Print("Cavalry", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			else
			{
				MBDebug.Print("Infantry", 0, Debug.DebugColor.White, 17592186044416UL);
			}
			foreach (FormationAI.BehaviorData behaviorData in this._specialBehaviorData.OrderBy<FormationAI.BehaviorData, string>((FormationAI.BehaviorData d) => d.Behavior.GetType().ToString()))
			{
				string text = behaviorData.Behavior.GetType().ToString().Replace("MBModule.Behavior", "");
				string text2 = behaviorData.Weight.ToString("0.00");
				string text3 = behaviorData.Preference.ToString("0.00");
				MBDebug.Print(string.Concat(new string[] { text, " \t\t w:", text2, "\t p:", text3 }), 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000298C4 File Offset: 0x00027AC4
		public void ResetBehaviorWeights()
		{
			foreach (BehaviorComponent behaviorComponent in this._behaviors)
			{
				behaviorComponent.ResetBehavior();
			}
		}

		// Token: 0x040003BC RID: 956
		private const float BehaviorPreserveTime = 5f;

		// Token: 0x040003BE RID: 958
		private readonly Formation _formation;

		// Token: 0x040003BF RID: 959
		private readonly List<FormationAI.BehaviorData> _specialBehaviorData;

		// Token: 0x040003C0 RID: 960
		private readonly List<BehaviorComponent> _behaviors = new List<BehaviorComponent>();

		// Token: 0x040003C1 RID: 961
		private BehaviorComponent _activeBehavior;

		// Token: 0x040003C2 RID: 962
		private FormationAI.BehaviorSide _side = FormationAI.BehaviorSide.Middle;

		// Token: 0x040003C3 RID: 963
		private readonly Timer _tickTimer;

		// Token: 0x02000453 RID: 1107
		public class BehaviorData
		{
			// Token: 0x040019BB RID: 6587
			public BehaviorComponent Behavior;

			// Token: 0x040019BC RID: 6588
			public float Preference = 1f;

			// Token: 0x040019BD RID: 6589
			public float Weight;

			// Token: 0x040019BE RID: 6590
			public bool IsRemovedOnCancel;

			// Token: 0x040019BF RID: 6591
			public bool IsPreprocessed;
		}

		// Token: 0x02000454 RID: 1108
		public enum BehaviorSide
		{
			// Token: 0x040019C1 RID: 6593
			Left,
			// Token: 0x040019C2 RID: 6594
			Middle,
			// Token: 0x040019C3 RID: 6595
			Right,
			// Token: 0x040019C4 RID: 6596
			BehaviorSideNotSet,
			// Token: 0x040019C5 RID: 6597
			ValidBehaviorSideCount = 3
		}
	}
}
