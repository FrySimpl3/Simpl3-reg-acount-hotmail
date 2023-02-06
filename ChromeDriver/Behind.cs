using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Support.UI;
using Selenium.Extensions;
using Sl.Selenium.Extensions.Chrome;

//NuGet\Install-Package Selenium.WebDriver.UndetectedChromeDriver -Version 2.3.0
//using Selenium.WebDriver.UndetectedChromeDriver;
//using Sl.Selenium.Extensions.Chrome;

//Install - Package Selenium.UndetectedChromeDriver
//using SeleniumUndetectedChromeDriver;

using UndetectedChromeDriverCustom;

namespace ChromeDriver
{
    internal static class Behind
    {
        static void log(string message="") { System.IO.File.AppendAllText("Log.txt.int", message+"\n"); }
        static IWebElement FindElementIfExists(this IWebDriver driver, By by)
        {
            try
            {
                var elements = driver.FindElements(by);
                return (elements.Count >= 1) ? elements.First() : null;
            }
            catch (Exception msg)
            {
                log(msg.ToString());
                driver.Quit();
                driver.Close();
                return null;
            }
            return null;
        }
        public static void _str()
        {
            UndetectedChromeDriver.ENABLE_PATCHER = true;

            ISet<string> driverarguments = new HashSet<string>();

            string pathex = System.Environment.CurrentDirectory + "\\anycaptcha";
            if (System.IO.File.Exists(pathex))
            {
                driverarguments.Add("load-extension=" + pathex);
            }
            var aprams = new ChromeDriverParameters()
            {
                ExcludedArguments = driverarguments
            };
            UndetectedChromeDriver.isHideCommandPromptWindow = true;
            //180 * 6 =1080;
            //320* 6 =1920;

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int luong = UndetectedChromeDriverCustom.UndetectedChromeDriver.soluong ;
            for (int i = 0; i < (luong / 2); i++)
            {
                int x = 320 * i;
                for (int j = 0; j < 2; j++)
                {
                    int y = 480 * j;
                    Thread thread = new Thread((ThreadStart)delegate
                    {
                        //IJavaScriptExecutor js = firefoxDriver as IJavaScriptExecutor;
                        //var dataFromJS = (string)js.ExecuteScript("var content = document.getElementsByClassName('contentpagetop')[0].children[0].innerHTML;return content;")
                        Thread.Sleep(200);                     
                        var driver = UndetectedChromeDriver.Instance(aprams);
                        driver.Manage().Window.Position = new Point(x, y);
                        driver.RandomWait(15, 20);
                        var clickreg = driver.FindElementIfExists(By.XPath("/html/body/div[1]/div/div/div[2]/div/div[1]/div[3]/div/div[1]/div[5]/div/div/form/div/div[1]/fieldset/div[2]/div/div[3]/a"));
                        if (clickreg != null) { clickreg.Click(); }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }
                        IWebElement dropdown = driver.FindElementIfExists(By.Id("LiveDomainBoxList"));
                        if (dropdown != null)
                        {
                            SelectElement select = new SelectElement(dropdown);
                            select.SelectByText("hotmail.com");
                        }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }
                        string username = "Fry_"+UndetectedChromeDriverCustom.UndetectedChromeDriver.randomCdc(13);
                        IWebElement _user = driver.FindElementIfExists(By.Name("MemberName"));
                        if (_user != null)
                        {
                            for (int k = 0; k < username.Length; k++)
                            {
                                _user.SendKeys(username[k].ToString());
                                Thread.Sleep(10);
                            }
                        }
                        driver.RandomWait(1, 2);
                        var action = driver.FindElementIfExists(By.Id("iSignupAction"));
                        if (action != null) { action.Click(); }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }
                        driver.RandomWait(3, 5);

                        string pass = "Fry_" + UndetectedChromeDriverCustom.UndetectedChromeDriver.randomCdc(13);
                        var hidepass = driver.FindElementIfExists(By.Id("ShowHidePasswordCheckbox"));
                        if (hidepass != null) { hidepass.Click(); }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }   
                        var _pass = driver.FindElementIfExists(By.Name("Password"));
                        if (_pass != null)
                        {
                            for (int k = 0; k < pass.Length; k++)
                            {
                                _pass.SendKeys(pass[k].ToString());
                                Thread.Sleep(10);
                            }
                        }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }
                        driver.ExecuteScript("window.scrollTo(0,200)");

