using UnityEngine;

public class controladorPuntos : MonoBehaviour
{
    public static controladorPuntos Instance;
    [SerializeField] private int puntajeActual;
    [SerializeField] private int puntajeMaximo;

    public void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SumarPuntos(int puntos) 
    {
        puntajeActual += puntos;
        if (puntajeActual > puntajeMaximo) 
        {
            puntajeMaximo = puntajeActual;
        }

        if (puntaje.Instance != null)
        {
            puntaje.Instance.SumarPuntos(puntos);
        }
    }
}
