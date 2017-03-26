using System;
using System.IO;

namespace Code.SEL_Bot
{
    public class Connect4
    {
        private int[][] Case { get; set; } = new int[7][] { new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 }, new int[] { 0, 0, 0, 0, 0, 0 } };
        public int Turn { get; set; } = 0;
        public int ID { get; set; } = 0;
        public bool Finished { get; set; } = false;
        public Discord.User[] Player { get; set; } = { null, null };
        public Discord.Message Board { get; set; } = null;
        private Discord.Channel Chan { get; set; } = null;

        public async void Start(Discord.User P1, Discord.User P2, Discord.Channel C)
        {
            string[] Data = File.ReadAllLines("../../Data.txt");
            ID = Convert.ToInt32(Data[7].Substring(14));
            Turn = new Random().Next(2);
            Player[0] = P1; Player[1] = P2; Chan = C;
            await C.SendMessage("Connect <:Connect_4:285813760277610496> started between :red_circle:" + P1.Mention + " & :large_blue_circle:" + P2.Mention + " `Game ID: " + ID + '`');
            Board = await Chan.SendMessage(RenderBoard() + '\n' + Player[Turn].Mention + "'s Turn to Play");
            Data[7] = "Connect4_ID = " + (ID + 1);
            File.WriteAllLines("../../Data.txt", Data);
            Save();
            if (Player[Turn].IsBot)
            { System.Threading.Thread.Sleep(1000); PlayBot(); }
        }

        public async void Play(int x)
        {
            x--;
            if (Add(x))
            {
                if (CheckWin())
                {
                    await Board.Edit(RenderBoard() + '\n' + Player[Turn].Mention + " Won the Game! :tada: :confetti_ball:");
                    Finished = true;
                }
                else if (CheckTie())
                {
                    await Board.Edit(RenderBoard() + "\nThe Game ended in a Tie :necktie:");
                    Finished = true;
                }
                else
                {
                    Turn = Opposite();
                    await Board.Edit(RenderBoard() + '\n' + Player[Turn].Mention + "'s Turn to play");
                    if (Player[Turn].IsBot)
                    { System.Threading.Thread.Sleep(1000); PlayBot(); }
                }
                Save();
            }
            else
            {
                Fcn.Discord.TimedMsg(await Chan.SendMessage(":anger: This column is full!"));
            }
        }

