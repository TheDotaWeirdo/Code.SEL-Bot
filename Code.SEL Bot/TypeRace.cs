using Discord;
using System;
using System.IO;
using System.Linq;

namespace Code.SEL_Bot
{
    public class TypeRace
    {
        public string Word { get; set; } = "";
        public DateTime StartTime { get; set; }
        public System.Timers.Timer Timer { get; set; }
        public ulong[] User_Ans_ID { get; set; } = { 0 };
        public int[] User_Rate { get; set; } = { 0 };
        public Message Mess { get; set; } = null;
        public int Difficulty { get; set; } = 3;

        public async void Start(MessageEventArgs e)
        {
            if (e.Message.RawText.Contains("#"))
            {
                Difficulty = Convert.ToInt32(e.Message.RawText[e.Message.RawText.IndexOf('#') + 1]) - 48;
            }
            else if (e.Message.RawText.ToLower().Contains("hard"))
            {
                Difficulty = 1;
            }
            else if (e.Message.RawText.ToLower().Contains("easy"))
            {
                Difficulty = 5;
            }

            string[] Data = File.ReadAllLines("../../Data.txt");
            Mess = await e.Channel.SendMessage("<:TypeRace:285808847254454272> TyperRace `Game ID: " + Data[8].Substring(14) + "` Starting in 3.");
            System.Threading.Thread.Sleep(1000);
            await Mess.Edit("<:TypeRace:285808847254454272> TyperRace `Game ID: " + Data[8].Substring(14) + "` Starting in 2..");
            System.Threading.Thread.Sleep(1000);
            await Mess.Edit("<:TypeRace:285808847254454272> TyperRace `Game ID: " + Data[8].Substring(14) + "` Starting in 1...");
            System.Threading.Thread.Sleep(1000);
            Word = Get();
            StartTime = DateTime.Now;
            await Mess.Edit("<:TypeRace:285808847254454272> TyperRace `Game ID: " + Data[8].Substring(14) + "` Time: `0 seconds`\n```" + Word + "```");
            Data[8] = "TypeRace_ID = " + (Convert.ToDouble(Data[8].Substring(14)) + 1).ToString();
            File.WriteAllLines("../../Data.txt", Data);
            Timer = new System.Timers.Timer(1500);
            Timer.Start();
            System.Timers.Timer Timer2 = new System.Timers.Timer(100000);
            Timer2.Start();
            Timer2.Elapsed += (TS, SE) =>
            {
                Timer2.Dispose();
                Word = "";
            };
            Timer.Elapsed += (TS, SE) =>
            {
                Mess.Edit("<:TypeRace:285808847254454272> TyperRace `Game ID: " + (Convert.ToDouble(Data[8].Substring(14)) - 1) + "` Time: `" + ((DateTime.Now - StartTime).TotalSeconds).ToString().Substring(0, ((DateTime.Now - StartTime).TotalSeconds).ToString().IndexOf('.')) + " seconds`\n```" + Word + "```");
            };
            Timer.Disposed += (S, E) =>
            {
                Timer.Stop();
                Mess.Edit("<:TypeRace:285808847254454272> TyperRace `Game ID: " + (Convert.ToDouble(Data[8].Substring(14)) - 1) + "` :hash::one: Time: `" + ((DateTime.Now - StartTime).TotalSeconds).ToString().Substring(0, 5) + " seconds`\n```" + Word + "```");
            };
        }

