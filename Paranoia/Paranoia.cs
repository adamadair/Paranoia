/* This is a solo paranoia game taken from the Jan/Feb issue (No 77) of
   "SpaceGamer/FantasyGamer" magazine.

   Article by Sam Shirley.

   Implemented in C on Vax 11/780 under UNIX by Tim Lister
   Implemented in C# on some lousy HP laptop running Windows 7 by Adam Adair

   This is a public domain adventure and may not be sold for profit 
 */
using System;
using System.Collections.Generic;

namespace Paranoia
{
    /// <summary>
    /// Reusable Paranoia game class.
    /// </summary>
    public class Paranoia
    {
        public const string MORE_STRING = "[%MORE%]";
        const int MOXIE = 13;
        const int AGILITY = 15;
        const int MAXKILL = 7;		// The maximum number of UV's you can kill 
        int clone = 1;
        int page = 1;
        int computer_request = 0;
        int ultra_violet = 0;
        int action_doll = 0;
        int hit_points = 10;
        int read_letter = 0;
        int plato_clone = 3;
        int blast_door = 0;
        int killer_count = 0;

        private List<string> _lineBuffer;
        private List<ParanoiaChoice> _choices;
        private int goToPage = 0;

        private MTRandom _random;

        public Paranoia()
        {
            _lineBuffer = new List<string>();       // initialize line buffer
            _choices = new List<ParanoiaChoice>();
            _random = new MTRandom();               // init random number gerenator using time
        }

        /// <summary>
        /// variable face die roll simulator. 
        /// </summary>
        /// <param name="number">number of dice to roll</param>
        /// <param name="faces">number of faces on each die</param>
        /// <returns>the total of all dice rolled</returns>
        private int dice_roll(int number, int faces)
        {
            int i, total = 0;

            for (i = number; i > 0; i--)
                total += _random.Next() % faces + 1;
            return total;
        }

        #region Public Interface
        public ParanoiaResponse Instructions()
        {
            ClearBuffer();
            instructions();
            return NewResponse();
        }

        public ParanoiaResponse Character()
        {
            ClearBuffer();
            character();
            return NewResponse();
        }

        public ParanoiaResponse Start()
        {
            this.page = 1;
            ClearBuffer();
            page1();
            return NewResponse();
        }

        public ParanoiaResponse PlayerChoice(ParanoiaChoice choice)
        {
            ClearBuffer();
            page = choice.Page;
            choice.Action();
            return NewResponse();
        }

        #endregion
        private void ClearBuffer()
        {
            _lineBuffer.Clear();
            _choices.Clear();
            goToPage = 0;
        }
        private ParanoiaResponse NewResponse()
        {
            ParanoiaResponse response = new ParanoiaResponse();
            response.TextLines = _lineBuffer.ToArray();
            response.Choices = _choices.ToArray();
            response.GoTo = goToPage;
            return response;
        }

        #region Pages
        void instructions()
        {
            printf("Welcome to Paranoia!~~");
            printf("HOW TO PLAY:~~");
            printf("  Just press <RETURN> until you are asked to make a choice.~");
            printf("  Select 'a' or 'b' or whatever for your choice, then press <RETURN>.~");
            printf("  You may select 'p' at any time to get a display of your statistics.~");
            printf("  Always choose the least dangerous option.  Continue doing this until you win.~");
            printf("  At times you will use a skill or engage in combat and and will be informed of~");
            printf("  the outcome.  These sections will be self explanatory.~~");
            printf("HOW TO DIE:~~");
            printf("  As Philo-R-DMD you will die at times during the adventure.~");
            printf("  When this happens you will be given an new clone at a particular location.~");
            printf("  The new Philo-R will usually have to retrace some of the old Philo-R's path;~");
            printf("  hopefully he won't make the same mistake as his predecessor.~~");
            printf("HOW TO WIN:~~");
            printf("  Simply complete the mission before you expend all six clones.~");
            printf("  If you make it, congratulations.~");
            printf("  If not, you can try again later.~");
        }

        void character()
        {
            printf("===============================================================================~");
            printf(string.Format("The Character : Philo-R-DMD {0}~", clone));
            printf("Primary Attributes                      Secondary Attributes~");
            printf("===============================================================================~");
            printf("Strength ..................... 13       Carrying Capacity ................. 30~");
            printf("Endurance .................... 13       Damage Bonus ....................... 0~");
            printf("Agility ...................... 15       Macho Bonus ....................... -1~");
            printf("Manual Dexterity ............. 15       Melee Bonus ...................... +5%%~");
            printf("Moxie ........................ 13       Aimed Weapon Bonus .............. +10%%~");
            printf("Chutzpah ...................... 8       Comprehension Bonus .............. +4%%~");
            printf("Mechanical Aptitude .......... 14       Believability Bonus .............. +5%%~");
            printf("Power Index .................. 10       Repair Bonus ..................... +5%%~");
            printf("===============================================================================~");
            printf("Credits: 160        Secret Society: Illuminati        Secret Society Rank: 1~");
            printf("Service Group: Power Services               Mutant Power: Precognition~");
            printf("Weapon: laser pistol; to hit, 40%%; type, L; Range, 50m; Reload, 6r; Malfnt, 00~");
            printf("Skills: Basics 1(20%%), Aimed Weapon Combat 2(35%%), Laser 3(40%%),~        Personal Development 1(20%%), Communications 2(29%%), Intimidation 3(34%%)~");
            printf("Equipment: Red Reflec Armour, Laser Pistol, Laser Barrel (red),~");
            printf("           Notebook & Stylus, Knife, Com Unit 1, Jump suit,~");
            printf("           Secret Illuminati Eye-In-The-Pyramid(tm) Decoder ring,~");
            printf("           Utility Belt & Pouches~");
            printf("===============================================================================~");
        }

        /// <summary>
        /// Kills current clone and activates new clone.
        /// </summary>
        /// <returns>true if new clone is available</returns>
        bool new_clone()
        {
            printf(string.Format("~Clone {0} just died.~", clone));
            if (++clone > 6) {
                printf("~*** You Lose ***~~All your clones are dead.  Your name has been stricken from the records.~~			THE END~");
                return false;
            } else {
                printf(string.Format("Clone {0} now activated.~", clone));
                ultra_violet = 0;
                action_doll = 0;
                hit_points = 10;
                killer_count = 0;
            }
            return true;
        }

