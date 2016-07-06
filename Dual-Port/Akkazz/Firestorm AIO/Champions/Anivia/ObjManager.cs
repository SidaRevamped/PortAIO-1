using System;
using Firestorm_AIO.Bases;
using LeagueSharp;
using SharpDX;
using EloBuddy;

namespace Firestorm_AIO.Champions.Anivia
{
    public class QManager
    {
        public static MyObjectBase QObject;

        //TODO Test this shit het the proper name
        public static void Load()
        {
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "cryo_FlashFrost_Player_mis.troy" && sender.IsAlly)
            {
                QObject = new MyObjectBase(sender.Position);
            }
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name == "cryo_FlashFrost_Player_mis.troy" && sender.IsAlly)
            {
                QObject = null;
            }
        }
    }

    public class RManager
    {
        public static MyObjectBase RObject;

        public static void Load()
        {
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
        }

        //TODO Test and get proper name
        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (sender.Name == "cryo_storm" && sender.IsAlly)
            {
                RObject = new MyObjectBase(sender.Position);
            }
        }


        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (sender.Name.Contains("cryo_storm") && sender.IsAlly)
            {
                RObject = null;
            }
        }
    }
}