                        driver.RandomWait(1, 2);
                        var action2 = driver.FindElementIfExists(By.Id("iSignupAction"));
                        if (action2 != null) { action2.Click(); }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }
                        driver.RandomWait(3, 5);
                        driver.ExecuteScript("window.scrollTo(0,100)");

                        Random random = new Random((int)DateTime.Now.Ticks);
                        string[] arrHo = { "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Huỳnh", "Võ", "Vũ", "Phan", "Trương", "Bùi", "Đặng", "Đỗ", "Ngô", "Hồ", "Dương", "Đinh", "Đoàn", "Lâm", "Mai", "Trịnh", "Đào", "Cao", "Lý", "Hà", "Lưu", "Lương", "Thái", "Châu", "Tạ", "Phùng", "Tô", "Vương", "Văn", "Tăng", "Quách", "Lại", "Hứa", "Thạch", "Diệp", "Từ", "Chu", "La", "Đàm", "Tống", "Giang", "Chung", "Triệu", "Kiều", "Hồng", "Trang", "Đồng", "Danh", "Lư", "Thân", "Kim", "Mã", "Bạch" };
                        string name = arrHo[random.Next(arrHo.Length)];
                        var _name = driver.FindElementIfExists(By.Name("LastName"));
                        string FirstName = arrHo[random.Next(arrHo.Length)] + " ";
                        if (name != null)
                        {
                            for (int k = 0; k < name.Length; k++)
                            {
                                _name.SendKeys(name[k].ToString());
                                Thread.Sleep(10);
                            }
                            driver.ExecuteScript("window.scrollTo(0,20)");
                        }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }

                        FirstName+= arrHo[random.Next(arrHo.Length)];
                        var _FirstName = driver.FindElementIfExists(By.Name("FirstName"));
                        if (_FirstName != null)
                        {
                            for (int k = 0; k < FirstName.Length; k++)
                            {
                                _FirstName.SendKeys(FirstName[k].ToString());
                                Thread.Sleep(10);
                            }
                            driver.ExecuteScript("window.scrollTo(0,220)");
                        }
                        else
                        {
                            driver.Quit();
                            driver.Close();
                        }
                        driver.RandomWait(1, 2);
                        var action3 = driver.FindElementIfExists(By.Id("iSignupAction"));
                        if (action3 != null) { action3.Click(); } else{ driver.Quit(); driver.Close();}
                        driver.RandomWait(3, 5);

                        int month = random.Next(1, 12);
                        var comboBox = driver.FindElementIfExists(By.Id("BirthMonth"));
                        if (comboBox == null)
                        {
                            driver.Quit();
                            driver.Close();
                        }
                        new SelectElement(comboBox).SelectByIndex(month);
                        int day = random.Next(1, 26);
                        var comboBox2 = driver.FindElement(By.Id("BirthDay"));
                        new SelectElement(comboBox2).SelectByIndex(day);
                        var birdyear = driver.FindElementIfExists(By.Id("BirthYear"));
                        string _year = random.Next(1985, 2000).ToString();
                        for (int k = 0; k < _year.Length; k++)
                        {
                            birdyear.SendKeys(_year[k].ToString());
                            Thread.Sleep(10);
                        }

                        driver.ExecuteScript("window.scrollTo(0,200)");
                        driver.RandomWait(1, 2);
                        var action4 = driver.FindElementIfExists(By.Id("iSignupAction"));
                        if (action4 != null) { /*action4.Click();*/ }
                        driver.RandomWait(3, 5);

                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }
    }
}
