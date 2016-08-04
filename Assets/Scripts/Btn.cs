using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Btn : MonoBehaviour
{
    public void pressed()
    {
        SceneManager.LoadScene("main");
    }
}
