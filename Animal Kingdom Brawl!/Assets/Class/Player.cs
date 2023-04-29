
public class Player
{
    public string id { get; set; }
    public string username { get; set; }    
    //public int age { get; set; }
    //public string gender { get; set; }
    //public string email { get; set; }
    public string[] unlockedAnimalHeroes;

    public Player(string id, string username)
    {
        this.id = id;
        this.username = username;
    }

}