        void page1()
        {
            nl();
            printf("You wake up face down on the red and pink checked E-Z-Kleen linoleum floor.~");
            printf("~You recognise the pattern, it's the type preferred in the internal security~briefing cells.  When you finally look around you, you see that you are alone~");
            printf("in a large mission briefing room.~");
            page57();
        }

        void page2()
        {
            nl();
            printf("\"Greetings,\" says the kindly Internal Security self incrimination expert who~");
            printf("meets you at the door, \"How are we doing today?\"  He offers you a doughnut~");
            printf("and coffee and asks what brings you here.  This doesn't seem so bad, so you~");
            printf("tell him that you have come to confess some possible security lapses.  He~");
            printf("smiles knowingly, deftly catching your coffee as you slump to the floor.~");
            printf("\"Nothing to be alarmed about; it's just the truth serum,\" he says,~");
            printf("dragging you back into a discussion room.~");
            printf("The next five hours are a dim haze, but you can recall snatches of conversation~");
            printf("about your secret society, your mutant power, and your somewhat paranoid~");
            printf("distrust of The Computer.  This should explain why you are hogtied and moving~");
            printf("slowly down the conveyer belt towards the meat processing unit in Food~");
            printf("Services.~");
            if (computer_request == 1) {

                if (new_clone())
                {
                    page45(); return;
                }
            }
            else
            {
                if (new_clone())
                {
                    page32();return;
                }
            }
        }

        void page3()
        {
            nl();
            printf("You walk to the nearest Computer terminal and request more information about~");
            printf("Christmas.  The Computer says, \"That is an A-1 ULTRAVIOLET ONLY IMMEDIATE~");
            printf("TERMINATION classified topic.  What is your clearance please, Troubleshooter?\"~");
            choose(4, "You give your correct clearance", page4, 5, "You lie and claim Ultraviolet clearance", page5);
        }
        void page4()
        {
            nl();
            printf("\"That is classified information, Troubleshooter, thank you for your inquiry.~");
            printf(" Please report to an Internal Security self incrimination station as soon as~");
            printf(" possible.\"~");
            page9();
        }

        void page5()
        {
            nl();
            printf("The computer says, \"Troubleshooter, you are not wearing the correct colour~");
            printf("uniform.  You must put on an Ultraviolet uniform immediately.  I have seen to~");
            printf("your needs and ordered one already; it will be here shortly.  Please wait with~");
            printf("your back to the wall until it arrives.\"  In less than a minute an infrared~");
            printf("arrives carrying a white bundle.  He asks you to sign for it, then hands it to~");
            printf("you and stands back, well outside of a fragmentation grenade's blast radius.~");
            choose(6, "You open the package and put on the uniform", page6, 7, "You finally come to your senses and run for it", page7);
        }

        void page6()
        {
            nl();
            printf("The uniform definitely makes you look snappy and pert.  It really looks~");
            printf("impressive, and even has the new lopsided lapel fashion that you admire so~");
            printf("much.  What's more, citizens of all ranks come to obsequious attention as you~");
            printf("walk past.  This isn't so bad being an Ultraviolet.  You could probably come~");
            printf("to like it, given time.~");
            printf("The beeping computer terminal interrupts your musings.~");
            ultra_violet = 1;
            page8();
        }

        void page7()
        {
            nl();
            printf("The corridor lights dim and are replaced by red battle lamps as the Security~");
            printf("Breach alarms howl all around you.  You run headlong down the corridor and~");
            printf("desperately windmill around a corner, only to collide with a squad of 12 Blue~");
            printf("clearance Vulture squadron soldiers.  \"Stop, Slime Face,\" shouts the~");
            printf("commander, \"or there won't be enough of you left for a tissue sample.\"~");
            printf("\"All right, soldiers, stuff the greasy traitor into the uniform,\" he orders,~");
            printf("waving the business end of his blue laser scant inches from your nose.~");
            printf("With his other hand he shakes open a white bundle to reveal a pristine new~");
            printf("Ultraviolet citizen's uniform.~");
            printf("One of the Vulture squadron Troubleshooters grabs you by the neck in the~");
            printf("exotic and very painful Vulture Clamp(tm) death grip (you saw a special about~");
            printf("it on the Teela O'Malley show), while the rest tear off your clothes and~");
            printf("force you into the Ultraviolet uniform.  The moment you are dressed they step~");
            printf("clear and stand at attention.~");
            printf("\"Thank you for your cooperation, sir,\" says the steely eyed leader of the~");
            printf("Vulture Squad.  \"We will be going about our business now.\"  With perfect~");
            printf("timing the Vultures wheel smartly and goosestep down the corridor.~");
            printf("Special Note: don't make the mistake of assuming that your skills have~");
            printf("improved any because of the uniform; you're only a Red Troubleshooter~");
            printf("traitorously posing as an Ultraviolet, and don't you forget it!~");
            printf("Suddenly, a computer terminal comes to life beside you.~");
            ultra_violet = 1;
            page8();
        }

        void page8()
        {
            nl();
            printf("\"Now, about your question, citizen.  Christmas was an old world marketing ploy~");
            printf("to induce lower clearance citizens to purchase vast quantities of goods, thus~");
            printf("accumulation a large amount of credit under the control of a single class of~");
            printf("citizen known as Retailers.  The strategy used is to imply that all good~");
            printf("citizens give gifts during Christmas, thus if one wishes to be a valuable~");
            printf("member of society one must also give gifts during Christmas.  More valuable~");
            printf("gifts make one a more valuable member, and thus did the Retailers come to~");
            printf("control a disproportionate amount of the currency.  In this way Christmas~");
            printf("eventually caused the collapse of the old world.  Understandably, Christmas~");
            printf("has been declared a treasonable practice in Alpha Complex.~");
            printf("Thank you for your inquiry.\"~");
            printf("You continue on your way to GDH7-beta.~");
            page10();
        }

