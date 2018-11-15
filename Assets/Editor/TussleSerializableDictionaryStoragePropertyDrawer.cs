using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SubactionDataListStorage))]
[CustomPropertyDrawer(typeof(SubactionListStorage))]
public class TussleSerializableDictionaryStoragePropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }