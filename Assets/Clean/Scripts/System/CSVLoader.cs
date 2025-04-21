using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVLoader
{
    public static List<WaveData> LoadWaveData(string fileName)
    {
        Dictionary<float, WaveData> waveMap = new Dictionary<float, WaveData>();

        TextAsset csvFile = Resources.Load<TextAsset>(fileName); // Resources ���� �ȿ� �ִ� CSV
        StringReader reader = new StringReader(csvFile.text);

        bool isFirstLine = true;
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            if (isFirstLine) { isFirstLine = false; continue; }

            string[] values = line.Split(',');

            float startTime = float.Parse(values[0]);
            string enemyType = values[1];
            int spawnCount = int.Parse(values[2]);
            float spawnInterval = float.Parse(values[3]);

            SubWaveData subWave = new SubWaveData
            {
                enemyType = enemyType,
                spawnCount = spawnCount,
                spawnInterval = spawnInterval
            };

            // �ش� startTime�� ���� ���̺갡 �̹� �ִٸ� �߰�, ������ ���� ����
            if (!waveMap.ContainsKey(startTime))
            {
                waveMap[startTime] = new WaveData
                {
                    startTime = startTime,
                    subWaves = new List<SubWaveData>()
                };
            }

            waveMap[startTime].subWaves.Add(subWave);
        }

        // Dictionary �� ����Ʈ�� ��ȯ + �ð� �� ����
        List<WaveData> waveList = new List<WaveData>(waveMap.Values);
        waveList.Sort((a, b) => a.startTime.CompareTo(b.startTime));

        return waveList;
    }
}