        void page9()
        {
            nl();
            printf("As you walk toward the tubecar that will take you to GDH7-beta, you pass one~");
            printf("of the bright blue and orange Internal Security self incrimination stations.~");
            printf("Inside, you can see an IS agent cheerfully greet an infrared citizen and then~");
            printf("lead him at gunpoint into one of the rubber lined discussion rooms.~");
            choose(2, "You decide to stop here and chat, as ordered by The Computer", talkToComputer, 10, "You just continue blithely on past", dontTalkToComputer);
        }

        void talkToComputer()
        {
            computer_request = 1;
            page2();
        }

        void dontTalkToComputer()
        {
            computer_request = 0;
            page10();
        }


        void page10()
        {
            nl();
            printf("You stroll briskly down the corridor, up a ladder, across an unrailed catwalk,~");
            printf("under a perilously swinging blast door in urgent need of repair, and into~");
            printf("tubecar grand central.  This is the bustling hub of Alpha Complex tubecar~");
            printf("transportation.  Before you spreads a spaghetti maze of magnalift tube tracks~");
            printf("and linear accelerators.  You bravely study the specially enhanced 3-D tube~");
            printf("route map; you wouldn't be the first Troubleshooter to take a fast tube ride~");
            printf("to nowhere.~");
            if (ultra_violet == 0)
            {
                choose(3, "You decide to ask The Computer about Christmas using a nearby terminal", page3, 10, "You think you have the route worked out, so you'll board a tube train", boardTubeTrain);
                return;
            };
            boardTubeTrain();
        }

        void boardTubeTrain()
        {
            printf("You nervously select a tubecar and step aboard.~");
            if (dice_roll(2, 10) < MOXIE)
            {
                printf("You just caught a purple line tubecar.~");               
                page13();
            }
            else
            {
                printf("You just caught a brown line tubecar.~");
                page48();
            }
        }


        void page11()
        {
            nl();
            printf("The printing on the folder says \"Experimental Self Briefing.\"~");
            printf("You open it and begin to read the following:~");
            printf("Step 1: Compel the briefing subject to attend the briefing.~");
            printf("        Note: See Experimental Briefing Sub Form Indigo-WY-2,~");
            printf("        'Experimental Self Briefing Subject Acquisition Through The Use Of~");
            printf("         Neurotoxin Room Foggers.'~");
            printf("Step 2: Inform the briefing subject that the briefing has begun.~");
            printf("        ATTENTION: THE BRIEFING HAS BEGUN.~");
            printf("Step 3: Present the briefing material to the briefing subject.~");
            printf("        GREETINGS TROUBLESHOOTER.~");
            printf("        YOU HAVE BEEN SPECIALLY SELECTED TO SINGLEHANDEDLY~");
            printf("        WIPE OUT A DEN OF TRAITOROUS CHRISTMAS ACTIVITY.  YOUR MISSION IS TO~");
            printf("        GO TO GOODS DISTRIBUTION HALL 7-BETA AND ASSESS ANY CHRISTMAS ACTIVITY~");
            printf("        YOU FIND THERE.  YOU ARE TO INFILTRATE THESE CHRISTMAS CELEBRANTS,~");
            printf("        LOCATE THEIR RINGLEADER, AN UNKNOWN MASTER RETAILER, AND BRING HIM~");
            printf("        BACK FOR EXECUTION AND TRIAL.  THANK YOU.  THE COMPUTER IS YOUR FRIEND.~");
            printf("Step 4: Sign the briefing subject's briefing release form to indicate that~");
            printf("        the briefing subject has completed the briefing.~");
            printf("        ATTENTION: PLEASE SIGN YOUR BRIEFING RELEASE FORM.~");
            printf("Step 5: Terminate the briefing~");
            printf("        ATTENTION: THE BRIEFING IS TERMINATED.~");
            more();
            printf("You walk to the door and hold your signed briefing release form up to the~");
            printf("plexiglass window.  A guard scrutinises it for a moment and then slides back~");
            printf("the megabolts holding the door shut.  You are now free to continue the~");
            printf("mission.~");
            choose(3, "You wish to ask The Computer for more information about Christmas", page3, 10, "You have decided to go directly to Goods Distribution Hall 7-beta", page10);
        }

        void page12()
        {
            nl();
            printf("You walk up to the door and push the button labelled \"push to exit.\"~");
            printf("Within seconds a surly looking guard shoves his face into the small plexiglass~");
            printf("window.  You can see his mouth forming words but you can't hear any of them.~");
            printf("You just stare at him blankly  for a few moments until he points down to a~");
            printf("speaker on your side of the door.  When you put your ear to it you can barely~");
            printf("hear him say, \"Let's see your briefing release form, bud.  You aren't~");
            printf("getting out of here without it.\"~");
            choose(11, "You sit down at the table and read the Orange packet", page11, 57, "You stare around the room some more", page57);
        }


        
        void page13()
        {
            nl();
            printf("You step into the shiny plasteel tubecar, wondering why the shape has always~");
            printf("reminded you of bullets.  The car shoots forward the instant your feet touch~");
            printf("the slippery gray floor, pinning you immobile against the back wall as the~");
            printf("tubecar careens toward GDH7-beta.  Your only solace is the knowledge that it~");
            printf("could be worse, much worse.~");
            printf("Before too long the car comes to a stop.  You can see signs for GDH7-beta~");
            printf("through the window.  With a little practice you discover that you can crawl~");
            printf("to the door and pull open the latch.~");
            page14();
        }

        void page14()
        {
            nl();
            printf("You manage to pull yourself out of the tubecar and look around.  Before you is~");
            printf("one of the most confusing things you have ever seen, a hallway that is~");
            printf("simultaneously both red and green clearance.  If this is the result of~");
            printf("Christmas then it's easy to see the evils inherent in its practice.~");
            printf("You are in the heart of a large goods distribution centre.  You can see all~");
            printf("about you evidence of traitorous secret society Christmas celebration; rubber~");
            printf("faced robots whiz back and forth selling toys to holiday shoppers, simul-plast~");
            printf("wreaths hang from every light fixture, while ahead in the shadows is a citizen~");
            printf("wearing a huge red synthetic flower.~");                       
            page22();
        }


