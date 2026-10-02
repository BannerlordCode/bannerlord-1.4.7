using System;
using SandBox.View.Map.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map.Visuals
{
	// Token: 0x02000062 RID: 98
	public class MapWeatherVisual : MapEntityVisual<WeatherNode>
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0001DFE5 File Offset: 0x0001C1E5
		public Vec2 Position
		{
			get
			{
				return base.MapEntity.Position.ToVec2();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0001DFF8 File Offset: 0x0001C1F8
		public Vec2 PrefabSpawnOffset
		{
			get
			{
				Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
				float num = terrainSize.X / (float)Campaign.Current.DefaultWeatherNodeDimension;
				float num2 = terrainSize.Y / (float)Campaign.Current.DefaultWeatherNodeDimension;
				return new Vec2(num * 0.5f, num2 * 0.5f);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060003DF RID: 991 RVA: 0x0001E050 File Offset: 0x0001C250
		public int MaskPixelIndex
		{
			get
			{
				if (this._maskPixelIndex == -1)
				{
					Vec2 terrainSize = Campaign.Current.MapSceneWrapper.GetTerrainSize();
					float num = terrainSize.X / (float)Campaign.Current.DefaultWeatherNodeDimension;
					float num2 = terrainSize.Y / (float)Campaign.Current.DefaultWeatherNodeDimension;
					int num3 = (int)(this.Position.X / num);
					int num4 = (int)(this.Position.Y / num2);
					this._maskPixelIndex = num4 * Campaign.Current.DefaultWeatherNodeDimension + num3;
				}
				return this._maskPixelIndex;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x0001E0E0 File Offset: 0x0001C2E0
		public override CampaignVec2 InteractionPositionForPlayer
		{
			get
			{
				return new CampaignVec2(this.Position, true);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001E0EE File Offset: 0x0001C2EE
		public override MapEntityVisual AttachedTo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001E0F4 File Offset: 0x0001C2F4
		public override string ToString()
		{
			return this.Position.ToString();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001E115 File Offset: 0x0001C315
		public MapWeatherVisual(WeatherNode weatherNode)
			: base(weatherNode)
		{
			this._previousWeatherEvent = MapWeatherModel.WeatherEvent.Clear;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001E12C File Offset: 0x0001C32C
		public void Tick()
		{
			if (base.MapEntity.IsVisuallyDirty)
			{
				bool flag = this._previousWeatherEvent == MapWeatherModel.WeatherEvent.HeavyRain;
				bool flag2 = this._previousWeatherEvent == MapWeatherModel.WeatherEvent.Blizzard;
				MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(this.Position);
				bool flag3 = weatherEventInPosition == MapWeatherModel.WeatherEvent.HeavyRain;
				bool flag4 = Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(this.Position) == MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
				bool flag5 = weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard;
				byte b = (flag4 ? 125 : (flag3 ? 200 : 0));
				byte b2 = (byte)Math.Max((int)b, flag5 ? 200 : 0);
				MapWeatherVisualManager.Current.SetRainData(this.MaskPixelIndex, b);
				MapWeatherVisualManager.Current.SetCloudData(this.MaskPixelIndex, b2);
				if (this.Prefab == null)
				{
					if (flag3)
					{
						this.AttachNewRainPrefabToVisual();
					}
					else if (flag5)
					{
						this.AttachNewBlizzardPrefabToVisual();
					}
					else if (MBRandom.RandomFloat < 0.1f)
					{
						MapWeatherVisualManager.Current.SetCloudData(this.MaskPixelIndex, 200);
					}
				}
				else
				{
					if (flag && !flag3 && flag5)
					{
						MapWeatherVisualManager.Current.ReleaseRainPrefab(this.Prefab);
						this.AttachNewBlizzardPrefabToVisual();
					}
					else if (flag2 && !flag5 && flag3)
					{
						MapWeatherVisualManager.Current.ReleaseBlizzardPrefab(this.Prefab);
						this.AttachNewRainPrefabToVisual();
					}
					if (!flag3 && !flag5)
					{
						if (flag)
						{
							MapWeatherVisualManager.Current.ReleaseRainPrefab(this.Prefab);
						}
						else if (flag2)
						{
							MapWeatherVisualManager.Current.ReleaseBlizzardPrefab(this.Prefab);
						}
						this.Prefab = null;
					}
				}
				this._previousWeatherEvent = weatherEventInPosition;
				base.MapEntity.OnVisualUpdated();
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001E2D8 File Offset: 0x0001C4D8
		private void AttachNewRainPrefabToVisual()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = new Vec3(this.Position + this.PrefabSpawnOffset, 26f, -1f);
			GameEntity rainPrefabFromPool = MapWeatherVisualManager.Current.GetRainPrefabFromPool();
			rainPrefabFromPool.SetVisibilityExcludeParents(true);
			rainPrefabFromPool.SetGlobalFrame(in identity, true);
			this.Prefab = rainPrefabFromPool;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001E334 File Offset: 0x0001C534
		private void AttachNewBlizzardPrefabToVisual()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = new Vec3(this.Position + this.PrefabSpawnOffset, 26f, -1f);
			GameEntity blizzardPrefabFromPool = MapWeatherVisualManager.Current.GetBlizzardPrefabFromPool();
			blizzardPrefabFromPool.SetVisibilityExcludeParents(true);
			blizzardPrefabFromPool.SetGlobalFrame(in identity, true);
			this.Prefab = blizzardPrefabFromPool;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001E390 File Offset: 0x0001C590
		public override bool OnMapClick(bool followModifierUsed)
		{
			return false;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001E393 File Offset: 0x0001C593
		public override void OnHover()
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001E395 File Offset: 0x0001C595
		public override void OnOpenEncyclopedia()
		{
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001E397 File Offset: 0x0001C597
		public override bool IsVisibleOrFadingOut()
		{
			return false;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001E39C File Offset: 0x0001C59C
		public override Vec3 GetVisualPosition()
		{
			return this.InteractionPositionForPlayer.AsVec3();
		}

		// Token: 0x040001ED RID: 493
		public GameEntity Prefab;

		// Token: 0x040001EE RID: 494
		private MapWeatherModel.WeatherEvent _previousWeatherEvent;

		// Token: 0x040001EF RID: 495
		private int _maskPixelIndex = -1;
	}
}
