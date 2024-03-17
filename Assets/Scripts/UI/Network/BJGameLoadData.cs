using System.Net;
using UnityEngine;

public class BJGameLoadData
{
    public GameObject GameManagerObject;
    public GameObject BJExternalPlayerGameObject;

    public User LocalUser;
    public User ExternalUser;

    public BJPlayer BJLocalUser;
    public CardStackHandler CardHandlerExternalPlayer;

    public BJCardManager BJCardManager;

    public BJGameManagerFactory Factory;
    public IPEndPoint EndPoint;

    public BJGameLoadData(IPEndPoint endPoint, User localUser, User externalUser, BJGameManagerFactory factory)
    {
        EndPoint = endPoint;
        LocalUser = localUser;
        ExternalUser = externalUser;
        Factory = factory;
    }

    public override string ToString()
    {
        return $"GameManagerObject:{GameManagerObject}\n" +
               $"BJExternalPlayerGameObject:{BJExternalPlayerGameObject}\n" +
               $"LocalUser:{LocalUser}\n" +
               $"ExternalUser:{ExternalUser}\n" +
               $"BJLocalUser:{BJLocalUser}\n" +
               $"CardHandlerExternalPlayer:{CardHandlerExternalPlayer}\n" +
               $"BJCardManager:{BJCardManager}\n" +
               $"Factory:{Factory}";
    }
}