        void page15()
        {
            
            nl();
            printf("You are set upon by a runty robot with a queer looking face and two pointy~");
            printf("rubber ears poking from beneath a tattered cap.  \"Hey mister,\" it says,~");
            printf("\"you done all your last minute Christmas shopping?  I got some real neat junk~");
            printf("here.  You don't wanna miss the big day tommorrow, if you know what I mean.\"~");
            printf("The robot opens its bag to show you a pile of shoddy Troubleshooter dolls.  It~");
            printf("reaches in and pulls out one of them.  \"Look, these Action Troubleshooter(tm)~");
            printf("dolls are the neatest thing.  This one's got moveable arms and when you~");
            printf("squeeze him, his little rifle squirts realistic looking napalm.  It's only~");
            printf("50 credits.  Oh yeah, Merry Christmas.\"~");
            addChoice(16, "You decide to buy the doll.", page16);
            addChoice(17, "You shoot the robot.", page17);
            addChoice(22, "You ignore the robot and keep searching the hall.", page22);        
        }

        void page16()
        {
            nl();
            printf("The doll is a good buy for fifty credits; it will make a fine Christmas present~");
            printf("for one of your friends.  After the sale the robot rolls away.  You can use~");
            printf("the doll later in combat.  It works just like a cone rifle firing napalm,~");
            printf("except that occasionally it will explode and blow the user to smithereens.~");
            printf("But don't let that stop you.~");
            action_doll = 1;
            page22();
        }
        
        void page17()
        {
            nl();
            int i, robot_hp = 15;

            printf("You whip out your laser and shoot the robot, but not before it squeezes the~");
            printf("toy at you.  The squeeze toy has the same effect as a cone rifle firing napalm,~");
            printf("and the elfbot's armour has no effect against your laser.~");
            for (i = 0; i < 2; i++)
            {
                if (dice_roll(1, 100) <= 25)
                {
                    printf("You have been hit!~");
                    hit_points -= dice_roll(1, 10);
                    if (hit_points <= 0)
                        if (new_clone())
                        {
                            page45();
                        }
                        return;
                }
                else
                    printf("It missed you, but not by much!~");
                if (dice_roll(1, 100) <= 40)
                {
                    printf("You zapped the little bastard!~");
                    robot_hp -= dice_roll(2, 10);
                    if (robot_hp <= 0)
                    {
                        printf("You wasted it! Good shooting!~");
                        printf("You will need more evidence, so you search GDH7-beta further~");
                        if (hit_points < 10)
                            printf("after the GDH medbot has patched you up.~");
                        hit_points = 10;
                        page22();
                        return;
                    }
                }
                else
                    printf("Damn! You missed!~");
            };
            printf("It tried to fire again, but the toy exploded and demolished it.~");
            printf("You will need more evidence, so you search GDH7-beta further~");
            if (hit_points < 10)
                printf("after the GDH medbot has patched you up.~");
            hit_points = 10;
            page22();
        }

        void page18()
        {
            nl();
            printf("You walk to the centre of the hall, ogling like an infrared fresh from the~");
            printf("clone vats.  Towering before you is the most unearthly thing you have ever~");
            printf("seen, a green multi armed mutant horror hulking 15 feet above your head.~");
            printf("Its skeletal body is draped with hundreds of metallic strips (probably to~");
            printf("negate the effects of some insidious mutant power), and the entire hideous~");
            printf("creature is wrapped in a thousand blinking hazard lights.  It's times like~");
            printf("this when you wish you'd had some training for this job.  Luckily the~");
            printf("creature doesn't take notice of you but stands unmoving, as though waiting for~");
            printf("a summons from its dark lord, the Master Retailer.~");
            printf("WHAM, suddenly you are struck from behind.~");
            if (dice_roll(2, 10) < AGILITY)
            {
                page19(); return;
            }
            else
                page20();
        }

        void page19()
        {
            nl();
            printf("Quickly you regain your balance, whirl and fire your laser into the Ultraviolet~");
            printf("citizen behind you.  For a moment your heart leaps to your throat, then you~");
            printf("realise that he is indeed dead and you will be the only one filing a report on~");
            printf("this incident.  Besides, he was participating in this traitorous Christmas~");
            printf("shopping, as is evident from the rain of shoddy toys falling all around you.~");
            printf("Another valorous deed done in the service of The Computer!~");
            if (++killer_count > (MAXKILL - clone))
            {
                page21();
                return;
            }
            if (read_letter == 1)
            {
                page22();
                return;
            }
            choose(34, "You search the body, keeping an eye open for Internal Security", page34, 22, "You run away like the cowardly dog you are", page22);
        }

        void page20()
        {
            nl();
            printf("Oh no! you can't keep your balance.  You're falling, falling head first into~");
            printf("the Christmas beast's gaping maw.  It's a valiant struggle; you think you are~");
            printf("gone when its poisonous needles dig into your flesh, but with a heroic effort~");
            printf("you jerk a string of lights free and jam the live wires into the creature's~");
            printf("spine.  The Christmas beast topples to the ground and begins to burn, filling~");
            printf("the area with a thick acrid smoke.  It takes only a moment to compose yourself,~");
            printf("and then you are ready to continue your search for the Master Retailer.~");
            page22();
        }

        void page21()
        {
            nl();
            printf("You have been wasting the leading citizens of Alpha Complex at a prodigious~");
            printf("rate.  This has not gone unnoticed by the Internal Security squad at GDH7-beta.~");
            printf("Suddenly, a net of laser beams spear out of the gloomy corners of the hall,~");
            printf("chopping you into teeny, weeny bite size pieces.~");
            if (new_clone())
            {
                page45();
            }            
        }
        

        void page22()
        {
            nl();
            printf("You are searching Goods Distribution Hall 7-beta.~");
            switch (dice_roll(1, 4))
            {
                case 1:
                    page18();return;        
                case 2:
                    page15(); return;         
                case 3:
                    page18(); return;                
                case 4:
                    page29(); return;                    
            }
        }


        void page23()
        {
            nl();
            printf("You go to the nearest computer terminal and declare yourself a mutant.~");
            printf("\"A mutant, he's a mutant,\" yells a previously unnoticed infrared who had~");
            printf("been looking over your shoulder.  You easily gun him down, but not before a~");
            printf("dozen more citizens take notice and aim their weapons at you.~");
            choose(28, "You tell them that it was really only a bad joke", page28, 24, "You want to fight it out, one against twelve", page24);
        }

