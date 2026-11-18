using System;
using Google.Protobuf.Reflection;

public static class ClientBattleProtoReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static ClientBattleProtoReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChdDbGllbnRCYXR0bGVQcm90by5wcm90byI/ChpCYXR0bGVSb2xlRW50ZXJS" + "ZXFfODAwMTAwMRIRCglkdW5nZW9uSWQYASABKAkSDgoGcm9sZUlkGAIgASgJ" + "IvsNChtCYXR0bGVSb2xlRW50ZXJSZXNwXzgwMDEwMDESDAoEY29kZRgBIAEo" + "BRIRCglpc1N1Y2Nlc3MYAiABKAgSDwoHbWFwTWluWBgDIAEoARIPCgdtYXBN" + "YXhYGAQgASgBEg8KB21hcE1pblkYBSABKAESDwoHbWFwTWF4WRgGIAEoARIM" + "CgRwb3NYGAcgASgBEgwKBHBvc1kYCCABKAESDQoFYW5nbGUYCSABKAESRAoH" + "b2JqZWN0cxgKIAMoCzIzLkJhdHRsZVJvbGVFbnRlclJlc3BfODAwMTAwMS5P" + "YmplY3RJbmZvUmVzcF84MDAxMDAyEkYKBnBsYW50cxgLIAMoCzI2LkJhdHRs" + "ZVJvbGVFbnRlclJlc3BfODAwMTAwMS5Ecm9wR29vZHNJbmZvUmVzcF84MDAx" + "MDEzEkgKCG1pbmVyYWxzGAwgAygLMjYuQmF0dGxlUm9sZUVudGVyUmVzcF84" + "MDAxMDAxLkRyb3BHb29kc0luZm9SZXNwXzgwMDEwMTMSEQoJcm9sZUluZGV4" + "GA0gASgFEkoKCnNlY3JldEFyZWEYDiABKAsyNi5CYXR0bGVSb2xlRW50ZXJS" + "ZXNwXzgwMDEwMDEuRHJvcEdvb2RzSW5mb1Jlc3BfODAwMTAxMxIQCghvdmVy" + "VGltZRgPIAEoAxrOCAoWT2JqZWN0SW5mb1Jlc3BfODAwMTAwMhITCgtvYmpl" + "Y3RJbmRleBgBIAEoBRIMCgRwb3NYGAIgASgBEgwKBHBvc1kYAyABKAESDQoF" + "YW5nbGUYBCABKAESEwoLZml2ZUVsZW1lbnQYBSABKAUSEgoKb2JqZWN0VHlw" + "ZRgGIAEoBRIMCgRuYW1lGAcgASgJEg0KBWxldmVsGAggASgFEh4KFmN1cnJl" + "bnRQaHlzaXF1ZURlZmVuc2UYCSABKAMSHwoXY3VycmVudFB1cmVCbG9vZERl" + "ZmVuc2UYCiABKAUSFwoPY3VycmVudFN1cGVyQWlyGAsgASgDEhkKEWN1cnJl" + "bnRQZXJjZXB0aW9uGAwgASgFEhgKEGN1cnJlbnRNb3ZlU3BlZWQYDSABKAES" + "DAoEaWNvbhgOIAEoBRIcChRzdXBlckFpclJlY292ZXJWYWx1ZRgPIAEoARIb" + "ChNkZWZlbnNlUmVjb3ZlclZhbHVlGBAgASgBEiQKHHN1cGVyQWlyUmVjb3Zl" + "clZhbHVlT25CYXR0bGUYESABKAESIwobZGVmZW5zZVJlY292ZXJWYWx1ZU9u" + "QmF0dGxlGBIgASgBEhUKDXN1cGVyQWlyUG93ZXIYEyABKAESXwoGc2tpbGxz" + "GBQgAygLMk8uQmF0dGxlUm9sZUVudGVyUmVzcF84MDAxMDAxLk9iamVjdElu" + "Zm9SZXNwXzgwMDEwMDIuT2JqZWN0SW5mb1NraWxsUmVzcF84MDAxMDAzEhYK" + "DmF0dGFja0Rpc3RhbmNlGBUgASgBEhQKDHVudXN1YWxMZXZlbBgWIAEoBRrk" + "AwobT2JqZWN0SW5mb1NraWxsUmVzcF84MDAxMDAzEgwKBGljb24YASABKAUS" + "DAoEdHlwZRgCIAEoBRIRCglza2lsbFR5cGUYDCABKAUSDAoEbmFtZRgDIAEo" + "CRISCgp0ZW1wbGF0ZUlkGAQgASgJEhAKCGNhc3RUaW1lGAUgASgFEhQKDGNv" + "bnRpbnVlVGltZRgGIAEoBRIQCghjb29sRG93bhgHIAEoBRITCgtmaXZlRWxl" + "bWVudBgIIAEoBRIWCg5jb252ZXJzaW9uUmF0ZRgJIAEoARISCgpiYXNlRGFt" + "YWdlGAogASgBEhYKDnVzZWRQZXJjZXB0aW9uGAsgASgFEg0KBWxldmVsGA0g" + "ASgFEhIKCnNraWxsSW5kZXgYDiABKAUSFAoMdXNlZFN1cGVyQWlyGA8gASgD" + "EhMKC2NhblVzZUNvdW50GBAgASgFEgwKBHJhbmsYESABKAUSEAoIdW5pdFVz" + "ZWQYEiABKAESFQoNZmlyZURhbWFnZUFkZBgTIAEoARIVCg13b29kRGFtYWdl" + "QWRkGBQgASgBEhYKDndhdGVyRGFtYWdlQWRkGBUgASgBEhUKDWdvbGREYW1h" + "Z2VBZGQYFiABKAESFgoOZWFydGhEYW1hZ2VBZGQYFyABKAEasQEKGURyb3BH" + "b29kc0luZm9SZXNwXzgwMDEwMTMSEwoLb2JqZWN0SW5kZXgYASABKAUSDAoE" + "cG9zWBgCIAEoARIMCgRwb3NZGAMgASgBEhMKC2ZpdmVFbGVtZW50GAUgASgF" + "EhIKCm9iamVjdFR5cGUYBiABKAUSDAoEbmFtZRgHIAEoCRINCgVsZXZlbBgI" + "IAEoBRIMCgRyYW5rGAkgASgFEg8KB2dvb2RzSWQYCiABKAkiWQoZQmF0dGxl" + "Um9sZU1vdmVSZXFfODAwMTAwNBIRCglyb2xlSW5kZXgYASABKAUSDAoEcG9z" + "WBgCIAEoARIMCgRwb3NZGAMgASgBEg0KBWFuZ2xlGAQgASgBImEKHUJhdHRs" + "ZVJvbGVVc2VTa2lsbFJlcV84MDAxMDA1EhEKCXJvbGVJbmRleBgBIAEoBRIS" + "Cgpza2lsbEluZGV4GAIgASgFEhkKEXRhcmdldE9iamVjdEluZGV4GAMgASgF" + "IlEKHkJhdHRsZVJvbGVVc2VTa2lsbFJlc3BfODAwMTAwNRIMCgRjb2RlGAEg" + "ASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIOCgZkYW1hZ2UYAyABKAMiagobTW9u" + "c3RlckdldERhbWFnZVJlcV84MDAxMDA2EhQKDG1vbnN0ZXJJbmRleBgBIAEo" + "BRIRCglyb2xlSW5kZXgYAiABKAUSEgoKc2tpbGxJbmRleBgDIAEoBRIOCgZk" + "YW1hZ2UYBCABKAUiPwocTW9uc3RlckdldERhbWFnZVJlc3BfODAwMTAwNhIM" + "CgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCCIvChhSb2xlUGlja0dv" + "b2RzUmVxXzgwMDEwMDcSEwoLb2JqZWN0SW5kZXgYASABKAUiPAoZUm9sZVBp" + "Y2tHb29kc1Jlc3BfODAwMTAwNxIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2Vz" + "cxgCIAEoCCJDChtSb2xlTGVhdmVEdW5nZW9uUmVxXzgwMDEwMDgSEQoJZHVu" + "Z2VvbklkGAEgASgJEhEKCXJvbGVJbmRleBgCIAEoBSI/ChxSb2xlTGVhdmVE" + "dW5nZW9uUmVzcF84MDAxMDA4EgwKBGNvZGUYASABKAUSEQoJaXNTdWNjZXNz" + "GAIgASgIIiAKHkVudGVyVG93ZXJOZXh0TGV2ZWxSZXFfODAwMTAwOSLgCQof" + "RW50ZXJUb3dlck5leHRMZXZlbFJlc3BfODAwMTAwORIMCgRjb2RlGAEgASgF" + "EhEKCWlzU3VjY2VzcxgCIAEoCBIPCgdtYXBNaW5YGAMgASgBEg8KB21hcE1h" + "eFgYBCABKAESDwoHbWFwTWluWRgFIAEoARIPCgdtYXBNYXhZGAYgASgBEgwK" + "BHBvc1gYByABKAESDAoEcG9zWRgIIAEoARINCgVhbmdsZRgJIAEoARJICgdv" + "YmplY3RzGAogAygLMjcuRW50ZXJUb3dlck5leHRMZXZlbFJlc3BfODAwMTAw" + "OS5PYmplY3RJbmZvUmVzcF84MDAxMDEwEhEKCXJvbGVJbmRleBgLIAEoBRIQ" + "CghvdmVyVGltZRgMIAEoAxq9BwoWT2JqZWN0SW5mb1Jlc3BfODAwMTAxMBIT" + "CgtvYmplY3RJbmRleBgBIAEoBRIMCgRwb3NYGAIgASgBEgwKBHBvc1kYAyAB" + "KAESDQoFYW5nbGUYBCABKAESEwoLZml2ZUVsZW1lbnQYBSABKAUSEgoKb2Jq" + "ZWN0VHlwZRgGIAEoBRIMCgRuYW1lGAcgASgJEg0KBWxldmVsGAggASgFEh4K" + "FmN1cnJlbnRQaHlzaXF1ZURlZmVuc2UYCSABKAMSHwoXY3VycmVudFB1cmVC" + "bG9vZERlZmVuc2UYCiABKAUSFwoPY3VycmVudFN1cGVyQWlyGAsgASgDEhkK" + "EWN1cnJlbnRQZXJjZXB0aW9uGAwgASgFEhgKEGN1cnJlbnRNb3ZlU3BlZWQY" + "DSABKAESDAoEaWNvbhgOIAEoBRIcChRzdXBlckFpclJlY292ZXJWYWx1ZRgP" + "IAEoARIbChNkZWZlbnNlUmVjb3ZlclZhbHVlGBAgASgBEiQKHHN1cGVyQWly" + "UmVjb3ZlclZhbHVlT25CYXR0bGUYESABKAESIwobZGVmZW5zZVJlY292ZXJW" + "YWx1ZU9uQmF0dGxlGBIgASgBEhUKDXN1cGVyQWlyUG93ZXIYEyABKAESYwoG" + "c2tpbGxzGBQgAygLMlMuRW50ZXJUb3dlck5leHRMZXZlbFJlc3BfODAwMTAw" + "OS5PYmplY3RJbmZvUmVzcF84MDAxMDEwLk9iamVjdEluZm9Ta2lsbFJlc3Bf" + "ODAwMTAxMhIWCg5hdHRhY2tEaXN0YW5jZRgVIAEoARIUCgx1bnVzdWFsTGV2" + "ZWwYFiABKAUazwIKG09iamVjdEluZm9Ta2lsbFJlc3BfODAwMTAxMhIMCgRp" + "Y29uGAEgASgFEgwKBHR5cGUYAiABKAUSDAoEbmFtZRgDIAEoCRISCgp0ZW1w" + "bGF0ZUlkGAQgASgJEhAKCGNhc3RUaW1lGAUgASgFEhQKDGNvbnRpbnVlVGlt" + "ZRgGIAEoBRIQCghjb29sRG93bhgHIAEoBRITCgtmaXZlRWxlbWVudBgIIAEo" + "BRIWCg5jb252ZXJzaW9uUmF0ZRgJIAEoARISCgpiYXNlRGFtYWdlGAogASgB" + "Eg0KBWxldmVsGA0gASgFEhEKCXNraWxsVHlwZRgMIAEoBRIWCg51c2VkUGVy" + "Y2VwdGlvbhgLIAEoBRISCgpza2lsbEluZGV4GA4gASgFEhQKDHVzZWRTdXBl" + "ckFpchgPIAEoAxITCgtjYW5Vc2VDb3VudBgQIAEoBSJcChhSb2xlR2V0RGFt" + "YWdlUmVxXzgwMDEwMTQSDgoGYmVmb3JlGAEgASgDEg0KBWFmdGVyGAIgASgD" + "Eg4KBmRhbWFnZRgDIAEoAxIRCglyb2xlSW5kZXgYBCABKAUiJgoRUmV2aXZl" + "UmVxXzgwMDEwMTESEQoJcm9sZUluZGV4GAEgASgFImwKElJldml2ZVJlc3Bf" + "ODAwMTAxMRIMCgRjb2RlGAEgASgFEhEKCWlzU3VjY2VzcxgCIAEoCBIZChF0" + "aVNoZW5LdWlMZWlDb3VudBgDIAEoBRIMCgRwb3NYGAQgASgBEgwKBHBvc1kY" + "BSABKAEizAEKHkJhdHRsZUJyb2FkY2FzdFBvc1Jlc3BfODAwMjAwMRJOCgxv" + "YmplY3RQb3NpbnMYASADKAsyOC5CYXR0bGVCcm9hZGNhc3RQb3NSZXNwXzgw" + "MDIwMDEuT2JqZWN0UG9zaW9uUmVzcF84MDAyMDAyGloKGE9iamVjdFBvc2lv" + "blJlc3BfODAwMjAwMhITCgtvYmplY3RJbmRleBgBIAEoBRIMCgRwb3NYGAIg" + "ASgBEgwKBHBvc1kYAyABKAESDQoFYW5nbGUYBCABKAEiTgoWTW9uc3RlckRl" + "YWRSZXFfODAwMjAwMxIYChBkZWFkTW9uc3RlckluZGV4GAEgASgFEgwKBHBv" + "c1gYAiABKAESDAoEcG9zWRgDIAEoASLXAQoXTW9uc3RlckRlYWRSZXNwXzgw" + "MDIwMDMSGAoQZGVhZE1vbnN0ZXJJbmRleBgBIAEoBRIPCgdnb29kc0lkGAIg" + "ASgJEgwKBHBvc1gYAyABKAESDAoEcG9zWRgEIAEoARITCgtmaXZlRWxlbWVu" + "dBgFIAEoBRIRCglnb29kc1R5cGUYBiABKAUSDAoEbmFtZRgHIAEoCRINCgVs" + "ZXZlbBgIIAEoBRIMCgRyYW5rGAkgASgFEhMKC29iamVjdEluZGV4GAogASgF" + "Eg0KBWNvdW50GAsgASgFQjAKG2NvbS5sb25nbW8uYmF0dGxlLm5ldC5wcm90" + "b0IRQ2xpZW50QmF0dGxlUHJvdG9iBnByb3RvMw=="), new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[19]
		{
			new GeneratedClrTypeInfo(typeof(BattleRoleEnterReq_8001001), BattleRoleEnterReq_8001001.Parser, new string[2] { "DungeonId", "RoleId" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001), BattleRoleEnterResp_8001001.Parser, new string[15]
			{
				"Code", "IsSuccess", "MapMinX", "MapMaxX", "MapMinY", "MapMaxY", "PosX", "PosY", "Angle", "Objects",
				"Plants", "Minerals", "RoleIndex", "SecretArea", "OverTime"
			}, null, null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002), BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Parser, new string[22]
				{
					"ObjectIndex", "PosX", "PosY", "Angle", "FiveElement", "ObjectType", "Name", "Level", "CurrentPhysiqueDefense", "CurrentPureBloodDefense",
					"CurrentSuperAir", "CurrentPerception", "CurrentMoveSpeed", "Icon", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle", "DefenseRecoverValueOnBattle", "SuperAirPower", "Skills",
					"AttackDistance", "UnusualLevel"
				}, null, null, new GeneratedClrTypeInfo[1]
				{
					new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Types.ObjectInfoSkillResp_8001003), BattleRoleEnterResp_8001001.Types.ObjectInfoResp_8001002.Types.ObjectInfoSkillResp_8001003.Parser, new string[23]
					{
						"Icon", "Type", "SkillType", "Name", "TemplateId", "CastTime", "ContinueTime", "CoolDown", "FiveElement", "ConversionRate",
						"BaseDamage", "UsedPerception", "Level", "SkillIndex", "UsedSuperAir", "CanUseCount", "Rank", "UnitUsed", "FireDamageAdd", "WoodDamageAdd",
						"WaterDamageAdd", "GoldDamageAdd", "EarthDamageAdd"
					}, null, null, null)
				}),
				new GeneratedClrTypeInfo(typeof(BattleRoleEnterResp_8001001.Types.DropGoodsInfoResp_8001013), BattleRoleEnterResp_8001001.Types.DropGoodsInfoResp_8001013.Parser, new string[9] { "ObjectIndex", "PosX", "PosY", "FiveElement", "ObjectType", "Name", "Level", "Rank", "GoodsId" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(BattleRoleMoveReq_8001004), BattleRoleMoveReq_8001004.Parser, new string[4] { "RoleIndex", "PosX", "PosY", "Angle" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleRoleUseSkillReq_8001005), BattleRoleUseSkillReq_8001005.Parser, new string[3] { "RoleIndex", "SkillIndex", "TargetObjectIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleRoleUseSkillResp_8001005), BattleRoleUseSkillResp_8001005.Parser, new string[3] { "Code", "IsSuccess", "Damage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MonsterGetDamageReq_8001006), MonsterGetDamageReq_8001006.Parser, new string[4] { "MonsterIndex", "RoleIndex", "SkillIndex", "Damage" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MonsterGetDamageResp_8001006), MonsterGetDamageResp_8001006.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RolePickGoodsReq_8001007), RolePickGoodsReq_8001007.Parser, new string[1] { "ObjectIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RolePickGoodsResp_8001007), RolePickGoodsResp_8001007.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleLeaveDungeonReq_8001008), RoleLeaveDungeonReq_8001008.Parser, new string[2] { "DungeonId", "RoleIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(RoleLeaveDungeonResp_8001008), RoleLeaveDungeonResp_8001008.Parser, new string[2] { "Code", "IsSuccess" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelReq_8001009), EnterTowerNextLevelReq_8001009.Parser, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelResp_8001009), EnterTowerNextLevelResp_8001009.Parser, new string[12]
			{
				"Code", "IsSuccess", "MapMinX", "MapMaxX", "MapMinY", "MapMaxY", "PosX", "PosY", "Angle", "Objects",
				"RoleIndex", "OverTime"
			}, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010), EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Parser, new string[22]
				{
					"ObjectIndex", "PosX", "PosY", "Angle", "FiveElement", "ObjectType", "Name", "Level", "CurrentPhysiqueDefense", "CurrentPureBloodDefense",
					"CurrentSuperAir", "CurrentPerception", "CurrentMoveSpeed", "Icon", "SuperAirRecoverValue", "DefenseRecoverValue", "SuperAirRecoverValueOnBattle", "DefenseRecoverValueOnBattle", "SuperAirPower", "Skills",
					"AttackDistance", "UnusualLevel"
				}, null, null, new GeneratedClrTypeInfo[1]
				{
					new GeneratedClrTypeInfo(typeof(EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Types.ObjectInfoSkillResp_8001012), EnterTowerNextLevelResp_8001009.Types.ObjectInfoResp_8001010.Types.ObjectInfoSkillResp_8001012.Parser, new string[16]
					{
						"Icon", "Type", "Name", "TemplateId", "CastTime", "ContinueTime", "CoolDown", "FiveElement", "ConversionRate", "BaseDamage",
						"Level", "SkillType", "UsedPerception", "SkillIndex", "UsedSuperAir", "CanUseCount"
					}, null, null, null)
				})
			}),
			new GeneratedClrTypeInfo(typeof(RoleGetDamageReq_8001014), RoleGetDamageReq_8001014.Parser, new string[4] { "Before", "After", "Damage", "RoleIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReviveReq_8001011), ReviveReq_8001011.Parser, new string[1] { "RoleIndex" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(ReviveResp_8001011), ReviveResp_8001011.Parser, new string[5] { "Code", "IsSuccess", "TiShenKuiLeiCount", "PosX", "PosY" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleBroadcastPosResp_8002001), BattleBroadcastPosResp_8002001.Parser, new string[1] { "ObjectPosins" }, null, null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(BattleBroadcastPosResp_8002001.Types.ObjectPosionResp_8002002), BattleBroadcastPosResp_8002001.Types.ObjectPosionResp_8002002.Parser, new string[4] { "ObjectIndex", "PosX", "PosY", "Angle" }, null, null, null)
			}),
			new GeneratedClrTypeInfo(typeof(MonsterDeadReq_8002003), MonsterDeadReq_8002003.Parser, new string[3] { "DeadMonsterIndex", "PosX", "PosY" }, null, null, null),
			new GeneratedClrTypeInfo(typeof(MonsterDeadResp_8002003), MonsterDeadResp_8002003.Parser, new string[11]
			{
				"DeadMonsterIndex", "GoodsId", "PosX", "PosY", "FiveElement", "GoodsType", "Name", "Level", "Rank", "ObjectIndex",
				"Count"
			}, null, null, null)
		}));
	}
}
