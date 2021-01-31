using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Int Range", menuName = "7A Utils/Balance/Ranges/Integer"), Serializable]
public class IntRangeCollection : RangeCollection<int, IntRangeNum>
{
}

[Serializable]
public class IntRangeNum : RangeNum<int>
{
}