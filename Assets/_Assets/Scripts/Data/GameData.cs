using System.Collections;
using UnityEngine;


public class GameData
{
    public static StaticGameData staticData = new StaticGameData();
    public static UserData userData = new UserData();

    public static void Reset()
    {
        staticData = new StaticGameData();
        userData = new UserData();
    }
}