        void page24()
        {
            nl();
            printf("Golly, I never expected someone to pick this.  I haven't even designed~");
            printf("the 12 citizens who are going to make a sponge out of you.  Tell you what,~");
            printf("I'll give you a second chance.~");
            choose(28, "You change your mind and say it was only a bad joke", page28, 25, "You REALLY want to shoot it out", page25);
        }

        void page25()
        {
            nl();
            printf("Boy, you really can't take a hint!~");
            printf("They're closing in.  Their trigger fingers are twitching, they're about to~");
            printf("shoot.  This is your last chance.~");
            choose(28, "You tell them it was all just a bad joke", page28, 26, "You are going to shoot", page26);
        }

        void page26()
        {
            nl();
            printf("You can read the cold, sober hatred in their eyes (They really didn't think~");
            printf("it was funny), as they tighten the circle around you.  One of them shoves a~");
            printf("blaster up your nose, but that doesn't hurt as much as the multi-gigawatt~");
            printf("carbonium tipped food drill in the small of your back.~");
            printf("You spend the remaining micro-seconds of your life wondering what you did wrong~");
            if (new_clone())
            {
                page32();
            }
        }

        void page27()
        {
            //doesn't exist.  Can't happen with computer version.
            //   designed to catch dice cheats
        }

        void page28()
        {
            nl();
            printf("They don't think it's funny.~");
            page26();
        }

        void page29()
        {
            nl();
            printf("\"Psst, hey citizen, come here.  Pssfft,\" you hear.  When you peer around~");
            printf("you can see someone's dim outline in the shadows.  \"I got some information~");
            printf("on the Master Retailer.  It'll only cost you 30 psst credits.\"~");
            addChoice(30, "You pay the 30 credits for the info.", page30);
            addChoice(31, "You would rather threaten him for the information.", page31);
            addChoice(22, "You ignore him and walk away.", page22);
        }

		void page30()
        {
            nl();
            printf("You step into the shadows and offer the man a thirty credit bill.  \"Just drop~");
            printf("it on the floor,\" he says.  \"So you're looking for the Master Retailer, pssfft?~");
            printf("I've seen him, he's a fat man in a fuzzy red and white jump suit.  They say~");
            printf("he's a high programmer with no respect for proper security.  If you want to~");
            printf("find him then pssfft step behind me and go through the door.\"~");
            printf("Behind the man is a reinforced plasteel blast door.  The centre of it has been~");
            printf("buckled toward you in a manner you only saw once before when you were field~");
            printf("testing the rocket assist plasma slingshot (you found it easily portable but~");
            printf("prone to misfire).  Luckily it isn't buckled too far for you to make out the~");
            printf("warning sign.  WARNING!! Don't open this door or the same thing will happen to~");
            printf("you.  Opening this door is a capital offense.  Do not do it.  Not at all. This~");
            printf("is not a joke.~");
            addChoice(56, "You use your Precognition mutant power on opening the door.", page56);
            addChoice(33, "You just go through the door anyway.", page33);
            addChoice(22, "You decide it's too dangerous and walk away.", page22);
        }

        void page31()
        {
            nl();
            printf("Like any good troubleshooter you make the least expensive decision and threaten~");
            printf("him for information.  With lightning like reflexes you whip out your laser and~");
            printf("stick it up his nose.  \"Talk, you traitorous Christmas celebrator, or who nose~");
            printf("what will happen to you, yuk yuk,\" you pun menacingly, and then you notice~");
            printf("something is very wrong.  He doesn't have a nose.  As a matter of fact he's~");
            printf("made of one eighth inch cardboard and your laser is sticking through the other~");
            printf("side of his head.  \"Are you going to pay?\" says his mouth speaker,~");
            printf("\"or are you going to pssfft go away stupid?\"~");
            choose(30, "You pay the 30 credits", page30, 22, "You pssfft go away stupid", page22);
        }

        void page32()
        {
            nl();
            printf("Finally it's your big chance to prove that you're as good a troubleshooter~");
            printf("as your previous clone.  You walk briskly to mission briefing and pick up your~");
            printf("previous clone's personal effects and notepad.  After reviewing the notes you~");
            printf("know what has to be done.  You catch the purple line to Goods Distribution Hall~");
            printf("7-beta and begin to search for the blast door.~");
            page22();
        }

        void page33()
        {
            blast_door = 1;
            nl();
            printf("You release the megabolts on the blast door, then strain against it with your~");
            printf("awesome strength.  Slowly the door creaks open.  You bravely leap through the~");
            printf("opening and smack your head into the barrel of a 300 mm 'ultra shock' class~");
            printf("plasma cannon.  It's dark in the barrel now, but just before your head got~");
            printf("stuck you can remember seeing a group of technicians anxiously watch you leap~");
            printf("into the room.~");
            if (ultra_violet == 1)
                page35();
            else
                page36();
        }

