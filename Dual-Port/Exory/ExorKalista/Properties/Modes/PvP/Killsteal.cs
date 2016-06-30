using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.Data.Enumerations;
using EloBuddy;
using EloBuddy.SDK;

namespace ExorAIO.Champions.Kalista
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Killsteal(EventArgs args)
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Vars.getCheckBoxItem(Vars.QMenu, "killsteal"))
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        !Invulnerable.Check(t) &&
                        !Bools.IsPerfectRendTarget(t) &&
                        t.LSIsValidTarget(Vars.Q.Range) &&
                        !t.LSIsValidTarget(Vars.AARange) &&
                        Vars.GetRealHealth(t) <
                            (float)GameObjects.Player.LSGetSpellDamage(t, SpellSlot.Q)))
                {
                    if (!Vars.Q.GetPrediction(target).CollisionObjects.Any() ||
						Vars.Q.GetPrediction(target).CollisionObjects.Count(
                        c =>
                            Targets.Minions.Contains(c) &&
                            c.Health <
                                (float)GameObjects.Player.LSGetSpellDamage(c, SpellSlot.Q)) == Vars.Q.GetPrediction(target).CollisionObjects.Count(c => Targets.Minions.Contains(c)))
                    {
                        Vars.Q.Cast(Vars.Q.GetPrediction(target).UnitPosition);
                    }
                }
            }

            /// <summary>
            ///     The KillSteal E Logic.
            /// </summary>
            if (Vars.E.IsReady() &&
                Vars.getCheckBoxItem(Vars.EMenu, "killsteal"))
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        Bools.IsPerfectRendTarget(t) &&
                        Vars.GetRealHealth(t) < EDamage(t)))
                {
                    Vars.E.Cast();
                }
            }
        }

        private static float EDamage(Obj_AI_Base target)
        {
            if (target.IsMinion || target.IsMonster)
            {
                int stacksMin = GetMinionStacks(target);
                var indexMin = Vars.E.Level - 1;

                var EDamageMinion = new float[] { 20, 30, 40, 50, 60 }[indexMin] + (0.6 * ObjectManager.Player.TotalAttackDamage);

                if (stacksMin > 1)
                {
                    EDamageMinion += ((new float[] { 10, 14, 19, 25, 32 }[indexMin] + (new float[] { 0.2f, 0.225f, 0.25f, 0.275f, 0.3f }[indexMin] * ObjectManager.Player.TotalAttackDamage)) * (stacksMin - 1));
                }

                return ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.Physical, (float)EDamageMinion) * 0.9f;
            }
            else
            {
                if (GetStacks(target) == 0) return 0;

                int stacksChamps = GetStacks(target);
                var indexChamp = Vars.E.Level - 1;

                var EDamageChamp = new[] { 0, 20, 30, 40, 50, 60 }[indexChamp] + (0.6 * ObjectManager.Player.TotalAttackDamage);

                if (stacksChamps > 1)
                {
                    EDamageChamp += ((new[] { 0, 10, 14, 19, 25, 32 }[indexChamp] + (new[] { 0, 0.2, 0.225, 0.25, 0.275, 0.3 }[indexChamp] * ObjectManager.Player.TotalAttackDamage)) * (stacksChamps - 1));
                }

                return ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.Physical, (float)EDamageChamp);
            }
        }

        private static int GetMinionStacks(Obj_AI_Base minion)
        {
            int stacks = 0;
            foreach (var rendbuff in minion.Buffs.Where(x => x.Name.ToLower().Contains("kalistaexpungemarker")))
            {
                stacks = rendbuff.Count;
            }

            if (stacks == 0 || !minion.HasBuff("kalistaexpungemarker")) return 0;
            return stacks;
        }

        private static int GetStacks(Obj_AI_Base target)
        {
            int stacks = 0;

            if (target.HasBuff("kalistaexpungemarker"))
            {
                foreach (var rendbuff in target.Buffs.Where(x => x.Name.ToLower().Contains("kalistaexpungemarker")))
                {
                    stacks = rendbuff.Count;
                }
            }
            else
            {
                return 0;
            }
            return stacks;
        }
    }
}