        public async void Play(Message M)
        {
            string A = M.RawText;
            int Chk = Check(A, Word);
            if (Chk <= Difficulty)
            {
                Console.WriteLine("Command Found: 'TypeAnswer'");
                double Tim = (DateTime.Now - StartTime).TotalMilliseconds;
                if ((A == Word && (A.Length / Tim) > 0.0035) || (A.Length / Tim) > (0.001 + GetDiff()))
                {
                    await M.Delete(); Console.WriteLine(M.User.Name + " was cought cheating with a speed of " + A.Length / Tim + " for " + (0.001 + GetDiff()));
                    Fcn.Discord.TimedMsg(await Mess.Channel.SendMessage(M.User.Mention + ", This is cheating, better luck next time.."));
                }
                else
                {
                    if (!User_Ans_ID.Contains(M.User.Id))
                    {
                        string[] Data = File.ReadAllLines("../../Data.txt");
                        double d = Convert.ToDouble(Data[9].Substring(15)) * Convert.ToDouble(Data[15].Substring(18)),
                                      star = ((Difficulty - Chk) / ((Difficulty - 3) * 0.4 + 1.2)) + (((A.Length / Tim) / (GetDiff())) * 2.5); star = (star - 1) / 4 * 5;
                        Console.WriteLine(M.User.Name + " had a Speed of " + (A.Length / Tim).ToString().Substring(0, 7) + " with Word difficulty: " + GetDiff().ToString().Substring(0, 7) + " with " + Chk + " errors and a rating of " + star.ToString().Substring(0, 4));
                        Data[15] = "TypeRace_Inputs = " + (Convert.ToDouble(Data[15].Substring(18)) + 1).ToString();
                        Data[9] = "TypeRace_Avg = " + ((A.Length / Tim) + d) / Convert.ToDouble(Data[15].Substring(18));
                        if (User_Ans_ID.Length == 1)
                        {
                            await Mess.Channel.SendMessage(M.User.Mention + " Finished the TypeRace :regional_indicator_f::regional_indicator_i::regional_indicator_r::regional_indicator_s::regional_indicator_t:! :tada:\nHis Time was: `" + (DateTime.Now - StartTime).TotalSeconds.ToString().Substring(0, 5) + " seconds` with a rating of  " + GetStars(star));
                        }
                        else
                        {
                            await Mess.Channel.SendMessage(M.User.Mention + " Finished the TypeRace\nHis Time was: `" + (DateTime.Now - StartTime).TotalSeconds.ToString().Substring(0, 5) + " seconds` with a rating of  " + GetStars(star));
                        }

                        Timer.Dispose();
                        ulong[] temp = new ulong[User_Ans_ID.Length + 1];
                        for (int i = 0; i <= User_Ans_ID.Length; i++)
                        {
                            if (i == User_Ans_ID.Length)
                            {
                                temp[i] = M.User.Id;
                            }
                            else
                            {
                                temp[i] = User_Ans_ID[i];
                            }
                        }
                        if (Convert.ToDouble(Data[16].Substring(16)) < (A.Length / Tim))
                        {
                            Data[16] = "TypeRace_Best = " + (A.Length / Tim);
                        }

                        User_Ans_ID = temp;
                        File.WriteAllLines("../../Data.txt", Data);
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await Mess.Channel.SendMessage(M.User.Mention + ", you've already participated in this Typing Race"));
                    }
                }
            }
            else if (!User_Ans_ID.Contains(M.User.Id) && Chk < 10)
            {
                Console.WriteLine("Command Found: 'TypeAnswer'");
                Fcn.Discord.TimedMsg(await Mess.Channel.SendMessage(M.User.Mention + ", you had too many errors in your input `(" + Chk + ")`"));
            }
        }

