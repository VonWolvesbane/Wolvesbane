Wolvesbane Housing Reclamation - Phase 2A House Sign Fix

Your ServUO HouseSign class exposes its house through:

    public BaseHouse Owner

It does not contain a Structure property.

This revision changes the target resolver from:

    sign.Structure as BaseHouse

to:

    sign.Owner

No reclamation/destruction logic changed.
