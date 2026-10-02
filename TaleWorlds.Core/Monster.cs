using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000BD RID: 189
	public sealed class Monster : MBObjectBase
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060009FC RID: 2556 RVA: 0x00020BA7 File Offset: 0x0001EDA7
		// (set) Token: 0x060009FD RID: 2557 RVA: 0x00020BAF File Offset: 0x0001EDAF
		public string BaseMonster { get; private set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x00020BB8 File Offset: 0x0001EDB8
		// (set) Token: 0x060009FF RID: 2559 RVA: 0x00020BC0 File Offset: 0x0001EDC0
		public float BodyCapsuleRadius { get; private set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00020BC9 File Offset: 0x0001EDC9
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x00020BD1 File Offset: 0x0001EDD1
		public Vec3 BodyCapsulePoint1 { get; private set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00020BDA File Offset: 0x0001EDDA
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x00020BE2 File Offset: 0x0001EDE2
		public Vec3 BodyCapsulePoint2 { get; private set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00020BEB File Offset: 0x0001EDEB
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00020BF3 File Offset: 0x0001EDF3
		public float CrouchedBodyCapsuleRadius { get; private set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00020BFC File Offset: 0x0001EDFC
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x00020C04 File Offset: 0x0001EE04
		public Vec3 CrouchedBodyCapsulePoint1 { get; private set; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00020C0D File Offset: 0x0001EE0D
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x00020C15 File Offset: 0x0001EE15
		public Vec3 CrouchedBodyCapsulePoint2 { get; private set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00020C1E File Offset: 0x0001EE1E
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x00020C26 File Offset: 0x0001EE26
		public AgentFlag Flags { get; private set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x00020C2F File Offset: 0x0001EE2F
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x00020C37 File Offset: 0x0001EE37
		public int Weight { get; private set; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00020C40 File Offset: 0x0001EE40
		// (set) Token: 0x06000A0F RID: 2575 RVA: 0x00020C48 File Offset: 0x0001EE48
		public int HitPoints { get; private set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00020C51 File Offset: 0x0001EE51
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x00020C59 File Offset: 0x0001EE59
		public string ActionSetCode { get; private set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00020C62 File Offset: 0x0001EE62
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x00020C6A File Offset: 0x0001EE6A
		public string FemaleActionSetCode { get; private set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x00020C73 File Offset: 0x0001EE73
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x00020C7B File Offset: 0x0001EE7B
		public int NumPaces { get; private set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00020C84 File Offset: 0x0001EE84
		// (set) Token: 0x06000A17 RID: 2583 RVA: 0x00020C8C File Offset: 0x0001EE8C
		public string MonsterUsage { get; private set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x00020C95 File Offset: 0x0001EE95
		// (set) Token: 0x06000A19 RID: 2585 RVA: 0x00020C9D File Offset: 0x0001EE9D
		public float WalkingSpeedLimit { get; private set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x00020CA6 File Offset: 0x0001EEA6
		// (set) Token: 0x06000A1B RID: 2587 RVA: 0x00020CAE File Offset: 0x0001EEAE
		public float CrouchWalkingSpeedLimit { get; private set; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x00020CB7 File Offset: 0x0001EEB7
		// (set) Token: 0x06000A1D RID: 2589 RVA: 0x00020CBF File Offset: 0x0001EEBF
		public float JumpAcceleration { get; private set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00020CC8 File Offset: 0x0001EEC8
		// (set) Token: 0x06000A1F RID: 2591 RVA: 0x00020CD0 File Offset: 0x0001EED0
		public float AbsorbedDamageRatio { get; private set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x00020CD9 File Offset: 0x0001EED9
		// (set) Token: 0x06000A21 RID: 2593 RVA: 0x00020CE1 File Offset: 0x0001EEE1
		public string SoundAndCollisionInfoClassName { get; private set; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x00020CEA File Offset: 0x0001EEEA
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x00020CF2 File Offset: 0x0001EEF2
		public float RiderCameraHeightAdder { get; private set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00020CFB File Offset: 0x0001EEFB
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x00020D03 File Offset: 0x0001EF03
		public float RiderBodyCapsuleHeightAdder { get; private set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00020D0C File Offset: 0x0001EF0C
		// (set) Token: 0x06000A27 RID: 2599 RVA: 0x00020D14 File Offset: 0x0001EF14
		public float RiderBodyCapsuleForwardAdder { get; private set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x00020D1D File Offset: 0x0001EF1D
		// (set) Token: 0x06000A29 RID: 2601 RVA: 0x00020D25 File Offset: 0x0001EF25
		public float StandingChestHeight { get; private set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x00020D2E File Offset: 0x0001EF2E
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x00020D36 File Offset: 0x0001EF36
		public float StandingPelvisHeight { get; private set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x00020D3F File Offset: 0x0001EF3F
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x00020D47 File Offset: 0x0001EF47
		public float StandingEyeHeight { get; private set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x00020D50 File Offset: 0x0001EF50
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x00020D58 File Offset: 0x0001EF58
		public float CrouchEyeHeight { get; private set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x00020D61 File Offset: 0x0001EF61
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x00020D69 File Offset: 0x0001EF69
		public float MountedEyeHeight { get; private set; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x00020D72 File Offset: 0x0001EF72
		// (set) Token: 0x06000A33 RID: 2611 RVA: 0x00020D7A File Offset: 0x0001EF7A
		public float RiderEyeHeightAdder { get; private set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x00020D83 File Offset: 0x0001EF83
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x00020D8B File Offset: 0x0001EF8B
		public Vec3 EyeOffsetWrtHead { get; private set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x00020D94 File Offset: 0x0001EF94
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x00020D9C File Offset: 0x0001EF9C
		public Vec3 FirstPersonCameraOffsetWrtHead { get; private set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00020DA5 File Offset: 0x0001EFA5
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x00020DAD File Offset: 0x0001EFAD
		public float ArmLength { get; private set; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00020DB6 File Offset: 0x0001EFB6
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x00020DBE File Offset: 0x0001EFBE
		public float ArmWeight { get; private set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00020DC7 File Offset: 0x0001EFC7
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x00020DCF File Offset: 0x0001EFCF
		public float JumpSpeedLimit { get; private set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00020DD8 File Offset: 0x0001EFD8
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x00020DE0 File Offset: 0x0001EFE0
		public float RelativeSpeedLimitForCharge { get; private set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x00020DE9 File Offset: 0x0001EFE9
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x00020DF1 File Offset: 0x0001EFF1
		public int FamilyType { get; private set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x00020DFA File Offset: 0x0001EFFA
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x00020E02 File Offset: 0x0001F002
		public sbyte[] IndicesOfRagdollBonesToCheckForCorpses { get; private set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00020E0B File Offset: 0x0001F00B
		// (set) Token: 0x06000A45 RID: 2629 RVA: 0x00020E13 File Offset: 0x0001F013
		public sbyte[] RagdollFallSoundBoneIndices { get; private set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00020E1C File Offset: 0x0001F01C
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x00020E24 File Offset: 0x0001F024
		public sbyte HeadLookDirectionBoneIndex { get; private set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x00020E2D File Offset: 0x0001F02D
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x00020E35 File Offset: 0x0001F035
		public sbyte SpineLowerBoneIndex { get; private set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00020E3E File Offset: 0x0001F03E
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x00020E46 File Offset: 0x0001F046
		public sbyte SpineUpperBoneIndex { get; private set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00020E4F File Offset: 0x0001F04F
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x00020E57 File Offset: 0x0001F057
		public sbyte ThoraxLookDirectionBoneIndex { get; private set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00020E60 File Offset: 0x0001F060
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x00020E68 File Offset: 0x0001F068
		public sbyte NeckRootBoneIndex { get; private set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x00020E71 File Offset: 0x0001F071
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x00020E79 File Offset: 0x0001F079
		public sbyte PelvisBoneIndex { get; private set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x00020E82 File Offset: 0x0001F082
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x00020E8A File Offset: 0x0001F08A
		public sbyte RightUpperArmBoneIndex { get; private set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x00020E93 File Offset: 0x0001F093
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x00020E9B File Offset: 0x0001F09B
		public sbyte LeftUpperArmBoneIndex { get; private set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x00020EA4 File Offset: 0x0001F0A4
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x00020EAC File Offset: 0x0001F0AC
		public sbyte FallBlowDamageBoneIndex { get; private set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x00020EB5 File Offset: 0x0001F0B5
		// (set) Token: 0x06000A59 RID: 2649 RVA: 0x00020EBD File Offset: 0x0001F0BD
		public sbyte TerrainDecalBone0Index { get; private set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000A5A RID: 2650 RVA: 0x00020EC6 File Offset: 0x0001F0C6
		// (set) Token: 0x06000A5B RID: 2651 RVA: 0x00020ECE File Offset: 0x0001F0CE
		public sbyte TerrainDecalBone1Index { get; private set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00020ED7 File Offset: 0x0001F0D7
		// (set) Token: 0x06000A5D RID: 2653 RVA: 0x00020EDF File Offset: 0x0001F0DF
		public sbyte[] RagdollStationaryCheckBoneIndices { get; private set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000A5E RID: 2654 RVA: 0x00020EE8 File Offset: 0x0001F0E8
		// (set) Token: 0x06000A5F RID: 2655 RVA: 0x00020EF0 File Offset: 0x0001F0F0
		public sbyte[] MoveAdderBoneIndices { get; private set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000A60 RID: 2656 RVA: 0x00020EF9 File Offset: 0x0001F0F9
		// (set) Token: 0x06000A61 RID: 2657 RVA: 0x00020F01 File Offset: 0x0001F101
		public sbyte[] SplashDecalBoneIndices { get; private set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000A62 RID: 2658 RVA: 0x00020F0A File Offset: 0x0001F10A
		// (set) Token: 0x06000A63 RID: 2659 RVA: 0x00020F12 File Offset: 0x0001F112
		public sbyte[] BloodBurstBoneIndices { get; private set; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x00020F1B File Offset: 0x0001F11B
		// (set) Token: 0x06000A65 RID: 2661 RVA: 0x00020F23 File Offset: 0x0001F123
		public sbyte MainHandBoneIndex { get; private set; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x00020F2C File Offset: 0x0001F12C
		// (set) Token: 0x06000A67 RID: 2663 RVA: 0x00020F34 File Offset: 0x0001F134
		public sbyte OffHandBoneIndex { get; private set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00020F3D File Offset: 0x0001F13D
		// (set) Token: 0x06000A69 RID: 2665 RVA: 0x00020F45 File Offset: 0x0001F145
		public sbyte MainHandItemBoneIndex { get; private set; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00020F4E File Offset: 0x0001F14E
		// (set) Token: 0x06000A6B RID: 2667 RVA: 0x00020F56 File Offset: 0x0001F156
		public sbyte OffHandItemBoneIndex { get; private set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x00020F5F File Offset: 0x0001F15F
		// (set) Token: 0x06000A6D RID: 2669 RVA: 0x00020F67 File Offset: 0x0001F167
		public sbyte MainHandItemSecondaryBoneIndex { get; private set; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x00020F70 File Offset: 0x0001F170
		// (set) Token: 0x06000A6F RID: 2671 RVA: 0x00020F78 File Offset: 0x0001F178
		public sbyte OffHandItemSecondaryBoneIndex { get; private set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x00020F81 File Offset: 0x0001F181
		// (set) Token: 0x06000A71 RID: 2673 RVA: 0x00020F89 File Offset: 0x0001F189
		public sbyte OffHandShoulderBoneIndex { get; private set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00020F92 File Offset: 0x0001F192
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x00020F9A File Offset: 0x0001F19A
		public sbyte HandNumBonesForIk { get; private set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00020FA3 File Offset: 0x0001F1A3
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x00020FAB File Offset: 0x0001F1AB
		public sbyte PrimaryFootBoneIndex { get; private set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00020FB4 File Offset: 0x0001F1B4
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x00020FBC File Offset: 0x0001F1BC
		public sbyte SecondaryFootBoneIndex { get; private set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00020FC5 File Offset: 0x0001F1C5
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x00020FCD File Offset: 0x0001F1CD
		public sbyte RightFootIkEndEffectorBoneIndex { get; private set; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00020FD6 File Offset: 0x0001F1D6
		// (set) Token: 0x06000A7B RID: 2683 RVA: 0x00020FDE File Offset: 0x0001F1DE
		public sbyte LeftFootIkEndEffectorBoneIndex { get; private set; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x00020FE7 File Offset: 0x0001F1E7
		// (set) Token: 0x06000A7D RID: 2685 RVA: 0x00020FEF File Offset: 0x0001F1EF
		public sbyte RightFootIkTipBoneIndex { get; private set; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000A7E RID: 2686 RVA: 0x00020FF8 File Offset: 0x0001F1F8
		// (set) Token: 0x06000A7F RID: 2687 RVA: 0x00021000 File Offset: 0x0001F200
		public sbyte LeftFootIkTipBoneIndex { get; private set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x00021009 File Offset: 0x0001F209
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x00021011 File Offset: 0x0001F211
		public sbyte FootNumBonesForIk { get; private set; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x0002101A File Offset: 0x0001F21A
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x00021022 File Offset: 0x0001F222
		public Vec3 ReinHandleLeftLocalPosition { get; private set; }

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0002102B File Offset: 0x0001F22B
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x00021033 File Offset: 0x0001F233
		public Vec3 ReinHandleRightLocalPosition { get; private set; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0002103C File Offset: 0x0001F23C
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x00021044 File Offset: 0x0001F244
		public string ReinSkeleton { get; private set; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0002104D File Offset: 0x0001F24D
		// (set) Token: 0x06000A89 RID: 2697 RVA: 0x00021055 File Offset: 0x0001F255
		public string ReinCollisionBody { get; private set; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0002105E File Offset: 0x0001F25E
		// (set) Token: 0x06000A8B RID: 2699 RVA: 0x00021066 File Offset: 0x0001F266
		public sbyte FrontBoneToDetectGroundSlopeIndex { get; private set; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0002106F File Offset: 0x0001F26F
		// (set) Token: 0x06000A8D RID: 2701 RVA: 0x00021077 File Offset: 0x0001F277
		public sbyte BackBoneToDetectGroundSlopeIndex { get; private set; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x00021080 File Offset: 0x0001F280
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x00021088 File Offset: 0x0001F288
		public sbyte[] BoneIndicesToModifyOnSlopingGround { get; private set; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00021091 File Offset: 0x0001F291
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x00021099 File Offset: 0x0001F299
		public sbyte BodyRotationReferenceBoneIndex { get; private set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x000210A2 File Offset: 0x0001F2A2
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x000210AA File Offset: 0x0001F2AA
		public sbyte RiderSitBoneIndex { get; private set; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x000210B3 File Offset: 0x0001F2B3
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x000210BB File Offset: 0x0001F2BB
		public sbyte ReinHandleBoneIndex { get; private set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x000210C4 File Offset: 0x0001F2C4
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x000210CC File Offset: 0x0001F2CC
		public sbyte ReinCollision1BoneIndex { get; private set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x000210D5 File Offset: 0x0001F2D5
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x000210DD File Offset: 0x0001F2DD
		public sbyte ReinCollision2BoneIndex { get; private set; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x000210E6 File Offset: 0x0001F2E6
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x000210EE File Offset: 0x0001F2EE
		public sbyte ReinHeadBoneIndex { get; private set; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x000210F7 File Offset: 0x0001F2F7
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x000210FF File Offset: 0x0001F2FF
		public sbyte ReinHeadRightAttachmentBoneIndex { get; private set; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x00021108 File Offset: 0x0001F308
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x00021110 File Offset: 0x0001F310
		public sbyte ReinHeadLeftAttachmentBoneIndex { get; private set; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x00021119 File Offset: 0x0001F319
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x00021121 File Offset: 0x0001F321
		public sbyte ReinRightHandBoneIndex { get; private set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0002112A File Offset: 0x0001F32A
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x00021132 File Offset: 0x0001F332
		public sbyte ReinLeftHandBoneIndex { get; private set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0002113C File Offset: 0x0001F33C
		[CachedData]
		public IMonsterMissionData MonsterMissionData
		{
			get
			{
				IMonsterMissionData monsterMissionData;
				if ((monsterMissionData = this._monsterMissionData) == null)
				{
					monsterMissionData = (this._monsterMissionData = Game.Current.MonsterMissionDataCreator.CreateMonsterMissionData(this));
				}
				return monsterMissionData;
			}
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0002116C File Offset: 0x0001F36C
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			bool flag = false;
			XmlAttribute xmlAttribute = node.Attributes["base_monster"];
			List<sbyte> list;
			List<sbyte> list2;
			List<sbyte> list3;
			List<sbyte> list4;
			List<sbyte> list5;
			List<sbyte> list6;
			List<sbyte> list7;
			if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value))
			{
				flag = true;
				this.BaseMonster = xmlAttribute.Value;
				Monster @object = objectManager.GetObject<Monster>(this.BaseMonster);
				if (!string.IsNullOrEmpty(@object.BaseMonster))
				{
					this.BaseMonster = @object.BaseMonster;
				}
				this.BodyCapsuleRadius = @object.BodyCapsuleRadius;
				this.BodyCapsulePoint1 = @object.BodyCapsulePoint1;
				this.BodyCapsulePoint2 = @object.BodyCapsulePoint2;
				this.CrouchedBodyCapsuleRadius = @object.CrouchedBodyCapsuleRadius;
				this.CrouchedBodyCapsulePoint1 = @object.CrouchedBodyCapsulePoint1;
				this.CrouchedBodyCapsulePoint2 = @object.CrouchedBodyCapsulePoint2;
				this.Flags = @object.Flags;
				this.Weight = @object.Weight;
				this.HitPoints = @object.HitPoints;
				this.ActionSetCode = @object.ActionSetCode;
				this.FemaleActionSetCode = @object.FemaleActionSetCode;
				this.MonsterUsage = @object.MonsterUsage;
				this.NumPaces = @object.NumPaces;
				this.WalkingSpeedLimit = @object.WalkingSpeedLimit;
				this.CrouchWalkingSpeedLimit = @object.CrouchWalkingSpeedLimit;
				this.JumpAcceleration = @object.JumpAcceleration;
				this.AbsorbedDamageRatio = @object.AbsorbedDamageRatio;
				this.SoundAndCollisionInfoClassName = @object.SoundAndCollisionInfoClassName;
				this.RiderCameraHeightAdder = @object.RiderCameraHeightAdder;
				this.RiderBodyCapsuleHeightAdder = @object.RiderBodyCapsuleHeightAdder;
				this.RiderBodyCapsuleForwardAdder = @object.RiderBodyCapsuleForwardAdder;
				this.StandingChestHeight = @object.StandingChestHeight;
				this.StandingPelvisHeight = @object.StandingPelvisHeight;
				this.StandingEyeHeight = @object.StandingEyeHeight;
				this.CrouchEyeHeight = @object.CrouchEyeHeight;
				this.MountedEyeHeight = @object.MountedEyeHeight;
				this.RiderEyeHeightAdder = @object.RiderEyeHeightAdder;
				this.EyeOffsetWrtHead = @object.EyeOffsetWrtHead;
				this.FirstPersonCameraOffsetWrtHead = @object.FirstPersonCameraOffsetWrtHead;
				this.ArmLength = @object.ArmLength;
				this.ArmWeight = @object.ArmWeight;
				this.JumpSpeedLimit = @object.JumpSpeedLimit;
				this.RelativeSpeedLimitForCharge = @object.RelativeSpeedLimitForCharge;
				this.FamilyType = @object.FamilyType;
				list = new List<sbyte>(@object.IndicesOfRagdollBonesToCheckForCorpses);
				list2 = new List<sbyte>(@object.RagdollFallSoundBoneIndices);
				this.HeadLookDirectionBoneIndex = @object.HeadLookDirectionBoneIndex;
				this.SpineLowerBoneIndex = @object.SpineLowerBoneIndex;
				this.SpineUpperBoneIndex = @object.SpineUpperBoneIndex;
				this.ThoraxLookDirectionBoneIndex = @object.ThoraxLookDirectionBoneIndex;
				this.NeckRootBoneIndex = @object.NeckRootBoneIndex;
				this.PelvisBoneIndex = @object.PelvisBoneIndex;
				this.RightUpperArmBoneIndex = @object.RightUpperArmBoneIndex;
				this.LeftUpperArmBoneIndex = @object.LeftUpperArmBoneIndex;
				this.FallBlowDamageBoneIndex = @object.FallBlowDamageBoneIndex;
				this.TerrainDecalBone0Index = @object.TerrainDecalBone0Index;
				this.TerrainDecalBone1Index = @object.TerrainDecalBone1Index;
				list3 = new List<sbyte>(@object.RagdollStationaryCheckBoneIndices);
				list4 = new List<sbyte>(@object.MoveAdderBoneIndices);
				list5 = new List<sbyte>(@object.SplashDecalBoneIndices);
				list6 = new List<sbyte>(@object.BloodBurstBoneIndices);
				this.MainHandBoneIndex = @object.MainHandBoneIndex;
				this.OffHandBoneIndex = @object.OffHandBoneIndex;
				this.MainHandItemBoneIndex = @object.MainHandItemBoneIndex;
				this.OffHandItemBoneIndex = @object.OffHandItemBoneIndex;
				this.MainHandItemSecondaryBoneIndex = @object.MainHandItemSecondaryBoneIndex;
				this.OffHandItemSecondaryBoneIndex = @object.OffHandItemSecondaryBoneIndex;
				this.OffHandShoulderBoneIndex = @object.OffHandShoulderBoneIndex;
				this.HandNumBonesForIk = @object.HandNumBonesForIk;
				this.PrimaryFootBoneIndex = @object.PrimaryFootBoneIndex;
				this.SecondaryFootBoneIndex = @object.SecondaryFootBoneIndex;
				this.RightFootIkEndEffectorBoneIndex = @object.RightFootIkEndEffectorBoneIndex;
				this.LeftFootIkEndEffectorBoneIndex = @object.LeftFootIkEndEffectorBoneIndex;
				this.RightFootIkTipBoneIndex = @object.RightFootIkTipBoneIndex;
				this.LeftFootIkTipBoneIndex = @object.LeftFootIkTipBoneIndex;
				this.FootNumBonesForIk = @object.FootNumBonesForIk;
				this.ReinHandleLeftLocalPosition = @object.ReinHandleLeftLocalPosition;
				this.ReinHandleRightLocalPosition = @object.ReinHandleRightLocalPosition;
				this.ReinSkeleton = @object.ReinSkeleton;
				this.ReinCollisionBody = @object.ReinCollisionBody;
				this.FrontBoneToDetectGroundSlopeIndex = @object.FrontBoneToDetectGroundSlopeIndex;
				this.BackBoneToDetectGroundSlopeIndex = @object.BackBoneToDetectGroundSlopeIndex;
				list7 = new List<sbyte>(@object.BoneIndicesToModifyOnSlopingGround);
				this.BodyRotationReferenceBoneIndex = @object.BodyRotationReferenceBoneIndex;
				this.RiderSitBoneIndex = @object.RiderSitBoneIndex;
				this.ReinHandleBoneIndex = @object.ReinHandleBoneIndex;
				this.ReinCollision1BoneIndex = @object.ReinCollision1BoneIndex;
				this.ReinCollision2BoneIndex = @object.ReinCollision2BoneIndex;
				this.ReinHeadBoneIndex = @object.ReinHeadBoneIndex;
				this.ReinHeadRightAttachmentBoneIndex = @object.ReinHeadRightAttachmentBoneIndex;
				this.ReinHeadLeftAttachmentBoneIndex = @object.ReinHeadLeftAttachmentBoneIndex;
				this.ReinRightHandBoneIndex = @object.ReinRightHandBoneIndex;
				this.ReinLeftHandBoneIndex = @object.ReinLeftHandBoneIndex;
			}
			else
			{
				list = new List<sbyte>(12);
				list2 = new List<sbyte>(4);
				list3 = new List<sbyte>(8);
				list4 = new List<sbyte>(8);
				list5 = new List<sbyte>(8);
				list6 = new List<sbyte>(8);
				list7 = new List<sbyte>(8);
			}
			XmlAttribute xmlAttribute2 = node.Attributes["action_set"];
			if (xmlAttribute2 != null && !string.IsNullOrEmpty(xmlAttribute2.Value))
			{
				this.ActionSetCode = xmlAttribute2.Value;
			}
			XmlAttribute xmlAttribute3 = node.Attributes["female_action_set"];
			if (xmlAttribute3 != null && !string.IsNullOrEmpty(xmlAttribute3.Value))
			{
				this.FemaleActionSetCode = xmlAttribute3.Value;
			}
			XmlAttribute xmlAttribute4 = node.Attributes["monster_usage"];
			if (xmlAttribute4 != null && !string.IsNullOrEmpty(xmlAttribute4.Value))
			{
				this.MonsterUsage = xmlAttribute4.Value;
			}
			else if (!flag)
			{
				this.MonsterUsage = "";
			}
			if (!flag)
			{
				this.Weight = 1;
			}
			XmlAttribute xmlAttribute5 = node.Attributes["weight"];
			int num;
			if (xmlAttribute5 != null && !string.IsNullOrEmpty(xmlAttribute5.Value) && int.TryParse(xmlAttribute5.Value, out num))
			{
				this.Weight = num;
			}
			if (!flag)
			{
				this.HitPoints = 1;
			}
			XmlAttribute xmlAttribute6 = node.Attributes["hit_points"];
			int num2;
			if (xmlAttribute6 != null && !string.IsNullOrEmpty(xmlAttribute6.Value) && int.TryParse(xmlAttribute6.Value, out num2))
			{
				this.HitPoints = num2;
			}
			XmlAttribute xmlAttribute7 = node.Attributes["num_paces"];
			int num3;
			if (xmlAttribute7 != null && !string.IsNullOrEmpty(xmlAttribute7.Value) && int.TryParse(xmlAttribute7.Value, out num3))
			{
				this.NumPaces = num3;
			}
			XmlAttribute xmlAttribute8 = node.Attributes["walking_speed_limit"];
			float num4;
			if (xmlAttribute8 != null && !string.IsNullOrEmpty(xmlAttribute8.Value) && float.TryParse(xmlAttribute8.Value, out num4))
			{
				this.WalkingSpeedLimit = num4;
			}
			XmlAttribute xmlAttribute9 = node.Attributes["crouch_walking_speed_limit"];
			if (xmlAttribute9 != null && !string.IsNullOrEmpty(xmlAttribute9.Value))
			{
				float num5;
				if (float.TryParse(xmlAttribute9.Value, out num5))
				{
					this.CrouchWalkingSpeedLimit = num5;
				}
			}
			else if (!flag)
			{
				this.CrouchWalkingSpeedLimit = this.WalkingSpeedLimit;
			}
			XmlAttribute xmlAttribute10 = node.Attributes["jump_acceleration"];
			float num6;
			if (xmlAttribute10 != null && !string.IsNullOrEmpty(xmlAttribute10.Value) && float.TryParse(xmlAttribute10.Value, out num6))
			{
				this.JumpAcceleration = num6;
			}
			XmlAttribute xmlAttribute11 = node.Attributes["absorbed_damage_ratio"];
			if (xmlAttribute11 != null && !string.IsNullOrEmpty(xmlAttribute11.Value))
			{
				float num7;
				if (float.TryParse(xmlAttribute11.Value, out num7))
				{
					if (num7 < 0f)
					{
						num7 = 0f;
					}
					this.AbsorbedDamageRatio = num7;
				}
			}
			else if (!flag)
			{
				this.AbsorbedDamageRatio = 1f;
			}
			XmlAttribute xmlAttribute12 = node.Attributes["sound_and_collision_info_class"];
			if (xmlAttribute12 != null && !string.IsNullOrEmpty(xmlAttribute12.Value))
			{
				this.SoundAndCollisionInfoClassName = xmlAttribute12.Value;
			}
			XmlAttribute xmlAttribute13 = node.Attributes["rider_camera_height_adder"];
			float num8;
			if (xmlAttribute13 != null && !string.IsNullOrEmpty(xmlAttribute13.Value) && float.TryParse(xmlAttribute13.Value, out num8))
			{
				this.RiderCameraHeightAdder = num8;
			}
			XmlAttribute xmlAttribute14 = node.Attributes["rider_body_capsule_height_adder"];
			float num9;
			if (xmlAttribute14 != null && !string.IsNullOrEmpty(xmlAttribute14.Value) && float.TryParse(xmlAttribute14.Value, out num9))
			{
				this.RiderBodyCapsuleHeightAdder = num9;
			}
			XmlAttribute xmlAttribute15 = node.Attributes["rider_body_capsule_forward_adder"];
			float num10;
			if (xmlAttribute15 != null && !string.IsNullOrEmpty(xmlAttribute15.Value) && float.TryParse(xmlAttribute15.Value, out num10))
			{
				this.RiderBodyCapsuleForwardAdder = num10;
			}
			XmlAttribute xmlAttribute16 = node.Attributes["preliminary_collision_capsule_radius_multiplier"];
			if (!flag && xmlAttribute16 != null && !string.IsNullOrEmpty(xmlAttribute16.Value))
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\Monster.cs", "Deserialize", 433);
			}
			XmlAttribute xmlAttribute17 = node.Attributes["rider_preliminary_collision_capsule_height_multiplier"];
			if (!flag && xmlAttribute17 != null && !string.IsNullOrEmpty(xmlAttribute17.Value))
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\Monster.cs", "Deserialize", 442);
			}
			XmlAttribute xmlAttribute18 = node.Attributes["rider_preliminary_collision_capsule_height_adder"];
			if (!flag && xmlAttribute18 != null && !string.IsNullOrEmpty(xmlAttribute18.Value))
			{
				Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\Monster.cs", "Deserialize", 451);
			}
			XmlAttribute xmlAttribute19 = node.Attributes["standing_chest_height"];
			float num11;
			if (xmlAttribute19 != null && !string.IsNullOrEmpty(xmlAttribute19.Value) && float.TryParse(xmlAttribute19.Value, out num11))
			{
				this.StandingChestHeight = num11;
			}
			XmlAttribute xmlAttribute20 = node.Attributes["standing_pelvis_height"];
			float num12;
			if (xmlAttribute20 != null && !string.IsNullOrEmpty(xmlAttribute20.Value) && float.TryParse(xmlAttribute20.Value, out num12))
			{
				this.StandingPelvisHeight = num12;
			}
			XmlAttribute xmlAttribute21 = node.Attributes["standing_eye_height"];
			float num13;
			if (xmlAttribute21 != null && !string.IsNullOrEmpty(xmlAttribute21.Value) && float.TryParse(xmlAttribute21.Value, out num13))
			{
				this.StandingEyeHeight = num13;
			}
			XmlAttribute xmlAttribute22 = node.Attributes["crouch_eye_height"];
			float num14;
			if (xmlAttribute22 != null && !string.IsNullOrEmpty(xmlAttribute22.Value) && float.TryParse(xmlAttribute22.Value, out num14))
			{
				this.CrouchEyeHeight = num14;
			}
			XmlAttribute xmlAttribute23 = node.Attributes["mounted_eye_height"];
			float num15;
			if (xmlAttribute23 != null && !string.IsNullOrEmpty(xmlAttribute23.Value) && float.TryParse(xmlAttribute23.Value, out num15))
			{
				this.MountedEyeHeight = num15;
			}
			XmlAttribute xmlAttribute24 = node.Attributes["rider_eye_height_adder"];
			float num16;
			if (xmlAttribute24 != null && !string.IsNullOrEmpty(xmlAttribute24.Value) && float.TryParse(xmlAttribute24.Value, out num16))
			{
				this.RiderEyeHeightAdder = num16;
			}
			if (!flag)
			{
				this.EyeOffsetWrtHead = new Vec3(0.01f, 0.01f, 0.01f, -1f);
			}
			XmlAttribute xmlAttribute25 = node.Attributes["eye_offset_wrt_head"];
			Vec3 vec;
			if (xmlAttribute25 != null && !string.IsNullOrEmpty(xmlAttribute25.Value) && Monster.ReadVec3(xmlAttribute25.Value, out vec))
			{
				this.EyeOffsetWrtHead = vec;
			}
			if (!flag)
			{
				this.FirstPersonCameraOffsetWrtHead = new Vec3(0.01f, 0.01f, 0.01f, -1f);
			}
			XmlAttribute xmlAttribute26 = node.Attributes["first_person_camera_offset_wrt_head"];
			Vec3 vec2;
			if (xmlAttribute26 != null && !string.IsNullOrEmpty(xmlAttribute26.Value) && Monster.ReadVec3(xmlAttribute26.Value, out vec2))
			{
				this.FirstPersonCameraOffsetWrtHead = vec2;
			}
			XmlAttribute xmlAttribute27 = node.Attributes["arm_length"];
			float num17;
			if (xmlAttribute27 != null && !string.IsNullOrEmpty(xmlAttribute27.Value) && float.TryParse(xmlAttribute27.Value, out num17))
			{
				this.ArmLength = num17;
			}
			XmlAttribute xmlAttribute28 = node.Attributes["arm_weight"];
			float num18;
			if (xmlAttribute28 != null && !string.IsNullOrEmpty(xmlAttribute28.Value) && float.TryParse(xmlAttribute28.Value, out num18))
			{
				this.ArmWeight = num18;
			}
			XmlAttribute xmlAttribute29 = node.Attributes["jump_speed_limit"];
			float num19;
			if (xmlAttribute29 != null && !string.IsNullOrEmpty(xmlAttribute29.Value) && float.TryParse(xmlAttribute29.Value, out num19))
			{
				this.JumpSpeedLimit = num19;
			}
			if (!flag)
			{
				this.RelativeSpeedLimitForCharge = float.MaxValue;
			}
			XmlAttribute xmlAttribute30 = node.Attributes["relative_speed_limit_for_charge"];
			float num20;
			if (xmlAttribute30 != null && !string.IsNullOrEmpty(xmlAttribute30.Value) && float.TryParse(xmlAttribute30.Value, out num20))
			{
				this.RelativeSpeedLimitForCharge = num20;
			}
			XmlAttribute xmlAttribute31 = node.Attributes["family_type"];
			int num21;
			if (xmlAttribute31 != null && !string.IsNullOrEmpty(xmlAttribute31.Value) && int.TryParse(xmlAttribute31.Value, out num21))
			{
				this.FamilyType = num21;
			}
			sbyte b = -1;
			this.DeserializeBoneIndexArray(list, node, flag, "ragdoll_bone_to_check_for_corpses_", b, false);
			this.DeserializeBoneIndexArray(list2, node, flag, "ragdoll_fall_sound_bone_", b, false);
			this.HeadLookDirectionBoneIndex = this.DeserializeBoneIndex(node, "head_look_direction_bone", flag ? this.HeadLookDirectionBoneIndex : b, b, true);
			this.SpineLowerBoneIndex = this.DeserializeBoneIndex(node, "spine_lower_bone", flag ? this.SpineLowerBoneIndex : b, b, false);
			this.SpineUpperBoneIndex = this.DeserializeBoneIndex(node, "spine_upper_bone", flag ? this.SpineUpperBoneIndex : b, b, false);
			this.ThoraxLookDirectionBoneIndex = this.DeserializeBoneIndex(node, "thorax_look_direction_bone", flag ? this.ThoraxLookDirectionBoneIndex : b, b, true);
			this.NeckRootBoneIndex = this.DeserializeBoneIndex(node, "neck_root_bone", flag ? this.NeckRootBoneIndex : b, b, true);
			this.PelvisBoneIndex = this.DeserializeBoneIndex(node, "pelvis_bone", flag ? this.PelvisBoneIndex : b, b, false);
			this.RightUpperArmBoneIndex = this.DeserializeBoneIndex(node, "right_upper_arm_bone", flag ? this.RightUpperArmBoneIndex : b, b, false);
			this.LeftUpperArmBoneIndex = this.DeserializeBoneIndex(node, "left_upper_arm_bone", flag ? this.LeftUpperArmBoneIndex : b, b, false);
			this.FallBlowDamageBoneIndex = this.DeserializeBoneIndex(node, "fall_blow_damage_bone", flag ? this.FallBlowDamageBoneIndex : b, b, false);
			this.TerrainDecalBone0Index = this.DeserializeBoneIndex(node, "terrain_decal_bone_0", flag ? this.TerrainDecalBone0Index : b, b, false);
			this.TerrainDecalBone1Index = this.DeserializeBoneIndex(node, "terrain_decal_bone_1", flag ? this.TerrainDecalBone1Index : b, b, false);
			this.DeserializeBoneIndexArray(list3, node, flag, "ragdoll_stationary_check_bone_", b, false);
			this.DeserializeBoneIndexArray(list4, node, flag, "move_adder_bone_", b, false);
			this.DeserializeBoneIndexArray(list5, node, flag, "splash_decal_bone_", b, false);
			this.DeserializeBoneIndexArray(list6, node, flag, "blood_burst_bone_", b, false);
			this.MainHandBoneIndex = this.DeserializeBoneIndex(node, "main_hand_bone", flag ? this.MainHandBoneIndex : b, b, true);
			this.OffHandBoneIndex = this.DeserializeBoneIndex(node, "off_hand_bone", flag ? this.OffHandBoneIndex : b, b, true);
			this.MainHandItemBoneIndex = this.DeserializeBoneIndex(node, "main_hand_item_bone", flag ? this.MainHandItemBoneIndex : b, b, true);
			this.OffHandItemBoneIndex = this.DeserializeBoneIndex(node, "off_hand_item_bone", flag ? this.OffHandItemBoneIndex : b, b, true);
			this.MainHandItemSecondaryBoneIndex = this.DeserializeBoneIndex(node, "main_hand_item_secondary_bone", flag ? this.MainHandItemSecondaryBoneIndex : b, b, false);
			this.OffHandItemSecondaryBoneIndex = this.DeserializeBoneIndex(node, "off_hand_item_secondary_bone", flag ? this.OffHandItemSecondaryBoneIndex : b, b, false);
			this.OffHandShoulderBoneIndex = this.DeserializeBoneIndex(node, "off_hand_shoulder_bone", flag ? this.OffHandShoulderBoneIndex : b, b, false);
			XmlAttribute xmlAttribute32 = node.Attributes["hand_num_bones_for_ik"];
			this.HandNumBonesForIk = ((xmlAttribute32 != null) ? sbyte.Parse(xmlAttribute32.Value) : (flag ? this.HandNumBonesForIk : 0));
			this.PrimaryFootBoneIndex = this.DeserializeBoneIndex(node, "primary_foot_bone", flag ? this.PrimaryFootBoneIndex : b, b, false);
			this.SecondaryFootBoneIndex = this.DeserializeBoneIndex(node, "secondary_foot_bone", flag ? this.SecondaryFootBoneIndex : b, b, false);
			this.RightFootIkEndEffectorBoneIndex = this.DeserializeBoneIndex(node, "right_foot_ik_end_effector_bone", flag ? this.RightFootIkEndEffectorBoneIndex : b, b, true);
			this.LeftFootIkEndEffectorBoneIndex = this.DeserializeBoneIndex(node, "left_foot_ik_end_effector_bone", flag ? this.LeftFootIkEndEffectorBoneIndex : b, b, true);
			this.RightFootIkTipBoneIndex = this.DeserializeBoneIndex(node, "right_foot_ik_tip_bone", flag ? this.RightFootIkTipBoneIndex : b, b, true);
			this.LeftFootIkTipBoneIndex = this.DeserializeBoneIndex(node, "left_foot_ik_tip_bone", flag ? this.LeftFootIkTipBoneIndex : b, b, true);
			XmlAttribute xmlAttribute33 = node.Attributes["foot_num_bones_for_ik"];
			this.FootNumBonesForIk = ((xmlAttribute33 != null) ? sbyte.Parse(xmlAttribute33.Value) : (flag ? this.FootNumBonesForIk : 0));
			XmlNode xmlNode = node.Attributes["rein_handle_left_local_pos"];
			Vec3 vec3;
			if (xmlNode != null && Monster.ReadVec3(xmlNode.Value, out vec3))
			{
				this.ReinHandleLeftLocalPosition = vec3;
			}
			XmlNode xmlNode2 = node.Attributes["rein_handle_right_local_pos"];
			Vec3 vec4;
			if (xmlNode2 != null && Monster.ReadVec3(xmlNode2.Value, out vec4))
			{
				this.ReinHandleRightLocalPosition = vec4;
			}
			XmlAttribute xmlAttribute34 = node.Attributes["rein_skeleton"];
			this.ReinSkeleton = ((xmlAttribute34 != null) ? xmlAttribute34.Value : this.ReinSkeleton);
			XmlAttribute xmlAttribute35 = node.Attributes["rein_collision_body"];
			this.ReinCollisionBody = ((xmlAttribute35 != null) ? xmlAttribute35.Value : this.ReinCollisionBody);
			this.DeserializeBoneIndexArray(list7, node, flag, "bones_to_modify_on_sloping_ground_", b, true);
			XmlAttribute xmlAttribute36 = node.Attributes["front_bone_to_detect_ground_slope_index"];
			this.FrontBoneToDetectGroundSlopeIndex = ((xmlAttribute36 != null) ? sbyte.Parse(xmlAttribute36.Value) : (flag ? this.FrontBoneToDetectGroundSlopeIndex : -1));
			XmlAttribute xmlAttribute37 = node.Attributes["back_bone_to_detect_ground_slope_index"];
			this.BackBoneToDetectGroundSlopeIndex = ((xmlAttribute37 != null) ? sbyte.Parse(xmlAttribute37.Value) : (flag ? this.BackBoneToDetectGroundSlopeIndex : -1));
			this.BodyRotationReferenceBoneIndex = this.DeserializeBoneIndex(node, "body_rotation_reference_bone", flag ? this.BodyRotationReferenceBoneIndex : b, b, true);
			this.RiderSitBoneIndex = this.DeserializeBoneIndex(node, "rider_sit_bone", flag ? this.RiderSitBoneIndex : b, b, false);
			this.ReinHandleBoneIndex = this.DeserializeBoneIndex(node, "rein_handle_bone", flag ? this.ReinHandleBoneIndex : b, b, false);
			this.ReinCollision1BoneIndex = this.DeserializeBoneIndex(node, "rein_collision_1_bone", flag ? this.ReinCollision1BoneIndex : b, b, false);
			this.ReinCollision2BoneIndex = this.DeserializeBoneIndex(node, "rein_collision_2_bone", flag ? this.ReinCollision2BoneIndex : b, b, false);
			this.ReinHeadBoneIndex = this.DeserializeBoneIndex(node, "rein_head_bone", flag ? this.ReinHeadBoneIndex : b, b, false);
			this.ReinHeadRightAttachmentBoneIndex = this.DeserializeBoneIndex(node, "rein_head_right_attachment_bone", flag ? this.ReinHeadRightAttachmentBoneIndex : b, b, false);
			this.ReinHeadLeftAttachmentBoneIndex = this.DeserializeBoneIndex(node, "rein_head_left_attachment_bone", flag ? this.ReinHeadLeftAttachmentBoneIndex : b, b, false);
			this.ReinRightHandBoneIndex = this.DeserializeBoneIndex(node, "rein_right_hand_bone", flag ? this.ReinRightHandBoneIndex : b, b, false);
			this.ReinLeftHandBoneIndex = this.DeserializeBoneIndex(node, "rein_left_hand_bone", flag ? this.ReinLeftHandBoneIndex : b, b, false);
			this.IndicesOfRagdollBonesToCheckForCorpses = list.ToArray();
			this.RagdollFallSoundBoneIndices = list2.ToArray();
			this.RagdollStationaryCheckBoneIndices = list3.ToArray();
			this.MoveAdderBoneIndices = list4.ToArray();
			this.SplashDecalBoneIndices = list5.ToArray();
			this.BloodBurstBoneIndices = list6.ToArray();
			this.BoneIndicesToModifyOnSlopingGround = list7.ToArray();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode3 = (XmlNode)obj;
				if (xmlNode3.Name == "Flags")
				{
					this.Flags = AgentFlag.None;
					using (IEnumerator enumerator2 = Enum.GetValues(typeof(AgentFlag)).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							AgentFlag agentFlag = (AgentFlag)obj2;
							XmlAttribute xmlAttribute38 = xmlNode3.Attributes[agentFlag.ToString()];
							if (xmlAttribute38 != null && !xmlAttribute38.Value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
							{
								this.Flags |= agentFlag;
							}
						}
						continue;
					}
				}
				if (xmlNode3.Name == "Capsules")
				{
					foreach (object obj3 in xmlNode3.ChildNodes)
					{
						XmlNode xmlNode4 = (XmlNode)obj3;
						if (xmlNode4.Attributes != null && (xmlNode4.Name == "preliminary_collision_capsule" || xmlNode4.Name == "body_capsule" || xmlNode4.Name == "crouched_body_capsule"))
						{
							bool flag2 = true;
							Vec3 vec5 = new Vec3(0f, 0f, 0.01f, -1f);
							Vec3 vec6 = Vec3.Zero;
							float num22 = 0.01f;
							if (xmlNode4.Attributes["pos1"] != null)
							{
								Vec3 vec7;
								flag2 = Monster.ReadVec3(xmlNode4.Attributes["pos1"].Value, out vec7) && flag2;
								if (flag2)
								{
									vec5 = vec7;
								}
							}
							if (xmlNode4.Attributes["pos2"] != null)
							{
								Vec3 vec8;
								flag2 = Monster.ReadVec3(xmlNode4.Attributes["pos2"].Value, out vec8) && flag2;
								if (flag2)
								{
									vec6 = vec8;
								}
							}
							if (xmlNode4.Attributes["radius"] != null)
							{
								string text = xmlNode4.Attributes["radius"].Value;
								text = text.Trim();
								flag2 = flag2 && float.TryParse(text, out num22);
							}
							if (flag2)
							{
								if (xmlNode4.Name.StartsWith("p"))
								{
									Debug.FailedAssert("false", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\Monster.cs", "Deserialize", 739);
								}
								else if (xmlNode4.Name.StartsWith("c"))
								{
									this.CrouchedBodyCapsuleRadius = num22;
									this.CrouchedBodyCapsulePoint1 = vec5;
									this.CrouchedBodyCapsulePoint2 = vec6;
								}
								else
								{
									this.BodyCapsuleRadius = num22;
									this.BodyCapsulePoint1 = vec5;
									this.BodyCapsulePoint2 = vec6;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00022840 File Offset: 0x00020A40
		private sbyte DeserializeBoneIndex(XmlNode node, string attributeName, sbyte baseValue, sbyte invalidBoneIndex, bool validateHasParentBone)
		{
			XmlAttribute xmlAttribute = node.Attributes[attributeName];
			sbyte b = ((Monster.GetBoneIndexWithId != null && xmlAttribute != null) ? Monster.GetBoneIndexWithId(this.ActionSetCode, xmlAttribute.Value) : baseValue);
			if (validateHasParentBone && b != invalidBoneIndex)
			{
				Func<string, sbyte, bool> getBoneHasParentBone = Monster.GetBoneHasParentBone;
			}
			return b;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00022890 File Offset: 0x00020A90
		private void DeserializeBoneIndexArray(List<sbyte> boneIndices, XmlNode node, bool hasBaseMonster, string attributeNamePrefix, sbyte invalidBoneIndex, bool validateHasParentBone)
		{
			int num = 0;
			for (;;)
			{
				bool flag = hasBaseMonster && num < boneIndices.Count;
				sbyte b = this.DeserializeBoneIndex(node, attributeNamePrefix + num, flag ? boneIndices[num] : invalidBoneIndex, invalidBoneIndex, validateHasParentBone);
				if (b == invalidBoneIndex)
				{
					break;
				}
				if (flag)
				{
					boneIndices[num] = b;
				}
				else
				{
					boneIndices.Add(b);
				}
				num++;
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x000228F8 File Offset: 0x00020AF8
		private static bool ReadVec3(string str, out Vec3 v)
		{
			str = str.Trim();
			string[] array = str.Split(",".ToCharArray());
			v = new Vec3(0f, 0f, 0f, -1f);
			return float.TryParse(array[0], out v.x) && float.TryParse(array[1], out v.y) && float.TryParse(array[2], out v.z);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00022970 File Offset: 0x00020B70
		public sbyte GetBoneToAttachForItemFlags(ItemFlags itemFlags)
		{
			ItemFlags itemFlags2 = itemFlags & ItemFlags.AttachmentMask;
			if (itemFlags2 <= (ItemFlags)0U)
			{
				return this.MainHandItemBoneIndex;
			}
			if (itemFlags2 == ItemFlags.ForceAttachOffHandPrimaryItemBone)
			{
				return this.OffHandItemBoneIndex;
			}
			if (itemFlags2 != ItemFlags.ForceAttachOffHandSecondaryItemBone)
			{
				return this.MainHandItemBoneIndex;
			}
			return this.OffHandItemSecondaryBoneIndex;
		}

		// Token: 0x04000590 RID: 1424
		public static Func<string, string, sbyte> GetBoneIndexWithId;

		// Token: 0x04000591 RID: 1425
		public static Func<string, sbyte, bool> GetBoneHasParentBone;

		// Token: 0x040005E6 RID: 1510
		[CachedData]
		private IMonsterMissionData _monsterMissionData;
	}
}
