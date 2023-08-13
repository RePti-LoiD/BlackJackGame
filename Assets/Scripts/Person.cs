using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackJackObj", menuName = "Person")]
public class Person : ScriptableObject
{
    [SerializeField] private Guid id;
    [SerializeField] private new string name;
    [SerializeField] private string nickName;

    public Guid ID { get; } = Guid.NewGuid();
    public string Name { get => name; set => name = value; }
    public string NickName { get => nickName; set => nickName = value; }
}