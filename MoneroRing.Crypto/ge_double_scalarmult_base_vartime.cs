﻿using MoneroSharp.NaCl.Internal.Ed25519Ref10;

namespace MoneroRing.Crypto;

public static partial class RingSig
{
    public static void ge_double_scalarmult_base_vartime(out GroupElementP2 r, byte[] a, ref GroupElementP3 A, byte[] b)
    {

        sbyte[] aslide = new sbyte[256];
        sbyte[] bslide = new sbyte[256];
        GroupElementCached[] Ai = new GroupElementCached[8]; /* A, 3A, 5A, 7A, 9A, 11A, 13A, 15A */ // ge_dsmp type is array of 8 GroupElementCached
        GroupElementP1P1 t;
        GroupElementP3 u;
        int i;

        GroupOperations.slide(aslide, a);
        GroupOperations.slide(bslide, b);
        ge_dsm_precomp(Ai, ref A);

        GroupOperations.ge_p2_0(out r);

        for (i = 255; i >= 0; --i)
        {
            if (aslide[i] != 0 || bslide[i] != 0)
                break;
        }

        for (; i >= 0; --i)
        {
            GroupOperations.ge_p2_dbl(out t, ref r);

            if (aslide[i] > 0)
            {
                GroupOperations.ge_p1p1_to_p3(out u, ref t);
                GroupOperations.ge_add(out t, ref u, ref Ai[aslide[i] / 2]);
            }
            else
            if (aslide[i] < 0)
            {
                GroupOperations.ge_p1p1_to_p3(out u, ref t);
                GroupOperations.ge_sub(out t, ref u, ref Ai[(-aslide[i]) / 2]);
            }

            if (bslide[i] > 0)
            {
                GroupOperations.ge_p1p1_to_p3(out u, ref t);
                GroupOperations.ge_madd(out t, ref u, ref ge_Bi[bslide[i] / 2]);
            }
            else if (bslide[i] < 0)
            {
                GroupOperations.ge_p1p1_to_p3(out u, ref t);
                GroupOperations.ge_msub(out t, ref u, ref ge_Bi[(-bslide[i]) / 2]);
            }

            GroupOperations.ge_p1p1_to_p2(out r, ref t);
        }
    }
}
