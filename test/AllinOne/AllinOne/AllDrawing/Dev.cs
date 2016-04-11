﻿namespace AllinOne.AllDrawing
{
    using AllinOne.Variables;
    using Ensage;
    using Ensage.Common.Extensions;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Dev
    {
        private static readonly Dictionary<Unit, ParticleEffect> EffectsLine = new Dictionary<Unit, ParticleEffect>();

        public static void DevInfo()
        {
            try
            {
                var vLine = new Vector2[2];
                if (Var.CreeptargetH != null)
                {
                    ParticleEffect rr;
                    if (EffectsLine.ContainsKey(Var.Me))
                    {
                        EffectsLine[Var.Me].SetControlPoint(1, Var.Me.Position);
                        EffectsLine[Var.Me].SetControlPoint(2, Var.CreeptargetH.Position);
                    }
                    else
                    {
                        rr = new ParticleEffect(Particles.Partlist[81], Var.Me.Position);
                        rr.SetControlPoint(1, Var.Me.Position);
                        rr.SetControlPoint(2, Var.CreeptargetH.Position);
                        EffectsLine.Add(Var.Me, rr);
                    }
                }
                else
                {
                    if (EffectsLine.ContainsKey(Var.Me))
                    {
                        EffectsLine[Var.Me].Dispose();
                        EffectsLine.Remove(Var.Me);
                    }
                }
                if (Var.Summons.Count != 0)
                {
                    foreach (var unit in Var.Summons)
                    {
                        var firstOrDefault = unit.Value.FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            ParticleEffect rr;
                            if (EffectsLine.ContainsKey(unit.Key))
                            {
                                EffectsLine[unit.Key].SetControlPoint(1, unit.Key.Position);
                                EffectsLine[unit.Key].SetControlPoint(2, firstOrDefault.Position);
                            }
                            else
                            {
                                rr = new ParticleEffect(Particles.Partlist[83], unit.Key.Position);
                                rr.SetControlPoint(1, unit.Key.Position);
                                rr.SetControlPoint(2, firstOrDefault.Position);
                                EffectsLine.Add(unit.Key, rr);
                            }
                        }
                        else
                        {
                            if (EffectsLine.ContainsKey(unit.Key))
                            {
                                EffectsLine[unit.Key].Dispose();
                                EffectsLine.Remove(unit.Key);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("ShowInfo Error");
            }
        }

        public static void DevInfoDispose()
        {
            EffectsLine.ForEach(x => x.Value.Dispose());
            EffectsLine.Clear();
        }
    }
}