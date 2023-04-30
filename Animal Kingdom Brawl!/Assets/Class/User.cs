
using System;

public class User
{
    public string userId { get; set; }
    public string username;
    public string age;
    public string gender;
    public User(string username, string age, string gender)
    {
        this.username = username;
        this.age = age;
        this.gender = gender;
    }
}
