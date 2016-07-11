using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.Data.Enumerations;
using EloBuddy;
using LeagueSharp.SDK.Core.Utils;

namespace ExorAIO.Champions.Sivir
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
        public static void Automatic(EventArgs args)
        {
            if (Bools.HasSheenBuff())
            {
                return;
            }

            /// <summary>
            ///     The Automatic Q Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Vars.getCheckBoxItem(Vars.QMenu, "logical"))
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        Bools.IsImmobile(t) &&
                        !Invulnerable.Check(t) &&
                        t.LSIsValidTarget(Vars.Q.Range)))
                {
                    Vars.Q.Cast(target.ServerPosition);
                }
            }
        }

        /// <summary>
        ///     Called while processing Spellcasting operations.
        ///     Port this berbb :^)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        public static void AutoShield(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Target == null)
            {
                return;
            }

            switch (sender.Type)
            {
                case GameObjectType.AIHeroClient:

                    if (Invulnerable.Check(GameObjects.Player, DamageType.True, false))
                    {
                        return;
                    }

                    /// <summary>
                    ///     Check for Special AoE Spells.
                    /// </summary>
                    if (!args.Target.IsMe)
                    {
                        /// <summary>
                        ///     Block Gangplank's Barrels 1st Part.
                        /// </summary>
                        if ((sender as AIHeroClient).ChampionName.Equals("Gangplank"))
                        {
                            if (AutoAttack.IsAutoAttack(args.SData.Name) ||
                                args.SData.Name.Equals("GangplankQProceed"))
                            {
                                if ((args.Target as Obj_AI_Minion).Health == 1 &&
                                    (args.Target as Obj_AI_Minion).CharData.BaseSkinName.Equals("gangplankbarrel"))
                                {
                                    if (GameObjects.Player.Distance(args.Target) < 450)
                                    {
                                        Vars.E.Cast();
                                        return;
                                    }
                                }
                            }
                        }

                        /// <summary>
                        ///     Block Gangplank's Barrels 2nd Part.
                        /// </summary>
                        if (args.SData.Name.Equals("GangplankEBarrelFuseMissile"))
                        {
                            if (GameObjects.Player.Distance(args.End) < 450)
                            {
                                Vars.E.Cast();
                                return;
                            }
                        }
                    }

                    /// <summary>
                    ///     Check for Special AutoAttacks & Melee AutoAttack Resets.
                    /// </summary>
                    if (AutoAttack.IsAutoAttack(args.SData.Name))
                    {
                        if ((!sender.IsMelee && args.SData.Name.Contains("Card")) ||
                            sender.Buffs.Any(b => AutoAttack.IsAutoAttackReset(args.SData.Name)))
                        {
                            Vars.E.Cast();
                            return;
                        }
                    }

                    /// <summary>
                    ///     Report whenever a Targetted spell doesn't exist in LeagueSharp.Data.
                    /// </summary>
                    if (SpellDatabase.GetByName(args.SData.Name) == null)
                    {
                        Console.WriteLine($"{args.SData.Name} + is null in the SpellDatabase!");
                        return;
                    }

                    /// <summary>
                    ///     Shield all the Targetted Spells.
                    /// </summary>
                    if (SpellDatabase.GetByName(args.SData.Name).CastType.Contains(CastType.EnemyChampions))
                    {
                        switch (SpellDatabase.GetByName(args.SData.Name).SpellType)
                        {
                            case SpellType.Targeted:
                            case SpellType.TargetedMissile:

                                if (args.SData.Name.Equals("KatarinaE") ||
                                    args.SData.Name.Equals("TalonCutthroat"))
                                {
                                    return;
                                }

                                switch (sender.CharData.BaseSkinName)
                                {
                                    case "Zed":
                                        DelayAction.Add(200, () => { Vars.E.Cast(); });
                                        break;

                                    case "Caitlyn":
                                        DelayAction.Add(1050, () => { Vars.E.Cast(); });
                                        break;

                                    case "Nocturne":
                                        DelayAction.Add(350, () => { Vars.E.Cast(); });
                                        break;

                                    default:
                                        DelayAction.Add(Vars.getSliderItem(Vars.EMenu, "delay"), () => { Vars.E.Cast(); });
                                        break;
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    break;

                case GameObjectType.obj_AI_Minion:

                    /// <summary>
                    ///     Block Dragon/Baron/RiftHerald's AutoAttacks.
                    /// </summary>
                    if (args.Target.IsMe &&
                        Vars.getCheckBoxItem(Vars.EMenu, "minions"))
                    {
                        if (sender.CharData.BaseSkinName.Equals("SRU_Baron") ||
                            sender.CharData.BaseSkinName.Contains("SRU_Dragon") ||
                            sender.CharData.BaseSkinName.Equals("SRU_RiftHerald"))
                        {
                            Vars.E.Cast();
                            return;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}