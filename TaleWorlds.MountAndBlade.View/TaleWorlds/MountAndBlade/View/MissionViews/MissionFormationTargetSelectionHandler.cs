using System;
using System.Collections.Generic;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000074 RID: 116
	public class MissionFormationTargetSelectionHandler : MissionView
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000455 RID: 1109 RVA: 0x000206EC File Offset: 0x0001E8EC
		// (remove) Token: 0x06000456 RID: 1110 RVA: 0x00020724 File Offset: 0x0001E924
		public event Action<MBReadOnlyList<Formation>> OnFormationFocused;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00020759 File Offset: 0x0001E959
		private Camera ActiveCamera
		{
			get
			{
				return base.MissionScreen.CustomCamera ?? base.MissionScreen.CombatCamera;
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00020778 File Offset: 0x0001E978
		public MissionFormationTargetSelectionHandler()
		{
			this._distanceCache = new List<ValueTuple<Formation, float>>();
			this._focusedFormationCache = new MBList<Formation>();
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000207DC File Offset: 0x0001E9DC
		public override void OnPreDisplayMissionTick(float dt)
		{
			base.OnPreDisplayMissionTick(dt);
			this._distanceCache.Clear();
			this._focusedFormationCache.Clear();
			Mission mission = base.Mission;
			if (((mission != null) ? mission.Teams : null) != null)
			{
				if (!this._isTargetingDisabled)
				{
					Vec3 position = this.ActiveCamera.Position;
					this._centerOfScreen.x = Screen.RealScreenResolutionWidth / 2f;
					this._centerOfScreen.y = Screen.RealScreenResolutionHeight / 2f;
					for (int i = 0; i < base.Mission.Teams.Count; i++)
					{
						Team team = base.Mission.Teams[i];
						if (!team.IsPlayerAlly)
						{
							for (int j = 0; j < team.FormationsIncludingEmpty.Count; j++)
							{
								Formation formation = team.FormationsIncludingEmpty[j];
								if (formation.CountOfUnits > 0)
								{
									bool flag;
									float num;
									this.TryGetFormationDistanceToCenter(formation, position, out flag, out num);
									if (flag)
									{
										this._distanceCache.Add(new ValueTuple<Formation, float>(formation, num));
									}
								}
							}
						}
					}
				}
				if (this._distanceCache.Count == 0)
				{
					Action<MBReadOnlyList<Formation>> onFormationFocused = this.OnFormationFocused;
					if (onFormationFocused == null)
					{
						return;
					}
					onFormationFocused(null);
					return;
				}
				else
				{
					Formation formation2 = null;
					float num2 = this.MaxDistanceToCenterForFocus;
					for (int k = 0; k < this._distanceCache.Count; k++)
					{
						ValueTuple<Formation, float> valueTuple = this._distanceCache[k];
						if (valueTuple.Item2 == 0f)
						{
							this._focusedFormationCache.Add(valueTuple.Item1);
						}
						else if (valueTuple.Item2 < num2)
						{
							num2 = valueTuple.Item2;
							formation2 = valueTuple.Item1;
						}
					}
					if (formation2 != null)
					{
						this._focusedFormationCache.Add(formation2);
					}
					Action<MBReadOnlyList<Formation>> onFormationFocused2 = this.OnFormationFocused;
					if (onFormationFocused2 == null)
					{
						return;
					}
					onFormationFocused2(this._focusedFormationCache);
				}
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000209A4 File Offset: 0x0001EBA4
		private void TryGetFormationDistanceToCenter(Formation formation, Vec3 cameraPosition, out bool isFormationFocusable, out float distanceToScreenCenter)
		{
			WorldPosition cachedMedianPosition = formation.CachedMedianPosition;
			float num = cachedMedianPosition.AsVec2.Distance(cameraPosition.AsVec2);
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(this.ActiveCamera, cachedMedianPosition.GetGroundVec3() + new Vec3(0f, 0f, 3f, -1f), ref num2, ref num3, ref num4);
			bool flag = num4 <= 0f;
			if (num >= 1000f)
			{
				distanceToScreenCenter = 2.1474836E+09f;
				isFormationFocusable = false;
				return;
			}
			if (num <= 10f)
			{
				isFormationFocusable = !flag;
				distanceToScreenCenter = 0f;
				return;
			}
			if (flag)
			{
				isFormationFocusable = false;
				distanceToScreenCenter = 2.1474836E+09f;
				return;
			}
			isFormationFocusable = true;
			distanceToScreenCenter = new Vec2(num2, num3).Distance(this._centerOfScreen);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00020A80 File Offset: 0x0001EC80
		public void SetIsFormationTargetingDisabled(bool isDisabled)
		{
			if (this._isTargetingDisabled != isDisabled)
			{
				this._isTargetingDisabled = isDisabled;
				if (isDisabled)
				{
					this._distanceCache.Clear();
					this._focusedFormationCache.Clear();
					Action<MBReadOnlyList<Formation>> onFormationFocused = this.OnFormationFocused;
					if (onFormationFocused == null)
					{
						return;
					}
					onFormationFocused(null);
				}
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00020ABC File Offset: 0x0001ECBC
		public override void OnRemoveBehavior()
		{
			this._distanceCache.Clear();
			this._focusedFormationCache.Clear();
			this.OnFormationFocused = null;
			base.OnRemoveBehavior();
		}

		// Token: 0x04000281 RID: 641
		public const float MaxDistanceForFocusCheck = 1000f;

		// Token: 0x04000282 RID: 642
		public const float MinDistanceForFocusCheck = 10f;

		// Token: 0x04000283 RID: 643
		public readonly float MaxDistanceToCenterForFocus = 70f * (Screen.RealScreenResolutionHeight / 1080f);

		// Token: 0x04000284 RID: 644
		private readonly List<ValueTuple<Formation, float>> _distanceCache;

		// Token: 0x04000285 RID: 645
		private readonly MBList<Formation> _focusedFormationCache;

		// Token: 0x04000286 RID: 646
		private Vec2 _centerOfScreen = new Vec2(Screen.RealScreenResolutionWidth / 2f, Screen.RealScreenResolutionHeight / 2f);

		// Token: 0x04000287 RID: 647
		private bool _isTargetingDisabled;
	}
}
