using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firestorm_AIO.Helpers;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using LeagueSharp.SDK.Core.Utils;
using static Firestorm_AIO.Champions.Anivia.ObjManager;
using static Firestorm_AIO.Helpers.Helpers;
using EloBuddy;
using EloBuddy.SDK;

namespace Firestorm_AIO.Champions.Anivia
{
    public class Anivia : Bases.ChampionBase
    {
        private int BreakRange = 1100;
        private int Q2Range = 200;

        public override void Init()
        {
            Q = new LeagueSharp.SDK.Spell(SpellSlot.Q, 1100);
            W = new LeagueSharp.SDK.Spell(SpellSlot.W, 1000);
            E = new LeagueSharp.SDK.Spell(SpellSlot.E, 600);
            R = new LeagueSharp.SDK.Spell(SpellSlot.R, 750);

            Q.SetSkillshot(0.25f, 200f, 850f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.25f, 200f, float.MaxValue, false, SkillshotType.SkillshotCircle);

            Q.IsAntiGapCloser();

            ObjManager.Load();
        }

        public override void Menu()
        {
            Q.CreateBool(ComboMenu);
            W.CreateBool(ComboMenu);
            E.CreateBool(ComboMenu);
            R.CreateBool(ComboMenu);

            Q.CreateBool(LaneClearMenu);
            E.CreateBool(LaneClearMenu);
            R.CreateBool(LaneClearMenu);

            Q.CreateBool(LastHitMenu);
            E.CreateBool(LastHitMenu);

            Q.CreateBool(MixedMenu);
            E.CreateBool(MixedMenu);
        }

        public override void Active()
        {
            Target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (Target == null) return;

            if (Q.Instance.ToggleState >= 2 && QObject != null && QObject.Position.LSIsInRange(Target, Q2Range))
            {
                Q.Cast();
            }
        }

        public override void Combo()
        {
            if (GetBoolValue(Q, ComboMenu))
            {
                if (Q.Instance.ToggleState == 1 && QObject == null)
                {
                    Q.SmartCast(Target, HitChance.High);
                }
            }

            if (GetBoolValue(W, ComboMenu))
            {
                //Cast Wall behind the target if Q is near
                if (QObject != null && QObject.Position.LSIsInRange(Target, 350))
                {
                    var pos = Me.Position.LSExtend(Target.Position, Me.Distance(Target) + 120);
                    if (pos.Distance(Me.Position) < W.Range)
                    {
                        W.Cast(pos);
                    }
                }
            }

            if (GetBoolValue(E, ComboMenu))
            {
                //Only if snowed
                if (Target.HasBuff("chilled"))
                {
                    E.SmartCast(Target);
                }
                //To kill
                if (Target.CanKillTarget(E, (int)(Me.Distance(Target) / 850f)))
                {
                    E.SmartCast(Target);
                }
            }

            if (GetBoolValue(R, ComboMenu))
            {
                R.SmartCast(Target, HitChance.High);
            }
        }

        public override void Mixed()
        {
            if (GetBoolValue(Q, MixedMenu))
            {
                if (Q.Instance.ToggleState == 1 && QObject == null)
                {
                    Q.SmartCast(Target, HitChance.High);
                }
            }

            if (GetBoolValue(E, MixedMenu))
            {
                //Only if snowed
                if (Target.HasBuff("chilled"))
                {
                    E.SmartCast(Target);
                }
                //To kill
                if (Target.CanKillTarget(E, (int)(Me.Distance(Target) / 850f)))
                {
                    E.SmartCast(Target);
                }
            }
        }

        public override void LaneClear()
        {
            if (GetBoolValue(Q, LaneClearMenu))
            {
                if (Q.Instance.ToggleState == 1 && QObject == null)
                {
                    Q.SmartCast(null, HitChance.Medium, 3);
                }

                if (Q.Instance.ToggleState >= 2 && QObject != null && QObject.Position.CountEnemyMinions(Q2Range) >= 3)
                {
                    Q.Cast();
                }
            }
        }

        public override void LastHit()
        {
        }

        public override void KillSteal()
        {
        }

        public override void Draw()
        {
            if (QObject != null)
            {
                Render.Circle.DrawCircle(QObject.Position, Q2Range, QColor);
            }

            if (RObject != null)
            {
                Render.Circle.DrawCircle(RObject.Position, BreakRange, RColor);
            }
        }
    }
}