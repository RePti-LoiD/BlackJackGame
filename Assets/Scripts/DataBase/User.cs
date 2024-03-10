using System;

public class User
{
    private static User userData;
    public static User UserData => userData;

    private string firstName;
    private string nickName;
    private string linkToAvatar;
    private long creationDate;
    private Guid id;
    private Wallet userWallet;

    public string FirstName { get => firstName; set => firstName = value; }
    public string NickName { get => nickName; set => nickName = value; }
    public string LinkToAvatar { get => linkToAvatar; set => linkToAvatar = value; }
    public long CreationDate { get => creationDate; private set => creationDate = value; }
    public Guid Id { get => id; set => id = value; }
    public Wallet UserWallet { get => userWallet; private set => userWallet = value; }

    //public User() : this("Bot", "Bot player", string.Empty, DateTime.Now.Ticks, null) { }

    public User(string firstName, string nickName, string linkToAvatar, long creationDate, Wallet userWallet)
    {
        this.firstName = firstName;
        this.nickName = nickName;
        this.linkToAvatar = linkToAvatar;
        this.creationDate = creationDate;
        this.userWallet = userWallet;
        id = Guid.NewGuid();
    }

    public override string ToString()
    {
        return $"First name {firstName}, nick {nickName}";
    }
}