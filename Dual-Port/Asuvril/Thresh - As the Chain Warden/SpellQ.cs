using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Prediction = SebbyLib.Prediction.Prediction;
using PredictionInput = SebbyLib.Prediction.PredictionInput;
using SkillshotType = SebbyLib.Prediction.SkillshotType;
using SPrediction;
using SebbyLib.Prediction;

namespace Thresh {

	public enum QState
    {
		ThreshQ,
		threshqleap,
		Cooldown
	}
    public static class SpellQ
    {

		public static bool CastQ1(AIHeroClient target)
        {

            var Q = Thresh.Q;
            if (Q.IsReady())
            {
                CastSpell(Q, target);
            }
            return false;
		}

        public static void CastSpell(LeagueSharp.Common.Spell QWER, AIHeroClient target)
        {
         //   Chat.Print("CastSpell");
             if (getBoxItem(Thresh.PredictConfig, "PredictionMode") == 2)
             {
          //       Chat.Print("SDK");
                 SebbyLib.Movement.SkillshotType CoreType2 = SebbyLib.Movement.SkillshotType.SkillshotLine;
                 bool aoe2 = false;

                 if (QWER.Type == LeagueSharp.Common.SkillshotType.SkillshotCircle)
                 {
                     //CoreType2 = SebbyLib.Movement.SkillshotType.SkillshotCircle;
                     //aoe2 = true;
                 }

                 if (QWER.Width > 80 && !QWER.Collision)
                     aoe2 = true;

                 var predInput2 = new SebbyLib.Movement.PredictionInput
                 {
                     Aoe = aoe2,
                     Collision = QWER.Collision,
                     Speed = QWER.Speed,
                     Delay = QWER.Delay,
                     Range = QWER.Range,
                     From = Thresh.Player.ServerPosition,
                     Radius = QWER.Width,
                     Unit = target,
                     Type = CoreType2
                 };
                 var poutput2 = SebbyLib.Movement.Prediction.GetPrediction(predInput2);

                 //var poutput2 = QWER.GetPrediction(target);

                 if (QWER.Speed != float.MaxValue && SebbyLib.OktwCommon.CollisionYasuo(Thresh.Player.ServerPosition, poutput2.CastPosition))
                     return;

                 if (getBoxItem(Thresh.PredictConfig, "HitChance") == 0)
                 {
                     if (poutput2.Hitchance >= SebbyLib.Movement.HitChance.VeryHigh)
                         QWER.Cast(poutput2.CastPosition);
                     else if (predInput2.Aoe && poutput2.AoeTargetsHitCount > 1 && poutput2.Hitchance >= SebbyLib.Movement.HitChance.High)
                     {
                         QWER.Cast(poutput2.CastPosition);
                     }

                 }
                 else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 1)
                 {
                     if (poutput2.Hitchance >= SebbyLib.Movement.HitChance.High)
                         QWER.Cast(poutput2.CastPosition);

                 }
                 else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 2)
                 {
                     if (poutput2.Hitchance >= SebbyLib.Movement.HitChance.Medium)
                         QWER.Cast(poutput2.CastPosition);
                 }
             }
             else
           if(Thresh.getBoxItem(Thresh.PredictConfig, "PredictionMode") == 1)
            {
          //      Chat.Print("OKTW");
                SebbyLib.Prediction.SkillshotType CoreType2 = SebbyLib.Prediction.SkillshotType.SkillshotLine;
                bool aoe2 = false;

                if (QWER.Type == LeagueSharp.Common.SkillshotType.SkillshotCircle)
                {
                    CoreType2 = SebbyLib.Prediction.SkillshotType.SkillshotCircle;
                    aoe2 = true;
                }

                if (QWER.Width > 80 && !QWER.Collision)
                    aoe2 = true;

                var predInput2 = new SebbyLib.Prediction.PredictionInput
                {
                    Aoe = aoe2,
                    Collision = QWER.Collision,
                    Speed = QWER.Speed,
                    Delay = QWER.Delay,
                    Range = QWER.Range,
                    From = Thresh.Player.ServerPosition,
                    Radius = QWER.Width,
                    Unit = target,
                    Type = CoreType2
                };
                var poutput2 = SebbyLib.Prediction.Prediction.GetPrediction(predInput2);

                //var poutput2 = QWER.GetPrediction(target);

                if (QWER.Speed != float.MaxValue && SebbyLib.OktwCommon.CollisionYasuo(Thresh.Player.ServerPosition, poutput2.CastPosition))
                    return;

                if (getBoxItem(Thresh.PredictConfig, "HitChance") == 0)
                {
                    if (poutput2.Hitchance >= SebbyLib.Prediction.HitChance.VeryHigh)
                        QWER.Cast(poutput2.CastPosition);
                    else if (predInput2.Aoe && poutput2.AoeTargetsHitCount > 1 && poutput2.Hitchance >= SebbyLib.Prediction.HitChance.High)
                    {
                        QWER.Cast(poutput2.CastPosition);
                    }

                }
                else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 1)
                {
                    if (poutput2.Hitchance >= SebbyLib.Prediction.HitChance.High)
                        QWER.Cast(poutput2.CastPosition);

                }
                else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 2)
                {
                    if (poutput2.Hitchance >= SebbyLib.Prediction.HitChance.Medium)
                        QWER.Cast(poutput2.CastPosition);
                }            
            }
            else 
            if (Thresh.getBoxItem(Thresh.PredictConfig, "PredictionMode") == 0)
            {
          //      Chat.Print("Common");
                if (getBoxItem(Thresh.PredictConfig, "HitChance") == 0)
                {
                    QWER.CastIfHitchanceEquals(target, LeagueSharp.Common.HitChance.VeryHigh);
                    return;
                }
                else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 1)
                {
                    QWER.CastIfHitchanceEquals(target, LeagueSharp.Common.HitChance.High);
                    return;
                }
                else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 2)
                {
                    QWER.CastIfHitchanceEquals(target, LeagueSharp.Common.HitChance.Medium);
                    return;
                }
            }
            
            else if (getBoxItem(Thresh.PredictConfig, "PredictionMode") == 3)
            {
          //  Chat.Print("Prediction");
                if (target is AIHeroClient && target.IsValid)
                {
              //      var t = target as AIHeroClient;
                    if (getBoxItem(Thresh.PredictConfig, "HitChance") == 0)
                    {
                        QWER.SPredictionCast(target, LeagueSharp.Common.HitChance.VeryHigh);
                        return;
                    }
                    else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 1)
                    {
                        QWER.SPredictionCast(target, LeagueSharp.Common.HitChance.High);
                        return;
                    }
                    else if (getBoxItem(Thresh.PredictConfig, "HitChance") == 2)
                    {
                        QWER.SPredictionCast(target, LeagueSharp.Common.HitChance.Medium);
                        return;
                    }
                }
                else
                {
                    QWER.CastIfHitchanceEquals(target, LeagueSharp.Common.HitChance.High);
                }
            }
        }

        public static bool CastQ2()
        {
            if (!Thresh.getCheckBoxItem(Thresh.SpellConfig, "dontQ2"))
            {
                if (Thresh.QTarget is AIHeroClient && Thresh.QTarget.GetPassiveTime("ThreshQ") < 0.3)
                {
                    return Thresh.Q.Cast();
                }
            }
             
			return false;
		}
        public static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }

        public static bool getKeyBindItem(Menu m, string item)
        {
            return m[item].Cast<KeyBind>().CurrentValue;
        }

        public static int getBoxItem(Menu m, string item)
        {
            return m[item].Cast<ComboBox>().CurrentValue;
        }
        public static QState GetState() {
			if (!Thresh.Q.IsReady())
			{
				return QState.Cooldown;
			}
			else
			{
				if (Thresh.Q.Instance.Name == "ThreshQ")
				{
					return QState.ThreshQ;
				}
				else
				{
					return QState.threshqleap;
				}
			}
		}
	}
}