        void page34()
        {
            nl();
            printf("You have found a sealed envelope on the body.  You open it and read:~");
            printf("\"WARNING: Ultraviolet Clearance ONLY.  DO NOT READ.~");
            printf("Memo from Chico-U-MRX4 to Harpo-U-MRX5.~");
            printf("The planned takeover of the Troubleshooter Training Course goes well, Comrade.~");
            printf("Once we have trained the unwitting bourgeois troubleshooters to work as~");
            printf("communist dupes, the overthrow of Alpha Complex will be unstoppable.  My survey~");
            printf("of the complex has convinced me that no one suspects a thing; soon it will be~");
            printf("too late for them to oppose the revolution.  The only thing that could possibly~");
            printf("impede the people's revolution would be someone alerting The Computer to our~");
            printf("plans (for instance, some enterprising Troubleshooter could tell The Computer~");
            printf("that the communists have liberated the Troubleshooter Training Course and plan~");
            printf("to use it as a jumping off point from which to undermine the stability of all~");
            printf("Alpha Complex), but as we both know, the capitalistic Troubleshooters would~");
            printf("never serve the interests of the proletariat above their own bourgeois desires.~");
            printf("P.S. I'm doing some Christmas shopping later today.  Would you like me to pick~");
            printf("you up something?\"~");
            more();
            printf("When you put down the memo you are overcome by that strange deja'vu again.~");
            printf("You see yourself talking privately with The Computer.  You are telling it all~");
            printf("about the communists' plan, and then the scene shifts and you see yourself~");
            printf("showered with awards for foiling the insidious communist plot to take over the~");
            printf("complex.~");
            read_letter = 1;
            choose(46, "You rush off to the nearest computer terminal to expose the commies", page46, 22, "You wander off to look for more evidence", page22);
        }

void page35()
        {
            nl();
            printf("\"Oh master,\" you hear through the gun barrel, \"where have you been? It is~");
            printf("time for the great Christmas gifting ceremony.  You had better hurry and get~");
            printf("the costume on or the trainee may begin to suspect.\"  For the second time~");
            printf("today you are forced to wear attire not of your own choosing.  They zip the~");
            printf("suit to your chin just as you hear gunfire erupt behind you.~");
            printf("\"Oh no! Who left the door open?  The commies will get in.  Quick, fire the~");
            printf("laser cannon or we're all doomed.\"~");
            printf("\"Too late you capitalist swine, the people's revolutionary strike force claims~");
            printf("this cannon for the proletariat's valiant struggle against oppression.  Take~");
            printf("that, you running dog imperialist lackey.  ZAP, KAPOW\"~");
            printf("Just when you think that things couldn't get worse, \"Aha, look what we have~");
            printf("here, the Master Retailer himself with his head caught in his own cannon.  His~");
            printf("death will serve as a symbol of freedom for all Alpha Complex.~");
            printf("Fire the cannon.\"~");
            if(new_clone())
            {
                page32();
            }            
        }

        void page36()
        {
            nl();
            printf("\"Congratulations, troubleshooter, you have successfully found the lair of the~");
            printf("Master Retailer and completed the Troubleshooter Training Course test mission,\"~");
            printf("a muffled voice tells you through the barrel.  \"Once we dislodge your head~");
            printf("from the barrel of the 'Ultra Shock' plasma cannon you can begin with the~");
            printf("training seminars, the first of which will concern the 100%% accurate~");
            printf("identification and elimination of unregistered mutants.  If you have any~");
            printf("objections please voice them now.\"~");
            addChoice(36, "You appreciate his courtesy and voice an objection.", voiceObjection);
            addChoice(23, "After your head is removed from the cannon, you register as a mutant.", page23);
            addChoice(37, "After your head is removed from the cannon, you go to the unregistered\n    mutant identification and elimination seminar.", page37);            
        }

        void voiceObjection()
        {
            nl();
            if (new_clone())
            {
                page32();
            }
        }

        void page37()
        {
            nl();
            printf("\"Come with me please, Troubleshooter,\" says the Green clearance technician~");
            printf("after he has dislodged your head from the cannon.  \"You have been participating~");
            printf("in the Troubleshooter Training Course since you got off the tube car in~");
            printf("GDH7-beta,\" he explains as he leads you down a corridor.  \"The entire~");
            printf("Christmas assignment was a test mission to assess your current level of~");
            printf("training.  You didn't do so well.  We're going to start at the beginning with~");
            printf("the other student.  Ah, here we are, the mutant identification and elimination~");
            printf("lecture.\"  He shows you into a vast lecture hall filled with empty seats.~");
            printf("There is only one other student here, a Troubleshooter near the front row~");
            printf("playing with his Action Troubleshooter(tm) figure.  \"Find a seat and I will~");
            printf("begin,\" says the instructor.~");
            page38();
        }

void page38()
        {
            nl();
            printf(string.Format("\"I am Plato-B-PHI{0}, head of mutant propaganda here at the training course.~", plato_clone));
            printf("If you have any questions about mutants please come to me.  Today I will be~");
            printf("talking about mutant detection.  Detecting mutants is very easy.  One simply~");
            printf("watches for certain tell tale signs, such as the green scaly skin, the third~");
            printf("arm growing from the forehead, or other similar disfigurements so common with~");
            printf("their kind.  There are, however, a few rare specimens that show no outward sign~");
            printf("of their treason.  This has been a significant problem, so our researchers have~");
            printf("been working on a solution.  I would like a volunteer to test this device,\"~");
            printf("he says, holding up a ray gun looking thing.  \"It is a mutant detection ray.~");
            printf("This little button detects for mutants, and this big button stuns them once~");
            printf("they are discovered.  Who would like to volunteer for a test?\"~");
            printf("The Troubleshooter down the front squirms deeper into his chair.~");
            choose(39, "You volunteer for the test", page39, 40, "You duck behind a chair and hope the instructor doesn't notice you", page40);
        }

void page39()
        {
            nl();
            printf("You bravely volunteer to test the mutant detection gun.  You stand up and walk~");
            printf("down the steps to the podium, passing a very relieved Troubleshooter along the~");
            printf("way.  When you reach the podium Plato-B-PHI hands you the mutant detection gun~");
            printf("and says, \"Here, aim the gun at that Troubleshooter and push the small button.~");
            printf("If you see a purple light, stun him.\"  Grasping the opportunity to prove your~");
            printf("worth to The Computer, you fire the mutant detection ray at the Troubleshooter.~");
            printf("A brilliant purple nimbus instantly surrounds his body.  You slip your finger~");
            printf("to the large stun button and he falls writhing to the floor.~");
            printf("\"Good shot,\" says the instructor as you hand him the mutant detection gun,~");
            printf("\"I'll see that you get a commendation for this.  It seems you have the hang~");
            printf("of mutant detection and elimination.  You can go on to the secret society~");
            printf("infiltration class.  I'll see that the little mutie gets packaged for~");
            printf("tomorrow's mutant dissection class.\"~");
            page41();
        }

void page40()
        {
            nl();
            printf("You breathe a sigh of relief as Plato-B-PHI picks on the other Troubleshooter.~");
            printf("\"You down here in the front,\" says the instructor pointing at the other~");
            printf("Troubleshooter, \"you'll make a good volunteer.  Please step forward.\"~");
            printf("The Troubleshooter looks around with a 'who me?' expression on his face, but~");
            printf("since he is the only one visible in the audience he figures his number is up.~");
            printf("He walks down to the podium clutching his Action Troubleshooter(tm) doll before~");
            printf("him like a weapon.  \"Here,\" says Plato-B-PHI, \"take the mutant detection ray~");
            printf("and point it at the audience.  If there are any mutants out there we'll know~");
            printf("soon enough.\"  Suddenly your skin prickles with static electricity as a bright~");
            printf("purple nimbus surrounds your body.  \"Ha Ha, got one,\" says the instructor.~");
            printf("\"Stun him before he gets away.\"~");
            more();
            while (true)
            {
                if (dice_roll(1, 100) <= 30)
                {
                    printf("His shot hits you.  You feel numb all over.~");
                    page49();
                    return;
                }
                else
                    printf("His shot just missed.~");

                if (dice_roll(1, 100) <= 40)
                {
                    printf("You just blew his head off.  His lifeless hand drops the mutant detector ray.~");
                    page50();
                    return;
                }
                else
                    printf("You burnt a hole in the podium.  He sights the mutant detector ray on you.~");
            }
        }

