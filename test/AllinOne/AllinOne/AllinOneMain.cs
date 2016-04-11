﻿using Ensage.Common.Extensions;

namespace AllinOne
{
    using AllinOne.AllDrawing;
    using AllinOne.AllEvents;
    using AllinOne.Menu;
    using AllinOne.Methods;
    using AllinOne.ObjectManager.Heroes;
    using AllinOne.Update;
    using AllinOne.Variables;
    using Ensage;
    using Ensage.Common;
    using SharpDX;
    using System;
    using System.Linq;

    internal class AllinOneMain
    {
        public static void Init()
        {
            Events.OnLoad += AllinOneEvents.OnLoad;
            Events.OnClose += AllinOneEvents.OnClose;
        }

        public static void Game_OnWndProc(WndEventArgs args)
        {
            if (!OnUpdate.CanUpdate()) return;

            if (args.Msg == (ulong) Utils.WindowsMessages.WM_LBUTTONDOWN && MenuVar.StackKey)
            {
                foreach (var camp in from camp in Var.Camps
                                     let position = Drawing.WorldToScreen(camp.Position)
                                     where Utils.IsUnderRectangle(Game.MouseScreenPosition, position.X, position.Y, 40, 40)
                                     select camp)
                {
                    camp.Stacked = camp.Stacked == false ? true : false;
                    camp.Unit = null;
                }
            }
        }

        public static void Game_OnUpdate(EventArgs args)
        {
            if (!OnUpdate.CanUpdate()) return;

            Var.Seconds = (int) Game.GameTime % 60;

            //if (Common.SleepCheck("sleepprint"))
            //{
            //    Console.WriteLine("--------------------");
            //    foreach (var d in Var.SleepDic)
            //    {
            //        Console.WriteLine("Text:[{0}]  Period:[{1}]", d.Text, d.Period / 10000);
            //    }
            //    Common.Sleep(900, "sleepprint");
            //}
            if (MenuVar.VisiblebyEnemy)
            {
                foreach (var hero in AllyHeroes.Heroes)
                {
                    Methods.ShowMeMore.ShowVisible(hero);
                }
            }
            else
            {
                Methods.ShowMeMore.ClearEffectsVisible();
            }

            foreach (var hero in EnemyHeroes.Heroes)
            {
                Methods.ShowMeMore.DrawShowMeMoreSpells(hero);
            }

            if (MenuVar.ShowRunesChat && (int) Game.GameTime / 60 % 2 == 0)
            {
                if (ObjectManager.Runes.TopRune != null && Common.SleepCheck("TopRunes"))
                {
                    ObjectManager.Runes.ChatTop();
                    Common.Sleep(110000, "TopRunes");
                }

                if (ObjectManager.Runes.BotRune != null && Common.SleepCheck("BotRunes"))
                {
                    ObjectManager.Runes.ChatBot();
                    Common.Sleep(110000, "BotRunes");
                }
            }

            if (MenuVar.StackKey && Var.StackableSummons.Count > 0)
            {
                Jungle.GetClosestCamp(Var.StackableSummons);
                Jungle.Stack();
            }

            if (MenuVar.LastHitEnable)
            {
                if (MenuVar.Test)
                    Lasthit.Attack_Calc();

                if ((Game.IsKeyDown(MenuVar.LastHitKey) || MenuVar.SummonsAutoLasthit) &&
                    MenuVar.SummonsEnable)
                {
                    Lasthit.SummonLastHit();
                }
                else if ((Game.IsKeyDown(MenuVar.FarmKey) || MenuVar.SummonsAutoFarm) &&
                    MenuVar.SummonsEnable)
                {
                    Lasthit.SummonFarm();
                }
                else
                {
                    if (!Var.SummonsAutoAttackTypeDef)
                    {
                        Common.AutoattackSummons(-1);
                        Var.SummonsDisableAaKeyPressed = false;
                        Var.SummonsAutoAttackTypeDef = true;
                    }
                    Var.CreeptargetS = null;
                }

                if (Game.IsKeyDown(MenuVar.LastHitKey))
                {
                    Lasthit.LastHit();
                }
                else if (Game.IsKeyDown(MenuVar.FarmKey))
                {
                    Lasthit.Farm();
                }
                else if (Game.IsKeyDown(MenuVar.CombatKey))
                {
                    Lasthit.Combat();
                }
                else if (Game.IsKeyDown(MenuVar.KiteKey))
                {
                    Lasthit.Kite();
                }
                else
                {
                    if (!Var.AutoAttackTypeDef)
                    {
                        Var.Me.Hold();
                        Common.Autoattack(MenuVar.AutoAttackMode);
                        Var.DisableAaKeyPressed = false;
                        Var.AutoAttackTypeDef = true;
                    }
                    Var.CreeptargetH = null;
                }
            }
        }
    }
}