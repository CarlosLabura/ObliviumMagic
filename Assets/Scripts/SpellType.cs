using UnityEngine;
public enum SpellType {Fist, Rock, Fire, Ice, Electricty, None}
public class SpellTypeScript : MonoBehaviour { public SpellType Type; }

/*
    COMO AÑADIR NUEVOS HECHIZOS: (en caso de ser necesario)
    1. Añadir a SpellType su nombre clave 
        ("None" tiene que ser siempre el ultimo elemento, ya que en la seleccion manual se usa como ultimo elemento para volver al primero)
        (si no es el ultimo, tu hechizo nunca se va a poder seleccionar en la rueda de hechizos)
    2. En PlayerSpell.cs dentro de Update() esta la logica para cuando se castea el hechizo al apretar click
        - Puede ser una hitbox (como fist, una hitbox que spawnea y despawnea tras del cooldown)
        - un projectil de hechizo hecho con Shoot() (no es mas que una colision como el fist pero disparada con AddForce())
        - Logica propia si es necesario
        (es importante añadir cooldown tras hacerlo)
    3. añadir una tecla bindeable con SpellChange()
    4. En HealthSystem.cs se encuentra la logica al entrar en contacto con la colision del hechizo
        - esa logica solo se va a aplicar si el enemigo es debil a el hechizo (ser debil a ninguno (None) significa ser debil a TODOS)
        - no hace falta destruir el hechizo al impactar, ya que su logica de destruccion esta automaticamente programada en ProjectileCollision.cs

*/