        private void PlayBot()
        {
            if (new Random().Next(101) < 95)
            {
                for (int y = 5; y >= 0; y--)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        if (Case[x][y] != 0)
                        {
                            //Horizontals
                            if (x + 2 < 7 && x - 1 >= 0)
                            {
                                if (Case[x][y] == Case[x + 1][y] && Case[x + 1][y] == Case[x + 2][y] && Case[x - 1][y] == 0)
                                {
                                    if (y == 0 || Case[x - 1][y - 1] != 0)
                                    { Console.WriteLine("Case H1 - " + x); Play(x); return; }
                                }
                            }

                            if (x + 3 < 7)
                            {
                                if (Case[x][y] == Case[x + 2][y] && Case[x + 3][y] == Case[x + 2][y] && Case[x + 1][y] == 0)
                                {
                                    if (y == 0 || Case[x + 1][y - 1] != 0)
                                    { Console.WriteLine("Case H2 - " + (x + 2)); Play(x + 2); return; }
                                }
                            }

                            if (x + 3 < 7)
                            {
                                if (Case[x][y] == Case[x + 1][y] && Case[x + 1][y] == Case[x + 3][y] && Case[x + 2][y] == 0)
                                {
                                    if (y == 0 || Case[x + 2][y - 1] != 0)
                                    { Console.WriteLine("Case H3 - " + (x + 3)); Play(x + 3); return; }
                                }
                            }

                            if (x + 3 < 7)
                            {
                                if (Case[x][y] == Case[x + 1][y] && Case[x + 1][y] == Case[x + 2][y] && Case[x + 3][y] == 0)
                                {
                                    if (y == 0 || Case[x + 3][y - 1] != 0)
                                    { Console.WriteLine("Case H4 - " + (x + 4)); Play(x + 4); return; }
                                }
                            }
                            //Verticals
                            if (y + 1 < 6 && y - 2 >= 0)
                            {
                                if (Case[x][y] == Case[x][y - 1] && Case[x][y - 1] == Case[x][y - 2] && Case[x][y + 1] == 0)
                                { Console.WriteLine("Case V - " + (x + 1)); Play(x + 1); return; }
                            }
                            //Diagonals Up
                            if (x + 2 < 7 && y + 2 < 6 && x - 1 >= 0 && y - 1 >= 0)
                            {
                                if (Case[x][y] == Case[x + 1][y + 1] && Case[x + 1][y + 1] == Case[x + 2][y + 2] && Case[x - 1][y - 1] == 0)
                                {
                                    if (y - 1 == 0 || Case[x - 1][y - 2] != 0)
                                    { Console.WriteLine("Case DU1 - " + x); Play(x); return; }
                                }
                            }

                            if (x + 3 < 7 && y + 3 < 6)
                            {
                                if (Case[x][y] == Case[x + 2][y + 2] && Case[x + 3][y + 3] == Case[x + 2][y + 2] && Case[x + 1][y + 1] == 0)
                                {
                                    if (Case[x + 1][y] != 0)
                                    { Console.WriteLine("Case DU2 - " + (x + 2)); Play(x + 2); return; }
                                }
                            }

                            if (x + 3 < 7 && y + 3 < 6)
                            {
                                if (Case[x][y] == Case[x + 1][y + 1] && Case[x + 1][y + 1] == Case[x + 3][y + 3] && Case[x + 2][y + 2] == 0)
                                {
                                    if (Case[x + 2][y + 1] != 0)
                                    { Console.WriteLine("Case DU3 - " + (x + 3)); Play(x + 3); return; }
                                }
                            }

                            if (x + 3 < 7 && y + 3 < 6)
                            {
                                if (Case[x][y] == Case[x + 1][y + 1] && Case[x + 1][y + 1] == Case[x + 2][y + 2] && Case[x + 3][y + 3] == 0)
                                {
                                    if (Case[x + 3][y + 2] != 0)
                                    { Console.WriteLine("Case DU4 - " + (x + 4)); Play(x + 4); return; }
                                }
                            }
                            //Disagonals Down
                            if (x + 2 < 7 && y - 2 >= 0 && x - 1 >= 0 && y + 1 < 6)
                            {
                                if (Case[x][y] == Case[x + 1][y - 1] && Case[x + 1][y - 1] == Case[x + 2][y - 2] && Case[x - 1][y + 1] == 0)
                                {
                                    if (Case[x - 1][y] != 0)
                                    { Console.WriteLine("Case DD1 - " + x); Play(x); return; }
                                }
                            }

                            if (x + 3 < 7 && y - 3 >= 0)
                            {
                                if (Case[x][y] == Case[x + 2][y - 2] && Case[x + 3][y - 3] == Case[x + 2][y - 2] && Case[x + 1][y - 1] == 0)
                                {
                                    if (Case[x + 1][y - 2] != 0)
                                    { Console.WriteLine("Case DD2 - " + (x + 2)); Play(x + 2); return; }
                                }
                            }

                            if (x + 3 < 7 && y - 3 >= 0)
                            {
                                if (Case[x][y] == Case[x + 1][y - 1] && Case[x + 1][y - 1] == Case[x + 3][y - 3] && Case[x + 2][y - 2] == 0)
                                {
                                    if (Case[x + 2][y - 3] != 0)
                                    { Console.WriteLine("Case DD3 - " + (x + 3)); Play(x + 3); return; }
                                }
                            }

                            if (x + 3 < 7 && y - 3 >= 0)
                            {
                                if (Case[x][y] == Case[x + 1][y - 1] && Case[x + 1][y - 1] == Case[x + 2][y - 2] && Case[x + 3][y - 3] == 0)
                                {
                                    if (y - 3 == 0 || Case[x + 3][y - 4] != 0)
                                    { Console.WriteLine("Case DD4 - " + (x + 4)); Play(x + 4); return; }
                                }
                            }
                        }
                    }
                }
            }

            int a = new Random().Next(7);
            while (Case[a][5] != 0)
            {
                a = new Random().Next(7);
            }

            Console.WriteLine("Bot Randomed - " + (1 + a));
            Play(a + 1);
        }

        private bool Add(int x)
        {
            for (int y = 0; y < 6; y++)
            {
                if (x < 0 || x > 6)
                {
                    Console.WriteLine("Error >>>  x = " + x);
                }

                if (Case[x][y] == 0 && x >= 0 && x < 7)
                { Case[x][y] = Turn + 1; return true; }
            }
            return false;
        }

        private bool CheckWin()
        {
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (Case[x][y] != 0)
                    {
                        if (x < 4)
                        {
                            if (Case[x][y] == Case[x + 1][y] && Case[x + 1][y] == Case[x + 2][y] && Case[x + 2][y] == Case[x + 3][y])
                            { Case[x][y] = Case[x + 1][y] = Case[x + 2][y] = Case[x + 3][y] = Turn + 3; return true; }
                        }

                        if (y < 3)
                        {
                            if (Case[x][y] == Case[x][y + 1] && Case[x][y + 1] == Case[x][y + 2] && Case[x][y + 2] == Case[x][y + 3])
                            { Case[x][y] = Case[x][y + 1] = Case[x][y + 2] = Case[x][y + 3] = Turn + 3; return true; }
                        }

                        if (y < 4 && x < 4)
                        {
                            if (Case[x][y] == Case[x + 1][y + 1] && Case[x + 1][y + 1] == Case[x + 2][y + 2] && Case[x + 2][y + 2] == Case[x + 3][y + 3])
                            { Case[x][y] = Case[x + 1][y + 1] = Case[x + 2][y + 2] = Case[x + 3][y + 3] = Turn + 3; return true; }
                        }

                        if (x < 4 && y > 2)
                        {
                            if (Case[x][y] == Case[x + 1][y - 1] && Case[x + 1][y - 1] == Case[x + 2][y - 2] && Case[x + 2][y - 2] == Case[x + 3][y - 3])
                            { Case[x][y] = Case[x + 1][y - 1] = Case[x + 2][y - 2] = Case[x + 3][y - 3] = Turn + 3; return true; }
                        }
                    }
                }
            }
            return false;
        }

        private bool CheckTie()
        {
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (Case[x][y] == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private string RenderBoard()
        {
            string s = ":one::two::three::four::five::six::seven:\n\n";
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (Case[x][y] == 0)
                    {
                        s = s + ":white_circle:";
                    }

                    if (Case[x][y] == 1)
                    {
                        s = s + ":red_circle:";
                    }

                    if (Case[x][y] == 2)
                    {
                        s = s + ":large_blue_circle:";
                    }

                    if (Case[x][y] == 3)
                    {
                        s = s + "<:Gold_Red_Circle:285466048084312065>";
                    }

                    if (Case[x][y] == 4)
                    {
                        s = s + "<:Gold_Blue_Circle:285466046494670848>";
                    }
                }
                s = s + '\n';
            }
            return s;
        }

        private int Opposite()
        {
            if (Turn == 0)
            {
                return 1;
            }

            return 0;
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

        private void Save()
        {
            char s;
            string[] Data = File.ReadAllLines("../../Connect_Data.txt");
            if (Data.Length < ID + 1)
            { s = Turn.ToString()[0]; Array.Resize(ref Data, Data.Length + 1); }
            else
            {
                s = Data[ID][27];
            }

            Data[ID] = "Match[" + Inttodigits(ID, 4) + "] 0 Win{";
            if (CheckWin())
            {
                Data[ID] = Data[ID] + Turn + "} Start{" + s + "} Moves{";
            }
            else if (CheckTie())
            {
                Data[ID] = Data[ID] + "T} Start{" + s + "} Moves{";
            }
            else
            {
                Data[ID] = Data[ID] + "X} Start{" + s + "} Moves{";
            }

            int i = 0; string S = "#";
            for (int y = 5; y >= 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    S = S + Case[x][y];
                    if (Case[x][y] != 0)
                    {
                        i++;
                    }
                }
            }
            Data[ID] = Data[ID] + Inttodigits(i, 2) + "} P1{" + Player[0].Name + "} P2{" + Player[1].Name + "} " + S;
            File.WriteAllLines("../../Connect_Data.txt", Data);
        }
    }
}
