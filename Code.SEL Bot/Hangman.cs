using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Code.SEL_Bot
{
    public class Hangman
    {
        public Discord.User Starter { get; set; } = null;
        public Discord.User Solver { get; set; } = null;
        public Discord.Message Hang { get; set; } = null;
        public Discord.Message Mess { get; set; } = null;
        public Discord.Channel Chan { get; set; } = null;
        public bool Finished { get; set; } = false;
        public List<bool> Reveal { get; set; } = new List<bool>();
        public string Word { get; set; } = "";
        public string Description { get; set; } = "";
        public string Letters { get; set; } = "";
        public int TotalLetters { get; set; } = 0;
        public int Stage { get; set; } = 0;
        public int Help { get; set; } = 0;
        public int ID { get; set; } = 0;
        private char Win = 'O';
        private string LettersSinceLastEdit = "~";
        private string Line = "~~                                                                                                                                                                            ~~\n";

        public void Continue(int Id, Discord.Channel C)
        {
            Chan = C; ID = Id;
            string[] St = File.ReadAllLines("../../Hang_Data.txt");
            Win = Fcn.Msg.Between(St[ID], "$Win\"", "\" ")[0];
            Stage = int.Parse(Fcn.Msg.Between(St[ID], "$Stage\"", "\" "));
            Help = int.Parse(Fcn.Msg.Between(St[ID], "$Helps\"", "\" "));
            Description = Fcn.Msg.Between(St[ID], "$Description\"", "\" ");
            Letters = Fcn.Msg.Between(St[ID], "$Letters\"", "\" ");
            Starter = C.Server.FindUsers(Fcn.Msg.Between(St[ID], "$Starter\"", "\" ")).FirstOrDefault();
            Solver = C.Server.FindUsers(Fcn.Msg.Between(St[ID], "$Solver\"", "\" ")).FirstOrDefault();
            Word = St[ID].Substring(St[ID].IndexOf("$Word:") + 6);

            foreach (char c in Word)
            {
                if (char.IsLetter(c))
                {
                    Reveal.Add(false);
                }
                else
                {
                    Reveal.Add(true);
                }
            }

            foreach (char c in Letters)
            {
                if (Word.Contains(c))
                {
                    for (int i = 0; i < Word.Length; i++)
                    {
                        if (Word[i] == c)
                        {
                            Reveal[i] = true;
                        }
                    }
                }
            }
        }

        public Hangman(Discord.User P, Discord.Channel C, string W)
        {
            if (P == null || C == null || W == null)
            {
                return;
            }
            W = W.Substring(0, Math.Min(1500, W.Length));
            Chan = C; Starter = P; Word = W.ToLower();
            foreach (char c in Word)
            {
                if (char.IsLetter(c))
                {
                    Reveal.Add(false);
                }
                else
                {
                    Reveal.Add(true);
                }
            }

            string[] Sts = File.ReadAllLines("../../Data.txt"), St = File.ReadAllLines("../../Hang_Data.txt");
            ID = Convert.ToInt32(Sts[1].Substring(13)) + 1;
            Array.Resize(ref St, ID + 1);
            St[ID] = $"Match[{Inttodigits(ID, 4)}] 1 Win(0) Letters(00) Stage(0) Helps({Help}) #! Word: {Word} Description: {Description}";
            Sts[1] = "Hangman_ID = " + ID;
            File.WriteAllLines("../../Data.txt", Sts);
            File.WriteAllLines("../../Hang_Data.txt", St);
        }

        public async void Start(bool Continue = false)
        {
            string alpha = "qwertyuiopasdfghjklzxcvbnm";
            foreach (char c in alpha)
            {
                if (Word.Contains(c))
                {
                    TotalLetters++;
                }
            }

            try
            {
                if (Description != "")
                {
                    if (Continue && !Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman originally started by {Starter.Mention} was continued with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`\n`Description: {Description}`");
                    else if (Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman originally started by {Starter.Mention} with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`\n`Description: {Description}`");
                    else
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman started by {Starter.Mention} with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`\n`Description: {Description}`");
                }
                else
                {
                    if (Continue && !Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman originally started by {Starter.Mention} was continued with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`");
                    else if (Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman originally started by {Starter.Mention} with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`");
                    else
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman started by {Starter.Mention} with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`");
                }
            }
            catch (NullReferenceException)
            {
                if (Description != "")
                {
                    if (Continue && !Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman was continued with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`\n`Description: {Description}`");
                    else if (Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`\n`Description: {Description}`");
                    else
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman started  with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`\n`Description: {Description}`");
                }
                else
                {
                    if (Continue && !Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman was continued with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`");
                    else if (Finished)
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`");
                    else
                        await Chan.SendMessage($"{Line}<:Hangman:285779439604727809> Hangman started with {CountWords()} word(s) and a total of {CountLetters()} letters `Game ID: {ID}`");
                }
            }
            Timer T2 = new Timer(850);
            T2.Elapsed += (s, ev) => { if (Finished) T2.Dispose(); };
            T2.Elapsed += T_ElapsedAsync;
            T2.Start();
        }

        private async void T_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            if (Letters != LettersSinceLastEdit)
            {
                LettersSinceLastEdit = Letters;
                if(Hang == null || Mess == null)
                {
                    Hang = await Chan.SendMessage(GetStageLink());
                    Mess = await Chan.SendMessage(RenderWord());
                }
                else
                {
                    if(Hang.RawText != GetStageLink())
                        await Hang.Edit(GetStageLink());
                    await Mess.Edit(RenderWord());
                }
                if (Finished)
                {
                    try
                    {
                        switch (Stage)
                        {
                            case 0: { await Chan.SendMessage($":balloon: {Solver.Mention} Found the Word in a Perfect Match! :confetti_ball: No Errors :tada:"); break; }
                            case 1: { await Chan.SendMessage($":balloon: {Solver.Mention} Found the Word with one tiny Error :tada:"); break; }
                            case 2: { await Chan.SendMessage($":balloon: {Solver.Mention} Got the Word having only 2 wrong moves! :tada:"); break; }
                            case 3: { await Chan.SendMessage($"{Solver.Mention} Found the Word with only 3 bad moves :tada:"); break; }
                            case 4: { await Chan.SendMessage($"{Solver.Mention} Finally found the Word with 4 wrong moves :tada:"); break; }
                            case 5: { await Chan.SendMessage($"{Solver.Mention} Finally found the Word with a whole 5 bad moves :tada:"); break; }
                            case 6: { await Chan.SendMessage($"{Solver.Mention} Actually Found the Word on his last Breath :skull_crossbones:"); break; }
                            case 7: { await Chan.SendMessage($"{Solver.Mention} Made the Final Wrong move and Lost the Hangman <:Hangman:285779439604727809>\n{Line}The Word was:  {RenderWord(true)}"); break; }
                            default:
                                break;
                        }
                    }
                    catch(NullReferenceException)
                    {
                        switch (Stage)
                        {
                            case 0: { await Chan.SendMessage($":balloon: Word was found in a Perfect Match! :confetti_ball: No Errors :tada:"); break; }
                            case 1: { await Chan.SendMessage($":balloon: Word was found with one tiny Error :tada:"); break; }
                            case 2: { await Chan.SendMessage($":balloon: Word was found having only 2 wrong moves! :tada:"); break; }
                            case 3: { await Chan.SendMessage($"Word was found with only 3 bad moves :tada:"); break; }
                            case 4: { await Chan.SendMessage($"Word was Finally found with 4 wrong moves :tada:"); break; }
                            case 5: { await Chan.SendMessage($"Word was Finally found with a whole 5 bad moves :tada:"); break; }
                            case 6: { await Chan.SendMessage($"Word was Actually found on its last Breath :skull_crossbones:"); break; }
                            case 7: { await Chan.SendMessage($"The Hangman <:Hangman:285779439604727809> was Lost\n{Line}The Word was:  {RenderWord(true)}"); break; }
                            default:
                                break;
                        }
                    }
                }
                Save();
                
            }
        }

        public bool Play(char c, Discord.User U)
        {
            c = c.ToString().ToLower()[0];
            if (!Letters.Contains(c))
            {
                Letters += c;
                if (Word.Contains(c))
                {
                    Solver = U;
                    for (int i = 0; i < Word.Length; i++)
                    {
                        if (Word[i] == c)
                        {
                            Reveal[i] = true;
                        }
                    }
                    string q = "";
                    foreach (char y in Letters)
                    {
                        if (Word.Contains(y))
                        {
                            q += c;
                        }
                    }
                    if (TotalLetters - q.Length == 0)
                    {
                        Win = 'Y';
                        Finished = true;
                        return true;
                    }
                }
                else
                {
                    Stage++; 
                    if(Stage >= 7)
                    {
                        Win = 'N';
                        Finished = true;
                    }

                }
            }
            else
            {
                Fcn.Discord.TimedMsg(Chan.SendMessage(Fcn.Msg.EmojiConverter(c) + " was already used!"), 15000); 
            }
            return false;
        }

        public async void End()
        {
            string[] Data = File.ReadAllLines("../../Data.txt"), HangData = File.ReadAllLines("../../Hang_Data.txt");
            Data[1] = "Hangman_ID = " + (int.Parse(Data[1].Substring(13)) - 1);
            Array.Resize(ref HangData, HangData.Length - 1);
            File.WriteAllLines("../../Data.txt", Data);
            File.WriteAllLines("../../Hang_Data.txt", HangData);
            await Hang.Edit(":information_source: Hangman was Terminated :information_source: ");
            await Mess.Delete();
            Finished = true;
            await Chan.SendMessage(":exclamation: Ongoing hangman game was terminated");
        }

        private string Inttodigits(int i, int d = 2)
        {
            string s = "";
            if (i.ToString().Length < d)
            {
                for (int y = 0; y < d - i.ToString().Length; y++)
                {
                    s = s + "0";
                }
            }

            s = s + i;
            return s;
        }

        private string RenderWord(bool B = false)
        {
            string s = "";
            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == ' ' || !char.IsLetterOrDigit(Word[i]))
                {
                    s = s + " " + Word[i] + " ";
                }
                else if ((Reveal[i] || B))
                {
                    s = s + Fcn.Msg.ToEmoji(Word[i]);
                }
                else
                {
                    s = s + "<:Empty:285770698625122304>";
                }
            }
            if(!B)
                s += "\n```\n🗚  " + ListLetters() + "\n✓  " + ListLetters(true) + "\n✗  " + ListLetters(false) + "```";
            if (s.Length < 1900)
            {
                return s;
            }

            s = "";
            for (int i = 0; i < Word.Length; i++)
            {
                if (Word[i] == ' ' || !char.IsLetter(Word[i]))
                {
                    s = s + " " + Word[i] + " ";
                }
                else if (Reveal[i] || Finished)
                {
                    s += Word[i];
                }
                else
                {
                    s = s + "-";
                }
            }
            if (!B)
                s += "\n```\n🗚  " + ListLetters() + "\n✓  " + ListLetters(true) + "\n✗  " + ListLetters(false) + "```";
            return s.Substring(0, Math.Min(1999, s.Length));
        }
        
        private int CountLetters()
        {
            int i = 0;
            foreach (char c in Word)
            {
                if (char.IsLetter(c))
                {
                    i++;
                }
            }

            return i;
        }

        private string ListLetters()
        {
            string s = "";
            foreach (char c in Letters)
            {
                if (c == Letters.Last())
                {
                    s = s + c;
                }
                else
                {
                    s = s + c + " - ";
                }
            }
            return s;
        }

        private string ListLetters(bool B)
        {
            string s = "";
            foreach (char c in Letters)
            {
                if (Word.Contains(c) == B)
                {
                    s = s + c + " - ";
                }
            }
            if (s.LastIndexOf("-") == s.Length - 2)
            {
                s = s.Substring(0, s.Length - 3);
            }

            return s;
        }

        private int CountWords()
        {
            int c = 1;
            for (int i = 1; i < Word.Length; i++)
            {
                if (Word[i] == ' ' && Word[i - 1] != ' ')
                {
                    c++;
                }
            }

            return c;
        }

        private string GetStageLink()
        {
            if (Stage == 7)
            {
                return "http://imgur.com/VREWUNq.png";
            }

            if (Stage == 6)
            {
                return "http://imgur.com/2Oy29tz.png";
            }

            if (Stage == 5)
            {
                return "http://imgur.com/t2WTwcj.png";
            }

            if (Stage == 4)
            {
                return "http://imgur.com/nFo8S6M.png";
            }

            if (Stage == 3)
            {
                return "http://imgur.com/Y9x4b8q.png";
            }

            if (Stage == 2)
            {
                return "http://imgur.com/qQvmeZD.png";
            }

            if (Stage == 1)
            {
                return "http://imgur.com/PGdcNQQ.png";
            }

            return "http://imgur.com/D7RHXzX.png";
        }

        private void Save()
        {
            string[] Data = File.ReadAllLines("../../Hang_Data.txt");
            try
            {
                Data[ID] = $"Match[{Inttodigits(ID, 4)}] = $Win\"{Win}\" $Stage\"{Stage}\" $Helps\"{Help}\" $Starter\"{Starter.Name.ToString()}\" $Solver\"{Solver.Name.ToString()}\" $Letters\"{Letters}\" $Description\"{Description}\" $Word:{Word}";
            }
            catch (NullReferenceException)
            {
                Data[ID] = $"Match[{Inttodigits(ID, 4)}] = $Win\"{Win}\" $Stage\"{Stage}\" $Helps\"{Help}\" $Starter\"\" $Solver\"\" $Letters\"{Letters}\" $Description\"{Description}\" $Word:{Word}";
            }
            File.WriteAllLines("../../Hang_Data.txt", Data);
        }

    }
}
