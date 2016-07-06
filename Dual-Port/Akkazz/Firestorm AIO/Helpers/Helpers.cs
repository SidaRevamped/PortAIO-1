using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Core.Utils;
using EloBuddy;

namespace Firestorm_AIO.Helpers
{
    public static class Helpers
    {
        public static AIHeroClient Me => GameObjects.Player;

        public static Obj_AI_Minion GetBestJungleClearMinion(this Spell spell)
        {
            return GameObjects.Jungle.Where(spell.CanCast).OrderByDescending(m => m.MaxHealth).FirstOrDefault();
        }
    }
}
