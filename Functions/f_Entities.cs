﻿using Daedalus.Data;
using EVE.ISXEVE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus.Functions
{
    public static class f_Entities
    {
        public static string GetEntityMode(Entity e)
        {
            int Mode = e.Mode;
            if (Mode == 0) /* Aligned */
            {
                return "Aligned";
            }
            else if (Mode == 1) /* Approaching */
            {
                return "Approaching";
            }
            else if(Mode == 2) /* Stopped */
            {
                return "Stopped";
            }
            else if(Mode == 3) /* Warping (In Warp) */
            {
                return "Warping";
            }
            else if(Mode == 4) /* Orbiting */
            {
                return "Orbiting";
            }
            else
            {
                return "NULL";
            }
        }
        public static List<Entity> GetStations()
        {
            return Daedalus.eve.QueryEntities("GroupID = 15");
        }
        public static List<Entity> GetAsteroidBelts()
        {
            return Daedalus.eve.QueryEntities("GroupID = 9");
        }
        public static Entity GetEntityByID(long id)
        {
            Entity idToReturn = null;
            List<Entity> entities = Daedalus.eve.QueryEntities("ID = " + id.ToString());
            if (entities.Count > 0) idToReturn = entities[0];
            return idToReturn;
        }
        public static List<Entity> GetAsteroids()
        {
            List<Entity> Asteroids = new List<Entity>();
            List<Entity> Entities = Daedalus.eve.QueryEntities("CategoryID = 25");
            foreach (Entity e in Entities)
            {
                Asteroids.Add(e);
            }
            return Asteroids;
        }
        public static double DistanceFromPlayerToEntity(Entity e)
        {
            if (!e.IsValid) return 0;
            return Daedalus.eve.DistanceBetween(Daedalus.me.ToEntity.ID, e.ID);
        }
        public static double DistanceBetween(Entity e1, Entity e2)
        {
            return Daedalus.eve.DistanceBetween(e1.ID, e2.ID);
        }
        public static List<Entity> GetNpcEntities()
        {
            List<Entity> toReturn = new List<Entity>();
            List<Entity> AllEntities = Daedalus.eve.QueryEntities();
            // Get all enemy npcs from NPC_Types.xml list
            foreach (Entity entity in AllEntities)
            {
                if (d_NPC_Types.All.Contains(entity.GroupID)) toReturn.Add(entity);
            }
            return toReturn;
        }
        public static Dictionary<Entity, double> GetNpcEntitiesSortedByHitchance()
        {
            Dictionary<Entity, double> toReturn = new Dictionary<Entity, double>();
            List<Entity> npcEntities = new List<Entity>();
            // Get all enemy npcs from NPC_Types.xml list
            foreach (long groupID in d_NPC_Types.All)
            {
                List<Entity> entities = Daedalus.eve.QueryEntities("GroupID = " + groupID);
                if (entities.Count > 0) npcEntities.AddRange(entities);
            }

            foreach (Entity npc in npcEntities)
            {

            }

            return toReturn;
        }
    }
}
