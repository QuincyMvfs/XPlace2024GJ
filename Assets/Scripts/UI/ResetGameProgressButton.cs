using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetGameProgressButton : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ResetGameProgress);
    }

    private void ResetGameProgress()
    {
        GameManager.Instance.ResetProfile();
    }
}
