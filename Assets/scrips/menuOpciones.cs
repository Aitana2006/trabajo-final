using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class menuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void PantallaCompleta(bool pantalllaCompleta) 
    {
        Screen.fullScreen = pantalllaCompleta;
    }

    public void CambiarVolumen(float volumen) 
    {
        audioMixer.SetFloat("volumen", volumen);
    }

}
