using UnityEngine;

public static class InputRouter
{
    public static IGameplayInputReceiver Receiver { get; set; }

    public static void RouteKey(string key)
    {
        if (string.IsNullOrEmpty(key)) return;

        if (int.TryParse(key, out int number))
        {
            Receiver?.TypeNumber(number);
        }
        else
        {
            Receiver?.TypeLetter(key[0]);
        }
    }
}
