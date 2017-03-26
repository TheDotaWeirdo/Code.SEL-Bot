using System;
using System.IO;

namespace Code.SEL_Bot
{
    public class TicTacToe
    {
        private int[][] Case { get; set; } = new int[4][] { new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0 } };
        public int Turn { get; set; } = 0;
        public int ID { get; set; } = 0;
        private int Moves { get; set; } = 0;
        private int Star { get; set; } = 0;
        public bool Finished { get; set; } = false;
        public Discord.User[] Player { get; set; } = { null, null };
        public Discord.Message Board { get; set; } = null;
        public Discord.Message Mess { get; set; } = null;
        private Discord.Channel Chan { get; set; } = null;

        public void Initialize(Discord.User P1, Discord.User P2, Discord.Channel C)
        {
            string[] Sts = File.ReadAllLines("../../Data.txt"), St = File.ReadAllLines("../../XO_Data.txt");
            ID = Convert.ToInt32(Sts[0].Substring(7)) + 1;
            Array.Resize(ref St, ID + 1);
            St[ID] = "Match[" + Inttodigits(ID, 4) + "] = Incomplete";
            Sts[0] = "XO_ID = " + ID;
            File.WriteAllLines("../../Data.txt", Sts);
            File.WriteAllLines("../../XO_Data.txt", St);
            Console.WriteLine("Initializing Tic Tac Toe Game");
            Case[1][1] = 0; Case[1][2] = 0; Case[1][3] = 0;
            Case[2][1] = 0; Case[2][2] = 0; Case[2][3] = 0;
            Case[3][1] = 0; Case[3][2] = 0; Case[3][3] = 0;
            Random rnd = new Random(); int i = rnd.Next(100); Console.Write(i);
            if (i >= 50)
            {
                Turn = Star = 1;
            }
            else
            {
                Turn = Star = 0;
            }

            Player[0] = P1; Player[1] = P2;
            Chan = C;
            Finished = false;
        }

        public async void Start()
        {
            await Chan.SendMessage("Tic <:TTT_X:285804765819174912> Tac <:TTT_O:285804759951212544> Toe started between " + Player[0].Mention + " <:TTT_X:285804765819174912> and " + Player[1].Mention + " <:TTT_O:285804759951212544> `Game ID: " + ID + "`");
            Board = await Chan.SendMessage(RenderBoard());
            Mess = await Chan.SendMessage(Player[Turn].Mention + "'s turn to play");
            if (Player[Turn].IsBot)
            {
                System.Timers.Timer MainTimer = new System.Timers.Timer(2000);
                MainTimer.Start();
                MainTimer.Elapsed += (s, e) =>
                {
                    MainTimer.Stop();
                    PlayBot();
                };
            }
        }

