using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x020001B8 RID: 440
	public abstract class MapWeatherModel : MBGameModel<MapWeatherModel>
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001DA7 RID: 7591
		public abstract CampaignTime WeatherUpdateFrequency { get; }

		// Token: 0x06001DA8 RID: 7592
		public abstract AtmosphereState GetInterpolatedAtmosphereState(CampaignTime timeOfYear, Vec3 pos);

		// Token: 0x06001DA9 RID: 7593
		public abstract AtmosphereInfo GetAtmosphereModel(CampaignVec2 position);

		// Token: 0x06001DAA RID: 7594
		public abstract void GetSeasonTimeFactorOfCampaignTime(CampaignTime ct, out float timeFactorForSnow, out float timeFactorForRain, bool snapCampaignTimeToWeatherPeriod = true);

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001DAB RID: 7595
		public abstract CampaignTime WeatherUpdatePeriod { get; }

		// Token: 0x06001DAC RID: 7596
		public abstract MapWeatherModel.WeatherEvent UpdateWeatherForPosition(CampaignVec2 position, CampaignTime ct);

		// Token: 0x06001DAD RID: 7597
		public abstract void InitializeCaches();

		// Token: 0x06001DAE RID: 7598
		public abstract MapWeatherModel.WeatherEvent GetWeatherEventInPosition(Vec2 pos);

		// Token: 0x06001DAF RID: 7599
		public abstract void GetSnowAndRainDataForPosition(Vec2 position, CampaignTime ct, out float snowValue, out float rainValue);

		// Token: 0x06001DB0 RID: 7600
		public abstract MapWeatherModel.WeatherEventEffectOnTerrain GetWeatherEffectOnTerrainForPosition(Vec2 pos);

		// Token: 0x06001DB1 RID: 7601
		public abstract Vec2 GetWindForPosition(CampaignVec2 position);

		// Token: 0x02000601 RID: 1537
		public enum WeatherEvent
		{
			// Token: 0x04001906 RID: 6406
			Clear,
			// Token: 0x04001907 RID: 6407
			LightRain,
			// Token: 0x04001908 RID: 6408
			HeavyRain,
			// Token: 0x04001909 RID: 6409
			Snowy,
			// Token: 0x0400190A RID: 6410
			Blizzard,
			// Token: 0x0400190B RID: 6411
			Storm
		}

		// Token: 0x02000602 RID: 1538
		public enum WeatherEventEffectOnTerrain
		{
			// Token: 0x0400190D RID: 6413
			Default,
			// Token: 0x0400190E RID: 6414
			Wet
		}
	}
}