        public string Get()
        {
            string[] W =
            {
                "Malls are great places to shop; I can find everything I need under one roof.",
                "I will never be this young again. Ever. Oh damn... I just got older.",
                "She advised him to come back at once.",
                "This is a Japanese doll.",
                "Italy is my favorite country; in fact, I plan to spend two weeks there next year.",
                "Sixty-Four comes asking for bread.",
                "She only paints with bold colors; she does not like pastels.",
                "Wednesday is hump day, but has anyone asked the camel if he's happy about it?",
                "She always speaks to him in a loud voice.",
                "We have never been to Asia, nor have we visited Africa.",
                "I am happy to take your donation; any amount will be greatly appreciated.",
                "He told us a very exciting adventure story.",
                "She wrote him a long letter, but he didn't read it.",
                "She was too short to see over the fence.",
                "Rock music approaches at high velocity.",
                "Mary plays the piano.",
                "There was no ice cream in the freezer, nor did they have money to go to the store.",
                "I hear that Nancy is very pretty.",
                "I really want to go to work, but I am too sick to drive.",
                "The mysterious diary records the voice.",
                "I think I will buy the red car, or I will lease the blue one.",
                "Writing a list of random sentences is harder than I initially thought it would be.",
                "The old apple revels in its authority.",
                "He turned in the research paper on Friday; otherwise, he would have not passed the class.",
                "I would have gotten the promotion, but my attendance wasn't good enough.",
                "He said he was not there yesterday; however, many people saw him there.",
                "Yeah, I think it's a good environment for learning English.",
                "Joe made the sugar cookies; Susan decorated them.",
                "Abstraction is often one floor above you.",
                "Where do random thoughts come from?",
                "She borrowed the book from him many years ago and hasn't yet returned it.",
                "I want more detailed information.",
                "We need to rent a room for our party.",
                "It was getting dark, and we weren't there yet.",
                "If I don't like something, I'll stay away from it.",
                "I'd rather be a bird than a fish.",
                "The lake is a long way from here.",
                "The sky is clear; the stars are twinkling.",
                "Don't step on the broken glass.",
                "How was the math test?",
                "He ran out of money, so he had to stop playing poker.",
                "If you like tuna and tomato sauce- try combining the two. It's really not as bad as it sounds.",
                "Last Friday in three week's time I saw a spotted striped blue worm shake hands with a legless lizard.",
                "Wow, does that work?",
                "What was the person thinking when they discovered cow's milk was fine for human consumption... and why did they do it in the first place!?",
                "She did not cheat on the test, for it was not the right thing to do.",
                "She did her best to help him.",
                "He didn't want to go to the dentist, yet he went anyway.",
                "Sometimes it is better to just walk away from things and go back to them later when you're in a better frame of mind.",
                "Cats are good pets, for they are clean and are not noisy.",
                "I was very proud of my nickname throughout high school but today- I couldn't be any different to what my nickname was.",
                "Check back tomorrow; I will see if the book has arrived.",
                "Should we start class now, or should we wait for everyone to get here?",
                "A song can make or ruin a person's day if they let it get to them.",
                "She folded her handkerchief neatly.",
                "The clock within this blog and the clock on my laptop are 1 hour different from each other.",
                "I love eating toasted cheese and tuna sandwiches.",
                "She works two jobs to make ends meet; at least, that was her reason for not having time to join us.",
                "This is the last random sentence I will be writing and I am going to stop mid-sent",
                "I checked to make sure that he was still alive.",
                "We have a lot of rain in June.",
                "When I was little I had a car door slammed shut on my hand. I still remember it quite vividly.",
                "The body may perhaps compensates for the loss of a true metaphysics.",
                "My Mum tries to be cool by saying that she likes all the same things that I do.",
                "Tom got a small piece of pie.",
                "The river stole the gods.",
                "Sometimes, all you need to do is completely make an ass of yourself and laugh it off to realise that life isn't so bad after all.",
                "Two seats were vacant.",
                "The book is in front of the table.",
                "I am never at home on Sundays.",
                "Everyone was busy, so I went to the movie alone.",
                "Is it free?",
                "The waves were crashing on the shore; it was a lovely sight.",
                "A glittering gem is not enough.",
                "The quick brown fox jumps over the lazy dog.",
                "The shooter says goodbye to his love.",
                "Christmas is coming.",
                "I am counting my calories, yet I really want dessert.",
                "Please wait outside of the house.",
                "Lets all be unique together until we realise we are all the same.",
                "The stranger officiates the meal.",
                "I want to buy a onesie... but know it won't suit me.",
                "Let me help you with your baggage.",
                "I often see the time 11:11 or 12:34 on clocks.",
                "The memory we used to share is no longer coherent.",
                "I currently have 4 windows open up... and I don't know why.",
                "They got there early, and they got really good seats.",
                "There were white out conditions in the town; subsequently, the roads were impassable.",
                "If the Easter Bunny and the Tooth Fairy had babies would they take your teeth and leave chocolate for you?",
                "She works two jobs to make ends meet; at least, that was her reason for not having time to join us.",
                "I checked to make sure that he was still alive.",
                "The waves were crashing on the shore; it was a lovely sight.",
                "How was the math test?",
                "Last Friday in three week's time I saw a spotted striped blue worm shake hands with a legless lizard.",
                "We need to rent a room for our party.",
                "The memory we used to share is no longer coherent.",
                "I would have gotten the promotion, but my attendance wasn't good enough.",
                "Sometimes, all you need to do is completely make an ass of yourself and laugh it off to realise that life isn't so bad after all.",
                "I am never at home on Sundays.",
                "The quick brown fox jumps over the lazy dog.",
                "The shooter says goodbye to his love.",
                "She did her best to help him.",
                "Joe made the sugar cookies; Susan decorated them.",
                "If you like tuna and tomato sauce- try combining the two. It's really not as bad as it sounds.",
                "If Purple People Eaters are real... where do they find purple people to eat?",
                "They got there early, and they got really good seats.",
                "I'd rather be a bird than a fish.",
                "I am counting my calories, yet I really want dessert.",
                "If I don't like something, I'll stay away from it.",
                "This is a Japanese doll.",
                "If the Easter Bunny and the Tooth Fairy had babies would they take your teeth and leave chocolate for you?",
                "She folded her handkerchief neatly.",
                "Hurry!",
                "He said he was not there yesterday; however, many people saw him there.",
                "Cats are good pets, for they are clean and are not noisy.",
                "A purple pig and a green donkey flew a kite in the middle of the night and ended up sunburnt.",
                "Italy is my favorite country; in fact, I plan to spend two weeks there next year.",
                "We have a lot of rain in June.",
                "Let me help you with your baggage.",
                "The lake is a long way from here.",
                "I want to buy a onesie... but know it won't suit me.",
                "The clock within this blog and the clock on my laptop are 1 hour different from each other.",
                "Don't step on the broken glass.",
                "The stranger officiates the meal.",
                "She advised him to come back at once.",
                "When I was little I had a car door slammed shut on my hand. I still remember it quite vividly.",
                "Wednesday is hump day, but has anyone asked the camel if he's happy about it?",
                "He didn't want to go to the dentist, yet he went anyway.",
                "Where do random thoughts come from?",
                "I currently have 4 windows open up... and I don't know why.",
                "This is a Japanese doll.",
                "Cats are good pets, for they are clean and are not noisy.",
                "Sometimes, all you need to do is completely make an ass of yourself and laugh it off to realise that life isn't so bad after all.",
                "The stranger officiates the meal.",
                "The body may perhapscompensatesfor the loss of a true metaphysics.",
                "She folded her handkerchief neatly.",
                "Italy is my favorite country; in fact, I plan to spend two weeks there next year.",
                "I would have gotten the promotion, but my attendance wasn't good enough.",
                "She did her best to help him."
            };
            return W[Program.rand.Next(W.Length)];
        }