        public async void Play(int x, int y)
        {
            if (Case[x][y] == 0)
            {
                Moves++;
                Console.WriteLine("Event: X/O case: " + x + "#" + y + " was played by " + Player[Turn].Name);
                Case[x][y] = Turn + 1;
                await Board.Edit(RenderBoard());
                int Winner = CheckWin();
                if (Winner == 0)
                {
                    Turn = Opposite();
                    await Mess.Edit(Player[Turn].Mention + "'s turn to play");
                    if (Player[Turn].IsBot)
                    {
                        System.Timers.Timer MainTimer = new System.Timers.Timer(1000);
                        MainTimer.Start();
                        MainTimer.Elapsed += (s, e) =>
                        {
                            MainTimer.Stop();
                            PlayBot();
                        }; ;
                    }
                }
                else if (Winner > 0)
                {
                    string[] Sts = File.ReadAllLines("../../XO_Data.txt");
                    Sts[ID] = "Match[" + Inttodigits(ID, 4) + "] 0 Win(";
                    if (Turn == 1)
                    {
                        Sts[ID] = Sts[ID] + "O) Start(";
                    }
                    else
                    {
                        Sts[ID] = Sts[ID] + "X) Start(";
                    }

                    if (Star == 1)
                    {
                        Sts[ID] = Sts[ID] + "O) Moves(" + Moves + ") Bot(";
                    }
                    else
                    {
                        Sts[ID] = Sts[ID] + "X) Moves(" + Moves + ") Bot(";
                    }

                    if (Player[1].IsBot)
                    {
                        Sts[ID] = Sts[ID] + "Y) Player[1](" + Player[0].Name + ") Player[2](" + Player[1].Name + ") #";
                    }
                    else
                    {
                        Sts[ID] = Sts[ID] + "N) Player[1](" + Player[0].Name + ") Player[2](" + Player[1].Name + ") #";
                    }

                    for (int i = 1; i < 4; i++)
                    {
                        for (int j = 1; j < 4; j++)
                        {
                            Sts[ID] = Sts[ID] + Case[j][i];
                        }
                    }
                    File.WriteAllLines("../../XO_Data.txt", Sts);

                    Console.WriteLine(Player[Turn].Mention + " Won the Game");
                    await Mess.Edit(Player[Turn].Mention + " Won the Game ! :tada:");
                    Finished = true;
                }
                else if (Winner == -1)
                {
                    string[] Sts = File.ReadAllLines("../../XO_Data.txt"); Array.Resize(ref Sts, Sts.Length + 1);
                    Sts[ID] = "Match[" + Inttodigits(ID, 4) + "] 0 Win(T) Start(";
                    if (Star == 1)
                    {
                        Sts[ID] = Sts[ID] + "O) Moves(9) Bot(";
                    }
                    else
                    {
                        Sts[ID] = Sts[ID] + "X) Moves(9) Bot(";
                    }

                    if (Player[1].IsBot)
                    {
                        Sts[ID] = Sts[ID] + "Y) Player[1](" + Player[0].Name + ") Player[2](" + Player[1].Name + ") #";
                    }
                    else
                    {
                        Sts[ID] = Sts[ID] + "N) Player[1](" + Player[0].Name + ") Player[2](" + Player[1].Name + ") #";
                    }

                    for (int i = 1; i < 4; i++)
                    {
                        for (int j = 1; j < 4; j++)
                        {
                            Sts[ID] = Sts[ID] + Case[j][i];
                        }
                    }
                    File.WriteAllLines("../../XO_Data.txt", Sts);

                    Console.WriteLine("The Game ended in a Tie!");
                    await Mess.Edit("The Game ended in a Tie! :necktie:");
                    Finished = true;
                }
            }
            else
            {
                Console.WriteLine(x + "#" + y + " already contains something - Error");
                Fcn.Discord.TimedMsg(await Chan.SendMessage(":anger: There's already an X/O in this case"));
            }
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

        private void PlayBot()
        {
            if (CheckWin() > 0)
            {
                return;
            }

            Random rnd = new Random();
            //Winning
            if (Case[1][1] == Case[1][2] && Case[1][2] == (Turn + 1) && Case[1][3] == 0) { Play(1, 3); return; } //Verticals
            if (Case[1][1] == Case[1][3] && Case[1][3] == (Turn + 1) && Case[1][2] == 0) { Play(1, 2); return; }
            if (Case[1][3] == Case[1][2] && Case[1][2] == (Turn + 1) && Case[1][1] == 0) { Play(1, 1); return; }
            if (Case[2][1] == Case[2][2] && Case[2][2] == (Turn + 1) && Case[2][3] == 0) { Play(2, 3); return; }
            if (Case[2][1] == Case[2][3] && Case[2][3] == (Turn + 1) && Case[2][2] == 0) { Play(2, 2); return; }
            if (Case[2][3] == Case[2][2] && Case[2][2] == (Turn + 1) && Case[2][1] == 0) { Play(2, 1); return; }
            if (Case[3][1] == Case[3][2] && Case[3][2] == (Turn + 1) && Case[3][3] == 0) { Play(3, 3); return; }
            if (Case[3][1] == Case[3][3] && Case[3][3] == (Turn + 1) && Case[3][2] == 0) { Play(3, 2); return; }
            if (Case[3][3] == Case[3][2] && Case[3][2] == (Turn + 1) && Case[3][1] == 0) { Play(3, 1); return; }

            if (Case[1][1] == Case[2][1] && Case[2][1] == (Turn + 1) && Case[3][1] == 0) { Play(3, 1); return; } //Horizonta;
            if (Case[1][2] == Case[2][2] && Case[2][2] == (Turn + 1) && Case[3][2] == 0) { Play(3, 2); return; }
            if (Case[1][3] == Case[2][3] && Case[2][3] == (Turn + 1) && Case[3][3] == 0) { Play(3, 3); return; }
            if (Case[2][1] == Case[3][1] && Case[3][1] == (Turn + 1) && Case[1][1] == 0) { Play(1, 1); return; }
            if (Case[2][2] == Case[3][2] && Case[3][2] == (Turn + 1) && Case[1][2] == 0) { Play(1, 2); return; }
            if (Case[2][3] == Case[3][3] && Case[3][3] == (Turn + 1) && Case[1][3] == 0) { Play(1, 3); return; }
            if (Case[3][1] == Case[1][1] && Case[1][1] == (Turn + 1) && Case[2][1] == 0) { Play(2, 1); return; }
            if (Case[3][2] == Case[1][2] && Case[1][2] == (Turn + 1) && Case[2][2] == 0) { Play(2, 2); return; }
            if (Case[3][3] == Case[1][3] && Case[1][3] == (Turn + 1) && Case[2][3] == 0) { Play(2, 3); return; }

            if (Case[1][1] == Case[2][2] && Case[2][2] == (Turn + 1) && Case[3][3] == 0) { Play(3, 3); return; } //Diagonal
            if (Case[1][1] == Case[3][3] && Case[3][3] == (Turn + 1) && Case[2][2] == 0) { Play(2, 2); return; }
            if (Case[2][2] == Case[3][3] && Case[3][3] == (Turn + 1) && Case[1][1] == 0) { Play(1, 1); return; }
            if (Case[3][1] == Case[2][2] && Case[2][2] == (Turn + 1) && Case[1][3] == 0) { Play(1, 3); return; }
            if (Case[3][1] == Case[1][3] && Case[1][3] == (Turn + 1) && Case[2][2] == 0) { Play(2, 2); return; }
            if (Case[2][2] == Case[1][3] && Case[1][3] == (Turn + 1) && Case[3][1] == 0) { Play(3, 1); return; }
            if (rnd.Next(101) < 90)
            {
                //Blocking
                if (Case[1][1] == Case[1][2] && Case[1][2] == (Opposite() + 1) && Case[1][3] == 0) { Play(1, 3); return; } //Verticals
                if (Case[1][1] == Case[1][3] && Case[1][3] == (Opposite() + 1) && Case[1][2] == 0) { Play(1, 2); return; }
                if (Case[1][3] == Case[1][2] && Case[1][2] == (Opposite() + 1) && Case[1][1] == 0) { Play(1, 1); return; }
                if (Case[2][1] == Case[2][2] && Case[2][2] == (Opposite() + 1) && Case[2][3] == 0) { Play(2, 3); return; }
                if (Case[2][1] == Case[2][3] && Case[2][3] == (Opposite() + 1) && Case[2][2] == 0) { Play(2, 2); return; }
                if (Case[2][3] == Case[2][2] && Case[2][2] == (Opposite() + 1) && Case[2][1] == 0) { Play(2, 1); return; }
                if (Case[3][1] == Case[3][2] && Case[3][2] == (Opposite() + 1) && Case[3][3] == 0) { Play(3, 3); return; }
                if (Case[3][1] == Case[3][3] && Case[3][3] == (Opposite() + 1) && Case[3][2] == 0) { Play(3, 2); return; }
                if (Case[3][3] == Case[3][2] && Case[3][2] == (Opposite() + 1) && Case[3][1] == 0) { Play(3, 1); return; }

                if (Case[1][1] == Case[2][1] && Case[2][1] == (Opposite() + 1) && Case[3][1] == 0) { Play(3, 1); return; } //Horizonta;
                if (Case[1][2] == Case[2][2] && Case[2][2] == (Opposite() + 1) && Case[3][2] == 0) { Play(3, 2); return; }
                if (Case[1][3] == Case[2][3] && Case[2][3] == (Opposite() + 1) && Case[3][3] == 0) { Play(3, 3); return; }
                if (Case[2][1] == Case[3][1] && Case[3][1] == (Opposite() + 1) && Case[1][1] == 0) { Play(1, 1); return; }
                if (Case[2][2] == Case[3][2] && Case[3][2] == (Opposite() + 1) && Case[1][2] == 0) { Play(1, 2); return; }
                if (Case[2][3] == Case[3][3] && Case[3][3] == (Opposite() + 1) && Case[1][3] == 0) { Play(1, 3); return; }
                if (Case[3][1] == Case[1][1] && Case[1][1] == (Opposite() + 1) && Case[2][1] == 0) { Play(2, 1); return; }
                if (Case[3][2] == Case[1][2] && Case[1][2] == (Opposite() + 1) && Case[2][2] == 0) { Play(2, 2); return; }
                if (Case[3][3] == Case[1][3] && Case[1][3] == (Opposite() + 1) && Case[2][3] == 0) { Play(2, 3); return; }

                if (Case[1][1] == Case[2][2] && Case[2][2] == (Opposite() + 1) && Case[3][3] == 0) { Play(3, 3); return; } //Diagonal
                if (Case[1][1] == Case[3][3] && Case[3][3] == (Opposite() + 1) && Case[2][2] == 0) { Play(2, 2); return; }
                if (Case[2][2] == Case[3][3] && Case[3][3] == (Opposite() + 1) && Case[1][1] == 0) { Play(1, 1); return; }
                if (Case[3][1] == Case[2][2] && Case[2][2] == (Opposite() + 1) && Case[1][3] == 0) { Play(1, 3); return; }
                if (Case[3][1] == Case[1][3] && Case[1][3] == (Opposite() + 1) && Case[2][2] == 0) { Play(2, 2); return; }
                if (Case[2][2] == Case[1][3] && Case[1][3] == (Opposite() + 1) && Case[3][1] == 0) { Play(3, 1); return; }

                if (Case[2][2] == 0) { Play(2, 2); return; }
            }
            else
            {
                Console.Write("Bot Skipped logical reasoning - ");
            }

            while (true)
            {
                int x = rnd.Next(1, 4), y = rnd.Next(1, 4);
                if (Case[x][y] == 0)
                {
                    Console.WriteLine("Event: Bot randomly chose " + x + "#" + y);
                    Play(x, y);
                    return;
                }
            }
        }

        private int Opposite()
        {
            if (Turn == 0)
            {
                return 1;
            }

            return 0;
        }

        private int CheckWin()
        {
            if (Case[1][1] == Case[2][2] && Case[2][2] == Case[3][3])
            {
                return Case[1][1];
            }

            if (Case[1][3] == Case[2][2] && Case[2][2] == Case[3][1])
            {
                return Case[1][3];
            }

            if (Case[1][1] == Case[1][2] && Case[1][2] == Case[1][3])
            {
                return Case[1][1];
            }

            if (Case[2][1] == Case[2][2] && Case[2][2] == Case[2][3])
            {
                return Case[2][1];
            }

            if (Case[3][1] == Case[3][2] && Case[3][2] == Case[3][3])
            {
                return Case[3][1];
            }

            if (Case[1][3] == Case[2][3] && Case[2][3] == Case[3][3])
            {
                return Case[1][3];
            }

            if (Case[1][2] == Case[2][2] && Case[2][2] == Case[3][2])
            {
                return Case[1][2];
            }

            if (Case[1][1] == Case[2][1] && Case[2][1] == Case[3][1])
            {
                return Case[1][1];
            }

            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (Case[i][j] == 0)
                    {
                        return 0;
                    }
                }
            }

