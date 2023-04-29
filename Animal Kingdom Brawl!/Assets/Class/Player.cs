
public class Player
{
    public string username { get; set; }
    public string age { get; set; }
    public string gender { get; set; }
    //public string[] unlockedAnimalHeroes = { };

    public Player(string username, string age, string gender)
    {
        this.username = username;
        this.age = age;
        this.gender = gender;
    }

}
