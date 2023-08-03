using System.Collections.Generic;

public class User
{
    public string username;
    public string age;
    public string gender;
    public List<string> matchHistory = new List<string>();
    
    // This class is used for Firebase Data Tree's nodes
    public User(string username, string age, string gender)
    {
        this.username = username;
        this.age = age;
        this.gender = gender;
    }
}