            return -1;
        }

        private string RenderBoard()
        {
            string s = ""
            + CaseToEmoji(Case[1][3]) + CaseToEmoji() + CaseToEmoji(Case[2][3]) + CaseToEmoji() + CaseToEmoji(Case[3][3]) + "\n"
            + CaseToEmoji() + CaseToEmoji() + CaseToEmoji() + CaseToEmoji() + CaseToEmoji() + "\n"
            + CaseToEmoji(Case[1][2]) + CaseToEmoji() + CaseToEmoji(Case[2][2]) + CaseToEmoji() + CaseToEmoji(Case[3][2]) + "\n"
            + CaseToEmoji() + CaseToEmoji() + CaseToEmoji() + CaseToEmoji() + CaseToEmoji() + "\n"
            + CaseToEmoji(Case[1][1]) + CaseToEmoji() + CaseToEmoji(Case[2][1]) + CaseToEmoji() + CaseToEmoji(Case[3][1]) + "\n";
            return s;
        }

        private string CaseToEmoji(int i = 3)
        {
            if (i == 0)
            {
                return "<:TTT_Empty:285804754171592704>";
            }

            if (i == 1)
            {
                return "<:TTT_X:285804765819174912>";
            }

            if (i == 2)
            {
                return "<:TTT_O:285804759951212544>";
            }

            return "<:TTT_B:285804751424192512>";
        }
    }
}
