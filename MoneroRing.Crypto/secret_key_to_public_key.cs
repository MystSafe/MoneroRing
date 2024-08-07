﻿/*
 * MoneroRing, C# .NET implementation of Monero keys, signatures, ring signatures, and key images
 * Github: https://github.com/MystSafe/MoneroRing
 * 
 * Copyright (C) 2024, MystSafe (https://mystsafe.com)
 * Copyright (C) 2024, Author: crypticana <crypticana@proton.me>
 * MystSafe is the only privacy preserving password manager
 *
 * Licensed under MIT (See LICENSE file)
 */

using MoneroSharp.NaCl.Internal.Ed25519Ref10;

namespace MoneroRing.Crypto;

public static partial class RingSig
{
    // initialie sec buffer with 32 bytes before calling this method: public_key = new byte[32]; 
    public static bool secret_key_to_public_key(byte[] sec, byte[] pub)
    {
        if (sc_check(sec) != 0)
            return false;
        GroupElementP3 pv_ge_p3;
        GroupOperations.ge_scalarmult_base(out pv_ge_p3, sec, 0);
        GroupOperations.ge_p3_tobytes(pub, 0, ref pv_ge_p3);
        return true;
    }

}

