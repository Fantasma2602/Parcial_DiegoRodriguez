using UnityEngine;

using UnityEngine.SceneManagement;


public class Escenas : MonoBehaviour
{
    public void Escena1()
    {
        SceneManager.LoadScene("ExamenProgra");
    }
    public void Escena2()
    {
        SceneManager.LoadScene("NuevoProf");
    }
    public void Escena3()
    {
        SceneManager.LoadScene("Sola");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
