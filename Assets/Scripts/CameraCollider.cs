using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        WaveEmitter wave = collider.GetComponent<WaveEmitter>();
        if (wave != null)
        {
            wave.emitting = true;
        }

        WaterCrystal crystal = collider.GetComponent<WaterCrystal>();
        if (crystal != null)
        {
            crystal.emitting = true;
        }

        MinerController miner = collider.GetComponent<MinerController>();
        if (miner != null)
        {
            miner.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        WaveEmitter wave = collider.GetComponent<WaveEmitter>();
        if (wave != null)
        {
            wave.emitting = false;
        }

        WaterCrystal crystal = collider.GetComponent<WaterCrystal>();
        if (crystal != null)
        {
            crystal.emitting = false;
        }

        MinerController miner = collider.GetComponent<MinerController>();
        if (miner != null)
        {
            miner.GetComponent<MinerController>().enabled = false;
        }
    }
}
