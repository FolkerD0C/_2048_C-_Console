using System;
using System.Threading;
using System.IO;
namespace _2048Game
{
    class Game
    {
        static void Main()
        {
            if (!Directory.Exists("2048"))
            {
                Directory.CreateDirectory("2048");
            }
            Console.Title = "2048";
            Console.CursorVisible = false;
            string[] maiN = { "New Game", "Load Game", "Highscores", "Controls", "Exit" };
            Menu MainMenu = new Menu(maiN, 24, 17);
            int saveCount = 0;
            Menu LoadGame = GettingSaveGames(ref saveCount);
            string[] loading = { "Load", "Delete", "Back" };
            Menu Loading = new Menu(loading, 34, 17);
            string[] back = { "Back" };
            Menu Back = new Menu(back, 34, 17);
            bool inGame = true;
            while (inGame)
            {
                MainMenu.ClearLines(0, 17);
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Welcome to the game!\n");
                MainMenu.InMenu = true;
                MainMenu.Navigation(2);
                switch (MainMenu.CursorPos)
                {
                    case 0: //New Game
                        {
                            Console.Clear();
                            if (saveCount != 7)
                            {
                                Grid playNew = new Grid($"2048\\2048save{saveCount + 1}.levi");
                                playNew.Game();
                                if (playNew.YouWon || playNew.Cheated)
                                {
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        Console.Clear();
                                        playNew.Continuing();
                                    }
                                    else
                                    {
                                        Grid.ClearLine(Console.CursorTop);
                                        Thread.Sleep(800);
                                    }
                                }
                                if (!playNew.Saved && !playNew.Cheated && !playNew.CheatedBeyond)
                                {
                                    playNew.HighScoreTracking();
                                }
                            }
                            else
                            {
                                Grid playNew = new Grid($"2048\\2048save7.levi");
                                playNew.Game();
                                if (playNew.YouWon || playNew.Cheated)
                                {
                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                    {
                                        Console.Clear();
                                        playNew.Continuing();
                                    }
                                    else
                                    {
                                        Grid.ClearLine(Console.CursorTop);
                                        Thread.Sleep(800);
                                    }
                                }
                                if (!playNew.Saved && !playNew.Cheated && !playNew.CheatedBeyond)
                                {
                                    playNew.HighScoreTracking();
                                }
                            }
                            break;
                        }
                    case 1: //Load Game
                        {
                            bool inLoading = true;
                            while (inLoading)
                            {
                                LoadGame.ClearLines(0, 17);
                                Console.SetCursorPosition(0, 0);
                                LoadGame.InMenu = true;
                                LoadGame.Navigation(0);
                                Console.Clear();
                                if (LoadGame.CursorPos != LoadGame.ListOfThings.Length - 1)
                                {
                                    Console.WriteLine(LoadGame.ListOfThings[LoadGame.CursorPos].Name);
                                    Loading.InMenu = true;
                                    Loading.Navigation(1);
                                    switch (Loading.CursorPos)
                                    {
                                        case 0:
                                            {
                                                inLoading = false;
                                                Grid loadedGame = new Grid($"2048\\2048save{LoadGame.CursorPos + 1}.levi");
                                                Grid.ClearLine(0);
                                                loadedGame.Loading();
                                                if (loadedGame.YouWon || loadedGame.Cheated && loadedGame.YouWon)
                                                {
                                                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                                                    {
                                                        Console.Clear();
                                                        loadedGame.Continuing();
                                                    }
                                                    else
                                                    {
                                                        Grid.ClearLine(Console.CursorTop);
                                                        Thread.Sleep(800);
                                                    }
                                                }
                                                if (!loadedGame.Saved && !loadedGame.Cheated && !loadedGame.CheatedBeyond)
                                                {
                                                    loadedGame.HighScoreTracking();
                                                }
                                                break;
                                            }
                                        case 1:
                                            {
                                                File.Delete($"2048\\2048save{LoadGame.CursorPos + 1}.levi");
                                                OrganizingSaves(LoadGame.CursorPos + 1, saveCount);
                                                LoadGame = GettingSaveGames(ref saveCount);
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    inLoading = false;
                                }
                            }
                            break;
                        }
                    case 2: //Highscores
                        {
                            Console.Clear();
                            Console.SetCursorPosition(0, 0);
                            string[] highs = File.ReadAllLines("2048\\2048highscores.levi");
                            Console.WriteLine("HIGHSCORES\n");
                            for (int i = 0; i < highs.Length; i++)
                            {
                                Console.WriteLine(highs[i]);
                            }
                            Back.InMenu = true;
                            Back.Navigation(13);
                            break;
                        }
                    case 3: //Controls
                        {
                            Console.Clear();
                            Console.SetCursorPosition(0, 0);
                            Console.WriteLine("Controls: arrows or WASD\nBackspace/Space-Undo\nM-Save\nESC-Give up");
                            Back.InMenu = true;
                            Back.Navigation(5);
                            break;
                        }
                    case 4: //Exit
                        {
                            Exiting();
                            inGame = false;
                            break;
                        }
                    default: break;
                }
                LoadGame = GettingSaveGames(ref saveCount);
            }
        }
        static Menu GettingSaveGames(ref int saveCount)
        {
            saveCount = 0;
            for (int i = 1; i <= 7; i++)
            {
                if (!File.Exists($"2048\\2048save{i}.levi"))
                {
                    saveCount = i - 1;
                    i = 8;
                }
                else if (File.Exists($"2048\\2048save{i}.levi") && i == 7)
                {
                    saveCount = i;
                }
            }
            string[] savesTemp = new string[saveCount];
            if (saveCount > 0)
            {
                for (int i = 1; i <= saveCount; i++)
                {
                    savesTemp[i - 1] = $"Save Game {i} - ";
                    savesTemp[i - 1] += File.ReadAllText($"2048\\2048save{i}.levi").Split('*')[0] + " Points";
                }
            }
            string[] saves = new string[savesTemp.Length + 1];
            for (int i = 0; i < savesTemp.Length; i++)
            {
                saves[i] = savesTemp[i];
            }
            saves[saves.Length - 1] = "Back";
            return new Menu(saves, 34, 17);
        }
        static void OrganizingSaves(int deletedSaveNumb, int saveCount)
        {
            if (deletedSaveNumb < saveCount)
            {
                for (int i = deletedSaveNumb; i < saveCount; i++)
                {
                    File.WriteAllText($"2048\\2048save{i}.levi", File.ReadAllText($"2048\\2048save{i + 1}.levi"));
                    File.Delete($"2048\\2048save{i + 1}.levi");
                }
            }
            else if (deletedSaveNumb < 7 && deletedSaveNumb != saveCount)
            {
                for (int i = deletedSaveNumb; i < 7; i++)
                {
                    File.WriteAllText($"2048\\2048save{i}.levi", File.ReadAllText($"2048\\2048save{i + 1}.levi"));
                    File.Delete($"2048\\2048save{i + 1}.levi");
                }
            }
        }
        /*ExitingAnimation*/
        static void Exiting()
        {
            for (int i = 10; i < 16; i++)
            {
                Grid.ClearLine(i);
            }
            Console.SetCursorPosition(0, 15);
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Exiting...");
                Thread.Sleep(400);
                Console.SetCursorPosition(0, 15); 
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
                Thread.Sleep(100);
            }
            Console.WriteLine("Exiting...");
            Thread.Sleep(400);
        }
    }
    internal class Grid
    {
        #region Properties
        int[,] playingGrid;
        int[,] copied;
        bool gameOver;
        bool youWon;
        int[,] availableSpaces;
        bool canMoveRight;
        bool canMoveLeft;
        bool canMoveUp;
        bool canMoveDown;
        bool moved;
        bool cheated;
        bool cheatedBeyond;
        bool added;
        bool saved;
        int points;
        int pointsTemp;
        string saveGamePath;
        bool beyond2048;
        public bool YouWon { get { return youWon; } }
        public bool Saved { get { return saved; } }
        public bool Cheated { get { return cheated; } }
        public bool CheatedBeyond { get { return cheatedBeyond; } }
        public bool Beyond2048 { get { return beyond2048; } set { beyond2048 = value; } }
        #endregion
        #region StaticProperties
        static Random rnd = new Random();
        static int[] intPool = { 2, 4 };
        #endregion
        /*ConstructorAndFileManagement*/
        public Grid(string path)
        {
            playingGrid = new int[4, 4];
            playingGrid[rnd.Next(0, 4), rnd.Next(0, 4)] = intPool[rnd.Next(0, 2)];
            bool placed = false;
            while (!placed)
            {
                int x = rnd.Next(0, 4);
                int y = rnd.Next(0, 4);
                if (playingGrid[x, y] == 0)
                {
                    playingGrid[x, y] = intPool[rnd.Next(0, 2)];
                    placed = true;
                }
            }
            availableSpaces = new int[14, 2];
            int aS = 0;
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        availableSpaces[aS, 0] = i;
                        availableSpaces[aS, 1] = j;
                        aS++;
                    }
                }
            }
            IsGameOver();
            copied = new int[4, 4];
            Copy();
            canMoveUp = true;
            canMoveRight = true;
            saveGamePath = path;
            if (!File.Exists("2048\\2048highscores.levi"))
            {
                File.WriteAllText("2048\\2048highscores.levi", "1. 50000 Levixd\n2. 45000 Levixd\n3. 40000 Levixd\n4. 35000 Levixd\n5. 30000 Levixd\n6. 25000 Levixd\n7. 20000 Levixd\n8. 15000 Levixd\n9. 10000 Levixd\n10. 5000 Levixd");
            }
        }
        public void Loading()
        {
            string loadingIn = File.ReadAllText(saveGamePath);
            points = int.Parse(loadingIn.Split('*')[0]);
            pointsTemp= int.Parse(loadingIn.Split('*')[4]);
            if (int.Parse(loadingIn.Split('*')[2]) == 1)
            {
                beyond2048 = true;
            }
            string[] gridLoad = loadingIn.Split('*')[1].Split('|');
            string[][] gridMatrix = new string[gridLoad.Length][];
            for (int i = 0; i < gridLoad.Length; i++)
            {
                gridMatrix[i] = gridLoad[i].Split(' ');
            }
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    playingGrid[i, j] = int.Parse(gridMatrix[i][j]);
                }
            }
            string[] gridLoad2 = loadingIn.Split('*')[3].Split('|');
            string[][] gridMatrix2 = new string[gridLoad.Length][];
            for (int i = 0; i < gridLoad2.Length; i++)
            {
                gridMatrix2[i] = gridLoad2[i].Split(' ');
            }
            for (int i = 0; i < copied.GetLength(0); i++)
            {
                for (int j = 0; j < copied.GetLength(1); j++)
                {
                    copied[i, j] = int.Parse(gridMatrix2[i][j]);
                }
            }
            if (!beyond2048)
            {
                Game();
            }
            else
            {
                Continuing();
            }
        }
        void Saving()
        {
            if (File.Exists(saveGamePath))
            {
                File.Delete(saveGamePath);
            }
            string saveString = "";
            saveString += points + "*";
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    saveString+=playingGrid[i, j] + " ";
                }
                if (i != playingGrid.GetLength(0) - 1)
                {
                    saveString += "|";
                }
            }
            if (beyond2048)
            {
                saveString += "*1*";
            }
            else
            {
                saveString += "*0*";
            }
            for (int i = 0; i < copied.GetLength(0); i++)
            {
                for (int j = 0; j < copied.GetLength(1); j++)
                {
                    saveString += copied[i, j] + " ";
                }
                if (i != copied.GetLength(0) - 1)
                {
                    saveString += "|";
                }
            }
            saveString += "*" + pointsTemp;
            File.WriteAllText(saveGamePath, saveString);
        }
        public void HighScoreTracking()
        {
            string[] scores = File.ReadAllLines("2048\\2048highscores.levi");
            int[] high = new int[10];
            for (int i = 0; i < scores.Length; i++)
            {
                high[i] = int.Parse(scores[i].Split(' ')[1]);
            }
            int x = -1;
            for (int i = 0; i < high.Length; i++)
            {
                if (points > high[i])
                {
                    x = i;
                    i = high.Length;
                }
            }
            if (x > -1)
            {
                ClearLine(Console.CursorTop);
                Console.WriteLine("You set a new record!\nType your name:");
                string name = Console.ReadLine();
                for (int i = scores.Length - 1; i >= x + 1; i--)
                {
                    if (i > 0)
                    {
                        scores[i] = scores[i - 1];
                    }
                }
                scores[x] = $"{x + 1}. {points} {name}";
                File.WriteAllLines("2048\\2048highscores.levi", scores);
            }
        }
        /*HandlingTurns*/
        public void Game()
        {
            Console.WindowHeight = 17;
            Console.WindowWidth = 24;
            Printing();
            while (!gameOver && !youWon && !cheated && !saved)
            {
                KeyPress();
                AvailableSpacesChange();
                NewRandom();
                Printing();
                IsGameOver();
            }
            if (saved)
            {
                Console.WriteLine("Saving...");
                Thread.Sleep(2000);
            }
            else if (cheated)
            {
                for (int i = 0; i < playingGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < playingGrid.GetLength(1); j++)
                    {
                        playingGrid[i, j] = 2048;
                    }
                }
                Printing();
                youWon= true;
                Console.WriteLine("You won you CHEATER!\nPress ENTER to continue\nor any other key to\nexit to the main menu!");
            }
            else if (youWon)
            {
                Console.WriteLine("You won!\nPress ENTER to continue\nor any other key to\nexit to the main menu!");
            }
            else
            {
                Console.WriteLine("You lost!\nPress any key to\nexit to the main menu!");
            }
        }
        public void Continuing()
        {
            Console.WindowHeight = 17;
            Console.WindowWidth = 32;
            beyond2048 = true;
            PrintingBeyond();
            while (!gameOver && !saved && !cheatedBeyond)
            {
                KeyPress();
                AvailableSpacesChange();
                NewRandom();
                PrintingBeyond();
                IsGameOver();
            }
            if (saved)
            {
                Console.WriteLine("Saving...");
                Thread.Sleep(2000);
            }
            else if (cheatedBeyond)
            {
                youWon = true;
                for (int i = 0; i < playingGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < playingGrid.GetLength(1); j++)
                    {
                        playingGrid[i, j] = 131072;
                    }
                }
                PrintingBeyond();
                Console.WriteLine("You won you CHEATER!\nPress any key to\nexit to main menu!");
                Console.ReadKey();
                ClearLine(Console.CursorTop);
            }
            else
            {
                Console.WriteLine("You lost!\nPress any key to\nexit to main menu!");
                Console.ReadKey();
                ClearLine(Console.CursorTop);
            }
        }
        /*PrivateMethods*/
        #region HandlingInputs
        void KeyPress()
        {
            bool success = false;
            while (!success)
            {
                var kP = Console.ReadKey();
                switch (kP.Key)
                {
                    case ConsoleKey.UpArrow: case ConsoleKey.W:
                        {
                            ClearLine(Console.CursorTop);
                            if (canMoveUp)
                            {
                                Copy();
                                ShiftingUp();
                                success = true;
                            }
                            break;
                        }
                    case ConsoleKey.DownArrow: case ConsoleKey.S:
                        {
                            ClearLine(Console.CursorTop);
                            if (canMoveDown)
                            {
                                Copy();
                                ShiftingDown();
                                success = true;
                            }
                            break;
                        }
                    case ConsoleKey.LeftArrow: case ConsoleKey.A:
                        {
                            ClearLine(Console.CursorTop);
                            if (canMoveLeft)
                            {
                                Copy();
                                ShiftingLeft();
                                success = true;
                            }
                            break;
                        }
                    case ConsoleKey.RightArrow: case ConsoleKey.D:
                        {
                            ClearLine(Console.CursorTop);
                            if (canMoveRight)
                            {
                                Copy();
                                ShiftingRight();
                                success = true;
                            }
                            break;
                        }
                    case ConsoleKey.Backspace: case ConsoleKey.Spacebar:
                        {
                            Undo();
                            IsGameOver();
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            gameOver = true;
                            success = true;
                            break;
                        }
                    case ConsoleKey.M:
                        {
                            Saving();
                            success = true;
                            saved = true;
                            break;
                        }
                    case ConsoleKey.C:
                        {
                            ClearLine(Console.CursorTop);
                            if (Console.ReadKey().Key == ConsoleKey.H)
                            {
                                ClearLine(Console.CursorTop);
                                if (Console.ReadKey().Key == ConsoleKey.E)
                                {
                                    ClearLine(Console.CursorTop);
                                    if (Console.ReadKey().Key == ConsoleKey.A)
                                    {
                                        ClearLine(Console.CursorTop);
                                        if (Console.ReadKey().Key == ConsoleKey.T)
                                        {
                                            ClearLine(Console.CursorTop);
                                            cheated = true;
                                            if (beyond2048)
                                            {
                                                cheatedBeyond = true;
                                            }
                                            success = true;
                                        }
                                    }
                                }
                            }
                            ClearLine(Console.CursorTop);
                            break;
                        }
                    default:
                        ClearLine(Console.CursorTop);
                        break;
                }
            }
        }
        #region MovingUpDownLeftRight
        void ShiftingUp()
        {
            for (int j = 0; j < playingGrid.GetLength(1); j++)
            {
                for (int i = 0; i < playingGrid.GetLength(0) - 1; i++)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = i + 1; k < playingGrid.GetLength(0); k++)
                        {
                            Move(k, j, k - 1, j);
                        }
                        if (moved)
                        {
                            i--;
                        }
                        moved = false;
                    }
                }
            }
            if (!beyond2048)
            {
                Printing();
            }
            else
            {
                PrintingBeyond();
            }
            Thread.Sleep(70);
            for (int j = 0; j < playingGrid.GetLength(1); j++)
            {
                for (int i = 0; i < playingGrid.GetLength(0) - 1; i++)
                {
                    if (playingGrid[i + 1, j] == playingGrid[i, j])
                    {
                        Adding(i + 1, j, i, j);
                        added = true;
                    }
                }
            }
            if (added)
            {
                added = false;
                if (!beyond2048)
                {
                    Printing();
                }
                else
                {
                    PrintingBeyond();
                }
                Thread.Sleep(30);
            }
            for (int j = 0; j < playingGrid.GetLength(1); j++)
            {
                for (int i = 0; i < playingGrid.GetLength(0) - 1; i++)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = i + 1; k < playingGrid.GetLength(0); k++)
                        {
                            Move(k, j, k - 1, j);
                        }
                        if (moved)
                        {
                            i--;
                        }
                        moved = false;
                    }
                }
            }
        }
        void ShiftingDown()
        {
            for (int j = 0; j < playingGrid.GetLength(1); j++)
            {
                for (int i = playingGrid.GetLength(0) - 1; i > 0; i--)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = i - 1; k > -1; k--)
                        {
                            Move(k, j, k + 1, j);
                        }
                        if (moved)
                        {
                            i++;
                        }
                        moved = false;
                    }
                }
            }
            if (!beyond2048)
            {
                Printing();
            }
            else
            {
                PrintingBeyond();
            }
            Thread.Sleep(70);
            for (int j = 0; j < playingGrid.GetLength(1); j++)
            {
                for (int i = playingGrid.GetLength(0) - 1; i > 0; i--)
                {
                    if (playingGrid[i - 1, j] == playingGrid[i, j])
                    {
                        Adding(i - 1, j, i, j);
                        added = true;
                    }
                }
            }
            if (added)
            {
                added = false;
                if (!beyond2048)
                {
                    Printing();
                }
                else
                {
                    PrintingBeyond();
                }
                Thread.Sleep(30);
            }
            for (int j = 0; j < playingGrid.GetLength(1); j++)
            {
                for (int i = playingGrid.GetLength(0) - 1; i > 0; i--)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = i - 1; k > -1; k--)
                        {
                            Move(k, j, k + 1, j);
                        }
                        if (moved)
                        {
                            i++;
                        }
                        moved = false;
                    }
                }
            }
        }
        void ShiftingLeft()
        {
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1) - 1; j++)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = j + 1; k < playingGrid.GetLength(1); k++)
                        {
                            Move(i, k, i, k - 1);
                        }
                        if (moved)
                        {
                            j--;
                        }
                        moved = false;
                    }
                }
            }
            if (!beyond2048)
            {
                Printing();
            }
            else
            {
                PrintingBeyond();
            }
            Thread.Sleep(70);
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1) - 1; j++)
                {
                    if (playingGrid[i, j + 1] == playingGrid[i, j])
                    {
                        Adding(i, j + 1, i, j);
                        added = true;
                    }
                }
            }
            if (added)
            {
                added = false;
                if (!beyond2048)
                {
                    Printing();
                }
                else
                {
                    PrintingBeyond();
                }
                Thread.Sleep(30);
            }
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1) - 1; j++)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = j + 1; k < playingGrid.GetLength(1); k++)
                        {
                            Move(i, k, i, k - 1);
                        }
                        if (moved)
                        {
                            j--;
                        }
                        moved = false;
                    }
                }
            }
        }
        void ShiftingRight()
        {
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = playingGrid.GetLength(1) - 1; j > 0; j--)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = j - 1; k > -1; k--)
                        {
                            Move(i, k, i, k + 1);
                        }
                        if (moved)
                        {
                            j++;
                        }
                        moved = false;
                    }
                }
            }
            if (!beyond2048)
            {
                Printing();
            }
            else
            {
                PrintingBeyond();
            }
            Thread.Sleep(70);
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = playingGrid.GetLength(1) - 1; j > 0; j--)
                {
                    if (playingGrid[i, j - 1] == playingGrid[i, j])
                    {
                        Adding(i, j - 1, i, j);
                        added = true;
                    }
                }
            }
            if (added)
            {
                added = false;
                if (!beyond2048)
                {
                    Printing();
                }
                else
                {
                    PrintingBeyond();
                }
                Thread.Sleep(30);
            }
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = playingGrid.GetLength(1) - 1; j > 0; j--)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        for (int k = j - 1; k > -1; k--)
                        {
                            Move(i, k, i, k + 1);
                        }
                        if (moved)
                        {
                            j++;
                        }
                        moved = false;
                    }
                }
            }
        }
        #endregion
        void Adding(int xPos1, int yPos1, int xPos2, int yPos2)
        {
            playingGrid[xPos2, yPos2] = 2 * playingGrid[xPos1, yPos1];
            playingGrid[xPos1, yPos1] = 0;
            points += playingGrid[xPos2, yPos2];
            if (playingGrid[xPos2, yPos2] == 2048 && !beyond2048)
            {
                youWon = true;
            }
        }
        void Move(int xPos1, int yPos1, int xPos2, int yPos2)
        {
            if (playingGrid[xPos1, yPos1] != 0)
            {
                playingGrid[xPos2, yPos2] = playingGrid[xPos1, yPos1];
                playingGrid[xPos1, yPos1] = 0;
                moved = true;
            }
        }
        void Copy()
        {
            pointsTemp = points;
            for (int i = 0; i < copied.GetLength(0); i++)
            {
                for (int j = 0; j < copied.GetLength(1); j++)
                {
                    copied[i, j] = playingGrid[i, j];
                }
            }
        }
        void Undo()
        {
            points = pointsTemp;
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    playingGrid[i, j] = copied[i, j];
                }
            }
            if (!beyond2048)
            {
                Printing();
            }
            else
            {
                PrintingBeyond();
            }
        }
        #endregion
        #region Analysis
        void AvailableSpacesChange()
        {
            int aS = 0;
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        aS++;
                    }
                }
            }
            availableSpaces = new int[aS, 2];
            int aSi = 0;
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    if (playingGrid[i, j] == 0)
                    {
                        availableSpaces[aSi, 0] = i;
                        availableSpaces[aSi, 1] = j;
                        aSi++;
                    }
                }
            }
        }
        void NewRandom()
        {
            if (availableSpaces.GetLength(0) != 0 && !gameOver && !saved)
            {
                int aSIndex = rnd.Next(0, availableSpaces.GetLength(0));
                playingGrid[availableSpaces[aSIndex, 0], availableSpaces[aSIndex, 1]] = intPool[rnd.Next(0, 2)];
            }
        }
        void IsGameOver()
        {
            #region Up
            canMoveUp = false;
            int jU = 0;
            while (jU < playingGrid.GetLength(1) && !canMoveUp)
            {
                int iU = 0;
                while (iU < playingGrid.GetLength(0) - 1 && !canMoveUp)
                {
                    if (playingGrid[iU, jU] == playingGrid[iU + 1, jU] && playingGrid[iU + 1, jU] != 0 || iU < playingGrid.GetLength(0) - 1 && playingGrid[iU, jU] == 0 && playingGrid[iU, jU] != playingGrid[iU + 1, jU])
                    {
                        canMoveUp = true;
                    }
                    iU++;
                }
                jU++;
            }
            #endregion
            #region Down
            canMoveDown = false;
            int jD = 0;
            while (jD < playingGrid.GetLength(1) && !canMoveDown)
            {
                int iD = playingGrid.GetLength(0) - 1;
                while (iD > 0 && !canMoveDown)
                {
                    if (playingGrid[iD, jD] == playingGrid[iD - 1, jD] && playingGrid[iD - 1, jD] != 0 || iD > 0 && playingGrid[iD, jD] == 0 && playingGrid[iD, jD] != playingGrid[iD - 1, jD])
                    {
                        canMoveDown = true;
                    }
                    iD--;
                }
                jD++;
            }
            #endregion
            #region Right
            canMoveRight = false;
            int iR = 0;
            while (iR < playingGrid.GetLength(0) && !canMoveRight)
            {
                int jR = playingGrid.GetLength(1) - 1;
                while (jR > 0 && !canMoveRight)
                {
                    if (playingGrid[iR, jR] == playingGrid[iR, jR - 1] && playingGrid[iR, jR - 1] != 0 || jR > 0 && playingGrid[iR, jR] == 0 && playingGrid[iR, jR] != playingGrid[iR, jR - 1])
                    {
                        canMoveRight = true;
                    }
                    jR--;
                }
                iR++;
            }
            #endregion
            #region Left
            canMoveLeft = false;
            int iL = 0;
            while (iL < playingGrid.GetLength(0) && !canMoveLeft)
            {
                int jL = 0;
                while (jL < playingGrid.GetLength(1) - 1 && !canMoveLeft)
                {
                    if (playingGrid[iL, jL] == playingGrid[iL, jL + 1] && playingGrid[iL, jL + 1] != 0 || jL < playingGrid.GetLength(1) - 1 && playingGrid[iL, jL] == 0 && playingGrid[iL, jL] != playingGrid[iL, jL + 1])
                    {
                        canMoveLeft = true;
                    }
                    jL++;
                }
                iL++;
            }
            #endregion
            if (!canMoveUp && !canMoveDown && !canMoveRight && !canMoveLeft)
            {
                gameOver = true;
            }
        }
        #endregion
        /*PublicMethods*/
        public void Printing()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Points: " + points);
            Console.WriteLine("+----+----+----+----+");
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                Console.Write("|");
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    switch (playingGrid[i, j])
                    {
                        case 2:
                            {
                                Console.Write("   2");
                                break;
                            }
                        case 4:
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("   4");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 8:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write("   8");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 16:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write("  16");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 32:
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("  32");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 64:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.Write("  64");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 128:
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(" 128");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 256:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write(" 256");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 512:
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(" 512");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 1024:
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("1024");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 2048:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("2048");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        default:
                            Console.Write("    ");
                            break;
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
                Console.WriteLine("+----+----+----+----+");
            }
        }
        public void PrintingBeyond()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Points: " + points);
            Console.WriteLine("+------+------+------+------+");
            for (int i = 0; i < playingGrid.GetLength(0); i++)
            {
                Console.Write("|");
                for (int j = 0; j < playingGrid.GetLength(1); j++)
                {
                    switch (playingGrid[i, j])
                    {
                        case 2:
                            {
                                Console.Write("     2");
                                break;
                            }
                        case 4:
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("     4");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 8:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write("     8");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 16:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write("    16");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 32:
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("    32");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 64:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.Write("    64");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 128:
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("   128");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 256:
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write("   256");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 512:
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("   512");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 1024:
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.Write("  1024");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 2048:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("  2048");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 4096:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("  4096");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 8192:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("  8192");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 16384:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" 16384");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 32768:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" 32786");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 65536:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(" 65536");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        case 131072:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("131072");
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            }
                        default:
                            Console.Write("      ");
                            break;
                    }
                    Console.Write("|");
                }
                Console.WriteLine();
                Console.WriteLine("+------+------+------+------+");
            }
        }
        public static void ClearLine( int top)
        {
            Console.SetCursorPosition(0, top);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
    internal class Menu 
    {
        MenuItem[] listOfThings;
        int cursorPos;
        bool inMenu;
        int sizeHeight;
        int sizeWidth;
        public MenuItem[] ListOfThings { get { return listOfThings; } }
        public int CursorPos { get { return cursorPos; } }
        public bool InMenu { get { return inMenu; } set { inMenu = value; } }
        public Menu(string[] toOutput, int width, int height)
        {
            listOfThings = new MenuItem[toOutput.Length];
            for (int i = 0; i < listOfThings.Length; i++) { listOfThings[i] = new MenuItem(toOutput[i]); }
            cursorPos = 0;
            inMenu = true;
            sizeHeight = height;
            sizeWidth = width;
        }
        public void Navigation(int pos)
        {
            Console.WindowHeight = sizeHeight;
            Console.WindowWidth = sizeWidth;
            cursorPos = 0;
            while (inMenu) { Printing(pos); InputHandler(); }
        }
        void InputHandler()
        {
            bool success = false;
            while (!success)
            {
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.UpArrow: case ConsoleKey.W: { Grid.ClearLine(Console.CursorTop); if (cursorPos > 0) { cursorPos--; } else { cursorPos = listOfThings.Length - 1; } success = true; break; }
                    case ConsoleKey.DownArrow: case ConsoleKey.S: { Grid.ClearLine(Console.CursorTop); if (cursorPos < listOfThings.Length - 1) { cursorPos++; } else { cursorPos = 0; } success = true; break; }
                    case ConsoleKey.Enter: case ConsoleKey.Spacebar: { inMenu = false; success = true; break; }
                    default: Grid.ClearLine(Console.CursorTop); break;
                }
            }
        }
        public void Printing(int consolePos)
        {
            ClearLines(consolePos, consolePos + listOfThings.Length - 1);
            Console.SetCursorPosition(0, consolePos);
            for (int i = 0; i < listOfThings.Length; i++)
            {
                if (i == cursorPos)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{listOfThings[i].Name}]");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine(listOfThings[i].Name);
                }
            }
        }
        public void ClearLines(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                Console.SetCursorPosition(0, i);
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }
        }
    }
    internal class MenuItem
    {
        string name;
        public string Name { get { return name; } }
        public MenuItem(string n) { name = n; }
    }
}