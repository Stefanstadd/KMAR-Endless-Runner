using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionsMenuCloser : MonoBehaviour
{
    public GameObject confirmMenu;

    public UnityEvent closeApply,closeCancel,cancel;

    public OptionsManager manager;

    public void CloseApply ( ) => closeApply.Invoke ( );

    public void CloseCancel ( ) => closeCancel.Invoke ( );

    public void Cancel ( ) => cancel.Invoke ( );
    public void TryClose ( )
    {
        if ( manager.isDirty )
        {
            confirmMenu.SetActive (true);
        }
        else
        {
            closeApply.Invoke ( );
        }
    }
}
