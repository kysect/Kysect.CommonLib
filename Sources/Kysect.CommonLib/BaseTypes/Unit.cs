﻿namespace Kysect.CommonLib.BaseTypes;

public class Unit
{
    public static Unit Instance { get; } = new Unit();

    private Unit()
    {
    }
}