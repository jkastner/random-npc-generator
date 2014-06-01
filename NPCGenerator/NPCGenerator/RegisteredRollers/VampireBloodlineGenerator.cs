using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NPCGenerator
{
    class VampireBloodlineGenerator : BaseRegisteredRoller
    {
        private List<String> _daevaBloodlines = new List<string>();
        private List<String> _gangrelBloodlines = new List<string>();
        private List<String> _juliiBloodlines = new List<string>();
        private List<String> _mekhetBloodlines = new List<string>();
        private List<String> _nosferatuBloodlines = new List<string>();
        private List<String> _ventrueBloodlines = new List<string>();

        internal VampireBloodlineGenerator()
        {
            String daevaBloodlines = @"Amara Havana (VTR: Ancient Bloodlines, p. 43 -52)
                                        Anvari (VTR: Bloodlines: The Hidden, p. 20 -29)
                                        The Asnâm (VTR: Circle of the Crone (book), p. 165 -167)
                                        The Carnival (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 23-37)
                                        Children of Judas (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 38-50)
                                        Duchagne (VTR: Vampire: The Requiem Rulebook, p. 105 ; Bloodlines: The Chosen, p. 36-45)
                                        En (VTR: Ancient Bloodlines, p. 153 -156)
                                        Erzsébet (VTR: Kiss of the Succubus, p. 114 -115)
                                        Eupraxus (VTR: Night Horrors: Immortal Sinners p. 54-55)
                                        Gulikan (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 64-77)
                                        Kallisti (VTR: Invictus (book), p. 162 )
                                        Kinnaree (VTR: Ancient Bloodlines, p. 117 -121)
                                        Malintzin (WOD: Shadows of Mexico, p. 101 )
                                        Mortifiers of the Flesh (VTR: Lancea Sanctum (book), p. 170 -172)
                                        Nelapsi (VTR: Bloodlines: The Hidden, p. 88 -95)
                                        Septemi (VTR: Ancient Bloodlines, p. 89 -96)
                                        Spina (VTR: Invictus (book), p. 173 )
                                        Toreador (VTR: Vampire: The Requiem Rulebook, p. 246 -248)
                                        Xiao (VTR: Vampire: The Requiem Rulebook, p. 105 ; Bloodlines: The Chosen, p. 85-104)
                                        California Xiao (VTR: Bloodlines: The Chosen, p. 85 -94)
                                        Tianpàn Xiao (VTR: Bloodlines: The Chosen, p. 95 -104)
                                        Zelani (VTR: Carthians (book), p. 166 -169)
                                        ";



            String gangrelBloodlines = @"Anavashra (VTR: Vampire: The Requiem Rulebook, p. 107 )
                                        Annunaku (VTR: Invictus (book), p. 159 )
                                        Anubi (VTR: Vampire: The Requiem Rulebook, p. 107 )
                                        Barjot (VTR: Carthians (book), p. 156 -159)
                                        Bohagande (VTR: Bloodlines: The Hidden, p. 38 -47)
                                        Bruja (VTR: Vampire: The Requiem Rulebook, p. 235 -237)
                                        Carnon (VTR: Circle of the Crone (book), p. 168 -171)
                                        Childer of the Morrigan (VTR: Circle of the Crone (book), p. 172 -175)
                                        Dead Wolves (WOD: Shadows of Mexico Bullet-pdf Bullet-nip, p. 102-103)
                                        Empusae (VTR: Night Horrors: Immortal Sinners p. 127)
                                        Hounds of Actaeon (VTR, p. 111-112)
                                        Les Gens Libres (VTR: Ancient Bloodlines, p. 69 -72)
                                        Larvae (VTR: Requiem for Rome Rulebook, p. 228 -230)
                                        Mabry (VTR: Savage and Macabre p. 110-111)
                                        The Mara (VTR: Circle of the Crone (book), p. 181 -183)
                                        Mistikoi (VTR: Ancient Bloodlines, p. 130 -136)
                                        Matasuntha (VTR: Vampire: The Requiem Rulebook, p. 107 )
                                        Moroi (VTR: Ordo Dracul (book), p. 159
                                        Nepheshim (VTR: Lancea Sanctum (book), p. 160 -161)
                                        Oberlochs (VTR: Bloodlines: The Hidden, p. 96 -105)
                                        Shepherds (VTR: Ancient Bloodlines, p. 36 -39)
                                        Sta-Au (VTR: Ancient Bloodlines, p. 30 -5). Parent clan technically unknown and possibly Mekhet.
                                        Taifa (VTR: Vampire: The Requiem Rulebook, p. 107 ; Bloodlines: The Chosen, p. 75-84)
                                        Vedma (VTR: Ordo Dracul (book) Bullet-pdf Bullet-nip, p. 169-171)
                                        ";


            String juliiBloodlines = "Licinii (VTR: Requiem for Rome Rulebook, p. 231 -232)";


            String mekhetBloodlines = @"Agonistes (VTR: Vampire: The Requiem Rulebook, p. 109 ; Bloodlines: The Chosen, p. 16-25)
                                        Alucinor (VTR: Bloodlines: The Hidden, p. 12 -19)
                                        Bak-Ra (VTR: Ancient Bloodlines, p. 167 -171)
                                        Brothers of Ypres (VTR: Ancient Bloodlines, p. 22 -26)
                                        Család (VTR: Night Horrors: Immortal Sinners p. 118-120)
                                        Khaibit (VTR: Bloodlines: The Hidden, p. 58 -67)
                                        Kuufukuji (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 78-90)
                                        Iltani (VTR: Ancient Bloodlines, p. 145 -152)
                                        Libitinarius (VTR: Ordo Dracul (book), p. 155 -158)
                                        Lynx (VTR: Invictus (book), p. 165 )
                                        Mnemosyne (VTR: Vampire: The Requiem Rulebook, p. 109 , Shadows in the Dark p. 82-83)
                                        Morbus (VTR: Vampire: The Requiem Rulebook, p. 245 -246; Requiem for Rome Rulebook, p. 233-235)
                                        Norvegi (VTR: Vampire: The Requiem Rulebook, p. 109 , Shadows in the Dark p. 84-85)
                                        Osites (VTR: Lancea Sanctum (book), p. 173 -175)
                                        Players (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 116-128)
                                        Qedeshah (VTR: Bloodlines: The Hidden, p. 106 -115)
                                        Sangiovanni (VTR: Vampire: The Requiem Rulebook, p. 109 ; Bloodlines: The Chosen, p. 65-74)
                                        Sta-Au (VTR: Ancient Bloodlines, p. 30 -35). Parent clan technically unknown and possibly Gangrel.
                                        Tismanu (VTR: Ordo Dracul (book), p. 164 -168)
                                        ";

            String nosferatuBloodlines = @"Acteius (VTR: Vampire: The Requiem Rulebook, p. 111 )
                                            Adroanzi (VTR: Ancient Bloodlines, p. 106 -111)
                                            Azerkatil (VTR: Ordo Dracul (book) Bullet-pdf Bullet-nip, p. 145-149)
                                            Baddacelli (VTR: Vampire: The Requiem Rulebook, p. 111 ; Bloodlines: The Chosen, p. 26-35)
                                            Burakumin (VTR: Vampire: The Requiem Rulebook, p. 238 -240)
                                            Calacas (WOD: Shadows of Mexico Bullet-pdf Bullet-nip, p. 99-101)
                                            Caporetti (VTR: Ancient Bloodlines, p. 18 -21)
                                            The Cockscomb Society (VTR: The Beast That Haunts the Night p. 106-107)
                                            Galloi (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 51-63)
                                            Gethsemani (VTR: Bloodlines: The Hidden, p. 48 -57)
                                            Licinii (VTR: Requiem for Rome Rulebook, p. 231 -232)
                                            Lygos (VTR: The Beast That Haunts the Night p. 107-108)
                                            Mayarap (VTR]: Ancient Bloodlines, p. 122-124)
                                            Moroi (VTR: Ordo Dracul (book) Bullet-pdf Bullet-nip, p. 159-169)
                                            Morotrophians (VTR: Bloodlines: The Hidden, p. 68 -77)
                                            Noctuku (VTR: Vampire: The Requiem Rulebook, p. 111 ; Bloodlines: The Chosen, p. 46-55)
                                            Order of Sir Martin (VTR: Ancient Bloodlines, p. 137 -140)
                                            Rakshasa (VTR: Bloodlines: The Hidden, p. 116 -125)
                                            Telamones (VTR: Night Horrors: Immortal Sinners p. 94-95)
                                            Usiri (VTR: Ancient Bloodlines, p. 161 -166)
                                            Yagnatia (VTR: Vampire: The Requiem Rulebook, p. 111 ; Bloodlines: The Chosen, p. 105-114)
                                            ";

            String ventrueBloodlines = @"Adrestoi (VTR: Lords Over the Damned, p. 104 -105)
                                            Appolinaire (VTR: Ancient Bloodlines, p. 63 -68)
                                            Architects of the Monolith (VTR: Bloodlines: The Hidden, p. 30 -37)
                                            Beni Murrahim (VTR: Vampire: The Requiem Rulebook, p. 113 )
                                            Bron (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 10-22)
                                            Canda Bhanu (VTR: Ancient Bloodlines, p. 53 -58)
                                            Cassians (VTR: Vampire: The Requiem Rulebook, p. 113 )
                                            Corajoso (VTR: Ancient Bloodlines, p. 99 -105)
                                            Deucalion (VTR: Carthians (book), p. 160 -165)
                                            Dragolescu (VTR: Ordo Dracul (book), p. 150 -154)
                                            Geheim (VTR: Ancient Bloodlines, p. 81 -88)
                                            Gorgons (VTR: Circle of the Crone (book), p. 176 -179)
                                            Icarians (VTR: Lancea Sanctum (book), p. 167 -169)
                                            Licini (VTR: Vampire: The Requiem Rulebook, p. 113 )
                                            Macellarius (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 90-102)
                                            Malkovians (VTR: Vampire: The Requiem Rulebook, p. 241 -243, Lords Over the Damned, p. 110-116)
                                            Malocusians (VTR: Invictus (book), p. 168 )
                                            Melissidae (VTR: Bloodlines: The Legendary Bullet-pdf Bullet-nip, p. 103-115)
                                            Nahualli (VTR: Bloodlines: The Hidden, p. 78 -87)
                                            Rötgrafen (VTR: Vampire: The Requiem Rulebook, p. 113 ; Bloodlines: The Chosen, p. 56-64)
                                            Sotoha (VTR: Invictus (book), p. 170 )
                                            The Sons of Cade (VTR: Night Horrors: Immortal Sinners p. 49)
                                            ";


            PopulateList(_daevaBloodlines, daevaBloodlines);
            PopulateList(_gangrelBloodlines, gangrelBloodlines);
            PopulateList(_mekhetBloodlines, mekhetBloodlines);
            PopulateList(_nosferatuBloodlines, nosferatuBloodlines);
            PopulateList(_juliiBloodlines, juliiBloodlines);
            PopulateList(_ventrueBloodlines, ventrueBloodlines);
            
            _nosferatuBloodlines.AddRange(_juliiBloodlines);
            _juliiBloodlines.AddRange(_ventrueBloodlines);
        }

        private void PopulateList(List<string> lineSeperatedList, string lines)
        {
            var splitLines = lines.Split('\n');
            foreach (var cur in splitLines.Where(x=>!String.IsNullOrWhiteSpace(x.Trim())))
            {
                lineSeperatedList.Add(cur.Trim());
            }
        }




        internal override string RollerName
        {
            get { return "VampireBloodlineGenerator"; }
        }

        internal override void Run(NPC newNPC)
        {
            var clanTrait =  newNPC.Traits.FirstOrDefault(x => x.Label.Equals("Clan"));
            if (clanTrait == null)
            {
                return;
            }
            var bloodlineTrait = newNPC.Traits.FirstOrDefault(x => x.Label.Equals("Bloodline"));
            if (bloodlineTrait == null)
            {
                newNPC.AddTrait("Bloodline", "");
                bloodlineTrait = newNPC.Traits.FirstOrDefault(x => x.Label.Equals("Bloodline"));
            }
            
            GetBloodline(bloodlineTrait, clanTrait);
        }

        private void GetBloodline(TraitLabelValue bloodlineTrait, TraitLabelValue clanTrait)
        {
            switch (clanTrait.Value)
            {
                case "Daeva":
                    GetBloodline(_daevaBloodlines, bloodlineTrait);
                    break;
                case "Gangrel":
                    GetBloodline(_gangrelBloodlines, bloodlineTrait);
                    break;
                case "Mekhet":
                    GetBloodline(_mekhetBloodlines, bloodlineTrait);
                    break;
                case "Nosferatu":
                    GetBloodline(_nosferatuBloodlines, bloodlineTrait);
                    break;
                case "Ventrue":
                    GetBloodline(_ventrueBloodlines, bloodlineTrait);
                    break;
                case "Julii":
                    GetBloodline(_juliiBloodlines, bloodlineTrait);
                    break;

            }
        }

        private void GetBloodline(List<string> bloodline, TraitLabelValue bloodlineTrait)
        {
            var index = NPCViewModel.RandomValue(0, bloodline.Count);
            bloodlineTrait.Value = bloodline[index];
        }

    }
}
