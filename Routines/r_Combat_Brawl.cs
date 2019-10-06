﻿using Daedalus.Functions;
using Daedalus.Controllers;
using EVE.ISXEVE;
using EVE.ISXEVE.Interfaces;
using LavishVMAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus.Routines
{
    static class r_Combat_Brawl
    {
        private static bool initComplete = false;
        public static void Pulse()
        {
            if (!initComplete)
            {
                initComplete = true;
            }

            if (!c_Targets.redAlert) c_Routines.activeRoutine = Routine.Combat_Idle;

            c_Status.Pulse();
            c_Targets.Pulse();

            if(c_Modules.afterburners.Count > 0) c_Modules.PropulsionPulse();
            if (c_Modules.armorHardeners.Count > 0 || c_Modules.armorRepairers.Count > 0)   c_Modules.ArmorPulse();
            if (c_Modules.shieldBoosters.Count > 0 || c_Modules.shieldHardeners.Count > 0)  c_Modules.ShieldPulse();

            DoCombat();
        }

        private static long orbitTargetID;
        public static void DoCombat()
        {
            if (c_Targets.targetsLockedIDs.Count > 0)
            {
                Entity target = f_Entities.GetEntityByID(c_Targets.targetsLockedIDs[0]);
                if(target.IsValid)
                {
                    if (target.IsActiveTarget)
                    {
                        if (orbitTargetID != target.ID)
                        {
                            orbitTargetID = target.ID;
                            target.Orbit(Convert.ToInt32(Daedalus.DaedalusUI.orbitRange()));
                        }
                        c_Modules.WeaponPulse(target);
                    }
                    else if (!target.IsActiveTarget) target.MakeActiveTarget();
                }
            }
        }
    }
}
