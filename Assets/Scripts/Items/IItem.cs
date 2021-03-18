using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public interface IItem
    {
        string ID { get; }

        void GenerateNewGuid();
        
        string Name { get; }
        
        int Rarity { get; }

        void Use();
    }
}