using System;
using System.Runtime.InteropServices;
using TaleWorlds.Core;
using TaleWorlds.DotNet;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021C RID: 540
	[EngineStruct("Skin_generation_params", false, null)]
	public struct SkinGenerationParams
	{
		// Token: 0x06001F64 RID: 8036 RVA: 0x0006C8D4 File Offset: 0x0006AAD4
		public static SkinGenerationParams Create()
		{
			SkinGenerationParams skinGenerationParams;
			skinGenerationParams._skinMeshesVisibilityMask = 481;
			skinGenerationParams._underwearType = Equipment.UnderwearTypes.FullUnderwear;
			skinGenerationParams._bodyMeshType = 0;
			skinGenerationParams._hairCoverType = 0;
			skinGenerationParams._beardCoverType = 0;
			skinGenerationParams._prepareImmediately = false;
			skinGenerationParams._bodyDeformType = -1;
			skinGenerationParams._faceDirtAmount = 0f;
			skinGenerationParams._gender = 0;
			skinGenerationParams._race = 0;
			skinGenerationParams._useTranslucency = false;
			skinGenerationParams._useTesselation = false;
			skinGenerationParams._faceCacheId = 0;
			return skinGenerationParams;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0006C954 File Offset: 0x0006AB54
		public SkinGenerationParams(int skinMeshesVisibilityMask, Equipment.UnderwearTypes underwearType, int bodyMeshType, int hairCoverType, int beardCoverType, int bodyDeformType, bool prepareImmediately, float faceDirtAmount, int gender, int race, bool useTranslucency, bool useTesselation, int faceCacheID)
		{
			this._skinMeshesVisibilityMask = skinMeshesVisibilityMask;
			this._underwearType = underwearType;
			this._bodyMeshType = bodyMeshType;
			this._hairCoverType = hairCoverType;
			this._beardCoverType = beardCoverType;
			this._bodyDeformType = bodyDeformType;
			this._prepareImmediately = prepareImmediately;
			this._faceDirtAmount = faceDirtAmount;
			this._gender = gender;
			this._race = race;
			this._useTranslucency = useTranslucency;
			this._useTesselation = useTesselation;
			this._faceCacheId = faceCacheID;
		}

		// Token: 0x04000AAC RID: 2732
		public int _skinMeshesVisibilityMask;

		// Token: 0x04000AAD RID: 2733
		public Equipment.UnderwearTypes _underwearType;

		// Token: 0x04000AAE RID: 2734
		public int _bodyMeshType;

		// Token: 0x04000AAF RID: 2735
		public int _hairCoverType;

		// Token: 0x04000AB0 RID: 2736
		public int _beardCoverType;

		// Token: 0x04000AB1 RID: 2737
		public int _bodyDeformType;

		// Token: 0x04000AB2 RID: 2738
		[MarshalAs(UnmanagedType.U1)]
		public bool _prepareImmediately;

		// Token: 0x04000AB3 RID: 2739
		[MarshalAs(UnmanagedType.U1)]
		public bool _useTranslucency;

		// Token: 0x04000AB4 RID: 2740
		[MarshalAs(UnmanagedType.U1)]
		public bool _useTesselation;

		// Token: 0x04000AB5 RID: 2741
		public float _faceDirtAmount;

		// Token: 0x04000AB6 RID: 2742
		public int _gender;

		// Token: 0x04000AB7 RID: 2743
		public int _race;

		// Token: 0x04000AB8 RID: 2744
		public int _faceCacheId;
	}
}
