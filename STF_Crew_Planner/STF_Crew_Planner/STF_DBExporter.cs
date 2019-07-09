using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlNado;
using System.IO;
using SqlNado.Utilities;

namespace STF_CharacterPlanner
{
    class STF_DBExporter
    {
        public static void Exporter()
        {
            string folderName = Directory.GetCurrentDirectory();
            string pathString = Path.Combine(folderName, "STFPlannerFiles");

            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            
            string path = "tools.db";

            using (var db = new SQLiteDatabase(path))
            {
                MakeFile_ShipData(db);
                MakeFile_TalentPoints(db);
                MakeFile_JobList(db);
                MakeFile_StfEngineData(db);
                MakeFile_ShipComponents(db);
                MakeFile_SkillPerJobList(db);
                MakeFile_TalentList(db);
                MakeFile_ShipWeaponData(db);
                MakeFile_SmallCraftList(db);
                MakeFile_ShipDefaultList(db);
            }
            MakeFile_SkillList();
        }

        public static string removeColon(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                str = str.Replace(":", "");
            }
            return str;
        }

        public static string AddQuotes(string str)
        {
            return string.Format("\"{0}\"", str);
        }

        private static void MakeFile_ShipDefaultList(SQLiteDatabase db)
        {
            // header
            StreamWriter shipDefault = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\STF_Ship_Default_Comp.csv");
            shipDefault.WriteLine("Ship:Component");

            foreach (var sDefault in db.Load<ShipDefault>("SELECT ShipType.shipTypeName, ShipComponent.componentName FROM ShipDataCompartment INNER JOIN ShipType ON ShipType._id = ShipDataCompartment.shipId INNER JOIN ShipComponent ON ShipComponent._id = ShipDataCompartment.defaultComponent; "))
            {
                shipDefault.WriteLine("{0}:{1}", sDefault.shipTypeName, removeColon(sDefault.componentName));
            }
            shipDefault.Close();
        }

