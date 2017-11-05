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
using Microsoft.SqlServer;
using System.Data.SqlClient;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using InstaTest;
using System.Windows.Threading;

namespace InstagramDiploma
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           // listBox1.Items.Add("123");

        }

        private void buttonAcc_Click(object sender, RoutedEventArgs e)
        {
           // ListBoxItem mySelectedItem = listBox1.SelectedItem as ListBoxItem;
           // MessageBox.Show(mySelectedItem.Content.ToString());

            string listenBox2 = Convert.ToString(listBox2.Items[0]);
            string[] ProxyPortLoginpPwdP = listenBox2.Split(':');
            string PROXY = ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1];
            MessageBox.Show(listenBox2);
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            uint.TryParse(textBoxThreads.Text, out uint ThreadsCount);
            if (ThreadsCount < 1) ThreadsCount = 1;

            for (int index = 0; index < ThreadsCount; ++index)
            {
                new Thread(InstagramLikerThreatWorker)
                {
                    IsBackground = true
                }.Start();
            }

           
        }
        public void InstagramLikerThreatWorker()
        {
            int? accountId = RequestNextAccountId();
            while (accountId.HasValue)
            {
                StartStandartLiker(accountId.Value);
                accountId = RequestNextAccountId();
            }
        }

        public int nextAccountId = 0;

        public async Task<int> tbCycle()
        {
            await Task.Delay(100);
            return await Task.FromResult(Convert.ToInt32(textBoxCycle.Text)); 
        }
        int itCount;
        public int? RequestNextAccountId()
        {
            
            Action action = async () => itCount = await tbCycle();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, action);
            
                //Convert.ToInt32(textBoxCycle.Text);

            if ( nextAccountId >= itCount)//listBox1.Items.Count)
            {
                return null;
            }
            return  nextAccountId++;
        }
        private void StartStandartLiker(int accountId)
        {
            MessageBox.Show(Convert.ToString(accountId));
            string listenBox1 = Convert.ToString(listBox1.Items[accountId]);
            string[] AccPwdMailPwd = listenBox1.Split(':');
            string listenBox2 = Convert.ToString(listBox2.Items[accountId]);
            string[] ProxyPortLoginpPwdP = listenBox2.Split(':');
            var profileManager = new FirefoxProfileManager();
            var profileManager2 = new FirefoxProfileManager();
            FirefoxProfile profileInstagram = profileManager.GetProfile("Selenium");
            FirefoxProfile profileMail = profileManager2.GetProfile("Mail.ru");
            //profileManager.GetProfile("Selenium");
            string PROXY = ProxyPortLoginpPwdP[0] + ":" + ProxyPortLoginpPwdP[1];
            MessageBox.Show(PROXY);
            Proxy proxy = new Proxy()
            {
                HttpProxy = PROXY,
                // FtpProxy = PROXY,
                // SslProxy = PROXY,
            };
          
            profileInstagram.SetProxyPreferences(proxy);

            FirefoxDriver browserInstagram = new FirefoxDriver(profileInstagram);
            // FirefoxDriver browserMail = new FirefoxDriver(profileMail);
            //FirefoxDriver browserInstagram = new FirefoxDriver();

            browserInstagram.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@instagram.com/accounts/login/"); //Good  http://username:password@website.com
                                                                                                                                                        //Browser2.Navigate().GoToUrl("https://" + ProxyPortLoginpPwdP[2] + ":" + ProxyPortLoginpPwdP[3] + "@mail.ru"); //Good


            Thread.Sleep(3000);

            browserInstagram.FindElement(By.XPath("//input[@name='username']"), 5).SendKeys(AccPwdMailPwd[0]);
            Thread.Sleep(3000);
            browserInstagram.FindElement(By.XPath("//input[@name='password']"), 5).SendKeys(AccPwdMailPwd[1]); //ListBox1Item2(instapwd)
            Thread.Sleep(3000);
            browserInstagram.FindElement(By.XPath("//*[@id=\"react-root\"]//section//main//div//article//div//div[1]//div//form//span//button"), 5).Click(); // InstaLoginButton
            Thread.Sleep(3001);
        }
    }
}
