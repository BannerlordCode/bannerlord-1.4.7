using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001E RID: 30
	internal class ScriptingInterfaceOfIMBSkeletonExtensions : IMBSkeletonExtensions
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public Skeleton CreateAgentSkeleton(string skeletonName, bool isHumanoid, int actionSetIndex, string monsterUsageSetName, ref AnimationSystemData animationSystemData)
		{
			byte[] array = null;
			if (skeletonName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(skeletonName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(skeletonName, 0, skeletonName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (monsterUsageSetName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(monsterUsageSetName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(monsterUsageSetName, 0, monsterUsageSetName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateAgentSkeletonDelegate(array, isHumanoid, actionSetIndex, array2, ref animationSystemData);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000D380 File Offset: 0x0000B580
		public Skeleton CreateSimpleSkeleton(string skeletonName)
		{
			byte[] array = null;
			if (skeletonName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(skeletonName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(skeletonName, 0, skeletonName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateSimpleSkeletonDelegate(array);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000D40C File Offset: 0x0000B60C
		public Skeleton CreateWithActionSet(ref AnimationSystemData animationSystemData)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateWithActionSetDelegate(ref animationSystemData);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000D456 File Offset: 0x0000B656
		public bool DoesActionContinueWithCurrentActionAtChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBSkeletonExtensions.call_DoesActionContinueWithCurrentActionAtChannelDelegate(skeletonPointer, actionChannelNo, actionIndex);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000D465 File Offset: 0x0000B665
		public int GetActionAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfIMBSkeletonExtensions.call_GetActionAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000D473 File Offset: 0x0000B673
		public void GetBoneEntitialFrame(UIntPtr skeletonPointer, sbyte bone, bool useBoneMapping, bool forceToUpdate, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_GetBoneEntitialFrameDelegate(skeletonPointer, bone, useBoneMapping, forceToUpdate, ref outFrame);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000D486 File Offset: 0x0000B686
		public void GetBoneEntitialFrameAtAnimationProgress(UIntPtr skeletonPointer, sbyte boneIndex, int animationIndex, float progress, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_GetBoneEntitialFrameAtAnimationProgressDelegate(skeletonPointer, boneIndex, animationIndex, progress, ref outFrame);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000D499 File Offset: 0x0000B699
		public string GetSkeletonFaceAnimationName(UIntPtr entityId)
		{
			if (ScriptingInterfaceOfIMBSkeletonExtensions.call_GetSkeletonFaceAnimationNameDelegate(entityId) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000D4B0 File Offset: 0x0000B6B0
		public float GetSkeletonFaceAnimationTime(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIMBSkeletonExtensions.call_GetSkeletonFaceAnimationTimeDelegate(entityId);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000D4BD File Offset: 0x0000B6BD
		public void SetAgentActionChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex, float channelParameter, float blendPeriodOverride, bool forceFaceMorphRestart, float blendWithNextActionFactor)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetAgentActionChannelDelegate(skeletonPointer, actionChannelNo, actionIndex, channelParameter, blendPeriodOverride, forceFaceMorphRestart, blendWithNextActionFactor);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000D4D4 File Offset: 0x0000B6D4
		public void SetAnimationAtChannel(UIntPtr skeletonPointer, int animationIndex, int channelNo, float animationSpeedMultiplier, float blendInPeriod, float startProgress)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetAnimationAtChannelDelegate(skeletonPointer, animationIndex, channelNo, animationSpeedMultiplier, blendInPeriod, startProgress);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000D4EC File Offset: 0x0000B6EC
		public void SetFacialAnimationOfChannel(UIntPtr skeletonPointer, int channel, string facialAnimationName, bool playSound, bool loop)
		{
			byte[] array = null;
			if (facialAnimationName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(facialAnimationName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(facialAnimationName, 0, facialAnimationName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetFacialAnimationOfChannelDelegate(skeletonPointer, channel, array, playSound, loop);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000D54C File Offset: 0x0000B74C
		public void SetSkeletonFaceAnimationTime(UIntPtr entityId, float time)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetSkeletonFaceAnimationTimeDelegate(entityId, time);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000D55A File Offset: 0x0000B75A
		public void TickActionChannels(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_TickActionChannelsDelegate(skeletonPointer);
		}

		// Token: 0x040002B2 RID: 690
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002B3 RID: 691
		public static ScriptingInterfaceOfIMBSkeletonExtensions.CreateAgentSkeletonDelegate call_CreateAgentSkeletonDelegate;

		// Token: 0x040002B4 RID: 692
		public static ScriptingInterfaceOfIMBSkeletonExtensions.CreateSimpleSkeletonDelegate call_CreateSimpleSkeletonDelegate;

		// Token: 0x040002B5 RID: 693
		public static ScriptingInterfaceOfIMBSkeletonExtensions.CreateWithActionSetDelegate call_CreateWithActionSetDelegate;

		// Token: 0x040002B6 RID: 694
		public static ScriptingInterfaceOfIMBSkeletonExtensions.DoesActionContinueWithCurrentActionAtChannelDelegate call_DoesActionContinueWithCurrentActionAtChannelDelegate;

		// Token: 0x040002B7 RID: 695
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetActionAtChannelDelegate call_GetActionAtChannelDelegate;

		// Token: 0x040002B8 RID: 696
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameDelegate call_GetBoneEntitialFrameDelegate;

		// Token: 0x040002B9 RID: 697
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameAtAnimationProgressDelegate call_GetBoneEntitialFrameAtAnimationProgressDelegate;

		// Token: 0x040002BA RID: 698
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationNameDelegate call_GetSkeletonFaceAnimationNameDelegate;

		// Token: 0x040002BB RID: 699
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationTimeDelegate call_GetSkeletonFaceAnimationTimeDelegate;

		// Token: 0x040002BC RID: 700
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetAgentActionChannelDelegate call_SetAgentActionChannelDelegate;

		// Token: 0x040002BD RID: 701
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetAnimationAtChannelDelegate call_SetAnimationAtChannelDelegate;

		// Token: 0x040002BE RID: 702
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetFacialAnimationOfChannelDelegate call_SetFacialAnimationOfChannelDelegate;

		// Token: 0x040002BF RID: 703
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetSkeletonFaceAnimationTimeDelegate call_SetSkeletonFaceAnimationTimeDelegate;

		// Token: 0x040002C0 RID: 704
		public static ScriptingInterfaceOfIMBSkeletonExtensions.TickActionChannelsDelegate call_TickActionChannelsDelegate;

		// Token: 0x0200030E RID: 782
		// (Invoke) Token: 0x06000F26 RID: 3878
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateAgentSkeletonDelegate(byte[] skeletonName, [MarshalAs(UnmanagedType.U1)] bool isHumanoid, int actionSetIndex, byte[] monsterUsageSetName, ref AnimationSystemData animationSystemData);

		// Token: 0x0200030F RID: 783
		// (Invoke) Token: 0x06000F2A RID: 3882
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateSimpleSkeletonDelegate(byte[] skeletonName);

		// Token: 0x02000310 RID: 784
		// (Invoke) Token: 0x06000F2E RID: 3886
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateWithActionSetDelegate(ref AnimationSystemData animationSystemData);

		// Token: 0x02000311 RID: 785
		// (Invoke) Token: 0x06000F32 RID: 3890
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool DoesActionContinueWithCurrentActionAtChannelDelegate(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex);

		// Token: 0x02000312 RID: 786
		// (Invoke) Token: 0x06000F36 RID: 3894
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetActionAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x02000313 RID: 787
		// (Invoke) Token: 0x06000F3A RID: 3898
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameDelegate(UIntPtr skeletonPointer, sbyte bone, [MarshalAs(UnmanagedType.U1)] bool useBoneMapping, [MarshalAs(UnmanagedType.U1)] bool forceToUpdate, ref MatrixFrame outFrame);

		// Token: 0x02000314 RID: 788
		// (Invoke) Token: 0x06000F3E RID: 3902
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameAtAnimationProgressDelegate(UIntPtr skeletonPointer, sbyte boneIndex, int animationIndex, float progress, ref MatrixFrame outFrame);

		// Token: 0x02000315 RID: 789
		// (Invoke) Token: 0x06000F42 RID: 3906
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSkeletonFaceAnimationNameDelegate(UIntPtr entityId);

		// Token: 0x02000316 RID: 790
		// (Invoke) Token: 0x06000F46 RID: 3910
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetSkeletonFaceAnimationTimeDelegate(UIntPtr entityId);

		// Token: 0x02000317 RID: 791
		// (Invoke) Token: 0x06000F4A RID: 3914
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAgentActionChannelDelegate(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex, float channelParameter, float blendPeriodOverride, [MarshalAs(UnmanagedType.U1)] bool forceFaceMorphRestart, float blendWithNextActionFactor);

		// Token: 0x02000318 RID: 792
		// (Invoke) Token: 0x06000F4E RID: 3918
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAnimationAtChannelDelegate(UIntPtr skeletonPointer, int animationIndex, int channelNo, float animationSpeedMultiplier, float blendInPeriod, float startProgress);

		// Token: 0x02000319 RID: 793
		// (Invoke) Token: 0x06000F52 RID: 3922
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFacialAnimationOfChannelDelegate(UIntPtr skeletonPointer, int channel, byte[] facialAnimationName, [MarshalAs(UnmanagedType.U1)] bool playSound, [MarshalAs(UnmanagedType.U1)] bool loop);

		// Token: 0x0200031A RID: 794
		// (Invoke) Token: 0x06000F56 RID: 3926
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonFaceAnimationTimeDelegate(UIntPtr entityId, float time);

		// Token: 0x0200031B RID: 795
		// (Invoke) Token: 0x06000F5A RID: 3930
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickActionChannelsDelegate(UIntPtr skeletonPointer);
	}
}
