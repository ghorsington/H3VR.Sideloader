using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
///     WAV utility for recording and audio playback functions in Unity.
///     Version: 1.0 alpha 1
///     - Use "ToAudioClip" method for loading wav file / bytes.
///     Loads .wav (PCM uncompressed) files at 8,16,24 and 32 bits and converts data to Unity's AudioClip.
///     - Use "FromAudioClip" method for saving wav file / bytes.
///     Converts an AudioClip's float data into wav byte array at 16 bit.
/// </summary>
/// <remarks>
///     For documentation and usage examples: https://github.com/deadlyfingers/UnityWav
/// </remarks>
public class WavUtility
{
    private static readonly Dictionary<int, BitReader> readers = new Dictionary<int, BitReader>
    {
        [8] = new BitReader
        {
            MaxValue = sbyte.MaxValue,
            SizeOf = sizeof(sbyte),
            Reader = br => (float) br.ReadSByte()
        },
        [16] = new BitReader
        {
            MaxValue = short.MaxValue,
            SizeOf = sizeof(short),
            Reader = br => (float) br.ReadInt16()
        },
        [24] = new BitReader
        {
            MaxValue = int.MaxValue,
            SizeOf = 3,
            Reader = br =>
            {
                var buf = new byte[4];
                br.Read(buf, 1, 3);
                return (float) BitConverter.ToInt32(buf, 0);
            }
        },
        [32] = new BitReader
        {
            MaxValue = int.MaxValue,
            SizeOf = sizeof(int),
            Reader = br => (float) br.ReadInt32()
        }
    };

    public static AudioClip ToAudioClip(BinaryReader br, string name = "wav")
    {
        // Skip headers
        br.BaseStream.Position = 16;
        var subChunk1Size = br.ReadInt32();
        var audioFormat = br.ReadUInt16();

        // NB: Only uncompressed PCM wav files are supported.
        var formatCode = FormatCode(audioFormat);
        if (audioFormat != 1 && audioFormat != 65534)
            throw new Exception(
                $"Detected format code '{formatCode}' ({audioFormat}), but only PCM and WaveFormatExtensable uncompressed formats are currently supported.");

        var channels = br.ReadUInt16();
        var sampleRate = br.ReadInt32();
        _ = br.ReadInt32(); // byteRate
        _ = br.ReadUInt16(); // blockAlign
        var bitDepth = br.ReadUInt16();

        br.BaseStream.Position = 16 + 4 + subChunk1Size + 4;
        var dataSize = br.ReadInt32();

        float[] data;
        if (readers.TryGetValue(bitDepth, out var reader))
            data = ReadAudioClipSamples(br, reader, dataSize);
        else
            throw new Exception(bitDepth + " bit depth is not supported.");

        var audioClip = AudioClip.Create(name, data.Length, channels, sampleRate, false);
        audioClip.SetData(data, 0);
        return audioClip;
    }

    private static float[] ReadAudioClipSamples(BinaryReader br, BitReader reader, int dataSize)
    {
        var result = new float[dataSize / reader.SizeOf];
        for (var i = 0; i < result.Length; i++)
            result[i] = reader.Reader(br) / reader.MaxValue;
        return result;
    }

    private static string FormatCode(ushort code)
    {
        switch (code)
        {
            case 1:
                return "PCM";
            case 2:
                return "ADPCM";
            case 3:
                return "IEEE";
            case 7:
                return "μ-law";
            case 65534:
                return "WaveFormatExtensable";
            default:
                Debug.LogWarning("Unknown wav code format:" + code);
                return "";
        }
    }

    private class BitReader
    {
        public int SizeOf { get; set; }
        public Func<BinaryReader, float> Reader { get; set; }
        public int MaxValue { get; set; }
    }
}