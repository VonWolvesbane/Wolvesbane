using System;
using System.Collections.Generic;
using Server;

namespace Server.Custom.BossDrops
{
    public class BossDropBossMeta
    {
        public string Key;
        public int Body;
        public int Hue;
        public int Difficulty;

        public BossDropBossMeta(string key, int body, int hue, int difficulty)
        {
            Key = key; Body = body; Hue = hue; Difficulty = difficulty;
        }
    }

    public static class BossDropBossData
    {
        private static readonly Dictionary<string, BossDropBossMeta> m_Data = new Dictionary<string, BossDropBossMeta>(StringComparer.OrdinalIgnoreCase);

        public static void Initialize()
        {
            if (m_Data.Count > 0) return;
            // Appearance/stats sourced from Custom/Tested/Idium/Idium.cs
            Register(new BossDropBossMeta("idium-evolution", 46, 1911, 5));
            // Appearance/stats sourced from Custom/Tested/Idium/Idium.cs
            Register(new BossDropBossMeta("idium-evolution-max", 46, 1911, 5));
            // Appearance/stats sourced from Custom/Tested/Goliath/Goliath.cs
            Register(new BossDropBossMeta("goliath-gargish-evolution", 1433, 0, 5));
            // Appearance/stats sourced from Custom/Tested/Goliath/Goliath.cs
            Register(new BossDropBossMeta("goliath-gargish-evolution-max", 1433, 0, 5));
            // Appearance/stats sourced from Custom/Wolvesbane Written/Crusader Gear/Hephastos.cs
            Register(new BossDropBossMeta("hephastos-crusader", 1575, 0, 5));
            // Appearance/stats sourced from Custom/Reapers Uploads/Quests/SwampQueenQuest/SwampQueen.cs
            Register(new BossDropBossMeta("swampqueen-nox", 316, 763, 2));
            // Appearance/stats sourced from Custom/Tested/Destructabo Robo/DestRobo.cs
            Register(new BossDropBossMeta("destrobo-robo", 752, 1985, 3));
            // Appearance/stats sourced from Custom/Tested/Oblivion/Thoroar.cs
            Register(new BossDropBossMeta("thoroar-oblivion", 126, 1261, 5));
            // Appearance/stats sourced from Custom/Wolvesbane Written/Mining Gear/KashmirtheMiner.cs
            Register(new BossDropBossMeta("kashmir-mining", 400, 0, 4));
            // Appearance/stats sourced from Custom/Wolvesbane Written/Taming Gear/VonWolvesbaneEvilShardOwner.cs
            Register(new BossDropBossMeta("vonwolvesbane-taming", 400, 0, 4));
            // Appearance/stats sourced from Custom/Tested/Dane Elec/DaneElec.cs
            Register(new BossDropBossMeta("daneelec-maniac-tailor", 400, 43, 3));
            // Appearance/stats sourced from Custom/Tested/Peccatus/Peccatus.cs
            Register(new BossDropBossMeta("peccatus-sin", 259, 2100, 5));
            // Appearance/stats sourced from Custom/Tested/Cratylus/Cratylus.cs
            Register(new BossDropBossMeta("cratylus", 788, 2245, 5));
            // Appearance/stats sourced from Custom/Tested/Malacoda/Malacoda.cs
            Register(new BossDropBossMeta("malacoda-malabranche", 400, 33775, 5));
            // Appearance/stats sourced from Custom/Tested/Necrosalutor/Necroloricatus.cs
            Register(new BossDropBossMeta("necroloricatus", 400, 0, 4));
            // Appearance/stats sourced from Custom/Tested/Minion/Minion.cs
            Register(new BossDropBossMeta("tmminion-twisted", 400, 2399, 5));
            // Appearance/stats sourced from Custom/Tested/Arachnis/Arachnis.cs
            Register(new BossDropBossMeta("arachnis", 28, 248, 3));
            // Appearance/stats sourced from Custom/Tested/Sataness/Sataness.cs
            Register(new BossDropBossMeta("sataness", 174, 2255, 5));
            // Appearance/stats sourced from Custom/Reapers Uploads/Quests/Naruto Quest/Orochimaru.cs
            Register(new BossDropBossMeta("orochimaru", 400, 1000, 4));
            // Appearance/stats sourced from Custom/Wolvesbane Written/X-Men/Wolverine.cs
            Register(new BossDropBossMeta("wolverine-xmen", 400, 1986, 5));
            // Appearance/stats sourced from Custom/Wolvesbane Written/X-Men/CyclopsXmen.cs
            Register(new BossDropBossMeta("cyclops-xmen", 400, 1984, 4));
            // Appearance/stats sourced from Custom/Wolvesbane Written/X-Men/StormXmen.cs
            Register(new BossDropBossMeta("storm-xmen", 401, 1545, 4));
            // Appearance/stats sourced from Custom/Wolvesbane Written/X-Men/GambitXMen.cs
            Register(new BossDropBossMeta("gambit-xmen", 400, 36, 4));
            // Appearance/stats sourced from Custom/Tested/God of Chaos/Seth.cs
            Register(new BossDropBossMeta("seth-chaos", 175, 2915, 5));
            // Appearance/stats sourced from Custom/Tested/Adalbrecht/Adalbrecht.cs
            Register(new BossDropBossMeta("adalbrecht-daminoc", 400, 33775, 2));
            // Appearance/stats sourced from Custom/Tested/Quardanic/Quardanic.cs
            Register(new BossDropBossMeta("quardanic-ancient-dragon", 400, 33775, 3));
            // Appearance/stats sourced from Custom/Tested/Void Knight/VoidKnight.cs
            Register(new BossDropBossMeta("void-knight", 311, 1931, 5));
            // Appearance/stats sourced from Custom/Tested/Leprechaun/TheLeprechaun.cs
            Register(new BossDropBossMeta("leprechaun", 140, 69, 2));
            // Appearance/stats sourced from Custom/Tested/Suicide/Suicide.cs
            Register(new BossDropBossMeta("suicide", 400, 33775, 4));
            // Appearance/stats sourced from Custom/Tested/Might Armor Quest/RS Mobiles/RS Zeus/RSZeus.cs
            Register(new BossDropBossMeta("zeus-heavens", 308, 2248, 3));
            // Appearance/stats sourced from Custom/Reapers Uploads/Quests/Quete of Elements english/Mobiles/Poseidon.cs
            Register(new BossDropBossMeta("poseidon-sea", 16, 0, 1));
            // Appearance/stats sourced from Custom/Tested/Might Armor Quest/RS Mobiles/RS Hades/RSHades.cs
            Register(new BossDropBossMeta("hades-underworld", 308, 1109, 4));
            // Appearance/stats sourced from Custom/Tested/Might Armor Quest/Crockett Scarr/Crockett.cs
            Register(new BossDropBossMeta("crockett-demon-pact", 400, 0, 3));
            // Appearance/stats sourced from Custom/Tested/Kage-Maru/KageMaru.cs
            Register(new BossDropBossMeta("kage-maru", 9, 1107, 3));
            // Appearance/stats sourced from Custom/Tested/Skeletal Serpent/SkeletalSerpent.cs
            Register(new BossDropBossMeta("skeletal-serpent", 104, 65, 2));
            // Appearance/stats sourced from Custom/Tested/Tergus/Tergus.cs
            Register(new BossDropBossMeta("tergus", 400, 33775, 2));
            // Appearance/stats sourced from Custom/Tested/Caveman/Caveman.cs
            Register(new BossDropBossMeta("alley-oop-caveman", 400, 33779, 3));
            // Appearance/stats sourced from Custom/Tested/Death Angel/DeathAngel.cs
            Register(new BossDropBossMeta("death-angel", 401, 33918, 3));
            // Appearance/stats sourced from Custom/Tested/Sexi Vampire/SexiVampire.cs
            Register(new BossDropBossMeta("sexi-vampire", 745, 33918, 2));
            // Appearance/stats sourced from Custom/Tested/King Kamuu/KingKamuu.cs
            Register(new BossDropBossMeta("king-kamuu-atlantis", 400, 2716, 2));
            // Appearance/stats sourced from Custom/Reapers Uploads/Kindreds Items/Kindred.cs
            Register(new BossDropBossMeta("witches", 401, 2452, 4));
            // Appearance/stats sourced from Custom/Tested/Akyndah/Akyndah.cs
            Register(new BossDropBossMeta("akyndah", 258, 2654, 3));
            // Appearance/stats sourced from Custom/Tested/Kronik/Kronik.cs
            Register(new BossDropBossMeta("kronik", 400, 487, 4));
            // Appearance/stats sourced from Custom/Tested/Love Angel/LoveAngel.cs
            Register(new BossDropBossMeta("love-angel-female", 401, 33775, 5));
            // Appearance/stats sourced from Custom/Tested/Love Angel/LoveAngel.cs
            Register(new BossDropBossMeta("love-angel-male", 401, 33775, 5));
            // Appearance/stats sourced from Custom/Tested/Megami/Megami.cs
            Register(new BossDropBossMeta("megami", 174, 2946, 5));
            // Appearance/stats sourced from Custom/Tested/Father Time/FatherTime.cs
            Register(new BossDropBossMeta("father-time", 24, 2377, 5));
            // Appearance/stats sourced from Custom/Tested/Fire Demon/IgnisMalum.cs
            Register(new BossDropBossMeta("ignis-malum", 9, 2666, 2));
            // Appearance/stats sourced from Custom/Tested/Scarab/Scarab.cs
            Register(new BossDropBossMeta("scarab-ancient", 9, 757, 2));
            // Appearance/stats sourced from Custom/Tested/Thief/Garuda.cs
            Register(new BossDropBossMeta("garuda-stolen-jewels", 9, 1161, 5));

            // Added v2.15: previously registered boss displays that were missing difficulty metadata.
            // Stats sourced from Custom/Tested/Demon Lord/DemonLord.cs
            Register(new BossDropBossMeta("demonlord", 792, 1795, 5));
            // Stats sourced from Custom/Tested/Revelation/Revelation.cs
            Register(new BossDropBossMeta("revelation", 123, 1700, 5));
            // Stats sourced from Custom/Reapers Uploads/Alien SuperBoss/monsters/Alien1.cs
            Register(new BossDropBossMeta("alien-perfect", 777, 0, 5));
            // Stats sourced from Custom/Tested/Pimp/Mr.Bones.cs
            Register(new BossDropBossMeta("pimp", 0, 1150, 5));
            // Stats sourced from Custom/Tested/Pillager/ThePillager.cs
            Register(new BossDropBossMeta("pillager", 182, 2313, 5));
            // Armor of Might is tied to Titanious in the Might Armor quest chain.
            Register(new BossDropBossMeta("might", 0, 0, 5));
            // Magi Armor is distributed across multiple boss scripts; use a representative
            // Wolvesbane rating rather than leaving the boss collection unrated.
            Register(new BossDropBossMeta("magi-armor", 0, 0, 4));
        }

        private static void Register(BossDropBossMeta data)
        {
            if (data != null && !String.IsNullOrEmpty(data.Key)) m_Data[data.Key] = data;
        }

        public static BossDropBossMeta Find(string key)
        {
            Initialize();
            BossDropBossMeta data;
            return key != null && m_Data.TryGetValue(key, out data) ? data : null;
        }
    }
}