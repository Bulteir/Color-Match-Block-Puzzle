using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InternetAvailabilityController : MonoBehaviour
{

    void Start()
    {
        InvokeRepeating(nameof(CheckNetwork), 5f, 5.0f);
    }

    public void CheckNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {

            GlobalVariables.internetAvaible = false;
        }
        else
        {
            GlobalVariables.internetAvaible = true;
        }
    }
}
