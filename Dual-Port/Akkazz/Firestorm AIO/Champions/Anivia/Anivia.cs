using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firestorm_AIO.Helpers;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using EloBuddy;
using EloBuddy.SDK;

namespace Firestorm_AIO.Champions.Anivia
{
    public class Anivia :Bases.ChampionBase
    {
        public override void Init()
        {
            Q = new LeagueSharp.SDK.Spell(SpellSlot.Q, 1100);
            W = new LeagueSharp.SDK.Spell(SpellSlot.W, 1000);
            E = new LeagueSharp.SDK.Spell(SpellSlot.E, 600);
            R = new LeagueSharp.SDK.Spell(SpellSlot.R, 750);

            Q.SetSkillshot(0.25f, 110f, 850f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.25f, 200f, float.MaxValue, false, SkillshotType.SkillshotCircle);
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

#region Functions

        private static void CastQ()
        {
            if (Q.Instance.ToggleState == 1)
            {
                Q.SmartCast(Target, HitChance.High);
            }

            if (Q.Instance.ToggleState == 2)
            {
                
            }
        }
#endregion Functions

        public override void Active()
        {
            Target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);
        }

        public override void Combo()
        {
        }

        public override void Mixed()
        {
        }

        public override void LaneClear()
        {
        }

        public override void LastHit()
        {
        }

        public override void KillSteal()
        {
        }

        public override void Draw()
        {
        }
    }
}
