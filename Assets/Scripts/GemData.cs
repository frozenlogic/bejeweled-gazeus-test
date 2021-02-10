using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "GemData", menuName = "Create New Gem Data")]
public class GemData : ScriptableObject
{
    public Sprite sprite;
    public int ScoreValue;
}