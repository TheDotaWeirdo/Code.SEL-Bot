using Discord;
using DiscordBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Code.SEL_Bot
{
    public static class Fcn
    {
        public static class Discord
        {
            public static string MentionByName(string S)
            {
                User u = MyBot.discord.Servers.First().FindUsers(S).FirstOrDefault();
                if (u != null)
                {
                    return u.Mention;
                }

                return S;
            }

            public static void TimedMsg(Message M, int T = 7500)
            {
                System.Timers.Timer MainTimer = new System.Timers.Timer(T);
                MainTimer.Start();
                MainTimer.Elapsed += async (s, e) =>
                {
                    MainTimer.Stop();
                    Console.WriteLine("Action: Automatically deleted '" + M.RawText + " from " + M.User.Name);
                    await M.Delete();
                };
            }

            public static void TimedMsg(Task<Message> Msg, int T = 7500)
            {
                Message M = Msg.Result;
                System.Timers.Timer MainTimer = new System.Timers.Timer(T);
                MainTimer.Start();
                MainTimer.Elapsed += async (s, e) =>
                {
                    MainTimer.Stop();
                    Console.WriteLine("Action: Automatically deleted '" + M.RawText + " from " + M.User.Name);
                    await M.Delete();
                };
            }

            public static void CheckKick(User U, bool KV)
            {
                if (KV)
                {
                    U.Kick();
                    Console.WriteLine("Event: " + U.Name + " was kicked from the Server");
                }
                else
                {
                    Console.WriteLine("Event: " + U.Name + " was NOT kicked from the Server due to kicking being disabled");
                }
            }

            public static string CountUsersInRole(Role R, Server S)
            {
                int i = 0;
                IEnumerable<User> Users = S.Users;
                foreach (User U in Users)
                {
                    if (U.Roles.Contains(R))
                    {
                        i++;
                    }
                }

                return i.ToString();
            }

            public static string CountOnlineUsers(Server S, bool Online = true)
            {
                int i = 0;
                IEnumerable<User> Users = S.Users;
                foreach (User U in Users)
                {
                    if (U.Status.ToString() == "online")
                    {
                        i++;
                    }
                }
                if (!Online)
                {
                    return (MyBot.discord.Servers.FirstOrDefault().UserCount - i).ToString();
                }

                return i.ToString();
            }

            public static string GetNickname(User U)
            {
                if (U.Nickname != null)
                {
                    return "  AKA " + U.Nickname.ToString();
                }

                return "";
            }

            public static string GetUserInfo(User u)
            {
                A_Users AU = Program.AllUsers[GetA_UsersIndex(u)];
                string output = u.Mention;
                if (u.Id == 184405311681986560)
                {
                    Console.WriteLine("Action: Proccessing FredBoatMusic's information");
                }
                else
                {
                    Console.WriteLine("Action: Proccessing " + u.Name + "'s information");
                }

                if (u.Nickname != null)
                {
                    output += "  AKA `" + u.Name.ToString() + "`";
                }

                output += "\n\n`      Discriminator:`     #" + u.Discriminator.ToString();
                output += "\n`      Long ID      :`     " + u.Id.ToString();
                output += "\n`      Join Date    :`     " + u.JoinedAt.ToShortDateString();
                if (u.Status.ToString() == "online")
                {
                    output += "\n`      Currently    :`     Online since " + AU.OnlineSince.AddHours(-2).ToShortTimeString() + " GMT";
                    if (!u.IsBot && u.CurrentGame != null)
                    {
                        output += "\n`      Playing      :`     " + u.CurrentGame.Value.Name.ToString();
                    }
                }
                else if (u.Status.ToString() == "idle")
                {
                    output += $"\n`      Currently    :`     AFK, Online since {AU.OnlineSince.AddHours(-2).ToShortTimeString()} GMT";
                }
                else if (u.Status.ToString() == "dnd")
                {
                    output += $"\n`      Currently    :`     Busy, Online since {AU.OnlineSince.AddHours(-2).ToShortTimeString()} GMT";
                }
                else
                {
                    output += "\n`      Currently    :`     Offline" + Fcn.Msg.ConditionalString(", Last Seen on " + AU.LastOnline.AddHours(-2).ToString() + " GMT", AU.LastOnline.ToString() != "01-Jan-01 12:00:00 AM");
                }

                output += "\n`      Msg Sent     :`     " + AU.MessagesSent;
                output += "\n`      Chars Sent   :`     " + AU.CharSent;
                if (!AU.User.IsBot)
                {
                    output += "\n`      Hangs Started:`     " + AU.HangStarted;
                    output += "\n`      Hangs Solved :`     " + AU.HangSolved;
                    output += "\n`      Songs Played :`     " + AU.SongsPlayed;
                }
                output += "\n`      Roles        :`    ";
                foreach (Role R in u.Roles)
                {
                    if (!R.IsEveryone && R.IsMentionable)
                    {
                        output += " " + R.Mention;
                    }
                }

                if (AU.Games.Count > 0 && !AU.User.IsBot)
                {
                    output += "\n`      Games        :`     " + AU.Games[0];
                    for (int i = 1; i < AU.Games.Count; i++)
                    {
                        output += ", " + AU.Games[i];
                    }
                }
                return output;
            }

            public static int GetA_UsersIndex(User U)
            {
                try
                {
                    for (int i = 0; i < Program.AllUsers.Length; i++)
                    {
                        if (Program.AllUsers[i].User == U)
                        {
                            return i;
                        }
                    }
                }
                catch (NullReferenceException) { }
                return 0;
            }

            public static int GetA_UsersLineIndex(User U)
            {
                string[] Data = File.ReadAllLines("../../User_Data.txt");
                for (int i = 0; i < Data.Length; i++)
                {
                    if (Fcn.Msg.Between(Data[i], "$ID\"", '"') == U.Id.ToString())
                    {
                        return i;
                    }
                }
                return 0;
            }

            public static async void Annoy(User U)
            {
                string[] msg =
                {
                "Hey!", "Bah",                ".",".",".",".",
                "random words",
                "why not",
                "fuck you",
                "Ha Gaaaaaaaaaaaaaaay",
                "?",
                "XD",
                "Hail Trump",
                "Allahu Akbar",
                "w/e"
            };
                await U.SendMessage(msg[new Random().Next(msg.Length - 1)]);
                Console.WriteLine("Action: Sending Spam message to " + U.Name);
            }

            public static Message WaitForMsg(Message M)
            {
                while (M.Timestamp.ToString() == "01-Jan-01 12:00:00 AM")
                {
                    System.Threading.Thread.Sleep(5);
                }

                return M;
            }

            public static Channel GetChannel(string S)
            {
                S = S.ToLower();
                if (S.Contains("afk"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(155958790124994560);
                }

                if (S.Contains("main"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(235871481307987968);
                }

                if (S.Contains("music"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(272736534418161664);
                }

                if (S.Contains("dota") || S.Contains("dots") || S.Contains("doto"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(155960591083634688);
                }

                if ((S.Contains("rl") || S.Contains("rocket") || S.Contains("league")) && !S.Contains("legends"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(218475874398371841);
                }

                if (S.Contains("cs") || S.Contains("counter"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(249452569880035329);
                }

                if (S.Contains("gta") || S.Contains("grand"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(216988791909384194);
                }

                if (S.Contains("overwatch"))
                {
                    return MyBot.discord.Servers.FirstOrDefault().GetChannel(235865341782261760);
                }

                return null;
            }
        }

        public static class Msg
        {
            public static void RmvSpaces(ref string S)
            {
                string s = "";
                foreach (char c in S)
                    if (!string.IsNullOrWhiteSpace(c.ToString()))
                        s += c;
                S = s;
            }

            public static string Decrypt(string msg, string[] Sts, bool StrictStart = true)
            {
                if (msg.StartsWith("-"))
                {
                    msg = msg.TrimStart('-');
                }
                else
                {
                    msg = Remove(msg, "<@272383726300692500>");
                }

                while (msg[0] == ' ')
                {
                    msg = msg.TrimStart(' ');
                }

                if (Sts == null)
                {
                    return msg;
                }

                string p = "Action: Decrypting " + msg + " with key: " + Sts[0];
                for (int i = 1; i < Sts.Length; i++)
                {
                    p = p + " | " + Sts[i];
                }
                Console.WriteLine(p);

                if (StrictStart)
                {
                    foreach (string S in Sts)
                    {
                        if (msg.ToLower().StartsWith(S))
                        {
                            msg = Remove(msg, S);
                        }
                    }
                }
                else
                {
                    foreach (string S in Sts)
                    {
                        if (msg.ToLower().Contains(S))
                        {
                            msg = Remove(msg, S);
                        }
                    }
                }
                while (msg != "" && msg[0] == ' ')
                {
                    msg = msg.TrimStart(' ');
                }

                return msg;
            }

            public static string Decrypt(string msg, string St, bool StrictStart = true)
            {
                if (msg.StartsWith("-"))
                {
                    msg = msg.TrimStart('-');
                }
                else
                {
                    msg = Remove(msg, "<@272383726300692500>");
                }

                while (msg[0] == ' ')
                {
                    msg = msg.TrimStart(' ');
                }

                if (St == null || St == "")
                {
                    return msg;
                }

                Console.WriteLine("Action: Decrypting " + msg + " with key: " + St);

                if (StrictStart)
                {
                    if (msg.ToLower().StartsWith(St))
                    {
                        msg = Remove(msg, St);
                    }
                }
                else
                {
                    msg = Remove(msg, St);
                }

                while (msg != "" && msg[0] == ' ')
                {
                    msg = msg.TrimStart(' ');
                }

                return msg;
            }

            public static bool CommandCheck(string msg, string[] Sts, bool StrictStart = true)
            {
                msg = msg.ToLower();
                if (msg.StartsWith("-"))
                {
                    msg = msg.TrimStart('-');
                }
                else if (msg.Contains("<@272383726300692500>"))
                {
                    msg = Remove(msg, "<@272383726300692500>");
                }
                else
                {
                    return false;
                }

                try
                {
                    while (msg[0] == ' ')
                    {
                        msg = msg.TrimStart(' ');
                    }
                }
                catch (IndexOutOfRangeException) { }

                if (Sts == null)
                {
                    return true;
                }

                if (StrictStart)
                {
                    foreach (string S in Sts)
                    {
                        if (msg.StartsWith(S))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    foreach (string S in Sts)
                    {
                        if (msg.Contains(S))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            public static bool CommandCheck(string msg, string St, bool StrictStart = true)
            {
                msg = msg.ToLower();
                if (msg.StartsWith("-"))
                {
                    msg = msg.TrimStart('-');
                }
                else if (msg.Contains("<@272383726300692500>"))
                {
                    msg = Remove(msg, "<@272383726300692500>");
                }
                else
                {
                    return false;
                }

                while (msg[0] == ' ')
                {
                    msg = msg.TrimStart(' ');
                }

                if (St == "" || St == null)
                {
                    return true;
                }

                if (StrictStart && msg.StartsWith(St))
                {
                    return true;
                }
                else if (!StrictStart && msg.Contains(St))
                {
                    return true;
                }

                return false;
            }

            public static string Remove(string S, string K, bool Opp = false)
            {
                if (S.ToLower().Contains(K))
                {
                    return S.Substring(0, S.ToLower().IndexOf(K)) + S.Substring(S.ToLower().IndexOf(K) + K.Length);
                }

                return S;
            }

            public static string Remove(string S, char K, bool Opp = false)
            {
                if (S.ToLower().Contains(K))
                {
                    return S.Substring(0, S.ToLower().IndexOf(K)) + S.Substring(S.ToLower().IndexOf(K) + 1);
                }

                return S;
            }

            public static string ToEmoji(string S)
            {
                if (S == "") return "";
                string output = "";
                foreach (char c in S)
                    output += EmojiConverter(c);
                return output;
            }

            public static string ToEmoji(int I)
            {
                string S = I.ToString();
                if (S == "") return "";
                string output = "";
                foreach (char c in S)
                    output += EmojiConverter(c);
                return output;
            }

            public static string EmojiConverter(char c)
            {
                if (c == '1')
                {
                    return ":one:";
                }

                if (c == '2')
                {
                    return ":two:";
                }

                if (c == '3')
                {
                    return ":three:";
                }

                if (c == '4')
                {
                    return ":four:";
                }

                if (c == '5')
                {
                    return ":five:";
                }

                if (c == '6')
                {
                    return ":six:";
                }

                if (c == '7')
                {
                    return ":seven:";
                }

                if (c == '8')
                {
                    return ":eight:";
                }

                if (c == '8')
                {
                    return ":nine:";
                }

                if (c == '0')
                {
                    return ":zero:";
                }

                if (c == ' ')
                {
                    return "  ";
                }

                if (c == '#')
                {
                    return ":hash:";
                }

                if (c == '*')
                {
                    return ":asterisk:";
                }

                if (c == 'a')
                {
                    return "<:H_A:285782726274056192>";
                }

                if (c == 'b')
                {
                    return "<:H_B:285782726706069504>";
                }

                if (c == 'c')
                {
                    return "<:H_C:285782726983024641>";
                }

                if (c == 'd')
                {
                    return "<:H_D:285782729734488065>";
                }

                if (c == 'e')
                {
                    return "<:H_E:285782730141204482>";
                }

                if (c == 'f')
                {
                    return "<:H_F:285782730573086720>";
                }

                if (c == 'g')
                {
                    return "<:H_G:285782731021877248>";
                }

                if (c == 'h')
                {
                    return "<:H_H:285782730699046913>";
                }

                if (c == 'i')
                {
                    return "<:H_I:285782730992779264>";
                }

                if (c == 'j')
                {
                    return "<:H_J:285782731298832393>";
                }

                if (c == 'k')
                {
                    return "<:H_K:285782731508678657>";
                }

                if (c == 'l')
                {
                    return "<:H_L:285782731919589376>";
                }

                if (c == 'm')
                {
                    return "<:H_M:285783607711367168>";
                }

                if (c == 'n')
                {
                    return "<:H_N:285783607707172865>";
                }

                if (c == 'o')
                {
                    return "<:H_O:285783610542522368>";
                }

                if (c == 'p')
                {
                    return "<:H_P:285783610362167296>";
                }

                if (c == 'q')
                {
                    return "<:H_Q:285783610949369856>";
                }

                if (c == 'r')
                {
                    return "<:H_R:285783610995507201>";
                }

                if (c == 's')
                {
                    return "<:H_S:285783612983476226>";
                }

                if (c == 't')
                {
                    return "<:H_T:285783613004316673>";
                }

                if (c == 'u')
                {
                    return "<:H_U:285783613126213634>";
                }

                if (c == 'v')
                {
                    return "<:H_V:285783613818011649>";
                }

                if (c == 'w')
                {
                    return "<:H_W:285783615013519360>";
                }

                if (c == 'x')
                {
                    return "<:H_X:285783617949401089>";
                }

                if (c == 'y')
                {
                    return "<:H_Y:285783618285076480>";
                }

                if (c == 'z')
                {
                    return "<:H_Z:285783618792456192>";
                }

                return "";
            }
           
            public static string Between(string S, char c1, char c2)
            {
                S = S.Substring(S.IndexOf(c1) + 1);
                return S.Substring(0, S.IndexOf(c2));
            }

            public static string Between(string S, string s1, string s2)
            {
                S = S.Substring(S.IndexOf(s1) + s1.Length);
                return S.Substring(0, S.IndexOf(s2));
            }

            public static string Between(string S, string s1, char c2)
            {
                S = S.Substring(S.IndexOf(s1) + s1.Length);
                return S.Substring(0, S.IndexOf(c2));
            }

            public static string Between(string S, char c1, string s2)
            {
                S = S.Substring(S.IndexOf(c1) + 1);
                return S.Substring(0, S.IndexOf(s2));
            }

            public static string ConditionalString(string S, bool B)
            {
                if (B)
                {
                    return S;
                }

                return "";
            }
        }

    }
}
