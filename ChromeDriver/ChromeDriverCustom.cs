#region Assembly Selenium.WebDriver.UndetectedChromeDriver, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null
// D:\source code\Csharp\TFT_BOT_LEarning\ChromeDriver\packages\Selenium.WebDriver.UndetectedChromeDriver.2.3.0\lib\netstandard2.0\Selenium.WebDriver.UndetectedChromeDriver.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;
using Selenium.Extensions;
using Sl.Selenium.Extensions;
using Sl.Selenium.Extensions.Chrome;

namespace UndetectedChromeDriverCustom
{
    public class UndetectedChromeDriver : Sl.Selenium.Extensions.ChromeDriver
    {
        private static readonly string[] ProcessNames = new string[3] { "chrome", "chromedriver", "Fry_chromedriver" };

        public static bool ENABLE_PATCHER = true;

        public ChromeOptions ChromeOptions { get; private set; }

        protected UndetectedChromeDriver(ChromeDriverParameters args)
            : base(args)
        {
        }

        public new static void KillAllChromeProcesses()
        {
            string[] processNames = ProcessNames;
            for (int i = 0; i < processNames.Length; i++)
            {
                Process[] processesByName = Process.GetProcessesByName(processNames[i]);
                foreach (Process process in processesByName)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
            }

            SlDriver.ClearDrivers(SlDriverBrowserType.Chrome);
        }

        public new static SlDriver Instance(bool Headless = false)
        {
            return Instance("sl_selenium_chrome", Headless);
        }

        public new static SlDriver Instance(string ProfileName, bool Headless = false)
        {
            return Instance(new HashSet<string>(), ProfileName, Headless);
        }

        public new static SlDriver Instance(ISet<string> DriverArguments, string ProfileName, bool Headless = false)
        {
            return Instance(DriverArguments, new HashSet<string>(), ProfileName, Headless);
        }

        public new static SlDriver Instance(ISet<string> DriverArguments, ISet<string> ExcludedArguments, string ProfileName, bool Headless = false)
        {
            return Instance(new ChromeDriverParameters
            {
                DriverArguments = DriverArguments,
                ExcludedArguments = ExcludedArguments,
                Headless = Headless,
                ProfileName = ProfileName
            });
        }

        public new static SlDriver Instance(ChromeDriverParameters args)
        {
            if (args.DriverArguments == null)
            {
                args.DriverArguments = new HashSet<string>();
            }

            if (args.ExcludedArguments == null)
            {
                args.ExcludedArguments = new HashSet<string>();
            }

            if (args.ProfileName == null)
            {
                args.ProfileName = "sl_selenium_chrome";
            }

            if (!SlDriver._openDrivers.IsOpen(SlDriverBrowserType.Chrome, args.ProfileName))
            {
                UndetectedChromeDriver driver = new UndetectedChromeDriver(args);
                SlDriver._openDrivers.OpenDriver(driver);
            }

            return SlDriver._openDrivers.GetDriver(SlDriverBrowserType.Chrome, args.ProfileName);
        }

