using System;

public class User
{
    public string FirstName;
    public string NickName;
    public string Email;
    public int PlayerBalance;
    public string LinkToAvatar;
    public long CreationDate;

    public User() { }

    public User(string firstName, string nickName, string email, int balance, string linkToAvatar, long creationDate)
    {
        this.FirstName = firstName;
        this.NickName = nickName;
        this.Email = email;
        this.PlayerBalance = balance;
        this.LinkToAvatar = linkToAvatar;
        this.CreationDate = creationDate;
    }
}