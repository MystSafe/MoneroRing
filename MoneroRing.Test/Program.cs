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

using MoneroSharp.Utils;
using MoneroRing.Crypto;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

string data = "MoneroRing library simple test";
Console.WriteLine(data);

byte[] data_bytes = Encoding.UTF8.GetBytes(data);
var keccak256 = new Nethereum.Util.Sha3Keccack();
byte[] hash = keccak256.CalculateHash(data_bytes);

byte[] sec1 = new byte[32];
byte[] pub1 = new byte[32];
byte[] sec2 = new byte[32];
byte[] pub2 = new byte[32];
byte[] sec3 = new byte[32];
byte[] pub3 = new byte[32];

RingSig.generate_keys(pub1, sec1);
RingSig.generate_keys(pub2, sec2);
RingSig.generate_keys(pub3, sec3);

byte[] image = new byte[32];
RingSig.generate_key_image(pub2, sec2, image);
Console.WriteLine("Key image: " + MoneroUtils.BytesToHex(image));

var pubs = new byte[3][];
pubs[0] = pub1;
pubs[1] = pub2;
pubs[2] = pub3;

Console.WriteLine("Generating Monero ring signature");
byte[] ring_sig = RingSig.generate_ring_signature(hash, image, pubs, 3, sec2, 1);

Console.WriteLine("ring signature length: " + ring_sig.Length.ToString());
Console.WriteLine("ring signature: " + MoneroUtils.BytesToHex(ring_sig));

var ring_is_valid = RingSig.check_ring_signature(hash, image, pubs, 3, ring_sig);
Console.WriteLine("ring signature is valid: " + ring_is_valid.ToString());

Console.WriteLine("Generating Monero signature");
var sig = RingSig.generate_signature(hash, pub1, sec1);
Console.WriteLine("sig length: " + sig.Length.ToString());
Console.WriteLine("sig: " + MoneroUtils.BytesToHex(sig));
var sig_is_valid = RingSig.check_signature(hash, pub1, sig);
Console.WriteLine("sig is valid: " + sig_is_valid.ToString());

