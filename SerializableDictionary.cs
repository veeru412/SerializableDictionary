using System;
using System.Collections.Generic;
using UnityEngine;

namespace VB.SerializeDictionary
{
  [Serializable]
  public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
  {
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key]
    {
      get => dictionary[key];
      set => dictionary[key] = value;
    }

    public Dictionary<TKey, TValue> ToDictionary() => dictionary;

    public void OnBeforeSerialize()
    {
      keys.Clear();
      values.Clear();
      foreach (var kvp in dictionary)
      {
        keys.Add(kvp.Key);
        values.Add(kvp.Value);
      }
    }

    public void OnAfterDeserialize()
    {
      dictionary.Clear();
      for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
      {
        dictionary[keys[i]] = values[i];
      }
    }

    public void Add(TKey key, TValue value)
    {
      dictionary[key] = value;
    }

    public bool Remove(TKey key)
    {
      if (!TryGetValue(key,out var x))
        return false;

      dictionary.Remove(key);

      int index = keys.IndexOf(key);
      if (index >= 0)
      {
        keys.RemoveAt(index);
        values.RemoveAt(index);
      }

      return true;
    }

    public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
  }
}
