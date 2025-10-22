using System;
using static System.Console;
using System.Globalization;
using System.Linq;

class GreenvilleRevenue
{
    const int ENTRY_FEE = 25;
    const int MIN_CONTESTANTS = 0;
    const int MAX_CONTESTANTS = 30;
    const char SENTINEL = 'Z';

    static void Main()
    {
        // IMPORTANT: Set the culture for currency formatting ONCE at the start of Main.
        // This makes ToString("C") output like "$25.00".
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

        int lastYearContestants = GetContestantNumber("last year");
        int thisYearContestants = GetContestantNumber("this year");

        // --- Initial Program Output ---

        // Exact string output.
        WriteLine($"Last year's competition had {lastYearContestants} contestants, and this year's has {thisYearContestants} contestants");

        int expectedRevenue = thisYearContestants * ENTRY_FEE;

        // Use "C" format specifier with the default culture set above.
        WriteLine($"Revenue expected this year is {expectedRevenue.ToString("C")}");

        DisplayRelationship(thisYearContestants, lastYearContestants);

        Contestant[] contestants = new Contestant[thisYearContestants];
        GetContestantData(contestants);
        DisplayTalentCounts(contestants);
        GetLists(contestants);
    }

    // --- REQUIRED METHOD 1: GetContestantNumber ---
    static int GetContestantNumber(string year)
    {
        int contestants;
        string input;
        bool isValid;

        do
        {
            // PROMPT FIX: Ensure exact match including trailing spaces.
            Write($"Enter number of contestants {year} >> "); // 2 spaces after >>
            input = ReadLine();

            isValid = int.TryParse(input, out contestants) &&
                      contestants >= MIN_CONTESTANTS &&
                      contestants <= MAX_CONTESTANTS;
            if (!isValid)
            {
                // ERROR MESSAGE FIX: Exact match.
                WriteLine("Please enter a number between 0 and 30, inclusive.");
            }
        } while (!isValid);

        return contestants;
    }

    // --- REQUIRED METHOD 2: DisplayRelationship ---
    static void DisplayRelationship(int thisYear, int lastYear)
    {
        if (thisYear > lastYear * 2)
        {
            WriteLine("The competition is more than twice as big this year!");
        }
        else if (thisYear > lastYear)
        {
            // EXACT STRING FIX: This specific message is critical.
            WriteLine("The competition is bigger than ever!");
        }
        else
        {
            WriteLine("A tighter race this year! Come out and cast your vote!");
        }
    }

    // --- REQUIRED METHOD 3: GetContestantData ---
    static void GetContestantData(Contestant[] contestants)
    {
        WriteLine(); // BLANK LINE FIX: Matches example output.

        for (int i = 0; i < contestants.Length; i++)
        {
            Contestant c = new Contestant();
            string rawCodeInput;

            // PROMPT FIX: Exact match.
            Write($"Enter contestant name >> "); // 2 spaces after >>
            c.Name = ReadLine();

            // TALENT CODES LIST FIX: Exact spacing and strings.
            WriteLine("Talent codes are:");
            WriteLine("S    Singing");           // 4 spaces
            WriteLine("D    Dancing");           // 4 spaces
            WriteLine("M    Musical instrument"); // 4 spaces
            WriteLine("O    Other");             // 4 spaces

            do
            {
                // PROMPT FIX: Exact match.
                Write($"Enter talent code >> "); // 2 spaces after >>
                rawCodeInput = ReadLine();
                string trimmedCode = rawCodeInput.Trim();

                // Set property (internal validation logic remains in Contestant class)
                c.TalentCode = trimmedCode.ToUpper();

                if (c.TalentCode == "I")
                {
                    string echoedCode;
                    if (string.IsNullOrWhiteSpace(rawCodeInput))
                    {
                        // CRITICAL FIX: Empty or whitespace input must echo "L".
                        echoedCode = "L";
                    }
                    else
                    {
                        // CRITICAL FIX: Otherwise, echo the EXACT raw input from the user.
                        // This preserves case and length for invalid entries like "x" or "Dance".
                        echoedCode = rawCodeInput;
                    }

                    // ERROR MESSAGE FIX: Exact format.
                    WriteLine($"{echoedCode} is not a valid code");
                }

            } while (c.TalentCode == "I");

            contestants[i] = c;
        }
    }

