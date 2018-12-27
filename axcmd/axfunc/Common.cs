using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO;
using System.Text.RegularExpressions;

using System.Security.Cryptography;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Gma.QrCodeNet.Encoding;

namespace axfunc
{
    public class Common
    {
        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x");
            }

            return byte2String;
        }


        public static string GetNumberFromString(string str)
        {
            //string str = "提取123.11abc提取"; //我们抓取当前字符当中的123.11


            Regex reg; Match match;

            reg = new Regex(@"^([+-]?\d*[.]?\d*).*$");
            match = reg.Match(str);
            string cnum = match.Groups[1].Value.Trim();
            return cnum;
        }


        public static string ReadPdfFile(string fileName)
        {

            StringBuilder text = new StringBuilder();



            if (File.Exists(fileName))
            {

                PdfReader pdfReader = new PdfReader(fileName);



                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {

                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);



                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));

                    text.Append(currentText);

                }

                pdfReader.Close();

            }

            return text.ToString();

        }



        #region 文本文件操作

       
        static string UTF8ToGb2312(string str)
        {
            string gb2312info = string.Empty;

            Encoding utf8 = Encoding.UTF8;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");

            byte[] unicodeBytes = utf8.GetBytes(str);

            byte[] asciiBytes = Encoding.Convert(utf8, gb2312, unicodeBytes);



            char[] asciiChars = new char[gb2312.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            gb2312.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            gb2312info = new string(asciiChars);
            return gb2312info;

        }
        static string Gb2312ToUTF8(string str)
        {
            string utfinfo = string.Empty;

            Encoding utf8 = Encoding.UTF8;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");

            byte[] unicodeBytes = gb2312.GetBytes(str);

            byte[] asciiBytes = Encoding.Convert(gb2312, utf8, unicodeBytes);



            char[] asciiChars = new char[utf8.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            utf8.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
            utfinfo = new string(asciiChars);
            return utfinfo;

        }

        public static void SavaTxtFile(string data, string FilePath)
        {


            //StreamReader sr = new StreamReader(path,Encoding.Default)

            //string upStr = File.ReadAllText(filePath, Encoding.Default);


            //文件覆盖方式添加内容
            System.IO.StreamWriter file = new System.IO.StreamWriter(FilePath, false);
            //保存数据到文件
            //System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");
            //byte[] gb = GB2312.GetBytes(data);
            //string d = Gb2312ToUTF8(data);
            //string d = Gb2312ToUTF8(data);
            //Encoding enc = TxtFileEncoding.GetEncoding(FilePath, System.Text.Encoding.GetEncoding("utf-8"));

            //string d = enc.GetString(System.Text.Encoding.Default.GetBytes(data));
            string d = data;

            file.Write(d);
            //关闭文件
            file.Close();
            //释放对象
            file.Dispose();

            //data = Encoding.Convert(data);
            //StreamWriter sw = File.CreateText(FilePath); 
            //sw.WriteLine(data);
            //sw.Close();

        }


        #endregion

        //二维码生成;
        public static void printQRCode(string sampleText)
        {
            ConsoleColor currentConsoleBackgroundColor = Console.BackgroundColor;

            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = qrEncoder.Encode(sampleText);

            Console.BackgroundColor = ConsoleColor.White;
            //上边扩展一行
            for (int j = 0; j < qrCode.Matrix.Width + 2; j++) Console.Write("  ");
            Console.WriteLine();
            for (int j = 0; j < qrCode.Matrix.Width; j++)
            {
                //左边扩展一列
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write("  ");
                //中间区域
                for (int i = 0; i < qrCode.Matrix.Width; i++)
                {

                    //string charToPrint = qrCode.Matrix[i, j] ? "█" : "  ";
                    ////string charToPrint = qrCode.Matrix[i, j] ? "\033[40m  \033[0m" : "\033[47m  \033[0m";
                    //Console.Write(charToPrint);

                    if (qrCode.Matrix[i, j])
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }

                    Console.Write("  ");
                }
                //右边扩展一列
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine("  ");
            }
            //下边扩展一行
            for (int j = 0; j < qrCode.Matrix.Width + 2; j++) Console.Write("  ");
            Console.WriteLine();



            //重置背景色
            Console.BackgroundColor = currentConsoleBackgroundColor;
        }

        //输入密码
        public static string InputPassword()
        {
            string password = "";
            while (true)
            {
                //存储用户输入的按键，并且在输入的位置不显示字符
                ConsoleKeyInfo ck = Console.ReadKey(true);

                //判断用户是否按下的Enter键
                if (ck.Key != ConsoleKey.Enter)
                {
                    if (ck.Key != ConsoleKey.Backspace)
                    {
                        //将用户输入的字符存入字符串中
                        password += ck.KeyChar.ToString();
                        //将用户输入的字符替换为*
                        Console.Write("*");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(password) && password.Length >= 1)
                        {
                            password = password.Remove(password.Length - 1, 1);
                        }
                        //删除错误的字符
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    Console.WriteLine();

                    break;
                }
            }
            return password;
        }



        public static string procStr(Object obj, string defaultvalue = "")
        {
            if (obj != null) return obj.ToString();
            return defaultvalue;
        }


 



    }
}
