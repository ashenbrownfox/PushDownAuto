using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushDownAutomata
{
    public class PDA
    {
        /** **/
        private readonly List<string> States = new List<string>();
        private readonly List<char> Alphabet = new List<char>();
        private readonly List<Transition> Transitions = new List<Transition>();
        private string accepted_states;
        private readonly List<string> Final_States = new List<string>();
        public UserUtility UI;
        private readonly List<string> accepting_states = new List<string>();
        private readonly List<string> rejecting_states = new List<string>();
        Stack<char> StackA = new Stack<char>();
        Stack<char> StackB = new Stack<char>();
        /**
         * 
         * The constructor initializes the 4 main variables(colllections) as well as the User Utility;
         * Basically just copied and paste the states and alphabet
         * Setting up FSM methods
         * @param IEnumerable<string> q, IEnumerable<char> sigma, IEnumerable<Transition> delta, string q0, IEnumerable<string> f
         * **/
        public PDA(IEnumerable<string> q, IEnumerable<char> sigma, IEnumerable<Transition> delta, string q0, IEnumerable<string> f)
        {
            States = q.ToList();
            Alphabet = sigma.ToList();
            AddTransitions(delta);
            AddInitialState(q0);
            AddFinalStates(f);
            UI = new UserUtility();
            UI.Write("Ok, created a new PDA.");
        }

        /**
         * Adding Transitions
         * @param IEnumerable<Transition>
         * **/
        private void AddTransitions(IEnumerable<Transition> transitions)
        {
            foreach (Transition transition in transitions.Where(ValidTransition))
            {
                Transitions.Add(transition);
            }
        }

        /**
         * This is the validation method
         * As for the validation of the states 
         * Check if the states are defined in the list of states
         * @param Transition
         * 
         * **/
        private bool ValidTransition(Transition transition)
        {
            return States.Contains(transition.StartState) &&
                States.Contains(transition.EndState) &&
                Alphabet.Contains(transition.Symbol) &&
                !TransitionAlreadyDefined(transition);
        }

        /** If the Transition is already defined, map it out**/
        private bool TransitionAlreadyDefined(Transition transition)
        {
            return Transitions.Any(t => t.StartState == transition.StartState &&
                                t.Symbol == transition.Symbol);
        }
        /**
         * Defining the initial states
         * **/
        private void AddInitialState(string q0)
        {
            if (q0 != null && States.Contains(q0))
            {
                accepted_states = q0;
            }
        }
        /**validating the final state
         * **/
        private void AddFinalStates(IEnumerable<string> finalStates)
        {
            foreach (var finalState in finalStates.Where(finalState => States.Contains(finalState)))
            {
                Final_States.Add(finalState);
            }
        }

        public void PlayStacks(char pop1, char push1, char pop2, char push2)
        {
            char alphabet_buffer;
            if (StackA.Count == 0)
            {
                //Console.WriteLine("Empty Stack A, nothing to pop here.");
            }
            else if (pop1 == StackA.Peek())
            {
                try
                {
                    alphabet_buffer = StackA.Pop();
                    //Console.WriteLine("Ok, popped {0} on Stack A",pop1);
                }
                catch(Exception ex)
                {
                    UI.FailMessage("REJECTED!");
                    return;
                }
                
            }
            else
            {
                //return;
                //Console.WriteLine("Nothing to Pop here on Stack A.");
            }
            if(push1 != 'E'){
                StackA.Push(push1);
                //Console.WriteLine("OK, pushed {0} to Stack A",push1);
            }      
            else
            {
                //Console.WriteLine("Empty string,nothing to push here on Stack A.");
            }
            if (StackB.Count == 0)
            {
                //Console.WriteLine("Empty Stack B, nothing to pop here.");
            }
            else if (pop2 == StackB.Peek())
            {
                try
                {
                    StackB.Pop(); 
                    //Console.WriteLine("Ok, popped {0} on Stack B", pop2);
                }
                catch
                {
                    UI.FailMessage("REJECTED!");
                    return;
                }
                
            }
            else
            {
                //Console.WriteLine("Nothing to pop here on Stack B.");
            }
            if (push2 != 'E'){
                StackB.Push(push2);
                //Console.WriteLine("OK, pushed {0} on Stack B",push2);
            }
            else
            {
                //Console.WriteLine("Empty String, Nothing to push here.");
            }    
        }

        /** This method prints out the contents of the Stack **/
        public void printStack()
        {
            Console.WriteLine("Stack A: ");
            foreach (char x in StackA)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("Stack B: ");
            foreach (char x in StackB)
            {
                Console.WriteLine(x);
            }
        }
        /** Performs a check whether or not the stack is empty. **/
        public Boolean checkStack()
        {
            Boolean empty;
            if (StackA.Count == 0 && StackB.Count == 0)
                empty = true;
            else
                empty = false;
            return empty;
        }
        /** Obsolete **/
        public void checkRedundantState()
        {
            //(trans.Where(ValidTransition)
            foreach (string accepted_state in accepting_states)
            {
                Console.WriteLine(accepted_state);
            }
            foreach (string non_state in rejecting_states)
            {
                Console.WriteLine(non_state);
            }
        }
        /** Methods that validate the string if it's in the alphabet  **/
        /**
         * The check method is
         * used called when checking if the alphabet is in
         * @param string input
         * **/
        public void Check(string input)
        {
            UI.SuccessMessage("Ok. Checking: " + input);
            if (InvalidInputOrFSM(input))
            {
                return;
            }
            string currentState = accepted_states;
            StringBuilder steps = new StringBuilder();  
            foreach (char symbol in input.ToCharArray())
            {
                Transition transition = Transitions.Find(t => t.StartState == currentState &&
                                                    t.Symbol == symbol);
                char a,b,c,d;
                a = transition.pop1; b = transition.push1; c = transition.Pop2; d = transition.Push2;
                PlayStacks(a, b, c, d); //pushes and pops the appropriate stack
                if (transition == null)
                {
                    UI.FailMessage("No transitions for current state and symbol");
                    //UI.FailMessage(steps.ToString());
                    return;
                }
                currentState = transition.EndState;
                steps.Append(transition + "\n");
            }
            if (!checkStack())
            {
                UI.FailMessage("One or multiple stacks are not empty. Rejected!");
                return;
            }
            if (Final_States.Contains(currentState))
            {
                UI.SuccessMessage("Accepted the input with steps:\n" + steps);
                UI.SuccessMessage("Accepted!");
                return;
            }
            UI.FailMessage("Stopped in state " + currentState +
                                " which is not a final state.");
            //UI.FailMessage(steps.ToString());
            UI.FailMessage("Rejected!");
        }

        /**
         * The rejection method to be used 
         * if the state has not been found
         * therefore the alphabet is not contained
         * **/
        private bool InvalidInputOrFSM(string input)
        {
            if (InputContainsNotDefinedSymbols(input))
            {
                return true;
            }
            if (InitialStateNotSet())
            {
                UI.FailMessage("No initial state has been set");
                return true;
            }
            if (NoFinalStates())
            {
                UI.FailMessage("No final states have been set");
                return true;
            }
            return false;
        }
        /**
         * Not defined method
         * @param inputs
         * **/
        private bool InputContainsNotDefinedSymbols(string input)
        {
            foreach (var symbol in input.ToCharArray().Where(symbol => !Alphabet.Contains(symbol)))
            {
                UI.FailMessage("Could not accept the input since the symbol, because " + symbol + " is not part of the alphabet");
                return true;
            }
            return false;
        }
        /**
         * When there is no initial state
         * return empty string
         * **/
        private bool InitialStateNotSet()
        {
            return string.IsNullOrEmpty(accepted_states);
        }
        /**
         * When there is no final state
         * **/
        private bool NoFinalStates()
        {
            return Final_States.Count == 0;
        }
    }
}
