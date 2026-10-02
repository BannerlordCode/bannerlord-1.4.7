using System;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009D RID: 157
	public struct WeakMaterial
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x000114E6 File Offset: 0x0000F6E6
		// (set) Token: 0x06000EE0 RID: 3808 RVA: 0x000114EE File Offset: 0x0000F6EE
		public UIntPtr Pointer { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x000114F7 File Offset: 0x0000F6F7
		public bool IsValid
		{
			get
			{
				return this.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00011509 File Offset: 0x0000F709
		internal WeakMaterial(UIntPtr pointer)
		{
			this.Pointer = pointer;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00011512 File Offset: 0x0000F712
		public Shader GetShader()
		{
			return EngineApplicationInterface.IMaterial.GetShader(this.Pointer);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00011524 File Offset: 0x0000F724
		public ulong GetShaderFlags()
		{
			return EngineApplicationInterface.IMaterial.GetShaderFlags(this.Pointer);
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00011536 File Offset: 0x0000F736
		public void SetShaderFlags(ulong flagEntry)
		{
			EngineApplicationInterface.IMaterial.SetShaderFlags(this.Pointer, flagEntry);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00011549 File Offset: 0x0000F749
		public void SetMeshVectorArgument(float x, float y, float z, float w)
		{
			EngineApplicationInterface.IMaterial.SetMeshVectorArgument(this.Pointer, x, y, z, w);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00011560 File Offset: 0x0000F760
		public void SetTexture(Material.MBTextureType textureType, Texture texture)
		{
			EngineApplicationInterface.IMaterial.SetTexture(this.Pointer, (int)textureType, texture.Pointer);
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00011579 File Offset: 0x0000F779
		public void SetTextureAtSlot(int textureSlot, Texture texture)
		{
			EngineApplicationInterface.IMaterial.SetTextureAtSlot(this.Pointer, textureSlot, texture.Pointer);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00011592 File Offset: 0x0000F792
		public void SetAreaMapScale(float scale)
		{
			EngineApplicationInterface.IMaterial.SetAreaMapScale(this.Pointer, scale);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x000115A5 File Offset: 0x0000F7A5
		public void SetEnableSkinning(bool enable)
		{
			EngineApplicationInterface.IMaterial.SetEnableSkinning(this.Pointer, enable);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x000115B8 File Offset: 0x0000F7B8
		public bool UsingSkinning()
		{
			return EngineApplicationInterface.IMaterial.UsingSkinning(this.Pointer);
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x000115CA File Offset: 0x0000F7CA
		public Texture GetTexture(Material.MBTextureType textureType)
		{
			return EngineApplicationInterface.IMaterial.GetTexture(this.Pointer, (int)textureType);
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000115DD File Offset: 0x0000F7DD
		public Texture GetTextureWithSlot(int textureSlot)
		{
			return EngineApplicationInterface.IMaterial.GetTexture(this.Pointer, textureSlot);
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x000115F0 File Offset: 0x0000F7F0
		// (set) Token: 0x06000EEF RID: 3823 RVA: 0x00011602 File Offset: 0x0000F802
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IMaterial.GetName(this.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMaterial.SetName(this.Pointer, value);
			}
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x00011615 File Offset: 0x0000F815
		public void AddMaterialShaderFlag(string flagName, bool showErrors)
		{
			EngineApplicationInterface.IMaterial.AddMaterialShaderFlag(this.Pointer, flagName, showErrors);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x00011629 File Offset: 0x0000F829
		public void RemoveMaterialShaderFlag(string flagName)
		{
			EngineApplicationInterface.IMaterial.RemoveMaterialShaderFlag(this.Pointer, flagName);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0001163C File Offset: 0x0000F83C
		public static bool operator ==(WeakMaterial weakMaterial1, WeakMaterial weakMaterial2)
		{
			return weakMaterial1.Pointer == weakMaterial2.Pointer;
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00011651 File Offset: 0x0000F851
		public static bool operator !=(WeakMaterial weakMaterial1, WeakMaterial weakMaterial2)
		{
			return weakMaterial1.Pointer != weakMaterial2.Pointer;
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00011666 File Offset: 0x0000F866
		public override bool Equals(object obj)
		{
			return ((Material)obj).Pointer == this.Pointer;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00011680 File Offset: 0x0000F880
		public override int GetHashCode()
		{
			return this.Pointer.GetHashCode();
		}

		// Token: 0x04000205 RID: 517
		public static readonly WeakMaterial Invalid = new WeakMaterial(UIntPtr.Zero);
	}
}