        public override void GoTo(string URL)
        {
            if (ExecuteScript("return navigator.webdriver") != null)
            {
                base.BaseDriver.ExecuteCdpCommand("Page.addScriptToEvaluateOnNewDocument", new Dictionary<string, object> { { "source", "\n                                Object.defineProperty(window, 'navigator', {\n                                       value: new Proxy(navigator, {\n                                       has: (target, key) => (key === 'webdriver' ? false : key in target),\n                                       get: (target, key) =>\n                                           key === 'webdriver'\n                                           ? undefined\n                                           : typeof target[key] === 'function'\n                                           ? target[key].bind(target)\n                                           : target[key]\n                                       })\n                                   });\n\n                        " } });
            }

            string text = (string)ExecuteScript("return navigator.userAgent");
            base.BaseDriver.ExecuteCdpCommand("Network.setUserAgentOverride", new Dictionary<string, object> {
            {
                "userAgent",
                text.Replace("Headless", "")
            } });
            object obj = ExecuteScript("\n               let objectToInspect = window,\n                        result = [];\n                    while(objectToInspect !== null)\n                    { result = result.concat(Object.getOwnPropertyNames(objectToInspect));\n                      objectToInspect = Object.getPrototypeOf(objectToInspect); }\n                    return result.filter(i => i.match(/.+_.+_(Array|Promise|Symbol)/ig))\n            ");
            if (obj != null && ((ReadOnlyCollection<object>)obj).Count > 0)
            {
                base.BaseDriver.ExecuteCdpCommand("Page.addScriptToEvaluateOnNewDocument", new Dictionary<string, object> { { "source", " \n                        let objectToInspect = window,\n                        result = [];\n                            while(objectToInspect !== null) \n                            { result = result.concat(Object.getOwnPropertyNames(objectToInspect));\n                              objectToInspect = Object.getPrototypeOf(objectToInspect); }\n                            result.forEach(p => p.match(/.+_.+_(Array|Promise|Symbol)/ig)\n                                                &&delete window[p]&&console.log('removed',p))\n                    " } });
            }

            base.GoTo(URL);
        }

        public override string DriverName()
        {
            return "Fry_" + base.DriverName();
        }

        public static SlDriver Instance(string ProfileName, ChromeOptions ChromeOptions)
        {
            UndetectedChromeDriver obj = (UndetectedChromeDriver)Instance(ProfileName);
            obj.ChromeOptions = ChromeOptions;
            return obj;
        }

        public static SlDriver Instance(string ProfileName, ChromeOptions ChromeOptions, TimeSpan Timeout)
        {
            UndetectedChromeDriver obj = (UndetectedChromeDriver)Instance(new ChromeDriverParameters
            {
                ProfileName = ProfileName,
                Timeout = Timeout
            });
            obj.ChromeOptions = ChromeOptions;
            return obj;
        }
        public static bool isHideCommandPromptWindow= false;
        public static int soluong = 0;
        protected override OpenQA.Selenium.Chrome.ChromeDriver CreateBaseDriver()
        {
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(DriversFolderPath(), DriverName());
            chromeDriverService.HostName = "127.0.0.1";
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            if (isHideCommandPromptWindow)
            {
                chromeDriverService.HideCommandPromptWindow = true;
            }
            DriverArguments.Add("start-maximized");
            DriverArguments.Add("--disable-blink-features");
            DriverArguments.Add("--disable-blink-features=AutomationControlled");
            DriverArguments.Add("disable-infobars");
            DriverArguments.Add("--app=https://signup.live.com/signup?");
            string pathex = System.Environment.CurrentDirectory + "\\anycaptcha";
            //180 * 6 =1080;
            //320* 6 =1920
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            string _check = "--window-size=320,480";

            DriverArguments.Add("--enable-precise-memory-info");
            DriverArguments.Add("--disable-popup-blocking");
            DriverArguments.Add("--disable-default-apps");

            //MessageBox.Show(_check);
            DriverArguments.Add(_check);
            DriverArguments.Add("load-extension=" + pathex);
            if (Headless)
            {
                DriverArguments.Add("headless");
            }
            else
            {
                DriverArguments.Remove("headless");
            }

            DriverArguments.Add("--no-default-browser-check");
            DriverArguments.Add("--no-first-run");
            if (!new HashSet<string>(DriverArguments.Select((string f) => f.Split('=')[0])).Contains("--log-level"))
            {
                DriverArguments.Add("--log-level=0");
            }



            if (ChromeOptions == null)
            {
                ChromeOptions = new ChromeOptions();
            }
            foreach (string driverArgument in DriverArguments)
            {
                ChromeOptions.AddArgument(driverArgument);
            }
            ChromeOptions.AcceptInsecureCertificates= true;
            ChromeOptions.AddExcludedArgument("enable-automation");
            ChromeOptions.AddAdditionalChromeOption("useAutomationExtension", false);
            //hide table save password
            ChromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            ChromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);

