using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238 

namespace _2048
{
    public class ActionEvent
    {
        public int fromx, fromy;
        public int tox, toy;
        public int ResultNum;
        public ActionEvent()
        {
            fromx = 0;
            fromy = 0;
            tox = 0;
            toy = 0;
            ResultNum = 0;
        }
        public ActionEvent(int _fromx, int _fromy, int _tox, int _toy, int result)
        {
            this.fromx = _fromx;
            this.fromy = _fromy;
            this.tox = _tox;
            this.toy = _toy;
            this.ResultNum = result;
        }
    }
    public class POINT
    {
        public int x, y;
        public POINT(int _x,int _y)
        {
            x = _x;
            y = _y;
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary
    public sealed partial class MainPage : Page
    {
        //Key event action
        VirtualKey Left = VirtualKey.A;
        VirtualKey Right = VirtualKey.D;
        VirtualKey Up = VirtualKey.W;
        VirtualKey Down = VirtualKey.S;
        //

        private Size CommonSize = new Size(1024, 768);
        private Rectangle[] DivideRectangle; // chia bảng trên map
        private Grid[,] Block;// khối ô vuông
        private TextBlock[,] TextInBlock; // số trong 1 ô vuông
        private Thickness[,] BlockMargin; //the margin before the action on
        private int[,] BlockNum; //num of block text
        private Random random = new Random();

        private DispatcherTimer timer;
        private int nowTick = 0;
        private const int StopTick = 30;
        private int StopTickNow = 0;
        private const double TimeGo = 0.1; //100ms
        private Queue<ActionEvent> Acevent;
        private Boolean IsAction = false;



        private int max(int x,int y) {
            if (x > y) return x;
            else return y;
        }
        private int abs(int x)
        {
            if (x < 0) return -x;
            else return x;
        }


        public MainPage()
        {
            this.InitializeComponent();
            this.BindDivideRectangle();
            this.BindBlockAndText();
            this.ActionInlizate();
            this.GameInlizate();
            this.SizeChanged += SizeChange;
            this.TabJoinDialog.ShowAsync();
            GameStart();
        }

        //Dialog
        private ContentDialog LostDialog = new ContentDialog()
        {
            Title = "Lost",
            Content = "You Lost The Game",// nội dung
            FullSizeDesired = false,  //tắt hiển thị toàn màn hình
            PrimaryButtonText = "Yes",
        };
        private ContentDialog TabJoinDialog = new ContentDialog()
        {
            Title = "Gợi ý",
            Content = "Bạn hãy sử dụng các phím A,W,S,D để di chuyển !!!",
            FullSizeDesired = false,
            PrimaryButtonText = "Yes"
        };

        private POINT RandomPos()
        {
            int i = -1;
            int j = -1;
            while (i == -1 && j == -1)
            {
                i = random.Next(0, 4);
                j = random.Next(0, 4);
                if (BlockNum[i, j] != 0)
                {
                    i = -1;
                    j = -1;
                }
                else break;
            }
            return new POINT(i, j);
        }
        private int GetRandomValue()
        {
            if (random.Next(1, 10) == 1) return 4;
            else return 2;
        }

        private void GameInlizate()
        {
            BlockNum = new int[4, 4];
        }
        private void GameStart()
        {
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                    BlockNum[i, j] = 0;
            BlockNum[0, 0] = 2;
            BlockNum[3, 3] = 2;
            BlockNum[0, 3] = 2;
            BlockNum[3, 0] = 2;
            KeepNumIsRight();
        }
        private void KeepNumIsRight()
        {
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                    if (BlockNum[i, j] != 0)
                    {
                        TextInBlock[i, j].Text = BlockNum[i, j].ToString();
                        Block[i, j].Visibility = Visibility.Visible;
                    }
                    else
                    {
                        TextInBlock[i, j].Text = "";
                        Block[i, j].Visibility = Visibility.Collapsed;
                    }
        }
        private void BindDivideRectangle()
        {
            DivideRectangle = new Rectangle[10];
            DivideRectangle[0] = rect1;
            DivideRectangle[1] = rect2;
            DivideRectangle[2] = rect3;
            DivideRectangle[3] = rect4;
            DivideRectangle[4] = rect5;
            DivideRectangle[5] = rect6;
            DivideRectangle[6] = rect7;
            DivideRectangle[7] = rect8;
            DivideRectangle[8] = rect9;
            DivideRectangle[9] = rect10;
        }
        private void BindBlockAndText()
        {
            Block = new Grid[4, 4];
            TextInBlock = new TextBlock[4, 4];
            Block[0, 0] = block1_1;
            Block[1, 0] = block1_2;
            Block[2, 0] = block1_3;
            Block[3, 0] = block1_4;
            Block[0, 1] = block2_1;
            Block[1, 1] = block2_2;
            Block[2, 1] = block2_3;
            Block[3, 1] = block2_4;
            Block[0, 2] = block3_1;
            Block[1, 2] = block3_2;
            Block[2, 2] = block3_3;
            Block[3, 2] = block3_4;
            Block[0, 3] = block4_1;
            Block[1, 3] = block4_2;
            Block[2, 3] = block4_3;
            Block[3, 3] = block4_4;
            TextInBlock[0, 0] = text1_1;
            TextInBlock[1, 0] = text1_2;
            TextInBlock[2, 0] = text1_3;
            TextInBlock[3, 0] = text1_4;
            TextInBlock[0, 1] = text2_1;
            TextInBlock[1, 1] = text2_2;
            TextInBlock[2, 1] = text2_3;
            TextInBlock[3, 1] = text2_4;
            TextInBlock[0, 2] = text3_1;
            TextInBlock[1, 2] = text3_2;
            TextInBlock[2, 2] = text3_3;
            TextInBlock[3, 2] = text3_4;
            TextInBlock[0, 3] = text4_1;
            TextInBlock[1, 3] = text4_2;
            TextInBlock[2, 3] = text4_3;
            TextInBlock[3, 3] = text4_4;
        }
        private void KeepBlockAndTextSize(Grid block, TextBlock text, Size size)
        {
            Thickness Margin = block.Margin;
            block.Margin = new Thickness(Margin.Left * size.Width,
                Margin.Top * size.Height, Margin.Right * size.Width,
                Margin.Bottom * size.Height);
            block.Width *= size.Width;
            block.Height *= size.Height;
            text.FontSize *= size.Width;
        }
        private void KeepRectangle(Rectangle rectangle,Size size)
        {
            //Duy trì tỉ lệ, kích thước, vị trí
            Thickness Margin = rectangle.Margin;
            rectangle.Margin = new Thickness(Margin.Left * size.Width,
                Margin.Top * size.Height, Margin.Right * size.Width,
                Margin.Bottom * size.Height);
            rectangle.Width *= size.Width;
            rectangle.Height *= size.Height;
        }
        private bool IsLost()
        {
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                    if (BlockNum[i, j] == 0) return false;
            for (int i = 0; i <= 3; i++) 
                for (int j = 0; j <= 3; j++)
                {
                    if (i > 0 && BlockNum[i - 1, j] == BlockNum[i, j]) return false;
                    if (i < 3 && BlockNum[i + 1, j] == BlockNum[i, j]) return false;
                    if (j > 0 && BlockNum[i, j - 1] == BlockNum[i, j]) return false;
                    if (j < 3 && BlockNum[i, j + 1] == BlockNum[i, j]) return false;
                }
            return true;
        }

        //Function of Action
        private void ActionInlizate()
        {
            timer = new DispatcherTimer();
            BlockMargin = new Thickness[4, 4];
            Acevent = new Queue<ActionEvent>();
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                    BlockMargin[i, j] = new Thickness(Block[i, j].Margin.Left,
                        Block[i, j].Margin.Top, Block[i, j].Margin.Right,
                        Block[i, j].Margin.Bottom);
            timer.Interval = TimeSpan.FromMilliseconds(TimeGo);
            timer.Tick += new EventHandler<object>(timer_Tick);
        }
        private void StartAction() //Action On
        {
            if (Acevent.Count == 0) return; 
            IsAction = true;
            //more need to write
            nowTick = 0;
            timer.Start();
            int maxdis = 0;
            foreach (ActionEvent it in Acevent)
            {
                maxdis = max(maxdis, abs(it.tox - it.fromx));
                maxdis = max(maxdis, abs(it.toy - it.fromy));
            }
            StopTickNow = (int)((double)(maxdis + 1) / 4.0 * StopTick);
        }
        private void AddEvent(int fromx,int fromy,int tox,int toy,int result)
        {
            ActionEvent Event = new ActionEvent(fromx, fromy, tox, toy, result);
            Acevent.Enqueue(Event);
        }
        private void EndAction()
        {
            IsAction = false;
            nowTick = 0;
            timer.Stop();
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                    Block[i, j].Margin = BlockMargin[i, j];
            KeepNumIsRight();
            Acevent.Clear();
            if (IsLost())
            {
                LostDialog.ShowAsync();
                GameStart();
                return;
            }
        }
        private void KeepBlockMargin(out Thickness Margin,Size size)
        {
            Margin.Left *= size.Width;
            Margin.Right *= size.Width;
            Margin.Top *= size.Height;
            Margin.Bottom *= size.Height;
        }
        private void timer_Tick(object sender,object e)
        {
            nowTick++;
            if (nowTick == StopTickNow)
                EndAction();
            else
            {
                foreach (ActionEvent it in Acevent)
                {
                    int fromx = it.fromx;
                    int fromy = it.fromy;
                    int tox = it.tox;
                    int toy = it.toy;
                    double offx = BlockMargin[tox, toy].Left - BlockMargin[fromx, fromy].Left;
                    double offy = BlockMargin[tox, toy].Top - BlockMargin[fromx, fromy].Top;
                    offx *= (double)nowTick / StopTickNow;
                    offy *= (double)nowTick / StopTickNow;
                    Thickness Margin = BlockMargin[fromx, fromy];
                    Block[fromx, fromy].Margin = new Thickness(Margin.Left + offx,
                        Margin.Top + offy, Margin.Right, Margin.Bottom);
                    
                }
            }
            
        }

        //Function of key
        private bool CoreMove(int fromx,int fromy,int tox,int toy,int result)
        {
            BlockNum[fromx, fromy] = 0;
            BlockNum[tox, toy] = result;
            AddEvent(fromx, fromy, tox, toy, result);
            return true;
        }
        private void OnUpKeyDown()
        {
            if (!IsAction)
            {
                bool NeedNew = false;
                bool Get = false;
                for (int i=0;i<=3;i++)
                    for (int j = 1; j <= 3; j++)
                    {
                        if (BlockNum[i, j] != 0)
                        {
                            Get = false;
                            for (int k = j - 1; k >= 0; k--)
                                if (BlockNum[i, k] != 0)
                                {
                                    Get = true;
                                    if (BlockNum[i, k] == BlockNum[i, j])
                                        NeedNew = CoreMove(i, j, i, k, BlockNum[i, j] * 2);
                                    else
                                        NeedNew = CoreMove(i, j, i, k + 1, BlockNum[i, j]);
                                    break;
                                }
                            if (Get == false)
                                NeedNew = CoreMove(i, j, i, 0, BlockNum[i, j]);
                        }
                    }
                StartAction();
                if (NeedNew)
                {
                    POINT P = RandomPos();
                    BlockNum[P.x, P.y] = GetRandomValue();
                }
               
            }
        }
        private void OnDownKeyDown()
        {
            if (!IsAction)
            {
                bool NeedNew = false;
                bool Get = false;
                for (int i=0;i<=3;i++)
                    for (int j = 2; j >= 0; j--)
                    {
                        if (BlockNum[i, j] != 0)
                        {
                            Get = false;
                            for (int k=j+1;k<=3;k++)
                                if (BlockNum[i, k] != 0)
                                {
                                    Get = true;
                                    if (BlockNum[i, j] == BlockNum[i, k])
                                        NeedNew = CoreMove(i, j, i, k, BlockNum[i, j] * 2);
                                    else
                                        NeedNew = CoreMove(i, j, i, k - 1, BlockNum[i, j]);
                                    break;
                                }
                            if (Get == false)
                                NeedNew = CoreMove(i, j, i, 3, BlockNum[i, j]);
                        }
                    }
                StartAction();
                if (NeedNew)
                {
                    POINT P = RandomPos();
                    BlockNum[P.x, P.y] = GetRandomValue();
                }
               
            }

        }
        private void OnLeftKeyDown()
        {
            if (!IsAction)
            {
                bool NeedNew = false;
                bool Get = false;
                for (int i=0;i<=3;i++)
                    for (int j = 1; j <= 3; j++)
                    {
                        if (BlockNum[j,i]!=0)
                        {
                            Get = false;
                            for (int k = j - 1; k >= 0; k--)
                                if (BlockNum[k,i]!=0)
                                {
                                    Get = true;
                                    if (BlockNum[k, i] == BlockNum[j, i])
                                        NeedNew = CoreMove(j, i, k, i, BlockNum[j, i] * 2);
                                    else
                                        NeedNew = CoreMove(j, i, k + 1, i, BlockNum[j, i]);
                                    break;
                                }
                            if (Get == false) NeedNew = CoreMove(j, i, 0, i, BlockNum[j, i]);
                        }
                    }
                StartAction();
                if (NeedNew)
                {
                    POINT P = RandomPos();
                    BlockNum[P.x, P.y] = GetRandomValue();
                }
              
            }
        }
        private void OnRightKeyDown()
        {
            if (!IsAction)
            {
                bool NeedNew = false;
                bool Get = false;
                for (int i=0;i<=3;i++)
                    for (int j = 2; j >= 0; j--)
                    {
                        if (BlockNum[j, i] != 0)
                        {
                            Get = false;
                            for (int k=j+1;k<=3;k++)
                                if (BlockNum[k, i] != 0)
                                {
                                    Get = true;
                                    if (BlockNum[k, i] == BlockNum[j, i])
                                        NeedNew = CoreMove(j, i, k, i, BlockNum[j, i] * 2);
                                    else
                                        NeedNew = CoreMove(j, i, k - 1, i, BlockNum[j, i]);
                                    break;
                                }
                            if (Get == false) NeedNew = CoreMove(j, i, 3, i, BlockNum[j, i]);
                        }
                    }
                StartAction();
                if (NeedNew)
                {
                    POINT P = RandomPos();
                    BlockNum[P.x, P.y] = GetRandomValue();
                }
               
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Loaded += delegate { this.Focus(FocusState.Programmatic); };
        }
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == Left)
                OnLeftKeyDown();
            if (e.Key == Right)
                OnRightKeyDown();
            if (e.Key == Up)
                OnUpKeyDown();
            if (e.Key == Down)
                OnDownKeyDown();
        }
       
        private void SizeChange(object sender, SizeChangedEventArgs e)
        {
            Size OldSize;
            if (e.PreviousSize.Width == 0 && e.PreviousSize.Height == 0)
            {
                OldSize = CommonSize;
            }
            else OldSize = e.PreviousSize;
            Size size;
            size.Width = e.NewSize.Width / OldSize.Width;
            size.Height = e.NewSize.Height / OldSize.Height;
            for (int i = 0; i <= 9; i++)
                KeepRectangle(DivideRectangle[i], size);
            for (int i = 0; i <= 3; i++)
                for (int j = 0; j <= 3; j++)
                {
                    KeepBlockAndTextSize(Block[i, j], TextInBlock[i, j], size);
                    KeepBlockMargin(out BlockMargin[i, j], size);
                }
            
        }
        
    }
}
