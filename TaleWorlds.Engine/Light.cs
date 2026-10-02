using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000055 RID: 85
	[EngineClass("rglLight")]
	public sealed class Light : GameEntityComponent
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00006E1B File Offset: 0x0000501B
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00006E2D File Offset: 0x0000502D
		internal Light(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00006E36 File Offset: 0x00005036
		public static Light CreatePointLight(float lightRadius)
		{
			return EngineApplicationInterface.ILight.CreatePointLight(lightRadius);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00006E44 File Offset: 0x00005044
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00006E64 File Offset: 0x00005064
		public MatrixFrame Frame
		{
			get
			{
				MatrixFrame matrixFrame;
				EngineApplicationInterface.ILight.GetFrame(base.Pointer, out matrixFrame);
				return matrixFrame;
			}
			set
			{
				EngineApplicationInterface.ILight.SetFrame(base.Pointer, ref value);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00006E78 File Offset: 0x00005078
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00006E8A File Offset: 0x0000508A
		public Vec3 LightColor
		{
			get
			{
				return EngineApplicationInterface.ILight.GetLightColor(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.SetLightColor(base.Pointer, value);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00006E9D File Offset: 0x0000509D
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00006EAF File Offset: 0x000050AF
		public float Intensity
		{
			get
			{
				return EngineApplicationInterface.ILight.GetIntensity(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.SetIntensity(base.Pointer, value);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00006EC2 File Offset: 0x000050C2
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00006ED4 File Offset: 0x000050D4
		public float Radius
		{
			get
			{
				return EngineApplicationInterface.ILight.GetRadius(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.SetRadius(base.Pointer, value);
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00006EE7 File Offset: 0x000050E7
		public void SetShadowType(Light.ShadowType type)
		{
			EngineApplicationInterface.ILight.SetShadows(base.Pointer, (int)type);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00006EFA File Offset: 0x000050FA
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x00006F0C File Offset: 0x0000510C
		public bool ShadowEnabled
		{
			get
			{
				return EngineApplicationInterface.ILight.IsShadowEnabled(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ILight.EnableShadow(base.Pointer, value);
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00006F1F File Offset: 0x0000511F
		public void SetLightFlicker(float magnitude, float interval)
		{
			EngineApplicationInterface.ILight.SetLightFlicker(base.Pointer, magnitude, interval);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00006F33 File Offset: 0x00005133
		public void SetVolumetricProperties(bool volumetricLightEnabled, float volumeParameters)
		{
			EngineApplicationInterface.ILight.SetVolumetricProperties(base.Pointer, volumetricLightEnabled, volumeParameters);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00006F47 File Offset: 0x00005147
		public void Dispose()
		{
			if (this.IsValid)
			{
				this.Release();
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00006F5D File Offset: 0x0000515D
		public void SetVisibility(bool value)
		{
			EngineApplicationInterface.ILight.SetVisibility(base.Pointer, value);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00006F70 File Offset: 0x00005170
		private void Release()
		{
			EngineApplicationInterface.ILight.Release(base.Pointer);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00006F84 File Offset: 0x00005184
		~Light()
		{
			this.Dispose();
		}

		// Token: 0x020000C2 RID: 194
		public enum ShadowType
		{
			// Token: 0x040003E7 RID: 999
			NoShadow,
			// Token: 0x040003E8 RID: 1000
			StaticShadow,
			// Token: 0x040003E9 RID: 1001
			DynamicShadow,
			// Token: 0x040003EA RID: 1002
			Count
		}
	}
}
