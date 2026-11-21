Dream Cat
Un juego de plataformas en 2D desarrollado en Unity como Trabajo Final. El jugador controla a Carbón, un ágil personaje que debe superar desafíos de plataformas, recolectar estrellas celestiales,
evitar peligros y alcanzar la meta a través de diferentes niveles.

Controles del Juego
Acción Teclas
Moverse a la Izquierda A o Flecha Izquierda
Moverse a la Derecha D o Flecha Derecha
Saltar Barra Espaciadora
W / Flecha Arriba
Menú de Pausa
Tecla P o Escape 

Estructura y Flujo de Escenas
El juego cuenta con un flujo continuo conectado mediante transiciones visuales animadas con LeanTween:
Menú Principal => Jugar => Nivel 1 => recoger todas las estrellas => Nivel 2  => Meta => Pantalla de Victoria 
(se puede salir entre el nivel 1 y el 2).

1. Menu Principal:
* Pantalla de inicio con botones Jugar, Opciones y Salir.
* Menú de opciones interactivo: control de volumen general con AudioMixer y selector de Pantalla Completa.
* Transición animada de entrada y salida con bloques móviles y disolución de pantalla.
  
2. Nivel 1 - El Despertar de Carbón
* Nivel nocturno con plataformas y efecto Parallax multicapa (cielo estrellado y nubes con movimiento relativo).
* 10 estrellas doradas distribuidas por el mapa (cada una otorga 10 puntos).
* Cámara dinámica inteligente con Cinemachine que sigue con suavidad al jugador dentro de los límites del nivel.
* Al recolectar todas las estrellas, se activa la animación de transición hacia el Nivel 2.

Cómo ejecutar el proyecto en Unity
1. Clona o abre la carpeta del proyecto en Unity Editor (versión compatible: Unity 2022 LTS / Unity 6).
2. En la ventana Project, ve a Assets/Scenes/.
3. Haz doble clic en Menu.unity para abrir la escena inicial.
4. Presiona el botón Play (▶) en la barra superior de Unity.
5. Haz clic en Jugar y ¡disfruta de la aventura!