            foreach (string excludedArgument in base.ChromeDriverParameters.ExcludedArguments)
            {
                ChromeOptions.AddExcludedArgument(excludedArgument);
            }
            AddProfileArgumentToBaseDriver(ChromeOptions);
            if (base.ChromeDriverParameters.Timeout != default(TimeSpan))
            {
                return new OpenQA.Selenium.Chrome.ChromeDriver(chromeDriverService, ChromeOptions, base.ChromeDriverParameters.Timeout);
            }

            return new OpenQA.Selenium.Chrome.ChromeDriver(chromeDriverService, ChromeOptions);
        }

        protected override void DownloadLatestDriver()
        {
            base.DownloadLatestDriver();
            if (ENABLE_PATCHER)
            {
                PatchDriver();
            }
        }

        private void PatchDriver()
        {
            string s = randomCdc(26);
            FileStream fileStream = new FileStream(DriverPath(), FileMode.Open, FileAccess.ReadWrite);
            byte[] array = new byte[1];
            StringBuilder stringBuilder = new StringBuilder("....");
            while (fileStream.Read(array, 0, array.Length) != 0)
            {
                stringBuilder.Remove(0, 1);
                stringBuilder.Append((char)array[0]);
                if (stringBuilder.ToString() == "Fry_")
                {
                    fileStream.Seek(-4L, SeekOrigin.Current);
                    byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(s);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }

            String lic = System.Environment.CurrentDirectory + "\\ChromeDrivers\\LICENSE.chromedriver";
            if (System.IO.File.Exists(lic))
            {
                System.IO.File.Delete(lic);
            }
        }

        public static string randomCdc(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            char[] array = new char[size];
            for (int i = 0; i < size; i++)
            {
                array[i] = "abcdefghijklmnopqrstuvwxyz"[random.Next("abcdefghijklmnopqrstuvwxyz".Length)];
            }

            array[2] = array[0];
            array[3] = '_';
            return new string(array);
        }
    }
}
#if false // Decompilation log
'27' items in cache
------------------
Resolve: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Facades\netstandard.dll'
------------------
Resolve: 'Sl.Selenium.Extensions.Chrome, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Sl.Selenium.Extensions.Chrome, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\source code\Csharp\TFT_BOT_LEarning\ChromeDriver\packages\Sl.Selenium.Extensions.Chrome.2.3.0\lib\netstandard2.0\Sl.Selenium.Extensions.Chrome.dll'
------------------
Resolve: 'WebDriver, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'WebDriver, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\source code\Csharp\TFT_BOT_LEarning\ChromeDriver\packages\Selenium.WebDriver.4.3.0\lib\net47\WebDriver.dll'
------------------
Resolve: 'Selenium.Extensions, Version=2.1.1.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'Selenium.Extensions, Version=2.1.1.0, Culture=neutral, PublicKeyToken=null'
Load from: 'D:\source code\Csharp\TFT_BOT_LEarning\ChromeDriver\packages\Sl.Selenium.Extensions.2.1.1\lib\netstandard2.0\Selenium.Extensions.dll'
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll'
------------------
Resolve: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll'
------------------
Resolve: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll'
------------------
Resolve: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.dll'
------------------
Resolve: 'System.Diagnostics.Tracing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Diagnostics.Tracing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Drawing.dll'
------------------
Resolve: 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.IO.Compression, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.IO.Compression.FileSystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.ComponentModel.Composition, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.ComponentModel.Composition, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.Net.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Http, Version=4.2.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '4.0.0.0', Got: '4.2.0.0'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Net.Http.dll'
------------------
Resolve: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Numerics.dll'
------------------
Resolve: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.Runtime.Serialization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Could not find by name: 'System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
------------------
Resolve: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Could not find by name: 'System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
------------------
Resolve: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.dll'
------------------
Resolve: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xml.Linq, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.Linq.dll'
#endif
