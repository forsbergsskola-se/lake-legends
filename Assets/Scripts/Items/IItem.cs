using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public interface IItem
    {
        string ID { get; }
        
        string Name { get; }
        
        int Rarity { get; }
    }
}