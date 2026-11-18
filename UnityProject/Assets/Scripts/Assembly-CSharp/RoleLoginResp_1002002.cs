using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

public sealed class RoleLoginResp_1002002 : IMessage<RoleLoginResp_1002002>, IMessage, IEquatable<RoleLoginResp_1002002>, IDeepCloneable<RoleLoginResp_1002002>
{
	private static readonly MessageParser<RoleLoginResp_1002002> _parser = new MessageParser<RoleLoginResp_1002002>(() => new RoleLoginResp_1002002());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int IdFieldNumber = 3;

	private string id_ = "";

	public const int MapIdFieldNumber = 4;

	private string mapId_ = "";

	public const int MapNameFieldNumber = 5;

	private string mapName_ = "";

	public const int SexFieldNumber = 6;

	private int sex_;

	public const int HeadPortraitIdFieldNumber = 7;

	private int headPortraitId_;

	public const int NameFieldNumber = 8;

	private string name_ = "";

	public const int LevelFieldNumber = 9;

	private int level_;

	public const int CurrentAgeFieldNumber = 10;

	private int currentAge_;

	public const int MaxAgeFieldNumber = 11;

	private int maxAge_;

	public const int ShinyStoneFieldNumber = 12;

	private long shinyStone_;

	public const int ShinyJadeFieldNumber = 13;

	private int shinyJade_;

	public const int PureBloodFieldNumber = 14;

	private int pureBlood_;

	public const int FlexibleRootFieldNumber = 15;

	private string flexibleRoot_ = "";

	public const int SuperAirFieldNumber = 16;

	private long superAir_;

	public const int PerceptionFieldNumber = 17;

	private long perception_;

	public const int PhysiqueFieldNumber = 18;

	private long physique_;

	public const int CurrentSuperAirFieldNumber = 19;

	private long currentSuperAir_;

	public const int CurrentPerceptionFieldNumber = 20;

	private long currentPerception_;

	public const int CurrentPhysiqueFieldNumber = 21;

	private long currentPhysique_;

	public const int CurrentVigorFieldNumber = 22;

	private int currentVigor_;

	public const int CurrentCleverFieldNumber = 23;

	private int currentClever_;

	public const int CurrentPoisonValFieldNumber = 24;

	private int currentPoisonVal_;

	public const int CurrentPureBloodFieldNumber = 25;

	private int currentPureBlood_;

	public const int LiveRelIdFieldNumber = 26;

	private int liveRelId_;

	public const int Live1TypeFieldNumber = 27;

	private int live1Type_;

	public const int Live2TypeFieldNumber = 28;

	private int live2Type_;

	public const int Live1LevelFieldNumber = 29;

	private int live1Level_;

	public const int Live2LevelFieldNumber = 30;

	private int live2Level_;

	public const int Live1ValueFieldNumber = 31;

	private int live1Value_;

	public const int Live2ValueFieldNumber = 32;

	private int live2Value_;

	public const int YueKaDaysFieldNumber = 33;

	private int yueKaDays_;

	public const int TitleFieldNumber = 34;

	private int title_;

	public const int HasWoodFieldNumber = 35;

	private bool hasWood_;

	public const int HasGoldFieldNumber = 36;

	private bool hasGold_;

	public const int HasWaterFieldNumber = 37;

	private bool hasWater_;

	public const int HasFireFieldNumber = 38;

	private bool hasFire_;

	public const int HasEarthFieldNumber = 39;

	private bool hasEarth_;

	public const int BattleGuideValFieldNumber = 40;

	private int battleGuideVal_;

	public const int PracticeGuideValFieldNumber = 41;

	private int practiceGuideVal_;

	public const int IsNewLetterFieldNumber = 42;

	private bool isNewLetter_;

	public const int AcupointGuideValFieldNumber = 43;

	private int acupointGuideVal_;

	public const int ShopGuideValFieldNumber = 44;

	private int shopGuideVal_;

	public const int HunLiValFieldNumber = 45;

	private int hunLiVal_;

	public const int SignGuideValFieldNumber = 46;

	private int signGuideVal_;

	public const int ElixirGuideValFieldNumber = 47;

	private int elixirGuideVal_;

	public const int IsGetSignInFieldNumber = 48;

	private bool isGetSignIn_;

	public const int SignInDayFieldNumber = 49;

	private int signInDay_;

	public const int GuildIdFieldNumber = 50;

	private int guildId_;

	public const int GuildNameFieldNumber = 51;

	private string guildName_ = "";

	public const int GuildSimpleNameFieldNumber = 52;

	private string guildSimpleName_ = "";

	public const int TokenIndexFieldNumber = 53;

	private int tokenIndex_;

	public const int LastNameIndexFieldNumber = 54;

	private int lastNameIndex_;

	public const int TaskIdFieldNumber = 55;

	private int taskId_;

	public const int TaskStatusFieldNumber = 56;

	private int taskStatus_;

	public const int TianShuBeiStatusFieldNumber = 57;

	private int tianShuBeiStatus_;

	public const int PowerFieldNumber = 58;

	private int power_;

	public const int CurrentPowerFieldNumber = 59;

	private int currentPower_;

	public const int VigorFieldNumber = 60;

	private int vigor_;

	public const int CleverFieldNumber = 61;

	private int clever_;

	public const int Live3TypeFieldNumber = 62;

	private int live3Type_;

	public const int Live3LevelFieldNumber = 63;

	private int live3Level_;

	public const int Live3ValueFieldNumber = 64;

	private int live3Value_;

	public const int IsTianDaoLingFieldNumber = 65;

	private bool isTianDaoLing_;

	public const int TitleIdFieldNumber = 66;

	private static readonly FieldCodec<int> _repeated_titleId_codec = FieldCodec.ForInt32(530u);

	private readonly RepeatedField<int> titleId_ = new RepeatedField<int>();

	public const int QiYunFieldNumber = 67;

	private int qiYun_;

	public const int Live4TypeFieldNumber = 68;

	private int live4Type_;

	public const int Live4LevelFieldNumber = 69;

	private int live4Level_;

	public const int Live4ValueFieldNumber = 70;

	private int live4Value_;

	public const int LeiChongFieldNumber = 71;

	private int leiChong_;

	public const int VipLevelFieldNumber = 72;

	private int vipLevel_;

	public const int TitleFrameFieldNumber = 73;

	private int titleFrame_;

	public const int HeadFrameFieldNumber = 74;

	private int headFrame_;

	public const int IsHaoYunFuFieldNumber = 75;

	private bool isHaoYunFu_;

	[DebuggerNonUserCode]
	public static MessageParser<RoleLoginResp_1002002> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => RoleProtoReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor();