        void page41()
        {
            nl();
            printf("You stumble down the hallway of the Troubleshooter Training Course looking for~");
            printf("your next class.  Up ahead you see one of the instructors waving to you.  When~");
            printf("you get there he shakes your hand and says, \"I am Jung-I-PSY.  Welcome to the~");
            printf("secret society infiltration seminar.  I hope you ...\"  You don't catch the~");
            printf("rest of his greeting because you're paying too much attention to his handshake;~");
            printf("it is the strangest thing that has ever been done to your hand, sort of how it~");
            printf("would feel if you put a neuro whip in a high energy palm massage unit.~");
            printf("It doesn't take you long to learn what he is up to; you feel him briefly shake~");
            printf("your hand with the secret Illuminati handshake.~");
            choose(42, "You respond with the proper Illuminati code phrase, \"Ewige Blumenkraft\"", page42, 43, "You ignore this secret society contact", page43);
        }

        void page42()
        {
            nl();
            printf("\"Aha, so you are a member of the elitist Illuminati secret society,\" he says~");
            printf("loudly, \"that is most interesting.\"  He turns to the large class already~");
            printf("seated in the auditorium and says, \"You see, class, by simply using the correct~");
            printf("hand shake you can identify the member of any secret society.  Please keep your~");
            printf("weapons trained on him while I call a guard.~");
            choose(51, "You run for it", page51, 52, "You wait for the guard", page52);
        }

void page43()
        {
            nl();
            printf("You sit through a long lecture on how to recognise and infiltrate secret~");
            printf("societies, with an emphasis on mimicking secret handshakes.  The basic theory,~");
            printf("which you realise to be sound from your Iluminati training, is that with the~");
            printf("proper handshake you can pass unnoticed in any secret society gathering.~");
            printf("What's more, the proper handshake will open doors faster than an 'ultra shock'~");
            printf("plasma cannon.  You are certain that with the information you learn here you~");
            printf("will easily be promoted to the next level of your Illuminati secret society.~");
            printf("The lecture continues for three hours, during which you have the opportunity~");
            printf("to practice many different handshakes.  Afterwards everyone is directed to~");
            printf("attend the graduation ceremony.  Before you must go you have a little time to~");
            printf("talk to The Computer about, you know, certain topics.~");
            choose(44, "You go looking for a computer terminal", page44, 55, "You go to the graduation ceremony immediately", page55);
        }

        void page44()
        {
            nl();
            printf("You walk down to a semi-secluded part of the training course complex and~");
            printf("activate a computer terminal.  \"AT YOUR SERVICE\" reads the computer screen.~");
            if (read_letter == 0)
            {
                choose(23, "You register yourself as a mutant", page23, 55, "You change your mind and go to the graduation ceremony", page55);
            }
            else {
                addChoice(23, "You register yourself as a mutant.", page23);
                addChoice(46, "You want to chat about the commies.", page46);
                addChoice(55, "You change your mind and go to the graduation ceremony.", page55);
            }
        }

        void page45()
        {
            nl();
            printf("\"Hrank Hrank,\" snorts the alarm in your living quarters.  Something is up.~");
            printf("You look at the monitor above the bathroom mirror and see the message you have~");
            printf("been waiting for all these years.  \"ATTENTION TROUBLESHOOTER, YOU ARE BEING~");
            printf("ACTIVATED. PLEASE REPORT IMMEDIATELY TO MISSION ASSIGNMENT ROOM A17/GAMMA/LB22.~");
            printf("THANK YOU. THE COMPUTER IS YOUR FRIEND.\"  When you arrive at mission~");
            printf("assignment room A17-gamma/LB22 you are given your previous clone's~");
            printf("remaining possessions and notebook.  You puzzle through your predecessor's~");
            printf("cryptic notes, managing to decipher enough to lead you to the tube station and~");
            printf("the tube car to GDH7-beta.~");
            page10();
        }

        void page46()
        {
            nl();
            printf("\"Why do you ask about the communists, Troubleshooter?  It is not in the~");
            printf("interest of your continued survival to be asking about such topics,\" says~");
            printf("The Computer.~");
            choose(53, "You insist on talking about the communists", page53, 54, "You change the subject", page54);
        }

        void page47()
        {
            nl();
            printf("The Computer orders the entire Vulture squadron to terminate the Troubleshooter~");
            printf("Training Course.  Unfortunately you too are terminated for possessing~");
            printf("classified information.~~");
            printf("Don't act so innocent, we both know that you are an Illuminatus which is in~");
            printf("itself an act of treason.~~");
            printf("Don't look to me for sympathy.~~");
            printf("			THE END~");            
        }

        void page48()
        {
            nl();
            printf("The tubecar shoots forward as you enter, slamming you back into a pile of~");
            printf("garbage.  The front end rotates upward and you, the garbage and the garbage~");
            printf("disposal car shoot straight up out of Alpha Complex.  One of the last things~");
            printf("you see is a small blue sphere slowly dwindling behind you.  After you fail to~");
            printf("report in, you will be assumed dead.~");
            if (new_clone())
            {
                page45();
            }
        }