    // Helper to display counts
    static void DisplayTalentCounts(Contestant[] contestants)
    {
        int[] counts = new int[Contestant.talentCodes.Length];

        foreach (var c in contestants)
        {
            if (c.TalentCode != "I") // Only count valid talent codes
            {
                int index = Array.IndexOf(Contestant.talentCodes, c.TalentCode);
                if (index >= 0) // Should always be true for non-"I" codes
                {
                    counts[index]++;
                }
            }
        }

        WriteLine(); // BLANK LINE FIX: Matches example.
        WriteLine("The types of talent are:");

        for (int i = 0; i < Contestant.talentStrings.Length; i++)
        {
            // FORMATTING FIX: "{0, -18}" means left-aligned in a field of 18 characters.
            // "{1, 1}" means right-aligned in a field of 1 character (for single digit counts).
            // This ensures the numbers align correctly.
            WriteLine("{0, -18}{1, 1}", Contestant.talentStrings[i], counts[i]);
        }
    }

    // --- REQUIRED METHOD 4: GetLists ---
    static void GetLists(Contestant[] contestants)
    {
        string codeInput;
        string code;

        do
        {
            WriteLine(); // BLANK LINE FIX: Matches example.
            // PROMPT FIX: Exact match, including trailing space.
            Write("Enter a talent code (S/D/M/O) to see the list or Z to exit: ");
            codeInput = ReadLine();

            string trimmedCode = codeInput.Trim();
            code = trimmedCode.ToUpper();

            if (code.Length == 1 && code[0] == SENTINEL)
            {
                break; // Exit the loop
            }

            bool codeValid = Contestant.talentCodes.Contains(code);

            if (codeValid)
            {
                DisplayContestantsByCode(contestants, code);
            }
            else
            {
                string echoedCode;
                if (string.IsNullOrWhiteSpace(codeInput)) // Use codeInput directly
                {
                    // CRITICAL FIX: Empty or whitespace input must echo "L".
                    echoedCode = "L";
                }
                else
                {
                    // CRITICAL FIX: Otherwise, echo the EXACT raw input from the user.
                    echoedCode = codeInput;
                }

                // ERROR MESSAGE FIX: Exact format.
                WriteLine($"{echoedCode} is not a valid code");
            }
        } while (true);
    }

    // Helper to display names for a given code
    static void DisplayContestantsByCode(Contestant[] contestants, string codeToMatch)
    {
        int index = Array.IndexOf(Contestant.talentCodes, codeToMatch);
        string talentName = Contestant.talentStrings[index];

        // HEADER FIX: Exact format.
        WriteLine($"Contestants with talent {talentName} are:");

        foreach (var c in contestants)
        {
            if (c.TalentCode == codeToMatch)
            {
                WriteLine(c.Name);
            }
        }
    }
}

// NEW REQUIRED CLASS: Contestant (No changes needed here as it handles data, not direct output)
class Contestant
{
    public static string[] talentCodes = { "S", "D", "M", "O" };
    public static string[] talentStrings = { "Singing", "Dancing", "Musical instrument", "Other" };

    public string Name { get; set; }

    private string talentCodeField;
    private string talentField;

    public string TalentCode
    {
        get { return talentCodeField; }
        set
        {
            int index = Array.IndexOf(talentCodes, value);
            if (index >= 0)
            {
                talentCodeField = value;
                talentField = talentStrings[index];
            }
            else
            {
                talentCodeField = "I";
                talentField = "Invalid";
            }
        }
    }

    public string Talent
    {
        get { return talentField; }
    }
}