        private static void MakeFile_SmallCraftList(SQLiteDatabase db)
        {
            //output the header
            StreamWriter smallCraft = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\smallCraft.csv");
            smallCraft.WriteLine("Name:Description:Cost:Hull:Armor:Shields:Launch Fuel Cost:Pilot:Electronics:Gunnery:AP:Agile:Speed:Repair Cost:Chance to hit Ship:Chance to hit Craft:Dodge");

            foreach (var craft in db.Load<SmallCraft>("SELECT * FROM SmallCraft;"))
            {
                smallCraft.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}",
                    craft.craftName, craft.description, craft.cost, craft.maxHull, craft.craftArmor, craft.craftDeflection,
                    craft.launchFuelCost, craft.skPilot, craft.skElectronics, craft.skGunnery, craft.baseActionPoints,
                    craft.craftAgile, craft.craftSpeed, craft.repairCost, craft.baseToHitShip, craft.baseToHitCraft,
                    craft.baseToDodgeHit);
            }
            smallCraft.Close();
        }

        private static void MakeFile_ShipData(SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipData = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\STF_Ship_Data.csv");
            shipData.WriteLine("Ship:Current Mass:Max Mass:Price:Total Slots:Large:Mid:Small:Hull:Armour:Shield:Max Officers:Max Crew:Cargo:Engine:Speed:Agility:Fuel Cost:Fuel Tank:Fuel Range:Jump Cost:Pilot:Navigation:Ship Ops:Electronics:Gunnery:Tier");

            for (int i = 1; i < 100; i++)
            {
                foreach (var shipType in db.Load<ShipType>(@"SELECT shipTypeName, sum(mass) AS currentMass, baseMass, shipCost, (smallSlots + mediumSlots + largeSlots) AS totalSlots, 
                    largeSlots, mediumSlots, smallSlots, hullPoints, (baseArmor + armorBonus) AS Armor, (baseDeflection + deflectionBonus) AS Deflection, maxOfficer, maxLifeSupport, 
                    sum(holdsCargo) as holdsCargo, sum(shipEngineId) as shipEngine , sum(shipSpeed) as shipSpeed, sum(shipAgile) as Agility, sum(mapFuelCost) as mapFuelCost, 
                    (sum(fuelBonus)+ baseFuel) AS fuelTank, (sum(fuelBonus)+ baseFuel)/sum(mapFuelCost) AS fuelRange, sum(jumpCost) AS jumpCost, sum(skPilot) AS pilot, sum(skNavigation) AS navigation, 
                    sum(skShipOps) AS shipOps, sum(skElectronics) AS electronics, sum(skGunnery) AS gunnery FROM ShipDataCompartment INNER JOIN ShipType ON ShipType._id = ShipDataCompartment.shipId 
                    INNER JOIN ShipComponent ON ShipComponent._id = ShipDataCompartment.defaultComponent LEFT JOIN ShipEngine ON ShipEngine._id = ShipComponent.shipEngineId WHERE ShipType._id = " + i + ";"))
                {
                    foreach (var subShip in db.Load<ShipType>(@"SELECT * FROM ShipDataCompartment INNER JOIN ShipComponent ON ShipComponent._id = ShipDataCompartment.defaultComponent INNER JOIN ShipType ON ShipType._id = ShipDataCompartment.shipId WHERE ShipComponent.componentType = 3 AND ShipType._id = " + i + ";"))
                    {
                        if (shipType.shipTypeName != null)
                        {
                            shipData.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}:{18}:{19}:{20}:{21}:{22}:{23}:{24}:{25}:{26}",
                            shipType.shipTypeName, shipType.currentMass, shipType.baseMass, shipType.shipCost, shipType.totalSlots, shipType.largeSlots, shipType.mediumSlots,
                            shipType.smallSlots, shipType.hullPoints, shipType.Armor, shipType.Deflection, shipType.maxOfficer, shipType.maxLifeSupport, shipType.holdsCargo,
                            removeColon(subShip.componentName), shipType.shipSpeed, shipType.Agility, shipType.mapFuelCost, shipType.fuelTank, shipType.fuelRange, shipType.jumpCost,
                            shipType.pilot, shipType.navigation, shipType.shipOps, shipType.electronics, shipType.gunnery, shipType.tier);
                        }
                    }
                }
            }
            shipData.Close();
        }

        private static void MakeFile_TalentPoints(SQLiteDatabase db)
        {
            //output the header
            StreamWriter talentPoints = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\talent_points.csv");
            talentPoints.WriteLine("Rank:Talents");

            foreach (var tPoint in db.Load<CharLevel>("SELECT * FROM CharacterLevel;"))
            {
                talentPoints.WriteLine("{0}:{1}", tPoint.level, addTalentPoint(tPoint.level, tPoint.levelType));
            }

            talentPoints.Close();
        }

        public static string actionCondensed(string str)
        {
            switch (str)
            {
                case "GAME_ACTION_TEST_SHIP_OPS":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_PILOT":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_ELECTRONICS":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_NAVIGATION":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_TACTICS":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_REPAIR":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_COMMAND":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_DOCTOR":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_INTIMIDATE":
                    return "SKILL SAVE";
                case "AME_ACTION_MISSION_CREATE":
                    return "MISSION";
                case "GAME_ACTION_MISSION_SEGMENT":
                    return "MISSION";
                case "GAME_ACTION_MISSION_SEGMENT_EDICT":
                    return "MISSION";
                case "GAME_ACTION_MISSION_COMPLETE":
                    return "MISSION";
                case "GAME_ACTION_CREW_RECRUIT":
                    return "RECRUIT";
                case "GAME_ACTION_LOW_MORALE":
                    return "";
                case "GAME_ACTION_PLANET_ARRIVE":
                    return "AT PORT";
                case "GAME_ACTION_PLANET_DEPART":
                    return "";
                case "GAME_ACTION_PLANET_NEW":
                    return "";
                case "GAME_ACTION_SPICE":
                    return "SPICE";
                case "GAME_ACTION_XENO_ENCOUNTER":
                    return "XENO";
                case "GAME_ACTION_RADIATION_STORM":
                    return "STORM";
                case "GAME_ACTION_CREW_PAY":
                    return "PAY";
                case "GAME_ACTION_COMBAT_CREW_KILL":
                    return "";
                case "GAME_ACTION_COMBAT_SHIP_KILL":
                    return "";
                case "GAME_ACTION_TEST_NEGOTIATE":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_STEALTH":
                    return "SKILL SAVE";
                case "GAME_ACTION_TEST_EXPLORER":
                    return "SKILL SAVE";
                case "GAME_ACTION_COMBAT_CREW_START":
                    return "";
                case "GAME_ACTION_COMBAT_SHIP_START":
                    return "";
                case "GAME_ACTION_COMBAT_CREW_INIT":
                    return "ON INIT";
                case "GAME_ACTION_COMBAT_SHIP_INIT":
                    return "";
                case "GAME_ACTION_CONTACT_BUY_RANK":
                    return "RANK";
                case "GAME_ACTION_CONTACT_BUY_PERMIT":
                    return "PERMIT";
                case "GAME_ACTION_CONTACT_BUY_EDICT":
                    return "EDICT";
                case "GAME_ACTION_CONTACT_INTERACT":
                    return "";
                case "GAME_ACTION_HYPERWARP":
                    return "JUMP";
                case "GAME_ACTION_TRADE_BIG":
                    return "TRADE";
                case "GAME_ACTION_TRADE_BIG_100K":
                    return "TRADE";
                case "GAME_ACTION_TRADE_INTO_CONFLICT":
                    return "TRADE";
                case "GAME_ACTION_CREW_COMBAT_VICTORY_BOARDING_ACTIVE":
                    return "BOARDING";
                case "GAME_ACTION_CONTACT_INTRODUCE":
                    return "INTRODUCTION";
                case "GAME_ACTION_TRADE_BIG_ARTIFACT":
                    return "ARTIFACTS";
                case "GAME_ACTION_TRADE_BIG_BLACK":
                    return "SMUGGLE";
                case "GAME_ACTION_TRADE_BIG_BLACK_100K":
                    return "SMUGGLE";
                case "GAME_ACTION_ORBIT":
                    return "ORBIT";
                case "GAME_ACTION_CREW_XENO_ENCOUNTER":
                    return "XENO";
                case "GAME_ACTION_CREW_ABANDON":
                    return "DESERTER";
                case "GAME_ACTION_CREW_MUTINY":
                    return "MUTINY";
                case "GAME_ACTION_STARPORT_REPAIR":
                    return "REPAIR";
                case "GAME_ACTION_STARPORT_UPGRADE":
                    return "UPGRADE";
                case "GAME_ACTION_STARPORT_NEWSHIP":
                    return "NEW SHIP";
                case "GAME_ACTION_ORBIT_METEOR":
                    return "METEOR";
                case "GAME_ACTION_PLANET_ARRIVE_WILD":
                    return "WILD";
                case "GAME_ACTION_TRAIT_MUTATE":
                    return "";
                case "GAME_ACTION_CREW_DEATH_HEALTH":
                    return "ON DEATH";
                case "GAME_ACTION_CREW_DEATH_MORALE":
                    return "";
                case "GAME_ACTION_EXPLORER_ACTIVE":
                    return "EXPLORE";
                case "GAME_ACTION_EXPLORER_PASSIVE":
                    return "EXPLORE";
                case "GAME_ACTION_SPY_ACTIVE":
                    return "SPY";
                case "GAME_ACTION_SPY_PASSIVE":
                    return "SPY";
                case "GAME_ACTION_PATROL_ACTIVE":
                    return "PATROL";
                case "GAME_ACTION_PATROL_PASSIVE":
                    return "PATROL";
                case "GAME_ACTION_BLOCKADE_ACTIVE":
                    return "BLOCKADE";
                case "GAME_ACTION_BLOCKADE_PASSIVE":
                    return "BLOCKADE";
                case "GAME_ACTION_NON_IMPL":
                    return "";
                case "GAME_ACTION_SHIP_ESCAPE":
                    return "ESCAPE";
                case "GAME_ACTION_MORALE_CHANGE":
                    return "";
                case "GAME_ACTION_XP_CHANGE":
                    return "";
                case "GAME_ACTION_PLANET_LANDING":
                    return "LANDING";
                case "GAME_ACTION_SHIP_COMBAT_DRAW":
                    return "ENCOUNTER";
                case "GAME_ACTION_SHIP_COMBAT_DEFEAT_SEARCHED":
                    return "SEARCHED";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_CAPTURE_ACTIVE":
                    return "VICTORY";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_DESTROY_ACTIVE":
                    return "DESTROY";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_CAPTURE_SMUGGLER_ACTIVE":
                    return "VICTORY";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_CAPTURE_MILITARY_ACTIVE":
                    return "VICTORY";
                case "GAME_ACTION_SHIP_COMBAT_PREAMBLE_ACTIVE":
                    return "ENCOUNTER";
                case "GAME_ACTION_SHIP_COMBAT_PREAMBLE_MILITARY_ACTIVE":
                    return "ENCOUNTER";
                case "GAME_ACTION_SHIP_COMBAT_PREAMBLE_PIRATE_ACTIVE":
                    return "ENCOUNTER";
                case "GAME_ACTION_SHIP_COMBAT_PREAMBLE_HUNTER_ACTIVE":
                    return "ENCOUNTER";
                case "GAME_ACTION_SHIP_COMBAT_PREAMBLE_MERCHANT_ACTIVE":
                    return "ENCOUNTER";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_SALVAGE":
                    return "SALVAGE";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_DESTROY":
                    return "DESTROY";
                case "GAME_ACTION_CREW_COMBAT_VICTORY_ACTIVE":
                    return "VICTORY";
                case "GAME_ACTION_MARKET_OPS_ACTIVE":
                    return "SMUGGLE";
                case "GAME_ACTION_MARKET_OPS_PASSIVE":
                    return "SMUGGLE";
                case "GAME_ACTION_CREW_DOCTOR":
                    return "DOCTOR";
                case "GAME_ACTION_CONTACT_SELL_DATA":
                    return "INTEL";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_GLOBAL":
                    return "VICTORY";
                case "GAME_ACTION_TRADE_UNIQUE":
                    return "RTG";
                case "GAME_ACTION_SALVAGE_ACTIVE":
                    return "SALVAGE";
                case "GAME_ACTION_SALVAGE_PASSIVE":
                    return "SALVAGE";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_CONFLICT_ACTIVE":
                    return "VICTORY";
                case "GAME_ACTION_SHIP_COMBAT_PREAMBLE_SMUGGLER_ACTIVE":
                    return "ENCOUNTER";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_XENO":
                    return "VICTORY";
                case "GAME_ACTION_SHIP_COMBAT_VICTORY_SALVAGE_XENO":
                    return "VICTORY";
                case "GAME_ACTION_SHIP_COMBAT_CRAFT_LANDED":
                    return "CRAFT LANDS";
                case "GAME_ACTION_SHIP_COMBAT_CRAFT_DESTROYED":
                    return "CRAFT DEATH";
                case "GAME_ACTION_STARPORT_CRAFT_PURCHASE":
                    return "NEW CRAFT";
                case "GAME_ACTION_SHIP_COMBAT_CRAFT_MAINT_POINT":
                    return "MAINT POINT";
                case "GAME_ACTION_SHIP_COMBAT_CRAFT_PLAN":
                    return "";
                case "GAME_ACTION_COMBAT_CREW_ACTIVE":
                    return "";
                case "GAME_ACTION_COMBAT_SHIP_ACTIVE":
                    return "";
                case "GAME_ACTION_CONFLICT_SCORE":
                    return "CONFLICT";
                default:
                    return "";
            }
        }

        public static string actionTypeToString(string aType)
        {
            switch (aType)
            {
                case "1":
                    return "GAME_ACTION_TEST_SHIP_OPS";
                case "2":
                    return "GAME_ACTION_TEST_PILOT";
                case "3":
                    return "GAME_ACTION_TEST_ELECTRONICS";
                case "4":
                    return "GAME_ACTION_TEST_NAVIGATION";
                case "5":
                    return "GAME_ACTION_TEST_TACTICS";
                case "6":
                    return "GAME_ACTION_TEST_REPAIR";
                case "7":
                    return "GAME_ACTION_TEST_COMMAND";
                case "8":
                    return "GAME_ACTION_TEST_DOCTOR";
                case "9":
                    return "GAME_ACTION_TEST_INTIMIDATE";
                case "10":
                    return "GAME_ACTION_MISSION_CREATE";
                case "11":
                    return "GAME_ACTION_MISSION_SEGMENT";
                case "12":
                    return "GAME_ACTION_MISSION_COMPLETE";
                case "13":
                    return "GAME_ACTION_CREW_RECRUIT";
                case "14":
                    return "GAME_ACTION_LOW_MORALE";
                case "15":
                    return "GAME_ACTION_PLANET_ARRIVE";
                case "16":
                    return "GAME_ACTION_PLANET_DEPART";
                case "17":
                    return "GAME_ACTION_PLANET_NEW";
                case "18":
                    return "GAME_ACTION_SPICE";
                case "19":
                    return "GAME_ACTION_XENO_ENCOUNTER";
                case "20":
                    return "GAME_ACTION_RADIATION_STORM";
                case "21":
                    return "GAME_ACTION_CREW_PAY";
                case "22":
                    return "GAME_ACTION_COMBAT_CREW_KILL";
                case "23":
                    return "GAME_ACTION_COMBAT_SHIP_KILL";
                case "24":
                    return "GAME_ACTION_TEST_NEGOTIATE";
                case "25":
                    return "GAME_ACTION_TEST_STEALTH";
                case "26":
                    return "GAME_ACTION_TEST_EXPLORER";
                case "27":
                    return "GAME_ACTION_COMBAT_CREW_START";
                case "28":
                    return "GAME_ACTION_COMBAT_SHIP_START";
                case "29":
                    return "GAME_ACTION_COMBAT_CREW_INIT";
                case "30":
                    return "GAME_ACTION_COMBAT_SHIP_INIT";
                case "31":
                    return "GAME_ACTION_CONTACT_BUY_RANK";
                case "32":
                    return "GAME_ACTION_CONTACT_BUY_PERMIT";
                case "33":
                    return "GAME_ACTION_CONTACT_BUY_EDICT";
                case "34":
                    return "GAME_ACTION_CONTACT_INTERACT";
                case "35":
                    return "GAME_ACTION_HYPERWARP";
                case "36":
                    return "GAME_ACTION_TRADE_BIG";
                case "37":
                    return "GAME_ACTION_TRADE_BIG_ARTIFACT";
                case "38":
                    return "GAME_ACTION_TRADE_BIG_BLACK";
                case "39":
                    return "GAME_ACTION_ORBIT";
                case "40":
                    return "GAME_ACTION_CREW_XENO_ENCOUNTER";
                case "41":
                    return "GAME_ACTION_CREW_ABANDON";
                case "42":
                    return "GAME_ACTION_CREW_MUTINY";
                case "43":
                    return "GAME_ACTION_STARPORT_REPAIR";
                case "44":
                    return "GAME_ACTION_STARPORT_UPGRADE";
                case "45":
                    return "GAME_ACTION_STARPORT_NEWSHIP";
                case "46":
                    return "GAME_ACTION_ORBIT_METEOR";
                case "47":
                    return "GAME_ACTION_PLANET_ARRIVE_WILD";
                case "48":
                    return "GAME_ACTION_TRAIT_MUTATE";
                case "49":
                    return "GAME_ACTION_CREW_DEATH_HEALTH";
                case "50":
                    return "GAME_ACTION_CREW_DEATH_MORALE";
                case "51":
                    return "GAME_ACTION_EXPLORER_ACTIVE";
                case "52":
                    return "GAME_ACTION_EXPLORER_PASSIVE";
                case "53":
                    return "GAME_ACTION_SPY_ACTIVE";
                case "54":
                    return "GAME_ACTION_SPY_PASSIVE";
                case "55":
                    return "GAME_ACTION_PATROL_ACTIVE";
                case "56":
                    return "GAME_ACTION_PATROL_PASSIVE";
                case "57":
                    return "GAME_ACTION_BLOCKADE_ACTIVE";
                case "58":
                    return "GAME_ACTION_BLOCKADE_PASSIVE";
                case "59":
                    return "GAME_ACTION_NON_IMPL";
                case "60":
                    return "GAME_ACTION_SHIP_ESCAPE";
                case "61":
                    return "GAME_ACTION_MORALE_CHANGE";
                case "62":
                    return "GAME_ACTION_XP_CHANGE";
                case "63":
                    return "GAME_ACTION_PLANET_LANDING";
                case "64":
                    return "GAME_ACTION_SHIP_COMBAT_DRAW";
                case "65":
                    return "GAME_ACTION_SHIP_COMBAT_DEFEAT_SEARCHED";
                case "66":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_CAPTURE_ACTIVE";
                case "67":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_DESTROY_ACTIVE";
                case "68":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_CAPTURE_SMUGGLER_ACTIVE";
                case "69":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_CAPTURE_MILITARY_ACTIVE";
                case "70":
                    return "GAME_ACTION_SHIP_COMBAT_PREAMBLE_ACTIVE";
                case "71":
                    return "GAME_ACTION_SHIP_COMBAT_PREAMBLE_MILITARY_ACTIVE";
                case "72":
                    return "GAME_ACTION_SHIP_COMBAT_PREAMBLE_PIRATE_ACTIVE";
                case "73":
                    return "GAME_ACTION_SHIP_COMBAT_PREAMBLE_HUNTER_ACTIVE";
                case "74":
                    return "GAME_ACTION_SHIP_COMBAT_PREAMBLE_MERCHANT_ACTIVE";
                case "75":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_SALVAGE";
                case "76":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_DESTROY";
                case "77":
                    return "GAME_ACTION_CREW_COMBAT_VICTORY_BOARDING_ACTIVE";
                case "78":
                    return "GAME_ACTION_CREW_COMBAT_VICTORY_ACTIVE";
                case "79":
                    return "GAME_ACTION_MARKET_OPS_ACTIVE";
                case "80":
                    return "GAME_ACTION_MARKET_OPS_PASSIVE";
                case "81":
                    return "GAME_ACTION_CREW_DOCTOR";
                case "82":
                    return "GAME_ACTION_CONTACT_SELL_DATA";
                case "83":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_GLOBAL";
                case "84":
                    return "GAME_ACTION_SHIP_COMBAT_PRESS_CREW";
                case "85":
                    return "GAME_ACTION_SHIP_COMBAT_PREAMBLE_TRIBUTE";
                case "86":
                    return "GAME_ACTION_SHIP_COMBAT_END";
                case "87":
                    return "GAME_ACTION_SHIP_COMBAT_RANSOM";
                case "88":
                    return "GAME_ACTION_CONFLICT_SCORE";
                case "89":
                    return "GAME_ACTION_SHIP_COMBAT_DEFEAT";
                case "90":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY";
                case "91":
                    return "GAME_ACTION_CONTACT_INTRODUCE";
                case "92":
                    return "GAME_ACTION_TRADE_UNIQUE";
                case "93":
                    return "GAME_ACTION_MISSION_SEGMENT_EDICT";
                case "94":
                    return "GAME_ACTION_TRADE_BIG_BLACK_100K";
                case "95":
                    return "GAME_ACTION_TRADE_BIG_100K";
                case "96":
                    return "GAME_ACTION_TRADE_INTO_CONFLICT";
                case "97":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_CONFLICT_ACTIVE";
                case "98":
                    return "GAME_ACTION_TEST_EVASION";
                case "99":
                    return "GAME_ACTION_PLAGUE_TRAIT";
                case "100":
                    return "GAME_ACTION_PLAGUE_RUMOR";
                case "101":
                    return "GAME_ACTION_SALVAGE_ACTIVE";
                case "102":
                    return "GAME_ACTION_SALVAGE_PASSIVE";
                case "103":
                    return "GAME_ACTION_SHIP_COMBAT_PREAMBLE_SMUGGLER_ACTIVE";
                case "104":
                    return "GAME_ACTION_STASH_RAIDED";
                case "105":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_XENO";
                case "106":
                    return "GAME_ACTION_SHIP_COMBAT_VICTORY_SALVAGE_XENO";
                case "107":
                    return "GAME_ACTION_SHIP_COMBAT_CRAFT_LAUNCHED";
                case "108":
                    return "GAME_ACTION_SHIP_COMBAT_CRAFT_LANDED";
                case "109":
                    return "GAME_ACTION_SHIP_COMBAT_CRAFT_DESTROYED";
                case "110":
                    return "GAME_ACTION_STARPORT_CRAFT_REPAIR";
                case "111":
                    return "GAME_ACTION_STARPORT_CRAFT_PURCHASE";
                case "112":
                    return "GAME_ACTION_SHIP_COMBAT_CRAFT_MAINT_POINT";
                case "113":
                    return "GAME_ACTION_SHIP_COMBAT_CRAFT_PLAN";
                case "-1":
                    return "GAME_ACTION_COMBAT_CREW_ACTIVE";
                case "-2":
                    return "GAME_ACTION_COMBAT_SHIP_ACTIVE";
                default:
                    return "";
            }
        }

        public static string weaponTypeToSting(int weapType)
        {
            //Takes the numeric value of weaponType and converts to string name of type
            switch (weapType)
            {
                case 1:
                    return "Autocannon";
                case 2:
                    return "Lance";
                case 3:
                    return "Plasma Cannon";
                case 4:
                    return "Railgun";
                case 5:
                    return "Grav Driver";
                case 6:
                    return "Missiles";
                case 7:
                    return "Torps";
                default:
                    return "";
            }
        }

        public static string componentSizeToString(int compSize)
        {
            //Takes the numeric value of componentSize and converts to string of Small, Medium and Large
            switch (compSize)
            {
                case 1:
                    return "Small";
                case 2:
                    return "Medium";
                case 3:
                    return "Large";
                default:
                    return "";
            }
        }

        public static string jobTypeToString(int jType)
        {
            //Takes jobType and returns the string job name. 
            //Should any new jobs be added they will need to be added to this otherwise the talent file will not associate the new talent with the new job.
            switch (jType)
            {
                case 1:
                    return "Crew Dog";
                case 2:
                    return "Mechanic";
                case 3:
                    return "Electronics Tech";
                case 4:
                    return "Gunner";
                case 5:
                    return "Soldier";
                case 6:
                    return "Pistoleer";
                case 7:
                    return "Sniper";
                case 8:
                    return "Swordsman";
                case 10:
                    return "Assassin";
                case 11:
                    return "Combat Medic";
                case 12:
                    return "Doctor";
                case 13:
                    return "Diplomat";
                case 15:
                    return "Zealot";
                case 16:
                    return "Hyperwarp Navigator";
                case 17:
                    return "Wing Tech";
                case 18:
                    return "Exo-Scout";
                case 19:
                    return "Scavenger";
                case 20:
                    return "Xeno Hunter";
                case 23:
                    return "Commander";
                case 24:
                    return "Pilot";
                case 25:
                    return "Wing Commando";
                case 26:
                    return "Merchant";
                case 27:
                    return "Smuggler";
                case 28:
                    return "Pirate";
                case 29:
                    return "Bounty Hunter";
                case 30:
                    return "Military Officer";
                case 31:
                    return "Explorer";
                case 32:
                    return "Spy";
                case 33:
                    return "Engineer";
                case 34:
                    return "Quartermaster";
                case 35:
                    return "Wing Bomber";
                case 36:
                    return "Wing Leader";
                default:
                    return "";
            }

        }

        public static int addTalentPoint(int level, int levelType)
        {

            if (levelType == 1)
            {
                switch (level)
                {
                    case 1:
                    case 3:
                    case 6:
                    case 8:
                    case 12:
                    case 14:
                    case 17:
                    case 21:
                    case 25:
                    case 28:
                    case 30:
                    case 31:
                    case 32:
                    case 35:
                    case 37:
                    case 40:
                        return 1;
                    default:
                        break;
                }

                return 0;
            }
            else if (levelType == 2)
            {
                switch (level)
                {
                    case 1:
                    case 3:
                    case 6:
                    case 8:
                    case 12:
                    case 14:
                    case 17:
                    case 21:
                    case 25:
                    case 28:
                    case 30:
                    case 32:
                    case 34:
                    case 37:
                    case 40:
                        return 1;
                    default:
                        break;
                }

                return 0;
            }
            else if (levelType == 3)
            {
                switch (level)
                {
                    case 1:
                    case 6:
                    case 11:
                    case 15:
                    case 17:
                    case 21:
                    case 25:
                    case 32:
                        return 1;
                    default:
                        break;
                }

                return 0;
            }
            return 0;
        }

        private static void MakeFile_SkillList()
        {
            StreamWriter skillList = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\skill_list.csv");
            skillList.WriteLine("Skill\nBlades\nEvasion\nStealth\nRifles\nIntimidate\nPistols\nDoctor\nTactics\nCommand\nShip Ops\nGunnery\nRepair\nNegotiate\nElectronics\nExplore\nNavigation\nPilot");
            skillList.Close();
        }

        private static void MakeFile_ShipWeaponData(SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipWeaponList = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\stf_weapon_data.csv");
            shipWeaponList.WriteLine("Name:Type:Damage:Radiation:Void:Range:AP:Accuracy:Critical Chance:Cripple Chance:Level");

            //iterate over filtered collection of rows, output text
            foreach (var shipWeap in db.Load<ShipWeapon>("SELECT * FROM ShipWeapon;"))
            {
                shipWeaponList.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}",
                    shipWeap.weaponName, weaponTypeToSting(shipWeap.weaponType), shipWeap.damage, shipWeap.radDamage,
                    shipWeap.voidDamage, shipWeap.range, shipWeap.ap, shipWeap.accuracy,
                    shipWeap.critChance, shipWeap.effectChance, shipWeap.level);
            }
            shipWeaponList.Close();
        }

        private static void MakeFile_TalentList(SQLiteDatabase db)
        {
            //output the header
            StreamWriter talentList = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\stf_talent_job.csv");
            talentList.WriteLine("Name:Rank:Description:Cooldown:Job:Type");

            //iterate over filtered collection of rows, output text
            foreach (var talent in db.Load<Talent>("SELECT * FROM Talent;"))
            {
                talentList.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}",
                    talent.talentName, talent.jobLevel, talent.talentName2, talent.cooldown,
                    jobTypeToString(talent.jobType), actionCondensed(actionTypeToString(talent.actionType)));
            }
            talentList.Close();
        }

        private static void MakeFile_StfEngineData(SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipEngine = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\stf_engine_data.csv");
            shipEngine.WriteLine("Name:Mass:Speed:Agility:Fuel Cost:Combat Cost:Safety:Range Cost");

            //iterate over filtered collection of rows, output text

            foreach (var engine in db.Load<ShipEngine>("SELECT * FROM ShipEngine INNER JOIN ShipComponent ON ShipComponent.shipEngineId = ShipEngine._id;"))
            {
                shipEngine.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}",
                    removeColon(engine.componentName), engine.designMass, engine.shipSpeed, engine.shipAgile,
                    engine.mapFuelCost, engine.combatFuelCost, engine.safetyRating, engine.moveCost);
            }
            shipEngine.Close();
        }

        private static void MakeFile_ShipComponents(SQLiteDatabase db)
        {
            //output the header
            StreamWriter shipComp = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\ship_components.csv");
            shipComp.WriteLine("Name:Size:Current Mass:Pilot:Ship Ops:Gunnery:Electronics:Navigation:Cargo:Max Crew:Max Officers:Jump Cost:Armour:Fuel Tank:Guest:Prison:Medical:Shield");

            //iterate over filtered collection of rows, output text

            foreach (var shipComponent in db.Load<ShipComponent>("SELECT * FROM ShipComponent ORDER BY componentType;"))
            {
                shipComp.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}",
                    removeColon(shipComponent.componentName), componentSizeToString(shipComponent.componentSize), shipComponent.mass,
                    shipComponent.skPilot, shipComponent.skShipOps, shipComponent.skGunnery, shipComponent.skElectronics, shipComponent.skNavigation,
                    shipComponent.holdsCargo, shipComponent.holdsCrew, shipComponent.holdsOfficer, shipComponent.jumpCost,
                    shipComponent.armorBonus, shipComponent.fuelBonus, shipComponent.holdsGuest, shipComponent.holdsPrisoner, shipComponent.medicalRating, shipComponent.deflectionBonus);
            }
            shipComp.Close();
        }

        private static void MakeFile_SkillPerJobList(SQLiteDatabase db)
        {
            //output the header
            StreamWriter skillPerJobList = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\skill_per_job_list.csv");
            skillPerJobList.WriteLine("Rank:1-Name:1-Num:2-Name:2-Num:3-Name:3-Num:Job");

            //Should the form be restructured this will provide all jobs and their level for all skills

            //output the header
            /*StreamWriter skillPerJobList = new StreamWriter(@"skill_per_job_list.csv");
            skillPerJobList.WriteLine("Job Type:Rank:Pistols:Rifles:Blades:Evasion:Tactics:Stealth:Gunnery:Pilot:Ship Ops:Repair:Electronics:Navigation:Doctor:Command:Negotiate:Intimidate:Explore:Job");
            //iterate over filtered collection of rows, output text
            foreach (var skillPerJob in db.Load<Job>("SELECT jobType, jobLevel, skLightFirearms, skHeavyFirearms, skMelee, skEvasion, skTactics, " +
                "skStealth, skGunnery, skPilot, skShipOps, skRepair, skElectronics, skNavigation, skDoctor, skCommand, skNegotiate, skIntimidate, skExplorer, jobName FROM Job ORDER BY jobType;"))
            {
                skillPerJobList.WriteLine("{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}:{9}:{10}:{11}:{12}:{13}:{14}:{15}:{16}:{17}:{18}:{19}",
                    skillPerJob.jobType, skillPerJob.jobLevel, skillPerJob.skLightFirearms, skillPerJob.skHeavyFirearms, skillPerJob.skMelee, skillPerJob.skEvasion,
                    skillPerJob.skTactics, skillPerJob.skStealth, skillPerJob.skGunnery, skillPerJob.skPilot, skillPerJob.skShipOps, skillPerJob.skRepair, skillPerJob.skElectronics,
                    skillPerJob.skNavigation, skillPerJob.skDoctor, skillPerJob.skCommand, skillPerJob.skNegotiate, skillPerJob.skIntimidate, skillPerJob.skExplorer, skillPerJob.jobName);
            }*/

            //Given the structure of the form any new jobs will need to be entered manually. 
            //To do this the SELECT statement should simply be SELECT * FROM Job WHERE jobType = "the jobType number of the new job"
            //You will then need to specify the 3 skills the job contains. Should the job only have 2 skills the third will be "Null" with a given value of "0".
            //Not how I wanted to handle this but the rewriting of the form would have been a much larger task than deemed necessary at the time.
            //This foreach will iterate over the filtered collection of rows and output text.
            foreach (var crewDog in db.Load<Job>("SELECT * FROM Job WHERE jobType = 1;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Gunnery:{2}:Repair:{3}:{4}",
                    crewDog.jobLevel, crewDog.skShipOps, crewDog.skGunnery, crewDog.skRepair, crewDog.jobName);
            }

            foreach (var mechanic in db.Load<Job>("SELECT * FROM Job WHERE jobType = 2;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Repair:{2}:Electronics:{3}:{4}",
                    mechanic.jobLevel, mechanic.skShipOps, mechanic.skRepair, mechanic.skElectronics, mechanic.jobName);
            }

            foreach (var electronicsTech in db.Load<Job>("SELECT * FROM Job WHERE jobType = 3;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Gunnery:{2}:Electronics:{3}:{4}",
                    electronicsTech.jobLevel, electronicsTech.skShipOps, electronicsTech.skGunnery, electronicsTech.skElectronics, electronicsTech.jobName);
            }

            foreach (var gunner in db.Load<Job>("SELECT * FROM Job WHERE jobType = 4;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Gunnery:{2}:Null:0:{3}",
                    gunner.jobLevel, gunner.skShipOps, gunner.skGunnery, gunner.jobName);
            }

            foreach (var soldier in db.Load<Job>("SELECT * FROM Job WHERE jobType = 5;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Evasion:{2}:Null:0:{3}",
                    soldier.jobLevel, soldier.skHeavyFirearms, soldier.skEvasion, soldier.jobName);
            }

            foreach (var pistoleer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 6;"))
            {
                skillPerJobList.WriteLine("{0}:Pistols:{1}:Evasion:{2}:Null:0:{3}",
                    pistoleer.jobLevel, pistoleer.skLightFirearms, pistoleer.skEvasion, pistoleer.jobName);
            }

            foreach (var sniper in db.Load<Job>("SELECT * FROM Job WHERE jobType = 7;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Evasion:{2}:Stealth:{3}:{4}",
                    sniper.jobLevel, sniper.skHeavyFirearms, sniper.skEvasion, sniper.skStealth, sniper.jobName);
            }

            foreach (var swordsman in db.Load<Job>("SELECT * FROM Job WHERE jobType = 8;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Evasion:{2}:Null:0:{3}",
                    swordsman.jobLevel, swordsman.skMelee, swordsman.skEvasion, swordsman.jobName);
            }

            foreach (var assassin in db.Load<Job>("SELECT * FROM Job WHERE jobType = 10;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Evasion:{2}:Stealth:{3}:{4}",
                    assassin.jobLevel, assassin.skMelee, assassin.skEvasion, assassin.skStealth, assassin.jobName);
            }

            foreach (var combatMedic in db.Load<Job>("SELECT * FROM Job WHERE jobType = 11;"))
            {
                skillPerJobList.WriteLine("{0}:Pistols:{1}:Doctor:{2}:Null:0:{3}",
                    combatMedic.jobLevel, combatMedic.skLightFirearms, combatMedic.skDoctor, combatMedic.jobName);
            }

            foreach (var doctor in db.Load<Job>("SELECT * FROM Job WHERE jobType = 12;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Doctor:{2}:Command:{3}:{4}",
                    doctor.jobLevel, doctor.skTactics, doctor.skDoctor, doctor.skCommand, doctor.jobName);
            }

            foreach (var diplomat in db.Load<Job>("SELECT * FROM Job WHERE jobType = 13;"))
            {
                skillPerJobList.WriteLine("{0}:Negotiate:{1}:Intimidate:{2}:Null:0:{3}",
                    diplomat.jobLevel, diplomat.skNegotiate, diplomat.skIntimidate, diplomat.jobName);
            }

            foreach (var zealot in db.Load<Job>("SELECT * FROM Job WHERE jobType = 15;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Intimidate:{2}:Command:{3}:{4}",
                    zealot.jobLevel, zealot.skMelee, zealot.skIntimidate, zealot.skCommand, zealot.jobName);
            }

            foreach (var hyperwarpNavigator in db.Load<Job>("SELECT * FROM Job WHERE jobType = 16;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Navigator:{2}:Null:0:{3}",
                    hyperwarpNavigator.jobLevel, hyperwarpNavigator.skTactics, hyperwarpNavigator.skNavigation, hyperwarpNavigator.jobName);
            }

            foreach (var wingTech in db.Load<Job>("SELECT * FROM Job WHERE jobType = 17;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Ship Ops:{2}:Repair:{3}:{4}",
                    wingTech.jobLevel, wingTech.skTactics, wingTech.skShipOps, wingTech.skRepair, wingTech.jobName);
            }

            foreach (var exoScout in db.Load<Job>("SELECT * FROM Job WHERE jobType = 18;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Explore:{2}:Null:0:{3}",
                    exoScout.jobLevel, exoScout.skHeavyFirearms, exoScout.skExplorer, exoScout.jobName);
            }

            foreach (var scavenger in db.Load<Job>("SELECT * FROM Job WHERE jobType = 19;"))
            {
                skillPerJobList.WriteLine("{0}:Repair:{1}:Doctor:{2}:Explore:{3}:{4}",
                    scavenger.jobLevel, scavenger.skRepair, scavenger.skDoctor, scavenger.skExplorer, scavenger.jobName);
            }

            foreach (var xenoHunter in db.Load<Job>("SELECT * FROM Job WHERE jobType = 20;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Intimidate:{2}:Explore:{3}:{4}",
                    xenoHunter.jobLevel, xenoHunter.skHeavyFirearms, xenoHunter.skIntimidate, xenoHunter.skExplorer, xenoHunter.jobName);
            }

            foreach (var commander in db.Load<Job>("SELECT * FROM Job WHERE jobType = 23;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Intimidate:{2}:Command:{3}:{4}",
                    commander.jobLevel, commander.skTactics, commander.skIntimidate, commander.skCommand, commander.jobName);
            }

            foreach (var pilot in db.Load<Job>("SELECT * FROM Job WHERE jobType = 24;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Pilot:{2}:Navigator:{3}:{4}",
                    pilot.jobLevel, pilot.skTactics, pilot.skPilot, pilot.skNavigation, pilot.jobName);
            }

            foreach (var wingCommando in db.Load<Job>("SELECT * FROM Job WHERE jobType = 25;"))
            {
                skillPerJobList.WriteLine("{0}:Blades:{1}:Pilot:{2}:Evasion:{3}:{4}",
                   wingCommando.jobLevel, wingCommando.skMelee, wingCommando.skPilot, wingCommando.skEvasion, wingCommando.jobName);
            }

            foreach (var merchant in db.Load<Job>("SELECT * FROM Job WHERE jobType = 26;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Command:{2}:Negotiate:{3}:{4}",
                    merchant.jobLevel, merchant.skTactics, merchant.skCommand, merchant.skNegotiate, merchant.jobName);
            }

            foreach (var smuggler in db.Load<Job>("SELECT * FROM Job WHERE jobType = 27;"))
            {
                skillPerJobList.WriteLine("{0}:Stealth:{1}:Negotiate:{2}:Intimidate:{3}:{4}",
                    smuggler.jobLevel, smuggler.skStealth, smuggler.skNegotiate, smuggler.skIntimidate, smuggler.jobName);
            }

            foreach (var pirate in db.Load<Job>("SELECT * FROM Job WHERE jobType = 28;"))
            {
                skillPerJobList.WriteLine("{0}:Gunnery:{1}:Pilot:{2}:Intimidate:{3}:{4}",
                    pirate.jobLevel, pirate.skGunnery, pirate.skPilot, pirate.skIntimidate, pirate.jobName);
            }

            foreach (var bountyHunter in db.Load<Job>("SELECT * FROM Job WHERE jobType = 29;"))
            {
                skillPerJobList.WriteLine("{0}:Rifles:{1}:Evasion:{2}:Intimidate:{3}:{4}",
                    bountyHunter.jobLevel, bountyHunter.skHeavyFirearms, bountyHunter.skEvasion, bountyHunter.skIntimidate, bountyHunter.jobName);
            }

            foreach (var militaryOfficer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 30;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Pistols:{2}:Command:{3}:{4}",
                    militaryOfficer.jobLevel, militaryOfficer.skTactics, militaryOfficer.skLightFirearms, militaryOfficer.skCommand, militaryOfficer.jobName);
            }

            foreach (var explorer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 31;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Electronics:{2}:Explore:{3}:{4}",
                    explorer.jobLevel, explorer.skTactics, explorer.skElectronics, explorer.skExplorer, explorer.jobName);
            }

            foreach (var spy in db.Load<Job>("SELECT * FROM Job WHERE jobType = 32;"))
            {
                skillPerJobList.WriteLine("{0}:Pistols:{1}:Stealth:{2}:Electronics:{3}:{4}",
                    spy.jobLevel, spy.skLightFirearms, spy.skStealth, spy.skElectronics, spy.jobName);
            }

            foreach (var engineer in db.Load<Job>("SELECT * FROM Job WHERE jobType = 33;"))
            {
                skillPerJobList.WriteLine("{0}:Repair:{1}:Electronics:{2}:Null:0:{3}",
                    engineer.jobLevel, engineer.skRepair, engineer.skElectronics, engineer.jobName);
            }

            foreach (var quartermaster in db.Load<Job>("SELECT * FROM Job WHERE jobType = 34;"))
            {
                skillPerJobList.WriteLine("{0}:Ship Ops:{1}:Command:{2}:Intimidate:{3}:{4}",
                    quartermaster.jobLevel, quartermaster.skShipOps, quartermaster.skCommand, quartermaster.skIntimidate, quartermaster.jobName);
            }

            foreach (var wingBomber in db.Load<Job>("SELECT * FROM Job WHERE jobType = 35;"))
            {
                skillPerJobList.WriteLine("{0}:Tactics:{1}:Pilot:{2}:Electronics:{3}:{4}",
                    wingBomber.jobLevel, wingBomber.skTactics, wingBomber.skPilot, wingBomber.skElectronics, wingBomber.jobName);
            }

            foreach (var wingLeader in db.Load<Job>("SELECT * FROM Job WHERE jobType = 36;"))
            {
                skillPerJobList.WriteLine("{0}:Gunnery:{1}:Pilot:{2}:Electronics:{3}:{4}",
                    wingLeader.jobLevel, wingLeader.skGunnery, wingLeader.skPilot, wingLeader.skElectronics, wingLeader.jobName);
            }

            skillPerJobList.Close();
        }

        private static void MakeFile_JobList(SQLiteDatabase db)
        {
            // output the header
            StreamWriter jobList = new StreamWriter(Directory.GetCurrentDirectory() + @"\STFPlannerFiles\job_list.csv");
            jobList.WriteLine("Job:Starter");

            // iterate over filtered collection of rows, output text
            foreach (var job in db.Load<Job>("SELECT * FROM JOB WHERE jobLevel = 1 ORDER BY jobType;"))
            {
                jobList.WriteLine("{0}:{1}", job.jobName, getIsStarter(job.jobType));
            }
            jobList.Close();
        }

        // map database keys to string YES/NO for tool
        public static string getIsStarter(int jobType)
        {
            switch (jobType)
            {
                case 15:
                case 20:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                    return "Yes";
                default:
                    break;
            }

            return "No";
        }
    }

    // schema definitions

    // CREATE TABLE Job (_id INTEGER PRIMARY KEY AUTOINCREMENT, jobName TEXT, jobType INTEGER, jobLevel INTEGER, skLightFirearms INTEGER, skHeavyFirearms INTEGER, skMelee INTEGER, 
    //          skEvasion INTEGER, skTactics INTEGER, skStealth INTEGER, skGunnery INTEGER, skPilot INTEGER, skShipOps INTEGER, skRepair INTEGER, skElectronics INTEGER, skNavigation INTEGER, 
    //          skDoctor INTEGER, skCommand INTEGER, skNegotiate INTEGER, skIntimidate INTEGER, skExplorer INTEGER, enabled INTEGER, sortBy INTEGER);
    public class Job
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string jobName { get; set; }
        public int jobType { get; set; }
        public int jobLevel { get; set; }
        public int skLightFirearms { get; set; }
        public int skHeavyFirearms { get; set; }
        public int skMelee { get; set; }
        public int skEvasion { get; set; }
        public int skTactics { get; set; }
        public int skStealth { get; set; }
        public int skGunnery { get; set; }
        public int skPilot { get; set; }
        public int skShipOps { get; set; }
        public int skRepair { get; set; }
        public int skElectronics { get; set; }
        public int skNavigation { get; set; }
        public int skDoctor { get; set; }
        public int skCommand { get; set; }
        public int skNegotiate { get; set; }
        public int skIntimidate { get; set; }
        public int skExplorer { get; set; }
    }

    public class ShipComponent
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string componentName { get; set; }
        public int componentType { get; set; }
        public int componentSize { get; set; }
        public int mass { get; set; }
        public int skPilot { get; set; }
        public int skShipOps { get; set; }
        public int skGunnery { get; set; }
        public int skElectronics { get; set; }
        public int skNavigation { get; set; }
        public int skExplorer { get; set; }
        public int holdsCargo { get; set; }
        public int holdsCrew { get; set; }
        public int holdsOfficer { get; set; }
        public int jumpCost { get; set; }
        public int armorBonus { get; set; }
        public int fuelBonus { get; set; }
        public int holdsGuest { get; set; }
        public int holdsPrisoner { get; set; }
        public int holdsCraft { get; set; }
        public int medicalRating { get; set; }
        public int deflectionBonus { get; set; }
    }

    public class ShipEngine
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string name { get; set; }
        public int designMass { get; set; }
        public int shipSpeed { get; set; }
        public int shipAgile { get; set; }
        public int mapFuelCost { get; set; }
        public int combatFuelCost { get; set; }
        public int safetyRating { get; set; }
        public int moveCost { get; set; }
        public string componentName { get; set; }
    }

    public class Talent
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string talentName { get; set; }
        public int jobLevel { get; set; }
        public int jobType { get; set; }
        public string talentName2 { get; set; }
        public int cooldown { get; set; }
        public string actionType { get; set; }
    }

    public class ShipWeapon
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string weaponName { get; set; }
        public int weaponType { get; set; }
        public int damage { get; set; }
        public int radDamage { get; set; }
        public int voidDamage { get; set; }
        public int range { get; set; }
        public int ap { get; set; }
        public int accuracy { get; set; }
        public int critChance { get; set; }
        public int effectChance { get; set; }
        public int level { get; set; }
    }

    public class CharLevel
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public int level { get; set; }
        public int levelType { get; set; }
    }

    public class ShipType
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string shipTypeName { get; set; }
        public int baseMass { get; set; }
        public int shipCost { get; set; }
        public int smallSlots { get; set; }
        public int mediumSlots { get; set; }
        public int largeSlots { get; set; }
        public int hullPoints { get; set; }
        public int Armor { get; set; }
        public int Deflection { get; set; }
        public int maxOfficer { get; set; }
        public int maxLifeSupport { get; set; }
        public int maxCraft { get; set; }
        public int baseFuel { get; set; }
        public int currentMass { get; set; }
        public int totalSlots { get; set; }
        public int holdsCargo { get; set; }
        public string componentName { get; set; }
        public int shipSpeed { get; set; }
        public int Agility { get; set; }
        public int mapFuelCost { get; set; }
        public int fuelTank { get; set; }
        public int fuelRange { get; set; }
        public int jumpCost { get; set; }
        public int pilot { get; set; }
        public int navigation { get; set; }
        public int shipOps { get; set; }
        public int electronics { get; set; }
        public int gunnery { get; set; }
        public int tier { get; set; }
    }

    public class SmallCraft
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string craftName { get; set; }
        public string description { get; set; }
        public int cost { get; set; }
        public int maxHull { get; set; }
        public int craftTypeId { get; set; }
        public int shipWeaponId { get; set; }
        public int craftArmor { get; set; }
        public int craftDeflection { get; set; }
        public int launchFuelCost { get; set; }
        public int skPilot { get; set; }
        public int skElectronics { get; set; }
        public int skGunnery { get; set; }
        public int baseActionPoints { get; set; }
        public int craftAgile { get; set; }
        public int craftSpeed { get; set; }
        public int repairCost { get; set; }
        public int maintCost { get; set; }
        public int baseToHitShip { get; set; }
        public int baseToHitCraft { get; set; }
        public int baseToDodgeHit { get; set; }
    }

    public class ShipDefault
    {
        [SQLiteColumn(IsPrimaryKey = true)]
        public int _id { get; set; }
        public string shipTypeName { get; set; }
        public string componentName { get; set; }
    }
}
    

