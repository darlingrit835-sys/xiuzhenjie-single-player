using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

public sealed class IntoYanWuResp_1015030 : IMessage<IntoYanWuResp_1015030>, IMessage, IEquatable<IntoYanWuResp_1015030>, IDeepCloneable<IntoYanWuResp_1015030>
{
	private static readonly MessageParser<IntoYanWuResp_1015030> _parser = new MessageParser<IntoYanWuResp_1015030>(() => new IntoYanWuResp_1015030());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private int code_;

	public const int IsSuccessFieldNumber = 2;

	private bool isSuccess_;

	public const int YanWuTaiStageFieldNumber = 3;

	private int yanWuTaiStage_;

	public const int YanWuTaiNumFieldNumber = 4;

	private int yanWuTaiNum_;

	public const int MatrixStageFieldNumber = 5;

	private int matrixStage_;

	public const int ZhiSiLvFieldNumber = 6;

	private int zhiSiLv_;

	public const int JiangDiHunLuanDuFieldNumber = 7;

	private int jiangDiHunLuanDu_;

	public const int TodaySiWangDiZiNumFieldNumber = 8;

	private int todaySiWangDiZiNum_;

	public const int JiangLiSuoDingFieldNumber = 9;

	private bool jiangLiSuoDing_;

	public const int JiangLi1FieldNumber = 10;

	private long jiangLi1_;

	public const int JiangLi2FieldNumber = 11;

	private int jiangLi2_;

	public const int JiangLi3FieldNumber = 12;

	private int jiangLi3_;

	public const int CreateYanWuTaiShengYuTimeFieldNumber = 13;

	private long createYanWuTaiShengYuTime_;

	public const int UpgradeYanWuTaiShengYuTimeFieldNumber = 14;

	private long upgradeYanWuTaiShengYuTime_;

	public const int UpgradeYanWuTaiMatrixShengYuTimeFieldNumber = 15;

	private long upgradeYanWuTaiMatrixShengYuTime_;

