// See https://aka.ms/new-console-template for more information
using System.Collections.ObjectModel;
try
{
    Console.WriteLine("Enter a dollar and cents amount with dot splitter.");
    Console.WriteLine("It must greater than 0 and lower than 2 billion with 2 decimal places:");
    //send input line to a constructor
    MoneyWriter money = new MoneyWriter(Console.ReadLine());
    money.CheckInput();
    money.Print();
}
catch (Exception ex) when (ex is NullReferenceException ||
                           ex is FormatException ||
                           ex is OverflowException)
{
    //print error message when exception occurs
    Console.WriteLine(ex.Message);
}

class MoneyWriter
{
    private const char dot = '.';
    private const int numCount = 2;
    private const int centsLength = 2;
    private const int max = 2_000_000_000;
    private const int thousand = 1000;
    private const int threeDigits = 3;
    private const int ten = 10;
    private const int hundred = 100;
    private const int twenty = 20;

    private string? inputLine;
    private uint dollars;
    private uint cents;
    private IReadOnlyDictionary<uint, string> Texts { get; } = new ReadOnlyDictionary<uint, string>(
        new Dictionary<uint, string>() {
            {1, "one"},
            {2, "two"},
            {3, "three"},
            {4, "four"},
            {5, "five"},
            {6, "six"},
            {7, "seven"},
            {8, "eight"},
            {9, "nine"},
            {10, "ten"},
            {11, "eleven"},
            {12, "twelve"},
            {13, "thirteen"},
            {14, "fourteen"},
            {15, "fifteen"},
            {16, "sixteen"},
            {17, "seventeen"},
            {18, "eighteen"},
            {19, "nineteen"},

            {20, "twenty"},
            {30, "thirty"},
            {40, "fourty"},
            {50, "fifty"},
            {60, "sixty"},
            {70, "seventy"},
            {80, "eighty"},
            {90, "ninety"},

            {100, "hundred"},
            {1_000, "thousand"},
            {1_000_000, "million"},
            {1_000_000_000, "billion" },
        });

    public MoneyWriter(string? inputLine)
    {
        this.inputLine = inputLine;
    }

    public void CheckInput()
    {
        if (inputLine is null || inputLine.Length == 0)
            throw new NullReferenceException("Input line must be filled");

        string[] textNumbers = inputLine.Split(dot);
        if (textNumbers.Length != numCount)
            throw new FormatException("Input line must contains only 2 numbers split one dot");
        //check dollars and cents length
        if (textNumbers[0].Length == 0 || textNumbers[1].Length != centsLength)
            throw new FormatException("Dollars and cents must be filled in correct format");
        
        //parse input line to dollars and cents numbers
        dollars = uint.Parse(textNumbers[0]);
        cents = uint.Parse(textNumbers[1]);
        if (dollars > max || cents > max)
            throw new OverflowException("Numbers out of range");
    }

    public void Print() => 
        Console.WriteLine(
            $"{Convert(dollars)} DOLLARS AND {Convert(cents)} CENTS");
    private string Convert(uint number)
    {
        string text = string.Empty;
        for (uint i = number, power = 0; i > 0; i /= thousand, power += threeDigits)
        {
            //every iteration split number by thousand scale
            uint toThousand = i % thousand;
            //power - thousand, million or billion, every iteraion shift by 3 digits
            //add text to begin
            text = TranslateToText(toThousand, (uint)Math.Pow(ten, power)) + text;
        }
        return text;
    }
    private string TranslateToText(uint number, uint scale)
    {
        string text = string.Empty;
        //if number contains hundred
        if(number >= hundred)
        {
            text += Texts[number / hundred] + " " + Texts[hundred] + " and ";
            //delete hundred from number
            number %= hundred;
        }
        //complex number that greater or equal than 20
        if(number >= twenty)
        {
            text += Texts[number / ten * ten] + " ";
            //delete decimal digit from number
            number %= ten;
        }
        //simple number that lower than 20
        text += Texts[number];
        //if number has thousand, million or billion scale
        if(scale > 1)
            text += " " + Texts[scale] + ", ";
        return text;
    }
}

//if i have more time, i will implement recursive function,
//that convert every 3 digits