using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.IO;




using OnlineEditLib;

using axfunc;

namespace axpdf
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = @"D:\life\ax\ref\tf - Axb8857f30170525\300389\2088944 contract_2676758.pdf";//借款协议
            //string fileName = @"D:\life\ax\ref\tf - Axb8857f30170525\300389\2116549 contract_2767099.pdf";//转手标协议
            //string fileName = @"D:\life\ax\ref\tf - Axb8857f30170525\300389\2062490 contract_2459143.pdf";//借款协议
            string path;
            if (args.Length == 0)
            {
                path = Environment.CurrentDirectory;
                //Console.WriteLine(path);
                //Console.Read();
                //return;

            }
            else
            {
                path = args[0];
                
            }
            Console.WriteLine(path);

            string csvfile = path + @"\借款人信息.csv";
            AXFunction.parseJiekuanrenCSVfromPathPDF(path, csvfile);

            Console.WriteLine("导出借款人完毕.");

            Console.Read();
        }
  

    
   
    }
}