	[DebuggerNonUserCode]
	public static MessageParser<IntoYanWuResp_1015030> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => GuildProtoReflection.Descriptor.MessageTypes[32];

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
	public int YanWuTaiStage
	{
		get
		{
			return yanWuTaiStage_;
		}
		set
		{
			yanWuTaiStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int YanWuTaiNum
	{
		get
		{
			return yanWuTaiNum_;
		}
		set
		{
			yanWuTaiNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MatrixStage
	{
		get
		{
			return matrixStage_;
		}
		set
		{
			matrixStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ZhiSiLv
	{
		get
		{
			return zhiSiLv_;
		}
		set
		{
			zhiSiLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JiangDiHunLuanDu
	{
		get
		{
			return jiangDiHunLuanDu_;
		}
		set
		{
			jiangDiHunLuanDu_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TodaySiWangDiZiNum
	{
		get
		{
			return todaySiWangDiZiNum_;
		}
		set
		{
			todaySiWangDiZiNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool JiangLiSuoDing
	{
		get
		{
			return jiangLiSuoDing_;
		}
		set
		{
			jiangLiSuoDing_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long JiangLi1
	{
		get
		{
			return jiangLi1_;
		}
		set
		{
			jiangLi1_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JiangLi2
	{
		get
		{
			return jiangLi2_;
		}
		set
		{
			jiangLi2_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int JiangLi3
	{
		get
		{
			return jiangLi3_;
		}
		set
		{
			jiangLi3_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CreateYanWuTaiShengYuTime
	{
		get
		{
			return createYanWuTaiShengYuTime_;
		}
		set
		{
			createYanWuTaiShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeYanWuTaiShengYuTime
	{
		get
		{
			return upgradeYanWuTaiShengYuTime_;
		}
		set
		{
			upgradeYanWuTaiShengYuTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long UpgradeYanWuTaiMatrixShengYuTime
	{
		get
		{
			return upgradeYanWuTaiMatrixShengYuTime_;
		}
		set
		{
			upgradeYanWuTaiMatrixShengYuTime_ = value;
		}
	}

	private MessageDescriptor pb_003A_003AGoogle_002EProtobuf_002EIMessage_002Eget_Descriptor()
	{
		return Descriptor;
	}

	[DebuggerNonUserCode]
	public IntoYanWuResp_1015030()
	{
	}

	[DebuggerNonUserCode]
	public IntoYanWuResp_1015030(IntoYanWuResp_1015030 other)
		: this()
	{
		code_ = other.code_;
		isSuccess_ = other.isSuccess_;
		yanWuTaiStage_ = other.yanWuTaiStage_;
		yanWuTaiNum_ = other.yanWuTaiNum_;
		matrixStage_ = other.matrixStage_;
		zhiSiLv_ = other.zhiSiLv_;
		jiangDiHunLuanDu_ = other.jiangDiHunLuanDu_;
		todaySiWangDiZiNum_ = other.todaySiWangDiZiNum_;
		jiangLiSuoDing_ = other.jiangLiSuoDing_;
		jiangLi1_ = other.jiangLi1_;
		jiangLi2_ = other.jiangLi2_;
		jiangLi3_ = other.jiangLi3_;
		createYanWuTaiShengYuTime_ = other.createYanWuTaiShengYuTime_;
		upgradeYanWuTaiShengYuTime_ = other.upgradeYanWuTaiShengYuTime_;
		upgradeYanWuTaiMatrixShengYuTime_ = other.upgradeYanWuTaiMatrixShengYuTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IntoYanWuResp_1015030 Clone()
	{
		return new IntoYanWuResp_1015030(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IntoYanWuResp_1015030);
	}

	[DebuggerNonUserCode]
	public bool Equals(IntoYanWuResp_1015030 other)
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
		if (YanWuTaiStage != other.YanWuTaiStage)
		{
			return false;
		}
		if (YanWuTaiNum != other.YanWuTaiNum)
		{
			return false;
		}
		if (MatrixStage != other.MatrixStage)
		{
			return false;
		}
		if (ZhiSiLv != other.ZhiSiLv)
		{
			return false;
		}
		if (JiangDiHunLuanDu != other.JiangDiHunLuanDu)
		{
			return false;
		}
		if (TodaySiWangDiZiNum != other.TodaySiWangDiZiNum)
		{
			return false;
		}
		if (JiangLiSuoDing != other.JiangLiSuoDing)
		{
			return false;
		}
		if (JiangLi1 != other.JiangLi1)
		{
			return false;
		}
		if (JiangLi2 != other.JiangLi2)
		{
			return false;
		}
		if (JiangLi3 != other.JiangLi3)
		{
			return false;
		}
		if (CreateYanWuTaiShengYuTime != other.CreateYanWuTaiShengYuTime)
		{
			return false;
		}
		if (UpgradeYanWuTaiShengYuTime != other.UpgradeYanWuTaiShengYuTime)
		{
			return false;
		}
		if (UpgradeYanWuTaiMatrixShengYuTime != other.UpgradeYanWuTaiMatrixShengYuTime)
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
		if (YanWuTaiStage != 0)
		{
			num ^= YanWuTaiStage.GetHashCode();
		}
		if (YanWuTaiNum != 0)
		{
			num ^= YanWuTaiNum.GetHashCode();
		}
		if (MatrixStage != 0)
		{
			num ^= MatrixStage.GetHashCode();
		}
		if (ZhiSiLv != 0)
		{
			num ^= ZhiSiLv.GetHashCode();
		}
		if (JiangDiHunLuanDu != 0)
		{
			num ^= JiangDiHunLuanDu.GetHashCode();
		}
		if (TodaySiWangDiZiNum != 0)
		{
			num ^= TodaySiWangDiZiNum.GetHashCode();
		}
		if (JiangLiSuoDing)
		{
			num ^= JiangLiSuoDing.GetHashCode();
		}
		if (JiangLi1 != 0L)
		{
			num ^= JiangLi1.GetHashCode();
		}
		if (JiangLi2 != 0)
		{
			num ^= JiangLi2.GetHashCode();
		}
		if (JiangLi3 != 0)
		{
			num ^= JiangLi3.GetHashCode();
		}
		if (CreateYanWuTaiShengYuTime != 0L)
		{
			num ^= CreateYanWuTaiShengYuTime.GetHashCode();
		}
		if (UpgradeYanWuTaiShengYuTime != 0L)
		{
			num ^= UpgradeYanWuTaiShengYuTime.GetHashCode();
		}
		if (UpgradeYanWuTaiMatrixShengYuTime != 0L)
		{
			num ^= UpgradeYanWuTaiMatrixShengYuTime.GetHashCode();
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
		if (YanWuTaiStage != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(YanWuTaiStage);
		}
		if (YanWuTaiNum != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(YanWuTaiNum);
		}
		if (MatrixStage != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(MatrixStage);
		}
		if (ZhiSiLv != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(ZhiSiLv);
		}
		if (JiangDiHunLuanDu != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(JiangDiHunLuanDu);
		}
		if (TodaySiWangDiZiNum != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(TodaySiWangDiZiNum);
		}
		if (JiangLiSuoDing)
		{
			output.WriteRawTag(72);
			output.WriteBool(JiangLiSuoDing);
		}
		if (JiangLi1 != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(JiangLi1);
		}
		if (JiangLi2 != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(JiangLi2);
		}
		if (JiangLi3 != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(JiangLi3);
		}
		if (CreateYanWuTaiShengYuTime != 0L)
		{
			output.WriteRawTag(104);
			output.WriteInt64(CreateYanWuTaiShengYuTime);
		}
		if (UpgradeYanWuTaiShengYuTime != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(UpgradeYanWuTaiShengYuTime);
		}
		if (UpgradeYanWuTaiMatrixShengYuTime != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(UpgradeYanWuTaiMatrixShengYuTime);
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
		if (YanWuTaiStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(YanWuTaiStage);
		}
		if (YanWuTaiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(YanWuTaiNum);
		}
		if (MatrixStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MatrixStage);
		}
		if (ZhiSiLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ZhiSiLv);
		}
		if (JiangDiHunLuanDu != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JiangDiHunLuanDu);
		}
		if (TodaySiWangDiZiNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TodaySiWangDiZiNum);
		}
		if (JiangLiSuoDing)
		{
			num += 2;
		}
		if (JiangLi1 != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(JiangLi1);
		}
		if (JiangLi2 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JiangLi2);
		}
		if (JiangLi3 != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(JiangLi3);
		}
		if (CreateYanWuTaiShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CreateYanWuTaiShengYuTime);
		}
		if (UpgradeYanWuTaiShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeYanWuTaiShengYuTime);
		}
		if (UpgradeYanWuTaiMatrixShengYuTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(UpgradeYanWuTaiMatrixShengYuTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IntoYanWuResp_1015030 other)
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
			if (other.YanWuTaiStage != 0)
			{
				YanWuTaiStage = other.YanWuTaiStage;
			}
			if (other.YanWuTaiNum != 0)
			{
				YanWuTaiNum = other.YanWuTaiNum;
			}
			if (other.MatrixStage != 0)
			{
				MatrixStage = other.MatrixStage;
			}
			if (other.ZhiSiLv != 0)
			{
				ZhiSiLv = other.ZhiSiLv;
			}
			if (other.JiangDiHunLuanDu != 0)
			{
				JiangDiHunLuanDu = other.JiangDiHunLuanDu;
			}
			if (other.TodaySiWangDiZiNum != 0)
			{
				TodaySiWangDiZiNum = other.TodaySiWangDiZiNum;
			}
			if (other.JiangLiSuoDing)
			{
				JiangLiSuoDing = other.JiangLiSuoDing;
			}
			if (other.JiangLi1 != 0L)
			{
				JiangLi1 = other.JiangLi1;
			}
			if (other.JiangLi2 != 0)
			{
				JiangLi2 = other.JiangLi2;
			}
			if (other.JiangLi3 != 0)
			{
				JiangLi3 = other.JiangLi3;
			}
			if (other.CreateYanWuTaiShengYuTime != 0L)
			{
				CreateYanWuTaiShengYuTime = other.CreateYanWuTaiShengYuTime;
			}
			if (other.UpgradeYanWuTaiShengYuTime != 0L)
			{
				UpgradeYanWuTaiShengYuTime = other.UpgradeYanWuTaiShengYuTime;
			}
			if (other.UpgradeYanWuTaiMatrixShengYuTime != 0L)
			{
				UpgradeYanWuTaiMatrixShengYuTime = other.UpgradeYanWuTaiMatrixShengYuTime;
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
			case 24u:
				YanWuTaiStage = input.ReadInt32();
				break;
			case 32u:
				YanWuTaiNum = input.ReadInt32();
				break;
			case 40u:
				MatrixStage = input.ReadInt32();
				break;
			case 48u:
				ZhiSiLv = input.ReadInt32();
				break;
			case 56u:
				JiangDiHunLuanDu = input.ReadInt32();
				break;
			case 64u:
				TodaySiWangDiZiNum = input.ReadInt32();
				break;
			case 72u:
				JiangLiSuoDing = input.ReadBool();
				break;
			case 80u:
				JiangLi1 = input.ReadInt64();
				break;
			case 88u:
				JiangLi2 = input.ReadInt32();
				break;
			case 96u:
				JiangLi3 = input.ReadInt32();
				break;
			case 104u:
				CreateYanWuTaiShengYuTime = input.ReadInt64();
				break;
			case 112u:
				UpgradeYanWuTaiShengYuTime = input.ReadInt64();
				break;
			case 120u:
				UpgradeYanWuTaiMatrixShengYuTime = input.ReadInt64();
				break;
			}
		}
	}
}
