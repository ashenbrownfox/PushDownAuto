/*
 * Ailun Shen CS5580
 * Simulates a PushDown Automata
 * start state
 * list of states
 * list of possible input characters
 * list of accepting states
 * transitions
 * As for the stack a capital E denotes empty
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushDownAutomata;

namespace DFS
{
    /**
     * Main Class simply contains the main method
     * 
     * **/
    public class Program
    {
        /**
         * The main method is used to test strings in our Finite State Machine
         * Strictly testing purposes
         * It also reads input files for now, which we may create a seperate method in UI for that in the future
         * **/
        public static UserUtility UI = new UserUtility();
        public static void Main(string[] args)
        {
            string path = ".//..//..//..//";
            Console.WriteLine("Welcome! This program will simulate a Pushdown Automata");
            String[] options = { "1) Read DFSM", "2) Read Input Strings", "3) Do Nothing", "4) Exit" };
            Boolean repeat = true;
            string line_buffer;
            //string Start_State = ""; int num_states = 0, num_alphabet = 0, num_accepting_states = 0, num_transitions = 0;
            string[] arraybuffer = new string[1000];
            PDA machine = null; // null until DFSM is loaded
            
            while (repeat)
            {
                Console.WriteLine("Please select an option.");
                for (int i = 0; i < options.Length; i++) { Console.WriteLine(options[i]); }
                line_buffer = Console.ReadLine();
                if (line_buffer.StartsWith("1"))
                {
                    Console.WriteLine("You have chosen option 1. ");
                    machine = loadDFSM();
                    //testing
                    machine.PlayStacks('E', 'a', 'E', 'E');
                    machine.PlayStacks('a', 'E', 'E', 'E');
                    machine.printStack();
                }
                else if (line_buffer.StartsWith("2"))
                {
                    Console.WriteLine("You have chosen option 2.");
                    Console.WriteLine("Please enter the name of the input file(default is input.txt):");
                    simulate("input.txt", machine);
                }
                else if (line_buffer.StartsWith("3"))
                {
                    Console.WriteLine("You have chosen option 3.");  
                    minimize(machine);
                    //Console.WriteLine("Error, unable to minimize. Something went wrong.");
                }
                else
                {
                    repeat = false;
                    Console.WriteLine("Thank you. Now exiting the program.");
                }
            }
            UI.Write("Done. Press any key to continue...");
            Console.ReadLine();
        }
        public static PDA loadDFSM()
        {
            string path = ".//..//..//..//";
            string line_buffer; string Start_State = "";
            int num_states = 0, num_alphabet = 0, num_accepting_states = 0, num_transitions = 0;
            string[] arraybuffer = new string[1000];
            /*************** reads and processes the DFSM formet file ****************/
            #region
            Console.WriteLine("Please type the name of the states file(default is state.txt):");
            //line_buffer = Console.ReadLine();
            FileStream fs = new FileStream(path + "state.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);


            //The Numbers of each Variable
            try
            {
                line_buffer = sr.ReadLine();
                num_states = int.Parse(line_buffer);
                line_buffer = sr.ReadLine();
                num_alphabet = int.Parse(line_buffer);
                line_buffer = sr.ReadLine();
                num_accepting_states = int.Parse(line_buffer);
                line_buffer = sr.ReadLine();
                num_transitions = int.Parse(line_buffer);
            }
            catch (Exception ex)
            {
                UI.FailMessage("State Input file formatted incorrectly.");
            }

            string[] Accepting_States = new string[num_accepting_states];
            //string[,] Finite_State_Array = new string[num_states, num_alphabet];
            char[] Alphabet_Array = new char[num_alphabet];
            Start_State = sr.ReadLine(); //Reading the Starting State

            line_buffer = sr.ReadLine();
            string[] States_Array = line_buffer.Split(' '); //The total number of States

            line_buffer = sr.ReadLine();
            arraybuffer = line_buffer.Split(' ');
            for (int i = 0; i < arraybuffer.Length; i++)
                Alphabet_Array[i] = arraybuffer[i][0]; //Processing the alphabet characters(0 and 1 in this case)

            line_buffer = sr.ReadLine();
            Accepting_States = line_buffer.Split(' '); //The Accepting States
            //string start, letter, next;

            //Start reading the Transitions, which would consist of StartState, Alphabet, Next State
            string[] start_array = new string[num_transitions];
            char[] letter_array = new char[num_transitions];
            string[] next_array = new string[num_transitions];
            char[] pop1 = new char[num_transitions];
            char[] push1 = new char[num_transitions];
            char[] pop2 = new char[num_transitions];
            char[] push2 = new char[num_transitions];
            try
            {
                for (int i = 0; i < num_transitions; i++)
                {
                    line_buffer = sr.ReadLine();
                    arraybuffer = line_buffer.Split(' ');
                    char[] char_buffer = arraybuffer[1].ToCharArray();
                    start_array[i] = arraybuffer[0];
                    letter_array[i] = char_buffer[0];
                    next_array[i] = arraybuffer[2];

                    char_buffer = arraybuffer[3].ToCharArray();
                    pop1[i] = char_buffer[0];
                    char_buffer = arraybuffer[4].ToCharArray();
                    push1[i] = char_buffer[0];
                    char_buffer = arraybuffer[5].ToCharArray();
                    pop2[i] = char_buffer[0];
                    char_buffer = arraybuffer[6].ToCharArray();
                    push2[i] = char_buffer[0];
                }
            }
            catch (Exception ex) 
            { Console.WriteLine("Incorrect Input File."); }
            
            //String transition_state = "new Transition(\"q0\", '0', \"q0\")";
            #endregion
            List<String> Q_States; List<char> Alpha; List<Transition> Trans_Delta;


            Q_States = new List<string> { }; //states
            Alpha = new List<char> { }; //alphabets
            Trans_Delta = new List<Transition> { };
            //After Processing, Stores the Data in 3 Lists
            for (int i = 0; i < num_states; i++)
            {
                Q_States.Add(States_Array[i]);
            }
            for (int i = 0; i < num_alphabet; i++)
            {
                Alpha.Add(Alphabet_Array[i]);
            }
            for (int i = 0; i < num_transitions; i++)
            {
                Trans_Delta.Add(new Transition(start_array[i], letter_array[i], next_array[i],pop1[i],push1[1],pop2[i],push2[i]));
            }

            PDA dFSM = new PDA(Q_States, Alpha, Trans_Delta, Start_State, Accepting_States);
            //Input Char, Stack Head, From State,  To State, Stack Replace
            //Starting State, Input AlphabetChars, transitions, Stack Alphabet
            Console.WriteLine("Ok, state machine processed.");
            return dFSM;
        }
        public static void simulate(String filename, PDA passedFSM)
        {
            string path = ".//..//..//..//"; string line_buffer;
            Console.WriteLine("Please type the name of the input file.");
            //string input_file = Console.ReadLine();

            FileStream fs_in = new FileStream(path + "input.txt", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader streamreader = new StreamReader(fs_in);
            string[] input_buff = new string[1000];
            int j = 0;
            while (!streamreader.EndOfStream)
            {
                line_buffer = streamreader.ReadLine();
                input_buff[j] = line_buffer; j++;
            }
            for (int a = 0; a < j; a++)
            {
                passedFSM.Check(input_buff[a]);
            }
        }
        public static void minimize(PDA themachine)
        {
            Boolean redun = false;
            if (redun)
            {
                themachine.checkRedundantState();
                Console.WriteLine("Ok, done.");
            }
            else
            {
                Console.WriteLine("This is a dummy method.");
            }
        }
    }
}
