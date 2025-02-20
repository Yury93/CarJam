using _Project.Scripts.StaticData;
using System;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services.PersonPool
{
    [Serializable]
    public struct PersonEntity
    {
        public Color Color;
        public ColorTag ColorTag;
        public PersonEntity(Color color, ColorTag colorTag)
        {
            this.Color = color;
            this.ColorTag = colorTag;
        }
    }
}