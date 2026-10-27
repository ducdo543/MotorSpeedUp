using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRaceManager : MonoBehaviour
{
    [SerializeField] private WinUIController winUIController;
    
    public void SetIsWin(bool isWin)
    {
        winUIController.SetIsWin(isWin);
    }    


}
