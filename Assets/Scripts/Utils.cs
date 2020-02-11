using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Atribute
{
    public int factor;
    public int baseValue;
    public int level;

    public int Value
    {
        get
        {
            return level * factor + baseValue;
        }
    }

    public float ValueFloat
    {
        get
        {
            return (((float)level * (float)factor + (float)baseValue)/1000.0f);
        }
    }

    public static Atribute operator +(Atribute a, Atribute b)
    {
        Atribute c = a;
        c.level += b.level;
        return c;
    }
}

[System.Serializable]
public struct EntityStats
{
    public EntityStats(int maxLifesc, int currentLifesc, Atribute fireRatec, Atribute speedc, Atribute rangec,Atribute stren)
    {
        maxLifes = maxLifesc;
        currentLifes = currentLifesc;
        fireRate = fireRatec;
        speed = speedc;
        range = rangec;
        strengh = stren;

    }

    public int maxLifes;
    public int currentLifes;

    public Atribute fireRate;

    public Atribute strengh;

    public Atribute speed;

    public Atribute range;

    public static EntityStats operator +(EntityStats a, EntityStats b)
    {
        EntityStats c = a;
        c.maxLifes += b.maxLifes;
        c.currentLifes += b.currentLifes;
        c.fireRate += b.fireRate;
        c.speed += b.speed;
        c.range += b.range;
        return c;
    }
}
static public class Utils
{
    public enum DirectionEnumerator : byte
    {
        NONE = 0x0, NORTH = 0x1, EAST = 0x2, SOUTH = 0x4, WEST = 0x8
    }


    public static Vector3 DirectionToVector(DirectionEnumerator dir)
    {
        Vector3 direction = new Vector3(0, 0);
        if (dir == DirectionEnumerator.NONE)
            return direction;
        if (CheckByte((byte)dir, 0))
        {
            direction.y += 1;
        }

        if (CheckByte((byte)dir, 2))
        {
            direction.y -= 1;
        }

        if (CheckByte((byte)dir, 1))
        {
            direction.x += 1;
        }

        if (CheckByte((byte)dir, 3))
        {
            direction.x -= 1;
        }
        direction = direction.normalized;
        return direction;
    }
    private static bool CheckByte(byte b, int pos)
    {
        return (b & (1 << pos)) != 0;
    }


}
