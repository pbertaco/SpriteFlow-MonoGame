namespace Dragon;

public class Names
{
    List<string> listMFirst = new();
    List<string> listFFirst = new();
    List<string> listLast = new();

    static Names _instance;
    public static Names instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Names();
                _instance.load();
            }

            return _instance;
        }
    }

    public string firstName(bool isMale)
    {
        if (isMale)
        {
            return listMFirst.random();
        }
        else
        {
            return listFFirst.random();
        }
    }

    public string lastName()
    {
        return listLast.random();
    }

    public string name(bool isMale, bool isFullName)
    {
        string name = "";

        if (isFullName)
        {
            name += firstName(isMale) + " " + lastName();
        }
        else
        {
            name += firstName(isMale);
        }

        return name;
    }

    void load()
    {
        listMFirst = new List<string>();
        listFFirst = new List<string>();
        listLast = new List<string>();

        listMFirst.Add("Alex");
        listMFirst.Add("Andy");
        listMFirst.Add("Arthur");
        listMFirst.Add("Ben");
        listMFirst.Add("Bob");
        listMFirst.Add("Brad");
        listMFirst.Add("Brian");
        listMFirst.Add("Caleb");
        listMFirst.Add("Calvin");
        listMFirst.Add("Cameron");
        listMFirst.Add("Charlie");
        listMFirst.Add("Chase");
        listMFirst.Add("Chris");
        listMFirst.Add("Christian");
        listMFirst.Add("Cory");
        listMFirst.Add("Daniel");
        listMFirst.Add("David");
        listMFirst.Add("Derek");
        listMFirst.Add("Donny");
        listMFirst.Add("Duane");
        listMFirst.Add("Eric");
        listMFirst.Add("Flynn");
        listMFirst.Add("Gary");
        listMFirst.Add("Geoff");
        listMFirst.Add("Greg");
        listMFirst.Add("Jack");
        listMFirst.Add("Jake");
        listMFirst.Add("James");
        listMFirst.Add("Jeff");
        listMFirst.Add("Jeremy");
        listMFirst.Add("Joe");
        listMFirst.Add("Jordan");
        listMFirst.Add("Landon");
        listMFirst.Add("Matt");
        listMFirst.Add("Mickey");
        listMFirst.Add("Mike");
        listMFirst.Add("Nat");
        listMFirst.Add("Neal");
        listMFirst.Add("Neil");
        listMFirst.Add("Nick");
        listMFirst.Add("Rob");
        listMFirst.Add("Ross");
        listMFirst.Add("Ruben");
        listMFirst.Add("Sam");
        listMFirst.Add("Samuel");
        listMFirst.Add("Shane");
        listMFirst.Add("Shaun");
        listMFirst.Add("Sidney");
        listMFirst.Add("Stephen");
        listMFirst.Add("Tariq");
        listMFirst.Add("Tim");
        listMFirst.Add("Toby");
        listMFirst.Add("Tom");
        listMFirst.Add("William");

        listFFirst.Add("Alice");
        listFFirst.Add("Amanda");
        listFFirst.Add("Amy");
        listFFirst.Add("Andrea");
        listFFirst.Add("Angela");
        listFFirst.Add("Anne");
        listFFirst.Add("April");
        listFFirst.Add("Ashley");
        listFFirst.Add("Bonnie");
        listFFirst.Add("Brittany");
        listFFirst.Add("Carol");
        listFFirst.Add("Carrie");
        listFFirst.Add("Catharine");
        listFFirst.Add("Christine");
        listFFirst.Add("Cynthia");
        listFFirst.Add("Dawn");
        listFFirst.Add("Denise");
        listFFirst.Add("Diane");
        listFFirst.Add("Donna");
        listFFirst.Add("Elizabeth");
        listFFirst.Add("Emily");
        listFFirst.Add("Erica");
        listFFirst.Add("Erin");
        listFFirst.Add("Heather");
        listFFirst.Add("Helen");
        listFFirst.Add("Jamie");
        listFFirst.Add("Jane");
        listFFirst.Add("Janet");
        listFFirst.Add("Jennifer");
        listFFirst.Add("Jessica");
        listFFirst.Add("Jill");
        listFFirst.Add("Joan");
        listFFirst.Add("Judy");
        listFFirst.Add("Julie");
        listFFirst.Add("Karen");
        listFFirst.Add("Kate");
        listFFirst.Add("Kelly");
        listFFirst.Add("Kim");
        listFFirst.Add("Laura");
        listFFirst.Add("Laurie");
        listFFirst.Add("Lisa");
        listFFirst.Add("Marilyn");
        listFFirst.Add("Mary");
        listFFirst.Add("Megan");
        listFFirst.Add("Melissa");
        listFFirst.Add("Michelle");
        listFFirst.Add("Nicole");
        listFFirst.Add("Rachel");
        listFFirst.Add("Rebecca");
        listFFirst.Add("Sally");
        listFFirst.Add("Sandra");
        listFFirst.Add("Sarah");
        listFFirst.Add("Shannon");
        listFFirst.Add("Sharon");
        listFFirst.Add("Stacy");
        listFFirst.Add("Stephanie");
        listFFirst.Add("Susan");
        listFFirst.Add("Tiffany");
        listFFirst.Add("Tina");
        listFFirst.Add("Tracy");
        listFFirst.Add("Wendy");

        listLast.Add("Adams");
        listLast.Add("Alexander");
        listLast.Add("Allen");
        listLast.Add("Anderson");
        listLast.Add("Andrews");
        listLast.Add("Armstrong");
        listLast.Add("Arnold");
        listLast.Add("Bailey");
        listLast.Add("Baker");
        listLast.Add("Barnes");
        listLast.Add("Bell");
        listLast.Add("Bennett");
        listLast.Add("Berry");
        listLast.Add("Black");
        listLast.Add("Boyd");
        listLast.Add("Bradley");
        listLast.Add("Brooks");
        listLast.Add("Brown");
        listLast.Add("Bryant");
        listLast.Add("Burns");
        listLast.Add("Burton");
        listLast.Add("Butler");
        listLast.Add("Campbell");
        listLast.Add("Carpenter");
        listLast.Add("Carroll");
        listLast.Add("Cartwright");
        listLast.Add("Clark");
        listLast.Add("Cole");
        listLast.Add("Coleman");
        listLast.Add("Collins");
        listLast.Add("Cook");
        listLast.Add("Cook");
        listLast.Add("Cooke");
        listLast.Add("Cooper");
        listLast.Add("Cox");
        listLast.Add("Crawford");
        listLast.Add("Cunningham");
        listLast.Add("Cunningham");
        listLast.Add("Daniels");
        listLast.Add("Davis");
        listLast.Add("Dixon");
        listLast.Add("Duncan");
        listLast.Add("Dunn");
        listLast.Add("Edwards");
        listLast.Add("Elliot");
        listLast.Add("Ellis");
        listLast.Add("Evans");
        listLast.Add("Ferguson");
        listLast.Add("Fisher");
        listLast.Add("Flores");
        listLast.Add("Fox");
        listLast.Add("Freeman");
        listLast.Add("Garcia");
        listLast.Add("Gardner");
        listLast.Add("Gibson");
        listLast.Add("Gordon");
        listLast.Add("Gordon");
        listLast.Add("Graham");
        listLast.Add("Grant");
        listLast.Add("Gray");
        listLast.Add("Green");
        listLast.Add("Greene");
        listLast.Add("Griffin");
        listLast.Add("Hall");
        listLast.Add("Hamilton");
        listLast.Add("Harper");
        listLast.Add("Harris");
        listLast.Add("Harrison");
        listLast.Add("Hart");
        listLast.Add("Hawkins");
        listLast.Add("Hayes");
        listLast.Add("Henderson");
        listLast.Add("Henry");
        listLast.Add("Hernandez");
        listLast.Add("Hicks");
        listLast.Add("Hill");
        listLast.Add("Holmes");
        listLast.Add("Howard");
        listLast.Add("Hudson");
        listLast.Add("Hudson");
        listLast.Add("Hughes");
        listLast.Add("Hunt");
        listLast.Add("Hunter");
        listLast.Add("Jackson");
        listLast.Add("James");
        listLast.Add("Jefferson");
        listLast.Add("Jenkins");
        listLast.Add("Johnson");
        listLast.Add("Jones");
        listLast.Add("Jordan");
        listLast.Add("Kelly");
        listLast.Add("Kennedy");
        listLast.Add("King");
        listLast.Add("Knight");
        listLast.Add("Lane");
        listLast.Add("Lawrence");
        listLast.Add("Lee");
        listLast.Add("Lewis");
        listLast.Add("Long");
        listLast.Add("Marshall");
        listLast.Add("Martin");
        listLast.Add("Martz");
        listLast.Add("Mason");
        listLast.Add("Matthews");
        listLast.Add("McDonald");
        listLast.Add("Meyers");
        listLast.Add("Mills");
        listLast.Add("Mitchell");
        listLast.Add("Moore");
        listLast.Add("Morgan");
        listLast.Add("Morris");
        listLast.Add("Murphy");
        listLast.Add("Murray");
        listLast.Add("Nelson");
        listLast.Add("Nichols");
        listLast.Add("Olson");
        listLast.Add("Owens");
        listLast.Add("Palmer");
        listLast.Add("Parker");
        listLast.Add("Patterson");
        listLast.Add("Payne");
        listLast.Add("Perkins");
        listLast.Add("Perry");
        listLast.Add("Peterson");
        listLast.Add("Phillips");
        listLast.Add("Pierce");
        listLast.Add("Porter");
        listLast.Add("Powell");
        listLast.Add("Price");
        listLast.Add("Ramirez");
        listLast.Add("Ray");
        listLast.Add("Reed");
        listLast.Add("Reynolds");
        listLast.Add("Reynolds");
        listLast.Add("Rice");
        listLast.Add("Richardson");
        listLast.Add("Riley");
        listLast.Add("Roberts");
        listLast.Add("Robertson");
        listLast.Add("Robinson");
        listLast.Add("Rodgers");
        listLast.Add("Rodriguez");
        listLast.Add("Rose");
        listLast.Add("Ross");
        listLast.Add("Russell");
        listLast.Add("Sanders");
        listLast.Add("Scott");
        listLast.Add("Shaw");
        listLast.Add("Simpson");
        listLast.Add("Smith");
        listLast.Add("Snyder");
        listLast.Add("Spencer");
        listLast.Add("Stevens");
        listLast.Add("Stewart");
        listLast.Add("Stone");
        listLast.Add("Sullivan");
        listLast.Add("Taylor");
        listLast.Add("Thomas");
        listLast.Add("Thompson");
        listLast.Add("Tucker");
        listLast.Add("Turner");
        listLast.Add("Wagner");
        listLast.Add("Walker");
        listLast.Add("Wallace");
        listLast.Add("Ward");
        listLast.Add("Warren");
        listLast.Add("Washington");
        listLast.Add("Watkins");
        listLast.Add("Watson");
        listLast.Add("Weaver");
        listLast.Add("Webb");
        listLast.Add("Wells");
        listLast.Add("Welsh");
        listLast.Add("West");
        listLast.Add("Wheeler");
        listLast.Add("White");
        listLast.Add("Williams");
        listLast.Add("Willis");
        listLast.Add("Wilson");
        listLast.Add("Wood");
        listLast.Add("Woods");
        listLast.Add("Wright");
        listLast.Add("Young");
    }
}
