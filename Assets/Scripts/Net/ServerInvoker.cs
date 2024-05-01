using System.Collections.Generic;
using UnityEngine;

public class ServerInvoker : MonoBehaviour
{
    [SerializeField] private ChatMessageHandler messageHandler;

    [SerializeField] private GameObject userProfilePrefab;
    [SerializeField] private GameObject listView;

    private List<GameObject> userProfiles = new List<GameObject>();

    private void Start()
    {
        NetServer server = new NetServer();
        server.OnServerStart += (ep) => Debug.Log($"Server start at {ep}");

        NetworkMessageHandler networkMessageHandler = new(server);
        networkMessageHandler.OnMessageReceive += messageHandler.HandleMessage;

        server.OnClientDisconnect += (client) => 
        {
            foreach (var profile in userProfiles)
            {
                if (profile.GetComponent<UserDataVisualization>().currentUser.Id == client.User.Id)
                {
                    Destroy(profile);
                    userProfiles.Remove(profile);

                    break;
                }
            }
        };

        server.OnClientConnect += (client) => 
        {
            GameObject obj = Instantiate(userProfilePrefab, listView.transform);

            obj.GetComponent<UserDataVisualization>().VisualizeUserData(client.User);

            userProfiles.Add(obj);
        };
    }
}