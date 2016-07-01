using EloBuddy;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;

namespace ExorAIO.Champions.Lucian
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void Weaving(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient) ||
                Invulnerable.Check(args.Target as AIHeroClient))
            {
                return;
            }

            /// <summary>
            ///     The E Combo Logic.
            /// </summary>
            if (Vars.E.IsReady() &&
                Vars.getBoxItem(Vars.EMenu, "mode") != 2)
            {
                if (!Game.CursorPos.IsUnderEnemyTurret() ||
                    (args.Target as AIHeroClient).Health <
                        GameObjects.Player.GetAutoAttackDamage(args.Target as AIHeroClient)*2)
                {
                    if ((GameObjects.Player.Distance(Game.CursorPos) < Vars.AARange ||
                        (args.Target as AIHeroClient).CountEnemyHeroesInRange(700f) >= 2) &&
                        Vars.getBoxItem(Vars.EMenu, "mode") == 0)
                    {
                        Vars.E.Cast(GameObjects.Player.ServerPosition.LSExtend(Game.CursorPos, 50f));
                        return;
                    }
                    else
                    {
                        Vars.E.Cast(GameObjects.Player.ServerPosition.LSExtend(Game.CursorPos, Vars.E.Range - Vars.AARange));
                    }
                }
            }

            /// <summary>
            ///     The Q Combo Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                (args.Target as AIHeroClient).LSIsValidTarget(Vars.Q.Range) &&
                (args.Target as AIHeroClient).Health >
                    GameObjects.Player.GetAutoAttackDamage(args.Target as AIHeroClient) &&
                Vars.getCheckBoxItem(Vars.QMenu, "combo"))
            {
                Vars.Q.CastOnUnit(args.Target as AIHeroClient);
                return;
            }

            /// <summary>
            ///     The W Combo Logic.
            /// </summary>
            if (Vars.W.IsReady() &&
                Vars.getCheckBoxItem(Vars.WMenu, "combo"))
            {
                Vars.W.Cast(Vars.W.GetPrediction(args.Target as AIHeroClient).UnitPosition);
            }
        }
    }
}