using System.Linq;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;
using static Firestorm_AIO.Helpers.Helpers;
using EloBuddy;
using EloBuddy.SDK;

namespace Firestorm_AIO.Helpers
{
    public static class SmartCaster
    {
        #region GetTarget

        public static Obj_AI_Minion GetBestLastHitMinion(this LeagueSharp.SDK.Spell spell)
        {
            return
                GameObjects.EnemyMinions.Where(m => m.LSIsValidTarget(spell.Range))
                    .OrderBy(m => m.Health)
                    .FirstOrDefault(m => Health.GetPrediction(m, (int) spell.Delay*1000) < spell.GetDamage(m));
        }

        #endregion GetTarget

        public static void SmartCast(this LeagueSharp.SDK.Spell spell, Obj_AI_Base target = null, HitChance hitchance = HitChance.Medium)
        {
            if (!spell.CanCast(target)) return;

            var hero = target as AIHeroClient;
            if (hero != null && (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)))
            {
                if (spell.IsSkillshot)
                {
                    if (spell.Collision)
                    {
                        if (Me.CountEnemyHeroesInRange(spell.Range) <= 2)
                        {
                            spell.CastOnBestTarget(0f, true);
                        }

                        if (Me.CountEnemyHeroesInRange(spell.Range) >= 2)
                        {
                            spell.CastOnBestTarget(0f, true, 1);
                        }
                    }
                    else
                    {
                        spell.CastIfHitchanceMinimum(target, hitchance);
                    }
                }
                else
                {
                    spell.CastOnUnit(target);
                }
            }

            var minion = target as Obj_AI_Minion;
            if (minion != null)
            {
                if (spell.IsSkillshot)
                {
                    if (spell.Collision)
                    {
                        spell.CastIfHitchanceMinimum(target, HitChance.Low);
                    }
                    else
                    {
                        if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                        {
                            var minions =
                                GameObjects.EnemyMinions.Where(m => m.LSIsValidTarget())
                                    .OrderBy(m => m.Distance(target))
                                    .ThenBy(m => m.Health)
                                    .ToList();

                            if (minions != null)
                            {
                                switch (spell.Type)
                                {
                                    case SkillshotType.SkillshotLine:
                                        var posLine = spell.GetLineFarmLocation(minions);
                                        if (posLine.MinionsHit >= 1)
                                        {
                                            spell.Cast(posLine.Position);
                                        }
                                        break;
                                    case SkillshotType.SkillshotCircle:
                                        var posCircle = spell.GetCircularFarmLocation(minions);
                                        if (posCircle.MinionsHit >= 1)
                                        {
                                            spell.Cast(posCircle.Position);
                                        }
                                        break;
                                    case SkillshotType.SkillshotCone:
                                        var posCone = spell.GetLineFarmLocation(minions, spell.Width);
                                        if (posCone.MinionsHit >= 1)
                                        {
                                            spell.Cast(posCone.Position);
                                        }
                                        break;
                                }
                            }
                            spell.Cast(target.Position);
                        }

                        if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                        {
                            spell.CastIfHitchanceMinimum(target, HitChance.Low);
                        }
                    }
                }
                else
                {
                    spell.CastOnUnit(target);
                }
            }
        }
    }
}
