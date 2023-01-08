using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{
    public List<Soil> Soils;
    public List<HarvestableCrop> HarvestedCrops;
    public override void Awake()
    {
        base.Awake();
        Soils = new List<Soil>();
        HarvestedCrops = new List<HarvestableCrop>();
    }

    public void AddSoil(Soil soil)
    {
        Soils.Add(soil);
    }

    public void EndDay()
    {
        foreach (Soil soil in Soils)
        {
            if (soil.crop != null)
            {
                soil.UpdateDay();
            }
        }
    }
}
