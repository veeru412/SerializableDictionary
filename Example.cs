using System.Collections;
using UnityEngine;

namespace VB.SerializeDictionary.Assets.SerializeDictionary
{
  public class Example : MonoBehaviour
  {
    [SerializeField] private SerializableDictionary<string, int> SerializableDictionary = new SerializableDictionary<string, int>();

    private void OnValidate()
    {
      SerializableDictionary.Add("abc", 1);
      SerializableDictionary.Add("tag", 2);
    }
  }
}