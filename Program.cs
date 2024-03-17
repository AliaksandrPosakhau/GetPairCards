using System;
using SFML.Learning;
using SFML.System;
using SFML.Window;
 
 
    class FindCouple : Game
    {
    static int[,] cards;
    static string[] iconsName;

    static int INITIAL_DELAY_MILLISEC = 2000;
    static uint WINDOW_WIDTH = 800;
    static uint WINDOW_HEIGHT = 600;
    static string WINDOW_GAME_TITLE = "Find couple 1.0";

    static byte WINDOW_COLOR_1 = 255;
    static byte WINDOW_COLOR_2 = 60;
    static byte WINDOW_COLOR_3 = 200;

    static string pairSound = LoadSound("card_pick.wav");
    static string endGameSuccess = LoadSound("pigSuccess.wav");
    static string pairFoundSuccess = LoadSound("pairFoundSuccess.wav");

    static int cardCounter = 20;
    static int cardWidth = 100;
    static int cardHeigh = 100;

    static int countPerLine = 5;
    static int space = 40;
    static int leftOffset = 70;
    static int topOffset = 20;

    static void loadIcons()
    {        
        iconsName = new string[7];
        iconsName[0] = LoadTexture("Icon_closed.png");
        for (int i = 1; i < iconsName.Length; i++)
        {
            iconsName[i] = LoadTexture("Icon_"+(i).ToString()+".png");
        } 
    }

    static void Shuffle(int[] arr)
    {
        Random random = new Random();
        for (int i = arr.Length - 1; i >= 1; i--) {
            int j = random.Next(1, i + 1);
            int tmp = arr[j];
            arr[j] = arr[i];
            arr[i] = tmp;
        }
    }
    static void InitCard()
    {
        Random rnd = new Random();
        cards = new int[cardCounter, 6];

        int[] iconID = new int[cards.GetLength(0)];
        int id = 0;
        for (int i=0;i<iconID.Length;++i)
        {
            if (i % 2 == 0)
            {
                id = rnd.Next(1, 7);
            }

            iconID[i] = id;
        }

        Shuffle(iconID);
        Shuffle(iconID);
        Shuffle(iconID);

        for (int i = 0; i < cards.GetLength(0); i++) {
            cards[i,0] = 0; // state
            cards[i, 1] = (i%countPerLine)*(cardWidth+space)+leftOffset;// position X
            cards[i, 2] = (i / countPerLine) * (cardWidth + space) + topOffset;//position Y
            cards[i, 3] = cardWidth;// width
            cards[i, 4] = cardHeigh;// height
            cards[i, 5] = iconID[i];// id
        }
    }

    static void DrawCards()
    {
        for(int i = 0;i<cards.GetLength(0);i++) {

            if (cards[i,0] == 1) // card opened
            {
                DrawSprite(iconsName[cards[i, 5]], cards[i,1], cards[i,2]);              
            }

            if (cards[i, 0] == 0) // card closed
            {
                DrawSprite(iconsName[0], cards[i, 1], cards[i, 2]);                 
            }            
        }
    }

    static void SetALlCardsState(int state)
    {
        for (int i = 0; i < cards.GetLength(0); i++)
        {
            cards[i, 0] = state;
        }
    }
        
    static int getCardIndexByMousePosition()
    {
        for(int i=0;i<cards.GetLength(0);++i)
        {
            if(MouseX >= cards[i,1] && MouseX <= cards[i, 1] + cards[i,3] && MouseY >= cards[i,2] && MouseY <= cards[i, 2] + cards[i,4]) { return i; }
        }

        return -1;
    }
    
    static void Main(string[] args)
        {

        int openCardAmount = 0;
        int firstOpenCardIndex = -1;
        int secondOpenCardIndex = -1;
        int remainingCardsAmount = cardCounter;

        loadIcons();
        SetFont("comic.ttf");

        InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_GAME_TITLE);
        InitCard();
        SetALlCardsState(1);
        ClearWindow(WINDOW_COLOR_1, WINDOW_COLOR_2, WINDOW_COLOR_3);
        DrawCards();
        DisplayWindow();
        Delay(INITIAL_DELAY_MILLISEC);
        SetALlCardsState(0);
         
            while (true)
            {
            DispatchEvents();

            if (remainingCardsAmount == 0) {break; }

            if (openCardAmount==2)
            {
                if (cards[firstOpenCardIndex,5] == cards[secondOpenCardIndex,5])
                {
                    cards[firstOpenCardIndex, 0] = -1;
                    cards[secondOpenCardIndex,0] = -1;
                    remainingCardsAmount-=2;
                    PlaySound(pairFoundSuccess);                    

                } else
                {
                    cards[firstOpenCardIndex, 0] = 0;
                    cards[secondOpenCardIndex, 0] = 0;
                }

                firstOpenCardIndex = -1;
                secondOpenCardIndex= -1;
                openCardAmount = 0;

                Delay(2000);
            }
            
            if(GetMouseButtonDown(0) == true ) 
            {
                int index = getCardIndexByMousePosition();
                if (index!=-1 && index!=firstOpenCardIndex)
                {
                    cards[index, 0] = 1;
                    openCardAmount++;
                    PlaySound(pairSound);

                    if (openCardAmount == 1) firstOpenCardIndex = index;
                    if (openCardAmount == 2) secondOpenCardIndex = index;
                }
            }
            
            ClearWindow(WINDOW_COLOR_1, WINDOW_COLOR_2, WINDOW_COLOR_3);
            DrawCards();
            DisplayWindow();
           
            Delay(1);
            }

        ClearWindow();
        SetFillColor(255, 255, 255);
        DrawText(200, 300, "Congratulations! You have opened all cards!",22);
        PlaySound(endGameSuccess);
        DisplayWindow();        
        Delay(5000);
        }
    }
 
