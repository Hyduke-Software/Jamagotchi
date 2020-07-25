using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Jamagotchi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {



        int healthLevel =50; //all up to 100
        int stinkinessLevel = 50;
        int happinessLevel = 50;
        int hungrinessLevel = 99;
        int boredomLevel = 50;
        int ageLevel = 0;
        bool awake = true; //true is awake, false is asleep
        string petName = "KILLER";
        bool start = false;




        public MainWindow()
        {
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
                postHistory($"{petName} woke up.");
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
                postHistory($"{petName} is asleep.");
            }
            if (awake == true)
            {
                switch ((hungrinessLevel > 75 ? "High" :
              hungrinessLevel > 55 ? "Mid" :
              hungrinessLevel > 25 ? "Low" :
              hungrinessLevel > 10 ? "Little" : "None")) //I think "NONE" is less than 1...
                {
                    case "High":
                        postHistory($"{petName} was very hungry and ate a lot.");
                        hungrinessLevel = hungrinessLevel - 70;
                        happinessLevel = happinessLevel + 50;
                        healthLevel++;
                        break;

                    case "Mid":
                        postHistory($"{petName} was quite hungry and ate a lot");
                        hungrinessLevel = hungrinessLevel - 50;
                        happinessLevel = happinessLevel + 30;
                        healthLevel++;
                        break;
                    case "Low":
                        postHistory($"{petName} was moderately hungry and ate a snack.");
                        hungrinessLevel = hungrinessLevel - 20;
                        happinessLevel = happinessLevel + 20;
                        break;

                    case "Little":
                        postHistory($"{petName} was barely hungry so had a nibble.");
                        hungrinessLevel = 0;
                        happinessLevel = happinessLevel + 5;
                        break;

                    case "None":
                        postHistory($"{petName} won't eat any more.");
                        hungrinessLevel = 0;
                        happinessLevel = happinessLevel - 5;
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
            hungrinessLevel++;
            ageLevel++;

            if (healthLevel < 10)
            {
                happinessLevel--;

            }

            if (happinessLevel < 10 || ageLevel > 365 || hungrinessLevel >95 || stinkinessLevel > 95)
            {

                healthLevel--;
            }

            if(happinessLevel > 50 && hungrinessLevel < 20 && stinkinessLevel < 20)
            {

                healthLevel++;
            }

            if(hungrinessLevel > 50)
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



        }
        private void randEventCalledSub(object sender, ElapsedEventArgs elapsedEventArgs)
        {

            Random rnd = new Random();
           switch( rnd.Next(0, 7))
            {
                case 0:
                    this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                    {
                        postHistory($"{petName} just had a big poo!");

                    });
                    stinkinessLevel = stinkinessLevel + 20;
                    hungrinessLevel++;
                    healthLevel++;
                    break;

                case 1:
                    if (awake)
                    {
                        this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                        {
                            postHistory($"{petName} fell asleep.");

                        });
                        sleepStart();
                        happinessLevel++;
                        healthLevel = healthLevel + 5;

                        break;
                    }
                    break;
                case 2:
                    this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                    {
                        postHistory($"{petName} is playing with its toys.");
                    });
                    hungrinessLevel++;
                    happinessLevel++;
                    healthLevel = healthLevel + 5;
                    break;

                case 3:
                    this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
                    {
                        postHistory($"{petName} feels unwell");
                    });
                    hungrinessLevel--;
                    happinessLevel--;
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
                    happinessLevel = happinessLevel + 50;
                    healthLevel++;
                    break;

                case "Mid":
                    postHistory("A medium poo was cleaned up.");
                    stinkinessLevel = stinkinessLevel - 50;
                    happinessLevel = happinessLevel + 30;
                    break;
                case "Low":
                    postHistory("A little poo was cleaned up.");
                    stinkinessLevel = stinkinessLevel - 20;
                    happinessLevel = happinessLevel + 20;
                    healthLevel++;

                    break;

                case "Little":
                    postHistory("The rest of the poo was cleaned up.");
                    stinkinessLevel = 0;
                    healthLevel++;

                    happinessLevel = happinessLevel + 15;
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

        public void RefreshPrintLabel()
        {


            //Thread thread = new Thread(new ThreadStart(WorkThreadFunction));
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
           
            this.Dispatcher.Invoke(() => //This allows the UI to be updated by another thread
            {
                //some re-freshing subs required to perform calculations
                StinkinessLabel.Content = (stinkinessLevel.ToString());
                HappinessLabel.Content = (happinessLevel.ToString());
                HungrinessLabel.Content = (hungrinessLevel.ToString());
                AgeLabel.Content = (ageLevel/24).ToString(); //age counter is an hour so we turn it into days.
                HealthinessLabel.Content = (healthLevel.ToString());
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
