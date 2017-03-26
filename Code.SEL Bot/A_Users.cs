using Discord;
using System;
using System.Collections.Generic;
using System.IO;

namespace Code.SEL_Bot
{
    public class A_Users
    {
        public User User { get; set; } = null;
        public string GameName { get; set; } = "";
        public bool Moved { get; set; } = false;
        public int MessagesSent { get; set; } = 0;
        public int HangStarted { get; set; } = 0;
        public int HangSolved { get; set; } = 0;
        public int CharSent { get; set; } = 0;
        public List<string> Games { get; set; } = new List<string>();
        public int SongsPlayed { get; set; } = 0;
        public bool AutoMove { get; set; } = true;
        public bool AFKMove { get; set; } = true;
        public bool OnlineSinceSet { get; set; } = false;
        public DateTime LastOnline { get; set; } = new DateTime();
        public DateTime OnlineSince { get; set; } = new DateTime();
        private bool Errord = false;

        public void Update()
        {
            bool b = true;
            try
            {
                string[] Data = File.ReadAllLines("../../User_Data.txt");
                for (int i = 0; i < Data.Length; i++)
                {
                    if (Data[i].Contains($"$ID\"{User.Id}\""))
                    {
                        b = false;
                        if (!IsEmpty())
                        {
                            Data[i] = $"$ID\"{User.Id}\" "
                                + $"$Name\"{User.Name}\" "
                                + $"$Discriminator\"{User.Discriminator}\" "
                                + $"$MessagesSent\"{MessagesSent}\" "
                                + $"$CharSent\"{CharSent}\" "
                                + $"$HangStarted\"{HangStarted}\" "
                                + $"$HangSolved\"{HangSolved}\" "
                                + $"$GamesPlayed\"{GetGames()}\" "
                                + $"$Automove\"{AutoMove.ToString()}\" "
                                + $"$LastOnline\"{LastOnline.ToString()}\" "
                                + $"$OnlineSince\"{OnlineSince.ToString()}\" "
                                + $"$AFKMove\"{AFKMove.ToString()}\" "
                                + $"$SongsPlayed\"{SongsPlayed}\" ;";
                            File.WriteAllLines("../../User_Data.txt", Data);
                            Errord = false;
                        }
                        else if(!Errord)
                        {
                            Errord = true;
                            Console.WriteLine($"Error with User Data for {User.Name}#{User.Discriminator}");
                        }
                    }
                }
                if(b)
                { 
                        Array.Resize(ref Data, Data.Length + 1);
                        Data[Data.Length - 1] = $"$ID\"{User.Id}\" "
                            + $"$Name\"{User.Name}\" "
                            + $"$Discriminator\"{User.Discriminator}\" "
                            + $"$MessagesSent\"{MessagesSent}\" "
                            + $"$CharSent\"{CharSent}\" "
                            + $"$HangStarted\"{HangStarted}\" "
                            + $"$HangSolved\"{HangSolved}\" "
                            + $"$GamesPlayed\"{GetGames()}\" "
                            + $"$Automove\"{AutoMove.ToString()}\" "
                            + $"$LastOnline\"{LastOnline.ToString()}\" "
                            + $"$OnlineSince\"{OnlineSince.ToString()}\" "
                            + $"$AFKMove\"{AFKMove.ToString()}\" "
                            + $"$SongsPlayed\"{SongsPlayed}\" ;";
                        File.WriteAllLines("../../User_Data.txt", Data);
                    Errord = false;
                }

            }
            catch (IOException) { }
        }

        public void Read()
        {
            try
            {
                string[] Data = File.ReadAllLines("../../User_Data.txt");
                for (int i = 0; i < Data.Length; i++)
                {
                    if (Data[i].Contains($"$ID\"{User.Id}\""))
                    {
                        try
                        { MessagesSent = int.Parse(Fcn.Msg.Between(Data[i], "$MessagesSent\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { CharSent = int.Parse(Fcn.Msg.Between(Data[i], "$CharSent\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { HangStarted = int.Parse(Fcn.Msg.Between(Data[i], "$HangStarted\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { HangSolved = int.Parse(Fcn.Msg.Between(Data[i], "$HangSolved\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { GetGames(false, Data[i]); }
                        catch (Exception) { }
                        try
                        { SongsPlayed = int.Parse(Fcn.Msg.Between(Data[i], "$SongsPlayed\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { AutoMove = bool.Parse(Fcn.Msg.Between(Data[i], "$Automove\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { AFKMove = bool.Parse(Fcn.Msg.Between(Data[i], "$AFKMove\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { OnlineSince = DateTime.Parse(Fcn.Msg.Between(Data[i], "$OnlineSince\"", "\" ")); }
                        catch (Exception) { }
                        try
                        { LastOnline = DateTime.Parse(Fcn.Msg.Between(Data[i], "$LastOnline\"", "\" ")); }
                        catch (Exception) { }

                        return;
                    }
                }
            }
            catch (System.IO.IOException) { }
        }

        private string GetGames(bool b = true, string s2 = "")
        {
            string s = "";
            if (b)
            {
                for (int i = 0; i < Games.Count; i++)
                {
                    if (Games[i] != "")
                    {
                        s += "||" + Games[i] + "||";
                    }
                }
                return s;
            }
            s2 = Fcn.Msg.Between(s2, "$GamesPlayed\"", "\" ");
            while (s2.Contains("||"))
            {
                Games.Add(Fcn.Msg.Between(s2, "||", "||"));
                if (s2 != "")
                {
                    s2 = s2.Substring(s2.IndexOf("||" + Fcn.Msg.Between(s2, "||", "||") + "||") + ("||" + Fcn.Msg.Between(s2, "||", "||") + "||").Length);
                }
            }
            return "";
        }
        
        private bool IsEmpty()
        {
            if (LastOnline.ToString() == "01-Jan-01 12:00:00 AM" && OnlineSince.ToString() == "01-Jan-01 12:00:00 AM" && AFKMove == true && AutoMove == true)
                if (SongsPlayed == 0 && CharSent == 0 && HangSolved == 0 && HangStarted == 0 && MessagesSent == 0)
                    return true;
            return false;
        }
    }
}