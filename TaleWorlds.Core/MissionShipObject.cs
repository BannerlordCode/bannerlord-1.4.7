using System;
using System.Collections;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000B9 RID: 185
	public class MissionShipObject : MBObjectBase
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000999 RID: 2457 RVA: 0x0001F73E File Offset: 0x0001D93E
		// (set) Token: 0x0600099A RID: 2458 RVA: 0x0001F746 File Offset: 0x0001D946
		public string Prefab { get; private set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0001F74F File Offset: 0x0001D94F
		// (set) Token: 0x0600099C RID: 2460 RVA: 0x0001F757 File Offset: 0x0001D957
		public Vec2 DeploymentArea { get; private set; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x0001F760 File Offset: 0x0001D960
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x0001F768 File Offset: 0x0001D968
		public float Mass { get; private set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x0001F771 File Offset: 0x0001D971
		// (set) Token: 0x060009A0 RID: 2464 RVA: 0x0001F779 File Offset: 0x0001D979
		public float FloatingForceMultiplier { get; private set; }

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0001F782 File Offset: 0x0001D982
		// (set) Token: 0x060009A2 RID: 2466 RVA: 0x0001F78A File Offset: 0x0001D98A
		public float MaximumSubmergedVolumeRatio { get; private set; }

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0001F793 File Offset: 0x0001D993
		// (set) Token: 0x060009A4 RID: 2468 RVA: 0x0001F79B File Offset: 0x0001D99B
		public Vec3 RudderStockPosition { get; private set; }

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060009A5 RID: 2469 RVA: 0x0001F7A4 File Offset: 0x0001D9A4
		// (set) Token: 0x060009A6 RID: 2470 RVA: 0x0001F7AC File Offset: 0x0001D9AC
		public float MaxLateralDragShift { get; private set; }

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0001F7B5 File Offset: 0x0001D9B5
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0001F7BD File Offset: 0x0001D9BD
		public float LateralDragShiftCriticalAngle { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060009A9 RID: 2473 RVA: 0x0001F7C6 File Offset: 0x0001D9C6
		// (set) Token: 0x060009AA RID: 2474 RVA: 0x0001F7CE File Offset: 0x0001D9CE
		public ShipPhysicsReference PhysicsReference { get; private set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0001F7D7 File Offset: 0x0001D9D7
		// (set) Token: 0x060009AC RID: 2476 RVA: 0x0001F7DF File Offset: 0x0001D9DF
		public Vec3 MomentOfInertiaMultiplier { get; private set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0001F7E8 File Offset: 0x0001D9E8
		// (set) Token: 0x060009AE RID: 2478 RVA: 0x0001F7F0 File Offset: 0x0001D9F0
		public LinearFrictionTerm LinearFrictionMultiplier { get; private set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x0001F7F9 File Offset: 0x0001D9F9
		// (set) Token: 0x060009B0 RID: 2480 RVA: 0x0001F801 File Offset: 0x0001DA01
		public Vec3 AngularFrictionMultiplier { get; private set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x0001F80A File Offset: 0x0001DA0A
		// (set) Token: 0x060009B2 RID: 2482 RVA: 0x0001F812 File Offset: 0x0001DA12
		public float TorqueMultiplierOfLateralBuoyantForces { get; private set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0001F81B File Offset: 0x0001DA1B
		// (set) Token: 0x060009B4 RID: 2484 RVA: 0x0001F823 File Offset: 0x0001DA23
		public Vec3 TorqueMultiplierOfVerticalBuoyantForces { get; private set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0001F82C File Offset: 0x0001DA2C
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x0001F834 File Offset: 0x0001DA34
		public float OarsmenForceMultiplier { get; private set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0001F83D File Offset: 0x0001DA3D
		// (set) Token: 0x060009B8 RID: 2488 RVA: 0x0001F845 File Offset: 0x0001DA45
		public float OarsTipSpeed { get; private set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x0001F84E File Offset: 0x0001DA4E
		// (set) Token: 0x060009BA RID: 2490 RVA: 0x0001F856 File Offset: 0x0001DA56
		public float OarFrictionMultiplier { get; private set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x0001F85F File Offset: 0x0001DA5F
		public MBReadOnlyList<ShipSail> Sails
		{
			get
			{
				return this._sails;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0001F867 File Offset: 0x0001DA67
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0001F86F File Offset: 0x0001DA6F
		public int OarCount { get; private set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0001F878 File Offset: 0x0001DA78
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0001F880 File Offset: 0x0001DA80
		public float RudderBladeLength { get; private set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0001F889 File Offset: 0x0001DA89
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0001F891 File Offset: 0x0001DA91
		public float RudderBladeHeight { get; private set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0001F89A File Offset: 0x0001DA9A
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0001F8A2 File Offset: 0x0001DAA2
		public float RudderDeflectionCoef { get; private set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0001F8AB File Offset: 0x0001DAAB
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0001F8B3 File Offset: 0x0001DAB3
		public float RudderRotationMax { get; private set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0001F8BC File Offset: 0x0001DABC
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0001F8C4 File Offset: 0x0001DAC4
		public float RudderRotationRate { get; private set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0001F8CD File Offset: 0x0001DACD
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0001F8D5 File Offset: 0x0001DAD5
		public float RudderForceMax { get; private set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0001F8DE File Offset: 0x0001DADE
		// (set) Token: 0x060009CB RID: 2507 RVA: 0x0001F8E6 File Offset: 0x0001DAE6
		public float MaxLinearSpeed { get; private set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060009CC RID: 2508 RVA: 0x0001F8EF File Offset: 0x0001DAEF
		// (set) Token: 0x060009CD RID: 2509 RVA: 0x0001F8F7 File Offset: 0x0001DAF7
		public float MaxLinearAccel { get; private set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x0001F900 File Offset: 0x0001DB00
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x0001F908 File Offset: 0x0001DB08
		public float MaxAngularSpeed { get; private set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x0001F911 File Offset: 0x0001DB11
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x0001F919 File Offset: 0x0001DB19
		public float MaxAngularAccel { get; private set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0001F922 File Offset: 0x0001DB22
		// (set) Token: 0x060009D3 RID: 2515 RVA: 0x0001F92A File Offset: 0x0001DB2A
		public float PartialHitPointsRatio { get; private set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0001F933 File Offset: 0x0001DB33
		public bool HasSails
		{
			get
			{
				return this._sails.Count > 0;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0001F944 File Offset: 0x0001DB44
		public bool HasValidRudderStockPosition
		{
			get
			{
				return this.RudderStockPosition.IsValid;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0001F95F File Offset: 0x0001DB5F
		// (set) Token: 0x060009D7 RID: 2519 RVA: 0x0001F967 File Offset: 0x0001DB67
		public string ShipPhysicsReferenceId { get; private set; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0001F970 File Offset: 0x0001DB70
		// (set) Token: 0x060009D9 RID: 2521 RVA: 0x0001F978 File Offset: 0x0001DB78
		public float BowAngleLimitFromCenterline { get; private set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0001F981 File Offset: 0x0001DB81
		// (set) Token: 0x060009DB RID: 2523 RVA: 0x0001F989 File Offset: 0x0001DB89
		public float LandingDepth { get; private set; }

		// Token: 0x060009DC RID: 2524 RVA: 0x0001F992 File Offset: 0x0001DB92
		public MissionShipObject()
		{
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001F9A5 File Offset: 0x0001DBA5
		public MissionShipObject(string stringId)
			: base(stringId)
		{
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001F9B9 File Offset: 0x0001DBB9
		public void SetPhysicsReference(ShipPhysicsReference physicsReference)
		{
			this.PhysicsReference = physicsReference;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001F9C4 File Offset: 0x0001DBC4
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			this.MomentOfInertiaMultiplier = new Vec3(1f, 1f, 1f, -1f);
			this.LinearFrictionMultiplier = LinearFrictionTerm.One;
			this.AngularFrictionMultiplier = Vec3.One;
			this.TorqueMultiplierOfVerticalBuoyantForces = new Vec3(1f, 1f, 1f, -1f);
			this.FloatingForceMultiplier = 1f;
			this.MaximumSubmergedVolumeRatio = 0.7f;
			this.OarsmenForceMultiplier = 1f;
			this.OarFrictionMultiplier = 1f;
			this.MaxLateralDragShift = 0f;
			this.LateralDragShiftCriticalAngle = 0f;
			this.MaximumSubmergedVolumeRatio = 0.7f;
			this.TorqueMultiplierOfLateralBuoyantForces = 0.5f;
			this.PhysicsReference = ShipPhysicsReference.Default;
			this.RudderStockPosition = Vec3.Invalid;
			this.DeserializeAux(objectManager, node);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001FAA4 File Offset: 0x0001DCA4
		private XmlNode GetSiblingShipNodeWithId(XmlNode node, string id)
		{
			foreach (object obj in node.ParentNode.SelectNodes("MissionShip"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlAttribute xmlAttribute = xmlNode.Attributes["id"];
				if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.InnerText) && xmlAttribute.Value == id)
				{
					return xmlNode;
				}
			}
			return null;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0001FB40 File Offset: 0x0001DD40
		private void DeserializeAux(MBObjectManager objectManager, XmlNode node)
		{
			bool flag = true;
			bool flag2;
			string text = this.DeserializeScalarAttribute<string>(node, "base_mission_ship", false, out flag2);
			if (flag2 && text != base.StringId)
			{
				XmlNode siblingShipNodeWithId = this.GetSiblingShipNodeWithId(node, text);
				if (siblingShipNodeWithId != null)
				{
					this.DeserializeAux(objectManager, siblingShipNodeWithId);
					flag = false;
				}
			}
			bool flag3;
			string text2 = this.DeserializeScalarAttribute<string>(node, "prefab", flag, out flag3);
			if (flag3)
			{
				if (!flag)
				{
					text2 == this.Prefab;
				}
				this.Prefab = text2;
			}
			bool flag4;
			string text3 = this.DeserializeScalarAttribute<string>(node, "ship_physics_reference_id", flag, out flag4);
			if (flag4)
			{
				if (!flag)
				{
					text3 == this.ShipPhysicsReferenceId;
				}
				this.ShipPhysicsReferenceId = text3;
				if (objectManager != null)
				{
					ShipPhysicsReference @object = objectManager.GetObject<ShipPhysicsReference>(this.ShipPhysicsReferenceId);
					this.PhysicsReference = @object;
				}
			}
			bool flag5;
			float num = this.DeserializeScalarAttribute<float>(node, "mass", flag, out flag5);
			if (flag5)
			{
				if (!flag)
				{
					num.ApproximatelyEqualsTo(this.Mass, 1E-05f);
				}
				this.Mass = num;
			}
			bool flag6;
			float num2 = this.DeserializeFloatAttribute(node, "floating_force_multiplier", false, out flag6, 0.01f, 5f, "");
			if (flag6)
			{
				if (!flag)
				{
					num2.ApproximatelyEqualsTo(this.FloatingForceMultiplier, 1E-05f);
				}
				this.FloatingForceMultiplier = num2;
			}
			bool flag7;
			float num3 = this.DeserializeFloatAttribute(node, "maximum_submerged_volume_ratio", false, out flag7, 0.01f, 5f, "");
			if (flag7)
			{
				if (!flag)
				{
					num3.ApproximatelyEqualsTo(this.MaximumSubmergedVolumeRatio, 1E-05f);
				}
				this.MaximumSubmergedVolumeRatio = num3;
			}
			bool flag8;
			float num4 = this.DeserializeFloatAttribute(node, "oars_tip_speed", flag, out flag8, float.MinValue, float.MaxValue, "");
			if (flag8)
			{
				if (!flag)
				{
					num4.ApproximatelyEqualsTo(this.OarsTipSpeed, 1E-05f);
				}
				this.OarsTipSpeed = num4;
			}
			bool flag9;
			float num5 = this.DeserializeFloatAttribute(node, "oarsmen_force_multiplier", false, out flag9, float.MinValue, float.MaxValue, "");
			if (flag9)
			{
				if (!flag)
				{
					num5.ApproximatelyEqualsTo(this.OarsmenForceMultiplier, 1E-05f);
				}
				this.OarsmenForceMultiplier = num5;
			}
			bool flag10;
			float num6 = this.DeserializeFloatAttribute(node, "oar_friction_multiplier", false, out flag10, float.MinValue, float.MaxValue, "");
			if (flag10)
			{
				if (!flag)
				{
					num6.ApproximatelyEqualsTo(this.OarFrictionMultiplier, 1E-05f);
				}
				this.OarFrictionMultiplier = num6;
			}
			bool flag11;
			int num7 = this.DeserializeScalarAttribute<int>(node, "oar_count", flag, out flag11);
			if (flag11)
			{
				if (!flag)
				{
					int oarCount = this.OarCount;
				}
				this.OarCount = num7;
			}
			bool flag12;
			float num8 = this.DeserializeScalarAttribute<float>(node, "rudder_blade_length", flag, out flag12);
			if (flag12)
			{
				if (!flag)
				{
					num8.ApproximatelyEqualsTo(this.RudderBladeLength, 1E-05f);
				}
				this.RudderBladeLength = num8;
			}
			bool flag13;
			float num9 = this.DeserializeScalarAttribute<float>(node, "rudder_blade_height", flag, out flag13);
			if (flag13)
			{
				if (!flag)
				{
					num9.ApproximatelyEqualsTo(this.RudderBladeHeight, 1E-05f);
				}
				this.RudderBladeHeight = num9;
			}
			bool flag14;
			float num10 = this.DeserializeScalarAttribute<float>(node, "rudder_deflection_coef", flag, out flag14);
			if (flag14)
			{
				if (!flag)
				{
					num10.ApproximatelyEqualsTo(this.RudderDeflectionCoef, 1E-05f);
				}
				this.RudderDeflectionCoef = num10;
			}
			bool flag15;
			float num11 = this.DeserializeScalarAttribute<float>(node, "rudder_rotation_max", flag, out flag15) * 0.017453292f;
			if (flag15)
			{
				if (!flag)
				{
					num11.ApproximatelyEqualsTo(this.RudderRotationMax, 1E-05f);
				}
				this.RudderRotationMax = num11;
			}
			bool flag16;
			float num12 = this.DeserializeScalarAttribute<float>(node, "rudder_rotation_rate", flag, out flag16) * 0.017453292f;
			if (flag16)
			{
				if (!flag)
				{
					num12.ApproximatelyEqualsTo(this.RudderRotationRate, 1E-05f);
				}
				this.RudderRotationRate = num12;
			}
			bool flag17;
			float num13 = this.DeserializeScalarAttribute<float>(node, "rudder_force_max", flag, out flag17);
			if (flag17)
			{
				if (!flag)
				{
					num13.ApproximatelyEqualsTo(this.RudderForceMax, 1E-05f);
				}
				this.RudderForceMax = num13;
			}
			bool flag18;
			float num14 = this.DeserializeFloatAttribute(node, "max_linear_speed", flag, out flag18, 1f, 100f, "m/s");
			if (flag18)
			{
				if (!flag)
				{
					num14.ApproximatelyEqualsTo(this.MaxLinearSpeed, 1E-05f);
				}
				this.MaxLinearSpeed = num14;
			}
			bool flag19;
			float num15 = this.DeserializeFloatAttribute(node, "max_linear_acceleration", flag, out flag19, 0.1f, 50f, "m/s^2");
			if (flag19)
			{
				if (!flag)
				{
					num15.ApproximatelyEqualsTo(this.MaxLinearAccel, 1E-05f);
				}
				this.MaxLinearAccel = num15;
			}
			bool flag20;
			float num16 = this.DeserializeFloatAttribute(node, "max_angular_speed", flag, out flag20, 1f, 180f, "deg") * 0.017453292f;
			if (flag20)
			{
				if (!flag)
				{
					num16.ApproximatelyEqualsTo(this.MaxAngularSpeed, 1E-05f);
				}
				this.MaxAngularSpeed = num16;
			}
			bool flag21;
			float num17 = this.DeserializeFloatAttribute(node, "max_angular_acceleration", flag, out flag21, 1f, 180f, "deg/s") * 0.017453292f;
			if (flag21)
			{
				if (!flag)
				{
					num17.ApproximatelyEqualsTo(this.MaxAngularAccel, 1E-05f);
				}
				this.MaxAngularAccel = num17;
			}
			bool flag22;
			float num18 = this.DeserializeFloatAttribute(node, "bow_angle_limit_from_centerline", flag, out flag22, 0f, 90f, "");
			if (flag22)
			{
				if (!flag)
				{
					num18.ApproximatelyEqualsTo(this.BowAngleLimitFromCenterline, 1E-05f);
				}
				this.BowAngleLimitFromCenterline = num18;
			}
			bool flag23;
			float num19 = this.DeserializeFloatAttribute(node, "landing_depth", flag, out flag23, 0f, 90f, "");
			if (flag23)
			{
				if (!flag)
				{
					num19.ApproximatelyEqualsTo(this.LandingDepth, 1E-05f);
				}
				this.LandingDepth = num19;
			}
			bool flag24;
			Vec2 vec = this.Deserialize2DDimensionElement(node, "deployment_area", flag, out flag24);
			if (flag24)
			{
				if (!flag)
				{
					vec.NearlyEquals(this.DeploymentArea, 1E-05f);
				}
				this.DeploymentArea = vec;
			}
			bool flag25;
			MBList<ShipSail> mblist = this.DeserializeSailsElement(node, flag, out flag25);
			if (flag25)
			{
				if (!flag && mblist.Count == this._sails.Count)
				{
					for (int i = 0; i < this._sails.Count; i++)
					{
						if (mblist[i].NearlyEquals(this._sails[i]))
						{
						}
					}
				}
				this._sails = mblist;
			}
			bool flag26;
			Vec3 vec2 = this.DeserializeVectorElement(node, "moment_of_inertia_multiplier", false, out flag26);
			if (flag26)
			{
				if (!flag)
				{
					Vec3 vec3 = this.MomentOfInertiaMultiplier;
					vec2.NearlyEquals(in vec3, 1E-05f);
				}
				this.MomentOfInertiaMultiplier = vec2;
			}
			bool flag27;
			LinearFrictionTerm linearFrictionTerm = this.DeserializeLinearDragTermElement(node, "linear_friction_multiplier", out flag27);
			if (flag27)
			{
				if (!flag)
				{
					LinearFrictionTerm linearFrictionMultiplier = this.LinearFrictionMultiplier;
					linearFrictionTerm.NearlyEquals(in linearFrictionMultiplier, 1E-05f);
				}
				this.LinearFrictionMultiplier = linearFrictionTerm;
			}
			bool flag28;
			Vec3 vec4 = this.DeserializeAngularDragTermElement(node, "angular_friction_multiplier", out flag28);
			if (flag28)
			{
				if (!flag)
				{
					Vec3 vec3 = this.AngularFrictionMultiplier;
					vec4.NearlyEquals(in vec3, 1E-05f);
				}
				this.AngularFrictionMultiplier = vec4;
			}
			bool flag29;
			float num20 = this.DeserializeFloatAttribute(node, "lateral_drag_shift_max", false, out flag29, 0f, 100f, "m");
			if (flag29)
			{
				if (!flag)
				{
					num20.ApproximatelyEqualsTo(this.MaxLateralDragShift, 1E-05f);
				}
				this.MaxLateralDragShift = num20;
			}
			bool flag30;
			float num21 = this.DeserializeFloatAttribute(node, "lateral_drag_shift_critical_angle", false, out flag30, 0f, 90f, "deg") * 0.017453292f;
			if (flag30)
			{
				if (!flag)
				{
					num21.ApproximatelyEqualsTo(this.LateralDragShiftCriticalAngle, 1E-05f);
				}
				this.LateralDragShiftCriticalAngle = num21;
			}
			bool flag31;
			Vec3 vec5 = this.DeserializeVectorElement(node, "rudder_stock_position", false, out flag31);
			if (flag31)
			{
				if (!flag)
				{
					Vec3 vec3 = this.RudderStockPosition;
					vec5.NearlyEquals(in vec3, 1E-05f);
				}
				this.RudderStockPosition = vec5;
			}
			bool flag32;
			float num22 = this.DeserializeFloatAttribute(node, "maximum_submerged_volume_ratio", false, out flag32, float.MinValue, float.MaxValue, "");
			if (flag32)
			{
				if (!flag)
				{
					num22.ApproximatelyEqualsTo(this.MaximumSubmergedVolumeRatio, 1E-05f);
				}
				this.MaximumSubmergedVolumeRatio = num22;
			}
			bool flag33;
			float num23 = this.DeserializeFloatAttribute(node, "partial_hit_points_ratio", flag, out flag33, float.MinValue, float.MaxValue, "");
			if (flag33)
			{
				if (!flag)
				{
					num23.ApproximatelyEqualsTo(this.PartialHitPointsRatio, 1E-05f);
				}
				this.PartialHitPointsRatio = num23;
			}
			bool flag34;
			float num24 = this.DeserializeFloatAttribute(node, "torque_multiplier_of_lateral_buoyant_forces", false, out flag34, float.MinValue, float.MaxValue, "");
			if (flag34)
			{
				if (!flag)
				{
					num24.ApproximatelyEqualsTo(this.TorqueMultiplierOfLateralBuoyantForces, 1E-05f);
				}
				this.TorqueMultiplierOfLateralBuoyantForces = num24;
			}
			bool flag35;
			Vec3 vec6 = this.DeserializeVectorElement(node, "torque_multiplier_of_vertical_buoyant_forces", false, out flag35);
			if (flag35)
			{
				if (!flag)
				{
					Vec3 vec3 = this.TorqueMultiplierOfVerticalBuoyantForces;
					vec6.NearlyEquals(in vec3, 1E-05f);
				}
				this.TorqueMultiplierOfVerticalBuoyantForces = vec6;
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0002037C File Offset: 0x0001E57C
		private T DeserializeScalarAttribute<T>(XmlNode node, string attributeName, bool isRequiredAttribute, out bool isAttributeValid)
		{
			XmlAttribute xmlAttribute = node.Attributes[attributeName];
			isAttributeValid = xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.InnerText);
			if (isAttributeValid)
			{
				return (T)((object)Convert.ChangeType(xmlAttribute.Value, typeof(T)));
			}
			this.AssertFieldValidity(!isRequiredAttribute, attributeName);
			return default(T);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000203E4 File Offset: 0x0001E5E4
		private float DeserializeFloatAttribute(XmlNode node, string attributeName, bool isRequiredAttribute, out bool isAttributeValid, float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, string unitString = "")
		{
			float num = this.DeserializeScalarAttribute<float>(node, attributeName, isRequiredAttribute, out isAttributeValid);
			if (isAttributeValid)
			{
				if (num < minValue)
				{
					Debug.FailedAssert(string.Concat(new object[] { "ShipObject(", base.StringId, "): ", attributeName, " field is less than the required minimum value of ", minValue, " ", unitString, "." }), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MissionShipObject.cs", "DeserializeFloatAttribute", 692);
					num = minValue;
				}
				if (num > maxValue)
				{
					Debug.FailedAssert(string.Concat(new object[] { "ShipObject(", base.StringId, "): ", attributeName, " field is greater than the required maximum value of ", maxValue, " ", unitString, "." }), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MissionShipObject.cs", "DeserializeFloatAttribute", 698);
					num = maxValue;
				}
			}
			return num;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000204E0 File Offset: 0x0001E6E0
		private Vec2 Deserialize2DDimensionElement(XmlNode node, string elementName, bool isRequiredElement, out bool isElementValid)
		{
			Vec2 zero = Vec2.Zero;
			XmlNode xmlNode = node.SelectSingleNode(elementName);
			isElementValid = xmlNode != null;
			if (isElementValid)
			{
				using (IEnumerator enumerator = xmlNode.Attributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						string text = xmlAttribute.Name.ToLower();
						float num = float.Parse(xmlAttribute.Value);
						if (text == "width")
						{
							zero.x = num;
						}
						else if (text == "length" || text == "height")
						{
							zero.y = num;
						}
					}
					return zero;
				}
			}
			this.AssertFieldValidity(!isRequiredElement, elementName);
			return zero;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x000205B0 File Offset: 0x0001E7B0
		private Vec3 DeserializeVectorElement(XmlNode node, string elementName, bool isRequiredElement, out bool isElementValid)
		{
			Vec3 invalid = Vec3.Invalid;
			XmlNode xmlNode = node.SelectSingleNode(elementName);
			isElementValid = xmlNode != null;
			if (isElementValid)
			{
				using (IEnumerator enumerator = xmlNode.Attributes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						string text = xmlAttribute.Name.ToLower();
						float num = float.Parse(xmlAttribute.Value);
						if (text == "x")
						{
							invalid.x = num;
						}
						else if (text == "y")
						{
							invalid.y = num;
						}
						else if (text == "z")
						{
							invalid.z = num;
						}
					}
					return invalid;
				}
			}
			this.AssertFieldValidity(!isRequiredElement, elementName);
			return invalid;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0002068C File Offset: 0x0001E88C
		private LinearFrictionTerm DeserializeLinearDragTermElement(XmlNode node, string elementName, out bool isElementValid)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			XmlNode xmlNode = node.SelectSingleNode(elementName);
			isElementValid = xmlNode != null;
			if (isElementValid)
			{
				XmlAttributeCollection attributes = xmlNode.Attributes;
				if (attributes.Count != 5)
				{
					Debug.FailedAssert(string.Concat(new object[] { "ShipObject(", base.StringId, "): ", elementName, " element must have exactly ", 5, " attributes for directional drag multipliers" }), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MissionShipObject.cs", "DeserializeLinearDragTermElement", 798);
					isElementValid = false;
				}
				else
				{
					foreach (object obj in attributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						string text = xmlAttribute.Name.ToLower();
						string value = xmlAttribute.Value;
						if (text == "side")
						{
							num = float.Parse(value);
							num2 = num;
						}
						else if (text == "forward")
						{
							num3 = float.Parse(value);
						}
						else if (text == "backward")
						{
							num4 = float.Parse(value);
						}
						else if (text == "up")
						{
							num5 = float.Parse(value);
						}
						else if (text == "down")
						{
							num6 = float.Parse(value);
						}
					}
				}
			}
			LinearFrictionTerm linearFrictionTerm = new LinearFrictionTerm(num, num2, num3, num4, num5, num6);
			isElementValid = isElementValid && linearFrictionTerm.IsValid;
			return linearFrictionTerm;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00020844 File Offset: 0x0001EA44
		private Vec3 DeserializeAngularDragTermElement(XmlNode node, string elementName, out bool isElementValid)
		{
			Vec3 one = Vec3.One;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			XmlNode xmlNode = node.SelectSingleNode(elementName);
			isElementValid = xmlNode != null;
			if (isElementValid)
			{
				XmlAttributeCollection attributes = xmlNode.Attributes;
				if (attributes.Count != 3)
				{
					Debug.FailedAssert(string.Concat(new object[] { "ShipObject(", base.StringId, "): ", elementName, " element must have exactly ", 3, " attributes for angular drag friction multipliers" }), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\MissionShipObject.cs", "DeserializeAngularDragTermElement", 864);
					isElementValid = false;
				}
				else
				{
					foreach (object obj in attributes)
					{
						XmlAttribute xmlAttribute = (XmlAttribute)obj;
						string text = xmlAttribute.Name.ToLower();
						string value = xmlAttribute.Value;
						if (text == "pitch")
						{
							one.x = float.Parse(value);
							flag = true;
						}
						else if (text == "roll")
						{
							one.y = float.Parse(value);
							flag2 = true;
						}
						else if (text == "yaw")
						{
							one.z = float.Parse(value);
							flag3 = true;
						}
					}
				}
			}
			isElementValid = isElementValid && flag && flag2 && flag3;
			return one;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000209AC File Offset: 0x0001EBAC
		private MBList<ShipSail> DeserializeSailsElement(XmlNode node, bool hasAttributeRequirement, out bool isElementValid)
		{
			MBList<ShipSail> mblist = new MBList<ShipSail>();
			XmlNode xmlNode = node.SelectSingleNode("sails");
			isElementValid = xmlNode != null && xmlNode.ChildNodes.Count > 0;
			if (isElementValid)
			{
				for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
				{
					XmlNode xmlNode2 = xmlNode.ChildNodes[i];
					bool flag;
					SailType sailType = this.DeserializeSailTypeAttribute(xmlNode2, "type", true, out flag);
					float num = this.DeserializeFloatAttribute(xmlNode2, "force_multiplier", true, out flag, float.MinValue, float.MaxValue, "");
					float num2 = this.DeserializeFloatAttribute(xmlNode2, "left_rotation_limit", true, out flag, 0f, float.MaxValue, "deg") * 0.017453292f;
					float num3 = this.DeserializeFloatAttribute(xmlNode2, "right_rotation_limit", true, out flag, 0f, float.MaxValue, "deg") * 0.017453292f;
					float num4 = this.DeserializeFloatAttribute(xmlNode2, "rotation_rate", true, out flag, 0f, float.MaxValue, "deg/s") * 0.017453292f;
					ShipSail shipSail = new ShipSail(sailType, num, num2, num3, num4);
					mblist.Add(shipSail);
				}
			}
			return mblist;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00020AD0 File Offset: 0x0001ECD0
		private SailType DeserializeSailTypeAttribute(XmlNode node, string attributeName, bool isRequiredAttribute, out bool isAttributeValid)
		{
			XmlAttribute xmlAttribute = node.Attributes[attributeName];
			SailType sailType = SailType.Square;
			isAttributeValid = xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.InnerText) && Enum.TryParse<SailType>(xmlAttribute.Value, true, out sailType);
			if (!isAttributeValid)
			{
				this.AssertFieldValidity(!isRequiredAttribute, attributeName);
			}
			return sailType;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00020B21 File Offset: 0x0001ED21
		private void AssertFieldValidity(bool assert, string fieldName)
		{
		}

		// Token: 0x04000569 RID: 1385
		private MBList<ShipSail> _sails = new MBList<ShipSail>();
	}
}
