using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;
using LeagueSharp.Data.Enumerations;
using EloBuddy;

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
                        Vars.GetRealHealth(t) <
                            (float)GameObjects.Player.LSGetSpellDamage(t, SpellSlot.E) +
                            (float)GameObjects.Player.LSGetSpellDamage(t, SpellSlot.E, DamageStage.Buff)))
                {
                    Vars.E.Cast();
                }
            }
        }
    }
}