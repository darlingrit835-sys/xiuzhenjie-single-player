using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiuZhenJie
{
	public class BaseLifeBiological : MonoBehaviour
	{
		public Image JianRenProgress;

		public Text BloodValueText;

		public Text BloodDetailValueText;

		public Image BloodProgress;

		public Image HuDunProgress;

		public Image ZhenYuanProgress;

		public Text ZhenYuanValueText;

		public Text ZhenYuanTitleText;

		public Text ZhenYuanDetailValueText;

		public Image ShenShiProgress;

		public Text ShenShiValueText;

		public Text ShenShiDetailValueText;

		public Image TouXiangImg;

		protected const int ChangShu = 1;

		public double CurrentSuperAir;

		public double CurrentPerception;

		protected double CurrentPureBlood;

		protected int SelfWuXing;

		protected double CurrentToughTenacity;

		public List<double> CurrentEquipmentTenacity;

		public List<int> CurrentEquipmentWuXing;

		protected Dictionary<string, HuDunModel> CurrentFaShuHuDunDic;

		protected double CurrentFaShuHuDun;

		protected int CurrentFaShuHuDunWuXing;

		protected double CurrentFaQiHuDun;

		protected int CurrentFaQiHuDunWuXing;

		public bool IsAlive;

		protected float AttackSpacingTime;

		protected double NaQiSeconds;

		protected double bloodOne;

		protected double bloodTwo;

		protected double bloodThree;

		protected double bloodFour;

		protected double bloodFive;

		protected double OldBloodOne;

		protected double OldBloodTwo;

		protected double OldBloodThree;

		protected double OldBloodFour;

		protected double OldBloodFive;

		private LifeType currentLifeType;

		public void Init()
		{
			CurrentEquipmentTenacity = new List<double>();
			CurrentEquipmentWuXing = new List<int>();
			CurrentFaShuHuDunDic = new Dictionary<string, HuDunModel>();
			IsAlive = true;
			AttackSpacingTime = 2f;
			SelfWuXing = 0;
		}

		public void InstanceData(object _arg, LifeType _lifeType)
		{
			currentLifeType = _lifeType;
			switch (_lifeType)
			{
			case LifeType.People:
				if (!(_arg is RoleInfoModel))
				{
					Debug.Log("数据源是空");
					break;
				}
				InitPeople(_arg as RoleInfoModel);
				StartCoroutine(ProgressValueInitAni(1));
				break;
			case LifeType.Monster:
				InitMonster(_arg as MonsterModel);
				StartCoroutine(ProgressValueInitAni(2));
				break;
			}
		}

		private IEnumerator ProgressValueInitAni(int type)
		{
			if (type == 1)
			{
				double value = 0.0;
				for (int i = 0; i < 50; i++)
				{
					JianRenProgress.fillAmount += 0.02f;
					BloodProgress.fillAmount += 0.02f;
					HuDunProgress.fillAmount += 0.02f;
					ZhenYuanProgress.fillAmount += 0.02f;
					if ((double)ShenShiProgress.fillAmount >= value)
					{
						ShenShiProgress.fillAmount = (float)value;
					}
					else
					{
						ShenShiProgress.fillAmount += 0.02f;
					}
					yield return new WaitForSeconds(0.01f);
				}
			}
			else
			{
				for (int j = 0; j < 50; j++)
				{
					JianRenProgress.fillAmount += 0.02f;
					BloodProgress.fillAmount += 0.02f;
					HuDunProgress.fillAmount += 0.02f;
					ZhenYuanProgress.fillAmount += 0.02f;
					ShenShiProgress.fillAmount += 0.02f;
				}
				yield return new WaitForSeconds(0.01f);
			}
		}

		private void InitMonster(MonsterModel _dataSource)
		{
			CurrentSuperAir = _dataSource.superAir;
			SelfWuXing = _dataSource.fiveElement;
			MonsterBloodValueInit(_dataSource);
			CurrentPerception = 100.0;
			BloodInit();
		}

		private void MonsterBloodValueInit(MonsterModel _dataSource)
		{
			double num = _dataSource.toughTenacity / 100.0 * (double)Mathf.Pow(_dataSource.level, 2f + (float)_dataSource.level / 60f);
			CurrentToughTenacity = num / 2.799999952316284;
			double num2 = _dataSource.maxPureBlood * (double)Mathf.Pow(_dataSource.level, 2f + (float)_dataSource.level / 60f);
			CurrentPureBlood = num2 / 2.799999952316284;
		}

		private void InitPeople(RoleInfoModel _roleInfoModel)
		{
		}

		private void PeopleBloodValueInit(RoleInfoModel _roleInfoModel)
		{
			double num = _roleInfoModel.js.physique / 100.0 * (double)Mathf.Pow(_roleInfoModel.js.level, 2f + (float)_roleInfoModel.js.level / (20f + 0.22f * (float)_roleInfoModel.js.level));
			CurrentToughTenacity = num / 2.0;
			double num2 = (_roleInfoModel.js.currentPureBlood - 7.0) * (double)Mathf.Pow(_roleInfoModel.js.level, 2f + (float)_roleInfoModel.js.level / (30f + 0.25f * (float)_roleInfoModel.js.level));
			CurrentPureBlood = num2 / 2.0;
		}

		protected virtual void ReflashData()
		{
			bloodOne = CurrentPureBlood;
			bloodTwo = CurrentToughTenacity * 1.0;
			bloodThree = ComputeBlood(CurrentEquipmentTenacity, CurrentEquipmentWuXing);
			if (CurrentFaShuHuDunDic != null)
			{
				double num = 0.0;
				foreach (HuDunModel value in CurrentFaShuHuDunDic.Values)
				{
					num += value.Value;
				}
				if (num != 0.0 && num > CurrentFaShuHuDun)
				{
					CurrentFaShuHuDun = num;
				}
			}
			bloodFour = CurrentFaShuHuDun;
			bloodFive = CurrentFaQiHuDun;
			double num2 = ((OldBloodOne == 0.0) ? 0.0 : (bloodOne / OldBloodOne));
			double num3 = ((OldBloodTwo + OldBloodThree == 0.0) ? 0.0 : ((bloodTwo + bloodThree) / (OldBloodTwo + OldBloodThree)));
			double num4 = (bloodFour + bloodFive) / (OldBloodFive + OldBloodFour);
			JianRenProgress.fillAmount = (float)num3;
			BloodProgress.fillAmount = (float)num2;
			HuDunProgress.fillAmount = (float)num4;
			if (IsAlive)
			{
				BloodValueText.text = $"{(ulong)(100.0 * ((bloodOne + bloodTwo + bloodThree + bloodFour + bloodFive) / (OldBloodOne + OldBloodTwo + OldBloodThree + OldBloodFour + OldBloodFive)))}%";
				BloodDetailValueText.text = WorldSetting.simplifyNumber((ulong)(bloodOne + bloodTwo + bloodThree + bloodFour + bloodFive));
			}
			else
			{
				BloodValueText.text = "0%";
				BloodDetailValueText.text = "0";
			}
			CurrentFaShuHuDunDic.Clear();
		}

		private void BloodInit()
		{
			OldBloodOne = CurrentPureBlood;
			OldBloodTwo = CurrentToughTenacity * 1.0;
			OldBloodThree = ComputeBlood(CurrentEquipmentTenacity, CurrentEquipmentWuXing, isDeleteData: false);
			OldBloodFour = 0.0;
			OldBloodFive = 0.0;
			ReflashData();
		}

		protected virtual void Attack(object skillModel, LifeType lifeType, BaseLifeBiological target)
		{
		}

		public virtual void AutoAttack()
		{
		}

		public virtual double UnderAttacked(double damageValue, WuXing AttackWuXing, SkillType damageType, SkillGroupType groupType, float dotSpacingTime = 0f, float dotDurationTime = 0f)
		{
			double result = 0.0;
			switch (damageType)
			{
			case SkillType.ATTACK:
				result = FinallyDamageCalculate(ref damageValue, AttackWuXing);
				break;
			case SkillType.DOT:
				result = FinallyDamageCalculate(ref damageValue, AttackWuXing);
				break;
			}
			return result;
		}

		public virtual void Deaded()
		{
			if (currentLifeType == LifeType.Monster)
			{
				IsAlive = false;
				BloodValueText.text = "0";
				CurrentSuperAir = 0.0;
				CurrentPerception = 0.0;
				CurrentPureBlood = 0.0;
				AttackSpacingTime = 0f;
				NaQiSeconds = 0.0;
				bloodOne = 0.0;
				bloodTwo = 0.0;
				bloodThree = 0.0;
				bloodFour = 0.0;
				bloodFive = 0.0;
				OldBloodOne = 0.0;
				OldBloodTwo = 0.0;
				OldBloodThree = 0.0;
				OldBloodFour = 0.0;
				OldBloodFive = 0.0;
				CurrentEquipmentTenacity.Clear();
				CurrentEquipmentWuXing.Clear();
				CurrentFaShuHuDun = 0.0;
				CurrentFaShuHuDunWuXing = -1;
				CurrentFaQiHuDun = 0.0;
				CurrentFaQiHuDunWuXing = -1;
			}
		}

		protected double FinallyDamageCalculate(ref double value, WuXing wuXing)
		{
			double num = 0.0;
			if (value > 0.0)
			{
				num += FaQiHuDunReduce(ref value, wuXing);
			}
			if (value > 0.0)
			{
				num += FaShuHuDunReduce(ref value, wuXing);
			}
			if (value > 0.0)
			{
				num += EquipmentQiangRenReduce(ref value, wuXing);
			}
			if (value > 0.0)
			{
				num += SelfQiangRenReduce(ref value, wuXing);
			}
			if (value > 0.0)
			{
				num += BloodReduce(value);
			}
			return num;
		}

		protected double BloodReduce(double damageValue)
		{
			double num = damageValue - damageValue * 0.25;
			CurrentPureBlood -= num;
			if (CurrentPureBlood <= 0.0)
			{
				CurrentPureBlood = 0.0;
				Deaded();
			}
			return num;
		}

		protected double SelfQiangRenReduce(ref double damageValue, WuXing wuXing)
		{
			if (CurrentToughTenacity == 0.0)
			{
				return 0.0;
			}
			return WorldSetting.GetWuXingComplement((WuXing)SelfWuXing, wuXing) switch
			{
				1 => DefanceComputeUp(ref CurrentToughTenacity, ref damageValue), 
				2 => DefanceComputeDown(ref CurrentToughTenacity, ref damageValue), 
				_ => DefanceCommon(ref CurrentToughTenacity, ref damageValue), 
			};
		}

		protected double EquipmentQiangRenReduce(ref double damageValue, WuXing wuXing)
		{
			if (CurrentEquipmentTenacity == null || CurrentEquipmentWuXing == null)
			{
				return 0.0;
			}
			double num = 0.0;
			for (int i = 0; i < CurrentEquipmentTenacity.Count; i++)
			{
				int wuXingComplement = WorldSetting.GetWuXingComplement((WuXing)CurrentEquipmentWuXing[i], wuXing);
				double DefanceValue = CurrentEquipmentTenacity[i];
				num = wuXingComplement switch
				{
					1 => num + DefanceComputeUp(ref DefanceValue, ref damageValue), 
					2 => num + DefanceComputeDown(ref DefanceValue, ref damageValue), 
					_ => num + DefanceCommon(ref DefanceValue, ref damageValue), 
				};
				CurrentEquipmentTenacity[i] = DefanceValue;
			}
			return num;
		}

		protected double FaQiHuDunReduce(ref double damageValue, WuXing wuXing)
		{
			if (CurrentFaQiHuDun == 0.0 || CurrentFaQiHuDunWuXing == -1)
			{
				return 0.0;
			}
			double num = 0.0;
			int wuXingComplement = WorldSetting.GetWuXingComplement((WuXing)CurrentFaQiHuDunWuXing, wuXing);
			double DefanceValue = CurrentFaQiHuDun;
			num = wuXingComplement switch
			{
				1 => num + DefanceComputeUp(ref DefanceValue, ref damageValue), 
				2 => num + DefanceComputeDown(ref DefanceValue, ref damageValue), 
				_ => num + DefanceCommon(ref DefanceValue, ref damageValue), 
			};
			CurrentFaQiHuDun = DefanceValue;
			if (CurrentFaQiHuDun <= 0.0)
			{
				CurrentFaQiHuDun = 0.0;
				CurrentFaQiHuDunWuXing = -1;
			}
			return num;
		}

		protected double FaShuHuDunReduce(ref double damageValue, WuXing wuXing)
		{
			if (CurrentFaShuHuDun == 0.0 || CurrentFaShuHuDunWuXing == -1)
			{
				return 0.0;
			}
			double num = 0.0;
			int wuXingComplement = WorldSetting.GetWuXingComplement((WuXing)CurrentFaShuHuDunWuXing, wuXing);
			double DefanceValue = CurrentFaShuHuDun;
			num = wuXingComplement switch
			{
				1 => num + DefanceComputeUp(ref DefanceValue, ref damageValue), 
				2 => num + DefanceComputeDown(ref DefanceValue, ref damageValue), 
				_ => num + DefanceCommon(ref DefanceValue, ref damageValue), 
			};
			CurrentFaShuHuDun = DefanceValue;
			if (CurrentFaShuHuDun <= 0.0)
			{
				CurrentFaShuHuDun = 0.0;
				CurrentFaShuHuDunWuXing = -1;
			}
			return num;
		}

		private double DefanceComputeUp(ref double DefanceValue, ref double damageValue)
		{
			double num = DefanceValue * 1.5;
			if (damageValue >= num)
			{
				damageValue -= num;
				DefanceValue = 0.0;
				return num;
			}
			double result = damageValue * 1.5;
			DefanceValue -= damageValue * 1.5;
			damageValue = 0.0;
			return result;
		}

		private double DefanceComputeDown(ref double DefanceValue, ref double damageValue)
		{
			double num = DefanceValue * 0.5;
			if (damageValue >= num)
			{
				damageValue -= num;
				DefanceValue = 0.0;
				return num;
			}
			double result = damageValue * 0.5;
			DefanceValue -= damageValue * 0.5;
			damageValue = 0.0;
			return result;
		}

		private double DefanceCommon(ref double DefanceValue, ref double damageValue)
		{
			double num = DefanceValue;
			if (damageValue >= num)
			{
				damageValue -= num;
				DefanceValue = 0.0;
				return num;
			}
			double result = damageValue;
			DefanceValue -= damageValue;
			damageValue = 0.0;
			return result;
		}

		private double ComputeBlood(List<double> data, List<int> wuXing, bool isDeleteData = true)
		{
			if (data.Count != wuXing.Count)
			{
				return 0.0;
			}
			double num = 0.0;
			List<double> list = new List<double>();
			List<int> list2 = new List<int>();
			if (data != null)
			{
				for (int i = 0; i < data.Count; i++)
				{
					if (data[i] != 0.0)
					{
						list.Add(data[i]);
						list2.Add(wuXing[i]);
						num += data[i];
					}
				}
			}
			if (isDeleteData && data.Contains(0.0))
			{
				data.Clear();
				wuXing.Clear();
				for (int j = 0; j < list.Count; j++)
				{
					data.Add(list[j]);
					wuXing.Add(list2[j]);
				}
			}
			return num;
		}
	}
}
