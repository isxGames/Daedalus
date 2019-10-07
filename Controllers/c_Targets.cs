﻿using System.Collections.Generic;
using EVE.ISXEVE;
using Daedalus.Data;
using Daedalus.Functions;
using System.Linq;

namespace Daedalus.Controllers
{
    static class c_Targets
    {
        #region Variables
        private static List<Entity> enemyNpcEntities = new List<Entity>();
        private static List<EnemyNPC> enemyNpcEntitiesInRange = new List<EnemyNPC>();
        public static List<EnemyNPC> optimalTargets = new List<EnemyNPC>();
        private static double maxTargetRange;
        private static double maxMeTargetsLocked;
        private static double maxShipTargetsLocked;
        public static bool redAlert = false;
        #endregion

        public static void Pulse()
        {
            enemyNpcEntities = f_Entities.GetNpcEntities(f_Entities.AllEntities());
            enemyNpcEntitiesInRange = GetEnemiesInRange();
            if (enemyNpcEntities.Count > 0)
            {
                redAlert = true;
                if (c_Routines.activeRoutine == Routine.Combat_Active) DoTargeting();
            }
            else
            {
                redAlert = false;
            }
        }
        public static void DoTargeting()
        {
            maxTargetRange = Daedalus.myShip.MaxTargetRange;
            maxMeTargetsLocked = Daedalus.me.MaxLockedTargets;
            maxShipTargetsLocked = Daedalus.myShip.MaxLockedTargets;

            // Step 1: If targets within targeting range
            if (enemyNpcEntitiesInRange.Count > 0)
            {
                if (UI.selectedTargetingProfile == TargetingProfile.byClass)
                {
                    optimalTargets = enemyNpcEntitiesInRange
                            .Where(npc => !npc.entity.IsMoribund)
                            .OrderBy(npc => npc.shipClass)
                            .ThenBy(npc => npc.distance)
                            .ThenBy(npc => npc.isPriority)
                            .Take((int)System.Math.Min(maxMeTargetsLocked, maxShipTargetsLocked))
                            .ToList();
                    Daedalus.DaedalusUI.setTargetsList(optimalTargets);
                }
                else if (UI.selectedTargetingProfile == TargetingProfile.byDistance)
                {
                    optimalTargets = enemyNpcEntitiesInRange
                            .Where(npc => !npc.entity.IsMoribund)
                            .OrderBy(npc => npc.distance)
                            .ThenBy(npc => npc.isPriority)
                            .Take((int)System.Math.Min(maxMeTargetsLocked, maxShipTargetsLocked))
                            .ToList();
                    Daedalus.DaedalusUI.setTargetsList(optimalTargets);
                }
            }
            // Step 2: If no targets within targeting range then if targets outside of it, approach the nearest one
            else if (enemyNpcEntitiesInRange.Count == 0 && enemyNpcEntities.Count > 0)
            {
                if (f_Entities.GetEntityMode(Daedalus.me.ToEntity) != EntityMode.Approaching) f_Movement.Approach(enemyNpcEntities[0]);
            }

            // Step 3: Unlock any locked targets that are not optimal targets
            List<Entity> lockedTargets = Daedalus.me.GetTargets();
            foreach (Entity target in lockedTargets)
            {
                bool found = false;
                foreach (EnemyNPC enemyNPC in optimalTargets)
                {
                    if (enemyNPC.entity.ID == target.ID) found = true;
                }
                if (!found)
                {
                    Daedalus.DaedalusUI.newConsoleMessage(target.Name + " is no longer optimal, unlocking now");
                    target.UnlockTarget();
                }
            }

            // Step 4: Lock optimal targets
            foreach (EnemyNPC optimalTarget in optimalTargets)
            {
                if (!optimalTarget.entity.IsLockedTarget && !optimalTarget.entity.BeingTargeted)
                {
                    Daedalus.DaedalusUI.newConsoleMessage(optimalTarget.entity.Name + " is not locked or locking, locking now");
                    optimalTarget.entity.LockTarget();
                }
            }
        }
        private static List<EnemyNPC> GetEnemiesInRange()
        {
            List<EnemyNPC> toReturn = new List<EnemyNPC>();
            List<Entity> entities = f_Entities.GetEntitiesWithinDistance(enemyNpcEntities, maxTargetRange);

            foreach (Entity entity in entities)
            {
                toReturn.Add(new EnemyNPC(
                    f_Entities.GetDistanceBetween(entity),
                    entity,
                    d_NPC_Classes.GetNpcClass(entity.GroupID),
                    d_Priority_Targets.IsPriority(entity.Name)
                    ));
            }
            return toReturn;
        }
    }

    public class EnemyNPC
    {
        public double distance { get; set; }
        public Entity entity { get; set; }
        public int shipClass { get; set; }
        public bool isPriority { get; set; }
        public EnemyNPC(double Distance, Entity anEntity, int ShipClass, bool IsPriority)
        {
            distance = Distance;
            entity = anEntity;
            shipClass = ShipClass;
            isPriority = IsPriority;
        }
    }
}
