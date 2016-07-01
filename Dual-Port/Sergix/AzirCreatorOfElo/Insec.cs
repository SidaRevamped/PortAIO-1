using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;
using SharpDX;
using System.Text.RegularExpressions;
using Color = System.Drawing.Color;
using Azir_Creator_of_Elo;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Azir_Free_elo_Machine
{
    class Insec
    {
        Azir_Creator_of_Elo.AzirMain azir;
        public Insec(AzirMain azir)
        {
            this.azir = azir;
            Game.OnUpdate += Game_OnUpdate;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            var target = TargetSelector.SelectedTarget;
            /*     var posWs = GeoAndExten.GetWsPosition(target.Position.To2D()).Where(x => x != null);
                 foreach (var posW in posWs)
                 {

                 }*/
        }
        private void Game_OnWndProc(WndEventArgs args)
        {

        }

        private void Game_OnUpdate(EventArgs args)
        {
            var insecPoint = Game.CursorPos;
            if (!Menu._jumpMenu["inseckey"].Cast<KeyBind>().CurrentValue)
                return;
            azir.Orbwalk(Game.CursorPos);
            if (!insecPoint.IsValid())
                return;
            var target = TargetSelector.SelectedTarget;
            if (!target.IsValidTarget() || target.IsZombie)
                return;
            var soldier = azir.soldierManager.ActiveSoldiers
        .Where(x => azir.Hero.LSDistance(x.Position) <= 1100)
        .OrderBy(x => x.Position.Distance(target.Position)).FirstOrDefault();


        }
    }
}