        public int Check(string S2, string TR2)
        {
            string TR = (TR2.ToLower()), S = (S2.ToLower());
            for (int i = 0; i < S.Length; i++)
            {
                if (TR[0] == S[i])
                {
                    TR = TR.Substring(1);
                }
                else
                {
                    if (TR[1] == S[i] && S[Math.Min(S.Length - 1, i + 1)] == TR[0])
                    { TR = TR.Substring(2) + "~~"; i++; }
                    else if (S[Math.Max(0, i - 1)] == S[i])
                    { TR = TR + "~"; }
                    else if (S[i] == TR[1])
                    { TR = TR.Substring(1) + "~"; i--; }
                    else
                    {
                        TR = TR.Substring(1) + "~";
                    }
                }
            }
            return TR.Length;
        }

        public string GetStars(double d)
        {
            decimal c = decimal.Round(Convert.ToDecimal(d));
            if (c >= 5)
            {
                return ":star::star::star::star::star:";
            }

            if (c == 4)
            {
                return ":star::star::star::star:<:Empty_Star:285469988226334720>";
            }

            if (c == 3)
            {
                return ":star::star::star:<:Empty_Star:285469988226334720><:Empty_Star:285469988226334720>";
            }

            if (c == 2)
            {
                return ":star::star:<:Empty_Star:285469988226334720><:Empty_Star:285469988226334720><:Empty_Star:285469988226334720>";
            }

            if (c == 1)
            {
                return ":star:<:Empty_Star:285469988226334720><:Empty_Star:285469988226334720><:Empty_Star:285469988226334720><:Empty_Star:285469988226334720>";
            }

            return "<:Empty_Star:285469988226334720><:Empty_Star:285469988226334720><:Empty_Star:285469988226334720><:Empty_Star:285469988226334720><:Empty_Star:285469988226334720>";
        }

        public double GetDiff()
        {
            string[] Data = File.ReadAllLines("../../Data.txt");
            return (Convert.ToDouble(Data[16].Substring(16)) + 0.001 - (0.000125 * CountAbnormalLetters()) - (0.0015 - (Word.Length / 45000))) * 0.75 + 0.25 * Convert.ToDouble(Data[9].Substring(15));
        }

        public int CountAbnormalLetters()
        {
            int i = 0;
            foreach (char c in Word)
            {
                if (c != ' ' && !char.IsLetter(c))
                {
                    i++;
                }
            }

            return i;
        }
    }
}
