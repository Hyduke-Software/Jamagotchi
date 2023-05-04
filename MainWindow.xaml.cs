using System;
using System.Windows;
using System.Windows.Controls;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Jamagotchi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        Creature doggy = new Creature();
        int healthLevel =50; //all up to 100
        int stinkinessLevel = 50;
        //int happinessLevel = 50;
        int ageLevel = 0;
        bool awake = true; //true is awake, false is asleep

        bool start = false;




        public MainWindow()
        {
           
            doggy.skill = 2;
            InitializeComponent();
            Timer timer = new Timer(); //runs every 2.5 seconds, somehow.
            timer.Interval = 2500;
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            //random events happen ever so often
            Timer randomEventtimer = new Timer(); //runs every 10 seconds, somehow.
            randomEventtimer.Interval = 15000;
            randomEventtimer.Elapsed += randEventCalledSub;
            randomEventtimer.Start();

        }
        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            RefreshPrintLabel();   // Do stuff
        
            updateValues();
           
        }
        public void inventory()
        {
            InventoryTextBox.Text = "1 slice of water melon";



        }

        void sleepStart()
        {
            awake = false;
            //to put the pet asleep and awaken at a rand time
            Random randomSleep = new Random();
            int sleepDuration = randomSleep.Next(1001,10001);

            Timer sleepSimer = new Timer(); //runs every 10 seconds, somehow.


            sleepSimer.Interval = sleepDuration;
            sleepSimer.Elapsed += sleepEnd;
            sleepSimer.Start();
            
        }

        void sleepEnd(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
            {
                postHistory($"{doggy.name} woke up.");
            });

            awake = true;
        }

        private void Feed(object sender, RoutedEventArgs e)
        {
            //button click listern, runs function below
            getFed();
        }

        void postHistory(string message)
        {
            //Posts a message to the history box, automatically adds the timestamp and tabs, new line
            HistoryText.AppendText($"\n{getTimestamp()}:\t "+ message);


        }
        string getTimestamp()
        {
            String timeStamp = (DateTime.Now.ToString("h:mm:ss"));
            return timeStamp;
        }
        void getFed()
        {
            
            if (awake == false)
            {
                postHistory($"{doggy.name} is asleep.");
            }
            if (awake == true)
            {
                switch ((doggy.hungerLevel > 75 ? "High" :
              doggy.hungerLevel > 55 ? "Mid" :
              doggy.hungerLevel > 25 ? "Low" :
              doggy.hungerLevel > 10 ? "Little" : "None")) //I think "NONE" is less than 1...
                {
                    case "High":
                        postHistory($"{doggy.name} was very hungry and ate a lot.");
                        doggy.hungerLevel = doggy.hungerLevel - 70;
                        doggy.happinessLevel = doggy.happinessLevel + 50;
                        healthLevel++;
                        break;

                    case "Mid":
                        postHistory($"{doggy.name} was quite hungry and ate a lot");
                        doggy.hungerLevel = doggy.hungerLevel - 50;
                        doggy.happinessLevel = doggy.happinessLevel + 30;
                        healthLevel++;
                        break;
                    case "Low":
                        postHistory($"{doggy.name} was moderately hungry and ate a snack.");
                        doggy.hungerLevel = doggy.hungerLevel - 20;
                        doggy.happinessLevel = doggy.happinessLevel + 20;
                        break;

                    case "Little":
                        postHistory($"{doggy.name} was barely hungry so had a nibble.");
                        doggy.hungerLevel = 0;
                        doggy.happinessLevel = doggy.happinessLevel + 5;
                        break;

                    case "None":
                        postHistory($"{doggy.name} won't eat any more.");
                        doggy.hungerLevel = 0;
                        doggy.happinessLevel = doggy.happinessLevel - 5;
                        break;
                }
            }
            HistoryText.ScrollToEnd();
            RefreshPrintLabel();

        }
        public void WorkThreadFunction()
        {
            TimerExample Jamestimer = new TimerExample();
            Jamestimer.Jtimer();

        }

        private void updateValues()
        {
            stinkinessLevel++;
            doggy.hungerLevel++;
            ageLevel++;

            if (healthLevel < 10)
            {
                doggy.happinessLevel--;

            }

            if (doggy.happinessLevel < 10 || ageLevel > 365 || doggy.hungerLevel >95 || stinkinessLevel > 95)
            {

                healthLevel--;
            }

            if(doggy.happinessLevel > 50 && doggy.hungerLevel < 20 && stinkinessLevel < 20)
            {

                healthLevel++;
            }

            if(doggy.hungerLevel > 50)
            {

                this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                {
                    getFed();
                });
            }
            
            if (stinkinessLevel > 50)
            {

                this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                {
                    getCleaned();
                });
            }

            this.Dispatcher.Invoke(() =>
            {
                inventory();
            });

        }
        private void randEventCalledSub(object sender, ElapsedEventArgs elapsedEventArgs)
        {

            Random rnd = new Random();
           switch( rnd.Next(0, 7))
            {
                case 0:
                    this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                    {
                        postHistory($"{doggy.name} just had a big poo!");

                    });
                    stinkinessLevel = stinkinessLevel + 20;
                    doggy.hungerLevel++;
                    healthLevel++;
                    break;

                case 1:
                    if (awake)
                    {
                        this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                        {
                            postHistory($"{doggy.name} fell asleep.");

                        });
                        sleepStart();
                        doggy.happinessLevel++;
                        healthLevel = healthLevel + 5;

                        break;
                    }
                    break;
                case 2:
                    this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                    {
                        postHistory($"{doggy.name} is playing with its toys.");
                    });
                    doggy.hungerLevel++;
                    doggy.happinessLevel++;
                    healthLevel = healthLevel + 5;
                    break;

                case 3:
                    this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                    {
                        postHistory($"{doggy.name} feels unwell");
                    });
                    doggy.hungerLevel--;
                    doggy.happinessLevel--;
                    healthLevel--;
                    break;



                default:
                break;

            }
            this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
            {
                RefreshPrintLabel();
            });
        }


        private void Clean(object sender, RoutedEventArgs e)
        {

            getCleaned();

        }

        void getCleaned()
        {
            switch ((stinkinessLevel > 75 ? "High" :
        stinkinessLevel > 55 ? "Mid" :
        stinkinessLevel > 25 ? "Low" :
        stinkinessLevel > 1 ? "Little" : "None")) //I think "NONE" is less than 1...
            {
                case "High":
                    postHistory("A big poo was cleaned up.");
                    stinkinessLevel = stinkinessLevel - 70;
                    doggy.happinessLevel = doggy.happinessLevel + 50;
                    healthLevel++;
                    break;

                case "Mid":
                    postHistory("A medium poo was cleaned up.");
                    stinkinessLevel = stinkinessLevel - 50;
                    doggy.happinessLevel = doggy.happinessLevel + 30;
                    break;
                case "Low":
                    postHistory("A little poo was cleaned up.");
                    stinkinessLevel = stinkinessLevel - 20;
                    doggy.happinessLevel = doggy.happinessLevel + 20;
                    healthLevel++;

                    break;

                case "Little":
                    postHistory("The rest of the poo was cleaned up.");
                    stinkinessLevel = 0;
                    healthLevel++;

                    doggy.happinessLevel = doggy.happinessLevel + 15;
                    break;

                case "None":
                    postHistory("There is no poo... for now.");
                    stinkinessLevel = 0;
                    break;
            }

            HistoryText.ScrollToEnd();
            RefreshPrintLabel();

        }

        private void Play(object sender, RoutedEventArgs e)
        {

      


        }
        public string ConvertToBars(int value)
        {
        //converts an int into bar lines for dispalying in the GUI
            string bar = "";
            float a = value / 10;
            int b = (int)Math.Round(a);
            for (int i = 0; i < b; i++)
            {
                bar += "|";
                    
            }

                return bar;
        }

        public void RefreshPrintLabel()
        {


            //Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
           
            this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
            {
                //some re-freshing subs required to perform calculations
                ConditionLabel.Content = ConvertToBars(healthLevel);
                //(stinkinessLevel.ToString());
                HappinessLabel.Content = ConvertToBars(doggy.happinessLevel);
                HungrinessLabel.Content = ConvertToBars(doggy.hungerLevel);
                AgeLabel.Content = (ageLevel/24).ToString(); //age counter is an hour so we turn it into days.
                ConditionLabel.Content = ConvertToBars(healthLevel);
                AwakeLabel.Content = awake;
                
            });
           


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void TextBlock_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
    }
}
