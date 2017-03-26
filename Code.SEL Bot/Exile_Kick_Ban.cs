using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.SEL_Bot
{
    public class Exile
    {
        public Discord.User U { get; set; }
        public Time Duration { get; set; }
        public IEnumerable<Discord.Role> Roles { get; set; }
        public Discord.Channel Chan { get; set; }
        public System.Timers.Timer ETimer { get; set; } = new System.Timers.Timer();

        public Exile(int Time)
        {
            Duration = new Time(Time); 
            System.Timers.Timer Timer = new System.Timers.Timer(1000);
            Timer.Start();
            Timer.Elapsed += (s, e) =>
            {
                Duration.AddSeconds(-1);
                if (Duration.Total <= 0)
                {
                    Timer.Stop();
                }
            };
        }
    }

    public class Kick
    {
        public int Votes { get; set; } = 0;
        public Discord.User User { get; set; }
        private string[] VotedId = { "" };
        public bool Canceled { get; set; } = false;

        public async void Vote(Discord.User U, Discord.Channel C)
        {
            Array.Resize(ref VotedId, VotedId.Length + 1);
            VotedId[VotedId.Length - 1] = U.Id.ToString();
            Votes++;
            if (U.Roles.First().Name.ToString() == "Code.SEL Member")
            {
                Votes++;
                await C.SendMessage("Code.SEL Memeber " + U.Mention + " voted to kick " + User.Mention + " with 2 votes");
                Console.WriteLine("Event: Code.SEL Memeber " + U.Name + " voted to kick " + User.Name);
            }
            else
            {
                await C.SendMessage(U.Mention + " voted to kick " + User.Mention);
                Console.WriteLine("Event: " + U.Name + " voted to kick " + User.Name);
            }
        }

        public bool HasVoted(string ID)
        {
            foreach (string Id in VotedId)
            {
                if (Id == ID)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class Ban
    {
        public int Votes { get; set; } = 0;
        public Discord.User User { get; set; }
        private string[] VotedId = { "" };
        public bool Canceled { get; set; } = false;

        public async void Vote(Discord.User U, Discord.Channel C)
        {
            Array.Resize(ref VotedId, VotedId.Length + 1);
            VotedId[VotedId.Length - 1] = U.Id.ToString();
            Votes++;
            if (U.Roles.First().Name.ToString() == "Code.SEL Member")
            {
                Votes++;
                await C.SendMessage("Code.SEL Memeber " + U.Mention + " voted to Ban " + User.Mention + " with 2 votes");
                Console.WriteLine("Event: Code.SEL Memeber " + U.Name + " voted to Ban " + User.Name);
            }
            else
            {
                await C.SendMessage(U.Mention + " voted to Ban " + User.Mention);
                Console.WriteLine("Event: " + U.Name + " voted to Ban " + User.Name);
            }
        }

        public bool HasVoted(string ID)
        {
            foreach (string Id in VotedId)
            {
                if (Id == ID)
                {
                    return true;
                }
            }

            return false;
        }
    }    
}
