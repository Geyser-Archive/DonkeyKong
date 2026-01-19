using UnityEngine;

[CreateAssetMenu(fileName = "CustomFont", menuName = "CustomFont")]
public class CustomFont : ScriptableObject
{
    public Sprite[] Letters => letters;

    public int Upper => upper;
    public int Lower => lower;
    public int Number => number;

    [SerializeField] private Sprite[] letters;

    [SerializeField] private int upper, lower, number;
}