        void page49()
        {
            nl();
            printf("The instructor drags your inert body into a specimen detainment cage.~");
            printf("\"He'll make a good subject for tomorrow's mutant dissection class,\" you hear.~");
            if (new_clone())
            {
                page32();
            }
        }

        void page50()
        {
            nl();
            printf("You put down the other Troubleshooter, and then wisely decide to drill a few~");
            printf("holes in the instructor as well; the only good witness is a dead witness.~");
            printf("You continue with the training course.~");
            plato_clone++;
            page41();
        }

        void page51()
        {
            nl();
            printf("You run for it, but you don't run far.  Three hundred strange and exotic~");
            printf("weapons turn you into a freeze dried cloud of soot.~");
            if (new_clone())
            {
                page32();
            }
        }

        void page52()
        {
            nl();
            printf("You wisely wait until the instructor returns with a Blue Internal Security~");
            printf("guard.  The guard leads you to an Internal Security self incrimination station.~");
            page2();
        }

        void page53()
        {
            nl();
            printf("You tell The Computer about:~");
            choose(47, "The commies who have infiltrated the Troubleshooter Training Course and the impending Peoples Revolution", page47, 54, "Something less dangerous", page54);
        }

        void page54()
        {
            nl();
            printf("\"Do not try to change the subject, Troubleshooter,\" says The Computer.~");
            printf("\"It is a serious crime to ask about the communists.  You will be terminated~");
            printf("immediately.  Thank you for your inquiry.  The Computer is your friend.\"~");
            printf("Steel bars drop to your left and right, trapping you here in the hallway.~");
            printf("A spotlight beams from the computer console to brilliantly iiluminate you while~");
            printf("the speaker above your head rapidly repeats \"Traitor, Traitor, Traitor.\"~");
            printf("It doesn't take long for a few guards to notice your predicament and come to~");
            printf("finish you off.~");
            if (blast_door == 0)
            {
                if (new_clone())
                {
                    page45();
                    return;
                }
            }

            else
            {
              if(new_clone())
                {
                    page32();
                    return;
                }
            }
        }

        void page55()
        {
            nl();
            printf("You and 300 other excited graduates are marched  from the lecture hall and into~");
            printf("a large auditorium for the graduation exercise.  The auditorium is~");
            printf("extravagantly decorated in the colours of the graduating class.  Great red and~");
            printf("green plasti-paper ribbons drape from the walls, while a huge sign reading~");
            printf("\"Congratulations class of GDH7-beta-203.44/A\" hangs from the raised stage down~");
            printf("front.  Once everyone finds a seat the ceremony begins.  Jung-I-PSY is the~");
            printf("first to speak, \"Congratulations students, you have successfully survived the~");
            printf("Troubleshooter Training Course.  It always brings me great pride to address~");
            printf("the graduating class, for I know, as I am sure you do too, that you are now~");
            printf("qualified for the most perilous missions The Computer may select for you.  The~");
            printf("thanks is not owed to us of the teaching staff, but to all of you, who have~");
            printf("persevered and graduated.  Good luck and die trying.\"  Then the instructor~");
            printf("begins reading the names of the students who one by one walk to the front of~");
            printf("the auditorium and receive their diplomas.  Soon it is your turn,~");
            printf("\"Philo-R-DMD, graduating a master of mutant identification and secret society~");
            printf(string.Format("infiltration.\"  You walk up and receive your diploma from Plato-B-PHI{0}, then~", plato_clone));
            printf("return to your seat.  There is another speech after the diplomas are handed~");
            printf("out, but it is cut short by by rapid fire laser bursts from the high spirited~");
            printf("graduating class.  You are free to return to your barracks to wait, trained~");
            printf("and fully qualified, for your next mission.  You also get that cherished~");
            printf("promotion from the Illuminati secret society.  In a week you receive a~");
            printf("detailed Training Course bill totalling 1,523 credits.~");
            printf("			THE END~");
        }

        void page56()
        {
            nl();
            printf("That familiar strange feeling of deja'vu envelops you again.  It is hard to~");
            printf("say, but whatever is on the other side of the door does not seem to be intended~");
            printf("for you.~");
            choose(33, "You open the door and step through", page33, 22, "You go looking for more information", page22);
        }
        
        void page57()
        {
            nl();
            printf("In the centre of the room is a table and a single chair.  There is an Orange~");
            printf("folder on the table top, but you can't make out the lettering on it.~");
            choose(11, "You sit down and read the folder", page11, 12, "You leave the room", page12);
        }

        #endregion

        private void choose(int p1, string d1, ParanoiaAction a1, int p2, string d2, ParanoiaAction a2)
        {
            addChoice(p1, d1, a1);
            addChoice(p2, d2, a2);
        }

        private void addChoice(int page, string desc, ParanoiaAction action)
        {
            _choices.Add(new ParanoiaChoice { Page = page, Description = desc, Action = action });
        }

        /// <summary>
        /// This is a port. I turned all the newline characters into '~', but since
        /// each line becomes its own string in the response array, the last ~
        /// is redundant. Some code has been added to remove that extra new line rather
        /// that manually remove each one from the code. 
        /// </summary>
        /// <param name="s">The string to parse.</param>
        private void printf(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
            char[] stringChars = s.ToCharArray();
            string ls = s;
            if (stringChars[stringChars.Length - 1] == '~')
            {
                Array.Resize<char>(ref stringChars, stringChars.Length - 1);
                ls = new string(stringChars);
            }

            string[] lines = ls.Split("~".ToCharArray());
            foreach (string l in lines)
            {
                _lineBuffer.Add(l);
            }
        }

        /// <summary>
        /// add a new line to the output buffer
        /// </summary>
        private void nl()
        {
            _lineBuffer.Add(" ");
        }

        private void more()
        {
            _lineBuffer.Add(MORE_STRING);
        }
    }

    public class ParanoiaResponse
    {
        public string[] TextLines;

        public ParanoiaChoice[] Choices;
        /// <summary>
        /// Used when the page chooses the next page automatically.
        /// </summary>
        public int GoTo;
    }

    public delegate void ParanoiaAction();

    public class ParanoiaChoice
    {
        public int Page;
        public string Description;
        public ParanoiaAction Action;
    }    
}