	[DebuggerNonUserCode]
	public int Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsSuccess
	{
		get
		{
			return isSuccess_;
		}
		set
		{
			isSuccess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string MapId
	{
		get
		{
			return mapId_;
		}
		set
		{
			mapId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string MapName
	{
		get
		{
			return mapName_;
		}
		set
		{
			mapName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Sex
	{
		get
		{
			return sex_;
		}
		set
		{
			sex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadPortraitId
	{
		get
		{
			return headPortraitId_;
		}
		set
		{
			headPortraitId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentAge
	{
		get
		{
			return currentAge_;
		}
		set
		{
			currentAge_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxAge
	{
		get
		{
			return maxAge_;
		}
		set
		{
			maxAge_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ShinyStone
	{
		get
		{
			return shinyStone_;
		}
		set
		{
			shinyStone_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ShinyJade
	{
		get
		{
			return shinyJade_;
		}
		set
		{
			shinyJade_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PureBlood
	{
		get
		{
			return pureBlood_;
		}
		set
		{
			pureBlood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string FlexibleRoot
	{
		get
		{
			return flexibleRoot_;
		}
		set
		{
			flexibleRoot_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long SuperAir
	{
		get
		{
			return superAir_;
		}
		set
		{
			superAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Perception
	{
		get
		{
			return perception_;
		}
		set
		{
			perception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Physique
	{
		get
		{
			return physique_;
		}
		set
		{
			physique_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentSuperAir
	{
		get
		{
			return currentSuperAir_;
		}
		set
		{
			currentSuperAir_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentPerception
	{
		get
		{
			return currentPerception_;
		}
		set
		{
			currentPerception_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentPhysique
	{
		get
		{
			return currentPhysique_;
		}
		set
		{
			currentPhysique_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentVigor
	{
		get
		{
			return currentVigor_;
		}
		set
		{
			currentVigor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentClever
	{
		get
		{
			return currentClever_;
		}
		set
		{
			currentClever_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPoisonVal
	{
		get
		{
			return currentPoisonVal_;
		}
		set
		{
			currentPoisonVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPureBlood
	{
		get
		{
			return currentPureBlood_;
		}
		set
		{
			currentPureBlood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LiveRelId
	{
		get
		{
			return liveRelId_;
		}
		set
		{
			liveRelId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live1Type
	{
		get
		{
			return live1Type_;
		}
		set
		{
			live1Type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live2Type
	{
		get
		{
			return live2Type_;
		}
		set
		{
			live2Type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live1Level
	{
		get
		{
			return live1Level_;
		}
		set
		{
			live1Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live2Level
	{
		get
		{
			return live2Level_;
		}
		set
		{
			live2Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live1Value
	{
		get
		{
			return live1Value_;
		}
		set
		{
			live1Value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live2Value
	{
		get
		{
			return live2Value_;
		}
		set
		{
			live2Value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int YueKaDays
	{
		get
		{
			return yueKaDays_;
		}
		set
		{
			yueKaDays_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Title
	{
		get
		{
			return title_;
		}
		set
		{
			title_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasWood
	{
		get
		{
			return hasWood_;
		}
		set
		{
			hasWood_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasGold
	{
		get
		{
			return hasGold_;
		}
		set
		{
			hasGold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasWater
	{
		get
		{
			return hasWater_;
		}
		set
		{
			hasWater_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasFire
	{
		get
		{
			return hasFire_;
		}
		set
		{
			hasFire_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasEarth
	{
		get
		{
			return hasEarth_;
		}
		set
		{
			hasEarth_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BattleGuideVal
	{
		get
		{
			return battleGuideVal_;
		}
		set
		{
			battleGuideVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PracticeGuideVal
	{
		get
		{
			return practiceGuideVal_;
		}
		set
		{
			practiceGuideVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsNewLetter
	{
		get
		{
			return isNewLetter_;
		}
		set
		{
			isNewLetter_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AcupointGuideVal
	{
		get
		{
			return acupointGuideVal_;
		}
		set
		{
			acupointGuideVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ShopGuideVal
	{
		get
		{
			return shopGuideVal_;
		}
		set
		{
			shopGuideVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HunLiVal
	{
		get
		{
			return hunLiVal_;
		}
		set
		{
			hunLiVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SignGuideVal
	{
		get
		{
			return signGuideVal_;
		}
		set
		{
			signGuideVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ElixirGuideVal
	{
		get
		{
			return elixirGuideVal_;
		}
		set
		{
			elixirGuideVal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsGetSignIn
	{
		get
		{
			return isGetSignIn_;
		}
		set
		{
			isGetSignIn_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SignInDay
	{
		get
		{
			return signInDay_;
		}
		set
		{
			signInDay_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int GuildId
	{
		get
		{
			return guildId_;
		}
		set
		{
			guildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string GuildName
	{
		get
		{
			return guildName_;
		}
		set
		{
			guildName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string GuildSimpleName
	{
		get
		{
			return guildSimpleName_;
		}
		set
		{
			guildSimpleName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int TokenIndex
	{
		get
		{
			return tokenIndex_;
		}
		set
		{
			tokenIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LastNameIndex
	{
		get
		{
			return lastNameIndex_;
		}
		set
		{
			lastNameIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TaskId
	{
		get
		{
			return taskId_;
		}
		set
		{
			taskId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TaskStatus
	{
		get
		{
			return taskStatus_;
		}
		set
		{
			taskStatus_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TianShuBeiStatus
	{
		get
		{
			return tianShuBeiStatus_;
		}
		set
		{
			tianShuBeiStatus_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Power
	{
		get
		{
			return power_;
		}
		set
		{
			power_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentPower
	{
		get
		{
			return currentPower_;
		}
		set
		{
			currentPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Vigor
	{
		get
		{
			return vigor_;
		}
		set
		{
			vigor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Clever
	{
		get
		{
			return clever_;
		}
		set
		{
			clever_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live3Type
	{
		get
		{
			return live3Type_;
		}
		set
		{
			live3Type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live3Level
	{
		get
		{
			return live3Level_;
		}
		set
		{
			live3Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live3Value
	{
		get
		{
			return live3Value_;
		}
		set
		{
			live3Value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsTianDaoLing
	{
		get
		{
			return isTianDaoLing_;
		}
		set
		{
			isTianDaoLing_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> TitleId => titleId_;

	[DebuggerNonUserCode]
	public int QiYun
	{
		get
		{
			return qiYun_;
		}
		set
		{
			qiYun_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live4Type
	{
		get
		{
			return live4Type_;
		}
		set
		{
			live4Type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live4Level
	{
		get
		{
			return live4Level_;
		}
		set
		{
			live4Level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Live4Value
	{
		get
		{
			return live4Value_;
		}
		set
		{
			live4Value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LeiChong
	{
		get
		{
			return leiChong_;
		}
		set
		{
			leiChong_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int VipLevel
	{
		get
		{
			return vipLevel_;
		}
		set
		{
			vipLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TitleFrame
	{
		get
		{
			return titleFrame_;
		}
		set
		{
			titleFrame_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadFrame
	{
		get
		{
			return headFrame_;
		}
		set
		{
			headFrame_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsHaoYunFu
	{
		get
		{
			return isHaoYunFu_;
		}
		set
		{
			isHaoYunFu_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public RoleLoginResp_1002002()
	{
	}

	[DebuggerNonUserCode]
	public RoleLoginResp_1002002(RoleLoginResp_1002002 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		id_ = other.id_;
		mapId_ = other.mapId_;
		mapName_ = other.mapName_;
		sex_ = other.sex_;
		headPortraitId_ = other.headPortraitId_;
		name_ = other.name_;
		level_ = other.level_;
		currentAge_ = other.currentAge_;
		maxAge_ = other.maxAge_;
		shinyStone_ = other.shinyStone_;
		shinyJade_ = other.shinyJade_;
		pureBlood_ = other.pureBlood_;
		flexibleRoot_ = other.flexibleRoot_;
		superAir_ = other.superAir_;
		perception_ = other.perception_;
		physique_ = other.physique_;
		currentSuperAir_ = other.currentSuperAir_;
		currentPerception_ = other.currentPerception_;
		currentPhysique_ = other.currentPhysique_;
		currentVigor_ = other.currentVigor_;
		currentClever_ = other.currentClever_;
		currentPoisonVal_ = other.currentPoisonVal_;
		currentPureBlood_ = other.currentPureBlood_;
		liveRelId_ = other.liveRelId_;
		live1Type_ = other.live1Type_;
		live2Type_ = other.live2Type_;
		live1Level_ = other.live1Level_;
		live2Level_ = other.live2Level_;
		live1Value_ = other.live1Value_;
		live2Value_ = other.live2Value_;
		yueKaDays_ = other.yueKaDays_;
		title_ = other.title_;
		hasWood_ = other.hasWood_;
		hasGold_ = other.hasGold_;
		hasWater_ = other.hasWater_;
		hasFire_ = other.hasFire_;
		hasEarth_ = other.hasEarth_;
		battleGuideVal_ = other.battleGuideVal_;
		practiceGuideVal_ = other.practiceGuideVal_;
		isNewLetter_ = other.isNewLetter_;
		acupointGuideVal_ = other.acupointGuideVal_;
		shopGuideVal_ = other.shopGuideVal_;
		hunLiVal_ = other.hunLiVal_;
		signGuideVal_ = other.signGuideVal_;
		elixirGuideVal_ = other.elixirGuideVal_;
		isGetSignIn_ = other.isGetSignIn_;
		signInDay_ = other.signInDay_;
		guildId_ = other.guildId_;
		guildName_ = other.guildName_;
		guildSimpleName_ = other.guildSimpleName_;
		tokenIndex_ = other.tokenIndex_;
		lastNameIndex_ = other.lastNameIndex_;
		taskId_ = other.taskId_;
		taskStatus_ = other.taskStatus_;
		tianShuBeiStatus_ = other.tianShuBeiStatus_;
		power_ = other.power_;
		currentPower_ = other.currentPower_;
		vigor_ = other.vigor_;
		clever_ = other.clever_;
		live3Type_ = other.live3Type_;
		live3Level_ = other.live3Level_;
		live3Value_ = other.live3Value_;
		isTianDaoLing_ = other.isTianDaoLing_;
		titleId_ = other.titleId_.Clone();
		qiYun_ = other.qiYun_;
		live4Type_ = other.live4Type_;
		live4Level_ = other.live4Level_;
		live4Value_ = other.live4Value_;
		leiChong_ = other.leiChong_;
		vipLevel_ = other.vipLevel_;
		titleFrame_ = other.titleFrame_;
		headFrame_ = other.headFrame_;
		isHaoYunFu_ = other.isHaoYunFu_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoleLoginResp_1002002 Clone()
	{
		return new RoleLoginResp_1002002(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoleLoginResp_1002002);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoleLoginResp_1002002 other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		if (IsSuccess != other.IsSuccess)
		{
			return false;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (MapId != other.MapId)
		{
			return false;
		}
		if (MapName != other.MapName)
		{
			return false;
		}
		if (Sex != other.Sex)
		{
			return false;
		}
		if (HeadPortraitId != other.HeadPortraitId)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (CurrentAge != other.CurrentAge)
		{
			return false;
		}
		if (MaxAge != other.MaxAge)
		{
			return false;
		}
		if (ShinyStone != other.ShinyStone)
		{
			return false;
		}
		if (ShinyJade != other.ShinyJade)
		{
			return false;
		}
		if (PureBlood != other.PureBlood)
		{
			return false;
		}
		if (FlexibleRoot != other.FlexibleRoot)
		{
			return false;
		}
		if (SuperAir != other.SuperAir)
		{
			return false;
		}
		if (Perception != other.Perception)
		{
			return false;
		}
		if (Physique != other.Physique)
		{
			return false;
		}
		if (CurrentSuperAir != other.CurrentSuperAir)
		{
			return false;
		}
		if (CurrentPerception != other.CurrentPerception)
		{
			return false;
		}
		if (CurrentPhysique != other.CurrentPhysique)
		{
			return false;
		}
		if (CurrentVigor != other.CurrentVigor)
		{
			return false;
		}
		if (CurrentClever != other.CurrentClever)
		{
			return false;
		}
		if (CurrentPoisonVal != other.CurrentPoisonVal)
		{
			return false;
		}
		if (CurrentPureBlood != other.CurrentPureBlood)
		{
			return false;
		}
		if (LiveRelId != other.LiveRelId)
		{
			return false;
		}
		if (Live1Type != other.Live1Type)
		{
			return false;
		}
		if (Live2Type != other.Live2Type)
		{
			return false;
		}
		if (Live1Level != other.Live1Level)
		{
			return false;
		}
		if (Live2Level != other.Live2Level)
		{
			return false;
		}
		if (Live1Value != other.Live1Value)
		{
			return false;
		}
		if (Live2Value != other.Live2Value)
		{
			return false;
		}
		if (YueKaDays != other.YueKaDays)
		{
			return false;
		}
		if (Title != other.Title)
		{
			return false;
		}
		if (HasWood != other.HasWood)
		{
			return false;
		}
		if (HasGold != other.HasGold)
		{
			return false;
		}
		if (HasWater != other.HasWater)
		{
			return false;
		}
		if (HasFire != other.HasFire)
		{
			return false;
		}
		if (HasEarth != other.HasEarth)
		{
			return false;
		}
		if (BattleGuideVal != other.BattleGuideVal)
		{
			return false;
		}
		if (PracticeGuideVal != other.PracticeGuideVal)
		{
			return false;
		}
		if (IsNewLetter != other.IsNewLetter)
		{
			return false;
		}
		if (AcupointGuideVal != other.AcupointGuideVal)
		{
			return false;
		}
		if (ShopGuideVal != other.ShopGuideVal)
		{
			return false;
		}
		if (HunLiVal != other.HunLiVal)
		{
			return false;
		}
		if (SignGuideVal != other.SignGuideVal)
		{
			return false;
		}
		if (ElixirGuideVal != other.ElixirGuideVal)
		{
			return false;
		}
		if (IsGetSignIn != other.IsGetSignIn)
		{
			return false;
		}
		if (SignInDay != other.SignInDay)
		{
			return false;
		}
		if (GuildId != other.GuildId)
		{
			return false;
		}
		if (GuildName != other.GuildName)
		{
			return false;
		}
		if (GuildSimpleName != other.GuildSimpleName)
		{
			return false;
		}
		if (TokenIndex != other.TokenIndex)
		{
			return false;
		}
		if (LastNameIndex != other.LastNameIndex)
		{
			return false;
		}
		if (TaskId != other.TaskId)
		{
			return false;
		}
		if (TaskStatus != other.TaskStatus)
		{
			return false;
		}
		if (TianShuBeiStatus != other.TianShuBeiStatus)
		{
			return false;
		}
		if (Power != other.Power)
		{
			return false;
		}
		if (CurrentPower != other.CurrentPower)
		{
			return false;
		}
		if (Vigor != other.Vigor)
		{
			return false;
		}
		if (Clever != other.Clever)
		{
			return false;
		}
		if (Live3Type != other.Live3Type)
		{
			return false;
		}
		if (Live3Level != other.Live3Level)
		{
			return false;
		}
		if (Live3Value != other.Live3Value)
		{
			return false;
		}
		if (IsTianDaoLing != other.IsTianDaoLing)
		{
			return false;
		}
		if (!titleId_.Equals(other.titleId_))
		{
			return false;
		}
		if (QiYun != other.QiYun)
		{
			return false;
		}
		if (Live4Type != other.Live4Type)
		{
			return false;
		}
		if (Live4Level != other.Live4Level)
		{
			return false;
		}
		if (Live4Value != other.Live4Value)
		{
			return false;
		}
		if (LeiChong != other.LeiChong)
		{
			return false;
		}
		if (VipLevel != other.VipLevel)
		{
			return false;
		}
		if (TitleFrame != other.TitleFrame)
		{
			return false;
		}
		if (HeadFrame != other.HeadFrame)
		{
			return false;
		}
		if (IsHaoYunFu != other.IsHaoYunFu)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code != 0)
		{
			num ^= Code.GetHashCode();
		}
		if (IsSuccess)
		{
			num ^= IsSuccess.GetHashCode();
		}
		if (Id.Length != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (MapId.Length != 0)
		{
			num ^= MapId.GetHashCode();
		}
		if (MapName.Length != 0)
		{
			num ^= MapName.GetHashCode();
		}
		if (Sex != 0)
		{
			num ^= Sex.GetHashCode();
		}
		if (HeadPortraitId != 0)
		{
			num ^= HeadPortraitId.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (CurrentAge != 0)
		{
			num ^= CurrentAge.GetHashCode();
		}
		if (MaxAge != 0)
		{
			num ^= MaxAge.GetHashCode();
		}
		if (ShinyStone != 0L)
		{
			num ^= ShinyStone.GetHashCode();
		}
		if (ShinyJade != 0)
		{
			num ^= ShinyJade.GetHashCode();
		}
		if (PureBlood != 0)
		{
			num ^= PureBlood.GetHashCode();
		}
		if (FlexibleRoot.Length != 0)
		{
			num ^= FlexibleRoot.GetHashCode();
		}
		if (SuperAir != 0L)
		{
			num ^= SuperAir.GetHashCode();
		}
		if (Perception != 0L)
		{
			num ^= Perception.GetHashCode();
		}
		if (Physique != 0L)
		{
			num ^= Physique.GetHashCode();
		}
		if (CurrentSuperAir != 0L)
		{
			num ^= CurrentSuperAir.GetHashCode();
		}
		if (CurrentPerception != 0L)
		{
			num ^= CurrentPerception.GetHashCode();
		}
		if (CurrentPhysique != 0L)
		{
			num ^= CurrentPhysique.GetHashCode();
		}
		if (CurrentVigor != 0)
		{
			num ^= CurrentVigor.GetHashCode();
		}
		if (CurrentClever != 0)
		{
			num ^= CurrentClever.GetHashCode();
		}
		if (CurrentPoisonVal != 0)
		{
			num ^= CurrentPoisonVal.GetHashCode();
		}
		if (CurrentPureBlood != 0)
		{
			num ^= CurrentPureBlood.GetHashCode();
		}
		if (LiveRelId != 0)
		{
			num ^= LiveRelId.GetHashCode();
		}
		if (Live1Type != 0)
		{
			num ^= Live1Type.GetHashCode();
		}
		if (Live2Type != 0)
		{
			num ^= Live2Type.GetHashCode();
		}
		if (Live1Level != 0)
		{
			num ^= Live1Level.GetHashCode();
		}
		if (Live2Level != 0)
		{
			num ^= Live2Level.GetHashCode();
		}
		if (Live1Value != 0)
		{
			num ^= Live1Value.GetHashCode();
		}
		if (Live2Value != 0)
		{
			num ^= Live2Value.GetHashCode();
		}
		if (YueKaDays != 0)
		{
			num ^= YueKaDays.GetHashCode();
		}
		if (Title != 0)
		{
			num ^= Title.GetHashCode();
		}
		if (HasWood)
		{
			num ^= HasWood.GetHashCode();
		}
		if (HasGold)
		{
			num ^= HasGold.GetHashCode();
		}
		if (HasWater)
		{
			num ^= HasWater.GetHashCode();
		}
		if (HasFire)
		{
			num ^= HasFire.GetHashCode();
		}
		if (HasEarth)
		{
			num ^= HasEarth.GetHashCode();
		}
		if (BattleGuideVal != 0)
		{
			num ^= BattleGuideVal.GetHashCode();
		}
		if (PracticeGuideVal != 0)
		{
			num ^= PracticeGuideVal.GetHashCode();
		}
		if (IsNewLetter)
		{
			num ^= IsNewLetter.GetHashCode();
		}
		if (AcupointGuideVal != 0)
		{
			num ^= AcupointGuideVal.GetHashCode();
		}
		if (ShopGuideVal != 0)
		{
			num ^= ShopGuideVal.GetHashCode();
		}
		if (HunLiVal != 0)
		{
			num ^= HunLiVal.GetHashCode();
		}
		if (SignGuideVal != 0)
		{
			num ^= SignGuideVal.GetHashCode();
		}
		if (ElixirGuideVal != 0)
		{
			num ^= ElixirGuideVal.GetHashCode();
		}
		if (IsGetSignIn)
		{
			num ^= IsGetSignIn.GetHashCode();
		}
		if (SignInDay != 0)
		{
			num ^= SignInDay.GetHashCode();
		}
		if (GuildId != 0)
		{
			num ^= GuildId.GetHashCode();
		}
		if (GuildName.Length != 0)
		{
			num ^= GuildName.GetHashCode();
		}
		if (GuildSimpleName.Length != 0)
		{
			num ^= GuildSimpleName.GetHashCode();
		}
		if (TokenIndex != 0)
		{
			num ^= TokenIndex.GetHashCode();
		}
		if (LastNameIndex != 0)
		{
			num ^= LastNameIndex.GetHashCode();
		}
		if (TaskId != 0)
		{
			num ^= TaskId.GetHashCode();
		}
		if (TaskStatus != 0)
		{
			num ^= TaskStatus.GetHashCode();
		}
		if (TianShuBeiStatus != 0)
		{
			num ^= TianShuBeiStatus.GetHashCode();
		}
		if (Power != 0)
		{
			num ^= Power.GetHashCode();
		}
		if (CurrentPower != 0)
		{
			num ^= CurrentPower.GetHashCode();
		}
		if (Vigor != 0)
		{
			num ^= Vigor.GetHashCode();
		}
		if (Clever != 0)
		{
			num ^= Clever.GetHashCode();
		}
		if (Live3Type != 0)
		{
			num ^= Live3Type.GetHashCode();
		}
		if (Live3Level != 0)
		{
			num ^= Live3Level.GetHashCode();
		}
		if (Live3Value != 0)
		{
			num ^= Live3Value.GetHashCode();
		}
		if (IsTianDaoLing)
		{
			num ^= IsTianDaoLing.GetHashCode();
		}
		num ^= titleId_.GetHashCode();
		if (QiYun != 0)
		{
			num ^= QiYun.GetHashCode();
		}
		if (Live4Type != 0)
		{
			num ^= Live4Type.GetHashCode();
		}
		if (Live4Level != 0)
		{
			num ^= Live4Level.GetHashCode();
		}
		if (Live4Value != 0)
		{
			num ^= Live4Value.GetHashCode();
		}
		if (LeiChong != 0)
		{
			num ^= LeiChong.GetHashCode();
		}
		if (VipLevel != 0)
		{
			num ^= VipLevel.GetHashCode();
		}
		if (TitleFrame != 0)
		{
			num ^= TitleFrame.GetHashCode();
		}
		if (HeadFrame != 0)
		{
			num ^= HeadFrame.GetHashCode();
		}
		if (IsHaoYunFu)
		{
			num ^= IsHaoYunFu.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (Code != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Code);
		}
		if (IsSuccess)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsSuccess);
		}
		if (Id.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Id);
		}
		if (MapId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(MapId);
		}
		if (MapName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(MapName);
		}
		if (Sex != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Sex);
		}
		if (HeadPortraitId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(HeadPortraitId);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(Name);
		}
		if (Level != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Level);
		}
		if (CurrentAge != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(CurrentAge);
		}
		if (MaxAge != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(MaxAge);
		}
		if (ShinyStone != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(ShinyStone);
		}
		if (ShinyJade != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(ShinyJade);
		}
		if (PureBlood != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(PureBlood);
		}
		if (FlexibleRoot.Length != 0)
		{
			output.WriteRawTag(122);
			output.WriteString(FlexibleRoot);
		}
		if (SuperAir != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(SuperAir);
		}
		if (Perception != 0L)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt64(Perception);
		}
		if (Physique != 0L)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt64(Physique);
		}
		if (CurrentSuperAir != 0L)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt64(CurrentSuperAir);
		}
		if (CurrentPerception != 0L)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt64(CurrentPerception);
		}
		if (CurrentPhysique != 0L)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt64(CurrentPhysique);
		}
		if (CurrentVigor != 0)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt32(CurrentVigor);
		}
		if (CurrentClever != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteSInt32(CurrentClever);
		}
		if (CurrentPoisonVal != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(CurrentPoisonVal);
		}
		if (CurrentPureBlood != 0)
		{
			output.WriteRawTag(200, 1);
			output.WriteInt32(CurrentPureBlood);
		}
		if (LiveRelId != 0)
		{
			output.WriteRawTag(208, 1);
			output.WriteInt32(LiveRelId);
		}
		if (Live1Type != 0)
		{
			output.WriteRawTag(216, 1);
			output.WriteInt32(Live1Type);
		}
		if (Live2Type != 0)
		{
			output.WriteRawTag(224, 1);
			output.WriteInt32(Live2Type);
		}
		if (Live1Level != 0)
		{
			output.WriteRawTag(232, 1);
			output.WriteInt32(Live1Level);
		}
		if (Live2Level != 0)
		{
			output.WriteRawTag(240, 1);
			output.WriteInt32(Live2Level);
		}
		if (Live1Value != 0)
		{
			output.WriteRawTag(248, 1);
			output.WriteInt32(Live1Value);
		}
		if (Live2Value != 0)
		{
			output.WriteRawTag(128, 2);
			output.WriteInt32(Live2Value);
		}
		if (YueKaDays != 0)
		{
			output.WriteRawTag(136, 2);
			output.WriteInt32(YueKaDays);
		}
		if (Title != 0)
		{
			output.WriteRawTag(144, 2);
			output.WriteInt32(Title);
		}
		if (HasWood)
		{
			output.WriteRawTag(152, 2);
			output.WriteBool(HasWood);
		}
		if (HasGold)
		{
			output.WriteRawTag(160, 2);
			output.WriteBool(HasGold);
		}
		if (HasWater)
		{
			output.WriteRawTag(168, 2);
			output.WriteBool(HasWater);
		}
		if (HasFire)
		{
			output.WriteRawTag(176, 2);
			output.WriteBool(HasFire);
		}
		if (HasEarth)
		{
			output.WriteRawTag(184, 2);
			output.WriteBool(HasEarth);
		}
		if (BattleGuideVal != 0)
		{
			output.WriteRawTag(192, 2);
			output.WriteInt32(BattleGuideVal);
		}
		if (PracticeGuideVal != 0)
		{
			output.WriteRawTag(200, 2);
			output.WriteInt32(PracticeGuideVal);
		}
		if (IsNewLetter)
		{
			output.WriteRawTag(208, 2);
			output.WriteBool(IsNewLetter);
		}
		if (AcupointGuideVal != 0)
		{
			output.WriteRawTag(216, 2);
			output.WriteInt32(AcupointGuideVal);
		}
		if (ShopGuideVal != 0)
		{
			output.WriteRawTag(224, 2);
			output.WriteInt32(ShopGuideVal);
		}
		if (HunLiVal != 0)
		{
			output.WriteRawTag(232, 2);
			output.WriteInt32(HunLiVal);
		}
		if (SignGuideVal != 0)
		{
			output.WriteRawTag(240, 2);
			output.WriteInt32(SignGuideVal);
		}
		if (ElixirGuideVal != 0)
		{
			output.WriteRawTag(248, 2);
			output.WriteInt32(ElixirGuideVal);
		}
		if (IsGetSignIn)
		{
			output.WriteRawTag(128, 3);
			output.WriteBool(IsGetSignIn);
		}
		if (SignInDay != 0)
		{
			output.WriteRawTag(136, 3);
			output.WriteInt32(SignInDay);
		}
		if (GuildId != 0)
		{
			output.WriteRawTag(144, 3);
			output.WriteInt32(GuildId);
		}
		if (GuildName.Length != 0)
		{
			output.WriteRawTag(154, 3);
			output.WriteString(GuildName);
		}
		if (GuildSimpleName.Length != 0)
		{
			output.WriteRawTag(162, 3);
			output.WriteString(GuildSimpleName);
		}
		if (TokenIndex != 0)
		{
			output.WriteRawTag(168, 3);
			output.WriteInt32(TokenIndex);
		}
		if (LastNameIndex != 0)
		{
			output.WriteRawTag(176, 3);
			output.WriteInt32(LastNameIndex);
		}
		if (TaskId != 0)
		{
			output.WriteRawTag(184, 3);
			output.WriteInt32(TaskId);
		}
		if (TaskStatus != 0)
		{
			output.WriteRawTag(192, 3);
			output.WriteInt32(TaskStatus);
		}
		if (TianShuBeiStatus != 0)
		{
			output.WriteRawTag(200, 3);
			output.WriteInt32(TianShuBeiStatus);
		}
		if (Power != 0)
		{
			output.WriteRawTag(208, 3);
			output.WriteInt32(Power);
		}
		if (CurrentPower != 0)
		{
			output.WriteRawTag(216, 3);
			output.WriteInt32(CurrentPower);
		}
		if (Vigor != 0)
		{
			output.WriteRawTag(224, 3);
			output.WriteInt32(Vigor);
		}
		if (Clever != 0)
		{
			output.WriteRawTag(232, 3);
			output.WriteSInt32(Clever);
		}
		if (Live3Type != 0)
		{
			output.WriteRawTag(240, 3);
			output.WriteInt32(Live3Type);
		}
		if (Live3Level != 0)
		{
			output.WriteRawTag(248, 3);
			output.WriteInt32(Live3Level);
		}
		if (Live3Value != 0)
		{
			output.WriteRawTag(128, 4);
			output.WriteInt32(Live3Value);
		}
		if (IsTianDaoLing)
		{
			output.WriteRawTag(136, 4);
			output.WriteBool(IsTianDaoLing);
		}
		titleId_.WriteTo(output, _repeated_titleId_codec);
		if (QiYun != 0)
		{
			output.WriteRawTag(152, 4);
			output.WriteInt32(QiYun);
		}
		if (Live4Type != 0)
		{
			output.WriteRawTag(160, 4);
			output.WriteInt32(Live4Type);
		}
		if (Live4Level != 0)
		{
			output.WriteRawTag(168, 4);
			output.WriteInt32(Live4Level);
		}
		if (Live4Value != 0)
		{
			output.WriteRawTag(176, 4);
			output.WriteInt32(Live4Value);
		}
		if (LeiChong != 0)
		{
			output.WriteRawTag(184, 4);
			output.WriteInt32(LeiChong);
		}
		if (VipLevel != 0)
		{
			output.WriteRawTag(192, 4);
			output.WriteInt32(VipLevel);
		}
		if (TitleFrame != 0)
		{
			output.WriteRawTag(200, 4);
			output.WriteInt32(TitleFrame);
		}
		if (HeadFrame != 0)
		{
			output.WriteRawTag(208, 4);
			output.WriteInt32(HeadFrame);
		}
		if (IsHaoYunFu)
		{
			output.WriteRawTag(216, 4);
			output.WriteBool(IsHaoYunFu);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Code != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Code);
		}
		if (IsSuccess)
		{
			num += 2;
		}
		if (Id.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Id);
		}
		if (MapId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapId);
		}
		if (MapName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MapName);
		}
		if (Sex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Sex);
		}
		if (HeadPortraitId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadPortraitId);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (CurrentAge != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentAge);
		}
		if (MaxAge != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxAge);
		}
		if (ShinyStone != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShinyStone);
		}
		if (ShinyJade != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ShinyJade);
		}
		if (PureBlood != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PureBlood);
		}
		if (FlexibleRoot.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FlexibleRoot);
		}
		if (SuperAir != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(SuperAir);
		}
		if (Perception != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(Perception);
		}
		if (Physique != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(Physique);
		}
		if (CurrentSuperAir != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(CurrentSuperAir);
		}
		if (CurrentPerception != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(CurrentPerception);
		}
		if (CurrentPhysique != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(CurrentPhysique);
		}
		if (CurrentVigor != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CurrentVigor);
		}
		if (CurrentClever != 0)
		{
			num += 2 + CodedOutputStream.ComputeSInt32Size(CurrentClever);
		}
		if (CurrentPoisonVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CurrentPoisonVal);
		}
		if (CurrentPureBlood != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CurrentPureBlood);
		}
		if (LiveRelId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LiveRelId);
		}
		if (Live1Type != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live1Type);
		}
		if (Live2Type != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live2Type);
		}
		if (Live1Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live1Level);
		}
		if (Live2Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live2Level);
		}
		if (Live1Value != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live1Value);
		}
		if (Live2Value != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live2Value);
		}
		if (YueKaDays != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(YueKaDays);
		}
		if (Title != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Title);
		}
		if (HasWood)
		{
			num += 3;
		}
		if (HasGold)
		{
			num += 3;
		}
		if (HasWater)
		{
			num += 3;
		}
		if (HasFire)
		{
			num += 3;
		}
		if (HasEarth)
		{
			num += 3;
		}
		if (BattleGuideVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(BattleGuideVal);
		}
		if (PracticeGuideVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(PracticeGuideVal);
		}
		if (IsNewLetter)
		{
			num += 3;
		}
		if (AcupointGuideVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(AcupointGuideVal);
		}
		if (ShopGuideVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ShopGuideVal);
		}
		if (HunLiVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(HunLiVal);
		}
		if (SignGuideVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SignGuideVal);
		}
		if (ElixirGuideVal != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ElixirGuideVal);
		}
		if (IsGetSignIn)
		{
			num += 3;
		}
		if (SignInDay != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SignInDay);
		}
		if (GuildId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(GuildId);
		}
		if (GuildName.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(GuildName);
		}
		if (GuildSimpleName.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(GuildSimpleName);
		}
		if (TokenIndex != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(TokenIndex);
		}
		if (LastNameIndex != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LastNameIndex);
		}
		if (TaskId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(TaskId);
		}
		if (TaskStatus != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(TaskStatus);
		}
		if (TianShuBeiStatus != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(TianShuBeiStatus);
		}
		if (Power != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Power);
		}
		if (CurrentPower != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(CurrentPower);
		}
		if (Vigor != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Vigor);
		}
		if (Clever != 0)
		{
			num += 2 + CodedOutputStream.ComputeSInt32Size(Clever);
		}
		if (Live3Type != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live3Type);
		}
		if (Live3Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live3Level);
		}
		if (Live3Value != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live3Value);
		}
		if (IsTianDaoLing)
		{
			num += 3;
		}
		num += titleId_.CalculateSize(_repeated_titleId_codec);
		if (QiYun != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(QiYun);
		}
		if (Live4Type != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live4Type);
		}
		if (Live4Level != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live4Level);
		}
		if (Live4Value != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Live4Value);
		}
		if (LeiChong != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(LeiChong);
		}
		if (VipLevel != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(VipLevel);
		}
		if (TitleFrame != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(TitleFrame);
		}
		if (HeadFrame != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(HeadFrame);
		}
		if (IsHaoYunFu)
		{
			num += 3;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoleLoginResp_1002002 other)
	{
		if (other != null)
		{
			if (other.Code != 0)
			{
				Code = other.Code;
			}
			if (other.IsSuccess)
			{
				IsSuccess = other.IsSuccess;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.MapId.Length != 0)
			{
				MapId = other.MapId;
			}
			if (other.MapName.Length != 0)
			{
				MapName = other.MapName;
			}
			if (other.Sex != 0)
			{
				Sex = other.Sex;
			}
			if (other.HeadPortraitId != 0)
			{
				HeadPortraitId = other.HeadPortraitId;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.CurrentAge != 0)
			{
				CurrentAge = other.CurrentAge;
			}
			if (other.MaxAge != 0)
			{
				MaxAge = other.MaxAge;
			}
			if (other.ShinyStone != 0L)
			{
				ShinyStone = other.ShinyStone;
			}
			if (other.ShinyJade != 0)
			{
				ShinyJade = other.ShinyJade;
			}
			if (other.PureBlood != 0)
			{
				PureBlood = other.PureBlood;
			}
			if (other.FlexibleRoot.Length != 0)
			{
				FlexibleRoot = other.FlexibleRoot;
			}
			if (other.SuperAir != 0L)
			{
				SuperAir = other.SuperAir;
			}
			if (other.Perception != 0L)
			{
				Perception = other.Perception;
			}
			if (other.Physique != 0L)
			{
				Physique = other.Physique;
			}
			if (other.CurrentSuperAir != 0L)
			{
				CurrentSuperAir = other.CurrentSuperAir;
			}
			if (other.CurrentPerception != 0L)
			{
				CurrentPerception = other.CurrentPerception;
			}
			if (other.CurrentPhysique != 0L)
			{
				CurrentPhysique = other.CurrentPhysique;
			}
			if (other.CurrentVigor != 0)
			{
				CurrentVigor = other.CurrentVigor;
			}
			if (other.CurrentClever != 0)
			{
				CurrentClever = other.CurrentClever;
			}
			if (other.CurrentPoisonVal != 0)
			{
				CurrentPoisonVal = other.CurrentPoisonVal;
			}
			if (other.CurrentPureBlood != 0)
			{
				CurrentPureBlood = other.CurrentPureBlood;
			}
			if (other.LiveRelId != 0)
			{
				LiveRelId = other.LiveRelId;
			}
			if (other.Live1Type != 0)
			{
				Live1Type = other.Live1Type;
			}
			if (other.Live2Type != 0)
			{
				Live2Type = other.Live2Type;
			}
			if (other.Live1Level != 0)
			{
				Live1Level = other.Live1Level;
			}
			if (other.Live2Level != 0)
			{
				Live2Level = other.Live2Level;
			}
			if (other.Live1Value != 0)
			{
				Live1Value = other.Live1Value;
			}
			if (other.Live2Value != 0)
			{
				Live2Value = other.Live2Value;
			}
			if (other.YueKaDays != 0)
			{
				YueKaDays = other.YueKaDays;
			}
			if (other.Title != 0)
			{
				Title = other.Title;
			}
			if (other.HasWood)
			{
				HasWood = other.HasWood;
			}
			if (other.HasGold)
			{
				HasGold = other.HasGold;
			}
			if (other.HasWater)
			{
				HasWater = other.HasWater;
			}
			if (other.HasFire)
			{
				HasFire = other.HasFire;
			}
			if (other.HasEarth)
			{
				HasEarth = other.HasEarth;
			}
			if (other.BattleGuideVal != 0)
			{
				BattleGuideVal = other.BattleGuideVal;
			}
			if (other.PracticeGuideVal != 0)
			{
				PracticeGuideVal = other.PracticeGuideVal;
			}
			if (other.IsNewLetter)
			{
				IsNewLetter = other.IsNewLetter;
			}
			if (other.AcupointGuideVal != 0)
			{
				AcupointGuideVal = other.AcupointGuideVal;
			}
			if (other.ShopGuideVal != 0)
			{
				ShopGuideVal = other.ShopGuideVal;
			}
			if (other.HunLiVal != 0)
			{
				HunLiVal = other.HunLiVal;
			}
			if (other.SignGuideVal != 0)
			{
				SignGuideVal = other.SignGuideVal;
			}
			if (other.ElixirGuideVal != 0)
			{
				ElixirGuideVal = other.ElixirGuideVal;
			}
			if (other.IsGetSignIn)
			{
				IsGetSignIn = other.IsGetSignIn;
			}
			if (other.SignInDay != 0)
			{
				SignInDay = other.SignInDay;
			}
			if (other.GuildId != 0)
			{
				GuildId = other.GuildId;
			}
			if (other.GuildName.Length != 0)
			{
				GuildName = other.GuildName;
			}
			if (other.GuildSimpleName.Length != 0)
			{
				GuildSimpleName = other.GuildSimpleName;
			}
			if (other.TokenIndex != 0)
			{
				TokenIndex = other.TokenIndex;
			}
			if (other.LastNameIndex != 0)
			{
				LastNameIndex = other.LastNameIndex;
			}
			if (other.TaskId != 0)
			{
				TaskId = other.TaskId;
			}
			if (other.TaskStatus != 0)
			{
				TaskStatus = other.TaskStatus;
			}
			if (other.TianShuBeiStatus != 0)
			{
				TianShuBeiStatus = other.TianShuBeiStatus;
			}
			if (other.Power != 0)
			{
				Power = other.Power;
			}
			if (other.CurrentPower != 0)
			{
				CurrentPower = other.CurrentPower;
			}
			if (other.Vigor != 0)
			{
				Vigor = other.Vigor;
			}
			if (other.Clever != 0)
			{
				Clever = other.Clever;
			}
			if (other.Live3Type != 0)
			{
				Live3Type = other.Live3Type;
			}
			if (other.Live3Level != 0)
			{
				Live3Level = other.Live3Level;
			}
			if (other.Live3Value != 0)
			{
				Live3Value = other.Live3Value;
			}
			if (other.IsTianDaoLing)
			{
				IsTianDaoLing = other.IsTianDaoLing;
			}
			titleId_.Add(other.titleId_);
			if (other.QiYun != 0)
			{
				QiYun = other.QiYun;
			}
			if (other.Live4Type != 0)
			{
				Live4Type = other.Live4Type;
			}
			if (other.Live4Level != 0)
			{
				Live4Level = other.Live4Level;
			}
			if (other.Live4Value != 0)
			{
				Live4Value = other.Live4Value;
			}
			if (other.LeiChong != 0)
			{
				LeiChong = other.LeiChong;
			}
			if (other.VipLevel != 0)
			{
				VipLevel = other.VipLevel;
			}
			if (other.TitleFrame != 0)
			{
				TitleFrame = other.TitleFrame;
			}
			if (other.HeadFrame != 0)
			{
				HeadFrame = other.HeadFrame;
			}
			if (other.IsHaoYunFu)
			{
				IsHaoYunFu = other.IsHaoYunFu;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				Code = input.ReadInt32();
				break;
			case 16u:
				IsSuccess = input.ReadBool();
				break;
			case 26u:
				Id = input.ReadString();
				break;
			case 34u:
				MapId = input.ReadString();
				break;
			case 42u:
				MapName = input.ReadString();
				break;
			case 48u:
				Sex = input.ReadInt32();
				break;
			case 56u:
				HeadPortraitId = input.ReadInt32();
				break;
			case 66u:
				Name = input.ReadString();
				break;
			case 72u:
				Level = input.ReadInt32();
				break;
			case 80u:
				CurrentAge = input.ReadInt32();
				break;
			case 88u:
				MaxAge = input.ReadInt32();
				break;
			case 96u:
				ShinyStone = input.ReadInt64();
				break;
			case 104u:
				ShinyJade = input.ReadInt32();
				break;
			case 112u:
				PureBlood = input.ReadInt32();
				break;
			case 122u:
				FlexibleRoot = input.ReadString();
				break;
			case 128u:
				SuperAir = input.ReadInt64();
				break;
			case 136u:
				Perception = input.ReadInt64();
				break;
			case 144u:
				Physique = input.ReadInt64();
				break;
			case 152u:
				CurrentSuperAir = input.ReadInt64();
				break;
			case 160u:
				CurrentPerception = input.ReadInt64();
				break;
			case 168u:
				CurrentPhysique = input.ReadInt64();
				break;
			case 176u:
				CurrentVigor = input.ReadInt32();
				break;
			case 184u:
				CurrentClever = input.ReadSInt32();
				break;
			case 192u:
				CurrentPoisonVal = input.ReadInt32();
				break;
			case 200u:
				CurrentPureBlood = input.ReadInt32();
				break;
			case 208u:
				LiveRelId = input.ReadInt32();
				break;
			case 216u:
				Live1Type = input.ReadInt32();
				break;
			case 224u:
				Live2Type = input.ReadInt32();
				break;
			case 232u:
				Live1Level = input.ReadInt32();
				break;
			case 240u:
				Live2Level = input.ReadInt32();
				break;
			case 248u:
				Live1Value = input.ReadInt32();
				break;
			case 256u:
				Live2Value = input.ReadInt32();
				break;
			case 264u:
				YueKaDays = input.ReadInt32();
				break;
			case 272u:
				Title = input.ReadInt32();
				break;
			case 280u:
				HasWood = input.ReadBool();
				break;
			case 288u:
				HasGold = input.ReadBool();
				break;
			case 296u:
				HasWater = input.ReadBool();
				break;
			case 304u:
				HasFire = input.ReadBool();
				break;
			case 312u:
				HasEarth = input.ReadBool();
				break;
			case 320u:
				BattleGuideVal = input.ReadInt32();
				break;
			case 328u:
				PracticeGuideVal = input.ReadInt32();
				break;
			case 336u:
				IsNewLetter = input.ReadBool();
				break;
			case 344u:
				AcupointGuideVal = input.ReadInt32();
				break;
			case 352u:
				ShopGuideVal = input.ReadInt32();
				break;
			case 360u:
				HunLiVal = input.ReadInt32();
				break;
			case 368u:
				SignGuideVal = input.ReadInt32();
				break;
			case 376u:
				ElixirGuideVal = input.ReadInt32();
				break;
			case 384u:
				IsGetSignIn = input.ReadBool();
				break;
			case 392u:
				SignInDay = input.ReadInt32();
				break;
			case 400u:
				GuildId = input.ReadInt32();
				break;
			case 410u:
				GuildName = input.ReadString();
				break;
			case 418u:
				GuildSimpleName = input.ReadString();
				break;
			case 424u:
				TokenIndex = input.ReadInt32();
				break;
			case 432u:
				LastNameIndex = input.ReadInt32();
				break;
			case 440u:
				TaskId = input.ReadInt32();
				break;
			case 448u:
				TaskStatus = input.ReadInt32();
				break;
			case 456u:
				TianShuBeiStatus = input.ReadInt32();
				break;
			case 464u:
				Power = input.ReadInt32();
				break;
			case 472u:
				CurrentPower = input.ReadInt32();
				break;
			case 480u:
				Vigor = input.ReadInt32();
				break;
			case 488u:
				Clever = input.ReadSInt32();
				break;
			case 496u:
				Live3Type = input.ReadInt32();
				break;
			case 504u:
				Live3Level = input.ReadInt32();
				break;
			case 512u:
				Live3Value = input.ReadInt32();
				break;
			case 520u:
				IsTianDaoLing = input.ReadBool();
				break;
			case 528u:
			case 530u:
				titleId_.AddEntriesFrom(input, _repeated_titleId_codec);
				break;
			case 536u:
				QiYun = input.ReadInt32();
				break;
			case 544u:
				Live4Type = input.ReadInt32();
				break;
			case 552u:
				Live4Level = input.ReadInt32();
				break;
			case 560u:
				Live4Value = input.ReadInt32();
				break;
			case 568u:
				LeiChong = input.ReadInt32();
				break;
			case 576u:
				VipLevel = input.ReadInt32();
				break;
			case 584u:
				TitleFrame = input.ReadInt32();
				break;
			case 592u:
				HeadFrame = input.ReadInt32();
				break;
			case 600u:
				IsHaoYunFu = input.ReadBool();
				break;
			}
		}
	}
}
