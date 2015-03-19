using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PushDownAutomata
{
    public class UserUtility
    {
        /**
         * THis method is used for Fails
         * @param String message
         * **/
        string path = ".//..//..//..//";
        public UserUtility()
        {

        }
        public void readInputFile()
        {
            Console.WriteLine("Please type the name of the input file.");
            string input_file = Console.ReadLine();
            FileStream fs_in = new FileStream(path + input_file, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr_in = new StreamReader(fs_in);

            sr_in.Close();
            fs_in.Close();
        }
        public void FailMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Write(message);
        }
        /**
         * THis method is used for Success
         * @param String message
         * **/
        public void SuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Write(message);
        }
        /**
         * Typical write method
         * @param String message
         * **/
        public void Write(string message)
        {
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
